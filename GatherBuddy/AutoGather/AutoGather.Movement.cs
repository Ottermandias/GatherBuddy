using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using GatherBuddy.CustomInfo;
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
                if (Dalamud.Conditions[ConditionFlag.Mounted])
                {
                    TaskManager.Enqueue(Dismount);
                }
                TaskManager.Enqueue(DoGatherTasks);
                return;
            }
            else
            {
                CurrentDestination = NearestNode?.Position ?? null;
                TaskManager.EnqueueDelay(3000);
                TaskManager.Enqueue(MountUp);
                TaskManager.Enqueue(Navigate);
                TaskManager.Enqueue(WaitForDestination);
            }
        }

        private Vector3? lastPosition = null;
        private DateTime lastMovementTime;
        private const int stuckThresholdSeconds = 2; // Define the time threshold for being stuck

        private void WaitForDestination()
        {
            if (CurrentDestination == null) return;

            if (!IsCloseToDestination())
            {
                // Check if character is stuck
                if (lastPosition.HasValue && Vector3.Distance(Player.Object.Position, lastPosition.Value) < 1.0f)
                {
                    // If the character hasn't moved much
                    if ((DateTime.Now - lastMovementTime).TotalSeconds > stuckThresholdSeconds)
                    {
                        GatherBuddy.Log.Warning("Character is stuck, resetting navigation...");
                        ResetNavigation();
                        return;
                    }
                }
                else
                {
                    // Character has moved, update last known position and time
                    lastPosition = Player.Object.Position;
                    lastMovementTime = DateTime.Now;
                }

                // If not close, enqueue the check again after a delay
                TaskManager.Enqueue(() => TaskManager.EnqueueDelay(50));
                TaskManager.Enqueue(WaitForDestination);
            }
            else
            {
                // Arrived at the destination
                GatherBuddy.Log.Verbose("Arrived at the destination");
                lastPosition = null; // Reset last known position
            }
        }
        private void ResetNavigation()
        {
            // Reset navigation logic here
            // For example, reinitiate navigation to the destination
            _lastNavigatedDestination = Vector3.Zero;
            CurrentDestination = null;
            VNavmesh_IPCSubscriber.Path_Stop();
        }
        private bool IsCloseToDestination()
        {
            if (CurrentDestination == null) return false;
            return Vector3.Distance(Player.Object.Position, CurrentDestination.Value) <= 3;
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
                LastNavigationResult = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(CurrentDestination.Value.CorrectForMesh(), ShouldFly);
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
