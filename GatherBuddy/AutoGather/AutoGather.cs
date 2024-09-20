using ECommons.Automation.LegacyTaskManager;
using GatherBuddy.Plugin;
using System;
using System.Linq;
using System.Numerics;
using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.UI;
using GatherBuddy.AutoGather.Movement;
using GatherBuddy.Classes;
using GatherBuddy.CustomInfo;
using GatherBuddy.Enums;
using HousingManager = GatherBuddy.SeFunctions.HousingManager;
using ECommons.Throttlers;
using ObjectKind = Dalamud.Game.ClientState.Objects.Enums.ObjectKind;
using GatherBuddy.Interfaces;
using GatherBuddy.Time;

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
        }

        private readonly OverrideMovement _movementController;

        private readonly GatherBuddy _plugin;
        private readonly SoundHelper _soundHelper;
        
        public           TaskManager TaskManager { get; }

        private bool _enabled { get; set; } = false;
        internal readonly GatheringTracker NodeTarcker = new();

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

                    TaskManager.Abort();
                    targetInfo                          = null;
                    _movementController.Enabled         = false;
                    _movementController.DesiredPosition = Vector3.Zero;
                    StopNavigation();
                    AutoStatus = "Idle...";
                }
                else
                {
                    RefreshNextTresureMapAllowance();
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
                return;
            }

            if (Lifestream_IPCSubscriber.IsEnabled)
            {
                TaskManager.Enqueue(VNavmesh_IPCSubscriber.Nav_PathfindCancelAll);
                TaskManager.Enqueue(VNavmesh_IPCSubscriber.Path_Stop);
                TaskManager.Enqueue(() => Lifestream_IPCSubscriber.ExecuteCommand("auto"));
                TaskManager.Enqueue(() => Svc.Condition[ConditionFlag.BetweenAreas]);
                TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.BetweenAreas]);
                TaskManager.DelayNext(1000);
            }
            else 
                GatherBuddy.Log.Warning("Lifestream not found or not ready");
        }

        private class NoGatherableItemsInNodeExceptions : Exception { }
        public void DoAutoGather()
        {
            if (!IsGathering)
                LuckUsed = new(0); //Reset the flag even if auto-gather was disabled mid-gathering

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

            if (FreeInventorySlots == 0)
            {
                AbortAutoGather("Inventory is full");
                return;
            }

            if (IsGathering)
            {
                if (targetInfo != null)
                {
                    if (targetInfo.Location != null && targetInfo.Item.NodeType is NodeType.Unspoiled or NodeType.Legendary)
                        VisitedTimedLocations[targetInfo.Location] = targetInfo.Time;

                    var target = Svc.Targets.Target;
                    if (target != null
                        && target.ObjectKind is ObjectKind.GatheringPoint
                        && targetInfo.Item.NodeType is NodeType.Regular or NodeType.Ephemeral
                        && VisitedNodes.Last?.Value != target.Position
                        && targetInfo.Location?.Territory.Id >= 397)
                    {
                        FarNodesSeenSoFar.Clear();
                        VisitedNodes.AddLast(target.Position);
                        while (VisitedNodes.Count > (targetInfo.Item.NodeType == NodeType.Regular ? 4 : 2))
                            VisitedNodes.RemoveFirst();
                    }
                }

                if (GatherBuddy.Config.AutoGatherConfig.DoGathering)
                {
                    AutoStatus = "Gathering...";
                    StopNavigation();
                    try
                    {
                        DoActionTasks(targetInfo?.Item);
                    }
                    catch (NoGatherableItemsInNodeExceptions)
                    {
                        UpdateItemsToGather();

                        //We may stuck in infinite loop attempt to gather the same item, therefore disable auto-gather
                        if (ItemsToGather.Count > 0 && targetInfo?.Item == ItemsToGather[0].Item)
                        {
                            AbortAutoGather("Couldn't gather any items from the last node, aborted");
                        }
                        else
                        {
                            CloseGatheringAddons();
                        }
                    }
                    return;
                }

                return;
            }

            if (IsPathGenerating)
            {
                AutoStatus = "Generating path...";
                advancedLastMovementTime = DateTime.Now;
                lastMovementTime = DateTime.Now;
                return;
            }

            if (IsPathing)
            {
                StuckCheck();
                AdvancedUnstuckCheck();
            }

            if (GatherBuddy.Config.AutoGatherConfig.DoMaterialize && !IsPathing && !Svc.Condition[ConditionFlag.Mounted] && SpiritBondMax > 0)
            {
                DoMateriaExtraction();
                return;
            }

            {//Block to limit the scope of the variable "next"
                UpdateItemsToGather();
                var next = ItemsToGather.FirstOrDefault();

                if (next == null)
                {
                    if (!_plugin.GatherWindowManager.ActiveItems.OfType<Gatherable>().Any(i => InventoryCount(i) < QuantityTotal(i) && !(i.IsTreasureMap && InventoryCount(i) != 0)))
                    {
                        AbortAutoGather();
                        return;
                    }

                    GoHome();
                    AutoStatus = "No available items to gather";
                    return;
                }

                foreach (var (loc, time) in VisitedTimedLocations)
                    if (time.End < AdjuctedServerTime)
                        VisitedTimedLocations.Remove(loc);

                if (targetInfo == null
                    || targetInfo.Location == null
                    || targetInfo.Time.End < AdjuctedServerTime
                    || targetInfo.Item != next.Item
                    || VisitedTimedLocations.ContainsKey(targetInfo.Location))
                {
                    //Find a new location only if the target item changes or the node expires to prevent switching to another node when a new one goes up
                    targetInfo = next;
                    FarNodesSeenSoFar.Clear();
                    VisitedNodes.Clear();
                }
            }

            if (targetInfo.Location == null)
            {
                //Should not happen because UpdateItemsToGather filters out all unaviable items
                GatherBuddy.Log.Debug("Couldn't find any location for the target item");
                return;
            }

            if (targetInfo.Item.IsTreasureMap && NextTresureMapAllowance == DateTime.MinValue)
            {
                //Wait for timer refresh
                RefreshNextTresureMapAllowance();
                return;
            }

            if (targetInfo.Location.Territory.Id != Svc.ClientState.TerritoryType || !LocationMatchesJob(targetInfo.Location))
            {
                StopNavigation();
                MoveToTerritory(targetInfo.Location);
                AutoStatus = "Teleporting...";
                return;
            }

            DoUseConsumablesWithoutCastTime();

            var allPositions = targetInfo.Location.WorldPositions
                .SelectMany(w => w.Value)
                .Where(v => !IsBlacklisted(v))
                .ToHashSet();

            var visibleNodes = Svc.Objects
                .Where(o => allPositions.Contains(o.Position))
                .ToList();

            var closestTargetableNode = visibleNodes
                .Where(o => o.IsTargetable)
                .MinBy(o => Vector3.Distance(Player.Position, o.Position));

            if (closestTargetableNode != null)
            {
                AutoStatus = "Moving to node...";
                MoveToCloseNode(closestTargetableNode, targetInfo.Item);
                return;
            }

            AutoStatus = "Moving to far node...";

            if (CurrentDestination != default && !allPositions.Contains(CurrentDestination))
            {
                GatherBuddy.Log.Debug("Current destination doesn't match the target item, resetting navigation");
                StopNavigation();
                FarNodesSeenSoFar.Clear();
                VisitedNodes.Clear();
            }

            if (CurrentDestination != default)
            {
                var currentNode = visibleNodes.FirstOrDefault(o => o.Position == CurrentDestination);
                if (currentNode != null && !currentNode.IsTargetable)
                    GatherBuddy.Log.Verbose($"Far node is not targetable, distance {currentNode.Position.DistanceToPlayer()}.");

                //It takes some time (roundtrip to the server) before a node becomes targetable after it becomes visible,
                //so we need to delay excluding it. But instead of measuring time, we use distance, since character is traveling at a constant speed.
                //Value 80 was determined empirically.
                foreach (var node in visibleNodes.Where(o => o.Position.DistanceToPlayer() < 80))
                    FarNodesSeenSoFar.Add(node.Position);

                if (FarNodesSeenSoFar.Contains(CurrentDestination))
                {
                    GatherBuddy.Log.Verbose("Far node is not targetable, choosing another");
                }
                else
                {
                    return;
                }
            }

            Vector3 selectedFarNode;

            // only Legendary and Unspoiled show marker
            if (ShouldUseFlag && targetInfo.Item.NodeType is NodeType.Legendary or NodeType.Unspoiled)
            {
                var pos = TimedNodePosition;
                // marker not yet loaded on game
                if (pos == null)
                {
                    AutoStatus = "Waiting on flag show up";
                    return;
                }

                selectedFarNode = allPositions
                    .Where(o => Vector2.Distance(pos.Value, new Vector2(o.X, o.Z)) < 10)
                    .OrderBy(o => Vector2.Distance(pos.Value, new Vector2(o.X, o.Z)))
                    .FirstOrDefault();
            }
            else
            {
                //Select the furthermost node from the last 4 visited ones (2 for ephemeral), ARR excluded.
                selectedFarNode = allPositions
                    .OrderByDescending(n => VisitedNodes.Select(v => Vector3.Distance(n, v)).Sum())
                    .ThenBy(v => Vector3.Distance(Player.Position, v))
                    .FirstOrDefault(n => !FarNodesSeenSoFar.Contains(n));

                if (selectedFarNode == default)
                {
                    FarNodesSeenSoFar.Clear();
                    GatherBuddy.Log.Verbose($"Selected node was null and far node filters have been cleared");
                    return;
                }

            }
             
            MoveToFarNode(selectedFarNode);
        }

        private void AbortAutoGather(string? status = null)
        {
            Enabled = false;
            if (!string.IsNullOrEmpty(status))
                AutoStatus = status;
            if (GatherBuddy.Config.AutoGatherConfig.HonkMode)
                _soundHelper.PlayHonkSound(3);
            CloseGatheringAddons();
            TaskManager.Enqueue(GoHome);
        }

        private unsafe void CloseGatheringAddons()
        {
            if (MasterpieceAddon != null)
                TaskManager.Enqueue(() => MasterpieceAddon->Close(true));

            if (GatheringAddon != null)
                TaskManager.Enqueue(() => GatheringAddon->Close(true));

            TaskManager.Enqueue(() => !IsGathering);
        }

        private static unsafe void RefreshNextTresureMapAllowance()
        {
            if (EzThrottler.Throttle("RequestResetTimestamps", 1000))
            {
                FFXIVClientStructs.FFXIV.Client.Game.UI.UIState.Instance()->RequestResetTimestamps();
            }
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
            NodeTarcker.Dispose();
        }
    }
}
