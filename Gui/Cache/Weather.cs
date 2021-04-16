using System;
using System.Linq;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
using ImGuiNET;
using ImGuiScene;

namespace GatherBuddy.Gui.Cache
{
    internal struct Weather
    {
        private static WeatherManager? _weather;

        public const int NumWeathers = 8;

        public readonly DateTime[] WeatherTimes;
        public readonly string[]   WeatherTimeStrings;

        public readonly CachedWeather[] Weathers;

        private long _totalHour;

        public          string Filter;
        public          string FilterLower;
        public readonly float  FilterSize;

        public Weather(WeatherManager weather)
        {
            _weather           = weather;
            WeatherTimes       = weather.NextWeatherChangeTimes(NumWeathers, -WeatherManager.SecondsPerWeather * 2);
            WeatherTimeStrings = new string[NumWeathers];
            _totalHour         = 0;
            Filter             = "";
            FilterLower        = "";
            Weathers           = CachedWeather.CreateWeatherCache();
            FilterSize         = Weathers.Max(c => ImGui.CalcTextSize(c.Zone).X);
        }

        public void Update(long totalHour)
        {
            if (totalHour - _totalHour < 8)
                return;

            UpdateTimes();
            UpdateWeather(totalHour);
        }

        private void UpdateTimes()
        {
            for(var i = 0; i < NumWeathers; ++i)
            {
                WeatherTimes[i]       = WeatherTimes[i].AddSeconds(WeatherManager.SecondsPerWeather);
                WeatherTimeStrings[i] = WeatherTimes[i].TimeOfDay.ToString();
            }
        }

        private void UpdateWeather(long totalHour)
        {
            var hour = totalHour % RealTime.HoursPerDay;
            _totalHour = totalHour - (hour & 0b111);

            for (var i = 0; i < Weathers.Length; ++i)
                Weathers[i].Update(i);
        }


        public readonly struct CachedWeather
        {
            public readonly string        Zone;
            public readonly string        ZoneLower;
            public readonly string[]      WeatherNames;
            public readonly TextureWrap[] Icons;

            public CachedWeather(string name)
            {
                Zone         = name;
                ZoneLower    = name.ToLowerInvariant();
                WeatherNames = new string[NumWeathers];
                Icons        = new TextureWrap[NumWeathers];
            }

            public void Update(int idx)
            {
                var timeline = _weather!.UniqueZones[idx];
                timeline.Update(NumWeathers);
                var icons = Service<Cache.Icons>.Get();
                for (var i = 0; i < NumWeathers; ++i)
                {
                    WeatherNames[i] = timeline.List[i].Weather.Name;
                    Icons[i]        = icons[timeline.List[i].Weather.Icon];
                }
            }

            public static CachedWeather[] CreateWeatherCache()
            {
                var ret = new CachedWeather[_weather!.UniqueZones.Count];
                for (var i = 0; i < _weather.UniqueZones.Count; ++i)
                {
                    ret[i] = new CachedWeather(_weather.UniqueZones[i].Territory.Name);
                    ret[i].Update(i);
                }

                return ret;
            }
        }
    }
}
