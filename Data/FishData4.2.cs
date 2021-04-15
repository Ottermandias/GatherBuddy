using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyRiseOfANewSun(this FishManager fish)
        {
            fish.Apply     (22389, Patch.RiseOfANewSun) // Mirage Mahi
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (22390, Patch.RiseOfANewSun) // Triplespine
                .Bait      (20676)
                .Tug       (BiteType.Weak)
                .Uptime    (5, 7)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (22391, Patch.RiseOfANewSun) // Alligator Snapping Turtle
                .Bait      (20619)
                .Tug       (BiteType.Strong)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (22392, Patch.RiseOfANewSun) // Redtail
                .Bait      (20613, 20064)
                .Tug       (BiteType.Legendary)
                .Weather   (3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (22393, Patch.RiseOfANewSun) // Usuginu Octopus
                .Bait      (20617, 20112)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (22394, Patch.RiseOfANewSun) // Saltmill
                .Bait      (20616, 20025)
                .Tug       (BiteType.Legendary)
                .Weather   (3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (22395, Patch.RiseOfANewSun) // Bonsai Fish
                .Bait      (20614)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (22396, Patch.RiseOfANewSun) // Ribbon Eel
                .Bait      (20617, 20112)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (22397, Patch.RiseOfANewSun) // Red Prismfish
                .Bait      (20675)
                .Tug       (BiteType.Strong)
                .Uptime    (4, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (22398, Patch.RiseOfANewSun) // Elder Gourami
                .Bait      (20614, 20127)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .Weather   (3, 4)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
