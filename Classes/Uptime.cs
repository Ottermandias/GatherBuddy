using System;
using System.Linq;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Classes
{
    public readonly struct RealUptime
    {
        public DateTime Time     { get; }
        public DateTime EndTime { get; }

        public TimeSpan Duration
            => EndTime - Time;

        public RealUptime(DateTime time, DateTime endTime)
        {
            Time    = time.ToUniversalTime();
            EndTime = endTime.ToUniversalTime();
        }

        public RealUptime(DateTime time, TimeSpan duration)
        {
            Time    = time.ToUniversalTime();
            EndTime = Time + duration;
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

            var diff = DateTime.Compare(EndTime, rhs.EndTime);
            return diff != 0 ? diff : DateTime.Compare(Time, Time);
        }
    }

    public readonly struct Uptime
    {
        private const int AllHoursValue = 0x00FFFFFF;

        private readonly uint _hours; // bitfield, 0-23 for each hour.

        public bool AlwaysUp()
            => _hours == AllHoursValue;

        public bool IsUp(int hour)
        {
            hour %= 24;
            return ((_hours >> hour) & 1) == 1;
        }

        public int FirstHour
            => Util.TrailingZeroCount(_hours);

        public int Count
            => Util.Popcount(_hours);

        public Uptime Overlap(Uptime rhs)
            => new(_hours & rhs._hours);

        public bool Overlaps(Uptime rhs)
            => Overlap(rhs)._hours != 0;

        private static int NextTime(int currentHour, uint hours)
        {
            if (hours == 0)
                return 25;

            var rotatedHours = currentHour == 0 ? hours : (hours >> currentHour) | ((hours << (32 - currentHour)) >> 8);
            return Util.TrailingZeroCount(rotatedHours);
        }

        public int NextUptime(int currentHour)
            => NextTime(currentHour, _hours);

        public int NextDowntime(int currentHour)
            => NextTime(currentHour, ~_hours & AllHoursValue);

        public RealUptime NextRealUptime()
        {
            var now          = DateTime.UtcNow;
            var timeStamp    = (long) (now - EorzeaTime.UnixEpoch).TotalSeconds;
            var hour         = (int) (timeStamp / 175) % 24;
            var nextUptime   = NextUptime(hour);
            var nextDowntime = NextDowntime((hour + nextUptime) % 24);
            if (nextUptime == 25)
                return RealUptime.Never;
            if (nextDowntime == 25)
                return RealUptime.Always;
            if (nextUptime == 0)
                return new RealUptime(now, now.AddSeconds(nextDowntime * 175 - (int) (timeStamp % 175)));

            now = now.AddSeconds(nextUptime * 175 - (timeStamp % 175));
            return new RealUptime(now, now.AddSeconds(nextDowntime * 175));
        }

        // Print a string of 24 '0' or '1' as uptimes.
        public string UptimeTable()
            => new(Convert.ToString(_hours, 2).PadLeft(24, '0').Reverse().ToArray());

        // Print hours in human readable format.
        public string PrintHours(bool simple = false, string simpleSeparator = "|")
        {
            var ret = "";
            int min = -1, max = -1;

            void AddString()
            {
                if (min < 0)
                    return;

                if (ret.Length > 0)
                {
                    if (simple)
                    {
                        ret += simpleSeparator;
                    }
                    else
                    {
                        ret =  ret.Replace(" and ", ", ");
                        ret += " and ";
                    }
                }

                if (simple)
                    ret += $"{min:D2}-{max + 1:D2}";
                else
                    ret += $"{min:D2}:00 - {max + 1:D2}:00 ET";

                min = -1;
                max = -1;
            }

            for (var i = 0; i < 24; ++i)
            {
                if (IsUp(i))
                {
                    if (min < 0)
                        min = i;
                    else
                        max = i;
                }
                else
                {
                    AddString();
                }
            }

            AddString();

            return ret;
        }

        // Convert the ephemeral time as given by the table to a bitfield.
        private static uint ConvertFromEphemeralTime(ushort start, ushort end)
        {
            // Up at all times
            if (start == end || start > 2400 || end > 2400)
                return AllHoursValue;

            var ret = 0u;
            start /= 100;
            end   /= 100;

            if (end < start)
                end += 24;

            for (int i = start; i < end; ++i)
                ret |= 1u << (i % 24);
            return ret;
        }

        public static Uptime AllHours { get; } = new(AllHoursValue);

        private Uptime(uint hours)
            => _hours = hours;

        public Uptime(ushort start, ushort end)
            => _hours = ConvertFromEphemeralTime(start, end);

        public static Uptime FromHours(uint startHour, uint endHour)
        {
            var hours = 0u;
            startHour %= 24;
            endHour   %= 24;
            if (endHour == startHour)
                return AllHours;

            if (endHour < startHour)
            {
                for (; startHour < 24; ++startHour)
                    hours |= 1u << (int) startHour;
                for (startHour = 0; startHour < endHour; ++startHour)
                    hours |= 1u << (int) startHour;
            }
            else
            {
                for (; startHour < endHour; ++startHour)
                    hours |= 1u << (int) startHour;
            }

            return new Uptime(hours);
        }


        public Uptime(DateTime time)
        {
            var hour = EorzeaTime.Hours(time);
            _hours = (hour / 8) switch
            {
                0 => 0x000000FF,
                1 => 0x0000FF00,
                2 => 0x00FF0000,
                _ => 0x00000000,
            };
        }


        // Convert the rare pop time given by the table to a bitfield.
        public Uptime(GatheringRarePopTimeTable table)
        {
            _hours = 0;

            // Convert the time slots to ephemeral format to reuse that function.
            foreach (var time in table.UnkStruct0)
            {
                if (time.Durationm == 0)
                    continue;

                var duration = time.Durationm == 160 ? (ushort) 200 : time.Durationm;
                var end      = (ushort) ((time.StartTime + duration) % 2400);
                _hours |= ConvertFromEphemeralTime(time.StartTime, end);
            }
        }
    }
}
