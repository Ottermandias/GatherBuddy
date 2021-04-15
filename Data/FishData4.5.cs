using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyARequiemForHeroes(this FishManager fish)
        {
            fish.Apply     (24557, Patch.ARequiemForHeroes) // Northern Oyster
                .Bait      (28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (24558, Patch.ARequiemForHeroes) // Ruby Laver
                .Bait      (20617);
            fish.Apply     (24559, Patch.ARequiemForHeroes) // Chakrafish
                .Bait      (20613)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24560, Patch.ARequiemForHeroes) // Wicked Wartfish
                .Bait      (20618)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24561, Patch.ARequiemForHeroes) // Othardian Salmon
                .Bait      (20614);
            fish.Apply     (24881, Patch.ARequiemForHeroes) // Lily of the Veil
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Transition(1)
                .Weather   (2)
                .HookType  (HookSet.Precise);
            fish.Apply     (24882, Patch.ARequiemForHeroes) // The Vegetarian
                .Bait      (20617, 20112)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 24)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24883, Patch.ARequiemForHeroes) // Seven Stars
                .Bait      (20676)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 18)
                .Transition(9)
                .Weather   (2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24884, Patch.ARequiemForHeroes) // Pinhead
                .Bait      (20617)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 24)
                .Transition(2, 1)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24885, Patch.ARequiemForHeroes) // Pomegranate Trout
                .Bait      (20614)
                .Tug       (BiteType.Legendary)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24886, Patch.ARequiemForHeroes) // Glarramundi
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Transition(2)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24887, Patch.ARequiemForHeroes) // Hermit's End
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 24)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24888, Patch.ARequiemForHeroes) // Suiten Ippeki
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 24)
                .Transition(2)
                .Weather   (8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24889, Patch.ARequiemForHeroes) // Axelrod
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 8)
                .Transition(3)
                .Weather   (2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24890, Patch.ARequiemForHeroes) // The Unraveled Bow
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 16)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24891, Patch.ARequiemForHeroes) // Nhaama's Treasure
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 8)
                .Transition(7)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24892, Patch.ARequiemForHeroes) // Garden Skipper
                .Bait      (20615)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 12)
                .Transition(2)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (24893, Patch.ARequiemForHeroes) // Banderole
                .Bait      (20614, 20127)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 8)
                .Transition(2)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24990, Patch.ARequiemForHeroes) // Xenacanthus
                .Bait      (20675, 24207)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 20)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24991, Patch.ARequiemForHeroes) // Drepanaspis
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Predators ((23060, 2))
                .Weather   (11)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24992, Patch.ARequiemForHeroes) // Stethacanthus
                .Bait      (20616, 20025)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 18)
                .Predators ((20040, 2))
                .HookType  (HookSet.Powerful);
            fish.Apply     (24993, Patch.ARequiemForHeroes) // The Ruby Dragon
                .Bait      (20676, 24214)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 8)
                .Transition(9)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24994, Patch.ARequiemForHeroes) // Warden of the Seven Hues
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Predators ((23056, 3), (24203, 3), (24204, 5))
                .HookType  (HookSet.Powerful);
            fish.Apply     (24995, Patch.ARequiemForHeroes) // The Unconditional
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (5, 7)
                .Transition(7)
                .Weather   (1)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
