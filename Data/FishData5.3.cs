using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyReflectionsInCrystal(this FishManager fish)
        {
            fish.Apply     (30432, Patch.ReflectionsInCrystal) // The Sinsteeped
                .Bait      (fish, 27582)
                .Tug       (BiteType.Legendary)
                .Uptime    (1320, 1440)
                .Weather   (1, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (30433, Patch.ReflectionsInCrystal) // Sweetheart
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .Transition(4)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (30434, Patch.ReflectionsInCrystal) // Giant Taimen
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (30435, Patch.ReflectionsInCrystal) // Leannisg
                .Bait      (fish, 27585)
                .Tug       (BiteType.Legendary)
                .Uptime    (360, 480)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (30436, Patch.ReflectionsInCrystal) // Gold Hammer
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .Transition(1, 2)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (30437, Patch.ReflectionsInCrystal) // Recordkiller
                .Bait      (fish, 27589)
                .Tug       (BiteType.Legendary)
                .Uptime    (1080, 1440)
                .Transition(1, 2)
                .Weather   (4)
                .HookType  (HookSet.Precise);
            fish.Apply     (30438, Patch.ReflectionsInCrystal) // The Mother of All Pancakes
                .Bait      (fish, 27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 180)
                .Transition(3)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (30439, Patch.ReflectionsInCrystal) // Opal Shrimp
                .Bait      (fish, 27590)
                .Tug       (BiteType.Legendary)
                .Uptime    (1080, 1200)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (30487, Patch.ReflectionsInCrystal) // Blue Crab
                .Bait      (fish, 27590)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (30488, Patch.ReflectionsInCrystal) // Hoplite
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (30489, Patch.ReflectionsInCrystal) // Hook Fish
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (30490, Patch.ReflectionsInCrystal) // Cloudweed
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (30491, Patch.ReflectionsInCrystal) // Fatty Herring
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (30492, Patch.ReflectionsInCrystal) // Inkshell
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (30593, Patch.ReflectionsInCrystal) // Fuchsia Bloom
                .Bait      (fish, 27587)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (31129, Patch.ReflectionsInCrystal) // Petal Shell
                .Bait      (fish);
            fish.Apply     (31134, Patch.ReflectionsInCrystal) // Allagan Hunter
                .Bait      (fish);
            fish.Apply     (31578, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Thunderbolt Sculpin
                .Bait      (fish);
            fish.Apply     (31579, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Alligator Snapping Turtle
                .Bait      (fish);
            fish.Apply     (31580, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Bonytongue
                .Bait      (fish);
            fish.Apply     (31581, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Hermit Goby
                .Bait      (fish);
            fish.Apply     (31582, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Mudskipper
                .Bait      (fish);
            fish.Apply     (31583, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Steppe Bullfrog
                .Bait      (fish);
            fish.Apply     (31584, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Golden Loach
                .Bait      (fish);
            fish.Apply     (31585, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Bass
                .Bait      (fish);
            fish.Apply     (31586, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Cherax
                .Bait      (fish);
            fish.Apply     (31587, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Marimo
                .Bait      (fish);
            fish.Apply     (31588, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Catfish
                .Bait      (fish);
            fish.Apply     (31589, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Ricefish
                .Bait      (fish);
            fish.Apply     (31590, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Scorpionfly
                .Bait      (fish);
            fish.Apply     (31591, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Whiteloom
                .Bait      (fish);
            fish.Apply     (31592, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Pteranodon
                .Bait      (fish);
            fish.Apply     (31593, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' King's Mantle
                .Bait      (fish);
            fish.Apply     (31594, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Blue Medusa
                .Bait      (fish);
            fish.Apply     (31595, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Gurnard
                .Bait      (fish);
            fish.Apply     (31596, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Oscar
                .Bait      (fish);
            fish.Apply     (31597, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Blind Manta
                .Bait      (fish);
            fish.Apply     (31598, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Mosasaur
                .Bait      (fish);
            fish.Apply     (31599, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Storm Chaser
                .Bait      (fish);
            fish.Apply     (31600, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Archaeopteryx
                .Bait      (fish);
            fish.Apply     (31601, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Wyvern
                .Bait      (fish);
            fish.Apply     (31602, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Cloudshark
                .Bait      (fish);
            fish.Apply     (31603, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Helicoprion
                .Bait      (fish);
        }
        // @formatter:on
    }
}
