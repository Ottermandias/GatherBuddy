using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
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
        public bool IsPathing => VNavmesh_IPCSubscriber.Path_IsRunning();
        public bool IsPathGenerating => VNavmesh_IPCSubscriber.Nav_PathfindInProgress();
        public bool NavReady => VNavmesh_IPCSubscriber.Nav_IsReady();
        public IEnumerable<GameObject> ValidNodesInRange => Dalamud.ObjectTable.Where(g => g.ObjectKind == ObjectKind.GatheringPoint)
                        .Where(g => g.IsTargetable)
                        .Where(IsDesiredNode)
                        .OrderBy(g => Vector3.Distance(g.Position, Dalamud.ClientState.LocalPlayer.Position));

        public GameObject NearestNode => ValidNodesInRange.FirstOrDefault();
        public float NearestNodeDistance => Vector3.Distance(Dalamud.ClientState.LocalPlayer.Position, NearestNode.Position);
        public bool IsGathering => Dalamud.Conditions[ConditionFlag.Gathering] || Dalamud.Conditions[ConditionFlag.Gathering42];

        public Dictionary<uint, List<Vector3>> DesiredNodesInZone
        {
            get
            {
                var nodes = new Dictionary<uint, List<Vector3>>();
                foreach (var item in ItemsToGatherInZone)
                {
                    foreach (var location in item.Locations)
                    {
                        if (location.Id != Dalamud.ClientState.TerritoryType)
                            continue;
                        var allNodesInZone = location.WorldPositions;
                        foreach (var node in allNodesInZone)
                        {
                            if (!nodes.ContainsKey(node.Key))
                                nodes[node.Key] = new List<Vector3>();
                            nodes[node.Key].AddRange(node.Value);
                        }
                    }
                }
                return nodes;
            }
        }

        private bool IsDesiredNode(GameObject @object)
        {
            if (@object == null)
                return false;
            var dataId = @object.DataId;
            foreach (var item in ItemsToGather)
            {
                if (item.Locations.Any(l => l.WorldPositions.ContainsKey(dataId)))
                    return true;
            }
            return false;
        }

        public IEnumerable<IGatherable> ItemsToGather => _plugin.GatherWindowManager.ActiveItems.Where(i => i.InventoryCount < i.Quantity);
        public IEnumerable<IGatherable> ItemsToGatherInZone => ItemsToGather.Where(i => i.Locations.Any(l => l.Territory.Id == Dalamud.ClientState.TerritoryType));
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
    }
}
