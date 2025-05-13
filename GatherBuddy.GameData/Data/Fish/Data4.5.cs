using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyARequiemForHeroes(this GameData data)
    {
        data.Apply     (24557, Patch.ARequiemForHeroes) // Northern Oyster
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (24558, Patch.ARequiemForHeroes) // Ruby Laver
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (24559, Patch.ARequiemForHeroes) // Chakrafish
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (24560, Patch.ARequiemForHeroes) // Wicked Wartfish
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (24561, Patch.ARequiemForHeroes) // Othardian Salmon
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (24881, Patch.ARequiemForHeroes) // Lily of the Veil
            .Bait      (data, 20619)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Transition(data, 1)
            .Weather   (data, 2);
        data.Apply     (24882, Patch.ARequiemForHeroes) // The Vegetarian
            .Mooch     (data, 20617, 20112)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (1200, 1440)
            .Weather   (data, 9);
        data.Apply     (24883, Patch.ARequiemForHeroes) // Seven Stars
            .Bait      (data, 20676)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (600, 1080)
            .Transition(data, 9)
            .Weather   (data, 2);
        data.Apply     (24884, Patch.ARequiemForHeroes) // Pinhead
            .Bait      (data, 20617)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 1440)
            .Transition(data, 2, 1)
            .Weather   (data, 9);
        data.Apply     (24885, Patch.ARequiemForHeroes) // Pomegranate Trout
            .Bait      (data, 20614)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 7);
        data.Apply     (24886, Patch.ARequiemForHeroes) // Glarramundi
            .Bait      (data, 20619)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 2)
            .Weather   (data, 1);
        data.Apply     (24887, Patch.ARequiemForHeroes) // Hermit's End
            .Bait      (data, 20675)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (1200, 1440)
            .Weather   (data, 1);
        data.Apply     (24888, Patch.ARequiemForHeroes) // Suiten Ippeki
            .Bait      (data, 20675)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 1440)
            .Transition(data, 2)
            .Weather   (data, 8);
        data.Apply     (24889, Patch.ARequiemForHeroes) // Axelrod
            .Bait      (data, 20675)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (0, 480)
            .Transition(data, 3)
            .Weather   (data, 2);
        data.Apply     (24890, Patch.ARequiemForHeroes) // The Unraveled Bow
            .Bait      (data, 20619)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (720, 960)
            .Weather   (data, 4);
        data.Apply     (24891, Patch.ARequiemForHeroes) // Nhaama's Treasure
            .Bait      (data, 20619)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (240, 480)
            .Transition(data, 7)
            .Weather   (data, 2, 1);
        data.Apply     (24892, Patch.ARequiemForHeroes) // Garden Skipper
            .Bait      (data, 20615)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Time      (480, 720)
            .Transition(data, 2)
            .Weather   (data, 1);
        data.Apply     (24893, Patch.ARequiemForHeroes) // Banderole
            .Mooch     (data, 20614, 20127)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (0, 480)
            .Transition(data, 2)
            .Weather   (data, 4);
        data.Apply     (24990, Patch.ARequiemForHeroes) // Xenacanthus
            .Mooch     (data, 20675, 24207)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .ForceLegendary()
            .Time      (960, 1200);
        data.Apply     (24991, Patch.ARequiemForHeroes) // Drepanaspis
            .Bait      (data, 20619)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 175, (23060, 2))
            .ForceLegendary()
            .Weather   (data, 11);
        data.Apply     (24992, Patch.ARequiemForHeroes) // Stethacanthus
            .Mooch     (data, 20616, 20025)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 1080)
            .ForceLegendary()
            .Predators (data, 350, (20040, 2));
        data.Apply     (24993, Patch.ARequiemForHeroes) // The Ruby Dragon
            .Mooch     (data, 20676, 24214)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (240, 480)
            .Transition(data, 9)
            .ForceLegendary()
            .Weather   (data, 3);
        data.Apply     (24994, Patch.ARequiemForHeroes) // Warden of the Seven Hues
            .Bait      (data, 20675)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .ForceLegendary()
            .Predators (data, 175, (23056, 3), (24203, 3), (24204, 5));
        data.Apply     (24995, Patch.ARequiemForHeroes) // The Unconditional
            .Bait      (data, 20675)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Time      (330, 390)
            .ForceLegendary()
            .Transition(data, 7)
            .Weather   (data, 1);
    }
    // @formatter:on
}
