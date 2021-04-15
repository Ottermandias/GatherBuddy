using GatherBuddy.Classes;
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
                .Bait      ();
            fish.Apply     (33220, Patch.DeathUntoDawn) // Cloud Kelp
                .Bait      ();
            fish.Apply     (33221, Patch.DeathUntoDawn) // Scarlet Frog
                .Bait      ();
            fish.Apply     (33222, Patch.DeathUntoDawn) // White Clam
                .Bait      ();
            fish.Apply     (33223, Patch.DeathUntoDawn) // Fragrant Sweetfish
                .Bait      ();
            fish.Apply     (33316, Patch.DeathUntoDawn) // Aster Trivi
                .Bait      ();
            fish.Apply     (33317, Patch.DeathUntoDawn) // Python Discus
                .Bait      ();
            fish.Apply     (33318, Patch.DeathUntoDawn) // Steel Razor
                .Bait      ();
            fish.Apply     (33319, Patch.DeathUntoDawn) // Shadeshifter
                .Bait      ();
            fish.Apply     (33320, Patch.DeathUntoDawn) // Nabaath Saw
                .Bait      ();
            fish.Apply     (33321, Patch.DeathUntoDawn) // Dammroen Herring
                .Bait      ();
            fish.Apply     (33322, Patch.DeathUntoDawn) // Celestial
                .Bait      ();
            fish.Apply     (33323, Patch.DeathUntoDawn) // Deephaunt
                .Bait      ();
            fish.Apply     (33324, Patch.DeathUntoDawn) // Golden Pipira
                .Bait      ();
            fish.Apply     (33325, Patch.DeathUntoDawn) // Mora Tecta
                .Bait      ();
            fish.Apply     (33326, Patch.DeathUntoDawn) // Maru Crab
                .Bait      ();
        }
        // @formatter:on
    }
}
