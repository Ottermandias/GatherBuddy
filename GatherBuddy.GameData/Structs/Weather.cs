using System;
using GatherBuddy.Utility;
using LuminaWeather = Lumina.Excel.Sheets.Weather;

namespace GatherBuddy.Structs;

public readonly struct Weather : IComparable<Weather>
{
    public readonly string Name;
    public readonly uint   Id;
    public readonly int    Icon;

    public static readonly Weather Invalid = new(0, "Invalid");

    public override string ToString()
        => Name;

    public int CompareTo(Weather other)
        => Id.CompareTo(other.Id);

    public Weather(in LuminaWeather weather)
    {
        Id   = weather.RowId;
        Icon = weather.Icon;
        Name = MultiString.ParseSeStringLumina(weather.Name);
    }

    private Weather(uint id, string name)
    {
        Id   = id;
        Icon = 0;
        Name = name;
    }
}
