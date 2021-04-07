using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Classes
{
    public enum GigHead : byte
    {
        None   = 0,
        Small  = 1,
        Normal = 2,
        Large  = 3,
    }

    [Flags]
    public enum Restrictions : byte
    {
        None           = 0,
        Time           = 1,
        Weather        = 2,
        TimeAndWeather = Time | Weather,
    }

    public class Fish : IComparable
    {
        public uint                 Id           { get; set; }
        public uint                 FishId       { get; set; }
        public HashSet<FishingSpot> FishingSpots { get; set; } = new();
        public FFName               Name         { get; set; }

        public GigHead Gig       { get; set; } = GigHead.None;
        public bool    InLog     { get; set; }
        public bool    OceanFish { get; set; }

        public FishRecord Record { get; set; } = new();

        public Restrictions Restrictions { get; set; }
        public CatchData?   CatchData    { get; set; } = null;

        public Fish(SpearfishingItem fishRow, GigHead gig, FFName name)
        {
            Id           = fishRow.Item.Row;
            FishId       = fishRow.RowId;
            Name         = name;
            Restrictions = Restrictions.None;
            Gig          = gig;
            InLog        = true;
        }

        public Fish(FishParameter fishRow, FFName name)
        {
            Id     = (uint) fishRow.Item;
            FishId = fishRow.RowId;
            Restrictions = (fishRow.TimeRestricted ? Restrictions.Time : Restrictions.None)
              | (fishRow.WeatherRestricted ? Restrictions.Weather : Restrictions.None);
            Name      = name;
            InLog     = fishRow.IsInLog;
            OceanFish = fishRow.TerritoryType.Row == 900;
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
                return 1;

            var rhs = obj as Fish;
            return Id.CompareTo(rhs?.Id ?? 0);
        }

        public RealUptime NextUptime(WeatherManager weather)
        {
            // Always
            if (Restrictions == Restrictions.None)
                return RealUptime.Always;

            // Unknown
            if (CatchData == null)
                return RealUptime.Unknown;

            // Update cache if necessary
            if (_nextUptime.EndTime <= DateTime.UtcNow)
                UpdateUptime(weather);

            // Cache valid
            return _nextUptime;
        }

        private void UpdateUptime(WeatherManager weather)
        {
            if (Restrictions == Restrictions.Time)
            {
                _nextUptime = CatchData!.Hours.NextRealUptime();
                return;
            }

            var epoch = EorzeaTime.UnixEpoch;
            var wl = FishingSpots
                .Select(fs => weather.RequestForecast(fs.Territory!, CatchData!.CurrentWeather, CatchData.PreviousWeather, CatchData.Hours))
                .ArgMin(w => w.Time);

            var overlap = wl.Uptime.Overlap(CatchData!.Hours);
            var offset  = overlap.FirstHour - wl.Uptime.FirstHour;
            _nextUptime = new RealUptime(wl.Time.AddSeconds(175 * offset), new TimeSpan(0, 0, 175 * overlap.Count));
        }

        private RealUptime _nextUptime = RealUptime.Unknown;
    }
}
