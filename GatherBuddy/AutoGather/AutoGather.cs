using ECommons.Automation.LegacyTaskManager;
using GatherBuddy.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Game.ClientState.Conditions;
using ECommons;
using ECommons.Automation;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.UI;
using GatherBuddy.AutoGather.Movement;
using GatherBuddy.Classes;
using GatherBuddy.CustomInfo;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using Lumina.Excel.GeneratedSheets;
using HousingManager = GatherBuddy.SeFunctions.HousingManager;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather : IDisposable
    {
        public AutoGather(GatherBuddy plugin)
        {
            // Initialize the task manager
            TaskManager                            =  new();
            TaskManager.ShowDebug                  =  false;
            _plugin                                =  plugin;
            _movementController                    =  new OverrideMovement();
            _soundHelper                           =  new SoundHelper();
            GatherBuddy.UptimeManager.UptimeChange += UptimeChange;
        }

        private void UptimeChange(IGatherable obj)
        {
            GatherBuddy.Log.Verbose($"Timer for {obj.Name[GatherBuddy.Language]} has expired and the item has been removed from memory.");
            TimedNodesGatheredThisTrip.Remove(obj.ItemId);
        }

        private readonly OverrideMovement _movementController;

        private readonly GatherBuddy _plugin;
        private readonly SoundHelper _soundHelper;
        
        public           TaskManager TaskManager { get; }

        private bool _enabled { get; set; } = false;

        public unsafe bool Enabled
        {
            get => _enabled;
            set
            {
                if (!value)
                {
                    //Do Reset Tasks
                    var gatheringMasterpiece = (AddonGatheringMasterpiece*)Dalamud.GameGui.GetAddonByName("GatheringMasterpiece", 1);
                    if (gatheringMasterpiece != null && !gatheringMasterpiece->AtkUnitBase.IsVisible)
                    {
                        gatheringMasterpiece->AtkUnitBase.IsVisible = true;
                    }

                    if (IsPathing || IsPathGenerating)
                    {
                        VNavmesh_IPCSubscriber.Path_Stop();
                    }

                    TaskManager.Abort();
                    HasSeenFlag                         = false;
                    HiddenRevealed                      = false;
                    _movementController.Enabled         = false;
                    _movementController.DesiredPosition = Vector3.Zero;
                    ResetNavigation();
                    AutoStatus = "Idle...";

                    currentCluster = 0;
                    gatheringsInCluster = 0;
                }

                _enabled = value;
            }
        }

        public void GoHome()
        {
            if (!GatherBuddy.Config.AutoGatherConfig.GoHomeWhenIdle || !CanAct)
                return;

            if (HousingManager.IsInHousing() || Lifestream_IPCSubscriber.IsBusy())
            {
                if (SpiritBondMax > 0 && GatherBuddy.Config.AutoGatherConfig.DoMaterialize)
                {
                    DoMateriaExtraction();
                    return;
                }
                return;
            }

            if (Lifestream_IPCSubscriber.IsEnabled)
            {
                TaskManager.Enqueue(VNavmesh_IPCSubscriber.Path_Stop);
                TaskManager.Enqueue(() => Lifestream_IPCSubscriber.ExecuteCommand("auto"));
                TaskManager.Enqueue(() => Svc.Condition[ConditionFlag.BetweenAreas]);
                TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.BetweenAreas]);
                TaskManager.DelayNext(1000);
            }
            else 
                GatherBuddy.Log.Warning("Lifestream not found or not ready");
        }

        // Cache k-means clustering for the current Gatherables
        private Dictionary<uint, List<Vector3>> kMeansClusters = new(); // Dictionary of itemId and their k-means cluster centers\
        private readonly int kMeansClusterCount = 3; // Number of clusters
        private readonly uint clusteringMaxIterations = 100; // Maximum number of iterations for k-means

        private void PrecalculateKMeans(Gatherable gatherable)
        {
            // Check if we already have the k-means for this gatherable
            if (kMeansClusters.ContainsKey(gatherable.ItemId))
                return;

            // Get the location of this gatherable
            var location = GatherBuddy.UptimeManager.BestLocation(gatherable);

            // If we're currently not in the same territory as the gatherable, we can't calculate the k-means
            if (location.Location.Territory.Id != Svc.ClientState.TerritoryType)
                return;

            // Get the nodes for this gatherable
            var validNodesForItem = gatherable.NodeList.SelectMany(n => n.WorldPositions).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var matchingNodesInZone = location.Location.WorldPositions.Where(w => validNodesForItem.ContainsKey(w.Key)).SelectMany(w => w.Value)
                .Where(v => !IsBlacklisted(v))
                .ToList();

            // Debug print all nodes
            GatherBuddy.Log.Verbose($"All nodes for {gatherable.Name[GatherBuddy.Language]}:");
            foreach (var node in matchingNodesInZone)
                GatherBuddy.Log.Verbose(node.ToString());

            // Select k random nodes as the initial cluster centers
            var clusterCenters = new List<Vector3>();
            for (var i = 0; i < kMeansClusterCount; i++)
            {
                // Don't actually randomly initialize them as if any two nodes are the same, the k-means will fail
                // We will use farthest point sampling instead
                if (i == 0)
                {
                    // Select the first node as the first cluster center, since there are no other nodes to compare to
                    clusterCenters.Add(matchingNodesInZone[0]);
                    continue;
                }

                // Otherwise find the node that is the farthest from all other cluster centers
                var node = matchingNodesInZone
                    .OrderBy(n => clusterCenters.Min(center => Vector3.Distance(n, center)))
                    .Last();
                clusterCenters.Add(node);
            }
            // Debug
            GatherBuddy.Log.Verbose($"Initial cluster centers for {gatherable.Name[GatherBuddy.Language]}:");
            for (var i = 0; i < kMeansClusterCount; i++)
                GatherBuddy.Log.Verbose($"Cluster {i}: {clusterCenters[i]}");

            // Run k-means clustering
            for (var iteration = 0; iteration < clusteringMaxIterations; iteration++)
            {
                // Calculate which cluster each node belongs to
                // List of clusters, each cluster is a list of node indices
                var clusters = new List<List<int>>();
                for (var i = 0; i < kMeansClusterCount; i++)
                    clusters.Add(new List<int>());

                for (var i = 0; i < matchingNodesInZone.Count; i++)
                {
                    // For each node...
                    var node = matchingNodesInZone[i];
                    var minDistance = float.MaxValue;
                    var minCluster = 0;

                    // ... find the closest cluster center
                    for (var j = 0; j < kMeansClusterCount; j++)
                    {
                        var distance = Vector3.Distance(node, clusterCenters[j]);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            minCluster = j;
                        }
                    }

                    clusters[minCluster].Add(i);
                }

                // Calculate new cluster centers
                var newClusterCenters = clusters.Select(
                    cluster => cluster.Aggregate(Vector3.Zero, (acc, index) => acc + matchingNodesInZone[index]) / cluster.Count
                ).ToList();

                // Check for early stopping
                const float epsilon = 0.01f; // Stop if the cluster centers don't change by more than epsilon, in total.

                var totalChange = newClusterCenters.Zip(clusterCenters, (a, b) => Vector3.Distance(a, b)).Sum();
                if (totalChange < epsilon)
                {
                    GatherBuddy.Log.Verbose($"Early stopping at iteration {iteration} for {gatherable.Name[GatherBuddy.Language]}");
                    break;
                }

                // Update the cluster centers
                clusterCenters = newClusterCenters;

                //GatherBuddy.Log.Verbose($"Iteration {iteration} for {gatherable.Name[GatherBuddy.Language]}:");
                //for (var i = 0; i < kMeansClusterCount; i++)
                //    GatherBuddy.Log.Verbose($"Cluster {i}: {clusterCenters[i]}");

            }
            // Do a debug print
            GatherBuddy.Log.Verbose($"K-means for {gatherable.Name[GatherBuddy.Language]}:");
            for (var i = 0; i < kMeansClusterCount; i++)
                GatherBuddy.Log.Verbose($"Cluster {i}: {clusterCenters[i]}");

            // Count how many nodes are in each cluster
            for (var c = 0; c < kMeansClusterCount; c++)
            {
                var count = 0;
                foreach (var node in matchingNodesInZone)
                {
                    if (Vector3.Distance(node, clusterCenters[c]) <= clusterCenters.Min(center => Vector3.Distance(node, center)))
                        count++;
                }

                GatherBuddy.Log.Verbose($"Cluster {c} has {count} nodes");
            }

            // Save the cluster centers
            kMeansClusters[gatherable.ItemId] = clusterCenters;
        }

        public bool ShouldKMeansCluster(Gatherable gatherable)
        {
            return GatherBuddy.Config.AutoGatherConfig.UseExperimentalKMeans &&
                gatherable.NodeType == NodeType.Regular &&
                gatherable.Level >= 51;
        }

        // Save which cluster we are currently in
        private int currentCluster = 0;
        private uint lastGatheredItemId = 0;
        private int gatheringsInCluster = 0;

        public void DoAutoGather()
        {
            if (!Enabled)
            {
                return;
            }

            try
            {
                if (!NavReady && Enabled)
                {
                    AutoStatus = "Waiting for Navmesh...";
                    return;
                }
            }
            catch (Exception e)
            {
                //GatherBuddy.Log.Error(e.Message);
                AutoStatus = "vnavmesh communication failed. Do you have it installed??";
                return;
            }

            if (_movementController.Enabled)
            {
                AutoStatus = $"Advanced unstuck in progress!";
                AdvancedUnstuckCheck();
                return;
            }

            DoSafetyChecks();
            if (TaskManager.IsBusy)
            {
                //GatherBuddy.Log.Verbose("TaskManager has tasks, skipping DoAutoGather");
                return;
            }

            if (!CanAct)
            {
                AutoStatus = "Player is busy...";
                return;
            }
            
            UpdateItemsToGather();
            Gatherable? targetItem =
                (TimedItemsToGather.Count > 0 ? TimedItemsToGather.MinBy(GetNodeTypeAsPriority) : ItemsToGather.FirstOrDefault()) as Gatherable;

            if (targetItem == null)
            {
                if (_plugin.GatherWindowManager.ActiveItems.All(i => InventoryCount(i) >= QuantityTotal(i)))
                {
                    AutoStatus         = "No items to gather...";
                    Enabled            = false;
                    CurrentDestination = null;
                    VNavmesh_IPCSubscriber.Path_Stop();
                    if (GatherBuddy.Config.AutoGatherConfig.HonkMode)
                        _soundHelper.PlayHonkSound(3);
                    GoHome();
                    return;
                }

                GoHome();
                //GatherBuddy.Log.Warning("No items to gather");
                AutoStatus = "No available items to gather";
                return;
            }

            if (IsGathering && GatherBuddy.Config.AutoGatherConfig.DoGathering)
            {
                AutoStatus = "Gathering...";
                TaskManager.Enqueue(VNavmesh_IPCSubscriber.Path_Stop);
                DoActionTasks(targetItem);
                return;
            }

            if (IsPathGenerating)
            {
                AutoStatus = "Generating path...";
                AdvancedUnstuckCheck();
                return;
            }

            if (IsPathing)
            {
                StuckCheck();
                AdvancedUnstuckCheck();
            }

            // Generate the kmeans clusters
            if (ShouldKMeansCluster(targetItem))
            {
                PrecalculateKMeans(targetItem);

                // Check if we need to update the cluster
                if (lastGatheredItemId != targetItem.ItemId)
                {
                    lastGatheredItemId = targetItem.ItemId;
                    currentCluster = 0;
                }
            }


            var location = GatherBuddy.UptimeManager.BestLocation(targetItem);
            if (location.Location.Territory.Id != Svc.ClientState.TerritoryType || !GatherableMatchesJob(targetItem))
            {
                HasSeenFlag = false;
                TaskManager.Enqueue(VNavmesh_IPCSubscriber.Path_Stop);
                TaskManager.Enqueue(() => MoveToTerritory(location.Location));
                return;
            }

            DoUseConsumablesWithoutCastTime();
            if (SpiritBondMax > 0 && GatherBuddy.Config.AutoGatherConfig.DoMaterialize)
            {
                DoMateriaExtraction();
                return;
            }


            var validNodesForItem = targetItem.NodeList.SelectMany(n => n.WorldPositions).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var matchingNodesInZone = location.Location.WorldPositions.Where(w => validNodesForItem.ContainsKey(w.Key)).SelectMany(w => w.Value)
                .Where(v => !IsBlacklisted(v))
                //.OrderBy(v => Vector3.Distance(Player.Position, v))
                .OrderBy(v => Vector3.Distance(Player.Position, v))
                .ToList();

            var allNodes = Svc.Objects.Where(o => matchingNodesInZone.Contains(o.Position)).ToList();
            var closeNodes = allNodes.Where(o => o.IsTargetable)
                .OrderBy(o => Vector3.Distance(Player.Position, o.Position));
            if (closeNodes.Any())
            {
                TaskManager.Enqueue(() => MoveToCloseNode(closeNodes.First(n => !IsBlacklisted(n.Position)), targetItem));
                return;
            }

            if (ShouldKMeansCluster(targetItem))
            {
                // If we're doing KMeans clustering, check if we're going clockwise or counter clockwise. 

                // Get the center of all clusters
                var clustersCenter = kMeansClusters[targetItem.ItemId].Aggregate(Vector3.Zero, (acc, cluster) => acc + cluster) / kMeansClusterCount;
                
                // Using the line from the player to the center of the clusters, calculate if we're going to the left or right of it.
                // Left = clockwise, Right = counter clockwise
                var playerToCenter = Vector2.Normalize(new Vector2(clustersCenter.X - Player.Position.X, clustersCenter.Z - Player.Position.Z));

                // Get position of the current cluster we're going to
                var currentClusterPosition = new Vector2(
                    kMeansClusters[targetItem.ItemId][currentCluster].X,
                    kMeansClusters[targetItem.ItemId][currentCluster].Z
                );

                // currentClusterPosition = Player.Position + t1 * playerToCenter + t2 * perpendicular
                // t2 is how far left/right we are from the line

                var b = currentClusterPosition - new Vector2(Player.Position.X, Player.Position.Z);

                //var t1 = ( playerToCenter.X * b.X + playerToCenter.Y * b.Y) / (playerToCenter.X * playerToCenter.X + playerToCenter.Y * playerToCenter.Y);
                var t2 = (-playerToCenter.Y * b.X + playerToCenter.X * b.Y) / (playerToCenter.X * playerToCenter.X + playerToCenter.Y * playerToCenter.Y);

                var sign = Math.Sign(t2);

                // Now, for the current cluster, we want to find the node that is the most clockwise or counter clockwise
                matchingNodesInZone = matchingNodesInZone
                .Where(n => {
                    // Our node is closest to our cluster center
                    // Get all distances
                    var distances = kMeansClusters[targetItem.ItemId]
                        .Select(c => Vector3.DistanceSquared(n, c))
                        .ToList();

                    var minCluster = distances.IndexOf(distances.Min());
                    return minCluster == currentCluster;

                })
                .OrderBy(n => {
                    // Using the line from the player to the center of the current cluster
                    var nodePosition = new Vector2(n.X, n.Z);
                    var b2 = nodePosition - new Vector2(Player.Position.X, Player.Position.Z);

                    var playerToClusterCenter = Vector2.Normalize(currentClusterPosition - new Vector2(Player.Position.X, Player.Position.Z));

                    // Calculate "t2" for the node
                    var t2Node = (-playerToClusterCenter.Y * b2.X + playerToClusterCenter.X * b2.Y) / (playerToClusterCenter.X * playerToClusterCenter.X + playerToClusterCenter.Y * playerToClusterCenter.Y);

                    return -sign * t2Node;
                }).ToList();

                //GatherBuddy.Log.Verbose($"Current cluster: {currentCluster}");
                //GatherBuddy.Log.Verbose($"Current cluster centers: {kMeansClusters[targetItem.ItemId].Select(n => n.ToString()).Join(", ")}");
                //GatherBuddy.Log.Verbose($"matchingNodesInZone:");
                //foreach (var node in matchingNodesInZone)
                //    GatherBuddy.Log.Verbose(node.ToString());
            }

            // Find the first node that is not in the blacklist and not yet in the far node list
            var selectedNode = matchingNodesInZone.FirstOrDefault(n => !FarNodesSeenSoFar.Contains(n));
            if (selectedNode == Vector3.Zero)
            {
                FarNodesSeenSoFar.Clear();
                GatherBuddy.Log.Verbose($"Selected node was null and far node filters have been cleared");
                return;
            }

            // only Legendary and Unspoiled show marker
            if (ShouldUseFlag && targetItem.NodeType is NodeType.Legendary or NodeType.Unspoiled)
            {
                // marker not yet loaded on game
                if (TimedNodePosition == null)
                {
                    AutoStatus = "Waiting on flag show up";
                    return;
                }

                //AutoStatus = "Moving to farming area...";
                selectedNode = matchingNodesInZone
                    .Where(o => Vector2.Distance(TimedNodePosition.Value, new Vector2(o.X, o.Z)) < 10).OrderBy(o
                        => Vector2.Distance(TimedNodePosition.Value, new Vector2(o.X, o.Z))).FirstOrDefault();
            }

            // FIX: Using Epsilon instead of == to avoid floating point errors
            if (Vector3.Distance(selectedNode, Player.Position) < 100)
            {
                GatherBuddy.Log.Verbose($"Selected node is too close to player, checking if it should be skipped...");
                if (allNodes.Any(n => Vector3.Distance(n.Position, selectedNode) < 2.5))
                {
                    GatherBuddy.Log.Verbose($"Node fails test, skipping...");
                    FarNodesSeenSoFar.Add(selectedNode);

                    CurrentDestination = null;
                    VNavmesh_IPCSubscriber.Path_Stop();
                    AutoStatus = "Looking for far away nodes...";
                    return;
                }
            }

            TaskManager.Enqueue(() => MoveToFarNode(selectedNode));
            return;


            AutoStatus = "Nothing to do...";
        }

        private void DoSafetyChecks()
        {
            // if (VNavmesh_IPCSubscriber.Path_GetAlignCamera())
            // {
            //     GatherBuddy.Log.Warning("VNavMesh Align Camera Option turned on! Forcing it off for GBR operation.");
            //     VNavmesh_IPCSubscriber.Path_SetAlignCamera(false);
            // }
        }

        public void Dispose()
        {
            _movementController.Dispose();
        }
    }
}
