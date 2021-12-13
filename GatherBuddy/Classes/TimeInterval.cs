using System;

namespace GatherBuddy.Classes;

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
}
