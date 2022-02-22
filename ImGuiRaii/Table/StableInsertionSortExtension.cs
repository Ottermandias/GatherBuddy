using System;
using System.Collections.Generic;

namespace ImGuiOtter.Table;

public static class StableInsertionSortExtension
{
    public static void StableSort<T>(this IList<T> list, Comparison<T> comp)
    {
        var count = list.Count;
        for(var i = 1; i < count; ++i)
        {
            var elem = list[i];
            var j = i - 1;
            for (; j >= 0 && comp(list[j], elem) > 0; --j)
                list[j + 1] = list[j];
            list[j + 1] = elem;
        }
    }
}
