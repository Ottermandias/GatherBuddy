using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using GatherBuddy.CustomInfo;
using GatherBuddy.Data;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using GatherBuddy.SeFunctions;
using System;
using System.Linq;
using System.Numerics;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using GatherBuddy.SeFunctions;
using GatherBuddy.Data;
using ECommons.MathHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using GatherBuddy.Enums;
using Aetheryte = GatherBuddy.Classes.Aetheryte;

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

            if (!GatherBuddy.Config.AutoGatherConfig.MoveWhileMounting)
                TaskManager.Enqueue(StopNavigation);
            EnqueueActionWithDelay(doMount);
            TaskManager.Enqueue(() => Svc.Condition[ConditionFlag.Mounted], 2000);

            // Reset navigation to find a better path if the mount can fly
            if (GatherBuddy.Config.AutoGatherConfig.MoveWhileMounting && !Dalamud.Conditions[ConditionFlag.Diving])
                TaskManager.Enqueue(StopNavigation);
        }

        private unsafe bool IsMountUnlocked(uint mount)
        {
            var instance = PlayerState.Instance();
            if (instance == null)
                return false;

            return instance->IsMountUnlocked(mount);
        }

        private void MoveToCloseNode(IGameObject gameObject, Gatherable targetItem, ConfigPreset config)
        {
            // We can open a node with less than 3 vertical and less than 3.5 horizontal separation
            var hSeparation = Vector2.Distance(gameObject.Position.ToVector2(), Player.Position.ToVector2());
            var vSeparation = Math.Abs(gameObject.Position.Y - Player.Position.Y);

            if (hSeparation < 3.5)
            {
                
                var waitGP = targetItem.ItemData.IsCollectable && Player.Object.CurrentGp < config.CollectableMinGP;
                waitGP |= !targetItem.ItemData.IsCollectable && Player.Object.CurrentGp < config.GatherableMinGP;

                if (Dalamud.Conditions[ConditionFlag.Mounted] && (waitGP || GetConsumablesWithCastTime(config) > 0))
                {
                    //Try to dismount early. It would help with nodes where it is not possible to dismount at vnavmesh's provided floor point
                    EnqueueDismount();
                    TaskManager.Enqueue(() => {
                        //If early dismount failed, navigate to the nearest floor point
                        if (Dalamud.Conditions[ConditionFlag.Mounted] && Dalamud.Conditions[ConditionFlag.InFlight] && !Dalamud.Conditions[ConditionFlag.Diving])
                        {
                            try
                            {
                                var floor = VNavmesh.Query.Mesh.PointOnFloor(Player.Position, false, 3);
                                Navigate(floor, true);
                                TaskManager.Enqueue(() => !IsPathGenerating);
                                TaskManager.DelayNext(50);
                                TaskManager.Enqueue(() => !IsPathing, 1000);
                                EnqueueDismount();
                            }
                            catch { }
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
                    if (GetConsumablesWithCastTime(config) is var consumable and > 0)
                    {
                        if (IsPathing)
                            StopNavigation();
                        else
                            EnqueueActionWithDelay(() => UseItem(consumable));
                    }
                    else
                    {
                        // Check perception requirement before interacting with node
                        if (DiscipleOfLand.Perception < targetItem.GatheringData.PerceptionReq)
                        {
                            Communicator.PrintError($"Insufficient Perception to gather this item. Required: {targetItem.GatheringData.PerceptionReq}, current: {DiscipleOfLand.Perception}");
                            AbortAutoGather();
                            return;
                        }

                        if (vSeparation < 3)
                        {
                            
                            var targetGatheringType = targetItem.GatheringType.ToGroup();
                            var isUmbralItem = UmbralNodes.IsUmbralItem(targetItem.ItemId);
                            if (isUmbralItem && Functions.InTheDiadem())
                            {
                                var currentWeather = EnhancedCurrentWeather.GetCurrentWeatherId();
                                if (UmbralNodes.IsUmbralWeather(currentWeather))
                                {
                                    var umbralWeather = (UmbralNodes.UmbralWeatherType)currentWeather;
                                    targetGatheringType = umbralWeather switch
                                    {
                                        UmbralNodes.UmbralWeatherType.UmbralFlare => GatheringType.Miner,
                                        UmbralNodes.UmbralWeatherType.UmbralLevin => GatheringType.Miner,
                                        UmbralNodes.UmbralWeatherType.UmbralDuststorms => GatheringType.Botanist,
                                        UmbralNodes.UmbralWeatherType.UmbralTempest => GatheringType.Botanist,
                                        _ => targetGatheringType
                                    };
                                }
                            }
                            
                            var shouldSkipJobSwitch = Functions.InTheDiadem() && _hasGatheredUmbralThisSession;
                            
                            if (targetGatheringType != JobAsGatheringType && targetGatheringType != GatheringType.Multiple && !shouldSkipJobSwitch) {
                                if (ChangeGearSet(targetGatheringType, 0)){
                                    EnqueueNodeInteraction(gameObject, targetItem);
                                } else {
                                    AbortAutoGather();
                                }
                            }
                            else {
                                if (shouldSkipJobSwitch && targetGatheringType != JobAsGatheringType)
                                {
                                    Svc.Log.Information($"[Umbral] Skipping job switch at node after umbral gathering (staying on {JobAsGatheringType})");
                                }
                                EnqueueNodeInteraction(gameObject, targetItem);
                            }
                        }

                        // The node could be behind a rock or a tree and not be interactable. This happened in the Endwalker, but seems not to be reproducible in the Dawntrail.
                        // Enqueue navigation anyway, just in case.
                        // Also move if vertical separation is too large.
                        if (!Dalamud.Conditions[ConditionFlag.Diving])
                        {
                            TaskManager.Enqueue(() => { if (!Dalamud.Conditions[ConditionFlag.Gathering]) Navigate(gameObject.Position, false); });
                        }
                    }
                }
            }
            else if (hSeparation < Math.Max(GatherBuddy.Config.AutoGatherConfig.MountUpDistance, 5))
            {
                Navigate(gameObject.Position, false);
            }
            else
            {
                if (!Dalamud.Conditions[ConditionFlag.Mounted])
                {
                    if (GatherBuddy.Config.AutoGatherConfig.MoveWhileMounting)
                        Navigate(gameObject.Position, false);
                    EnqueueMountUp();
                }
                else
                {
                    Navigate(gameObject.Position, ShouldFly(gameObject.Position));
                }
            }
        }

        private void StopNavigation()
        {
            // Reset navigation logic here
            // For example, reinitiate navigation to the destination
            CurrentDestination = default;
            CurrentRotation    = default;
            if (VNavmesh.Enabled)
            {
                VNavmesh.Path.Stop();
            }
        }

        private unsafe void SetRotation(Angle angle)
        {
            var playerObject = (GameObject*)Player.Object.Address;
            Svc.Log.Debug($"Setting rotation to {angle.Rad}");
            playerObject->SetRotation(angle.Rad);
        }

        private void Navigate(Vector3 destination, bool shouldFly, Angle angle = default, bool preferGround = false)
        {
            if (CurrentDestination == destination && (IsPathing || IsPathGenerating))
                return;

            shouldFly |= Dalamud.Conditions[ConditionFlag.Diving];

            StopNavigation();
            CurrentDestination = destination;
            CurrentRotation    = angle;
            var correctedDestination = GetCorrectedDestination(CurrentDestination, preferGround);
            GatherBuddy.Log.Debug($"Navigating to {destination} (corrected to {correctedDestination})");

            LastNavigationResult = VNavmesh.SimpleMove.PathfindAndMoveTo(correctedDestination, shouldFly);
        }

        private static Vector3 GetCorrectedDestination(Vector3 destination, bool preferGround = false)
        {
            const float MaxHorizontalSeparation = 3.0f;
            const float MaxVerticalSeparation = 2.5f;

            try
            {
                float separation;
                if (WorldData.NodeOffsets.TryGetValue(destination, out var offset))
                {
                    offset = VNavmesh.Query.Mesh.NearestPoint(offset, MaxHorizontalSeparation, MaxVerticalSeparation);
                    if ((separation = Vector2.Distance(offset.ToVector2(), destination.ToVector2())) > MaxHorizontalSeparation)
                        GatherBuddy.Log.Warning($"Offset is ignored because the horizontal separation {separation} is too large after correcting for mesh. Maximum allowed is {MaxHorizontalSeparation}.");
                    else if ((separation = Math.Abs(offset.Y - destination.Y)) > MaxVerticalSeparation)
                        GatherBuddy.Log.Warning($"Offset is ignored because the vertical separation {separation} is too large after correcting for mesh. Maximum allowed is {MaxVerticalSeparation}.");
                    else
                        return offset;
                }

                var correctedDestination = VNavmesh.Query.Mesh.NearestPoint(destination, MaxHorizontalSeparation, MaxVerticalSeparation);
                if ((separation = Vector2.Distance(correctedDestination.ToVector2(), destination.ToVector2())) > MaxHorizontalSeparation)
                    GatherBuddy.Log.Warning($"Query.Mesh.NearestPoint() returned a point with too large horizontal separation {separation}. Maximum allowed is {MaxHorizontalSeparation}.");
                else if ((separation = Math.Abs(correctedDestination.Y - destination.Y)) > MaxVerticalSeparation)
                    GatherBuddy.Log.Warning($"Query.Mesh.NearestPoint() returned a point with too large vertical separation {separation}. Maximum allowed is {MaxVerticalSeparation}.");
                else
                    return correctedDestination;
                
                if (preferGround)
                {
                    const float GroundSearchRadius = 15f;
                    const float MaxGroundHorizontalSeparation = 7.5f;
                    const float MaxGroundVerticalSeparation = 10f;
                    
                    try
                    {
                        var groundPoint = VNavmesh.Query.Mesh.PointOnFloor(destination, false, GroundSearchRadius);
                        var hDist = Vector2.Distance(groundPoint.ToVector2(), destination.ToVector2());
                        var vDist = Math.Abs(groundPoint.Y - destination.Y);
                        
                        if (hDist <= MaxGroundHorizontalSeparation && vDist <= MaxGroundVerticalSeparation)
                        {
                            GatherBuddy.Log.Debug($"Using ground point for fishing: horizontal {hDist:F2}y, vertical {vDist:F2}y from target");
                            return groundPoint;
                        }
                    }
                    catch (Exception e)
                    {
                        GatherBuddy.Log.Debug($"Failed to find ground point for fishing: {e.Message}");
                    }
                }
            }
            catch (Exception) { }

            return destination;
        }

        private void MoveToFarNode(Vector3 position)
        {
            var farNode = position;

            if (!Dalamud.Conditions[ConditionFlag.Mounted])
            {
                if (GatherBuddy.Config.AutoGatherConfig.MoveWhileMounting)
                    Navigate(farNode, false);
                EnqueueMountUp();
            }
            else
            {
                Navigate(farNode, ShouldFly(farNode));
            }
        }

        private void MoveToFishingSpot(Vector3 position, Angle angle)
        {
            if (!Dalamud.Conditions[ConditionFlag.Mounted])
            {
                if (GatherBuddy.Config.AutoGatherConfig.MoveWhileMounting)
                    Navigate(position, false, angle, preferGround: true);
                EnqueueMountUp();
            }
            else
            {
                Navigate(position, ShouldFly(position), angle, preferGround: true);
            }
        }

        public static Aetheryte? FindClosestAetheryte(ILocation location)
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

            return aetheryte;
        }

        private bool MoveToTerritory(ILocation location)
        {
            var aetheryte = FindClosestAetheryte(location);
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
