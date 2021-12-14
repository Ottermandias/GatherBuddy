using System;

namespace GatherBuddy.Config;

[Flags]
public enum JobFlags
{
    Logging      = 0x01,
    Harvesting   = 0x02,
    Mining       = 0x04,
    Quarrying    = 0x08,
    Fishing      = 0x10,
    Spearfishing = 0x20,
}