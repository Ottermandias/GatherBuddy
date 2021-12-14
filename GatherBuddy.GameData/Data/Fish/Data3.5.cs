using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyTheFarEdgeOfFate(this GameData data)
    {
        data.Apply     (17562, Patch.TheFarEdgeOfFate) // Thavnairian Leaf
            .Bait      (data, 28634)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (17563, Patch.TheFarEdgeOfFate) // Ghost Faerie
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (17564, Patch.TheFarEdgeOfFate) // Red Sky Coral
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required);
        data.Apply     (17565, Patch.TheFarEdgeOfFate) // Lovers' Clam
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (17566, Patch.TheFarEdgeOfFate) // River Shrimp
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (17577, Patch.TheFarEdgeOfFate) // Bishopfish
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (600, 840)
            .Snag      (Snagging.None)
            .Weather   (data, 1);
        data.Apply     (17578, Patch.TheFarEdgeOfFate) // Captain Nemo
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Snag      (Snagging.None)
            .Transition(data, 15)
            .Weather   (data, 16);
        data.Apply     (17579, Patch.TheFarEdgeOfFate) // Paikiller
            .Bait      (data, 12707, 12730)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 720)
            .Snag      (Snagging.None)
            .Transition(data, 4)
            .Weather   (data, 1);
        data.Apply     (17580, Patch.TheFarEdgeOfFate) // Ceti
            .Bait      (data, 12710)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1080, 1320)
            .Snag      (Snagging.None)
            .Transition(data, 3)
            .Weather   (data, 9);
        data.Apply     (17581, Patch.TheFarEdgeOfFate) // Crystal Pigeon
            .Bait      (data, 12712, 12805)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Snag      (Snagging.None)
            .Transition(data, 2)
            .Weather   (data, 9);
        data.Apply     (17582, Patch.TheFarEdgeOfFate) // Thunderscale
            .Bait      (data, 12704, 12722)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (360, 480)
            .Snag      (Snagging.None)
            .Transition(data, 11, 3, 4)
            .Weather   (data, 9);
        data.Apply     (17583, Patch.TheFarEdgeOfFate) // Riddle
            .Bait      (data, 12709, 12754)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 960)
            .Snag      (Snagging.None)
            .Transition(data, 1)
            .Weather   (data, 1);
        data.Apply     (17584, Patch.TheFarEdgeOfFate) // The Lord of Lords
            .Bait      (data, 12709, 12754)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 960)
            .Snag      (Snagging.None)
            .Transition(data, 4, 11, 3)
            .Weather   (data, 1, 2);
        data.Apply     (17585, Patch.TheFarEdgeOfFate) // The Speaker
            .Bait      (data, 12704, 12757)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Snag      (Snagging.None)
            .Transition(data, 3, 4)
            .Weather   (data, 8);
        data.Apply     (17586, Patch.TheFarEdgeOfFate) // Thousand Fin
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Snag      (Snagging.None)
            .Transition(data, 3)
            .Weather   (data, 6);
        data.Apply     (17587, Patch.TheFarEdgeOfFate) // Bloodchaser
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Snag      (Snagging.None)
            .Transition(data, 1)
            .Weather   (data, 6);
        data.Apply     (17588, Patch.TheFarEdgeOfFate) // Problematicus
            .Bait      (data, 12709, 12754)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (600, 900)
            .Snag      (Snagging.None)
            .Predators (data, 180, (12800, 3), (12754, 5))
            .Weather   (data, 1, 2);
        data.Apply     (17589, Patch.TheFarEdgeOfFate) // Opabinia
            .Bait      (data, 12710, 12776)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Snag      (Snagging.None)
            .Predators (data, 105, (13727, 3))
            .Weather   (data, 9);
        data.Apply     (17590, Patch.TheFarEdgeOfFate) // Armor Fish
            .Bait      (data, 12705, 12757)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (60, 240)
            .Snag      (Snagging.None)
            .Predators (data, 160, (12757, 6))
            .Weather   (data, 1);
        data.Apply     (17591, Patch.TheFarEdgeOfFate) // Sea Butterfly
            .Bait      (data, 12712)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (300, 420)
            .Snag      (Snagging.None)
            .Predators (data, 60, (12810, 3), (12753, 3))
            .Weather   (data, 1);
        data.Apply     (17592, Patch.TheFarEdgeOfFate) // Charibenet
            .Bait      (data, 12705, 12715)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (0, 180)
            .Snag      (Snagging.None)
            .Predators (data, 120, (12715, 5))
            .Weather   (data, 16);
        data.Apply     (17593, Patch.TheFarEdgeOfFate) // Raimdellopterus
            .Bait      (data, 12712, 12805)
            .Bite      (HookSet.Precise, BiteType.Legendary)
            .Time      (300, 480)
            .Snag      (Snagging.None)
            .Predators (data, 100, (12805, 5))
            .Weather   (data, 6);
    }
    // @formatter:on
}
