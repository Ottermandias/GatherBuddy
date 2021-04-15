using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyDefendersOfEorzea(this FishManager fish)
        {
            fish.Apply     (7902, Patch.DefendersOfEorzea) // Cupfish
                .Bait      (2597)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 19)
                .HookType  (HookSet.Precise);
            fish.Apply     (7903, Patch.DefendersOfEorzea) // Meteor Survivor
                .Bait      (2591)
                .Tug       (BiteType.Legendary)
                .Uptime    (3, 5)
                .Weather   (3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7904, Patch.DefendersOfEorzea) // Joan of Trout
                .Bait      (2614)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 6)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7905, Patch.DefendersOfEorzea) // Toramafish
                .Bait      (2620, 4995)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 20)
                .Weather   (4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7906, Patch.DefendersOfEorzea) // Fingers
                .Bait      (2606)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 18)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7907, Patch.DefendersOfEorzea) // The Assassin
                .Bait      (2594)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 23)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7908, Patch.DefendersOfEorzea) // Vip Viper
                .Bait      (2594)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 19)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7909, Patch.DefendersOfEorzea) // The Greatest Bream in the World
                .Bait      (2613)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 23)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7910, Patch.DefendersOfEorzea) // Dirty Herry
                .Bait      (2606)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 22)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7911, Patch.DefendersOfEorzea) // Old Hollow Eyes
                .Bait      (2592, 4948)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7912, Patch.DefendersOfEorzea) // Sylphsbane
                .Bait      (2594, 4948)
                .Tug       (BiteType.Legendary)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7913, Patch.DefendersOfEorzea) // Floating Boulder
                .Bait      (2595)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 8)
                .Weather   (4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7914, Patch.DefendersOfEorzea) // The Grinner
                .Bait      (2597)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7915, Patch.DefendersOfEorzea) // Shark Tuna
                .Bait      (2596, 4898)
                .Tug       (BiteType.Legendary)
                .Uptime    (19, 21)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7916, Patch.DefendersOfEorzea) // Worm of Nym
                .Bait      (2594)
                .Tug       (BiteType.Legendary)
                .Uptime    (19, 22)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7917, Patch.DefendersOfEorzea) // Twitchbeard
                .Bait      (2596, 4898)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 6)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7918, Patch.DefendersOfEorzea) // The Warden's Wand
                .Bait      (2599)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 20)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7919, Patch.DefendersOfEorzea) // Spearnose
                .Bait      (2607)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 24)
                .Weather   (4, 3)
                .HookType  (HookSet.Precise);
            fish.Apply     (7920, Patch.DefendersOfEorzea) // Levinlight
                .Bait      (2597)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 23)
                .HookType  (HookSet.Precise);
            fish.Apply     (7921, Patch.DefendersOfEorzea) // The Sinker
                .Bait      (2617)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 3)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7922, Patch.DefendersOfEorzea) // The Gobfather
                .Bait      (2599, 4978)
                .Tug       (BiteType.Legendary)
                .Weather   (9, 10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7923, Patch.DefendersOfEorzea) // Sweetnewt
                .Bait      (2592, 4942)
                .Tug       (BiteType.Legendary)
                .Uptime    (23, 5)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7924, Patch.DefendersOfEorzea) // Bombardfish
                .Bait      (2598)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 15)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7925, Patch.DefendersOfEorzea) // The Salter
                .Bait      (2599)
                .Tug       (BiteType.Weak)
                .Uptime    (17, 20)
                .HookType  (HookSet.Precise);
            fish.Apply     (7926, Patch.DefendersOfEorzea) // The Lone Ripper
                .Bait      (2619)
                .Tug       (BiteType.Legendary)
                .Weather   (6)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7927, Patch.DefendersOfEorzea) // King of the Spring
                .Bait      (2626)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 19)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7928, Patch.DefendersOfEorzea) // Discobolus
                .Bait      (2603)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 18)
                .Weather   (14)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7929, Patch.DefendersOfEorzea) // Iron Noose
                .Bait      (2620)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7930, Patch.DefendersOfEorzea) // Olgoi-Khorkhoi
                .Bait      (2600, 5035)
                .Tug       (BiteType.Legendary)
                .Weather   (14)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7931, Patch.DefendersOfEorzea) // Magic Carpet
                .Bait      (2600, 5035)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 16)
                .Weather   (14)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7932, Patch.DefendersOfEorzea) // Daniffen's Mark
                .Bait      (2623)
                .Tug       (BiteType.Legendary)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7933, Patch.DefendersOfEorzea) // Charon's Lantern
                .Bait      (2603)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 4)
                .HookType  (HookSet.Precise);
            fish.Apply     (7934, Patch.DefendersOfEorzea) // The Green Jester
                .Bait      (2599)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 21)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7935, Patch.DefendersOfEorzea) // Bloodbath
                .Bait      (2599, 4978)
                .Tug       (BiteType.Legendary)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7936, Patch.DefendersOfEorzea) // Son of Levin
                .Bait      (2607)
                .Tug       (BiteType.Legendary)
                .Weather   (10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7937, Patch.DefendersOfEorzea) // Thundergut
                .Bait      (2601)
                .Tug       (BiteType.Legendary)
                .Uptime    (19, 3)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7938, Patch.DefendersOfEorzea) // The Drowned Sniper
                .Bait      (2618)
                .Tug       (BiteType.Legendary)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7939, Patch.DefendersOfEorzea) // The Terpsichorean
                .Bait      (2599)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7940, Patch.DefendersOfEorzea) // Mirrorscale
                .Bait      (2592, 4948)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 16)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (7941, Patch.DefendersOfEorzea) // Helmsman's Hand
                .Bait      (2587, 4872)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 15)
                .Weather   (4, 3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7942, Patch.DefendersOfEorzea) // The Thousand-year Itch
                .Bait      (2601)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7943, Patch.DefendersOfEorzea) // Hannibal
                .Bait      (2626, 4995)
                .Tug       (BiteType.Legendary)
                .Uptime    (22, 4)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7944, Patch.DefendersOfEorzea) // Dawn Maiden
                .Bait      (2623)
                .Tug       (BiteType.Legendary)
                .Uptime    (5, 7)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7945, Patch.DefendersOfEorzea) // Starbright
                .Bait      (2597, 4937)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 4)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7946, Patch.DefendersOfEorzea) // The Matriarch
                .Bait      (2599, 4937)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (7947, Patch.DefendersOfEorzea) // Shadowstreak
                .Bait      (2624)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 10)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7948, Patch.DefendersOfEorzea) // The Captain's Chalice
                .Bait      (2616, 4898)
                .Tug       (BiteType.Legendary)
                .Uptime    (23, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7949, Patch.DefendersOfEorzea) // Anomalocaris
                .Bait      (2605, 5040)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 15)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7950, Patch.DefendersOfEorzea) // Frilled Shark
                .Bait      (2585, 4869, 4904, 4919)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 3)
                .Weather   (4, 3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7951, Patch.DefendersOfEorzea) // Aetherlouse
                .Bait      (2603)
                .Tug       (BiteType.Legendary)
                .Uptime    (3, 13)
                .Weather   (17)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
