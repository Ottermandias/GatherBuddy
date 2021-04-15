using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplySoulSurrender(this FishManager fish)
        {
            fish.Apply     (16742, Patch.SoulSurrender) // Dimorphodon
                .Bait      (12712, 12805)
                .Tug       (BiteType.Legendary)
                .Transition(1)
                .Weather   (6)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16743, Patch.SoulSurrender) // Basking Shark
                .Bait      (12708, 12753)
                .Tug       (BiteType.Legendary)
                .Transition(4)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16744, Patch.SoulSurrender) // Allagan Bladeshark
                .Bait      (12710, 12776)
                .Tug       (BiteType.Legendary)
                .Transition(3)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16745, Patch.SoulSurrender) // Hailfinder
                .Bait      (12708, 12724)
                .Tug       (BiteType.Legendary)
                .Transition(15)
                .Weather   (16)
                .HookType  (HookSet.Precise);
            fish.Apply     (16746, Patch.SoulSurrender) // Flarefish
                .Bait      (12705, 12715)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 16)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16747, Patch.SoulSurrender) // Twin-tongued Carp
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 16)
                .Transition(3, 11)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16748, Patch.SoulSurrender) // Madam Butterfly
                .Bait      (12705, 12757)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 2)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16749, Patch.SoulSurrender) // Moggle Mogpom
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 13)
                .Transition(6)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (16750, Patch.SoulSurrender) // Cirrostratus
                .Bait      (12712)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 13)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16751, Patch.SoulSurrender) // Hraesvelgr's Tear
                .Bait      (12709)
                .Tug       (BiteType.Legendary)
                .Uptime    (2, 6)
                .HookType  (HookSet.Precise);
            fish.Apply     (16752, Patch.SoulSurrender) // Aetherochemical Compound #666
                .Bait      (12710, 12776)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (16753, Patch.SoulSurrender) // Hundred-eyed Axolotl
                .Bait      (12705, 12777)
                .Tug       (BiteType.Legendary)
                .Uptime    (6, 10)
                .HookType  (HookSet.Precise);
            fish.Apply     (16754, Patch.SoulSurrender) // Bobgoblin Bass
                .Bait      (12711, 12780)
                .Tug       (BiteType.Legendary)
                .Uptime    (2, 6)
                .Transition(7)
                .Weather   (8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16756, Patch.SoulSurrender) // Merciless
                .Bait      (2607, 12715)
                .Tug       (BiteType.Legendary)
                .Transition(15, 16)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
