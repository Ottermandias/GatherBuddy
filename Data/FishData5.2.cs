using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyEchoesOfAFallenStar(this FishManager fish)
        {
            fish.Apply     (28925, Patch.EchoesOfAFallenStar) // The Jaws of Undeath
                .Bait      (27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 24)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28926, Patch.EchoesOfAFallenStar) // White Ronso
                .Bait      (27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (28927, Patch.EchoesOfAFallenStar) // Ambling Caltrop
                .Bait      (27584, 27461)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 16)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (28928, Patch.EchoesOfAFallenStar) // Fae Rainbow
                .Bait      (27585)
                .Tug       (BiteType.Legendary)
                .Transition(2, 1)
                .Weather   (4)
                .HookType  (HookSet.Precise);
            fish.Apply     (28929, Patch.EchoesOfAFallenStar) // Black Jet
                .Bait      (27587)
                .Tug       (BiteType.Legendary)
                .Uptime    (2, 12)
                .Transition(3, 4)
                .Weather   (2)
                .HookType  (HookSet.Precise);
            fish.Apply     (28930, Patch.EchoesOfAFallenStar) // Ondo Sigh
                .Bait      (27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 14)
                .Weather   (2, 1)
                .HookType  (HookSet.Precise);
            fish.Apply     (28937, Patch.EchoesOfAFallenStar) // Galadion Goby
                .Bait      ();
            fish.Apply     (28938, Patch.EchoesOfAFallenStar) // Galadion Chovy
                .Bait      ();
            fish.Apply     (28939, Patch.EchoesOfAFallenStar) // Rosy Bream
                .Bait      ();
            fish.Apply     (28940, Patch.EchoesOfAFallenStar) // Tripod Fish
                .Bait      ();
            fish.Apply     (28941, Patch.EchoesOfAFallenStar) // Sunfly
                .Bait      ();
            fish.Apply     (28942, Patch.EchoesOfAFallenStar) // Tarnished Shark
                .Bait      ();
            fish.Apply     (29673, Patch.EchoesOfAFallenStar) // Thinker's Coral
                .Bait      (30136)
                .HookType  (HookSet.Precise);
            fish.Apply     (29678, Patch.EchoesOfAFallenStar) // Dragonspine
                .Bait      (30136)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (29718, Patch.EchoesOfAFallenStar) // Tossed Dagger
                .Bait      ();
            fish.Apply     (29719, Patch.EchoesOfAFallenStar) // Jasperhead
                .Bait      ();
            fish.Apply     (29720, Patch.EchoesOfAFallenStar) // Merlthor Lobster
                .Bait      ();
            fish.Apply     (29721, Patch.EchoesOfAFallenStar) // Heavenswimmer
                .Bait      ();
            fish.Apply     (29722, Patch.EchoesOfAFallenStar) // Ghoul Barracuda
                .Bait      ();
            fish.Apply     (29723, Patch.EchoesOfAFallenStar) // Leopard Eel
                .Bait      ();
            fish.Apply     (29724, Patch.EchoesOfAFallenStar) // Marine Bomb
                .Bait      ();
            fish.Apply     (29725, Patch.EchoesOfAFallenStar) // Momora Mora
                .Bait      ();
            fish.Apply     (29726, Patch.EchoesOfAFallenStar) // Merlthor Butterfly
                .Bait      ();
            fish.Apply     (29727, Patch.EchoesOfAFallenStar) // Gladius
                .Bait      ();
            fish.Apply     (29728, Patch.EchoesOfAFallenStar) // Rhotano Wahoo
                .Bait      ();
            fish.Apply     (29729, Patch.EchoesOfAFallenStar) // Rhotano Sardine
                .Bait      ();
            fish.Apply     (29730, Patch.EchoesOfAFallenStar) // Deep Plaice
                .Bait      ();
            fish.Apply     (29731, Patch.EchoesOfAFallenStar) // Crimson Monkfish
                .Bait      ();
            fish.Apply     (29732, Patch.EchoesOfAFallenStar) // Lampfish
                .Bait      ();
            fish.Apply     (29733, Patch.EchoesOfAFallenStar) // Ogre Eel
                .Bait      ();
            fish.Apply     (29734, Patch.EchoesOfAFallenStar) // Cyan Octopus
                .Bait      ();
            fish.Apply     (29735, Patch.EchoesOfAFallenStar) // Chrome Hammerhead
                .Bait      ();
            fish.Apply     (29736, Patch.EchoesOfAFallenStar) // Floefish
                .Bait      ();
            fish.Apply     (29737, Patch.EchoesOfAFallenStar) // Megasquid
                .Bait      ();
            fish.Apply     (29738, Patch.EchoesOfAFallenStar) // Oschon's Stone
                .Bait      ();
            fish.Apply     (29739, Patch.EchoesOfAFallenStar) // La Noscean Jelly
                .Bait      ();
            fish.Apply     (29740, Patch.EchoesOfAFallenStar) // Shaggy Seadragon
                .Bait      ();
            fish.Apply     (29741, Patch.EchoesOfAFallenStar) // Net Crawler
                .Bait      ();
            fish.Apply     (29742, Patch.EchoesOfAFallenStar) // Dark Nautilus
                .Bait      ();
            fish.Apply     (29743, Patch.EchoesOfAFallenStar) // Elder Dinichthys
                .Bait      ();
            fish.Apply     (29744, Patch.EchoesOfAFallenStar) // Drunkfish
                .Bait      ();
            fish.Apply     (29745, Patch.EchoesOfAFallenStar) // Little Leviathan
                .Bait      ();
            fish.Apply     (29746, Patch.EchoesOfAFallenStar) // Sabaton
                .Bait      ();
            fish.Apply     (29747, Patch.EchoesOfAFallenStar) // Shooting Star
                .Bait      ();
            fish.Apply     (29748, Patch.EchoesOfAFallenStar) // Hammerclaw
                .Bait      ();
            fish.Apply     (29749, Patch.EchoesOfAFallenStar) // Heavenskey
                .Bait      ();
            fish.Apply     (29750, Patch.EchoesOfAFallenStar) // Ghost Shark
                .Bait      ();
            fish.Apply     (29751, Patch.EchoesOfAFallenStar) // Quicksilver Blade
                .Bait      ();
            fish.Apply     (29752, Patch.EchoesOfAFallenStar) // Navigator's Print
                .Bait      ();
            fish.Apply     (29753, Patch.EchoesOfAFallenStar) // Casket Oyster
                .Bait      ();
            fish.Apply     (29754, Patch.EchoesOfAFallenStar) // Fishmonger
                .Bait      ();
            fish.Apply     (29755, Patch.EchoesOfAFallenStar) // Mythril Sovereign
                .Bait      ();
            fish.Apply     (29756, Patch.EchoesOfAFallenStar) // Nimble Dancer
                .Bait      ();
            fish.Apply     (29757, Patch.EchoesOfAFallenStar) // Sea Nettle
                .Bait      ();
            fish.Apply     (29758, Patch.EchoesOfAFallenStar) // Great Grandmarlin
                .Bait      ();
            fish.Apply     (29759, Patch.EchoesOfAFallenStar) // Shipwreck's Sail
                .Bait      ();
            fish.Apply     (29760, Patch.EchoesOfAFallenStar) // Azeyma's Sleeve
                .Bait      ();
            fish.Apply     (29761, Patch.EchoesOfAFallenStar) // Hi-aetherlouse
                .Bait      ();
            fish.Apply     (29762, Patch.EchoesOfAFallenStar) // Floating Saucer
                .Bait      ();
            fish.Apply     (29763, Patch.EchoesOfAFallenStar) // Aetheric Seadragon
                .Bait      ();
            fish.Apply     (29764, Patch.EchoesOfAFallenStar) // Coral Seadragon
                .Bait      ();
            fish.Apply     (29765, Patch.EchoesOfAFallenStar) // Roguesaurus
                .Bait      ();
            fish.Apply     (29766, Patch.EchoesOfAFallenStar) // Merman's Mane
                .Bait      ();
            fish.Apply     (29767, Patch.EchoesOfAFallenStar) // Sweeper
                .Bait      ();
            fish.Apply     (29768, Patch.EchoesOfAFallenStar) // Silencer
                .Bait      ();
            fish.Apply     (29769, Patch.EchoesOfAFallenStar) // Deep-sea Eel
                .Bait      ();
            fish.Apply     (29770, Patch.EchoesOfAFallenStar) // Executioner
                .Bait      ();
            fish.Apply     (29771, Patch.EchoesOfAFallenStar) // Wild Urchin
                .Bait      ();
            fish.Apply     (29772, Patch.EchoesOfAFallenStar) // True Barramundi
                .Bait      ();
            fish.Apply     (29773, Patch.EchoesOfAFallenStar) // Mopbeard
                .Bait      ();
            fish.Apply     (29774, Patch.EchoesOfAFallenStar) // Slipsnail
                .Bait      ();
            fish.Apply     (29775, Patch.EchoesOfAFallenStar) // Aronnax
                .Bait      ();
            fish.Apply     (29776, Patch.EchoesOfAFallenStar) // Coccosteus
                .Bait      ();
            fish.Apply     (29777, Patch.EchoesOfAFallenStar) // Bartholomew the Chopper
                .Bait      ();
            fish.Apply     (29778, Patch.EchoesOfAFallenStar) // Prowler
                .Bait      ();
            fish.Apply     (29779, Patch.EchoesOfAFallenStar) // Charlatan Survivor
                .Bait      ();
            fish.Apply     (29780, Patch.EchoesOfAFallenStar) // Prodigal Son
                .Bait      ();
            fish.Apply     (29781, Patch.EchoesOfAFallenStar) // Gugrusaurus
                .Bait      ();
            fish.Apply     (29782, Patch.EchoesOfAFallenStar) // Funnel Shark
                .Bait      ();
            fish.Apply     (29783, Patch.EchoesOfAFallenStar) // The Fallen One
                .Bait      ();
            fish.Apply     (29784, Patch.EchoesOfAFallenStar) // Spectral Megalodon
                .Bait      ();
            fish.Apply     (29785, Patch.EchoesOfAFallenStar) // Spectral Discus
                .Bait      ();
            fish.Apply     (29786, Patch.EchoesOfAFallenStar) // Spectral Sea Bo
                .Bait      ();
            fish.Apply     (29787, Patch.EchoesOfAFallenStar) // Spectral Bass
                .Bait      ();
            fish.Apply     (29788, Patch.EchoesOfAFallenStar) // Sothis
                .Bait      ();
            fish.Apply     (29789, Patch.EchoesOfAFallenStar) // Coral Manta
                .Bait      ();
            fish.Apply     (29790, Patch.EchoesOfAFallenStar) // Stonescale
                .Bait      ();
            fish.Apply     (29791, Patch.EchoesOfAFallenStar) // Elasmosaurus
                .Bait      ();
        }
        // @formatter:on
    }
}
