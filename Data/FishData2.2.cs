using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyThroughTheMaelstrom(this FishManager fish)
        {
            fish.Apply     (7678, Patch.ThroughTheMaelstrom) // Zalera
                .Bait      (fish, 2591)
                .Tug       (BiteType.Legendary)
                .Uptime    (540, 840)
                .HookType  (HookSet.Precise);
            fish.Apply     (7679, Patch.ThroughTheMaelstrom) // Beguiler Chub
                .Bait      (fish, 2586)
                .Tug       (BiteType.Legendary)
                .Uptime    (540, 840)
                .HookType  (HookSet.Precise);
            fish.Apply     (7680, Patch.ThroughTheMaelstrom) // Oschon's Print
                .Bait      (fish, 2589)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7681, Patch.ThroughTheMaelstrom) // Caterwauler
                .Bait      (fish, 2586)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (7682, Patch.ThroughTheMaelstrom) // Crystal Perch
                .Bait      (fish, 2594)
                .Tug       (BiteType.Legendary)
                .Weather   (4, 3, 5)
                .HookType  (HookSet.Precise);
            fish.Apply     (7683, Patch.ThroughTheMaelstrom) // Moldva
                .Bait      (fish, 2592, 4942)
                .Tug       (BiteType.Legendary)
                .Uptime    (1080, 120)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7684, Patch.ThroughTheMaelstrom) // Junkmonger
                .Bait      (fish, 2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .Uptime    (960, 120)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7685, Patch.ThroughTheMaelstrom) // Goldenfin
                .Bait      (fish, 2587)
                .Tug       (BiteType.Legendary)
                .Uptime    (540, 840)
                .HookType  (HookSet.Precise);
            fish.Apply     (7686, Patch.ThroughTheMaelstrom) // Gigantshark
                .Bait      (fish, 2585, 4869)
                .Tug       (BiteType.Legendary)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7687, Patch.ThroughTheMaelstrom) // Armorer
                .Bait      (fish, 2606)
                .Tug       (BiteType.Legendary)
                .Uptime    (1140, 180)
                .HookType  (HookSet.Precise);
            fish.Apply     (7688, Patch.ThroughTheMaelstrom) // Great Gudgeon
                .Bait      (fish, 2588)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (7689, Patch.ThroughTheMaelstrom) // Dark Knight
                .Bait      (fish, 2614)
                .Tug       (BiteType.Legendary)
                .Weather   (4, 3, 11)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7690, Patch.ThroughTheMaelstrom) // Silver Sovereign
                .Bait      (fish, 2628)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7691, Patch.ThroughTheMaelstrom) // Sabertooth Cod
                .Bait      (fish, 2589)
                .Tug       (BiteType.Legendary)
                .Uptime    (960, 1320)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7692, Patch.ThroughTheMaelstrom) // Dream Goby
                .Bait      (fish, 2588)
                .Tug       (BiteType.Legendary)
                .Uptime    (1020, 180)
                .HookType  (HookSet.Precise);
            fish.Apply     (7693, Patch.ThroughTheMaelstrom) // Navigator's Brand
                .Bait      (fish, 2628)
                .Tug       (BiteType.Legendary)
                .Uptime    (540, 840)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7694, Patch.ThroughTheMaelstrom) // Dark Ambusher
                .Bait      (fish, 2586, 4927)
                .Tug       (BiteType.Legendary)
                .Uptime    (1260, 180)
                .HookType  (HookSet.Precise);
            fish.Apply     (7695, Patch.ThroughTheMaelstrom) // Judgeray
                .Bait      (fish, 2623)
                .Tug       (BiteType.Legendary)
                .Uptime    (1020, 1260)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7696, Patch.ThroughTheMaelstrom) // Bloody Brewer
                .Bait      (fish, 2588)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7697, Patch.ThroughTheMaelstrom) // Faerie Queen
                .Bait      (fish, 2626)
                .Tug       (BiteType.Legendary)
                .Weather   (4, 3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7698, Patch.ThroughTheMaelstrom) // Slime King
                .Bait      (fish, 2588)
                .Tug       (BiteType.Legendary)
                .Uptime    (1140, 1440)
                .HookType  (HookSet.Precise);
            fish.Apply     (7699, Patch.ThroughTheMaelstrom) // Blue Widow
                .Bait      (fish, 2611)
                .Tug       (BiteType.Legendary)
                .Uptime    (540, 840)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7700, Patch.ThroughTheMaelstrom) // Ghost Carp
                .Bait      (fish, 2594)
                .Tug       (BiteType.Legendary)
                .Uptime    (1260, 180)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7701, Patch.ThroughTheMaelstrom) // Carp Diem
                .Bait      (fish, 2614)
                .Tug       (BiteType.Legendary)
                .Uptime    (540, 840)
                .Weather   (4, 3)
                .HookType  (HookSet.Precise);
            fish.Apply     (7702, Patch.ThroughTheMaelstrom) // Mud Pilgrim
                .Bait      (fish, 2592)
                .Tug       (BiteType.Legendary)
                .Uptime    (1020, 420)
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7703, Patch.ThroughTheMaelstrom) // Old Softie
                .Bait      (fish, 2590)
                .Tug       (BiteType.Legendary)
                .Uptime    (1020, 1260)
                .HookType  (HookSet.Precise);
            fish.Apply     (7704, Patch.ThroughTheMaelstrom) // Marrow Sucker
                .Bait      (fish, 2597)
                .Tug       (BiteType.Legendary)
                .Uptime    (1200, 180)
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7705, Patch.ThroughTheMaelstrom) // Chirurgeon
                .Bait      (fish, 2586, 4927)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (7706, Patch.ThroughTheMaelstrom) // Mud Golem
                .Bait      (fish, 2594)
                .Tug       (BiteType.Legendary)
                .Uptime    (1260, 180)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7707, Patch.ThroughTheMaelstrom) // Octomammoth
                .Bait      (fish, 2587, 4874)
                .Tug       (BiteType.Legendary)
                .Uptime    (540, 1020)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7708, Patch.ThroughTheMaelstrom) // Matron Carp
                .Bait      (fish, 2590)
                .Tug       (BiteType.Legendary)
                .Uptime    (900, 1260)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7709, Patch.ThroughTheMaelstrom) // High Perch
                .Bait      (fish, 2617)
                .Tug       (BiteType.Legendary)
                .Uptime    (300, 480)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7710, Patch.ThroughTheMaelstrom) // Syldra
                .Bait      (fish, 2596)
                .Tug       (BiteType.Legendary)
                .Weather   (7)
                .HookType  (HookSet.Precise);
            fish.Apply     (7711, Patch.ThroughTheMaelstrom) // Rivet Oyster
                .Bait      (fish, 2619)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7712, Patch.ThroughTheMaelstrom) // Jacques the Snipper
                .Bait      (fish, 2589)
                .Tug       (BiteType.Legendary)
                .Uptime    (1260, 180)
                .HookType  (HookSet.Precise);
            fish.Apply     (7713, Patch.ThroughTheMaelstrom) // Stormdancer
                .Bait      (fish, 2601)
                .Tug       (BiteType.Legendary)
                .Uptime    (1020, 120)
                .Weather   (7, 15)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7714, Patch.ThroughTheMaelstrom) // Glimmerscale
                .Bait      (fish, 2594)
                .Tug       (BiteType.Legendary)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
