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
                .Bait      (fish, 2597)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 19)
                .HookType  (HookSet.Precise);
            fish.Apply     (7903, Patch.DefendersOfEorzea) // Meteor Survivor
                .Bait      (fish, 2591)
                .Tug       (BiteType.Legendary)
                .Uptime    (3, 5)
                .Weather   (3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7904, Patch.DefendersOfEorzea) // Joan of Trout
                .Bait      (fish, 2614)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 6)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7905, Patch.DefendersOfEorzea) // Toramafish
                .Bait      (fish, 2620, 4995)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 20)
                .Weather   (4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7906, Patch.DefendersOfEorzea) // Fingers
                .Bait      (fish, 2606)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 18)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7907, Patch.DefendersOfEorzea) // The Assassin
                .Bait      (fish, 2594)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 23)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7908, Patch.DefendersOfEorzea) // Vip Viper
                .Bait      (fish, 2594)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 19)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7909, Patch.DefendersOfEorzea) // The Greatest Bream in the World
                .Bait      (fish, 2613)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 23)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7910, Patch.DefendersOfEorzea) // Dirty Herry
                .Bait      (fish, 2606)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 22)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7911, Patch.DefendersOfEorzea) // Old Hollow Eyes
                .Bait      (fish, 2592, 4948)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7912, Patch.DefendersOfEorzea) // Sylphsbane
                .Bait      (fish, 2594, 4948)
                .Tug       (BiteType.Legendary)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7913, Patch.DefendersOfEorzea) // Floating Boulder
                .Bait      (fish, 2595)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 8)
                .Weather   (4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7914, Patch.DefendersOfEorzea) // The Grinner
                .Bait      (fish, 2597)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7915, Patch.DefendersOfEorzea) // Shark Tuna
                .Bait      (fish, 2596, 4898)
                .Tug       (BiteType.Legendary)
                .Uptime    (19, 21)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7916, Patch.DefendersOfEorzea) // Worm of Nym
                .Bait      (fish, 2594)
                .Tug       (BiteType.Legendary)
                .Uptime    (19, 22)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7917, Patch.DefendersOfEorzea) // Twitchbeard
                .Bait      (fish, 2596, 4898)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 6)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7918, Patch.DefendersOfEorzea) // The Warden's Wand
                .Bait      (fish, 2599)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 20)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7919, Patch.DefendersOfEorzea) // Spearnose
                .Bait      (fish, 2607)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 24)
                .Weather   (4, 3)
                .HookType  (HookSet.Precise);
            fish.Apply     (7920, Patch.DefendersOfEorzea) // Levinlight
                .Bait      (fish, 2597)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 23)
                .HookType  (HookSet.Precise);
            fish.Apply     (7921, Patch.DefendersOfEorzea) // The Sinker
                .Bait      (fish, 2617)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 3)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7922, Patch.DefendersOfEorzea) // The Gobfather
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Legendary)
                .Weather   (9, 10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7923, Patch.DefendersOfEorzea) // Sweetnewt
                .Bait      (fish, 2592, 4942)
                .Tug       (BiteType.Legendary)
                .Uptime    (23, 5)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7924, Patch.DefendersOfEorzea) // Bombardfish
                .Bait      (fish, 2598)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 15)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7925, Patch.DefendersOfEorzea) // The Salter
                .Bait      (fish, 2599)
                .Tug       (BiteType.Weak)
                .Uptime    (17, 20)
                .HookType  (HookSet.Precise);
            fish.Apply     (7926, Patch.DefendersOfEorzea) // The Lone Ripper
                .Bait      (fish, 2619)
                .Tug       (BiteType.Legendary)
                .Weather   (6)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7927, Patch.DefendersOfEorzea) // King of the Spring
                .Bait      (fish, 2626)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 19)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7928, Patch.DefendersOfEorzea) // Discobolus
                .Bait      (fish, 2603)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 18)
                .Weather   (14)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7929, Patch.DefendersOfEorzea) // Iron Noose
                .Bait      (fish, 2620)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7930, Patch.DefendersOfEorzea) // Olgoi-Khorkhoi
                .Bait      (fish, 2600, 5035)
                .Tug       (BiteType.Legendary)
                .Weather   (14)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7931, Patch.DefendersOfEorzea) // Magic Carpet
                .Bait      (fish, 2600, 5035)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 16)
                .Weather   (14)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7932, Patch.DefendersOfEorzea) // Daniffen's Mark
                .Bait      (fish, 2623)
                .Tug       (BiteType.Legendary)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7933, Patch.DefendersOfEorzea) // Charon's Lantern
                .Bait      (fish, 2603)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 4)
                .HookType  (HookSet.Precise);
            fish.Apply     (7934, Patch.DefendersOfEorzea) // The Green Jester
                .Bait      (fish, 2599)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 21)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7935, Patch.DefendersOfEorzea) // Bloodbath
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Legendary)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7936, Patch.DefendersOfEorzea) // Son of Levin
                .Bait      (fish, 2607)
                .Tug       (BiteType.Legendary)
                .Weather   (10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7937, Patch.DefendersOfEorzea) // Thundergut
                .Bait      (fish, 2601)
                .Tug       (BiteType.Legendary)
                .Uptime    (19, 3)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7938, Patch.DefendersOfEorzea) // The Drowned Sniper
                .Bait      (fish, 2618)
                .Tug       (BiteType.Legendary)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7939, Patch.DefendersOfEorzea) // The Terpsichorean
                .Bait      (fish, 2599)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7940, Patch.DefendersOfEorzea) // Mirrorscale
                .Bait      (fish, 2592, 4948)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 16)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (7941, Patch.DefendersOfEorzea) // Helmsman's Hand
                .Bait      (fish, 2587, 4872)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 15)
                .Weather   (4, 3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7942, Patch.DefendersOfEorzea) // The Thousand-year Itch
                .Bait      (fish, 2601)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7943, Patch.DefendersOfEorzea) // Hannibal
                .Bait      (fish, 2626, 4995)
                .Tug       (BiteType.Legendary)
                .Uptime    (22, 4)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7944, Patch.DefendersOfEorzea) // Dawn Maiden
                .Bait      (fish, 2623)
                .Tug       (BiteType.Legendary)
                .Uptime    (5, 7)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7945, Patch.DefendersOfEorzea) // Starbright
                .Bait      (fish, 2597, 4937)
                .Tug       (BiteType.Legendary)
                .Uptime    (21, 4)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7946, Patch.DefendersOfEorzea) // The Matriarch
                .Bait      (fish, 2599, 4937)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (7947, Patch.DefendersOfEorzea) // Shadowstreak
                .Bait      (fish, 2624)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 10)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7948, Patch.DefendersOfEorzea) // The Captain's Chalice
                .Bait      (fish, 2616, 4898)
                .Tug       (BiteType.Legendary)
                .Uptime    (23, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7949, Patch.DefendersOfEorzea) // Anomalocaris
                .Bait      (fish, 2605, 5040)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 15)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7950, Patch.DefendersOfEorzea) // Frilled Shark
                .Bait      (fish, 2585, 4869, 4904, 4919)
                .Tug       (BiteType.Legendary)
                .Uptime    (17, 3)
                .Weather   (4, 3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (7951, Patch.DefendersOfEorzea) // Aetherlouse
                .Bait      (fish, 2603)
                .Tug       (BiteType.Legendary)
                .Uptime    (3, 13)
                .Weather   (17)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
