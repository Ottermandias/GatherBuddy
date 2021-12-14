using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyDeathUntoDawn(this GameData data)
    {
        data.Apply     (33219, Patch.DeathUntoDawn) // Dravanian Scallop
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Bait      (data, 28634);
        data.Apply     (33220, Patch.DeathUntoDawn) // Cloud Kelp
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Bait      (data, 28634);
        data.Apply     (33221, Patch.DeathUntoDawn) // Scarlet Frog
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Bait      (data, 28634);
        data.Apply     (33222, Patch.DeathUntoDawn) // White Clam
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Bait      (data, 28634);
        data.Apply     (33223, Patch.DeathUntoDawn) // Fragrant Sweetfish
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Bait      (data, 28634);
        data.Apply     (33239, Patch.DeathUntoDawn) // Listracanthus
            .Bait      (data, 27589, 28925)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 1440)
            .Transition(data, 1, 2)
            .Weather   (data, 4);
        data.Apply     (33240, Patch.DeathUntoDawn) // Aquamaton
            .Bait      (data, 27588)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (600, 960)
            .Predators (data, 350, (33319, 1), (27452, 5));
        data.Apply     (33241, Patch.DeathUntoDawn) // Cinder Surprise
            .Bait      (data, 27584)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (0, 120)
            .Predators (data, 350, (27462, 10))
            .Transition(data, 11)
            .Weather   (data, 14);
        data.Apply     (33242, Patch.DeathUntoDawn) // Ealad Skaan
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1410, 1440)
            .Transition(data, 10)
            .Weather   (data, 1);
        data.Apply     (33243, Patch.DeathUntoDawn) // Greater Serpent of Ronka
            .Bait      (data, 27587, 27490, 27491, 28071)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (600, 720);
        data.Apply     (33244, Patch.DeathUntoDawn) // Lancetfish
            .Bait      (data, 27590)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (0, 120)
            .Predators (data, 700, (33325, 2))
            .Transition(data, 2)
            .Weather   (data, 3);
        data.Apply     (33316, Patch.DeathUntoDawn) // Aster Trivi
            .Bait      (data, 27585)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1320, 1440)
            .Transition(data, 1)
            .Weather   (data, 1);
        data.Apply     (33317, Patch.DeathUntoDawn) // Python Discus
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Transition(data, 7)
            .Weather   (data, 10);
        data.Apply     (33318, Patch.DeathUntoDawn) // Steel Razor
            .Bait      (data, 27583)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (360, 480);
        data.Apply     (33319, Patch.DeathUntoDawn) // Shadeshifter
            .Bait      (data, 27590)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (600, 840)
            .Transition(data, 1, 2)
            .Weather   (data, 6);
        data.Apply     (33320, Patch.DeathUntoDawn) // Nabaath Saw
            .Bait      (data, 27586, 27464)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (360, 600)
            .Transition(data, 1, 2, 14)
            .Weather   (data, 14);
        data.Apply     (33321, Patch.DeathUntoDawn) // Dammroen Herring
            .Bait      (data, 27589)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (33322, Patch.DeathUntoDawn) // Celestial
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (420, 1020)
            .Transition(data, 10)
            .Weather   (data, 1);
        data.Apply     (33323, Patch.DeathUntoDawn) // Deephaunt
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (120, 360)
            .Transition(data, 3)
            .Weather   (data, 4);
        data.Apply     (33324, Patch.DeathUntoDawn) // Golden Pipira
            .Bait      (data, 27587)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (360, 420);
        data.Apply     (33325, Patch.DeathUntoDawn) // Mora Tecta
            .Bait      (data, 27588)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1320, 1440)
            .Weather   (data, 2);
        data.Apply     (33326, Patch.DeathUntoDawn) // Maru Crab
            .Bait      (data, 27588, 27506)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (960, 1200)
            .Weather   (data, 3);
    }
    // @formatter:on
}
