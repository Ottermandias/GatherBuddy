using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyARealmAwoken(this GameData data)
    {
        data.Apply(6185, Patch.ARealmAwoken) // Tiny Tortoise
            .Bait (data, 2628)
            .Bite (HookSet.Powerful, BiteType.Legendary);
        data.Apply(6191, Patch.ARealmAwoken) // Gigantpole
            .Bait (data, 2624)
            .Bite (HookSet.Powerful, BiteType.Legendary);
    }
    // @formatter:on
}
