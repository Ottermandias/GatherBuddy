using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyCrossroads(this GameData data)
    {
        data.Apply(44339, Patch.Crossroads) // Icuvlo's Barter
            .Bait(data, 43858)
            .Bite(data, HookSet.Precise, BiteType.Legendary)
            .Weather(data, 4); 
        data.Apply(44340, Patch.Crossroads) // Moongripper
            .Bait(data, 43854)
            .Bite(data, HookSet.Precise, BiteType.Legendary)
            .Lure(Enums.Lure.Modest)
            .Time(720, 840);
        data.Apply(44341, Patch.Crossroads) // Cazuela Crab
            .Bait(data, 43858)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Time(960, 1200)
            .Weather(data, 3, 4);
        data.Apply(44342, Patch.Crossroads) // Stardust Sleeper
            .Mooch(data, 43747)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Time(1200, 1440);
        data.Apply(44343, Patch.Crossroads) // Ilyon Asoh Cichlid
            .Bait(data, 43858)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 1);
        data.Apply(44344, Patch.Crossroads) // Pixel Loach
            .Bait(data, 43858)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 2)
            .Time(0, 240);
        data.Apply(44345, Patch.Crossroads) // Hwittayoanaan Cichlid
            .Bait(data, 43858)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 1, 2)
            .Time(240, 480);
        data.Apply(44346, Patch.Crossroads) // Thunderswift Trout
            .Bait(data, 43858)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Lure(Enums.Lure.Ambitious)
            .Time(540, 660);
        data.Apply(44347, Patch.Crossroads) // Cloudsail
            .Bait(data, 43853)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(44334, Patch.Crossroads) // Oily Ropefish
            .Bait(data, 28634)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(44336, Patch.Crossroads) // Yak Awak Core
            .Bait(data, 28634)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(44336, Patch.Crossroads) // Inktopus
            .Bait(data, 28634)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(44337, Patch.Crossroads) // Honeycomb Sponge
            .Bait(data, 28634)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(44338, Patch.Crossroads) // Cenote Oyster
            .Bait(data, 28634)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(44335, Patch.Crossroads) // Yak Awak Core
            .Bait(data, 28634)
            .Bite(data, HookSet.Precise, BiteType.Weak);
    }
    // @formatter:on
}
