using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyEndwalker(this GameData data)
    {
        data.Apply     (35604, Patch.Endwalker) // Giant Aetherlouse
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (35605, Patch.Endwalker) // Garjana Wrasse
            .Bait      (data, 36592)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (35606, Patch.Endwalker) // Garlean Clam
            .Bait      (data, 36589)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (35607, Patch.Endwalker) // Smaragdos
            .Bait      (data, 36590)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36385, Patch.Endwalker) // Pecten
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required);
        data.Apply     (36386, Patch.Endwalker) // Northern Herring
            .Bait      (data, 36592)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36387, Patch.Endwalker) // Dog-faced Puffer
            .Bait      (data, 36592)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36388, Patch.Endwalker) // Cobalt Chromis
            .Bait      (data, 36592)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36389, Patch.Endwalker) // Guitarfish
            .Bait      (data, 36592)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36390, Patch.Endwalker) // Astacus
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36392, Patch.Endwalker) // Peacock Bass
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36393, Patch.Endwalker) // Academician
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36394, Patch.Endwalker) // Swordspine Snook
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36395, Patch.Endwalker) // Ponderer
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36396, Patch.Endwalker) // Tidal Dahlia
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36397, Patch.Endwalker) // Butterfly Fry
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36398, Patch.Endwalker) // Xenocypris
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36399, Patch.Endwalker) // Topminnow
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36400, Patch.Endwalker) // Tessera
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36402, Patch.Endwalker) // Fat Snook
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36403, Patch.Endwalker) // Prochilodus Luminosus
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36404, Patch.Endwalker) // Mesonauta
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36405, Patch.Endwalker) // Greengill Salmon
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36407, Patch.Endwalker) // Raiamas
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36408, Patch.Endwalker) // Red Bowfin
            .Bait      (data, 36590)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36409, Patch.Endwalker) // Macrobrachium Lar
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36410, Patch.Endwalker) // Blowgun
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36411, Patch.Endwalker) // Darksteel Knifefish
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36412, Patch.Endwalker) // Astacus Aetherius
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36414, Patch.Endwalker) // Labyrinthos Tilapia
            .Bait      (data, 36589, 36412)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (480, 960)
            .Weather   (data, 2);
        data.Apply     (36415, Patch.Endwalker) // Trunkblessed
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36417, Patch.Endwalker) // Seema Duta
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36418, Patch.Endwalker) // Longear Sunfish
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36419, Patch.Endwalker) // Silver Characin
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36420, Patch.Endwalker) // Thavnairian Goby
            .Bait      (data, 36592)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36421, Patch.Endwalker) // Qeyiq Sole
            .Bait      (data, 36592)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36422, Patch.Endwalker) // Gwl Crab
            .Bait      (data, 36592)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36423, Patch.Endwalker) // Pantherscale Grouper
            .Bait      (data, 36592)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36425, Patch.Endwalker) // Fate's Design
            .Bait      (data, 36592)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36426, Patch.Endwalker) // Shadowdart Sardine
            .Bait      (data, 36592)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36427, Patch.Endwalker) // Paksa Fish
            .Bait      (data, 36592)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36430, Patch.Endwalker) // Golden Barramundi
            .Bait      (data, 36592)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36431, Patch.Endwalker) // Kadjaya's Castaway
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36432, Patch.Endwalker) // Marid Frog
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36434, Patch.Endwalker) // Bluegill
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36435, Patch.Endwalker) // Bronze Pipira
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36436, Patch.Endwalker) // Green Swordtail
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36438, Patch.Endwalker) // Ksirapayin
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36439, Patch.Endwalker) // Wakeful Watcher
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36440, Patch.Endwalker) // Red Drum
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (960, 1440)
            .Transition(data, 8, 3)
            .Weather   (data, 2);
        data.Apply     (36441, Patch.Endwalker) // Forgeflame
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36442, Patch.Endwalker) // Bicuda
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36443, Patch.Endwalker) // Radzbalik
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36444, Patch.Endwalker) // Half-moon Betta
            .Bait      (data, 36589)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36446, Patch.Endwalker) // Banana Eel
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36447, Patch.Endwalker) // Handy Hamsa
            .Bait      (data, 36589)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36448, Patch.Endwalker) // Flowerhorn
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36449, Patch.Endwalker) // Thavnairian Caiman
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36450, Patch.Endwalker) // Fiery Goby
            .Bait      (data, 36592)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36451, Patch.Endwalker) // Puff-paya
            .Bait      (data, 36593)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36452, Patch.Endwalker) // Narunnairian Octopus
            .Bait      (data, 36592)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36453, Patch.Endwalker) // Roosterfish
            .Bait      (data, 36593)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36454, Patch.Endwalker) // Basilosaurus
            .Bait      (data, 36593, 36451)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 2);
        data.Apply     (36456, Patch.Endwalker) // Eblan Trout
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36457, Patch.Endwalker) // Animulus
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36458, Patch.Endwalker) // Cerule Core
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36459, Patch.Endwalker) // Icepike
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36460, Patch.Endwalker) // Dark Crown
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36461, Patch.Endwalker) // Imperial Pleco
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36462, Patch.Endwalker) // Bluetail
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36463, Patch.Endwalker) // Star-blue Guppy
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36465, Patch.Endwalker) // Lunar Cichlid
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36466, Patch.Endwalker) // Teareye
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36467, Patch.Endwalker) // Replipirarucu
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36468, Patch.Endwalker) // Feverfish
            .Bait      (data, 36594)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36470, Patch.Endwalker) // Calicia
            .Bait      (data, 36594)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36471, Patch.Endwalker) // Protomyke #987
            .Bait      (data, 36594)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36472, Patch.Endwalker) // Lunar Deathworm
            .Bait      (data, 36594, 36470)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 2);
        data.Apply     (36473, Patch.Endwalker) // Fleeting Brand
            .Bait      (data, 36594)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36475, Patch.Endwalker) // Regotoise
            .Bait      (data, 36594)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36476, Patch.Endwalker) // Isle Skipper
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36477, Patch.Endwalker) // Iribainion
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36478, Patch.Endwalker) // Albino Loach
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36479, Patch.Endwalker) // Golden Shiner
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36480, Patch.Endwalker) // Mangar
            .Bait      (data, 36588, 36478)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 3);
        data.Apply     (36481, Patch.Endwalker) // Dermogenys
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36484, Patch.Endwalker) // Antheia
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36485, Patch.Endwalker) // Colossoma
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36487, Patch.Endwalker) // Superstring
            .Bait      (data, 36594)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36488, Patch.Endwalker) // Star Eater
            .Bait      (data, 36594)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36489, Patch.Endwalker) // Vacuum Shrimp
            .Bait      (data, 36594)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36491, Patch.Endwalker) // Cosmic Noise
            .Bait      (data, 36594)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36492, Patch.Endwalker) // Glassfish
            .Bait      (data, 36594)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36494, Patch.Endwalker) // Foun Myhk
            .Bait      (data, 36594)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36495, Patch.Endwalker) // Dragonscale
            .Bait      (data, 36594)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36496, Patch.Endwalker) // Ypupîara
            .Bait      (data, 36594)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36497, Patch.Endwalker) // Eehs Forhnesh
            .Bait      (data, 36594)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36499, Patch.Endwalker) // Katoptron
            .Bait      (data, 36588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36501, Patch.Endwalker) // Comet Tail
            .Bait      (data, 36589)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36502, Patch.Endwalker) // Aoide
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36503, Patch.Endwalker) // Protoflesh
            .Bait      (data, 36588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36505, Patch.Endwalker) // Wandering Starscale
            .Bait      (data, 36589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36506, Patch.Endwalker) // Wormhole Worm
            .Bait      (data, 36596)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36507, Patch.Endwalker) // Unidentified Flying Biomass II
            .Bait      (data, 36596)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36508, Patch.Endwalker) // Triaina
            .Bait      (data, 36596)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36509, Patch.Endwalker) // Sophos Deka-okto
            .Bait      (data, 36596)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36510, Patch.Endwalker) // Class Twenty-four
            .Bait      (data, 36596)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36511, Patch.Endwalker) // Terrifyingway
            .Bait      (data, 36596)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36512, Patch.Endwalker) // Alien Mertone
            .Bait      (data, 36596)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36513, Patch.Endwalker) // Monster Carrot
            .Bait      (data, 36597)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36514, Patch.Endwalker) // Argonaut
            .Bait      (data, 36596)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (36515, Patch.Endwalker) // Echinos
            .Bait      (data, 36596)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36516, Patch.Endwalker) // Space Bishop
            .Bait      (data, 36596)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36517, Patch.Endwalker) // Alyketos
            .Bait      (data, 36597)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36518, Patch.Endwalker) // Horizon Event
            .Bait      (data, 36596)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36519, Patch.Endwalker) // E.B.E.-9318
            .Bait      (data, 36596)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36520, Patch.Endwalker) // Unbegotten
            .Bait      (data, 36596, 36518)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (36521, Patch.Endwalker) // Phallaina 
            .Bait      (data, 36596, 36518)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1320, 240);
        data.Apply     (36522, Patch.Endwalker) // Thavnairian Cucumber
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (36523, Patch.Endwalker) // Spiny King Crab
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (36524, Patch.Endwalker) // Thavnairian Eel
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (36525, Patch.Endwalker) // Gilled Topknot
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Fast);
        data.Apply     (36526, Patch.Endwalker) // Purusa Fish
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (36527, Patch.Endwalker) // Giantsgall Jaw
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply     (36528, Patch.Endwalker) // Akyaali Sardine
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply     (36529, Patch.Endwalker) // Spicy Pickle
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (36530, Patch.Endwalker) // Mayavahana
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (36531, Patch.Endwalker) // Hedonfish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (36532, Patch.Endwalker) // Satrap Trapfish
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply     (36533, Patch.Endwalker) // Blue Marlin
            .Spear     (SpearfishSize.Large, SpearfishSpeed.HyperFast);
        data.Apply     (36534, Patch.Endwalker) // Satrap's Whisper
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (36535, Patch.Endwalker) // Tebqeyiq Smelt
            .Spear     (SpearfishSize.Small, SpearfishSpeed.SuperFast)
            .Predators (data, 0, (36531, 10), (36546, 2), (36547, 3));
        data.Apply     (36536, Patch.Endwalker) // Shallows Cod
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (36537, Patch.Endwalker) // Meyhane Reveler
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply     (36538, Patch.Endwalker) // Daemir's Alloy
            .Spear     (SpearfishSize.Large, SpearfishSpeed.ExtremelyFast);
        data.Apply     (36539, Patch.Endwalker) // Rasa Fish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (36540, Patch.Endwalker) // Agama's Palm
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (36541, Patch.Endwalker) // Rummy-nosed Tetra
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average);
        data.Apply     (36542, Patch.Endwalker) // Monksblade
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (36543, Patch.Endwalker) // Atamra Cichlid
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Fast);
        data.Apply     (36544, Patch.Endwalker) // Root of Maya
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow);
        data.Apply     (36545, Patch.Endwalker) // Floral Snakehead
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (36546, Patch.Endwalker) // Xiphactinus
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average)
            .Predators (data, 0, (36531, 10));
        data.Apply     (36547, Patch.Endwalker) // Dusky Shark
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply     (36548, Patch.Endwalker) // Coffer Shell
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (36549, Patch.Endwalker) // Onihige
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average);
        data.Apply     (36550, Patch.Endwalker) // Onokoro Carp
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (36551, Patch.Endwalker) // Ruby-spotted Crab
            .Spear     (SpearfishSize.Average, SpearfishSpeed.ExtremelySlow);
        data.Apply     (36552, Patch.Endwalker) // Marrow-eater
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (36553, Patch.Endwalker) // Cloudy Catshark
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply     (36554, Patch.Endwalker) // Red Gurnard
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (36555, Patch.Endwalker) // Dream Pickle
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (36556, Patch.Endwalker) // Ruby Haddock
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (36557, Patch.Endwalker) // Crown Fish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Fast);
        data.Apply     (36558, Patch.Endwalker) // Sword of Isari 
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow);
        data.Apply     (36559, Patch.Endwalker) // Blue Shark
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (36560, Patch.Endwalker) // Barb of Exile
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply     (36561, Patch.Endwalker) // Smooth Lumpfish
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (36562, Patch.Endwalker) // Hells' Cap
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (36563, Patch.Endwalker) // Keeled Fugu
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (36564, Patch.Endwalker) // Eastern Seerfish
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (36565, Patch.Endwalker) // False Fusilier
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow);
        data.Apply     (36566, Patch.Endwalker) // Skipping Stone
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (36567, Patch.Endwalker) // Red-spotted Blenny
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Fast);
        data.Apply     (36568, Patch.Endwalker) // Othardian Wrasse
            .Spear     (SpearfishSize.Average, SpearfishSpeed.ExtremelyFast);
        data.Apply     (36569, Patch.Endwalker) // Grey Mullet
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (36570, Patch.Endwalker) // Prayer Cushion
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply     (36571, Patch.Endwalker) // Deepbody Boarfish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (36572, Patch.Endwalker) // Jointed Razorfish
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply     (36573, Patch.Endwalker) // Pipefish
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Fast)
            .Predators (data, 0, (36553, 10));
        data.Apply     (36574, Patch.Endwalker) // Righteye Flounder
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (36575, Patch.Endwalker) // Mini Yasha
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average);
        data.Apply     (36576, Patch.Endwalker) // Sawshark
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply     (36577, Patch.Endwalker) // Othardian Lumpsucker
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (36578, Patch.Endwalker) // Shogun's Kabuto
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow)
            .Predators (data, 0, (36553, 10));
        data.Apply     (36579, Patch.Endwalker) // Bluefin Trevally
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply     (36580, Patch.Endwalker) // Kitefin Shark
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply     (36581, Patch.Endwalker) // Uzumaki
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (36582, Patch.Endwalker) // Natron Puffer
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (36583, Patch.Endwalker) // Diamond Dagger
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VeryFast);
        data.Apply     (36584, Patch.Endwalker) // Queenly Fan
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (36585, Patch.Endwalker) // Pale Panther
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (36586, Patch.Endwalker) // Saltsquid
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply     (36587, Patch.Endwalker) // Platinum Hammerhead
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VeryFast);
    }
    // @formatter:on
}
