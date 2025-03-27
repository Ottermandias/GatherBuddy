using System;
using System.Collections.Generic;
using GatherBuddy.Structs;
using GatherBuddy.Utility;
using Lumina.Excel.Sheets;
using TerritoryType = Lumina.Excel.Sheets.TerritoryType;

namespace GatherBuddy.Classes;

public class Territory : IComparable<Territory>, IEquatable<Territory>
{
    public static readonly Territory Invalid = new();

    public TerritoryType          Data         { get; }
    public string                 Name         { get; }       = string.Empty;
    public HashSet<Aetheryte>     Aetherytes   { get; }       = [];
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
        Name       = MultiString.ParseSeStringLumina(data.PlaceName.ValueNullable?.Name);
        SizeFactor = (data.Map.ValueNullable?.SizeFactor ?? 100f) / 100f;
        XStream    = aether?.X ?? 0;
        YStream    = aether?.Y ?? 0;
        Plane      = (ushort) (aether?.Relay.RowId ?? 0);

        WeatherRates = gameData.CumulativeWeatherRates.TryGetValue((byte) data.WeatherRate.RowId, out var wr)
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
