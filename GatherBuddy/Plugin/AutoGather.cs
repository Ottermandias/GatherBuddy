using ClickLib.Structures;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface;
using ECommons.Automation.NeoTaskManager;
using ECommons.DalamudServices;
using ECommons.ExcelServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.Classes;
using GatherBuddy.CustomInfo;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Utility;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets2;
using OtterGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentMJIFarmManagement;

namespace GatherBuddy.Plugin
{
    public class AutoGather
    {
        public enum AutoStateType
        {
            Idle,
            WaitingForTeleport,
            Pathing,
            WaitingForNavmesh,
            GatheringNode,
            MovingToNode,
            Mounting,
            Dismounting,
            Error,
            Finish,
        }
        private readonly GatherBuddy _plugin;
        public string AutoStatus { get; set; } = "Not Running";
        public AutoStateType AutoState { get; set; } = AutoStateType.Idle;
        private AutoStateType _lastAutoState = AutoStateType.Idle;
        public AutoGather(GatherBuddy plugin)
        {
            _plugin = plugin;
        }

        private DateTime _teleportInitiated = DateTime.MinValue;

        public bool ShouldAutoGather { get; set; } = false;

        public IEnumerable<GameObject> ValidGatherables => Dalamud.ObjectTable.Where(g => g.ObjectKind == ObjectKind.GatheringPoint)
                        .Where(g => g.IsTargetable)
                        .Where(IsDesiredNode)
                        .OrderBy(g => Vector3.Distance(g.Position, Dalamud.ClientState.LocalPlayer.Position));

        public Gatherable? DesiredItem => _plugin.GatherWindowManager.ActiveItems.Where(g => InventoryLessThanQuantity(g)).FirstOrDefault() as Gatherable;

        private unsafe InventoryManager* Inventory => InventoryManager.Instance();
        private unsafe bool InventoryLessThanQuantity(IGatherable g)
        {
            var invCount = Inventory->GetInventoryItemCount(g.ItemId);
            return invCount < g.Quantity;
        }

        public bool IsPathing => VNavmesh_IPCSubscriber.Path_IsRunning();
        public bool IsPathGenerating => VNavmesh_IPCSubscriber.Nav_PathfindInProgress();
        public bool NavReady => VNavmesh_IPCSubscriber.Nav_IsReady();

        public bool CanAct
        {
            get
            {
                if (Dalamud.ClientState.LocalPlayer == null)
                    return false;
                if (Dalamud.Conditions[ConditionFlag.BetweenAreas] ||
                    Dalamud.Conditions[ConditionFlag.BetweenAreas51] ||
                    Dalamud.Conditions[ConditionFlag.BeingMoved] ||
                    Dalamud.Conditions[ConditionFlag.Casting] ||
                    Dalamud.Conditions[ConditionFlag.Casting87] ||
                    Dalamud.Conditions[ConditionFlag.Jumping] ||
                    Dalamud.Conditions[ConditionFlag.Jumping61] ||
                    Dalamud.Conditions[ConditionFlag.LoggingOut] ||
                    Dalamud.Conditions[ConditionFlag.Occupied] ||
                    Dalamud.Conditions[ConditionFlag.Unconscious] ||
                    Dalamud.ClientState.LocalPlayer.CurrentHp < 1)
                    return false;
                return true;
            }
        }

        private bool IsBlacklisted(Vector3 position)
        {
            if (position == null)
                return true;
            if (!GatherBuddy.Config.BlacklistedAutoGatherNodesByTerritoryId.TryGetValue(Dalamud.ClientState.TerritoryType, out var list))
            {
                list = new List<Vector3>();
                GatherBuddy.Config.BlacklistedAutoGatherNodesByTerritoryId[Dalamud.ClientState.TerritoryType] = list;
            }
            return list.Contains(position);
        }

        public void DoAutoGather()
        {
            if (!ShouldAutoGather) return;
            if (!CanAct) return;

            DetermineAutoState();
        }
        private unsafe static Vector2? GetFlagPosition()
        {
            var map = FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentMap.Instance();
            if (map == null || map->IsFlagMarkerSet == 0)
                return null;
            var marker = map->FlagMapMarker;
            return new(marker.XFloat, marker.YFloat);
        }
        private unsafe void PathfindToFlag()
        {
            var flagPosition = GetFlagPosition();
            if (flagPosition == null)
                return;
            var vector3 = new Vector3(flagPosition.Value.X, 1024, flagPosition.Value.Y);
            var nearestPoint = VNavmesh_IPCSubscriber.Query_Mesh_NearestPoint(vector3, 0, 0);
            if (nearestPoint == null)
                return;
            if (Vector3.Distance(nearestPoint, Dalamud.ClientState.LocalPlayer.Position) < 100)
            {
                AutoStatus = "We're close to the area but no nodes are available.";
                return;
            }
            else
            {
                AutoStatus = "Pathing to flag...";
                PathfindToNode(nearestPoint, true);
            }
        }
        private int _currentNodeIndex = 0;
        public List<Vector3> RecentlyVistedNodes = new List<Vector3>();
        private void PathfindToFarNode(Gatherable desiredItem)
        {
            if (desiredItem == null)
                return;

            var nodeList = desiredItem.NodeList;
            if (nodeList == null)
                return;

            var currentPosition = Dalamud.ClientState.LocalPlayer.Position;
            var coordList = nodeList.Where(n => n.Territory.Id == Dalamud.ClientState.TerritoryType)
                                    .SelectMany(n => n.WorldCoords)
                                    .SelectMany(w => w.Value)
                                    .OrderBy(n => Vector3.Distance(n, currentPosition))
                                    .ToList();

            var closestKnownNode = coordList.FirstOrDefault();
            if (closestKnownNode == null)
                return;

            // If the closest node is too close, filter out close nodes and select a random node from the rest
            if (Vector3.Distance(closestKnownNode, currentPosition) < 30)
            {
                var farNodes = coordList.Where(n => Vector3.Distance(n, currentPosition) >= 150)
                                        .Where(n => !IsBlacklisted(n))
                                        .Where(n => !RecentlyVistedNodes.Contains(n))
                                        .ToList();

                if (farNodes.Any())
                {
                    _currentNodeIndex = (_currentNodeIndex + 1) % farNodes.Count;
                    closestKnownNode = farNodes[_currentNodeIndex];

                    AutoStatus = "Pathing to a farther node...";
                    PathfindToNode(closestKnownNode, true);
                }
                else
                {
                    VNavmesh_IPCSubscriber.Path_Stop();
                    AutoStatus = "No suitable nodes found to path.";
                    // You can add additional logic here if needed when no suitable nodes are found
                    return;
                }
            }
            else
            {
                AutoStatus = "Pathing to the closest known node...";
                PathfindToNode(closestKnownNode, true);
            }
        }
        public bool LastPathfindResult = true;
        private void PathfindToNode(Vector3 position, bool correct)
        {
            if (IsPathing || IsPathGenerating)
                return;
            if (!RecentlyVistedNodes.Contains(position))
                RecentlyVistedNodes.Add(position);
            LastPathfindResult = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(correct ? position.CorrectForMesh() : position, true);
        }

        private unsafe void DetermineAutoState()
        {
            try
            {
                if (!NavReady)
                {
                    AutoState = AutoStateType.WaitingForNavmesh;
                    AutoStatus = "Waiting for Navmesh...";
                    return;
                }
            }
            catch (Exception e)
            {
                GatherBuddy.Log.Error(e.Message);
                AutoState = AutoStateType.WaitingForNavmesh;
                AutoStatus = "vnavmesh communication failed. Do you have it installed??";
                return;
            }

            if (!CanAct)
            {
                AutoState = AutoStateType.Idle;
                AutoStatus = "Player is busy...";
                return;
            }

            if (Dalamud.Conditions[ConditionFlag.Gathering])
            {
                // This is where you can handle additional logic when close to the node without being mounted.
                AutoState = AutoStateType.GatheringNode;
                AutoStatus = $"Gathering {DesiredItem?.Name.ToString() ?? "Goose Egg"} from {Svc.Targets.Target?.Name ?? "Unknown Node"}...";
                GatherNode();
                return;
            }
            if (DesiredItem == null)
            {
                AutoState = AutoStateType.Finish;
                AutoStatus = "No valid items in shopping list...";
                return;
            }
            var location = _plugin.Executor.FindClosestLocation(DesiredItem);
            var neededJob = GetECommonsJobFromDesiredItem();
            if (neededJob != Player.Job && (location?.Territory.Id ?? 0) == Dalamud.ClientState.TerritoryType)
            {
                AutoState = AutoStateType.Error;
                AutoStatus = $"Switching to {neededJob} for {DesiredItem.Name[GatherBuddy.Language]}...";
                return;
            }

            NavmeshStuckCheck();
            var currentTerritory = Dalamud.ClientState.TerritoryType;
            var nodeCount = GatherBuddy.GameData.GatheringNodes.Where(g => g.Value.Territory.Id == currentTerritory)
                                                                .Where(NodeMatchesCurrentJob)
                                                                .Where(NodeMatchesDesiredItem).Count();
            var blacklistedNodes = GatherBuddy.Config.BlacklistedAutoGatherNodesByTerritoryId.TryGetValue(currentTerritory, out var list) ? list : new List<Vector3>();
            if (RecentlyVistedNodes.Count >= nodeCount - blacklistedNodes.Count())
                RecentlyVistedNodes.Clear();

            if (!ValidGatherables.Any())
            {
                if (location == null)
                {
                    AutoState = AutoStateType.Error;
                    AutoStatus = "No locations for item " + DesiredItem.Name[GatherBuddy.Language] + ".";
                    return;
                }

                if (location.Territory.Id != currentTerritory)
                {
                    if (_teleportInitiated < DateTime.Now)
                    {
                        AutoState = AutoStateType.WaitingForTeleport;
                        AutoStatus = "Teleporting to " + location.Territory.Name + "...";
                        if (IsPathing)
                            VNavmesh_IPCSubscriber.Path_Stop();
                        else
                        {
                            _teleportInitiated = DateTime.Now.AddSeconds(15);
                            _plugin.Executor.GatherItem(DesiredItem);
                        }
                        return;
                    }
                    else
                    {
                        AutoState = AutoStateType.WaitingForTeleport;
                        AutoStatus = "Waiting for teleport...";
                        return;
                    }
                }

                if (!Dalamud.Conditions[ConditionFlag.Mounted])
                {
                    AutoState = AutoStateType.Mounting;
                    AutoStatus = "Mounting for travel...";
                    MountUp();
                    return;
                }

                AutoState = AutoStateType.Pathing;
                PathfindToFarNode(DesiredItem);
                return;
            }

            if (ValidGatherables.Any())
            {
                var targetGatherable = ValidGatherables.Where(g => !IsBlacklisted(g.Position)).FirstOrDefault();
                if (targetGatherable == null)
                {
                    AutoState = AutoStateType.Error;
                    AutoStatus = "No valid nodes found...";
                    return;
                }
                var distance = Vector3.Distance(targetGatherable.Position, Dalamud.ClientState.LocalPlayer.Position);

                if (distance < 2.5)
                {
                    if (Dalamud.Conditions[ConditionFlag.Mounted])
                    {
                        AutoState = AutoStateType.Dismounting;
                        AutoStatus = "Dismounting...";
                        Dismount();
                        return;
                    }
                    else
                    {
                        AutoState = AutoStateType.GatheringNode;
                        AutoStatus = $"Targeting {targetGatherable.Name}...";
                        InteractNode(targetGatherable);
                        return;
                    }
                }
                else
                {
                    if (!Dalamud.Conditions[ConditionFlag.Mounted])
                    {
                        AutoState = AutoStateType.Mounting;
                        AutoStatus = "Mounting for travel...";
                        MountUp();
                        return;
                    }

                    if (AutoState != AutoStateType.MovingToNode)
                    {
                        _hiddenRevealed = false;
                        AutoState = AutoStateType.MovingToNode;
                        AutoStatus = $"Moving to node {targetGatherable.Name} at {targetGatherable.Position}";
                        PathfindToNode(targetGatherable.Position, true);
                        return;
                    }

                    if (AutoState == AutoStateType.MovingToNode && Vector3.Distance(targetGatherable.Position.CorrectForMesh() , Dalamud.ClientState.LocalPlayer.Position) < 1)
                    {
                        AutoState = AutoStateType.MovingToNode;
                        AutoStatus = $"Moving to node {targetGatherable.Name} at {targetGatherable.Position}";
                        PathfindToNode(targetGatherable.Position, false);
                        return;
                    }
                }
            }

            AutoState = AutoStateType.Error;
            //AutoStatus = "Nothing to do...";
        }

        private bool NodeMatchesDesiredItem(KeyValuePair<uint, GatheringNode> pair)
        {
            var desiredItem = DesiredItem;
            if (desiredItem == null)
                return false;
            return desiredItem.NodeList.Any(n => n.Id == pair.Value.Id);
        }

        private bool NodeMatchesCurrentJob(KeyValuePair<uint, GatheringNode> g)
        {
            var job = GetECommonsJobFromDesiredItem();
            if (job == Job.ADV)
                return false;
            if (job == Job.BTN && g.Value.IsBotanist)
                return true;
            if (job == Job.MIN && g.Value.IsMiner)
                return true;
            return false;
        }

        private Job GetECommonsJobFromDesiredItem()
        {
            if (DesiredItem == null)
                return Job.ADV;
            switch (_plugin.Executor.FindClosestLocation(DesiredItem)?.GatheringType.ToGroup() ?? Enums.GatheringType.Unknown)
            {
                case Enums.GatheringType.Botanist:
                    return Job.BTN;
                case Enums.GatheringType.Miner:
                    return Job.MIN;
                case Enums.GatheringType.Fisher:
                    return Job.FSH;
                default: return Job.ADV;
            }
        }

        private unsafe void GatherNode()
        {
            var gatheringWindow = (AddonGathering*)Dalamud.GameGui.GetAddonByName("Gathering", 1);
            if (gatheringWindow == null) return;

            var ids = new List<uint>()
                    {
                    gatheringWindow->GatheredItemId1,
                    gatheringWindow->GatheredItemId2,
                    gatheringWindow->GatheredItemId3,
                    gatheringWindow->GatheredItemId4,
                    gatheringWindow->GatheredItemId5,
                    gatheringWindow->GatheredItemId6,
                    gatheringWindow->GatheredItemId7,
                    gatheringWindow->GatheredItemId8
                    };

            UseActions(ids);

            var itemIndex = ids.IndexOf(DesiredItem?.ItemId ?? 0);
            if (itemIndex < 0) itemIndex = ids.IndexOf(ids.FirstOrDefault(i => i > 0));

            var receiveEventAddress = new nint(gatheringWindow->AtkUnitBase.AtkEventListener.vfunc[2]);
            var eventDelegate = Marshal.GetDelegateForFunctionPointer<ReceiveEventDelegate>(receiveEventAddress);

            var target = AtkStage.GetSingleton();
            var eventData = EventData.ForNormalTarget(target, &gatheringWindow->AtkUnitBase);
            var inputData = InputData.Empty();

            eventDelegate.Invoke(&gatheringWindow->AtkUnitBase.AtkEventListener, ClickLib.Enums.EventType.CHANGE, (uint)itemIndex, eventData.Data, inputData.Data);
        }

        private bool IsRare(uint i)
        {
            var item = GatherBuddy.GameData.Gatherables[i];
            return item?.Stars > 1;
        }

        private void UseActions(List<uint> itemIds)
        {
            UseLuck(itemIds);
            Use100GPAction(itemIds);
        }

        private bool _hiddenRevealed = false;
        private unsafe void UseLuck(List<uint> itemIds)
        {
            if (!DesiredItem?.GatheringData.IsHidden ?? false)
                return;
            if (itemIds.Count > 0 && itemIds.Any(i => i == DesiredItem?.ItemId))
            {
                return;
            }
            //if (Dalamud.ClientState.LocalPlayer.CurrentGp < 500) return;
            if (_hiddenRevealed) return;
            var actionManager = ActionManager.Instance();
            switch (Svc.ClientState.LocalPlayer.ClassJob.Id)
            {
                case 17: //BTN
                    if (actionManager->GetActionStatus(ActionType.Action, 4095) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 4095);
                        _hiddenRevealed = true;
                    }
                    break;
                case 16: //MIN
                    if (actionManager->GetActionStatus(ActionType.Action, 4081) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 4081);
                        _hiddenRevealed = true;
                    }
                    break;
            }
        }


        private unsafe void Use100GPAction(List<uint> itemIds)
        {
            if (itemIds.Count > 0 && !itemIds.Any(i => i == DesiredItem?.ItemId))
            {
                return;
            }
            if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 1286 || s.StatusId == 756))
                return;
            if ((Dalamud.ClientState.LocalPlayer?.CurrentGp ?? 0) < 100)
                return;

            var actionManager = ActionManager.Instance();
            switch (Svc.ClientState.LocalPlayer.ClassJob.Id)
            {
                case 17:
                    if (actionManager->GetActionStatus(ActionType.Action, 273) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 273);
                    }
                    else if (actionManager->GetActionStatus(ActionType.Action, 4087) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 4087);
                    }
                    break;
                case 16:
                    if (actionManager->GetActionStatus(ActionType.Action, 272) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 272);
                    }
                    else if (actionManager->GetActionStatus(ActionType.Action, 4073) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 4073);
                    }
                    break;
            }
        }

        private unsafe delegate nint ReceiveEventDelegate(AtkEventListener* eventListener, ClickLib.Enums.EventType eventType, uint eventParam, void* eventData, void* inputData);

        private static unsafe ReceiveEventDelegate GetReceiveEvent(AtkEventListener* listener)
        {
            var receiveEventAddress = new IntPtr(listener->vfunc[2]);
            return Marshal.GetDelegateForFunctionPointer<ReceiveEventDelegate>(receiveEventAddress)!;
        }

        private bool _isInteracting = false;
        private TaskManager _taskManager = new TaskManager();
        private unsafe void InteractNode(GameObject targetGatherable)
        {
            if (!CanAct) return;
            var targetSystem = TargetSystem.Instance();
            if (targetSystem == null)
                return;
            if (_isInteracting) return;
            _isInteracting = true;
            _taskManager.EnqueueDelay(1000);
            _taskManager.Enqueue(() =>
            {
                targetSystem->OpenObjectInteraction((FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)targetGatherable.Address);
                _isInteracting = false;
            });
        }

        private Vector3 _lastKnownPosition = Vector3.Zero;
        private Vector3 _lastKnownPositionSuperStuck = Vector3.Zero;
        private DateTime _lastPositionCheckTime = DateTime.Now;
        private DateTime _lastSuperStuckPositionCheckTime = DateTime.Now;
        private TimeSpan _stuckDurationThreshold = TimeSpan.FromSeconds(5);
        private TimeSpan _superStuckDurationThreshold = TimeSpan.FromSeconds(60);

        private void NavmeshStuckCheck()
        {
            if (Dalamud.Conditions[ConditionFlag.Gathering] || Dalamud.Conditions[ConditionFlag.Gathering42]) return;
            if (VNavmesh_IPCSubscriber.Nav_BuildProgress() > 0) return;
            var currentPosition = Dalamud.ClientState.LocalPlayer.Position;
            var currentTime = DateTime.Now;

            // Check if enough time has passed since the last position check
            if (currentTime - _lastPositionCheckTime >= _stuckDurationThreshold)
            {
                var distance = Vector3.Distance(currentPosition, _lastKnownPosition);

                // If the player has not moved a significant distance, consider them stuck
                if (IsPathing && distance < 3)
                {
                    GatherBuddy.Log.Warning("Navmesh is stuck, reloading...");
                    VNavmesh_IPCSubscriber.Path_Stop();
                    VNavmesh_IPCSubscriber.Nav_Reload();
                }

                // Update the last known position and time for the next check
                _lastKnownPosition = currentPosition;
                _lastPositionCheckTime = currentTime;
            }
            if (currentTime - _lastSuperStuckPositionCheckTime >= _superStuckDurationThreshold)
            {
                var distance = Vector3.Distance(currentPosition, _lastKnownPositionSuperStuck);

                // If the player has not moved a significant distance, consider them stuck
                if (distance < 3)
                {
                    GatherBuddy.Log.Warning("Navmesh is super stuck, hard reloading...");
                    VNavmesh_IPCSubscriber.Path_Stop();
                    VNavmesh_IPCSubscriber.Nav_Reload();
                    RecentlyVistedNodes.Clear();
                }

                _lastKnownPositionSuperStuck = currentPosition;
                _lastSuperStuckPositionCheckTime = currentTime;
            }
        }


        private bool IsDesiredNode(GameObject gameObject)
        {
            return DesiredItem?.NodeList.Any(n => n.WorldCoords.Keys.Any(k => k == gameObject.DataId)) ?? false;
        }


        private unsafe void Dismount()
        {
            var am = ActionManager.Instance();
            am->UseAction(ActionType.Mount, 0);
        }

        private unsafe void MountUp()
        {
            var am = ActionManager.Instance();
            var mount = GatherBuddy.Config.AutoGatherMountId;
            if (am->GetActionStatus(ActionType.Mount, mount) != 0) return;
            am->UseAction(ActionType.Mount, mount);
        }

    }
}
