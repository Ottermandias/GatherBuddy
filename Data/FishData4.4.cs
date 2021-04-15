using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyPreludeInViolet(this FishManager fish)
        {
            fish.Apply     (24203, Patch.PreludeInViolet) // Indigo Prismfish
                .Bait      (20675, 21177)
                .Tug       (BiteType.Weak)
                .Uptime    (0, 4)
                .HookType  (HookSet.Precise);
            fish.Apply     (24204, Patch.PreludeInViolet) // Green Prismfish
                .Bait      (20675)
                .Tug       (BiteType.Weak)
                .Uptime    (0, 16)
                .Transition(2)
                .Weather   (2, 1)
                .HookType  (HookSet.Precise);
            fish.Apply     (24205, Patch.PreludeInViolet) // Watcher Catfish
                .Bait      (20615, 20056)
                .Tug       (BiteType.Legendary)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24206, Patch.PreludeInViolet) // Bloodtail Zombie
                .Bait      (20613, 20064)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 12)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24207, Patch.PreludeInViolet) // Hardhead Trout
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 20)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24208, Patch.PreludeInViolet) // Downstream Loach
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24209, Patch.PreludeInViolet) // Corpse Chub
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 24)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24210, Patch.PreludeInViolet) // The Last Tear
                .Bait      (20613)
                .Tug       (BiteType.Legendary)
                .Transition(4)
                .Weather   (2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24211, Patch.PreludeInViolet) // Hemon
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 20)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24212, Patch.PreludeInViolet) // Moksha
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Transition(3, 5, 4, 11)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24213, Patch.PreludeInViolet) // Princess Killifish
                .Bait      (20614)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 6)
                .HookType  (HookSet.Precise);
            fish.Apply     (24214, Patch.PreludeInViolet) // Ku'er
                .Bait      (20676)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 8)
                .Transition(9)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24215, Patch.PreludeInViolet) // Argonautica
                .Bait      (20617, 20112)
                .Tug       (BiteType.Legendary)
                .Weather   (5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24216, Patch.PreludeInViolet) // Hagoromo Bijin
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Transition(3)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24217, Patch.PreludeInViolet) // Duskfish
                .Bait      (20614, 20127)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 16)
                .Transition(7, 6)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (24218, Patch.PreludeInViolet) // Blade Skipper
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 8)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
