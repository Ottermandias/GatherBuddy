using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyRiseOfANewSun(this GameData data)
    {
        data.Apply     (22389, Patch.RiseOfANewSun) // Mirage Mahi
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (240, 480);
        data.Apply     (22390, Patch.RiseOfANewSun) // Triplespine
            .Bait      (data, 20676)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (300, 420)
            .Snag      (Snagging.None);
        data.Apply     (22391, Patch.RiseOfANewSun) // Alligator Snapping Turtle
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 2, 1);
        data.Apply     (22392, Patch.RiseOfANewSun) // Redtail
            .Bait      (data, 20613, 20064)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 3, 4);
        data.Apply     (22393, Patch.RiseOfANewSun) // Usuginu Octopus
            .Bait      (data, 20617)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (22394, Patch.RiseOfANewSun) // Saltmill
            .Bait      (data, 20616, 20025)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 3, 4);
        data.Apply     (22395, Patch.RiseOfANewSun) // Bonsai Fish
            .Bait      (data, 20614)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (22396, Patch.RiseOfANewSun) // Ribbon Eel
            .Bait      (data, 20617, 20112)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (22397, Patch.RiseOfANewSun) // Red Prismfish
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (240, 480);
        data.Apply     (22398, Patch.RiseOfANewSun) // Elder Gourami
            .Bait      (data, 20614, 20127)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Snag      (Snagging.None)
            .Weather   (data, 3, 4);
    }
    // @formatter:on
}
