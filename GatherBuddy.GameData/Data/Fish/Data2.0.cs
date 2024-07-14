using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyARealmReborn(this GameData data)
    {
        data.Apply     (4776, Patch.ARealmReborn) // Malm Kelp
            .Bait      (data, 2585)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4869, Patch.ARealmReborn) // Merlthor Goby
            .Bait      (data, 2585)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Snag      (data, Snagging.None);
        data.Apply     (4870, Patch.ARealmReborn) // Lominsan Anchovy
            .Bait      (data, 2585)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4871, Patch.ARealmReborn) // Finger Shrimp
            .Bait      (data, 2585)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4872, Patch.ARealmReborn) // Ocean Cloud
            .Bait      (data, 2587)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Snag      (data, Snagging.None);
        data.Apply     (4873, Patch.ARealmReborn) // Sea Cucumber
            .Bait      (data, 2589)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4874, Patch.ARealmReborn) // Harbor Herring
            .Bait      (data, 2587)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Snag      (data, Snagging.None);
        data.Apply     (4875, Patch.ARealmReborn) // Vongola Clam
            .Bait      (data, 2606)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4876, Patch.ARealmReborn) // Coral Butterfly
            .Bait      (data, 2587)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4877, Patch.ARealmReborn) // Moraby Flounder
            .Bait      (data, 2628)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4878, Patch.ARealmReborn) // Pebble Crab
            .Bait      (data, 2628)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4879, Patch.ARealmReborn) // Tiger Cod
            .Bait      (data, 2591)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4880, Patch.ARealmReborn) // Helmet Crab
            .Bait      (data, 2591)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4881, Patch.ARealmReborn) // Rothlyt Oyster
            .Bait      (data, 2591)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4882, Patch.ARealmReborn) // Navigator's Dagger
            .Bait      (data, 2628)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4883, Patch.ARealmReborn) // Angeldata
            .Bait      (data, 2616)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4884, Patch.ARealmReborn) // Razor Clam
            .Bait      (data, 2591)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4885, Patch.ARealmReborn) // Blue Octopus
            .Mooch     (data, 2585, 4869)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4886, Patch.ARealmReborn) // Blowdata
            .Bait      (data, 2606)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4887, Patch.ARealmReborn) // Saber Sardine
            .Bait      (data, 2619)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4888, Patch.ARealmReborn) // Ogre Barracuda
            .Mooch     (data, 2587, 4874)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Snag      (data, Snagging.None);
        data.Apply     (4889, Patch.ARealmReborn) // Monkdata
            .Bait      (data, 2619)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4890, Patch.ARealmReborn) // Sea Bo
            .Bait      (data, 2613)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4891, Patch.ARealmReborn) // Bianaq Bream
            .Bait      (data, 2619)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4892, Patch.ARealmReborn) // Black Sole
            .Bait      (data, 2598)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4893, Patch.ARealmReborn) // Hammerhead Shark
            .Bait      (data, 2616)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4894, Patch.ARealmReborn) // Sea Pickle
            .Bait      (data, 2598)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4895, Patch.ARealmReborn) // Indigo Herring
            .Bait      (data, 2596)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4896, Patch.ARealmReborn) // Ash Tuna
            .Bait      (data, 2596)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4897, Patch.ARealmReborn) // Leafy Seadragon
            .Bait      (data, 2606)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4898, Patch.ARealmReborn) // Fullmoon Sardine
            .Bait      (data, 2596)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Time      (1080, 360);
        data.Apply     (4899, Patch.ARealmReborn) // Haraldr Haddock
            .Bait      (data, 2619)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4900, Patch.ARealmReborn) // Whitelip Oyster
            .Bait      (data, 2598)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4901, Patch.ARealmReborn) // Lavender Remora
            .Bait      (data, 2619)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4902, Patch.ARealmReborn) // Balloonfish
            .Bait      (data, 2619)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4903, Patch.ARealmReborn) // Silver Shark
            .Mooch     (data, 2585, 4869)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4904, Patch.ARealmReborn) // Wahoo
            .Mooch     (data, 2585, 4869)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4905, Patch.ARealmReborn) // Raincaller
            .Bait      (data, 2601)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 7, 15);
        data.Apply     (4906, Patch.ARealmReborn) // Nautilus
            .Mooch     (data, 2616, 4898)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4907, Patch.ARealmReborn) // Pike Eel
            .Bait      (data, 2628)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4908, Patch.ARealmReborn) // Mummer Wrasse
            .Bait      (data, 2628)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4909, Patch.ARealmReborn) // Plaice
            .Bait      (data, 2628)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4910, Patch.ARealmReborn) // Sea Devil
            .Bait      (data, 2628)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4911, Patch.ARealmReborn) // Rock Lobster
            .Bait      (data, 2606)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Time      (1020, 1320);
        data.Apply     (4912, Patch.ARealmReborn) // Goosefish
            .Mooch     (data, 2585, 4869, 4904)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4913, Patch.ARealmReborn) // Little Thalaos
            .Bait      (data, 2606)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 7, 8);
        data.Apply     (4914, Patch.ARealmReborn) // Shall Shell
            .Bait      (data, 2606)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4915, Patch.ARealmReborn) // Mahi-mahi
            .Bait      (data, 2628)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (600, 1080);
        data.Apply     (4916, Patch.ARealmReborn) // Halibut
            .Bait      (data, 2606)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (4917, Patch.ARealmReborn) // Mazlaya Marlin
            .Mooch     (data, 2587, 4874, 4888)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (4918, Patch.ARealmReborn) // Coelacanth
            .Mooch     (data, 2616, 4898)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4919, Patch.ARealmReborn) // Giant Squid
            .Mooch     (data, 2585, 4869, 4904)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Snag      (data, Snagging.None);
        data.Apply     (4920, Patch.ARealmReborn) // Gigant Octopus
            .Mooch     (data, 2587, 4874, 4888)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (4921, Patch.ARealmReborn) // Sunfish
            .Mooch     (data, 2587, 4872)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4922, Patch.ARealmReborn) // Dinichthys
            .Mooch     (data, 2585, 4869, 4904)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (4923, Patch.ARealmReborn) // Megalodon
            .Mooch     (data, 2585, 4869, 4904)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (4924, Patch.ARealmReborn) // Titanic Sawdata
            .Mooch     (data, 2585, 4869, 4904)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (540, 900)
            .Weather   (data, 1, 2);
        data.Apply     (4925, Patch.ARealmReborn) // Craydata
            .Bait      (data, 2586)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4926, Patch.ARealmReborn) // Chub
            .Bait      (data, 2586)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4927, Patch.ARealmReborn) // Striped Goby
            .Bait      (data, 2586)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Snag      (data, Snagging.None);
        data.Apply     (4928, Patch.ARealmReborn) // Dwarf Catdata
            .Bait      (data, 2586)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4929, Patch.ARealmReborn) // Bone Craydata
            .Bait      (data, 2586)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4930, Patch.ARealmReborn) // Princess Trout
            .Bait      (data, 2588)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4931, Patch.ARealmReborn) // Dusk Goby
            .Bait      (data, 2588)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4932, Patch.ARealmReborn) // Pipira
            .Bait      (data, 2588)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4933, Patch.ARealmReborn) // Crimson Craydata
            .Bait      (data, 2588)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4934, Patch.ARealmReborn) // Gudgeon
            .Bait      (data, 2588)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4935, Patch.ARealmReborn) // Brass Loach
            .Bait      (data, 2623)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4936, Patch.ARealmReborn) // Maiden Carp
            .Bait      (data, 2590)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4937, Patch.ARealmReborn) // Abalathian Smelt
            .Bait      (data, 2599)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Snag      (data, Snagging.None);
        data.Apply     (4938, Patch.ARealmReborn) // Blinddata
            .Bait      (data, 2597)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4939, Patch.ARealmReborn) // Mudskipper
            .Bait      (data, 2592)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4940, Patch.ARealmReborn) // Rainbow Trout
            .Bait      (data, 2618)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4941, Patch.ARealmReborn) // River Crab
            .Bait      (data, 2590)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4942, Patch.ARealmReborn) // Ala Mhigan Fighting data
            .Bait      (data, 2592)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Snag      (data, Snagging.None);
        data.Apply     (4943, Patch.ARealmReborn) // Faerie Bass
            .Bait      (data, 2592)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4944, Patch.ARealmReborn) // Acorn Snail
            .Bait      (data, 2592)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4945, Patch.ARealmReborn) // Dark Sleeper
            .Bait      (data, 2592)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Time      (900, 600)
            .Snag      (data, Snagging.None);
        data.Apply     (4946, Patch.ARealmReborn) // La Noscean Perch
            .Bait      (data, 2611)
            .Bite      (data, HookSet.Precise, BiteType.Strong);
        data.Apply     (4947, Patch.ARealmReborn) // Moat Carp
            .Bait      (data, 2614)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4948, Patch.ARealmReborn) // Copperdata
            .Bait      (data, 2594)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Snag      (data, Snagging.None);
        data.Apply     (4949, Patch.ARealmReborn) // Bluebell Salmon
            .Bait      (data, 2611)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4950, Patch.ARealmReborn) // Mudcrab
            .Bait      (data, 2599)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4951, Patch.ARealmReborn) // Tri-colored Carp
            .Bait      (data, 2594)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4952, Patch.ARealmReborn) // Eunuch Craydata
            .Mooch     (data, 2594, 4948)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4953, Patch.ARealmReborn) // Jade Eel
            .Bait      (data, 2594)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Time      (1020, 600)
            .Snag      (data, Snagging.None);
        data.Apply     (4954, Patch.ARealmReborn) // Pond Mussel
            .Bait      (data, 2594)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4955, Patch.ARealmReborn) // Warmwater Trout
            .Bait      (data, 2614)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4956, Patch.ARealmReborn) // Glass Perch
            .Bait      (data, 2610)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4957, Patch.ARealmReborn) // Four-eyed data
            .Bait      (data, 2626)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4958, Patch.ARealmReborn) // Black Eel
            .Bait      (data, 2594)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Time      (1020, 600)
            .Snag      (data, Snagging.None);
        data.Apply     (4959, Patch.ARealmReborn) // Dark Bass
            .Bait      (data, 2614)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4960, Patch.ARealmReborn) // Aegis Shrimp
            .Bait      (data, 2595)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4961, Patch.ARealmReborn) // Five-ilm Pleco
            .Bait      (data, 2611)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4962, Patch.ARealmReborn) // Climbing Perch
            .Bait      (data, 2595)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4963, Patch.ARealmReborn) // Shadow Catdata
            .Mooch     (data, 2586, 4927)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4964, Patch.ARealmReborn) // Black Ghost
            .Mooch     (data, 2592, 4942)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4965, Patch.ARealmReborn) // Lamprey
            .Mooch     (data, 2594, 4948)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4966, Patch.ARealmReborn) // Plaguedata
            .Bait      (data, 2610)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4967, Patch.ARealmReborn) // Yugr'am Salmon
            .Bait      (data, 2595)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4968, Patch.ARealmReborn) // Spotted Pleco
            .Bait      (data, 2597)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4969, Patch.ARealmReborn) // Ropedata
            .Bait      (data, 2615)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4970, Patch.ARealmReborn) // Grip Killidata
            .Bait      (data, 2610)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4971, Patch.ARealmReborn) // Bone Cleaner
            .Bait      (data, 2597)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4972, Patch.ARealmReborn) // Root Skipper
            .Bait      (data, 2597)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4973, Patch.ARealmReborn) // Bonytongue
            .Bait      (data, 2627)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4974, Patch.ARealmReborn) // Mitten Crab
            .Bait      (data, 2599)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4975, Patch.ARealmReborn) // Monke Onke
            .Mooch     (data, 2592, 4942)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4976, Patch.ARealmReborn) // Seema
            .Bait      (data, 2623)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4977, Patch.ARealmReborn) // Sanddata
            .Bait      (data, 27591)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4978, Patch.ARealmReborn) // Silverdata
            .Bait      (data, 2599)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Snag      (data, Snagging.None);
        data.Apply     (4979, Patch.ARealmReborn) // Clown Loach
            .Bait      (data, 2599)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4980, Patch.ARealmReborn) // Armored Pleco
            .Bait      (data, 2599)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4981, Patch.ARealmReborn) // Giant Bass
            .Bait      (data, 2599)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4982, Patch.ARealmReborn) // Velodyna Carp
            .Bait      (data, 2627)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4983, Patch.ARealmReborn) // Spotted Puffer
            .Bait      (data, 2599)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4984, Patch.ARealmReborn) // Golden Loach
            .Bait      (data, 2599)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4985, Patch.ARealmReborn) // Trader Eel
            .Bait      (data, 2599)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Time      (1020, 600)
            .Snag      (data, Snagging.None);
        data.Apply     (4986, Patch.ARealmReborn) // Crimson Trout
            .Bait      (data, 2620)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4987, Patch.ARealmReborn) // Discus
            .Bait      (data, 2599)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4988, Patch.ARealmReborn) // Bronze Lake Trout
            .Bait      (data, 2626)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4989, Patch.ARealmReborn) // Ignus Snail
            .Bait      (data, 2601)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4990, Patch.ARealmReborn) // Loyal Pleco
            .Bait      (data, 2601)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4991, Patch.ARealmReborn) // Thunderbolt Sculpin
            .Bait      (data, 2620)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 7, 8);
        data.Apply     (4992, Patch.ARealmReborn) // Fall Jumper
            .Mooch     (data, 2594, 4948)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4993, Patch.ARealmReborn) // Knifedata
            .Bait      (data, 2603)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4994, Patch.ARealmReborn) // Oakroot
            .Bait      (data, 2601)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Time      (1020, 600)
            .Snag      (data, Snagging.None);
        data.Apply     (4995, Patch.ARealmReborn) // Common Sculpin
            .Bait      (data, 2626)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Snag      (data, Snagging.None);
        data.Apply     (4996, Patch.ARealmReborn) // Southern Pike
            .Bait      (data, 2624)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4997, Patch.ARealmReborn) // Northern Pike
            .Bait      (data, 2624)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (4998, Patch.ARealmReborn) // Kobold Puffer
            .Bait      (data, 2626)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (4999, Patch.ARealmReborn) // Archerdata
            .Bait      (data, 2618)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Snag      (data, Snagging.None)
            .Weather   (data, 1, 2, 3, 4);
        data.Apply     (5000, Patch.ARealmReborn) // Goblin Perch
            .Mooch     (data, 2599, 4978)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5001, Patch.ARealmReborn) // Agelyss Carp
            .Bait      (data, 2618)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5002, Patch.ARealmReborn) // Assassin Betta
            .Mooch     (data, 2599, 4978)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Snag      (data, Snagging.None);
        data.Apply     (5003, Patch.ARealmReborn) // Sludgeskipper
            .Bait      (data, 2601)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (5004, Patch.ARealmReborn) // Boltdata
            .Mooch     (data, 2599, 4978)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5005, Patch.ARealmReborn) // Garpike
            .Mooch     (data, 2599, 4978)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5006, Patch.ARealmReborn) // Ilsabardian Bass
            .Bait      (data, 2624)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5007, Patch.ARealmReborn) // Paglth'an Discus
            .Bait      (data, 2603)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 14, 2, 1);
        data.Apply     (5008, Patch.ARealmReborn) // Boxing Pleco
            .Bait      (data, 2603)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5009, Patch.ARealmReborn) // Kissing Trout
            .Mooch     (data, 2586, 4927)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5010, Patch.ARealmReborn) // Angry Pike
            .Mooch     (data, 2599, 4937)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5011, Patch.ARealmReborn) // Golddata
            .Mooch     (data, 2599, 4978)
            .Bite      (data, HookSet.Precise, BiteType.Weak)
            .Snag      (data, Snagging.None);
        data.Apply     (5012, Patch.ARealmReborn) // Vampire Lampern
            .Mooch     (data, 2599, 4978)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5013, Patch.ARealmReborn) // Wandering Sculpin
            .Bait      (data, 2607)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5014, Patch.ARealmReborn) // Cave Cherax
            .Mooch     (data, 2586, 4927)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5015, Patch.ARealmReborn) // Coeurldata
            .Mooch     (data, 2626, 4995)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (5016, Patch.ARealmReborn) // Giant Donko
            .Bait      (data, 2626)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Time      (240, 540);
        data.Apply     (5017, Patch.ARealmReborn) // Sundisc
            .Bait      (data, 2607)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Time      (600, 900)
            .Weather   (data, 1, 2);
        data.Apply     (5018, Patch.ARealmReborn) // Alligator Gardata
            .Mooch     (data, 2626, 4995)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (5019, Patch.ARealmReborn) // Wootz Knifedata
            .Mooch     (data, 2599, 4978, 5011)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5020, Patch.ARealmReborn) // Giant Catdata
            .Mooch     (data, 2599, 4978)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5021, Patch.ARealmReborn) // Cadaver Carp
            .Bait      (data, 2625)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 17);
        data.Apply     (5022, Patch.ARealmReborn) // Mushroom Crab
            .Mooch     (data, 2620, 4995)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 4, 3);
        data.Apply     (5023, Patch.ARealmReborn) // Judgment Staff
            .Bait      (data, 2607)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 9, 10);
        data.Apply     (5024, Patch.ARealmReborn) // Poxpike
            .Mooch     (data, 2599, 4978, 5011)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5025, Patch.ARealmReborn) // Emperor fish
            .Mooch     (data, 2586, 4927)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5026, Patch.ARealmReborn) // Bowfin
            .Mooch     (data, 2599, 4978, 5011)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5027, Patch.ARealmReborn) // Heliobatis
            .Bait      (data, 2607)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Time      (1020, 540);
        data.Apply     (5028, Patch.ARealmReborn) // Takitaro
            .Bait      (data, 2627)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5029, Patch.ARealmReborn) // Pirarucu
            .Bait      (data, 2627)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5030, Patch.ARealmReborn) // Morinabaligi
            .Mooch     (data, 2599, 4978, 5011)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (5031, Patch.ARealmReborn) // Jungle Catdata
            .Mooch     (data, 2599, 4978, 5011)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5032, Patch.ARealmReborn) // Sand Bream
            .Bait      (data, 2604)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5033, Patch.ARealmReborn) // Desert Catdata
            .Bait      (data, 2604)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5034, Patch.ARealmReborn) // Dustdata
            .Bait      (data, 27591)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5035, Patch.ARealmReborn) // Storm Rider
            .Bait      (data, 2600)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Snag      (data, Snagging.None);
        data.Apply     (5036, Patch.ARealmReborn) // Antlion Slug
            .Bait      (data, 2602)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5037, Patch.ARealmReborn) // Dune Manta
            .Mooch     (data, 2600, 5035)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5038, Patch.ARealmReborn) // Cloud Jellydata
            .Bait      (data, 2609)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (5039, Patch.ARealmReborn) // Skydata
            .Bait      (data, 2609)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (5040, Patch.ARealmReborn) // Cloud Cutter
            .Bait      (data, 2605)
            .Bite      (data, HookSet.Powerful, BiteType.Strong)
            .Snag      (data, Snagging.None);
        data.Apply     (5041, Patch.ARealmReborn) // Blind Manta
            .Mooch     (data, 2605, 5040)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5042, Patch.ARealmReborn) // Rift Sailor
            .Bait      (data, 2609)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (5043, Patch.ARealmReborn) // Sagolii Monkdata
            .Mooch     (data, 27591, 4977)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (5044, Patch.ARealmReborn) // Saucerdata
            .Mooch     (data, 2605, 5040)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (5045, Patch.ARealmReborn) // Caravan Eel
            .Mooch     (data, 2600, 5035)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (5046, Patch.ARealmReborn) // Rhamphorhynchus
            .Mooch     (data, 2605, 5040, 5044)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (5460, Patch.ARealmReborn) // White Coral
            .Bait      (data, 2587)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (5461, Patch.ARealmReborn) // Blue Coral
            .Bait      (data, 2613)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (5462, Patch.ARealmReborn) // Red Coral
            .Bait      (data, 2598)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (5466, Patch.ARealmReborn) // Blacklip Oyster
            .Bait      (data, 2621)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (5544, Patch.ARealmReborn) // Lamp Marimo
            .Bait      (data, 2610)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
    }
    // @formatter:on
}
