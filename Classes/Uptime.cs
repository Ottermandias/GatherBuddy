using System;
using System.Linq;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Classes
{
    public readonly struct Uptime
    {
        private const    uint BadHour       = RealTime.HoursPerDay + 1;
        private const    uint AllHoursValue = 0x00FFFFFF;
        private readonly uint _hours; // bitfield, 0-23 for each hour.

        public static Uptime AllHours { get; } = new(AllHoursValue);

        public bool Equals(Uptime rhs)
            => _hours == rhs._hours;

        public bool AlwaysUp()
            => _hours == AllHoursValue;

        public bool IsUp(uint hour)
        {
            hour %= RealTime.HoursPerDay;
            return ((_hours >> (int) hour) & 1) == 1;
        }

        public uint FirstHour
            => Util.TrailingZeroCount(_hours);

        public uint EndHour
            => Util.HighestSetBit(_hours) + 1;

        // Returns null if the time is not continuous.
        public (uint, uint)? StartAndEnd()
        {
            var startHour = FirstHour;
            var endHour   = EndHour;
            var count     = Count;
            if (startHour + count == endHour)
                return (startHour, endHour);

            startHour = Util.HighestSetBit(~_hours & AllHoursValue) + 1;
            endHour   = Util.TrailingZeroCount(~_hours);
            if ((startHour + count) % RealTime.HoursPerDay == endHour)
                return (startHour, endHour);

            return null;
        }

        public uint Count
            => Util.Popcount(_hours);

        public Uptime Overlap(Uptime rhs)
            => new(_hours & rhs._hours);

        public bool Overlaps(Uptime rhs)
            => Overlap(rhs)._hours != 0;

        private static uint NextTime(uint currentHour, uint hours)
        {
            if (hours == 0)
                return BadHour;

            var rotatedHours = currentHour == 0 ? hours : (hours >> (int) currentHour) | ((hours << (32 - (int) currentHour)) >> 8);
            return Util.TrailingZeroCount(rotatedHours);
        }

        public uint NextUptime(uint currentHour)
            => NextTime(currentHour, _hours);

        public uint NextDowntime(uint currentHour)
            => NextTime(currentHour, ~_hours & AllHoursValue);

        public RealUptime NextRealUptime()
        {
            var now          = DateTime.UtcNow;
            var timeStamp    = RealTime.CurrentTimestamp();
            var hour         = (uint) (timeStamp / EorzeaTime.SecondsPerEorzeaHour) % RealTime.HoursPerDay;
            var nextUptime   = NextUptime(hour);
            var nextDowntime = NextDowntime((hour + nextUptime) % RealTime.HoursPerDay);
            if (nextUptime == BadHour)
                return RealUptime.Never;
            if (nextDowntime == BadHour)
                return RealUptime.Always;
            if (nextUptime == 0)
                return new RealUptime(now,
                    now.AddSeconds(nextDowntime * EorzeaTime.SecondsPerEorzeaHour - (int) (timeStamp % EorzeaTime.SecondsPerEorzeaHour)));

            now = now.AddSeconds(nextUptime * EorzeaTime.SecondsPerEorzeaHour - timeStamp % EorzeaTime.SecondsPerEorzeaHour);
            return new RealUptime(now, now.AddSeconds(nextDowntime * EorzeaTime.SecondsPerEorzeaHour));
        }

        // Print a string of 24 '0' or '1' as uptimes.
        public string UptimeTable()
            => new(Convert.ToString(_hours, 2).PadLeft(RealTime.HoursPerDay, '0').Reverse().ToArray());

        // Print hours in human readable format.
        public string PrintHours(bool simple = false, string simpleSeparator = "|")
        {
            var ret = "";
            int min = -1, max = -1;

            var hours = StartAndEnd();
            if (hours != null)
            {
                var (start, end) = hours.Value;
                return simple ? $"{start:D2}-{end:D2}" : $"{start:D2}:00 - {end:D2}:00 ET";
            }

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

            for (var i = 0u; i < RealTime.HoursPerDay; ++i)
            {
                if (IsUp(i))
                    if (min < 0)
                        min = (int) i;
                    else
                        max = (int) i;
                else
                    AddString();
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
                end += RealTime.HoursPerDay;

            for (int i = start; i < end; ++i)
                ret |= 1u << (i % RealTime.HoursPerDay);
            return ret;
        }

        private Uptime(uint hours)
            => _hours = hours;

        public Uptime(ushort start, ushort end)
            => _hours = ConvertFromEphemeralTime(start, end);

        public static Uptime FromHours(uint startHour, uint endHour)
        {
            var hours = 0u;
            startHour %= RealTime.HoursPerDay;
            endHour   %= RealTime.HoursPerDay;
            if (endHour == startHour)
                return AllHours;

            if (endHour < startHour)
            {
                for (; startHour < RealTime.HoursPerDay; ++startHour)
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


        // For Weather.
        public Uptime(DateTime time)
        {
            var hour = EorzeaTime.HourOfDay(time);
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
