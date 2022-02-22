using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GatherBuddy.Levenshtein;

public partial class PatriciaTrie<T> : IEnumerable<(StringView, T)>
{
    public const int DeleteCost  = 8;
    public const int InsertCost  = 12;
    public const int ReplaceCost = 16;

    public readonly  Node                         Root                = Node.CreateRoot();
    private readonly Dictionary<StringView, Node> _leafs              = new();
    private readonly List<List<ushort>>           _tableCache         = new() { new List<ushort>() };
    private          int                          _longestCheckedWord = 0;

    public int FuzzyFind(string word, int maxDistance, out T? data)
    {
        var smallestDistance = maxDistance + 1;
        data = default;
        if (word.Length == 0)
            return smallestDistance;

        if (maxDistance < 1)
            return smallestDistance;

        IncreaseTableColumns(word.Length + 1);

        foreach (var child in Root.Children)
            PopulateTable(word, child, ref data, ref smallestDistance);

        return smallestDistance;
    }

    public int FuzzyFindMatch(string word, int maxDistance, out T? data)
    {
        if (!_leafs.TryGetValue(word, out var node))
            return FuzzyFind(word, maxDistance, out data);

        data = node.Data;
        return 0;
    }

    private void PopulateTable(string word, Node root, ref T? data, ref int smallestDistance)
    {
        for (var i = 1 + root.TotalWord.Length - root.Word.Length; i <= root.TotalWord.Length; ++i)
        {
            for (var j = 1; j <= word.Length; j++)
            {
                var cost = root.TotalWord[i - 1] != word[j - 1] ? ReplaceCost : 0;
                var min1 = _tableCache[i - 1][j] + InsertCost;
                var min2 = _tableCache[i][j - 1] + DeleteCost;
                var min3 = _tableCache[i - 1][j - 1] + cost;
                _tableCache[i][j] = (ushort)Math.Min(Math.Min(min1, min2), min3);
            }
        }

        if (root.HasData)
        {
            var dist = (_tableCache[root.TotalWord.Length][word.Length] + 4) >> 3;
            if (dist < smallestDistance)
            {
                smallestDistance = dist;
                data             = root.Data;
            }
        }

        if (root.TotalWord.Length >= word.Length + smallestDistance)
            return;

        foreach (var child in root.Children)
            PopulateTable(word, child, ref data, ref smallestDistance);
    }

    public IEnumerator<(StringView, T)> GetEnumerator()
        => Enumerate(Root).GetEnumerator();

    private static IEnumerable<(StringView, T)> Enumerate(Node root)
    {
        if (root.HasData)
            yield return (root.TotalWord, root.Data!);

        foreach (var child in root.Children.SelectMany(Enumerate))
            yield return child;
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    private void IncreaseTableRows(int length)
    {
        if (length <= _tableCache.Count)
            return;

        for (var i = _tableCache.Count; i < length; ++i)
        {
            var list = new List<ushort>(_longestCheckedWord + 1) { (ushort)(i * DeleteCost) };
            list.AddRange(Enumerable.Repeat((ushort)0, _longestCheckedWord));
            _tableCache.Add(list);
        }
    }

    private void IncreaseTableColumns(int length)
    {
        if (length <= _longestCheckedWord + 1)
            return;

        if (_tableCache[0].Count < length)
            _tableCache[0].AddRange(Enumerable.Range(_tableCache[0].Count, length - _tableCache[0].Count)
                .Select(i => (ushort)(i * DeleteCost)));

        foreach (var list in _tableCache.Skip(1).Where(list => list.Count < length))
            list.AddRange(Enumerable.Repeat((ushort)0, length - list.Count));

        _longestCheckedWord = length - 1;
    }

    public bool Add(string word, T data)
    {
        if (word.Length == 0 || _leafs.ContainsKey(word))
            return false;

        IncreaseTableRows(word.Length + 1);

        var node = Root;
        var w    = new StringView(word);
        while (true)
        {
            if (node.Children.Count == 0)
            {
                _leafs.Add(word, new Node(node, word, data, true));
                return true;
            }

            var next = false;
            foreach (var child in node.Children.Where(child => child.Word[0] >= w[0]))
            {
                if (child.Word[0] > w[0])
                {
                    _leafs.Add(word, new Node(node, word, data, true));
                    return true;
                }

                var minLength = Math.Min(child.Word.Length, w.Length);
                for (var i = 1; i < minLength; ++i)
                {
                    if (child.Word[i] == w[i])
                        continue;

                    var newNode = child.SplitOff(i);
                    _leafs.Add(word, new Node(newNode, word, data, true));
                    return true;
                }

                // Same length, i.e. already contained case is only possible when the node has no data:
                // since the dictionary check would have caught it otherwise
                if (child.Word.Length == w.Length)
                {
                    child.Data    = data;
                    child.HasData = true;
                    _leafs.Add(word, child);
                    return true;
                }

                if (w.Length == minLength) // Is contained as Substring.
                {
                    _leafs.Add(word, child.SplitOff(w.Length, data, true));
                    return true;
                }

                node = child;
                w    = new StringView(word, child.TotalWord.Length);
                next = true;
                break;
            }

            if (next)
                continue;

            _leafs.Add(word, new Node(node, word, data, true));
            return true;
        }
    }
}
