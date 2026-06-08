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
            .Mooch(data, 43709)
            .Weather(data, 4)
            .Transition(data, 3)
            .Time(780, 900)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52001, Patch.TrailToTheHeavens) // Excavator Catfish
            .Bait(data, 43855)
            .Weather(data, 3)
            .Transition(data, 7)
            .Time(240, 360)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52002, Patch.TrailToTheHeavens) // Moonmarking Saucer
            .Mooch(data, 43740)
            .Weather(data, 7)
            .Transition(data, 1)
            .Time(960, 1260)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52003, Patch.TrailToTheHeavens) // Autarch's Supper
            .Bait(data, 43858)
            .Weather(data, 4)
            .Transition(data, 7)
            .Time(960, 1080)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52004, Patch.TrailToTheHeavens) // Bitterbark Caiman 
            .Mooch(data, 43743)
            .Weather(data, 1)
            .Transition(data, 4)
            .Time(960, 1080)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
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

        // Custom Delivery Fish
        data.Apply(52012, Patch.TrailToTheHeavens) // Pote'uka
            .Bait(data, 28634)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52013, Patch.TrailToTheHeavens) // Harbor Cloud
            .Bait(data, 28634)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52014, Patch.TrailToTheHeavens) // Iq Ebaji
            .Bait(data, 28634)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52015, Patch.TrailToTheHeavens) // Yowekwa Inkhead
            .Bait(data, 28634)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52016, Patch.TrailToTheHeavens) // Burst Lungfish
            .Bait(data, 28634)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Ocean Fish
        data.Apply(51209, Patch.TrailToTheHeavens) // First Mate's Finger
            .Bait(data, 29714)
            .Points(10)
            .MultiHook (3)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .OceanType(OceanSpecies.Mantis);
        data.Apply(51210, Patch.TrailToTheHeavens) // Specterfish
            .Bait(data, 29715)
            .Points(15)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 1, 2, 3, 4);
        data.Apply(51211, Patch.TrailToTheHeavens) // Rhotano Permit
            .Bait(data, 29716)
            .Points(11)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(51212, Patch.TrailToTheHeavens) // Rhotano Dahlia
            .Bait(data, 29715)
            .Points(35)
            .MultiHook (2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(51213, Patch.TrailToTheHeavens) // Lodesmate's Pen
            .Bait(data, 29715)
            .Points(35)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 1, 2, 3, 4, 7);
        data.Apply(51214, Patch.TrailToTheHeavens) // Rhotano Roosterfish
            .Bait(data, 29716)
            .Points(40)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 1, 2, 7, 8);
        data.Apply(51215, Patch.TrailToTheHeavens) // Rhotanosaurus
            .Bait(data, 29716)
            .Points(59)
            .MultiHook (3)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51216, Patch.TrailToTheHeavens) // Royal Handmaiden
            .Bait(data, 29714)
            .Points(52)
            .MultiHook (2)
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
            .MultiHook (4)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Always)
            .OceanType(OceanSpecies.Mantis);
        data.Apply(51220, Patch.TrailToTheHeavens) // Bright Red Coral
            .Bait(data, 29715)
            .Points(84)
            .MultiHook (2)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Always);
        data.Apply(51221, Patch.TrailToTheHeavens) // Royal Favorite
            .Bait(data, 29714)
            .Points(69)
            .MultiHook (2)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Always);
        data.Apply(51222, Patch.TrailToTheHeavens) // Pirate's Purse
            .Bait(data, 29714)
            .Points(199)
            .MultiHook (2)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Night);
        data.Apply(51223, Patch.TrailToTheHeavens) // Captain's Pen
            .Bait(data, 29715)
            .Points(58)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Always);
        data.Apply(51224, Patch.TrailToTheHeavens) // Tylosaurus
            .Bait(data, 29716)
            .Points(68)
            .MultiHook (3)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Always)
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51225, Patch.TrailToTheHeavens) // Cieldalaes Roosterfish
            .Mooch(data, 51223)
            .Points(100)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Always)
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51226, Patch.TrailToTheHeavens) // Renegade Rhotanosaurus
            .Bait(data, 29716)
            .Points(160)
            .MultiHook (4)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Sunset);
        data.Apply(51227, Patch.TrailToTheHeavens) // Red Boarfish
            .Bait(data, 29715)
            .Points(200)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Day);
        data.Apply(51228, Patch.TrailToTheHeavens) // Akupara
            .Mooch(data, 29715, 51223, 51225)
            .Points(500)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Sunset)
            .Predators(data, 20, (51225, 2))
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51229, Patch.TrailToTheHeavens) // Red Mantis
            .Bait(data, 29714)
            .Points(8)
            .MultiHook (3)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .OceanType(OceanSpecies.Mantis);
        data.Apply(51230, Patch.TrailToTheHeavens) // Datli Gwl
            .Bait(data, 29715)
            .Points(9)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(51231, Patch.TrailToTheHeavens) // Agama's Strand
            .Bait(data, 29714)
            .Points(13)
            .MultiHook (2)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Weather(data, 1, 2, 3, 4);
        data.Apply(51232, Patch.TrailToTheHeavens) // Palaka's Blade
            .Bait(data, 29714)
            .Points(36)
            .MultiHook (2)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Weather(data, 1, 2, 3, 4, 7);
        data.Apply(51233, Patch.TrailToTheHeavens) // Parjanya Wrasse
            .Bait(data, 29715)
            .Points(31)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 1, 2, 7, 8);
        data.Apply(51234, Patch.TrailToTheHeavens) // Simolestes
            .Bait(data, 29716)
            .Points(27)
            .MultiHook (3)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51235, Patch.TrailToTheHeavens) // Thavnasaurus
            .Bait(data, 29716)
            .Points(48)
            .MultiHook (3)
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
            .MultiHook (3)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Always);
        data.Apply(51240, Patch.TrailToTheHeavens) // Guzel Gwl
            .Bait(data, 29715)
            .Points(70)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Always);
        data.Apply(51241, Patch.TrailToTheHeavens) // Darpana
            .Bait(data, 29715)
            .Points(92)
            .MultiHook (2)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Day);
        data.Apply(51242, Patch.TrailToTheHeavens) // Silverdart
            .Bait(data, 29715)
            .Points(164)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Sunset);
        data.Apply(51243, Patch.TrailToTheHeavens) // Satrapsaurus
            .Bait(data, 29716)
            .Points(70)
            .MultiHook (3)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Always);
        data.Apply(51244, Patch.TrailToTheHeavens) // Tiger Mantis
            .Bait(data, 29714)
            .Points(150)
            .MultiHook (4)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Sunset, OceanTime.Night);
        data.Apply(51245, Patch.TrailToTheHeavens) // Mehi-mahi
            .Bait(data, 29716)
            .Points(70)
            .MultiHook (2)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Always);
        data.Apply(51246, Patch.TrailToTheHeavens) // Pliosaurus
            .Bait(data, 29716)
            .Points(204)
            .MultiHook (4)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Day)
            .OceanType(OceanSpecies.Prehistoric);
        data.Apply(51247, Patch.TrailToTheHeavens) // Manasvin
            .Bait(data, 2591)
            .Points(500)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Night)
            .Predators(data, 15, (51243, 3));
        data.Apply(51687, Patch.TrailToTheHeavens) // Junior Jinbei
            .Bait(data)
            .Points(300)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Always);


        // Timberpond  Aquaculture Impact Survey
        data.Apply(52096, Patch.TrailToTheHeavens) // Timberpond Skipper
            .Bait(data, 52236)
            .Mission(data, 1650)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52097, Patch.TrailToTheHeavens) // Highwood Leaffish
            .Bait(data, 52236)
            .Mission(data, 1650)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52098, Patch.TrailToTheHeavens) // Prochilodus Cosmica
            .Bait(data, 52236)
            .Mission(data, 1650)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // New Growth Tapwaters  Timberpond Specimen Survey
        data.Apply(52099, Patch.TrailToTheHeavens) // Fishform: Modulight
            .Bait(data, 52236)
            .Mission(data, 1651)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52100, Patch.TrailToTheHeavens) // New Growth Leaffish
            .Bait(data, 52236)
            .Mission(data, 1651)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52101, Patch.TrailToTheHeavens) // Auxesian Peacock Bass
            .Bait(data, 52236)
            .Mission(data, 1651)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Timberpond  Timberpond Environmental Survey
        data.Apply(52102, Patch.TrailToTheHeavens) // Timberpond Skipper
            .Bait(data, 52237)
            .Mission(data, 1652)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52103, Patch.TrailToTheHeavens) // Highwood Leaffish
            .Bait(data, 52237)
            .Mission(data, 1652)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52104, Patch.TrailToTheHeavens) // Pioneering Pondfish
            .Bait(data, 52237)
            .Mission(data, 1652)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // New Growth Tapwaters  Regular Base Resupply
        data.Apply(52105, Patch.TrailToTheHeavens) // Fishform: Modulight
            .Bait(data, 52237)
            .Mission(data, 1653)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52106, Patch.TrailToTheHeavens) // New Growth Leaffish
            .Bait(data, 52237)
            .Mission(data, 1653)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52107, Patch.TrailToTheHeavens) // Timberlouse
            .Bait(data, 52237)
            .Mission(data, 1653)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Timberpond  Large Aquatic Specimen Distribution Survey
        data.Apply(52108, Patch.TrailToTheHeavens) // Timberpond Skipper
            .Bait(data, 52238)
            .Mission(data, 1654)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52109, Patch.TrailToTheHeavens) // Highwood Leaffish
            .Bait(data, 52238)
            .Mission(data, 1654)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52110, Patch.TrailToTheHeavens) // Fishform: Aquaclear
            .Bait(data, 52238)
            .Mission(data, 1654)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52111, Patch.TrailToTheHeavens) // Cosmic Belonesox
            .Bait(data, 52238)
            .Mission(data, 1654)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // New Growth Tapwaters  Auxesia Bait Suitability Testing
        data.Apply(52112, Patch.TrailToTheHeavens) // Fishform: Modulight
            .Bait(data, 52239)
            .Mission(data, 1655)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52113, Patch.TrailToTheHeavens) // New Growth Carp
            .Bait(data, 52239)
            .Mission(data, 1655)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52114, Patch.TrailToTheHeavens) // Fishform: Alluviscoop
            .Bait(data, 52239)
            .Mission(data, 1655)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52115, Patch.TrailToTheHeavens) // Verdant Dancer
            .Bait(data, 52239)
            .Mission(data, 1655)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Timberpond  Large Aquatic Specimen Collection
        data.Apply(52116, Patch.TrailToTheHeavens) // Timberpond Skipper
            .Bait(data, 52239)
            .Mission(data, 1656)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52117, Patch.TrailToTheHeavens) // Highwood Leaffish
            .Bait(data, 52239)
            .Mission(data, 1656)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52118, Patch.TrailToTheHeavens) // Stargazy Perch
            .Bait(data, 52239)
            .Mission(data, 1656)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52119, Patch.TrailToTheHeavens) // Timberpond Muskellunge
            .Bait(data, 52239)
            .Mission(data, 1656)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // New Growth Tapwaters  Valuable Southern-central Specimens
        data.Apply(52120, Patch.TrailToTheHeavens) // Fishform: Modulight
            .Bait(data, 52238)
            .Mission(data, 1657)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52121, Patch.TrailToTheHeavens) // New Growth Leaffish
            .Bait(data, 52238)
            .Mission(data, 1657)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52122, Patch.TrailToTheHeavens) // Gallivanting Croppie
            .Bait(data, 52238)
            .Mission(data, 1657)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52123, Patch.TrailToTheHeavens) // Nervure Stalk
            .Bait(data, 52238)
            .Mission(data, 1657)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Emerald Springs  Sprouting Towerling Environmental Survey
        data.Apply(52124, Patch.TrailToTheHeavens) // Mossy Globule
            .Bait(data, 52240)
            .Mission(data, 1658)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52125, Patch.TrailToTheHeavens) // Greatfrond Turtle
            .Bait(data, 52240)
            .Mission(data, 1658)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52126, Patch.TrailToTheHeavens) // Towerling Stemling
            .Bait(data, 52240)
            .Mission(data, 1658)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52127, Patch.TrailToTheHeavens) // Crenicichla Cosmica
            .Bait(data, 52240)
            .Mission(data, 1658)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52128, Patch.TrailToTheHeavens) // Timberland Camofin
            .Bait(data, 52240)
            .Mission(data, 1658)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Full Bloom Fountain  Cosmic Tarantula Testing
        data.Apply(52129, Patch.TrailToTheHeavens) // Cosmic Hedgemole Cricket
            .Bait(data, 52241)
            .Mission(data, 1659)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52130, Patch.TrailToTheHeavens) // Fishform: Aquaflow
            .Bait(data, 52241)
            .Mission(data, 1659)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52131, Patch.TrailToTheHeavens) // Fatleaf Frog
            .Bait(data, 52241)
            .Mission(data, 1659)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52132, Patch.TrailToTheHeavens) // Trailing Tendril
            .Bait(data, 52241)
            .Mission(data, 1659)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52133, Patch.TrailToTheHeavens) // Roaming Pipira
            .Bait(data, 52241)
            .Mission(data, 1659)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Emerald Springs  Botanifish Research
        data.Apply(52134, Patch.TrailToTheHeavens) // Mossy Globule
            .Bait(data, 52242)
            .Mission(data, 1660)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52135, Patch.TrailToTheHeavens) // Sproutfin
            .Bait(data, 52242)
            .Mission(data, 1660)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52136, Patch.TrailToTheHeavens) // Radial Cichlid
            .Bait(data, 52242)
            .Mission(data, 1660)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52137, Patch.TrailToTheHeavens) // Pseudohedron
            .Bait(data, 52242)
            .Mission(data, 1660)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52138, Patch.TrailToTheHeavens) // Topiary Gar
            .Bait(data, 52242)
            .Mission(data, 1660)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Full Bloom Fountain  Valuable Ruin Specimens
        data.Apply(52139, Patch.TrailToTheHeavens) // Cosmic Hedgemole Cricket
            .Bait(data, 52240)
            .Mission(data, 1661)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52140, Patch.TrailToTheHeavens) // Flourishing Guppy
            .Bait(data, 52240)
            .Mission(data, 1661)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52141, Patch.TrailToTheHeavens) // Cosmic Piraputanga
            .Bait(data, 52240)
            .Mission(data, 1661)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52142, Patch.TrailToTheHeavens) // Green Guppy
            .Bait(data, 52240)
            .Mission(data, 1661)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52143, Patch.TrailToTheHeavens) // Fishform: Aquacleanse
            .Bait(data, 52240)
            .Mission(data, 1661)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Emerald Springs  Aquatic Flora Distribution Survey
        data.Apply(52144, Patch.TrailToTheHeavens) // Mossy Globule
            .Bait(data, 52241)
            .Mission(data, 1662)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52145, Patch.TrailToTheHeavens) // Greatfrond Turtle
            .Bait(data, 52241)
            .Mission(data, 1662)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52146, Patch.TrailToTheHeavens) // Soil Stirrer
            .Bait(data, 52241)
            .Mission(data, 1662)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52147, Patch.TrailToTheHeavens) // Glade Platter
            .Bait(data, 52241)
            .Mission(data, 1662)
            .MultiHook(3) // Double Hook gives 3; Triple Hook gives 5
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Emerald Springs Emerald Springs Specimen Survey
        data.Apply(52148, Patch.TrailToTheHeavens) // Mossy Globule
            .Bait(data, 52244)
            .Mission(data, 1663)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52149, Patch.TrailToTheHeavens) // Greatfrond Turtle
            .Bait(data, 52244)
            .Mission(data, 1663)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52150, Patch.TrailToTheHeavens) // Cosmocarp
            .Bait(data, 52244)
            .Mission(data, 1663)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52151, Patch.TrailToTheHeavens) // Netroot Tortoise
            .Bait(data, 52244)
            .Mission(data, 1663)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52152, Patch.TrailToTheHeavens) // Arowana Analog
            .Bait(data, 52244)
            .Mission(data, 1663)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52153, Patch.TrailToTheHeavens) // Topiary Megagar
            .Bait(data, 52244)
            .Mission(data, 1663)
            .MultiHook(1) // Double Hook gives 1; Triple Hook gives 1
            .Predators(data, 300, (52152, 5))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Full Bloom Fountain Freshwater Arthrolure Testing
        data.Apply(52154, Patch.TrailToTheHeavens) // Cosmic Hedgemole Cricket
            .Bait(data, 52246)
            .Mission(data, 1664)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52155, Patch.TrailToTheHeavens) // Fishform: Aquaflow
            .Bait(data, 52246)
            .Mission(data, 1664)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52156, Patch.TrailToTheHeavens) // Eriocaulon Auxesicum
            .Bait(data, 52246)
            .Mission(data, 1664)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52157, Patch.TrailToTheHeavens) // Aquagrapes
            .Bait(data, 52246)
            .Mission(data, 1664)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52158, Patch.TrailToTheHeavens) // Cosmoss
            .Bait(data, 52246)
            .Mission(data, 1664)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Western Nervure Aquatic Mechanism Operational Survey
        data.Apply(52159, Patch.TrailToTheHeavens) // Perchsplorer
            .Bait(data, 52246)
            .Mission(data, 1665)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52160, Patch.TrailToTheHeavens) // Nervure Driftwood
            .Bait(data, 52246)
            .Mission(data, 1665)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52161, Patch.TrailToTheHeavens) // Auxesian Marjoram
            .Bait(data, 52246)
            .Mission(data, 1665)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52162, Patch.TrailToTheHeavens) // Fishform: Aquamotion
            .Bait(data, 52246)
            .Mission(data, 1665)
            .MultiHook(3) // Double Hook gives 3; Triple Hook gives 5
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52163, Patch.TrailToTheHeavens) // Nervure Corntail
            .Bait(data, 52246)
            .Mission(data, 1665)
            .MultiHook(3) // Double Hook gives 3; Triple Hook gives 5
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Emerald Springs Emerald Springs Environmental Survey
        data.Apply(52164, Patch.TrailToTheHeavens) // Mossy Globule
            .Bait(data, 52243)
            .Mission(data, 1666)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52165, Patch.TrailToTheHeavens) // Mossy Spongescale
            .Bait(data, 52243)
            .Mission(data, 1666)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52166, Patch.TrailToTheHeavens) // Family Netweaver
            .Bait(data, 52243)
            .Mission(data, 1666)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52167, Patch.TrailToTheHeavens) // Autumn's Garb
            .Bait(data, 52243)
            .Mission(data, 1666)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52168, Patch.TrailToTheHeavens) // Eupseudohedron
            .Bait(data, 52243)
            .Mission(data, 1666)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Full Bloom Fountain Valuable Northern-central Specimens
        data.Apply(52169, Patch.TrailToTheHeavens) // Cosmic Hedgemole Cricket
            .Bait(data, 52244)
            .Mission(data, 1667)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52170, Patch.TrailToTheHeavens) // Fishform: Aquaflow
            .Bait(data, 52244)
            .Mission(data, 1667)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52171, Patch.TrailToTheHeavens) // Stacks Foliafin
            .Bait(data, 52244)
            .Mission(data, 1667)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52172, Patch.TrailToTheHeavens) // Sylvan Strand
            .Mooch(data, 52171)
            .Mission(data, 1667)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52173, Patch.TrailToTheHeavens) // Fishform: Aquapure
            .Mooch(data, 52171)
            .Mission(data, 1667)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Western Nervure Aquatic Mechanism Distribution Survey
        data.Apply(52174, Patch.TrailToTheHeavens) // Perchsplorer
            .Bait(data, 52244)
            .Mission(data, 1668)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52175, Patch.TrailToTheHeavens) // Nervure Driftwood
            .Bait(data, 52244)
            .Mission(data, 1668)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52176, Patch.TrailToTheHeavens) // Chlorotic Smelt
            .Bait(data, 52244)
            .Mission(data, 1668)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52177, Patch.TrailToTheHeavens) // Barkscale
            .Mooch(data, 52176)
            .Mission(data, 1668)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52178, Patch.TrailToTheHeavens) // Nervure Taproot
            .Mooch(data, 52176)
            .Mission(data, 1668)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52179, Patch.TrailToTheHeavens) // Orchidelirium Principal
            .Mooch(data, 52176)
            .Mission(data, 1668)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Full Bloom Fountain EX: Northern-central Specimen Survey
        data.Apply(52180, Patch.TrailToTheHeavens) // Cosmic Hedgemole Cricket
            .Bait(data, 52243)
            .Mission(data, 1669)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52181, Patch.TrailToTheHeavens) // Fishform: Aquaflow
            .Bait(data, 52243)
            .Mission(data, 1669)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52182, Patch.TrailToTheHeavens) // Sylvan Swell
            .Bait(data, 52243)
            .Mission(data, 1669)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52183, Patch.TrailToTheHeavens) // Resinwhisker
            .Bait(data, 52243)
            .Mission(data, 1669)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52184, Patch.TrailToTheHeavens) // Grand Green Guppy
            .Bait(data, 52243)
            .Mission(data, 1669)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Predators(data, 95, (52182, 3))
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Western Nervure EX: Large Aquatic Organism Distribution Survey
        data.Apply(52185, Patch.TrailToTheHeavens) // Perchsplorer
            .Bait(data, 52245)
            .Mission(data, 1670)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52186, Patch.TrailToTheHeavens) // Nervure Driftwood
            .Bait(data, 52245)
            .Mission(data, 1670)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52187, Patch.TrailToTheHeavens) // Fishform: Aquacycle
            .Bait(data, 52245)
            .Mission(data, 1670)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52188, Patch.TrailToTheHeavens) // Ripe Citrusfish
            .Bait(data, 52245)
            .Mission(data, 1670)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52189, Patch.TrailToTheHeavens) // Fishform: Vegiclean
            .Bait(data, 52245)
            .Mission(data, 1670)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52190, Patch.TrailToTheHeavens) // Topiary Carp
            .Bait(data, 52245)
            .Mission(data, 1670)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Sylvan Soaking Pool EX: Large Mutant Distribution Survey // Time-restricted Mission: 04:00-07:59 (Stage 15+)
        data.Apply(52191, Patch.TrailToTheHeavens) // Fishform: Alluvimix
            .Bait(data, 52245)
            .Mission(data, 1671)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52192, Patch.TrailToTheHeavens) // Soaking Betta
            .Bait(data, 52245)
            .Mission(data, 1671)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52193, Patch.TrailToTheHeavens) // Fishform: Alluvimark
            .Bait(data, 52245)
            .Mission(data, 1671)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52194, Patch.TrailToTheHeavens) // Indicoresin Trahira
            .Bait(data, 52245)
            .Mission(data, 1671)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52195, Patch.TrailToTheHeavens) // Turquoise Vine
            .Bait(data, 52245)
            .Mission(data, 1671)
            .Lure(Enums.Lure.Ambitious)
            .MultiHook(3) // Double Hook gives 3; Triple Hook gives 5
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Full Bloom Fountain EX+: Full Bloom Fountain Specimen Survey // Time-restricted Mission: 16:00-19:59
        data.Apply(52196, Patch.TrailToTheHeavens) // Cosmic Hedgemole Cricket
            .Bait(data, 52245)
            .Mission(data, 1672)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52197, Patch.TrailToTheHeavens) // Fishform: Aquaflow
            .Bait(data, 52245)
            .Mission(data, 1672)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52198, Patch.TrailToTheHeavens) // Sylvan Herald
            .Bait(data, 52245)
            .Mission(data, 1672)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52199, Patch.TrailToTheHeavens) // Chlorotic Pleco
            .Bait(data, 52245)
            .Mission(data, 1672)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52200, Patch.TrailToTheHeavens) // Blooming Characin
            .Bait(data, 52245)
            .Mission(data, 1672)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52201, Patch.TrailToTheHeavens) // Topiary Pirarucu
            .Bait(data, 52245)
            .Mission(data, 1672)
            .MultiHook(1) // Double Hook gives 1; Triple Hook gives 1
            .Predators(data, 350, (52200, 1))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Western Nervure EX+: Western Nervure Cultivation Survey // Weather-restricted Mission: Clear Skies
        data.Apply(52202, Patch.TrailToTheHeavens) // Perchsplorer
            .Bait(data, 52243)
            .Mission(data, 1673)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52203, Patch.TrailToTheHeavens) // Nervure Driftwood
            .Bait(data, 52243)
            .Mission(data, 1673)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52204, Patch.TrailToTheHeavens) // Autumn's Wealth
            .Bait(data, 52243)
            .Mission(data, 1673)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52205, Patch.TrailToTheHeavens) // Chlorotic Butterfly
            .Bait(data, 52243)
            .Mission(data, 1673)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52206, Patch.TrailToTheHeavens) // Pileus Corn Dace
            .Bait(data, 52243)
            .Mission(data, 1673)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52207, Patch.TrailToTheHeavens) // Verdant Veil
            .Bait(data, 52243)
            .Mission(data, 1673)
            .MultiHook(5) // Double Hook gives 5; Triple Hook gives 8
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Stratostone Shores EX+: Stratostone Shores Environmental Survey // Weather-restricted Mission: Clouds (Stage 22+)
        data.Apply(52208, Patch.TrailToTheHeavens) // Skylit Petal
            .Bait(data, 52243)
            .Mission(data, 1674)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52209, Patch.TrailToTheHeavens) // Indigo Pulpscale
            .Bait(data, 52243)
            .Mission(data, 1674)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52210, Patch.TrailToTheHeavens) // Sylvan Core
            .Bait(data, 52243)
            .Mission(data, 1674)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52211, Patch.TrailToTheHeavens) // Fishform: Aquameter
            .Bait(data, 52243)
            .Mission(data, 1674)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52212, Patch.TrailToTheHeavens) // Bluebloom
            .Bait(data, 52243)
            .Mission(data, 1674)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52213, Patch.TrailToTheHeavens) // Crabby Indicolite
            .Bait(data, 52243)
            .Mission(data, 1674)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Sprouting Cisterns Master: Botanifish Diversity Survey // Tool Mastery Mission
        data.Apply(52214, Patch.TrailToTheHeavens) // Topiary Gibal
            .Bait(data, 52247)
            .Mission(data, 1675)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52215, Patch.TrailToTheHeavens) // Botanitrout
            .Bait(data, 52247)
            .Mission(data, 1675)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .MultiHook(2); // Double Hook gives 2
        data.Apply(52216, Patch.TrailToTheHeavens) // Peachpetal Shell
            .Bait(data, 52247)
            .Mission(data, 1675)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52217, Patch.TrailToTheHeavens) // Chlorotic Pike
            .Bait(data, 52247)
            .Mission(data, 1675)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52218, Patch.TrailToTheHeavens) // Riotous Dianthus
            .Bait(data, 52247)
            .Mission(data, 1675)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .MultiHook(3); // Double Hook gives 3; Triple Hook gives 5
        data.Apply(52219, Patch.TrailToTheHeavens) // Wisteria Jellyfish
            .Bait(data, 52247)
            .Mission(data, 1675)
            .MultiHook(5) // Double Hook gives 5-7; Triple Hook gives 8-10
            .Predators(data, 300, (52218, 3))
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Sprouting Cisterns Master: Botanifish Ecosystem Survey // Tool Mastery Mission
        data.Apply(52220, Patch.TrailToTheHeavens) // Topiary Gibal
            .Bait(data, 52248)
            .Mission(data, 1676)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52221, Patch.TrailToTheHeavens) // Botanitrout
            .Bait(data, 52251)
            .Mission(data, 1676)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52222, Patch.TrailToTheHeavens) // Cotyledon Grouper
            .Mooch(data, 52221)
            .Mission(data, 1676)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52223, Patch.TrailToTheHeavens) // Greater Petalwing
            .Mooch(data, 52221)
            .Mission(data, 1676)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52224, Patch.TrailToTheHeavens) // Shroud in a Bottle
            .Bait(data, 52248)
            .Mission(data, 1676)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52225, Patch.TrailToTheHeavens) // Topiary Cetacean
            .Mooch(data, 52221)
            .Mission(data, 1676)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Sprouting Cisterns Master: Legendary Botanifish Survey // Tool Mastery Mission
        data.Apply(52226, Patch.TrailToTheHeavens) // Topiary Gibal
            .Bait(data, 52252)
            .Mission(data, 1677)
            .MultiHook(2) // Double Hook gives 2
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52227, Patch.TrailToTheHeavens) // Fishform: Nutrigrowth
            .Bait(data, 52249)
            .Mission(data, 1677)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52228, Patch.TrailToTheHeavens) // Drifting Dragon
            .Bait(data, 52252)
            .Mission(data, 1677)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52229, Patch.TrailToTheHeavens) // Auroral Petalfish
            .Bait(data, 52249)
            .Mission(data, 1677)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(52230, Patch.TrailToTheHeavens) // Fishform: Aqualevel
            .Bait(data, 52252)
            .Mission(data, 1677)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(52231, Patch.TrailToTheHeavens) // Topiary Serpent
            .Bait(data, 52252)
            .Mission(data, 1677)
            .MultiHook(1) // Double Hook gives 1; Triple Hook gives 1
            .Predators(data, 90, (52226, 2), (52228, 1), (52230, 1))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Western Nervure  Luminescent Aquatic Flora // Red Alert Mission: Auroral Flare
        data.Apply(52232, Patch.TrailToTheHeavens) // Perchsplorer
            .Bait(data, 52253)
            .Mission(data, 1698)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52233, Patch.TrailToTheHeavens) // Auxesian Core
            .Bait(data, 52253)
            .Mission(data, 1698)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Full Bloom Fountain  Incense Materials // Red Alert Mission: Floracane 
        data.Apply(52234, Patch.TrailToTheHeavens) // Cosmic Hedgemole Cricket
            .Bait(data, 52253)
            .Mission(data, 1699)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(52235, Patch.TrailToTheHeavens) // Auxesian Cabomba
            .Bait(data, 52253)
            .Mission(data, 1699)
            .MultiHook(2) // Double Hook gives 2; Triple Hook gives 3
            .Bite(data, HookSet.Powerful, BiteType.Strong);

    }
}
