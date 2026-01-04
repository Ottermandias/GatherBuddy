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
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
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
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(41419, Patch.GrowingLight) // Stargilt Lobster
            .Bait(data, 36594)
            .Time(1140, 1320)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(41298, Patch.GrowingLight) // Deadwood Shadow
            .Bait(data, 38808)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(41299, Patch.GrowingLight) // Ronkan Bullion
            .Mooch(data, 41302)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(41300, Patch.GrowingLight) // Little Bounty
            .Bait(data, 38808)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(41301, Patch.GrowingLight) // Saint Fathric's Face
            .Bait(data, 38808)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Predators(data, 180, (41300, 3));
        data.Apply(41302, Patch.GrowingLight)  // Golding
            .Bait(data, 38808)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(41407, Patch.GrowingLight) // Hyphalosaurus
            .Mooch(data, 36412)
            .Time(540, 720)
            .ForceLegendary()
            .Predators(data, 90, (36412, 3))
            .Transition(data, 1)
            .Weather(data, 2)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(41408, Patch.GrowingLight) // Gharlichthys
            .Bait(data, 36593)
            .Time(840, 960)
            .ForceLegendary()
            .Predators(data, 300, (36454, 12))
            .Transition(data, 8)
            .Weather(data, 2)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(41409, Patch.GrowingLight) // Snowy Parexus
            .Bait(data, 36591)
            .Time(960, 1440)
            .ForceLegendary()
            .Predators(data, 35, (36458, 3))
            .Transition(data, 2)
            .Weather(data, 15)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(41410, Patch.GrowingLight) // Furcacauda
            .Bait(data, 36590)
            .Time(930, 990)
            .Transition(data, 2)
            .ForceLegendary()
            .Weather(data, 49)
            .Bite(data, HookSet.Precise, BiteType.Legendary);
        data.Apply(41411, Patch.GrowingLight) // Lopoceras Elegans
            .Bait(data, 36595)
            .Time(480, 600)
            .Transition(data, 148)
            .Weather(data, 2)
            .ForceLegendary()
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(41412, Patch.GrowingLight) // Sidereal Whale
            .Mooch(data, 36518)
            .Time(0, 480)
            .ForceLegendary()
            .Predators(data, 600, (36521, 1), (36520, 2), (36519, 3))
            .Transition(data, 49)
            .Weather(data, 149)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
    }
    // @formatter:on
}
