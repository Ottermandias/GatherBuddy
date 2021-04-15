using GatherBuddy.Managers;
using GatherBuddy.Utility;
using ImGuiScene;

namespace GatherBuddy.Gui.Cache
{
    internal struct Header
    {
        public long         DrawHour;
        public uint         Territory;
        public TextureWrap? CurrentWeather;
        public TextureWrap? NextWeather;

        public void Setup()
            => Update(0, 0);

        public void Update(long hour, uint territory)
        {
            if (hour - DrawHour < 8 && Territory == territory)
                return;

            DrawHour = hour - (hour & 0b111);
            if (territory == 0)
            {
                Territory      = 0;
                CurrentWeather = null;
                NextWeather    = null;
                return;
            }

            Territory = territory;
            var weathers = Service<SkyWatcher>.Get().GetForecast(Territory, 2);
            var icons    = Service<Cache.Icons>.Get();
            CurrentWeather = icons[weathers[0].Weather.Icon];
            NextWeather    = icons[weathers[1].Weather.Icon];
        }
    }
}
