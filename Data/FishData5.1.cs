using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyVowsOfVirtueDeedsOfCruelty(this FishManager fish)
        {
            fish.Apply     (28065, Patch.VowsOfVirtueDeedsOfCruelty) // Loose Pendant
                .Bait      (27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 21)
                .HookType  (HookSet.Precise);
            fish.Apply     (28066, Patch.VowsOfVirtueDeedsOfCruelty) // Winged Dame
                .Bait      (27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 19)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (28067, Patch.VowsOfVirtueDeedsOfCruelty) // The Unforgiven
                .Bait      (27589)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28068, Patch.VowsOfVirtueDeedsOfCruelty) // Bronze Sole
                .Bait      (27585)
                .Tug       (BiteType.Legendary)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28069, Patch.VowsOfVirtueDeedsOfCruelty) // The Horned King
                .Bait      (27586, 27460)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 6)
                .Weather   (11)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28070, Patch.VowsOfVirtueDeedsOfCruelty) // The Sound of Fury
                .Bait      (27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 24)
                .Weather   (2, 1)
                .HookType  (HookSet.Precise);
            fish.Apply     (28071, Patch.VowsOfVirtueDeedsOfCruelty) // Priest of Yx'Lokwa
                .Bait      (27587, 27490, 27491)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 12)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28072, Patch.VowsOfVirtueDeedsOfCruelty) // Starchaser
                .Bait      (27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (6, 10)
                .Weather   (3)
                .HookType  (HookSet.Precise);
            fish.Apply     (28189, Patch.VowsOfVirtueDeedsOfCruelty) // Laxan Inkhorn
                .Bait      (28634)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (28190, Patch.VowsOfVirtueDeedsOfCruelty) // White Oil Perch
                .Bait      (28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28191, Patch.VowsOfVirtueDeedsOfCruelty) // Faeshine Clam
                .Bait      (28634)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (28192, Patch.VowsOfVirtueDeedsOfCruelty) // Areng Dire
                .Bait      (28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28193, Patch.VowsOfVirtueDeedsOfCruelty) // Kholusian King Crab
                .Bait      (28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28719, Patch.VowsOfVirtueDeedsOfCruelty) // Sweetmeat Mussel
                .Bait      (27590)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
