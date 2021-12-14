using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyVowsOfVirtueDeedsOfCruelty(this GameData data)
    {
        data.Apply     (28065, Patch.VowsOfVirtueDeedsOfCruelty) // Loose Pendant
            .Bait      (data, 27589)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1080, 1320);
        data.Apply     (28066, Patch.VowsOfVirtueDeedsOfCruelty) // Winged Dame
            .Bait      (data, 27590)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (720, 1140)
            .Weather   (data, 1);
        data.Apply     (28067, Patch.VowsOfVirtueDeedsOfCruelty) // The Unforgiven
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Weather   (data, 4);
        data.Apply     (28068, Patch.VowsOfVirtueDeedsOfCruelty) // Bronze Sole
            .Bait      (data, 27585)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 7);
        data.Apply     (28069, Patch.VowsOfVirtueDeedsOfCruelty) // The Horned King
            .Bait      (data, 27586, 27460)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (0, 360)
            .Weather   (data, 11);
        data.Apply     (28070, Patch.VowsOfVirtueDeedsOfCruelty) // The Sound of Fury
            .Bait      (data, 27589)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1200, 1440)
            .Weather   (data, 2, 1);
        data.Apply     (28071, Patch.VowsOfVirtueDeedsOfCruelty) // Priest of Yx'Lokwa
            .Bait      (data, 27587, 27490, 27491)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (600, 720);
        data.Apply     (28072, Patch.VowsOfVirtueDeedsOfCruelty) // Starchaser
            .Bait      (data, 27590)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (360, 600)
            .Weather   (data, 3);
        data.Apply     (28189, Patch.VowsOfVirtueDeedsOfCruelty) // Laxan Inkhorn
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (28190, Patch.VowsOfVirtueDeedsOfCruelty) // White Oil Perch
            .Bait      (data, 28634)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (28191, Patch.VowsOfVirtueDeedsOfCruelty) // Faeshine Clam
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (28192, Patch.VowsOfVirtueDeedsOfCruelty) // Areng Dire
            .Bait      (data, 28634)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (28193, Patch.VowsOfVirtueDeedsOfCruelty) // Kholusian King Crab
            .Bait      (data, 28634)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (28719, Patch.VowsOfVirtueDeedsOfCruelty) // Sweetmeat Mussel
            .Bait      (data, 27590)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required);
    }
    // @formatter:on
}
