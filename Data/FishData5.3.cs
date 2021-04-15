using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyReflectionsInCrystal(this FishManager fish)
        {
            fish.Apply     (30432, Patch.ReflectionsInCrystal) // The Sinsteeped
                .Bait      (27582)
                .Tug       (BiteType.Legendary)
                .Uptime    (22, 24)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (30433, Patch.ReflectionsInCrystal) // Sweetheart
                .Bait      (27589)
                .Tug       (BiteType.Legendary)
                .Transition(4)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (30434, Patch.ReflectionsInCrystal) // Giant Taimen
                .Bait      (27589)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (30435, Patch.ReflectionsInCrystal) // Leannisg
                .Bait      (27585)
                .Tug       (BiteType.Legendary)
                .Uptime    (6, 8)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (30436, Patch.ReflectionsInCrystal) // Gold Hammer
                .Bait      (27589)
                .Tug       (BiteType.Legendary)
                .Transition(1, 2)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (30437, Patch.ReflectionsInCrystal) // Recordkiller
                .Bait      (27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 24)
                .Transition(1, 2)
                .Weather   (4)
                .HookType  (HookSet.Precise);
            fish.Apply     (30438, Patch.ReflectionsInCrystal) // The Mother of All Pancakes
                .Bait      (27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 3)
                .Transition(3)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (30439, Patch.ReflectionsInCrystal) // Opal Shrimp
                .Bait      (27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 20)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (30487, Patch.ReflectionsInCrystal) // Blue Crab
                .Bait      (27590)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (30488, Patch.ReflectionsInCrystal) // Hoplite
                .Bait      (28634);
            fish.Apply     (30489, Patch.ReflectionsInCrystal) // Hook Fish
                .Bait      (28634);
            fish.Apply     (30490, Patch.ReflectionsInCrystal) // Cloudweed
                .Bait      (28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (30491, Patch.ReflectionsInCrystal) // Fatty Herring
                .Bait      (28634);
            fish.Apply     (30492, Patch.ReflectionsInCrystal) // Inkshell
                .Bait      (28634);
            fish.Apply     (30593, Patch.ReflectionsInCrystal) // Fuchsia Bloom
                .Bait      (27587)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (31129, Patch.ReflectionsInCrystal) // Petal Shell
                .Bait      ();
            fish.Apply     (31134, Patch.ReflectionsInCrystal) // Allagan Hunter
                .Bait      ();
        }
        // @formatter:on
    }
}
