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
            .Time(0, 120)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52000, Patch.TrailToTheHeavens) // Iron Oxydoras
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(52001, Patch.TrailToTheHeavens) // Excavator Catfish
            .Bait(data, 43855)
            .Weather(data, 3)
            .Transition(data, 7)
            .Time(240, 480)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
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
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52004, Patch.TrailToTheHeavens) // Bitterbark Caiman 
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(52005, Patch.TrailToTheHeavens) // Vagrant Keeper
            .Bait(data, 43857)
            .Weather(data, 3)
            .Transition(data, 6)
            .Time(360, 480)
            .Predators(data, 300, (43760, 3))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52006, Patch.TrailToTheHeavens) // Shined Copper Shark
            .Bait(data, 43859)
            .Weather(data, 4)
            .Transition(data, 3)
            .Time(480, 780)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Ocean Fish
        data.Apply(51209, Patch.TrailToTheHeavens) // First Mate's Finger
            .Bait(data, 29714)
            .Points(10)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .OceanType(OceanSpecies.Mantis);
        data.Apply(51210, Patch.TrailToTheHeavens) // Specterfish
            .Bait(data, 29715)
            .Points(15)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 1, 2, 3, 4);
        data.Apply(51211, Patch.TrailToTheHeavens) // Rhotano Permit
            .Bait(data, 29716)
            .Points(11)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(51212, Patch.TrailToTheHeavens) // Rhotano Dahlia
            .Bait(data, 29715)
            .Points(35)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(51213, Patch.TrailToTheHeavens) // Lodesmate's Pen
            .Bait(data, 29715)
            .Points(35)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 1, 2, 3, 4, 7);
        data.Apply(51214, Patch.TrailToTheHeavens) // Rhotano Roosterfish
            .Bait(data, 29716)
            .Points(40)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 1, 2, 7, 8);
        data.Apply(51215, Patch.TrailToTheHeavens) // Rhotanosaurus
            .Bait(data, 29716)
            .Points(59)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51216, Patch.TrailToTheHeavens) // Royal Handmaiden
            .Bait(data, 29714)
            .Points(52)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(51217, Patch.TrailToTheHeavens) // Spectral Starfish
            .Bait(data, 29714)
            .Points(100)
            .Bite(data, HookSet.Precise, BiteType.Legendary)
            .Weather(data, 2, 3, 4, 7, 8);
        data.Apply(51218, Patch.TrailToTheHeavens) // Frolicsome Fish
            .Bait(data, 29714)
            .Points(236)
            .Bite(data, HookSet.Precise, BiteType.Legendary)
            .Predators(data, 60, (51212, 3));
        data.Apply(51219, Patch.TrailToTheHeavens) // Captain's Finger
            .Bait(data, 29714)
            .Points(86)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Always)
            .OceanType(OceanSpecies.Mantis);
        data.Apply(51220, Patch.TrailToTheHeavens) // Bright Red Coral
            .Bait(data, 29715)
            .Points(84)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Always);
        data.Apply(51221, Patch.TrailToTheHeavens) // Royal Favorite
            .Bait(data, 29714)
            .Points(69)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Always);
        data.Apply(51222, Patch.TrailToTheHeavens) // Pirate's Purse
            .Bait(data, 29714)
            .Points(199)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Night);
        data.Apply(51223, Patch.TrailToTheHeavens) // Captain's Pen
            .Bait(data, 29715)
            .Points(58)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Always);
        data.Apply(51224, Patch.TrailToTheHeavens) // Tylosaurus
            .Bait(data, 29716)
            .Points(68)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Always)
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51225, Patch.TrailToTheHeavens) // Cieldalaes Roosterfish
            .Mooch(data, 51223)
            .Points(100)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Always)
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51226, Patch.TrailToTheHeavens) // Renegade Rhotanosaurus
            .Bait(data, 29716)
            .Points(160)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Sunset);
        data.Apply(51227, Patch.TrailToTheHeavens) // Red Boarfish
            .Bait(data, 29715)
            .Points(200)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Day);
        data.Apply(51228, Patch.TrailToTheHeavens) // Akupara
            .Mooch(data, 29714, 51223, 51225)
            .Points(500)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Sunset)
            .Predators(data, 20, (51225, 2))
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51229, Patch.TrailToTheHeavens) // Red Mantis
            .Bait(data, 29714)
            .Points(8)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .OceanType(OceanSpecies.Mantis);
        data.Apply(51230, Patch.TrailToTheHeavens) // Datli Gwl
            .Bait(data, 29715)
            .Points(9)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(51231, Patch.TrailToTheHeavens) // Agama's Strand
            .Bait(data, 29714)
            .Points(13)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Weather(data, 1, 2, 3, 4);
        data.Apply(51232, Patch.TrailToTheHeavens) // Palaka's Blade
            .Bait(data, 29714)
            .Points(36)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Weather(data, 1, 2, 3, 4, 7);
        data.Apply(51233, Patch.TrailToTheHeavens) // Parjanya Wrasse
            .Bait(data, 29715)
            .Points(31)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 1, 2, 7, 8);
        data.Apply(51234, Patch.TrailToTheHeavens) // Simolestes
            .Bait(data, 29716)
            .Points(27)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51235, Patch.TrailToTheHeavens) // Thavnasaurus
            .Bait(data, 29716)
            .Points(48)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51237, Patch.TrailToTheHeavens) // Spectral Grouper
            .Bait(data, 29715)
            .Points(100)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 2, 3, 4, 7, 8);
        data.Apply(51238, Patch.TrailToTheHeavens) // Silkfin
            .Bait(data, 29715)
            .Points(210)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Predators(data, 60, (51229, 3));
        data.Apply(51239, Patch.TrailToTheHeavens) // Great Red Mantis
            .Bait(data, 29714)
            .Points(75)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Always);
        data.Apply(51240, Patch.TrailToTheHeavens) // Guzel Gwl
            .Bait(data, 29715)
            .Points(70)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Always);
        data.Apply(51241, Patch.TrailToTheHeavens) // Darpana
            .Bait(data, 29715)
            .Points(92)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Day);
        data.Apply(51242, Patch.TrailToTheHeavens) // Silverdart
            .Bait(data, 29715)
            .Points(164)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Sunset);
        data.Apply(51243, Patch.TrailToTheHeavens) // Satrapsaurus
            .Bait(data, 29716)
            .Points(70)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Always);
        data.Apply(51244, Patch.TrailToTheHeavens) // Tiger Mantis
            .Bait(data, 29714)
            .Points(150)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Sunset, OceanTime.Night);
        data.Apply(51245, Patch.TrailToTheHeavens) // Mehi-mahi
            .Bait(data, 29716)
            .Points(70)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Always);
        data.Apply(51246, Patch.TrailToTheHeavens) // Pliosaurus
            .Bait(data, 29716)
            .Points(204)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Day)
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51247, Patch.TrailToTheHeavens) // Manasvin
            .Bait(data, 2591)
            .Points(500)
            .Bite(data, HookSet.Unknown, BiteType.Legendary)
            .Ocean(OceanTime.Night)
            .Predators(data, 15, (51243, 2));
        data.Apply(51687, Patch.TrailToTheHeavens) // Junior Jinbei
            .Bait(data)
            .Points(300)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Always);
    }
    // @formatter:on
}
