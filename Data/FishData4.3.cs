using GatherBuddy.Classes;
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
                .Bait      (20617, 20112)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23055, Patch.UnderTheMoonlight) // White Goldfish
                .Bait      (20675, 22397)
                .Tug       (BiteType.Strong)
                .Uptime    (4, 8)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23056, Patch.UnderTheMoonlight) // Firelight Goldfish
                .Bait      (20675, 22397)
                .Tug       (BiteType.Weak)
                .Uptime    (4, 8)
                .HookType  (HookSet.Precise);
            fish.Apply     (23057, Patch.UnderTheMoonlight) // Hookstealer
                .Bait      (20615, 20056)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Precise);
            fish.Apply     (23058, Patch.UnderTheMoonlight) // Sapphire Fan
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Weather   (9)
                .HookType  (HookSet.Precise);
            fish.Apply     (23059, Patch.UnderTheMoonlight) // The Archbishop
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 16)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23060, Patch.UnderTheMoonlight) // Bondsplitter
                .Bait      (20619)
                .Tug       (BiteType.Legendary)
                .Weather   (11)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23061, Patch.UnderTheMoonlight) // The Undecided
                .Bait      (20615, 20056)
                .Tug       (BiteType.Legendary)
                .Uptime    (8, 12)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23062, Patch.UnderTheMoonlight) // Diamond-eye
                .Bait      (20616, 20025)
                .Tug       (BiteType.Legendary)
                .Weather   (1)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23063, Patch.UnderTheMoonlight) // Rising Dragon
                .Bait      (20617, 20112)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23064, Patch.UnderTheMoonlight) // The Gambler
                .Bait      (20676)
                .Tug       (BiteType.Legendary)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23065, Patch.UnderTheMoonlight) // The Winter Queen
                .Bait      (20676)
                .Tug       (BiteType.Legendary)
                .Uptime    (16, 20)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23066, Patch.UnderTheMoonlight) // Rakshasa
                .Bait      (20617, 20112)
                .Tug       (BiteType.Legendary)
                .Weather   (9)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23067, Patch.UnderTheMoonlight) // Bokuden
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (12, 14)
                .HookType  (HookSet.Precise);
            fish.Apply     (23068, Patch.UnderTheMoonlight) // Hagoromo Koi
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Weather   (4)
                .HookType  (HookSet.Powerful);
            fish.Apply     (23069, Patch.UnderTheMoonlight) // The Word of God
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Uptime    (20, 24)
                .HookType  (HookSet.Precise);
            fish.Apply     (23070, Patch.UnderTheMoonlight) // Yat Khan
                .Bait      (20675)
                .Tug       (BiteType.Legendary)
                .Weather   (5)
                .HookType  (HookSet.Precise);
            fish.Apply     (23071, Patch.UnderTheMoonlight) // Curefish
                .Bait      (20613);
            fish.Apply     (23072, Patch.UnderTheMoonlight) // Lake Sphairai
                .Bait      (20613);
            fish.Apply     (23073, Patch.UnderTheMoonlight) // Warmscale Pleco
                .Bait      ();
            fish.Apply     (23074, Patch.UnderTheMoonlight) // Shirogane Clam
                .Bait      (20617);
            fish.Apply     (23075, Patch.UnderTheMoonlight) // Illuminati Mask
                .Bait      ();
        }
        // @formatter:on
    }
}
