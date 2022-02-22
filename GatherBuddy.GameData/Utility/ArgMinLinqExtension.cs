using System;
using System.Collections.Generic;

namespace GatherBuddy.Utility;

public static class ArgMinLinqExtension
{
    public static T? ArgMin<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> selector)
        => enumerable.ArgMin(selector, null);

    public static T? ArgMin<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> selector, IComparer<TKey>? comparer)
    {
        comparer ??= Comparer<TKey>.Default;
        using var sourceIterator = enumerable.GetEnumerator();

        if (!sourceIterator.MoveNext())
            return default;

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
