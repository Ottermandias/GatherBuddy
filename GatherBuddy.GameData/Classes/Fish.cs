using System;
using System.Collections.Generic;
using Dalamud.Plugin.Services;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Utility;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using ItemRow = Lumina.Excel.GeneratedSheets.Item;
using FishRow = Lumina.Excel.GeneratedSheets.FishParameter;
using SpearFishRow = Lumina.Excel.GeneratedSheets.SpearfishingItem;
using FFXIVClientStructs.FFXIV.Client.Game;

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
        => BigFishOverride.Value ?? FishData?.IsHidden ?? false;

    public OceanArea OceanArea { get; internal set; } = OceanArea.None;

    public bool OceanFish
        => OceanArea is not OceanArea.None;

    public FishRestrictions FishRestrictions { get; set; }

    public string Folklore { get; init; }

    public uint Quantity { get; set; } = 1;
    public unsafe int InventoryCount
    {
        get
        {
            var inventory = InventoryManager.Instance();
            return inventory->GetInventoryItemCount(ItemId);
        }
    }

    public Fish(IDataManager gameData, SpearFishRow fishRow, ExcelSheet<FishingNoteInfo> catchData)
    {
        ItemData         = fishRow.Item.Value ?? new ItemRow();
        _fishData        = fishRow;
        Name             = MultiString.FromItem(gameData, ItemData.RowId);
        var note = catchData.GetRow(fishRow.RowId + 20000);
        FishRestrictions = note is { TimeRestriction: 1 } ? FishRestrictions.Time : FishRestrictions.None;
        Folklore         = string.Empty;
        Size             = SpearfishSize.Unknown;
        Speed            = SpearfishSpeed.Unknown;
        BiteType         = BiteType.None;
        Snagging         = Snagging.None;
        HookSet          = HookSet.None;
    }

    public Fish(IDataManager gameData, FishRow fishRow, ExcelSheet<FishingNoteInfo> catchData)
    {
        ItemData  = gameData.GetExcelSheet<ItemRow>()?.GetRow((uint)fishRow.Item) ?? new Item();
        _fishData = fishRow;
        var note = catchData.GetRow(fishRow.RowId);
        FishRestrictions = (note is { TimeRestriction: 1 } ? FishRestrictions.Time : FishRestrictions.None)
          | (note is { WeatherRestriction: 1 } ? FishRestrictions.Weather : FishRestrictions.None);
        Name      = MultiString.FromItem(gameData, ItemData.RowId);
        Folklore  = MultiString.ParseSeStringLumina(fishRow.GatheringSubCategory.Value?.FolkloreBook);
        Size      = SpearfishSize.None;
        Speed     = SpearfishSpeed.None;
        BiteType  = BiteType.Unknown;
        Snagging  = Snagging.Unknown;
        HookSet   = HookSet.Unknown;
    }

    public int CompareTo(Fish? obj)
        => ItemId.CompareTo(obj?.ItemId ?? 0);
}
