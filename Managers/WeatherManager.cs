using System;
using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Classes;
using GatherBuddy.Game;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Managers
{
    public class WeatherTimeline : IComparable<WeatherTimeline>
    {
        public const int MillisecondsPerWeather = EorzeaTimeStampExtensions.MillisecondsPerEorzeaWeather;

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
            var now    = TimeStamp.UtcNow;
            var remove = List.FindIndex(w => w.Offset(now) < 2 * MillisecondsPerWeather);
            if (remove > 0)
                List.RemoveRange(0, remove);
        }

        private IEnumerable<WeatherListing> RequestData(uint amount, long millisecondOffset)
            => Service<SkyWatcher>.Get()
                .GetForecastOffset(Territory.Id, amount, millisecondOffset);

        public void Append(uint amount)
        {
            var offset = List.Count > 0 ? MillisecondsPerWeather - (int) List.Last().Offset(TimeStamp.UtcNow) : -MillisecondsPerWeather;
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
            List      = RequestData(cache, -MillisecondsPerWeather).ToList();
        }

        public int CompareTo(WeatherTimeline? other)
            => Territory.Id.CompareTo(other?.Territory.Id ?? 0);


        public WeatherListing Find(IList<uint> weather, IList<uint> previousWeather, RepeatingInterval eorzeanHours, long offset = 0, uint increment = 32)
        {
            var now = TimeStamp.UtcNow + offset;
            TrimFront();
            var previousFit = false;
            var idx         = 1;

            while (true)
            {
                for (--idx; idx < List.Count; ++idx)
                {
                    if (previousFit)
                    {
                        var w       = List[idx];
                        if (weather.Count == 0 || weather.Contains(w.Weather.RowId))
                        {
                            var overlap = w.Uptime.FirstOverlap(eorzeanHours);
                            var duration = overlap.Duration;
                            if (duration > 0 && overlap.End > now)
                                return w;
                        }
                    }

                    previousFit = previousWeather.Count == 0 || previousWeather.Contains(List[idx].Weather.RowId);
                }

                Append(increment);
            }
        }

        public int FindIndex(WeatherListing listing)
            => List.FindIndex(w => w.Timestamp == listing.Timestamp);
    }

    public class WeatherManager
    {
        public SortedList<uint, WeatherTimeline> Forecast    { get; } = new();
        public List<WeatherTimeline>             UniqueZones { get; } = new();


        public WeatherManager(TerritoryManager territories)
        {
            var skyWatcher     = Service<SkyWatcher>.Set(Dalamud.GameData, GatherBuddy.Language)!;
            var territoryTypes = Dalamud.GameData.GetExcelSheet<TerritoryType>()!;

            foreach (var t in territoryTypes.Where(t => skyWatcher.GetRates(t.RowId).Length > 1))
            {
                var territory = territories.FindOrAddTerritory(t);
                if (territory == null)
                    continue;

                var timeLine = new WeatherTimeline(territory);
                Forecast[territory.Id] = timeLine;
                if (UniqueZones.All(w => w.Territory.Name != (string) territory.Name))
                    UniqueZones.Add(timeLine);
            }
        }

        public static DateTime[] NextWeatherChangeTimes(int num, long offset = 0)
        {
            var currentWeatherTime = (TimeStamp.UtcNow + offset).SyncToEorzeaWeather();
            var ret                = new DateTime[num];
            for (var i = 0; i < num; ++i)
                ret[i] = currentWeatherTime.AddEorzeaHours(i * 8).LocalTime;
            return ret;
        }

        private WeatherTimeline FindOrCreateForecast(Territory territory, uint increment)
        {
            if (Forecast.TryGetValue(territory.Id, out var values))
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
            => RequestForecast(territory, weather, Array.Empty<uint>(), RepeatingInterval.Always, offset, increment);

        public WeatherListing RequestForecast(Territory territory, IList<uint> weather, RepeatingInterval eorzeanHours, long offset = 0, uint increment = 32)
            => RequestForecast(territory, weather, Array.Empty<uint>(), eorzeanHours, offset, increment);


        public WeatherListing RequestForecast(Territory territory, IList<uint> weather, IList<uint> previousWeather,
            RepeatingInterval eorzeanHours, long offset = 0, uint increment = 32)
        {
            var values = FindOrCreateForecast(territory, increment);
            return values.Find(weather, previousWeather, eorzeanHours, offset, increment);
        }

        public TimeStamp ExtendedDuration(Territory territory, IList<uint> weather, IList<uint> previousWeather, WeatherListing listing, uint increment = 32)
        {
            var checkWeathers = weather.Any();
            var checkPrevious = previousWeather.Any();
            if (!checkWeathers && !checkPrevious)
                return TimeStamp.MaxValue;

            var duration = listing.End;
            if (checkPrevious && !previousWeather.Contains(listing.Weather.RowId))
                return duration;

            var values = FindOrCreateForecast(territory, increment);
            values.TrimFront();
            var idx    = values.FindIndex(listing);
            if (idx < 0)
                return duration;

            for(var sanityStop = 0; sanityStop < 24; ++sanityStop)
            {
                if (checkPrevious && !previousWeather.Contains(listing.Weather.RowId))
                    return duration;

                if (idx == values.List.Count - 1)
                    values.Append(increment);
                listing = values.List[++idx];
                if (checkWeathers && !weather.Contains(listing.Weather.RowId))
                    return duration;

                duration = duration.AddEorzeaHours(8);
            };
            return duration;
        }
    }
}
