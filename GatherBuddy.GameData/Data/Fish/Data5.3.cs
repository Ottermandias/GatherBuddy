using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyReflectionsInCrystal(this GameData data)
    {
        data.Apply     (30432, Patch.ReflectionsInCrystal) // The Sinsteeped
            .Bait      (data, 27582)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Time      (1320, 1440)
            .Weather   (data, 1, 2);
        data.Apply     (30433, Patch.ReflectionsInCrystal) // Sweetheart
            .Bait      (data, 27589)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 4)
            .Weather   (data, 1, 2);
        data.Apply     (30434, Patch.ReflectionsInCrystal) // Giant Taimen
            .Bait      (data, 27589)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (30435, Patch.ReflectionsInCrystal) // Leannisg
            .Bait      (data, 27585)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (360, 480)
            .Weather   (data, 7);
        data.Apply     (30436, Patch.ReflectionsInCrystal) // Gold Hammer
            .Bait      (data, 27589)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Transition(data, 1, 2)
            .Weather   (data, 1);
        data.Apply     (30437, Patch.ReflectionsInCrystal) // Recordkiller
            .Bait      (data, 27589)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Time      (1080, 1440)
            .Transition(data, 1, 2)
            .Weather   (data, 4);
        data.Apply     (30438, Patch.ReflectionsInCrystal) // The Mother of All Pancakes
            .Bait      (data, 27590)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Time      (0, 180)
            .Transition(data, 3)
            .Weather   (data, 1);
        data.Apply     (30439, Patch.ReflectionsInCrystal) // Opal Shrimp
            .Bait      (data, 27590)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Time      (1080, 1200)
            .Weather   (data, 1);
        data.Apply     (30487, Patch.ReflectionsInCrystal) // Blue Crab
            .Bait      (data, 27590)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (30488, Patch.ReflectionsInCrystal) // Hoplite
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (30489, Patch.ReflectionsInCrystal) // Hook Fish
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (30490, Patch.ReflectionsInCrystal) // Cloudweed
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (30491, Patch.ReflectionsInCrystal) // Fatty Herring
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (30492, Patch.ReflectionsInCrystal) // Inkshell
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (30593, Patch.ReflectionsInCrystal) // Fuchsia Bloom
            .Bait      (data, 27587)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (31129, Patch.ReflectionsInCrystal) // Petal Shell
            .Bait      (data, 30136)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .ForceBig  (false);
        data.Apply     (31134, Patch.ReflectionsInCrystal) // Allagan Hunter
            .Bait      (data, 30136)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .ForceBig  (false);
        data.Apply     (31578, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Thunderbolt Sculpin
            .Bait      (data);
        data.Apply     (31579, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Alligator Snapping Turtle
            .Bait      (data);
        data.Apply     (31580, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Bonytongue
            .Bait      (data);
        data.Apply     (31581, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Hermit Goby
            .Bait      (data);
        data.Apply     (31582, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Mudskipper
            .Bait      (data);
        data.Apply     (31583, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Steppe Bullfrog
            .Bait      (data);
        data.Apply     (31584, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Golden Loach
            .Bait      (data);
        data.Apply     (31585, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Bass
            .Bait      (data);
        data.Apply     (31586, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Cherax
            .Bait      (data);
        data.Apply     (31587, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Marimo
            .Bait      (data);
        data.Apply     (31588, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Catfish
            .Bait      (data);
        data.Apply     (31589, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Ricefish
            .Bait      (data);
        data.Apply     (31590, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Scorpionfly
            .Bait      (data);
        data.Apply     (31591, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Whiteloom
            .Bait      (data);
        data.Apply     (31592, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Pteranodon
            .Bait      (data);
        data.Apply     (31593, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' King's Mantle
            .Bait      (data);
        data.Apply     (31594, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Blue Medusa
            .Bait      (data);
        data.Apply     (31595, Patch.ReflectionsInCrystal) // Grade 3 Skybuilders' Gurnard
            .Bait      (data);
        data.Apply     (31596, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Oscar
            .Bait      (data, 30278)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (31597, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Blind Manta
            .Bait      (data, 30279)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (31598, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Mosasaur
            .Mooch     (data, 32894)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);  
        data.Apply     (31599, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Storm Chaser
            .Mooch     (data, 32894)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (31600, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Archaeopteryx
            .Bait      (data, 30280)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Weather   (data, 136);
        data.Apply     (31601, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Wyvern
            .Mooch     (data, 32894)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 133);
        data.Apply     (31602, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Cloudshark
            .Bait      (data, 30279)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 135);
        data.Apply     (31603, Patch.ReflectionsInCrystal) // Grade 3 Artisanal Skybuilders' Helicoprion
            .Mooch     (data, 32894)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 134);
    }
    // @formatter:on
}
