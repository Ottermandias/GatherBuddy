using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyCrossroads(this GameData data)
    {
        data.Apply(44339, Patch.Crossroads); // Icuvlo's Barter
        data.Apply(44340, Patch.Crossroads); // Moongripper
        data.Apply(44341, Patch.Crossroads); // Cazuela Crab
        data.Apply(44342, Patch.Crossroads); // Stardust Sleeper
        data.Apply(44343, Patch.Crossroads); // Ilyon Asoh Cichlid
        data.Apply(44344, Patch.Crossroads); // Pixel Loach
        data.Apply(44345, Patch.Crossroads); // Hwittayoanaan Cichlid
        data.Apply(44346, Patch.Crossroads); // Thunderswift Trout
        data.Apply(44347, Patch.Crossroads); // Cloudsail
    }
    // @formatter:on
}
