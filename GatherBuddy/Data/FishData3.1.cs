using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyAsGoesLightSoGoesDarkness(this FishManager fish)
        {
            fish.Apply     (13727, Patch.AsGoesLightSoGoesDarkness) // Functional Proto-hropken
                .Bait      (fish, 12710)
                .Tug       (BiteType.Strong)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (13728, Patch.AsGoesLightSoGoesDarkness) // Coerthan Clione
                .Bait      (fish, 12711)
                .Tug       (BiteType.Weak)
                .Uptime    (0, 120)
                .Weather   (16)
                .HookType  (HookSet.Precise);
            fish.Apply     (13729, Patch.AsGoesLightSoGoesDarkness) // Dravanian Smelt
                .Bait      (fish, 12711)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (13730, Patch.AsGoesLightSoGoesDarkness) // Heavens Coral
                .Bait      (fish, 12712)
                .Tug       (BiteType.Weak)
                .Uptime    (0, 360)
                .Snag      (Snagging.Required)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (13731, Patch.AsGoesLightSoGoesDarkness) // Sunsail
                .Bait      (fish, 12712)
                .Tug       (BiteType.Strong)
                .Uptime    (900, 1020)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (13732, Patch.AsGoesLightSoGoesDarkness) // Goblin Bass
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .Uptime    (0, 300)
                .Weather   (3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (14211, Patch.AsGoesLightSoGoesDarkness) // Amber Salamander
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (360, 720)
                .HookType  (HookSet.Powerful);
            fish.Apply     (14212, Patch.AsGoesLightSoGoesDarkness) // Gnomefish
                .Bait      (fish, 12705)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (14213, Patch.AsGoesLightSoGoesDarkness) // Fleece Stingray
                .Bait      (fish, 12712)
                .Tug       (BiteType.Strong)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (14214, Patch.AsGoesLightSoGoesDarkness) // Nyctosaur
                .Bait      (fish, 12712)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (14215, Patch.AsGoesLightSoGoesDarkness) // Lava Snail
                .Bait      (fish, 12709)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (14216, Patch.AsGoesLightSoGoesDarkness) // Priestfish
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (14217, Patch.AsGoesLightSoGoesDarkness) // Coerthan Oyster
                .Bait      (fish, 12711)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (14218, Patch.AsGoesLightSoGoesDarkness) // Oliphant's Trunk
                .Bait      (fish, 12710)
                .Tug       (BiteType.Weak)
                .Uptime    (1080, 1380)
                .Weather   (9)
                .HookType  (HookSet.Precise);
            fish.Apply     (14219, Patch.AsGoesLightSoGoesDarkness) // Mountain Kraken
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (14220, Patch.AsGoesLightSoGoesDarkness) // Armored Catfish
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .Weather   (4, 3)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
