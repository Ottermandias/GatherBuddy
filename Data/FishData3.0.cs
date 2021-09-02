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
                .Bait      (fish, 12706)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12714, Patch.Heavensward) // Cloud Coral
                .Bait      (fish, 2609)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12715, Patch.Heavensward) // Ice Faerie
                .Bait      (fish, 12705)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12716, Patch.Heavensward) // Skyworm
                .Bait      (fish, 12712)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (12718, Patch.Heavensward) // Coerthan Crab
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12719, Patch.Heavensward) // Fanged Clam
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12720, Patch.Heavensward) // Lake Urchin
                .Bait      (fish, 12711)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (12721, Patch.Heavensward) // Whilom Catfish
                .Bait      (fish, 12707)
                .Tug       (BiteType.Strong)
                .Weather   (3, 4, 11)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12722, Patch.Heavensward) // Blueclaw Shrimp
                .Bait      (fish, 12704)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (12723, Patch.Heavensward) // Starflower
                .Bait      (fish, 12708)
                .Tug       (BiteType.Weak)
                .Weather   (2, 1)
                .HookType  (HookSet.Precise);
            fish.Apply     (12724, Patch.Heavensward) // Glacier Core
                .Bait      (fish, 12708)
                .Tug       (BiteType.Weak)
                .Weather   (16, 15)
                .HookType  (HookSet.Precise);
            fish.Apply     (12725, Patch.Heavensward) // Ogre Horn Snail
                .Bait      (fish, 12704)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12726, Patch.Heavensward) // Sorcerer Fish
                .Bait      (fish, 2599, 4937)
                .Tug       (BiteType.Strong)
                .Uptime    (480, 1200)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12727, Patch.Heavensward) // Hotrod
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .Weather   (15, 16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12728, Patch.Heavensward) // Maiboi
                .Bait      (fish, 12704)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12729, Patch.Heavensward) // Three-lip Carp
                .Bait      (fish, 12707)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12730, Patch.Heavensward) // Bullfrog
                .Bait      (fish, 12707)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (12731, Patch.Heavensward) // Cloudfish
                .Bait      (fish, 12704)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12732, Patch.Heavensward) // Mahu Wai
                .Bait      (fish, 12708)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12733, Patch.Heavensward) // Rock Mussel
                .Bait      (fish, 12706)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (12734, Patch.Heavensward) // Buoyant Oviform
                .Bait      (fish, 12708)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12735, Patch.Heavensward) // Whiteloom
                .Bait      (fish, 12708)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12736, Patch.Heavensward) // Blue Cloud Coral
                .Bait      (fish, 12708)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (12737, Patch.Heavensward) // Seema Patrician
                .Bait      (fish, 12706)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12738, Patch.Heavensward) // Ammonite
                .Bait      (fish, 12706)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12739, Patch.Heavensward) // Bubble Eye
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .Uptime    (600, 1080)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12740, Patch.Heavensward) // Grass Carp
                .Bait      (fish, 12707)
                .Tug       (BiteType.Strong)
                .Weather   (11, 3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12741, Patch.Heavensward) // Pipira Pira
                .Bait      (fish, 12706)
                .Tug       (BiteType.Strong)
                .Weather   (11, 4, 3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12742, Patch.Heavensward) // Dravanian Squeaker
                .Bait      (fish, 12704)
                .Tug       (BiteType.Weak)
                .Uptime    (960, 1140)
                .HookType  (HookSet.Precise);
            fish.Apply     (12743, Patch.Heavensward) // Kissing Fish
                .Bait      (fish, 12704)
                .Tug       (BiteType.Weak)
                .Uptime    (540, 120)
                .HookType  (HookSet.Precise);
            fish.Apply     (12744, Patch.Heavensward) // Mitre Slug
                .Bait      (fish, 12708)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12745, Patch.Heavensward) // Lava Crab
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12746, Patch.Heavensward) // Storm Core
                .Bait      (fish, 12708)
                .Tug       (BiteType.Weak)
                .Weather   (3, 4, 5)
                .HookType  (HookSet.Precise);
            fish.Apply     (12747, Patch.Heavensward) // Scholar Sculpin
                .Bait      (fish, 12706)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12748, Patch.Heavensward) // Gigant Grouper
                .Bait      (fish, 12706)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12749, Patch.Heavensward) // Vanuhead
                .Bait      (fish, 12707)
                .Tug       (BiteType.Strong)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12750, Patch.Heavensward) // Marble Oscar
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12751, Patch.Heavensward) // Lungfish
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12752, Patch.Heavensward) // Tigerfish
                .Bait      (fish, 12707, 12730)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12753, Patch.Heavensward) // Sky Faerie
                .Bait      (fish, 12708)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12754, Patch.Heavensward) // Granite Crab
                .Bait      (fish, 12709)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12755, Patch.Heavensward) // Aithon's Colt
                .Bait      (fish, 12709)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12756, Patch.Heavensward) // Shipworm
                .Bait      (fish, 12706)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12757, Patch.Heavensward) // Hedgemole Cricket
                .Bait      (fish, 12704)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12758, Patch.Heavensward) // Mogpom
                .Bait      (fish, 29717)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12759, Patch.Heavensward) // Magma Tree
                .Bait      (fish, 12709)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (12760, Patch.Heavensward) // Cloud Rider
                .Bait      (fish, 12708)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12761, Patch.Heavensward) // Dravanian Bass
                .Bait      (fish, 12704, 12722)
                .Tug       (BiteType.Strong)
                .Uptime    (0, 360)
                .Weather   (11, 3, 4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12762, Patch.Heavensward) // Coerthan Puffer
                .Bait      (fish, 12705)
                .Tug       (BiteType.Weak)
                .Weather   (15, 16)
                .HookType  (HookSet.Precise);
            fish.Apply     (12763, Patch.Heavensward) // Snowcaller
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .Weather   (15, 16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12764, Patch.Heavensward) // Dragonhead
                .Bait      (fish, 12706)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12765, Patch.Heavensward) // Mercy Staff
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .Weather   (15, 16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12766, Patch.Heavensward) // Rime Eater
                .Bait      (fish, 12708, 12724)
                .Tug       (BiteType.Weak)
                .Weather   (15, 16)
                .HookType  (HookSet.Precise);
            fish.Apply     (12767, Patch.Heavensward) // Warmwater Bichir
                .Bait      (fish, 12705)
                .Tug       (BiteType.Strong)
                .Uptime    (1260, 180)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12768, Patch.Heavensward) // Noontide Oscar
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12769, Patch.Heavensward) // Shadowhisker
                .Bait      (fish, 29717)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12770, Patch.Heavensward) // Gobbie Mask
                .Bait      (fish, 12712)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12771, Patch.Heavensward) // Blue Medusa
                .Bait      (fish, 12708, 12753)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12772, Patch.Heavensward) // Cindersmith
                .Bait      (fish, 12709)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12773, Patch.Heavensward) // Bullwhip
                .Bait      (fish, 12710)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12774, Patch.Heavensward) // Tiny Axolotl
                .Bait      (fish, 12711)
                .Tug       (BiteType.Weak)
                .Uptime    (1260, 1440)
                .HookType  (HookSet.Precise);
            fish.Apply     (12775, Patch.Heavensward) // High Allagan Helmet
                .Bait      (fish, 12710, 12776)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12776, Patch.Heavensward) // Platinum Fish
                .Bait      (fish, 12710)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (12777, Patch.Heavensward) // Aether Eye
                .Bait      (fish, 12705)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Precise);
            fish.Apply     (12778, Patch.Heavensward) // Azysfish
                .Bait      (fish, 12710)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12779, Patch.Heavensward) // Crystalfin
                .Bait      (fish, 12707)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12780, Patch.Heavensward) // Sweetfish
                .Bait      (fish, 12706)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12781, Patch.Heavensward) // Orn Butterfly
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12782, Patch.Heavensward) // Hundred Fin
                .Bait      (fish, 12706)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12783, Patch.Heavensward) // Autumn Leaf
                .Bait      (fish, 12705)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12784, Patch.Heavensward) // Manasail
                .Bait      (fish, 12708, 12753, 12805)
                .Tug       (BiteType.Strong)
                .Uptime    (600, 840)
                .Weather   (1, 2)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12785, Patch.Heavensward) // Sky Sweeper
                .Bait      (fish, 12708, 12753)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12786, Patch.Heavensward) // Magma Louse
                .Bait      (fish, 12709, 12754)
                .Tug       (BiteType.Strong)
                .Uptime    (1080, 300)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12787, Patch.Heavensward) // Cometoise
                .Bait      (fish, 12709, 12754)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12788, Patch.Heavensward) // Aetherochemical Compound #123
                .Bait      (fish, 12710, 12776)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12789, Patch.Heavensward) // Brown Bolo
                .Bait      (fish, 12708)
                .Tug       (BiteType.Strong)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12790, Patch.Heavensward) // Philosopher's Stone
                .Bait      (fish, 12707)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12791, Patch.Heavensward) // Fountfish
                .Bait      (fish, 12706)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12792, Patch.Heavensward) // Weston Bowfin
                .Bait      (fish, 12706)
                .Tug       (BiteType.Strong)
                .Uptime    (480, 720)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12793, Patch.Heavensward) // Letter Puffer
                .Bait      (fish, 12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12794, Patch.Heavensward) // Star Faerie
                .Bait      (fish, 12712, 12805)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12795, Patch.Heavensward) // Gloaming Coral
                .Bait      (fish, 12708)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (12796, Patch.Heavensward) // Albino Octopus
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .Uptime    (480, 1020)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12797, Patch.Heavensward) // Dragon's Soul
                .Bait      (fish, 29717)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12798, Patch.Heavensward) // Tornado Shark
                .Bait      (fish, 12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12799, Patch.Heavensward) // Warballoon
                .Bait      (fish, 12712, 12716)
                .Tug       (BiteType.Weak)
                .Weather   (2, 1)
                .HookType  (HookSet.Precise);
            fish.Apply     (12800, Patch.Heavensward) // Fossiltongue
                .Bait      (fish, 12709)
                .Tug       (BiteType.Strong)
                .Snag      (Snagging.None)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12801, Patch.Heavensward) // Proto-hropken
                .Bait      (fish, 12710, 12776)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12802, Patch.Heavensward) // Caiman
                .Bait      (fish, 12707, 12730)
                .Tug       (BiteType.Legendary)
                .Uptime    (1080, 1230)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12803, Patch.Heavensward) // Euphotic Pirarucu
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (1080, 120)
                .Weather   (3)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12804, Patch.Heavensward) // Illuminati Perch
                .Bait      (fish, 12704, 12757)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12805, Patch.Heavensward) // Rudderfish
                .Bait      (fish, 12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12806, Patch.Heavensward) // Bomb Puffer
                .Bait      (fish, 12708)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12807, Patch.Heavensward) // Mucous Minnow
                .Bait      (fish, 12710, 12776)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12808, Patch.Heavensward) // Unidentified Flying Biomass
                .Bait      (fish, 12708)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12809, Patch.Heavensward) // Hospitalier Fish
                .Bait      (fish, 12705)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12810, Patch.Heavensward) // Scorpionfly
                .Bait      (fish, 12712)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12811, Patch.Heavensward) // Rockclimber
                .Bait      (fish, 12707)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (12812, Patch.Heavensward) // Blood Skipper
                .Bait      (fish, 12710)
                .Tug       (BiteType.Strong)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12813, Patch.Heavensward) // Cobrafish
                .Bait      (fish, 12712, 12805)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12814, Patch.Heavensward) // Moogle Spirit
                .Bait      (fish, 12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12815, Patch.Heavensward) // Oil Eel
                .Bait      (fish, 12710, 12776)
                .Tug       (BiteType.Strong)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12816, Patch.Heavensward) // Jeweled Jellyfish
                .Bait      (fish, 12710, 12776)
                .Tug       (BiteType.Weak)
                .Uptime    (1200, 180)
                .HookType  (HookSet.Precise);
            fish.Apply     (12817, Patch.Heavensward) // Battle Galley
                .Bait      (fish, 12705, 12715)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12818, Patch.Heavensward) // Yalm Lobster
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12819, Patch.Heavensward) // Hinterlands Perch
                .Bait      (fish, 28634)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12820, Patch.Heavensward) // Oven Catfish
                .Bait      (fish, 12709, 12754)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12821, Patch.Heavensward) // Pteranodon
                .Bait      (fish, 12712)
                .Tug       (BiteType.Legendary)
                .Uptime    (540, 1020)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12822, Patch.Heavensward) // Winged Gurnard
                .Bait      (fish, 12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12823, Patch.Heavensward) // Spring Urchin
                .Bait      (fish, 12711)
                .Tug       (BiteType.Weak)
                .Snag      (Snagging.Required)
                .HookType  (HookSet.Precise);
            fish.Apply     (12824, Patch.Heavensward) // Cherry Trout
                .Bait      (fish, 12707)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12825, Patch.Heavensward) // Stupendemys
                .Bait      (fish, 12712, 12805)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12826, Patch.Heavensward) // Black Magefish
                .Bait      (fish, 12709, 12754)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12827, Patch.Heavensward) // Barreleye
                .Bait      (fish, 12710, 12776)
                .Tug       (BiteType.Strong)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12828, Patch.Heavensward) // Thunderbolt Eel
                .Bait      (fish, 12704, 12722)
                .Tug       (BiteType.Strong)
                .Uptime    (1320, 210)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12829, Patch.Heavensward) // Catkiller
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12830, Patch.Heavensward) // Loosetongue
                .Bait      (fish, 12711)
                .Tug       (BiteType.Legendary)
                .Uptime    (780, 1200)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12831, Patch.Heavensward) // Thaliak Caiman
                .Bait      (fish, 12707, 12730)
                .Tug       (BiteType.Legendary)
                .Uptime    (900, 1080)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12832, Patch.Heavensward) // Lavalord
                .Bait      (fish, 12709, 12754)
                .Tug       (BiteType.Legendary)
                .Uptime    (540, 960)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12833, Patch.Heavensward) // Tupuxuara
                .Bait      (fish, 12708)
                .Tug       (BiteType.Legendary)
                .Uptime    (900, 1080)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12834, Patch.Heavensward) // Vampiric Tapestry
                .Bait      (fish, 12712)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12835, Patch.Heavensward) // Storm Chaser
                .Bait      (fish, 12712)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12836, Patch.Heavensward) // Berserker Betta
                .Bait      (fish, 12711)
                .Tug       (BiteType.Strong)
                .Weather   (2, 1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (12837, Patch.Heavensward) // Capelin
                .Bait      (fish, 12704)
                .Tug       (BiteType.Weak)
                .Uptime    (0, 360)
                .HookType  (HookSet.Precise);
        }
        // @formatter:on
    }
}
