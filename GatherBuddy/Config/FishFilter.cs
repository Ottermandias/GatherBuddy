using System;

namespace GatherBuddy.Config;

[Flags]
public enum FishFilter
{
    NoFish       = 0,
    OceanFish    = 0x0001,
    Spearfishing = 0x0002,
    SmallFish    = 0x0004,
    BigFish      = 0x0008,

    AlreadyCaught = 0x0010,
    Uncaught      = 0x0020,
    NotInLog      = 0x0040,

    TimeDependency    = 0x0100,
    WeatherDependency = 0x0200,
    FishDependency    = 0x0400,
    NoDependency      = 0x0800,

    Available   = 0x1000,
    Unavailable = 0x2000,

    All = 0x3F7F,
}
