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
                .Bait      (fish, 12712, 12805)
                .Tug       (BiteType.Legendary)
                .Transition(1)
                .Weather   (6)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16743, Patch.SoulSurrender) // Basking Shark
                .Bait      (fish, 28634, 12753)
                .Tug       (BiteType.Legendary)
                .Transition(4)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16744, Patch.SoulSurrender) // Allagan Bladeshark
                .Bait      (fish, 12710, 12776)
                .Tug       (BiteType.Legendary)
                .Transition(3)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16745, Patch.SoulSurrender) // Hailfinder
                .Bait      (fish, 12708, 12724)
                .Tug       (BiteType.Legendary)
                .Transition(16, 15)
                .Weather   (16)
                .HookType  (HookSet.Precise);
            fish.Apply     (16746, Patch.SoulSurrender) // Flarefish
                .Bait      (fish, 12705, 12715)
                .Tug       (BiteType.Legendary)
                .Uptime    (600, 960)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16747, Patch.SoulSurrender) // Twin-tongued Carp
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (720, 960)
                .Transition(3, 11)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16748, Patch.SoulSurrender) // Madam Butterfly
                .Bait      (fish, 12705, 12757)
                .Tug       (BiteType.Legendary)
                .Uptime    (1260, 120)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16749, Patch.SoulSurrender) // Moggle Mogpom
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (600, 780)
                .Transition(6)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (16750, Patch.SoulSurrender) // Cirrostratus
                .Bait      (fish, 12712)
                .Tug       (BiteType.Legendary)
                .Uptime    (600, 780)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16751, Patch.SoulSurrender) // Hraesvelgr's Tear
                .Bait      (fish, 12709)
                .Tug       (BiteType.Legendary)
                .Uptime    (120, 360)
                .HookType  (HookSet.Precise);
            fish.Apply     (16752, Patch.SoulSurrender) // Aetherochemical Compound #666
                .Bait      (fish, 12710, 12776)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (16753, Patch.SoulSurrender) // Hundred-eyed Axolotl
                .Bait      (fish, 12705, 12777)
                .Tug       (BiteType.Legendary)
                .Uptime    (360, 600)
                .HookType  (HookSet.Precise);
            fish.Apply     (16754, Patch.SoulSurrender) // Bobgoblin Bass
                .Bait      (fish, 12711, 12780)
                .Tug       (BiteType.Legendary)
                .Uptime    (120, 360)
                .Transition(7)
                .Weather   (8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (16756, Patch.SoulSurrender) // Merciless
                .Bait      (fish, 2607, 12715)
                .Tug       (BiteType.Legendary)
                .Transition(15, 16)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
