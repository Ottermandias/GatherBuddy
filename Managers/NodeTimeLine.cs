using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Enums;
using GatherBuddy.Nodes;

namespace GatherBuddy.Managers
{
    public class NodeTimeLine
    {
        public Dictionary<NodeType, Dictionary<GatheringType, List<Node>>> TimedNodes { get; }

        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node lhs, Node rhs)
                => rhs!.Meta!.Level - lhs!.Meta!.Level;
        }

        public NodeTimeLine(NodeManager nodes)
        {
            TimedNodes = new Dictionary<NodeType, Dictionary<GatheringType, List<Node>>>()
            {
                {
                    NodeType.Unspoiled, new Dictionary<GatheringType, List<Node>>()
                    {
                        { GatheringType.Botanist, new List<Node>() },
                        { GatheringType.Miner, new List<Node>() },
                    }
                },
                {
                    NodeType.Ephemeral, new Dictionary<GatheringType, List<Node>>()
                    {
                        { GatheringType.Botanist, new List<Node>() },
                        { GatheringType.Miner, new List<Node>() },
                    }
                },
            };

            foreach (var node in nodes.BaseNodes())
            {
                if (node.Meta!.NodeType == NodeType.Regular)
                    continue;
                if (node.Meta!.GatheringType == GatheringType.Spearfishing)
                    continue;

                TimedNodes[node.Meta.NodeType][node.Meta.GatheringType.ToGroup()].Add(node);
            }

            TimedNodes[NodeType.Unspoiled][GatheringType.Miner].Sort(new NodeComparer());
            TimedNodes[NodeType.Unspoiled][GatheringType.Botanist].Sort(new NodeComparer());
            TimedNodes[NodeType.Ephemeral][GatheringType.Miner].Sort(new NodeComparer());
            TimedNodes[NodeType.Ephemeral][GatheringType.Botanist].Sort(new NodeComparer());
        }

        public List<(Node, uint)> GetNewList(ShowNodes which)
        {
            var list = Enumerable.Empty<Node>();

            bool LevelCheck(Node n)
                => n.Meta!.Level switch
                {
                    <= 50 => which.HasFlag(ShowNodes.ARealmReborn),
                    <= 60 => which.HasFlag(ShowNodes.Heavensward),
                    <= 70 => which.HasFlag(ShowNodes.Stormblood),
                    <= 80 => which.HasFlag(ShowNodes.Shadowbringers),
                    <= 90 => which.HasFlag(ShowNodes.Endwalker),
                    _     => false,
                };

            if (which.HasFlag(ShowNodes.Unspoiled))
            {
                if (which.HasFlag(ShowNodes.Mining))
                    list = list.Concat(TimedNodes[NodeType.Unspoiled][GatheringType.Miner]).Where(LevelCheck);
                if (which.HasFlag(ShowNodes.Botanist))
                    list = list.Concat(TimedNodes[NodeType.Unspoiled][GatheringType.Botanist]).Where(LevelCheck);
            }

            if (which.HasFlag(ShowNodes.Ephemeral))
            {
                if (which.HasFlag(ShowNodes.Mining))
                    list = list.Concat(TimedNodes[NodeType.Ephemeral][GatheringType.Miner]).Where(LevelCheck);
                if (which.HasFlag(ShowNodes.Botanist))
                    list = list.Concat(TimedNodes[NodeType.Ephemeral][GatheringType.Botanist]).Where(LevelCheck);
            }

            return list.Select(n => (n, 25u)).ToList();
        }

        private class Comparer : IComparer<(Node, uint)>
        {
            public int Compare((Node, uint) lhs, (Node, uint) rhs)
            {
                if (lhs.Item2 != rhs.Item2)
                    return (int) (lhs.Item2 - rhs.Item2);

                return rhs.Item1.Meta!.Level - lhs.Item1.Meta!.Level;
            }
        }

        private static void UpdateUptimes(uint currentHour, IList<(Node, uint)> nodes)
        {
            for (var i = 0; i < nodes.Count; ++i)
                nodes[i] = (nodes[i].Item1, nodes[i].Item1.Times!.NextUptime(currentHour));
        }

        public static void SortByUptime(uint currentHour, List<(Node, uint)> nodes)
        {
            UpdateUptimes(currentHour, nodes);
            nodes.Sort(new Comparer());
        }
    }
}
