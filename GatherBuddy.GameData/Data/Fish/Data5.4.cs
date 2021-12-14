using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyFuturesRewritten(this GameData data)
    {
        data.Apply     (31770, Patch.FuturesRewritten) // Flintstrike
            .Bait      (data, 30136)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (31771, Patch.FuturesRewritten) // Pickled Pom
            .Bait      (data, 30136)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (32049, Patch.FuturesRewritten) // Moonlight Guppy
            .Bait      (data, 27589)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (360, 480)
            .Transition(data, 1)
            .Weather   (data, 2);
        data.Apply     (32050, Patch.FuturesRewritten) // Steel Fan
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Transition(data, 3)
            .Weather   (data, 4);
        data.Apply     (32051, Patch.FuturesRewritten) // Henodus Grandis
            .Bait      (data, 27588, 27457)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1320, 1440)
            .Weather   (data, 3);
        data.Apply     (32052, Patch.FuturesRewritten) // Sunken Tome
            .Bait      (data, 27585)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (720, 960)
            .Transition(data, 2)
            .Weather   (data, 10);
        data.Apply     (32053, Patch.FuturesRewritten) // Pearl Pipira
            .Bait      (data, 27587, 27492)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (1020, 1200)
            .Weather   (data, 4);
        data.Apply     (32054, Patch.FuturesRewritten) // The Ondotaker
            .Bait      (data, 27590)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 600)
            .Transition(data, 1)
            .Weather   (data, 3);
        data.Apply     (32055, Patch.FuturesRewritten) // Tortoiseshell Crab
            .Bait      (data);
        data.Apply     (32056, Patch.FuturesRewritten) // Lady's Cameo
            .Bait      (data);
        data.Apply     (32057, Patch.FuturesRewritten) // Metallic Boxfish
            .Bait      (data);
        data.Apply     (32058, Patch.FuturesRewritten) // Goobbue Ray
            .Bait      (data);
        data.Apply     (32059, Patch.FuturesRewritten) // Watermoura
            .Bait      (data);
        data.Apply     (32060, Patch.FuturesRewritten) // King Cobrafish
            .Bait      (data);
        data.Apply     (32061, Patch.FuturesRewritten) // Mamahi-mahi
            .Bait      (data);
        data.Apply     (32062, Patch.FuturesRewritten) // Lavandin Remora
            .Bait      (data);
        data.Apply     (32063, Patch.FuturesRewritten) // Spectral Butterfly
            .Bait      (data);
        data.Apply     (32064, Patch.FuturesRewritten) // Cieldalaes Geode
            .Bait      (data);
        data.Apply     (32065, Patch.FuturesRewritten) // Titanshell Crab
            .Bait      (data);
        data.Apply     (32066, Patch.FuturesRewritten) // Mythril Boxfish
            .Bait      (data);
        data.Apply     (32067, Patch.FuturesRewritten) // Mistbeard's Cup
            .Bait      (data);
        data.Apply     (32068, Patch.FuturesRewritten) // Anomalocaris Saron
            .Bait      (data);
        data.Apply     (32069, Patch.FuturesRewritten) // Flaming Eel
            .Bait      (data);
        data.Apply     (32070, Patch.FuturesRewritten) // Jetborne Manta
            .Bait      (data);
        data.Apply     (32071, Patch.FuturesRewritten) // Devil's Sting
            .Bait      (data);
        data.Apply     (32072, Patch.FuturesRewritten) // Callichthyid
            .Bait      (data);
        data.Apply     (32073, Patch.FuturesRewritten) // Meandering Mora
            .Bait      (data);
        data.Apply     (32074, Patch.FuturesRewritten) // Hafgufa
            .Bait      (data);
        data.Apply     (32075, Patch.FuturesRewritten) // Thaliak Crab
            .Bait      (data);
        data.Apply     (32076, Patch.FuturesRewritten) // Star of the Destroyer
            .Bait      (data);
        data.Apply     (32077, Patch.FuturesRewritten) // True Scad
            .Bait      (data);
        data.Apply     (32078, Patch.FuturesRewritten) // Blooded Wrasse
            .Bait      (data);
        data.Apply     (32079, Patch.FuturesRewritten) // Bloodpolish Crab
            .Bait      (data);
        data.Apply     (32080, Patch.FuturesRewritten) // Blue Stitcher
            .Bait      (data);
        data.Apply     (32081, Patch.FuturesRewritten) // Bloodfresh Tuna
            .Bait      (data);
        data.Apply     (32082, Patch.FuturesRewritten) // Sunken Mask
            .Bait      (data);
        data.Apply     (32083, Patch.FuturesRewritten) // Spectral Eel
            .Bait      (data);
        data.Apply     (32084, Patch.FuturesRewritten) // Bareface
            .Bait      (data);
        data.Apply     (32085, Patch.FuturesRewritten) // Oracular Crab
            .Bait      (data);
        data.Apply     (32086, Patch.FuturesRewritten) // Dravanian Bream
            .Bait      (data);
        data.Apply     (32087, Patch.FuturesRewritten) // Skaldminni
            .Bait      (data);
        data.Apply     (32088, Patch.FuturesRewritten) // Serrated Clam
            .Bait      (data);
        data.Apply     (32089, Patch.FuturesRewritten) // Beatific Vision
            .Bait      (data);
        data.Apply     (32090, Patch.FuturesRewritten) // Exterminator
            .Bait      (data);
        data.Apply     (32091, Patch.FuturesRewritten) // Gory Tuna
            .Bait      (data);
        data.Apply     (32092, Patch.FuturesRewritten) // Ticinepomis
            .Bait      (data);
        data.Apply     (32093, Patch.FuturesRewritten) // Quartz Hammerhead
            .Bait      (data);
        data.Apply     (32094, Patch.FuturesRewritten) // Seafaring Toad
            .Bait      (data);
        data.Apply     (32095, Patch.FuturesRewritten) // Crow Puffer
            .Bait      (data);
        data.Apply     (32096, Patch.FuturesRewritten) // Rothlyt Kelp
            .Bait      (data);
        data.Apply     (32097, Patch.FuturesRewritten) // Living Lantern
            .Bait      (data);
        data.Apply     (32098, Patch.FuturesRewritten) // Honeycomb Fish
            .Bait      (data);
        data.Apply     (32099, Patch.FuturesRewritten) // Godsbed
            .Bait      (data);
        data.Apply     (32100, Patch.FuturesRewritten) // Lansquenet
            .Bait      (data);
        data.Apply     (32101, Patch.FuturesRewritten) // Thavnairian Shark
            .Bait      (data);
        data.Apply     (32102, Patch.FuturesRewritten) // Nephrite Eel
            .Bait      (data);
        data.Apply     (32103, Patch.FuturesRewritten) // Spectresaur
            .Bait      (data);
        data.Apply     (32104, Patch.FuturesRewritten) // Ginkgo Fin
            .Bait      (data);
        data.Apply     (32105, Patch.FuturesRewritten) // Garum Jug
            .Bait      (data);
        data.Apply     (32106, Patch.FuturesRewritten) // Smooth Jaguar
            .Bait      (data);
        data.Apply     (32107, Patch.FuturesRewritten) // Rothlyt Mussel
            .Bait      (data);
        data.Apply     (32108, Patch.FuturesRewritten) // Levi Elver
            .Bait      (data);
        data.Apply     (32109, Patch.FuturesRewritten) // Pearl Bombfish
            .Bait      (data);
        data.Apply     (32110, Patch.FuturesRewritten) // Trollfish
            .Bait      (data);
        data.Apply     (32111, Patch.FuturesRewritten) // Panoptes
            .Bait      (data);
        data.Apply     (32112, Patch.FuturesRewritten) // Crepe Sole
            .Bait      (data);
        data.Apply     (32113, Patch.FuturesRewritten) // Knifejaw
            .Bait      (data);
        data.Apply     (32114, Patch.FuturesRewritten) // Placodus
            .Bait      (data);
        data.Apply     (32882, Patch.FuturesRewritten) // Grade 4 Skybuilders' Zagas Khaal
            .Bait      (data);
        data.Apply     (32883, Patch.FuturesRewritten) // Grade 4 Skybuilders' Goldsmith Crab
            .Bait      (data);
        data.Apply     (32884, Patch.FuturesRewritten) // Grade 4 Skybuilders' Common Bitterling
            .Bait      (data);
        data.Apply     (32885, Patch.FuturesRewritten) // Grade 4 Skybuilders' Skyloach
            .Bait      (data);
        data.Apply     (32886, Patch.FuturesRewritten) // Grade 4 Skybuilders' Glacier Core
            .Bait      (data);
        data.Apply     (32887, Patch.FuturesRewritten) // Grade 4 Skybuilders' Kissing Fish
            .Bait      (data);
        data.Apply     (32888, Patch.FuturesRewritten) // Grade 4 Skybuilders' Cavalry Catfish
            .Bait      (data);
        data.Apply     (32889, Patch.FuturesRewritten) // Grade 4 Skybuilders' Manasail
            .Bait      (data);
        data.Apply     (32890, Patch.FuturesRewritten) // Grade 4 Skybuilders' Starflower
            .Bait      (data);
        data.Apply     (32891, Patch.FuturesRewritten) // Grade 4 Skybuilders' Cyan Crab
            .Bait      (data);
        data.Apply     (32892, Patch.FuturesRewritten) // Grade 4 Skybuilders' Fickle Krait
            .Bait      (data);
        data.Apply     (32893, Patch.FuturesRewritten) // Grade 4 Skybuilders' Proto-hropken
            .Bait      (data);
        data.Apply     (32894, Patch.FuturesRewritten) // Grade 4 Skybuilders' Ghost Faerie
            .Bait      (data);
        data.Apply     (32895, Patch.FuturesRewritten) // Grade 4 Skybuilders' Ashfish
            .Bait      (data);
        data.Apply     (32896, Patch.FuturesRewritten) // Grade 4 Skybuilders' Whitehorse
            .Bait      (data);
        data.Apply     (32897, Patch.FuturesRewritten) // Grade 4 Skybuilders' Ocean Cloud
            .Bait      (data);
        data.Apply     (32898, Patch.FuturesRewritten) // Grade 4 Skybuilders' Black Fanfish
            .Bait      (data);
        data.Apply     (32899, Patch.FuturesRewritten) // Grade 4 Skybuilders' Sunfish
            .Bait      (data);
        data.Apply     (32900, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Sweatfish
            .Bait      (data);
        data.Apply     (32901, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Sculptor
            .Bait      (data);
        data.Apply     (32902, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Little Thalaos
            .Bait      (data);
        data.Apply     (32903, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Lightning Chaser
            .Bait      (data);
        data.Apply     (32904, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Marrella
            .Bait      (data);
        data.Apply     (32905, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Crimson Namitaro
            .Bait      (data);
        data.Apply     (32906, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Griffin
            .Bait      (data);
        data.Apply     (32907, Patch.FuturesRewritten) // Grade 4 Artisanal Skybuilders' Meganeura
            .Bait      (data);
    }
    // @formatter:on
}
