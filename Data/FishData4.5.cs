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
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (24558, Patch.ARequiemForHeroes) // Ruby Laver
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (24559, Patch.ARequiemForHeroes) // Chakrafish
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24560, Patch.ARequiemForHeroes) // Wicked Wartfish
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24561, Patch.ARequiemForHeroes) // Othardian Salmon
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24881, Patch.ARequiemForHeroes) // Lily of the Veil
                .Bait      (fish, 20619)
                .Tug       (BiteType.Legendary)
                .Transition(1)
                .Weather   (2)
                .HookType  (HookSet.Precise);
            fish.Apply     (24882, Patch.ARequiemForHeroes) // The Vegetarian
                .Bait      (fish, 20617, 20112)
                .Tug       (BiteType.Legendary)
                .Uptime    (1200, 1440)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24883, Patch.ARequiemForHeroes) // Seven Stars
                .Bait      (fish, 20676)
                .Tug       (BiteType.Legendary)
                .Uptime    (600, 1080)
                .Transition(9)
                .Weather   (2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24884, Patch.ARequiemForHeroes) // Pinhead
                .Bait      (fish, 20617)
                .Tug       (BiteType.Legendary)
                .Uptime    (960, 1440)
                .Transition(2, 1)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24885, Patch.ARequiemForHeroes) // Pomegranate Trout
                .Bait      (fish, 20614)
                .Tug       (BiteType.Legendary)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24886, Patch.ARequiemForHeroes) // Glarramundi
                .Bait      (fish, 20619)
                .Tug       (BiteType.Legendary)
                .Transition(2)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24887, Patch.ARequiemForHeroes) // Hermit's End
                .Bait      (fish, 20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (1200, 1440)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24888, Patch.ARequiemForHeroes) // Suiten Ippeki
                .Bait      (fish, 20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (960, 1440)
                .Transition(2)
                .Weather   (8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24889, Patch.ARequiemForHeroes) // Axelrod
                .Bait      (fish, 20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 480)
                .Transition(3)
                .Weather   (2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24890, Patch.ARequiemForHeroes) // The Unraveled Bow
                .Bait      (fish, 20619)
                .Tug       (BiteType.Legendary)
                .Uptime    (720, 960)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24891, Patch.ARequiemForHeroes) // Nhaama's Treasure
                .Bait      (fish, 20619)
                .Tug       (BiteType.Legendary)
                .Uptime    (240, 480)
                .Transition(7)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24892, Patch.ARequiemForHeroes) // Garden Skipper
                .Bait      (fish, 20615)
                .Tug       (BiteType.Legendary)
                .Uptime    (480, 720)
                .Transition(2)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (24893, Patch.ARequiemForHeroes) // Banderole
                .Bait      (fish, 20614, 20127)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 480)
                .Transition(2)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24990, Patch.ARequiemForHeroes) // Xenacanthus
                .Bait      (fish, 20675, 24207)
                .Tug       (BiteType.Legendary)
                .Uptime    (960, 1200)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24991, Patch.ARequiemForHeroes) // Drepanaspis
                .Bait      (fish, 20619)
                .Tug       (BiteType.Legendary)
                .Predators (fish, (23060, 2))
                .Intuition (175)
                .Weather   (11)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24992, Patch.ARequiemForHeroes) // Stethacanthus
                .Bait      (fish, 20616, 20025)
                .Tug       (BiteType.Legendary)
                .Uptime    (960, 1080)
                .Predators (fish, (20040, 2))
                .Intuition (350)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24993, Patch.ARequiemForHeroes) // The Ruby Dragon
                .Bait      (fish, 20676, 24214)
                .Tug       (BiteType.Legendary)
                .Uptime    (240, 480)
                .Transition(9)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24994, Patch.ARequiemForHeroes) // Warden of the Seven Hues
                .Bait      (fish, 20675)
                .Tug       (BiteType.Legendary)
                .Predators (fish, (23056, 3), (24203, 3), (24204, 5))
                .Intuition (175)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24995, Patch.ARequiemForHeroes) // The Unconditional
                .Bait      (fish, 20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (330, 390)
                .Transition(7)
                .Weather   (1)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
