using GatherBuddy.Enums;
using GatherBuddy.Data;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyDreamsOfIce(this GameData data)
    {
        data.Apply     (8752, Patch.DreamsOfIce) // Imperial Goldfish
            .Bait      (data, 2599, 4978)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 600, (5031, 3));
        data.Apply     (8753, Patch.DreamsOfIce) // The Old Man in the Sea
            .Bait      (data, 2587, 4874, 4888)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 7, 8)
            .Weather   (data, 1);
        data.Apply     (8754, Patch.DreamsOfIce) // Nepto Dragon
            .Bait      (data, 2606)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 350, (4913, 3))
            .Weather   (data, 7, 8);
        data.Apply     (8755, Patch.DreamsOfIce) // Coelacanthus
            .Bait      (data, 2596, 4898)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1320, 180)
            .Weather   (data, 4, 3, 5);
        data.Apply     (8756, Patch.DreamsOfIce) // Endoceras
            .Bait      (data, 2596, 4898)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1200, 240)
            .Transition(data, 1, 2)
            .Weather   (data, 4, 3, 5);
        data.Apply     (8757, Patch.DreamsOfIce) // Seahag
            .Bait      (data, 2585, 4869, 4904)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1140, 120)
            .Transition(data, 1, 2)
            .Weather   (data, 1, 2);
        data.Apply     (8758, Patch.DreamsOfIce) // Ignus Horn
            .Bait      (data, 2603)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Transition(data, 17)
            .Weather   (data, 1, 2);
        data.Apply     (8759, Patch.DreamsOfIce) // Void Bass
            .Bait      (data, 2603)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 1, 2)
            .Weather   (data, 17);
        data.Apply     (8760, Patch.DreamsOfIce) // Cornelia
            .Bait      (data, 2625)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 300, (5008, 5));
        data.Apply     (8761, Patch.DreamsOfIce) // Ninja Betta
            .Bait      (data, 2592, 4942, 5002)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1080, 540)
            .Weather   (data, 17);
        data.Apply     (8762, Patch.DreamsOfIce) // Canavan
            .Bait      (data, 2599, 4978, 5011)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 1080)
            .Weather   (data, 17);
        data.Apply     (8763, Patch.DreamsOfIce) // Kuno the Killer
            .Bait      (data, 2599, 4978, 5002)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 1400, (8762, 1));
        data.Apply     (8764, Patch.DreamsOfIce) // Pirate's Bane
            .Bait      (data, 2585, 4869, 4904)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 120, (4904, 6));
        data.Apply     (8765, Patch.DreamsOfIce) // Ndendecki
            .Bait      (data, 2599, 4978, 5002)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1080, 300)
            .Weather   (data, 4);
        data.Apply     (8766, Patch.DreamsOfIce) // Bat-o'-Nine-Tails
            .Bait      (data, 2599, 4978)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 7)
            .Weather   (data, 4, 3, 1, 2);
        data.Apply     (8767, Patch.DreamsOfIce) // Wootz Knifefish Zenith
            .Bait      (data, 2599, 4978)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (60, 240)
            .Transition(data, 1, 2)
            .Weather   (data, 4);
        data.Apply     (8768, Patch.DreamsOfIce) // Helicoprion
            .Bait      (data, 2600, 5035)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 1200)
            .Transition(data, 4, 3)
            .Weather   (data, 14);
        data.Apply     (8769, Patch.DreamsOfIce) // Darkstar
            .Bait      (data, 2599, 4937)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1140, 240)
            .Predators (data, 120, (5544, 5))
            .Weather   (data, 16, 15);
        data.Apply     (8770, Patch.DreamsOfIce) // Blue Corpse
            .Bait      (data, 2607)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Transition(data, 16, 15)
            .Weather   (data, 1, 2);
        data.Apply     (8771, Patch.DreamsOfIce) // Mahar
            .Bait      (data, 2605, 5040)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 1, 2)
            .Weather   (data, 16);
        data.Apply     (8772, Patch.DreamsOfIce) // Shonisaurus
            .Bait      (data, 2605, 5040, 8771)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (8773, Patch.DreamsOfIce) // Magicked Mushroom
            .Bait      (data, 2620, 4995)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 7, 9)
            .Weather   (data, 4, 3);
        data.Apply     (8774, Patch.DreamsOfIce) // Giant Takitaro
            .Bait      (data, 2603)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 9, 10);
        data.Apply     (8775, Patch.DreamsOfIce) // Namitaro
            .Bait      (data, 2624)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 60, (8774, 1));
        data.Apply     (8776, Patch.DreamsOfIce) // Blood Red Bonytongue
            .Bait      (data, 2599, 4978)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (240, 720)
            .Transition(data, 1, 2)
            .Weather   (data, 4);
    }
    // @formatter:on
}
