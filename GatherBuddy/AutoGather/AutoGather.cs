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
using ECommons.Throttlers;

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
                    _movementController.Enabled         = false;
                    _movementController.DesiredPosition = Vector3.Zero;
                    ResetNavigation();
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

        private class NoGatherableItemsInNodeExceptions : Exception { }
        public void DoAutoGather()
        {
            if (!IsGathering)
                HiddenRevealed = false; //Reset the "Used Luck" flag event if auto-gather was disabled mid-gathering

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

            if (!IsGathering) UpdateItemsToGather();
            Gatherable? targetItem = ItemsToGather.FirstOrDefault() as Gatherable;

            if (targetItem == null)
            {
                if (!_plugin.GatherWindowManager.ActiveItems.Any(i => InventoryCount(i) < QuantityTotal(i) && !(IsTreasureMap(i) && InventoryCount(i) != 0)))
                {
                    AbortAutoGather();
                    return;
                }

                GoHome();
                //GatherBuddy.Log.Warning("No items to gather");
                AutoStatus = "No available items to gather";
                return;
            }

            if (IsTreasureMap(targetItem) && NextTresureMapAllowance == DateTime.MinValue)
            {
                //Wait for timer refresh
                return;
            }

            if (IsGathering && GatherBuddy.Config.AutoGatherConfig.DoGathering)
            {
                AutoStatus = "Gathering...";
                TaskManager.Enqueue(VNavmesh_IPCSubscriber.Path_Stop);
                try
                {
                    DoActionTasks(targetItem);
                }
                catch (NoGatherableItemsInNodeExceptions)
                {
                    UpdateItemsToGather();

                    //We may stuck in infinite loop attempt to gather the same item, therefore disable auto-gather
                    if (targetItem == ItemsToGather.FirstOrDefault())
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

            if (IsGathering)
                return;

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

            if (!IsPathing && !Svc.Condition[ConditionFlag.Mounted] && SpiritBondMax > 0 && GatherBuddy.Config.AutoGatherConfig.DoMaterialize)
            {
                DoMateriaExtraction();
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

            DoUseConsumablesWithoutCastTime();

            var validNodesForItem = targetItem.NodeList.SelectMany(n => n.WorldPositions).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var matchingNodesInZone = location.Location.WorldPositions.Where(w => validNodesForItem.ContainsKey(w.Key)).SelectMany(w => w.Value)
                .Where(v => !IsBlacklisted(v))
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

            if (allNodes.Any(n => n.Position == selectedNode && Vector3.Distance(n.Position, Player.Position) < 100))
            {
                FarNodesSeenSoFar.Add(selectedNode);

                CurrentDestination = null;
                VNavmesh_IPCSubscriber.Path_Stop();
                AutoStatus = "Looking for far away nodes...";
                return;
            }

            TaskManager.Enqueue(() => MoveToFarNode(selectedNode));
            return;


            AutoStatus = "Nothing to do...";
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
        }
    }
}
