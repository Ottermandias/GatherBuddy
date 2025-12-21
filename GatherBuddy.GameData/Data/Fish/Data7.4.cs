using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyIntoTheMist(this GameData data)
    {
        data.Apply(49794, Patch.IntoTheMist) // Purse of Riches
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(49795, Patch.IntoTheMist) // Punutiy Pain
            .Mooch(data, 43701)
            .Time(480, 720)
            .Weather(data, 7)
            .Transition(data, 3)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(49796, Patch.IntoTheMist) // Shuckfin Dace
            .Bait(data, 43858)
            .Time(240, 360)
            .Weather(data, 7)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49797, Patch.IntoTheMist) // Shin Snuffler
            .Bait(data, 43858)
            .Time(0, 120)
            .Weather(data, 4)
            .Predators(data, 300, (43735, 3))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49798, Patch.IntoTheMist) // Moxutural Greatgar
            .Mooch(data, 43736)
            .Time(1200, 1320)
            .Weather(data, 7)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49799, Patch.IntoTheMist) // Heirloom Goldgrouper
            .Bait(data, 43855)
            .Time(480, 960)
            .Weather(data, 2)
            .Transition(data, 4)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49800, Patch.IntoTheMist) // Datnioides Aeroplanos
            .Bait(data, 43858)
            .Time(120, 240)
            .Weather(data, 7)
            .Transition(data, 4)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49801, Patch.IntoTheMist) // Esperance Carp
            .Bait(data)
            .Predators(data, 300, (43796, 3), (43798, 3))
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
    }
    // @formatter:on
}

