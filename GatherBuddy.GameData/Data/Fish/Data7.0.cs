using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyDawntrail(this GameData data)
    {
        data.Apply(43664, Patch.Dawntrail)  // Mosquito Fish
            .Bait(data, 43852)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43665, Patch.Dawntrail) // Pel Frog
            .Bait(data, 43849)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43666, Patch.Dawntrail) // Petticoat Tetra
            .Bait(data, 43849)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43667, Patch.Dawntrail) // Reasonscale Silverside
            .Bait(data, 43850)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43668, Patch.Dawntrail) // Stardust Shrimp
            .Bait(data, 43850)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43669, Patch.Dawntrail) // Shallows Sot
            .Bait(data, 43850)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43670, Patch.Dawntrail) // Ghostfish
            .Bait(data, 43850)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43671, Patch.Dawntrail) // Permit
            .Bait(data, 43859)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43672, Patch.Dawntrail) // Floating Fife
            .Bait(data, 43850)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43673, Patch.Dawntrail) // Blue Purse
            .Bait(data, 43850)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43674, Patch.Dawntrail) // Goldfin Cavalli
            .Bait(data, 43850)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43675, Patch.Dawntrail) // Sea Alpaca
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43676, Patch.Dawntrail) // Marbled Hatchetfish
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43677, Patch.Dawntrail) // Starsnipper
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43678, Patch.Dawntrail) // Urqotrout
            .Bait(data, 43851)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43679, Patch.Dawntrail) // Yok Huy Stonecutter
            .Bait(data, 43851)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43680, Patch.Dawntrail) // Chirwagur Loach
            .Bait(data, 43851)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43681, Patch.Dawntrail) // Alright Alright Alright
            .Bait(data, 43851)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43682, Patch.Dawntrail) // Zorlortor
            .Bait(data, 43851)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43683, Patch.Dawntrail) // Shooting Starscale
            .Bait(data, 43851)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43684, Patch.Dawntrail) // First Feastfish
            .Bait(data, 43852)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43685, Patch.Dawntrail) // Urqofrog
            .Bait(data, 43858)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Time(1200, 1440)
            .Lure(Enums.Lure.Modest);
        data.Apply(43686, Patch.Dawntrail) // Zeh Tortoh
            .Bait(data, 43852)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43687, Patch.Dawntrail) // Morsel Trout
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43688, Patch.Dawntrail) // Deepwarden
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43689, Patch.Dawntrail) // Kozama'uka Skipper
            .Bait(data, 43852)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43690, Patch.Dawntrail) // Bopo'u Sleeper
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43691, Patch.Dawntrail) // Poison Dyefrog
            .Bait(data, 43849)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43692, Patch.Dawntrail) // Pleated Matamata
            .Mooch(data, 43691)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43693, Patch.Dawntrail) // Uyuy Xage
            .Mooch(data, 43691)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43694, Patch.Dawntrail) // Plattershell
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43695, Patch.Dawntrail) // Candiru
            .Bait(data, 43849)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43696, Patch.Dawntrail) // Trahira
            .Bait(data, 43849)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43697, Patch.Dawntrail) // Driftwood Catfish
            .Bait(data, 43849)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43698, Patch.Dawntrail) // Oho Hunu
            .Bait(data, 43849)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43699, Patch.Dawntrail) // Bronze Pleco
            .Bait(data, 43849)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43700, Patch.Dawntrail) // Crenicichla
            .Bait(data, 43849)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43701, Patch.Dawntrail) // Hunu Peacock Bass
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43702, Patch.Dawntrail) // Pelubane
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43703, Patch.Dawntrail) // Giant Trahira
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43704, Patch.Dawntrail) // Ku'uxage Bitterling
            .Bait(data, 43852)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43705, Patch.Dawntrail) // Tonguesnatcher
            .Bait(data, 43852)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43706, Patch.Dawntrail) // Corn Dace
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43707, Patch.Dawntrail) // Ka Puyhu
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43708, Patch.Dawntrail) // Ligaka Guppy
            .Bait(data, 43849)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43709, Patch.Dawntrail) // Dumplingfish
            .Bait(data, 43849)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43710, Patch.Dawntrail) // Banded Candiru
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43711, Patch.Dawntrail) // Oxydoras
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43712, Patch.Dawntrail) // Spotted Stingray
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43713, Patch.Dawntrail) // Yellow Peacock Bass
            .Mooch(data, 43709)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Time(720, 780);
        data.Apply(43714, Patch.Dawntrail) // Shovelnose Catfish
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43715, Patch.Dawntrail) // Shimmering Shadow
            .Bait(data, 43849)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43716, Patch.Dawntrail) // Ut'ohmu Dawnfish
            .Bait(data, 43849)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43717, Patch.Dawntrail) // Belonesox
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43718, Patch.Dawntrail) // Xd'aa Talat Cichlid
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43719, Patch.Dawntrail) // Iq Br'aax Sailfin
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43720, Patch.Dawntrail) // Hunmu Ob
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43721, Patch.Dawntrail) // Apoda
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43722, Patch.Dawntrail) // Mirror Carp
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43723, Patch.Dawntrail) // Tsoly Mu
            .Bait(data, 43849)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43724, Patch.Dawntrail) // Iq Rrax Leaffish
            .Bait(data, 43849)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43725, Patch.Dawntrail) // Archmatron Angelfish
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43726, Patch.Dawntrail) // Yak T'el Salamander
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43727, Patch.Dawntrail) // Shadowtongue
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43728, Patch.Dawntrail) // Cloud-eye Carp
            .Bait(data, 43849)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43729, Patch.Dawntrail) // Xobr'it Lobster
            .Bait(data, 43852)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43730, Patch.Dawntrail) // Sunbright Axolotl
            .Mooch(data, 43728)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43731, Patch.Dawntrail) // Horned Frog
            .Mooch(data, 43728)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Time(0, 120);
        data.Apply(43732, Patch.Dawntrail) // Mudfish
            .Bait(data, 43852)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43733, Patch.Dawntrail) // Turali Beaded Lizard
            .Bait(data, 43855)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43734, Patch.Dawntrail) // Yak T'el Catfish
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43735, Patch.Dawntrail) // Ankledeep Catfish
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43736, Patch.Dawntrail) // Checkered Cichlid
            .Bait(data, 43852)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43737, Patch.Dawntrail) // Moxutural Gar
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43738, Patch.Dawntrail) // Variatus
            .Bait(data, 43852)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43739, Patch.Dawntrail) // Welkin Catfish
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43740, Patch.Dawntrail) // Flawless Saucer
            .Bait(data, 43852)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43741, Patch.Dawntrail) // Glittergill
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43742, Patch.Dawntrail) // Twig Catfish
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43743, Patch.Dawntrail) // Blind Brotula
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43744, Patch.Dawntrail) // Yak T'el Caiman
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43745, Patch.Dawntrail) // Ut'ohmu Tiika
            .Bait(data, 43850)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43746, Patch.Dawntrail) // Sharknose Goby
            .Bait(data, 43850)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43747, Patch.Dawntrail) // Yak T'el Crab
            .Mooch(data, 43746)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43748, Patch.Dawntrail) // Xty'iinbek Sleeper
            .Bait(data, 43850)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43749, Patch.Dawntrail) // Chain Shark
            .Mooch(data, 43747)
            .Time(1080, 1200)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43750, Patch.Dawntrail) // Bonytail Chub
            .Bait(data, 43855)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43751, Patch.Dawntrail) // Niikwerepi Trout
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43752, Patch.Dawntrail) // Pyariyoanaan Cichlid
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43753, Patch.Dawntrail) // Shaaloani Salmon
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43754, Patch.Dawntrail) // Trailtrout
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43755, Patch.Dawntrail) // Toari Sucker
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43756, Patch.Dawntrail) // Turali Paddlefish
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43757, Patch.Dawntrail) // Zorgor Core
            .Bait(data, 43857)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43758, Patch.Dawntrail) // Cloudribbon
            .Bait(data, 43853)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43759, Patch.Dawntrail) // Zorgor Scorpion
            .Bait(data, 43853)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43760, Patch.Dawntrail) // Vagrant Cascade
            .Bait(data, 43853)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43761, Patch.Dawntrail) // Zorgor Condor
            .Bait(data, 43853)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Lure(Enums.Lure.Ambitious);
        data.Apply(43762, Patch.Dawntrail) // Cloud Wasp
            .Bait(data, 43857)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Lure(Enums.Lure.Modest);
        data.Apply(43763, Patch.Dawntrail) // Azure Glider
            .Bait(data, 43857)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43764, Patch.Dawntrail) // Hellsnow Shark
            .Bait(data, 43857)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43765, Patch.Dawntrail) // Heritage Loach
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43766, Patch.Dawntrail) // Outskirts Sniffer
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43767, Patch.Dawntrail) // Pumpkin Perch
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43768, Patch.Dawntrail) // Tiger Muskellunge
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43769, Patch.Dawntrail) // Moon Croppie
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43770, Patch.Dawntrail) // Driftdowns Trout
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43771, Patch.Dawntrail) // Electrobetta
            .Bait(data, 43855)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43772, Patch.Dawntrail) // Thundering Redbelly
            .Bait(data, 43855)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43773, Patch.Dawntrail) // Yyasulani Bowfin
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43774, Patch.Dawntrail) // Crackling Flounder
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43775, Patch.Dawntrail) // Goldgrouper
            .Bait(data, 43855)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Lure(Enums.Lure.Ambitious);
        data.Apply(43776, Patch.Dawntrail) // Everkeep Yabby
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43777, Patch.Dawntrail) // Stickleback
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43778, Patch.Dawntrail) // Custodian Carp
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43779, Patch.Dawntrail) // Mosaic Loach
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43780, Patch.Dawntrail) // Rosebud Frog
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43781, Patch.Dawntrail) // Knight Goby
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43782, Patch.Dawntrail) // Windspath Eel
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43783, Patch.Dawntrail) // Harlequin Lancer
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43784, Patch.Dawntrail) // Cleyran Carp
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Lure(Enums.Lure.Ambitious);
        data.Apply(43785, Patch.Dawntrail) // Giant Snakehead
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43786, Patch.Dawntrail) // Datnioides
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43787, Patch.Dawntrail) // Neo Arowana
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43788, Patch.Dawntrail) // Canal Bream
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43789, Patch.Dawntrail) // Iris Wrasse
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43790, Patch.Dawntrail) // Canal Drum
            .Bait(data, 43859)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43791, Patch.Dawntrail) // Butterfly Ray
            .Bait(data, 43859)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43792, Patch.Dawntrail) // Hydro Louvar
            .Bait(data, 43859)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43793, Patch.Dawntrail) // Devotion Clam
            .Bait(data, 43856)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43794, Patch.Dawntrail) // Forgotten One
            .Bait(data, 43859)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43795, Patch.Dawntrail) // Copper Shark
            .Bait(data, 43859)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Lure(Enums.Lure.Ambitious)
            .Weather(data, 3, 4);
        data.Apply(43796, Patch.Dawntrail) // Golden Day
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43797, Patch.Dawntrail) // Starglass Gibel
            .Bait(data, 29717)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(43798, Patch.Dawntrail) // Scarlet Queen
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43799, Patch.Dawntrail) // Ultimoat Carp
            .Bait(data, 29717)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(43803, Patch.Dawntrail) // Turali Loach
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply(43804, Patch.Dawntrail) // Yok Huy Toad
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply(43805, Patch.Dawntrail) // Stone Pleco
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply(43806, Patch.Dawntrail) // Shark Catfish
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.Fast);
        data.Apply(43807, Patch.Dawntrail) // Zeh Lar Dor
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply(43808, Patch.Dawntrail) // Karvarichthys
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply(43809, Patch.Dawntrail) // Severum
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.Average);
        data.Apply(43810, Patch.Dawntrail) // Ligaka Blossom
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply(43811, Patch.Dawntrail) // Potglaze Stingray
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.ExtremelySlow);
        data.Apply(43812, Patch.Dawntrail) // Emberwisp
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply(43813, Patch.Dawntrail) // Ihuyka Colossoma
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply(43814, Patch.Dawntrail) // Torchtail
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply(43815, Patch.Dawntrail) // Variegated Sisterscale
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply(43816, Patch.Dawntrail) // Kozama'uka Characin
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply(43817, Patch.Dawntrail) // Charcoal Eel
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply(43818, Patch.Dawntrail) // Rainbow Pipira
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply(43819, Patch.Dawntrail) // Lau Lau
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.ExtremelyFast);
        data.Apply(43820, Patch.Dawntrail) // Ligaka Pirarucu
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply(43821, Patch.Dawntrail) // Piraputanga
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.Fast)
            .Predators(data, 300, (43810, 16));
        data.Apply(43822, Patch.Dawntrail) // Purussaurus
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.ExtremelyFast)
            .Predators(data, 300, (43810, 16));
        data.Apply(43823, Patch.Dawntrail) // Turali Land Crab
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.SuperSlow);
        data.Apply(43824, Patch.Dawntrail) // Crested Goby
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply(43825, Patch.Dawntrail) // Tsoly Turtle
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.ExtremelySlow);
        data.Apply(43826, Patch.Dawntrail) // Viperfish
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply(43827, Patch.Dawntrail) // Horizon Crocodile
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply(43828, Patch.Dawntrail) // Tsoly Gar
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply(43829, Patch.Dawntrail) // Sunlit Prism
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply(43830, Patch.Dawntrail) // Fategazer
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply(43831, Patch.Dawntrail) // Frillfin Goby
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply(43832, Patch.Dawntrail) // Black Ibruq
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply(43833, Patch.Dawntrail) // Speckled Peacock Bass
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply(43834, Patch.Dawntrail) // Golden Characin
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply(43835, Patch.Dawntrail) // Deadleaf Minnow
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply(43836, Patch.Dawntrail) // Glistening Discus
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply(43837, Patch.Dawntrail) // Lightning Eel
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply(43838, Patch.Dawntrail) // Longwhisker Lungfish
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.ExtremelySlow);
        data.Apply(43839, Patch.Dawntrail) // Minted Arowana
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply(43840, Patch.Dawntrail) // Br'aaxfish
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply(43841, Patch.Dawntrail) // Iq Rrax Crab
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.VeryFast)
            .Predators(data, 300, (43836, 16), (43842, 2), (43833, 3));
        data.Apply(43842, Patch.Dawntrail) // Wivre Cod
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.SuperFast)
            .Predators(data, 300, (43836, 16));
        data.Apply(43843, Patch.Dawntrail) // Warmouth
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply(43844, Patch.Dawntrail) // Shaaloani Goby
            .Spear(data, SpearfishSize.Small, SpearfishSpeed.SuperSlow);
        data.Apply(43845, Patch.Dawntrail) // Sauger
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply(43846, Patch.Dawntrail) // Niikwerepi Bass
            .Spear(data, SpearfishSize.Average, SpearfishSpeed.ExtremelySlow);
        data.Apply(43847, Patch.Dawntrail) // Longnose Gar
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply(43848, Patch.Dawntrail) // Midnight Carp
            .Spear(data, SpearfishSize.Large, SpearfishSpeed.Average);
    }
    // @formatter:on
}
