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
                .Bait      (fish, 2585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4869, Patch.ARealmReborn) // Merlthor Goby
                .Bait      (fish, 2585)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4870, Patch.ARealmReborn) // Lominsan Anchovy
                .Bait      (fish, 2585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4871, Patch.ARealmReborn) // Finger Shrimp
                .Bait      (fish, 2585)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4872, Patch.ARealmReborn) // Ocean Cloud
                .Bait      (fish, 2587)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4873, Patch.ARealmReborn) // Sea Cucumber
                .Bait      (fish, 2589)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4874, Patch.ARealmReborn) // Harbor Herring
                .Bait      (fish, 2587)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4875, Patch.ARealmReborn) // Vongola Clam
                .Bait      (fish, 2606)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4876, Patch.ARealmReborn) // Coral Butterfly
                .Bait      (fish, 2587)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4877, Patch.ARealmReborn) // Moraby Flounder
                .Bait      (fish, 2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4878, Patch.ARealmReborn) // Pebble Crab
                .Bait      (fish, 2628)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4879, Patch.ARealmReborn) // Tiger Cod
                .Bait      (fish, 2591)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4880, Patch.ARealmReborn) // Helmet Crab
                .Bait      (fish, 2591)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4881, Patch.ARealmReborn) // Rothlyt Oyster
                .Bait      (fish, 2591)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4882, Patch.ARealmReborn) // Navigator's Dagger
                .Bait      (fish, 2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4883, Patch.ARealmReborn) // Angelfish
                .Bait      (fish, 2616)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4884, Patch.ARealmReborn) // Razor Clam
                .Bait      (fish, 2591)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4885, Patch.ARealmReborn) // Blue Octopus
                .Bait      (fish, 2585, 4869)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4886, Patch.ARealmReborn) // Blowfish
                .Bait      (fish, 2606)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4887, Patch.ARealmReborn) // Saber Sardine
                .Bait      (fish, 2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4888, Patch.ARealmReborn) // Ogre Barracuda
                .Bait      (fish, 2587, 4874)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4889, Patch.ARealmReborn) // Monkfish
                .Bait      (fish, 2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4890, Patch.ARealmReborn) // Sea Bo
                .Bait      (fish, 2613)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4891, Patch.ARealmReborn) // Bianaq Bream
                .Bait      (fish, 2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4892, Patch.ARealmReborn) // Black Sole
                .Bait      (fish, 2598)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4893, Patch.ARealmReborn) // Hammerhead Shark
                .Bait      (fish, 2616)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4894, Patch.ARealmReborn) // Sea Pickle
                .Bait      (fish, 2598)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4895, Patch.ARealmReborn) // Indigo Herring
                .Bait      (fish, 2596)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4896, Patch.ARealmReborn) // Ash Tuna
                .Bait      (fish, 2596)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4897, Patch.ARealmReborn) // Leafy Seadragon
                .Bait      (fish, 2606)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4898, Patch.ARealmReborn) // Fullmoon Sardine
                .Bait      (fish, 2596)
                .Tug       (BiteType.Weak)
                .Uptime    (1080, 360)
                .HookType  (HookSet.Precise);
            fish.Apply     (4899, Patch.ARealmReborn) // Haraldr Haddock
                .Bait      (fish, 2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4900, Patch.ARealmReborn) // Whitelip Oyster
                .Bait      (fish, 2598)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4901, Patch.ARealmReborn) // Lavender Remora
                .Bait      (fish, 2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4902, Patch.ARealmReborn) // Balloonfish
                .Bait      (fish, 2619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4903, Patch.ARealmReborn) // Silver Shark
                .Bait      (fish, 2585, 4869)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4904, Patch.ARealmReborn) // Wahoo
                .Bait      (fish, 2585, 4869)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4905, Patch.ARealmReborn) // Raincaller
                .Bait      (fish, 2601)
                .Tug       (BiteType.Strong)
                .Weather   (7, 15)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4906, Patch.ARealmReborn) // Nautilus
                .Bait      (fish, 2616, 4898)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4907, Patch.ARealmReborn) // Pike Eel
                .Bait      (fish, 2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4908, Patch.ARealmReborn) // Mummer Wrasse
                .Bait      (fish, 2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4909, Patch.ARealmReborn) // Plaice
                .Bait      (fish, 2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4910, Patch.ARealmReborn) // Sea Devil
                .Bait      (fish, 2628)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4911, Patch.ARealmReborn) // Rock Lobster
                .Bait      (fish, 2606)
                .Tug       (BiteType.Strong)
                .Uptime    (1020, 1320)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4912, Patch.ARealmReborn) // Goosefish
                .Bait      (fish, 2585, 4869, 4904)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4913, Patch.ARealmReborn) // Little Thalaos
                .Bait      (fish, 2606)
                .Tug       (BiteType.Strong)
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4914, Patch.ARealmReborn) // Shall Shell
                .Bait      (fish, 2606)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4915, Patch.ARealmReborn) // Mahi-mahi
                .Bait      (fish, 2628)
                .Tug       (BiteType.Legendary)
                .Uptime    (600, 1080)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4916, Patch.ARealmReborn) // Halibut
                .Bait      (fish, 2606)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4917, Patch.ARealmReborn) // Mazlaya Marlin
                .Bait      (fish, 2587, 4874, 4888)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4918, Patch.ARealmReborn) // Coelacanth
                .Bait      (fish, 2616, 4898)
                .Tug       (BiteType.Strong)
                .Uptime    (1080, 360)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4919, Patch.ARealmReborn) // Giant Squid
                .Bait      (fish, 2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4920, Patch.ARealmReborn) // Gigant Octopus
                .Bait      (fish, 2587, 4874, 4888)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4921, Patch.ARealmReborn) // Sunfish
                .Bait      (fish, 2587, 4872)
                .Weather   (3, 4, 5)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4922, Patch.ARealmReborn) // Dinichthys
                .Bait      (fish, 2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4923, Patch.ARealmReborn) // Megalodon
                .Bait      (fish, 2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4924, Patch.ARealmReborn) // Titanic Sawfish
                .Bait      (fish, 2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .Uptime    (540, 900)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4925, Patch.ARealmReborn) // Crayfish
                .Bait      (fish, 2586)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4926, Patch.ARealmReborn) // Chub
                .Bait      (fish, 2586)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4927, Patch.ARealmReborn) // Striped Goby
                .Bait      (fish, 2586)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4928, Patch.ARealmReborn) // Dwarf Catfish
                .Bait      (fish, 2586)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4929, Patch.ARealmReborn) // Bone Crayfish
                .Bait      (fish, 2586)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4930, Patch.ARealmReborn) // Princess Trout
                .Bait      (fish, 2588)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4931, Patch.ARealmReborn) // Dusk Goby
                .Bait      (fish, 2588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4932, Patch.ARealmReborn) // Pipira
                .Bait      (fish, 2588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4933, Patch.ARealmReborn) // Crimson Crayfish
                .Bait      (fish, 2588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4934, Patch.ARealmReborn) // Gudgeon
                .Bait      (fish, 2588)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4935, Patch.ARealmReborn) // Brass Loach
                .Bait      (fish, 2623)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4936, Patch.ARealmReborn) // Maiden Carp
                .Bait      (fish, 2590)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4937, Patch.ARealmReborn) // Abalathian Smelt
                .Bait      (fish, 2599)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4938, Patch.ARealmReborn) // Blindfish
                .Bait      (fish, 2597)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4939, Patch.ARealmReborn) // Mudskipper
                .Bait      (fish, 2592)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4940, Patch.ARealmReborn) // Rainbow Trout
                .Bait      (fish, 2618)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4941, Patch.ARealmReborn) // River Crab
                .Bait      (fish, 2590)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4942, Patch.ARealmReborn) // Ala Mhigan Fighting Fish
                .Bait      (fish, 2592)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4943, Patch.ARealmReborn) // Faerie Bass
                .Bait      (fish, 2592)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4944, Patch.ARealmReborn) // Acorn Snail
                .Bait      (fish, 2592)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4945, Patch.ARealmReborn) // Dark Sleeper
                .Bait      (fish, 2592)
                .Tug       (BiteType.Weak)
                .Uptime    (900, 600)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4946, Patch.ARealmReborn) // La Noscean Perch
                .Bait      (fish, 2611)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4947, Patch.ARealmReborn) // Moat Carp
                .Bait      (fish, 2614)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4948, Patch.ARealmReborn) // Copperfish
                .Bait      (fish, 2594)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4949, Patch.ARealmReborn) // Bluebell Salmon
                .Bait      (fish, 2611)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4950, Patch.ARealmReborn) // Mudcrab
                .Bait      (fish, 2599)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4951, Patch.ARealmReborn) // Tri-colored Carp
                .Bait      (fish, 2594)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4952, Patch.ARealmReborn) // Eunuch Crayfish
                .Bait      (fish, 2594, 4948)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4953, Patch.ARealmReborn) // Jade Eel
                .Bait      (fish, 2594)
                .Tug       (BiteType.Strong)
                .Uptime    (1020, 600)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4954, Patch.ARealmReborn) // Pond Mussel
                .Bait      (fish, 2594)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4955, Patch.ARealmReborn) // Warmwater Trout
                .Bait      (fish, 2614)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4956, Patch.ARealmReborn) // Glass Perch
                .Bait      (fish, 2610)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4957, Patch.ARealmReborn) // Four-eyed Fish
                .Bait      (fish, 2626)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4958, Patch.ARealmReborn) // Black Eel
                .Bait      (fish, 2594)
                .Tug       (BiteType.Strong)
                .Uptime    (1020, 600)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4959, Patch.ARealmReborn) // Dark Bass
                .Bait      (fish, 2614)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4960, Patch.ARealmReborn) // Aegis Shrimp
                .Bait      (fish, 2595)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4961, Patch.ARealmReborn) // Five-ilm Pleco
                .Bait      (fish, 2611)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4962, Patch.ARealmReborn) // Climbing Perch
                .Bait      (fish, 2595)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4963, Patch.ARealmReborn) // Shadow Catfish
                .Bait      (fish, 2586, 4927)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4964, Patch.ARealmReborn) // Black Ghost
                .Bait      (fish, 2592, 4942)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4965, Patch.ARealmReborn) // Lamprey
                .Bait      (fish, 2594, 4948)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4966, Patch.ARealmReborn) // Plaguefish
                .Bait      (fish, 2610)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4967, Patch.ARealmReborn) // Yugr'am Salmon
                .Bait      (fish, 2595)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4968, Patch.ARealmReborn) // Spotted Pleco
                .Bait      (fish, 2597)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4969, Patch.ARealmReborn) // Ropefish
                .Bait      (fish, 2615)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4970, Patch.ARealmReborn) // Grip Killifish
                .Bait      (fish, 2610)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4971, Patch.ARealmReborn) // Bone Cleaner
                .Bait      (fish, 2597)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4972, Patch.ARealmReborn) // Root Skipper
                .Bait      (fish, 2597)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4973, Patch.ARealmReborn) // Bonytongue
                .Bait      (fish, 2627)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4974, Patch.ARealmReborn) // Mitten Crab
                .Bait      (fish, 2599)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4975, Patch.ARealmReborn) // Monke Onke
                .Bait      (fish, 2592, 4942)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4976, Patch.ARealmReborn) // Seema
                .Bait      (fish, 2623)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4977, Patch.ARealmReborn) // Sandfish
                .Bait      (fish, 27591)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4978, Patch.ARealmReborn) // Silverfish
                .Bait      (fish, 2599)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4979, Patch.ARealmReborn) // Clown Loach
                .Bait      (fish, 2599)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4980, Patch.ARealmReborn) // Armored Pleco
                .Bait      (fish, 2599)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4981, Patch.ARealmReborn) // Giant Bass
                .Bait      (fish, 2599)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4982, Patch.ARealmReborn) // Velodyna Carp
                .Bait      (fish, 2627)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4983, Patch.ARealmReborn) // Spotted Puffer
                .Bait      (fish, 2599)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4984, Patch.ARealmReborn) // Golden Loach
                .Bait      (fish, 2599)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4985, Patch.ARealmReborn) // Trader Eel
                .Bait      (fish, 2599)
                .Tug       (BiteType.Strong)
                .Uptime    (1020, 600)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4986, Patch.ARealmReborn) // Crimson Trout
                .Bait      (fish, 2620)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4987, Patch.ARealmReborn) // Discus
                .Bait      (fish, 2599)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4988, Patch.ARealmReborn) // Bronze Lake Trout
                .Bait      (fish, 2626)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4989, Patch.ARealmReborn) // Ignus Snail
                .Bait      (fish, 2601)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4990, Patch.ARealmReborn) // Loyal Pleco
                .Bait      (fish, 2601)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4991, Patch.ARealmReborn) // Thunderbolt Sculpin
                .Bait      (fish, 2620)
                .Tug       (BiteType.Strong)
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4992, Patch.ARealmReborn) // Fall Jumper
                .Bait      (fish, 2594, 4948)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4993, Patch.ARealmReborn) // Knifefish
                .Bait      (fish, 2603)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4994, Patch.ARealmReborn) // Oakroot
                .Bait      (fish, 2601)
                .Tug       (BiteType.Strong)
                .Uptime    (1020, 600)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4995, Patch.ARealmReborn) // Common Sculpin
                .Bait      (fish, 2626)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (4996, Patch.ARealmReborn) // Southern Pike
                .Bait      (fish, 2624)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4997, Patch.ARealmReborn) // Northern Pike
                .Bait      (fish, 2624)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (4998, Patch.ARealmReborn) // Kobold Puffer
                .Bait      (fish, 2626)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (4999, Patch.ARealmReborn) // Archerfish
                .Bait      (fish, 2618)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .Weather   (1, 2, 3, 4)
                .HookType  (HookSet.Precise);
            fish.Apply     (5000, Patch.ARealmReborn) // Goblin Perch
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5001, Patch.ARealmReborn) // Agelyss Carp
                .Bait      (fish, 2618)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5002, Patch.ARealmReborn) // Assassin Betta
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5003, Patch.ARealmReborn) // Sludgeskipper
                .Bait      (fish, 2601)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5004, Patch.ARealmReborn) // Boltfish
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5005, Patch.ARealmReborn) // Garpike
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5006, Patch.ARealmReborn) // Ilsabardian Bass
                .Bait      (fish, 2624)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5007, Patch.ARealmReborn) // Paglth'an Discus
                .Bait      (fish, 2603)
                .Tug       (BiteType.Strong)
                .Weather   (14, 2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5008, Patch.ARealmReborn) // Boxing Pleco
                .Bait      (fish, 2603)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5009, Patch.ARealmReborn) // Kissing Trout
                .Bait      (fish, 2586, 4927)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5010, Patch.ARealmReborn) // Angry Pike
                .Bait      (fish, 2599, 4937)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5011, Patch.ARealmReborn) // Goldfish
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (5012, Patch.ARealmReborn) // Vampire Lampern
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5013, Patch.ARealmReborn) // Wandering Sculpin
                .Bait      (fish, 2607)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5014, Patch.ARealmReborn) // Cave Cherax
                .Bait      (fish, 2586, 4927)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5015, Patch.ARealmReborn) // Coeurlfish
                .Bait      (fish, 2626, 4995)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5016, Patch.ARealmReborn) // Giant Donko
                .Bait      (fish, 2626)
                .Tug       (BiteType.Strong)
                .Uptime    (240, 540)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5017, Patch.ARealmReborn) // Sundisc
                .Bait      (fish, 2607)
                .Tug       (BiteType.Strong)
                .Uptime    (600, 900)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5018, Patch.ARealmReborn) // Alligator Garfish
                .Bait      (fish, 2626, 4995)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5019, Patch.ARealmReborn) // Wootz Knifefish
                .Bait      (fish, 2599, 4978, 5011)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5020, Patch.ARealmReborn) // Giant Catfish
                .Bait      (fish, 2599, 4978)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5021, Patch.ARealmReborn) // Cadaver Carp
                .Bait      (fish, 2625)
                .Tug       (BiteType.Strong)
                .Weather   (17)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5022, Patch.ARealmReborn) // Mushroom Crab
                .Bait      (fish, 2620, 4995)
                .Tug       (BiteType.Strong)
                .Weather   (4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5023, Patch.ARealmReborn) // Judgment Staff
                .Bait      (fish, 2607)
                .Tug       (BiteType.Strong)
                .Weather   (9, 10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5024, Patch.ARealmReborn) // Poxpike
                .Bait      (fish, 2599, 4978, 5011)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5025, Patch.ARealmReborn) // Emperor Fish
                .Bait      (fish, 2599, 4937)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5026, Patch.ARealmReborn) // Bowfin
                .Bait      (fish, 2599, 4978, 5011)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5027, Patch.ARealmReborn) // Heliobatis
                .Bait      (fish, 2607)
                .Tug       (BiteType.Strong)
                .Uptime    (1020, 540)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5028, Patch.ARealmReborn) // Takitaro
                .Bait      (fish, 2627)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5029, Patch.ARealmReborn) // Pirarucu
                .Bait      (fish, 2627)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5030, Patch.ARealmReborn) // Morinabaligi
                .Bait      (fish, 2599, 4978, 5011)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5031, Patch.ARealmReborn) // Jungle Catfish
                .Bait      (fish, 2599, 4978, 5011)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5032, Patch.ARealmReborn) // Sand Bream
                .Bait      (fish, 2604)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5033, Patch.ARealmReborn) // Desert Catfish
                .Bait      (fish, 2604)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5034, Patch.ARealmReborn) // Dustfish
                .Bait      (fish, 27591)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5035, Patch.ARealmReborn) // Storm Rider
                .Bait      (fish, 2600)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5036, Patch.ARealmReborn) // Antlion Slug
                .Bait      (fish, 2602)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5037, Patch.ARealmReborn) // Dune Manta
                .Bait      (fish, 2600, 5035)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5038, Patch.ARealmReborn) // Cloud Jellyfish
                .Bait      (fish, 2609)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5039, Patch.ARealmReborn) // Skyfish
                .Bait      (fish, 2609)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5040, Patch.ARealmReborn) // Cloud Cutter
                .Bait      (fish, 2605)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5041, Patch.ARealmReborn) // Blind Manta
                .Bait      (fish, 2605, 5040)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5042, Patch.ARealmReborn) // Rift Sailor
                .Bait      (fish, 2609)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5043, Patch.ARealmReborn) // Sagolii Monkfish
                .Bait      (fish, 27591, 4977)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5044, Patch.ARealmReborn) // Saucerfish
                .Bait      (fish, 2605, 5040)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5045, Patch.ARealmReborn) // Caravan Eel
                .Bait      (fish, 2600, 5035)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5046, Patch.ARealmReborn) // Rhamphorhynchus
                .Bait      (fish, 2605, 5040, 5044)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (5460, Patch.ARealmReborn) // White Coral
                .Bait      (fish, 2587)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5461, Patch.ARealmReborn) // Blue Coral
                .Bait      (fish, 2613)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5462, Patch.ARealmReborn) // Red Coral
                .Bait      (fish, 2598)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5466, Patch.ARealmReborn) // Blacklip Oyster
                .Bait      (fish, 2621)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (5544, Patch.ARealmReborn) // Lamp Marimo
                .Bait      (fish, 2610)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
