using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyIntoTheMist(this GameData data)
    {
        data.Apply(49794, Patch.IntoTheMist) // Purse of Riches
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(49795, Patch.IntoTheMist) // Punutiy Pain
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(49796, Patch.IntoTheMist) // Shuckfin Dace
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(49797, Patch.IntoTheMist) // Shin Snuffler
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(49798, Patch.IntoTheMist) // Moxutural Greatgar
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(49799, Patch.IntoTheMist) // Heirloom Goldgrouper
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(49800, Patch.IntoTheMist) // Datnioides Aeroplanos
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(49801, Patch.IntoTheMist) // Esperance Carp
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(17951, Patch.IntoTheMist) // Dafangshi
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(29573, Patch.IntoTheMist) // Splendid Piranha
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(29574, Patch.IntoTheMist) // Splendid Pirarucu
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(29578, Patch.IntoTheMist) // Splendid Sponge
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(29579, Patch.IntoTheMist) // Splendid Cockle
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(29581, Patch.IntoTheMist) // Splendid Night's Bass
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(29589, Patch.IntoTheMist) // Splendid Mammoth Shellfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(43556, Patch.IntoTheMist) // Timeworn Loboskin Map
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
    }
    // @formatter:on
}
