using System;
using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Interfaces;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;
using GatheringType = GatherBuddy.Enums.GatheringType;

namespace GatherBuddy.Classes;

public class FishingSpot : IComparable<FishingSpot>, ILocation
{
    public const uint SpearfishingIdOffset = 1u << 31;

    private readonly object _data;

    public SpearfishingNotebook? SpearfishingSpotData
        => _data as SpearfishingNotebook;

    public Lumina.Excel.GeneratedSheets.FishingSpot? FishingSpotData
        => _data as Lumina.Excel.GeneratedSheets.FishingSpot;

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
            : ((Lumina.Excel.GeneratedSheets.FishingSpot)_data).RowId;

    public uint Id
        => _data is SpearfishingNotebook sf
            ? sf.RowId | SpearfishingIdOffset
            : ((Lumina.Excel.GeneratedSheets.FishingSpot)_data).RowId;

    public ObjectType Type
        => ObjectType.Fish;

    public Aetheryte? ClosestAetheryte { get; set; }
    public int        IntegralXCoord   { get; set; }
    public int        IntegralYCoord   { get; set; }

    public Aetheryte? DefaultAetheryte { get; internal set; }
    public int        DefaultXCoord    { get; internal set; }
    public int        DefaultYCoord    { get; internal set; }

    public bool Spearfishing
        => _data is SpearfishingNotebook;

    public int CompareTo(FishingSpot? obj)
        => SheetId.CompareTo(obj?.SheetId ?? 0);

    public FishingSpot(GameData data, Lumina.Excel.GeneratedSheets.FishingSpot spot)
    {
        _data     = spot;
        Territory = data.FindOrAddTerritory(spot.TerritoryType.Value) ?? Territory.Invalid;
        Name      = MultiString.ParseSeStringLumina(spot.PlaceName.Value?.Name);

        IntegralXCoord = Maps.MarkerToMap(spot.X, Territory.SizeFactor);
        IntegralYCoord = Maps.MarkerToMap(spot.Z, Territory.SizeFactor);
        ClosestAetheryte = Territory.Aetherytes.Count > 0
            ? Territory.Aetherytes.ArgMin(a => a.WorldDistance(Territory.Id, IntegralXCoord, IntegralYCoord))
            : null;

        DefaultXCoord    = IntegralXCoord;
        DefaultYCoord    = IntegralYCoord;
        DefaultAetheryte = ClosestAetheryte;

        Items = spot.Item.Where(i => i.Row > 0)
            .Select(i => data.Fishes.TryGetValue(i.Row, out var fish) ? fish : null)
            .Where(f => f != null).Cast<Fish>()
            .ToArray();
        foreach (var item in Items)
            item.FishingSpots.Add(this);
    }

    public FishingSpot(GameData data, SpearfishingNotebook spot)
    {
        _data     = spot;
        Territory = data.FindOrAddTerritory(spot.TerritoryType.Value) ?? Territory.Invalid;
        Name      = MultiString.ParseSeStringLumina(spot.PlaceName.Value?.Name);

        IntegralXCoord = Maps.MarkerToMap(spot.X, Territory.SizeFactor);
        IntegralYCoord = Maps.MarkerToMap(spot.Y, Territory.SizeFactor);
        ClosestAetheryte = Territory.Aetherytes.Count > 0
            ? Territory.Aetherytes.ArgMin(a => a.WorldDistance(Territory.Id, IntegralXCoord, IntegralYCoord))
            : null;

        DefaultXCoord    = IntegralXCoord;
        DefaultYCoord    = IntegralYCoord;
        DefaultAetheryte = ClosestAetheryte;

        Items = spot.GatheringPointBase.Value?.Item.Where(i => i > 0)
                .Select(i => data.Fishes.Values.FirstOrDefault(f => f.IsSpearFish && f.FishId == i))
                .Where(f => f != null).Cast<Fish>()
                .ToArray()
         ?? Array.Empty<Fish>();
        foreach (var item in Items)
            item.FishingSpots.Add(this);
    }
}
