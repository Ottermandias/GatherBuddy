using System.Linq;
using System.Numerics;
using Dalamud.Plugin;
using GatherBuddy.Enums;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
using ImGuiNET;
using ImGuiScene;

namespace GatherBuddy.Gui.Cache
{
    internal class Fish
    {
        public struct BaitOrder
        {
            public TextureWrap    Icon    { get; set; }
            public string         Name    { get; set; }
            public Game.Fish?     Fish    { get; set; }
            public TextureWrap?   HookSet { get; set; }
            public (string, uint) Bite    { get; set; }
        }

        public struct Predator
        {
            public TextureWrap Icon   { get; set; }
            public string      Name   { get; set; }
            public string      Amount { get; set; }
        }

        public Game.Fish       Base             { get; }
        public TextureWrap     Icon             { get; }
        public string          Name             { get; }
        public string          NameLower        { get; }
        public string          Time             { get; }
        public TextureWrap[][] WeatherIcons     { get; }
        public BaitOrder[]     Bait             { get; }
        public string          Territory        { get; }
        public string          TerritoryLower   { get; }
        public string          FishingSpot      { get; }
        public string          FishingSpotLower { get; }
        public TextureWrap?    Snagging         { get; }
        public Predator[]      Predators        { get; }
        public string          Patch            { get; }


        private static string SetTime(Game.Fish fish)
        {
            if (!fish.FishRestrictions.HasFlag(FishRestrictions.Time))
                return "Always Up";
            if (fish.CatchData?.Hours.AlwaysUp() ?? true)
                return "Unknown Uptime";

            return fish.CatchData!.Hours.PrintHours();
        }

        private static TextureWrap[][] SetWeather(Game.Fish fish)
        {
            if (!fish.FishRestrictions.HasFlag(FishRestrictions.Weather))
                return new TextureWrap[0][];
            if (fish.CatchData == null || fish.CatchData.PreviousWeather.Length == 0 && fish.CatchData.CurrentWeather.Length == 0)
                return new TextureWrap[0][];

            var icons = Service<Cache.Icons>.Get();
            var sheet = Service<DalamudPluginInterface>.Get().Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Weather>();
            return new TextureWrap[][]
            {
                fish.CatchData.PreviousWeather.Select(w => icons[sheet.GetRow(w).Icon]).ToArray(),
                fish.CatchData.CurrentWeather.Select(w => icons[sheet.GetRow(w).Icon]).ToArray(),
            };
        }

        private static Predator[] SetPredators(FishManager manager, Game.Fish fish)
        {
            if (fish.CatchData == null || fish.CatchData.Predator.Length == 0)
                return new Predator[0];

            return fish.CatchData.Predator.Select(p =>
            {
                var f    = manager.Fish[p.Item1];
                var icon = Service<Cache.Icons>.Get()[f.ItemData.Icon];
                return new Predator()
                {
                    Amount = p.Item2.ToString(),
                    Name   = f.Name[GatherBuddy.Language],
                    Icon   = icon,
                };
            }).ToArray();
        }

        private static (string, uint) FromBiteType(BiteType bite)
        {
            return bite switch
            {
                BiteType.Weak      => FishTab.WeakBite,
                BiteType.Strong    => FishTab.StrongBite,
                BiteType.Legendary => FishTab.LegendaryBite,
                _                  => FishTab.UnknownBite,
            };
        }

        private static TextureWrap FromHookSet(FishTab cache, HookSet hook)
        {
            return hook switch
            {
                HookSet.Precise  =>cache.IconPrecisionHookSet,
                HookSet.Powerful =>cache.IconPowerfulHookSet,
                _                =>cache.IconHookSet,
            };
        }

        private static BaitOrder[] SetBait(FishTab cache, FishManager manager, Game.Fish fish)
        {
            if (fish.IsSpearFish)
                return new BaitOrder[1]
                {
                    new()
                    {
                        Name = $"{fish.Gig} Gig Head",
                        Fish = null,
                        Icon = fish.Gig switch
                        {
                            GigHead.Small  => cache.IconSmallGig,
                            GigHead.Normal => cache.IconNormalGig,
                            GigHead.Large  => cache.IconLargeGig,
                            _              => cache.IconGigs,
                        },
                        Bite    = FishTab.UnknownBite,
                        HookSet = null,
                    },
                };

            if (fish.CatchData == null || fish.CatchData.BaitOrder.Length == 0)
                return new BaitOrder[0];

            var ret  = new BaitOrder[fish.CatchData.BaitOrder.Length];
            var bait = manager.Bait[fish.CatchData.BaitOrder[0]];
            ret[0] = new BaitOrder()
            {
                Icon = Service<Cache.Icons>.Get()[bait.Data.Icon],
                Name = bait.Name[GatherBuddy.Language],
                Fish = null,
            };
            for (var idx = 1; idx < fish.CatchData.BaitOrder.Length; ++idx)
            {
                var tmp  = fish.CatchData.BaitOrder[idx];
                var f    = manager.Fish[tmp];
                var hook = f.CatchData?.HookSet ?? HookSet.Unknown;
                ret[idx - 1].HookSet = FromHookSet(cache, hook);
                ret[idx - 1].Bite    = FromBiteType(f.CatchData?.BiteType ?? f.Record.BiteType);
                ret[idx] = new BaitOrder()
                {
                    Icon = Service<Cache.Icons>.Get()[f.ItemData.Icon],
                    Name = f.Name[GatherBuddy.Language],
                    Fish = f,
                };
            }

            ret[ret.Length - 1].HookSet = FromHookSet(cache, fish.CatchData?.HookSet ?? HookSet.Unknown);
            ret[ret.Length - 1].Bite    = FromBiteType(fish.CatchData?.BiteType ?? fish.Record.BiteType);
            return ret;
        }

        private static TextureWrap? SetSnagging(FishTab cache, BaitOrder[] baitOrder, Game.Fish fish)
        {
            if ((fish.CatchData?.Snagging ?? Enums.Snagging.Unknown) == Enums.Snagging.Required)
                return cache.IconSnagging;

            return baitOrder.Any(bait => (bait.Fish?.CatchData?.Snagging ?? Enums.Snagging.Unknown) == Enums.Snagging.Required)
                ? cache.IconSnagging
                : null;
        }

        public Fish(FishTab cache, FishManager manager, Game.Fish fish)
        {
            Base             = fish;
            Icon             = Service<Cache.Icons>.Get()[fish.ItemData.Icon];
            Name             = fish.Name[GatherBuddy.Language];
            NameLower        = Name.ToLowerInvariant();
            Time             = SetTime(fish);
            WeatherIcons     = SetWeather(fish);
            Bait             = SetBait(cache, manager, fish);
            Predators        = SetPredators(manager, fish);
            Territory        = fish.FishingSpots.First().Territory!.Name[GatherBuddy.Language];
            TerritoryLower   = Territory.ToLowerInvariant();
            FishingSpot      = fish.FishingSpots.First().PlaceName![GatherBuddy.Language];
            FishingSpotLower = FishingSpot.ToLowerInvariant();
            Snagging         = SetSnagging(cache, Bait, fish);
            Patch            = $"Patch {fish.CatchData?.Patch.ToVersionString() ?? "???"}";
        }
    }
}
