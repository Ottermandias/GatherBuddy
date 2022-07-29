using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyNewfoundAdventure(this GameData data)
    {
        data.Apply(36679, Patch.NewfoundAdventure) // Aetherolectric Guitarfish
            .Bait(data, 36593)
            .Time(1200, 1440)
            .Bite(HookSet.Powerful, BiteType.Legendary);
        data.Apply(36680, Patch.NewfoundAdventure) // Jumbo Snook
            .Bait(data, 36591)
            .Weather(data, 3)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Time(1080, 1320);
        data.Apply(36681, Patch.NewfoundAdventure)  // Earful
            .Bait(data, 36591)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Time(240, 480);
        data.Apply(36682, Patch.NewfoundAdventure)  // Hippo Frog
            .Bait(data, 36591)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 8);
        data.Apply(36683, Patch.NewfoundAdventure) // Rimepike
            .Bait(data, 36589, 36458)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 15);
        data.Apply(36684, Patch.NewfoundAdventure) // Foun Ahlm
            .Bait(data, 36595)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 49)
            .Time(600, 840);
        data.Apply(36685, Patch.NewfoundAdventure) // Forbiddingway
            .Bait(data, 36597)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 149)
            .Time(480, 720);
        data.Apply(36686, Patch.NewfoundAdventure) // Thavnairian Calamari
            .Bait(data, 36593)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(36659, Patch.NewfoundAdventure) // Inksquid
            .Spear(SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply(36660, Patch.NewfoundAdventure) // Seedtoad
            .Bait(data, 28634)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(36661, Patch.NewfoundAdventure) // Auroral Clam
            .Spear(SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply(36662, Patch.NewfoundAdventure) // Holier-than Mogpom
            .Bait(data, 28634)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(36663, Patch.NewfoundAdventure) // Chalky Coral
            .Bait(data, 28634)
            .Bite(HookSet.Precise, BiteType.Weak);
    }
    // @formatter:on
}
