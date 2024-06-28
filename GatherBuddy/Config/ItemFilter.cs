using System;

namespace GatherBuddy.Config;

[Flags]
public enum ItemFilter
{
    NoItems    = 0,
    Logging    = 0x000001,
    Harvesting = 0x000002,
    Mining     = 0x000004,
    Quarrying  = 0x000008,

    Regular   = 0x000010,
    Ephemeral = 0x000020,
    Unspoiled = 0x000040,
    Legendary = 0x000080,

    ARealmReborn   = 0x000100,
    Heavensward    = 0x000200,
    Stormblood     = 0x000400,
    Shadowbringers = 0x000800,
    Endwalker      = 0x001000,
    Dawntrail      = 0x002000,

    Available    = 0x010000,
    Unavailable  = 0x020000,

    All = 0x073FFF,
}