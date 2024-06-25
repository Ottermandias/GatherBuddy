using System.Linq;
using Dalamud.Interface.Textures;
using GatherBuddy.Weather;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private readonly struct CachedWeather
    {
        public const int NumWeathers = 12;

        private readonly WeatherTimeline                              _timeline;
        public readonly  (Structs.Weather, ISharedImmediateTexture)[] Weathers;

        public string Zone
            => _timeline.Territory.Name;

        private CachedWeather(WeatherTimeline t)
        {
            _timeline = t;
            Weathers  = new (Structs.Weather, ISharedImmediateTexture)[NumWeathers];
        }

        public void Update()
        {
            _timeline.Update(NumWeathers);
            for (var i = 0; i < NumWeathers; ++i)
            {
                var weather = _timeline.List[i].Weather;
                Weathers[i] = (weather, Icons.DefaultStorage.TextureProvider.GetFromGameIcon(weather.Data.Icon));
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
