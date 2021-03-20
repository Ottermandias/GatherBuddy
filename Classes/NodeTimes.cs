using System;
using System.Linq;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Classes
{
    public class NodeTimes
    {
        private const int AllHours = 0x00FFFFFF;

        private readonly int _hours; // bitfield, 0-23 for each hour.

        public bool AlwaysUp()
            => _hours == AllHours;

        public bool IsUp(int hour)
        {
            hour %= 24;
            return ((_hours >> hour) & 1) == 1;
        }

        public int NextUptime(int currentHour)
        {
            for (var i = 0; i < 24; ++i)
            {
                if (IsUp(currentHour))
                    return i;

                ++currentHour;
                if (currentHour > 23)
                    currentHour -= 24;
            }

            return 25;
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

            AddString();

            return ret;
        }

        // Convert the ephemeral time as given by the table to a bitfield.
        private static int ConvertFromEphemeralTime(ushort start, ushort end)
        {
            // Up at all times
            if (start == end || start > 2400 || end > 2400)
                return AllHours;

            var ret = 0;
            start /= 100;
            end   /= 100;

            if (end < start)
                end += 24;

            for (int i = start; i < end; ++i)
                ret |= 1 << (i % 24);
            return ret;
        }

        public NodeTimes()
            => _hours = AllHours;

        public NodeTimes(ushort start, ushort end)
            => _hours = ConvertFromEphemeralTime(start, end);

        // Convert the rare pop time given by the table to a bitfield.
        public NodeTimes(GatheringRarePopTimeTable table)
        {
            if (table == null)
                return;

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
