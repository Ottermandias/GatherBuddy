using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyTheFarEdgeOfFate(this FishManager fish)
        {
            fish.Apply     (17562, Patch.TheFarEdgeOfFate) // Thavnairian Leaf
                .Bait      (12704);
            fish.Apply     (17563, Patch.TheFarEdgeOfFate) // Ghost Faerie
                .Bait      (28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (17564, Patch.TheFarEdgeOfFate) // Red Sky Coral
                .Bait      (12707)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (17565, Patch.TheFarEdgeOfFate) // Lovers' Clam
                .Bait      (12707)
                .Tug       (BiteType.Weak);
            fish.Apply     (17566, Patch.TheFarEdgeOfFate) // River Shrimp
                .Bait      (12711)
                .Tug       (BiteType.Weak);
            fish.Apply     (17577, Patch.TheFarEdgeOfFate) // Bishopfish
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 14)
                .Snag      (Snagging.None)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17578, Patch.TheFarEdgeOfFate) // Captain Nemo
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Transition(15)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17579, Patch.TheFarEdgeOfFate) // Paikiller
                .Bait      (12707, 12730)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 12)
                .Snag      (Snagging.None)
                .Transition(4)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17580, Patch.TheFarEdgeOfFate) // Ceti
                .Bait      (12710)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 22)
                .Snag      (Snagging.None)
                .Transition(3)
                .Weather   (9)
                .HookType  (HookSet.Precise);
            fish.Apply     (17581, Patch.TheFarEdgeOfFate) // Crystal Pigeon
                .Bait      (12712, 12805)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Transition(2)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17582, Patch.TheFarEdgeOfFate) // Thunderscale
                .Bait      (12704, 12722)
                .Tug       (BiteType.Legendary)
                .Uptime    (6, 8)
                .Snag      (Snagging.None)
                .Transition(11, 3, 4)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17583, Patch.TheFarEdgeOfFate) // Riddle
                .Bait      (12709, 12754)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 16)
                .Snag      (Snagging.None)
                .Transition(1)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17584, Patch.TheFarEdgeOfFate) // The Lord of Lords
                .Bait      (12709, 12754)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 16)
                .Snag      (Snagging.None)
                .Transition(4, 11, 3)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17585, Patch.TheFarEdgeOfFate) // The Speaker
                .Bait      (12704, 12757)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Transition(3, 4)
                .Weather   (8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17586, Patch.TheFarEdgeOfFate) // Thousand Fin
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Transition(3)
                .Weather   (6)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17587, Patch.TheFarEdgeOfFate) // Bloodchaser
                .Bait      (12712)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Transition(1)
                .Weather   (6)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17588, Patch.TheFarEdgeOfFate) // Problematicus
                .Bait      (12709, 12754)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 15)
                .Snag      (Snagging.None)
                .Predators ((12800, 3), (12754, 5))
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17589, Patch.TheFarEdgeOfFate) // Opabinia
                .Bait      (12710, 12776)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Predators ((13727, 3))
                .Weather   (9)
                .HookType  (HookSet.Precise);
            fish.Apply     (17590, Patch.TheFarEdgeOfFate) // Armor Fish
                .Bait      (12705, 12757)
                .Tug       (BiteType.Legendary)
                .Uptime    (1, 4)
                .Snag      (Snagging.None)
                .Predators ((12757, 6))
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (17591, Patch.TheFarEdgeOfFate) // Sea Butterfly
                .Bait      (12712)
                .Tug       (BiteType.Legendary)
                .Uptime    (5, 7)
                .Snag      (Snagging.None)
                .Predators ((12810, 3), (12753, 3))
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (17592, Patch.TheFarEdgeOfFate) // Charibenet
                .Bait      (12705, 12715)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 3)
                .Snag      (Snagging.None)
                .Predators ((12715, 5))
                .Weather   (16)
                .HookType  (HookSet.Precise);
            fish.Apply     (17593, Patch.TheFarEdgeOfFate) // Raimdellopterus
                .Bait      (12712, 12805)
                .Tug       (BiteType.Legendary)
                .Uptime    (5, 8)
                .Snag      (Snagging.None)
                .Predators ((12805, 5))
                .Weather   (6)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
