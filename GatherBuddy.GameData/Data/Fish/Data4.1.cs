using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyTheLegendReturns(this GameData data)
    {
        data.Apply     (20785, Patch.TheLegendReturns) // Gyr Abanian Chub
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20786, Patch.TheLegendReturns) // Coral Horse
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (20787, Patch.TheLegendReturns) // Maiden's Heart
            .Bait      (data, 20614)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20788, Patch.TheLegendReturns) // Velodyna Salmon
            .Bait      (data, 28634)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20789, Patch.TheLegendReturns) // Purple Buckler
            .Bait      (data, 20615)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (21174, Patch.TheLegendReturns) // Cardinalfish
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (1140, 1380)
            .Weather   (data, 3, 4);
        data.Apply     (21175, Patch.TheLegendReturns) // Rockfish
            .Bait      (data, 20619)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (720, 960)
            .Snag      (Snagging.None)
            .Weather   (data, 3, 4, 5, 11);
        data.Apply     (21176, Patch.TheLegendReturns) // Ukiki
            .Bait      (data, 20676)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 720)
            .Weather   (data, 5);
        data.Apply     (21177, Patch.TheLegendReturns) // Violet Prismfish
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (0, 240);
        data.Apply     (21178, Patch.TheLegendReturns) // Guppy
            .Bait      (data, 20675)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (960, 1200)
            .Weather   (data, 2, 1);
        data.Apply     (21179, Patch.TheLegendReturns) // Ichimonji
            .Spear     (SpearfishSize.Large, SpearfishSpeed.LynFast);
        data.Apply     (21180, Patch.TheLegendReturns) // Snailfish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Fast)
            .Predators (data, 0, (21179, 10));
    }
    // @formatter:on
}
