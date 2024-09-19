using Dalamud.Game.ClientState.Conditions;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ECommons.ExcelServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.UI;
using GatherBuddy.Classes;
using GatherBuddy.CustomInfo;
using GatherBuddy.Enums;
using GatherBuddy.Time;
using FFXIVClientStructs.FFXIV.Client.Game;
using System.Collections.Specialized;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        public bool IsPathing
            => VNavmesh_IPCSubscriber.Path_IsRunning();

        public bool IsPathGenerating
            => VNavmesh_IPCSubscriber.Nav_PathfindInProgress();

        public bool NavReady
            => VNavmesh_IPCSubscriber.Nav_IsReady();

        private bool IsBlacklisted(Vector3 g)
        {
            var blacklisted = GatherBuddy.Config.AutoGatherConfig.BlacklistedNodesByTerritoryId.ContainsKey(Dalamud.ClientState.TerritoryType)
             && GatherBuddy.Config.AutoGatherConfig.BlacklistedNodesByTerritoryId[Dalamud.ClientState.TerritoryType].Contains(g);
            return blacklisted;
        }

        public bool IsGathering
            => Dalamud.Conditions[ConditionFlag.Gathering] || Dalamud.Conditions[ConditionFlag.Gathering42];

        public bool?    LastNavigationResult { get; set; } = null;
        public Vector3 CurrentDestination   { get; private set; } = default;

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
            => !GatherBuddy.Config.AutoGatherConfig.DisableFlagPathing;

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
                var correctedVector3   = uncorrectedVector3.CorrectForMesh(0.5f);
                if (uncorrectedVector3 == correctedVector3)
                    return null;

                if (!correctedVector3.SanityCheck())
                    return null;

                return correctedVector3;
            }
        }

        public bool ShouldFly(Vector3 destination)
        {
            if (GatherBuddy.Config.AutoGatherConfig.ForceWalking || Dalamud.ClientState.LocalPlayer == null)
            {
                return false;
            }

            return Vector3.Distance(Dalamud.ClientState.LocalPlayer.Position, destination)
                >= GatherBuddy.Config.AutoGatherConfig.MountUpDistance;
        }

        public unsafe Vector2? TimedNodePosition
        {
            get
            {
                var      map     = FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentMap.Instance();
                var      markers = map->MiniMapGatheringMarkers;
                if (markers == null)
                    return null;
                Vector2? result  = null;
                foreach (var miniMapGatheringMarker in markers)
                {
                    if (miniMapGatheringMarker.MapMarker.X != 0 && miniMapGatheringMarker.MapMarker.Y != 0)
                    {
                        // ReSharper disable twice PossibleLossOfFraction
                        result = new Vector2(miniMapGatheringMarker.MapMarker.X / 16, miniMapGatheringMarker.MapMarker.Y / 16);
                        break;
                    }
                    // GatherBuddy.Log.Information(miniMapGatheringMarker.MapMarker.IconId +  " => X: " + miniMapGatheringMarker.MapMarker.X / 16 + " Y: " + miniMapGatheringMarker.MapMarker.Y / 16);
                }

                return result;
            }
        }

        public string AutoStatus { get; set; } = "Idle";
        public int LastCollectability = 0;
        public int LastIntegrity = 0;
        public BitVector32 LuckUsed;

        public readonly List<GatherInfo> ItemsToGather = [];
        public readonly Dictionary<ILocation, TimeInterval> VisitedTimedLocations = [];
        public readonly HashSet<Vector3> FarNodesSeenSoFar = [];
        public readonly LinkedList<Vector3> VisitedNodes = [];
        private GatherInfo? targetInfo = null;

        public void UpdateItemsToGather()
        {
            ItemsToGather.Clear();
            var activeItems = OrderActiveItems(_plugin.GatherWindowManager.ActiveItems.OfType<Gatherable>().Select(GetBestLocation));
            var RegularItemsToGather = new List<GatherInfo>();
            foreach (var (item, location, time) in activeItems)
            {
                if (InventoryCount(item) >= QuantityTotal(item) || item.IsTreasureMap && InventoryCount(item) > 0)
                    continue;

                if (item.IsTreasureMap && NextTresureMapAllowance >= AdjuctedServerTime.DateTime)
                    continue;

                if (GatherBuddy.UptimeManager.TimedGatherables.Contains(item))
                {
                    if (time.InRange(AdjuctedServerTime))
                        ItemsToGather.Add((item, location, time));
                }
                else
                {
                    RegularItemsToGather.Add((item, location, time));
                }
            }
            ItemsToGather.Sort((x, y) => GetNodeTypeAsPriority(x.Item).CompareTo(GetNodeTypeAsPriority(y.Item)));
            ItemsToGather.AddRange(RegularItemsToGather);
        }

        private GatherInfo GetBestLocation(Gatherable item)
        {
            (ILocation? Location, TimeInterval Time) res = default;
            //First priority: selected preferred location.
            var node = _plugin.GatherWindowManager.GetPreferredLocation(item);
            if (node != null && !VisitedTimedLocations.ContainsKey(node))
            {
                res = (node, node.Times.NextUptime(AdjuctedServerTime));
            }
            //Second priority: location for preferred job.
            else if (GatherBuddy.Config.PreferredGatheringType is GatheringType.Miner or GatheringType.Botanist)
            {
                res = GatherBuddy.UptimeManager.NextUptime(item, GatherBuddy.Config.PreferredGatheringType, AdjuctedServerTime, [.. VisitedTimedLocations.Keys]);
            }
            //Otherwise: location for any job.
            if (res.Location == null)
            {
                res = GatherBuddy.UptimeManager.NextUptime(item, AdjuctedServerTime, [.. VisitedTimedLocations.Keys]);
            }
            return (item, res.Location, res.Time);
        }

        private IEnumerable<GatherInfo> OrderActiveItems(IEnumerable<GatherInfo> activeItems)
        {
            switch (GatherBuddy.Config.AutoGatherConfig.SortingMethod)
            {
                case AutoGatherConfig.SortingType.Location: return activeItems.OrderBy(i => i.Location?.Territory.Id);
                case AutoGatherConfig.SortingType.None:
                default:
                    return activeItems;
            }
        }

        public unsafe AddonGathering* GatheringAddon
            => (AddonGathering*)Dalamud.GameGui.GetAddonByName("Gathering", 1);

        public unsafe AddonGatheringMasterpiece* MasterpieceAddon
            => (AddonGatheringMasterpiece*)Dalamud.GameGui.GetAddonByName("GatheringMasterpiece", 1);

        public unsafe AddonMaterializeDialog* MaterializeAddon
            => (AddonMaterializeDialog*)Dalamud.GameGui.GetAddonByName("Materialize", 1);

        public IEnumerable<IGatherable> ItemsToGatherInZone
            => ItemsToGather.Where(i => i.Location?.Territory.Id == Dalamud.ClientState.TerritoryType).Select(i => i.Item);

        private bool LocationMatchesJob(ILocation loc)
            => loc.GatheringType.ToGroup() == JobAsGatheringType;

        public bool CanAct
        {
            get
            {
                if (Dalamud.ClientState.LocalPlayer == null)
                    return false;
                if (Dalamud.Conditions[ConditionFlag.BetweenAreas]
                 || Dalamud.Conditions[ConditionFlag.BetweenAreas51]
                 || Dalamud.Conditions[ConditionFlag.BeingMoved]
                 || Dalamud.Conditions[ConditionFlag.Casting]
                 || Dalamud.Conditions[ConditionFlag.Casting87]
                 || Dalamud.Conditions[ConditionFlag.Jumping]
                 || Dalamud.Conditions[ConditionFlag.Jumping61]
                 || Dalamud.Conditions[ConditionFlag.LoggingOut]
                 || Dalamud.Conditions[ConditionFlag.Occupied]
                 || Dalamud.Conditions[ConditionFlag.Occupied39]
                 || Dalamud.Conditions[ConditionFlag.Unconscious]
                 || Dalamud.Conditions[ConditionFlag.Gathering42]
                 || Dalamud.ClientState.LocalPlayer.CurrentHp < 1
                 || Player.IsAnimationLocked)
                    return false;

                return true;
            }
        }

        private int InventoryCount(IGatherable gatherable)
            => _plugin.GatherWindowManager.GetInventoryCountForItem(gatherable);

        private uint QuantityTotal(IGatherable gatherable)
            => _plugin.GatherWindowManager.GetTotalQuantitiesForItem(gatherable);

        private int GetNodeTypeAsPriority(Gatherable item)
        {
            if (GatherBuddy.Config.AutoGatherConfig.SortingMethod == AutoGatherConfig.SortingType.None)
                return 0;
            Gatherable gatherable = item;
            switch (gatherable?.NodeType)
            {
                case NodeType.Legendary: return 0;
                case NodeType.Unspoiled: return 1;
                case NodeType.Ephemeral: return 2;
                case NodeType.Regular:   return 9;
                case NodeType.Unknown:   return 99;
            }

            return 99;
        }

        private static unsafe bool HasGivingLandBuff
            => Dalamud.ClientState.LocalPlayer?.StatusList.Any(s => s.StatusId == 1802) ?? false;

        private static unsafe bool IsGivingLandOffCooldown
            => ActionManager.Instance()->IsActionOffCooldown(ActionType.Action, Actions.GivingLand.ActionID);

        //Should be near the upper bound to reduce the probability of overcapping.
        private const int GivingLandYeild = 30;

        private static unsafe DateTime NextTresureMapAllowance
            => FFXIVClientStructs.FFXIV.Client.Game.UI.UIState.Instance()->GetNextMapAllowanceDateTime();

        private static unsafe uint FreeInventorySlots
            => InventoryManager.Instance()->GetEmptySlotsInBag();

        private static TimeStamp AdjuctedServerTime
            => GatherBuddy.Time.ServerTime.AddSeconds(GatherBuddy.Config.AutoGatherConfig.TimedNodePrecog);
    }

    public record class GatherInfo(Gatherable Item, ILocation? Location, TimeInterval Time)
    {
        public static implicit operator (Gatherable Item, ILocation? Location, TimeInterval Time)(GatherInfo value)
        {
            return (value.Item, value.Location, value.Time);
        }

        public static implicit operator GatherInfo((Gatherable Item, ILocation? Location, TimeInterval Time) value)
        {
            return new GatherInfo(value.Item, value.Location, value.Time);
        }
    }
}
