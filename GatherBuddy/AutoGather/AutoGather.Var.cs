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
using GatherBuddy.Enums;
using GatherBuddy.Time;
using FFXIVClientStructs.FFXIV.Client.Game;
using System.Collections.Specialized;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using NodeType = GatherBuddy.Enums.NodeType;

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
        private ILocation? CurrentFarNodeLocation;

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

        public bool ShouldFly(Vector3 destination)
        {
            if (Dalamud.Conditions[ConditionFlag.InFlight] || Dalamud.Conditions[ConditionFlag.Diving])
                return true;

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
        private BitVector32 LuckUsed;
        private bool WentHome;

        public readonly List<GatherInfo> ItemsToGather = [];
        public readonly Dictionary<ILocation, TimeInterval> VisitedTimedLocations = [];
        public readonly HashSet<Vector3> FarNodesSeenSoFar = [];
        public readonly LinkedList<uint> VisitedNodes = [];
        private GatherInfo? targetInfo = null;

        private IEnumerator<Actions.BaseAction?>? ActionSequence;

        public void UpdateItemsToGather()
        {
            ItemsToGather.Clear();
            var activeItems = OrderActiveItems(_plugin.AutoGatherListsManager.ActiveItems.OfType<Gatherable>().Select(GetBestLocation));
            var RegularItemsToGather = new List<GatherInfo>();
            foreach (var (item, location, time) in activeItems)
            {
                uint quest = item.GatheringType switch
                {
                    GatheringType.Miner    => 65728,
                    GatheringType.Botanist => 65729,
                    _                      => 0,
                };
                if (quest > 0 && !QuestManager.IsQuestComplete(quest))
                {
                    GatherBuddy.Log.Warning($"{item.Name} was requested but {item.GatheringType} is not unlocked");
                    continue;
                }
                if (InventoryCount(item) >= QuantityTotal(item) || item.IsTreasureMap && InventoryCount(item) > 0)
                    continue;

                if (item.IsTreasureMap && NextTreasureMapAllowance >= AdjustedServerTime.DateTime)
                    continue;

                if (GatherBuddy.UptimeManager.TimedGatherables.Contains(item))
                {
                    if (time.InRange(AdjustedServerTime))
                    {
                        ItemsToGather.Add((item, location, time));
                        _ipcEventWaitingFired = true;
                    }
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
            var node = _plugin.AutoGatherListsManager.GetPreferredLocation(item);
            var time = AdjustedServerTime;
            if (node != null && !VisitedTimedLocations.ContainsKey(node))
            {
                res = (node, node.Times.NextUptime(time));
            }
            //Second priority: location for preferred job.
            if ((res.Location == null || !res.Time.InRange(time)) && GatherBuddy.Config.PreferredGatheringType is GatheringType.Miner or GatheringType.Botanist)
            {
                res = GatherBuddy.UptimeManager.NextUptime(item, GatherBuddy.Config.PreferredGatheringType, time, [.. VisitedTimedLocations.Keys]);
            }
            //Otherwise: location for any job.
            if (res.Location == null || !res.Time.InRange(time))
            {
                res = GatherBuddy.UptimeManager.NextUptime(item, time, [.. VisitedTimedLocations.Keys]);
            }
            return (item, (GatheringNode?)res.Location, res.Time);
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


        private unsafe T* GetAddon<T>(string name)
        {
            var addon = (AtkUnitBase*)Dalamud.GameGui.GetAddonByName(name);
            if (addon != null && addon->IsFullyLoaded() && addon->IsReady)
                return (T*)addon;
            else
                return null;
        }
        public unsafe AddonGathering* GatheringAddon
            => GetAddon<AddonGathering>("Gathering");

        public unsafe AddonGatheringMasterpiece* MasterpieceAddon
            => GetAddon<AddonGatheringMasterpiece>("GatheringMasterpiece");

        public unsafe AddonMaterializeDialog* MaterializeAddon
            => GetAddon<AddonMaterializeDialog>("Materialize");

        public unsafe AddonMaterializeDialog* MaterializeDialogAddon
            => GetAddon<AddonMaterializeDialog>("MaterializeDialog");

        public unsafe AddonSelectYesno* SelectYesnoAddon
            => GetAddon<AddonSelectYesno>("SelectYesno");

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
                 //Node is open? Fades off shortly after closing the node, can't use items (but can mount) while it's set
                 || Dalamud.Conditions[85] && !Dalamud.Conditions[ConditionFlag.Gathering]
                 || Dalamud.ClientState.LocalPlayer.CurrentHp < 1
                 || Player.IsAnimationLocked)
                    return false;

                return true;
            }
        }

        private int InventoryCount(IGatherable gatherable)
            => _plugin.AutoGatherListsManager.GetInventoryCountForItem(gatherable);

        private uint QuantityTotal(IGatherable gatherable)
            => _plugin.AutoGatherListsManager.GetTotalQuantitiesForItem(gatherable);

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

        public static unsafe bool IsGivingLandOffCooldown
            => ActionManager.Instance()->IsActionOffCooldown(ActionType.Action, Actions.GivingLand.ActionId);

        //Should be near the upper bound to reduce the probability of overcapping.
        private const int GivingLandYield = 30;

        private static unsafe DateTime NextTreasureMapAllowance
            => FFXIVClientStructs.FFXIV.Client.Game.UI.UIState.Instance()->GetNextMapAllowanceDateTime();

        private static unsafe uint FreeInventorySlots
            => InventoryManager.Instance()->GetEmptySlotsInBag();

        private static TimeStamp AdjustedServerTime
            => GatherBuddy.Time.ServerTime.AddSeconds(GatherBuddy.Config.AutoGatherConfig.TimedNodePrecog);

        private static unsafe int CharacterGatheringStat
            => PlayerState.Instance()->Attributes[72];

        private ConfigPreset MatchConfigPreset(Gatherable? item) => _plugin.Interface.MatchConfigPreset(item);
}

    public record class GatherInfo(Gatherable Item, GatheringNode? Location, TimeInterval Time)
    {
        public static implicit operator (Gatherable Item, GatheringNode? Location, TimeInterval Time)(GatherInfo value)
        {
            return (value.Item, value.Location, value.Time);
        }

        public static implicit operator GatherInfo((Gatherable Item, GatheringNode? Location, TimeInterval Time) value)
        {
            return new GatherInfo(value.Item, value.Location, value.Time);
        }
    }
}
