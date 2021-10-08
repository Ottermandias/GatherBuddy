using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyTheLegendReturns(this FishManager fish)
        {
            fish.Apply     (20785, Patch.TheLegendReturns) // Gyr Abanian Chub
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (20786, Patch.TheLegendReturns) // Coral Horse
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (20787, Patch.TheLegendReturns) // Maiden's Heart
                .Bait      (fish, 20614)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (20788, Patch.TheLegendReturns) // Velodyna Salmon
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20789, Patch.TheLegendReturns) // Purple Buckler
                .Bait      (fish, 20615)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (21174, Patch.TheLegendReturns) // Cardinalfish
                .Bait      (fish, 20619)
                .Tug       (BiteType.Strong)
                .Uptime    (1140, 1380)
                .Weather   (3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (21175, Patch.TheLegendReturns) // Rockfish
                .Bait      (fish, 20619)
                .Tug       (BiteType.Weak)
                .Uptime    (720, 960)
                .Snag      (Snagging.None)
                .Weather   (3, 4, 5, 11)
                .HookType  (HookSet.Precise);
            fish.Apply     (21176, Patch.TheLegendReturns) // Ukiki
                .Bait      (fish, 20676)
                .Tug       (BiteType.Legendary)
                .Uptime    (480, 720)
                .Weather   (5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (21177, Patch.TheLegendReturns) // Violet Prismfish
                .Bait      (fish, 20675)
                .Tug       (BiteType.Strong)
                .Uptime    (0, 240)
                .HookType  (HookSet.Powerful);
            fish.Apply     (21178, Patch.TheLegendReturns) // Guppy
                .Bait      (fish, 20675)
                .Tug       (BiteType.Weak)
                .Uptime    (960, 1200)
                .Weather   (2, 1)
                .HookType  (HookSet.Precise);
            fish.Apply     (21179, Patch.TheLegendReturns) // Ichimonji
                .Gig       (GigHead.Large)
                .Uptime    (120, 720)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (21180, Patch.TheLegendReturns) // Snailfish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .Predators (fish, (21179, 10))
                .HookType  (HookSet.None);
        }
        // @formatter:on
    }
}
