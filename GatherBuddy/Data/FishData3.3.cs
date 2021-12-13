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
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15626, Patch.RevengeOfTheHorde) // Fat Purse
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (15627, Patch.RevengeOfTheHorde) // The Impaler
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (15628, Patch.RevengeOfTheHorde) // La Reale
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15629, Patch.RevengeOfTheHorde) // Scaleripper
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Weather   (11, 4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15630, Patch.RevengeOfTheHorde) // The Dreamweaver
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (1320, 120)
                .Weather   (11, 4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15631, Patch.RevengeOfTheHorde) // Meteortoise
                .Bait      (fish, 12709, 12754)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15632, Patch.RevengeOfTheHorde) // The Ewer King
                .Bait      (fish, 12706, 12780)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15633, Patch.RevengeOfTheHorde) // Vidofnir
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (480, 600)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15634, Patch.RevengeOfTheHorde) // The Soul of the Martyr
                .Bait      (fish, 12712)
                .Tug       (BiteType.Legendary)
                .Uptime    (240, 360)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (15635, Patch.RevengeOfTheHorde) // Inkfish
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (840, 960)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15636, Patch.RevengeOfTheHorde) // The Second One
                .Bait      (fish, 12712)
                .Tug       (BiteType.Legendary)
                .Weather   (5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15637, Patch.RevengeOfTheHorde) // Augmented High Allagan Helmet
                .Bait      (fish, 12710, 12776)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (15638, Patch.RevengeOfTheHorde) // Aphotic Pirarucu
                .Bait      (fish, 12705, 12777)
                .Tug       (BiteType.Legendary)
                .Uptime    (1320, 120)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
