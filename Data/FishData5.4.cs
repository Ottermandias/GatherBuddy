using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyFuturesRewritten(this FishManager fish)
        {
            fish.Apply     (31770, Patch.FuturesRewritten) // Flintstrike
                .Bait      (fish);
            fish.Apply     (31771, Patch.FuturesRewritten) // Pickled Pom
                .Bait      (fish, 30136)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (32049, Patch.FuturesRewritten) // Moonlight Guppy
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (6, 8)
                .Transition(1)
                .Weather   (2)
                .HookType  (HookSet.Precise);
            fish.Apply     (32050, Patch.FuturesRewritten) // Steel Fan
                .Bait      (fish, 27585)
                .Tug       (BiteType.Legendary)
                .Transition(3)
                .Weather   (4)
                .HookType  (HookSet.Precise);
            fish.Apply     (32051, Patch.FuturesRewritten) // Henodus Grandis
                .Bait      (fish, 27588, 27457)
                .Tug       (BiteType.Legendary)
                .Uptime    (22, 24)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (32052, Patch.FuturesRewritten) // Sunken Tome
                .Bait      (fish, 27585)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 16)
                .Transition(2)
                .Weather   (10)
                .HookType  (HookSet.Precise);
            fish.Apply     (32053, Patch.FuturesRewritten) // Pearl Pipira
                .Bait      (fish, 27587, 27492)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 20)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (32054, Patch.FuturesRewritten) // The Ondotaker
                .Bait      (fish, 27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 11)
                .Transition(1)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (32055, Patch.FuturesRewritten) // Tortoiseshell Crab
                .Bait      (fish);
            fish.Apply     (32056, Patch.FuturesRewritten) // Lady's Cameo
                .Bait      (fish);
            fish.Apply     (32057, Patch.FuturesRewritten) // Metallic Boxfish
                .Bait      (fish);
            fish.Apply     (32058, Patch.FuturesRewritten) // Goobbue Ray
                .Bait      (fish);
            fish.Apply     (32059, Patch.FuturesRewritten) // Watermoura
                .Bait      (fish);
            fish.Apply     (32060, Patch.FuturesRewritten) // King Cobrafish
                .Bait      (fish);
            fish.Apply     (32061, Patch.FuturesRewritten) // Mamahi-mahi
                .Bait      (fish);
            fish.Apply     (32062, Patch.FuturesRewritten) // Lavandin Remora
                .Bait      (fish);
            fish.Apply     (32063, Patch.FuturesRewritten) // Spectral Butterfly
                .Bait      (fish);
            fish.Apply     (32064, Patch.FuturesRewritten) // Cieldalaes Geode
                .Bait      (fish);
            fish.Apply     (32065, Patch.FuturesRewritten) // Titanshell Crab
                .Bait      (fish);
            fish.Apply     (32066, Patch.FuturesRewritten) // Mythril Boxfish
                .Bait      (fish);
            fish.Apply     (32067, Patch.FuturesRewritten) // Mistbeard's Cup
                .Bait      (fish);
            fish.Apply     (32068, Patch.FuturesRewritten) // Anomalocaris Saron
                .Bait      (fish);
            fish.Apply     (32069, Patch.FuturesRewritten) // Flaming Eel
                .Bait      (fish);
            fish.Apply     (32070, Patch.FuturesRewritten) // Jetborne Manta
                .Bait      (fish);
            fish.Apply     (32071, Patch.FuturesRewritten) // Devil's Sting
                .Bait      (fish);
            fish.Apply     (32072, Patch.FuturesRewritten) // Callichthyid
                .Bait      (fish);
            fish.Apply     (32073, Patch.FuturesRewritten) // Meandering Mora
                .Bait      (fish);
            fish.Apply     (32074, Patch.FuturesRewritten) // Hafgufa
                .Bait      (fish);
            fish.Apply     (32075, Patch.FuturesRewritten) // Thaliak Crab
                .Bait      (fish);
            fish.Apply     (32076, Patch.FuturesRewritten) // Star of the Destroyer
                .Bait      (fish);
            fish.Apply     (32077, Patch.FuturesRewritten) // True Scad
                .Bait      (fish);
            fish.Apply     (32078, Patch.FuturesRewritten) // Blooded Wrasse
                .Bait      (fish);
            fish.Apply     (32079, Patch.FuturesRewritten) // Bloodpolish Crab
                .Bait      (fish);
            fish.Apply     (32080, Patch.FuturesRewritten) // Blue Stitcher
                .Bait      (fish);
            fish.Apply     (32081, Patch.FuturesRewritten) // Bloodfresh Tuna
                .Bait      (fish);
            fish.Apply     (32082, Patch.FuturesRewritten) // Sunken Mask
                .Bait      (fish);
            fish.Apply     (32083, Patch.FuturesRewritten) // Spectral Eel
                .Bait      (fish);
            fish.Apply     (32084, Patch.FuturesRewritten) // Bareface
                .Bait      (fish);
            fish.Apply     (32085, Patch.FuturesRewritten) // Oracular Crab
                .Bait      (fish);
            fish.Apply     (32086, Patch.FuturesRewritten) // Dravanian Bream
                .Bait      (fish);
            fish.Apply     (32087, Patch.FuturesRewritten) // Skaldminni
                .Bait      (fish);
            fish.Apply     (32088, Patch.FuturesRewritten) // Serrated Clam
                .Bait      (fish);
            fish.Apply     (32089, Patch.FuturesRewritten) // Beatific Vision
                .Bait      (fish);
            fish.Apply     (32090, Patch.FuturesRewritten) // Exterminator
                .Bait      (fish);
            fish.Apply     (32091, Patch.FuturesRewritten) // Gory Tuna
                .Bait      (fish);
            fish.Apply     (32092, Patch.FuturesRewritten) // Ticinepomis
                .Bait      (fish);
            fish.Apply     (32093, Patch.FuturesRewritten) // Quartz Hammerhead
                .Bait      (fish);
            fish.Apply     (32094, Patch.FuturesRewritten) // Seafaring Toad
                .Bait      (fish);
            fish.Apply     (32095, Patch.FuturesRewritten) // Crow Puffer
                .Bait      (fish);
            fish.Apply     (32096, Patch.FuturesRewritten) // Rothlyt Kelp
                .Bait      (fish);
            fish.Apply     (32097, Patch.FuturesRewritten) // Living Lantern
                .Bait      (fish);
            fish.Apply     (32098, Patch.FuturesRewritten) // Honeycomb Fish
                .Bait      (fish);
            fish.Apply     (32099, Patch.FuturesRewritten) // Godsbed
                .Bait      (fish);
            fish.Apply     (32100, Patch.FuturesRewritten) // Lansquenet
                .Bait      (fish);
            fish.Apply     (32101, Patch.FuturesRewritten) // Thavnairian Shark
                .Bait      (fish);
            fish.Apply     (32102, Patch.FuturesRewritten) // Nephrite Eel
                .Bait      (fish);
            fish.Apply     (32103, Patch.FuturesRewritten) // Spectresaur
                .Bait      (fish);
            fish.Apply     (32104, Patch.FuturesRewritten) // Ginkgo Fin
                .Bait      (fish);
            fish.Apply     (32105, Patch.FuturesRewritten) // Garum Jug
                .Bait      (fish);
            fish.Apply     (32106, Patch.FuturesRewritten) // Smooth Jaguar
                .Bait      (fish);
            fish.Apply     (32107, Patch.FuturesRewritten) // Rothlyt Mussel
                .Bait      (fish);
            fish.Apply     (32108, Patch.FuturesRewritten) // Levi Elver
                .Bait      (fish);
            fish.Apply     (32109, Patch.FuturesRewritten) // Pearl Bombfish
                .Bait      (fish);
            fish.Apply     (32110, Patch.FuturesRewritten) // Trollfish
                .Bait      (fish);
            fish.Apply     (32111, Patch.FuturesRewritten) // Panoptes
                .Bait      (fish);
            fish.Apply     (32112, Patch.FuturesRewritten) // Crepe Sole
                .Bait      (fish);
            fish.Apply     (32113, Patch.FuturesRewritten) // Knifejaw
                .Bait      (fish);
            fish.Apply     (32114, Patch.FuturesRewritten) // Placodus
                .Bait      (fish);
        }
        // @formatter:on
    }
}
