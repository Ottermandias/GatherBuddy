using System;
using System.Collections.Generic;

namespace GatherBuddy.Utility
{
    public static class ArgMinLinqExtension
    {
        public static T ArgMin<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> selector)
            => enumerable.ArgMin(selector, null);

        public static T ArgMin<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> selector, IComparer<TKey>? comparer)
        {
            comparer ??= Comparer<TKey>.Default;
            using var sourceIterator = enumerable.GetEnumerator();

            if (!sourceIterator.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");

            var min    = sourceIterator.Current;
            var minKey = selector(min);
            while (sourceIterator.MoveNext())
            {
                var candidate          = sourceIterator.Current;
                var candidateProjected = selector(candidate);
                if (comparer.Compare(candidateProjected, minKey) >= 0)
                    continue;

                min    = candidate;
                minKey = candidateProjected;
            }

            return min;
        }
    }

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

        public static uint Popcount(uint value)
        {
            value -= (value >> 1) & 0x55555555u;
            value =  (value & 0x33333333u) + ((value >> 2) & 0x33333333u);
            return (((value + (value >> 4)) & 0xF0F0F0Fu) * 0x1010101u) >> 24;
        }

        public static uint HighestSetBit(uint value)
        {
            var r = 0u; // result of log2(v) will go here
            if ((value & 0xFFFF0000u) != 0u)
            {
                value >>= 16;
                r     |=  16;
            }

            if ((value & 0xFF00u) != 0u)
            {
                value >>= 8;
                r     |=  8;
            }

            if ((value & 0xF0u) != 0u)
            {
                value >>= 4;
                r     |=  4;
            }

            if ((value & 0xCu) != 0u)
            {
                value >>= 2;
                r     |=  2;
            }

            return (value & 0x2) != 0u ? r | 1 : r;
        }

        public static uint TrailingZeroCount(uint value)
        {
            if ((value & 0x1u) == 0x1u)
                return 0;

            var ret = 1;
            if ((value & 0xFFFFu) == 0)
            {
                value >>= 16;
                ret   +=  16;
            }

            if ((value & 0xFFu) == 0)
            {
                value >>= 8;
                ret   +=  8;
            }

            if ((value & 0xFu) == 0)
            {
                value >>= 4;
                ret   +=  4;
            }

            if ((value & 0x3u) == 0)
            {
                value >>= 2;
                ret   +=  2;
            }

            return (uint) (ret - (value & 0x1));
        }

        public static int MapMarkerToMap(double coord, double scale)
            => (int) (100.0 * (coord * 41.0 / 2048.0 / scale + 1.0));

        public static string RemoveSplitMarkers(string s)
            => s.Replace("\x02\x16\x01\x03", "");

        public static string RemoveItalics(string s)
            => s.Replace("\x02\x1a\x02\x02\x03", "").Replace("\x02\x1a\x02\x01\x03", "");
    }
}
