using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyNewfoundAdventure(this GameData data)
    {
        data.Apply(36679, Patch.NewfoundAdventure); // Aetherolectric Guitarfish
        data.Apply(36680, Patch.NewfoundAdventure); // Jumbo Snook
        data.Apply(36681, Patch.NewfoundAdventure); // Earful
        data.Apply(36682, Patch.NewfoundAdventure); // Hippo Frog
        data.Apply(36683, Patch.NewfoundAdventure); // Rimepike
        data.Apply(36684, Patch.NewfoundAdventure); // Foun Ahlm
        data.Apply(36685, Patch.NewfoundAdventure); // Forbiddingway
        data.Apply(36686, Patch.NewfoundAdventure); // Thavnairian Calamari
    }
    // @formatter:on
}
