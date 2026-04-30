using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyTrailToTheHeavens(this GameData data)
    {
        data.Apply(51999, Patch.TrailToTheHeavens) // Ole Ole Ole
            .Bait(data, 43854)
            .Weather(data, 15)
            .Transition(data, 3)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52000, Patch.TrailToTheHeavens) // Iron Oxydoras
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(52001, Patch.TrailToTheHeavens) // Excavator Catfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(52002, Patch.TrailToTheHeavens) // Moonmarking Saucer
            .Mooch(data, 43740)
            .Weather(data, 7)
            .Transition(data, 1)
            .Time(960, 1440)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52003, Patch.TrailToTheHeavens) // Autarch's Supper
            .Bait(data, 43858)
            .Weather(data, 4)
            .Transition(data, 7)
            .Time(960, 1080)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(52004, Patch.TrailToTheHeavens) // Bitterbark Caiman 
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(52005, Patch.TrailToTheHeavens) // Vagrant Keeper
            .Bait(data, 43857)
            .Weather(data, 3)
            .Transition(data, 6)
            .Time(0, 480)
            .Predators(data, 300, (43760, 3))
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(52006, Patch.TrailToTheHeavens) // Shined Copper Shark
            .Bait(data, 43859)
            .Weather(data, 4)
            .Transition(data, 3)
            .Time(480, 780)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Ocean Fish
        data.Apply(51209, Patch.TrailToTheHeavens) // First Mate's Finger
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51210, Patch.TrailToTheHeavens) // Specterfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51211, Patch.TrailToTheHeavens) // Rhotano Permit
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51212, Patch.TrailToTheHeavens) // Rhotano Dahlia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51213, Patch.TrailToTheHeavens) // Lodesmate's Pen
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51214, Patch.TrailToTheHeavens) // Rhotano Roosterfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51215, Patch.TrailToTheHeavens) // Rhotanosaurus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51216, Patch.TrailToTheHeavens) // Royal Handmaiden
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51217, Patch.TrailToTheHeavens) // Spectral Starfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51218, Patch.TrailToTheHeavens) // Frolicsome Fish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51219, Patch.TrailToTheHeavens) // Captain's Finger
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51220, Patch.TrailToTheHeavens) // Bright Red Coral
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51221, Patch.TrailToTheHeavens) // Royal Favorite
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51222, Patch.TrailToTheHeavens) // Pirate's Purse
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51223, Patch.TrailToTheHeavens) // Captain's Pen
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51224, Patch.TrailToTheHeavens) // Tylosaurus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51225, Patch.TrailToTheHeavens) // Cieldalaes Roosterfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51226, Patch.TrailToTheHeavens) // Renegade Rhotanosaurus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51227, Patch.TrailToTheHeavens) // Red Boarfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51228, Patch.TrailToTheHeavens) // Akupara
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51229, Patch.TrailToTheHeavens) // Red Mantis
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51230, Patch.TrailToTheHeavens) // Datli Gwl
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51231, Patch.TrailToTheHeavens) // Agama's Strand
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51232, Patch.TrailToTheHeavens) // Palaka's Blade
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51233, Patch.TrailToTheHeavens) // Parjanya Wrasse
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51234, Patch.TrailToTheHeavens) // Simolestes
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51235, Patch.TrailToTheHeavens) // Thavnasaurus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51237, Patch.TrailToTheHeavens) // Spectral Grouper
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51238, Patch.TrailToTheHeavens) // Silkfin
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51239, Patch.TrailToTheHeavens) // Great Red Mantis
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51240, Patch.TrailToTheHeavens) // Guzel Gwl
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51241, Patch.TrailToTheHeavens) // Darpana
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51242, Patch.TrailToTheHeavens) // Silverdart
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51243, Patch.TrailToTheHeavens) // Satrapsaurus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51244, Patch.TrailToTheHeavens) // Tiger Mantis
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51245, Patch.TrailToTheHeavens) // Mehi-mahi
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51246, Patch.TrailToTheHeavens) // Pliosaurus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51247, Patch.TrailToTheHeavens) // Manasvin
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(51687, Patch.TrailToTheHeavens) // Junior Jinbei
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
    }
    // @formatter:on
}
