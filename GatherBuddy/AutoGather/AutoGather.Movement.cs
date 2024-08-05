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
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.GeneratedSheets;

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

                if (!IsMountUnlocked(mount))
                {
                    if (am->GetActionStatus(ActionType.GeneralAction, 24) != 0)
                    {
                        return;
                    }

                    am->UseAction(ActionType.GeneralAction, 24);
                    return;
                }

                am->UseAction(ActionType.Mount, mount);
            }
        }

        private unsafe bool IsMountUnlocked(uint mount)
        {
            var instance = PlayerState.Instance();
            if (instance == null)
                return false;

            return instance->IsMountUnlocked(mount);
        }

        private void MoveToCloseNode(IGameObject gameObject, Gatherable targetItem)
        {
            var distance = Vector3.Distance(Player.Position, gameObject.Position);
            if (distance < 3)
            {
                if (!Dalamud.Conditions[ConditionFlag.Gathering]
                 && (targetItem.ItemData.IsCollectable && Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.MinimumGPForCollectable
                     || !targetItem.ItemData.IsCollectable
                     && Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.MinimumGPForGathering))
                {
                    if (IsPathing || IsPathGenerating)
                        VNavmesh_IPCSubscriber.Path_Stop();
                    AutoStatus = "Waiting for GP to regenerate...";
                    return;
                }

                if (Dalamud.Conditions[ConditionFlag.InFlight] && GatherBuddy.Config.AutoGatherConfig.UseExperimentalNavigation)
                {
                    Vector3 floorPoint;
                    try
                    {
                        floorPoint = VNavmesh_IPCSubscriber.Query_Mesh_PointOnFloor(Player.Position, 1f, 1f);
                        GatherBuddy.Log.Debug($"Got floor point {floorPoint} from vnavmesh while trying to land");
                    }
                    catch
                    {
                        AutoStatus = "We're stuck in flight and navmesh can't see the floor!";
                        return;
                    }

                    CurrentDestination = floorPoint;
                    TaskManager.Enqueue(() => Navigate(ShouldFly));
                    TaskManager.DelayNext(1000);
                }

                if (Dalamud.Conditions[ConditionFlag.Mounted])
                {
                    AdvancedUnstuckCheck();
                    TaskManager.Enqueue(() => VNavmesh_IPCSubscriber.Path_Stop());
                    TaskManager.Enqueue(Dismount);
                    TaskManager.DelayNext(1500);
                    return;
                }

                if (!Dalamud.Conditions[ConditionFlag.Mounted] && !Dalamud.Conditions[ConditionFlag.Jumping])
                {
                    // Use consumables with cast time just before gathering a node when player is surely not mounted
                    DoUseConsumablesWithCastTime();

                    TaskManager.Enqueue(() => InteractWithNode(gameObject, targetItem));
                    return;
                }

                AutoStatus = "We fell out of a loop that it shouldn't be possible to fall out of. What did you do?!?!";
            }
            else if (distance > 3 && distance < GatherBuddy.Config.AutoGatherConfig.MountUpDistance)
            {
                CurrentDestination = gameObject?.Position ?? null;
                TaskManager.Enqueue(() => Navigate(false));
            }
            else
            {
                CurrentDestination = gameObject?.Position ?? null;
                if (!Dalamud.Conditions[ConditionFlag.Mounted])
                {
                    TaskManager.Enqueue(MountUp);
                    TaskManager.Enqueue(() => Svc.Condition[ConditionFlag.Mounted]);
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
                if (CurrentDestination == null || IsPathing)
                    return;
                VNavmesh_IPCSubscriber.Path_Stop();
                GatherBuddy.Log.Verbose($"Navigating to {CurrentDestination}");
                _lastNavigatedDestination = CurrentDestination.Value;
                var loop                 = 1;
                Vector3 correctedDestination = GetCorrectedDestination(shouldFly);
                while (Vector3.Distance(correctedDestination, CurrentDestination.Value) > 15 && loop < 8)
                {
                    GatherBuddy.Log.Information("Distance last node and gatherpoint is too big : "
                      + Vector3.Distance(correctedDestination, CurrentDestination.Value));
                    correctedDestination = shouldFly ? CurrentDestination.Value.CorrectForMesh(loop * 0.5f) : CurrentDestination.Value;
                    loop++;
                }

                if (Vector3.Distance(correctedDestination, CurrentDestination.Value) > 10)
                {
                    GatherBuddy.Log.Warning($"Invalid destination: {correctedDestination}");
                    ResetNavigation();
                    return;
                }

                if (!correctedDestination.SanityCheck())
                {
                    GatherBuddy.Log.Warning($"Invalid destination: {correctedDestination}");
                    ResetNavigation();
                    return;
                }

                LastNavigationResult = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(correctedDestination, shouldFly);
            }
        }

        private Vector3 GetCorrectedDestination(bool shouldFly)
        {
            var selectedOffset = WorldData.NodeOffsets.FirstOrDefault(o => o.Original == CurrentDestination.Value);
            if (selectedOffset != null)
            {
                return selectedOffset.Offset;
            }
            else
            {
                return shouldFly ? CurrentDestination.Value.CorrectForMesh(0.5f) : CurrentDestination.Value;
            }
        }

        public List<Vector3> FarNodesSeenSoFar = new();

        private void MoveToFarNode(Vector3 position)
        {
            var farNode = position;

            AutoStatus         = "Moving to far node...";
            CurrentDestination = farNode;
            if (!Dalamud.Conditions[ConditionFlag.Mounted])
            {
                TaskManager.Enqueue(MountUp);
                TaskManager.Enqueue(() => Svc.Condition[ConditionFlag.Mounted]);
            }

            TaskManager.Enqueue(() => Navigate(ShouldFly));
        }

        private void MoveToTerritory(ILocation location)
        {
            TaskManager.EnqueueImmediate(() => _plugin.Executor.GatherLocation(location));
            if (location.Id != Svc.ClientState.TerritoryType)
            {
                TaskManager.Enqueue(() => Svc.Condition[ConditionFlag.BetweenAreas]);
                TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.BetweenAreas]);
                TaskManager.DelayNext(3000);
            }
        }

        private Vector3? advandedLastPosition = null;
        private DateTime advancedLastMovementTime;
        private DateTime advancedMovementStart = DateTime.MinValue;

        private void AdvancedUnstuckCheck()
        {
            if (!GatherBuddy.Config.AutoGatherConfig.UseExperimentalUnstuck)
                return;

            if (advandedLastPosition.HasValue
             && Vector3.Distance(Player.Object.Position, advandedLastPosition.Value) < 2.0f
             && !_movementController.Enabled)
            {
                // If the character hasn't moved much
                if ((DateTime.Now - advancedLastMovementTime).TotalSeconds > GatherBuddy.Config.AutoGatherConfig.NavResetThreshold)
                {
                    GatherBuddy.Log.Warning($"Character is stuck, using advanced unstuck methods");
                    if (!_movementController.Enabled)
                    {
                        Vector3 newPosition = CurrentDestination ?? Player.Position + new Vector3(1, 1, 1);
                        _movementController.DesiredPosition = newPosition;
                        _movementController.Enabled         = true;
                        advancedMovementStart               = DateTime.Now;
                        //VNavmesh_IPCSubscriber.Path_Stop();
                    }
                }
            }
            else if (_movementController.Enabled && (DateTime.Now - advancedMovementStart).TotalSeconds > 1.5)
            {
                _movementController.Enabled         = false;
                _movementController.DesiredPosition = Vector3.Zero;
            }
            else
            {
                // Character has moved, update last known position and time
                advandedLastPosition     = Player.Object.Position;
                advancedLastMovementTime = DateTime.Now;
            }
        }
    }
}
