using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyRevengeOfTheHorde(this FishManager fish)
        {
            fish.Apply     (15439, Patch.RevengeOfTheHorde) // Magic Bucket
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15626, Patch.RevengeOfTheHorde) // Fat Purse
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (15627, Patch.RevengeOfTheHorde) // The Impaler
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (15628, Patch.RevengeOfTheHorde) // La Reale
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15629, Patch.RevengeOfTheHorde) // Scaleripper
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Weather   (11, 4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15630, Patch.RevengeOfTheHorde) // The Dreamweaver
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (22, 2)
                .Weather   (11, 4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15631, Patch.RevengeOfTheHorde) // Meteortoise
                .Bait      (12709, 12754)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15632, Patch.RevengeOfTheHorde) // The Ewer King
                .Bait      (12706, 12780)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15633, Patch.RevengeOfTheHorde) // Vidofnir
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15634, Patch.RevengeOfTheHorde) // The Soul of the Martyr
                .Bait      (12712)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 6)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (15635, Patch.RevengeOfTheHorde) // Inkfish
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (14, 16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15636, Patch.RevengeOfTheHorde) // The Second One
                .Bait      (12712)
                .Tug       (BiteType.Legendary)
                .Weather   (5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15637, Patch.RevengeOfTheHorde) // Augmented High Allagan Helmet
                .Bait      (12710, 12776)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15638, Patch.RevengeOfTheHorde) // Aphotic Pirarucu
                .Bait      (12705, 12777)
                .Tug       (BiteType.Legendary)
                .Uptime    (22, 2)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
