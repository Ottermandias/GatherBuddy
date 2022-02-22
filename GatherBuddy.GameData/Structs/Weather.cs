using System;
using GatherBuddy.Utility;
using LuminaWeather = Lumina.Excel.GeneratedSheets.Weather;

namespace GatherBuddy.Structs;

public readonly struct Weather : IComparable<Weather>
{
    public readonly LuminaWeather Data;
    public readonly string        Name;

    public uint Id
        => Data.RowId;

    public static readonly Weather Invalid = new(new LuminaWeather());

    public Weather(LuminaWeather data)
    {
        Data = data;
        Name = MultiString.ParseSeStringLumina(data.Name);
    }

    public override string ToString()
        => Name;

    public int CompareTo(Weather other)
        => Id.CompareTo(other.Id);
}
