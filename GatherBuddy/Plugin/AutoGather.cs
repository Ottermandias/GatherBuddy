using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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

        public IEnumerable<GameObject> ValidGatherables = new List<GameObject>();

        public Gatherable? DesiredItem => _plugin.GatherWindowManager.ActiveItems.FirstOrDefault() as Gatherable;
        public bool IsPathing => GatherBuddy.Navmesh.IsPathing();
        public bool NavReady => GatherBuddy.Navmesh.IsReady();

        private void UpdateObjects()
        {
            ValidGatherables = Dalamud.ObjectTable.Where(g => g.ObjectKind == ObjectKind.GatheringPoint)
                        .Where(g => g.IsTargetable)
                        .Where(IsDesiredNode)
                        .OrderBy(g => Vector3.Distance(g.Position, Dalamud.ClientState.LocalPlayer.Position));

        }
        private int _frameCounter = 0;
        public void DoAutoGather()
        {
            if (!GatherBuddy.Config.AutoGather) return;
            NavmeshStuckCheck();
            InventoryCheck();

            UpdateObjects();
            DetermineAutoState();
        }
        private void PathfindToFarNode(Gatherable desiredItem)
        {
            if (desiredItem == null)
                return;
            var nodeList = desiredItem.NodeList;
            if (nodeList == null)
                return;
            var closestKnownNode = nodeList.SelectMany(n => n.WorldCoords).SelectMany(w => w.Value).OrderBy(n => Vector3.Distance(n, Dalamud.ClientState.LocalPlayer.Position)).FirstOrDefault();
            if (closestKnownNode == null)
                return;
            PathfindToNode(closestKnownNode);
        }
        private void PathfindToNode(Vector3 position)
        {
            GatherBuddy.Navmesh.PathfindAndMoveTo(position, true);
        }

        private void DetermineAutoState()
        {
            if (!NavReady)
            {
                AutoState = AutoStateType.WaitingForNavmesh;
                AutoStatus = "Waiting for Navmesh...";
                return;
            }

            if (IsPathing)
            {
                AutoState = AutoStateType.Pathing;
                AutoStatus = "Pathing to node...";
                return;
            }

            if (IsPlayerBusy())
            {
                AutoState = AutoStateType.Idle;
                AutoStatus = "Player is busy...";
                return;
            }

            if (DesiredItem == null)
            {
                AutoState = AutoStateType.Finish;
                AutoStatus = "No active items in shopping list...";
                return;
            }

            var currentTerritory = Dalamud.ClientState.TerritoryType;

            if (!ValidGatherables.Any())
            {
                var location = DesiredItem.Locations.FirstOrDefault();
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
                        _teleportInitiated = DateTime.Now.AddSeconds(15);
                        AutoState = AutoStateType.WaitingForTeleport;
                        AutoStatus = "Teleporting to " + location.Territory.Name + "...";
                        _plugin.Executor.GatherItem(DesiredItem);
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
                AutoStatus = "Pathing to node...";
                PathfindToFarNode(DesiredItem);
                return;
            }

            if (ValidGatherables.Any())
            {
                var targetGatherable = ValidGatherables.First();
                var distance = Vector3.Distance(targetGatherable.Position, Dalamud.ClientState.LocalPlayer.Position);

                if (distance < 5)
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
                        // This is where you can handle additional logic when close to the node without being mounted.
                        AutoState = AutoStateType.GatheringNode;
                        AutoStatus = $"Gathering {targetGatherable.Name}...";
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
                        AutoState = AutoStateType.MovingToNode;
                        AutoStatus = $"Moving to node {targetGatherable.Name} at {targetGatherable.Position}";
                        PathfindToNode(targetGatherable.Position);
                        return;
                    }
                }
            }

            AutoState = AutoStateType.Error;
            AutoStatus = "Nothing to do...";
        }

        private bool IsPlayerBusy()
        {
            var player = Dalamud.ClientState.LocalPlayer;
            if (player == null)
                return true;
            if (player.IsCasting)
                return true;
            if (player.IsDead)
                return true;

            return false;
        }

        private unsafe void InventoryCheck()
        {
            var presets = _plugin.GatherWindowManager.Presets;
            if (presets == null)
                return;

            var inventory = InventoryManager.Instance();
            if (inventory == null) return;

            foreach (var preset in presets)
            {
                var items = preset.Items;
                if (items == null)
                    continue;

                var indicesToRemove = new List<int>();

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    var itemCount = inventory->GetInventoryItemCount(item.ItemId);
                    if (itemCount >= item.Quantity)
                    {
                        _plugin.GatherWindowManager.RemoveItem(preset, i);
                    }
                }
            }
        }
        private DateTime _lastNavReset = DateTime.MinValue;
        private void NavmeshStuckCheck()
        {
            if (_lastNavReset.AddSeconds(10) < DateTime.Now)
            {
                _lastNavReset = DateTime.Now;
                GatherBuddy.Navmesh.Reload();
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
