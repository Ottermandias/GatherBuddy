using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplySeekersOfEternity(this GameData data)
    {
        data.Apply(47988, Patch.SeekersOfEternity) // Cabinkeep Permit
            .Bait(data, 43859)
            .Time(240, 480)
            .Transition(data, 1, 2, 4, 7)
            .Weather(data, 1, 2)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47989, Patch.SeekersOfEternity) // Muttering Matamata
            .Bait(data, 43691)
            .Time(480, 960)
            .Transition(data, 1, 2, 3)
            .Weather(data, 1)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(47990, Patch.SeekersOfEternity) // Riverlong Candiru
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(47991, Patch.SeekersOfEternity) // Deep Canopy
            .Bait(data, 43858)
            .Time(600, 720)
            .Weather(data, 1, 2)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47992, Patch.SeekersOfEternity) // Awaksbane Apoda
            .Bait(data, 43858)
            .Transition(data, 1, 2, 3, 7)
            .Weather(data, 3, 4)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47993, Patch.SeekersOfEternity) // Ttokatoa
            .Mooch(data, 43751)
            .Time(960, 1440)
            .Transition(data, 2)
            .Weather(data, 11)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47994, Patch.SeekersOfEternity) // Thunderous Flounder
            .Bait(data, 43858)
            .Transition(data, 3, 4, 50)
            .Weather(data, 7)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47995, Patch.SeekersOfEternity) // Harlequin Queen
            .Bait(data, 43858)
            .Time(960, 1080)
            .Transition(data, 2, 3, 4)
            .Weather(data, 2)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
    }
    // @formatter:on
}
