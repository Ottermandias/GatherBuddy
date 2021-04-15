using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Plugin;
using GatherBuddy.Classes;
using GatherBuddy.Game;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Managers
{
    public class WeatherTimeline : IComparable<WeatherTimeline>
    {
        private const int SecondsPerWeather = WeatherManager.SecondsPerWeather;

        public Territory            Territory { get; }
        public List<WeatherListing> List      { get; }

        public WeatherListing CurrentWeather
            => Get(1);

        public WeatherListing LastWeather
            => Get(0);

        private WeatherListing Get(uint idx)
        {
            TrimFront();
            if (List.Count <= idx)
                Append((uint) (idx + 1 - List.Count));
            return List[(int) idx];
        }

        public void TrimFront()
        {
            var remove = List.FindIndex(w => w.Offset(DateTime.UtcNow) < 2 * SecondsPerWeather);
            if (remove > 0)
                List.RemoveRange(0, remove);
        }

        private IEnumerable<WeatherListing> RequestData(uint amount, long offset)
            => Service<SkyWatcher>.Get()
                .GetForecast(Territory.Id, amount, offset);

        public void Append(uint amount)
        {
            var offset = List.Count > 0 ? SecondsPerWeather - List.Last().Offset(DateTime.UtcNow) : -SecondsPerWeather;
            List.AddRange(RequestData(amount, offset));
        }

        public WeatherTimeline Update(uint amount)
        {
            TrimFront();
            if (List.Count < amount)
                Append((uint) (amount - List.Count));
            return this;
        }

        public WeatherTimeline(Territory territory, uint cache = 32)
        {
            Territory = territory;
            List      = RequestData(cache, -SecondsPerWeather).ToList();
        }

        public int CompareTo(WeatherTimeline other)
            => Territory.Id.CompareTo(other.Territory.Id);


        public WeatherListing Find(IList<uint> weather, IList<uint> previousWeather, Uptime uptime, long offset = 0, uint increment = 32)
        {
            var now = DateTime.UtcNow;
            TrimFront();
            var previousFit = previousWeather.Count == 0;
            var idx         = offset == 0 ? 1 : List.FindIndex(w => w.Offset(now.AddSeconds(offset - SecondsPerWeather)) > 0) + 1;
            while (true)
            {
                for (--idx; idx < List.Count; ++idx)
                {
                    var w = List[idx];
                    if (previousFit
                     && w.Time.AddSeconds(w.Uptime.Overlap(uptime).Count * EorzeaTime.SecondsPerEorzeaHour) >= DateTime.UtcNow
                     && w.Uptime.Overlaps(uptime)
                     && (weather.Count == 0 || weather.Contains(w.Weather.RowId)))
                        return w;

                    previousFit = previousWeather.Count == 0 || previousWeather.Contains(List[idx].Weather.RowId);
                }

                Append(increment);
            }
        }
    }

    public class WeatherManager
    {
        public const int SecondsPerWeather = SkyWatcher.SecondsPerWeather;

        public SortedList<uint, WeatherTimeline> Forecast    { get; } = new();
        public List<WeatherTimeline>             UniqueZones { get; } = new();


        public WeatherManager(DalamudPluginInterface pi, TerritoryManager territories)
        {
            var skyWatcher     = Service<SkyWatcher>.Set(pi);
            var territoryTypes = pi.Data.GetExcelSheet<TerritoryType>();

            foreach (var t in territoryTypes.Where(t => skyWatcher.GetRates(t.RowId).Length > 1))
            {
                var territory = territories.FindOrAddTerritory(pi, t);
                if (territory == null)
                    continue;

                var timeLine = new WeatherTimeline(territory);
                Forecast[territory.Id] = timeLine;
                if (UniqueZones.All(w => w.Territory.Name != (string) territory.Name))
                    UniqueZones.Add(timeLine);
            }
        }

        public DateTime[] NextWeatherChangeTimes(int num, long offset = 0)
        {
            var timeStamp          = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + offset;
            var currentWeatherTime = timeStamp - timeStamp % SecondsPerWeather;
            var dateTime           = DateTimeOffset.FromUnixTimeSeconds(currentWeatherTime).LocalDateTime;
            var ret                = new DateTime[num];
            for (var i = 0; i < num; ++i)
                ret[i] = dateTime.AddSeconds(i * SecondsPerWeather);
            return ret;
        }

        private WeatherTimeline FindOrCreateForecast(Territory territory, uint increment)
        {
            if (!Forecast.TryGetValue(territory.Id, out var values))
                return values;

            var timeline = new WeatherTimeline(territory, increment);
            Forecast[territory.Id] = timeline;
            return timeline;
        }

        public WeatherTimeline RequestForecast(Territory territory, uint amount)
        {
            var list = FindOrCreateForecast(territory, amount);
            return list.Update(amount);
        }

        public WeatherListing RequestForecast(Territory territory, IList<uint> weather, long offset = 0, uint increment = 32)
            => RequestForecast(territory, weather, Array.Empty<uint>(), Uptime.AllHours, offset, increment);

        public WeatherListing RequestForecast(Territory territory, IList<uint> weather, Uptime uptime, long offset = 0, uint increment = 32)
            => RequestForecast(territory, weather, Array.Empty<uint>(), uptime, offset, increment);


        public WeatherListing RequestForecast(Territory territory, IList<uint> weather, IList<uint> previousWeather,
            Uptime uptime, long offset = 0, uint increment = 32)
        {
            var values = FindOrCreateForecast(territory, increment);
            return values.Find(weather, previousWeather, uptime, offset, increment);
        }
    }
}
