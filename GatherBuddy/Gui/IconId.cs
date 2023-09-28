using Dalamud.Interface.Internal;
using GatherBuddy.Enums;

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

    public static IDalamudTextureWrap FromHookSet(HookSet hook)
        => hook switch
        {
            Enums.HookSet.Precise  => Icons.DefaultStorage.LoadIcon(PrecisionHookSet, true),
            Enums.HookSet.Powerful => Icons.DefaultStorage.LoadIcon(PowerfulHookSet,  true),
            _                      => Icons.DefaultStorage.LoadIcon(HookSet,          true),
        };

    public static IDalamudTextureWrap FromSize(SpearfishSize size)
        => size switch
        {
            SpearfishSize.Small   => Icons.DefaultStorage.LoadIcon(SmallGig,  true),
            SpearfishSize.Average => Icons.DefaultStorage.LoadIcon(NormalGig, true),
            SpearfishSize.Large   => Icons.DefaultStorage.LoadIcon(LargeGig,  true),
            _                     => Icons.DefaultStorage.LoadIcon(Gigs,      true),
        };

    public static IDalamudTextureWrap FromSpeed(SpearfishSpeed speed)
        => speed switch
        {
            SpearfishSpeed.SuperSlow     => Icons.DefaultStorage.LoadIcon(83101, true),
            SpearfishSpeed.ExtremelySlow => Icons.DefaultStorage.LoadIcon(83102, true),
            SpearfishSpeed.VerySlow      => Icons.DefaultStorage.LoadIcon(83103, true),
            SpearfishSpeed.Slow          => Icons.DefaultStorage.LoadIcon(83104, true),
            SpearfishSpeed.Average       => Icons.DefaultStorage.LoadIcon(83105, true),
            SpearfishSpeed.Fast          => Icons.DefaultStorage.LoadIcon(83106, true),
            SpearfishSpeed.VeryFast      => Icons.DefaultStorage.LoadIcon(83107, true),
            SpearfishSpeed.ExtremelyFast => Icons.DefaultStorage.LoadIcon(83108, true),
            SpearfishSpeed.SuperFast     => Icons.DefaultStorage.LoadIcon(83109, true),
            SpearfishSpeed.HyperFast     => Icons.DefaultStorage.LoadIcon(83110, true),
            SpearfishSpeed.LynFast       => Icons.DefaultStorage.LoadIcon(83111, true),
            _                            => Icons.DefaultStorage.LoadIcon(83121, true),
        };

    public static IDalamudTextureWrap GetSnagging()
        => Icons.DefaultStorage.LoadIcon(Snagging, true);
}
