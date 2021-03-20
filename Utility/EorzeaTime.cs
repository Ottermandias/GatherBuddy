using System;

namespace GatherBuddy.Utility
{
    public static class EorzeaTime
    {   
        public static int CurrentHours(int minuteOffset = 0)
            => ToHours(CurrentEorzeaTimestamp() + 60 * minuteOffset);

        public static int CurrentMinutes(int minuteOffset = 0)
            => ToMinutes(CurrentEorzeaTimestamp() + 60 * minuteOffset);

        public static int CurrentMinuteOfDay()
            => (int) (CurrentEorzeaTimestamp() / 60) % (24 * 60);

        public static (int hours, int minutes) CurrentTime(int minuteOffset = 0)
        {
            var timestamp = CurrentEorzeaTimestamp() + 60 * minuteOffset;
            return (hours: ToHours(timestamp), minutes: ToMinutes(timestamp));
        }

        private static long CurrentEorzeaTimestamp()
            => DateTimeOffset.UtcNow.ToUnixTimeSeconds() * 144 / 7;

        public static (int minutes, int seconds) MinutesToReal(long eorzeaMinutes)
        {
            var time = eorzeaMinutes * 7.0 / 144.0;
            var m    = (int) time;
            var s    = (int) ((time - m) * 60.0);
            return (m, s);
        }

        private static int ToHours(long eorzeaTimestamp)
            => (int)(eorzeaTimestamp / 3600) % 24;

        private static int ToMinutes(long eorzeaTimestamp)
            => (int)(eorzeaTimestamp / 60 % 60);
    }
}