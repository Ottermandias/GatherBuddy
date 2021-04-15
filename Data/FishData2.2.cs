using GatherBuddy.Classes;
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
                .Bait      (2591)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 14)
                .HookType  (HookSet.Precise);
            fish.Apply     (7679, Patch.ThroughTheMaelstrom) // Beguiler Chub
                .Bait      (2586)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 14)
                .HookType  (HookSet.Precise);
            fish.Apply     (7680, Patch.ThroughTheMaelstrom) // Oschon's Print
                .Bait      (2589)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7681, Patch.ThroughTheMaelstrom) // Caterwauler
                .Bait      (2586)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (7682, Patch.ThroughTheMaelstrom) // Crystal Perch
                .Bait      (2594)
                .Tug       (BiteType.Legendary)
                .Weather   (4, 3)
                .HookType  (HookSet.Precise);
            fish.Apply     (7683, Patch.ThroughTheMaelstrom) // Moldva
                .Bait      (2592, 4942)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7684, Patch.ThroughTheMaelstrom) // Junkmonger
                .Bait      (2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7685, Patch.ThroughTheMaelstrom) // Goldenfin
                .Bait      (2587)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 14)
                .HookType  (HookSet.Precise);
            fish.Apply     (7686, Patch.ThroughTheMaelstrom) // Gigantshark
                .Bait      (2585, 4869)
                .Tug       (BiteType.Legendary)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7687, Patch.ThroughTheMaelstrom) // Armorer
                .Bait      (2606)
                .Tug       (BiteType.Legendary)
                .Uptime    (19, 3)
                .HookType  (HookSet.Precise);
            fish.Apply     (7688, Patch.ThroughTheMaelstrom) // Great Gudgeon
                .Bait      (2588)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (7689, Patch.ThroughTheMaelstrom) // Dark Knight
                .Bait      (2614)
                .Tug       (BiteType.Legendary)
                .Weather   (4, 3, 11)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7690, Patch.ThroughTheMaelstrom) // Silver Sovereign
                .Bait      (2628)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7691, Patch.ThroughTheMaelstrom) // Sabertooth Cod
                .Bait      (2589)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 22)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7692, Patch.ThroughTheMaelstrom) // Dream Goby
                .Bait      (2588)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 3)
                .HookType  (HookSet.Precise);
            fish.Apply     (7693, Patch.ThroughTheMaelstrom) // Navigator's Brand
                .Bait      (2628)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 14)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7694, Patch.ThroughTheMaelstrom) // Dark Ambusher
                .Bait      (2586, 4927)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 3)
                .HookType  (HookSet.Precise);
            fish.Apply     (7695, Patch.ThroughTheMaelstrom) // Judgeray
                .Bait      (2623)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 21)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7696, Patch.ThroughTheMaelstrom) // Bloody Brewer
                .Bait      (2588)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7697, Patch.ThroughTheMaelstrom) // Faerie Queen
                .Bait      (2626)
                .Tug       (BiteType.Legendary)
                .Weather   (4, 3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7698, Patch.ThroughTheMaelstrom) // Slime King
                .Bait      (2588)
                .Tug       (BiteType.Legendary)
                .Uptime    (19, 24)
                .HookType  (HookSet.Precise);
            fish.Apply     (7699, Patch.ThroughTheMaelstrom) // Blue Widow
                .Bait      (2611)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 14)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7700, Patch.ThroughTheMaelstrom) // Ghost Carp
                .Bait      (2594)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 3)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7701, Patch.ThroughTheMaelstrom) // Carp Diem
                .Bait      (2614)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 14)
                .Weather   (4, 3)
                .HookType  (HookSet.Precise);
            fish.Apply     (7702, Patch.ThroughTheMaelstrom) // Mud Pilgrim
                .Bait      (2592)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 8)
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7703, Patch.ThroughTheMaelstrom) // Old Softie
                .Bait      (2590)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 21)
                .HookType  (HookSet.Precise);
            fish.Apply     (7704, Patch.ThroughTheMaelstrom) // Marrow Sucker
                .Bait      (2597)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 3)
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7705, Patch.ThroughTheMaelstrom) // Chirurgeon
                .Bait      (2586, 4927)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (7706, Patch.ThroughTheMaelstrom) // Mud Golem
                .Bait      (2594)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7707, Patch.ThroughTheMaelstrom) // Octomammoth
                .Bait      (2587, 4874)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 17)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7708, Patch.ThroughTheMaelstrom) // Matron Carp
                .Bait      (2590)
                .Tug       (BiteType.Legendary)
                .Uptime    (15, 21)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7709, Patch.ThroughTheMaelstrom) // High Perch
                .Bait      (2617)
                .Tug       (BiteType.Legendary)
                .Uptime    (5, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7710, Patch.ThroughTheMaelstrom) // Syldra
                .Bait      (2596)
                .Tug       (BiteType.Legendary)
                .Weather   (7)
                .HookType  (HookSet.Precise);
            fish.Apply     (7711, Patch.ThroughTheMaelstrom) // Rivet Oyster
                .Bait      (2619)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7712, Patch.ThroughTheMaelstrom) // Jacques the Snipper
                .Bait      (2589)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 3)
                .HookType  (HookSet.Precise);
            fish.Apply     (7713, Patch.ThroughTheMaelstrom) // Stormdancer
                .Bait      (2601)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 2)
                .Weather   (7, 15)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7714, Patch.ThroughTheMaelstrom) // Glimmerscale
                .Bait      (2594)
                .Tug       (BiteType.Legendary)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
