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
            .Time(960, 1440);
        data.Apply(36681, Patch.NewfoundAdventure)  // Earful
            .Bait(data, 36591)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Time(240, 480);
        data.Apply(36682, Patch.NewfoundAdventure)  // Hippo Frog
            .Bait(data, 36591)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 1, 2, 3, 4)
            .Weather(data, 7, 8);
        data.Apply(36683, Patch.NewfoundAdventure) // Rimepike
            .Bait(data, 36588, 36458)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 15);
        data.Apply(36684, Patch.NewfoundAdventure) // Foun Ahlm
            .Bait(data, 36595)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 49)
            .Transition(data, 2, 49)
            .Time(480, 920);
        data.Apply(36685, Patch.NewfoundAdventure) // Forbiddingway
            .Bait(data, 36597)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 149)
            .Transition(data, 2)
            .Time(480, 920);
        data.Apply(36686, Patch.NewfoundAdventure) // Thavnairian Calamari
            .Bait(data, 36593)
            .Bite(HookSet.Powerful, BiteType.Strong);
    }
    // @formatter:on
}
