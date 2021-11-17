using System;

namespace GatherBuddy.Classes;

public readonly struct RepeatingInterval : IEquatable<RepeatingInterval>
{
    public int OnTime    { get; init; }
    public int OffTime   { get; init; }
    public int ShiftTime { get; init; }

    public long Period
        => OnTime + OffTime;

    public bool AlwaysUp()
        => OffTime == 0;

    private TimeStamp SyncToShift(TimeStamp ts)
        => new ((ts - ShiftTime) / Period * Period + ShiftTime);

    public TimeInterval FirstOverlap(TimeInterval interval)
    {
        if (interval == TimeInterval.Invalid)
            return TimeInterval.Invalid;

        if (OnTime == 0)
            return OffTime == 0 ? TimeInterval.Invalid : TimeInterval.Never;
        if (OffTime == 0)
            return interval;

        if (interval == TimeInterval.Always)
            return new TimeInterval
            {
                Start = TimeStamp.Epoch + ShiftTime,
                End   = TimeStamp.Epoch + ShiftTime + OnTime,
            };

        var start = SyncToShift(interval.Start);
        var end   = start + OnTime;
        if (end < interval.Start)
        {
            start += Period;
            end   += Period;
        }

        var newStart = interval.Start.Max(start);
        var newEnd   = interval.End.Min(end);
        return newEnd <= newStart
            ? TimeInterval.Never
            : new TimeInterval(newStart, newEnd);
    }

    public TimeInterval NextRealUptime()
    {
        if (AlwaysUp())
            return TimeInterval.Always;
        if (OnTime == 0)
            return TimeInterval.Never;

        var now       = TimeStamp.UtcNow;
        var syncedNow = SyncToShift(now);
        var end       = syncedNow + OnTime;
        return end > now
            ? new TimeInterval(syncedNow,          end)
            : new TimeInterval(syncedNow + Period, end + Period);
    }

    public static readonly RepeatingInterval Always = new()
    {
        OnTime    = 1,
        OffTime   = 0,
        ShiftTime = 0,
    };

    public static readonly RepeatingInterval Never = new()
    {
        OnTime    = 0,
        OffTime   = 1,
        ShiftTime = 0,
    };

    public static readonly RepeatingInterval Invalid = new()
    {
        OnTime    = 0,
        OffTime   = 0,
        ShiftTime = 0,
    };

    public static bool operator ==(RepeatingInterval left, RepeatingInterval right)
        => left.Equals(right);

    public static bool operator !=(RepeatingInterval left, RepeatingInterval right)
        => !(left == right);

    public bool Equals(RepeatingInterval other)
        => OnTime == other.OnTime
         && OffTime == other.OffTime
         && ShiftTime == other.ShiftTime;

    public override bool Equals(object? obj)
        => obj is RepeatingInterval other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(OnTime, OffTime, ShiftTime);

    public static RepeatingInterval FromEorzeanMinutes(int startMinute, int endMinute)
    {
        if (startMinute == endMinute)
            return Never;

        startMinute %= RealTime.MinutesPerDay;
        endMinute   %= RealTime.MinutesPerDay;
        if (startMinute == endMinute)
            return Always;

        var duration = TimeStamp.Epoch.AddEorzeaMinutes(endMinute < startMinute
            ? endMinute + RealTime.MinutesPerDay - startMinute
            : endMinute - startMinute);
        var offset = TimeStamp.Epoch.AddEorzeaMinutes(startMinute);
        return new RepeatingInterval()
        {
            ShiftTime = (int)(startMinute < endMinute ? offset : offset.AddEorzeaDays(1)),
            OnTime    = (int)duration,
            OffTime   = (int)(TimeStamp.Epoch.AddEorzeaDays(1) - duration),
        };
    }

    // Print eorzean hours in human readable format.
    public string PrintHours(bool simple = false)
    {
        var start = new TimeStamp(ShiftTime).CurrentEorzeaMinuteOfDay();
        if (start < 0)
            start += RealTime.MinutesPerDay;
        var end = start + new TimeStamp(OnTime).TotalEorzeaMinutes();
        if (end > RealTime.MinutesPerDay)
            end -= RealTime.MinutesPerDay;

        var hStart = start / RealTime.MinutesPerHour;
        var hEnd   = end / RealTime.MinutesPerHour;
        var mStart = start - hStart * RealTime.MinutesPerHour;
        var mEnd   = end - hEnd * RealTime.MinutesPerHour;
        var sStart = $"{hStart:D2}:{mStart:D2}";
        var sEnd   = $"{hEnd:D2}:{mEnd:D2}";

        return simple ? $"{sStart}-{sEnd}" : $"{sStart} - {sEnd} ET";
    }
}
