using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyThroughTheMaelstrom(this GameData data)
    {
        data.Apply     (7678, Patch.ThroughTheMaelstrom) // Zalera
            .Bait      (data, 2591)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (540, 840);
        data.Apply     (7679, Patch.ThroughTheMaelstrom) // Beguiler Chub
            .Bait      (data, 2586)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (540, 840);
        data.Apply     (7680, Patch.ThroughTheMaelstrom) // Oschon's Print
            .Bait      (data, 2589)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (7681, Patch.ThroughTheMaelstrom) // Caterwauler
            .Bait      (data, 2586)
            .Bite      (HookSet.Precise, BiteType.Legendary);
        data.Apply     (7682, Patch.ThroughTheMaelstrom) // Crystal Perch
            .Bait      (data, 2594)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Weather   (data, 4, 3, 5);
        data.Apply     (7683, Patch.ThroughTheMaelstrom) // Moldva
            .Bait      (data, 2592, 4942)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1080, 120);
        data.Apply     (7684, Patch.ThroughTheMaelstrom) // Junkmonger
            .Bait      (data, 2585, 4869, 4904)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 120);
        data.Apply     (7685, Patch.ThroughTheMaelstrom) // Goldenfin
            .Bait      (data, 2587)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (540, 840);
        data.Apply     (7686, Patch.ThroughTheMaelstrom) // Gigantshark
            .Bait      (data, 2585, 4869)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 1, 2);
        data.Apply     (7687, Patch.ThroughTheMaelstrom) // Armorer
            .Bait      (data, 2606)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1140, 180);
        data.Apply     (7688, Patch.ThroughTheMaelstrom) // Great Gudgeon
            .Bait      (data, 2588)
            .Bite      (HookSet.Precise, BiteType.Legendary);
        data.Apply     (7689, Patch.ThroughTheMaelstrom) // Dark Knight
            .Bait      (data, 2614)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 4, 3, 11);
        data.Apply     (7690, Patch.ThroughTheMaelstrom) // Silver Sovereign
            .Bait      (data, 2628)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (7691, Patch.ThroughTheMaelstrom) // Sabertooth Cod
            .Bait      (data, 2589)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 1320);
        data.Apply     (7692, Patch.ThroughTheMaelstrom) // Dream Goby
            .Bait      (data, 2588)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1020, 180);
        data.Apply     (7693, Patch.ThroughTheMaelstrom) // Navigator's Brand
            .Bait      (data, 2628)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (540, 840)
            .Weather   (data, 1, 2);
        data.Apply     (7694, Patch.ThroughTheMaelstrom) // Dark Ambusher
            .Bait      (data, 2586, 4927)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1260, 180);
        data.Apply     (7695, Patch.ThroughTheMaelstrom) // Judgeray
            .Bait      (data, 2623)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1020, 1260);
        data.Apply     (7696, Patch.ThroughTheMaelstrom) // Bloody Brewer
            .Bait      (data, 2588)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (7697, Patch.ThroughTheMaelstrom) // Faerie Queen
            .Bait      (data, 2626)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 4, 3, 5);
        data.Apply     (7698, Patch.ThroughTheMaelstrom) // Slime King
            .Bait      (data, 2588)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1140, 1440);
        data.Apply     (7699, Patch.ThroughTheMaelstrom) // Blue Widow
            .Bait      (data, 2611)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (540, 840);
        data.Apply     (7700, Patch.ThroughTheMaelstrom) // Ghost Carp
            .Bait      (data, 2594)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1260, 180)
            .Weather   (data, 7);
        data.Apply     (7701, Patch.ThroughTheMaelstrom) // Carp Diem
            .Bait      (data, 2614)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (540, 840)
            .Weather   (data, 4, 3);
        data.Apply     (7702, Patch.ThroughTheMaelstrom) // Mud Pilgrim
            .Bait      (data, 2592)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1020, 420)
            .Weather   (data, 7, 8);
        data.Apply     (7703, Patch.ThroughTheMaelstrom) // Old Softie
            .Bait      (data, 2590)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1020, 1260);
        data.Apply     (7704, Patch.ThroughTheMaelstrom) // Marrow Sucker
            .Bait      (data, 2597)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1200, 180)
            .Weather   (data, 7, 8);
        data.Apply     (7705, Patch.ThroughTheMaelstrom) // Chirurgeon
            .Bait      (data, 2586, 4927)
            .Bite      (HookSet.Precise, BiteType.Legendary);
        data.Apply     (7706, Patch.ThroughTheMaelstrom) // Mud Golem
            .Bait      (data, 2594)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1260, 180);
        data.Apply     (7707, Patch.ThroughTheMaelstrom) // Octomammoth
            .Bait      (data, 2587, 4874)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (540, 1020);
        data.Apply     (7708, Patch.ThroughTheMaelstrom) // Matron Carp
            .Bait      (data, 2590)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (900, 1260);
        data.Apply     (7709, Patch.ThroughTheMaelstrom) // High Perch
            .Bait      (data, 2617)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (300, 480);
        data.Apply     (7710, Patch.ThroughTheMaelstrom) // Syldra
            .Bait      (data, 2596)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Weather   (data, 7);
        data.Apply     (7711, Patch.ThroughTheMaelstrom) // Rivet Oyster
            .Bait      (data, 2619)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (7712, Patch.ThroughTheMaelstrom) // Jacques the Snipper
            .Bait      (data, 2589)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1260, 180);
        data.Apply     (7713, Patch.ThroughTheMaelstrom) // Stormdancer
            .Bait      (data, 2601)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1020, 120)
            .Weather   (data, 7, 15);
        data.Apply     (7714, Patch.ThroughTheMaelstrom) // Glimmerscale
            .Bait      (data, 2594)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Weather   (data, 1, 2);
    }
    // @formatter:on
}
