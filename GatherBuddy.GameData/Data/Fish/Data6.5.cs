using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyGrowingLight(this GameData data)
    {
        data.Apply(41058, Patch.GrowingLight) // Ossicula
            .Bait(data, 28634)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(41059, Patch.GrowingLight) // Kitty Herring
            .Bait(data, 28634)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(41060, Patch.GrowingLight) // Empyreal Spiral
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply(41061, Patch.GrowingLight) // Sarikuyruk
            .Bait(data, 28634)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(41062, Patch.GrowingLight) // Opal Tetra
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.Average);
        data.Apply(41401, Patch.GrowingLight) // Circuit Tilapia
            .Mooch(data, 36412)
            .Time(480, 720)
            .Transition(data, 3)
            .Weather(data, 2)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(41402, Patch.GrowingLight) // Durdina Fish
            .Mooch(data, 36426)
            .Time(0, 240)
            .Transition(data, 2)
            .Weather(data, 1)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(41403, Patch.GrowingLight) // Mayaman
            .Bait(data, 36591)
            .Predators(data, 180, (36449, 5))
            .Time(240, 480)
            .Transition(data, 3)
            .Weather(data, 2)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(41404, Patch.GrowingLight) // Chlorophos Deathworm
            .Mooch(data, 36470)
            .Time(120, 480)
            .Transition(data, 2)
            .Weather(data, 148)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(41405, Patch.GrowingLight) // Starscale Ephemeris
            .Bait(data, 36591)
            .Time(1080, 1320)
            .Transition(data, 149)
            .Weather(data, 2)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(41406, Patch.GrowingLight) // E.B.E.-852738
            .Bait(data, 36597)
            .Time(720, 960)
            .Transition(data, 2)
            .Weather(data, 49)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(41419, Patch.GrowingLight) // Stargilt Lobster
            .Bait(data, 36594)
            .Time(960, 1440)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
    }
    // @formatter:on
}
