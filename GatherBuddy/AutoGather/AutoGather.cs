using ECommons.Automation.LegacyTaskManager;
using GatherBuddy.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.UI;
using GatherBuddy.Classes;
using GatherBuddy.CustomInfo;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        public AutoGather(GatherBuddy plugin)
        {
            // Initialize the task manager
            TaskManager                            =  new();
            _plugin                                =  plugin;
            GatherBuddy.UptimeManager.UptimeChange += UptimeChange;
        }

        private void UptimeChange(IGatherable obj)
        {
            TimedNodesGatheredThisTrip.Remove(obj.ItemId);
        }

        private GatherBuddy _plugin;

        public TaskManager TaskManager { get; }

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
                    HasSeenFlag    = false;
                    HiddenRevealed = false;
                    TimedNodesGatheredThisTrip.Clear();
                    ResetNavigation();
                    AutoStatus         = "Idle...";
                }

                _enabled = value;
            }
        }

        public unsafe void DoAutoGather()
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

            DoSafetyChecks();
            if (TaskManager.NumQueuedTasks > 0)
            {
                //GatherBuddy.Log.Verbose("TaskManager has tasks, skipping DoAutoGather");
                return;
            }

            if (!_plugin.GatherWindowManager.ActiveItems.Any(i => i.InventoryCount < i.Quantity))
            {
                AutoStatus         = "No items to gather...";
                Enabled            = false;
                CurrentDestination = null;
                VNavmesh_IPCSubscriber.Path_Stop();
                return;
            }

            if (!CanAct)
            {
                AutoStatus = "Player is busy...";
                return;
            }

            if (IsGathering)
            {
                AutoStatus = "Gathering...";
                TaskManager.Enqueue(VNavmesh_IPCSubscriber.Path_Stop);
                TaskManager.Enqueue(DoActionTasks);
                return;
            }

            if (IsPathGenerating)
            {
                AutoStatus = "Generating path...";
                return;
            }

            if (IsPathing)
            {
                StuckCheck();
            }
            
            UpdateItemsToGather();


            Gatherable? targetItem = (TimedItemsToGather.Count > 0 ? TimedItemsToGather.FirstOrDefault() : ItemsToGather.FirstOrDefault()) as Gatherable;
            if (targetItem == null)
            {
                //GatherBuddy.Log.Warning("No items to gather");
                AutoStatus = "No available items to gather";
                return;
            }

            var location = GatherBuddy.UptimeManager.BestLocation(targetItem);
            if (location.Location.Territory.Id != Svc.ClientState.TerritoryType || !GatherableMatchesJob(targetItem))
            {
                HasSeenFlag = false;
                TaskManager.Enqueue(VNavmesh_IPCSubscriber.Path_Stop);
                TaskManager.Enqueue(() => MoveToTerritory(location.Location));
                return;
            }

            if (MapFlagPosition != null && MapFlagPosition.Value.DistanceToPlayer() > 150 && ShouldUseFlag)
            {
                HasSeenFlag = true;
                AutoStatus  = "Moving to farming area...";
                TaskManager.Enqueue(MoveToFlag);
                return;
            }

            var validNodesForItem   = targetItem.NodeList.SelectMany(n => n.WorldPositions).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var matchingNodesInZone = location.Location.WorldPositions.Where(w => validNodesForItem.ContainsKey(w.Key)).SelectMany(w => w.Value).ToList();
            var closeNodes          = Svc.Objects.Where(o => matchingNodesInZone.Contains(o.Position) && o.IsTargetable).ToList().OrderBy(o => Vector3.Distance(Player.Position, o.Position));
            if (closeNodes.Any())
            {
                TaskManager.Enqueue(() =>MoveToCloseNode(closeNodes.First(), targetItem));
                return;
            }
            else
            {
                var selectedNode = matchingNodesInZone[CurrentFarNodeIndex];
                if (Vector3.Distance(Player.Object.Position, selectedNode) < GatherBuddy.Config.AutoGatherConfig.FarNodeFilterDistance)
                {
                    CurrentFarNodeIndex++;
                    if (CurrentFarNodeIndex >= matchingNodesInZone.Count)
                    {
                        CurrentFarNodeIndex = 0;
                    }

                    CurrentDestination = null;
                    AutoStatus         = "Looking for far away nodes...";
                    return;
                }
                TaskManager.Enqueue(() => MoveToFarNode(selectedNode));
                return;
            }

            AutoStatus = "Nothing to do...";
        }

        private void DoSafetyChecks()
        {
            if (VNavmesh_IPCSubscriber.Path_GetAlignCamera())
            {
                GatherBuddy.Log.Warning("VNavMesh Align Camera Option turned on! Forcing it off for GBR operation.");
                VNavmesh_IPCSubscriber.Path_SetAlignCamera(false);
            }
        }
    }
}
