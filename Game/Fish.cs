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

namespace GatherBuddy.Game;

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

    private TimeInterval _nextUptime = TimeInterval.Invalid;

    private GigHead _gig = GigHead.None;

    public GigHead Gig
    {
        get => _gig != GigHead.Unknown ? _gig : CatchData?.GigHead ?? _gig;
        set => _gig = value;
    }

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
        ItemData         = fishRow.Item.Value!;
        _fishData        = fishRow;
        Name             = name;
        FishRestrictions = FishRestrictions.None;
        _gig             = gig;
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

    public TimeInterval NextUptime(WeatherManager weather, Territory? territory, out bool cacheUpdated)
    {
        cacheUpdated = false;
        // Always up
        if (FishRestrictions == FishRestrictions.None)
            return TimeInterval.Always;

        // Unknown
        if (CatchData == null
         || FishRestrictions.HasFlag(FishRestrictions.Time) && CatchData.Interval.AlwaysUp()
         || FishRestrictions.HasFlag(FishRestrictions.Weather)
         && CatchData.PreviousWeather.Length == 0
         && CatchData.CurrentWeather.Length == 0)
            return TimeInterval.Invalid;


        // If different from home territory is requested
        if (territory != null && territory.Id != FishingSpots.First().Territory!.Id)
        {
            cacheUpdated = true;
            return GetUptime(weather, territory);
        }

        // Cache valid
        if (_nextUptime.End > TimeStamp.UtcNow)
            return _nextUptime;

        // Update cache if necessary
        cacheUpdated = true;
        UpdateUptime(weather);
        return _nextUptime;
    }

    public TimeInterval NextUptime(WeatherManager weather)
        => NextUptime(weather, FishingSpots.First().Territory!, out _);

    public TimeInterval NextUptime(WeatherManager weather, Territory? territory)
        => NextUptime(weather, territory, out _);

    public TimeInterval NextUptime(WeatherManager weather, out bool cacheUpdated)
        => NextUptime(weather, FishingSpots.First().Territory!, out cacheUpdated);

    private TimeInterval GetUptime(WeatherManager weather, Territory territory)
    {
        if (FishRestrictions == FishRestrictions.Time)
            return CatchData!.Interval.NextRealUptime();

        var wl      = weather.RequestForecast(territory, CatchData!.CurrentWeather, CatchData.PreviousWeather, CatchData.Interval);
        var end     = weather.ExtendedDuration(territory, CatchData.CurrentWeather, CatchData.PreviousWeather, wl);
        var overlap = new TimeInterval(wl.Timestamp, end).FirstOverlap(CatchData!.Interval);
        return overlap;
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
