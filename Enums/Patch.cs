namespace GatherBuddy.Enums
{
    public enum Patch : ushort
    {
        ARealmReborn               = 200,
        ARealmAwoken               = 210,
        ThroughTheMaelstrom        = 220,
        DefendersOfEorzea          = 230,
        DreamsOfIce                = 240,
        BeforeTheFall              = 250,
        Heavensward                = 300,
        AsGoesLightSoGoesDarkness  = 310,
        TheGearsOfChange           = 320,
        RevengeOfTheHorde          = 330,
        SoulSurrender              = 340,
        TheFarEdgeOfFate           = 350,
        Stormblood                 = 400,
        TheLegendReturns           = 410,
        RiseOfANewSun              = 420,
        UnderTheMoonlight          = 430,
        PreludeInViolet            = 440,
        ARequiemForHeroes          = 450,
        Shadowbringers             = 500,
        VowsOfVirtueDeedsOfCruelty = 510,
        EchoesOfAFallenStar        = 520,
        ReflectionsInCrystal       = 530,
        FuturesRewritten           = 540,
        DeathUntoDawn              = 550,
        Endwalker                  = 600,
    }

    public static class PatchExtensions
    {
        public static byte ToMajor(this Patch value)
            => (byte) ((ushort) value / 100);

        public static byte ToMinor(this Patch value)
        {
            var val = (ushort) value % 100;
            if (val % 10 == 0)
                return (byte) (val / 10);

            return (byte) val;
        }

        public static Patch ToExpansion(this Patch value)
            => (Patch) (value.ToMajor() * 100);

        public static string ToVersionString(this Patch value)
            => $"{value.ToMajor()}.{value.ToMinor()}";

        public static string ToPatchName(this Patch value)
        {
            return ((ushort) value / 10) switch
            {
                20 => "A Realm Reborn",
                21 => "A Realm Awoken",
                22 => "Through The Maelstrom",
                23 => "Defenders Of Eorzea",
                24 => "Dreams Of Ice",
                25 => "Before The Fall",
                30 => "Heavensward",
                31 => "As Goes Light, So Goes Darkness",
                32 => "The Gears Of Change",
                33 => "Revenge Of The Horde",
                34 => "Soul Surrender",
                35 => "The Far Edge Of Fate",
                40 => "Stormblood",
                41 => "The Legend Returns",
                42 => "Rise Of A New Sun",
                43 => "Under The Moonlight",
                44 => "Prelude In Violet",
                45 => "A Requiem For Heroes",
                50 => "Shadowbringers",
                51 => "Vows Of Virtue, Deeds Of Cruelty",
                52 => "Echoes Of A Fallen Star",
                53 => "Reflections In Crystal",
                54 => "Futures Rewritten",
                55 => "Death Unto Dawn",
                60 => "Endwalker",
                _  => "Unknown",
            };
        }
    }
}
