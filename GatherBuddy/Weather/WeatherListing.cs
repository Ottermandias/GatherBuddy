using GatherBuddy.Time;

namespace GatherBuddy.Weather;

public readonly struct WeatherListing
{
    public Structs.Weather Weather   { get; }
    public TimeStamp       Timestamp { get; }

    public WeatherListing(Structs.Weather weather, TimeStamp timeStamp)
    {
        Weather   = weather;
        Timestamp = timeStamp;
    }

    public static WeatherListing RoundedListing(Structs.Weather weather, TimeStamp inexactTimestamp)
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
