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
using ECommons.ExcelServices;
using ECommons.GameHelpers;
using GatherBuddy.CustomInfo;
using GatherBuddy.Enums;

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
                        .Where(g => !IsBlacklisted(g.Position))
                        .OrderBy(g => Vector3.Distance(g.Position, Dalamud.ClientState.LocalPlayer.Position));

        private bool IsBlacklisted(Vector3 g)
        {
            var blacklisted = GatherBuddy.Config.AutoGatherConfig.BlacklistedNodesByTerritoryId.ContainsKey(Dalamud.ClientState.TerritoryType) &&
                GatherBuddy.Config.AutoGatherConfig.BlacklistedNodesByTerritoryId[Dalamud.ClientState.TerritoryType].Contains(g);
            return blacklisted;
        }

        public GameObject? NearestNode => ValidNodesInRange.FirstOrDefault();
        public float NearestNodeDistance => Vector3.Distance(Dalamud.ClientState.LocalPlayer.Position, NearestNode?.Position ?? Vector3.Zero);
        public bool IsGathering => Dalamud.Conditions[ConditionFlag.Gathering] || Dalamud.Conditions[ConditionFlag.Gathering42];
        public bool? LastNavigationResult { get; set; } = null;
        public Vector3? CurrentDestination { get; set; } = null;
        public bool HasSeenFlag { get; set; } = false;

        public GatheringType JobAsGatheringType
        {
            get
            {
                var job = Player.Job;
                switch (job)
                {
                    case Job.MIN: return GatheringType.Miner;
                    case Job.BTN: return GatheringType.Botanist;
                    case Job.FSH: return GatheringType.Fisher;
                    default:      return GatheringType.Unknown;
                }
            }
        }

        public bool ShouldUseFlag
        {
            get
            {
                if (GatherBuddy.Config.AutoGatherConfig.DisableFlagPathing)
                    return false;
                if (HasSeenFlag)
                    return false;

                return true;
            }
        }

        public unsafe Vector3? MapFlagPosition
        {
            get
            {
                var map = FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentMap.Instance();
                if (map == null || map->IsFlagMarkerSet == 0)
                    return null;
                if (map->CurrentTerritoryId != Dalamud.ClientState.TerritoryType)
                    return null;
                var marker             = map->FlagMapMarker;
                var mapPosition        = new Vector2(marker.XFloat, marker.YFloat);
                var uncorrectedVector3 = new Vector3(mapPosition.X, 1024, mapPosition.Y);
                var correctedVector3   = uncorrectedVector3.CorrectForMesh();
                if (uncorrectedVector3 == correctedVector3)
                    return null;
                else
                {
                    return correctedVector3;
                }
            }
        }

        public bool ShouldFly
        {
            get
            {
                if (GatherBuddy.Config.AutoGatherConfig.ForceWalking)
                {
                    return false;
                }

                return Vector3.Distance(Dalamud.ClientState.LocalPlayer.Position, CurrentDestination ?? Vector3.Zero) >= GatherBuddy.Config.AutoGatherConfig.MountUpDistance;
            }
        }
        public string AutoStatus { get; set; } = "Idle";
        public int    LastCollectability = 0;
        public Dictionary<uint, List<Vector3>> DesiredNodesInZone
        {
            get
            {
                var nodes = new Dictionary<uint, List<Vector3>>();
                foreach (var item in ItemsToGatherInZone)
                {
                    foreach (var location in item.Locations)
                    {
                        if (location.Territory.Id != Dalamud.ClientState.TerritoryType)
                            continue;
                        var allNodesInZone = location.WorldPositions;
                        foreach (var node in allNodesInZone)
                        {
                            if (!nodes.ContainsKey(node.Key))
                                nodes[node.Key] = new List<Vector3>();
                            foreach (var pos in node.Value)
                            {
                                if (IsBlacklisted(pos)) continue;
                                nodes[node.Key].Add(pos);
                            }
                        }
                    }
                }
                return nodes;
            }
        }

        public List<Vector3> DesiredNodeCoordsInZone => DesiredNodesInZone.SelectMany(n => n.Value).ToList();

        public bool IsDesiredNode(GameObject @object)
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
