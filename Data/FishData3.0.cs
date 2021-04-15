using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyHeavensward(this FishManager fish)
        {
            fish.Apply     (12713, Patch.Heavensward) // Icepick
                .Bait      (12706)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12714, Patch.Heavensward) // Cloud Coral
                .Bait      (2609)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12715, Patch.Heavensward) // Ice Faerie
                .Bait      (12705)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12716, Patch.Heavensward) // Skyworm
                .Bait      (12712)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (12718, Patch.Heavensward) // Coerthan Crab
                .Bait      (28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12719, Patch.Heavensward) // Fanged Clam
                .Bait      (28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12720, Patch.Heavensward) // Lake Urchin
                .Bait      (12711)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (12721, Patch.Heavensward) // Whilom Catfish
                .Bait      (12707)
                .Tug       (BiteType.Strong)
                .Weather   (3, 4, 11)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12722, Patch.Heavensward) // Blueclaw Shrimp
                .Bait      (12704)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (12723, Patch.Heavensward) // Starflower
                .Bait      (12708)
                .Tug       (BiteType.Weak)
                .Weather   (2, 1)
                .HookType  (HookSet.Precise);
            fish.Apply     (12724, Patch.Heavensward) // Glacier Core
                .Bait      (12708)
                .Tug       (BiteType.Weak)
                .Weather   (16, 15)
                .HookType  (HookSet.Precise);
            fish.Apply     (12725, Patch.Heavensward) // Ogre Horn Snail
                .Bait      (12704)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12726, Patch.Heavensward) // Sorcerer Fish
                .Bait      (2599, 4937)
                .Tug       (BiteType.Strong)
                .Uptime    (8, 20)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12727, Patch.Heavensward) // Hotrod
                .Bait      (12711)
                .Tug       (BiteType.Strong)
                .Weather   (15, 16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12728, Patch.Heavensward) // Maiboi
                .Bait      (12704)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12729, Patch.Heavensward) // Three-lip Carp
                .Bait      (12707);
            fish.Apply     (12730, Patch.Heavensward) // Bullfrog
                .Bait      (12707)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (12731, Patch.Heavensward) // Cloudfish
                .Bait      (12704)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12732, Patch.Heavensward) // Mahu Wai
                .Bait      (12708)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12733, Patch.Heavensward) // Rock Mussel
                .Bait      (12706)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (12734, Patch.Heavensward) // Buoyant Oviform
                .Bait      (12708)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12735, Patch.Heavensward) // Whiteloom
                .Bait      (12708)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12736, Patch.Heavensward) // Blue Cloud Coral
                .Bait      (12708)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (12737, Patch.Heavensward) // Seema Patrician
                .Bait      (12706)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12738, Patch.Heavensward) // Ammonite
                .Bait      (12706)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12739, Patch.Heavensward) // Bubble Eye
                .Bait      (12711)
                .Tug       (BiteType.Strong)
                .Uptime    (10, 18)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12740, Patch.Heavensward) // Grass Carp
                .Bait      (12707)
                .Tug       (BiteType.Strong)
                .Weather   (11, 3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12741, Patch.Heavensward) // Pipira Pira
                .Bait      (12706)
                .Tug       (BiteType.Strong)
                .Weather   (11, 4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12742, Patch.Heavensward) // Dravanian Squeaker
                .Bait      (12704)
                .Tug       (BiteType.Weak)
                .Uptime    (16, 19)
                .HookType  (HookSet.Precise);
            fish.Apply     (12743, Patch.Heavensward) // Kissing Fish
                .Bait      (12704)
                .Tug       (BiteType.Weak)
                .Uptime    (9, 2)
                .HookType  (HookSet.Precise);
            fish.Apply     (12744, Patch.Heavensward) // Mitre Slug
                .Bait      (12708)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12745, Patch.Heavensward) // Lava Crab
                .Bait      (12709);
            fish.Apply     (12746, Patch.Heavensward) // Storm Core
                .Bait      (12708)
                .Tug       (BiteType.Weak)
                .Weather   (3, 4, 5)
                .HookType  (HookSet.Precise);
            fish.Apply     (12747, Patch.Heavensward) // Scholar Sculpin
                .Bait      (12705);
            fish.Apply     (12748, Patch.Heavensward) // Gigant Grouper
                .Bait      (12706)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12749, Patch.Heavensward) // Vanuhead
                .Bait      (12707)
                .Tug       (BiteType.Strong)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12750, Patch.Heavensward) // Marble Oscar
                .Bait      (12705)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12751, Patch.Heavensward) // Lungfish
                .Bait      (12711)
                .Tug       (BiteType.Strong)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12752, Patch.Heavensward) // Tigerfish
                .Bait      (12711)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12753, Patch.Heavensward) // Sky Faerie
                .Bait      (12708)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12754, Patch.Heavensward) // Granite Crab
                .Bait      (12709)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12755, Patch.Heavensward) // Aithon's Colt
                .Bait      (12709)
                .HookType  (HookSet.Precise);
            fish.Apply     (12756, Patch.Heavensward) // Shipworm
                .Bait      (12706);
            fish.Apply     (12757, Patch.Heavensward) // Hedgemole Cricket
                .Bait      (12704)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12758, Patch.Heavensward) // Mogpom
                .Bait      (12706)
                .HookType  (HookSet.Precise);
            fish.Apply     (12759, Patch.Heavensward) // Magma Tree
                .Bait      (12709)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (12760, Patch.Heavensward) // Cloud Rider
                .Bait      (12708);
            fish.Apply     (12761, Patch.Heavensward) // Dravanian Bass
                .Bait      (12704, 12722)
                .Uptime    (0, 6)
                .Weather   (11, 3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12762, Patch.Heavensward) // Coerthan Puffer
                .Bait      (12705)
                .Tug       (BiteType.Weak)
                .Weather   (15, 16)
                .HookType  (HookSet.Precise);
            fish.Apply     (12763, Patch.Heavensward) // Snowcaller
                .Bait      (12711)
                .Tug       (BiteType.Strong)
                .Weather   (15, 16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12764, Patch.Heavensward) // Dragonhead
                .Bait      (12706);
            fish.Apply     (12765, Patch.Heavensward) // Mercy Staff
                .Bait      (12711)
                .Tug       (BiteType.Strong)
                .Weather   (15, 16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12766, Patch.Heavensward) // Rime Eater
                .Bait      (12708, 12724)
                .Tug       (BiteType.Weak)
                .Weather   (15, 16)
                .HookType  (HookSet.Precise);
            fish.Apply     (12767, Patch.Heavensward) // Warmwater Bichir
                .Bait      (12705)
                .Tug       (BiteType.Strong)
                .Uptime    (21, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12768, Patch.Heavensward) // Noontide Oscar
                .Bait      (12711)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12769, Patch.Heavensward) // Shadowhisker
                .Bait      (12711);
            fish.Apply     (12770, Patch.Heavensward) // Gobbie Mask
                .Bait      (12712);
            fish.Apply     (12771, Patch.Heavensward) // Blue Medusa
                .Bait      (12708);
            fish.Apply     (12772, Patch.Heavensward) // Cindersmith
                .Bait      (12709)
                .HookType  (HookSet.Precise);
            fish.Apply     (12773, Patch.Heavensward) // Bullwhip
                .Bait      (12710)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12774, Patch.Heavensward) // Tiny Axolotl
                .Bait      (12711)
                .Tug       (BiteType.Weak)
                .Uptime    (21, 24)
                .HookType  (HookSet.Precise);
            fish.Apply     (12775, Patch.Heavensward) // High Allagan Helmet
                .Bait      (12710, 12776)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12776, Patch.Heavensward) // Platinum Fish
                .Bait      (12710)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (12777, Patch.Heavensward) // Aether Eye
                .Bait      (12705)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (12778, Patch.Heavensward) // Azysfish
                .Bait      (12710);
            fish.Apply     (12779, Patch.Heavensward) // Crystalfin
                .Bait      (12707);
            fish.Apply     (12780, Patch.Heavensward) // Sweetfish
                .Bait      (12706)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12781, Patch.Heavensward) // Orn Butterfly
                .Bait      (12711)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12782, Patch.Heavensward) // Hundred Fin
                .Bait      (12706);
            fish.Apply     (12783, Patch.Heavensward) // Autumn Leaf
                .Bait      (12705)
                .HookType  (HookSet.Precise);
            fish.Apply     (12784, Patch.Heavensward) // Manasail
                .Bait      (12708, 12753, 12805)
                .Tug       (BiteType.Strong)
                .Uptime    (10, 14)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12785, Patch.Heavensward) // Sky Sweeper
                .Bait      (12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12786, Patch.Heavensward) // Magma Louse
                .Bait      (12709, 12754)
                .Tug       (BiteType.Strong)
                .Uptime    (18, 5)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12787, Patch.Heavensward) // Cometoise
                .Bait      (12709, 12754)
                .Tug       (BiteType.Strong);
            fish.Apply     (12788, Patch.Heavensward) // Aetherochemical Compound #123
                .Bait      (12710, 12776)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12789, Patch.Heavensward) // Brown Bolo
                .Bait      (12708)
                .Tug       (BiteType.Strong)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12790, Patch.Heavensward) // Philosopher's Stone
                .Bait      (12707);
            fish.Apply     (12791, Patch.Heavensward) // Fountfish
                .Bait      (12706)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12792, Patch.Heavensward) // Weston Bowfin
                .Bait      (12706)
                .Tug       (BiteType.Strong)
                .Uptime    (8, 12)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12793, Patch.Heavensward) // Letter Puffer
                .Bait      (12712);
            fish.Apply     (12794, Patch.Heavensward) // Star Faerie
                .Bait      (12708)
                .HookType  (HookSet.Precise);
            fish.Apply     (12795, Patch.Heavensward) // Gloaming Coral
                .Bait      (12708)
                .Snag      (Snagging.Required);
            fish.Apply     (12796, Patch.Heavensward) // Albino Octopus
                .Bait      (12711)
                .Tug       (BiteType.Strong)
                .Uptime    (8, 17)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12797, Patch.Heavensward) // Dragon's Soul
                .Bait      (12712);
            fish.Apply     (12798, Patch.Heavensward) // Tornado Shark
                .Bait      (12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12799, Patch.Heavensward) // Warballoon
                .Bait      (12712, 12716)
                .Tug       (BiteType.Weak)
                .Weather   (2, 1)
                .HookType  (HookSet.Precise);
            fish.Apply     (12800, Patch.Heavensward) // Fossiltongue
                .Bait      (12709)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12801, Patch.Heavensward) // Proto-hropken
                .Bait      (12710, 12776);
            fish.Apply     (12802, Patch.Heavensward) // Caiman
                .Bait      (12707, 12730)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 21)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12803, Patch.Heavensward) // Euphotic Pirarucu
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (18, 2)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12804, Patch.Heavensward) // Illuminati Perch
                .Bait      (12704, 12757)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12805, Patch.Heavensward) // Rudderfish
                .Bait      (12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12806, Patch.Heavensward) // Bomb Puffer
                .Bait      (12712)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12807, Patch.Heavensward) // Mucous Minnow
                .Bait      (12710)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12808, Patch.Heavensward) // Unidentified Flying Biomass
                .Bait      (12708);
            fish.Apply     (12809, Patch.Heavensward) // Hospitalier Fish
                .Bait      (12705)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12810, Patch.Heavensward) // Scorpionfly
                .Bait      (12712)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12811, Patch.Heavensward) // Rockclimber
                .Bait      (12707);
            fish.Apply     (12812, Patch.Heavensward) // Blood Skipper
                .Bait      (12710)
                .Tug       (BiteType.Strong)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12813, Patch.Heavensward) // Cobrafish
                .Bait      (12712)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12814, Patch.Heavensward) // Moogle Spirit
                .Bait      (12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12815, Patch.Heavensward) // Oil Eel
                .Bait      (12710, 12776)
                .Tug       (BiteType.Strong)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12816, Patch.Heavensward) // Jeweled Jellyfish
                .Bait      (12710, 12776)
                .Tug       (BiteType.Weak)
                .Uptime    (20, 3)
                .HookType  (HookSet.Precise);
            fish.Apply     (12817, Patch.Heavensward) // Battle Galley
                .Bait      (12705, 12715)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12818, Patch.Heavensward) // Yalm Lobster
                .Bait      (12707)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12819, Patch.Heavensward) // Hinterlands Perch
                .Bait      (12707)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12820, Patch.Heavensward) // Oven Catfish
                .Bait      (12709, 12754);
            fish.Apply     (12821, Patch.Heavensward) // Pteranodon
                .Bait      (12712)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12822, Patch.Heavensward) // Winged Gurnard
                .Bait      (12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12823, Patch.Heavensward) // Spring Urchin
                .Bait      (12711)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (12824, Patch.Heavensward) // Cherry Trout
                .Bait      (12707)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12825, Patch.Heavensward) // Stupendemys
                .Bait      (12712, 12805)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12826, Patch.Heavensward) // Black Magefish
                .Bait      (12709, 12754)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12827, Patch.Heavensward) // Barreleye
                .Bait      (12710, 12776)
                .Tug       (BiteType.Strong)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12828, Patch.Heavensward) // Thunderbolt Eel
                .Bait      (12704, 12722)
                .Tug       (BiteType.Strong)
                .Uptime    (22, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12829, Patch.Heavensward) // Catkiller
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12830, Patch.Heavensward) // Loosetongue
                .Bait      (12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (13, 20)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12831, Patch.Heavensward) // Thaliak Caiman
                .Bait      (12707, 12730)
                .Tug       (BiteType.Legendary)
                .Uptime    (15, 18)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12832, Patch.Heavensward) // Lavalord
                .Bait      (12709, 12754)
                .Tug       (BiteType.Legendary)
                .Uptime    (9, 16)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12833, Patch.Heavensward) // Tupuxuara
                .Bait      (12708)
                .Tug       (BiteType.Legendary)
                .Uptime    (15, 18)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12834, Patch.Heavensward) // Vampiric Tapestry
                .Bait      (12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12835, Patch.Heavensward) // Storm Chaser
                .Bait      (12712)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12836, Patch.Heavensward) // Berserker Betta
                .Bait      (12711)
                .Tug       (BiteType.Strong)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12837, Patch.Heavensward) // Capelin
                .Bait      (12704)
                .Tug       (BiteType.Weak)
                .Uptime    (0, 6)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
