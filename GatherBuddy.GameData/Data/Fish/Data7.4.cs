using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyIntoTheMist(this GameData data)
    {
        data.Apply(49794, Patch.IntoTheMist) // Purse of Riches
            .Bait(data, 43859)
            .Time(960, 1080)
            .Weather(data, 7)
            .Transition(data, 3)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49795, Patch.IntoTheMist) // Punutiy Pain
            .Mooch(data, 43701)
            .Time(480, 720)
            .Weather(data, 7)
            .Transition(data, 3)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49796, Patch.IntoTheMist) // Shuckfin Dace
            .Bait(data, 43858)
            .Time(240, 360)
            .Weather(data, 7)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49797, Patch.IntoTheMist) // Shin Snuffler
            .Bait(data, 43858)
            .Time(0, 120)
            .Weather(data, 4)
            .Predators(data, 300, (43735, 3))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49798, Patch.IntoTheMist) // Moxutural Greatgar
            .Mooch(data, 43736)
            .Time(1200, 1320)
            .Weather(data, 7)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49799, Patch.IntoTheMist) // Heirloom Goldgrouper
            .Bait(data, 43855)
            .Time(720, 960)
            .Weather(data, 2)
            .Transition(data, 4)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49800, Patch.IntoTheMist) // Datnioides Aeroplanos
            .Bait(data, 43858)
            .Time(120, 240)
            .Weather(data, 7)
            .Transition(data, 4)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(49801, Patch.IntoTheMist) // Esperance Carp
            .Bait(data, 43858)
            .Time(1320, 1440)
            .Weather(data, 3)
            .Transition(data, 7)
            .Predators(data, 300, (43796, 3), (43798, 3))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);


        // Foregone Oasis  Anomaly Impact Aquaculture Survey
        data.Apply(50090, Patch.IntoTheMist) // Voyaging Loach
            .Bait(data, 50230)
            .Mission(data, 1320)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50091, Patch.IntoTheMist) // Inverted Trout
            .Bait(data, 50230)
            .Mission(data, 1320)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50092, Patch.IntoTheMist) // Inverted Whitefish
            .Bait(data, 50230)
            .Mission(data, 1320)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Megalithopolis  Megalithopolis Specimen Survey
        data.Apply(50093, Patch.IntoTheMist) // Inverted Skipper
            .Bait(data, 50230)
            .Mission(data, 1321)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50094, Patch.IntoTheMist) // Site Sleeper
            .Bait(data, 50230)
            .Mission(data, 1321)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50095, Patch.IntoTheMist) // Lithic Megaloach
            .Bait(data, 50230)
            .Mission(data, 1321)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Foregone Oasis  Foregone Oasis Environmental Survey
        data.Apply(50096, Patch.IntoTheMist) // Voyaging Loach
            .Bait(data, 50231)
            .Mission(data, 1322)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50097, Patch.IntoTheMist) // Inverted Trout
            .Bait(data, 50231)
            .Mission(data, 1322)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50098, Patch.IntoTheMist) // Oizytrout
            .Bait(data, 50231)
            .Mission(data, 1322)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Megalithopolis  Regular Base Resupply
        data.Apply(50099, Patch.IntoTheMist) // Inverted Skipper
            .Bait(data, 50231)
            .Mission(data, 1323)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50100, Patch.IntoTheMist) // Site Sleeper
            .Bait(data, 50231)
            .Mission(data, 1323)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50101, Patch.IntoTheMist) // Inverse-eye
            .Bait(data, 50231)
            .Mission(data, 1323)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Foregone Oasis  Large Aquatic Life-form Distribution Survey
        data.Apply(50102, Patch.IntoTheMist) // Voyaging Loach
            .Bait(data, 50232)
            .Mission(data, 1324)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50103, Patch.IntoTheMist) // Inverted Trout
            .Bait(data, 50232)
            .Mission(data, 1324)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50104, Patch.IntoTheMist) // Oizys Catfish
            .Bait(data, 50232)
            .Mission(data, 1324)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50105, Patch.IntoTheMist) // Foregone Sturgeon
            .Bait(data, 50232)
            .Mission(data, 1324)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Megalithopolis  Oizys Bait Suitability Testing
        data.Apply(50106, Patch.IntoTheMist) // Inverted Skipper
            .Bait(data, 50233)
            .Mission(data, 1325)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50107, Patch.IntoTheMist) // Site Sleeper
            .Bait(data, 50233)
            .Mission(data, 1325)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50108, Patch.IntoTheMist) // Inverted Aoide
            .Bait(data, 50233)
            .Mission(data, 1325)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50109, Patch.IntoTheMist) // Megalith Axolotl
            .Bait(data, 50233)
            .Mission(data, 1325)
            .Bite(data, HookSet.Precise, BiteType.Weak);

        // Foregone Oasis  Large Aquatic Resource Emergency
        data.Apply(50110, Patch.IntoTheMist) // Voyaging Loach
            .Bait(data, 50233)
            .Mission(data, 1326)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50111, Patch.IntoTheMist) // Inverted Trout
            .Bait(data, 50233)
            .Mission(data, 1326)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50112, Patch.IntoTheMist) // Foregone Goby
            .Bait(data, 50233)
            .Mission(data, 1326)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50113, Patch.IntoTheMist) // Temporal Staff
            .Bait(data, 50233)
            .Mission(data, 1326)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Megalithopolis  Valuable Plains Specimens
        data.Apply(50114, Patch.IntoTheMist) // Inverted Skipper
            .Bait(data, 50232)
            .Mission(data, 1327)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50115, Patch.IntoTheMist) // Site Sleeper
            .Bait(data, 50232)
            .Mission(data, 1327)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50116, Patch.IntoTheMist) // Megalith Lithoshell
            .Bait(data, 50232)
            .Mission(data, 1327)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50117, Patch.IntoTheMist) // Megalith Bichir
            .Bait(data, 50232)
            .Mission(data, 1327)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Magnetic Horizon  Magnetic Horizon Environmental Survey
        data.Apply(50118, Patch.IntoTheMist) // Inverted Anchovy
            .Bait(data, 50236)
            .Mission(data, 1328)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50119, Patch.IntoTheMist) // Inverted Barracuda
            .Bait(data, 50236)
            .Mission(data, 1328)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50120, Patch.IntoTheMist) // Geras Crab
            .Bait(data, 50236)
            .Mission(data, 1328)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50121, Patch.IntoTheMist) // Pole Shifter
            .Bait(data, 50236)
            .Mission(data, 1328)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50122, Patch.IntoTheMist) // Iron Longnose
            .Bait(data, 50236)
            .Mission(data, 1328)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Collapsed Tunnel  Freshwater Arthrolure Testing
        data.Apply(50123, Patch.IntoTheMist) // Upside-down Squeaker
            .Bait(data, 50235)
            .Mission(data, 1329)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50124, Patch.IntoTheMist) // Inverted Lungfish
            .Bait(data, 50235)
            .Mission(data, 1329)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50125, Patch.IntoTheMist) // Frayed Wirefish
            .Bait(data, 50235)
            .Mission(data, 1329)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50126, Patch.IntoTheMist) // Captivating Cap
            .Bait(data, 50235)
            .Mission(data, 1329)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50127, Patch.IntoTheMist) // Cameroceras
            .Bait(data, 50235)
            .Mission(data, 1329)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Magnetic Horizon  Oceanic Impesctor Research
        data.Apply(50128, Patch.IntoTheMist) // Inverted Anchovy
            .Bait(data, 50238)
            .Mission(data, 1330)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50129, Patch.IntoTheMist) // Magnetic Clam
            .Bait(data, 50238)
            .Mission(data, 1330)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50130, Patch.IntoTheMist) // Calciferous Squid
            .Bait(data, 50238)
            .Mission(data, 1330)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50131, Patch.IntoTheMist) // Geras Nautilus
            .Bait(data, 50238)
            .Mission(data, 1330)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50132, Patch.IntoTheMist) // Pseudoshark
            .Bait(data, 50238)
            .Mission(data, 1330)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Collapsed Tunnel  Valuable Ruin Specimens
        data.Apply(50133, Patch.IntoTheMist) // Upside-down Squeaker
            .Bait(data, 50234)
            .Mission(data, 1331)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50134, Patch.IntoTheMist) // Upside-down Minnow
            .Bait(data, 50234)
            .Mission(data, 1331)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50135, Patch.IntoTheMist) // Inverted Tilapia
            .Bait(data, 50234)
            .Mission(data, 1331)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50136, Patch.IntoTheMist) // Oizyshrimp
            .Bait(data, 50234)
            .Mission(data, 1331)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50137, Patch.IntoTheMist) // Oizys Bullion
            .Bait(data, 50234)
            .Mission(data, 1331)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Magnetic Horizon  Anomaly Adaptation Aquaculture Survey
        data.Apply(50138, Patch.IntoTheMist) // Inverted Anchovy
            .Bait(data, 50237)
            .Mission(data, 1332)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50139, Patch.IntoTheMist) // Inverted Barracuda
            .Bait(data, 50237)
            .Mission(data, 1332)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50140, Patch.IntoTheMist) // Geras Seahorse
            .Bait(data, 50237)
            .Mission(data, 1332)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50141, Patch.IntoTheMist) // Rocky Sunfish
            .Bait(data, 50237)
            .Mission(data, 1332)
            .MultiHook(3)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Collapsed Tunnel Collapsed Tunnel Specimen Survey
        data.Apply(50142, Patch.IntoTheMist) // Upside-down Squeaker
            .Bait(data, 50240)
            .Mission(data, 1333)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50143, Patch.IntoTheMist) // Inverted Lungfish
            .Bait(data, 50240)
            .Mission(data, 1333)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50144, Patch.IntoTheMist) // Wronghair Catfish
            .Bait(data, 50240)
            .Mission(data, 1333)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50145, Patch.IntoTheMist) // Gravity Core
            .Bait(data, 50240)
            .Mission(data, 1333)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50146, Patch.IntoTheMist) // Pseudomander
            .Bait(data, 50240)
            .Mission(data, 1333)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(50147, Patch.IntoTheMist) // Collapsed Sucker
            .Bait(data, 50240)
            .Mission(data, 1333)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Collapsed Tunnel Red Cosmomaggot Testing
        data.Apply(50148, Patch.IntoTheMist) // Upside-down Squeaker
            .Bait(data, 50239)
            .Mission(data, 1334)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50149, Patch.IntoTheMist) // Inverted Lungfish
            .Bait(data, 50239)
            .Mission(data, 1334)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50150, Patch.IntoTheMist) // Magnetic Shell
            .Bait(data, 50239)
            .Mission(data, 1334)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50151, Patch.IntoTheMist) // Pseudogar
            .Mooch(data, 50150)
            .Mission(data, 1334)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50152, Patch.IntoTheMist) // Magnetite Skink
            .Mooch(data, 50150)
            .Mission(data, 1334)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Magnetic Horizon Edible Marine Life Survey
        data.Apply(50153, Patch.IntoTheMist) // Inverted Anchovy
            .Bait(data, 50243)
            .Mission(data, 1335)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50154, Patch.IntoTheMist) // Inverted Barracuda
            .Bait(data, 50243)
            .Mission(data, 1335)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50155, Patch.IntoTheMist) // Vacuum Worm
            .Bait(data, 50243)
            .Mission(data, 1335)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50156, Patch.IntoTheMist) // Pseudocrab
            .Bait(data, 50243)
            .Mission(data, 1335)
            .MultiHook(3)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50157, Patch.IntoTheMist) // Ancient Oizys Fossil
            .Bait(data, 50243)
            .Mission(data, 1335)
            .MultiHook(3)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // The Moros Well Moros Well Environmental Survey
        data.Apply(50158, Patch.IntoTheMist) // Moros Scorpion
            .Bait(data, 50246)
            .Mission(data, 1336)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50159, Patch.IntoTheMist) // Moros Crab
            .Bait(data, 50246)
            .Mission(data, 1336)
            .Bite(data, HookSet.Precise, BiteType.Strong);
        data.Apply(50160, Patch.IntoTheMist) // Sky Sniffer
            .Bait(data, 50246)
            .Mission(data, 1336)
            .Bite(data, HookSet.Powerful, BiteType.Weak);
        data.Apply(50161, Patch.IntoTheMist) // Unmoored Submariner
            .Bait(data, 50246)
            .Mission(data, 1336)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50162, Patch.IntoTheMist) // Moros Horned Viper
            .Bait(data, 50246)
            .Mission(data, 1336)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Magnetic Horizon Valuable Marine Specimens
        data.Apply(50163, Patch.IntoTheMist) // Inverted Anchovy
            .Bait(data, 50245)
            .Mission(data, 1337)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50164, Patch.IntoTheMist) // Inverted Barracuda
            .Bait(data, 50245)
            .Mission(data, 1337)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50165, Patch.IntoTheMist) // Oizylouse
            .Bait(data, 50245)
            .Mission(data, 1337)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50166, Patch.IntoTheMist) // Pseudoguitar
            .Bait(data, 50245)
            .Mission(data, 1337)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50167, Patch.IntoTheMist) // Steel Longnose
            .Bait(data, 50245)
            .Mission(data, 1337)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Magnetic Horizon Marine Distribution Survey
        data.Apply(50168, Patch.IntoTheMist) // Inverted Anchovy
            .Bait(data, 50242)
            .Mission(data, 1338)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50169, Patch.IntoTheMist) // Inverted Barracuda
            .Bait(data, 50242)
            .Mission(data, 1338)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50170, Patch.IntoTheMist) // Geras Trap
            .Bait(data, 50242)
            .Mission(data, 1338)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50171, Patch.IntoTheMist) // Pseudolumpsucker
            .Bait(data, 50242)
            .Mission(data, 1338)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50172, Patch.IntoTheMist) // Inverted Brindlebass
            .Bait(data, 50242)
            .Mission(data, 1338)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50173, Patch.IntoTheMist) // Craggy Sunfish
            .Bait(data, 50242)
            .Mission(data, 1338)
            .Predators(data, 360, (50171, 2))
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Collapsed Tunnel EX: Large Mutant Distribution Survey
        data.Apply(50174, Patch.IntoTheMist) // Upside-down Squeaker
            .Bait(data, 50241)
            .Mission(data, 1339)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50175, Patch.IntoTheMist) // Inverted Lungfish
            .Bait(data, 50241)
            .Mission(data, 1339)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50176, Patch.IntoTheMist) // Pseudosaucer
            .Bait(data, 50241)
            .Mission(data, 1339)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50177, Patch.IntoTheMist) // Iron Thornytooth
            .Bait(data, 50241)
            .Mission(data, 1339)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(50178, Patch.IntoTheMist) // Elongated Cameroceras
            .Bait(data, 50241)
            .Mission(data, 1339)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // The Moros Well EX: Dune Resource Emergency I
        data.Apply(50179, Patch.IntoTheMist) // Moros Scorpion
            .Bait(data, 50248)
            .Mission(data, 1340)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50180, Patch.IntoTheMist) // Moros Crab
            .Bait(data, 50248)
            .Mission(data, 1340)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50181, Patch.IntoTheMist) // Inverted Hornhelm
            .Bait(data, 50248)
            .Mission(data, 1340)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50182, Patch.IntoTheMist) // Stygian Pseudomander
            .Bait(data, 50248)
            .Mission(data, 1340)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50183, Patch.IntoTheMist) // Ironsand Lizard
            .Mooch(data, 50181)
            .Mission(data, 1340)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(50184, Patch.IntoTheMist) // Oddweight Ironsand Lizard
            .Mooch(data, 50181)
            .Mission(data, 1340)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // The Subterrain EX: Desert Burrow Environmental Survey
        data.Apply(50185, Patch.IntoTheMist) // Burrow Shrimp
            .Bait(data)
            .Mission(data, 1341)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50186, Patch.IntoTheMist) // Pseudoturtle
            .Bait(data)
            .Mission(data, 1341)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50187, Patch.IntoTheMist) // Burrow Clam
            .Bait(data)
            .Mission(data, 1341)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50188, Patch.IntoTheMist) // Inverted Bass
            .Bait(data)
            .Mission(data, 1341)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50189, Patch.IntoTheMist) // Pseudosnake
            .Bait(data)
            .Mission(data, 1341)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50190, Patch.IntoTheMist) // Ruined Relic
            .Bait(data)
            .Mission(data, 1341)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Under Overboard EX: Gravitational Sphere Impact
        data.Apply(50191, Patch.IntoTheMist) // Oizykelp
            .Bait(data, 50244)
            .Mission(data, 1342)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50192, Patch.IntoTheMist) // Geras Cloud
            .Bait(data, 50244)
            .Mission(data, 1342)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50193, Patch.IntoTheMist) // Magnetic Needler
            .Bait(data, 50244)
            .Mission(data, 1342)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50194, Patch.IntoTheMist) // Pseudonautilus
            .Bait(data, 50244)
            .Mission(data, 1342)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50195, Patch.IntoTheMist) // Marine Magnet
            .Bait(data, 50244)
            .Mission(data, 1342)
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // The Moros Well EX+: Dune Resource Emergency II
        data.Apply(50196, Patch.IntoTheMist) // Moros Scorpion
            .Bait(data, 50249)
            .Mission(data, 1343)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50197, Patch.IntoTheMist) // Moros Crab
            .Bait(data, 50249)
            .Mission(data, 1343)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50198, Patch.IntoTheMist) // Moros Egg
            .Bait(data, 50249)
            .Mission(data, 1343)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50199, Patch.IntoTheMist) // Desert Expanse Catfish
            .Bait(data, 50249)
            .Mission(data, 1343)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50200, Patch.IntoTheMist) // Moros Saw
            .Bait(data, 50249)
            .Mission(data, 1343)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(50201, Patch.IntoTheMist) // Moros Sandworm
            .Bait(data, 50249)
            .Mission(data, 1343)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Magnetic Horizon EX+: Cosmokrill Testing
        data.Apply(50202, Patch.IntoTheMist) // Inverted Anchovy
            .Bait(data, 50244)
            .Mission(data, 1344)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50203, Patch.IntoTheMist) // Inverted Barracuda
            .Bait(data, 50244)
            .Mission(data, 1344)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50204, Patch.IntoTheMist) // Captivating Clam
            .Bait(data, 50244)
            .Mission(data, 1344)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50205, Patch.IntoTheMist) // Geras Eel
            .Bait(data, 50244)
            .Mission(data, 1344)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(50206, Patch.IntoTheMist) // Mirrorshark
            .Bait(data, 50244)
            .Mission(data, 1344)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(50207, Patch.IntoTheMist) // Pseudosaurus
            .Bait(data, 50244)
            .Mission(data, 1344)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);

        // Under Overboard EX+: Voyager Relic Survey I
        data.Apply(50208, Patch.IntoTheMist) // Oizykelp
            .Bait(data)
            .Mission(data, 1345)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50209, Patch.IntoTheMist) // Geras Cloud
            .Bait(data)
            .Mission(data, 1345)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50210, Patch.IntoTheMist) // Platinum Coral
            .Bait(data)
            .Mission(data, 1345)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50211, Patch.IntoTheMist) // Reliqued Bangle
            .Bait(data)
            .Mission(data, 1345)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50212, Patch.IntoTheMist) // Reliqued Ring
            .Bait(data)
            .Mission(data, 1345)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50213, Patch.IntoTheMist) // Reliqued Coin
            .Bait(data)
            .Mission(data, 1345)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // The Moros Well EX+: Voyager Relic Survey II
        data.Apply(50214, Patch.IntoTheMist) // Moros Scorpion
            .Bait(data)
            .Mission(data, 1346)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50215, Patch.IntoTheMist) // Pseudomonkfish
            .Bait(data)
            .Mission(data, 1346)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50216, Patch.IntoTheMist) // Pseudogecko
            .Bait(data)
            .Mission(data, 1346)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50217, Patch.IntoTheMist) // Molten Lamprey
            .Bait(data)
            .Mission(data, 1346)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50218, Patch.IntoTheMist) // Oizyhelix
            .Bait(data)
            .Mission(data, 1346)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50219, Patch.IntoTheMist) // Reliqued Vessel
            .Bait(data)
            .Mission(data, 1346)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // The Subterrain EX+: Ruin Currents Environmental Survey
        data.Apply(50220, Patch.IntoTheMist) // Burrow Shrimp
            .Bait(data)
            .Mission(data, 1347)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50221, Patch.IntoTheMist) // Pseudoturtle
            .Bait(data)
            .Mission(data, 1347)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50222, Patch.IntoTheMist) // Upside-down Aether Eye
            .Bait(data)
            .Mission(data, 1347)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50223, Patch.IntoTheMist) // Inverted Carp
            .Bait(data)
            .Mission(data, 1347)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50224, Patch.IntoTheMist) // Burrow Bolo
            .Bait(data)
            .Mission(data, 1347)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(50225, Patch.IntoTheMist) // Euhedroturtle
            .Bait(data)
            .Mission(data, 1347)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);

        // Magnetic Horizon  Iron Coral Collection
        data.Apply(50226, Patch.IntoTheMist) // Inverted Anchovy
            .Bait(data, 50237)
            .Mission(data, 1368)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50227, Patch.IntoTheMist) // Iron Coral
            .Bait(data, 50237)
            .Mission(data, 1368)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

        // Megalithopolis  Sunken Relic Salvage
        data.Apply(50228, Patch.IntoTheMist) // Inverted Skipper
            .Bait(data, 50241)
            .Mission(data, 1369)
            .Bite(data, HookSet.Precise, BiteType.Weak);
        data.Apply(50229, Patch.IntoTheMist) // Oizys Rock
            .Bait(data, 50241)
            .Mission(data, 1369)
            .Bite(data, HookSet.Powerful, BiteType.Strong);

    }
    // @formatter:on
}







