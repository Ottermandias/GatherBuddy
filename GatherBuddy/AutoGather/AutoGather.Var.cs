using System;
using Dalamud.Game.ClientState.Conditions;
using ECommons.ExcelServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.AutoGather.Lists;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using GatherBuddy.Time;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using ECommons;
using ECommons.DalamudServices;
using ECommons.MathHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using GatherBuddy.AutoGather.AtkReaders;
using LuminaTerritoryType = Lumina.Excel.Sheets.TerritoryType;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        public bool IsPathing
            => VNavmesh.Path.IsRunning();

        public bool IsPathGenerating
            => VNavmesh.Nav.PathfindInProgress();

        public bool NavReady
            => VNavmesh.Nav.IsReady();

        private bool IsBlacklisted(Vector3 g)
        {
            var blacklisted = GatherBuddy.Config.AutoGatherConfig.BlacklistedNodesByTerritoryId.ContainsKey(Dalamud.ClientState.TerritoryType)
             && GatherBuddy.Config.AutoGatherConfig.BlacklistedNodesByTerritoryId[Dalamud.ClientState.TerritoryType].Contains(g);
            return blacklisted;
        }

        public bool IsGathering
            => Dalamud.Conditions[ConditionFlag.Gathering] || Dalamud.Conditions[ConditionFlag.ExecutingGatheringAction];

        public bool IsFishing
            => Dalamud.Conditions[ConditionFlag.Fishing];

        public  bool?      LastNavigationResult { get; set; }         = null;
        public  Vector3    CurrentDestination   { get; private set; } = default;
        public  Angle      CurrentRotation      { get; private set; } = default;
        private ILocation? CurrentFarNodeLocation;
        public bool LureSuccess { get; private set; } = false;

        public unsafe GatheringReader? GatheringWindowReader
            => GenericHelpers.TryGetAddonByName("Gathering", out AtkUnitBase* addon)
                ? new GatheringReader(addon)
                : null;

        public unsafe GatheringMasterpieceReader? MasterpieceReader
            => GenericHelpers.TryGetAddonByName("GatheringMasterpiece", out AtkUnitBase* add)
                ? new GatheringMasterpieceReader(add)
                : null;

        public static IReadOnlyList<InventoryType> InventoryTypes { get; } =
        [
            InventoryType.Inventory1,
            InventoryType.Inventory2,
            InventoryType.Inventory3,
            InventoryType.Inventory4,
        ];

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

        public unsafe bool ShouldFly(Vector3 destination)
        {
            if (Dalamud.Conditions[ConditionFlag.InFlight] || Dalamud.Conditions[ConditionFlag.Diving])
                return true;

            if (GatherBuddy.Config.AutoGatherConfig.ForceWalking || Dalamud.ClientState.LocalPlayer == null)
            {
                return false;
            }

            if (Functions.InTheDiadem())
            {
                return Vector3.Distance(Dalamud.ClientState.LocalPlayer.Position, destination)
                 >= GatherBuddy.Config.AutoGatherConfig.MountUpDistance;
            }

            var territory = Dalamud.ClientState.TerritoryType;
            var territoryRow = Svc.Data.GameData.GetExcelSheet<LuminaTerritoryType>();
            if (territoryRow == null)
                return false;

            var playerState = PlayerState.Instance();
            if (playerState == null)
                return false;

            var aetherCurrentComp = territoryRow.GetRow(territory).AetherCurrentCompFlgSet.RowId;
            if (aetherCurrentComp == 0)
                return false;

            return playerState->IsAetherCurrentZoneComplete(aetherCurrentComp) && Vector3.Distance(Dalamud.ClientState.LocalPlayer.Position, destination)
             >= GatherBuddy.Config.AutoGatherConfig.MountUpDistance;
        }

        public unsafe Vector2? TimedNodePosition
        {
            get
            {
                var map     = FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentMap.Instance();
                var markers = map->MiniMapGatheringMarkers;
                if (markers == null)
                    return null;

                Vector2? result = null;
                foreach (var miniMapGatheringMarker in markers)
                {
                    if (miniMapGatheringMarker.MapMarker.X != 0 && miniMapGatheringMarker.MapMarker.Y != 0)
                    {
                        // ReSharper disable twice PossibleLossOfFraction
                        result = new Vector2(miniMapGatheringMarker.MapMarker.X / 16, miniMapGatheringMarker.MapMarker.Y / 16);
                        break;
                    }
                }

                return result;
            }
        }

        public  string      AutoStatus { get; private set; } = "Idle";
        public  int         LastCollectability = 0;
        public  int         LastIntegrity      = 0;
        private bool LuckUsed;
        private bool        WentHome;
        private bool        _fishingYesAlreadyUnlocked = false;

        internal IEnumerable<GatherTarget> ItemsToGather
            => _activeItemList;

        internal ReadOnlyDictionary<GatheringNode, TimeInterval> DebugVisitedTimedLocations
            => _activeItemList.DebugVisitedTimedLocations;

        public readonly HashSet<Vector3> FarNodesSeenSoFar = [];
        public readonly LinkedList<uint> VisitedNodes      = [];

        private readonly Dictionary<Vector3, DateTime> _diademSpawnAreaLastChecked = new();
        private Vector3? _currentDiademPatrolTarget = null;
        private const float DiademSpawnAreaCheckRadius = 80f;
        private const int DiademSpawnAreaRecheckSeconds = 180;
        
        private readonly LinkedList<Vector3> _diademRecentlyGatheredNodes = new();
        private const int DiademNodeRespawnWindow = 8;
        
        private DateTime _diademArborCallUsedAt = DateTime.MinValue;
        private Vector3? _diademArborCallTarget = null;
        
        private readonly LinkedList<Vector3> _diademVisitedNodes = new();
        private const int DiademVisitedNodeTrackingCount = 20;
        private const float DiademNodeProximityThreshold = 5f;
        
        private uint _lastUmbralWeather = 0;
        private bool _hasGatheredUmbralThisSession = false;
        private uint _lastTerritory = 0;
        
        private uint _lastNonTimedNodeTerritory = 0;
        private GatheringType _lastJob = GatheringType.Unknown;

        private IEnumerator<Actions.BaseAction?>? ActionSequence;

        private static unsafe T* GetAddon<T>(string name) where T : unmanaged
        {
            var addon = (AtkUnitBase*)(nint)Dalamud.GameGui.GetAddonByName(name);
            if (addon != null && addon->IsFullyLoaded() && addon->IsReady)
                return (T*)addon;
            else
                return null;
        }

        public static unsafe AddonGathering* GatheringAddon
            => GetAddon<AddonGathering>("Gathering");

        public static unsafe AddonGatheringMasterpiece* MasterpieceAddon
            => GetAddon<AddonGatheringMasterpiece>("GatheringMasterpiece");

        public static unsafe AddonMaterializeDialog* MaterializeAddon
            => GetAddon<AddonMaterializeDialog>("Materialize");

        public static unsafe AddonMaterializeDialog* MaterializeDialogAddon
            => GetAddon<AddonMaterializeDialog>("MaterializeDialog");

        public static unsafe AddonSelectYesno* SelectYesnoAddon
            => GetAddon<AddonSelectYesno>("SelectYesno");

        public static unsafe AtkUnitBase* PurifyItemSelectorAddon
            => GetAddon<AtkUnitBase>("PurifyItemSelector");

        public static unsafe AtkUnitBase* PurifyResultAddon
            => GetAddon<AtkUnitBase>("PurifyResult");

        public static unsafe AddonRepair* RepairAddon
            => GetAddon<AddonRepair>("Repair");

        public IEnumerable<IGatherable> ItemsToGatherInZone
            => _activeItemList
                .Where(i => i.Node?.Territory.Id == Dalamud.ClientState.TerritoryType)
                .Where(i => LocationMatchesJob(i.Location))
                .Select(i => i.Item);

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
                 || Dalamud.Conditions[ConditionFlag.OccupiedInQuestEvent]
                 || Dalamud.Conditions[ConditionFlag.OccupiedSummoningBell]
                 || Dalamud.Conditions[ConditionFlag.BeingMoved]
                 || Dalamud.Conditions[ConditionFlag.Casting]
                 || Dalamud.Conditions[ConditionFlag.Casting87]
                 || Dalamud.Conditions[ConditionFlag.Jumping]
                 || Dalamud.Conditions[ConditionFlag.Jumping61]
                 || Dalamud.Conditions[ConditionFlag.LoggingOut]
                 || Dalamud.Conditions[ConditionFlag.Occupied]
                 || Dalamud.Conditions[ConditionFlag.Occupied39]
                 || Dalamud.Conditions[ConditionFlag.Unconscious]
                 || Dalamud.Conditions[ConditionFlag.ExecutingGatheringAction]
                 || Dalamud.Conditions[ConditionFlag.Mounting] // Mounting up
                    //Node is open? Fades off shortly after closing the node, can't use items (but can mount) while it's set
                 || Dalamud.Conditions[85] && !Dalamud.Conditions[ConditionFlag.Gathering]
                 || Dalamud.ClientState.LocalPlayer.IsDead
                 || Player.IsAnimationLocked)
                    return false;

                return true;
            }
        }

        private static unsafe bool HasGivingLandBuff
            => Dalamud.ClientState.LocalPlayer?.StatusList.Any(s => s.StatusId == 1802) ?? false;

        public static unsafe bool IsGivingLandOffCooldown
            => ActionManager.Instance()->IsActionOffCooldown(ActionType.Action, Actions.GivingLand.ActionId);

        //Should be near the upper bound to reduce the probability of overcapping.
        private const int GivingLandYield = 30;

        private static unsafe uint FreeInventorySlots
            => InventoryManager.Instance()->GetEmptySlotsInBag();

        public static TimeStamp AdjustedServerTime
            => GatherBuddy.Time.ServerTime.AddSeconds(GatherBuddy.Config.AutoGatherConfig.TimedNodePrecog);

        private ConfigPreset MatchConfigPreset(Gatherable? item)
            => _plugin.Interface.MatchConfigPreset(item);

        private ConfigPreset MatchConfigPreset(Fish? item)
            => _plugin.Interface.MatchConfigPreset(item);
    }
}
