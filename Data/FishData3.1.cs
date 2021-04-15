using GatherBuddy.Classes;
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
                .Bait      (12710)
                .Tug       (BiteType.Strong)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (13728, Patch.AsGoesLightSoGoesDarkness) // Coerthan Clione
                .Bait      (12711)
                .Tug       (BiteType.Weak)
                .Uptime    (0, 2)
                .Weather   (16)
                .HookType  (HookSet.Precise);
            fish.Apply     (13729, Patch.AsGoesLightSoGoesDarkness) // Dravanian Smelt
                .Bait      (12711)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (13730, Patch.AsGoesLightSoGoesDarkness) // Heavens Coral
                .Bait      (12712)
                .Tug       (BiteType.Weak)
                .Uptime    (0, 6)
                .Snag      (Snagging.Required)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (13731, Patch.AsGoesLightSoGoesDarkness) // Sunsail
                .Bait      (12712)
                .Tug       (BiteType.Strong)
                .Uptime    (15, 17)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (13732, Patch.AsGoesLightSoGoesDarkness) // Goblin Bass
                .Bait      (12711)
                .Tug       (BiteType.Strong)
                .Uptime    (0, 5)
                .Weather   (3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (14211, Patch.AsGoesLightSoGoesDarkness) // Amber Salamander
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (6, 12)
                .HookType  (HookSet.Powerful);
            fish.Apply     (14212, Patch.AsGoesLightSoGoesDarkness) // Gnomefish
                .Bait      (12705)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (14213, Patch.AsGoesLightSoGoesDarkness) // Fleece Stingray
                .Bait      (12712)
                .Tug       (BiteType.Strong)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (14214, Patch.AsGoesLightSoGoesDarkness) // Nyctosaur
                .Bait      (12712);
            fish.Apply     (14215, Patch.AsGoesLightSoGoesDarkness) // Lava Snail
                .Bait      (12709)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (14216, Patch.AsGoesLightSoGoesDarkness) // Priestfish
                .Bait      (12711)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (14217, Patch.AsGoesLightSoGoesDarkness) // Coerthan Oyster
                .Bait      (12711)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (14218, Patch.AsGoesLightSoGoesDarkness) // Oliphant's Trunk
                .Bait      (12710)
                .Tug       (BiteType.Weak)
                .Uptime    (18, 23)
                .Weather   (9)
                .HookType  (HookSet.Precise);
            fish.Apply     (14219, Patch.AsGoesLightSoGoesDarkness) // Mountain Kraken
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (14220, Patch.AsGoesLightSoGoesDarkness) // Armored Catfish
                .Bait      (12711)
                .Tug       (BiteType.Strong)
                .Weather   (4, 3)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
