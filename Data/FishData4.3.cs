using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        // @formatter:off
        private static void ApplyUnderTheMoonlight(this FishManager fish)
        {
            fish.Apply     (23054, Patch.UnderTheMoonlight) // Shrieker
                .Bait      (fish, 20617, 20112)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23055, Patch.UnderTheMoonlight) // White Goldfish
                .Bait      (fish, 20675, 22397)
                .Tug       (BiteType.Strong)
                .Uptime    (240, 480)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23056, Patch.UnderTheMoonlight) // Firelight Goldfish
                .Bait      (fish, 20675, 22397)
                .Tug       (BiteType.Weak)
                .Uptime    (240, 480)
                .HookType  (HookSet.Precise);
            fish.Apply     (23057, Patch.UnderTheMoonlight) // Hookstealer
                .Bait      (fish, 20615, 20056)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (23058, Patch.UnderTheMoonlight) // Sapphire Fan
                .Bait      (fish, 20675)
                .Tug       (BiteType.Legendary)
                .Weather   (9)
                .HookType  (HookSet.Precise);
            fish.Apply     (23059, Patch.UnderTheMoonlight) // The Archbishop
                .Bait      (fish, 20619)
                .Tug       (BiteType.Legendary)
                .Uptime    (720, 960)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23060, Patch.UnderTheMoonlight) // Bondsplitter
                .Bait      (fish, 20619)
                .Tug       (BiteType.Legendary)
                .Weather   (11)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23061, Patch.UnderTheMoonlight) // The Undecided
                .Bait      (fish, 20615, 20056)
                .Tug       (BiteType.Legendary)
                .Uptime    (480, 720)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23062, Patch.UnderTheMoonlight) // Diamond-eye
                .Bait      (fish, 20616, 20025)
                .Tug       (BiteType.Legendary)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23063, Patch.UnderTheMoonlight) // Rising Dragon
                .Bait      (fish, 20617, 20112)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23064, Patch.UnderTheMoonlight) // The Gambler
                .Bait      (fish, 20676)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23065, Patch.UnderTheMoonlight) // The Winter Queen
                .Bait      (fish, 20676)
                .Tug       (BiteType.Legendary)
                .Uptime    (960, 1200)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23066, Patch.UnderTheMoonlight) // Rakshasa
                .Bait      (fish, 20617, 20112)
                .Tug       (BiteType.Legendary)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23067, Patch.UnderTheMoonlight) // Bokuden
                .Bait      (fish, 20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (720, 840)
                .HookType  (HookSet.Precise);
            fish.Apply     (23068, Patch.UnderTheMoonlight) // Hagoromo Koi
                .Bait      (fish, 20675)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23069, Patch.UnderTheMoonlight) // The Word of God
                .Bait      (fish, 20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (1200, 1440)
                .HookType  (HookSet.Precise);
            fish.Apply     (23070, Patch.UnderTheMoonlight) // Yat Khan
                .Bait      (fish, 20675)
                .Tug       (BiteType.Legendary)
                .Weather   (5)
                .HookType  (HookSet.Precise);
            fish.Apply     (23071, Patch.UnderTheMoonlight) // Curefish
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23072, Patch.UnderTheMoonlight) // Lake Sphairai
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23073, Patch.UnderTheMoonlight) // Warmscale Pleco
                .Bait      (fish, 28634)
                .Tug       (BiteType.Strong)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23074, Patch.UnderTheMoonlight) // Shirogane Clam
                .Bait      (fish, 28634)
                .Tug       (BiteType.Weak)
                .HookType  (HookSet.Precise);
            fish.Apply     (23075, Patch.UnderTheMoonlight) // Illuminati Mask
                .Bait      (fish);
        }
        // @formatter:on
    }
}
