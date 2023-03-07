using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyGodsRevelLandsTremble(this GameData data)
    {
        data.Apply(38810, Patch.GodsRevelLandsTremble) // Ondo Kelp
            .Bait(data, 28634)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(38811, Patch.GodsRevelLandsTremble) // Ken Kiln
            .Spear(SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply(38812, Patch.GodsRevelLandsTremble) // Purpure Cod
            .Bait(data, 28634)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(38813, Patch.GodsRevelLandsTremble) // Glorianda's Tear
            .Spear(SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply(38814, Patch.GodsRevelLandsTremble) // Il Lydha
            .Bait(data, 28634)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(38830, Patch.GodsRevelLandsTremble) // Catastrophizer
            .Bait(data, 36591)
            .Time(480, 840)
            .Transition(data, 2)
            .Weather(data, 1)
            .Bite(HookSet.Precise, BiteType.Legendary);
        data.Apply(38831, Patch.GodsRevelLandsTremble) // Mossgill Salmon
            .Bait(data, 36590)
            .Transition(data, 3)
            .Weather(data, 7)
            .Bite(HookSet.Powerful, BiteType.Legendary);
        data.Apply(38832, Patch.GodsRevelLandsTremble) // Vidyutvat Wrasse
            .Bait(data, 36592)
            .Time(1200, 1440)
            .Transition(data, 3)
            .Weather(data, 1)
            .Bite(HookSet.Powerful, BiteType.Legendary);
        data.Apply(38833, Patch.GodsRevelLandsTremble) // Browned Banana Eel
            .Bait(data, 36591)
            .Time(1320, 120)
            .Weather(data, 7)
            .Bite(HookSet.Powerful, BiteType.Legendary);
        data.Apply(38834, Patch.GodsRevelLandsTremble) // Frozen Regotoise
            .Bait(data, 36595)
            .Time(600, 780)
            .Weather(data, 49)
            .Bite(HookSet.Powerful, BiteType.Legendary);
        data.Apply(38835, Patch.GodsRevelLandsTremble) // Cosmic Haze
            .Bait(data, 36594, 36489)
            .Time(1200, 240)
            .Weather(data, 149)
            .Bite(HookSet.Powerful, BiteType.Legendary);
        data.Apply(38836, Patch.GodsRevelLandsTremble) // Antheian Dahlia
            .Bait(data, 36591)
            .Time(240, 600)
            .Transition(data, 3)
            .Weather(data, 1)
            .Bite(HookSet.Precise, BiteType.Legendary);
        data.Apply(38837, Patch.GodsRevelLandsTremble) // Lakeskipper
            .Spear(SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply(38838, Patch.GodsRevelLandsTremble) // Bronze Eel
            .Spear(SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply(38839, Patch.GodsRevelLandsTremble) // Striped Peacock Bass
            .Spear(SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply(38840, Patch.GodsRevelLandsTremble) // Bronze Trout
            .Spear(SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply(38841, Patch.GodsRevelLandsTremble) // Nosceasaur
            .Spear(SpearfishSize.Large, SpearfishSpeed.Fast)
            .Predators(data, 60, (38939, 4))
            .Comment("Catch 4 Guppies in one node.");
        data.Apply(38935, Patch.GodsRevelLandsTremble) // Jhinga
            .Bait(data, 36593)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(38939, Patch.GodsRevelLandsTremble) // Verdigris Guppy
            .Spear(SpearfishSize.Small, SpearfishSpeed.Fast);
        data.Apply(39240, Patch.GodsRevelLandsTremble) // Phyllinos
            .Bait(data, 36591)
            .Time(300, 480)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(38792, Patch.GodsRevelLandsTremble) // Platinum Seahorse
            .Bait(data, 38808)
            .Bite(HookSet.Precise, BiteType.Weak)
            .ForceBig(false);
        data.Apply(38793, Patch.GodsRevelLandsTremble) // Clavekeeper
            .Bite(HookSet.Powerful, BiteType.Strong)
            .Bait(data, 38808)
            .ForceBig(false);
        data.Apply(38798, Patch.GodsRevelLandsTremble) // Mirror Image
            .Bait(data, 38808)
            .Bite(HookSet.Precise, BiteType.Weak)
            .ForceBig(false);
        data.Apply(38799, Patch.GodsRevelLandsTremble) // Spangled Pirarucu
            .Bait(data, 38808)
            .Bite(HookSet.Powerful, BiteType.Strong)
            .ForceBig(false);
    }
    // @formatter:on
}
