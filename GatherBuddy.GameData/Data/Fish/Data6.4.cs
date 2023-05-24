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
            .Time(1320, 1440)
            .Transition(data, 7)
            .Weather(data, 2);
        data.Apply(39880, Patch.TheDarkThrone) // Wakeful Warden
            .Bait(data, 36589)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Time(480, 960)
            .Transition(data, 4)
            .Weather(data, 3);
        data.Apply(39881, Patch.TheDarkThrone) // Basilosaurus Rex
            .Bait(data, 36593, 36451, 36454)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Weather(data, 2);
        data.Apply(39882, Patch.TheDarkThrone) // Eehs Fan
            .Bait(data, 36595)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Time(720, 960)
            .Transition(data, 149)
            .Weather(data, 49);
        data.Apply(39883, Patch.TheDarkThrone) // Gilt Dermogenys
            .Bait(data)
            .Bite(HookSet.Unknown, BiteType.Legendary);
        data.Apply(39912, Patch.TheDarkThrone) // The Fury's Aegis
            .Bait(data);
        data.Apply(40521, Patch.TheDarkThrone) // Pink Shrimp
            .Bait(data);
        data.Apply(40522, Patch.TheDarkThrone) // Sirensong Mussel
            .Bait(data);
        data.Apply(40523, Patch.TheDarkThrone) // Arrowhead
            .Bait(data);
        data.Apply(40524, Patch.TheDarkThrone) // Deepshade Sardine
            .Bait(data);
        data.Apply(40525, Patch.TheDarkThrone) // Sirensong Mullet
            .Bait(data);
        data.Apply(40526, Patch.TheDarkThrone) // Selkie Puffer
            .Bait(data);
        data.Apply(40527, Patch.TheDarkThrone) // Poet's Pipe
            .Bait(data);
        data.Apply(40528, Patch.TheDarkThrone) // Marine Matanga
            .Bait(data);
        data.Apply(40529, Patch.TheDarkThrone) // Spectral Coelacanth
            .Bait(data);
        data.Apply(40530, Patch.TheDarkThrone) // Dusk Shark
            .Bait(data);
        data.Apply(40531, Patch.TheDarkThrone) // Mermaid Scale
            .Bait(data);
        data.Apply(40532, Patch.TheDarkThrone) // Broadhead
            .Bait(data);
        data.Apply(40533, Patch.TheDarkThrone) // Vivid Pink Shrimp
            .Bait(data);
        data.Apply(40534, Patch.TheDarkThrone) // Sunken Coelacanth
            .Bait(data);
        data.Apply(40535, Patch.TheDarkThrone) // Siren's Sigh
            .Bait(data);
        data.Apply(40536, Patch.TheDarkThrone) // Black-jawed Helicoprion
            .Bait(data);
        data.Apply(40537, Patch.TheDarkThrone) // Impostopus
            .Bait(data);
        data.Apply(40538, Patch.TheDarkThrone) // Jade Shrimp
            .Bait(data);
        data.Apply(40539, Patch.TheDarkThrone) // Nymeia's Wheel
            .Bait(data);
        data.Apply(40540, Patch.TheDarkThrone) // Taniwha
            .Bait(data);
        data.Apply(40541, Patch.TheDarkThrone) // Ruby Herring
            .Bait(data);
        data.Apply(40542, Patch.TheDarkThrone) // Whirpool Turban
            .Bait(data);
        data.Apply(40543, Patch.TheDarkThrone) // Leopard Prawn
            .Bait(data);
        data.Apply(40544, Patch.TheDarkThrone) // Spear Squid
            .Bait(data);
        data.Apply(40545, Patch.TheDarkThrone) // Floating Lantern
            .Bait(data);
        data.Apply(40546, Patch.TheDarkThrone) // Rubescent Tatsunoko
            .Bait(data);
        data.Apply(40547, Patch.TheDarkThrone) // Hatatate
            .Bait(data);
        data.Apply(40548, Patch.TheDarkThrone) // Silent Shark
            .Bait(data);
        data.Apply(40549, Patch.TheDarkThrone) // Spectral Wrasse
            .Bait(data);
        data.Apply(40550, Patch.TheDarkThrone) // Mizuhiki
            .Bait(data);
        data.Apply(40551, Patch.TheDarkThrone) // Snapping Koban
            .Bait(data);
        data.Apply(40552, Patch.TheDarkThrone) // Silkweft Prawn
            .Bait(data);
        data.Apply(40553, Patch.TheDarkThrone) // Stingfin Trevally
            .Bait(data);
        data.Apply(40554, Patch.TheDarkThrone) // Swordtip Squid
            .Bait(data);
        data.Apply(40555, Patch.TheDarkThrone) // Mailfish
            .Bait(data);
        data.Apply(40556, Patch.TheDarkThrone) // Idaten's Bolt
            .Bait(data);
        data.Apply(40557, Patch.TheDarkThrone) // Maelstrom Turban
            .Bait(data);
        data.Apply(40558, Patch.TheDarkThrone) // Shoshitsuki
            .Bait(data);
        data.Apply(40559, Patch.TheDarkThrone) // Spadefish
            .Bait(data);
        data.Apply(40560, Patch.TheDarkThrone) // Glass Dragon
            .Bait(data);
        data.Apply(40561, Patch.TheDarkThrone) // Crimson Kelp
            .Bait(data);
        data.Apply(40562, Patch.TheDarkThrone) // Reef Squid
            .Bait(data);
        data.Apply(40563, Patch.TheDarkThrone) // Pinebark Flounder
            .Bait(data);
        data.Apply(40564, Patch.TheDarkThrone) // Mantle Moray
            .Bait(data);
        data.Apply(40565, Patch.TheDarkThrone) // Shisui Goby
            .Bait(data);
        data.Apply(40566, Patch.TheDarkThrone) // Sanbaso
            .Bait(data);
        data.Apply(40567, Patch.TheDarkThrone) // Barded Lobster
            .Bait(data);
        data.Apply(40568, Patch.TheDarkThrone) // Violet Sentry
            .Bait(data);
        data.Apply(40569, Patch.TheDarkThrone) // Spectral Snake Eel
            .Bait(data);
        data.Apply(40570, Patch.TheDarkThrone) // Heavensent Shark
            .Bait(data);
        data.Apply(40571, Patch.TheDarkThrone) // Fleeting Squid
            .Bait(data);
        data.Apply(40572, Patch.TheDarkThrone) // Bowbarb Lobster
            .Bait(data);
        data.Apply(40573, Patch.TheDarkThrone) // Pitch Pickle
            .Bait(data);
        data.Apply(40574, Patch.TheDarkThrone) // Senbei Octopus
            .Bait(data);
        data.Apply(40575, Patch.TheDarkThrone) // Tentacle Thresher
            .Bait(data);
        data.Apply(40576, Patch.TheDarkThrone) // Bekko Rockhugger
            .Bait(data);
        data.Apply(40577, Patch.TheDarkThrone) // Yellow Iris
            .Bait(data);
        data.Apply(40578, Patch.TheDarkThrone) // Crimson Sentry
            .Bait(data);
        data.Apply(40579, Patch.TheDarkThrone) // Flying Squid
            .Bait(data);
        data.Apply(40580, Patch.TheDarkThrone) // Hells' Claw
            .Bait(data);
        data.Apply(40581, Patch.TheDarkThrone) // Catching Carp
            .Bait(data);
        data.Apply(40582, Patch.TheDarkThrone) // Garlean Bluegill
            .Bait(data);
        data.Apply(40583, Patch.TheDarkThrone) // Yanxian Softshell
            .Bait(data);
        data.Apply(40584, Patch.TheDarkThrone) // Princess Salmon
            .Bait(data);
        data.Apply(40585, Patch.TheDarkThrone) // Calligraph
            .Bait(data);
        data.Apply(40586, Patch.TheDarkThrone) // Singular Shrimp
            .Bait(data);
        data.Apply(40587, Patch.TheDarkThrone) // Brocade Carp
            .Bait(data);
        data.Apply(40588, Patch.TheDarkThrone) // Yanxian Sturgeon
            .Bait(data);
        data.Apply(40589, Patch.TheDarkThrone) // Spectral Kotsu Zetsu
            .Bait(data);
        data.Apply(40590, Patch.TheDarkThrone) // Fishy Shark
            .Bait(data);
        data.Apply(40591, Patch.TheDarkThrone) // Gensui Shrimp
            .Bait(data);
        data.Apply(40592, Patch.TheDarkThrone) // Yato-no-kami
            .Bait(data);
        data.Apply(40593, Patch.TheDarkThrone) // Heron's Eel
            .Bait(data);
        data.Apply(40594, Patch.TheDarkThrone) // Crowshadow Mussel
            .Bait(data);
        data.Apply(40595, Patch.TheDarkThrone) // Yanxian Goby
            .Bait(data);
        data.Apply(40596, Patch.TheDarkThrone) // Iridescent Trout
            .Bait(data);
        data.Apply(40597, Patch.TheDarkThrone) // Un-Namazu
            .Bait(data);
        data.Apply(40598, Patch.TheDarkThrone) // Gakugyo
            .Bait(data);
        data.Apply(40599, Patch.TheDarkThrone) // Ginrin Goshiki
            .Bait(data);
        data.Apply(40600, Patch.TheDarkThrone) // Jewel of Plum Spring
            .Bait(data);
    }
    // @formatter:on
}
