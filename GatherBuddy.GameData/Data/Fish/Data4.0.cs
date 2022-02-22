using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyStormblood(this GameData data)
    {
        data.Apply     (20018, Patch.Stormblood) // Liopleurodon
            .Bait      (data, 20617, 20112)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 9);
        data.Apply     (20019, Patch.Stormblood) // Ala Mhigan Ribbon
            .Bait      (data, 20675)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20020, Patch.Stormblood) // Yanxian Barramundi
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (20021, Patch.Stormblood) // Seraphim
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (960, 1440);
        data.Apply     (20022, Patch.Stormblood) // Blackfin Snake Eel
            .Bait      (data, 20676)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20023, Patch.Stormblood) // Tail Mountains Minnow
            .Bait      (data, 20614, 20127)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20024, Patch.Stormblood) // Sweatfish
            .Bait      (data, 20675)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20025, Patch.Stormblood) // Rock Saltfish
            .Bait      (data, 20616)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Snag      (Snagging.None);
        data.Apply     (20026, Patch.Stormblood) // Comet Minnow
            .Bait      (data, 20614)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20027, Patch.Stormblood) // Doman Grass Carp
            .Bait      (data, 20615)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1200, 240);
        data.Apply     (20028, Patch.Stormblood) // Samurai Fish
            .Bait      (data, 29717)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20029, Patch.Stormblood) // Golden Cichlid
            .Bait      (data, 29717)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20030, Patch.Stormblood) // Hak Bitterling
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (0, 240)
            .Weather   (data, 7);
        data.Apply     (20031, Patch.Stormblood) // Yat Goby
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20032, Patch.Stormblood) // Ruby Meagre
            .Bait      (data, 20676)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20033, Patch.Stormblood) // Greenstream Loach
            .Bait      (data, 20614)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20034, Patch.Stormblood) // Tawny Wench Shark
            .Bait      (data, 20676)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (600, 1080)
            .Weather   (data, 1);
        data.Apply     (20035, Patch.Stormblood) // Whitehorse
            .Bait      (data, 20616)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20036, Patch.Stormblood) // Killifish
            .Bait      (data, 20615)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Weather   (data, 1);
        data.Apply     (20037, Patch.Stormblood) // Coeurl Snake Eel
            .Bait      (data, 20618)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20038, Patch.Stormblood) // Zekki Grouper
            .Bait      (data, 20676)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20039, Patch.Stormblood) // Saltshield Snapper
            .Bait      (data, 20616)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (20040, Patch.Stormblood) // Sculptor
            .Bait      (data, 20616, 20025)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (720, 1080)
            .Weather   (data, 10);
        data.Apply     (20041, Patch.Stormblood) // Pearl-eye
            .Bait      (data, 20616, 20025)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (20042, Patch.Stormblood) // Abalathian Bitterling
            .Bait      (data, 20614)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20043, Patch.Stormblood) // Steelshark
            .Bait      (data, 20675)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Weather   (data, 1);
        data.Apply     (20044, Patch.Stormblood) // Tao Bitterling
            .Bait      (data, 29717)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20045, Patch.Stormblood) // Idle Goby
            .Bait      (data, 28634)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20046, Patch.Stormblood) // River Barramundi
            .Bait      (data, 29717)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20047, Patch.Stormblood) // Mirage Chub
            .Bait      (data, 20613)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20048, Patch.Stormblood) // Harutsuge
            .Bait      (data, 20676)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20049, Patch.Stormblood) // Steelhead Trout
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20050, Patch.Stormblood) // Heather Charr
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20051, Patch.Stormblood) // Silken Koi
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 7);
        data.Apply     (20052, Patch.Stormblood) // Yellow Prismfish
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20053, Patch.Stormblood) // Blue Prismfish
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20054, Patch.Stormblood) // Hanatatsu
            .Bait      (data, 20617, 20112)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20055, Patch.Stormblood) // Broken Crab
            .Bait      (data, 20613)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20056, Patch.Stormblood) // Gyr Abanian Trout
            .Bait      (data, 20615)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Snag      (Snagging.None);
        data.Apply     (20057, Patch.Stormblood) // Bloodsipper
            .Bait      (data, 20615, 20056)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20058, Patch.Stormblood) // Miounnefish
            .Bait      (data, 20614, 20064)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 3, 4);
        data.Apply     (20059, Patch.Stormblood) // Monk Betta
            .Bait      (data, 20615, 20056)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20060, Patch.Stormblood) // Electric Catfish
            .Bait      (data, 20614)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20061, Patch.Stormblood) // Pagan Pirarucu
            .Bait      (data, 20615, 20056)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20062, Patch.Stormblood) // Temple Carp
            .Bait      (data, 20613)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20063, Patch.Stormblood) // Gilfish
            .Bait      (data, 20614)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20064, Patch.Stormblood) // Balloon Frog
            .Bait      (data, 20613)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (20065, Patch.Stormblood) // Lantern Marimo
            .Bait      (data, 20619)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20066, Patch.Stormblood) // Death Loach
            .Bait      (data, 20613)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20067, Patch.Stormblood) // Grymm Crab
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20068, Patch.Stormblood) // Enid Shrimp
            .Bait      (data, 20614)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20069, Patch.Stormblood) // Invisible Crayfish
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20070, Patch.Stormblood) // Rapids Jumper
            .Bait      (data, 20614)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20071, Patch.Stormblood) // Abalathian Salamander
            .Bait      (data, 20614)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20072, Patch.Stormblood) // Adamantite Bichir
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20073, Patch.Stormblood) // Meditator
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Snag      (Snagging.None)
            .Weather   (data, 3, 5, 4, 11);
        data.Apply     (20074, Patch.Stormblood) // Deemster
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 5);
        data.Apply     (20075, Patch.Stormblood) // Stonytongue
            .Bait      (data, 29717)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20076, Patch.Stormblood) // Bull's Bite
            .Bait      (data, 20615)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 4, 5, 3, 11);
        data.Apply     (20077, Patch.Stormblood) // Peeping Pisces
            .Bait      (data, 20615)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20078, Patch.Stormblood) // Scimitarfish
            .Bait      (data, 20615, 20056)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20079, Patch.Stormblood) // Gigant Bass
            .Bait      (data, 20615, 20056)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20080, Patch.Stormblood) // Nhaama's Boon
            .Bait      (data, 20615)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20081, Patch.Stormblood) // Cave Killifish
            .Bait      (data, 20619)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20082, Patch.Stormblood) // Bone Melter
            .Bait      (data, 20613)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20083, Patch.Stormblood) // Fallen Leaf
            .Bait      (data, 20619)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20084, Patch.Stormblood) // Falling Star
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Weather   (data, 3, 4);
        data.Apply     (20085, Patch.Stormblood) // Capsized Squeaker
            .Bait      (data, 20613)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (960, 1140);
        data.Apply     (20086, Patch.Stormblood) // Nirvana Crab
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 3, 4);
        data.Apply     (20087, Patch.Stormblood) // Velodyna Grass Carp
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20088, Patch.Stormblood) // Black Velodyna Carp
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20089, Patch.Stormblood) // Rhalgr's Bolt
            .Bait      (data, 20613)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20090, Patch.Stormblood) // Highland Perch
            .Bait      (data, 20613)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20091, Patch.Stormblood) // Gravel Gudgeon
            .Bait      (data, 20613)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20092, Patch.Stormblood) // Grinning Anchovy
            .Bait      (data, 20617)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20093, Patch.Stormblood) // Glass Herring
            .Bait      (data, 20618)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20094, Patch.Stormblood) // Hellyfish
            .Bait      (data, 20617)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20095, Patch.Stormblood) // Ruby Coral
            .Bait      (data, 20618)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required);
        data.Apply     (20096, Patch.Stormblood) // Sapphire Coral
            .Bait      (data, 20618)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required);
        data.Apply     (20097, Patch.Stormblood) // Bone Coral
            .Bait      (data, 20618)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required);
        data.Apply     (20098, Patch.Stormblood) // Butterfly Fish
            .Bait      (data, 20676)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20099, Patch.Stormblood) // Dafu
            .Bait      (data, 20618)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20100, Patch.Stormblood) // Swordfish
            .Bait      (data, 20676)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 960)
            .Weather   (data, 3, 5);
        data.Apply     (20101, Patch.Stormblood) // Leaf Tatsunoko
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20102, Patch.Stormblood) // Glass Flounder
            .Bait      (data, 20618)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20103, Patch.Stormblood) // Zekki Gator
            .Bait      (data, 20617, 20112)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (20104, Patch.Stormblood) // Daio Squid
            .Bait      (data, 20618)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (20105, Patch.Stormblood) // Koromo Octopus
            .Bait      (data, 20618)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (20106, Patch.Stormblood) // Gliding Fish
            .Bait      (data, 20618)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20107, Patch.Stormblood) // Globefish
            .Bait      (data, 28634)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20108, Patch.Stormblood) // Fan Clam
            .Bait      (data, 20617)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Snag      (Snagging.Required);
        data.Apply     (20109, Patch.Stormblood) // Blockhead
            .Bait      (data, 20617, 20112)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (20110, Patch.Stormblood) // Glass Tuna
            .Bait      (data, 20617, 20112)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20111, Patch.Stormblood) // Doman Crayfish
            .Bait      (data, 20675)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20112, Patch.Stormblood) // Ruby Shrimp
            .Bait      (data, 20617)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (20113, Patch.Stormblood) // Striped Fugu
            .Bait      (data, 20617, 20112)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20114, Patch.Stormblood) // Raitonfish
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 7, 8);
        data.Apply     (20115, Patch.Stormblood) // Blank Oscar
            .Bait      (data, 20615)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20116, Patch.Stormblood) // Dragonfish
            .Bait      (data, 20615)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20117, Patch.Stormblood) // Lordly Salmon
            .Bait      (data, 29717)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20118, Patch.Stormblood) // Yanxian Koi
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20119, Patch.Stormblood) // Kotsu Zetsu
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20120, Patch.Stormblood) // Longhair Catfish
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20121, Patch.Stormblood) // Plum Gazer
            .Bait      (data, 20615)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20122, Patch.Stormblood) // Pandamoth
            .Bait      (data, 20615)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (600, 1080);
        data.Apply     (20123, Patch.Stormblood) // Doman Trout
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20124, Patch.Stormblood) // Doman Eel
            .Bait      (data, 29717)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (1020, 600)
            .Snag      (Snagging.None);
        data.Apply     (20125, Patch.Stormblood) // Brassfish
            .Bait      (data, 20613)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20126, Patch.Stormblood) // Othardian Trout
            .Bait      (data, 20614, 20127)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20127, Patch.Stormblood) // Zagas Khaal
            .Bait      (data, 20614)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (20128, Patch.Stormblood) // Steppe Skipper
            .Bait      (data, 20615)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20129, Patch.Stormblood) // Dry Steppe Skipper
            .Bait      (data, 29717)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20130, Patch.Stormblood) // Sun Bass
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20131, Patch.Stormblood) // Skytear
            .Bait      (data, 29717)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20132, Patch.Stormblood) // Dawn Crayfish
            .Bait      (data, 20614, 20127)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20133, Patch.Stormblood) // Dusk Crayfish
            .Bait      (data, 20613)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20134, Patch.Stormblood) // Bowfish
            .Bait      (data, 29717)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20135, Patch.Stormblood) // Jade Sculpin
            .Bait      (data, 20619)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (20136, Patch.Stormblood) // Padjali Loach
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20137, Patch.Stormblood) // Nogoi
            .Bait      (data, 20614)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20138, Patch.Stormblood) // Curtain Pleco
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20140, Patch.Stormblood) // Hardscale
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (20141, Patch.Stormblood) // Eastern Pike
            .Bait      (data, 20619)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 1, 2);
        data.Apply     (20142, Patch.Stormblood) // Wraithfish
            .Bait      (data, 20675)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (0, 240)
            .Weather   (data, 4);
        data.Apply     (20143, Patch.Stormblood) // Little Perykos
            .Bait      (data, 2585, 4869, 4904)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 7, 8);
        data.Apply     (20144, Patch.Stormblood) // Wentletrap
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20145, Patch.Stormblood) // Black Boxfish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (20146, Patch.Stormblood) // Glass Manta
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (20147, Patch.Stormblood) // Regal Silverside
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply     (20148, Patch.Stormblood) // Snowflake Moray
            .Spear     (SpearfishSize.Average, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20149, Patch.Stormblood) // Hoppfish
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average);
        data.Apply     (20150, Patch.Stormblood) // Lightscale
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow);
        data.Apply     (20151, Patch.Stormblood) // Grass Fugu
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (20152, Patch.Stormblood) // Giant Eel
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply     (20153, Patch.Stormblood) // Kamina Crab
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (20154, Patch.Stormblood) // Spider Crab
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow);
        data.Apply     (20155, Patch.Stormblood) // Little Dragonfish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (20156, Patch.Stormblood) // Black Fanfish
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (20157, Patch.Stormblood) // Zebra Shark
            .Spear     (SpearfishSize.Large, SpearfishSpeed.HyperFast);
        data.Apply     (20158, Patch.Stormblood) // Nophica's Comb
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (20159, Patch.Stormblood) // Warty Wartfish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20160, Patch.Stormblood) // Common Whelk
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20161, Patch.Stormblood) // Hairless Barb
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply     (20162, Patch.Stormblood) // Hatchetfish
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply     (20163, Patch.Stormblood) // Threadfish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (20164, Patch.Stormblood) // Garden Eel
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (20165, Patch.Stormblood) // Eastern Sea Pickle
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20166, Patch.Stormblood) // Brindlebass
            .Spear     (SpearfishSize.Large, SpearfishSpeed.ExtremelyFast);
        data.Apply     (20167, Patch.Stormblood) // Demon Stonefish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (20168, Patch.Stormblood) // Armored Crayfish
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply     (20169, Patch.Stormblood) // Bighead Carp
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply     (20170, Patch.Stormblood) // Zeni Clam
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20171, Patch.Stormblood) // Corpse-eater
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply     (20172, Patch.Stormblood) // Ronin Trevally
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply     (20173, Patch.Stormblood) // Toothsome Grouper
            .Spear     (SpearfishSize.Large, SpearfishSpeed.SuperFast);
        data.Apply     (20174, Patch.Stormblood) // Horned Turban
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20175, Patch.Stormblood) // Ruby Sea Star
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20176, Patch.Stormblood) // Gauntlet Crab
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (20177, Patch.Stormblood) // Hermit Goby
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (20178, Patch.Stormblood) // Skythorn
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (20179, Patch.Stormblood) // Swordtip
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (20180, Patch.Stormblood) // False Scad
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (20181, Patch.Stormblood) // Snow Crab
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (20182, Patch.Stormblood) // Red-eyed Lates
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply     (20183, Patch.Stormblood) // Common Bitterling
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (20184, Patch.Stormblood) // Fifty-summer Cod
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (20185, Patch.Stormblood) // Nagxian Mullet
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (20186, Patch.Stormblood) // Redcoat
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (20187, Patch.Stormblood) // Yanxian Tiger Prawn
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average);
        data.Apply     (20188, Patch.Stormblood) // Tengu Fan
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (20189, Patch.Stormblood) // Star Turban
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20190, Patch.Stormblood) // Blue-fish
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow);
        data.Apply     (20191, Patch.Stormblood) // Steppe Bullfrog
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20192, Patch.Stormblood) // Cavalry Catfish
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply     (20193, Patch.Stormblood) // Redfin
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (20194, Patch.Stormblood) // Moondisc
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (20195, Patch.Stormblood) // Bleached Bonytongue
            .Spear     (SpearfishSize.Large, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20196, Patch.Stormblood) // Salt Shark
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VeryFast);
        data.Apply     (20197, Patch.Stormblood) // King's Mantle
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply     (20198, Patch.Stormblood) // Sea Lamp
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20199, Patch.Stormblood) // Amberjack
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply     (20200, Patch.Stormblood) // Cherry Salmon
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (20201, Patch.Stormblood) // Yu-no-hana Crab
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (20202, Patch.Stormblood) // Dotharli Gudgeon
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Slow);
        data.Apply     (20203, Patch.Stormblood) // River Clam
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20204, Patch.Stormblood) // Grass Shark
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Slow);
        data.Apply     (20205, Patch.Stormblood) // Typhoon Shrimp
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (20206, Patch.Stormblood) // Rock Oyster
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20207, Patch.Stormblood) // Salt Urchin
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20208, Patch.Stormblood) // Carpenter Crab
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (20209, Patch.Stormblood) // Spiny Lobster
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (20210, Patch.Stormblood) // Mitsukuri Shark
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Fast)
            .Predators (data, 0, (20217, 7));
        data.Apply     (20211, Patch.Stormblood) // Doman Bubble Eye
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (20212, Patch.Stormblood) // Dragon Squeaker
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply     (20213, Patch.Stormblood) // Dawn Herald
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average);
        data.Apply     (20214, Patch.Stormblood) // Salt Cellar
            .Spear     (SpearfishSize.Small, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20215, Patch.Stormblood) // White Sturgeon
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (20216, Patch.Stormblood) // Tithe Collector
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (20217, Patch.Stormblood) // Bashful Batfish
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VerySlow);
        data.Apply     (20218, Patch.Stormblood) // River Bream
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Fast);
        data.Apply     (20219, Patch.Stormblood) // Snipe Eel
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (20220, Patch.Stormblood) // Cherubfish
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average)
            .Predators (data, 0, (20228, 7));
        data.Apply     (20221, Patch.Stormblood) // Dusk Herald
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (20222, Patch.Stormblood) // Glaring Perch
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Fast);
        data.Apply     (20223, Patch.Stormblood) // Abalathian Pipira
            .Spear     (SpearfishSize.Average, SpearfishSpeed.Average);
        data.Apply     (20224, Patch.Stormblood) // Steel Loach
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply     (20225, Patch.Stormblood) // Ivory Sole
            .Spear     (SpearfishSize.Average, SpearfishSpeed.ExtremelySlow);
        data.Apply     (20226, Patch.Stormblood) // Motley Beakfish
            .Spear     (SpearfishSize.Large, SpearfishSpeed.Average);
        data.Apply     (20227, Patch.Stormblood) // Thousandfang
            .Spear     (SpearfishSize.Large, SpearfishSpeed.SuperFast)
            .Predators (data, 0, (20217, 7));
        data.Apply     (20228, Patch.Stormblood) // Ichthyosaur
            .Spear     (SpearfishSize.Large, SpearfishSpeed.ExtremelyFast);
        data.Apply     (20229, Patch.Stormblood) // Sailfin
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VeryFast);
        data.Apply     (20230, Patch.Stormblood) // Fangshi
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow)
            .Predators (data, 0, (20228, 7));
        data.Apply     (20231, Patch.Stormblood) // Flamefish
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VeryFast);
        data.Apply     (20232, Patch.Stormblood) // Fickle Krait
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (20233, Patch.Stormblood) // Eternal Eye
            .Spear     (SpearfishSize.Small, SpearfishSpeed.VeryFast)
            .Predators (data, 0, (20222, 7));
        data.Apply     (20234, Patch.Stormblood) // Soul of the Stallion
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average)
            .Predators (data, 0, (20222, 7));
        data.Apply     (20235, Patch.Stormblood) // Flood Tuna
            .Spear     (SpearfishSize.Large, SpearfishSpeed.ExtremelyFast);
        data.Apply     (20236, Patch.Stormblood) // Mercenary Crab
            .Spear     (SpearfishSize.Average, SpearfishSpeed.VerySlow);
        data.Apply     (20237, Patch.Stormblood) // Ashfish
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Slow);
        data.Apply     (20238, Patch.Stormblood) // Silken Sunfish
            .Spear     (SpearfishSize.Large, SpearfishSpeed.VerySlow)
            .Predators (data, 0, (20236, 7));
        data.Apply     (20239, Patch.Stormblood) // Mosasaur
            .Spear     (SpearfishSize.Large, SpearfishSpeed.SuperFast)
            .Predators (data, 0, (20236, 7));
        data.Apply     (20524, Patch.Stormblood) // Castaway Chocobo Chick
            .Bait      (data, 2585, 4869, 4904)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 960);
        data.Apply     (20528, Patch.Stormblood) // Tiny Tatsunoko
            .Spear     (SpearfishSize.Small, SpearfishSpeed.Average)
            .Predators (data, 0, (20217, 7));
    }
    // @formatter:on
}
