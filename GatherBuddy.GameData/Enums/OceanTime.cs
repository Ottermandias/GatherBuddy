using System;
using System.Collections.Generic;

namespace GatherBuddy.Enums;

// Ordered by the progression of time throughout a fishing route.
[Flags]
public enum OceanTime : byte
{
    Never  = 0,
    Sunset = 0x01,
    Night  = 0x02,
    Day    = 0x04,

    Always = Sunset | Night | Day,
}

public static class OceanTimeExtensions
{
    public static OceanTime Next(this OceanTime time)
        => time switch
        {
            OceanTime.Sunset => OceanTime.Night,
            OceanTime.Night  => OceanTime.Day,
            OceanTime.Day    => OceanTime.Sunset,
            _                => OceanTime.Sunset,
        };

    public static OceanTime Previous(this OceanTime time)
        => time switch
        {
            OceanTime.Sunset => OceanTime.Day,
            OceanTime.Night  => OceanTime.Sunset,
            OceanTime.Day    => OceanTime.Night,
            _                => OceanTime.Sunset,
        };

    public static IEnumerable<OceanTime> Enumerate(this OceanTime time)
    {
        if (time.HasFlag(OceanTime.Sunset))
            yield return OceanTime.Sunset;
        if (time.HasFlag(OceanTime.Night))
            yield return OceanTime.Night;
        if (time.HasFlag(OceanTime.Day))
            yield return OceanTime.Day;
    }
}
