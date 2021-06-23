using System.Globalization;
using System.Linq;
using Dalamud.Plugin;
using GatherBuddy.Enums;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
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

        public Game.Fish       Base                { get; }
        public TextureWrap     Icon                { get; }
        public string          Name                { get; }
        public string          NameLower           { get; }
        public string          Time                { get; }
        public TextureWrap[][] WeatherIcons        { get; }
        public BaitOrder[]     Bait                { get; }
        public string          FirstBaitLower      { get; }
        public string          Territory           { get; }
        public string          TerritoryLower      { get; }
        public string          FishingSpot         { get; }
        public string          FishingSpotLower    { get; }
        public TextureWrap?    Snagging            { get; }
        public Predator[]      Predators           { get; }
        public string          Patch               { get; }
        public string          UptimeString        { get; }
        public string          IntuitionText       { get; } = "";
        public bool            HasUptimeDependency { get; }


        private static string SetTime(Game.Fish fish, ref ushort uptime)
        {
            if (!fish.FishRestrictions.HasFlag(FishRestrictions.Time))
                return "Always Up";

            if (fish.CatchData?.Hours.AlwaysUp() ?? true)
            {
                uptime = 0;
                return "Unknown Uptime";
            }

            uptime = (ushort) (uptime * fish.CatchData!.Hours.Count / RealTime.HoursPerDay);
            return fish.CatchData!.Hours.PrintHours();
        }

        private static TextureWrap[][] SetWeather(Game.Fish fish, ref ushort uptime)
        {
            if (!fish.FishRestrictions.HasFlag(FishRestrictions.Weather))
                return new TextureWrap[0][];

            if (fish.CatchData == null || fish.CatchData.PreviousWeather.Length == 0 && fish.CatchData.CurrentWeather.Length == 0)
            {
                uptime = 0;
                return new TextureWrap[0][];
            }

            var icons      = Service<Icons>.Get();
            var sheet      = Service<DalamudPluginInterface>.Get().Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Weather>();
            var skyWatcher = Service<SkyWatcher>.Get();

            var previousChance = skyWatcher.ChanceForWeather(fish.FishingSpots.First().Territory!.Id, fish.CatchData.PreviousWeather);
            var currentChance  = skyWatcher.ChanceForWeather(fish.FishingSpots.First().Territory!.Id, fish.CatchData.CurrentWeather);
            if (previousChance == 0)
                previousChance = 100;
            if (currentChance == 0)
                currentChance = 100;

            uptime = (ushort) (uptime * currentChance * previousChance / 10000);

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
                var f    = p.Item1;
                var icon = Service<Icons>.Get()[f.ItemData.Icon];
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
                HookSet.Precise  => cache.IconPrecisionHookSet,
                HookSet.Powerful => cache.IconPowerfulHookSet,
                _                => cache.IconHookSet,
            };
        }

        private static BaitOrder[] SetBait(FishTab cache, Game.Fish fish)
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

            if (fish.CatchData == null || fish.CatchData.InitialBait.Equals(Game.Bait.Unknown))
                return new BaitOrder[0];

            var ret  = new BaitOrder[fish.CatchData.Mooches.Length + 1];
            var bait = fish.CatchData.InitialBait;
            ret[0] = new BaitOrder()
            {
                Icon = Service<Icons>.Get()[bait.Data.Icon],
                Name = bait.Name[GatherBuddy.Language],
                Fish = null,
            };
            for (var idx = 0; idx < fish.CatchData.Mooches.Length; ++idx)
            {
                var f    = fish.CatchData.Mooches[idx];
                var hook = f.CatchData?.HookSet ?? HookSet.Unknown;
                ret[idx].HookSet = FromHookSet(cache, hook);
                ret[idx].Bite    = FromBiteType(f.CatchData?.BiteType ?? BiteType.Unknown);
                ret[idx + 1] = new BaitOrder()
                {
                    Icon = Service<Icons>.Get()[f.ItemData.Icon],
                    Name = f.Name[GatherBuddy.Language],
                    Fish = f,
                };
            }

            ret[ret.Length - 1].HookSet = FromHookSet(cache, fish.CatchData?.HookSet ?? HookSet.Unknown);
            ret[ret.Length - 1].Bite    = FromBiteType(fish.CatchData?.BiteType ?? BiteType.Unknown);
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

        private static bool SetUptimeDependency(Game.Fish f, BaitOrder[] bait)
        {
            if (bait.Any(b => (b.Fish?.FishRestrictions ?? FishRestrictions.None) != FishRestrictions.None))
                return true;

            return f.CatchData?.Predator.Any(p => p.Item1.FishRestrictions != FishRestrictions.None) ?? false;
        }

        public Fish(FishTab cache, FishManager manager, Game.Fish fish)
        {
            ushort uptime = 10000;
            Base                = fish;
            Icon                = Service<Icons>.Get()[fish.ItemData.Icon];
            Name                = fish.Name[GatherBuddy.Language];
            NameLower           = Name.ToLowerInvariant();
            Time                = SetTime(fish, ref uptime);
            WeatherIcons        = SetWeather(fish, ref uptime);
            Bait                = SetBait(cache, fish);
            FirstBaitLower      = Bait.Length > 0 ? Bait[0].Name.ToLowerInvariant() : "unknown bait";
            Predators           = SetPredators(manager, fish);
            Territory           = fish.FishingSpots.First().Territory!.Name[GatherBuddy.Language];
            TerritoryLower      = Territory.ToLowerInvariant();
            FishingSpot         = fish.FishingSpots.First().PlaceName![GatherBuddy.Language];
            FishingSpotLower    = FishingSpot.ToLowerInvariant();
            Snagging            = SetSnagging(cache, Bait, fish);
            Patch               = $"Patch {fish.CatchData?.Patch.ToVersionString() ?? "???"}";
            UptimeString        = $"{(uptime / 100f).ToString("F1", CultureInfo.InvariantCulture)}%%";
            HasUptimeDependency = SetUptimeDependency(fish, Bait);
            var intuition = fish.CatchData?.IntuitionLength ?? 0;
            if (intuition > 0)
            {
                var minutes = intuition / RealTime.SecondsPerMinute;
                var seconds = intuition % RealTime.SecondsPerMinute;
                if (seconds == 0)
                    IntuitionText = minutes == 1 ? $"Intuition for {minutes} Minute" : $"Intuition for {minutes} Minutes";
                else
                    IntuitionText = $"Intuition for {minutes}:{seconds:D2} Minutes";
            }
        }
    }
}
