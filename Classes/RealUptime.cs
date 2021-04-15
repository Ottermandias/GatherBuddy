using System;

namespace GatherBuddy.Classes
{
    public readonly struct RealUptime
    {
        public DateTime Time    { get; }
        public DateTime EndTime { get; }

        public TimeSpan Duration
            => EndTime - Time;

        public RealUptime(DateTime time, DateTime endTime)
        {
            Time    = time.ToUniversalTime().AddTicks(-(time.Ticks % TimeSpan.TicksPerSecond));
            EndTime = endTime.ToUniversalTime().AddTicks(-(endTime.Ticks % TimeSpan.TicksPerSecond));
        }

        public RealUptime(DateTime time, TimeSpan duration)
        {
            Time    = time.ToUniversalTime().AddTicks(-(time.Ticks % TimeSpan.TicksPerSecond));
            EndTime = (Time + duration).AddTicks(-(duration.Ticks % TimeSpan.TicksPerSecond));
        }

        public static RealUptime Always  = new(DateTime.MinValue, DateTime.MaxValue.AddSeconds(-1));
        public static RealUptime Unknown = new(DateTime.MinValue, DateTime.MinValue);
        public static RealUptime Never   = new(DateTime.MaxValue, DateTime.MaxValue);

        public bool Equals(RealUptime rhs)
            => EndTime == rhs.EndTime && Time == rhs.Time;

        public int Compare(RealUptime rhs)
        {
            if (Equals(Unknown))
                return Never.Compare(rhs);
            if (rhs.Equals(Unknown))
                return Compare(Never);

            var diff = EndTime - rhs.EndTime;
            if (Math.Abs(diff.TotalSeconds) >= 0.2)
                return diff.CompareTo(TimeSpan.Zero);

            var diff2 = Time - rhs.Time;
            return Math.Abs(diff2.TotalSeconds) < 0.2 ? 0 : diff2.CompareTo(TimeSpan.Zero);
        }
    }
}
