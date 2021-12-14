using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyDefendersOfEorzea(this GameData data)
    {
        data.Apply     (7902, Patch.DefendersOfEorzea) // Cupfish
            .Bait      (data, 2597)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1020, 1140);
        data.Apply     (7903, Patch.DefendersOfEorzea) // Meteor Survivor
            .Bait      (data, 2591)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (180, 300)
            .Weather   (data, 3, 4, 5);
        data.Apply     (7904, Patch.DefendersOfEorzea) // Joan of Trout
            .Bait      (data, 2614)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (240, 360);
        data.Apply     (7905, Patch.DefendersOfEorzea) // Toramafish
            .Bait      (data, 2620, 4995)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1020, 1200)
            .Weather   (data, 4, 3);
        data.Apply     (7906, Patch.DefendersOfEorzea) // Fingers
            .Bait      (data, 2606)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1020, 1080);
        data.Apply     (7907, Patch.DefendersOfEorzea) // The Assassin
            .Bait      (data, 2594)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1260, 1380);
        data.Apply     (7908, Patch.DefendersOfEorzea) // Vip Viper
            .Bait      (data, 2594)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1080, 1140);
        data.Apply     (7909, Patch.DefendersOfEorzea) // The Greatest Bream in the World
            .Bait      (data, 2613)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1200, 1380);
        data.Apply     (7910, Patch.DefendersOfEorzea) // Dirty Herry
            .Bait      (data, 2606)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1200, 1320);
        data.Apply     (7911, Patch.DefendersOfEorzea) // Old Hollow Eyes
            .Bait      (data, 2592, 4948)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 4);
        data.Apply     (7912, Patch.DefendersOfEorzea) // Sylphsbane
            .Bait      (data, 2594, 4948)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 7);
        data.Apply     (7913, Patch.DefendersOfEorzea) // Floating Boulder
            .Bait      (data, 2595)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (0, 480)
            .Weather   (data, 4, 3);
        data.Apply     (7914, Patch.DefendersOfEorzea) // The Grinner
            .Bait      (data, 2597)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (7915, Patch.DefendersOfEorzea) // Shark Tuna
            .Bait      (data, 2596, 4898)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1140, 1260)
            .Weather   (data, 1, 2);
        data.Apply     (7916, Patch.DefendersOfEorzea) // Worm of Nym
            .Bait      (data, 2594)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1140, 1320);
        data.Apply     (7917, Patch.DefendersOfEorzea) // Twitchbeard
            .Bait      (data, 2596, 4898)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (240, 360)
            .Weather   (data, 1, 2);
        data.Apply     (7918, Patch.DefendersOfEorzea) // The Warden's Wand
            .Bait      (data, 2599)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1020, 1200)
            .Weather   (data, 1);
        data.Apply     (7919, Patch.DefendersOfEorzea) // Spearnose
            .Bait      (data, 2607)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1260, 1440)
            .Weather   (data, 4, 3);
        data.Apply     (7920, Patch.DefendersOfEorzea) // Levinlight
            .Bait      (data, 2597)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1080, 1380);
        data.Apply     (7921, Patch.DefendersOfEorzea) // The Sinker
            .Bait      (data, 2617)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1260, 180)
            .Weather   (data, 1, 2);
        data.Apply     (7922, Patch.DefendersOfEorzea) // The Gobfather
            .Bait      (data, 2599, 4978)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 9, 10);
        data.Apply     (7923, Patch.DefendersOfEorzea) // Sweetnewt
            .Bait      (data, 2592, 4942)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1380, 240)
            .Weather   (data, 4);
        data.Apply     (7924, Patch.DefendersOfEorzea) // Bombardfish
            .Bait      (data, 2598)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (540, 900)
            .Weather   (data, 1);
        data.Apply     (7925, Patch.DefendersOfEorzea) // The Salter
            .Bait      (data, 2599)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (1020, 1200);
        data.Apply     (7926, Patch.DefendersOfEorzea) // The Lone Ripper
            .Bait      (data, 2619)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 6);
        data.Apply     (7927, Patch.DefendersOfEorzea) // King of the Spring
            .Bait      (data, 2626)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 1140);
        data.Apply     (7928, Patch.DefendersOfEorzea) // Discobolus
            .Bait      (data, 2603)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (720, 1080)
            .Weather   (data, 14);
        data.Apply     (7929, Patch.DefendersOfEorzea) // Iron Noose
            .Bait      (data, 2620)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 4);
        data.Apply     (7930, Patch.DefendersOfEorzea) // Olgoi-Khorkhoi
            .Bait      (data, 2600, 5035)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 14);
        data.Apply     (7931, Patch.DefendersOfEorzea) // Magic Carpet
            .Bait      (data, 2600, 5035)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (540, 960)
            .Weather   (data, 14);
        data.Apply     (7932, Patch.DefendersOfEorzea) // Daniffen's Mark
            .Bait      (data, 2623)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 16);
        data.Apply     (7933, Patch.DefendersOfEorzea) // Charon's Lantern
            .Bait      (data, 2603)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1320, 240);
        data.Apply     (7934, Patch.DefendersOfEorzea) // The Green Jester
            .Bait      (data, 2599)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1080, 1260);
        data.Apply     (7935, Patch.DefendersOfEorzea) // Bloodbath
            .Bait      (data, 2599, 4978)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 9);
        data.Apply     (7936, Patch.DefendersOfEorzea) // Son of Levin
            .Bait      (data, 2607)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 10);
        data.Apply     (7937, Patch.DefendersOfEorzea) // Thundergut
            .Bait      (data, 2601)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1140, 180)
            .Weather   (data, 7);
        data.Apply     (7938, Patch.DefendersOfEorzea) // The Drowned Sniper
            .Bait      (data, 2618)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 1, 2);
        data.Apply     (7939, Patch.DefendersOfEorzea) // The Terpsichorean
            .Bait      (data, 2599)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 4);
        data.Apply     (7940, Patch.DefendersOfEorzea) // Mirrorscale
            .Bait      (data, 2592, 4948)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (540, 960)
            .Weather   (data, 1, 2);
        data.Apply     (7941, Patch.DefendersOfEorzea) // Helmsman's Hand
            .Bait      (data, 2587, 4872)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (540, 900)
            .Weather   (data, 4, 3, 5);
        data.Apply     (7942, Patch.DefendersOfEorzea) // The Thousand-year Itch
            .Bait      (data, 2601)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 4);
        data.Apply     (7943, Patch.DefendersOfEorzea) // Hannibal
            .Bait      (data, 2626, 4995)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1320, 240)
            .Weather   (data, 4);
        data.Apply     (7944, Patch.DefendersOfEorzea) // Dawn Maiden
            .Bait      (data, 2623)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (300, 420)
            .Weather   (data, 1, 2);
        data.Apply     (7945, Patch.DefendersOfEorzea) // Starbright
            .Bait      (data, 2597, 4937)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1260, 240)
            .Weather   (data, 1, 2);
        data.Apply     (7946, Patch.DefendersOfEorzea) // The Matriarch
            .Bait      (data, 2599, 4937)
            .Bite      (HookSet.Precise, BiteType.Legendary);
        data.Apply     (7947, Patch.DefendersOfEorzea) // Shadowstreak
            .Bait      (data, 2624)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (240, 600)
            .Weather   (data, 4);
        data.Apply     (7948, Patch.DefendersOfEorzea) // The Captain's Chalice
            .Bait      (data, 2616, 4898)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1380, 120);
        data.Apply     (7949, Patch.DefendersOfEorzea) // Anomalocaris
            .Bait      (data, 2605, 5040)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (600, 900)
            .Weather   (data, 1, 2);
        data.Apply     (7950, Patch.DefendersOfEorzea) // Frilled Shark
            .Bait      (data, 2585, 4869, 4904, 4919)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1020, 180)
            .Weather   (data, 4, 3, 5);
        data.Apply     (7951, Patch.DefendersOfEorzea) // Aetherlouse
            .Bait      (data, 2603)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (180, 780)
            .Weather   (data, 17);
    }
    // @formatter:on
}
