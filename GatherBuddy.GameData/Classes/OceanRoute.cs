using System;
using GatherBuddy.Enums;

namespace GatherBuddy.Classes;

public enum OceanArea : byte
{
    None,
    Unknown,
    Aldenard,
    Othard,
}

public class OceanRoute
{
    public string                                     Name       { get; internal init; } = string.Empty;
    public byte                                       Id         { get; internal init; }
    public OceanArea                                  Area       { get; internal init; }
    public OceanTime                                  StartTime  { get; internal init; }
    public (FishingSpot Normal, FishingSpot Spectral) SpotDay    { get; internal init; }
    public (FishingSpot Normal, FishingSpot Spectral) SpotSunset { get; internal init; }
    public (FishingSpot Normal, FishingSpot Spectral) SpotNight  { get; internal init; }

    public override string ToString()
        => $"{Name} ({StartTime})";

    public (FishingSpot Normal, FishingSpot Spectral) GetSpots(OceanTime time)
        => time switch
        {
            OceanTime.Day    => SpotDay,
            OceanTime.Sunset => SpotSunset,
            OceanTime.Night  => SpotNight,
            _                => throw new ArgumentOutOfRangeException(nameof(time)),
        };

    public (FishingSpot Normal, FishingSpot Spectral) GetSpots(int order)
        => (StartTime, order % 3) switch
        {
            (OceanTime.Day, 0)    => SpotSunset,
            (OceanTime.Day, 1)    => SpotNight,
            (OceanTime.Day, 2)    => SpotDay,
            (OceanTime.Sunset, 0) => SpotNight,
            (OceanTime.Sunset, 1) => SpotDay,
            (OceanTime.Sunset, 2) => SpotSunset,
            (OceanTime.Night, 0)  => SpotDay,
            (OceanTime.Night, 1)  => SpotSunset,
            (OceanTime.Night, 2)  => SpotNight,
            _                     => throw new ArgumentOutOfRangeException(nameof(StartTime)),
        };

    public FishingSpot GetSpot(OceanTime time, bool spectral)
    {
        var (main, sub) = GetSpots(time);
        return spectral ? sub : main;
    }

    public FishingSpot GetSpot(int order, bool spectral)
    {
        var (main, sub) = GetSpots(order);
        return spectral ? sub : main;
    }
}
