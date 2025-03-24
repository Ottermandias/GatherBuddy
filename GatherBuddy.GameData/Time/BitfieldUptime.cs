using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Utility;
using Lumina.Excel.Sheets;

namespace GatherBuddy.Time;

public readonly struct BitfieldUptime : IEquatable<BitfieldUptime>
{
    private const    uint BadHour       = RealTime.HoursPerDay + 1;
    private const    uint AllHoursValue = 0x00FFFFFF;
    private readonly uint _hours; // bitfield, 0-23 for each hour.

    public static readonly BitfieldUptime AllHours = new(AllHoursValue);

    public static BitfieldUptime Combine(BitfieldUptime lhs, BitfieldUptime rhs)
        => new(lhs._hours | rhs._hours);

    public bool Equals(BitfieldUptime rhs)
        => _hours == rhs._hours;

    public bool AlwaysUp()
        => _hours == AllHoursValue;

    public bool IsUp(uint hour)
    {
        if (hour >= RealTime.HoursPerDay)
            hour %= RealTime.HoursPerDay;
        return ((_hours >> (int)hour) & 1) == 1;
    }

    public uint FirstHour
        => Bits.TrailingZeroCount(_hours);

    public uint EndHour
        => Bits.HighestSetBit(_hours) + 1;

    // Returns null if the time is not continuous.
    public (uint, uint)? StartAndEnd()
    {
        var startHour = FirstHour;
        var endHour   = EndHour;
        var count     = Count;
        if (startHour + count == endHour)
            return (startHour, endHour);

        startHour = Bits.HighestSetBit(~_hours & AllHoursValue) + 1;
        endHour   = Bits.TrailingZeroCount(~_hours);
        if ((startHour + count) % RealTime.HoursPerDay == endHour)
            return (startHour, endHour);

        return null;
    }

    public uint Count
        => Bits.Popcount(_hours);

    public BitfieldUptime Overlap(BitfieldUptime rhs)
        => new(_hours & rhs._hours);

    public bool Overlaps(BitfieldUptime rhs)
        => Overlap(rhs)._hours != 0;

    private static uint NextTime(int currentHour, uint hours)
    {
        if (hours == 0)
            return BadHour;

        var rotatedHours = currentHour == 0 ? hours : (hours >> currentHour) | ((hours << (32 - currentHour)) >> 8);
        return Bits.TrailingZeroCount(rotatedHours);
    }

    public IEnumerable<(uint StartHour, uint EndHour)> AllUptimes()
    {
        if (AlwaysUp())
        {
            yield return (0, 24);

            yield break;
        }

        var firstHour        = FirstHour;
        var currentStartHour = BadHour;
        for (var i = 0u; i < RealTime.HoursPerDay; ++i)
        {
            var actualHour = (firstHour + i) % RealTime.HoursPerDay;
            if ((_hours & (1u << (int) actualHour)) != 0)
            {
                if (currentStartHour is BadHour)
                    currentStartHour = actualHour;
            }
            else if (currentStartHour is not BadHour)
            {
                yield return (currentStartHour, actualHour);

                currentStartHour = BadHour;
            }
        }

        // This should not happen I think?
        if (currentStartHour is not BadHour)
            yield return (currentStartHour, firstHour);
    }

    public uint NextUptime(int currentHour)
        => NextTime(currentHour, _hours);

    public uint NextDowntime(int currentHour)
        => NextTime(currentHour, ~_hours & AllHoursValue);

    public TimeInterval NextUptime(TimeStamp now)
    {
        var hour         = now.CurrentEorzeaHour();
        var nextUptime   = NextUptime(hour);
        var nextDowntime = NextDowntime((int)(hour + nextUptime) % RealTime.HoursPerDay);
        if (nextUptime == BadHour)
            return TimeInterval.Never;
        if (nextDowntime == BadHour)
            return TimeInterval.Always;

        now = now.SyncToEorzeaHour();
        if (nextUptime == 0)
            return new TimeInterval(now, now.AddEorzeaHours(nextDowntime));

        now = now.AddEorzeaHours(nextUptime);
        return new TimeInterval(now, now.AddEorzeaHours(nextDowntime));
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
                    min = (int)i;
                else
                    max = (int)i;
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

    private BitfieldUptime(uint hours)
        => _hours = hours;

    public BitfieldUptime(ushort start, ushort end)
        => _hours = ConvertFromEphemeralTime(start, end);

    public static BitfieldUptime FromHours(uint startHour, uint endHour)
    {
        var hours = 0u;
        startHour %= RealTime.HoursPerDay;
        endHour   %= RealTime.HoursPerDay;
        if (endHour == startHour)
            return AllHours;

        if (endHour < startHour)
        {
            for (; startHour < RealTime.HoursPerDay; ++startHour)
                hours |= 1u << (int)startHour;
            for (startHour = 0; startHour < endHour; ++startHour)
                hours |= 1u << (int)startHour;
        }
        else
        {
            for (; startHour < endHour; ++startHour)
                hours |= 1u << (int)startHour;
        }

        return new BitfieldUptime(hours);
    }

    // Convert the rare pop time given by the table to a bitfield.
    public BitfieldUptime(GatheringRarePopTimeTable table)
    {
        _hours = 0;

        // Convert the time slots to ephemeral format to reuse that function.
        foreach (var (durationBase, startTime) in table.Duration.Zip(table.StartTime))
        {
            if (durationBase == 0)
                continue;

            var duration = durationBase == 160 ? (ushort)200 : durationBase;
            var end      = (ushort)((startTime + duration) % 2400);
            _hours |= ConvertFromEphemeralTime(startTime, end);
        }
    }
}
