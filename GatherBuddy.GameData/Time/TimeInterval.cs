using System;
using System.Globalization;

namespace GatherBuddy.Time;

public readonly struct TimeInterval : IEquatable<TimeInterval>
{
    public TimeStamp Start { get; init; }
    public TimeStamp End   { get; init; }

    public TimeInterval(TimeStamp start, TimeStamp end)
    {
        Start = start;
        End   = end;
    }

    public long Duration
        => this == Always ? long.MaxValue : this == Invalid ? 0 : End - Start;

    public long SecondDuration
        => this == Always ? long.MaxValue : this == Invalid ? 0 : (End - Start) / RealTime.MillisecondsPerSecond;

    public string DurationString(bool shortString = false)
        => DurationString(Start, End, shortString);

    public TimeInterval Overlap(TimeInterval rhs)
    {
        if (rhs == Invalid || this == Invalid)
            return Invalid;

        var newStart = Start.Max(rhs.Start);
        var newEnd   = End.Min(rhs.End);
        return newEnd <= newStart ? Never : new TimeInterval(newStart, newEnd);
    }

    public TimeInterval FirstOverlap(RepeatingInterval rhs)
        => rhs.FirstOverlap(this);

    public TimeInterval Merge(TimeInterval rhs)
    {
        if (rhs.Start > End || Start > rhs.End || this == Invalid || rhs == Invalid)
            return Invalid;

        var newStart = Start.Min(rhs.Start);
        var newEnd   = End.Max(rhs.End);
        return new TimeInterval(newStart, newEnd);
    }

    public TimeInterval Extend(long duration)
    {
        if (duration == 0)
            return this;
        if (this == Always)
            return Always;
        if (this == Invalid)
            return Invalid;
        if (this == Never)
            return Never;

        return duration > 0
            ? new TimeInterval(Start,            End + duration)
            : new TimeInterval(Start + duration, End);
    }

    public bool this[TimeStamp timeStamp]
        => InRange(timeStamp);

    public bool InRange(TimeStamp timeStamp)
        => timeStamp >= Start && timeStamp < End;

    public static readonly TimeInterval Always = new(TimeStamp.MinValue, TimeStamp.MaxValue);

    public static readonly TimeInterval Never = new(TimeStamp.Epoch, TimeStamp.Epoch);

    public static readonly TimeInterval Invalid = new(TimeStamp.MaxValue, TimeStamp.MinValue);

    public bool Equals(TimeInterval other)
        => Start == other.Start
         && End == other.End;

    public override bool Equals(object? obj)
        => obj is TimeInterval other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Start, End);

    public static bool operator ==(TimeInterval left, TimeInterval right)
        => left.Equals(right);

    public static bool operator !=(TimeInterval left, TimeInterval right)
        => !(left == right);

    public int Compare(TimeInterval rhs)
    {
        if (this == Invalid)
            return Never.Compare(rhs);

        if (rhs == Invalid)
            rhs = Never;

        var diff = End - rhs.End;
        if (Math.Abs(diff) > 0)
            return diff.CompareTo(0);

        var diff2 = Start - rhs.Start;
        return diff2.CompareTo(0);
    }

    public static string DurationString(TimeStamp a, TimeStamp b, bool shortString)
    {
        (a, b) = a < b ? (a, b) : (b, a);
        var tmp = new TimeStamp(b - a);
        return tmp.Time switch
        {
            > RealTime.MillisecondsPerDay => shortString
                ? $">{tmp.TotalDays}d"
                : $"{((float)tmp.Time / RealTime.MillisecondsPerDay).ToString("F2", CultureInfo.InvariantCulture)} Days",
            > RealTime.MillisecondsPerHour => shortString
                ? $">{tmp.TotalHours}h"
                : $"{tmp.TotalHours:D2}:{tmp.CurrentMinute:D2} Hours",
            _ => shortString
                ? $"{tmp.TotalMinutes}:{tmp.CurrentSecond:D2}m"
                : $"{tmp.TotalMinutes:D2}:{tmp.CurrentSecond:D2} Minutes",
        };
    }

    // Returns true if currently active and false otherwise.
    public bool ToTimeString(TimeStamp now, bool shortString, out string timeString)
    {
        if (this == Always)
        {
            timeString = "Always";
            return true;
        }

        if (this == Never)
        {
            timeString = "Never";
            return false;
        }

        if (this == Invalid)
        {
            timeString = "Invalid";
            return false;
        }

        if (Start < now)
        {
            if (End < now)
            {
                timeString = "Never";
                return false;
            }

            timeString = DurationString(End, now, shortString);
            return true;
        }

        timeString = DurationString(Start, now, shortString);
        return false;
    }
}
