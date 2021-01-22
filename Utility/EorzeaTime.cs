using System;

namespace Gathering
{
    public static class EorzeaTime
    {   
        public static int CurrentHours(int minuteOffset = 0)
        {
            var timestamp = CurrentEorzeaTimestamp() + 60 * minuteOffset;
            return ToHours(timestamp);
        }

        public static int CurrentMinutes(int minuteOffset = 0)
        {
            var timestamp = CurrentEorzeaTimestamp() + 60 * minuteOffset;
            return ToMinutes(timestamp);
        }

        public static (int hours, int minutes) CurrentTime(int minuteOffset = 0)
        {
            var timestamp = CurrentEorzeaTimestamp() + 60 * minuteOffset;
            return (hours: ToHours(timestamp), minutes: ToMinutes(timestamp));
        }

        #region conversions
        private static long CurrentEorzeaTimestamp()
        {
            return (DateTimeOffset.UtcNow.ToUnixTimeSeconds() * 144) / 7;
        }
    
        private static int ToHours(long eorzeaTimestamp)
        {
            return (int)(eorzeaTimestamp / 3600) % 24;
        }

        private static int ToMinutes(long eorzeaTimestamp)
        {
            return (int)((eorzeaTimestamp / 60) % 60);
        }
        #endregion
    }
}