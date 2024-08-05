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
using ECommons;
using ECommons.DalamudServices;
using ECommons.ExcelServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.UI;
using GatherBuddy.Classes;
using GatherBuddy.CustomInfo;
using GatherBuddy.Enums;
using GatherBuddy.Time;
using OtterGui.Log;

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
        public Vector3? CurrentDestination   { get; set; } = null;
        public bool     HasSeenFlag          { get; set; } = false;

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
                var correctedVector3   = uncorrectedVector3.CorrectForMesh(0.5f);
                if (uncorrectedVector3 == correctedVector3)
                    return null;

                if (!correctedVector3.SanityCheck())
                    return null;

                return correctedVector3;
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

                return Vector3.Distance(Dalamud.ClientState.LocalPlayer.Position, CurrentDestination ?? Vector3.Zero)
                 >= GatherBuddy.Config.AutoGatherConfig.MountUpDistance;
            }
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
        public int    LastCollectability = 0;
        public int    LastIntegrity      = 0;

        public List<IGatherable> TimedItemsToGather = new();
        public List<IGatherable> ItemsToGather      = new();

        public List<uint> TimedNodesGatheredThisTrip = new();

        public void UpdateItemsToGather()
        {
            if (IsGathering)
                return;
            TimedItemsToGather.Clear();
            ItemsToGather.Clear();
            List<IGatherable> activeItems = OrderActiveItems(_plugin.GatherWindowManager.ActiveItems).ToList();
            foreach (var item in activeItems)
            {
                if (InventoryCount(item) >= QuantityTotal(item))
                    continue;

                if (GatherBuddy.UptimeManager.TimedGatherables.Contains(item))
                {
                    var location = GatherBuddy.UptimeManager.BestLocation(item);
                    if (location.Interval.InRange(GatherBuddy.Time.ServerTime.AddSeconds(GatherBuddy.Config.AutoGatherConfig.TimedNodePrecog))
                     && !TimedNodesGatheredThisTrip.Contains(item.ItemId))
                        TimedItemsToGather.Add(item);
                }
                else
                {
                    ItemsToGather.Add(item);
                }
            }
            //GatherBuddy.Log.Verbose($"Sorted {activeItems.Count} items into {TimedItemsToGather.Count} timed items and {ItemsToGather.Count} static items");
        }

        private IEnumerable<IGatherable> OrderActiveItems(List<IGatherable> activeItems)
        {
            switch (GatherBuddy.Config.AutoGatherConfig.SortingMethod)
            {
                case AutoGatherConfig.SortingType.Location: return activeItems.OrderBy(i => i.Locations.First().Id);
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
            => ItemsToGather.Where(i => i.Locations.Any(l => l.Territory.Id == Dalamud.ClientState.TerritoryType)).Where(GatherableMatchesJob);

        private bool GatherableMatchesJob(IGatherable arg)
        {
            var gatherable = arg as Gatherable;
            return gatherable != null
             && (gatherable.GatheringType.ToGroup() == JobAsGatheringType || gatherable.GatheringType.ToGroup() == GatheringType.Multiple);
        }

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
                 || Dalamud.Conditions[ConditionFlag.Unconscious]
                 || Dalamud.ClientState.LocalPlayer.CurrentHp < 1)
                    return false;

                return true;
            }
        }

        private int InventoryCount(IGatherable gatherable)
            => _plugin.GatherWindowManager.GetInventoryCountForItem(gatherable);

        private uint QuantityTotal(IGatherable gatherable)
            => _plugin.GatherWindowManager.GetTotalQuantitiesForItem(gatherable);

        private int GetNodeTypeAsPriority(IGatherable item)
        {
            if (GatherBuddy.Config.AutoGatherConfig.SortingMethod == AutoGatherConfig.SortingType.None)
                return 0;
            Gatherable gatherable = item as Gatherable;
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
    }
}
