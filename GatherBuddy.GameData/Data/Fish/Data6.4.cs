using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyTheDarkThrone(this GameData data)
    {
        data.Apply(39879, Patch.TheDarkThrone) // Onyx Knifefish
            .Bait(data, 36591)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Time(1320, 120)
            .Transition(data, 7)
            .Weather(data, 2);
        data.Apply(39880, Patch.TheDarkThrone) // Wakeful Warden
            .Bait(data, 36589)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Time(480, 1440)
            .Transition(data, 4)
            .Weather(data, 3);
        data.Apply(39881, Patch.TheDarkThrone) // Basilosaurus Rex
            .Bait(data, 36593, 36451, 36454)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Weather(data, 2);
        data.Apply(39882, Patch.TheDarkThrone) // Eehs Fan
            .Bait(data, 36595)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Time(720, 1200)
            .Transition(data, 149)
            .Weather(data, 49);
        data.Apply(39883, Patch.TheDarkThrone) // Gilt Dermogenys
            .Bait(data, 36590)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Time(1200, 1320)
            .Transition(data, 49)
            .Weather(data, 1, 2);
        data.Apply(39912, Patch.TheDarkThrone) // The Fury's Aegis
            .Bait(data, 36589)
            .Time(960, 1140)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40521, Patch.TheDarkThrone) // Pink Shrimp
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40522, Patch.TheDarkThrone) // Sirensong Mussel
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40523, Patch.TheDarkThrone) // Arrowhead
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40524, Patch.TheDarkThrone) // Deepshade Sardine
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40525, Patch.TheDarkThrone) // Sirensong Mullet
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40526, Patch.TheDarkThrone) // Selkie Puffer
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40527, Patch.TheDarkThrone) // Poet's Pipe
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40528, Patch.TheDarkThrone) // Marine Matanga
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40529, Patch.TheDarkThrone) // Spectral Coelacanth
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40530, Patch.TheDarkThrone) // Dusk Shark
            .Bait(data, 29716)
            .Predators (data, 60, (40527, 2))
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40531, Patch.TheDarkThrone) // Mermaid Scale
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40532, Patch.TheDarkThrone) // Broadhead
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40533, Patch.TheDarkThrone) // Vivid Pink Shrimp
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40534, Patch.TheDarkThrone) // Sunken Coelacanth
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40535, Patch.TheDarkThrone) // Siren's Sigh
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40536, Patch.TheDarkThrone) // Black-jawed Helicoprion
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40537, Patch.TheDarkThrone) // Impostopus
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40538, Patch.TheDarkThrone) // Jade Shrimp
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40539, Patch.TheDarkThrone) // Nymeia's Wheel
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40540, Patch.TheDarkThrone) // Taniwha
            .Bait(data, 36593)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Predators (data, 15, (40534, 3))
            .Ocean(OceanTime.Day);
        data.Apply(40541, Patch.TheDarkThrone) // Ruby Herring
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40542, Patch.TheDarkThrone) // Whirpool Turban
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40543, Patch.TheDarkThrone) // Leopard Prawn
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40544, Patch.TheDarkThrone) // Spear Squid
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40545, Patch.TheDarkThrone) // Floating Lantern
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40546, Patch.TheDarkThrone) // Rubescent Tatsunoko
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40547, Patch.TheDarkThrone) // Hatatate
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40548, Patch.TheDarkThrone) // Silent Shark
            .Bait(data, 29714, 40543)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40549, Patch.TheDarkThrone) // Spectral Wrasse
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40550, Patch.TheDarkThrone) // Mizuhiki
            .Bait(data, 29714)
            .Predators (data, 60, (40548, 2))
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40551, Patch.TheDarkThrone) // Snapping Koban
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40552, Patch.TheDarkThrone) // Silkweft Prawn
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40553, Patch.TheDarkThrone) // Stingfin Trevally
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40554, Patch.TheDarkThrone) // Swordtip Squid
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40555, Patch.TheDarkThrone) // Mailfish
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40556, Patch.TheDarkThrone) // Idaten's Bolt
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40557, Patch.TheDarkThrone) // Maelstrom Turban
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40558, Patch.TheDarkThrone) // Shoshitsuki
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40559, Patch.TheDarkThrone) // Spadefish
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40560, Patch.TheDarkThrone) // Glass Dragon
            .Bait(data, 29715, 40551)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Predators (data, 45, (40558, 2))
            .Ocean(OceanTime.Night);
        data.Apply(40561, Patch.TheDarkThrone) // Crimson Kelp
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40562, Patch.TheDarkThrone) // Reef Squid
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40563, Patch.TheDarkThrone) // Pinebark Flounder
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40564, Patch.TheDarkThrone) // Mantle Moray
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40565, Patch.TheDarkThrone) // Shisui Goby
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40566, Patch.TheDarkThrone) // Sanbaso
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40567, Patch.TheDarkThrone) // Barded Lobster
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40568, Patch.TheDarkThrone) // Violet Sentry
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40569, Patch.TheDarkThrone) // Spectral Snake Eel
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40570, Patch.TheDarkThrone) // Heavensent Shark
            .Bait(data, 29715)
            .Predators (data, 60, (40561, 3))
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40571, Patch.TheDarkThrone) // Fleeting Squid
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40572, Patch.TheDarkThrone) // Bowbarb Lobster
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40573, Patch.TheDarkThrone) // Pitch Pickle
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40574, Patch.TheDarkThrone) // Senbei Octopus
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40575, Patch.TheDarkThrone) // Tentacle Thresher
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40576, Patch.TheDarkThrone) // Bekko Rockhugger
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40577, Patch.TheDarkThrone) // Yellow Iris
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40578, Patch.TheDarkThrone) // Crimson Sentry
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40579, Patch.TheDarkThrone) // Flying Squid
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40580, Patch.TheDarkThrone) // Hells' Claw
            .Bait(data, 27590)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Predators (data, 15, (40571, 2), (40579, 1))
            .Ocean(OceanTime.Sunset);
        data.Apply(40581, Patch.TheDarkThrone) // Catching Carp
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40582, Patch.TheDarkThrone) // Garlean Bluegill
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40583, Patch.TheDarkThrone) // Yanxian Softshell
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40584, Patch.TheDarkThrone) // Princess Salmon
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40585, Patch.TheDarkThrone) // Calligraph
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40586, Patch.TheDarkThrone) // Singular Shrimp
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40587, Patch.TheDarkThrone) // Brocade Carp
            .Bait(data)
            .Bite(HookSet.Powerful, BiteType.Strong);
        data.Apply(40588, Patch.TheDarkThrone) // Yanxian Sturgeon
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40589, Patch.TheDarkThrone) // Spectral Kotsu Zetsu
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40590, Patch.TheDarkThrone) // Fishy Shark
            .Bait(data, 29716)
            .Predators (data, 60, (40581, 2))
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40591, Patch.TheDarkThrone) // Gensui Shrimp
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40592, Patch.TheDarkThrone) // Yato-no-kami
            .Bait(data);
        data.Apply(40593, Patch.TheDarkThrone) // Heron's Eel
            .Bait(data);
        data.Apply(40594, Patch.TheDarkThrone) // Crowshadow Mussel
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40595, Patch.TheDarkThrone) // Yanxian Goby
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40596, Patch.TheDarkThrone) // Iridescent Trout
            .Bait(data)
            .Bite(HookSet.Precise, BiteType.Weak);
        data.Apply(40597, Patch.TheDarkThrone) // Un-Namazu
            .Bait(data);
        data.Apply(40598, Patch.TheDarkThrone) // Gakugyo
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(40599, Patch.TheDarkThrone) // Ginrin Goshiki
            .Bait(data);
        data.Apply(40600, Patch.TheDarkThrone) // Jewel of Plum Spring
            .Bait(data, 12704)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Predators (data, 15, (40595, 2), (40591, 1))
            .Ocean(OceanTime.Day);
    }
    // @formatter:on
}
