using System;

namespace Otter
{
    public static class Util
    {

        public static bool CompareCI(string lhs, string rhs)
        {
            return string.Compare(lhs, rhs, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public static bool TryParseBoolean(string text, out bool parsed)
        {
            parsed = false;
            if (text.Length == 1)
            {
                if (text[0] == '1')
                {
                    parsed = true;
                    return true;
                }
                else 
                    return text[0] == '0';
            }
            
            if (CompareCI(text, "on") || CompareCI(text, "true"))
            {
                parsed = true;
                return true;
            }

            if (CompareCI(text, "off") || CompareCI(text, "false"))
                return true;
            return false;
        }

        public static int MapMarkerToMap(double coord, double scale)
        {
            return (int) (100.0 * (coord * 41.0 / 2048.0 / scale + 1.0));
        }

        public static string RemoveSplitMarkers(string s)
        {
            return s.Replace("\x02\x16\x01\x03", "");
        }

        public static string RemoveItalics(string s)
        {
            return s.Replace("\x02\x1a\x02\x02\x03", "").Replace("\x02\x1a\x02\x01\x03", "");
        }
    }
}