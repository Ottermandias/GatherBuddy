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
        
        // Cosmic Exploration
        // Weeping Pool  Lunch Emergency
        data.Apply(45693, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45694, Patch.SeekersOfEternity) // Lunar Tilapia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45695, Patch.SeekersOfEternity) // Lunar Blue Guppy
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Northward Hop-print  Large Aquatic Specimens
        data.Apply(45696, Patch.SeekersOfEternity) // Melancholia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45697, Patch.SeekersOfEternity) // Hop-shrimp
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45698, Patch.SeekersOfEternity) // Blue Cichlid
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Westward Hop-print  Western Water Inspection
        data.Apply(45699, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45700, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45701, Patch.SeekersOfEternity) // Cobalt Eel
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Southeast Well  Aquatic Foodstuffs
        data.Apply(45702, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45703, Patch.SeekersOfEternity) // Lunar Axolotl
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45704, Patch.SeekersOfEternity) // Lunar Catfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Weeping Pool  Weeping Pool Ecological Survey
        data.Apply(45705, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45706, Patch.SeekersOfEternity) // Lunar Peacock Bass
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45707, Patch.SeekersOfEternity) // Lunar Hemiodus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Northward Hop-print  Assorted Alchemical Materials
        data.Apply(45708, Patch.SeekersOfEternity) // Melancholia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45709, Patch.SeekersOfEternity) // High Noontide Oscar
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45710, Patch.SeekersOfEternity) // Hopping Fish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Southeast Well  Big Fish
        data.Apply(45711, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45712, Patch.SeekersOfEternity) // Lunar Grass Carp
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45713, Patch.SeekersOfEternity) // Lover's Staff
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45714, Patch.SeekersOfEternity) // Nightscale
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Weeping Pool  Environmental Inspection
        data.Apply(45715, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45716, Patch.SeekersOfEternity) // Starcrier
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45717, Patch.SeekersOfEternity) // Weepingeye
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45718, Patch.SeekersOfEternity) // Lunar Eel
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Westward Hop-print  Polarized Dyes
        data.Apply(45719, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45720, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45721, Patch.SeekersOfEternity) // Lunar Salmon
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45722, Patch.SeekersOfEternity) // Lunar Aetherlouse
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Southeast Well  Southeast Well Ecological Survey
        data.Apply(45723, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45724, Patch.SeekersOfEternity) // Pinkmoon Cichlid
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45725, Patch.SeekersOfEternity) // Lunar Sole
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45726, Patch.SeekersOfEternity) // White Sturgeon
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Weeping Pool  Fish Sauce Ingredients
        data.Apply(45727, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45728, Patch.SeekersOfEternity) // Starcrier
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45729, Patch.SeekersOfEternity) // Weeping Cichlid
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45730, Patch.SeekersOfEternity) // Yellowcheek
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Westward Hop-print  Aquatic Samples
        data.Apply(45731, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45732, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45733, Patch.SeekersOfEternity) // Hopping Star
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45734, Patch.SeekersOfEternity) // Blue-peppered Platy
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Northward Hop-print  Northwestern Water Inspection
        data.Apply(45735, Patch.SeekersOfEternity) // Melancholia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45736, Patch.SeekersOfEternity) // Hopped-on Leaffish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45737, Patch.SeekersOfEternity) // Lunar Discus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45738, Patch.SeekersOfEternity) // Solar Flarefish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Hollow Harbor  Hollow Harbor Water Inspection
        data.Apply(45739, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45740, Patch.SeekersOfEternity) // Moongill
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45741, Patch.SeekersOfEternity) // Crowntail Betta
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45742, Patch.SeekersOfEternity) // Speckled Pike
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45743, Patch.SeekersOfEternity) // Prismatic Fish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Weeping Pool  Preserved Foodstuffs
        data.Apply(45744, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45745, Patch.SeekersOfEternity) // Starcrier
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45746, Patch.SeekersOfEternity) // Rainbow Tear
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45747, Patch.SeekersOfEternity) // Ammoonite
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45748, Patch.SeekersOfEternity) // Lunar Coelacanth
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Hollow Harbor  Hollow Harbor Ecological Survey
        data.Apply(45749, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45750, Patch.SeekersOfEternity) // Moongill
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45751, Patch.SeekersOfEternity) // Lunar Cabomba
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45752, Patch.SeekersOfEternity) // Opal Eel
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45753, Patch.SeekersOfEternity) // Lunar Pirarucu
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Northward Hop-print  Absolute Specimen
        data.Apply(45754, Patch.SeekersOfEternity) // Melancholia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45755, Patch.SeekersOfEternity) // Mooncrystal Perch
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45756, Patch.SeekersOfEternity) // Lunar Prismfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45757, Patch.SeekersOfEternity) // Gold Hopping Fish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45758, Patch.SeekersOfEternity) // Lunar Bichir
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Palus Arsenici  Aetherochemical Creatures
        data.Apply(45759, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45760, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45761, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45762, Patch.SeekersOfEternity) // Eolactoria
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45763, Patch.SeekersOfEternity) // Raw Lunar Tourmaline
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Westward Hop-print  Westward Ecological Survey
        data.Apply(45764, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45765, Patch.SeekersOfEternity) // Leaping Loach
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45766, Patch.SeekersOfEternity) // Lunar Bronze Pleco
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45767, Patch.SeekersOfEternity) // Starry Stingray
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45768, Patch.SeekersOfEternity) // Lunar Lungfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Southeast Well  Supper Emergency
        data.Apply(45769, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45770, Patch.SeekersOfEternity) // Lunar Grass Carp
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45771, Patch.SeekersOfEternity) // Macrobrachium Lunaris
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45772, Patch.SeekersOfEternity) // Ataxite
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Palus Arsenici  Alchemical Resources
        data.Apply(45773, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45774, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45775, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45776, Patch.SeekersOfEternity) // Wiwaxia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45777, Patch.SeekersOfEternity) // Atopodentatus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Northward Hop-print A-1: Northward Ecological Survey
        data.Apply(45778, Patch.SeekersOfEternity) // Melancholia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45779, Patch.SeekersOfEternity) // Mooncrystal Perch
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45780, Patch.SeekersOfEternity) // Lunar Prismfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45781, Patch.SeekersOfEternity) // Hopping Goby
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45782, Patch.SeekersOfEternity) // Lunar Wolffish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45783, Patch.SeekersOfEternity) // Platinum Bichir
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Westward Hop-print A-1: Rare Aquatic Specimens
        data.Apply(45784, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45785, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45786, Patch.SeekersOfEternity) // Preceptor Betta
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45787, Patch.SeekersOfEternity) // Lunar Academician
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45788, Patch.SeekersOfEternity) // Skipping Star
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Hollow Harbor A-1: Foodstuff Emergency
        data.Apply(45789, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45790, Patch.SeekersOfEternity) // Moongill
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45791, Patch.SeekersOfEternity) // Darkside Bass
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45792, Patch.SeekersOfEternity) // Lunar Sisterscale
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45793, Patch.SeekersOfEternity) // Grand Crowntail Betta
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Palus Arsenici A-1: Precise Water Survey
        data.Apply(45794, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45795, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45796, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45797, Patch.SeekersOfEternity) // Broodingway
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45798, Patch.SeekersOfEternity) // Galactic Haze
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Hollow Harbor A-1: Fine-grade Aquatic Processing Materials
        data.Apply(45799, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45800, Patch.SeekersOfEternity) // Moongill
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45801, Patch.SeekersOfEternity) // Darkside Bass
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45802, Patch.SeekersOfEternity) // Lunar Turtle
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45803, Patch.SeekersOfEternity) // Dark Crowntail Betta
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Weeping Pool A-2: Large Aquatic Specimens
        data.Apply(45826, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45827, Patch.SeekersOfEternity) // Starcrier
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45828, Patch.SeekersOfEternity) // Rainbow Tear
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45829, Patch.SeekersOfEternity) // Dark Carp
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45830, Patch.SeekersOfEternity) // Goldcheek
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Southeast Well A-2: Assorted Alchemical Materials
        data.Apply(45820, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45821, Patch.SeekersOfEternity) // Lunar Grass Carp
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45822, Patch.SeekersOfEternity) // Moonlit Snakehead
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45823, Patch.SeekersOfEternity) // Southeastern Pike
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45824, Patch.SeekersOfEternity) // Lunarsword Snook
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45825, Patch.SeekersOfEternity) // Lamentorum Garpike
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Aetherial Falls A-2: Aetherial Falls Experiment
        data.Apply(45831, Patch.SeekersOfEternity) // Ctenophora Lunaris
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45832, Patch.SeekersOfEternity) // Protomyke #721
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45833, Patch.SeekersOfEternity) // Argonauta Lunaris
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45834, Patch.SeekersOfEternity) // Nadirichthys
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45835, Patch.SeekersOfEternity) // Atolla Jellyfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Glimmerpond Alpha A-2: Foodstuff Emergency
        data.Apply(45836, Patch.SeekersOfEternity) // Arsenic Axolotl
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45837, Patch.SeekersOfEternity) // Sunny Jellyfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45838, Patch.SeekersOfEternity) // Universal Darkfin
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45839, Patch.SeekersOfEternity) // Etheirys Croppie
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45840, Patch.SeekersOfEternity) // Moon Mora
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Palus Arsenici A-3: Palus Arsenici Ecological Survey
        data.Apply(45865, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45866, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45867, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45868, Patch.SeekersOfEternity) // Arsenic Flower
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45869, Patch.SeekersOfEternity) // Arsenic Salamander
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45870, Patch.SeekersOfEternity) // Eyeballingway
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Hollow Harbor A-3: Large Aquatic Specimens
        data.Apply(45871, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45872, Patch.SeekersOfEternity) // Lunar Platyfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45873, Patch.SeekersOfEternity) // Crenicichla Lunaris
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45874, Patch.SeekersOfEternity) // Fullmoon Ray
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45875, Patch.SeekersOfEternity) // Lunar Yellowfin
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45876, Patch.SeekersOfEternity) // Wayeater
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Palus Arsenici A-1: Aquatic Inspection I
        data.Apply(45804, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45805, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45806, Patch.SeekersOfEternity) // Lunar Anemone
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45807, Patch.SeekersOfEternity) // Culter Arsenici
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45808, Patch.SeekersOfEternity) // Lamentorum Regotoise
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45809, Patch.SeekersOfEternity) // Polypus Sulfuris
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Southeast Well A-2: Aquatic Inspection II
        data.Apply(45841, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45842, Patch.SeekersOfEternity) // Lunar Grass Carp
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45843, Patch.SeekersOfEternity) // Moonlit Snakehead
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45844, Patch.SeekersOfEternity) // Moonbeam
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45845, Patch.SeekersOfEternity) // Moongate Cod
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45846, Patch.SeekersOfEternity) // Lunar Sturgeon
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Westward Hop-print A-1: Fine-grade Water Filter Materials I
        data.Apply(45810, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45811, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45812, Patch.SeekersOfEternity) // Preceptor Betta
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45813, Patch.SeekersOfEternity) // Starcrystal Knife
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45814, Patch.SeekersOfEternity) // Bright Cobalt Eel
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Weeping Pool A-2: Fine-grade Water Filter Materials II
        data.Apply(45847, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45848, Patch.SeekersOfEternity) // Teardrop Knifefish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45849, Patch.SeekersOfEternity) // Weeping Crab
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45850, Patch.SeekersOfEternity) // Silvermoon Tilapia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45851, Patch.SeekersOfEternity) // Weeping Minnow
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Hollow Harbor A-3: Fine-grade Water Filter Materials III
        data.Apply(45877, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45878, Patch.SeekersOfEternity) // Moongill
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45879, Patch.SeekersOfEternity) // Darkside Bass
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45880, Patch.SeekersOfEternity) // Opal Guppy
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45881, Patch.SeekersOfEternity) // Harbor Fang
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45882, Patch.SeekersOfEternity) // Deepmoon Cabomba
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Northward Hop-print A-2: Coexisting Species I
        data.Apply(45853, Patch.SeekersOfEternity) // Melancholia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45857, Patch.SeekersOfEternity) // Mooncrystal Perch
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45854, Patch.SeekersOfEternity) // Lunar Prismfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45855, Patch.SeekersOfEternity) // Hopping Flounder
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45858, Patch.SeekersOfEternity) // Cobalt Fish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45856, Patch.SeekersOfEternity) // Galactic Flarefish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Glimmerpond Alpha A-3: Coexisting Species II
        data.Apply(45883, Patch.SeekersOfEternity) // Arsenic Axolotl
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45884, Patch.SeekersOfEternity) // Sunny Jellyfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45885, Patch.SeekersOfEternity) // Universal Darkfin
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45886, Patch.SeekersOfEternity) // Glimmerfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45887, Patch.SeekersOfEternity) // Lepopredator
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45888, Patch.SeekersOfEternity) // Soliclymenia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Palus Arsenici A-1: Aetherochemical Samples I
        data.Apply(45815, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45816, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45817, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45818, Patch.SeekersOfEternity) // Lunar Tadpole
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45819, Patch.SeekersOfEternity) // Grand Atopodentatus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Glimmerpond Beta A-2: Aetherochemical Samples II
        data.Apply(45859, Patch.SeekersOfEternity) // Lunar Goldfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45860, Patch.SeekersOfEternity) // Lunar Minnow
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45861, Patch.SeekersOfEternity) // Gleamingray
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45862, Patch.SeekersOfEternity) // Lunar Butterfly
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45863, Patch.SeekersOfEternity) // Lunar Seagrapes
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45864, Patch.SeekersOfEternity) // Fishingway
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Aetherial Falls A-3: Aetherochemical Samples III
        data.Apply(45889, Patch.SeekersOfEternity) // Ctenophora Lunaris
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45890, Patch.SeekersOfEternity) // Protomyke #721
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45891, Patch.SeekersOfEternity) // Argonauta Lunaris
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45892, Patch.SeekersOfEternity) // Aetherial Sword
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45893, Patch.SeekersOfEternity) // Moon Manta
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45894, Patch.SeekersOfEternity) // Nadir Ambulocetus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Aetherial Falls A-3: Unidentified Aquatic Specimens I
        data.Apply(45907, Patch.SeekersOfEternity) // Ctenophora Lunaris
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45908, Patch.SeekersOfEternity) // Protomyke #721
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45909, Patch.SeekersOfEternity) // Argonauta Lunaris
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45910, Patch.SeekersOfEternity) // Aetherial Sword
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45911, Patch.SeekersOfEternity) // Macropinna
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45912, Patch.SeekersOfEternity) // Deepmoon Seadragon
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Palus Arsenici A-3: Unidentified Aquatic Specimens II
        data.Apply(45895, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45896, Patch.SeekersOfEternity) // Arsenical Proto-hropken
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45897, Patch.SeekersOfEternity) // Lunar Oil Eel
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45898, Patch.SeekersOfEternity) // Galactic Noise
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45899, Patch.SeekersOfEternity) // Onychodictyon
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45900, Patch.SeekersOfEternity) // Eolactoria Arsenici
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Glimmerpond Beta A-3: Unidentified Aquatic Specimens III
        data.Apply(45901, Patch.SeekersOfEternity) // Lunar Goldfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45902, Patch.SeekersOfEternity) // Lunar Minnow
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45903, Patch.SeekersOfEternity) // Lunar Glassfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45904, Patch.SeekersOfEternity) // Gleaming Deathworm
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45905, Patch.SeekersOfEternity) // Leanchoilia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45906, Patch.SeekersOfEternity) // Harpagofututor
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Southeast Well A-2: Processed Aquatic Metals
        data.Apply(45913, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45914, Patch.SeekersOfEternity) // Lunar Grass Carp
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45915, Patch.SeekersOfEternity) // Lunar Catfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45916, Patch.SeekersOfEternity) // Lunar Longhair
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45917, Patch.SeekersOfEternity) // Heavy Ataxite 
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Westward Hop-print A-2: Refined Moon Gel
        data.Apply(45918, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45919, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45920, Patch.SeekersOfEternity) // Preceptor Betta
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45921, Patch.SeekersOfEternity) // Moonlit Carp
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45922, Patch.SeekersOfEternity) // Hopping Lungfish
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Palus Arsenici A-3: Crystallic Gems
        data.Apply(45923, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45924, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45925, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45926, Patch.SeekersOfEternity) // Darkside Shrimp
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45927, Patch.SeekersOfEternity) // Stardust Octopus
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45928, Patch.SeekersOfEternity) // Raw Moonbright Tourmaline
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Hollow Harbor A-3: Eel Rations
        data.Apply(45929, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45930, Patch.SeekersOfEternity) // Moongill
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45931, Patch.SeekersOfEternity) // Waxscale
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45932, Patch.SeekersOfEternity) // Corydoras Lunaris
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45933, Patch.SeekersOfEternity) // Infinity Eel
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45934, Patch.SeekersOfEternity) // Hollow Eel
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Westward Hop-print  Work Ladders
        data.Apply(45935, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45936, Patch.SeekersOfEternity) // Lamentorum Geayi
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45937, Patch.SeekersOfEternity) // Moon Bluetail
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Northward Hop-print  Kindling
        data.Apply(45938, Patch.SeekersOfEternity) // Melancholia
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45939, Patch.SeekersOfEternity) // Sunken Drone
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Weeping Pool  Mutated Fish
        data.Apply(45945, Patch.SeekersOfEternity) // Moonlight Pleco
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(45946, Patch.SeekersOfEternity) // Lunastone Pleco
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
    }
    // @formatter:on
}
