using System;

namespace GatherBuddy.Utility
{
    public static class Util
    {
        public static bool CompareCi(string lhs, string rhs)
            => string.Compare(lhs, rhs, StringComparison.InvariantCultureIgnoreCase) == 0;

        public static bool TryParseBoolean(string text, out bool parsed)
        {
            parsed = false;
            if (text.Length == 1)
            {
                if (text[0] != '1')
                    return text[0] == '0';

                parsed = true;
                return true;
            }

            if (!CompareCi(text,       "on") && !CompareCi(text, "true"))
                return CompareCi(text, "off") || CompareCi(text, "false");

            parsed = true;
            return true;
        }

        public static int MapMarkerToMap(double coord, double scale)
            => (int) (100.0 * (coord * 41.0 / 2048.0 / scale + 1.0));

        public static string RemoveSplitMarkers(string s)
            => s.Replace("\x02\x16\x01\x03", "");

        public static string RemoveItalics(string s)
            => s.Replace("\x02\x1a\x02\x02\x03", "").Replace("\x02\x1a\x02\x01\x03", "");
    }
}
