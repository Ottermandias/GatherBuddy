using GatherBuddy.Enums;
using GatherBuddy;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyHeavensward(this GameData data)
    {
        data.Apply     (12713, Patch.Heavensward) // Icepick
            .Bait      (data, 12706)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12714, Patch.Heavensward) // Cloud Coral
            .Bait      (data, 2609)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12715, Patch.Heavensward) // Ice Faerie
            .Bait      (data, 12705)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12716, Patch.Heavensward) // Skyworm
            .Bait      (data, 12712)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (12718, Patch.Heavensward) // Coerthan Crab
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12719, Patch.Heavensward) // Fanged Clam
            .Bait      (data, 28634)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12720, Patch.Heavensward) // Lake Urchin
            .Bait      (data, 12711)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (12721, Patch.Heavensward) // Whilom Catfish
            .Bait      (data, 12707)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 3, 4, 11);
        data.Apply     (12722, Patch.Heavensward) // Blueclaw Shrimp
            .Bait      (data, 12704)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (12723, Patch.Heavensward) // Starflower
            .Bait      (data, 12708)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Weather   (data, 2, 1);
        data.Apply     (12724, Patch.Heavensward) // Glacier Core
            .Bait      (data, 12708)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Weather   (data, 16, 15);
        data.Apply     (12725, Patch.Heavensward) // Ogre Horn Snail
            .Bait      (data, 12704)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12726, Patch.Heavensward) // Sorcerer Fish
            .Bait      (data, 2599, 4937)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (480, 1200);
        data.Apply     (12727, Patch.Heavensward) // Hotrod
            .Bait      (data, 12705, 12715)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 15, 16);
        data.Apply     (12728, Patch.Heavensward) // Maiboi
            .Bait      (data, 12704)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12729, Patch.Heavensward) // Three-lip Carp
            .Bait      (data, 12707)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12730, Patch.Heavensward) // Bullfrog
            .Bait      (data, 12707)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (12731, Patch.Heavensward) // Cloudfish
            .Bait      (data, 12704)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12732, Patch.Heavensward) // Mahu Wai
            .Bait      (data, 12708)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12733, Patch.Heavensward) // Rock Mussel
            .Bait      (data, 12706)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (12734, Patch.Heavensward) // Buoyant Oviform
            .Bait      (data, 12708)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12735, Patch.Heavensward) // Whiteloom
            .Bait      (data, 12708)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12736, Patch.Heavensward) // Blue Cloud Coral
            .Bait      (data, 12708)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (12737, Patch.Heavensward) // Seema Patrician
            .Bait      (data, 12706)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12738, Patch.Heavensward) // Ammonite
            .Bait      (data, 12706)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12739, Patch.Heavensward) // Bubble Eye
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (600, 1080);
        data.Apply     (12740, Patch.Heavensward) // Grass Carp
            .Bait      (data, 12707)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 11, 3, 4);
        data.Apply     (12741, Patch.Heavensward) // Pipira Pira
            .Bait      (data, 12706)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 11, 4, 3);
        data.Apply     (12742, Patch.Heavensward) // Dravanian Squeaker
            .Bait      (data, 12704)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (960, 1140);
        data.Apply     (12743, Patch.Heavensward) // Kissing Fish
            .Bait      (data, 12711)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (540, 120);
        data.Apply     (12744, Patch.Heavensward) // Mitre Slug
            .Bait      (data, 12708)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12745, Patch.Heavensward) // Lava Crab
            .Bait      (data, 28634)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12746, Patch.Heavensward) // Storm Core
            .Bait      (data, 12708)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Weather   (data, 3, 4, 5);
        data.Apply     (12747, Patch.Heavensward) // Scholar Sculpin
            .Bait      (data, 12706)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12748, Patch.Heavensward) // Gigant Grouper
            .Bait      (data, 12706)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12749, Patch.Heavensward) // Vanuhead
            .Bait      (data, 12707)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 1, 2);
        data.Apply     (12750, Patch.Heavensward) // Marble Oscar
            .Bait      (data, 28634)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12751, Patch.Heavensward) // Lungfish
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 3);
        data.Apply     (12752, Patch.Heavensward) // Tigerfish
            .Bait      (data, 12707, 12730)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12753, Patch.Heavensward) // Sky Faerie
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12754, Patch.Heavensward) // Granite Crab
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12755, Patch.Heavensward) // Aithon's Colt
            .Bait      (data, 12709)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12756, Patch.Heavensward) // Shipworm
            .Bait      (data, 12706)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12757, Patch.Heavensward) // Hedgemole Cricket
            .Bait      (data, 12705)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12758, Patch.Heavensward) // Mogpom
            .Bait      (data, 29717)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12759, Patch.Heavensward) // Magma Tree
            .Bait      (data, 12709)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (12760, Patch.Heavensward) // Cloud Rider
            .Bait      (data, 12708)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12761, Patch.Heavensward) // Dravanian Bass
            .Bait      (data, 12704, 12722)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (0, 360)
            .Weather   (data, 11, 3, 4);
        data.Apply     (12762, Patch.Heavensward) // Coerthan Puffer
            .Bait      (data, 12705)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Weather   (data, 15, 16);
        data.Apply     (12763, Patch.Heavensward) // Snowcaller
            .Bait      (data, 2607, 12715)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 15, 16);
        data.Apply     (12764, Patch.Heavensward) // Dragonhead
            .Bait      (data, 12706)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12765, Patch.Heavensward) // Mercy Staff
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 15, 16);
        data.Apply     (12766, Patch.Heavensward) // Rime Eater
            .Bait      (data, 12708, 12724)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Weather   (data, 15, 16);
        data.Apply     (12767, Patch.Heavensward) // Warmwater Bichir
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (1260, 180);
        data.Apply     (12768, Patch.Heavensward) // Noontide Oscar
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12769, Patch.Heavensward) // Shadowhisker
            .Bait      (data, 29717)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12770, Patch.Heavensward) // Gobbie Mask
            .Bait      (data, 12712)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12771, Patch.Heavensward) // Blue Medusa
            .Bait      (data, 28634, 12753)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12772, Patch.Heavensward) // Cindersmith
            .Bait      (data, 12709)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (12773, Patch.Heavensward) // Bullwhip
            .Bait      (data, 12710)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12774, Patch.Heavensward) // Tiny Axolotl
            .Bait      (data, 12711)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (1260, 1440);
        data.Apply     (12775, Patch.Heavensward) // High Allagan Helmet
            .Bait      (data, 12710, 12776)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12776, Patch.Heavensward) // Platinum Fish
            .Bait      (data, 12710)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (12777, Patch.Heavensward) // Aether Eye
            .Bait      (data, 12705)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.None);
        data.Apply     (12778, Patch.Heavensward) // Azysfish
            .Bait      (data, 12710)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12779, Patch.Heavensward) // Crystalfin
            .Bait      (data, 12707)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12780, Patch.Heavensward) // Sweetfish
            .Bait      (data, 12706)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Snag      (Snagging.None);
        data.Apply     (12781, Patch.Heavensward) // Orn Butterfly
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12782, Patch.Heavensward) // Hundred Fin
            .Bait      (data, 12706)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12783, Patch.Heavensward) // Autumn Leaf
            .Bait      (data, 12705)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12784, Patch.Heavensward) // Manasail
            .Bait      (data, 28634, 12753, 12805)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (600, 840)
            .Weather   (data, 1, 2);
        data.Apply     (12785, Patch.Heavensward) // Sky Sweeper
            .Bait      (data, 28634, 12753)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12786, Patch.Heavensward) // Magma Louse
            .Bait      (data, 12709, 12754)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (1080, 300);
        data.Apply     (12787, Patch.Heavensward) // Cometoise
            .Bait      (data, 28634, 12754)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12788, Patch.Heavensward) // Aetherochemical Compound #123
            .Bait      (data, 12710, 12776)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12789, Patch.Heavensward) // Brown Bolo
            .Bait      (data, 12708)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 3);
        data.Apply     (12790, Patch.Heavensward) // Philosopher's Stone
            .Bait      (data, 12707)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12791, Patch.Heavensward) // Fountfish
            .Bait      (data, 12706)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12792, Patch.Heavensward) // Weston Bowfin
            .Bait      (data, 12706)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (480, 720);
        data.Apply     (12793, Patch.Heavensward) // Letter Puffer
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12794, Patch.Heavensward) // Star Faerie
            .Bait      (data, 12712, 12805)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12795, Patch.Heavensward) // Gloaming Coral
            .Bait      (data, 12708)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (12796, Patch.Heavensward) // Albino Octopus
            .Bait      (data, 28634, 12715)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (480, 1020);
        data.Apply     (12797, Patch.Heavensward) // Dragon's Soul
            .Bait      (data, 29717)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12798, Patch.Heavensward) // Tornado Shark
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12799, Patch.Heavensward) // Warballoon
            .Bait      (data, 12712, 12716)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Weather   (data, 2, 1);
        data.Apply     (12800, Patch.Heavensward) // Fossiltongue
            .Bait      (data, 12709)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Snag      (Snagging.None);
        data.Apply     (12801, Patch.Heavensward) // Proto-hropken
            .Bait      (data, 12710, 12776)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12802, Patch.Heavensward) // Caiman
            .Bait      (data, 12707, 12730)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1080, 1230);
        data.Apply     (12803, Patch.Heavensward) // Euphotic Pirarucu
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (1080, 120)
            .Weather   (data, 3);
        data.Apply     (12804, Patch.Heavensward) // Illuminati Perch
            .Bait      (data, 12704, 12757)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12805, Patch.Heavensward) // Rudderfish
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12806, Patch.Heavensward) // Bomb Puffer
            .Bait      (data, 12708)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12807, Patch.Heavensward) // Mucous Minnow
            .Bait      (data, 12710, 12776)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12808, Patch.Heavensward) // Unidentified Flying Biomass
            .Bait      (data, 12708)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12809, Patch.Heavensward) // Hospitalier Fish
            .Bait      (data, 12705)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12810, Patch.Heavensward) // Scorpionfly
            .Bait      (data, 28634)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12811, Patch.Heavensward) // Rockclimber
            .Bait      (data, 12707)
            .Bite      (HookSet.Precise, BiteType.Weak);
        data.Apply     (12812, Patch.Heavensward) // Blood Skipper
            .Bait      (data, 12710)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 9);
        data.Apply     (12813, Patch.Heavensward) // Cobrafish
            .Bait      (data, 12712, 12805)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12814, Patch.Heavensward) // Moogle Spirit
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12815, Patch.Heavensward) // Oil Eel
            .Bait      (data, 30136, 12776)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 9);
        data.Apply     (12816, Patch.Heavensward) // Jeweled Jellyfish
            .Bait      (data, 30136, 12776)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (1200, 180);
        data.Apply     (12817, Patch.Heavensward) // Battle Galley
            .Bait      (data, 12705, 12715)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (12818, Patch.Heavensward) // Yalm Lobster
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12819, Patch.Heavensward) // Hinterlands Perch
            .Bait      (data, 28634)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (12820, Patch.Heavensward) // Oven Catfish
            .Bait      (data, 28634, 12754)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12821, Patch.Heavensward) // Pteranodon
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (540, 1020);
        data.Apply     (12822, Patch.Heavensward) // Winged Gurnard
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12823, Patch.Heavensward) // Spring Urchin
            .Bait      (data, 12711)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Snag      (Snagging.Required)
            .ForceBig  (false);
        data.Apply     (12824, Patch.Heavensward) // Cherry Trout
            .Bait      (data, 12707)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12825, Patch.Heavensward) // Stupendemys
            .Bait      (data, 12712, 12805)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (12826, Patch.Heavensward) // Black Magefish
            .Bait      (data, 12709, 12754)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12827, Patch.Heavensward) // Barreleye
            .Bait      (data, 30136, 12776)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 9);
        data.Apply     (12828, Patch.Heavensward) // Thunderbolt Eel
            .Bait      (data, 12704, 12722)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Time      (1320, 210);
        data.Apply     (12829, Patch.Heavensward) // Catkiller
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 2, 1);
        data.Apply     (12830, Patch.Heavensward) // Loosetongue
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (780, 1200);
        data.Apply     (12831, Patch.Heavensward) // Thaliak Caiman
            .Bait      (data, 12707, 12730)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (900, 1080);
        data.Apply     (12832, Patch.Heavensward) // Lavalord
            .Bait      (data, 28634, 12754)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (540, 960)
            .Weather   (data, 2, 1);
        data.Apply     (12833, Patch.Heavensward) // Tupuxuara
            .Bait      (data, 12708)
            .Bite      (HookSet.Powerful, BiteType.Legendary)
            .Time      (900, 1080);
        data.Apply     (12834, Patch.Heavensward) // Vampiric Tapestry
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Strong);
        data.Apply     (12835, Patch.Heavensward) // Storm Chaser
            .Bait      (data, 12712)
            .Bite      (HookSet.Powerful, BiteType.Legendary);
        data.Apply     (12836, Patch.Heavensward) // Berserker Betta
            .Bait      (data, 12711)
            .Bite      (HookSet.Powerful, BiteType.Strong)
            .Weather   (data, 2, 1);
        data.Apply     (12837, Patch.Heavensward) // Capelin
            .Bait      (data, 12711)
            .Bite      (HookSet.Precise, BiteType.Weak)
            .Time      (0, 360);
    }
    // @formatter:on
}
