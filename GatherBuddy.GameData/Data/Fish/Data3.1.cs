using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyAsGoesLightSoGoesDarkness(this GameData data)
    {
        data.Apply     (13727, Patch.AsGoesLightSoGoesDarkness) // Functional Proto-hropken
            .Bait      (data, 12710)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 9);
        data.Apply     (13728, Patch.AsGoesLightSoGoesDarkness) // Coerthan Clione
            .Bait      (data, 12711)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (0, 120)
            .Weather   (data, 16);
        data.Apply     (13729, Patch.AsGoesLightSoGoesDarkness) // Dravanian Smelt
            .Bait      (data, 12711)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (13730, Patch.AsGoesLightSoGoesDarkness) // Heavens Coral
            .Bait      (data, 12712)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (0, 360)
            .Snag      (Snagging.Required)
            .Weather   (data, 1, 2);
        data.Apply     (13731, Patch.AsGoesLightSoGoesDarkness) // Sunsail
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (900, 1020)
            .Weather   (data, 2, 1);
        data.Apply     (13732, Patch.AsGoesLightSoGoesDarkness) // Goblin Bass
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (0, 300)
            .Weather   (data, 3, 4);
        data.Apply     (14211, Patch.AsGoesLightSoGoesDarkness) // Amber Salamander
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (360, 720);
        data.Apply     (14212, Patch.AsGoesLightSoGoesDarkness) // Gnomefish
            .Bait      (data, 12705)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (14213, Patch.AsGoesLightSoGoesDarkness) // Fleece Stingray
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 2, 1);
        data.Apply     (14214, Patch.AsGoesLightSoGoesDarkness) // Nyctosaur
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (14215, Patch.AsGoesLightSoGoesDarkness) // Lava Snail
            .Bait      (data, 12709)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .Weather   (data, 1, 2);
        data.Apply     (14216, Patch.AsGoesLightSoGoesDarkness) // Priestfish
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (14217, Patch.AsGoesLightSoGoesDarkness) // Coerthan Oyster
            .Bait      (data, 12711)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required);
        data.Apply     (14218, Patch.AsGoesLightSoGoesDarkness) // Oliphant's Trunk
            .Bait      (data, 12710)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (1080, 1380)
            .Weather   (data, 9);
        data.Apply     (14219, Patch.AsGoesLightSoGoesDarkness) // Mountain Kraken
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (14220, Patch.AsGoesLightSoGoesDarkness) // Armored Catfish
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 4, 3);
    }
    // @formatter:on
}
