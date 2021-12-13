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

        public class ComparisonComparer<T> : IComparer<T>
        {
            private readonly Comparison<T> _comparison;

            public ComparisonComparer(Comparison<T> comparison)
                => _comparison = comparison;

            public int Compare(T? x, T? y)
                => x != null ? y != null ? _comparison(x, y) : 1 : y != null ? -1 : 0;
        }
    }
}
