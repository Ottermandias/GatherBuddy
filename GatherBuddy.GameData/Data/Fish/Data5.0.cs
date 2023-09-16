using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyShadowbringers(this GameData data)
    {
        data.Apply     (26746, Patch.Shadowbringers) // Eighteyes Eel
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (26747, Patch.Shadowbringers) // Creamy Oyster
            .Bait      (data, 27583)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (26748, Patch.Shadowbringers) // Brightmirror Clam
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (26749, Patch.Shadowbringers) // Amber Monkfish
            .Bait      (data, 27584)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27410, Patch.Shadowbringers) // Crystarium Tetra
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27411, Patch.Shadowbringers) // Laxan Carp
            .Bait      (data, 27582)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27412, Patch.Shadowbringers) // Asteroidea
            .Bait      (data, 27587)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (27413, Patch.Shadowbringers) // Jacketed Snail
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27414, Patch.Shadowbringers) // Lover's Flower
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27415, Patch.Shadowbringers) // Sinspitter
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27416, Patch.Shadowbringers) // Wandering Catfish
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27417, Patch.Shadowbringers) // Crystal Knife
            .Bait      (data, 27582)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27418, Patch.Shadowbringers) // Skeletonfish
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27419, Patch.Shadowbringers) // Milky Coral
            .Bait      (data, 27583)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (27420, Patch.Shadowbringers) // Gravel Mussel
            .Bait      (data, 27583)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (27421, Patch.Shadowbringers) // Crimson Sea Spider
            .Bait      (data, 27583)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27422, Patch.Shadowbringers) // Bonefish
            .Bait      (data, 27583)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27423, Patch.Shadowbringers) // Eulmore Butterfly
            .Bait      (data, 27583)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27424, Patch.Shadowbringers) // Red Hammerhead
            .Bait      (data, 27590)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27425, Patch.Shadowbringers) // Hard Candy
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27426, Patch.Shadowbringers) // Water Ball
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27427, Patch.Shadowbringers) // Abyssal Snail
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27428, Patch.Shadowbringers) // Clean Saucer
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27429, Patch.Shadowbringers) // Rabbit Skipper
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27430, Patch.Shadowbringers) // Xanthic Bass
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27431, Patch.Shadowbringers) // Albino Caiman
            .Bait      (data, 27587)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27432, Patch.Shadowbringers) // Platinum Guppy
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Weather   (data, 1);
        data.Apply     (27433, Patch.Shadowbringers) // Misty Killifish
            .Bait      (data, 27587)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27434, Patch.Shadowbringers) // Albino Rock Crab
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27435, Patch.Shadowbringers) // Snakeskin Discus
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27436, Patch.Shadowbringers) // Albino Garfish
            .Bait      (data, 27585)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27437, Patch.Shadowbringers) // Vicejaw
            .Bait      (data, 27589)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (27438, Patch.Shadowbringers) // Skulleater
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27439, Patch.Shadowbringers) // Toffee Snail
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27440, Patch.Shadowbringers) // Watts Trout
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27441, Patch.Shadowbringers) // Hucho Taimen
            .Bait      (data, 27589)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27442, Patch.Shadowbringers) // Winged Hatchetfish
            .Bait      (data, 27587)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27443, Patch.Shadowbringers) // Noble's Fan
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27444, Patch.Shadowbringers) // Zebra Catfish
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27445, Patch.Shadowbringers) // Sepia Sole
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27446, Patch.Shadowbringers) // Shellfracture Kelp
            .Bait      (data, 27583)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27447, Patch.Shadowbringers) // Blood Cloud
            .Bait      (data, 27583)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27448, Patch.Shadowbringers) // Kholusian Flounder
            .Bait      (data, 27583)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27449, Patch.Shadowbringers) // South Kholusian Cod
            .Bait      (data, 27583)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27450, Patch.Shadowbringers) // Razorfish
            .Bait      (data, 27588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27451, Patch.Shadowbringers) // Rose Shrimp
            .Bait      (data, 27588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27452, Patch.Shadowbringers) // Shapeshifter
            .Bait      (data, 27583)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27453, Patch.Shadowbringers) // Minstrelfish
            .Bait      (data, 27588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27454, Patch.Shadowbringers) // Kholusian Wrasse
            .Mooch     (data, 27588, 27457)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27455, Patch.Shadowbringers) // Weedy Seadragon
            .Bait      (data, 27588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27456, Patch.Shadowbringers) // Henodus
            .Mooch     (data, 27588, 27457)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 1440)
            .Weather   (data, 4, 3);
        data.Apply     (27457, Patch.Shadowbringers) // Spearhead Squid
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27458, Patch.Shadowbringers) // Shadow Crab
            .Bait      (data, 27584)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27459, Patch.Shadowbringers) // Desert Dustfish
            .Bait      (data, 27584)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27460, Patch.Shadowbringers) // Hornhelm
            .Bait      (data, 27584)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27461, Patch.Shadowbringers) // Web-footed Sand Gecko
            .Bait      (data, 27584)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27462, Patch.Shadowbringers) // Sand Egg
            .Bait      (data, 27584)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27463, Patch.Shadowbringers) // Amber Lamprey
            .Bait      (data, 27584)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27464, Patch.Shadowbringers) // Desert Saw
            .Bait      (data, 27586)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (27465, Patch.Shadowbringers) // Garik Crab
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27466, Patch.Shadowbringers) // Nabaath Manta
            .Mooch     (data, 27584, 27461)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27467, Patch.Shadowbringers) // Thorned Lizard
            .Mooch     (data, 27584, 27461)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (600, 1080)
            .Weather   (data, 14, 1, 2);
        data.Apply     (27468, Patch.Shadowbringers) // Spotted Blue-eye
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27469, Patch.Shadowbringers) // Official Ball
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27470, Patch.Shadowbringers) // Grey Skipper
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27471, Patch.Shadowbringers) // Pixie Fish
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27472, Patch.Shadowbringers) // Blood-eyed Frog
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27473, Patch.Shadowbringers) // Cherry Herring
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27474, Patch.Shadowbringers) // Wimple Carp
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27475, Patch.Shadowbringers) // Lemonfish
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27476, Patch.Shadowbringers) // Rebel
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27477, Patch.Shadowbringers) // Wild Red Betta
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27478, Patch.Shadowbringers) // Spotted Ctenopoma
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27479, Patch.Shadowbringers) // Cerulean Loach
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27480, Patch.Shadowbringers) // Golden Lobster
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27481, Patch.Shadowbringers) // Toadhead
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Weather   (data, 1);
        data.Apply     (27482, Patch.Shadowbringers) // Clawbow
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27483, Patch.Shadowbringers) // Black Tri-star
            .Bait      (data, 27587)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27484, Patch.Shadowbringers) // Robber Crab
            .Bait      (data, 27585)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27485, Patch.Shadowbringers) // Darkroot
            .Bait      (data, 27587)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27486, Patch.Shadowbringers) // Yellow Pipira
            .Bait      (data, 27582)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27487, Patch.Shadowbringers) // Oathfish
            .Bait      (data, 27585)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27488, Patch.Shadowbringers) // Night's Bass
            .Bait      (data, 27587)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27489, Patch.Shadowbringers) // Rak'tika Trout
            .Bait      (data, 27587)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27490, Patch.Shadowbringers) // Clown Tetra
            .Bait      (data, 27587)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27491, Patch.Shadowbringers) // Eryops
            .Mooch     (data, 27587, 27490)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27492, Patch.Shadowbringers) // Diamond Pipira
            .Bait      (data, 27587)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (720, 1200);
        data.Apply     (27493, Patch.Shadowbringers) // Silver Kitten
            .Bait      (data, 27587)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27494, Patch.Shadowbringers) // Darkdweller
            .Bait      (data, 27589)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (0, 480)
            .Weather   (data, 4);
        data.Apply     (27495, Patch.Shadowbringers) // Deep Purple Coral
            .Bait      (data, 27588)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (27496, Patch.Shadowbringers) // Sycorax
            .Bait      (data, 27588)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (27497, Patch.Shadowbringers) // Stormfish
            .Bait      (data, 27583)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27498, Patch.Shadowbringers) // Azure Sea Spider
            .Bait      (data, 27588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27499, Patch.Shadowbringers) // Caliban
            .Bait      (data, 27583)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27500, Patch.Shadowbringers) // Hoodwinker
            .Bait      (data, 27588)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 2);
        data.Apply     (27501, Patch.Shadowbringers) // Bubble Angler
            .Bait      (data, 27590)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27502, Patch.Shadowbringers) // Ondo Harpoon
            .Bait      (data, 27590)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27503, Patch.Shadowbringers) // Ondobane
            .Bait      (data, 27590)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27504, Patch.Shadowbringers) // Stippled Eel
            .Bait      (data, 27588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27505, Patch.Shadowbringers) // Seatrap
            .Bait      (data, 27590)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27506, Patch.Shadowbringers) // Ancient Shrimp
            .Bait      (data, 27588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27507, Patch.Shadowbringers) // Yeti Crab
            .Mooch     (data, 27588, 27506)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27508, Patch.Shadowbringers) // Aapoak
            .Mooch     (data, 27588, 27506)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (720, 960)
            .Weather   (data, 1);
        data.Apply     (27509, Patch.Shadowbringers) // Cyan Sea Devil
            .Bait      (data, 27588)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27510, Patch.Shadowbringers) // Pancake Octopus
            .Bait      (data, 27590)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27511, Patch.Shadowbringers) // Maneater Clam
            .Bait      (data, 27588)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (27512, Patch.Shadowbringers) // Stargazer
            .Bait      (data, 27590)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27513, Patch.Shadowbringers) // Sweetflesh Oyster
            .Bait      (data, 27588)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (27514, Patch.Shadowbringers) // Rainbow Shrimp
            .Bait      (data, 27588)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (27515, Patch.Shadowbringers) // Predator
            .Bait      (data, 27590)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (27516, Patch.Shadowbringers) // Grey Carp
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (27517, Patch.Shadowbringers) // Lilac Goby
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (27518, Patch.Shadowbringers) // Purple Ghost
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average);
        data.Apply     (27519, Patch.Shadowbringers) // Lakelouse
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (27520, Patch.Shadowbringers) // Gazing Glass
            .Spear     (SpearfishSize.Average, SpearfishSpeed.ExtremelySlow);
        data.Apply     (27521, Patch.Shadowbringers) // Source Octopus
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow);
        data.Apply     (27522, Patch.Shadowbringers) // Elven Spear
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply     (27523, Patch.Shadowbringers) // Shade Gudgeon
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average);
        data.Apply     (27524, Patch.Shadowbringers) // Lakethistle
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (27525, Patch.Shadowbringers) // Platinum Bream
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (27526, Patch.Shadowbringers) // Wardenfish
            .Spear     (SpearfishSize.Large, SpearfishSpeed.ExtremelySlow);
        data.Apply     (27527, Patch.Shadowbringers) // Finned Eggplant
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply     (27528, Patch.Shadowbringers) // Skykisser
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (27529, Patch.Shadowbringers) // Viola Clam
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (27530, Patch.Shadowbringers) // Geayi
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (27531, Patch.Shadowbringers) // Noblefish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (27532, Patch.Shadowbringers) // Misteye
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply     (27533, Patch.Shadowbringers) // Lakeland Cod
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow);
        data.Apply     (27534, Patch.Shadowbringers) // Little Bismarck
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VeryFast)
            .Predators (data, 0, (27531, 7));
        data.Apply     (27535, Patch.Shadowbringers) // Bothriolepis
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow)
            .Predators (data, 0, (27531, 7));
        data.Apply     (27536, Patch.Shadowbringers) // Big-eye
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Fast);
        data.Apply     (27537, Patch.Shadowbringers) // Jenanna's Tear
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (27538, Patch.Shadowbringers) // Daisy Turban
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (27539, Patch.Shadowbringers) // Shade Axolotl
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (27540, Patch.Shadowbringers) // Little Flirt
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (27541, Patch.Shadowbringers) // Peallaidh
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow);
        data.Apply     (27542, Patch.Shadowbringers) // Voeburt Bichir
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply     (27543, Patch.Shadowbringers) // Poecilia
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (27544, Patch.Shadowbringers) // Gilded Batfish
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average);
        data.Apply     (27545, Patch.Shadowbringers) // Petalfish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Fast);
        data.Apply     (27546, Patch.Shadowbringers) // Mirrorfish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (27547, Patch.Shadowbringers) // Glass Eel
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (27548, Patch.Shadowbringers) // Voeburt Salamander
            .Spear     (SpearfishSize.Large, SpearfishSpeed.ExtremelySlow);
        data.Apply     (27549, Patch.Shadowbringers) // Bedskipper
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Fast);
        data.Apply     (27550, Patch.Shadowbringers) // Dandyfish
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (27551, Patch.Shadowbringers) // Sauldia Ruby
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (27552, Patch.Shadowbringers) // Mock Pixie
            .Spear     (SpearfishSize.Average, SpearfishSpeed.ExtremelySlow);
        data.Apply     (27553, Patch.Shadowbringers) // Hunter's Arrow
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow);
        data.Apply     (27554, Patch.Shadowbringers) // Fuathgobbler
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (27555, Patch.Shadowbringers) // Paradise Crab
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (27556, Patch.Shadowbringers) // Saint Fathric's Ire
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (27557, Patch.Shadowbringers) // Queensgown
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (27558, Patch.Shadowbringers) // Flowering Kelpie
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (27559, Patch.Shadowbringers) // Ghoulfish
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply     (27560, Patch.Shadowbringers) // Measan Deala
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply     (27561, Patch.Shadowbringers) // Dohn Horn
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (27562, Patch.Shadowbringers) // Aquabloom
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (27563, Patch.Shadowbringers) // Jester Fish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (27564, Patch.Shadowbringers) // Blue Lightning
            .Spear     (SpearfishSize.Large, SpearfishSpeed.ExtremelyFast);
        data.Apply     (27565, Patch.Shadowbringers) // Maidenhair
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (27566, Patch.Shadowbringers) // Blue Mountain Bubble
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Fast)
            .Predators (data, 0, (27551, 7));
        data.Apply     (27567, Patch.Shadowbringers) // Elder Pixie
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow)
            .Predators (data, 0, (27551, 7));
        data.Apply     (27568, Patch.Shadowbringers) // Ankle Snipper
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (27569, Patch.Shadowbringers) // Treescale
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (27570, Patch.Shadowbringers) // Ronkan Pleco
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (27571, Patch.Shadowbringers) // Gourmand Crab
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (27572, Patch.Shadowbringers) // Gatorl's Bead
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply     (27573, Patch.Shadowbringers) // Diamondtongue
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply     (27574, Patch.Shadowbringers) // Hermit's Hood
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (27575, Patch.Shadowbringers) // Hermit Crab
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (27576, Patch.Shadowbringers) // Megapiranha
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Fast);
        data.Apply     (27577, Patch.Shadowbringers) // Everdark Bass
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (27578, Patch.Shadowbringers) // Lozatl Pirarucu
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow);
        data.Apply     (27579, Patch.Shadowbringers) // Anpa's Handmaid
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (27580, Patch.Shadowbringers) // Viis Ear
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VeryFast)
            .Predators (data, 0, (27569, 10));
        data.Apply     (27581, Patch.Shadowbringers) // Rak'tika Goby
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average)
            .Predators (data, 0, (27569, 10));
    }
    // @formatter:on
}
