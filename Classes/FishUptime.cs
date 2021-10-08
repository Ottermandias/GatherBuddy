using System;
using System.Diagnostics;
using System.Globalization;
using GatherBuddy.Utility;

namespace GatherBuddy.Classes
{
    public readonly struct FishUptime
    {
        private const    uint       BadHour       = RealTime.HoursPerDay + 1;
        private const    uint       AllHoursValue = RealTime.MinutesPerDay << 16;
        private readonly uint       _fromFor; // two ushorts, first start time in minute of day, second length in minutes.
        public static    FishUptime AllTime { get; } = new(AllHoursValue);
        public static    FishUptime NoHours { get; } = new(0);

        private FishUptime(uint value)
        {
            Debug.Assert((value & 0xFFFF) < RealTime.MinutesPerDay);
            Debug.Assert(value >> 16 <= RealTime.MinutesPerDay);
            _fromFor = value;
        }

        public uint StartMinute
            => _fromFor & 0xFFFF;

        public uint Duration
            => _fromFor >> 16;

        public uint EndMinute
            => StartAndEnd().Item2;

        public float FirstHour
            => (float) StartMinute / RealTime.MinutesPerHour;

        public float EndHour
            => (float) EndMinute / RealTime.MinutesPerHour;

        public (uint, uint) StartAndEnd()
        {
            var start = StartMinute;
            var end   = start + Duration;
            if (end > RealTime.MinutesPerDay)
                end -= RealTime.MinutesPerDay;
            return (start, end);
        }

        public FishUptime Overlap(FishUptime rhs)
        {
            var start1 = StartMinute;
            var end1   = start1 + Duration;
            var start2 = rhs.StartMinute;
            var end2   = start2 + rhs.Duration;
            start1 = Math.Max(start1, start2);
            end1   = Math.Min(end1, end2);
            if (end1 <= start1)
                return NoHours;

            return new FishUptime(start1, end1 - start1);
        }

        public bool Overlaps(FishUptime rhs)
            => !Overlap(rhs).Equals(NoHours);

        public bool Equals(FishUptime rhs)
            => _fromFor == rhs._fromFor;

        public bool AlwaysUp()
            => _fromFor == AllHoursValue;

        public FishUptime(uint startMinute, uint length)
        {
            if (length >= RealTime.MinutesPerDay)
            {
                _fromFor = AllHoursValue;
            }
            else
            {
                if (startMinute > RealTime.MinutesPerDay)
                    startMinute = (ushort) (startMinute % RealTime.MinutesPerDay);
                _fromFor = startMinute | ((uint) length << 16);
            }
        }

        public static FishUptime FromStartEnd(uint startMinute, uint endMinute)
            => new(startMinute, endMinute > startMinute ? endMinute - startMinute : RealTime.MinutesPerDay - startMinute + endMinute);

        public static FishUptime FromStartEnd(float startHour, float endHour)
        {
            var startMinute = (uint) (startHour * RealTime.MinutesPerHour + 0.5f);
            var endMinute   = (uint) (endHour * RealTime.MinutesPerHour + 0.5f);
            return FromStartEnd(startMinute, endMinute);
        }

        private static uint NextTime(uint currentHour, uint hours)
        {
            if (hours == 0)
                return BadHour;

            var rotatedHours = currentHour == 0 ? hours : (hours >> (int) currentHour) | ((hours << (32 - (int) currentHour)) >> 8);
            return Util.TrailingZeroCount(rotatedHours);
        }

        private uint NextTime(uint currentMinute, uint start, uint end)
        {
            if (end > start)
            {
                if (start <= currentMinute)
                    return end > currentMinute ? 0 : start + RealTime.MinutesPerDay - currentMinute;

                return start - currentMinute;
            }

            if (currentMinute >= start || currentMinute < end)
                return 0;

            return start - currentMinute;
        }

        public uint NextUptime(uint currentMinute)
        {
            var (start, end) = StartAndEnd();
            return NextTime(currentMinute, start, end);
        }

        public uint NextDowntime(uint currentMinute)
        {
            var (start, end) = StartAndEnd();
            return NextTime(currentMinute, end, start);
        }

        public RealUptime NextRealUptime()
        {
            if (AlwaysUp())
                return RealUptime.Always;
            if (Equals(NoHours))
                return RealUptime.Never;

            var now       = DateTime.UtcNow;
            var syncedNow = EorzeaTime.SyncToEorzeaDay(now);
            var start     = StartMinute;
            var duration  = Duration;
            var end       = StartMinute + Duration;
            var startTime = syncedNow.AddSeconds((start - (end > RealTime.MinutesPerDay ? RealTime.MinutesPerDay : 0))
              * EorzeaTime.SecondsPerEorzeaMinute);

            var endTime = startTime.AddSeconds(duration * EorzeaTime.SecondsPerEorzeaMinute);
            if (now > endTime)
            {
                startTime = startTime.AddSeconds(RealTime.MinutesPerDay * EorzeaTime.SecondsPerEorzeaMinute);
                endTime = endTime.AddSeconds(RealTime.MinutesPerDay * EorzeaTime.SecondsPerEorzeaMinute);
            }

            return new RealUptime(startTime, endTime);
        }

        // Print hours in human readable format.
        public string PrintHours(bool simple = false)
        {
            var (start, end) = StartAndEnd();
            var hStart = start / RealTime.MinutesPerHour;
            var hEnd   = end / RealTime.MinutesPerHour;
            var mStart = start - hStart * RealTime.MinutesPerHour;
            var mEnd   = end - hEnd * RealTime.MinutesPerHour;
            var sStart = $"{hStart:D2}:{mStart:D2}";
            var sEnd   = $"{hEnd:D2}:{mEnd:D2}";

            return simple ? $"{sStart}-{sEnd}" : $"{sStart} - {sEnd} ET";
        }

        // For Weather.
        public FishUptime(DateTime time)
        {
            var hour = EorzeaTime.HourOfDay(time);
            var startTime = (hour / 8) switch
            {
                0 => 0,
                1 => 8 * RealTime.MinutesPerHour,
                2 => 16 * RealTime.MinutesPerHour,
                _ => RealTime.MinutesPerDay,
            };
            _fromFor = (uint) (startTime | ((8 * RealTime.MinutesPerHour) << 16));
        }
    }
}
