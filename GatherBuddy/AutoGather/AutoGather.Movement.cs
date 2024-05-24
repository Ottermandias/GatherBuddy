using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        private unsafe void Dismount()
        {
            if (!Dalamud.Conditions[ConditionFlag.Mounted]) return;
            var am = ActionManager.Instance();
            am->UseAction(ActionType.Mount, 0);
        }

        private void MoveToClosestAetheryte()
        {
            _plugin.Executor.GatherItem(ItemsToGather.FirstOrDefault());
            TaskManager.EnqueueDelay(8000);
        }

        private unsafe void MountUp()
        {
            if (!ShouldFly) return;
            var am = ActionManager.Instance();
            var mount = GatherBuddy.Config.AutoGatherConfig.AutoGatherMountId;
            if (am->GetActionStatus(ActionType.Mount, mount) != 0) return;
            am->UseAction(ActionType.Mount, mount);
        }

        private void MoveToCloseNode()
        {
            if (NearestNodeDistance < 3)
            {
                TaskManager.Enqueue(DoGatherTasks);
                return;
            }
            else
            {
                CurrentDestination = NearestNode?.Position ?? null;
                TaskManager.EnqueueDelay(500);
                TaskManager.Enqueue(MountUp);
                TaskManager.Enqueue(Navigate);
                TaskManager.Enqueue(WaitForDestination);
            }
        }

        private void WaitForDestination()
        {
            if (CurrentDestination == null) return;
            while (Vector3.Distance(Player.Object.Position, CurrentDestination.Value) > 3)
            {
                TaskManager.EnqueueDelay(500);
            }
        }

        private Vector3 _lastNavigatedDestination = Vector3.Zero;
        private void Navigate()
        {
            if (CurrentDestination == null)
            {
                GatherBuddy.Log.Verbose("No destination set, skipping navigation");
            }
            else if (_lastNavigatedDestination == CurrentDestination)
            {
                GatherBuddy.Log.Verbose("Already navigated to destination, skipping navigation");
            }
            else
            {
                GatherBuddy.Log.Verbose($"Navigating to {CurrentDestination}");
                _lastNavigatedDestination = CurrentDestination.Value;
                LastNavigationResult = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(CurrentDestination.Value, true);
            }
        }

        public int CurrentFarNodeIndex = 0;
        private void MoveToFarNode()
        {
            var farNode = DesiredNodeCoordsInZone.ElementAt(CurrentFarNodeIndex);
            if (Vector3.Distance(Player.Object.Position, farNode) < 50)
            {
                CurrentFarNodeIndex++;
                if (CurrentFarNodeIndex >= DesiredNodeCoordsInZone.Count)
                {
                    CurrentFarNodeIndex = 0;
                }
                CurrentDestination = null;
                return;
            }
            else
            {
                CurrentDestination = farNode;
                TaskManager.EnqueueDelay(500);
                TaskManager.Enqueue(MountUp);
                TaskManager.Enqueue(Navigate);
                TaskManager.Enqueue(WaitForDestination);
            }
        }
    }
}
