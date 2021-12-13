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
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (1080, 1320)
                .HookType  (HookSet.Precise);
            fish.Apply     (28066, Patch.VowsOfVirtueDeedsOfCruelty) // Winged Dame
                .Bait      (fish, 27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (720, 1140)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (28067, Patch.VowsOfVirtueDeedsOfCruelty) // The Unforgiven
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28068, Patch.VowsOfVirtueDeedsOfCruelty) // Bronze Sole
                .Bait      (fish, 27585)
                .Tug       (BiteType.Legendary)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28069, Patch.VowsOfVirtueDeedsOfCruelty) // The Horned King
                .Bait      (fish, 27586, 27460)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 360)
                .Weather   (11)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28070, Patch.VowsOfVirtueDeedsOfCruelty) // The Sound of Fury
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (1200, 1440)
                .Weather   (2, 1)
                .HookType  (HookSet.Precise);
            fish.Apply     (28071, Patch.VowsOfVirtueDeedsOfCruelty) // Priest of Yx'Lokwa
                .Bait      (fish, 27587, 27490, 27491)
                .Tug       (BiteType.Legendary)
                .Uptime    (600, 720)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28072, Patch.VowsOfVirtueDeedsOfCruelty) // Starchaser
                .Bait      (fish, 27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (360, 600)
                .Weather   (3)
                .HookType  (HookSet.Precise);
            fish.Apply     (28189, Patch.VowsOfVirtueDeedsOfCruelty) // Laxan Inkhorn
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (28190, Patch.VowsOfVirtueDeedsOfCruelty) // White Oil Perch
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28191, Patch.VowsOfVirtueDeedsOfCruelty) // Faeshine Clam
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (28192, Patch.VowsOfVirtueDeedsOfCruelty) // Areng Dire
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28193, Patch.VowsOfVirtueDeedsOfCruelty) // Kholusian King Crab
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28719, Patch.VowsOfVirtueDeedsOfCruelty) // Sweetmeat Mussel
                .Bait      (fish, 27590)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
