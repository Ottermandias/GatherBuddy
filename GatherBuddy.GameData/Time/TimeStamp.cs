using System;
using System.Globalization;

namespace GatherBuddy.Time;

public readonly struct TimeStamp : IComparable<TimeStamp>, IEquatable<TimeStamp>
{
    public long Time { get; init; }

    public static implicit operator long(TimeStamp ts)
        => ts.Time;

    public TimeStamp()
        : this(0)
    { }

    public TimeStamp(long value)
        => Time = value;

    public static TimeStamp UtcNow
        => new(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

    public static readonly TimeStamp Epoch
        = new(0);

    public static readonly TimeStamp MinValue
        = new(long.MinValue);

    public static readonly TimeStamp MaxValue
        = new(long.MaxValue);

    public TimeStamp AddMilliseconds(long value)
        => new(Time + value);

    public TimeStamp AddSeconds(long value)
        => new(Time + RealTime.MillisecondsPerSecond * value);

    public TimeStamp AddMinutes(long value)
        => new(Time + RealTime.MillisecondsPerMinute * value);

    public TimeStamp AddHours(long value)
        => new(Time + RealTime.MillisecondsPerHour * value);

    public TimeStamp AddDays(long value)
        => new(Time + RealTime.MillisecondsPerDay * value);

    public long TotalSeconds
        => Time / RealTime.MillisecondsPerSecond;

    public long TotalMinutes
        => Time / RealTime.MillisecondsPerMinute;

    public long TotalHours
        => Time / RealTime.MillisecondsPerHour;

    public long TotalDays
        => Time / RealTime.MillisecondsPerDay;

    public int CurrentSecond
        => (int)(TotalSeconds % RealTime.SecondsPerMinute);

    public int CurrentMinute
        => (int)(TotalMinutes % RealTime.MinutesPerHour);

    public int CurrentHour
        => (int)(TotalHours % RealTime.HoursPerDay);

    public int CurrentSecondOfHour
        => (int)(TotalSeconds % RealTime.SecondsPerHour);

    public int CurrentSecondOfDay
        => (int)(TotalSeconds % RealTime.SecondsPerDay);

    public int CurrentMinuteOfDay
        => (int)(TotalMinutes % RealTime.MinutesPerDay);

    public int CompareTo(TimeStamp other)
        => Time.CompareTo(other.Time);

    public bool Equals(TimeStamp other)
        => Time == other.Time;

    public override bool Equals(object? obj)
        => obj is TimeStamp other && Equals(other);

    public override int GetHashCode()
        => Time.GetHashCode();

    public TimeStamp Max(TimeStamp other)
        => Time < other.Time ? other : this;

    public TimeStamp Min(TimeStamp other)
        => Time < other.Time ? this : other;

    public static long operator -(TimeStamp lhs, TimeStamp rhs)
        => lhs.Time - rhs.Time;

    public static TimeStamp operator +(TimeStamp lhs, long offset)
        => new(lhs.Time + offset);

    public static TimeStamp operator +(long offset, TimeStamp rhs)
        => rhs + offset;

    public static TimeStamp operator -(TimeStamp lhs, long offset)
        => new(lhs.Time - offset);

    public static bool operator ==(TimeStamp left, TimeStamp right)
        => left.Time == right.Time;

    public static bool operator !=(TimeStamp left, TimeStamp right)
        => left.Time != right.Time;

    public static bool operator <(TimeStamp left, TimeStamp right)
        => left.Time < right.Time;

    public static bool operator <=(TimeStamp left, TimeStamp right)
        => left.Time <= right.Time;

    public static bool operator >(TimeStamp left, TimeStamp right)
        => left.Time > right.Time;

    public static bool operator >=(TimeStamp left, TimeStamp right)
        => left.Time >= right.Time;

    public DateTime LocalTime
        => DateTimeOffset.FromUnixTimeMilliseconds(Time).LocalDateTime;

    public DateTime DateTime
        => DateTimeOffset.FromUnixTimeMilliseconds(Time).UtcDateTime;

    public override string ToString()
        => LocalTime.ToString(CultureInfo.InvariantCulture);
}
