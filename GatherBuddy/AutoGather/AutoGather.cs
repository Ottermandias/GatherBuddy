using ECommons.Automation.NeoTaskManager;
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
                GatherBuddy.Log.Verbose("TaskManager has tasks, skipping DoAutoGather");
                return;
            }

            if (ValidNodesInRange.Any())
            {
                TaskManager.Enqueue(MoveToCloseNode);
            }
            else if (DesiredNodesInZone.Any())
            {
                TaskManager.Enqueue(MoveToFarNode);
            }
            else if (ItemsToGather.Any())
            {
                TaskManager.Enqueue(MoveToClosestAetheryte);
            }
            else
            {
                AutoStatus = "Nothing to do...";
            }
        }
    }
}
