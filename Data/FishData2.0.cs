using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyARealmReborn(this FishManager fish)
        {
            fish.Apply     (4776, Patch.ARealmReborn) // Malm Kelp
                .Bait      (2585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4869, Patch.ARealmReborn) // Merlthor Goby
                .Bait      (2585)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4870, Patch.ARealmReborn) // Lominsan Anchovy
                .Bait      (2585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4871, Patch.ARealmReborn) // Finger Shrimp
                .Bait      (2585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4872, Patch.ARealmReborn) // Ocean Cloud
                .Bait      (2587)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4873, Patch.ARealmReborn) // Sea Cucumber
                .Bait      (2589)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4874, Patch.ARealmReborn) // Harbor Herring
                .Bait      (2587)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4875, Patch.ARealmReborn) // Vongola Clam
                .Bait      (2606)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4876, Patch.ARealmReborn) // Coral Butterfly
                .Bait      (2587)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4877, Patch.ARealmReborn) // Moraby Flounder
                .Bait      (2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4878, Patch.ARealmReborn) // Pebble Crab
                .Bait      (2628)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4879, Patch.ARealmReborn) // Tiger Cod
                .Bait      (2591)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4880, Patch.ARealmReborn) // Helmet Crab
                .Bait      (2591)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4881, Patch.ARealmReborn) // Rothlyt Oyster
                .Bait      (2591)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4882, Patch.ARealmReborn) // Navigator's Dagger
                .Bait      (2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4883, Patch.ARealmReborn) // Angelfish
                .Bait      (2616)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4884, Patch.ARealmReborn) // Razor Clam
                .Bait      (2591)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4885, Patch.ARealmReborn) // Blue Octopus
                .Bait      (2585, 4869)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4886, Patch.ARealmReborn) // Blowfish
                .Bait      (2606)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4887, Patch.ARealmReborn) // Saber Sardine
                .Bait      (2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4888, Patch.ARealmReborn) // Ogre Barracuda
                .Bait      (2587, 4874)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4889, Patch.ARealmReborn) // Monkfish
                .Bait      (2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4890, Patch.ARealmReborn) // Sea Bo
                .Bait      (2613)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4891, Patch.ARealmReborn) // Bianaq Bream
                .Bait      (2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4892, Patch.ARealmReborn) // Black Sole
                .Bait      (2598)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4893, Patch.ARealmReborn) // Hammerhead Shark
                .Bait      (2616)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4894, Patch.ARealmReborn) // Sea Pickle
                .Bait      (2598)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4895, Patch.ARealmReborn) // Indigo Herring
                .Bait      (2596)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4896, Patch.ARealmReborn) // Ash Tuna
                .Bait      (2596)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4897, Patch.ARealmReborn) // Leafy Seadragon
                .Bait      (2606)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4898, Patch.ARealmReborn) // Fullmoon Sardine
                .Bait      (2596)
                .Tug       (BiteType.Weak)
                .Uptime    (18, 6)
                .HookType  (HookSet.Precise);
            fish.Apply     (4899, Patch.ARealmReborn) // Haraldr Haddock
                .Bait      (2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4900, Patch.ARealmReborn) // Whitelip Oyster
                .Bait      (2598)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4901, Patch.ARealmReborn) // Lavender Remora
                .Bait      (2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4902, Patch.ARealmReborn) // Balloonfish
                .Bait      (2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4903, Patch.ARealmReborn) // Silver Shark
                .Bait      (2585, 4869)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4904, Patch.ARealmReborn) // Wahoo
                .Bait      (2585, 4869)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4905, Patch.ARealmReborn) // Raincaller
                .Bait      (2601)
                .Tug       (BiteType.Strong)
                .Weather   (7, 15)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4906, Patch.ARealmReborn) // Nautilus
                .Bait      (2616, 4898)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4907, Patch.ARealmReborn) // Pike Eel
                .Bait      (2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4908, Patch.ARealmReborn) // Mummer Wrasse
                .Bait      (2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4909, Patch.ARealmReborn) // Plaice
                .Bait      (2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4910, Patch.ARealmReborn) // Sea Devil
                .Bait      (2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4911, Patch.ARealmReborn) // Rock Lobster
                .Bait      (2606)
                .Tug       (BiteType.Strong)
                .Uptime    (17, 21)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4912, Patch.ARealmReborn) // Goosefish
                .Bait      (2585, 4869, 4904)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4913, Patch.ARealmReborn) // Little Thalaos
                .Bait      (2606)
                .Tug       (BiteType.Strong)
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4914, Patch.ARealmReborn) // Shall Shell
                .Bait      (2606)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4915, Patch.ARealmReborn) // Mahi-Mahi
                .Bait      (2628)
                .Tug       (BiteType.Legendary)
                .Uptime    (10, 18)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4916, Patch.ARealmReborn) // Halibut
                .Bait      (2606)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4917, Patch.ARealmReborn) // Mazlaya Marlin
                .Bait      (2587, 4874, 4888)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4918, Patch.ARealmReborn) // Coelacanth
                .Bait      (2616, 4898)
                .Tug       (BiteType.Strong)
                .Uptime    (18, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4919, Patch.ARealmReborn) // Giant Squid
                .Bait      (2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4920, Patch.ARealmReborn) // Gigant Octopus
                .Bait      (2587, 4874, 4888)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4921, Patch.ARealmReborn) // Sunfish
                .Bait      (2587, 4872)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4922, Patch.ARealmReborn) // Dinichthys
                .Bait      (2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4923, Patch.ARealmReborn) // Megalodon
                .Bait      (2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4924, Patch.ARealmReborn) // Titanic Sawfish
                .Bait      (2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 15)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4925, Patch.ARealmReborn) // Crayfish
                .Bait      (2586)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4926, Patch.ARealmReborn) // Chub
                .Bait      (2586)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4927, Patch.ARealmReborn) // Striped Goby
                .Bait      (2586)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4928, Patch.ARealmReborn) // Dwarf Catfish
                .Bait      (2586)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4929, Patch.ARealmReborn) // Bone Crayfish
                .Bait      (2586)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4930, Patch.ARealmReborn) // Princess Trout
                .Bait      (2588)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4931, Patch.ARealmReborn) // Dusk Goby
                .Bait      (2588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4932, Patch.ARealmReborn) // Pipira
                .Bait      (2588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4933, Patch.ARealmReborn) // Crimson Crayfish
                .Bait      (2588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4934, Patch.ARealmReborn) // Gudgeon
                .Bait      (2588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4935, Patch.ARealmReborn) // Brass Loach
                .Bait      (2623)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4936, Patch.ARealmReborn) // Maiden Carp
                .Bait      (2590)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4937, Patch.ARealmReborn) // Abalathian Smelt
                .Bait      (2599)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4938, Patch.ARealmReborn) // Blindfish
                .Bait      (2597)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4939, Patch.ARealmReborn) // Mudskipper
                .Bait      (2592)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4940, Patch.ARealmReborn) // Rainbow Trout
                .Bait      (2618)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4941, Patch.ARealmReborn) // River Crab
                .Bait      (2590)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4942, Patch.ARealmReborn) // Ala Mhigan Fighting Fish
                .Bait      (2592)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4943, Patch.ARealmReborn) // Faerie Bass
                .Bait      (2592)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4944, Patch.ARealmReborn) // Acorn Snail
                .Bait      (2592)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4945, Patch.ARealmReborn) // Dark Sleeper
                .Bait      (2592)
                .Tug       (BiteType.Weak)
                .Uptime    (15, 10)
                .HookType  (HookSet.Precise);
            fish.Apply     (4946, Patch.ARealmReborn) // La Noscean Perch
                .Bait      (2611)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4947, Patch.ARealmReborn) // Moat Carp
                .Bait      (2614)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4948, Patch.ARealmReborn) // Copperfish
                .Bait      (2594)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4949, Patch.ARealmReborn) // Bluebell Salmon
                .Bait      (2611)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4950, Patch.ARealmReborn) // Mudcrab
                .Bait      (2599)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4951, Patch.ARealmReborn) // Tri-colored Carp
                .Bait      (2594)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4952, Patch.ARealmReborn) // Eunuch Crayfish
                .Bait      (2594, 4948)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4953, Patch.ARealmReborn) // Jade Eel
                .Bait      (2594)
                .Tug       (BiteType.Strong)
                .Uptime    (17, 10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4954, Patch.ARealmReborn) // Pond Mussel
                .Bait      (2594)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4955, Patch.ARealmReborn) // Warmwater Trout
                .Bait      (2614)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4956, Patch.ARealmReborn) // Glass Perch
                .Bait      (2610)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4957, Patch.ARealmReborn) // Four-eyed Fish
                .Bait      (2626)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4958, Patch.ARealmReborn) // Black Eel
                .Bait      (2594)
                .Tug       (BiteType.Strong)
                .Uptime    (17, 10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4959, Patch.ARealmReborn) // Dark Bass
                .Bait      (2614)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4960, Patch.ARealmReborn) // Aegis Shrimp
                .Bait      (2595)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4961, Patch.ARealmReborn) // Five-ilm Pleco
                .Bait      (2611)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4962, Patch.ARealmReborn) // Climbing Perch
                .Bait      (2595)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4963, Patch.ARealmReborn) // Shadow Catfish
                .Bait      (2586, 4927)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4964, Patch.ARealmReborn) // Black Ghost
                .Bait      (2592, 4942)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4965, Patch.ARealmReborn) // Lamprey
                .Bait      (2594, 4948)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4966, Patch.ARealmReborn) // Plaguefish
                .Bait      (2610)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4967, Patch.ARealmReborn) // Yugr'am Salmon
                .Bait      (2595)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4968, Patch.ARealmReborn) // Spotted Pleco
                .Bait      (2597)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4969, Patch.ARealmReborn) // Ropefish
                .Bait      (2615)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4970, Patch.ARealmReborn) // Grip Killifish
                .Bait      (2610)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4971, Patch.ARealmReborn) // Bone Cleaner
                .Bait      (2597)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4972, Patch.ARealmReborn) // Root Skipper
                .Bait      (2597)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4973, Patch.ARealmReborn) // Bonytongue
                .Bait      (2627)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4974, Patch.ARealmReborn) // Mitten Crab
                .Bait      (2599)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4975, Patch.ARealmReborn) // Monke Onke
                .Bait      (2592, 4942)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4976, Patch.ARealmReborn) // Seema
                .Bait      (2623)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4977, Patch.ARealmReborn) // Sandfish
                .Bait      (27591)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4978, Patch.ARealmReborn) // Silverfish
                .Bait      (2599)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4979, Patch.ARealmReborn) // Clown Loach
                .Bait      (2599)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4980, Patch.ARealmReborn) // Armored Pleco
                .Bait      (2599)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4981, Patch.ARealmReborn) // Giant Bass
                .Bait      (2599)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4982, Patch.ARealmReborn) // Velodyna Carp
                .Bait      (2627)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4983, Patch.ARealmReborn) // Spotted Puffer
                .Bait      (2599)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4984, Patch.ARealmReborn) // Golden Loach
                .Bait      (2599)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4985, Patch.ARealmReborn) // Trader Eel
                .Bait      (2599)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4986, Patch.ARealmReborn) // Crimson Trout
                .Bait      (2620)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4987, Patch.ARealmReborn) // Discus
                .Bait      (2599)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4988, Patch.ARealmReborn) // Bronze Lake Trout
                .Bait      (2626)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4989, Patch.ARealmReborn) // Ignus Snail
                .Bait      (2601)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4990, Patch.ARealmReborn) // Loyal Pleco
                .Bait      (2601)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4991, Patch.ARealmReborn) // Thunderbolt Sculpin
                .Bait      (2620)
                .Tug       (BiteType.Strong)
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4992, Patch.ARealmReborn) // Fall Jumper
                .Bait      (2594, 4948)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4993, Patch.ARealmReborn) // Knifefish
                .Bait      (2603)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4994, Patch.ARealmReborn) // Oakroot
                .Bait      (2627)
                .Tug       (BiteType.Strong)
                .Uptime    (17, 10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4995, Patch.ARealmReborn) // Common Sculpin
                .Bait      (2626)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4996, Patch.ARealmReborn) // Southern Pike
                .Bait      (2624)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4997, Patch.ARealmReborn) // Northern Pike
                .Bait      (2624)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4998, Patch.ARealmReborn) // Kobold Puffer
                .Bait      (2626)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4999, Patch.ARealmReborn) // Archerfish
                .Bait      (2618)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5000, Patch.ARealmReborn) // Goblin Perch
                .Bait      (2599, 4978)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5001, Patch.ARealmReborn) // Agelyss Carp
                .Bait      (2618)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5002, Patch.ARealmReborn) // Assassin Betta
                .Bait      (2599, 4978)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5003, Patch.ARealmReborn) // Sludgeskipper
                .Bait      (2601)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5004, Patch.ARealmReborn) // Boltfish
                .Bait      (2599, 4978)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5005, Patch.ARealmReborn) // Garpike
                .Bait      (2599, 4978)
                .Tug       (BiteType.Strong)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5006, Patch.ARealmReborn) // Ilsabardian Bass
                .Bait      (2624)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5007, Patch.ARealmReborn) // Paglth'an Discus
                .Bait      (2603)
                .Tug       (BiteType.Strong)
                .Weather   (14, 2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5008, Patch.ARealmReborn) // Boxing Pleco
                .Bait      (2603)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5009, Patch.ARealmReborn) // Kissing Trout
                .Bait      (2586, 4927)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5010, Patch.ARealmReborn) // Angry Pike
                .Bait      (2599, 4937)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5011, Patch.ARealmReborn) // Goldfish
                .Bait      (2599, 4978)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (5012, Patch.ARealmReborn) // Vampire Lampern
                .Bait      (2599, 4978)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5013, Patch.ARealmReborn) // Wandering Sculpin
                .Bait      (2607)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5014, Patch.ARealmReborn) // Cave Cherax
                .Bait      (2586, 4927)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5015, Patch.ARealmReborn) // Coeurlfish
                .Bait      (2626, 4995)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5016, Patch.ARealmReborn) // Giant Donko
                .Bait      (2626)
                .Tug       (BiteType.Strong)
                .Uptime    (4, 9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5017, Patch.ARealmReborn) // Sundisc
                .Bait      (2607)
                .Tug       (BiteType.Strong)
                .Uptime    (10, 15)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5018, Patch.ARealmReborn) // Alligator Garfish
                .Bait      (2626, 4995)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5019, Patch.ARealmReborn) // Wootz Knifefish
                .Bait      (2599, 4978, 5011)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5020, Patch.ARealmReborn) // Giant Catfish
                .Bait      (2599, 4978)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5021, Patch.ARealmReborn) // Cadaver Carp
                .Bait      (2625)
                .Tug       (BiteType.Strong)
                .Weather   (17)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5022, Patch.ARealmReborn) // Mushroom Crab
                .Bait      (2620, 4995)
                .Tug       (BiteType.Strong)
                .Weather   (4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5023, Patch.ARealmReborn) // Judgment Staff
                .Bait      (2607)
                .Tug       (BiteType.Strong)
                .Weather   (9, 10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5024, Patch.ARealmReborn) // Poxpike
                .Bait      (2599, 4978, 5011)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5025, Patch.ARealmReborn) // Emperor Fish
                .Bait      (2599, 4937)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5026, Patch.ARealmReborn) // Bowfin
                .Bait      (2599, 4978, 5011)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5027, Patch.ARealmReborn) // Heliobatis
                .Bait      (2607)
                .Tug       (BiteType.Strong)
                .Uptime    (17, 9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5028, Patch.ARealmReborn) // Takitaro
                .Bait      (2627)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5029, Patch.ARealmReborn) // Pirarucu
                .Bait      (2627)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5030, Patch.ARealmReborn) // Morinabaligi
                .Bait      (2599, 4978, 5011)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5031, Patch.ARealmReborn) // Jungle Catfish
                .Bait      (2599, 4978, 5011)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5032, Patch.ARealmReborn) // Sand Bream
                .Bait      (2604)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5033, Patch.ARealmReborn) // Desert Catfish
                .Bait      (2604)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5034, Patch.ARealmReborn) // Dustfish
                .Bait      (27591)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5035, Patch.ARealmReborn) // Storm Rider
                .Bait      (2600)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5036, Patch.ARealmReborn) // Antlion Slug
                .Bait      (2602)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5037, Patch.ARealmReborn) // Dune Manta
                .Bait      (2600, 5035)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5038, Patch.ARealmReborn) // Cloud Jellyfish
                .Bait      (2609)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5039, Patch.ARealmReborn) // Skyfish
                .Bait      (2609)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5040, Patch.ARealmReborn) // Cloud Cutter
                .Bait      (2605)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5041, Patch.ARealmReborn) // Blind Manta
                .Bait      (2605, 5040)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5042, Patch.ARealmReborn) // Rift Sailor
                .Bait      (2609)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5043, Patch.ARealmReborn) // Sagolii Monkfish
                .Bait      (27591, 4977)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5044, Patch.ARealmReborn) // Saucerfish
                .Bait      (2605, 5040)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5045, Patch.ARealmReborn) // Caravan Eel
                .Bait      (2600, 5035)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5046, Patch.ARealmReborn) // Rhamphorhynchus
                .Bait      (2605, 5040, 5044)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5460, Patch.ARealmReborn) // White Coral
                .Bait      (2587)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5461, Patch.ARealmReborn) // Blue Coral
                .Bait      (2613)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5462, Patch.ARealmReborn) // Red Coral
                .Bait      (2598)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5466, Patch.ARealmReborn) // Blacklip Oyster
                .Bait      (2621)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5544, Patch.ARealmReborn) // Lamp Marimo
                .Bait      (2610)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
