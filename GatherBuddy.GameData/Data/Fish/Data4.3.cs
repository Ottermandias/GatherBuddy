using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyUnderTheMoonlight(this GameData data)
    {
        data.Apply     (23054, Patch.UnderTheMoonlight) // Shrieker
            .Mooch     (data, 20617, 20112)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (23055, Patch.UnderTheMoonlight) // White Goldfish
            .Mooch     (data, 20675, 22397)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (23056, Patch.UnderTheMoonlight) // Firelight Goldfish
            .Mooch     (data, 20675, 22397)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (23057, Patch.UnderTheMoonlight) // Hookstealer
            .Mooch     (data, 20615, 20056)
            .Bite      (data, HookSet.Precise, BiteType.Legendary);
        data.Apply     (23058, Patch.UnderTheMoonlight) // Sapphire Fan
            .Bait      (data, 20675)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Weather   (data, 9);
        data.Apply     (23059, Patch.UnderTheMoonlight) // The Archbishop
            .Bait      (data, 20619)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (720, 960);
        data.Apply     (23060, Patch.UnderTheMoonlight) // Bondsplitter
            .Bait      (data, 20619)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 11);
        data.Apply     (23061, Patch.UnderTheMoonlight) // The Undecided
            .Mooch     (data, 20615, 20056)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (480, 720);
        data.Apply     (23062, Patch.UnderTheMoonlight) // Diamond-eye
            .Mooch     (data, 20616, 20025)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 1);
        data.Apply     (23063, Patch.UnderTheMoonlight) // Rising Dragon
            .Mooch     (data, 20617, 20112)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (23064, Patch.UnderTheMoonlight) // The Gambler
            .Bait      (data, 20676)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary);
        data.Apply     (23065, Patch.UnderTheMoonlight) // The Winter Queen
            .Bait      (data, 20676)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Time      (960, 1200);
        data.Apply     (23066, Patch.UnderTheMoonlight) // Rakshasa
            .Mooch     (data, 20617, 20112)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 9);
        data.Apply     (23067, Patch.UnderTheMoonlight) // Bokuden
            .Bait      (data, 20675)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Time      (720, 840)
            .Slap      ( data, 20115);
        data.Apply     (23068, Patch.UnderTheMoonlight) // Hagoromo Koi
            .Bait      (data, 20675)
            .Bite      (data, HookSet.Powerful, BiteType.Legendary)
            .Weather   (data, 4);
        data.Apply     (23069, Patch.UnderTheMoonlight) // The Word of God
            .Bait      (data, 20675)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Time      (1200, 1440);
        data.Apply     (23070, Patch.UnderTheMoonlight) // Yat Khan
            .Bait      (data, 20675)
            .Bite      (data, HookSet.Precise, BiteType.Legendary)
            .Weather   (data, 5);
        data.Apply     (23071, Patch.UnderTheMoonlight) // Curefish
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (23072, Patch.UnderTheMoonlight) // Lake Sphairai
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (23073, Patch.UnderTheMoonlight) // Warmscale Pleco
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
        data.Apply     (23074, Patch.UnderTheMoonlight) // Shirogane Clam
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Precise, BiteType.Weak);
        data.Apply     (23075, Patch.UnderTheMoonlight) // Illuminati Mask
            .Bait      (data, 28634)
            .Bite      (data, HookSet.Powerful, BiteType.Strong);
    }
    // @formatter:on
}
