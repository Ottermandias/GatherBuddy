using ECommons.Automation.NeoTaskManager;
using GatherBuddy.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        public AutoGather(GatherBuddy plugin)
        {
            // Initialize the task manager
            TaskManager = new();
            _plugin = plugin;
        }

        private GatherBuddy _plugin;

        public TaskManager TaskManager { get; }
        public bool Enabled { get; set; } = false;

        public void DoAutoGather()
        {
            if (!Enabled)
                return;
            if (TaskManager.Tasks.Count > 0)
            {
                //GatherBuddy.Log.Verbose("TaskManager has tasks, skipping DoAutoGather");
                return;
            }
            try
            {
                if (!NavReady)
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
            if (!ItemsToGather.Any())
            {
                AutoStatus = "No items to gather...";
                Enabled = false;
                VNavmesh_IPCSubscriber.Path_Stop();
                return;
            }
            var location = _plugin.Executor.FindClosestLocation(ItemsToGather.FirstOrDefault());
            if (location == null)
            {
                AutoStatus = "No locations found...";
                Enabled = false;
                VNavmesh_IPCSubscriber.Path_Stop();
                return;
            }
            if (!CanAct)
            {
                AutoStatus = "Player is busy...";
                return;
            }
            else if (IsGathering)
            {
                AutoStatus = "Gathering...";
                TaskManager.Enqueue(VNavmesh_IPCSubscriber.Path_Stop);
                TaskManager.Enqueue(DoActionTasks);
            }
            else if (location.Territory.Id != Dalamud.ClientState.TerritoryType)
            {
                AutoStatus = $"Teleporting to {location.Territory.Name}...";
                TaskManager.Enqueue(VNavmesh_IPCSubscriber.Path_Stop);
                TaskManager.Enqueue(MoveToClosestAetheryte);
            }
            else if (IsPathGenerating)
            {
                AutoStatus = "Generating path...";
            }
            else if (ValidNodesInRange.Any())
            {
                AutoStatus = "Moving to node...";
                HiddenRevealed = false;
                TaskManager.Enqueue(MoveToCloseNode);
            }
            else if (DesiredNodesInZone.Any())
            {
                AutoStatus = "Moving to far node...";
                TaskManager.Enqueue(MoveToFarNode);
            }
            else
            {
                AutoStatus = "Nothing to do...";
            }
        }
    }
}
