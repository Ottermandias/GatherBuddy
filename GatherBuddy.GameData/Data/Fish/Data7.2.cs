using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplySeekersOfEternity(this GameData data)
    {
        data.Apply(47988, Patch.SeekersOfEternity) // Cabinkeep Permit
            .Bait(data, 43859)
            .Time(300, 420)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47989, Patch.SeekersOfEternity) // Muttering Matamata
            .Bait(data, 43691)
            .Time(720, 840)
            .Weather(data, 1)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(47990, Patch.SeekersOfEternity) // Riverlong Candiru
            .Bait(data, 43858)
            .Time(0, 240)
            .Transition(data, 2)
            .Weather(data, 3)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47991, Patch.SeekersOfEternity) // Deep Canopy
            .Bait(data, 43858)
            .Time(600, 720)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47992, Patch.SeekersOfEternity) // Awaksbane Apoda
            .Bait(data, 43858)
            .Weather(data, 3, 4)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47993, Patch.SeekersOfEternity) // Ttokatoa
            .Mooch(data, 43751)
            .Time(1200, 1440)
            .Transition(data, 2)
            .Weather(data, 11)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47994, Patch.SeekersOfEternity) // Thunderous Flounder
            .Bait(data, 43858)
            .Weather(data, 7)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47995, Patch.SeekersOfEternity) // Harlequin Queen
            .Bait(data, 43858)
            .Time(960, 1080)
            .Weather(data, 2)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        
        // Pretty sure this is all the fish from Cosmic Exploration. Maybe wanna double check this stuff lol.
        data.Apply(45693, Patch.SeekersOfEternity); //Astacus Lamentorum
        data.Apply(45694, Patch.SeekersOfEternity); //Lunar Tilapia
        data.Apply(45695, Patch.SeekersOfEternity); //Lunar Blue Guppy
        data.Apply(45696, Patch.SeekersOfEternity); //Melancholia
        data.Apply(45697, Patch.SeekersOfEternity); //Hop-shrimp
        data.Apply(45698, Patch.SeekersOfEternity); //Blue Cichlid
        data.Apply(45699, Patch.SeekersOfEternity); //Bluemoon Loach
        data.Apply(45700, Patch.SeekersOfEternity); //Lunar Raiamas
        data.Apply(45701, Patch.SeekersOfEternity); //Cobalt Eel
        data.Apply(45702, Patch.SeekersOfEternity); //Star Pleco
        data.Apply(45703, Patch.SeekersOfEternity); //Lunar Axolotl
        data.Apply(45704, Patch.SeekersOfEternity); //Lunar Catfish
        data.Apply(45705, Patch.SeekersOfEternity); //Astacus Lamentorum
        data.Apply(45706, Patch.SeekersOfEternity); //Lunar Peacock Bass
        data.Apply(45707, Patch.SeekersOfEternity); //Lunar Hemiodus
        data.Apply(45708, Patch.SeekersOfEternity); //Melancholia
        data.Apply(45709, Patch.SeekersOfEternity); //High Noontide Oscar
        data.Apply(45710, Patch.SeekersOfEternity); //Hopping Fish
        data.Apply(45711, Patch.SeekersOfEternity); //Star Pleco
        data.Apply(45712, Patch.SeekersOfEternity); //Lunar Grass Carp
        data.Apply(45713, Patch.SeekersOfEternity); //Lover's Staff
        data.Apply(45714, Patch.SeekersOfEternity); //Nightscale
        data.Apply(45715, Patch.SeekersOfEternity); //Astacus Lamentorum
        data.Apply(45716, Patch.SeekersOfEternity); //Starcrier
        data.Apply(45717, Patch.SeekersOfEternity); //Weepingeye
        data.Apply(45718, Patch.SeekersOfEternity); //Lunar Eel
        data.Apply(45719, Patch.SeekersOfEternity); //Bluemoon Loach
        data.Apply(45720, Patch.SeekersOfEternity); //Lunar Raiamas
        data.Apply(45721, Patch.SeekersOfEternity); //Lunar Salmon
        data.Apply(45722, Patch.SeekersOfEternity); //Lunar Aetherlouse
        data.Apply(45723, Patch.SeekersOfEternity); //Star Pleco
        data.Apply(45724, Patch.SeekersOfEternity); //Pinkmoon Cichlid
        data.Apply(45725, Patch.SeekersOfEternity); //Lunar Sole
        data.Apply(45726, Patch.SeekersOfEternity); //White Sturgeon
        data.Apply(45727, Patch.SeekersOfEternity); //Astacus Lamentorum
        data.Apply(45728, Patch.SeekersOfEternity); //Starcrier
        data.Apply(45729, Patch.SeekersOfEternity); //Weeping Cichlid
        data.Apply(45730, Patch.SeekersOfEternity); //Yellowcheek
        data.Apply(45731, Patch.SeekersOfEternity); //Bluemoon Loach
        data.Apply(45732, Patch.SeekersOfEternity); //Lunar Raiamas
        data.Apply(45733, Patch.SeekersOfEternity); //Hopping Star
        data.Apply(45734, Patch.SeekersOfEternity); //Blue-peppered Platy
        data.Apply(45735, Patch.SeekersOfEternity); //Melancholia
        data.Apply(45736, Patch.SeekersOfEternity); //Hopped-on Leaffish
        data.Apply(45737, Patch.SeekersOfEternity); //Lunar Discus
        data.Apply(45738, Patch.SeekersOfEternity); //Solar Flarefish
        data.Apply(45739, Patch.SeekersOfEternity); //Moonrock Candy
        data.Apply(45740, Patch.SeekersOfEternity); //Moongill
        data.Apply(45741, Patch.SeekersOfEternity); //Crowntail Betta
        data.Apply(45742, Patch.SeekersOfEternity); //Speckled Pike
        data.Apply(45743, Patch.SeekersOfEternity); //Prismatic Fish
        data.Apply(45744, Patch.SeekersOfEternity); //Astacus Lamentorum
        data.Apply(45745, Patch.SeekersOfEternity); //Starcrier
        data.Apply(45746, Patch.SeekersOfEternity); //Rainbow Tear
        data.Apply(45747, Patch.SeekersOfEternity); //Ammoonite
        data.Apply(45748, Patch.SeekersOfEternity); //Lunar Coelacanth
        data.Apply(45749, Patch.SeekersOfEternity); //Moonrock Candy
        data.Apply(45750, Patch.SeekersOfEternity); //Moongill
        data.Apply(45751, Patch.SeekersOfEternity); //Lunar Cabomba
        data.Apply(45752, Patch.SeekersOfEternity); //Opal Eel
        data.Apply(45753, Patch.SeekersOfEternity); //Lunar Pirarucu
        data.Apply(45754, Patch.SeekersOfEternity); //Melancholia
        data.Apply(45755, Patch.SeekersOfEternity); //Mooncrystal Perch
        data.Apply(45756, Patch.SeekersOfEternity); //Lunar Prismfish
        data.Apply(45757, Patch.SeekersOfEternity); //Gold Hopping Fish
        data.Apply(45758, Patch.SeekersOfEternity); //Lunar Bichir
        data.Apply(45759, Patch.SeekersOfEternity); //Lunar Scorpion
        data.Apply(45760, Patch.SeekersOfEternity); //Moonwhip
        data.Apply(45761, Patch.SeekersOfEternity); //Polypus Arsenici
        data.Apply(45762, Patch.SeekersOfEternity); //Eolactoria
        data.Apply(45763, Patch.SeekersOfEternity); //Raw Lunar Tourmaline
        data.Apply(45764, Patch.SeekersOfEternity); //Bluemoon Loach
        data.Apply(45765, Patch.SeekersOfEternity); //Leaping Loach
        data.Apply(45766, Patch.SeekersOfEternity); //Lunar Bronze Pleco
        data.Apply(45767, Patch.SeekersOfEternity); //Starry Stingray
        data.Apply(45768, Patch.SeekersOfEternity); //Lunar Lungfish
        data.Apply(45769, Patch.SeekersOfEternity); //Star Pleco
        data.Apply(45770, Patch.SeekersOfEternity); //Lunar Grass Carp
        data.Apply(45771, Patch.SeekersOfEternity); //Macrobrachium Lunaris
        data.Apply(45772, Patch.SeekersOfEternity); //Ataxite
        data.Apply(45773, Patch.SeekersOfEternity); //Lunar Scorpion
        data.Apply(45774, Patch.SeekersOfEternity); //Moonwhip
        data.Apply(45775, Patch.SeekersOfEternity); //Polypus Arsenici
        data.Apply(45776, Patch.SeekersOfEternity); //Wiwaxia
        data.Apply(45777, Patch.SeekersOfEternity); //Atopodentatus
        data.Apply(45778, Patch.SeekersOfEternity); //Melancholia
        data.Apply(45779, Patch.SeekersOfEternity); //Mooncrystal Perch
        data.Apply(45780, Patch.SeekersOfEternity); //Lunar Prismfish
        data.Apply(45781, Patch.SeekersOfEternity); //Hopping Goby
        data.Apply(45782, Patch.SeekersOfEternity); //Lunar Wolffish
        data.Apply(45783, Patch.SeekersOfEternity); //Platinum Bichir
        data.Apply(45784, Patch.SeekersOfEternity); //Bluemoon Loach
        data.Apply(45785, Patch.SeekersOfEternity); //Lunar Raiamas
        data.Apply(45786, Patch.SeekersOfEternity); //Preceptor Betta
        data.Apply(45787, Patch.SeekersOfEternity); //Lunar Academician
        data.Apply(45788, Patch.SeekersOfEternity); //Skipping Star
        data.Apply(45789, Patch.SeekersOfEternity); //Moonrock Candy
        data.Apply(45790, Patch.SeekersOfEternity); //Moongill
        data.Apply(45791, Patch.SeekersOfEternity); //Darkside Bass
        data.Apply(45792, Patch.SeekersOfEternity); //Lunar Sisterscale
        data.Apply(45793, Patch.SeekersOfEternity); //Grand Crowntail Betta
        data.Apply(45794, Patch.SeekersOfEternity); //Lunar Scorpion
        data.Apply(45795, Patch.SeekersOfEternity); //Moonwhip
        data.Apply(45796, Patch.SeekersOfEternity); //Polypus Arsenici
        data.Apply(45797, Patch.SeekersOfEternity); //Broodingway
        data.Apply(45798, Patch.SeekersOfEternity); //Galactic Haze
        data.Apply(45799, Patch.SeekersOfEternity); //Moonrock Candy
        data.Apply(45800, Patch.SeekersOfEternity); //Moongill
        data.Apply(45801, Patch.SeekersOfEternity); //Darkside Bass
        data.Apply(45802, Patch.SeekersOfEternity); //Lunar Turtle
        data.Apply(45803, Patch.SeekersOfEternity); //Dark Crowntail Betta
        data.Apply(45804, Patch.SeekersOfEternity); //Lunar Scorpion
        data.Apply(45805, Patch.SeekersOfEternity); //Moonwhip
        data.Apply(45806, Patch.SeekersOfEternity); //Lunar Anemone
        data.Apply(45807, Patch.SeekersOfEternity); //Culter Arsenici
        data.Apply(45808, Patch.SeekersOfEternity); //Lamentorum Regotoise
        data.Apply(45809, Patch.SeekersOfEternity); //Polypus Sulfuris
        data.Apply(45810, Patch.SeekersOfEternity); //Bluemoon Loach
        data.Apply(45811, Patch.SeekersOfEternity); //Lunar Raiamas
        data.Apply(45812, Patch.SeekersOfEternity); //Preceptor Betta
        data.Apply(45813, Patch.SeekersOfEternity); //Starcrystal Knife
        data.Apply(45814, Patch.SeekersOfEternity); //Bright Cobalt Eel
        data.Apply(45815, Patch.SeekersOfEternity); //Lunar Scorpion
        data.Apply(45816, Patch.SeekersOfEternity); //Moonwhip
        data.Apply(45817, Patch.SeekersOfEternity); //Polypus Arsenici
        data.Apply(45818, Patch.SeekersOfEternity); //Lunar Tadpole
        data.Apply(45819, Patch.SeekersOfEternity); //Grand Atopodentatus
        data.Apply(45820, Patch.SeekersOfEternity); //Star Pleco
        data.Apply(45821, Patch.SeekersOfEternity); //Lunar Grass Carp
        data.Apply(45822, Patch.SeekersOfEternity); //Moonlit Snakehead
        data.Apply(45823, Patch.SeekersOfEternity); //Southeastern Pike
        data.Apply(45824, Patch.SeekersOfEternity); //Lunarsword Snook
        data.Apply(45825, Patch.SeekersOfEternity); //Lamentorum Garpike
        data.Apply(45826, Patch.SeekersOfEternity); //Astacus Lamentorum
        data.Apply(45827, Patch.SeekersOfEternity); //Starcrier
        data.Apply(45828, Patch.SeekersOfEternity); //Rainbow Tear
        data.Apply(45829, Patch.SeekersOfEternity); //Dark Carp
        data.Apply(45830, Patch.SeekersOfEternity); //Goldcheek
        data.Apply(45831, Patch.SeekersOfEternity); //Ctenophora Lunaris
        data.Apply(45832, Patch.SeekersOfEternity); //Protomyke #721
        data.Apply(45833, Patch.SeekersOfEternity); //Argonauta Lunaris
        data.Apply(45834, Patch.SeekersOfEternity); //Nadirichthys
        data.Apply(45835, Patch.SeekersOfEternity); //Atolla Jellyfish
        data.Apply(45836, Patch.SeekersOfEternity); //Arsenic Axolotl
        data.Apply(45837, Patch.SeekersOfEternity); //Sunny Jellyfish
        data.Apply(45838, Patch.SeekersOfEternity); //Universal Darkfin
        data.Apply(45839, Patch.SeekersOfEternity); //Etheirys Croppie
        data.Apply(45840, Patch.SeekersOfEternity); //Moon Mora
        data.Apply(45841, Patch.SeekersOfEternity); //Star Pleco
        data.Apply(45842, Patch.SeekersOfEternity); //Lunar Grass Carp
        data.Apply(45843, Patch.SeekersOfEternity); //Moonlit Snakehead
        data.Apply(45844, Patch.SeekersOfEternity); //Moonbeam
        data.Apply(45845, Patch.SeekersOfEternity); //Moongate Cod
        data.Apply(45846, Patch.SeekersOfEternity); //Lunar Sturgeon
        data.Apply(45847, Patch.SeekersOfEternity); //Astacus Lamentorum
        data.Apply(45848, Patch.SeekersOfEternity); //Teardrop Knifefish
        data.Apply(45849, Patch.SeekersOfEternity); //Weeping Crab
        data.Apply(45850, Patch.SeekersOfEternity); //Silvermoon Tilapia
        data.Apply(45851, Patch.SeekersOfEternity); //Weeping Minnow
        data.Apply(45853, Patch.SeekersOfEternity); //Melancholia
        data.Apply(45854, Patch.SeekersOfEternity); //Lunar Prismfish
        data.Apply(45855, Patch.SeekersOfEternity); //Hopping Flounder
        data.Apply(45856, Patch.SeekersOfEternity); //Galactic Flarefish
        data.Apply(45857, Patch.SeekersOfEternity); //Mooncrystal Perch
        data.Apply(45858, Patch.SeekersOfEternity); //Cobalt Fish
        data.Apply(45859, Patch.SeekersOfEternity); //Lunar Goldfish
        data.Apply(45860, Patch.SeekersOfEternity); //Lunar Minnow
        data.Apply(45861, Patch.SeekersOfEternity); //Gleamingray
        data.Apply(45862, Patch.SeekersOfEternity); //Lunar Butterfly
        data.Apply(45863, Patch.SeekersOfEternity); //Lunar Seagrapes
        data.Apply(45864, Patch.SeekersOfEternity); //Fishingway
        data.Apply(45865, Patch.SeekersOfEternity); //Lunar Scorpion
        data.Apply(45866, Patch.SeekersOfEternity); //Moonwhip
        data.Apply(45867, Patch.SeekersOfEternity); //Polypus Arsenici
        data.Apply(45868, Patch.SeekersOfEternity); //Arsenic Flower
        data.Apply(45869, Patch.SeekersOfEternity); //Arsenic Salamander
        data.Apply(45870, Patch.SeekersOfEternity); //Eyeballingway
        data.Apply(45871, Patch.SeekersOfEternity); //Moonrock Candy
        data.Apply(45872, Patch.SeekersOfEternity); //Lunar Platyfish
        data.Apply(45873, Patch.SeekersOfEternity); //Crenicichla Lunaris
        data.Apply(45874, Patch.SeekersOfEternity); //Fullmoon Ray
        data.Apply(45875, Patch.SeekersOfEternity); //Lunar Yellowfin
        data.Apply(45876, Patch.SeekersOfEternity); //Wayeater
        data.Apply(45877, Patch.SeekersOfEternity); //Moonrock Candy
        data.Apply(45878, Patch.SeekersOfEternity); //Moongill
        data.Apply(45879, Patch.SeekersOfEternity); //Darkside Bass
        data.Apply(45880, Patch.SeekersOfEternity); //Opal Guppy
        data.Apply(45881, Patch.SeekersOfEternity); //Harbor Fang
        data.Apply(45882, Patch.SeekersOfEternity); //Deepmoon Cabomba
        data.Apply(45883, Patch.SeekersOfEternity); //Arsenic Axolotl
        data.Apply(45884, Patch.SeekersOfEternity); //Sunny Jellyfish
        data.Apply(45885, Patch.SeekersOfEternity); //Universal Darkfin
        data.Apply(45886, Patch.SeekersOfEternity); //Glimmerfish
        data.Apply(45887, Patch.SeekersOfEternity); //Lepopredator
        data.Apply(45888, Patch.SeekersOfEternity); //Soliclymenia
        data.Apply(45889, Patch.SeekersOfEternity); //Ctenophora Lunaris
        data.Apply(45890, Patch.SeekersOfEternity); //Protomyke #721
        data.Apply(45891, Patch.SeekersOfEternity); //Argonauta Lunaris
        data.Apply(45892, Patch.SeekersOfEternity); //Aetherial Sword
        data.Apply(45893, Patch.SeekersOfEternity); //Moon Manta
        data.Apply(45894, Patch.SeekersOfEternity); //Nadir Ambulocetus
        data.Apply(45895, Patch.SeekersOfEternity); //Lunar Scorpion
        data.Apply(45896, Patch.SeekersOfEternity); //Arsenical Proto-hropken
        data.Apply(45897, Patch.SeekersOfEternity); //Lunar Oil Eel
        data.Apply(45898, Patch.SeekersOfEternity); //Galactic Noise
        data.Apply(45899, Patch.SeekersOfEternity); //Onychodictyon
        data.Apply(45900, Patch.SeekersOfEternity); //Eolactoria Arsenici
        data.Apply(45901, Patch.SeekersOfEternity); //Lunar Goldfish
        data.Apply(45902, Patch.SeekersOfEternity); //Lunar Minnow
        data.Apply(45903, Patch.SeekersOfEternity); //Lunar Glassfish
        data.Apply(45904, Patch.SeekersOfEternity); //Gleaming Deathworm
        data.Apply(45905, Patch.SeekersOfEternity); //Leanchoilia
        data.Apply(45906, Patch.SeekersOfEternity); //Harpagofututor
        data.Apply(45907, Patch.SeekersOfEternity); //Ctenophora Lunaris
        data.Apply(45908, Patch.SeekersOfEternity); //Protomyke #721
        data.Apply(45909, Patch.SeekersOfEternity); //Argonauta Lunaris
        data.Apply(45910, Patch.SeekersOfEternity); //Aetherial Sword
        data.Apply(45911, Patch.SeekersOfEternity); //Macropinna
        data.Apply(45912, Patch.SeekersOfEternity); //Deepmoon Seadragon
        data.Apply(45913, Patch.SeekersOfEternity); //Star Pleco
        data.Apply(45914, Patch.SeekersOfEternity); //Lunar Grass Carp
        data.Apply(45915, Patch.SeekersOfEternity); //Lunar Catfish
        data.Apply(45916, Patch.SeekersOfEternity); //Lunar Longhair
        data.Apply(45917, Patch.SeekersOfEternity); //Heavy Ataxite 
        data.Apply(45918, Patch.SeekersOfEternity); //Bluemoon Loach
        data.Apply(45919, Patch.SeekersOfEternity); //Lunar Raiamas
        data.Apply(45920, Patch.SeekersOfEternity); //Preceptor Betta
        data.Apply(45921, Patch.SeekersOfEternity); //Moonlit Carp
        data.Apply(45922, Patch.SeekersOfEternity); //Hopping Lungfish
        data.Apply(45923, Patch.SeekersOfEternity); //Lunar Scorpion
        data.Apply(45924, Patch.SeekersOfEternity); //Moonwhip
        data.Apply(45925, Patch.SeekersOfEternity); //Polypus Arsenici
        data.Apply(45926, Patch.SeekersOfEternity); //Darkside Shrimp
        data.Apply(45927, Patch.SeekersOfEternity); //Stardust Octopus
        data.Apply(45928, Patch.SeekersOfEternity); //Raw Moonbright Tourmaline
        data.Apply(45929, Patch.SeekersOfEternity); //Moonrock Candy
        data.Apply(45930, Patch.SeekersOfEternity); //Moongill
        data.Apply(45931, Patch.SeekersOfEternity); //Waxscale
        data.Apply(45932, Patch.SeekersOfEternity); //Corydoras Lunaris
        data.Apply(45933, Patch.SeekersOfEternity); //Infinity Eel
        data.Apply(45934, Patch.SeekersOfEternity); //Hollow Eel
        data.Apply(45935, Patch.SeekersOfEternity); //Bluemoon Loach
        data.Apply(45936, Patch.SeekersOfEternity); //Lamentorum Geayi
        data.Apply(45937, Patch.SeekersOfEternity); //Moon Bluetail
        data.Apply(45938, Patch.SeekersOfEternity); //Melancholia
        data.Apply(45939, Patch.SeekersOfEternity); //Sunken Drone
        data.Apply(45945, Patch.SeekersOfEternity); //Moonlight Pleco
        data.Apply(45946, Patch.SeekersOfEternity); //Lunastone Pleco
    }
    // @formatter:on
}
