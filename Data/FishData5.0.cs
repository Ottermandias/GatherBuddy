using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyShadowbringers(this FishManager fish)
        {
            fish.Apply     (26746, Patch.Shadowbringers) // Eighteyes Eel
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (26747, Patch.Shadowbringers) // Creamy Oyster
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (26748, Patch.Shadowbringers) // Brightmirror Clam
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (26749, Patch.Shadowbringers) // Amber Monkfish
                .Bait      (27584)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27410, Patch.Shadowbringers) // Crystarium Tetra
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27411, Patch.Shadowbringers) // Laxan Carp
                .Bait      (27582)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27412, Patch.Shadowbringers) // Asteroidea
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (27413, Patch.Shadowbringers) // Jacketed Snail
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27414, Patch.Shadowbringers) // Lover's Flower
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27415, Patch.Shadowbringers) // Sinspitter
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27416, Patch.Shadowbringers) // Wandering Catfish
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27417, Patch.Shadowbringers) // Crystal Knife
                .Bait      (27582)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27418, Patch.Shadowbringers) // Skeletonfish
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27419, Patch.Shadowbringers) // Milky Coral
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (27420, Patch.Shadowbringers) // Gravel Mussel
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (27421, Patch.Shadowbringers) // Crimson Sea Spider
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27422, Patch.Shadowbringers) // Bonefish
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27423, Patch.Shadowbringers) // Eulmore Butterfly
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27424, Patch.Shadowbringers) // Red Hammerhead
                .Bait      (27583)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27425, Patch.Shadowbringers) // Hard Candy
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27426, Patch.Shadowbringers) // Water Ball
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27427, Patch.Shadowbringers) // Abyssal Snail
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27428, Patch.Shadowbringers) // Clean Saucer
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27429, Patch.Shadowbringers) // Rabbit Skipper
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27430, Patch.Shadowbringers) // Xanthic Bass
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27431, Patch.Shadowbringers) // Albino Caiman
                .Bait      (27582)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27432, Patch.Shadowbringers) // Platinum Guppy
                .Bait      (27589)
                .Tug       (BiteType.Weak)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (27433, Patch.Shadowbringers) // Misty Killifish
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27434, Patch.Shadowbringers) // Albino Rock Crab
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27435, Patch.Shadowbringers) // Snakeskin Discus
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27436, Patch.Shadowbringers) // Albino Garfish
                .Bait      (27587)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27437, Patch.Shadowbringers) // Vicejaw
                .Bait      (27587)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27438, Patch.Shadowbringers) // Skulleater
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27439, Patch.Shadowbringers) // Toffee Snail
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27440, Patch.Shadowbringers) // Watts Trout
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27441, Patch.Shadowbringers) // Hucho Taimen
                .Bait      (27587)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27442, Patch.Shadowbringers) // Winged Hatchetfish
                .Bait      (27587)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27443, Patch.Shadowbringers) // Noble's Fan
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27444, Patch.Shadowbringers) // Zebra Catfish
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27445, Patch.Shadowbringers) // Sepia Sole
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27446, Patch.Shadowbringers) // Shellfracture Kelp
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27447, Patch.Shadowbringers) // Blood Cloud
                .Bait      (27583)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27448, Patch.Shadowbringers) // Kholusian Flounder
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27449, Patch.Shadowbringers) // South Kholusian Cod
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27450, Patch.Shadowbringers) // Razorfish
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27451, Patch.Shadowbringers) // Rose Shrimp
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27452, Patch.Shadowbringers) // Shapeshifter
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27453, Patch.Shadowbringers) // Minstrelfish
                .Bait      (27583)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27454, Patch.Shadowbringers) // Kholusian Wrasse
                .Bait      (27588, 27457)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27455, Patch.Shadowbringers) // Weedy Seadragon
                .Bait      (27583)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27456, Patch.Shadowbringers) // Henodus
                .Bait      (27588, 27457)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 24)
                .Weather   (4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27457, Patch.Shadowbringers) // Spearhead Squid
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27458, Patch.Shadowbringers) // Shadow Crab
                .Bait      (27584)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27459, Patch.Shadowbringers) // Desert Dustfish
                .Bait      (27584)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27460, Patch.Shadowbringers) // Hornhelm
                .Bait      (27584)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27461, Patch.Shadowbringers) // Web-footed Sand Gecko
                .Bait      (27584)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27462, Patch.Shadowbringers) // Sand Egg
                .Bait      (27584)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27463, Patch.Shadowbringers) // Amber Lamprey
                .Bait      (27584)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27464, Patch.Shadowbringers) // Desert Saw
                .Bait      (27586)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27465, Patch.Shadowbringers) // Garik Crab
                .Bait      (27584)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27466, Patch.Shadowbringers) // Nabaath Manta
                .Bait      (27584, 27461)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27467, Patch.Shadowbringers) // Thorned Lizard
                .Bait      (27584, 27461)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 18)
                .Weather   (14, 1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27468, Patch.Shadowbringers) // Spotted Blue-eye
                .Bait      (27585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27469, Patch.Shadowbringers) // Official Ball
                .Bait      (27585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27470, Patch.Shadowbringers) // Grey Skipper
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27471, Patch.Shadowbringers) // Pixie Fish
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27472, Patch.Shadowbringers) // Blood-eyed Frog
                .Bait      (27585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27473, Patch.Shadowbringers) // Cherry Herring
                .Bait      (27585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27474, Patch.Shadowbringers) // Wimple Carp
                .Bait      (27585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27475, Patch.Shadowbringers) // Lemonfish
                .Bait      (27585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27476, Patch.Shadowbringers) // Rebel
                .Bait      (27585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27477, Patch.Shadowbringers) // Wild Red Betta
                .Bait      (27585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27478, Patch.Shadowbringers) // Spotted Ctenopoma
                .Bait      (27585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27479, Patch.Shadowbringers) // Cerulean Loach
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27480, Patch.Shadowbringers) // Golden Lobster
                .Bait      (27587)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27481, Patch.Shadowbringers) // Toadhead
                .Bait      (27589)
                .Tug       (BiteType.Weak)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (27482, Patch.Shadowbringers) // Clawbow
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27483, Patch.Shadowbringers) // Black Tri-star
                .Bait      (27587)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27484, Patch.Shadowbringers) // Robber Crab
                .Bait      (27585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27485, Patch.Shadowbringers) // Darkroot
                .Bait      (27587)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27486, Patch.Shadowbringers) // Yellow Pipira
                .Bait      (27582)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27487, Patch.Shadowbringers) // Oathfish
                .Bait      (27585)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27488, Patch.Shadowbringers) // Night's Bass
                .Bait      (27587)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27489, Patch.Shadowbringers) // Rak'tika Trout
                .Bait      (27587)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27490, Patch.Shadowbringers) // Clown Tetra
                .Bait      (27587)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27491, Patch.Shadowbringers) // Eryops
                .Bait      (27587, 27490)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27492, Patch.Shadowbringers) // Diamond Pipira
                .Bait      (27587)
                .Tug       (BiteType.Strong)
                .Uptime    (12, 20)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27493, Patch.Shadowbringers) // Silver Kitten
                .Bait      (27587)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27494, Patch.Shadowbringers) // Darkdweller
                .Bait      (27589)
                .Tug       (BiteType.Weak)
                .Uptime    (0, 8)
                .Weather   (4)
                .HookType  (HookSet.Precise);
            fish.Apply     (27495, Patch.Shadowbringers) // Deep Purple Coral
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (27496, Patch.Shadowbringers) // Sycorax
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (27497, Patch.Shadowbringers) // Stormfish
                .Bait      (27583)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27498, Patch.Shadowbringers) // Azure Sea Spider
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27499, Patch.Shadowbringers) // Caliban
                .Bait      (27583)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27500, Patch.Shadowbringers) // Hoodwinker
                .Bait      (27588)
                .Tug       (BiteType.Strong)
                .Weather   (2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27501, Patch.Shadowbringers) // Bubble Angler
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27502, Patch.Shadowbringers) // Ondo Harpoon
                .Bait      (27588)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27503, Patch.Shadowbringers) // Ondobane
                .Bait      (27588)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27504, Patch.Shadowbringers) // Stippled Eel
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27505, Patch.Shadowbringers) // Seatrap
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27506, Patch.Shadowbringers) // Ancient Shrimp
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27507, Patch.Shadowbringers) // Yeti Crab
                .Bait      (27588, 27506)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27508, Patch.Shadowbringers) // Aapoak
                .Bait      (27588, 27506)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 16)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27509, Patch.Shadowbringers) // Cyan Sea Devil
                .Bait      (27588)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27510, Patch.Shadowbringers) // Pancake Octopus
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27511, Patch.Shadowbringers) // Maneater Clam
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (27512, Patch.Shadowbringers) // Stargazer
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27513, Patch.Shadowbringers) // Sweetflesh Oyster
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (27514, Patch.Shadowbringers) // Rainbow Shrimp
                .Bait      (27588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (27515, Patch.Shadowbringers) // Predator
                .Bait      (27588)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (27516, Patch.Shadowbringers) // Grey Carp
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27517, Patch.Shadowbringers) // Lilac Goby
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27518, Patch.Shadowbringers) // Purple Ghost
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27519, Patch.Shadowbringers) // Lakelouse
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27520, Patch.Shadowbringers) // Gazing Glass
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27521, Patch.Shadowbringers) // Source Octopus
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27522, Patch.Shadowbringers) // Elven Spear
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27523, Patch.Shadowbringers) // Shade Gudgeon
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27524, Patch.Shadowbringers) // Lakethistle
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27525, Patch.Shadowbringers) // Platinum Bream
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27526, Patch.Shadowbringers) // Wardenfish
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27527, Patch.Shadowbringers) // Finned Eggplant
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27528, Patch.Shadowbringers) // Skykisser
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27529, Patch.Shadowbringers) // Viola Clam
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27530, Patch.Shadowbringers) // Geayi
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27531, Patch.Shadowbringers) // Noblefish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27532, Patch.Shadowbringers) // Misteye
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27533, Patch.Shadowbringers) // Lakeland Cod
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27534, Patch.Shadowbringers) // Little Bismarck
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .Predators ((27531, 10))
                .HookType  (HookSet.None);
            fish.Apply     (27535, Patch.Shadowbringers) // Bothriolepis
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .Predators ((27531, 10))
                .HookType  (HookSet.None);
            fish.Apply     (27536, Patch.Shadowbringers) // Big-eye
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27537, Patch.Shadowbringers) // Jenanna's Tear
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27538, Patch.Shadowbringers) // Daisy Turban
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27539, Patch.Shadowbringers) // Shade Axolotl
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27540, Patch.Shadowbringers) // Little Flirt
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27541, Patch.Shadowbringers) // Peallaidh
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27542, Patch.Shadowbringers) // Voeburt Bichir
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27543, Patch.Shadowbringers) // Poecilia
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27544, Patch.Shadowbringers) // Gilded Batfish
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27545, Patch.Shadowbringers) // Petalfish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27546, Patch.Shadowbringers) // Mirrorfish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27547, Patch.Shadowbringers) // Glass Eel
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27548, Patch.Shadowbringers) // Voeburt Salamander
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27549, Patch.Shadowbringers) // Bedskipper
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27550, Patch.Shadowbringers) // Dandyfish
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27551, Patch.Shadowbringers) // Sauldia Ruby
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27552, Patch.Shadowbringers) // Mock Pixie
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27553, Patch.Shadowbringers) // Hunter's Arrow
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27554, Patch.Shadowbringers) // Fuathgobbler
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27555, Patch.Shadowbringers) // Paradise Crab
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27556, Patch.Shadowbringers) // Saint Fathric's Ire
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27557, Patch.Shadowbringers) // Queensgown
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27558, Patch.Shadowbringers) // Flowering Kelpie
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27559, Patch.Shadowbringers) // Ghoulfish
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27560, Patch.Shadowbringers) // Measan Deala
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27561, Patch.Shadowbringers) // Dohn Horn
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27562, Patch.Shadowbringers) // Aquabloom
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27563, Patch.Shadowbringers) // Jester Fish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27564, Patch.Shadowbringers) // Blue Lightning
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27565, Patch.Shadowbringers) // Maidenhair
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27566, Patch.Shadowbringers) // Blue Mountain Bubble
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .Predators ((27551, 10))
                .HookType  (HookSet.None);
            fish.Apply     (27567, Patch.Shadowbringers) // Elder Pixie
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .Predators ((27551, 10))
                .HookType  (HookSet.None);
            fish.Apply     (27568, Patch.Shadowbringers) // Ankle Snipper
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27569, Patch.Shadowbringers) // Treescale
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27570, Patch.Shadowbringers) // Ronkan Pleco
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27571, Patch.Shadowbringers) // Gourmand Crab
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27572, Patch.Shadowbringers) // Gatorl's Bead
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27573, Patch.Shadowbringers) // Diamondtongue
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27574, Patch.Shadowbringers) // Hermit's Hood
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27575, Patch.Shadowbringers) // Hermit Crab
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27576, Patch.Shadowbringers) // Megapiranha
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27577, Patch.Shadowbringers) // Everdark Bass
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27578, Patch.Shadowbringers) // Lozatl Pirarucu
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27579, Patch.Shadowbringers) // Anpa's Handmaid
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (27580, Patch.Shadowbringers) // Viis Ear
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .Predators ((27569, 10))
                .HookType  (HookSet.None);
            fish.Apply     (27581, Patch.Shadowbringers) // Rak'tika Goby
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .Predators ((27569, 10))
                .HookType  (HookSet.None);
        }
        // @formatter:on
    }
}
