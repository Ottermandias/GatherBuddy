using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyThePromiseOfTomorrow(this GameData data)
    {
        data.Apply(46188, Patch.ThePromiseOfTomorrow) // Goldentail
            .Bait(data, 43858)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(46189, Patch.ThePromiseOfTomorrow) // Prime Adjudicator
            .Bait(data, 43858)
            .Time(720, 960)
            .Transition(data, 2)
            .Weather(data, 4)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(46190, Patch.ThePromiseOfTomorrow) // Crenicichla Miyaka
            .Bait(data, 43858)
            .Time(360, 480)
            .Weather(data, 7)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(46191, Patch.ThePromiseOfTomorrow) // Iron Shadowtongue
            .Bait(data, 43858)
            .Time(960, 1080)
            .Weather(data, 7)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(46192, Patch.ThePromiseOfTomorrow) // Lotl-in-waiting
            .Mooch(data, 43728)
            .Time(0, 240)
            .Transition(data, 2)
            .Weather(data, 3)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(46193, Patch.ThePromiseOfTomorrow) // Azure Diver
            .Bait(data, 43857)
            .Time(1080, 1440)
            .Transition(data, 1, 2)
            .Weather(data, 6)
            .Bite(data, HookSet.Precise, BiteType.Legendary);
        data.Apply(46194, Patch.ThePromiseOfTomorrow) // Sprouting Perch
            .Bait(data, 43858)
            .Time(1200, 1440)
            .Weather(data, 10)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(46195, Patch.ThePromiseOfTomorrow) // Gigagiant Snakehead
            .Bait(data, 43858)
            .Time(240, 360)
            .Predators(data, 350, (43781, 3))
            .Weather(data, 7)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(46196, Patch.ThePromiseOfTomorrow) // Gondola Louvar
            .Bait(data, 43859)
            .Time(480, 720)
            .Transition(data, 7)
            .Weather(data, 2)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(46249, Patch.ThePromiseOfTomorrow) // Purple Palate
            .Bait(data, 43858)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(46779, Patch.ThePromiseOfTomorrow) // Sunray Ray
            .Bait(data, 43857)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);


        // The Lehr  Aquatic Foodstuffs
        data.Apply(47422, Patch.ThePromiseOfTomorrow) // Macrobrachium Phaennense
            .Bait(data, 47681)
            .Mission(data, 965)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47423, Patch.ThePromiseOfTomorrow) // Eelsplorer
            .Bait(data, 47681)
            .Mission(data, 965)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47424, Patch.ThePromiseOfTomorrow) // Lehr Brotula
            .Bait(data, 47681)
            .Mission(data, 965)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // West Beaconveil  Efficient Large Specimen Procurement
        data.Apply(47425, Patch.ThePromiseOfTomorrow) // Glass Stitcher
            .Bait(data, 47681)
            .Mission(data, 966)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47426, Patch.ThePromiseOfTomorrow) // Glass Discus
            .Bait(data, 47681)
            .Mission(data, 966)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47427, Patch.ThePromiseOfTomorrow) // Opal Ammonite
            .Bait(data, 47681)
            .Mission(data, 966)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Opalescent Crossing  Opalescent Crossing Specimen Survey //TODO: Confirm Multihook
        data.Apply(47428, Patch.ThePromiseOfTomorrow) // Pearl Shell
            .Bait(data, 47681)
            .Mission(data, 967)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47429, Patch.ThePromiseOfTomorrow) // Cobalt Bijou
            .Bait(data, 47681)
            .Mission(data, 967)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47430, Patch.ThePromiseOfTomorrow) // Untitled Work No. 33
            .Bait(data, 47681)
            .Mission(data, 967)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // The Lehr  Bulk Provision Procurement
        data.Apply(47431, Patch.ThePromiseOfTomorrow) // Macrobrachium Phaennense
            .Bait(data, 47682)
            .Mission(data, 968)
            .MultiHook(2)
            .Points(50)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47432, Patch.ThePromiseOfTomorrow) // Eelsplorer
            .Bait(data, 47682)
            .Mission(data, 968)
            .MultiHook(2)
            .Points(100)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47433, Patch.ThePromiseOfTomorrow) // Lehr Salamander
            .Bait(data, 47682)
            .Mission(data, 968)
            .MultiHook(2)
            .Points(150)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Opalescent Crossing  Opalescent Crossing Distribution Survey //TODO: Confirm Multihook
        data.Apply(47434, Patch.ThePromiseOfTomorrow) // Pearl Shell
            .Bait(data, 47682)
            .Mission(data, 969)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47435, Patch.ThePromiseOfTomorrow) // Cobalt Bijou
            .Bait(data, 47682)
            .Mission(data, 969)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47436, Patch.ThePromiseOfTomorrow) // Lampwork Cucumber
            .Bait(data, 47682)
            .Mission(data, 969)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // West Beaconveil  Large Mutant Cultivated Specimens
        data.Apply(47437, Patch.ThePromiseOfTomorrow) // Glass Stitcher
            .Bait(data, 47682)
            .Mission(data, 970)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47438, Patch.ThePromiseOfTomorrow) // Biodome
            .Bait(data, 47682)
            .Mission(data, 970)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47439, Patch.ThePromiseOfTomorrow) // Roseveil Bijou
            .Bait(data, 47682)
            .Mission(data, 970)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // The Lehr  Efficient Aquatic Foodstuffs Procurement //TODO Confirm Multihook
        data.Apply(47440, Patch.ThePromiseOfTomorrow) // Macrobrachium Phaennense
            .Bait(data, 47683)
            .Mission(data, 971)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47441, Patch.ThePromiseOfTomorrow) // Eelsplorer
            .Bait(data, 47683)
            .Mission(data, 971)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47442, Patch.ThePromiseOfTomorrow) // Dark Expanse Catfish
            .Bait(data, 47683)
            .Mission(data, 971)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47443, Patch.ThePromiseOfTomorrow) // Light Expanse Catfish
            .Bait(data, 47683)
            .Mission(data, 971)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // West Beaconveil  West Beaconveil Distribution Survey //TODO: Confirm Multihook
        data.Apply(47444, Patch.ThePromiseOfTomorrow) // Glass Stitcher
            .Bait(data, 47683)
            .Mission(data, 972)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47445, Patch.ThePromiseOfTomorrow) // Glass Jellyfish
            .Bait(data, 47683)
            .Mission(data, 972)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47446, Patch.ThePromiseOfTomorrow) // Untitled Work No. 11
            .Bait(data, 47683)
            .Mission(data, 972)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47447, Patch.ThePromiseOfTomorrow) // Glass Ribbon-arm
            .Bait(data, 47683)
            .Mission(data, 972)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // The Lehr  Fish Paste Ingredients
        data.Apply(47448, Patch.ThePromiseOfTomorrow) // Macrobrachium Phaennense
            .Bait(data, 47684)
            .Mission(data, 973)
            .MultiHook(2)
            .Points(50)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47449, Patch.ThePromiseOfTomorrow) // Eelsplorer
            .Bait(data, 47684)
            .Mission(data, 973)
            .MultiHook(2)
            .Points(100)
            .Bite(data, HookSet.Unknown, BiteType.Strong);
        data.Apply(47450, Patch.ThePromiseOfTomorrow) // Cosmocaecilian
            .Bait(data, 47684)
            .Mission(data, 973)
            .MultiHook(2)
            .Points(200)
            .Bite(data, HookSet.Unknown, BiteType.Strong);
        data.Apply(47451, Patch.ThePromiseOfTomorrow) // Lehr Carp
            .Bait(data, 47684)
            .Mission(data, 973)
            .MultiHook(2)
            .Points(300)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // West Beaconveil  Multi-purpose Bait Test //TODO: Confirm Multihook
        data.Apply(47452, Patch.ThePromiseOfTomorrow) // Glass Stitcher
            .Bait(data, 47684)
            .Mission(data, 974)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47453, Patch.ThePromiseOfTomorrow) // Glass Discus
            .Bait(data, 47684)
            .Mission(data, 974)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47454, Patch.ThePromiseOfTomorrow) // Phaenna's Arrow
            .Bait(data, 47684)
            .Mission(data, 974)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47455, Patch.ThePromiseOfTomorrow) // Boro Bead
            .Bait(data, 47684)
            .Mission(data, 974)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // The Lehr  Emergency Bulk Provision Procurement
        data.Apply(47456, Patch.ThePromiseOfTomorrow) // Macrobrachium Phaennense
            .Bait(data, 47685)
            .Mission(data, 975)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47457, Patch.ThePromiseOfTomorrow) // Eelsplorer
            .Bait(data, 47685)
            .Mission(data, 975)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47458, Patch.ThePromiseOfTomorrow) // Gymnarchus
            .Bait(data, 47685)
            .Mission(data, 975)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47459, Patch.ThePromiseOfTomorrow) // Inlay Ray
            .Bait(data, 47685)
            .Mission(data, 975)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Opalescent Crossing  Vitrified Freshwater Fish
        data.Apply(47460, Patch.ThePromiseOfTomorrow) // Pearl Shell
            .Bait(data, 47683)
            .Mission(data, 976)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47461, Patch.ThePromiseOfTomorrow) // Cobalt Bijou
            .Bait(data, 47683)
            .Mission(data, 976)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47462, Patch.ThePromiseOfTomorrow) // Bony Wraithfish
            .Bait(data, 47683)
            .Mission(data, 976)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47463, Patch.ThePromiseOfTomorrow) // Emeraldback
            .Bait(data, 47683)
            .Mission(data, 976)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Opalescent Crossing  High-silica Environment Observation //TODO: Confirm Baits
        data.Apply(47464, Patch.ThePromiseOfTomorrow) // Pearl Shell
            .Bait(data)
            .Mission(data, 977)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47465, Patch.ThePromiseOfTomorrow) // Cobalt Bijou
            .Bait(data)
            .Mission(data, 977)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47466, Patch.ThePromiseOfTomorrow) // Sandblaster
            .Bait(data, 47685)
            .Mission(data, 977)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47467, Patch.ThePromiseOfTomorrow) // Opal Shell
            .Bait(data, 47685)
            .Mission(data, 977)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Upper Soda-lime Float  Upper Soda-lime Float Distribution Survey //TODO: Confirm Multihook
        data.Apply(47480, Patch.ThePromiseOfTomorrow) // Frosted Shell
            .Bait(data, 47686)
            .Mission(data, 978)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47481, Patch.ThePromiseOfTomorrow) // Untitled Work No. 98
            .Bait(data, 47686)
            .Mission(data, 978)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47482, Patch.ThePromiseOfTomorrow) // Pendantfish
            .Bait(data, 47686)
            .Mission(data, 978)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47483, Patch.ThePromiseOfTomorrow) // Hammerhead Antiblaster
            .Bait(data, 47686)
            .Mission(data, 978)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47484, Patch.ThePromiseOfTomorrow) // Isosceles Cobalt
            .Bait(data, 47686)
            .Mission(data, 978)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Central Soda-lime Channel  Central Soda-lime Channel Distribution Survey //TODO: Confirm Multihook
        data.Apply(47485, Patch.ThePromiseOfTomorrow) // Glass Dahlia
            .Bait(data, 47687)
            .Mission(data, 979)
            .Points(50)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47486, Patch.ThePromiseOfTomorrow) // Fishy Jellyfish
            .Bait(data, 47687)
            .Mission(data, 979)
            .Points(100)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47487, Patch.ThePromiseOfTomorrow) // Glass Skeleton
            .Bait(data, 47687)
            .Mission(data, 979)
            .Points(150)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47488, Patch.ThePromiseOfTomorrow) // Blue Pane
            .Bait(data, 47687)
            .Mission(data, 979)
            .Points(200)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47489, Patch.ThePromiseOfTomorrow) // Crystallized Throwstone
            .Bait(data, 47687)
            .Mission(data, 979)
            .Points(500)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Western Soda-lime Tributary  Arthrolure Field Test
        data.Apply(47490, Patch.ThePromiseOfTomorrow) // White Starburst
            .Bait(data, 47589)
            .Mission(data, 980)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47491, Patch.ThePromiseOfTomorrow) // Skippingway
            .Bait(data, 47589)
            .Mission(data, 980)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47492, Patch.ThePromiseOfTomorrow) // Glass Coral
            .Bait(data, 47589)
            .Mission(data, 980)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47493, Patch.ThePromiseOfTomorrow) // Sun-purpled Tapestry
            .Bait(data, 47589)
            .Mission(data, 980)
            .Points(400)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47494, Patch.ThePromiseOfTomorrow) // Lime Impesctor
            .Bait(data, 47589)
            .Mission(data, 980)
            .Points(1000)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Lower Soda-lime Float  River Basin Large Aquatic Resources //TODO: Confirm Points
        data.Apply(47495, Patch.ThePromiseOfTomorrow) // Kissing Counterfeit
            .Bait(data, 47687)
            .Mission(data, 981)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47496, Patch.ThePromiseOfTomorrow) // Float Cloud
            .Bait(data, 47687)
            .Mission(data, 981)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47497, Patch.ThePromiseOfTomorrow) // Navy Coelacanth
            .Bait(data, 47687)
            .Mission(data, 981)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47498, Patch.ThePromiseOfTomorrow) // Iceglass Gar
            .Bait(data, 47687)
            .Mission(data, 981)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47499, Patch.ThePromiseOfTomorrow) // Moonstone Mora
            .Bait(data, 47687)
            .Mission(data, 981)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // The Prismatic Pull  Saltpeter Shore Sunken Resources //TODO: confirm Points
        data.Apply(47500, Patch.ThePromiseOfTomorrow) // Untitled Work No. 288
            .Bait(data, 47691)
            .Mission(data, 982)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47501, Patch.ThePromiseOfTomorrow) // Desertingway
            .Bait(data, 47691)
            .Mission(data, 982)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47502, Patch.ThePromiseOfTomorrow) // Goldflake Sandfish
            .Bait(data, 47691)
            .Mission(data, 982)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47503, Patch.ThePromiseOfTomorrow) // Sandglass Glob
            .Bait(data, 47691)
            .Mission(data, 982)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47504, Patch.ThePromiseOfTomorrow) // Prismatic Moraine
            .Bait(data, 47691)
            .Mission(data, 982)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // West Beaconveil  Mutant Aquatic Specimen Observations //Todo: Confirm Multihook
        data.Apply(47505, Patch.ThePromiseOfTomorrow) // Glass Stitcher
            .Bait(data, 47689)
            .Mission(data, 983)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47506, Patch.ThePromiseOfTomorrow) // Glass Discus
            .Bait(data, 47686)
            .Mission(data, 983)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47507, Patch.ThePromiseOfTomorrow) // Isosceles Amethyst
            .Bait(data, 47689)
            .Mission(data, 983)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47508, Patch.ThePromiseOfTomorrow) // Iceglass Floe
            .Bait(data, 47686)
            .Mission(data, 983)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47509, Patch.ThePromiseOfTomorrow) // Super Starburst
            .Bait(data, 47689)
            .Mission(data, 983)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Opalescent Crossing  Vitrified Freshwater Fish Observations
        data.Apply(47510, Patch.ThePromiseOfTomorrow) // Pearl Shell
            .Bait(data, 47688)
            .Mission(data, 984)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47511, Patch.ThePromiseOfTomorrow) // Cobalt Bijou
            .Bait(data, 47688)
            .Mission(data, 984)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47512, Patch.ThePromiseOfTomorrow) // Kintsugi Chip
            .Bait(data, 47688)
            .Mission(data, 984)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47513, Patch.ThePromiseOfTomorrow) // Opalescent Stingship
            .Bait(data, 47688)
            .Mission(data, 984)
            .MultiHook(3)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // The Prismatic Pull  Saltpeter Shore Large Resources //TODO: confirm multi //TODO: confirm points
        data.Apply(47514, Patch.ThePromiseOfTomorrow) // Untitled Work No. 288
            .Bait(data, 47690)
            .Mission(data, 985)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47515, Patch.ThePromiseOfTomorrow) // Desertingway
            .Bait(data, 47690)
            .Mission(data, 985)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47516, Patch.ThePromiseOfTomorrow) // Ammochronologist
            .Bait(data, 47690)
            .Mission(data, 985)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47517, Patch.ThePromiseOfTomorrow) // Polarized Eel
            .Bait(data, 47690)
            .Mission(data, 985)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47518, Patch.ThePromiseOfTomorrow) // Quicksand Mantaroid
            .Bait(data, 47690)
            .Mission(data, 985)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // The Lehr Cultivated Specimen Survey
        data.Apply(47519, Patch.ThePromiseOfTomorrow) // Macrobrachium Phaennense
            .Bait(data, 47693)
            .Mission(data, 986)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47520, Patch.ThePromiseOfTomorrow) // Eelsplorer
            .Bait(data, 47693)
            .Mission(data, 986)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47521, Patch.ThePromiseOfTomorrow) // Star Crabtain
            .Bait(data, 47693)
            .Mission(data, 986)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47522, Patch.ThePromiseOfTomorrow) // Lehr Killifish
            .Bait(data, 47693)
            .Mission(data, 986)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47523, Patch.ThePromiseOfTomorrow) // Doomed Voyager
            .Bait(data, 47693)
            .Mission(data, 986)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47524, Patch.ThePromiseOfTomorrow) // Bottom-layer
            .Bait(data, 47693)
            .Mission(data, 986)
            .Predators(data, 220, (47523, 2))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // West Beaconveil Stardust Bait Test
        data.Apply(47525, Patch.ThePromiseOfTomorrow) // Potash Guppish
            .Bait(data, 47692)
            .Mission(data, 987)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47526, Patch.ThePromiseOfTomorrow) // Aurora Shell
            .Bait(data, 47692)
            .Mission(data, 987)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47527, Patch.ThePromiseOfTomorrow) // Untitled Work No. 66
            .Bait(data, 47692)
            .Mission(data, 987)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47528, Patch.ThePromiseOfTomorrow) // Imitation Engraver
            .Bait(data, 47692)
            .Mission(data, 987)
            .Points(500)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47529, Patch.ThePromiseOfTomorrow) // Isosceles Pearl
            .Bait(data, 47692)
            .Mission(data, 987)
            .Points(1000)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Opalescent Crossing Elemental-esque Aquaculture Specimens
        data.Apply(47530, Patch.ThePromiseOfTomorrow) // Pearl Shell
            .Bait(data)
            .Mission(data, 988)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47531, Patch.ThePromiseOfTomorrow) // Cobalt Bijou
            .Bait(data)
            .Mission(data, 988)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47532, Patch.ThePromiseOfTomorrow) // Selenium Herring
            .Bait(data)
            .Mission(data, 988)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47533, Patch.ThePromiseOfTomorrow) // Codmonaut
            .Bait(data)
            .Mission(data, 988)
            .MultiHook(3)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47534, Patch.ThePromiseOfTomorrow) // Opalite Impesctor
            .Bait(data)
            .Mission(data, 988)
            .MultiHook(3)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // The Prismatic Pull Prismatic Pull Distribution Survey //TODO: Confirm Multi
        data.Apply(47535, Patch.ThePromiseOfTomorrow) // Untitled Work No. 288
            .Bait(data, 47697)
            .Mission(data, 989)
            .Points(50)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47536, Patch.ThePromiseOfTomorrow) // Desertingway
            .Bait(data, 47697)
            .Mission(data, 989)
            .Points(100)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47537, Patch.ThePromiseOfTomorrow) // Gold Broochback
            .Bait(data, 47697)
            .Mission(data, 989)
            .Points(150)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47538, Patch.ThePromiseOfTomorrow) // Nitric Chelid
            .Bait(data, 47697)
            .Mission(data, 989)
            .Points(400)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47539, Patch.ThePromiseOfTomorrow) // Sunken Kite
            .Bait(data, 47697)
            .Mission(data, 989)
            .Points(600)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Central Soda-lime Channel Aquatic Glass Resource Distribution Survey
        data.Apply(47540, Patch.ThePromiseOfTomorrow) // Glass Dahlia
            .Bait(data, 47695)
            .Mission(data, 990)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47541, Patch.ThePromiseOfTomorrow) // Fishy Jellyfish
            .Bait(data, 47695)
            .Mission(data, 990)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47542, Patch.ThePromiseOfTomorrow) // Suspect Skeletonfish
            .Bait(data, 47695)
            .Mission(data, 990)
            .Points(500)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47543, Patch.ThePromiseOfTomorrow) // Cobalt Glass Eel
            .Bait(data, 47695)
            .Mission(data, 990)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47544, Patch.ThePromiseOfTomorrow) // Cast Flowers
            .Bait(data, 47695)
            .Mission(data, 990)
            .Points(1250)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Western Soda-lime Tributary EX: Red Cosmomaggot Field Test //TODO: Confirm Points
        data.Apply(47556, Patch.ThePromiseOfTomorrow) // White Starburst
            .Bait(data, 47593)
            .Mission(data, 991)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47557, Patch.ThePromiseOfTomorrow) // Skippingway
            .Bait(data, 47593)
            .Mission(data, 991)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47558, Patch.ThePromiseOfTomorrow) // Glass Coral
            .Bait(data, 47593)
            .Mission(data, 991)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47559, Patch.ThePromiseOfTomorrow) // Blown Bubble
            .Bait(data, 47593)
            .Mission(data, 991)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47560, Patch.ThePromiseOfTomorrow) // Cobalt Horn
            .Bait(data, 47593)
            .Mission(data, 991)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Lower Soda-lime Float EX: Large River Resources //TODO: Confirm Multi //TODO: Confirm Points
        data.Apply(47561, Patch.ThePromiseOfTomorrow) // Kissing Counterfeit
            .Bait(data, 47695)
            .Mission(data, 992)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47562, Patch.ThePromiseOfTomorrow) // Float Cloud
            .Bait(data, 47695)
            .Mission(data, 992)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47563, Patch.ThePromiseOfTomorrow) // Navy Coelacanth
            .Bait(data, 47695)
            .Mission(data, 992)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47564, Patch.ThePromiseOfTomorrow) // Obsidian Mussel
            .Bait(data, 47695)
            .Mission(data, 992)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47565, Patch.ThePromiseOfTomorrow) // Glass Sculptor
            .Bait(data, 47695)
            .Mission(data, 992)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47566, Patch.ThePromiseOfTomorrow) // Prismatic Cluster
            .Bait(data, 47695)
            .Mission(data, 992)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Capsule Pools EX: Capsule Pools Distribution Survey
        data.Apply(47609, Patch.ThePromiseOfTomorrow) // Glass Guppish
            .Bait(data)
            .Mission(data, 993)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47610, Patch.ThePromiseOfTomorrow) // Nyctichthys
            .Bait(data)
            .Mission(data, 993)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47611, Patch.ThePromiseOfTomorrow) // Five-star Bijou
            .Bait(data)
            .Mission(data, 993)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47612, Patch.ThePromiseOfTomorrow) // Capsule Castoff
            .Bait(data)
            .Mission(data, 993)
            .Points(400)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47613, Patch.ThePromiseOfTomorrow) // Capsule Amber
            .Bait(data)
            .Mission(data, 993)
            .Points(500)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // The Prismatic Pull EX+: Large Saltpeter Shore Resources //TODO: Confirm Multi //TODO: Confirm Points
        data.Apply(47632, Patch.ThePromiseOfTomorrow) // Untitled Work No. 288
            .Bait(data)
            .Mission(data, 994)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47633, Patch.ThePromiseOfTomorrow) // Desertingway
            .Bait(data)
            .Mission(data, 994)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47634, Patch.ThePromiseOfTomorrow) // Sandglass Helm
            .Bait(data)
            .Mission(data, 994)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47635, Patch.ThePromiseOfTomorrow) // Reluctant Paperweight
            .Bait(data)
            .Mission(data, 994)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47636, Patch.ThePromiseOfTomorrow) // Glass Vajrajaw
            .Bait(data)
            .Mission(data, 994)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47637, Patch.ThePromiseOfTomorrow) // Saltpeter Rose
            .Bait(data)
            .Mission(data, 994)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Upper Soda-lime Float EX+: Stardust Bait Test //TODO: Confirm Points
        data.Apply(47638, Patch.ThePromiseOfTomorrow) // Frosted Shell
            .Bait(data, 47692)
            .Mission(data, 995)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47639, Patch.ThePromiseOfTomorrow) // Untitled Work No. 98
            .Bait(data, 47692)
            .Mission(data, 995)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47640, Patch.ThePromiseOfTomorrow) // Pendantfish
            .Bait(data, 47692)
            .Mission(data, 995)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47641, Patch.ThePromiseOfTomorrow) // Amethyst Bijou
            .Bait(data, 47692)
            .Mission(data, 995)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47642, Patch.ThePromiseOfTomorrow) // Frothfish
            .Bait(data, 47692)
            .Mission(data, 995)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47643, Patch.ThePromiseOfTomorrow) // Soda-blue Impesctor
            .Bait(data, 47692)
            .Mission(data, 995)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Upper Soda-lime Float Precision Lens Development Support //TODO: Confirm Points
        data.Apply(47545, Patch.ThePromiseOfTomorrow) // Frosted Shell
            .Bait(data, 47694)
            .Mission(data, 996)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47546, Patch.ThePromiseOfTomorrow) // Untitled Work No. 98
            .Bait(data, 47694)
            .Mission(data, 996)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47547, Patch.ThePromiseOfTomorrow) // Pendantfish
            .Bait(data, 47694)
            .Mission(data, 996)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47548, Patch.ThePromiseOfTomorrow) // Untitled Work No. 432
            .Bait(data, 47694)
            .Mission(data, 996)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47549, Patch.ThePromiseOfTomorrow) // Sodiment Licker
            .Bait(data, 47694)
            .Mission(data, 996)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47550, Patch.ThePromiseOfTomorrow) // Soda-lime Streamer
            .Bait(data, 47694)
            .Mission(data, 996)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Central Soda-lime Channel Hyper-aetheroconductive Materials //TODO: Confirm Multihook
        data.Apply(47567, Patch.ThePromiseOfTomorrow) // Glass Dahlia
            .Bait(data, 47692)
            .Mission(data, 997)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47568, Patch.ThePromiseOfTomorrow) // Fishy Jellyfish
            .Bait(data, 47692)
            .Mission(data, 997)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47569, Patch.ThePromiseOfTomorrow) // Decorative Disc
            .Bait(data, 47692)
            .Mission(data, 997)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47570, Patch.ThePromiseOfTomorrow) // Milky Irisfish
            .Bait(data, 47692)
            .Mission(data, 997)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47571, Patch.ThePromiseOfTomorrow) // Applebrow
            .Bait(data, 47692)
            .Mission(data, 997)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47572, Patch.ThePromiseOfTomorrow) // Untitled Work No. 765
            .Bait(data, 47692)
            .Mission(data, 997)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .MultiHook(2);

        // Central Soda-lime Channel EX: Large Aquatic Resources
        data.Apply(47551, Patch.ThePromiseOfTomorrow) // Glass Dahlia
            .Bait(data, 47693)
            .Mission(data, 998)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47552, Patch.ThePromiseOfTomorrow) // Fishy Jellyfish
            .Bait(data, 47693)
            .Mission(data, 998)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47553, Patch.ThePromiseOfTomorrow) // Limingway
            .Bait(data, 47693)
            .Mission(data, 998)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47554, Patch.ThePromiseOfTomorrow) // Gold Ruby Pansyfin
            .Bait(data, 47693)
            .Mission(data, 998)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47555, Patch.ThePromiseOfTomorrow) // Sinking Crystascute
            .Bait(data, 47693)
            .Mission(data, 998)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Western Soda-lime Tributary EX: Rare Aquatic Resources //TODO: Confirm Multihook
        data.Apply(47573, Patch.ThePromiseOfTomorrow) // White Starburst
            .Bait(data)
            .Mission(data, 999)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47574, Patch.ThePromiseOfTomorrow) // Skippingway
            .Bait(data)
            .Mission(data, 999)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47575, Patch.ThePromiseOfTomorrow) // Glass Coral
            .Bait(data)
            .Mission(data, 999)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47576, Patch.ThePromiseOfTomorrow) // Shallnot Shell
            .Bait(data)
            .Mission(data, 999)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47577, Patch.ThePromiseOfTomorrow) // Full-blown Bubble
            .Bait(data)
            .Mission(data, 999)
            .Predators(data, 160, (47576, 2))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Lower Soda-lime Float EX: Elemental-esque River Specimens //TODO: Confirm Multihook
        data.Apply(47614, Patch.ThePromiseOfTomorrow) // Kissing Counterfeit
            .Bait(data, 47692)
            .Mission(data, 1000)
            .Points(100)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47615, Patch.ThePromiseOfTomorrow) // Float Cloud
            .Bait(data, 47692)
            .Mission(data, 1000)
            .Points(100)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47616, Patch.ThePromiseOfTomorrow) // Navy Coelacanths
            .Bait(data, 47692)
            .Mission(data, 1000)
            .Points(200)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47617, Patch.ThePromiseOfTomorrow) // Filigree Floret
            .Bait(data, 47692)
            .Mission(data, 1000)
            .Points(50)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47618, Patch.ThePromiseOfTomorrow) // Soda Mocktail
            .Bait(data, 47692)
            .Mission(data, 1000)
            .Points(500)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47619, Patch.ThePromiseOfTomorrow) // Bubbling Impesctor
            .Bait(data, 47692)
            .Mission(data, 1000)
            .Points(1500)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Lower Soda-lime Float Lower Soda-lime Float Distribution Survey
        data.Apply(47579, Patch.ThePromiseOfTomorrow) // Kissing Counterfeit
            .Bait(data, 47692)
            .Mission(data, 1001)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47580, Patch.ThePromiseOfTomorrow) // Float Cloud
            .Bait(data, 47692)
            .Mission(data, 1001)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47581, Patch.ThePromiseOfTomorrow) // Navy Coelacanth
            .Bait(data, 47692)
            .Mission(data, 1001)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47582, Patch.ThePromiseOfTomorrow) // Untitled Work No. 345
            .Bait(data, 47692)
            .Mission(data, 1001)
            .Points(200)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47583, Patch.ThePromiseOfTomorrow) // Royal Comet tail
            .Bait(data, 47692)
            .Mission(data, 1001)
            .Points(360)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47584, Patch.ThePromiseOfTomorrow) // Grim Impesctor
            .Bait(data, 47692)
            .Mission(data, 1001)
            .Points(440)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Bubble Bursters EX: Bubble Bursters Distribution Survey
        data.Apply(47626, Patch.ThePromiseOfTomorrow) // Bursting Anemone
            .Bait(data)
            .Mission(data, 1002)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47627, Patch.ThePromiseOfTomorrow) // Bubble Eater
            .Bait(data)
            .Mission(data, 1002)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47628, Patch.ThePromiseOfTomorrow) // Phainofly Fish
            .Bait(data)
            .Mission(data, 1002)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47629, Patch.ThePromiseOfTomorrow) // Bubblegazer
            .Bait(data)
            .Mission(data, 1002)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47630, Patch.ThePromiseOfTomorrow) // Cthonic Tapestry
            .Bait(data)
            .Mission(data, 1002)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47631, Patch.ThePromiseOfTomorrow) // Chasm Avatar
            .Bait(data)
            .Mission(data, 1002)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Fusingway Flow EX+: Fusingway Flow Distribution Survey //TODO: Confirm Multihook, Unfinished Data, This fish is labeled wrong?!
        data.Apply(47650, Patch.ThePromiseOfTomorrow) // Shearclaw
            .Bait(data)
            .Mission(data, 1003)
            .Bite(data, HookSet.Unknown, BiteType.Weak);
        data.Apply(47651, Patch.ThePromiseOfTomorrow) // Bellows Crab
            .Bait(data)
            .Mission(data, 1003)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47652, Patch.ThePromiseOfTomorrow) // Crimplouse
            .Bait(data)
            .Mission(data, 1003)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47653, Patch.ThePromiseOfTomorrow) // Pipetongue
            .Bait(data)
            .Mission(data, 1003)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47654, Patch.ThePromiseOfTomorrow) // Specious Shark <- ????? Garnet Seahorse????
            .Bait(data)
            .Mission(data, 1003)
            .MultiHook(3)
            .Bite(data, HookSet.Unknown, BiteType.Weak);
        data.Apply(47655, Patch.ThePromiseOfTomorrow) // Vent Impesctor
            .Bait(data)
            .Mission(data, 1003)
            .MultiHook(4)
            .Bite(data, HookSet.Precise, BiteType.Legendary);

        // The Prismatic Pull EX+: Elemental-esque Saltpeter Shore Specimens //TODO: Unfinished Data
        data.Apply(47668, Patch.ThePromiseOfTomorrow) // Untitled Work No. 288
            .Bait(data)
            .Mission(data, 1004)
            .Points(200)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(47669, Patch.ThePromiseOfTomorrow) // Desertingway
            .Bait(data)
            .Mission(data, 1004)
            .MultiHook(2)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(47670, Patch.ThePromiseOfTomorrow) // Mosaic Bricklayer
            .Bait(data)
            .Mission(data, 1004)
            .Points(600)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(47671, Patch.ThePromiseOfTomorrow) // Sandglass Slasher
            .Bait(data)
            .Mission(data, 1004)
            .Points(200)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47672, Patch.ThePromiseOfTomorrow) // Garbled Gar
            .Bait(data)
            .Mission(data, 1004)
            .Points(2000)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47673, Patch.ThePromiseOfTomorrow) // Sandwyrm
            .Bait(data)
            .Mission(data, 1004)
            .Points(10000)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Capsule Pools EX+: Elemental-esque Chasm Specimens //TODO:Unfinished Data
        data.Apply(47656, Patch.ThePromiseOfTomorrow) // Glass Guppish
            .Bait(data)
            .Mission(data, 1005)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47657, Patch.ThePromiseOfTomorrow) // Nyctichthys
            .Bait(data)
            .Mission(data, 1005)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47658, Patch.ThePromiseOfTomorrow) // False Tetraform
            .Bait(data)
            .Mission(data, 1005)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47659, Patch.ThePromiseOfTomorrow) // Capsulette
            .Bait(data)
            .Mission(data, 1005)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47660, Patch.ThePromiseOfTomorrow) // Glass Nib
            .Bait(data)
            .Mission(data, 1005)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47661, Patch.ThePromiseOfTomorrow) // Encapsulated Impesctor
            .Bait(data)
            .Mission(data, 1005)
            .Predators(data, 420, (47659,2))
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Fusingway Flow EX+: Elemental-esque Vent Specimens //TODO: Unifinished Data, This is missing a fish?!
        data.Apply(47662, Patch.ThePromiseOfTomorrow) // Shearclaw
            .Bait(data)
            .Mission(data, 1006)
            .Bite(data, HookSet.Unknown, BiteType.Weak);
        data.Apply(47663, Patch.ThePromiseOfTomorrow) // Bellows Crab
            .Bait(data)
            .Mission(data, 1006)
            .Bite(data, HookSet.Unknown, BiteType.Weak);
        data.Apply(47664, Patch.ThePromiseOfTomorrow) // Crimplouse
            .Bait(data)
            .Mission(data, 1006)
            .Bite(data, HookSet.Unknown, BiteType.Strong);
        data.Apply(47665, Patch.ThePromiseOfTomorrow) // Garnet Seahorse <- Why are u here, perhaps Specious Shark?! Seems to be confusion in fishcord too....
            .Bait(data)
            .Mission(data, 1006)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(47666, Patch.ThePromiseOfTomorrow) // Confusing Catfusion
            .Bait(data)
            .Mission(data, 1006)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(47667, Patch.ThePromiseOfTomorrow) // Fused Animus
            .Bait(data)
            .Mission(data, 1006)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // The Lehr  Soup Broth Ingredients
        data.Apply(47674, Patch.ThePromiseOfTomorrow) // Starfaring Goby
            .Bait(data, 47702)
            .Mission(data, 1037)
            .MultiHook(2)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47675, Patch.ThePromiseOfTomorrow) // Initiative Trout
            .Bait(data, 47702)
            .Mission(data, 1037)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Opalescent Crossing  Oil-extractable Aquatic Life
        data.Apply(47676, Patch.ThePromiseOfTomorrow) // Faux Cloudfish
            .Bait(data)
            .Mission(data, 1038)
            .MultiHook(2)
            .Bite(data, HookSet.Unknown, BiteType.Weak);
        data.Apply(47677, Patch.ThePromiseOfTomorrow) // Trailing Snailfish
            .Bait(data)
            .Mission(data, 1038)
            .MultiHook(2)
            .Bite(data, HookSet.Unknown, BiteType.Strong);

        // Bubble Bursters  Elemental-esque Specimen Acquisition
        data.Apply(47678, Patch.ThePromiseOfTomorrow) // Bursting Anemone
            .Bait(data)
            .Mission(data, 1039)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47679, Patch.ThePromiseOfTomorrow) // Midnight Impression
            .Bait(data)
            .Mission(data, 1039)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(47680, Patch.ThePromiseOfTomorrow) // Blue Starburst
            .Bait(data)
            .Mission(data, 1039)
            .MultiHook(2)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
    }
    // @formatter:on
}


