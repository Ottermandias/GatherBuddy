using System;

namespace GatherBuddy.Utility
{
    public static class RealTime
    {
        public const int SecondsPerMinute = 60;
        public const int MinutesPerHour   = 60;
        public const int HoursPerDay      = 24;
        public const int SecondsPerHour   = SecondsPerMinute * MinutesPerHour;
        public const int MinutesPerDay    = MinutesPerHour * HoursPerDay;
        public const int SecondsPerDay    = SecondsPerMinute * MinutesPerDay;

        public static readonly DateTime UnixEpoch = new(1970, 1, 1);

        public static long CurrentTimestamp()
            => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public static class EorzeaTime
    {
        public const int    SecondsPerEorzeaHour = 175;
        public const int    SecondsPerEorzeaDay  = RealTime.HoursPerDay * SecondsPerEorzeaHour;
        public const double ConversionFactor     = 7.0 / 144.0;

        public static long CurrentTimestamp()
            => RealTime.CurrentTimestamp() * 144 / 7;

        public static long CurrentHour()
            => RealTime.CurrentTimestamp() / SecondsPerEorzeaHour;

        public static long CurrentHour(int eorzeaMinuteOffset)
            => (CurrentTimestamp() + RealTime.SecondsPerMinute * eorzeaMinuteOffset) / RealTime.SecondsPerHour;

        public static uint CurrentHourOfDay()
            => (uint) CurrentHour() % RealTime.HoursPerDay;

        public static uint CurrentHourOfDay(int eorzeaMinuteOffset)
            => (uint) CurrentHour(eorzeaMinuteOffset) % RealTime.HoursPerDay;

        public static long CurrentMinute(int eorzeaMinuteOffset = 0)
            => RealTime.CurrentTimestamp() * 36 / 105 + eorzeaMinuteOffset;

        public static uint CurrentMinuteOfHour(int eorzeaMinuteOffset = 0)
            => (uint) CurrentMinute(eorzeaMinuteOffset) % RealTime.MinutesPerHour;

        public static uint CurrentMinuteOfDay(int eorzeaMinuteOffset = 0)
            => (uint) CurrentMinute(eorzeaMinuteOffset) % RealTime.MinutesPerDay;

        public static (uint hours, uint minutes) CurrentTimeOfDay(int eorzeaMinuteOffset = 0)
        {
            var timestamp = CurrentTimestamp() + RealTime.SecondsPerMinute * eorzeaMinuteOffset;
            return ((uint) (timestamp / RealTime.SecondsPerHour) % RealTime.HoursPerDay,
                (uint) (timestamp / RealTime.SecondsPerMinute) % RealTime.MinutesPerHour);
        }

        private static long RealTimestamp(DateTime time)
            => (long) (time - RealTime.UnixEpoch).TotalSeconds;

        public static long Hour(DateTime time)
            => RealTimestamp(time) / SecondsPerEorzeaHour;

        public static uint HourOfDay(DateTime time)
            => (uint) Hour(time) % RealTime.HoursPerDay;

        public static uint MinuteOfDay(DateTime time)
            => (uint) (RealTimestamp(time) * 36 / 105) % RealTime.SecondsPerMinute;

        public static (long minutes, long seconds) MinutesToReal(long eorzeaMinutes)
        {
            var time = eorzeaMinutes * ConversionFactor;
            var m    = (long) time;
            var s    = (long) ((time - m) * RealTime.SecondsPerMinute);
            return (m, s);
        }
    }
}
