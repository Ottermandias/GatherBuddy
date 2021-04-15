using GatherBuddy.Enums;

namespace GatherBuddy.Gui
{
    public static class Colors
    {
        public struct FishTimer
        {
            public const uint Invalid        = 0x20FFFFFF;
            public const uint Weak           = 0x8030D030;
            public const uint Strong         = 0x8030D0D0;
            public const uint Legendary      = 0x803030D0;
            public const uint Background     = 0xFF000020;
            public const uint Line           = 0xFF000000;
            public const uint EditBackground = 0x20FFFFFF;

            public static uint FromBiteType(BiteType bite)
            {
                return bite switch
                {
                    BiteType.Weak      => Weak,
                    BiteType.Strong    => Strong,
                    BiteType.Legendary => Legendary,
                    BiteType.Unknown   => Invalid,
                    _                  => Invalid,
                };
            }
        }

        public struct HeaderRow
        {
            public const uint EorzeaTime     = 0xFF008080;
            public const uint NextEorzeaHour = 0xFF404040;
            public const uint Weather        = 0xFFA0A000;
        }
    }
}
