using System.Collections.Generic;

namespace GatherBuddy.Levenshtein;

public partial class PatriciaTrie<T>
{
    public class Node
    {
        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node? x, Node? y)
                => x?.Word.CompareTo(y?.Word ?? StringView.Empty) ?? StringView.Empty.CompareTo(y?.Word ?? StringView.Empty);

            public static readonly NodeComparer Default = new();
        }

        public          StringView      Word;
        private         Node?           _parent;
        public readonly StringView      TotalWord;
        public readonly SortedSet<Node> Children;
        public          T?              Data;
        public          bool            HasData;

        private Node(bool _)
        {
            _parent   = null;
            Data      = default;
            Word      = StringView.Empty;
            TotalWord = StringView.Empty;
            Children  = new SortedSet<Node>(NodeComparer.Default);
            HasData   = false;
        }

        public static Node CreateRoot()
            => new(true);

        public Node(Node parent, StringView totalWord, T? data, bool hasData)
        {
            Data      = data;
            _parent   = parent;
            Word      = totalWord.Substring(parent.TotalWord.Length, totalWord.Length - parent.TotalWord.Length);
            TotalWord = totalWord;
            HasData   = hasData;
            Children  = new SortedSet<Node>(NodeComparer.Default);
            parent.Children.Add(this);
        }

        public Node SplitOff(int from)
            => SplitOff(from, default, false);

        public Node SplitOff(int from, T? data, bool hasData)
        {
            var newNode = new Node(_parent!, TotalWord.Substring(0, from + TotalWord.Length - Word.Length), data, hasData);
            _parent!.Children.Remove(this);
            Word = Word.Substring(from);
            newNode.Children.Add(this);
            _parent = newNode;
            return newNode;
        }
    }
}
