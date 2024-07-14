using Dalamud.Interface.Textures;
using Dalamud.Plugin.Services;
using GatherBuddy.Enums;
using OtterGui.Classes;

namespace GatherBuddy;

public static class Icons
{
    public static TextureCache            DefaultStorage   = null!;
    public static ISharedImmediateTexture HookSet          = null!;
    public static ISharedImmediateTexture PrecisionHookSet = null!;
    public static ISharedImmediateTexture PowerfulHookSet  = null!;
    public static ISharedImmediateTexture Snagging         = null!;
    public static ISharedImmediateTexture SmallGig         = null!;
    public static ISharedImmediateTexture NormalGig        = null!;
    public static ISharedImmediateTexture LargeGig         = null!;
    public static ISharedImmediateTexture Gigs             = null!;

    public static ISharedImmediateTexture AmbitiousLure = null!;
    public static ISharedImmediateTexture ModestLure = null!;

    public static ISharedImmediateTexture SuperSlow     = null!;
    public static ISharedImmediateTexture ExtremelySlow = null!;
    public static ISharedImmediateTexture VerySlow      = null!;
    public static ISharedImmediateTexture Slow          = null!;
    public static ISharedImmediateTexture Average       = null!;
    public static ISharedImmediateTexture Fast          = null!;
    public static ISharedImmediateTexture VeryFast      = null!;
    public static ISharedImmediateTexture ExtremelyFast = null!;
    public static ISharedImmediateTexture SuperFast     = null!;
    public static ISharedImmediateTexture HyperFast     = null!;
    public static ISharedImmediateTexture LynFast       = null!;
    public static ISharedImmediateTexture UnknownSpeed  = null!;


    public static void Init(IDataManager dataManager, ITextureProvider textures)
    {
        DefaultStorage   = new TextureCache(dataManager, textures);
        HookSet          = textures.GetFromGameIcon(1103);
        PrecisionHookSet = textures.GetFromGameIcon(1116);
        PowerfulHookSet  = textures.GetFromGameIcon(1115);
        Snagging         = textures.GetFromGameIcon(1109);
        AmbitiousLure    = textures.GetFromGameIcon(1146);
        ModestLure       = textures.GetFromGameIcon(1147);
        Gigs             = textures.GetFromGameIcon(60037);
        SmallGig         = textures.GetFromGameIcon(60671);
        NormalGig        = textures.GetFromGameIcon(60672);
        LargeGig         = textures.GetFromGameIcon(60673);
        SuperSlow        = textures.GetFromGameIcon(83101);
        ExtremelySlow    = textures.GetFromGameIcon(83102);
        VerySlow         = textures.GetFromGameIcon(83103);
        Slow             = textures.GetFromGameIcon(83104);
        Average          = textures.GetFromGameIcon(83105);
        Fast             = textures.GetFromGameIcon(83106);
        VeryFast         = textures.GetFromGameIcon(83107);
        ExtremelyFast    = textures.GetFromGameIcon(83108);
        SuperFast        = textures.GetFromGameIcon(83109);
        HyperFast        = textures.GetFromGameIcon(83110);
        LynFast          = textures.GetFromGameIcon(83111);
        UnknownSpeed     = textures.GetFromGameIcon(83121);
    }

    public static ISharedImmediateTexture FromHookSet(HookSet hook)
        => hook switch
        {
            Enums.HookSet.Precise  => PrecisionHookSet,
            Enums.HookSet.Powerful => PowerfulHookSet,
            _                      => HookSet,
        };

    public static ISharedImmediateTexture FromSize(SpearfishSize size)
        => size switch
        {
            SpearfishSize.Small   => SmallGig,
            SpearfishSize.Average => NormalGig,
            SpearfishSize.Large   => LargeGig,
            _                     => Gigs,
        };

    public static ISharedImmediateTexture FromSpeed(SpearfishSpeed speed)
        => speed switch
        {
            SpearfishSpeed.SuperSlow     => SuperSlow,
            SpearfishSpeed.ExtremelySlow => ExtremelySlow,
            SpearfishSpeed.VerySlow      => VerySlow,
            SpearfishSpeed.Slow          => Slow,
            SpearfishSpeed.Average       => Average,
            SpearfishSpeed.Fast          => Fast,
            SpearfishSpeed.VeryFast      => VeryFast,
            SpearfishSpeed.ExtremelyFast => ExtremelyFast,
            SpearfishSpeed.SuperFast     => SuperFast,
            SpearfishSpeed.HyperFast     => HyperFast,
            SpearfishSpeed.LynFast       => LynFast,
            _                            => UnknownSpeed,
        };
}
