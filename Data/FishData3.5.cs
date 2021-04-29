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
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17563, Patch.TheFarEdgeOfFate) // Ghost Faerie
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (17564, Patch.TheFarEdgeOfFate) // Red Sky Coral
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (17565, Patch.TheFarEdgeOfFate) // Lovers' Clam
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (17566, Patch.TheFarEdgeOfFate) // River Shrimp
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (17577, Patch.TheFarEdgeOfFate) // Bishopfish
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 14)
                .Snag      (Snagging.None)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17578, Patch.TheFarEdgeOfFate) // Captain Nemo
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Transition(15)
                .Weather   (16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17579, Patch.TheFarEdgeOfFate) // Paikiller
                .Bait      (fish, 12707, 12730)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 12)
                .Snag      (Snagging.None)
                .Transition(4)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17580, Patch.TheFarEdgeOfFate) // Ceti
                .Bait      (fish, 12710)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 22)
                .Snag      (Snagging.None)
                .Transition(3)
                .Weather   (9)
                .HookType  (HookSet.Precise);
            fish.Apply     (17581, Patch.TheFarEdgeOfFate) // Crystal Pigeon
                .Bait      (fish, 12712, 12805)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Transition(2)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17582, Patch.TheFarEdgeOfFate) // Thunderscale
                .Bait      (fish, 12704, 12722)
                .Tug       (BiteType.Legendary)
                .Uptime    (6, 8)
                .Snag      (Snagging.None)
                .Transition(11, 3, 4)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17583, Patch.TheFarEdgeOfFate) // Riddle
                .Bait      (fish, 12709, 12754)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 16)
                .Snag      (Snagging.None)
                .Transition(1)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17584, Patch.TheFarEdgeOfFate) // The Lord of Lords
                .Bait      (fish, 12709, 12754)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 16)
                .Snag      (Snagging.None)
                .Transition(4, 11, 3)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17585, Patch.TheFarEdgeOfFate) // The Speaker
                .Bait      (fish, 12704, 12757)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Transition(3, 4)
                .Weather   (8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17586, Patch.TheFarEdgeOfFate) // Thousand Fin
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Transition(3)
                .Weather   (6)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17587, Patch.TheFarEdgeOfFate) // Bloodchaser
                .Bait      (fish, 12712)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Transition(1)
                .Weather   (6)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17588, Patch.TheFarEdgeOfFate) // Problematicus
                .Bait      (fish, 12709, 12754)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 15)
                .Snag      (Snagging.None)
                .Predators (fish, (12800, 3), (12754, 5))
                .Intuition (180)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (17589, Patch.TheFarEdgeOfFate) // Opabinia
                .Bait      (fish, 12710, 12776)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .Predators (fish, (13727, 3))
                .Intuition (105)
                .Weather   (9)
                .HookType  (HookSet.Precise);
            fish.Apply     (17590, Patch.TheFarEdgeOfFate) // Armor Fish
                .Bait      (fish, 12705, 12757)
                .Tug       (BiteType.Legendary)
                .Uptime    (1, 4)
                .Snag      (Snagging.None)
                .Predators (fish, (12757, 6))
                .Intuition (160)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (17591, Patch.TheFarEdgeOfFate) // Sea Butterfly
                .Bait      (fish, 12712)
                .Tug       (BiteType.Legendary)
                .Uptime    (5, 7)
                .Snag      (Snagging.None)
                .Predators (fish, (12810, 3), (12753, 3))
                .Intuition (60)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (17592, Patch.TheFarEdgeOfFate) // Charibenet
                .Bait      (fish, 12705, 12715)
                .Tug       (BiteType.Legendary)
                .Uptime    (0, 3)
                .Snag      (Snagging.None)
                .Predators (fish, (12715, 5))
                .Intuition (120)
                .Weather   (16)
                .HookType  (HookSet.Precise);
            fish.Apply     (17593, Patch.TheFarEdgeOfFate) // Raimdellopterus
                .Bait      (fish, 12712, 12805)
                .Tug       (BiteType.Legendary)
                .Uptime    (5, 8)
                .Snag      (Snagging.None)
                .Predators (fish, (12805, 5))
                .Intuition (100)
                .Weather   (6)
                .HookType  (HookSet.Powerful);
        }
        // @formatter:on
    }
}
