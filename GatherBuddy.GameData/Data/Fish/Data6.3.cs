using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyGodsRevelLandsTremble(this GameData data)
    {
        data.Apply(38810, Patch.GodsRevelLandsTremble) // Ondo Kelp
            .Bait(data);

        data.Apply(38811, Patch.GodsRevelLandsTremble) // Ken Kiln
            .Spear(SpearfishSize.Unknown, SpearfishSpeed.Unknown);

        data.Apply(38812, Patch.GodsRevelLandsTremble) // Purpure Cod
            .Bait(data);

        data.Apply(38813, Patch.GodsRevelLandsTremble) // Glorianda's Tear
            .Spear(SpearfishSize.Unknown, SpearfishSpeed.Unknown);

        data.Apply(38814, Patch.GodsRevelLandsTremble) // Il Lydha
            .Bait(data);

        data.Apply(38830, Patch.GodsRevelLandsTremble) // Catastrophizer
            .Bait(data);

        data.Apply(38831, Patch.GodsRevelLandsTremble) // Mossgill Salmon
            .Bait(data);

        data.Apply(38832, Patch.GodsRevelLandsTremble) // Vidyutvat Wrasse
            .Bait(data);

        data.Apply(38833, Patch.GodsRevelLandsTremble) // Browned Banana Eel
            .Bait(data);

        data.Apply(38834, Patch.GodsRevelLandsTremble) // Frozen Regotoise
            .Bait(data);

        data.Apply(38835, Patch.GodsRevelLandsTremble) // Cosmic Haze
            .Bait(data);

        data.Apply(38836, Patch.GodsRevelLandsTremble) // Antheian Dahlia
            .Bait(data);

        data.Apply(38837, Patch.GodsRevelLandsTremble) // Lakeskipper
            .Spear(SpearfishSize.Unknown, SpearfishSpeed.Unknown);

        data.Apply(38838, Patch.GodsRevelLandsTremble) // Bronze Eel
            .Spear(SpearfishSize.Unknown, SpearfishSpeed.Unknown);

        data.Apply(38839, Patch.GodsRevelLandsTremble) // Striped Peacock Bass
            .Spear(SpearfishSize.Unknown, SpearfishSpeed.Unknown);

        data.Apply(38840, Patch.GodsRevelLandsTremble) // Bronze Trout
            .Spear(SpearfishSize.Unknown, SpearfishSpeed.Unknown);

        data.Apply(38841, Patch.GodsRevelLandsTremble) // Nosceasaur
            .Spear(SpearfishSize.Unknown, SpearfishSpeed.Unknown);

        data.Apply(38935, Patch.GodsRevelLandsTremble) // Jhinga
            .Bait(data);

        data.Apply(38939, Patch.GodsRevelLandsTremble) // Verdigris Guppy
            .Spear(SpearfishSize.Unknown, SpearfishSpeed.Unknown);

        data.Apply(39240, Patch.GodsRevelLandsTremble) // Phyllinos
            .Bait(data);
    }
    // @formatter:on
}
