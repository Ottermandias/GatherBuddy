using System;
using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
using ItemRow = Lumina.Excel.GeneratedSheets.Item;
using FishRow = Lumina.Excel.GeneratedSheets.FishParameter;
using SpearFishRow = Lumina.Excel.GeneratedSheets.SpearfishingItem;

namespace GatherBuddy.Game
{
    public class Fish : IComparable
    {
        public ItemRow ItemData { get; set; }

        public FishRow? FishData
            => _fishData as FishRow;

        public SpearFishRow? SpearFishData
            => _fishData as SpearFishRow;

        public HashSet<FishingSpot> FishingSpots { get; set; } = new();
        public FFName               Name         { get; set; }

        public uint ItemId
            => ItemData.RowId;

        public uint FishId
            => IsSpearFish ? SpearFishData!.RowId : FishData!.RowId;

        public bool InLog
            => IsSpearFish || FishData!.IsInLog;

        private readonly dynamic _fishData;

        public FishRecord Record { get; set; } = new();

        public CatchData? CatchData { get; set; }

        private RealUptime _nextUptime = RealUptime.Unknown;

        public GigHead Gig { get; set; } = GigHead.None;

        public bool IsSpearFish
            => Gig != GigHead.None;

        public bool IsBigFish
            => !IsSpearFish && FishData!.Unknown5;

        public bool             OceanFish        { get; set; }
        public FishRestrictions FishRestrictions { get; set; }

        public string Folklore
            => IsSpearFish ? string.Empty : FishData!.GatheringSubCategory.Value?.FolkloreBook ?? string.Empty;

        public Fish(SpearFishRow fishRow, GigHead gig, FFName name)
        {
            ItemData         = fishRow.Item.Value;
            _fishData        = fishRow;
            Name             = name;
            FishRestrictions = FishRestrictions.None;
            Gig              = gig;
        }

        public Fish(ItemRow item, FishRow fishRow, FFName name)
        {
            ItemData  = item;
            _fishData = fishRow;
            FishRestrictions = (fishRow.TimeRestricted ? FishRestrictions.Time : FishRestrictions.None)
              | (fishRow.WeatherRestricted ? FishRestrictions.Weather : FishRestrictions.None);
            Name      = name;
            OceanFish = fishRow.TerritoryType.Row == 900;
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
                return 1;

            var rhs = obj as Fish;
            return ItemId.CompareTo(rhs?.ItemId ?? 0);
        }

        public RealUptime NextUptime(WeatherManager weather, Territory? territory)
        {
            // Always or predator-based
            if (FishRestrictions == FishRestrictions.None)
            {
                if (CatchData != null && CatchData.Predator.Length > 0)
                    return CatchData.Predator.Select(f => f.Item1.NextUptime(weather, territory)).ArgMin(t => t.Time);
                return RealUptime.Always;
            }

            // Unknown
            if (CatchData == null
             || FishRestrictions.HasFlag(FishRestrictions.Time) && CatchData.Hours.AlwaysUp()
             || FishRestrictions.HasFlag(FishRestrictions.Weather)
             && CatchData.PreviousWeather.Length == 0
             && CatchData.CurrentWeather.Length == 0)
                return RealUptime.Unknown;

            
            // If home territory is requested
            if (territory == null || territory.Id == FishingSpots.First().Territory!.Id)
            {
                // Update cache if necessary
                if (_nextUptime.EndTime <= DateTime.UtcNow)
                    UpdateUptime(weather);

                // Cache valid
                return _nextUptime;
            }

            // If another territory is requested
            return GetUptime(weather, territory);
        }

        public RealUptime NextUptime(WeatherManager weather)
            => NextUptime(weather, FishingSpots.First().Territory!);


        private RealUptime GetUptime(WeatherManager weather, Territory territory)
        {
            if (FishRestrictions == FishRestrictions.Time)
                return CatchData!.Hours.NextRealUptime();

            var wl = weather.RequestForecast(FishingSpots.First().Territory!, CatchData!.CurrentWeather, CatchData.PreviousWeather,
                CatchData.Hours);

            var overlap   = wl.Uptime.Overlap(CatchData!.Hours);
            var offset    = overlap.FirstHour - wl.Uptime.FirstHour;
            var startTime = wl.Time.AddSeconds(EorzeaTime.SecondsPerEorzeaHour * offset);
            var duration  = TimeSpan.FromSeconds(EorzeaTime.SecondsPerEorzeaHour * (int) overlap.Count);

            bool valid;
            do
            {
                var endTime   = startTime + duration;
                var timestamp = (long) (endTime - DateTime.UtcNow).TotalSeconds + 1;
                wl = weather.RequestForecast(territory, CatchData!.CurrentWeather, CatchData.PreviousWeather,
                    CatchData.Hours, timestamp);
                var newOverlap = wl.Uptime.Overlap(CatchData!.Hours);
                if (wl.Time <= endTime && newOverlap.FirstHour == overlap.EndHour % RealTime.HoursPerDay)
                {
                    duration += TimeSpan.FromSeconds(EorzeaTime.SecondsPerEorzeaHour * (int) newOverlap.Count);
                    overlap  =  newOverlap;
                    valid    =  true;
                }
                else
                {
                    valid = false;
                }
            } while (valid);

            return new RealUptime(startTime, duration);
        }

        private void UpdateUptime(WeatherManager weather)
            => _nextUptime = GetUptime(weather, FishingSpots.First().Territory!);

        public Fish(Fish fish)
        {
            ItemData         = fish.ItemData;
            FishingSpots     = fish.FishingSpots;
            Name             = fish.Name;
            _fishData        = fish._fishData;
            Gig              = fish.Gig;
            OceanFish        = fish.OceanFish;
            FishRestrictions = fish.FishRestrictions;
            Record           = fish.Record;
            CatchData        = fish.CatchData;
            _nextUptime      = fish._nextUptime;
        }
    }
}
