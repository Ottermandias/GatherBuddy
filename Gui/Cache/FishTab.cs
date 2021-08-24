using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Managers;
using ImGuiNET;
using ImGuiScene;

namespace GatherBuddy.Gui.Cache
{
    internal class FishTab
    {
        public readonly TextureWrap IconHookSet;
        public readonly TextureWrap IconPowerfulHookSet;
        public readonly TextureWrap IconPrecisionHookSet;
        public readonly TextureWrap IconSnagging;
        public readonly TextureWrap IconGigs;
        public readonly TextureWrap IconSmallGig;
        public readonly TextureWrap IconNormalGig;
        public readonly TextureWrap IconLargeGig;

        public static readonly string[]                Patches       = PreparePatches();
        public static readonly Func<Game.Fish, bool>[] PatchSelector = PreparePatchSelectors();

        public static readonly (string, uint) WeakBite      = ("  !  ", Colors.FishTab.WeakBite);
        public static readonly (string, uint) StrongBite    = (" ! ! ", Colors.FishTab.StrongBite);
        public static readonly (string, uint) LegendaryBite = (" !!! ", Colors.FishTab.LegendaryBite);
        public static readonly (string, uint) UnknownBite   = (" ? ? ", Colors.FishTab.UnknownBite);

        public readonly Dictionary<Game.Fish, Fish> AllCachedFish;
        public          Game.Fish[]                 RelevantFish = new Game.Fish[0];
        public          Fish[]                      CachedFish   = new Fish[0];
        public readonly List<Fish>                  FixedFish;

        public readonly float LongestFish;
        public readonly float LongestSpot;
        public readonly float LongestZone;
        public readonly float LongestBait;
        public readonly float LongestPercentage;
        public readonly float LongestMinutes;

        public  string FishFilter      = "";
        public  string FishFilterLower = "";
        public  string BaitFilter      = "";
        public  string BaitFilterLower = "";
        public  string SpotFilter      = "";
        public  string SpotFilterLower = "";
        public  string ZoneFilter      = "";
        public  string ZoneFilterLower = "";
        private byte   _whichFilters;

        public byte WhichFilters
            => (byte) (_whichFilters | (GatherBuddy.Config.ShowAlreadyCaught ? 0 : 1));

        public void UpdateFishFilter()
        {
            FishFilterLower = FishFilter.ToLowerInvariant();
            if (FishFilter.Length > 0)
                _whichFilters |= 0b00010;
            else
                _whichFilters &= 0b11100;
        }

        public void UpdateBaitFilter()
        {
            BaitFilterLower = BaitFilter.ToLowerInvariant();
            if (BaitFilter.Length > 0)
                _whichFilters |= 0b00100;
            else
                _whichFilters &= 0b11010;
        }

        public void UpdateSpotFilter()
        {
            SpotFilterLower = SpotFilter.ToLowerInvariant();
            if (SpotFilter.Length > 0)
                _whichFilters |= 0b01000;
            else
                _whichFilters &= 0b10110;
        }

        public void UpdateZoneFilter()
        {
            ZoneFilterLower = ZoneFilter.ToLowerInvariant();
            if (ZoneFilter.Length > 0)
                _whichFilters |= 0b10000;
            else
                _whichFilters &= 0b01110;
        }

        public Func<Game.Fish, bool> Selector;

        private          long           _totalHour;
        private readonly WeatherManager _weather;
        private readonly FishManager    _fishManager;
        private readonly UptimeComparer _uptimeComparer = new();

        private List<Fish> SetupFixedFish(IList<uint> fishIds, FishManager fishManager)
        {
            var bad = false;
            var ret = new List<Fish>(fishIds.Count);
            for (var i = 0; i < fishIds.Count; ++i)
            {
                var id = fishIds[i];
                if (!fishManager.Fish.TryGetValue(id, out var fish))
                {
                    bad = true;
                    fishIds.RemoveAt(i);
                    --i;
                    PluginLog.Information($"Removed invalid fish id {id} from fixed fishes.");
                    continue;
                }

                if (!AllCachedFish.TryGetValue(fish, out var cachedFish))
                {
                    PluginLog.Debug($"Could not get {fish} from cache.");
                    continue;
                }

                ret.Add(cachedFish);
            }

            if (bad)
                GatherBuddy.Config.Save();

            return ret;
        }

        public FishTab(WeatherManager weather, FishManager fishManager, Icons icons)
        {
            _weather     = weather;
            _fishManager = fishManager;

            IconHookSet          = icons[1103];
            IconPowerfulHookSet  = icons[1115];
            IconPrecisionHookSet = icons[1116];
            IconSnagging         = icons[1109];
            IconGigs             = icons[1121];
            IconSmallGig         = icons[60671];
            IconNormalGig        = icons[60672];
            IconLargeGig         = icons[60673];

            Selector = PatchSelector[GatherBuddy.Config.ShowFishFromPatch];

            AllCachedFish = fishManager.FishByUptime.ToDictionary(f => f, f => new Fish(this, f));
            foreach (var fish in AllCachedFish.Values)
            {
                LongestFish       = Math.Max(LongestFish,       ImGui.CalcTextSize(fish.Name).X / ImGui.GetIO().FontGlobalScale);
                LongestSpot       = Math.Max(LongestSpot,       ImGui.CalcTextSize(fish.FishingSpot).X / ImGui.GetIO().FontGlobalScale);
                LongestZone       = Math.Max(LongestZone,       ImGui.CalcTextSize(fish.Territory).X / ImGui.GetIO().FontGlobalScale);
                LongestPercentage = Math.Max(LongestPercentage, ImGui.CalcTextSize(fish.UptimeString).X / ImGui.GetIO().FontGlobalScale);
                if (fish.Bait.Length > 0)
                    LongestBait = Math.Max(LongestBait, ImGui.CalcTextSize(fish.Bait[0].Name).X / ImGui.GetIO().FontGlobalScale);
            }

            LongestMinutes = ImGui.CalcTextSize("0000:00 Minutes").X / ImGui.GetIO().FontGlobalScale;
            FixedFish      = SetupFixedFish(GatherBuddy.Config.FixedFish, fishManager);

            SetCurrentlyRelevantFish();
        }

        public void FixFish(Fish fish)
        {
            if (FixedFish.Contains(fish))
                return;

            fish.IsFixed = true;
            FixedFish.Add(fish);
            GatherBuddy.Config.FixedFish.Add(fish.Base.ItemId);
            GatherBuddy.Config.Save();
        }

        public void UnfixFish(Fish fish)
        {
            if (!FixedFish.Remove(fish))
                return;

            fish.IsFixed = false;
            GatherBuddy.Config.FixedFish.Remove(fish.Base.ItemId);
            GatherBuddy.Config.Save();
        }

        public void ToggleFishFix(Fish fish)
        {
            if (FixedFish.Contains(fish))
            {
                UnfixFish(fish);
            }
            else
            {
                fish.IsFixed = true;
                FixedFish.Add(fish);
                GatherBuddy.Config.FixedFish.Add(fish.Base.ItemId);
                GatherBuddy.Config.Save();
            }
        }

        public void UpdateFish(long totalHour)
        {
            if (totalHour <= _totalHour)
                return;

            _totalHour = totalHour;
            CachedFish = RelevantFish
                .OrderBy(f => f.NextUptime(_weather), _uptimeComparer)
                .Select(f => AllCachedFish[f]).ToArray();
        }


        public void SetCurrentlyRelevantFish()
        {
            RelevantFish = _fishManager.FishByUptime
                .Where(SelectFish)
                .ToArray();
            CachedFish = RelevantFish
                .Select(f => AllCachedFish[f])
                .OrderBy(f => f.Base.NextUptime(_weather), _uptimeComparer)
                .ToArray();
        }

        public Fish[] GetFishToSettings()
        {
            // @formatter:off
            var filters = WhichFilters;
            if (FixedFish.Count > 0)
                filters |= 0b100000;
            return filters switch
            {
                0b000000 => CachedFish,
                0b000001 => CachedFish.Where(f => FishUncaught(f.Base)).ToArray(),
                0b000010 => CachedFish.Where(f => f.NameLower.Contains(FishFilterLower)).ToArray(),
                0b000011 => CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower)).ToArray(),
                0b000100 => CachedFish.Where(f => f.FirstBaitLower.Contains(BaitFilterLower)).ToArray(),
                0b000101 => CachedFish.Where(f => FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower)).ToArray(),
                0b000110 => CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower)).ToArray(),
                0b000111 => CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower)).ToArray(),
                0b001000 => CachedFish.Where(f => f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b001001 => CachedFish.Where(f => FishUncaught(f.Base) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b001010 => CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b001011 => CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b001100 => CachedFish.Where(f => f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b001101 => CachedFish.Where(f => FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b001110 => CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b001111 => CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b010000 => CachedFish.Where(f => f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b010001 => CachedFish.Where(f => FishUncaught(f.Base) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b010010 => CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b010011 => CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b010100 => CachedFish.Where(f => f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b010101 => CachedFish.Where(f => FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b010110 => CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b010111 => CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b011000 => CachedFish.Where(f => f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b011001 => CachedFish.Where(f => FishUncaught(f.Base) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b011010 => CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b011011 => CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b011100 => CachedFish.Where(f => f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b011101 => CachedFish.Where(f => FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b011110 => CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b011111 => CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b100000 => CachedFish.Where(f => !FixedFish.Contains(f)).ToArray(),
                0b100001 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base)).ToArray(),
                0b100010 => CachedFish.Where(f => !FixedFish.Contains(f) && f.NameLower.Contains(FishFilterLower)).ToArray(),
                0b100011 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower)).ToArray(),
                0b100100 => CachedFish.Where(f => !FixedFish.Contains(f) && f.FirstBaitLower.Contains(BaitFilterLower)).ToArray(),
                0b100101 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower)).ToArray(),
                0b100110 => CachedFish.Where(f => !FixedFish.Contains(f) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower)).ToArray(),
                0b100111 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower)).ToArray(),
                0b101000 => CachedFish.Where(f => !FixedFish.Contains(f) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b101001 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b101010 => CachedFish.Where(f => !FixedFish.Contains(f) && f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b101011 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b101100 => CachedFish.Where(f => !FixedFish.Contains(f) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b101101 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b101110 => CachedFish.Where(f => !FixedFish.Contains(f) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b101111 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray(),
                0b110000 => CachedFish.Where(f => !FixedFish.Contains(f) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b110001 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b110010 => CachedFish.Where(f => !FixedFish.Contains(f) && f.NameLower.Contains(FishFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b110011 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b110100 => CachedFish.Where(f => !FixedFish.Contains(f) && f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b110101 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b110110 => CachedFish.Where(f => !FixedFish.Contains(f) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b110111 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b111000 => CachedFish.Where(f => !FixedFish.Contains(f) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b111001 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b111010 => CachedFish.Where(f => !FixedFish.Contains(f) && f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b111011 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b111100 => CachedFish.Where(f => !FixedFish.Contains(f) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b111101 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b111110 => CachedFish.Where(f => !FixedFish.Contains(f) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                0b111111 => CachedFish.Where(f => !FixedFish.Contains(f) && FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray(),
                // @formatter:on
                _ => throw new InvalidEnumArgumentException(),
            };
        }

        private bool CheckFishType(Game.Fish f)
        {
            if (f.IsBigFish)
                return GatherBuddy.Config.ShowBigFish;

            return f.IsSpearFish ? GatherBuddy.Config.ShowSpearFish : GatherBuddy.Config.ShowSmallFish;
        }

        private bool SelectFish(Game.Fish f)
        {
            if (!Selector(f))
                return false;

            if (!GatherBuddy.Config.ShowAlwaysUp && f.FishRestrictions == FishRestrictions.None)
                return false;

            return CheckFishType(f);
        }

        private bool FishUncaught(Game.Fish f)
            => !_fishManager.FishLog.IsUnlocked(f);

        private static string[] PreparePatches()
        {
            var patches = (Patch[]) Enum.GetValues(typeof(Patch));
            var expansions = new[]
            {
                "All",
                "A Realm Reborn",
                "Heavensward",
                "Stormblood",
                "Shadowbringers",
                "Endwalker",
            };
            return expansions.Concat(patches
                .Select(PatchExtensions.ToVersionString)).ToArray();
        }

        private static Func<Game.Fish, bool>[] PreparePatchSelectors()
        {
            var patches = (Patch[]) Enum.GetValues(typeof(Patch));
            var expansions = new Func<Game.Fish, bool>[]
            {
                _ => true,
                f => (f.CatchData?.Patch.ToExpansion() ?? 0) == Patch.ARealmReborn,
                f => (f.CatchData?.Patch.ToExpansion() ?? 0) == Patch.Heavensward,
                f => (f.CatchData?.Patch.ToExpansion() ?? 0) == Patch.Stormblood,
                f => (f.CatchData?.Patch.ToExpansion() ?? 0) == Patch.Shadowbringers,
                f => (f.CatchData?.Patch.ToExpansion() ?? 0) == Patch.Endwalker,
            };
            return expansions.Concat(patches
                .Select(p => new Func<Game.Fish, bool>(f => (f.CatchData?.Patch ?? 0) == p))).ToArray();
        }

        private class UptimeComparer : IComparer<RealUptime>
        {
            public int Compare(RealUptime x, RealUptime y)
                => x.Compare(y);
        }
    }
}
