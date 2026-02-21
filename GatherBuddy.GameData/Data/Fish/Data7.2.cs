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
            .Mooch(data, 43691)
            .Time(720, 840)
            .Weather(data, 1)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
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
            .Bait(data, 45947)
            .Mission(data, 451)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45694, Patch.SeekersOfEternity) // Lunar Tilapia
            .Bait(data, 45947)
            .Mission(data, 451)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45695, Patch.SeekersOfEternity) // Lunar Blue Guppy
            .Bait(data, 45947)
            .Mission(data, 451)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Northward Hop-print  Large Aquatic Specimens
        data.Apply(45696, Patch.SeekersOfEternity) // Melancholia
            .Bait(data, 45947)
            .Mission(data, 452)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45697, Patch.SeekersOfEternity) // Hop-shrimp
            .Bait(data, 45947)
            .Mission(data, 452)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45698, Patch.SeekersOfEternity) // Blue Cichlid
            .Bait(data, 45947)
            .Mission(data, 452)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Westward Hop-print  Western Water Inspection
        data.Apply(45699, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data, 45947)
            .Mission(data, 453)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45700, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data, 45947)
            .Mission(data, 453)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45701, Patch.SeekersOfEternity) // Cobalt Eel
            .Bait(data, 45947)
            .Mission(data, 453)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Southeast Well  Aquatic Foodstuffs
        data.Apply(45702, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data, 45947)
            .Mission(data, 454)
            .Points(50)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45703, Patch.SeekersOfEternity) // Lunar Axolotl
            .Bait(data, 45947)
            .Mission(data, 454)
            .Points(150)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45704, Patch.SeekersOfEternity) // Lunar Catfish
            .Bait(data, 45947)
            .Mission(data, 454)
            .Points(300)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Weeping Pool  Weeping Pool Ecological Survey
        data.Apply(45705, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data, 45948)
            .Mission(data, 455)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45706, Patch.SeekersOfEternity) // Lunar Peacock Bass
            .Bait(data, 45948)
            .Mission(data, 455)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45707, Patch.SeekersOfEternity) // Lunar Hemiodus
            .Bait(data, 45948)
            .Mission(data, 455)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Northward Hop-print  Assorted Alchemical Materials
        data.Apply(45708, Patch.SeekersOfEternity) // Melancholia
            .Bait(data, 45948)
            .Mission(data, 456)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45709, Patch.SeekersOfEternity) // High Noontide Oscar
            .Bait(data, 45948)
            .Mission(data, 456)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45710, Patch.SeekersOfEternity) // Hopping Fish
            .Bait(data, 45948)
            .Mission(data, 456)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Southeast Well  Big Fish
        data.Apply(45711, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data, 45949)
            .Mission(data, 457)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45712, Patch.SeekersOfEternity) // Lunar Grass Carp
            .Bait(data, 45949)
            .Mission(data, 457)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45713, Patch.SeekersOfEternity) // Lover's Staff
            .Bait(data, 45949)
            .Mission(data, 457)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45714, Patch.SeekersOfEternity) // Nightscale
            .Bait(data, 45949)
            .Mission(data, 457)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Weeping Pool  Environmental Inspection
        data.Apply(45715, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data, 45949)
            .Mission(data, 458)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45716, Patch.SeekersOfEternity) // Starcrier
            .Bait(data, 45949)
            .Mission(data, 458)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45717, Patch.SeekersOfEternity) // Weepingeye
            .Bait(data, 45949)
            .Mission(data, 458)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45718, Patch.SeekersOfEternity) // Lunar Eel
            .Bait(data, 45949)
            .Mission(data, 458)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Westward Hop-print  Polarized Dyes
        data.Apply(45719, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data, 45949)
            .Mission(data, 459)
            .Points(100)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45720, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data, 45949)
            .Mission(data, 459)
            .Points(100)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45721, Patch.SeekersOfEternity) // Lunar Salmon
            .Bait(data, 45949)
            .Mission(data, 459)
            .Points(200)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45722, Patch.SeekersOfEternity) // Lunar Aetherlouse
            .Bait(data, 45949)
            .Mission(data, 459)
            .Points(150)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Southeast Well  Southeast Well Ecological Survey
        data.Apply(45723, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data, 45949)
            .Mission(data, 460)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45724, Patch.SeekersOfEternity) // Pinkmoon Cichlid
            .Bait(data, 45949)
            .Mission(data, 460)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45725, Patch.SeekersOfEternity) // Lunar Sole
            .Bait(data, 45949)
            .Mission(data, 460)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45726, Patch.SeekersOfEternity) // White Sturgeon
            .Bait(data, 45949)
            .Mission(data, 460)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Weeping Pool  Fish Sauce Ingredients
        data.Apply(45727, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data, 45950)
            .Mission(data, 461)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45728, Patch.SeekersOfEternity) // Starcrier
            .Bait(data, 45950)
            .Mission(data, 461)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45729, Patch.SeekersOfEternity) // Weeping Cichlid
            .Bait(data, 45950)
            .Mission(data, 461)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45730, Patch.SeekersOfEternity) // Yellowcheek
            .Bait(data, 45950)
            .Mission(data, 461)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Westward Hop-print  Aquatic Samples
        data.Apply(45731, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data, 45949)
            .Mission(data, 462)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45732, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data, 45949)
            .Mission(data, 462)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45733, Patch.SeekersOfEternity) // Hopping Star
            .Bait(data, 45949)
            .Mission(data, 462)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45734, Patch.SeekersOfEternity) // Blue-peppered Platy
            .Bait(data, 45949)
            .Mission(data, 462)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Northward Hop-print  Northwestern Water Inspection
        data.Apply(45735, Patch.SeekersOfEternity) // Melancholia
            .Bait(data)
            .Mission(data, 463)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45736, Patch.SeekersOfEternity) // Hopped-on Leaffish
            .Bait(data)
            .Mission(data, 463)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45737, Patch.SeekersOfEternity) // Lunar Discus
            .Bait(data, 45949)
            .Mission(data, 463)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45738, Patch.SeekersOfEternity) // Solar Flarefish
            .Bait(data, 45950)
            .Mission(data, 463)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Hollow Harbor  Hollow Harbor Water Inspection
        data.Apply(45739, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data, 45952)
            .Mission(data, 464)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45740, Patch.SeekersOfEternity) // Moongill
            .Bait(data, 45952)
            .Mission(data, 464)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45741, Patch.SeekersOfEternity) // Crowntail Betta
            .Bait(data, 45952)
            .Mission(data, 464)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45742, Patch.SeekersOfEternity) // Speckled Pike
            .Bait(data, 45952)
            .Mission(data, 464)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45743, Patch.SeekersOfEternity) // Prismatic Fish
            .Bait(data, 45952)
            .MultiHook(2)
            .Mission(data, 464)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Weeping Pool  Preserved Foodstuffs
        data.Apply(45744, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data, 45952)
            .Mission(data, 465)
            .Points(50)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45745, Patch.SeekersOfEternity) // Starcrier
            .Bait(data, 45952)
            .Mission(data, 465)
            .Points(100)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45746, Patch.SeekersOfEternity) // Rainbow Tear
            .Bait(data, 45952)
            .Mission(data, 465)
            .Points(150)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45747, Patch.SeekersOfEternity) // Ammoonite
            .Bait(data, 45952)
            .Mission(data, 465)
            .Points(200)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45748, Patch.SeekersOfEternity) // Lunar Coelacanth
            .Bait(data, 45952)
            .Mission(data, 465)
            .Points(500)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Hollow Harbor  Hollow Harbor Ecological Survey
        data.Apply(45749, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data)
            .Mission(data, 466)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45750, Patch.SeekersOfEternity) // Moongill
            .Bait(data, 45955)
            .Mission(data, 466)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45751, Patch.SeekersOfEternity) // Lunar Cabomba
            .Bait(data, 45955)
            .Mission(data, 466)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45752, Patch.SeekersOfEternity) // Opal Eel
            .Bait(data, 45954)
            .Mission(data, 466)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45753, Patch.SeekersOfEternity) // Lunar Pirarucu
            .Bait(data, 45954)
            .Mission(data, 466)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Northward Hop-print  Absolute Specimen
        data.Apply(45754, Patch.SeekersOfEternity) // Melancholia
            .Bait(data, 45953)
            .Mission(data, 467)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45755, Patch.SeekersOfEternity) // Mooncrystal Perch
            .Bait(data, 45953)
            .Mission(data, 467)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45756, Patch.SeekersOfEternity) // Lunar Prismfish
            .Bait(data, 45953)
            .Mission(data, 467)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45757, Patch.SeekersOfEternity) // Gold Hopping Fish
            .Bait(data, 45953)
            .Mission(data, 467)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45758, Patch.SeekersOfEternity) // Lunar Bichir
            .Bait(data, 45953)
            .Mission(data, 467)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Palus Arsenici  Aetherochemical Creatures
        data.Apply(45759, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data, 45957)
            .Mission(data, 468)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45760, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data, 45957)
            .Mission(data, 468)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45761, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data, 45957)
            .Mission(data, 468)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45762, Patch.SeekersOfEternity) // Eolactoria
            .Bait(data, 45957)
            .Mission(data, 468)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45763, Patch.SeekersOfEternity) // Raw Lunar Tourmaline
            .Bait(data, 45957)
            .Mission(data, 468)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Westward Hop-print  Westward Ecological Survey
        data.Apply(45764, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data)
            .Mission(data, 469)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45765, Patch.SeekersOfEternity) // Leaping Loach
            .Bait(data, 45952)
            .Mission(data, 469)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45766, Patch.SeekersOfEternity) // Lunar Bronze Pleco
            .Bait(data, 45953)
            .Mission(data, 469)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45767, Patch.SeekersOfEternity) // Starry Stingray
            .Bait(data, 45952)
            .Mission(data, 469)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45768, Patch.SeekersOfEternity) // Lunar Lungfish
            .Bait(data, 45953)
            .Mission(data, 469)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Southeast Well  Supper Emergency
        data.Apply(45769, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data, 45955)
            .Mission(data, 470)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45770, Patch.SeekersOfEternity) // Lunar Grass Carp
            .Bait(data, 45955)
            .Mission(data, 470)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45771, Patch.SeekersOfEternity) // Macrobrachium Lunaris
            .Bait(data, 45955)
            .MultiHook(2)
            .Mission(data, 470)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45772, Patch.SeekersOfEternity) // Ataxite
            .Bait(data, 45955)
            .MultiHook(4)
            .Mission(data, 470)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Palus Arsenici  Alchemical Resources
        data.Apply(45773, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data, 45956)
            .Mission(data, 471)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45774, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data, 45956)
            .Mission(data, 471)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45775, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data, 45956)
            .Mission(data, 471)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45776, Patch.SeekersOfEternity) // Wiwaxia
            .Bait(data, 45956)
            .Mission(data, 471)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45777, Patch.SeekersOfEternity) // Atopodentatus
            .Bait(data, 45956)
            .Mission(data, 471)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Northward Hop-print A-1: Northward Ecological Survey
        data.Apply(45778, Patch.SeekersOfEternity) // Melancholia
            .Bait(data, 45959)
            .Mission(data, 472)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45779, Patch.SeekersOfEternity) // Mooncrystal Perch
            .Bait(data, 45959)
            .Mission(data, 472)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45780, Patch.SeekersOfEternity) // Lunar Prismfish
            .Bait(data, 45959)
            .Mission(data, 472)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45781, Patch.SeekersOfEternity) // Hopping Goby
            .Bait(data, 45959)
            .Mission(data, 472)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45782, Patch.SeekersOfEternity) // Lunar Wolffish
            .Bait(data, 45959)
            .MultiHook(2)
            .Mission(data, 472)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(45783, Patch.SeekersOfEternity) // Platinum Bichir
            .Bait(data, 45959)
            .Predators(data, 180, (45782, 3))
            .Mission(data, 472)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Westward Hop-print A-1: Rare Aquatic Specimens
        data.Apply(45784, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data, 45958)
            .Mission(data, 473)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45785, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data, 45958)
            .Mission(data, 473)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45786, Patch.SeekersOfEternity) // Preceptor Betta
            .Bait(data, 45958)
            .Mission(data, 473)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45787, Patch.SeekersOfEternity) // Lunar Academician
            .Bait(data, 45958)
            .Mission(data, 473)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45788, Patch.SeekersOfEternity) // Skipping Star
            .Bait(data, 45958)
            .Mission(data, 473)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Hollow Harbor A-1: Foodstuff Emergency
        data.Apply(45789, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data, 45958)
            .Mission(data, 474)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45790, Patch.SeekersOfEternity) // Moongill
            .Bait(data, 45958)
            .Mission(data, 474)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45791, Patch.SeekersOfEternity) // Darkside Bass
            .Bait(data, 45958)
            .Mission(data, 474)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45792, Patch.SeekersOfEternity) // Lunar Sisterscale
            .Bait(data, 45958)
            .MultiHook(2)
            .Mission(data, 474)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45793, Patch.SeekersOfEternity) // Grand Crowntail Betta
            .Bait(data, 45958)
            .MultiHook(4)
            .Mission(data, 474)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Palus Arsenici A-1: Precise Water Survey
        data.Apply(45794, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data, 45963)
            .Mission(data, 475)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45795, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data, 45963)
            .Mission(data, 475)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45796, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data, 45963)
            .Mission(data, 475)
            .Points(150)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45797, Patch.SeekersOfEternity) // Broodingway
            .Bait(data, 45963)
            .Mission(data, 475)
            .Points(400)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45798, Patch.SeekersOfEternity) // Galactic Haze
            .Bait(data, 45963)
            .Mission(data, 475)
            .Points(600)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Hollow Harbor A-1: Fine-grade Aquatic Processing Materials
        data.Apply(45799, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data, 45960)
            .Mission(data, 476)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45800, Patch.SeekersOfEternity) // Moongill
            .Bait(data, 45960)
            .Mission(data, 476)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45801, Patch.SeekersOfEternity) // Darkside Bass
            .Bait(data, 45960)
            .Mission(data, 476)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45802, Patch.SeekersOfEternity) // Lunar Turtle
            .Bait(data, 45960)
            .Mission(data, 476)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45803, Patch.SeekersOfEternity) // Dark Crowntail Betta
            .Bait(data, 45960)
            .Lure(Enums.Lure.Modest)
            .Mission(data, 476)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Weeping Pool A-2: Large Aquatic Specimens
        data.Apply(45826, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data, 45959)
            .Mission(data, 477)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45827, Patch.SeekersOfEternity) // Starcrier
            .Bait(data, 45959)
            .Mission(data, 477)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45828, Patch.SeekersOfEternity) // Rainbow Tear
            .Bait(data, 45959)
            .Mission(data, 477)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45829, Patch.SeekersOfEternity) // Dark Carp
            .Bait(data, 45959)
            .Mission(data, 477)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45830, Patch.SeekersOfEternity) // Goldcheek
            .Bait(data, 45959)
            .Lure(Enums.Lure.Ambitious)
            .Mission(data, 477)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Southeast Well A-2: Assorted Alchemical Materials
        data.Apply(45820, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data, 45958)
            .Mission(data, 478)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45821, Patch.SeekersOfEternity) // Lunar Grass Carp
            .Bait(data, 45958)
            .Mission(data, 478)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45822, Patch.SeekersOfEternity) // Moonlit Snakehead
            .Bait(data, 45958)
            .Mission(data, 478)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45823, Patch.SeekersOfEternity) // Southeastern Pike
            .Bait(data, 45958)
            .Mission(data, 478)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45824, Patch.SeekersOfEternity) // Lunarsword Snook
            .Bait(data, 45958)
            .Mission(data, 478)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45825, Patch.SeekersOfEternity) // Lamentorum Garpike
            .Bait(data, 45958)
            .Mission(data, 478)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Aetherial Falls A-2: Aetherial Falls Experiment
        data.Apply(45831, Patch.SeekersOfEternity) // Ctenophora Lunaris
            .Bait(data)
            .Mission(data, 479)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45832, Patch.SeekersOfEternity) // Protomyke #721
            .Bait(data)
            .Mission(data, 479)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45833, Patch.SeekersOfEternity) // Argonauta Lunaris
            .Bait(data)
            .Mission(data, 479)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45834, Patch.SeekersOfEternity) // Nadirichthys
            .Bait(data)
            .Mission(data, 479)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45835, Patch.SeekersOfEternity) // Atolla Jellyfish
            .Bait(data)
            .Mission(data, 479)
            .Bite(data, HookSet.Unknown, BiteType.Strong);

        // Glimmerpond Alpha A-2: Foodstuff Emergency
        data.Apply(45836, Patch.SeekersOfEternity) // Arsenic Axolotl
            .Bait(data, 45962)
            .Mission(data, 480)
            .Weather(data, 148)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45837, Patch.SeekersOfEternity) // Sunny Jellyfish
            .Bait(data, 45962)
            .Mission(data, 480)
            .Weather(data, 148)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45838, Patch.SeekersOfEternity) // Universal Darkfin
            .Bait(data, 45962)
            .Mission(data, 480)
            .Weather(data, 148)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45839, Patch.SeekersOfEternity) // Etheirys Croppie
            .Bait(data, 45962)
            .Mission(data, 480)
            .MultiHook(2)
            .Weather(data, 148)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45840, Patch.SeekersOfEternity) // Moon Mora
            .Bait(data, 45962)
            .Mission(data, 480)
            .MultiHook(4)
            .Weather(data, 148)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Palus Arsenici A-3: Palus Arsenici Ecological Survey
        data.Apply(45865, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data, 45964)
            .Weather(data, 49)
            .Mission(data, 481)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45866, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data, 45964)
            .Weather(data, 49)
            .Mission(data, 481)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45867, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data, 45964)
            .Weather(data, 49)
            .Mission(data, 481)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45868, Patch.SeekersOfEternity) // Arsenic Flower
            .Bait(data, 45964)
            .Weather(data, 49)
            .Mission(data, 481)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45869, Patch.SeekersOfEternity) // Arsenic Salamander
            .Bait(data, 45964)
            .Weather(data, 49)
            .Mission(data, 481)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(45870, Patch.SeekersOfEternity) // Eyeballingway
            .Bait(data, 45964)
            .Weather(data, 49)
            .Mission(data, 481)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Hollow Harbor A-3: Large Aquatic Specimens
        data.Apply(45871, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data, 45958)
            .Weather(data, 148)
            .Mission(data, 482)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45872, Patch.SeekersOfEternity) // Lunar Platyfish
            .Bait(data, 45958)
            .Weather(data, 148)
            .Mission(data, 482)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45873, Patch.SeekersOfEternity) // Crenicichla Lunaris
            .Bait(data, 45958)
            .Weather(data, 148)
            .Mission(data, 482)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45874, Patch.SeekersOfEternity) // Fullmoon Ray
            .Mooch(data, 45873)
            .Weather(data, 148)
            .Mission(data, 482)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45875, Patch.SeekersOfEternity) // Lunar Yellowfin
            .Mooch(data, 45873)
            .Weather(data, 148)
            .Mission(data, 482)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45876, Patch.SeekersOfEternity) // Wayeater
            .Mooch(data, 45873)
            .Weather(data, 148)
            .Mission(data, 482)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Palus Arsenici A-1: Aquatic Inspection I
        data.Apply(45804, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data)
            .Mission(data, 483)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45805, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data)
            .Mission(data, 483)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45806, Patch.SeekersOfEternity) // Lunar Anemone
            .Bait(data, 45964)
            .Mission(data, 483)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45807, Patch.SeekersOfEternity) // Culter Arsenici
            .Bait(data, 45962)
            .Mission(data, 483)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45808, Patch.SeekersOfEternity) // Lamentorum Regotoise
            .Bait(data, 45962)
            .Mission(data, 483)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45809, Patch.SeekersOfEternity) // Polypus Sulfuris
            .Bait(data, 45964)
            .Mission(data, 483)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Southeast Well A-2: Aquatic Inspection II
        data.Apply(45841, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data, 45959)
            .Mission(data, 484)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45842, Patch.SeekersOfEternity) // Lunar Grass Carp
            .Bait(data, 45959)
            .Mission(data, 484)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45843, Patch.SeekersOfEternity) // Moonlit Snakehead
            .Bait(data, 45959)
            .Mission(data, 484)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45844, Patch.SeekersOfEternity) // Moonbeam
            .Bait(data, 45959)
            .Mission(data, 484)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45845, Patch.SeekersOfEternity) // Moongate Cod
            .Mooch(data, 45844)
            .Mission(data, 484)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45846, Patch.SeekersOfEternity) // Lunar Sturgeon
            .Mooch(data, 45845)
            .Mission(data, 484)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Westward Hop-print A-1: Fine-grade Water Filter Materials I
        data.Apply(45810, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data, 45959)
            .Mission(data, 485)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45811, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data, 45959)
            .Mission(data, 485)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45812, Patch.SeekersOfEternity) // Preceptor Betta
            .Bait(data, 45959)
            .Mission(data, 485)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45813, Patch.SeekersOfEternity) // Starcrystal Knife
            .Bait(data, 45959)
            .Mission(data, 485)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45814, Patch.SeekersOfEternity) // Bright Cobalt Eel
            .Bait(data, 45959)
            .Mission(data, 485)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Weeping Pool A-2: Fine-grade Water Filter Materials II
        data.Apply(45847, Patch.SeekersOfEternity) // Astacus Lamentorum
            .Bait(data)
            .Mission(data, 486)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45848, Patch.SeekersOfEternity) // Teardrop Knifefish
            .Bait(data, 45959)
            .Mission(data, 486)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45849, Patch.SeekersOfEternity) // Weeping Crab
            .Bait(data)
            .Mission(data, 486)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45850, Patch.SeekersOfEternity) // Silvermoon Tilapia
            .Bait(data, 45959)
            .Mission(data, 486)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45851, Patch.SeekersOfEternity) // Weeping Minnow
            .Bait(data, 45960)
            .Mission(data, 486)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Hollow Harbor A-3: Fine-grade Water Filter Materials III
        data.Apply(45877, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data, 45961)
            .Mission(data, 487)
            .Points(100)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45878, Patch.SeekersOfEternity) // Moongill
            .Bait(data, 45961)
            .Mission(data, 487)
            .Points(100)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45879, Patch.SeekersOfEternity) // Darkside Bass
            .Bait(data, 45961)
            .Mission(data, 487)
            .Points(100)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45880, Patch.SeekersOfEternity) // Opal Guppy
            .Bait(data, 45961)
            .Mission(data, 487)
            .Points(400)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45881, Patch.SeekersOfEternity) // Harbor Fang
            .Bait(data, 45961)
            .Mission(data, 487)
            .Points(600)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45882, Patch.SeekersOfEternity) // Deepmoon Cabomba
            .Bait(data, 45961)
            .Mission(data, 487)
            .Points(1000)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Northward Hop-print A-2: Coexisting Species I
        data.Apply(45853, Patch.SeekersOfEternity) // Melancholia
            .Bait(data, 45960)
            .Mission(data, 488)
            .Points(50)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45857, Patch.SeekersOfEternity) // Mooncrystal Perch
            .Bait(data, 45960)
            .Mission(data, 488)
            .Points(100)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45854, Patch.SeekersOfEternity) // Lunar Prismfish
            .Bait(data, 45960)
            .Mission(data, 488)
            .Points(150)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45855, Patch.SeekersOfEternity) // Hopping Flounder
            .Bait(data, 45960)
            .Mission(data, 488)
            .Points(400)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45858, Patch.SeekersOfEternity) // Cobalt Fish
            .Bait(data, 45960)
            .Mission(data, 488)
            .Points(600)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45856, Patch.SeekersOfEternity) // Galactic Flarefish
            .Bait(data, 45960)
            .Mission(data, 488)
            .Points(1000)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Glimmerpond Alpha A-3: Coexisting Species II
        data.Apply(45883, Patch.SeekersOfEternity) // Arsenic Axolotl
            .Bait(data, 45963)
            .Mission(data, 489)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45884, Patch.SeekersOfEternity) // Sunny Jellyfish
            .Bait(data, 45963)
            .Mission(data, 489)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45885, Patch.SeekersOfEternity) // Universal Darkfin
            .Bait(data, 45963)
            .Mission(data, 489)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45886, Patch.SeekersOfEternity) // Glimmerfish
            .Bait(data, 45963)
            .Mission(data, 489)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45887, Patch.SeekersOfEternity) // Lepopredator
            .Bait(data, 45963)
            .Mission(data, 489)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45888, Patch.SeekersOfEternity) // Soliclymenia
            .Bait(data, 45963)
            .Mission(data, 489)
            .MultiHook(4)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Palus Arsenici A-1: Aetherochemical Samples I
        data.Apply(45815, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data, 45964)
            .Time(360, 480)
            .Mission(data, 490)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45816, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data, 45964)
            .Time(360, 480)
            .Mission(data, 490)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45817, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data, 45964)
            .Time(360, 480)
            .Mission(data, 490)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45818, Patch.SeekersOfEternity) // Lunar Tadpole
            .Bait(data, 45964)
            .Time(360, 480)
            .Mission(data, 490)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45819, Patch.SeekersOfEternity) // Grand Atopodentatus
            .Bait(data, 45964)
            .Time(360, 480)
            .Mission(data, 490)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Glimmerpond Beta A-2: Aetherochemical Samples II
        data.Apply(45859, Patch.SeekersOfEternity) // Lunar Goldfish
            .Bait(data)
            .Mission(data, 491)
            .Bite(data, HookSet.Unknown, BiteType.Weak);
        data.Apply(45860, Patch.SeekersOfEternity) // Lunar Minnow
            .Bait(data)
            .Mission(data, 491)
            .Bite(data, HookSet.Unknown, BiteType.Strong);
        data.Apply(45861, Patch.SeekersOfEternity) // Gleamingray
            .Bait(data, 45963)
            .Mission(data, 491)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45862, Patch.SeekersOfEternity) // Lunar Butterfly
            .Bait(data, 45965)
            .Mission(data, 491)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45863, Patch.SeekersOfEternity) // Lunar Seagrapes
            .Bait(data, 45965)
            .Mission(data, 491)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(45864, Patch.SeekersOfEternity) // Fishingway
            .Bait(data, 45963)
            .Mission(data, 491)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Aetherial Falls A-3: Aetherochemical Samples III
        data.Apply(45889, Patch.SeekersOfEternity) // Ctenophora Lunaris
            .Bait(data)
            .Mission(data, 492)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45890, Patch.SeekersOfEternity) // Protomyke #721
            .Bait(data)
            .Mission(data, 492)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45891, Patch.SeekersOfEternity) // Argonauta Lunaris
            .Bait(data)
            .Mission(data, 492)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45892, Patch.SeekersOfEternity) // Aetherial Sword
            .Bait(data)
            .Mission(data, 492)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45893, Patch.SeekersOfEternity) // Moon Manta
            .Bait(data)
            .Mission(data, 492)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(45894, Patch.SeekersOfEternity) // Nadir Ambulocetus
            .Bait(data)
            .Mission(data, 492)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Aetherial Falls A-3: Unidentified Aquatic Specimens I
        data.Apply(45907, Patch.SeekersOfEternity) // Ctenophora Lunaris
            .Bait(data)
            .Mission(data, 493)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45908, Patch.SeekersOfEternity) // Protomyke #721
            .Bait(data)
            .Mission(data, 493)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45909, Patch.SeekersOfEternity) // Argonauta Lunaris
            .Bait(data)
            .Mission(data, 493)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45910, Patch.SeekersOfEternity) // Aetherial Sword
            .Bait(data)
            .Mission(data, 493)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45911, Patch.SeekersOfEternity) // Macropinna
            .Bait(data)
            .Mission(data, 493)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(45912, Patch.SeekersOfEternity) // Deepmoon Seadragon
            .Bait(data)
            .Mission(data, 493)
            .MultiHook(5)
            .Bite(data, HookSet.Precise, BiteType.Legendary);

        // Palus Arsenici A-3: Unidentified Aquatic Specimens II
        data.Apply(45895, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data)
            .Mission(data, 494)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45896, Patch.SeekersOfEternity) // Arsenical Proto-hropken
            .Bait(data)
            .Mission(data, 494)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45897, Patch.SeekersOfEternity) // Lunar Oil Eel
            .Bait(data)
            .Mission(data, 494)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45898, Patch.SeekersOfEternity) // Galactic Noise
            .Bait(data)
            .Mission(data, 494)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45899, Patch.SeekersOfEternity) // Onychodictyon
            .Bait(data)
            .Mission(data, 494)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45900, Patch.SeekersOfEternity) // Eolactoria Arsenici
            .Bait(data)
            .Mission(data, 494)
            .Lure(Enums.Lure.Modest)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Glimmerpond Beta A-3: Unidentified Aquatic Specimens III
        data.Apply(45901, Patch.SeekersOfEternity) // Lunar Goldfish
            .Bait(data)
            .Mission(data, 495)
            .Points(50)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45902, Patch.SeekersOfEternity) // Lunar Minnow
            .Bait(data)
            .Mission(data, 495)
            .Points(50)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45903, Patch.SeekersOfEternity) // Lunar Glassfish
            .Bait(data)
            .Mission(data, 495)
            .Points(50)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45904, Patch.SeekersOfEternity) // Gleaming Deathworm
            .Bait(data)
            .Mission(data, 495)
            .Points(500)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(45905, Patch.SeekersOfEternity) // Leanchoilia
            .Bait(data)
            .Mission(data, 495)
            .Points(150)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45906, Patch.SeekersOfEternity) // Harpagofututor //TODO: Intuition Fish, 3 Leanchoilia + 1 Lunar Glassfish
            .Bait(data)
            .Mission(data, 495)
            .Points(5000)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Southeast Well A-2: Processed Aquatic Metals
        data.Apply(45913, Patch.SeekersOfEternity) // Star Pleco
            .Bait(data, 45961)
            .Mission(data, 508)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45914, Patch.SeekersOfEternity) // Lunar Grass Carp
            .Bait(data, 45961)
            .Mission(data, 508)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45915, Patch.SeekersOfEternity) // Lunar Catfish
            .Bait(data, 45961)
            .Mission(data, 508)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45916, Patch.SeekersOfEternity) // Lunar Longhair
            .Bait(data, 45961)
            .Mission(data, 508)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(45917, Patch.SeekersOfEternity) // Heavy Ataxite 
            .Bait(data, 45961)
            .MultiHook(4)
            .Mission(data, 508)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Westward Hop-print A-2: Refined Moon Gel
        data.Apply(45918, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data, 45960)
            .Mission(data, 509)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45919, Patch.SeekersOfEternity) // Lunar Raiamas
            .Bait(data, 45960)
            .Mission(data, 509)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45920, Patch.SeekersOfEternity) // Preceptor Betta
            .Bait(data, 45960)
            .Mission(data, 509)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45921, Patch.SeekersOfEternity) // Moonlit Carp
            .Bait(data, 45960)
            .Mission(data, 509)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45922, Patch.SeekersOfEternity) // Hopping Lungfish
            .Bait(data, 45960)
            .Mission(data, 509)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Palus Arsenici A-3: Crystallic Gems
        data.Apply(45923, Patch.SeekersOfEternity) // Lunar Scorpion
            .Bait(data, 45965)
            .Mission(data, 510)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45924, Patch.SeekersOfEternity) // Moonwhip
            .Bait(data, 45965)
            .Mission(data, 510)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45925, Patch.SeekersOfEternity) // Polypus Arsenici
            .Bait(data, 45965)
            .Mission(data, 510)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45926, Patch.SeekersOfEternity) // Darkside Shrimp
            .Bait(data, 45965)
            .Mission(data, 510)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45927, Patch.SeekersOfEternity) // Stardust Octopus
            .Bait(data, 45965)
            .Mission(data, 510)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45928, Patch.SeekersOfEternity) // Raw Moonbright Tourmaline
            .Bait(data, 45965)
            .Mission(data, 510)
            .MultiHook(4)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Hollow Harbor A-3: Eel Rations
        data.Apply(45929, Patch.SeekersOfEternity) // Moonrock Candy
            .Bait(data, 45959)
            .Mission(data, 511)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45930, Patch.SeekersOfEternity) // Moongill
            .Bait(data, 45959)
            .Mission(data, 511)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45931, Patch.SeekersOfEternity) // Waxscale
            .Bait(data, 45959)
            .Mission(data, 511)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45932, Patch.SeekersOfEternity) // Corydoras Lunaris
            .Bait(data, 45959)
            .Mission(data, 511)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45933, Patch.SeekersOfEternity) // Infinity Eel
            .Bait(data, 45959)
            .Mission(data, 511)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45934, Patch.SeekersOfEternity) // Hollow Eel
            .Bait(data, 45959)
            .Mission(data, 511)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Westward Hop-print  Edible Fish
        data.Apply(45935, Patch.SeekersOfEternity) // Bluemoon Loach
            .Bait(data, 45966)
            .Mission(data, 542)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45936, Patch.SeekersOfEternity) // Lamentorum Geayi
            .Bait(data, 45966)
            .Mission(data, 542)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45937, Patch.SeekersOfEternity) // Moon Bluetail
            .Bait(data, 45966)
            .Mission(data, 542)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Northward Hop-print  Sunken Drone Salvage
        data.Apply(45938, Patch.SeekersOfEternity) // Melancholia
            .Bait(data, 45967)
            // .Weather(data, 196) Astromagnetic Storms, but not from normal weather.
            .Mission(data, 543)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(45939, Patch.SeekersOfEternity) // Sunken Drone
            .Bait(data, 45967)
            // .Weather(data, 196) Astromagnetic Storms, but not from normal weather.
            .Mission(data, 543)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Weeping Pool  Mutated Fish
        data.Apply(45945, Patch.SeekersOfEternity) // Moonlight Pleco
            .Bait(data, 45966)
            .Mission(data, 544)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(45946, Patch.SeekersOfEternity) // Lunastone Pleco
            .Bait(data, 45966)
            .Mission(data, 544)
            .Bite(data, HookSet.Precise, BiteType.Weak);
    }
    // @formatter:on
}
