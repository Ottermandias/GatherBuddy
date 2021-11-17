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
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise)
                .Bait      (fish, 28634);
            fish.Apply     (33220, Patch.DeathUntoDawn) // Cloud Kelp
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise)
                .Bait      (fish, 28634);
            fish.Apply     (33221, Patch.DeathUntoDawn) // Scarlet Frog
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise)
                .Bait      (fish, 28634);
            fish.Apply     (33222, Patch.DeathUntoDawn) // White Clam
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise)
                .Bait      (fish, 28634);
            fish.Apply     (33223, Patch.DeathUntoDawn) // Fragrant Sweetfish
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise)
                .Bait      (fish, 28634);
            fish.Apply     (33239, Patch.DeathUntoDawn) // Listracanthus
                .Bait      (fish, 27589, 28925)
                .Tug       (BiteType.Legendary)
                .Uptime    (960, 1440)
                .Transition(1, 2)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (33240, Patch.DeathUntoDawn) // Aquamaton
                .Bait      (fish, 27588)
                .Tug       (BiteType.Legendary)
                .Uptime    (600, 960)
                .Predators (fish, (33319, 1), (27452, 5))
                .Intuition (350)
                .HookType  (HookSet.Powerful);
            fish.Apply     (33241, Patch.DeathUntoDawn) // Cinder Surprise
                .Bait      (fish, 27584)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 120)
                .Predators (fish, (27462, 10))
                .Intuition (350)
                .Transition(11)
                .Weather   (14)
                .HookType  (HookSet.Precise);
            fish.Apply     (33242, Patch.DeathUntoDawn) // Ealad Skaan
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (1410, 1440)
                .Transition(10)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (33243, Patch.DeathUntoDawn) // Greater Serpent of Ronka
                .Bait      (fish, 27587, 27490, 27491, 28071)
                .Tug       (BiteType.Legendary)
                .Uptime    (600, 720)
                .HookType  (HookSet.Powerful);
            fish.Apply     (33244, Patch.DeathUntoDawn) // Lancetfish
                .Bait      (fish, 27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 120)
                .Predators (fish, (33325, 2))
                .Intuition (700)
                .Transition(2)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (33316, Patch.DeathUntoDawn) // Aster Trivi
                .Bait      (fish, 27585)
                .Tug       (BiteType.Legendary)
                .Uptime    (1320, 1440)
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
                .Uptime    (360, 480)
                .HookType  (HookSet.Precise);
            fish.Apply     (33319, Patch.DeathUntoDawn) // Shadeshifter
                .Bait      (fish, 27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (600, 840)
                .Transition(1, 2)
                .Weather   (6)
                .HookType  (HookSet.Precise);
            fish.Apply     (33320, Patch.DeathUntoDawn) // Nabaath Saw
                .Bait      (fish, 27586, 27464)
                .Tug       (BiteType.Legendary)
                .Uptime    (360, 600)
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
                .Uptime    (420, 1020)
                .Transition(10)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (33323, Patch.DeathUntoDawn) // Deephaunt
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (120, 360)
                .Transition(3)
                .Weather   (4)
                .HookType  (HookSet.Precise);
            fish.Apply     (33324, Patch.DeathUntoDawn) // Golden Pipira
                .Bait      (fish, 27587)
                .Tug       (BiteType.Legendary)
                .Uptime    (360, 420)
                .HookType  (HookSet.Powerful);
            fish.Apply     (33325, Patch.DeathUntoDawn) // Mora Tecta
                .Bait      (fish, 27588)
                .Tug       (BiteType.Legendary)
                .Uptime    (1320, 1440)
                .Weather   (2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (33326, Patch.DeathUntoDawn) // Maru Crab
                .Bait      (fish, 27588, 27506)
                .Tug       (BiteType.Legendary)
                .Uptime    (960, 1200)
                .Weather   (3)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
