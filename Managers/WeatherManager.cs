using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dalamud.Plugin;
using FFXIVWeather.Lumina;
using GatherBuddy.Classes;
using Lumina.Excel.GeneratedSheets;
using GatherBuddy.Utility;

namespace GatherBuddy.Managers
{
    public readonly struct WeatherListing
    {
        public Weather  Weather { get; }
        public DateTime Time    { get; }
        public Uptime   Uptime  { get; }

        public WeatherListing(Weather weather, DateTime time)
        {
            Weather = weather;
            Time    = time.ToUniversalTime();
            Uptime  = new Uptime(time);
        }

        public long Offset(DateTime time)
            => (long) (time - Time).TotalSeconds;
    }

    public class WeatherManager
    {
        private const    double                    SecondPerWeather = 1400D;
        private readonly FFXIVWeatherLuminaService _weather;

        public Dictionary<Territory, List<WeatherListing>> Forecast { get; set; } = new();

        public WeatherManager(DalamudPluginInterface pi)
        {
            var gameDataField = pi.Data.GetType().GetField("gameData", BindingFlags.Instance | BindingFlags.NonPublic);
            if (gameDataField == null)
                throw new Exception("Missing gameData field");

            var lumina = (Lumina.GameData) gameDataField.GetValue(pi.Data);
            _weather = new FFXIVWeatherLuminaService(lumina);
        }

        public WeatherListing RequestForecast(Territory territory, IList<uint> weather, long offset = 0, uint increment = 32)
            => RequestForecast(territory, weather, Array.Empty<uint>(), Uptime.AllHours, offset, increment);

        public WeatherListing RequestForecast(Territory territory, IList<uint> weather, Uptime uptime, long offset = 0, uint increment = 32)
            => RequestForecast(territory, weather, Array.Empty<uint>(), uptime, offset, increment);


        public WeatherListing RequestForecast(Territory territory, IList<uint> weather, IList<uint> previousWeather,
            Uptime uptime, long offset = 0, uint increment = 32)
        {
            if (!Forecast.TryGetValue(territory, out var values))
            {
                var weathers = _weather.GetForecast((int) territory.Id, increment);
                values = weathers.Select(w => new WeatherListing(w.Item1, w.Item2)).ToList();
            }

            var now    = DateTime.UtcNow;
            var remove = values.FindIndex(w => w.Offset(now) > -SecondPerWeather);
            if (remove > 0)
                values.RemoveRange(0, remove - 1);

            var previousFit = previousWeather.Count == 0;
            var idx         = offset == 0 ? 1 : values.FindIndex(w => w.Offset(now.AddSeconds(offset)) > -SecondPerWeather) + 1;
            while (true)
            {
                for (--idx; idx < values.Count; ++idx)
                {
                    var w = values[idx];
                    if (previousFit && w.Uptime.Overlaps(uptime) && (weather.Count == 0 || weather.Contains(w.Weather.RowId)))
                    {
                        Forecast[territory] = values;
                        return w;
                    }

                    previousFit = previousWeather.Count == 0 || previousWeather.Contains(values[idx].Weather.RowId);
                }

                var offset2 = -values.Last().Offset(now) + SecondPerWeather + 1;
                values.AddRange(_weather.GetForecast((int) territory.Id, increment, SecondPerWeather, offset2)
                    .Select(w => new WeatherListing(w.Item1, w.Item2)));
            }
        }
    }
}
