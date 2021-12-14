using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyEchoesOfAFallenStar(this GameData data)
    {
        data.Apply     (28925, Patch.EchoesOfAFallenStar) // The Jaws of Undeath
            .Bait      (data, 27589)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 1440)
            .Transition(data, 1, 2)
            .Weather   (data, 3, 4);
        data.Apply     (28926, Patch.EchoesOfAFallenStar) // White Ronso
            .Bait      (data, 27589)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (0, 120);
        data.Apply     (28927, Patch.EchoesOfAFallenStar) // Ambling Caltrop
            .Bait      (data, 27584, 27461)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (720, 960)
            .Weather   (data, 1);
        data.Apply     (28928, Patch.EchoesOfAFallenStar) // Fae Rainbow
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Transition(data, 2, 1)
            .Weather   (data, 4);
        data.Apply     (28929, Patch.EchoesOfAFallenStar) // Black Jet
            .Bait      (data, 27587)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (120, 720)
            .Transition(data, 3, 4)
            .Weather   (data, 2);
        data.Apply     (28930, Patch.EchoesOfAFallenStar) // Ondo Sigh
            .Bait      (data, 27590)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (720, 840)
            .Weather   (data, 2, 1);
        data.Apply     (28937, Patch.EchoesOfAFallenStar) // Galadion Goby
            .Bait      (data);
        data.Apply     (28938, Patch.EchoesOfAFallenStar) // Galadion Chovy
            .Bait      (data);
        data.Apply     (28939, Patch.EchoesOfAFallenStar) // Rosy Bream
            .Bait      (data);
        data.Apply     (28940, Patch.EchoesOfAFallenStar) // Tripod Fish
            .Bait      (data);
        data.Apply     (28941, Patch.EchoesOfAFallenStar) // Sunfly
            .Bait      (data);
        data.Apply     (28942, Patch.EchoesOfAFallenStar) // Tarnished Shark
            .Bait      (data);
        data.Apply     (29673, Patch.EchoesOfAFallenStar) // Thinker's Coral
            .Bait      (data, 30136)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (29678, Patch.EchoesOfAFallenStar) // Dragonspine
            .Bait      (data, 30136)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (29718, Patch.EchoesOfAFallenStar) // Tossed Dagger
            .Bait      (data);
        data.Apply     (29719, Patch.EchoesOfAFallenStar) // Jasperhead
            .Bait      (data);
        data.Apply     (29720, Patch.EchoesOfAFallenStar) // Merlthor Lobster
            .Bait      (data);
        data.Apply     (29721, Patch.EchoesOfAFallenStar) // Heavenswimmer
            .Bait      (data);
        data.Apply     (29722, Patch.EchoesOfAFallenStar) // Ghoul Barracuda
            .Bait      (data);
        data.Apply     (29723, Patch.EchoesOfAFallenStar) // Leopard Eel
            .Bait      (data);
        data.Apply     (29724, Patch.EchoesOfAFallenStar) // Marine Bomb
            .Bait      (data);
        data.Apply     (29725, Patch.EchoesOfAFallenStar) // Momora Mora
            .Bait      (data);
        data.Apply     (29726, Patch.EchoesOfAFallenStar) // Merlthor Butterfly
            .Bait      (data);
        data.Apply     (29727, Patch.EchoesOfAFallenStar) // Gladius
            .Bait      (data);
        data.Apply     (29728, Patch.EchoesOfAFallenStar) // Rhotano Wahoo
            .Bait      (data);
        data.Apply     (29729, Patch.EchoesOfAFallenStar) // Rhotano Sardine
            .Bait      (data);
        data.Apply     (29730, Patch.EchoesOfAFallenStar) // Deep Plaice
            .Bait      (data);
        data.Apply     (29731, Patch.EchoesOfAFallenStar) // Crimson Monkfish
            .Bait      (data);
        data.Apply     (29732, Patch.EchoesOfAFallenStar) // Lampfish
            .Bait      (data);
        data.Apply     (29733, Patch.EchoesOfAFallenStar) // Ogre Eel
            .Bait      (data);
        data.Apply     (29734, Patch.EchoesOfAFallenStar) // Cyan Octopus
            .Bait      (data);
        data.Apply     (29735, Patch.EchoesOfAFallenStar) // Chrome Hammerhead
            .Bait      (data);
        data.Apply     (29736, Patch.EchoesOfAFallenStar) // Floefish
            .Bait      (data);
        data.Apply     (29737, Patch.EchoesOfAFallenStar) // Megasquid
            .Bait      (data);
        data.Apply     (29738, Patch.EchoesOfAFallenStar) // Oschon's Stone
            .Bait      (data);
        data.Apply     (29739, Patch.EchoesOfAFallenStar) // La Noscean Jelly
            .Bait      (data);
        data.Apply     (29740, Patch.EchoesOfAFallenStar) // Shaggy Seadragon
            .Bait      (data);
        data.Apply     (29741, Patch.EchoesOfAFallenStar) // Net Crawler
            .Bait      (data);
        data.Apply     (29742, Patch.EchoesOfAFallenStar) // Dark Nautilus
            .Bait      (data);
        data.Apply     (29743, Patch.EchoesOfAFallenStar) // Elder Dinichthys
            .Bait      (data);
        data.Apply     (29744, Patch.EchoesOfAFallenStar) // Drunkfish
            .Bait      (data);
        data.Apply     (29745, Patch.EchoesOfAFallenStar) // Little Leviathan
            .Bait      (data);
        data.Apply     (29746, Patch.EchoesOfAFallenStar) // Sabaton
            .Bait      (data);
        data.Apply     (29747, Patch.EchoesOfAFallenStar) // Shooting Star
            .Bait      (data);
        data.Apply     (29748, Patch.EchoesOfAFallenStar) // Hammerclaw
            .Bait      (data);
        data.Apply     (29749, Patch.EchoesOfAFallenStar) // Heavenskey
            .Bait      (data);
        data.Apply     (29750, Patch.EchoesOfAFallenStar) // Ghost Shark
            .Bait      (data);
        data.Apply     (29751, Patch.EchoesOfAFallenStar) // Quicksilver Blade
            .Bait      (data);
        data.Apply     (29752, Patch.EchoesOfAFallenStar) // Navigator's Print
            .Bait      (data);
        data.Apply     (29753, Patch.EchoesOfAFallenStar) // Casket Oyster
            .Bait      (data);
        data.Apply     (29754, Patch.EchoesOfAFallenStar) // Fishmonger
            .Bait      (data);
        data.Apply     (29755, Patch.EchoesOfAFallenStar) // Mythril Sovereign
            .Bait      (data);
        data.Apply     (29756, Patch.EchoesOfAFallenStar) // Nimble Dancer
            .Bait      (data);
        data.Apply     (29757, Patch.EchoesOfAFallenStar) // Sea Nettle
            .Bait      (data);
        data.Apply     (29758, Patch.EchoesOfAFallenStar) // Great Grandmarlin
            .Bait      (data);
        data.Apply     (29759, Patch.EchoesOfAFallenStar) // Shipwreck's Sail
            .Bait      (data);
        data.Apply     (29760, Patch.EchoesOfAFallenStar) // Azeyma's Sleeve
            .Bait      (data);
        data.Apply     (29761, Patch.EchoesOfAFallenStar) // Hi-aetherlouse
            .Bait      (data);
        data.Apply     (29762, Patch.EchoesOfAFallenStar) // Floating Saucer
            .Bait      (data);
        data.Apply     (29763, Patch.EchoesOfAFallenStar) // Aetheric Seadragon
            .Bait      (data);
        data.Apply     (29764, Patch.EchoesOfAFallenStar) // Coral Seadragon
            .Bait      (data);
        data.Apply     (29765, Patch.EchoesOfAFallenStar) // Roguesaurus
            .Bait      (data);
        data.Apply     (29766, Patch.EchoesOfAFallenStar) // Merman's Mane
            .Bait      (data);
        data.Apply     (29767, Patch.EchoesOfAFallenStar) // Sweeper
            .Bait      (data);
        data.Apply     (29768, Patch.EchoesOfAFallenStar) // Silencer
            .Bait      (data);
        data.Apply     (29769, Patch.EchoesOfAFallenStar) // Deep-sea Eel
            .Bait      (data);
        data.Apply     (29770, Patch.EchoesOfAFallenStar) // Executioner
            .Bait      (data);
        data.Apply     (29771, Patch.EchoesOfAFallenStar) // Wild Urchin
            .Bait      (data);
        data.Apply     (29772, Patch.EchoesOfAFallenStar) // True Barramundi
            .Bait      (data);
        data.Apply     (29773, Patch.EchoesOfAFallenStar) // Mopbeard
            .Bait      (data);
        data.Apply     (29774, Patch.EchoesOfAFallenStar) // Slipsnail
            .Bait      (data);
        data.Apply     (29775, Patch.EchoesOfAFallenStar) // Aronnax
            .Bait      (data);
        data.Apply     (29776, Patch.EchoesOfAFallenStar) // Coccosteus
            .Bait      (data);
        data.Apply     (29777, Patch.EchoesOfAFallenStar) // Bartholomew the Chopper
            .Bait      (data);
        data.Apply     (29778, Patch.EchoesOfAFallenStar) // Prowler
            .Bait      (data);
        data.Apply     (29779, Patch.EchoesOfAFallenStar) // Charlatan Survivor
            .Bait      (data);
        data.Apply     (29780, Patch.EchoesOfAFallenStar) // Prodigal Son
            .Bait      (data);
        data.Apply     (29781, Patch.EchoesOfAFallenStar) // Gugrusaurus
            .Bait      (data);
        data.Apply     (29782, Patch.EchoesOfAFallenStar) // Funnel Shark
            .Bait      (data);
        data.Apply     (29783, Patch.EchoesOfAFallenStar) // The Fallen One
            .Bait      (data);
        data.Apply     (29784, Patch.EchoesOfAFallenStar) // Spectral Megalodon
            .Bait      (data);
        data.Apply     (29785, Patch.EchoesOfAFallenStar) // Spectral Discus
            .Bait      (data);
        data.Apply     (29786, Patch.EchoesOfAFallenStar) // Spectral Sea Bo
            .Bait      (data);
        data.Apply     (29787, Patch.EchoesOfAFallenStar) // Spectral Bass
            .Bait      (data);
        data.Apply     (29788, Patch.EchoesOfAFallenStar) // Sothis
            .Bait      (data);
        data.Apply     (29789, Patch.EchoesOfAFallenStar) // Coral Manta
            .Bait      (data);
        data.Apply     (29790, Patch.EchoesOfAFallenStar) // Stonescale
            .Bait      (data);
        data.Apply     (29791, Patch.EchoesOfAFallenStar) // Elasmosaurus
            .Bait      (data);
        data.Apply     (29994, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Cloudskipper
            .Bait      (data);
        data.Apply     (29995, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Meditator
            .Bait      (data);
        data.Apply     (29996, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Coeurlfish
            .Bait      (data);
        data.Apply     (29997, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Garpike
            .Bait      (data);
        data.Apply     (29998, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Pirarucu
            .Bait      (data);
        data.Apply     (29999, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Brown Bolo
            .Bait      (data);
        data.Apply     (30000, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Bitterling
            .Bait      (data);
        data.Apply     (30001, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Caiman
            .Bait      (data);
        data.Apply     (30002, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Cloud Cutter
            .Bait      (data);
        data.Apply     (30003, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Vampiric Tapestry
            .Bait      (data);
        data.Apply     (30004, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Tupuxuara
            .Bait      (data);
        data.Apply     (30005, Patch.EchoesOfAFallenStar) // Grade 2 Skybuilders' Blind Manta
            .Bait      (data);
        data.Apply     (30006, Patch.EchoesOfAFallenStar) // Grade 2 Artisanal Skybuilders' Rhomaleosaurus
            .Bait      (data);
        data.Apply     (30007, Patch.EchoesOfAFallenStar) // Grade 2 Artisanal Skybuilders' Gobbie Mask
            .Bait      (data);
        data.Apply     (30008, Patch.EchoesOfAFallenStar) // Grade 2 Artisanal Skybuilders' Pterodactyl
            .Bait      (data);
        data.Apply     (30009, Patch.EchoesOfAFallenStar) // Grade 2 Artisanal Skybuilders' Skyfish
            .Bait      (data);
        data.Apply     (30010, Patch.EchoesOfAFallenStar) // Grade 2 Artisanal Skybuilders' Cometfish
            .Bait      (data);
        data.Apply     (30011, Patch.EchoesOfAFallenStar) // Grade 2 Artisanal Skybuilders' Anomalocaris
            .Bait      (data);
        data.Apply     (30012, Patch.EchoesOfAFallenStar) // Grade 2 Artisanal Skybuilders' Rhamphorhynchus
            .Bait      (data);
        data.Apply     (30013, Patch.EchoesOfAFallenStar) // Grade 2 Artisanal Skybuilders' Dragon's Soul
            .Bait      (data);
    }
    // @formatter:on
}
