using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyDeathUntoDawn(this FishManager fish)
        {
            fish.Apply     (33219, Patch.DeathUntoDawn) // Dravanian Scallop
                .Bait      (fish);
            fish.Apply     (33220, Patch.DeathUntoDawn) // Cloud Kelp
                .Bait      (fish);
            fish.Apply     (33221, Patch.DeathUntoDawn) // Scarlet Frog
                .Bait      (fish);
            fish.Apply     (33222, Patch.DeathUntoDawn) // White Clam
                .Bait      (fish);
            fish.Apply     (33223, Patch.DeathUntoDawn) // Fragrant Sweetfish
                .Bait      (fish);
            fish.Apply     (33316, Patch.DeathUntoDawn) // Aster Trivi
                .Bait      (fish, 27585)
                .Tug       (BiteType.Legendary)
                .Uptime    (22, 24)
                .Transition(1)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (33317, Patch.DeathUntoDawn) // Python Discus
                .Bait      (fish, 27582)
                .Tug       (BiteType.Legendary)
                .Transition(7)
                .Weather   (10)
                .HookType  (HookSet.Precise);
            fish.Apply     (33318, Patch.DeathUntoDawn) // Steel Razor
                .Bait      (fish, 27583)
                .Tug       (BiteType.Legendary)
                .Uptime    (6, 8)
                .HookType  (HookSet.Precise);
            fish.Apply     (33319, Patch.DeathUntoDawn) // Shadeshifter
                .Bait      (fish, 27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 14)
                .Transition(1, 2)
                .Weather   (6)
                .HookType  (HookSet.Precise);
            fish.Apply     (33320, Patch.DeathUntoDawn) // Nabaath Saw
                .Bait      (fish, 27586, 27464)
                .Tug       (BiteType.Legendary)
                .Uptime    (6, 10)
                .Transition(1, 2, 14)
                .Weather   (14)
                .HookType  (HookSet.Powerful);
            fish.Apply     (33321, Patch.DeathUntoDawn) // Dammroen Herring
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (33322, Patch.DeathUntoDawn) // Celestial
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (7, 17)
                .Transition(10)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (33323, Patch.DeathUntoDawn) // Deephaunt
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (2, 6)
                .Transition(3)
                .Weather   (4)
                .HookType  (HookSet.Precise);
            fish.Apply     (33324, Patch.DeathUntoDawn) // Golden Pipira
                .Bait      (fish, 27587)
                .Tug       (BiteType.Legendary)
                .Uptime    (6, 7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (33325, Patch.DeathUntoDawn) // Mora Tecta
                .Bait      (fish, 27588)
                .Tug       (BiteType.Legendary)
                .Uptime    (22, 24)
                .Weather   (2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (33326, Patch.DeathUntoDawn) // Maru Crab
                .Bait      (fish, 27588, 27506)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 20)
                .Weather   (3)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
