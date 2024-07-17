using ECommons.Automation.LegacyTaskManager;
using GatherBuddy.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.UI;
using GatherBuddy.CustomInfo;
using GatherBuddy.Enums;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        public AutoGather(GatherBuddy plugin)
        {
            // Initialize the task manager
            TaskManager = new();
            _plugin     = plugin;
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
                    AutoStatus     = "Idle...";
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

            if (IsPathing)
            {
                TaskManager.Enqueue(StuckCheck);
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

            var location = _plugin.Executor.FindClosestLocation(ItemsToGather.FirstOrDefault());
            if (location == null)
            {
                AutoStatus = "No locations to travel to ...";
                return;
            }

            if (ValidNodesInRange.Any())
            {
                HiddenRevealed = false;
                TaskManager.Enqueue(MoveToCloseNode);
                return;
            }

            if (location.Territory.Id != Dalamud.ClientState.TerritoryType || location.GatheringType.ToGroup() != JobAsGatheringType)
            {
                AutoStatus  = $"Moving to gather item in {location.Territory.Name}...";
                HasSeenFlag = false;
                TaskManager.Enqueue(VNavmesh_IPCSubscriber.Path_Stop);
                TaskManager.Enqueue(MoveToClosestAetheryte);
                return;
            }

            if (MapFlagPosition != null && MapFlagPosition.Value.DistanceToPlayer() > 150 && ShouldUseFlag)
            {
                AutoStatus = "Moving to farming area...";
                TaskManager.Enqueue(MoveToFlag);
                return;
            }

            if (DesiredNodeCoordsInZone.Any())
            {
                TaskManager.Enqueue(MoveToFarNode);
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
