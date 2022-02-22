using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyBeforeTheFall(this GameData data)
    {
        data.Apply     (10123, Patch.BeforeTheFall) // Gigant Clam
            .Bait      (data, 2619)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1380, 120)
            .Snag      (Snagging.None);
    }
    // @formatter:on
}
