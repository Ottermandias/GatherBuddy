using System;
using System.Globalization;

namespace GatherBuddy.Classes;

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

public static class RealTime
{
    public const int MillisecondsPerSecond = 1000;
    public const int SecondsPerMinute      = 60;
    public const int MinutesPerHour        = 60;
    public const int HoursPerDay           = 24;

    public const int MillisecondsPerMinute = MillisecondsPerSecond * SecondsPerMinute;
    public const int MillisecondsPerHour   = MillisecondsPerMinute * MinutesPerHour;
    public const int MillisecondsPerDay    = MillisecondsPerMinute * HoursPerDay;

    public const int SecondsPerHour = SecondsPerMinute * MinutesPerHour;
    public const int SecondsPerDay  = SecondsPerHour * HoursPerDay;
    public const int MinutesPerDay  = MinutesPerHour * HoursPerDay;
}

public static class EorzeaTimeStampExtensions
{
    public const int MillisecondsPerEorzeaHour    = 175000;
    public const int SecondsPerEorzeaHour         = MillisecondsPerEorzeaHour / RealTime.MillisecondsPerSecond;
    public const int MillisecondsPerEorzeaWeather = 8 * MillisecondsPerEorzeaHour;
    public const int SecondsPerEorzeaWeather      = MillisecondsPerEorzeaWeather / RealTime.MillisecondsPerSecond;
    public const int MillisecondsPerEorzeaDay     = RealTime.HoursPerDay * MillisecondsPerEorzeaHour;
    public const int SecondsPerEorzeaDay          = MillisecondsPerEorzeaDay / RealTime.MillisecondsPerSecond;

    public static TimeStamp AddEorzeaSeconds(this TimeStamp t, long value)
        => new(t.Time + value * 875 / 18);

    public static TimeStamp AddEorzeaMinutes(this TimeStamp t, long value)
        => new(t.Time + value * 8750 / 3);

    public static TimeStamp AddEorzeaHours(this TimeStamp t, long value)
        => new(t.Time + value * 175000);

    public static TimeStamp AddEorzeaDays(this TimeStamp t, long value)
        => new(t.Time + value * 4200000);

    public static long TotalEorzeaMinutes(this TimeStamp t)
        => t.Time * 144 / 7 / RealTime.MillisecondsPerMinute;

    public static long TotalEorzeaHours(this TimeStamp t)
        => t.Time * 144 / 7 / RealTime.MillisecondsPerHour;

    public static long TotalEorzeaDays(this TimeStamp t)
        => t.Time * 144 / 7 / RealTime.MillisecondsPerDay;

    public static int CurrentEorzeaMinute(this TimeStamp t)
        => (int)(t.TotalEorzeaMinutes() % RealTime.MinutesPerHour);

    public static int CurrentEorzeaMinuteOfDay(this TimeStamp t)
        => (int)(t.TotalEorzeaMinutes() % RealTime.MinutesPerDay);

    public static int CurrentEorzeaHour(this TimeStamp t)
        => (int)(t.TotalEorzeaHours() % RealTime.HoursPerDay);

    public static TimeStamp SyncToEorzeaHour(this TimeStamp now)
        => new(now.Time - now.Time % MillisecondsPerEorzeaHour);

    public static TimeStamp SyncToEorzeaDay(this TimeStamp now)
        => new(now.Time - now.Time % MillisecondsPerEorzeaDay);

    public static TimeStamp SyncToEorzeaWeather(this TimeStamp now)
        => new(now.Time - now.Time % MillisecondsPerEorzeaWeather);

    public static (int Hours, int Minutes) CurrentEorzeaTimeOfDay(this TimeStamp now, int eorzeaMinuteOffset = 0)
    {
        var minutes = now.TotalEorzeaMinutes() + eorzeaMinuteOffset;
        return ((int)(minutes / RealTime.MinutesPerHour) % RealTime.HoursPerDay, (int)minutes % RealTime.MinutesPerHour);
    }

    public static (long minutes, int seconds) MinutesToReal(long eorzeaMinutes)
    {
        var seconds = (double)eorzeaMinutes * SecondsPerEorzeaHour / RealTime.MinutesPerHour;
        var m       = (long)seconds;
        var s       = (int)((seconds - m) * RealTime.SecondsPerMinute);
        return (m, s);
    }
}
