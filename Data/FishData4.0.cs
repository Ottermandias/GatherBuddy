using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyStormblood(this FishManager fish)
        {
            fish.Apply     (20018, Patch.Stormblood) // Liopleurodon
                .Bait      (20617, 20112)
                .Tug       (BiteType.Legendary)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20019, Patch.Stormblood) // Ala Mhigan Ribbon
                .Bait      (20619)
                .HookType  (HookSet.Precise);
            fish.Apply     (20020, Patch.Stormblood) // Yanxian Barramundi
                .Bait      (20614)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20021, Patch.Stormblood) // Seraphim
                .Bait      (20619)
                .Tug       (BiteType.Strong)
                .Uptime    (16, 24)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20022, Patch.Stormblood) // Blackfin Snake Eel
                .Bait      (20676)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20023, Patch.Stormblood) // Tail Mountains Minnow
                .Bait      (20615)
                .HookType  (HookSet.Precise);
            fish.Apply     (20024, Patch.Stormblood) // Sweatfish
                .Bait      (20675)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (20025, Patch.Stormblood) // Rock Saltfish
                .Bait      (20616)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20026, Patch.Stormblood) // Comet Minnow
                .Bait      (20619);
            fish.Apply     (20027, Patch.Stormblood) // Doman Grass Carp
                .Bait      (20615)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20028, Patch.Stormblood) // Samurai Fish
                .Bait      (20615)
                .HookType  (HookSet.Precise);
            fish.Apply     (20029, Patch.Stormblood) // Golden Cichlid
                .Bait      (20613);
            fish.Apply     (20030, Patch.Stormblood) // Hak Bitterling
                .Bait      (20675)
                .Uptime    (0, 4)
                .Weather   (7);
            fish.Apply     (20031, Patch.Stormblood) // Yat Goby
                .Bait      (20615)
                .HookType  (HookSet.Precise);
            fish.Apply     (20032, Patch.Stormblood) // Ruby Meagre
                .Bait      (20618)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20033, Patch.Stormblood) // Greenstream Loach
                .Bait      (20613);
            fish.Apply     (20034, Patch.Stormblood) // Tawny Wench Shark
                .Bait      (20676)
                .Tug       (BiteType.Strong)
                .Uptime    (10, 19)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20035, Patch.Stormblood) // Whitehorse
                .Bait      (20616)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (20036, Patch.Stormblood) // Killifish
                .Bait      (20615)
                .Tug       (BiteType.Weak)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (20037, Patch.Stormblood) // Coeurl Snake Eel
                .Bait      (20617)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20038, Patch.Stormblood) // Zekki Grouper
                .Bait      (20617, 20112);
            fish.Apply     (20039, Patch.Stormblood) // Saltshield Snapper
                .Bait      (20616)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20040, Patch.Stormblood) // Sculptor
                .Bait      (20616, 20025)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 18)
                .Weather   (10)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20041, Patch.Stormblood) // Pearl-eye
                .Bait      (20616, 20025)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20042, Patch.Stormblood) // Abalathian Bitterling
                .Bait      (20613)
                .HookType  (HookSet.Precise);
            fish.Apply     (20043, Patch.Stormblood) // Steelshark
                .Bait      (20619)
                .Tug       (BiteType.Weak)
                .Weather   (1)
                .HookType  (HookSet.Precise);
            fish.Apply     (20044, Patch.Stormblood) // Tao Bitterling
                .Bait      (20615)
                .HookType  (HookSet.Precise);
            fish.Apply     (20045, Patch.Stormblood) // Idle Goby
                .Bait      (20613)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20046, Patch.Stormblood) // River Barramundi
                .Bait      (20614)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20047, Patch.Stormblood) // Mirage Chub
                .Bait      (20613);
            fish.Apply     (20048, Patch.Stormblood) // Harutsuge
                .Bait      (20676)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20049, Patch.Stormblood) // Steelhead Trout
                .Bait      (20619);
            fish.Apply     (20050, Patch.Stormblood) // Heather Charr
                .Bait      (20619)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20051, Patch.Stormblood) // Silken Koi
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Weather   (7)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20052, Patch.Stormblood) // Yellow Prismfish
                .Bait      (20619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20053, Patch.Stormblood) // Blue Prismfish
                .Bait      (20619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20054, Patch.Stormblood) // Hanatatsu
                .Bait      (20617, 20112)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20055, Patch.Stormblood) // Broken Crab
                .Bait      (20613);
            fish.Apply     (20056, Patch.Stormblood) // Gyr Abanian Trout
                .Bait      (20615)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20057, Patch.Stormblood) // Bloodsipper
                .Bait      (20615, 20056);
            fish.Apply     (20058, Patch.Stormblood) // Miounnefish
                .Bait      (20614, 20064)
                .Tug       (BiteType.Strong)
                .Weather   (3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20059, Patch.Stormblood) // Monk Betta
                .Bait      (20615, 20056);
            fish.Apply     (20060, Patch.Stormblood) // Electric Catfish
                .Bait      (20615)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20061, Patch.Stormblood) // Pagan Pirarucu
                .Bait      (20615, 20056)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20062, Patch.Stormblood) // Temple Carp
                .Bait      (20613)
                .HookType  (HookSet.Precise);
            fish.Apply     (20063, Patch.Stormblood) // Gilfish
                .Bait      (20614)
                .HookType  (HookSet.Precise);
            fish.Apply     (20064, Patch.Stormblood) // Balloon Frog
                .Bait      (20613)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (20065, Patch.Stormblood) // Lantern Marimo
                .Bait      (20619)
                .HookType  (HookSet.Precise);
            fish.Apply     (20066, Patch.Stormblood) // Death Loach
                .Bait      (20613);
            fish.Apply     (20067, Patch.Stormblood) // Grymm Crab
                .Bait      (20614);
            fish.Apply     (20068, Patch.Stormblood) // Enid Shrimp
                .Bait      (20614)
                .HookType  (HookSet.Precise);
            fish.Apply     (20069, Patch.Stormblood) // Invisible Crayfish
                .Bait      (20614);
            fish.Apply     (20070, Patch.Stormblood) // Rapids Jumper
                .Bait      (20614)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20071, Patch.Stormblood) // Abalathian Salamander
                .Bait      (20619)
                .HookType  (HookSet.Precise);
            fish.Apply     (20072, Patch.Stormblood) // Adamantite Bichir
                .Bait      (20619)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20073, Patch.Stormblood) // Meditator
                .Bait      (20619)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20074, Patch.Stormblood) // Deemster
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Weather   (5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20075, Patch.Stormblood) // Stonytongue
                .Bait      (20619)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20076, Patch.Stormblood) // Bull's Bite
                .Bait      (20615)
                .Tug       (BiteType.Strong)
                .Weather   (4, 5, 3, 11)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20077, Patch.Stormblood) // Peeping Pisces
                .Bait      (20615)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20078, Patch.Stormblood) // Scimitarfish
                .Bait      (20619)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20079, Patch.Stormblood) // Gigant Bass
                .Bait      (20675)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20080, Patch.Stormblood) // Nhaama's Boon
                .Bait      (20615)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20081, Patch.Stormblood) // Cave Killifish
                .Bait      (20619);
            fish.Apply     (20082, Patch.Stormblood) // Bone Melter
                .Bait      (20613);
            fish.Apply     (20083, Patch.Stormblood) // Fallen Leaf
                .Bait      (20619)
                .HookType  (HookSet.Precise);
            fish.Apply     (20084, Patch.Stormblood) // Falling Star
                .Bait      (20613)
                .Tug       (BiteType.Weak)
                .Weather   (3, 4)
                .HookType  (HookSet.Precise);
            fish.Apply     (20085, Patch.Stormblood) // Capsized Squeaker
                .Bait      (20613)
                .Tug       (BiteType.Weak)
                .Uptime    (16, 19)
                .HookType  (HookSet.Precise);
            fish.Apply     (20086, Patch.Stormblood) // Nirvana Crab
                .Bait      (20613)
                .Tug       (BiteType.Strong)
                .Weather   (3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20087, Patch.Stormblood) // Velodyna Grass Carp
                .Bait      (20614);
            fish.Apply     (20088, Patch.Stormblood) // Black Velodyna Carp
                .Bait      (20613);
            fish.Apply     (20089, Patch.Stormblood) // Rhalgr's Bolt
                .Bait      (20613)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20090, Patch.Stormblood) // Highland Perch
                .Bait      (20613)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20091, Patch.Stormblood) // Gravel Gudgeon
                .Bait      (20613)
                .HookType  (HookSet.Precise);
            fish.Apply     (20092, Patch.Stormblood) // Grinning Anchovy
                .Bait      (20617);
            fish.Apply     (20093, Patch.Stormblood) // Glass Herring
                .Bait      (20617);
            fish.Apply     (20094, Patch.Stormblood) // Hellyfish
                .Bait      (20617);
            fish.Apply     (20095, Patch.Stormblood) // Ruby Coral
                .Bait      (20617)
                .Snag      (Snagging.Required);
            fish.Apply     (20096, Patch.Stormblood) // Sapphire Coral
                .Bait      (20617)
                .Snag      (Snagging.Required);
            fish.Apply     (20097, Patch.Stormblood) // Bone Coral
                .Bait      (20617)
                .Snag      (Snagging.Required);
            fish.Apply     (20098, Patch.Stormblood) // Butterfly Fish
                .Bait      (20676);
            fish.Apply     (20099, Patch.Stormblood) // Dafu
                .Bait      (20617, 20112);
            fish.Apply     (20100, Patch.Stormblood) // Swordfish
                .Bait      (20676)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 16)
                .Weather   (3, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20101, Patch.Stormblood) // Leaf Tatsunoko
                .Bait      (20617, 20112);
            fish.Apply     (20102, Patch.Stormblood) // Glass Flounder
                .Bait      (20617, 20112);
            fish.Apply     (20103, Patch.Stormblood) // Zekki Gator
                .Bait      (20617, 20112)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20104, Patch.Stormblood) // Daio Squid
                .Bait      (20617, 20112)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20105, Patch.Stormblood) // Koromo Octopus
                .Bait      (20617, 20112)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20106, Patch.Stormblood) // Gliding Fish
                .Bait      (20618)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20107, Patch.Stormblood) // Globefish
                .Bait      (20617)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20108, Patch.Stormblood) // Fan Clam
                .Bait      (20617)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20109, Patch.Stormblood) // Blockhead
                .Bait      (20617, 20112);
            fish.Apply     (20110, Patch.Stormblood) // Glass Tuna
                .Bait      (20617, 20112);
            fish.Apply     (20111, Patch.Stormblood) // Doman Crayfish
                .Bait      (20614)
                .HookType  (HookSet.Precise);
            fish.Apply     (20112, Patch.Stormblood) // Ruby Shrimp
                .Bait      (20617)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (20113, Patch.Stormblood) // Striped Fugu
                .Bait      (20617, 20112);
            fish.Apply     (20114, Patch.Stormblood) // Raitonfish
                .Bait      (20619)
                .Tug       (BiteType.Strong)
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20115, Patch.Stormblood) // Blank Oscar
                .Bait      (20615);
            fish.Apply     (20116, Patch.Stormblood) // Dragonfish
                .Bait      (20619);
            fish.Apply     (20117, Patch.Stormblood) // Lordly Salmon
                .Bait      (20615);
            fish.Apply     (20118, Patch.Stormblood) // Yanxian Koi
                .Bait      (20614)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20119, Patch.Stormblood) // Kotsu Zetsu
                .Bait      (20619);
            fish.Apply     (20120, Patch.Stormblood) // Longhair Catfish
                .Bait      (20619)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20121, Patch.Stormblood) // Plum Gazer
                .Bait      (20615);
            fish.Apply     (20122, Patch.Stormblood) // Pandamoth
                .Bait      (20615)
                .Tug       (BiteType.Strong)
                .Uptime    (10, 18)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20123, Patch.Stormblood) // Doman Trout
                .Bait      (20614)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20124, Patch.Stormblood) // Doman Eel
                .Bait      (20615);
            fish.Apply     (20125, Patch.Stormblood) // Brassfish
                .Bait      (20614);
            fish.Apply     (20126, Patch.Stormblood) // Othardian Trout
                .Bait      (20614, 20127);
            fish.Apply     (20127, Patch.Stormblood) // Zagas Khaal
                .Bait      (20614)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (20128, Patch.Stormblood) // Steppe Skipper
                .Bait      (20615)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (20129, Patch.Stormblood) // Dry Steppe Skipper
                .Bait      (20615)
                .HookType  (HookSet.Precise);
            fish.Apply     (20130, Patch.Stormblood) // Sun Bass
                .Bait      (20615)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20131, Patch.Stormblood) // Skytear
                .Bait      (20615)
                .HookType  (HookSet.Precise);
            fish.Apply     (20132, Patch.Stormblood) // Dawn Crayfish
                .Bait      (20614, 20127)
                .HookType  (HookSet.Precise);
            fish.Apply     (20133, Patch.Stormblood) // Dusk Crayfish
                .Bait      (20614, 20127);
            fish.Apply     (20134, Patch.Stormblood) // Bowfish
                .Bait      (20615);
            fish.Apply     (20135, Patch.Stormblood) // Jade Sculpin
                .Bait      (20615)
                .HookType  (HookSet.Precise);
            fish.Apply     (20136, Patch.Stormblood) // Padjali Loach
                .Bait      (20613);
            fish.Apply     (20137, Patch.Stormblood) // Nogoi
                .Bait      (20614)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20138, Patch.Stormblood) // Curtain Pleco
                .Bait      (20619)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20140, Patch.Stormblood) // Hardscale
                .Bait      (20619);
            fish.Apply     (20141, Patch.Stormblood) // Eastern Pike
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20142, Patch.Stormblood) // Wraithfish
                .Bait      (20675)
                .Tug       (BiteType.Strong)
                .Uptime    (0, 4)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20143, Patch.Stormblood) // Little Perykos
                .Bait      (2585, 4869, 4904)
                .Tug       (BiteType.Strong)
                .Weather   (7, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20144, Patch.Stormblood) // Wentletrap
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20145, Patch.Stormblood) // Black Boxfish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20146, Patch.Stormblood) // Glass Manta
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20147, Patch.Stormblood) // Regal Silverside
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20148, Patch.Stormblood) // Snowflake Moray
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20149, Patch.Stormblood) // Hoppfish
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20150, Patch.Stormblood) // Lightscale
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20151, Patch.Stormblood) // Grass Fugu
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20152, Patch.Stormblood) // Giant Eel
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20153, Patch.Stormblood) // Kamina Crab
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20154, Patch.Stormblood) // Spider Crab
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20155, Patch.Stormblood) // Little Dragonfish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20156, Patch.Stormblood) // Black Fanfish
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20157, Patch.Stormblood) // Zebra Shark
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20158, Patch.Stormblood) // Nophica's Comb
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20159, Patch.Stormblood) // Warty Wartfish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20160, Patch.Stormblood) // Common Whelk
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20161, Patch.Stormblood) // Hairless Barb
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20162, Patch.Stormblood) // Hatchetfish
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20163, Patch.Stormblood) // Threadfish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20164, Patch.Stormblood) // Garden Eel
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20165, Patch.Stormblood) // Eastern Sea Pickle
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20166, Patch.Stormblood) // Brindlebass
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20167, Patch.Stormblood) // Demon Stonefish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20168, Patch.Stormblood) // Armored Crayfish
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20169, Patch.Stormblood) // Bighead Carp
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20170, Patch.Stormblood) // Zeni Clam
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20171, Patch.Stormblood) // Corpse-eater
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20172, Patch.Stormblood) // Ronin Trevally
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20173, Patch.Stormblood) // Toothsome Grouper
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20174, Patch.Stormblood) // Horned Turban
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20175, Patch.Stormblood) // Ruby Sea Star
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20176, Patch.Stormblood) // Gauntlet Crab
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20177, Patch.Stormblood) // Hermit Goby
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20178, Patch.Stormblood) // Skythorn
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20179, Patch.Stormblood) // Swordtip
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20180, Patch.Stormblood) // False Scad
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20181, Patch.Stormblood) // Snow Crab
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20182, Patch.Stormblood) // Red-eyed Lates
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20183, Patch.Stormblood) // Common Bitterling
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20184, Patch.Stormblood) // Fifty-summer Cod
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20185, Patch.Stormblood) // Nagxian Mullet
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20186, Patch.Stormblood) // Redcoat
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20187, Patch.Stormblood) // Yanxian Tiger Prawn
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20188, Patch.Stormblood) // Tengu Fan
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20189, Patch.Stormblood) // Star Turban
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20190, Patch.Stormblood) // Blue-fish
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20191, Patch.Stormblood) // Steppe Bullfrog
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20192, Patch.Stormblood) // Cavalry Catfish
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20193, Patch.Stormblood) // Redfin
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20194, Patch.Stormblood) // Moondisc
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20195, Patch.Stormblood) // Bleached Bonytongue
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20196, Patch.Stormblood) // Salt Shark
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20197, Patch.Stormblood) // King's Mantle
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20198, Patch.Stormblood) // Sea Lamp
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20199, Patch.Stormblood) // Amberjack
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20200, Patch.Stormblood) // Cherry Salmon
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20201, Patch.Stormblood) // Yu-no-hana Crab
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20202, Patch.Stormblood) // Dotharli Gudgeon
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20203, Patch.Stormblood) // River Clam
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20204, Patch.Stormblood) // Grass Shark
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20205, Patch.Stormblood) // Typhoon Shrimp
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20206, Patch.Stormblood) // Rock Oyster
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20207, Patch.Stormblood) // Salt Urchin
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20208, Patch.Stormblood) // Carpenter Crab
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20209, Patch.Stormblood) // Spiny Lobster
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20210, Patch.Stormblood) // Mitsukuri Shark
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .Predators ((20217, 7))
                .HookType  (HookSet.None);
            fish.Apply     (20211, Patch.Stormblood) // Doman Bubble Eye
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20212, Patch.Stormblood) // Dragon Squeaker
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20213, Patch.Stormblood) // Dawn Herald
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20214, Patch.Stormblood) // Salt Cellar
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20215, Patch.Stormblood) // White Sturgeon
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20216, Patch.Stormblood) // Tithe Collector
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20217, Patch.Stormblood) // Bashful Batfish
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20218, Patch.Stormblood) // River Bream
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20219, Patch.Stormblood) // Snipe Eel
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20220, Patch.Stormblood) // Cherubfish
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .Predators ((20228, 7))
                .HookType  (HookSet.None);
            fish.Apply     (20221, Patch.Stormblood) // Dusk Herald
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20222, Patch.Stormblood) // Glaring Perch
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20223, Patch.Stormblood) // Abalathian Pipira
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20224, Patch.Stormblood) // Steel Loach
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20225, Patch.Stormblood) // Ivory Sole
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20226, Patch.Stormblood) // Motley Beakfish
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20227, Patch.Stormblood) // Thousandfang
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .Predators ((20217, 7))
                .HookType  (HookSet.None);
            fish.Apply     (20228, Patch.Stormblood) // Ichthyosaur
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20229, Patch.Stormblood) // Sailfin
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20230, Patch.Stormblood) // Fangshi
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .Predators ((20228, 7))
                .HookType  (HookSet.None);
            fish.Apply     (20231, Patch.Stormblood) // Flamefish
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20232, Patch.Stormblood) // Fickle Krait
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20233, Patch.Stormblood) // Eternal Eye
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .Predators ((20222, 7))
                .HookType  (HookSet.None);
            fish.Apply     (20234, Patch.Stormblood) // Soul of the Stallion
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .Predators ((20222, 7))
                .HookType  (HookSet.None);
            fish.Apply     (20235, Patch.Stormblood) // Flood Tuna
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20236, Patch.Stormblood) // Mercenary Crab
                .Gig       (GigHead.Normal)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20237, Patch.Stormblood) // Ashfish
                .Gig       (GigHead.Small)
                .Snag      (Snagging.None)
                .HookType  (HookSet.None);
            fish.Apply     (20238, Patch.Stormblood) // Silken Sunfish
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .Predators ((20236, 7))
                .HookType  (HookSet.None);
            fish.Apply     (20239, Patch.Stormblood) // Mosasaur
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .Predators ((20236, 7))
                .HookType  (HookSet.None);
            fish.Apply     (20524, Patch.Stormblood) // Castaway Chocobo Chick
                .Bait      (2585, 4869, 4904)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (20528, Patch.Stormblood) // Tiny Tatsunoko
                .Gig       (GigHead.Large)
                .Snag      (Snagging.None)
                .Predators ((20217, 7))
                .HookType  (HookSet.None);
        }
        // @formatter:on
    }
}
