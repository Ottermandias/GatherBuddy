using System;
using System.Diagnostics;
using System.Linq;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Structs;
using GatherBuddy.Time;

namespace GatherBuddy.Weather;

public partial class WeatherManager
{
    private static TimeStamp GetRootTime(TimeStamp timestamp)
        => timestamp.SyncToEorzeaWeather();

    private static byte CalculateTarget(TimeStamp timestamp)
    {
        var seconds     = timestamp.TotalSeconds;
        var hour        = seconds / EorzeaTimeStampExtensions.SecondsPerEorzeaHour;
        var shiftedHour = (uint)(hour + 8 - hour % 8) % RealTime.HoursPerDay;
        var day         = seconds / EorzeaTimeStampExtensions.SecondsPerEorzeaDay;

        var ret = (uint)day * 100 + shiftedHour;
        ret =  (ret << 11) ^ ret;
        ret =  (ret >> 8) ^ ret;
        ret %= 100;
        return (byte)ret;
    }

    private static Structs.Weather GetWeather(byte target, CumulativeWeatherRates rates)
    {
        Debug.Assert(target < 100, "Call error, target weather rate above 100.");
        foreach (var (w, r) in rates.Rates)
        {
            if (r > target)
                return w;
        }

#if DEBUG
        PluginLog.Warning($"Should never be reached, weather rates not adding up to 100. {rates.Rates.Last().CumulativeRate}");
#endif
        return rates.Rates[^1].Weather;
    }

    public static WeatherListing[] GetForecast(Territory territory, uint amount, TimeStamp timestamp)
    {
        if (amount == 0)
            return Array.Empty<WeatherListing>();

        if (territory.WeatherRates.Rates.Length == 0)
        {
            PluginLog.Error($"Trying to get forecast for territory {territory.Id} which has no weather rates.");
            return Array.Empty<WeatherListing>();
        }

        var ret  = new WeatherListing[amount];
        var root = GetRootTime(timestamp);
        for (var i = 0; i < amount; ++i)
        {
            var target  = CalculateTarget(root);
            var weather = GetWeather(target, territory.WeatherRates);
            ret[i] =  new WeatherListing(weather, root);
            root   += EorzeaTimeStampExtensions.MillisecondsPerEorzeaWeather;
        }

        return ret;
    }

    public static WeatherListing[] GetForecast(Territory territory, uint amount)
        => GetForecast(territory, amount, GatherBuddy.Time.ServerTime);

    public static WeatherListing GetForecast(Territory territory, TimeStamp timestamp)
        => GetForecast(territory, 1, timestamp)[0];

    public static WeatherListing[] GetForecastOffset(Territory territory, uint amount, long millisecondOffset)
        => GetForecast(territory, amount, GatherBuddy.Time.ServerTime.AddMilliseconds(millisecondOffset));
}
