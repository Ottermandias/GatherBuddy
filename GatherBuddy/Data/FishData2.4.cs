using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyDreamsOfIce(this FishManager fish)
        {
            fish.Apply     (8752, Patch.DreamsOfIce) // Imperial Goldfish
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Legendary)
                .Predators (fish, (5031, 3))
                .Intuition (600)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8753, Patch.DreamsOfIce) // The Old Man in the Sea
                .Bait      (fish, 2587, 4874, 4888)
                .Tug       (BiteType.Legendary)
                .Transition(7, 8)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8754, Patch.DreamsOfIce) // Nepto Dragon
                .Bait      (fish, 2606)
                .Tug       (BiteType.Legendary)
                .Predators (fish, (4913, 3))
                .Intuition (350)
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8755, Patch.DreamsOfIce) // Coelacanthus
                .Bait      (fish, 2596, 4898)
                .Tug       (BiteType.Legendary)
                .Uptime    (1320, 180)
                .Weather   (4, 3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8756, Patch.DreamsOfIce) // Endoceras
                .Bait      (fish, 2596, 4898)
                .Tug       (BiteType.Legendary)
                .Uptime    (1200, 240)
                .Transition(1, 2)
                .Weather   (4, 3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8757, Patch.DreamsOfIce) // Seahag
                .Bait      (fish, 2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .Uptime    (1140, 120)
                .Transition(1, 2)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8758, Patch.DreamsOfIce) // Ignus Horn
                .Bait      (fish, 2603)
                .Tug       (BiteType.Legendary)
                .Transition(17)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (8759, Patch.DreamsOfIce) // Void Bass
                .Bait      (fish, 2603)
                .Tug       (BiteType.Legendary)
                .Transition(1, 2)
                .Weather   (17)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8760, Patch.DreamsOfIce) // Cornelia
                .Bait      (fish, 2625)
                .Tug       (BiteType.Legendary)
                .Predators (fish, (5008, 5))
                .Intuition (300)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8761, Patch.DreamsOfIce) // Ninja Betta
                .Bait      (fish, 2592, 4942, 5002)
                .Tug       (BiteType.Legendary)
                .Uptime    (1080, 540)
                .Weather   (17)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8762, Patch.DreamsOfIce) // Canavan
                .Bait      (fish, 2599, 4978, 5011)
                .Tug       (BiteType.Legendary)
                .Uptime    (480, 1080)
                .Weather   (17)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8763, Patch.DreamsOfIce) // Kuno the Killer
                .Bait      (fish, 2599, 4978, 5002)
                .Tug       (BiteType.Legendary)
                .Predators (fish, (8762, 1))
                .Intuition (1400)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8764, Patch.DreamsOfIce) // Pirate's Bane
                .Bait      (fish, 2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .Predators (fish, (4904, 6))
                .Intuition (120)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8765, Patch.DreamsOfIce) // Ndendecki
                .Bait      (fish, 2599, 4978, 5002)
                .Tug       (BiteType.Legendary)
                .Uptime    (1080, 300)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8766, Patch.DreamsOfIce) // Bat-o'-Nine-Tails
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Legendary)
                .Transition(7)
                .Weather   (4, 3, 1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8767, Patch.DreamsOfIce) // Wootz Knifefish Zenith
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Legendary)
                .Uptime    (60, 240)
                .Transition(1, 2)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8768, Patch.DreamsOfIce) // Helicoprion
                .Bait      (fish, 2600, 5035)
                .Tug       (BiteType.Legendary)
                .Uptime    (480, 1200)
                .Transition(4, 3)
                .Weather   (14)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8769, Patch.DreamsOfIce) // Darkstar
                .Bait      (fish, 2599, 4937)
                .Tug       (BiteType.Legendary)
                .Uptime    (1140, 240)
                .Predators (fish, (5544, 5))
                .Intuition (120)
                .Weather   (16, 15)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8770, Patch.DreamsOfIce) // Blue Corpse
                .Bait      (fish, 2607)
                .Tug       (BiteType.Legendary)
                .Transition(16, 15)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (8771, Patch.DreamsOfIce) // Mahar
                .Bait      (fish, 2605, 5040)
                .Tug       (BiteType.Legendary)
                .Transition(1, 2)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8772, Patch.DreamsOfIce) // Shonisaurus
                .Bait      (fish, 2605, 5040, 8771)
                .Tug       (BiteType.Legendary)
                .Transition(1, 2)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8773, Patch.DreamsOfIce) // Magicked Mushroom
                .Bait      (fish, 2620, 4995)
                .Tug       (BiteType.Legendary)
                .Transition(7, 9)
                .Weather   (4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8774, Patch.DreamsOfIce) // Giant Takitaro
                .Bait      (fish, 2603)
                .Tug       (BiteType.Legendary)
                .Weather   (9, 10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8775, Patch.DreamsOfIce) // Namitaro
                .Bait      (fish, 2624)
                .Tug       (BiteType.Legendary)
                .Predators (fish, (8774, 1))
                .Intuition (60)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8776, Patch.DreamsOfIce) // Blood Red Bonytongue
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Legendary)
                .Uptime    (240, 720)
                .Transition(1, 2)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
