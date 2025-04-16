namespace GatherBuddy.Models;

[Flags]
public enum Effects : uint
{
    None           = 0x00000000,
    Snagging       = 0x00000001,
    Chum           = 0x00000002,
    Intuition      = 0x00000004,
    FishEyes       = 0x00000008,
    IdenticalCast  = 0x00000010,
    SurfaceSlap    = 0x00000020,
    PrizeCatch     = 0x00000040,
    Patience       = 0x00000080,
    Patience2      = 0x00000100,
    Collectible    = 0x00002000,
    Large          = 0x00004000,
    Valid          = 0x00008000,
    Legacy         = 0x00010000,
    BigGameFishing = 0x00020000,
    AmbitiousLure1 = 0x00040000,
    AmbitiousLure2 = 0x00080000,
    ModestLure1    = 0x00100000,
    ModestLure2    = 0x00200000,
    ValidLure      = 0x00400000,
}
