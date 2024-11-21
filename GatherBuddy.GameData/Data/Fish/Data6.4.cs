using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyTheDarkThrone(this GameData data)
    {
        data.Apply(39809, Patch.TheDarkThrone) // Gold Dustfish
            .Bait(data, 38808)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .ForceBig(false);
        data.Apply(39810, Patch.TheDarkThrone) // Forgiven Melancholy
            .Bait(data, 38808)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .ForceBig(false);
        data.Apply(39815, Patch.TheDarkThrone) // Oil Slick
            .Bait(data, 38808)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .ForceBig(false);
        data.Apply(39816, Patch.TheDarkThrone) // Gonzalo's Grace
            .Bait(data, 38808)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .ForceBig(false);
        data.Apply(39879, Patch.TheDarkThrone) // Onyx Knifefish
            .Bait(data, 36591)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Time(1320, 120)
            .Transition(data, 7)
            .Weather(data, 2);
        data.Apply(39880, Patch.TheDarkThrone) // Wakeful Warden
            .Bait(data, 36589)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 4)
            .Weather(data, 3);
        data.Apply(39881, Patch.TheDarkThrone) // Basilosaurus Rex
            .Mooch(data, 36454)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 2);
        data.Apply(39882, Patch.TheDarkThrone) // Eehs Fan
            .Bait(data, 36595)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Time(720, 1200)
            .Transition(data, 149)
            .Weather(data, 49);
        data.Apply(39883, Patch.TheDarkThrone) // Gilt Dermogenys
            .Bait(data, 36590)
            .Bite(data, HookSet.Precise, BiteType.Legendary)
            .Time(1200, 1320)
            .Transition(data, 49)
            .Weather(data, 1, 2);
        data.Apply(39912, Patch.TheDarkThrone) // The Fury's Aegis
            .Bait(data, 36589)
            .Time(960, 1140)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40521, Patch.TheDarkThrone) // Pink Shrimp
            .Bait(data, 29715)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40522, Patch.TheDarkThrone) // Sirensong Mussel
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40523, Patch.TheDarkThrone) // Arrowhead
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40524, Patch.TheDarkThrone) // Deepshade Sardine
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 2, 3, 4, 1);
        data.Apply(40525, Patch.TheDarkThrone) // Sirensong Mullet
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 2, 3, 4, 7, 1);
        data.Apply(40526, Patch.TheDarkThrone) // Selkie Puffer
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Weather(data, 2, 7, 10, 1);
        data.Apply(40527, Patch.TheDarkThrone) // Poet's Pipe
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40528, Patch.TheDarkThrone) // Marine Matanga
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(40529, Patch.TheDarkThrone) // Spectral Coelacanth
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 2, 3, 4, 7, 10);
        data.Apply(40530, Patch.TheDarkThrone) // Dusk Shark
            .Bait(data, 29716)
            .Predators (data, 60, (40527, 2))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(40531, Patch.TheDarkThrone) // Mermaid Scale
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40532, Patch.TheDarkThrone) // Broadhead
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Day, OceanTime.Sunset);
        data.Apply(40533, Patch.TheDarkThrone) // Vivid Pink Shrimp
            .Bait(data, 29715)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Night);
        data.Apply(40534, Patch.TheDarkThrone) // Sunken Coelacanth
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(40535, Patch.TheDarkThrone) // Siren's Sigh
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40536, Patch.TheDarkThrone) // Black-jawed Helicoprion
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Night);
        data.Apply(40537, Patch.TheDarkThrone) // Impostopus
            .Bait(data, 29715)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40538, Patch.TheDarkThrone) // Jade Shrimp
            .Bait(data, 29715)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40539, Patch.TheDarkThrone) // Nymeia's Wheel
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Sunset);
        data.Apply(40540, Patch.TheDarkThrone) // Taniwha
            .Bait(data, 36593)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 15, (40534, 3))
            .Ocean(OceanTime.Day);
        data.Apply(40541, Patch.TheDarkThrone) // Ruby Herring
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 2, 3, 4, 1);
        data.Apply(40542, Patch.TheDarkThrone) // Whirpool Turban
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40543, Patch.TheDarkThrone) // Leopard Prawn
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40544, Patch.TheDarkThrone) // Spear Squid
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40545, Patch.TheDarkThrone) // Floating Lantern
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 2, 3, 4, 7, 1);
        data.Apply(40546, Patch.TheDarkThrone) // Rubescent Tatsunoko
            .Bait(data, 29715)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Weather(data, 2, 7, 8, 1);
        data.Apply(40547, Patch.TheDarkThrone) // Hatatate
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40548, Patch.TheDarkThrone) // Silent Shark
            .Mooch(data, 29714, 40543)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(40549, Patch.TheDarkThrone) // Spectral Wrasse
            .Bait(data, 29714)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 2, 3, 4, 7, 8);
        data.Apply(40550, Patch.TheDarkThrone) // Mizuhiki
            .Bait(data, 29714)
            .Predators (data, 60, (40548, 2))
            .Bite(data, HookSet.Precise, BiteType.Legendary);
        data.Apply(40551, Patch.TheDarkThrone) // Snapping Koban
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40552, Patch.TheDarkThrone) // Silkweft Prawn
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40553, Patch.TheDarkThrone) // Stingfin Trevally
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Day);
        data.Apply(40554, Patch.TheDarkThrone) // Swordtip Squid
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40555, Patch.TheDarkThrone) // Mailfish
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Sunset);
        data.Apply(40556, Patch.TheDarkThrone) // Idaten's Bolt
            .Bait(data, 29715)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40557, Patch.TheDarkThrone) // Maelstrom Turban
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Day, OceanTime.Sunset);
        data.Apply(40558, Patch.TheDarkThrone) // Shoshitsuki
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(40559, Patch.TheDarkThrone) // Spadefish
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Sunset);
        data.Apply(40560, Patch.TheDarkThrone) // Glass Dragon
            .Mooch(data, 29715, 40551)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 45, (40558, 2))
            .Ocean(OceanTime.Night);
        data.Apply(40561, Patch.TheDarkThrone) // Crimson Kelp
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40562, Patch.TheDarkThrone) // Reef Squid
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40563, Patch.TheDarkThrone) // Pinebark Flounder
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Weather(data, 2, 3, 4, 6, 9, 1);
        data.Apply(40564, Patch.TheDarkThrone) // Mantle Moray
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 2, 3, 4, 5, 6, 1);
        data.Apply(40565, Patch.TheDarkThrone) // Shisui Goby
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40566, Patch.TheDarkThrone) // Sanbaso
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 2, 5, 6, 9, 1);
        data.Apply(40567, Patch.TheDarkThrone) // Barded Lobster
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40568, Patch.TheDarkThrone) // Violet Sentry
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(40569, Patch.TheDarkThrone) // Spectral Snake Eel
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 2, 3, 4, 5, 6, 9);
        data.Apply(40570, Patch.TheDarkThrone) // Heavensent Shark
            .Bait(data, 29715)
            .Predators (data, 60, (40561, 3))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(40571, Patch.TheDarkThrone) // Fleeting Squid
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40572, Patch.TheDarkThrone) // Bowbarb Lobster
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40573, Patch.TheDarkThrone) // Pitch Pickle
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Day);
        data.Apply(40574, Patch.TheDarkThrone) // Senbei Octopus
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40575, Patch.TheDarkThrone) // Tentacle Thresher
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40576, Patch.TheDarkThrone) // Bekko Rockhugger
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Night);
        data.Apply(40577, Patch.TheDarkThrone) // Yellow Iris
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Day);
        data.Apply(40578, Patch.TheDarkThrone) // Crimson Sentry
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Night);
        data.Apply(40579, Patch.TheDarkThrone) // Flying Squid
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40580, Patch.TheDarkThrone) // Hells' Claw
            .Bait(data, 27590)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 15, (40571, 2), (40579, 1))
            .Ocean(OceanTime.Sunset);
        data.Apply(40581, Patch.TheDarkThrone) // Catching Carp
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40582, Patch.TheDarkThrone) // Garlean Bluegill
            .Bait(data, 29715)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Weather(data, 2, 3, 4, 1);
        data.Apply(40583, Patch.TheDarkThrone) // Yanxian Softshell
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40584, Patch.TheDarkThrone) // Princess Salmon
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Weather(data, 2, 7, 8, 1);
        data.Apply(40585, Patch.TheDarkThrone) // Calligraph
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Weather(data, 2, 3, 4, 7, 1);
        data.Apply(40586, Patch.TheDarkThrone) // Singular Shrimp
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40587, Patch.TheDarkThrone) // Brocade Carp
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40588, Patch.TheDarkThrone) // Yanxian Sturgeon
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(40589, Patch.TheDarkThrone) // Spectral Kotsu Zetsu
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 2, 3, 4, 7, 8);
        data.Apply(40590, Patch.TheDarkThrone) // Fishy Shark
            .Bait(data, 29716)
            .Predators (data, 60, (40581, 3))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(40591, Patch.TheDarkThrone) // Gensui Shrimp
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40592, Patch.TheDarkThrone) // Yato-no-kami
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40593, Patch.TheDarkThrone) // Heron's Eel
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(40594, Patch.TheDarkThrone) // Crowshadow Mussel
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40595, Patch.TheDarkThrone) // Yanxian Goby
            .Bait(data, 29714)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(40596, Patch.TheDarkThrone) // Iridescent Trout
            .Bait(data, 29715)
            .Bite(data, HookSet.Precise, BiteType.Weak)
            .Ocean(OceanTime.Sunset);
        data.Apply(40597, Patch.TheDarkThrone) // Un-Namazu
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Night);
        data.Apply(40598, Patch.TheDarkThrone) // Gakugyo
            .Bait(data, 29716)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Ocean(OceanTime.Night);
        data.Apply(40599, Patch.TheDarkThrone) // Ginrin Goshiki
            .Bait(data, 29715)
            .Bite(data, HookSet.Powerful, BiteType.Strong)
            .Ocean(OceanTime.Sunset);
        data.Apply(40600, Patch.TheDarkThrone) // Jewel of Plum Spring
            .Bait(data, 12704)
            .Bite(data, HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 15, (40595, 2), (40591, 1))
            .Ocean(OceanTime.Day);
    }
    // @formatter:on
}
