using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyRevengeOfTheHorde(this GameData data)
    {
        data.Apply     (15439, Patch.RevengeOfTheHorde) // Magic Bucket
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Snag      (data, Snagging.Required)
            .ForceBig  (false);
        data.Apply     (15626, Patch.RevengeOfTheHorde) // Fat Purse
            .Bait      (data, 12711)
            .Bite      (data, HookSet.Precise, BiteType.Legendary);
        data.Apply     (15627, Patch.RevengeOfTheHorde) // The Impaler
            .Bait      (data, 12711)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Snag      (data, Snagging.Required);
        data.Apply     (15628, Patch.RevengeOfTheHorde) // La Reale
            .Bait      (data, 12711)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (15629, Patch.RevengeOfTheHorde) // Scaleripper
            .Bait      (data, 12711)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 11, 4, 3);
        data.Apply     (15630, Patch.RevengeOfTheHorde) // The Dreamweaver
            .Bait      (data, 12711)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (1320, 120)
            .Weather   (data, 11, 4, 3);
        data.Apply     (15631, Patch.RevengeOfTheHorde) // Meteortoise
            .Mooch     (data, 12709, 12754)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (15632, Patch.RevengeOfTheHorde) // The Ewer King
            .Mooch     (data, 12707, 12780)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (15633, Patch.RevengeOfTheHorde) // Vidofnir
            .Bait      (data, 12711)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 600);
        data.Apply     (15634, Patch.RevengeOfTheHorde) // The Soul of the Martyr
            .Bait      (data, 12712)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Time      (240, 360)
            .Weather   (data, 1, 2);
        data.Apply     (15635, Patch.RevengeOfTheHorde) // Inkfish
            .Bait      (data, 12711)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (840, 960)
            .Slap      (data, 12739);
        data.Apply     (15636, Patch.RevengeOfTheHorde) // The Second One
            .Bait      (data, 12712)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 5);
        data.Apply     (15637, Patch.RevengeOfTheHorde) // Augmented High Allagan Helmet
            .Mooch     (data, 12710, 12776)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (15638, Patch.RevengeOfTheHorde) // Aphotic Pirarucu
            .Mooch     (data, 12705, 12777)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (1260, 120)
            .Weather   (data, 3);
    }
    // @formatter:on
}
