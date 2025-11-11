using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyFuturesRewritten(this GameData data)
    {
        data.Apply     (31770, Patch.FuturesRewritten) // Flintstrike
            .Bait      (data, 30136)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .ForceBig  (false);
        data.Apply     (31771, Patch.FuturesRewritten) // Pickled Pom
            .Bait      (data, 30136)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .ForceBig  (false);
        data.Apply     (32049, Patch.FuturesRewritten) // Moonlight Guppy
            .Bait      (data, 27589)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Time      (360, 480)
            .Transition(data, 1)
            .Weather   (data, 2);
        data.Apply     (32050, Patch.FuturesRewritten) // Steel Fan
            .Bait      (data, 27585)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Transition(data, 3)
            .Weather   (data, 4);
        data.Apply     (32051, Patch.FuturesRewritten) // Henodus Grandis
            .Mooch     (data, 27457)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (1320, 1440)
            .Weather   (data, 3);
        data.Apply     (32052, Patch.FuturesRewritten) // Sunken Tome
            .Bait      (data, 27585)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Time      (720, 960)
            .Transition(data, 2)
            .Weather   (data, 10);
        data.Apply     (32053, Patch.FuturesRewritten) // Pearl Pipira
            .Mooch     (data, 27587, 27492)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (1020, 1200)
            .Weather   (data, 4);
        data.Apply     (32054, Patch.FuturesRewritten) // The Ondotaker
            .Bait      (data, 27590)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 600)
            .Transition(data, 1)
            .Weather   (data, 3);
        data.Apply     (32055, Patch.FuturesRewritten) // Tortoiseshell Crab
            .Bait      (data, 29715)
            .Points    (10)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32056, Patch.FuturesRewritten) // Lady's Cameo
            .Bait      (data, 29714)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Points    (15)
            .Weather   (data, 2, 3, 4, 1);
        data.Apply     (32057, Patch.FuturesRewritten) // Metallic Boxfish
            .Bait      (data, 29714)
            .Points    (9)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32058, Patch.FuturesRewritten) // Goobbue Ray
            .Bait      (data, 29716)
            .Points    (33)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32059, Patch.FuturesRewritten) // Watermoura
            .Bait      (data, 29715)
            .Points    (41)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Weather   (data, 2, 3, 4, 9, 1);
        data.Apply     (32060, Patch.FuturesRewritten) // King Cobrafish
            .Bait      (data, 29716)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Points    (39)
            .Weather   (data, 2, 9, 10, 1);
        data.Apply     (32061, Patch.FuturesRewritten) // Mamahi-mahi
            .Bait      (data, 29716)
            .Points    (58)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (32062, Patch.FuturesRewritten) // Lavandin Remora
            .Bait      (data, 29715)
            .Points    (52)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32063, Patch.FuturesRewritten) // Spectral Butterfly
            .Bait      (data, 29714)
            .Points    (100)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Weather   (data, 2, 3, 4, 9, 10);
        data.Apply     (32064, Patch.FuturesRewritten) // Cieldalaes Geode
            .Bait      (data, 29715)
            .Points    (246)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 60, (32057, 3));
        data.Apply     (32065, Patch.FuturesRewritten) // Titanshell Crab
            .Bait      (data, 29715)
            .Points    (84)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32066, Patch.FuturesRewritten) // Mythril Boxfish
            .Bait      (data, 29714)
            .Points    (64)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32067, Patch.FuturesRewritten) // Mistbeard's Cup
            .Bait      (data, 29715)
            .Points    (84)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32068, Patch.FuturesRewritten) // Anomalocaris Saron
            .Bait      (data, 29715)
            .Points    (84)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32069, Patch.FuturesRewritten) // Flaming Eel
            .Bait      (data, 29715)
            .Points    (198)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Ocean     (OceanTime.Sunset);
        data.Apply     (32070, Patch.FuturesRewritten) // Jetborne Manta
            .Bait      (data, 29716)
            .Points    (75)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (32071, Patch.FuturesRewritten) // Devil's Sting
            .Bait      (data, 29715)
            .Points    (201)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Ocean     (OceanTime.Day);
        data.Apply     (32072, Patch.FuturesRewritten) // Callichthyid
            .Bait      (data, 29716)
            .Points    (178)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Ocean     (OceanTime.Day);
        data.Apply     (32073, Patch.FuturesRewritten) // Meandering Mora
            .Bait      (data, 29716)
            .Points    (283)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Ocean     (OceanTime.Sunset);
        data.Apply     (32074, Patch.FuturesRewritten) // Hafgufa
            .Bait      (data, 27590)
            .Points    (500)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Ocean     (OceanTime.Night)
            .Predators (data, 15, (32070, 2), (32067, 1));
        data.Apply     (32075, Patch.FuturesRewritten) // Thaliak Crab
            .Bait      (data, 29714)
            .Points    (9)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32076, Patch.FuturesRewritten) // Star of the Destroyer
            .Bait      (data, 29714)
            .Points    (14)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Weather   (data, 2, 3, 4, 1);
        data.Apply     (32077, Patch.FuturesRewritten) // True Scad
            .Bait      (data, 29715)
            .Points    (8)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32078, Patch.FuturesRewritten) // Blooded Wrasse
            .Bait      (data, 29716)
            .Points    (35)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 2, 3, 4, 7, 1);
        data.Apply     (32079, Patch.FuturesRewritten) // Bloodpolish Crab
            .Bait      (data, 29714)
            .Points    (28)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32080, Patch.FuturesRewritten) // Blue Stitcher
            .Bait      (data, 29715)
            .Points    (30)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Weather   (data, 2, 7, 8, 1);
        data.Apply     (32081, Patch.FuturesRewritten) // Bloodfresh Tuna
            .Bait      (data, 29716)
            .Points    (43)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32082, Patch.FuturesRewritten) // Sunken Mask
            .Bait      (data, 29714)
            .Points    (49)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (32083, Patch.FuturesRewritten) // Spectral Eel
            .Bait      (data, 29715)
            .Points    (100)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 2, 3, 4, 7, 8);
        data.Apply     (32084, Patch.FuturesRewritten) // Bareface
            .Bait      (data, 29715)
            .Points    (326)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 60, (32082, 1));
        data.Apply     (32085, Patch.FuturesRewritten) // Oracular Crab
            .Bait      (data, 29714)
            .Points    (102)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Ocean     (OceanTime.Day);
        data.Apply     (32086, Patch.FuturesRewritten) // Dravanian Bream
            .Bait      (data, 29715)
            .Points    (77)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32087, Patch.FuturesRewritten) // Skaldminni
            .Bait      (data, 29715)
            .Points    (102)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Ocean     (OceanTime.Night);
        data.Apply     (32088, Patch.FuturesRewritten) // Serrated Clam
            .Bait      (data, 29714)
            .Points    (74)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32089, Patch.FuturesRewritten) // Beatific Vision
            .Bait      (data, 29715)
            .Points    (77)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32090, Patch.FuturesRewritten) // Exterminator
            .Bait      (data, 29714)
            .Points    (255)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Ocean     (OceanTime.Day);
        data.Apply     (32091, Patch.FuturesRewritten) // Gory Tuna
            .Bait      (data, 29716)
            .Points    (92)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32092, Patch.FuturesRewritten) // Ticinepomis
            .Bait      (data, 29716)
            .Points    (92)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (32093, Patch.FuturesRewritten) // Quartz Hammerhead
            .Bait      (data, 29716)
            .Points    (460)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (32094, Patch.FuturesRewritten) // Seafaring Toad
            .Bait      (data, 2587)
            .Points    (500)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Ocean     (OceanTime.Day)
            .Predators (data, 15, (32089, 3));
        data.Apply     (32095, Patch.FuturesRewritten) // Crow Puffer
            .Bait      (data, 29714)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32096, Patch.FuturesRewritten) // Rothlyt Kelp
            .Bait      (data, 29714)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32097, Patch.FuturesRewritten) // Living Lantern
            .Bait      (data, 29715)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 2, 3, 4, 1);
        data.Apply     (32098, Patch.FuturesRewritten) // Honeycomb Fish
            .Bait      (data, 29714)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32099, Patch.FuturesRewritten) // Godsbed
            .Bait      (data, 29716)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 2, 9, 10, 1);
        data.Apply     (32100, Patch.FuturesRewritten) // Lansquenet
            .Bait      (data, 29716)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 2, 3, 4, 9, 1);
        data.Apply     (32101, Patch.FuturesRewritten) // Thavnairian Shark
            .Bait      (data, 29715)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (32102, Patch.FuturesRewritten) // Nephrite Eel
            .Bait      (data, 29715)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32103, Patch.FuturesRewritten) // Spectresaur
            .Bait      (data, 29716)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 2, 3, 4, 9, 10);
        data.Apply     (32104, Patch.FuturesRewritten) // Ginkgo Fin
            .Bait      (data, 29714)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Predators (data, 60, (32096, 3));
        data.Apply     (32105, Patch.FuturesRewritten) // Garum Jug
            .Bait      (data, 29715)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Ocean     (OceanTime.Day, OceanTime.Night);
        data.Apply     (32106, Patch.FuturesRewritten) // Smooth Jaguar
            .Bait      (data, 29716)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32107, Patch.FuturesRewritten) // Rothlyt Mussel
            .Bait      (data, 29714)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32108, Patch.FuturesRewritten) // Levi Elver
            .Bait      (data, 29715)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32109, Patch.FuturesRewritten) // Pearl Bombfish
            .Bait      (data, 29715)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Ocean     (OceanTime.Day, OceanTime.Night);
        data.Apply     (32110, Patch.FuturesRewritten) // Trollfish
            .Mooch     (data, 29714, 32107)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32111, Patch.FuturesRewritten) // Panoptes
            .Bait      (data, 29716)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Ocean     (OceanTime.Day);
        data.Apply     (32112, Patch.FuturesRewritten) // Crepe Sole
            .Bait      (data, 29714)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32113, Patch.FuturesRewritten) // Knifejaw
            .Bait      (data, 29715)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32114, Patch.FuturesRewritten) // Placodus
            .Mooch     (data, 29714, 32107)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Ocean     (OceanTime.Sunset)
            .Predators (data, 45, (32110, 1));
        data.Apply     (32882, Patch.FuturesRewritten) // Grade 4 Skybuilders' Zagas Khaal
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32883, Patch.FuturesRewritten) // Grade 4 Skybuilders' Goldsmith Crab
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32884, Patch.FuturesRewritten) // Grade 4 Skybuilders' Common Bitterling
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32885, Patch.FuturesRewritten) // Grade 4 Skybuilders' Skyloach
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32886, Patch.FuturesRewritten) // Grade 4 Skybuilders' Glacier Core
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32887, Patch.FuturesRewritten) // Grade 4 Skybuilders' Kissing Fish
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32888, Patch.FuturesRewritten) // Grade 4 Skybuilders' Cavalry Catfish
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32889, Patch.FuturesRewritten) // Grade 4 Skybuilders' Manasail
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32890, Patch.FuturesRewritten) // Grade 4 Skybuilders' Starflower
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32891, Patch.FuturesRewritten) // Grade 4 Skybuilders' Cyan Crab
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32892, Patch.FuturesRewritten) // Grade 4 Skybuilders' Fickle Krait
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32893, Patch.FuturesRewritten) // Grade 4 Skybuilders' Proto-hropken
            .Bait      (data, 29717)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (32894, Patch.FuturesRewritten) // Grade 4 Skybuilders' Ghost Faerie
            .Bait      (data, 30280)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32895, Patch.FuturesRewritten) // Grade 4 Skybuilders' Ashfish
            .Mooch     (data, 32894)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32896, Patch.FuturesRewritten) // Grade 4 Skybuilders' Whitehorse
            .Bait      (data, 30281)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32897, Patch.FuturesRewritten) // Grade 4 Skybuilders' Ocean Cloud
            .Bait      (data, 30280)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32898, Patch.FuturesRewritten) // Grade 4 Skybuilders' Black Fanfish
            .Mooch     (data, 32894)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32899, Patch.FuturesRewritten) // Grade 4 Skybuilders' Sunfish
            .Mooch     (data, 32894)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32900, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Sweatfish
            .Bait      (data, 30281)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (32901, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Sculptor
            .Bait      (data, 30280)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32902, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Little Thalaos
            .Mooch     (data, 32894)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (32903, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Lightning Chaser
            .Mooch     (data, 32894)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (32904, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Marrella
            .Bait      (data, 30281)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Weather   (data, 134);
        data.Apply     (32905, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Crimson Namitaro
            .Bait      (data, 30280)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 133);
        data.Apply     (32906, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Griffin
            .Mooch     (data, 32894)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 136);
        data.Apply     (32907, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Meganeura
            .Mooch     (data, 32894)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 135);
    }
    // @formatter:on
}
