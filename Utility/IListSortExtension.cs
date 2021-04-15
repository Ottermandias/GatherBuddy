using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GatherBuddy.Utility
{
    public static class ListSortExtension
    {
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, Comparison<T> comparison)
            => list.OrderBy(t => t, new ComparisonComparer<T>(comparison));

        public class ComparisonComparer<T> : IComparer<T>, IComparer
        {
            private readonly Comparison<T> _comparison;

            public ComparisonComparer(Comparison<T> comparison)
                => _comparison = comparison;

            public int Compare(T x, T y)
                => _comparison(x, y);

            public int Compare(object x, object y)
                => _comparison((T) x, (T) y);
        }
    }
}
