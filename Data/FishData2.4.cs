using GatherBuddy.Classes;
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
                .Bait      (2599, 4978)
                .Tug       (BiteType.Legendary)
                .Predators ((5031, 3))
                .HookType  (HookSet.Powerful);
            fish.Apply     (8753, Patch.DreamsOfIce) // The Old Man in the Sea
                .Bait      (2587, 4874, 4888)
                .Tug       (BiteType.Legendary)
                .Transition(7, 8)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8754, Patch.DreamsOfIce) // Nepto Dragon
                .Bait      (2606)
                .Tug       (BiteType.Legendary)
                .Predators ((4913, 3))
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8755, Patch.DreamsOfIce) // Coelacanthus
                .Bait      (2596, 4898)
                .Tug       (BiteType.Legendary)
                .Uptime    (22, 3)
                .Weather   (4, 3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8756, Patch.DreamsOfIce) // Endoceras
                .Bait      (2596, 4898)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 5)
                .Transition(1, 2)
                .Weather   (4, 3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8757, Patch.DreamsOfIce) // Seahag
                .Bait      (2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .Uptime    (19, 2)
                .Transition(1, 2)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8758, Patch.DreamsOfIce) // Ignus Horn
                .Bait      (2603)
                .Tug       (BiteType.Legendary)
                .Transition(17)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (8759, Patch.DreamsOfIce) // Void Bass
                .Bait      (2603)
                .Tug       (BiteType.Legendary)
                .Transition(1, 2)
                .Weather   (17)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8760, Patch.DreamsOfIce) // Cornelia
                .Bait      (2625)
                .Tug       (BiteType.Legendary)
                .Predators ((5008, 5))
                .HookType  (HookSet.Powerful);
            fish.Apply     (8761, Patch.DreamsOfIce) // Ninja Betta
                .Bait      (2592, 4942, 5002)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 9)
                .Weather   (17)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8762, Patch.DreamsOfIce) // Canavan
                .Bait      (2599, 4978, 5011)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 18)
                .Weather   (17)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8763, Patch.DreamsOfIce) // Kuno the Killer
                .Bait      (2599, 4978, 5002)
                .Tug       (BiteType.Legendary)
                .Predators ((8762, 1))
                .HookType  (HookSet.Powerful);
            fish.Apply     (8764, Patch.DreamsOfIce) // Pirate's Bane
                .Bait      (2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .Predators ((4904, 6))
                .HookType  (HookSet.Powerful);
            fish.Apply     (8765, Patch.DreamsOfIce) // Ndendecki
                .Bait      (2599, 4978, 5002)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 5)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8766, Patch.DreamsOfIce) // Bat-o'-Nine-Tails
                .Bait      (2599, 4978)
                .Tug       (BiteType.Legendary)
                .Transition(7)
                .Weather   (4, 3, 1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8767, Patch.DreamsOfIce) // Wootz Knifefish Zenith
                .Bait      (2599, 4978)
                .Tug       (BiteType.Legendary)
                .Uptime    (1, 4)
                .Transition(1, 2)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8768, Patch.DreamsOfIce) // Helicoprion
                .Bait      (2600, 5035)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 20)
                .Transition(4, 3)
                .Weather   (14)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8769, Patch.DreamsOfIce) // Darkstar
                .Bait      (2599, 4937)
                .Tug       (BiteType.Legendary)
                .Uptime    (19, 4)
                .Predators ((5544, 5))
                .Weather   (16, 15)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8770, Patch.DreamsOfIce) // Blue Corpse
                .Bait      (2607)
                .Tug       (BiteType.Legendary)
                .Transition(16, 15)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (8771, Patch.DreamsOfIce) // Mahar
                .Bait      (2605, 5040)
                .Tug       (BiteType.Legendary)
                .Transition(1, 2)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8772, Patch.DreamsOfIce) // Shonisaurus
                .Bait      (2605, 5040, 8771)
                .Tug       (BiteType.Legendary)
                .Transition(1, 2)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8773, Patch.DreamsOfIce) // Magicked Mushroom
                .Bait      (2620, 4995)
                .Tug       (BiteType.Legendary)
                .Transition(7, 9)
                .Weather   (4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8774, Patch.DreamsOfIce) // Giant Takitaro
                .Bait      (2603)
                .Tug       (BiteType.Legendary)
                .Weather   (9, 10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (8775, Patch.DreamsOfIce) // Namitaro
                .Bait      (2624)
                .Tug       (BiteType.Legendary)
                .Predators ((8774, 1))
                .HookType  (HookSet.Powerful);
            fish.Apply     (8776, Patch.DreamsOfIce) // Blood Red Bonytongue
                .Bait      (2599, 4978)
                .Tug       (BiteType.Legendary)
                .Uptime    (4, 12)
                .Transition(1, 2)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
