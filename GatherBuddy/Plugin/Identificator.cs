using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game;
using GatherBuddy.Classes;

namespace GatherBuddy.Plugin;

public class Identificator
{
    public const int MaxDistance = 4;

    private readonly GameData                               _data;
    private readonly FrozenDictionary<string, Gatherable>[] _gatherableFromLanguage;
    private readonly FrozenDictionary<string, Fish>[]       _fishFromLanguage;

    public Identificator()
    {
        _data = GatherBuddy.GameData;
        var languages = new[]
        {
            GatherBuddy.Language,
            (ClientLanguage)(((int)GatherBuddy.Language + 1) % 4),
            (ClientLanguage)(((int)GatherBuddy.Language + 2) % 4),
            (ClientLanguage)(((int)GatherBuddy.Language + 3) % 4),
        };

        _gatherableFromLanguage = languages.Select(CreateGatherableDictionary).ToArray();
        _fishFromLanguage       = languages.Select(CreateFishDictionary).ToArray();
    }

    private FrozenDictionary<string, Gatherable> CreateGatherableDictionary(ClientLanguage l)
    {
        var dict = new Dictionary<string, Gatherable>(_data.Gatherables.Count);
        foreach (var (gatherable, name) in _data.Gatherables.Values.Select(g => (g, g.Name[l].ToLowerInvariant())))
        {
            if (!dict.TryAdd(name, gatherable))
            {
#if DEBUG
                GatherBuddy.Log.Warning($"Item Name Overlap in {l}: {gatherable.ItemId} and {dict[name].ItemId} resolve to {name}.");
#endif
            }
        }

        return dict.ToFrozenDictionary();
    }

    private FrozenDictionary<string, Fish> CreateFishDictionary(ClientLanguage l)
    {
        var dict = new Dictionary<string, Fish>(_data.Fishes.Count);
        foreach (var (fish, name) in _data.Fishes.Values.Select(f => (f, f.Name[l].ToLowerInvariant())))
        {
            if (!dict.TryAdd(name, fish))
            {
#if DEBUG
                GatherBuddy.Log.Warning($"Item Name Overlap in {l}: {fish.ItemId} and {dict[name].ItemId} resolve to {name}.");
#endif
            }
        }

        return dict.ToFrozenDictionary();
    }

    private static bool SearchContains<T>(FrozenDictionary<string, T> dict, string name, out T? ret) where T : class
    {
        ret = null;
        var length = int.MaxValue;
        foreach (var (n, obj) in dict)
        {
            if (length < 0)
            {
                if (n.Length >= -length || !n.StartsWith(name))
                    continue;

                ret    = obj;
                length = -n.Length;
            }
            else if (n.Length < length)
            {
                if (!n.Contains(name))
                    continue;

                ret = obj;
                if (n.StartsWith(name))
                    length = -n.Length;
                else
                    length = n.Length;
            }
            else if (n.StartsWith(name))
            {
                ret    = obj;
                length = -n.Length;
            }
        }

        return ret != null;
    }

    public Gatherable? IdentifyGatherable(string itemName)
    {
        if (itemName.Length == 0)
            return null;

        // Check for full matches in current language first, by initialization order.
        var itemNameLower = itemName.ToLowerInvariant();
        foreach (var dict in _gatherableFromLanguage)
        {
            if (dict.TryGetValue(itemNameLower, out var item))
                return item;
        }

        // Search for the shortest object in the current language that starts with the given string.
        // If none does, use the shortest object that contains the given string.
        if (SearchContains(_gatherableFromLanguage[0], itemNameLower, out var ret))
            return ret;

        // Check for fuzzy matches up to the given MaxDistance.
        return _data.GatherablesTrie.FuzzyFind(itemNameLower, MaxDistance, out var data) < MaxDistance ? data : null;
    }

    public Fish? IdentifyFish(string itemName)
    {
        if (itemName.Length == 0)
            return null;

        // Same as for gatherables.
        var itemNameLower = itemName.ToLowerInvariant();
        foreach (var dict in _fishFromLanguage)
        {
            if (dict.TryGetValue(itemNameLower, out var item))
                return item;
        }

        if (SearchContains(_fishFromLanguage[0], itemNameLower, out var ret))
            return ret;

        return _data.FishTrie.FuzzyFind(itemNameLower, MaxDistance, out var data) < MaxDistance ? data : null;
    }
}
