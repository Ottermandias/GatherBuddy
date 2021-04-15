using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dalamud.Plugin;
using GatherBuddy.Game;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Managers
{
    public class SkyWatcher
    {
        public const int SecondsPerWeather = 8 * EorzeaTime.SecondsPerEorzeaHour;

        private readonly Dictionary<uint, (Weather Weather, byte CumulativeRate)[]> _weatherRates;

        public SkyWatcher(DalamudPluginInterface pi)
        {
            var weathers    = pi.Data.GetExcelSheet<Weather>(pi.ClientState.ClientLanguage);
            var rates       = pi.Data.GetExcelSheet<WeatherRate>();
            var territories = pi.Data.GetExcelSheet<TerritoryType>();

            _weatherRates = new Dictionary<uint, (Weather Weather, byte CumulativeRate)[]>((int) territories.RowCount);
            var weatherRates = new Dictionary<uint, (Weather Weather, byte CumulativeRate)[]>((int) rates.RowCount);

            foreach (var rate in rates)
            {
                var value = rate.UnkStruct0.Where(w => w.Rate > 0).Select(w => (weathers.GetRow((uint) w.Weather), w.Rate)).ToArray();

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

                weatherRates.Add(rate.RowId, value);
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

        private static DateTime GetRootTime(DateTime fromWhen)
        {
            var seconds = (long) (fromWhen.ToUniversalTime() - RealTime.UnixEpoch).TotalSeconds % SecondsPerWeather;
            return fromWhen.AddTicks(-(fromWhen.Ticks % TimeSpan.TicksPerSecond) - seconds * TimeSpan.TicksPerSecond);
        }

        private static byte CalculateTarget(DateTime fromWhen)
        {
            var timeStamp   = (long) (fromWhen - RealTime.UnixEpoch).TotalSeconds;
            var hour        = timeStamp / EorzeaTime.SecondsPerEorzeaHour;
            var shiftedHour = (uint) (hour + 8 - hour % 8) % 24;
            var day         = (uint) timeStamp / EorzeaTime.SecondsPerEorzeaDay;

            var ret = day * 100 + shiftedHour;
            ret =  (ret << 11) ^ ret;
            ret =  (ret >> 8) ^ ret;
            ret %= 100;
            return (byte) ret;
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

        public WeatherListing[] GetForecast(uint territoryId, uint amount, DateTime fromWhen)
        {
            if (amount == 0)
                return new WeatherListing[0];

            if (!_weatherRates.TryGetValue(territoryId, out var rates))
            {
                PluginLog.Error($"Trying to get forecast for unknown territory {territoryId}.");
                return new WeatherListing[0];
            }

            var ret  = new WeatherListing[amount];
            var root = GetRootTime(fromWhen);
            for (var i = 0; i < amount; ++i)
            {
                var target  = CalculateTarget(root);
                var weather = GetWeather(target, rates);
                ret[i] = new WeatherListing(weather, root);
                root   = root.AddSeconds(SecondsPerWeather);
            }

            return ret;
        }

        public WeatherListing[] GetForecast(uint territoryId, uint amount, long secondOffset)
            => GetForecast(territoryId, amount, DateTime.UtcNow.AddSeconds(secondOffset));

        public WeatherListing[] GetForecast(uint territoryId, uint amount)
            => GetForecast(territoryId, amount, DateTime.UtcNow);

        public WeatherListing GetForecast(uint territoryId)
            => GetForecast(territoryId, 1, DateTime.UtcNow)[0];

        public WeatherListing GetForecast(uint territoryId, DateTime fromWhen)
            => GetForecast(territoryId, 1, fromWhen)[0];

        public WeatherListing GetForecast(uint territoryId, long secondOffset)
            => GetForecast(territoryId, 1, DateTime.UtcNow.AddSeconds(secondOffset))[0];

        public (Weather, byte)[] GetRates(uint territoryId)
            => _weatherRates[territoryId];
    }
}
