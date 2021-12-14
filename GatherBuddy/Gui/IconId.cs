using System;
using GatherBuddy.Enums;
using ImGuiScene;

namespace GatherBuddy.Gui;

public static class IconId
{
    public const uint HookSet          = 1103;
    public const uint PowerfulHookSet  = 1115;
    public const uint PrecisionHookSet = 1116;
    public const uint Snagging         = 1109;
    public const uint Gigs             = 60037;
    public const uint SmallGig         = 60671;
    public const uint NormalGig        = 60672;
    public const uint LargeGig         = 60673;

    public static TextureWrap FromHookSet(HookSet hook)
        => hook switch
        {
            Enums.HookSet.Precise  => Icons.DefaultStorage[PrecisionHookSet],
            Enums.HookSet.Powerful => Icons.DefaultStorage[PowerfulHookSet],
            _                      => Icons.DefaultStorage[HookSet],
        };

    public static TextureWrap FromSize(SpearfishSize size)
        => size switch
        {
            SpearfishSize.Small   => Icons.DefaultStorage[SmallGig],
            SpearfishSize.Average => Icons.DefaultStorage[NormalGig],
            SpearfishSize.Large   => Icons.DefaultStorage[LargeGig],
            _                     => Icons.DefaultStorage[Gigs],
        };

    public static TextureWrap FromSpeed(SpearfishSpeed speed)
        => speed switch
        {
            SpearfishSpeed.SuperSlow     => Icons.DefaultStorage[83101],
            SpearfishSpeed.ExtremelySlow => Icons.DefaultStorage[83102],
            SpearfishSpeed.VerySlow      => Icons.DefaultStorage[83103],
            SpearfishSpeed.Slow          => Icons.DefaultStorage[83104],
            SpearfishSpeed.Average       => Icons.DefaultStorage[83105],
            SpearfishSpeed.Fast          => Icons.DefaultStorage[83106],
            SpearfishSpeed.VeryFast      => Icons.DefaultStorage[83107],
            SpearfishSpeed.ExtremelyFast => Icons.DefaultStorage[83108],
            SpearfishSpeed.SuperFast     => Icons.DefaultStorage[83109],
            SpearfishSpeed.HyperFast     => Icons.DefaultStorage[83110],
            SpearfishSpeed.LynFast       => Icons.DefaultStorage[83111],
            _                            => Icons.DefaultStorage[83121],
        };

    public static TextureWrap GetSnagging()
        => Icons.DefaultStorage[Snagging];
}
