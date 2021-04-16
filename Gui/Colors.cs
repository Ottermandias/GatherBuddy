using System.Numerics;
using GatherBuddy.Enums;
using ImGuiNET;

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
            public const uint RectBackground = 0x80000000;
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

        public struct WeatherTab
        {
            public const uint CurrentWeather       = 0xFF008000;
            public const uint LastWeather          = 0xFF000080;
            public const uint HeaderCurrentWeather = 0x1000FF00;
            public const uint HeaderLastWeather    = 0x100000FF;
        }

        public struct FishTab
        {
            public const uint WeakBite      = 0xFFFF6020;
            public const uint StrongBite    = 0xFF60D030;
            public const uint LegendaryBite = 0xFF0040C0;
            public const uint UnknownBite   = 0xFF404040;

            public const uint Time     = HeaderRow.EorzeaTime;
            public const uint Weather  = HeaderRow.Weather;
            public const uint Bait     = 0xFF0000A0;
            public const uint Predator = LegendaryBite;

            public const uint Patch     = 0xFFC0C0C0;
            public const uint PatchText = 0xFF000000;
            public const uint Folklore  = 0xFF802080;

            public static readonly Vector4 WeatherVec4    = ImGui.ColorConvertU32ToFloat4(Weather);
            public static readonly Vector4 UptimeRunning  = new(0f, 0.75f, 0f, 1f);
            public static readonly Vector4 UptimeUnknown  = new(0.75f, 0.75f, 0f, 1f);
            public static readonly Vector4 UptimeUpcoming = UptimeUnknown;
        }

        public struct NodeTab
        {
            public static readonly Vector4 Default     = new(0.8f, 0.8f, 0.8f, 1.0f);
            public static readonly Vector4 CurrentlyUp = new(0.0f, 1.0f, 0.0f, 1.0f);
            public static readonly Vector4 SoonUp      = new(0.6f, 1.0f, 0.4f, 1.0f);
            public static readonly Vector4 SoonishUp   = new(1.0f, 1.0f, 0.0f, 1.0f);
            public static readonly Vector4 LateUp      = new(1.0f, 1.0f, 0.2f, 1.0f);
            public static readonly Vector4 FarUp       = new(1.0f, 1.0f, 0.6f, 1.0f);

            public static Vector4 GetColor(Nodes.Node n, uint hour)
            {
                Vector4 colors;
                if (n.Times!.IsUp(hour))
                    colors = CurrentlyUp;
                else if (n.Times.IsUp(hour + 1))
                    colors = SoonUp;
                else if (n.Times.IsUp(hour + 2))
                    colors = SoonishUp;
                else if (n.Times.IsUp(hour + 3))
                    colors = LateUp;
                else if (n.Times.IsUp(hour + 4))
                    colors = FarUp;
                else
                    colors = Default;
                return colors;
            }
        }
    }
}
