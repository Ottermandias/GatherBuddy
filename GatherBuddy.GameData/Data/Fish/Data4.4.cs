using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyPreludeInViolet(this GameData data)
    {
        data.Apply     (24203, Patch.PreludeInViolet) // Indigo Prismfish
            .Bait      (data, 20675, 21177)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (24204, Patch.PreludeInViolet) // Green Prismfish
            .Bait      (data, 20675)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (0, 960)
            .Transition(data, 2)
            .Weather   (data, 2, 1);
        data.Apply     (24205, Patch.PreludeInViolet) // Watcher Catfish
            .Bait      (data, 20615, 20056)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 9);
        data.Apply     (24206, Patch.PreludeInViolet) // Bloodtail Zombie
            .Bait      (data, 20613, 20064)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 720)
            .Weather   (data, 3);
        data.Apply     (24207, Patch.PreludeInViolet) // Hardhead Trout
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 1200);
        data.Apply     (24208, Patch.PreludeInViolet) // Downstream Loach
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 4);
        data.Apply     (24209, Patch.PreludeInViolet) // Corpse Chub
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1200, 1440)
            .Weather   (data, 1);
        data.Apply     (24210, Patch.PreludeInViolet) // The Last Tear
            .Bait      (data, 20613)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 4)
            .Weather   (data, 2);
        data.Apply     (24211, Patch.PreludeInViolet) // Hemon
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 1200)
            .Weather   (data, 3);
        data.Apply     (24212, Patch.PreludeInViolet) // Moksha
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 3, 5, 4, 11)
            .Weather   (data, 1);
        data.Apply     (24213, Patch.PreludeInViolet) // Princess Killifish
            .Bait      (data, 20614)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (240, 360);
        data.Apply     (24214, Patch.PreludeInViolet) // Ku'er
            .Bait      (data, 20676)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (0, 480)
            .Transition(data, 9)
            .Weather   (data, 3);
        data.Apply     (24215, Patch.PreludeInViolet) // Argonautica
            .Bait      (data, 20617, 20112)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 5);
        data.Apply     (24216, Patch.PreludeInViolet) // Hagoromo Bijin
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 3)
            .Weather   (data, 2, 1);
        data.Apply     (24217, Patch.PreludeInViolet) // Duskfish
            .Bait      (data, 20614, 20127)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 960)
            .Transition(data, 7, 6)
            .Weather   (data, 3);
        data.Apply     (24218, Patch.PreludeInViolet) // Blade Skipper
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (240, 480)
            .Weather   (data, 4);
    }
    // @formatter:on
}
