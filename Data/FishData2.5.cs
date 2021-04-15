using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyBeforeTheFall(this FishManager fish)
        {
            fish.Apply     (10123, Patch.BeforeTheFall) // Gigant Clam
                .Bait      (2619)
                .Tug       (BiteType.Legendary)
                .Uptime    (23, 2)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
