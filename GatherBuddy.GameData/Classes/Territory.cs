using System;
using System.Collections.Generic;
using GatherBuddy.Structs;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;
using TerritoryType = Lumina.Excel.GeneratedSheets.TerritoryType;

namespace GatherBuddy.Classes;

public class Territory : IComparable<Territory>, IEquatable<Territory>
{
    public static readonly Territory Invalid = new();

    public TerritoryType          Data         { get; }       = new();
    public string                 Name         { get; }       = string.Empty;
    public HashSet<Aetheryte>     Aetherytes   { get; }       = new();
    public CumulativeWeatherRates WeatherRates { get; init; } = CumulativeWeatherRates.InvalidWeather;
    public float                  SizeFactor   { get; init; }
    public ushort                 XStream      { get; init; }
    public ushort                 YStream      { get; init; }
    public ushort                 Plane        { get; init; }


    public uint Id
        => Data.RowId;

    public Territory(GameData gameData, TerritoryType data, TerritoryTypeTelepo? aether)
    {
        Data       = data;
        Name       = MultiString.ParseSeStringLumina(data.PlaceName.Value?.Name);
        SizeFactor = (data.Map.Value?.SizeFactor ?? 100f) / 100f;
        XStream    = aether?.Unknown0 ?? 0;
        YStream    = aether?.Unknown1 ?? 0;
        Plane      = aether?.Unknown2 ?? 0;

        WeatherRates = gameData.CumulativeWeatherRates.TryGetValue(data.WeatherRate, out var wr)
            ? wr
            : CumulativeWeatherRates.InvalidWeather;
    }

    private Territory()
    { }

    public override string ToString()
        => Name;

    public int CompareTo(Territory? other)
        => Id.CompareTo(other?.Id ?? 0);

    public bool Equals(Territory? other)
        => Id.Equals(other?.Id);


    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;

        return obj switch
        {
            Territory t => Equals(t),
            _           => false,
        };
    }

    public override int GetHashCode()
        => Id.GetHashCode();
}
