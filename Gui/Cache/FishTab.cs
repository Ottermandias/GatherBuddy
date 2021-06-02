using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public          Fish[]                      CachedFish = new Fish[0];

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
        private byte   _whichFilters   = 0;

        public byte WhichFilters
            => (byte) (_whichFilters | (_config.ShowAlreadyCaught ? 0 : 1));

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

        private          long                     _totalHour = 0;
        private readonly WeatherManager           _weather;
        private readonly FishManager              _fishManager;
        private readonly UptimeComparer           _uptimeComparer = new ();
        private readonly GatherBuddyConfiguration _config;

        public FishTab(WeatherManager weather, GatherBuddyConfiguration config, FishManager fishManager, Icons icons)
        {
            _weather     = weather;
            _fishManager = fishManager;
            _config      = config;

            IconHookSet          = icons[1103];
            IconPowerfulHookSet  = icons[1115];
            IconPrecisionHookSet = icons[1116];
            IconSnagging         = icons[1109];
            IconGigs             = icons[1121];
            IconSmallGig         = icons[60671];
            IconNormalGig        = icons[60672];
            IconLargeGig         = icons[60673];

            Selector = PatchSelector[config.ShowFishFromPatch];

            AllCachedFish = fishManager.FishByUptime.ToDictionary(f => f, f => new Fish(this, fishManager, f));
            foreach (var fish in AllCachedFish.Values)
            {
                LongestFish       = Math.Max(LongestFish, ImGui.CalcTextSize(fish.Name).X / ImGui.GetIO().FontGlobalScale);
                LongestSpot       = Math.Max(LongestSpot, ImGui.CalcTextSize(fish.FishingSpot).X / ImGui.GetIO().FontGlobalScale);
                LongestZone       = Math.Max(LongestZone, ImGui.CalcTextSize(fish.Territory).X / ImGui.GetIO().FontGlobalScale);
                LongestPercentage = Math.Max(LongestPercentage, ImGui.CalcTextSize(fish.UptimeString).X / ImGui.GetIO().FontGlobalScale);
                if (fish.Bait.Length > 0)
                    LongestBait = Math.Max(LongestBait, ImGui.CalcTextSize(fish.Bait[0].Name).X / ImGui.GetIO().FontGlobalScale);
            }
            LongestMinutes = ImGui.CalcTextSize("0000:00 Minutes").X / ImGui.GetIO().FontGlobalScale;

            SetCurrentlyRelevantFish();
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

        public Cache.Fish[] GetFishToSettings()
        {
            // @formatter:off
            switch (WhichFilters)
            {
                case 0b00000: return CachedFish;
                case 0b00001: return CachedFish.Where(f => FishUncaught(f.Base)).ToArray();
                case 0b00010: return CachedFish.Where(f => f.NameLower.Contains(FishFilterLower)).ToArray();
                case 0b00011: return CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower)).ToArray();
                case 0b00100: return CachedFish.Where(f => f.FirstBaitLower.Contains(BaitFilterLower)).ToArray();
                case 0b00101: return CachedFish.Where(f => FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower)).ToArray();
                case 0b00110: return CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower)).ToArray();
                case 0b00111: return CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower)).ToArray();
                case 0b01000: return CachedFish.Where(f => f.FishingSpotLower.Contains(SpotFilterLower)).ToArray();
                case 0b01001: return CachedFish.Where(f => FishUncaught(f.Base) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray();
                case 0b01010: return CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray();
                case 0b01011: return CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray();
                case 0b01100: return CachedFish.Where(f => f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray();
                case 0b01101: return CachedFish.Where(f => FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray();
                case 0b01110: return CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray();
                case 0b01111: return CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower)).ToArray();
                case 0b10000: return CachedFish.Where(f => f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b10001: return CachedFish.Where(f => FishUncaught(f.Base) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b10010: return CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b10011: return CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b10100: return CachedFish.Where(f => f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b10101: return CachedFish.Where(f => FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b10110: return CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b10111: return CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b11000: return CachedFish.Where(f => f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b11001: return CachedFish.Where(f => FishUncaught(f.Base) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b11010: return CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b11011: return CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b11100: return CachedFish.Where(f => f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b11101: return CachedFish.Where(f => FishUncaught(f.Base) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b11110: return CachedFish.Where(f => f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
                case 0b11111: return CachedFish.Where(f => FishUncaught(f.Base) && f.NameLower.Contains(FishFilterLower) && f.FirstBaitLower.Contains(BaitFilterLower) && f.FishingSpotLower.Contains(SpotFilterLower) && f.TerritoryLower.Contains(ZoneFilterLower)).ToArray();
            }
            // @formatter:on
            throw new InvalidEnumArgumentException();
        }

        private bool CheckFishType(Game.Fish f)
        {
            if (f.IsBigFish)
                return _config.ShowBigFish;

            return f.IsSpearFish ? _config.ShowSpearFish : _config.ShowSmallFish;
        }

        private bool SelectFish(Game.Fish f)
        {
            if (!Selector(f))
                return false;

            if (!_config.ShowAlwaysUp && f.FishRestrictions == FishRestrictions.None)
                return false;

            if (!CheckFishType(f))
                return false;

            return true;
        }

        private bool FishUncaught(Game.Fish f)
            => !_fishManager.FishLog.IsUnlocked(f);

        private static string[] PreparePatches()
        {
            var patches = (Patch[]) Enum.GetValues(typeof(Patch));
            var expansions = new string[]
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
