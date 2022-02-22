using System.Linq;
using GatherBuddy.Weather;
using ImGuiScene;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private readonly struct CachedWeather
    {
        public const int NumWeathers = 12;

        private readonly WeatherTimeline                  _timeline;
        public readonly  (Structs.Weather, TextureWrap)[] Weathers;

        public string Zone
            => _timeline.Territory.Name;

        private CachedWeather(WeatherTimeline t)
        {
            _timeline = t;
            Weathers  = new (Structs.Weather, TextureWrap)[NumWeathers];
        }

        public void Update()
        {
            _timeline.Update(NumWeathers);
            for (var i = 0; i < NumWeathers; ++i)
            {
                var weather = _timeline.List[i].Weather;
                Weathers[i] = (weather, Icons.DefaultStorage[weather.Data.Icon]);
            }
        }

        public static CachedWeather[] CreateWeatherCache()
            => GatherBuddy.WeatherManager.UniqueZones.Select(t =>
            {
                var ret = new CachedWeather(t);
                ret.Update();
                return ret;
            }).ToArray();
    }
}
