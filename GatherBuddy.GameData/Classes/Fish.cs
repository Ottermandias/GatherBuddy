using System;
using System.Collections.Generic;
using Dalamud.Data;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;
using ItemRow = Lumina.Excel.GeneratedSheets.Item;
using FishRow = Lumina.Excel.GeneratedSheets.FishParameter;
using SpearFishRow = Lumina.Excel.GeneratedSheets.SpearfishingItem;

namespace GatherBuddy.Classes;

public partial class Fish : IComparable<Fish>, IGatherable
{
    public ItemRow ItemData { get; init; }

    private readonly object _fishData;

    public FishRow? FishData
        => _fishData as FishRow;

    public SpearFishRow? SpearfishData
        => _fishData as SpearFishRow;

    public IList<FishingSpot> FishingSpots { get; init; } = new List<FishingSpot>();
    public MultiString          Name         { get; init; }

    public IEnumerable<ILocation> Locations
        => FishingSpots;

    public int InternalLocationId { get; internal set; } = 0;

    public uint ItemId
        => ItemData.RowId;

    public ObjectType Type
        => ObjectType.Fish;

    public uint FishId
        => FishData?.RowId ?? SpearfishData!.RowId;

    public bool InLog
        => IsSpearFish || FishData!.IsInLog;

    public bool IsSpearFish
        => _fishData is SpearFishRow;

    public bool IsBigFish
        => FishData?.Unknown5 ?? false;

    public bool OceanFish
        => (FishData?.TerritoryType.Row ?? 0) == 900;

    public FishRestrictions FishRestrictions { get; set; }

    public string Folklore { get; init; }

    public Fish(DataManager gameData, SpearFishRow fishRow)
    {
        ItemData         = fishRow.Item.Value ?? new ItemRow();
        _fishData        = fishRow;
        Name             = MultiString.FromItem(gameData, ItemData.RowId);
        FishRestrictions = FishRestrictions.None;
        Folklore         = string.Empty;
        Size             = SpearfishSize.Unknown;
        Speed            = SpearfishSpeed.Unknown;
        BiteType         = BiteType.None;
        Snagging         = Snagging.None;
        HookSet          = HookSet.None;
    }

    public Fish(DataManager gameData, FishRow fishRow)
    {
        ItemData  = gameData.GetExcelSheet<ItemRow>()?.GetRow((uint)fishRow.Item) ?? new Item();
        _fishData = fishRow;
        FishRestrictions = (fishRow.TimeRestricted ? FishRestrictions.Time : FishRestrictions.None)
          | (fishRow.WeatherRestricted ? FishRestrictions.Weather : FishRestrictions.None);
        Name     = MultiString.FromItem(gameData, ItemData.RowId);
        Folklore = MultiString.ParseSeStringLumina(fishRow.GatheringSubCategory.Value?.FolkloreBook);
        Size     = SpearfishSize.None;
        Speed    = SpearfishSpeed.None;
        BiteType = BiteType.Unknown;
        Snagging = Snagging.Unknown;
        HookSet  = HookSet.Unknown;
    }

    public int CompareTo(Fish? obj)
        => ItemId.CompareTo(obj?.ItemId ?? 0);
}
