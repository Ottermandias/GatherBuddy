using System;
using GatherBuddy.Classes;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Game
{
    public readonly struct WeatherListing
    {
        public Weather  Weather { get; }
        public DateTime Time    { get; }
        public Uptime   Uptime  { get; }

        public WeatherListing(Weather weather, DateTime time)
        {
            Weather = weather;
            Time    = time.ToUniversalTime().AddTicks(-(time.Ticks % TimeSpan.TicksPerSecond));
            Uptime  = new Uptime(time);
        }

        public long Offset(DateTime time)
            => (long) (time - Time).TotalSeconds;
    }
}
