using System;

namespace GatherBuddy.Utility
{
    public static class Levenshtein
    {
        public static int Distance(string lhs, string rhs)
        {
            if (string.IsNullOrEmpty(lhs))
                return string.IsNullOrEmpty(rhs) ? rhs.Length : 0;

            if (string.IsNullOrEmpty(rhs))
                return lhs.Length;

            var d = new int[lhs.Length + 1, rhs.Length + 1];
            for (var i = 0; i <= d.GetUpperBound(0); i += 1)
                d[i, 0] = i;
            for (var i = 0; i <= d.GetUpperBound(1); i += 1)
                d[0, i] = i;
            for (var i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (var j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    var cost = Convert.ToInt32(lhs[i - 1] != rhs[j - 1]);
                    var min1 = d[i - 1, j] + 1;
                    var min2 = d[i, j - 1] + 1;
                    var min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }
    }
}
