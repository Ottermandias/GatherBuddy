using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyARealmAwoken(this FishManager fish)
        {
            fish.Apply     (6185, Patch.ARealmAwoken) // Tiny Tortoise
                .Bait      (fish, 2628)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (6191, Patch.ARealmAwoken) // Gigantpole
                .Bait      (fish, 2624)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
