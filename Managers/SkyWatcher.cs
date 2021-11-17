using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dalamud;
using Dalamud.Data;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Game;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Managers;

public class SkyWatcher
{
    private readonly Dictionary<uint, (Weather Weather, byte CumulativeRate)[]> _weatherRates;

    public SkyWatcher(DataManager gameData, ClientLanguage clientLanguage)
    {
        var weathers    = gameData.GetExcelSheet<Weather>(clientLanguage)!;
        var rates       = gameData.GetExcelSheet<WeatherRate>()!;
        var territories = gameData.GetExcelSheet<TerritoryType>()!;

        _weatherRates = new Dictionary<uint, (Weather Weather, byte CumulativeRate)[]>((int)territories.RowCount);
        var weatherRates = new Dictionary<uint, (Weather Weather, byte CumulativeRate)[]>((int)rates.RowCount);

        foreach (var rate in rates)
        {
            var value = rate.UnkStruct0.Where(w => w.Rate > 0).Select(w => (weathers.GetRow((uint)w.Weather), w.Rate)).ToArray();

            var  bad            = false;
            byte cumulativeRate = 0;
            for (var i = 0; i < value.Length; ++i)
            {
                if (value[i].Item1 == null)
                {
                    PluginLog.Error($"Could not find Weather {rate.UnkStruct0[i].Weather} for WeatherRate {rate.RowId}.");
                    bad = true;
                    break;
                }

                cumulativeRate += value[i].Rate;
                value[i].Rate  =  cumulativeRate;
            }

            if (bad)
                continue;

            if (value.Last().Rate != 100)
            {
                PluginLog.Error($"Cumulative Rates for WeatherRate {rate.RowId} do not end up at 100.");
                continue;
            }

            weatherRates.Add(rate.RowId, value!);
        }

        foreach (var territory in territories)
        {
            if (!weatherRates.TryGetValue(territory.WeatherRate, out var value))
            {
                PluginLog.Error($"Could not find WeatherRate {territory.WeatherRate} for Territory {territory.RowId}.");
                continue;
            }

            _weatherRates.Add(territory.RowId, value);
        }
    }

    private static TimeStamp GetRootTime(TimeStamp timestamp)
        => timestamp.SyncToEorzeaWeather();

    private static byte CalculateTarget(TimeStamp timestamp)
    {
        var seconds     = timestamp.TotalSeconds;
        var hour        = seconds / EorzeaTimeStampExtensions.SecondsPerEorzeaHour;
        var shiftedHour = (uint)(hour + 8 - hour % 8) % RealTime.HoursPerDay;
        var day         = seconds / EorzeaTimeStampExtensions.SecondsPerEorzeaDay;

        var ret = (uint) day * 100 + shiftedHour;
        ret =  (ret << 11) ^ ret;
        ret =  (ret >> 8) ^ ret;
        ret %= 100;
        return (byte)ret;
    }

    private static Weather GetWeather(byte target, IList<(Weather, byte)> rates)
    {
        Debug.Assert(target < 100);
        foreach (var (w, r) in rates)
        {
            if (r > target)
                return w;
        }

        // Should never be reached.
        Debug.Assert(false);
        return rates[0].Item1;
    }

    public byte ChanceForWeather(uint territoryId, IEnumerable<uint> weatherId)
    {
        if (!_weatherRates.TryGetValue(territoryId, out var rate))
            return 0;

        var summedChance = 0;
        foreach (var id in weatherId)
        {
            for (var i = 0; i < rate.Length; ++i)
            {
                if (rate[i].Weather.RowId != id)
                    continue;

                if (i == 0)
                    summedChance += rate[i].CumulativeRate;
                else
                    summedChance += rate[i].CumulativeRate - rate[i - 1].CumulativeRate;
            }
        }

        return (byte)summedChance;
    }

    public byte ChanceForWeather(uint territoryId, params uint[] weatherIds)
        => ChanceForWeather(territoryId, (IEnumerable<uint>)weatherIds);

    public WeatherListing[] GetForecast(uint territoryId, uint amount, TimeStamp timestamp)
    {
        if (amount == 0)
            return Array.Empty<WeatherListing>();

        if (!_weatherRates.TryGetValue(territoryId, out var rates))
        {
            PluginLog.Error($"Trying to get forecast for unknown territory {territoryId}.");
            return Array.Empty<WeatherListing>();
        }

        var ret  = new WeatherListing[amount];
        var root = GetRootTime(timestamp);
        for (var i = 0; i < amount; ++i)
        {
            var target  = CalculateTarget(root);
            var weather = GetWeather(target, rates);
            ret[i] =  new WeatherListing(weather, root);
            root = root.AddEorzeaHours(8);
        }

        return ret;
    }

    public WeatherListing[] GetForecastOffset(uint territoryId, uint amount, long millisecondOffset)
        => GetForecast(territoryId, amount, TimeStamp.UtcNow.AddMilliseconds(millisecondOffset));

    public WeatherListing[] GetForecast(uint territoryId, uint amount)
        => GetForecast(territoryId, amount, TimeStamp.UtcNow);

    public WeatherListing GetForecast(uint territoryId)
        => GetForecast(territoryId, 1, TimeStamp.UtcNow)[0];

    public WeatherListing GetForecast(uint territoryId, TimeStamp timestamp)
        => GetForecast(territoryId, 1, timestamp)[0];

    public WeatherListing GetForecastOffset(uint territoryId, long millisecondOffset)
        => GetForecastOffset(territoryId, 1, millisecondOffset)[0];

    public (Weather, byte)[] GetRates(uint territoryId)
        => _weatherRates[territoryId];
}
