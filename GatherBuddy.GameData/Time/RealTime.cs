namespace GatherBuddy.Time;

public static class RealTime
{
    public const int MillisecondsPerSecond = 1000;
    public const int SecondsPerMinute      = 60;
    public const int MinutesPerHour        = 60;
    public const int HoursPerDay           = 24;

    public const int MillisecondsPerMinute = MillisecondsPerSecond * SecondsPerMinute;
    public const int MillisecondsPerHour   = MillisecondsPerMinute * MinutesPerHour;
    public const int MillisecondsPerDay    = MillisecondsPerHour * HoursPerDay;

    public const int SecondsPerHour = SecondsPerMinute * MinutesPerHour;
    public const int SecondsPerDay  = SecondsPerHour * HoursPerDay;
    public const int MinutesPerDay  = MinutesPerHour * HoursPerDay;
}
