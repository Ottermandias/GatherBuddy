using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyEndwalker(this FishManager fish)
        {
            fish.Apply     (35604, Patch.Endwalker) // Giant Aetherlouse
                .Bait      (fish, 36588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (35605, Patch.Endwalker) // Garjana Wrasse
                .Bait      (fish, 36592)
                .Tug       (BiteType.Strong);
            fish.Apply     (35606, Patch.Endwalker) // Garlean Clam
                .Bait      (fish, 36589)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (35607, Patch.Endwalker) // Smaragdos
                .Bait      (fish)
                .Tug       (BiteType.Strong);
            fish.Apply     (36385, Patch.Endwalker) // Pecten
                .Bait      (fish, 36592)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required);
            fish.Apply     (36386, Patch.Endwalker) // Northern Herring
                .Bait      (fish, 36592)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (36387, Patch.Endwalker) // Dog-faced Puffer
                .Bait      (fish, 36592)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (36388, Patch.Endwalker) // Cobalt Chromis
                .Bait      (fish, 36592)
                .Tug       (BiteType.Strong);
            fish.Apply     (36389, Patch.Endwalker) // Guitarfish
                .Bait      (fish, 36592)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (36390, Patch.Endwalker) // Astacus
                .Bait      (fish, 36588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (36392, Patch.Endwalker) // Peacock Bass
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong);
            fish.Apply     (36393, Patch.Endwalker) // Academician
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong);
            fish.Apply     (36394, Patch.Endwalker) // Swordspine Snook
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong);
            fish.Apply     (36395, Patch.Endwalker) // Ponderer
                .Bait      (fish, 36588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (36396, Patch.Endwalker) // Tidal Dahlia
                .Bait      (fish, 36588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (36397, Patch.Endwalker) // Butterfly Fry
                .Bait      (fish, 36588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (36398, Patch.Endwalker) // Xenocypris
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (36399, Patch.Endwalker) // Topminnow
                .Bait      (fish, 36588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (36400, Patch.Endwalker) // Tessera
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (36402, Patch.Endwalker) // Fat Snook
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (36403, Patch.Endwalker) // Prochilodus Luminosus
                .Bait      (fish, 36589)
                .Tug       (BiteType.Weak);
            fish.Apply     (36404, Patch.Endwalker) // Mesonauta
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong);
            fish.Apply     (36405, Patch.Endwalker) // Greengill Salmon
                .Bait      (fish);
            fish.Apply     (36407, Patch.Endwalker) // Raiamas
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong);
            fish.Apply     (36408, Patch.Endwalker) // Red Bowfin
                .Bait      (fish);
            fish.Apply     (36409, Patch.Endwalker) // Macrobrachium Lar
                .Bait      (fish, 36588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (36410, Patch.Endwalker) // Blowgun
                .Bait      (fish, 36588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (36411, Patch.Endwalker) // Darksteel Knifefish
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong);
            fish.Apply     (36412, Patch.Endwalker) // Astacus Aetherius
                .Bait      (fish);
            fish.Apply     (36414, Patch.Endwalker) // Labyrinthos Tilapia
                .Bait      (fish);
            fish.Apply     (36415, Patch.Endwalker) // Trunkblessed
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (36417, Patch.Endwalker) // Seema Duta
                .Bait      (fish);
            fish.Apply     (36418, Patch.Endwalker) // Longear Sunfish
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong);
            fish.Apply     (36419, Patch.Endwalker) // Silver Characin
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong);
            fish.Apply     (36420, Patch.Endwalker) // Thavnairian Goby
                .Bait      (fish, 36592)
                .Tug       (BiteType.Weak);
            fish.Apply     (36421, Patch.Endwalker) // Qeyiq Sole
                .Bait      (fish, 36592)
                .Tug       (BiteType.Weak);
            fish.Apply     (36422, Patch.Endwalker) // Gwl Crab
                .Bait      (fish, 36592)
                .Tug       (BiteType.Strong);
            fish.Apply     (36423, Patch.Endwalker) // Pantherscale Grouper
                .Bait      (fish, 36592)
                .Tug       (BiteType.Strong);
            fish.Apply     (36425, Patch.Endwalker) // Fate's Design
                .Bait      (fish, 36592)
                .Tug       (BiteType.Strong);
            fish.Apply     (36426, Patch.Endwalker) // Shadowdart Sardine
                .Bait      (fish, 36592)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (36427, Patch.Endwalker) // Paksa Fish
                .Bait      (fish, 36592)
                .Tug       (BiteType.Strong);
            fish.Apply     (36430, Patch.Endwalker) // Golden Barramundi
                .Bait      (fish)
                .Tug       (BiteType.Strong);
            fish.Apply     (36431, Patch.Endwalker) // Kadjaya's Castaway
                .Bait      (fish, 36589)
                .Tug       (BiteType.Weak);
            fish.Apply     (36432, Patch.Endwalker) // Marid Frog
                .Bait      (fish, 36589)
                .Tug       (BiteType.Strong);
            fish.Apply     (36434, Patch.Endwalker) // Bluegill
                .Bait      (fish)
                .Tug       (BiteType.Strong);
            fish.Apply     (36435, Patch.Endwalker) // Bronze Pipira
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong);
            fish.Apply     (36436, Patch.Endwalker) // Green Swordtail
                .Bait      (fish);
            fish.Apply     (36438, Patch.Endwalker) // Ksirapayin
                .Bait      (fish);
            fish.Apply     (36439, Patch.Endwalker) // Wakeful Watcher
                .Bait      (fish);
            fish.Apply     (36440, Patch.Endwalker) // Red Drum
                .Bait      (fish);
            fish.Apply     (36441, Patch.Endwalker) // Forgeflame
                .Bait      (fish);
            fish.Apply     (36442, Patch.Endwalker) // Bicuda
                .Bait      (fish);
            fish.Apply     (36443, Patch.Endwalker) // Radzbalik
                .Bait      (fish);
            fish.Apply     (36444, Patch.Endwalker) // Half-moon Betta
                .Bait      (fish);
            fish.Apply     (36446, Patch.Endwalker) // Banana Eel
                .Bait      (fish);
            fish.Apply     (36447, Patch.Endwalker) // Handy Hamsa
                .Bait      (fish);
            fish.Apply     (36448, Patch.Endwalker) // Flowerhorn
                .Bait      (fish);
            fish.Apply     (36449, Patch.Endwalker) // Thavnairian Caiman
                .Bait      (fish);
            fish.Apply     (36450, Patch.Endwalker) // Fiery Goby
                .Bait      (fish);
            fish.Apply     (36451, Patch.Endwalker) // Puff-paya
                .Bait      (fish);
            fish.Apply     (36452, Patch.Endwalker) // Narunnairian Octopus
                .Bait      (fish);
            fish.Apply     (36453, Patch.Endwalker) // Roosterfish
                .Bait      (fish);
            fish.Apply     (36454, Patch.Endwalker) // Basilosaurus
                .Bait      (fish);
            fish.Apply     (36456, Patch.Endwalker) // Eblan Trout
                .Bait      (fish, 36588)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (36457, Patch.Endwalker) // Animulus
                .Bait      (fish, 36588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (36458, Patch.Endwalker) // Cerule Core
                .Bait      (fish);
            fish.Apply     (36459, Patch.Endwalker) // Icepike
                .Bait      (fish, 36589)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (36460, Patch.Endwalker) // Dark Crown
                .Bait      (fish);
            fish.Apply     (36461, Patch.Endwalker) // Imperial Pleco
                .Bait      (fish);
            fish.Apply     (36462, Patch.Endwalker) // Bluetail
                .Bait      (fish);
            fish.Apply     (36463, Patch.Endwalker) // Star-blue Guppy
                .Bait      (fish);
            fish.Apply     (36465, Patch.Endwalker) // Lunar Cichlid
                .Bait      (fish);
            fish.Apply     (36466, Patch.Endwalker) // Teareye
                .Bait      (fish);
            fish.Apply     (36467, Patch.Endwalker) // Replipirarucu
                .Bait      (fish);
            fish.Apply     (36468, Patch.Endwalker) // Feverfish
                .Bait      (fish);
            fish.Apply     (36470, Patch.Endwalker) // Calicia
                .Bait      (fish);
            fish.Apply     (36471, Patch.Endwalker) // Protomyke #987
                .Bait      (fish)
                .Tug       (BiteType.Strong);
            fish.Apply     (36472, Patch.Endwalker) // Lunar Deathworm
                .Bait      (fish);
            fish.Apply     (36473, Patch.Endwalker) // Fleeting Brand
                .Bait      (fish);
            fish.Apply     (36475, Patch.Endwalker) // Regotoise
                .Bait      (fish);
            fish.Apply     (36476, Patch.Endwalker) // Isle Skipper
                .Bait      (fish)
                .Tug       (BiteType.Weak);
            fish.Apply     (36477, Patch.Endwalker) // Iribainion
                .Bait      (fish)
                .Tug       (BiteType.Strong);
            fish.Apply     (36478, Patch.Endwalker) // Albino Loach
                .Bait      (fish);
            fish.Apply     (36479, Patch.Endwalker) // Golden Shiner
                .Bait      (fish);
            fish.Apply     (36480, Patch.Endwalker) // Mangar
                .Bait      (fish);
            fish.Apply     (36481, Patch.Endwalker) // Dermogenys
                .Bait      (fish);
            fish.Apply     (36484, Patch.Endwalker) // Antheia
                .Bait      (fish)
                .Tug       (BiteType.Weak);
            fish.Apply     (36485, Patch.Endwalker) // Colossoma
                .Bait      (fish)
                .Tug       (BiteType.Strong);
            fish.Apply     (36487, Patch.Endwalker) // Superstring
                .Bait      (fish);
            fish.Apply     (36488, Patch.Endwalker) // Star Eater
                .Bait      (fish);
            fish.Apply     (36489, Patch.Endwalker) // Vacuum Shrimp
                .Bait      (fish);
            fish.Apply     (36491, Patch.Endwalker) // Cosmic Noise
                .Bait      (fish);
            fish.Apply     (36492, Patch.Endwalker) // Glassfish
                .Bait      (fish);
            fish.Apply     (36494, Patch.Endwalker) // Foun Myhk
                .Bait      (fish);
            fish.Apply     (36495, Patch.Endwalker) // Dragonscale
                .Bait      (fish);
            fish.Apply     (36496, Patch.Endwalker) // Ypupîara
                .Bait      (fish);
            fish.Apply     (36497, Patch.Endwalker) // Eehs Forhnesh
                .Bait      (fish);
            fish.Apply     (36499, Patch.Endwalker) // Katoptron
                .Bait      (fish);
            fish.Apply     (36501, Patch.Endwalker) // Comet Tail
                .Bait      (fish);
            fish.Apply     (36502, Patch.Endwalker) // Aoide
                .Bait      (fish);
            fish.Apply     (36503, Patch.Endwalker) // Protoflesh
                .Bait      (fish);
            fish.Apply     (36505, Patch.Endwalker) // Wandering Starscale
                .Bait      (fish);
            fish.Apply     (36506, Patch.Endwalker) // Wormhole Worm
                .Bait      (fish);
            fish.Apply     (36507, Patch.Endwalker) // Unidentified Flying Biomass II
                .Bait      (fish);
            fish.Apply     (36508, Patch.Endwalker) // Triaina
                .Bait      (fish);
            fish.Apply     (36509, Patch.Endwalker) // Sophos Deka-okto
                .Bait      (fish);
            fish.Apply     (36510, Patch.Endwalker) // Class Twenty-four
                .Bait      (fish);
            fish.Apply     (36511, Patch.Endwalker) // Terrifyingway
                .Bait      (fish);
            fish.Apply     (36512, Patch.Endwalker) // Alien Mertone
                .Bait      (fish);
            fish.Apply     (36513, Patch.Endwalker) // Monster Carrot
                .Bait      (fish);
            fish.Apply     (36514, Patch.Endwalker) // Argonaut
                .Bait      (fish);
            fish.Apply     (36515, Patch.Endwalker) // Echinos
                .Bait      (fish);
            fish.Apply     (36516, Patch.Endwalker) // Space Bishop
                .Bait      (fish);
            fish.Apply     (36517, Patch.Endwalker) // Alyketos
                .Bait      (fish);
            fish.Apply     (36518, Patch.Endwalker) // Horizon Event
                .Bait      (fish);
            fish.Apply     (36519, Patch.Endwalker) // E.B.E.-9318
                .Bait      (fish);
            fish.Apply     (36520, Patch.Endwalker) // Unbegotten
                .Bait      (fish);
            fish.Apply     (36521, Patch.Endwalker) // Phallaina 
                .Bait      (fish);
            fish.Apply     (36522, Patch.Endwalker) // Thavnairian Cucumber
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36523, Patch.Endwalker) // Spiny King Crab
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36524, Patch.Endwalker) // Thavnairian Eel
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36525, Patch.Endwalker) // Gilled Topknot
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36526, Patch.Endwalker) // Purusa Fish
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36527, Patch.Endwalker) // Giantsgall Jaw
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36528, Patch.Endwalker) // Akyaali Sardine
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36529, Patch.Endwalker) // Spicy Pickle
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36530, Patch.Endwalker) // Mayavahana
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36531, Patch.Endwalker) // Hedonfish
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36532, Patch.Endwalker) // Satrap Trapfish
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36533, Patch.Endwalker) // Blue Marlin
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36534, Patch.Endwalker) // Satrap's Whisper
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36535, Patch.Endwalker) // Tebqeyiq Smelt
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36536, Patch.Endwalker) // Shallows Cod
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36537, Patch.Endwalker) // Meyhane Reveler
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36538, Patch.Endwalker) // Daemir's Alloy
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36539, Patch.Endwalker) // Rasa Fish
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36540, Patch.Endwalker) // Agama's Palm
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36541, Patch.Endwalker) // Rummy-nosed Tetra
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36542, Patch.Endwalker) // Monksblade
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36543, Patch.Endwalker) // Atamra Cichlid
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36544, Patch.Endwalker) // Root of Maya
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36545, Patch.Endwalker) // Floral Snakehead
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36546, Patch.Endwalker) // Xiphactinus
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36547, Patch.Endwalker) // Dusky Shark
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36548, Patch.Endwalker) // Coffer Shell
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36549, Patch.Endwalker) // Onihige
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36550, Patch.Endwalker) // Onokoro Carp
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36551, Patch.Endwalker) // Ruby-spotted Crab
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36552, Patch.Endwalker) // Marrow-eater
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36553, Patch.Endwalker) // Cloudy Catshark
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36554, Patch.Endwalker) // Red Gurnard
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36555, Patch.Endwalker) // Dream Pickle
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36556, Patch.Endwalker) // Ruby Haddock
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36557, Patch.Endwalker) // Crown Fish
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36558, Patch.Endwalker) // Sword of Isari 
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36559, Patch.Endwalker) // Blue Shark
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36560, Patch.Endwalker) // Barb of Exile
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36561, Patch.Endwalker) // Smooth Lumpfish
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36562, Patch.Endwalker) // Hells' Cap
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36563, Patch.Endwalker) // Keeled Fugu
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36564, Patch.Endwalker) // Eastern Seerfish
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36565, Patch.Endwalker) // False Fusilier
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36566, Patch.Endwalker) // Skipping Stone
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36567, Patch.Endwalker) // Red-spotted Blenny
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36568, Patch.Endwalker) // Othardian Wrasse
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36569, Patch.Endwalker) // Grey Mullet
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36570, Patch.Endwalker) // Prayer Cushion
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36571, Patch.Endwalker) // Deepbody Boarfish
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36572, Patch.Endwalker) // Jointed Razorfish
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36573, Patch.Endwalker) // Pipefish
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36574, Patch.Endwalker) // Righteye Flounder
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36575, Patch.Endwalker) // Mini Yasha
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36576, Patch.Endwalker) // Sawshark
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36577, Patch.Endwalker) // Othardian Lumpsucker
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36578, Patch.Endwalker) // Shogun's Kabuto
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36579, Patch.Endwalker) // Bluefin Trevally
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36580, Patch.Endwalker) // Kitefin Shark
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36581, Patch.Endwalker) // Uzumaki
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36582, Patch.Endwalker) // Natron Puffer
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36583, Patch.Endwalker) // Diamond Dagger
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36584, Patch.Endwalker) // Queenly Fan
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36585, Patch.Endwalker) // Pale Panther
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36586, Patch.Endwalker) // Saltsquid
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (36587, Patch.Endwalker) // Platinum Hammerhead
                .Gig       (GigHead.Unknown)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
        }
        // @formatter:on
    }
}
