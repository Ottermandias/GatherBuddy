using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dalamud;
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

        public Game.Fish          Base                   { get; }
        public TextureWrap        Icon                   { get; }
        public string             Name                   { get; }
        public string             NameLower              { get; }
        public string             Time                   { get; }
        public TextureWrap[][]    WeatherIcons           { get; }
        public BaitOrder[]        Bait                   { get; }
        public string             FirstBaitLower         { get; }
        public string             Territory              { get; }
        public string             TerritoryLower         { get; }
        public string             FishingSpot            { get; }
        public (string, string)[] AdditionalFishingSpots { get; }
        public string             FishingSpotTcAddress   { get; }
        public string             FishingSpotLower       { get; }
        public TextureWrap?       Snagging               { get; }
        public Predator[]         Predators              { get; }
        public string             Patch                  { get; }
        public string             UptimeString           { get; }
        public string             IntuitionText          { get; } = "";
        public bool               HasUptimeDependency    { get; }
        public bool               IsFixed                { get; set; } = false;

        public string FishingSpotTooltip { get; }
        public string TerritoryTooltip   { get; }

        private static string SetTime(Game.Fish fish, ref ushort uptime)
        {
            if (!fish.FishRestrictions.HasFlag(FishRestrictions.Time))
                return "Always Up";

            if (fish.CatchData?.Minutes.AlwaysUp() ?? true)
            {
                uptime = 0;
                return "Unknown Uptime";
            }

            uptime = (ushort) (uptime * fish.CatchData!.Minutes.Duration / RealTime.MinutesPerDay);
            return fish.CatchData!.Minutes.PrintHours();
        }

        private static TextureWrap[][] SetWeather(Game.Fish fish, ref ushort uptime)
        {
            if (!fish.FishRestrictions.HasFlag(FishRestrictions.Weather))
                return new TextureWrap[0][];

            if (fish.CatchData == null || fish.CatchData.PreviousWeather.Length == 0 && fish.CatchData.CurrentWeather.Length == 0)
            {
                uptime = 0;
                return Array.Empty<TextureWrap[]>();
            }

            var icons      = Service<Icons>.Get();
            var sheet      = Dalamud.GameData.GetExcelSheet<Lumina.Excel.GeneratedSheets.Weather>()!;
            var skyWatcher = Service<SkyWatcher>.Get();

            var previousChance = skyWatcher.ChanceForWeather(fish.FishingSpots.First().Territory!.Id, fish.CatchData.PreviousWeather);
            var currentChance  = skyWatcher.ChanceForWeather(fish.FishingSpots.First().Territory!.Id, fish.CatchData.CurrentWeather);
            if (previousChance == 0)
                previousChance = 100;
            if (currentChance == 0)
                currentChance = 100;

            uptime = (ushort) (uptime * currentChance * previousChance / 10000);

            return new[]
            {
                fish.CatchData.PreviousWeather.Select(w => icons[sheet.GetRow(w)!.Icon]).ToArray(),
                fish.CatchData.CurrentWeather.Select(w => icons[sheet.GetRow(w)!.Icon]).ToArray(),
            };
        }

        private static Predator[] SetPredators(Game.Fish fish)
        {
            if (fish.CatchData == null || fish.CatchData.Predator.Length == 0)
                return Array.Empty<Predator>();

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
                return new BaitOrder[]
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

            ret[^1].HookSet = FromHookSet(cache, fish.CatchData?.HookSet ?? HookSet.Unknown);
            ret[^1].Bite    = FromBiteType(fish.CatchData?.BiteType ?? BiteType.Unknown);
            return ret;
        }

        private static TextureWrap? SetSnagging(FishTab cache, IEnumerable<BaitOrder> baitOrder, Game.Fish fish)
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

        private static string GetFishingSpotTcAddress(uint fishingSpotId)
        {
            var lang = GatherBuddy.Language switch
            {
                ClientLanguage.English  => "en",
                ClientLanguage.German   => "de",
                ClientLanguage.French   => "fr",
                ClientLanguage.Japanese => "ja",
                _                       => "en",
            };
            var s = $"https://ffxivteamcraft.com/db/{lang}/fishing-spot/{fishingSpotId}";
            return s;
        }

        public Fish(FishTab cache, Game.Fish fish)
        {
            ushort uptime = 10000;
            Base           = fish;
            Icon           = Service<Icons>.Get()[fish.ItemData.Icon];
            Name           = fish.Name[GatherBuddy.Language];
            NameLower      = Name.ToLowerInvariant();
            Time           = SetTime(fish, ref uptime);
            WeatherIcons   = SetWeather(fish, ref uptime);
            Bait           = SetBait(cache, fish);
            FirstBaitLower = Bait.Length > 0 ? Bait[0].Name.ToLowerInvariant() : "unknown bait";
            Predators      = SetPredators(fish);
            Territory      = fish.FishingSpots.First().Territory!.Name[GatherBuddy.Language];
            FishingSpot    = fish.FishingSpots.First().PlaceName![GatherBuddy.Language];
            AdditionalFishingSpots = fish.FishingSpots.Count > 1
                ? fish.FishingSpots.Select(s => (s.PlaceName![GatherBuddy.Language], s.Territory!.Name[GatherBuddy.Language])).ToArray()
                : Array.Empty<(string, string)>();
            var tmpTerritories = AdditionalFishingSpots.Select(s => s.Item2).Prepend(Territory).Distinct().ToArray();
            TerritoryLower       = string.Join('\0', tmpTerritories.Select(s => s.ToLowerInvariant()));
            FishingSpotTcAddress = GetFishingSpotTcAddress(fish.FishingSpots.First().Id);
            FishingSpotLower     = FishingSpot.ToLowerInvariant();
            if (AdditionalFishingSpots.Any())
                FishingSpotLower += '\0' + string.Join('\0', AdditionalFishingSpots.Select(s => s.Item1.ToLowerInvariant()));
            Snagging            = SetSnagging(cache, Bait, fish);
            Patch               = $"Patch {fish.CatchData?.Patch.ToVersionString() ?? "???"}";
            UptimeString        = $"{(uptime / 100f).ToString("F1", CultureInfo.InvariantCulture)}%%";
            HasUptimeDependency = SetUptimeDependency(fish, Bait);

            FishingSpotTooltip = $"{Territory}\nRight-click to open TeamCraft site for this spot.";
            if (AdditionalFishingSpots.Any())
                FishingSpotTooltip += $"\nAdditional Fishing Spots:\n\t\t{string.Join("\n\t\t", AdditionalFishingSpots.Select(s => $"{s.Item1} ({s.Item2})"))}";

            TerritoryTooltip = tmpTerritories.Length > 1 ? $"Additional Zones:\n\t\t{string.Join("\n\t\t", tmpTerritories)}" : string.Empty;

            var intuition = fish.CatchData?.IntuitionLength ?? 0;
            if (intuition <= 0)
                return;

            var minutes = intuition / RealTime.SecondsPerMinute;
            var seconds = intuition % RealTime.SecondsPerMinute;
            if (seconds == 0)
                IntuitionText = minutes == 1 ? $"Intuition for {minutes} Minute" : $"Intuition for {minutes} Minutes";
            else
                IntuitionText = $"Intuition for {minutes}:{seconds:D2} Minutes";
        }
    }
}
