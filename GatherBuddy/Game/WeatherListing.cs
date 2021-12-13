using System;
using GatherBuddy.Classes;
using GatherBuddy.Managers;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Game;

public readonly struct WeatherListing
{
    public Weather   Weather   { get; }
    public TimeStamp Timestamp { get; }

    public WeatherListing(Weather weather, TimeStamp timeStamp)
    {
        Weather   = weather;
        Timestamp = timeStamp;
    }

    public static WeatherListing RoundedListing(Weather weather, TimeStamp inexactTimestamp)
        => new(weather, inexactTimestamp.SyncToEorzeaWeather());

    public TimeStamp End
        => Timestamp + EorzeaTimeStampExtensions.MillisecondsPerEorzeaWeather;

    public long Offset(TimeStamp timeStamp)
        => timeStamp - Timestamp;

    public TimeInterval Uptime
        => new()
        {
            Start = Timestamp,
            End   = End,
        };
}
