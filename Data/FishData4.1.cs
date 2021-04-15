using GatherBuddy.Classes;
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
                .Bait      (20613)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (20786, Patch.TheLegendReturns) // Coral Horse
                .Bait      (20617)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (20787, Patch.TheLegendReturns) // Maiden's Heart
                .Bait      (20614)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (20788, Patch.TheLegendReturns) // Velodyna Salmon
                .Bait      (20614)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20789, Patch.TheLegendReturns) // Purple Buckler
                .Bait      (20615)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (21174, Patch.TheLegendReturns) // Cardinalfish
                .Bait      (20619)
                .Tug       (BiteType.Strong)
                .Uptime    (19, 23)
                .Weather   (3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (21175, Patch.TheLegendReturns) // Rockfish
                .Bait      (20619)
                .Tug       (BiteType.Weak)
                .Uptime    (12, 16)
                .Snag      (Snagging.None)
                .Weather   (3, 4, 5, 11)
                .HookType  (HookSet.Precise);
            fish.Apply     (21176, Patch.TheLegendReturns) // Ukiki
                .Bait      (20676)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 12)
                .Weather   (5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (21177, Patch.TheLegendReturns) // Violet Prismfish
                .Bait      (20675)
                .Tug       (BiteType.Strong)
                .Uptime    (0, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (21178, Patch.TheLegendReturns) // Guppy
                .Bait      (20675)
                .Tug       (BiteType.Weak)
                .Uptime    (16, 20)
                .Weather   (2, 1)
                .HookType  (HookSet.Precise);
            fish.Apply     (21179, Patch.TheLegendReturns) // Ichimonji
                .Gig       (GigHead.Large)
                .Uptime    (2, 12)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (21180, Patch.TheLegendReturns) // Snailfish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .Predators ((21179, 10))
                .HookType  (HookSet.None);
        }
        // @formatter:on
    }
}
