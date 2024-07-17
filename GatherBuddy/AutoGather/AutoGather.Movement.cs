using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using ECommons.Throttlers;
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
using FFXIVClientStructs.FFXIV.Client.Game.UI;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        private unsafe void Dismount()
        {
            if (!Dalamud.Conditions[ConditionFlag.Mounted])
                return;

            var am = ActionManager.Instance();
            am->UseAction(ActionType.Mount, 0);
        }

        private void MoveToClosestAetheryte()
        {
            if (EzThrottler.Throttle("Teleport", 10000))
            {
                var item = ItemsToGather.FirstOrDefault();
                if (item == null)
                    return;

                var location = _plugin.Executor.FindClosestLocation(item);
                if (location == null)
                    return;

                VNavmesh_IPCSubscriber.Path_Stop();
                TaskManager.Enqueue(() => _plugin.Executor.GatherItem(item));
            }
        }

        private unsafe void MountUp()
        {
            if (EzThrottler.Throttle("MountUp", 10))
            {
                VNavmesh_IPCSubscriber.Path_Stop();
                var am = ActionManager.Instance();
                if (Vector3.Distance(Player.Object.Position, CurrentDestination ?? Vector3.Zero)
                 <= GatherBuddy.Config.AutoGatherConfig.MountUpDistance)
                    return;

                var mount = GatherBuddy.Config.AutoGatherConfig.AutoGatherMountId;
                if (am->GetActionStatus(ActionType.Mount, mount) != 0)
                    return;

                am->UseAction(ActionType.Mount, mount);
            }
        }

        private void MoveToCloseNode()
        {
            if (NearestNodeDistance < 3)
            {
                VNavmesh_IPCSubscriber.Path_Stop();
                if (!Dalamud.Conditions[ConditionFlag.Gathering]
                 && Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.MinimumGPForGathering)
                {
                    AutoStatus = "Waiting for GP to regenerate...";
                    return;
                }

                if (Dalamud.Conditions[ConditionFlag.Mounted])
                {
                    TaskManager.Enqueue(Dismount);
                    TaskManager.DelayNext(1000);
                }

                TaskManager.Enqueue(InteractWithNode);
                return;
            }
            else if (NearestNodeDistance > 3 && NearestNodeDistance < GatherBuddy.Config.AutoGatherConfig.MountUpDistance)
            {
                CurrentDestination = NearestNode?.Position ?? null;
                TaskManager.Enqueue(() => Navigate(false));
            }
            else
            {
                CurrentDestination = NearestNode?.Position ?? null;
                if (!Dalamud.Conditions[ConditionFlag.Mounted])
                {
                    TaskManager.Enqueue(MountUp);
                    TaskManager.DelayNext(2500);
                }

                TaskManager.Enqueue(() => Navigate(ShouldFly));
            }

            AutoStatus = "Moving to node...";
        }

        private Vector3? lastPosition = null;
        private DateTime lastMovementTime;
        private DateTime lastResetTime;


        private void StuckCheck()
        {
            if (EzThrottler.Throttle("StuckCheck", 100))
            {
                // Check if character is stuck
                if (lastPosition.HasValue && Vector3.Distance(Player.Object.Position, lastPosition.Value) < 2.0f)
                {
                    // If the character hasn't moved much
                    if ((DateTime.Now - lastMovementTime).TotalSeconds > GatherBuddy.Config.AutoGatherConfig.NavResetThreshold)
                    {
                        // Check if enough time has passed since the last reset
                        if ((DateTime.Now - lastResetTime).TotalSeconds > GatherBuddy.Config.AutoGatherConfig.NavResetCooldown)
                        {
                            GatherBuddy.Log.Warning("Character is stuck, resetting navigation...");
                            ResetNavigation();
                            lastResetTime = DateTime.Now;
                            return;
                        }
                    }
                }
                else
                {
                    // Character has moved, update last known position and time
                    lastPosition     = Player.Object.Position;
                    lastMovementTime = DateTime.Now;
                }
            }
        }

        private void ResetNavigation()
        {
            // Reset navigation logic here
            // For example, reinitiate navigation to the destination
            _lastNavigatedDestination = Vector3.Zero;
            CurrentDestination        = null;
            VNavmesh_IPCSubscriber.Path_Stop();
        }

        private bool IsCloseToDestination()
        {
            if (CurrentDestination == null)
                return false;

            return Vector3.Distance(Player.Object.Position, CurrentDestination.Value) <= 3;
        }

        private Vector3 _lastNavigatedDestination = Vector3.Zero;

        private void Navigate(bool shouldFly)
        {
            if (EzThrottler.Throttle("Navigate", 10))
            {
                if (CurrentDestination == null)
                {
                    GatherBuddy.Log.Verbose("No destination set, skipping navigation");
                }
                else if (IsPathing)
                {
                    GatherBuddy.Log.Verbose("Already navigating, skipping navigation");
                }
                else
                {
                    VNavmesh_IPCSubscriber.Path_Stop();
                    GatherBuddy.Log.Verbose($"Navigating to {CurrentDestination}");
                    _lastNavigatedDestination = CurrentDestination.Value;
                    var correctedDestination = shouldFly ? CurrentDestination.Value.CorrectForMesh() : CurrentDestination.Value;
                    if (!correctedDestination.SanityCheck())
                    {
                        GatherBuddy.Log.Warning($"Invalid destination: {correctedDestination}");
                        CurrentDestination = null;
                        ResetNavigation();
                        return;
                    }
                    LastNavigationResult = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(correctedDestination, shouldFly);
                }
            }
        }

        public int CurrentFarNodeIndex = 0;

        private void MoveToFarNode()
        {
            var farNode = DesiredNodeCoordsInZone.ElementAtOrDefault(CurrentFarNodeIndex);
            if (farNode == null
             || Vector3.Distance(Player.Object.Position, farNode) < GatherBuddy.Config.AutoGatherConfig.FarNodeFilterDistance)
            {
                CurrentFarNodeIndex++;
                if (CurrentFarNodeIndex >= DesiredNodeCoordsInZone.Count)
                {
                    CurrentFarNodeIndex = 0;
                }

                CurrentDestination = null;
                AutoStatus         = "Looking for far away nodes...";
                return;
            }
            else
            {
                AutoStatus         = "Moving to far node...";
                CurrentDestination = farNode;
                if (!Dalamud.Conditions[ConditionFlag.Mounted])
                {
                    TaskManager.Enqueue(MountUp);
                    TaskManager.DelayNext(2500);
                }

                TaskManager.Enqueue(() => Navigate(ShouldFly));
            }
        }

        private void MoveToFlag()
        {
            if (Vector3.Distance(MapFlagPosition.Value, Player.Object.Position) < 50)
            {
                HasSeenFlag = true;
            }
            CurrentDestination = MapFlagPosition;
            if (!Dalamud.Conditions[ConditionFlag.Mounted])
            {
                TaskManager.Enqueue(MountUp);
                TaskManager.DelayNext(2500);
            }

            TaskManager.Enqueue(() => Navigate(ShouldFly));
        }
    }
}
