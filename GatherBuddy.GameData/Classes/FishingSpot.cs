using System;
using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Interfaces;
using GatherBuddy.Structs;
using GatherBuddy.Utility;
using Lumina.Excel.Sheets;
using GatheringType = GatherBuddy.Enums.GatheringType;
using FishingSpotRow = Lumina.Excel.Sheets.FishingSpot;

namespace GatherBuddy.Classes;

public class FishingSpot : IComparable<FishingSpot>, ILocation
{
    public const uint SpearfishingIdOffset = 1u << 31;

    private readonly object _data;

    public SpearfishingNotebook? SpearfishingSpotData
        => _data is SpearfishingNotebook n ? n : null;

    public FishingSpotRow? FishingSpotData
        => _data is FishingSpotRow f ? f : null;

    public Territory Territory { get; init; }
    public string    Name      { get; init; }
    public Fish[]    Items     { get; init; }

    public GatheringType GatheringType
        => Spearfishing ? GatheringType.Spearfishing : GatheringType.Fisher;

    public IEnumerable<IGatherable> Gatherables
        => Items;

    public uint SheetId
        => _data is SpearfishingNotebook sf
            ? sf.RowId
            : ((FishingSpotRow)_data).RowId;

    public uint Id
        => _data is SpearfishingNotebook sf
            ? sf.RowId | SpearfishingIdOffset
            : ((FishingSpotRow)_data).RowId;

    public ObjectType Type
        => ObjectType.Fish;

    public Aetheryte? ClosestAetheryte { get; set; }
    public int        IntegralXCoord   { get; set; }
    public int        IntegralYCoord   { get; set; }

    public Aetheryte? DefaultAetheryte { get; internal set; }
    public int        DefaultXCoord    { get; internal set; }
    public int        DefaultYCoord    { get; internal set; }

    public ushort Radius        { get; set; }
    public ushort DefaultRadius { get; internal set; }


    public WaymarkSet Markers { get; set; } = WaymarkSet.None;

    public bool Spearfishing
        => _data is SpearfishingNotebook;

    public int CompareTo(FishingSpot? obj)
        => SheetId.CompareTo(obj?.SheetId ?? 0);

    public FishingSpot(GameData data, FishingSpotRow spot)
    {
        _data          = spot;
        Territory      = data.FindOrAddTerritory(FishingSpotTerritoryHacks(data, spot)) ?? Territory.Invalid;
        Name           = FishingSpotNameHacks(data, spot);
        IntegralXCoord = Maps.MarkerToMap(spot.X, Territory.SizeFactor);
        IntegralYCoord = Maps.MarkerToMap(spot.Z, Territory.SizeFactor);
        ClosestAetheryte = Territory.Aetherytes.Count > 0
            ? Territory.Aetherytes.ArgMin(a => a.WorldDistance(Territory.Id, IntegralXCoord, IntegralYCoord))
            : null;
        Radius           = (ushort)(spot.Radius / 7);
        DefaultXCoord    = IntegralXCoord;
        DefaultYCoord    = IntegralYCoord;
        DefaultAetheryte = ClosestAetheryte;
        DefaultRadius    = Radius;

        Items = spot.Item.Where(i => i.RowId > 0)
            .Select(i => data.Fishes.GetValueOrDefault(i.RowId))
            .Where(f => f != null).Cast<Fish>()
            .ToArray();
        foreach (var item in Items)
            item.FishingSpots.Add(this);
    }

    public FishingSpot(GameData data, SpearfishingNotebook spot)
    {
        _data     = spot;
        Territory = data.FindOrAddTerritory(spot.TerritoryType.Value) ?? Territory.Invalid;
        Name      = MultiString.ParseSeStringLumina(spot.PlaceName.ValueNullable?.Name);

        IntegralXCoord = Maps.MarkerToMap(spot.X, Territory.SizeFactor);
        IntegralYCoord = Maps.MarkerToMap(spot.Y, Territory.SizeFactor);
        ClosestAetheryte = Territory.Aetherytes.Count > 0
            ? Territory.Aetherytes.ArgMin(a => a.WorldDistance(Territory.Id, IntegralXCoord, IntegralYCoord))
            : null;
        Radius = (ushort)(spot.Radius / 7);

        DefaultXCoord    = IntegralXCoord;
        DefaultYCoord    = IntegralYCoord;
        DefaultAetheryte = ClosestAetheryte;
        DefaultRadius    = Radius;

        Items = spot.GatheringPointBase.ValueNullable?.Item.Where(i => i.RowId > 0)
                .Select(i => data.Fishes.Values.FirstOrDefault(f => f.IsSpearFish && f.FishId == i.RowId))
                .Where(f => f != null).Cast<Fish>()
                .ToArray()
         ?? [];
        foreach (var item in Items)
            item.FishingSpots.Add(this);
    }

    private static TerritoryType? FishingSpotTerritoryHacks(GameData data, FishingSpotRow spot)
        => spot.RowId switch
        {
            10_000 => data.DataManager.GetExcelSheet<TerritoryType>().GetRow(759), // the rows in between are no longer used diadem objects
            > 10_016 and < 10_026 => data.DataManager.GetExcelSheet<TerritoryType>().GetRow(939),
            > 10_025 and < 10_031 => data.DataManager.GetExcelSheet<TerritoryType>().GetRow(1073),

            // Sinus Ardorum
            > 10_030 and < 10_040 => data.DataManager.GetExcelSheet<TerritoryType>().GetRow(1237),
            > 10_042 and < 10_094 => data.DataManager.GetExcelSheet<TerritoryType>().GetRow(1237),
            10_096                => data.DataManager.GetExcelSheet<TerritoryType>().GetRow(1237),

            // Phaenna
            > 10_107 and < 10_136 => data.DataManager.GetExcelSheet<TerritoryType>().GetRow(1291),
            > 10_136 and < 10_145 => data.DataManager.GetExcelSheet<TerritoryType>().GetRow(1291),
            > 10_146 and < 10_156 => data.DataManager.GetExcelSheet<TerritoryType>().GetRow(1291),

            // Oizys
            > 10_162 and < 10_193 => data.DataManager.GetExcelSheet<TerritoryType>().GetRow(1310),

            _ => spot.TerritoryType.ValueNullable,
        };

    private static string FishingSpotNameHacks(GameData data, FishingSpotRow spot)
    {
        var name = MultiString.ParseSeStringLumina(spot.PlaceName.ValueNullable?.Name);
        switch (spot.RowId)
        {
            case > 10_042 and < 10_094:
            {
                var missionRow = spot.RowId - 10_043 + 451;
                if (missionRow > 495)
                {
                    if (missionRow > 499)
                        missionRow += 42;
                    else
                        missionRow += 12;
                }

                return name + $" ({missionRow:D5})";
            }
            case 10_096:                return $"{name} (00544)";
            case > 10_107 and < 10_136: return $"{name} ({spot.RowId - 10_108 + 965:D5})";
            case > 10_136 and < 10_145: return $"{name} ({spot.RowId - 10_137 + 993:D5})";
            case > 10_146 and < 10_153: return $"{name} ({spot.RowId - 10_147 + 1001:D5})";
            case > 10_152 and < 10_156: return $"{name} ({spot.RowId - 10_153 + 1037:D5})";

            case > 10_162 and < 10_191: return $"{name} ({spot.RowId - 10_163 + 1320:D5})";
            case > 10_190 and < 10_193: return $"{name} ({spot.RowId - 10_191 + 1368:D5})";
        }

        return name;
    }
}
