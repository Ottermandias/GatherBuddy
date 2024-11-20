using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using GatherBuddy.CustomInfo;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using System;
using System.Linq;
using System.Numerics;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using GatherBuddy.SeFunctions;
using GatherBuddy.Data;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        private unsafe void EnqueueDismount()
        {
            TaskManager.Enqueue(StopNavigation);

            var am = ActionManager.Instance();
            TaskManager.Enqueue(() => { if (Dalamud.Conditions[ConditionFlag.Mounted]) am->UseAction(ActionType.Mount, 0); }, "Dismount");

            TaskManager.Enqueue(() => !Dalamud.Conditions[ConditionFlag.InFlight] && CanAct, 1000, "Wait for not in flight");
            TaskManager.Enqueue(() => { if (Dalamud.Conditions[ConditionFlag.Mounted]) am->UseAction(ActionType.Mount, 0); }, "Dismount 2");
            TaskManager.Enqueue(() => !Dalamud.Conditions[ConditionFlag.Mounted] && CanAct, 1000, "Wait for dismount");
            TaskManager.Enqueue(() => { if (!Dalamud.Conditions[ConditionFlag.Mounted]) TaskManager.DelayNextImmediate(500); } );//Prevent "Unable to execute command while jumping."
        }

        private unsafe void EnqueueMountUp()
        {
            var am = ActionManager.Instance();
            var mount = GatherBuddy.Config.AutoGatherConfig.AutoGatherMountId;
            Action doMount;

            if (IsMountUnlocked(mount) && am->GetActionStatus(ActionType.Mount, mount) == 0)
            {
                doMount = () => am->UseAction(ActionType.Mount, mount);
            }
            else
            {
                if (am->GetActionStatus(ActionType.GeneralAction, 24) != 0)
                {
                    return;
                }

                doMount = () => am->UseAction(ActionType.GeneralAction, 24);
            }

            TaskManager.Enqueue(StopNavigation);
            EnqueueActionWithDelay(doMount);
            TaskManager.Enqueue(() => Svc.Condition[ConditionFlag.Mounted], 2000);
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
            var distance = gameObject.Position.DistanceToPlayer();

            if (distance < 3)
            {
                var waitGP = targetItem.ItemData.IsCollectable && Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.MinimumGPForCollectable;
                waitGP |= !targetItem.ItemData.IsCollectable && Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.MinimumGPForGathering;

                if (Dalamud.Conditions[ConditionFlag.Mounted] && (waitGP || Dalamud.Conditions[ConditionFlag.InFlight] || GetConsumablesWithCastTime() > 0))
                {
                    //Try to dismount early. It would help with nodes where it is not possible to dismount at vnavmesh's provided floor point
                    EnqueueDismount();
                    TaskManager.Enqueue(() => {
                        //If early dismount failed, navigate to the nearest floor point
                        if (Dalamud.Conditions[ConditionFlag.Mounted] && Dalamud.Conditions[ConditionFlag.InFlight] && !Dalamud.Conditions[ConditionFlag.Diving])
                        {
                            var floor = VNavmesh_IPCSubscriber.Query_Mesh_PointOnFloor(Player.Position, false, 3);
                            Navigate(floor, true);
                            TaskManager.Enqueue(() => !IsPathGenerating);
                            TaskManager.Enqueue(() => !IsPathing, 1000);
                            EnqueueDismount();
                            //If even that fails, do advanced unstuck
                            TaskManager.Enqueue(() => { if (Dalamud.Conditions[ConditionFlag.Mounted]) _advancedUnstuck.Force(); });
                        }
                    });
                }
                else if (waitGP)
                {
                    StopNavigation();
                    AutoStatus = "Waiting for GP to regenerate...";
                } 
                else
                {
                    // Use consumables with cast time just before gathering a node when player is surely not mounted
                    if (GetConsumablesWithCastTime() is var consumable and > 0)
                    {
                        if (IsPathing)
                            StopNavigation();
                        else
                            EnqueueActionWithDelay(() => UseItem(consumable));
                    }
                    else
                    {
                        EnqueueNodeInteraction(gameObject, targetItem);
                        //The node could be behind a rock or a tree and not be interactable. This happened in the Endwalker, but seems not to be reproducible in the Dawntrail.
                        //Enqueue navigation anyway, just in case.
                        if (!Dalamud.Conditions[ConditionFlag.Diving])
                        {
                            TaskManager.Enqueue(() => { if (!Dalamud.Conditions[ConditionFlag.Gathering]) Navigate(gameObject.Position, false); });
                        }
                    }
                }
            }
            else if (distance < Math.Max(GatherBuddy.Config.AutoGatherConfig.MountUpDistance, 5) && !Dalamud.Conditions[ConditionFlag.Diving])
            {
                Navigate(gameObject.Position, false);
            }
            else
            {
                if (!Dalamud.Conditions[ConditionFlag.Mounted])
                {
                    EnqueueMountUp();
                }
                else
                {
                    Navigate(gameObject.Position, ShouldFly(gameObject.Position));
                }
            }
        }

        private Vector3? lastPosition = null;
        private DateTime lastMovementTime;
        private DateTime lastResetTime;


        private void StuckCheck()
        {
            if (GatherBuddy.Config.AutoGatherConfig.UseExperimentalUnstuck)
                return;
            
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
                            StopNavigation();
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

        private void StopNavigation()
        {
            // Reset navigation logic here
            // For example, reinitiate navigation to the destination
            CurrentDestination = default;
            if (VNavmesh_IPCSubscriber.IsEnabled)
            {
                //VNavmesh_IPCSubscriber.Nav_PathfindCancelAll();
                VNavmesh_IPCSubscriber.Path_Stop();
            }
            lastResetTime = DateTime.Now;
        }

        private void Navigate(Vector3 destination, bool shouldFly)
        {
            if (CurrentDestination == destination && (IsPathing || IsPathGenerating))
                return;

            //vnavmesh can't find a path on mesh underwater (because the character is basically flying), nor can it fly without a mount.
            //Ensure that you are always mounted when underwater.
            if (Dalamud.Conditions[ConditionFlag.Diving] && !Dalamud.Conditions[ConditionFlag.Mounted])
            {
                GatherBuddy.Log.Error("BUG: Navigate() called underwater without mounting up first");
                Enabled = false;
                return;
            }

            shouldFly |= Dalamud.Conditions[ConditionFlag.Diving];

            StopNavigation();
            CurrentDestination = destination;
            var correctedDestination = GetCorrectedDestination(CurrentDestination);
            GatherBuddy.Log.Debug($"Navigating to {destination} (corrected to {correctedDestination})");

            LastNavigationResult = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(correctedDestination, shouldFly);
        }

        private static Vector3 GetCorrectedDestination(Vector3 destination)
        {
            var correctedDestination = destination;
            if (WorldData.NodeOffsets.TryGetValue(destination, out var offset))
                correctedDestination = offset;

            try
            {
                correctedDestination = VNavmesh_IPCSubscriber.Query_Mesh_NearestPoint(correctedDestination, 3, 3);
                if (Vector3.Distance(correctedDestination, destination) is var distance and > 3)
                {
                    GatherBuddy.Log.Warning($"Offset is ignored, because distance {distance} is too large after correcting for mesh.");
                    correctedDestination = VNavmesh_IPCSubscriber.Query_Mesh_NearestPoint(destination, 3, 3);
                }
            }
            catch (Exception) { }

            return correctedDestination;
        }

        private void MoveToFarNode(Vector3 position)
        {
            var farNode = position;

            if (!Dalamud.Conditions[ConditionFlag.Mounted])
            {
                EnqueueMountUp();
            }
            else
            {
                Navigate(farNode, ShouldFly(farNode));
            }
        }

        private bool MoveToTerritory(ILocation location)
        {
            var aetheryte = location.ClosestAetheryte;

            var territory = location.Territory;
            if (ForcedAetherytes.ZonesWithoutAetherytes.FirstOrDefault(x => x.ZoneId == territory.Id).AetheryteId is var alt && alt > 0)
                territory = GatherBuddy.GameData.Aetherytes[alt].Territory;

            if (aetheryte == null || !Teleporter.IsAttuned(aetheryte.Id) || aetheryte.Territory != territory)
            {
                aetheryte = territory.Aetherytes
                    .Where(a => Teleporter.IsAttuned(a.Id))
                    .OrderBy(a => a.WorldDistance(territory.Id, location.IntegralXCoord, location.IntegralYCoord))
                    .FirstOrDefault();
            }
            if (aetheryte == null)
            {
                Communicator.PrintError("Couldn't find an attuned aetheryte to teleport to.");
                return false;
            }

            EnqueueActionWithDelay(() => Teleporter.Teleport(aetheryte.Id));
            TaskManager.Enqueue(() => Svc.Condition[ConditionFlag.BetweenAreas]);
            TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.BetweenAreas]);
            TaskManager.DelayNext(1500);

            return true;
        }
    }
}
