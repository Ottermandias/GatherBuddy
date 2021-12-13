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
            public int Compare(Node? lhs, Node? rhs)
                => (rhs?.Meta?.Level ?? 0) - (lhs?.Meta?.Level ?? 0);
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

        private ShowNodes ExpansionForNode(Node n)
        {
            var addon = n.Nodes?.Territory?.Data.ExVersion.Row ?? uint.MaxValue;
            if (addon == 0 && n.Meta!.Level <= 50)
                return ShowNodes.ARealmReborn;
            if (addon <= 1 && n.Meta!.Level <= 60)
                return ShowNodes.Heavensward;
            if (addon <= 2 && n.Meta!.Level <= 70)
                return ShowNodes.Stormblood;
            if (addon <= 3 && n.Meta!.Level <= 80)
                return ShowNodes.Shadowbringers;
            if (addon <= 4 && n.Meta!.Level <= 90)
                return ShowNodes.Endwalker;

            return ShowNodes.AllNodes;
        }

        public List<(Node, uint)> GetNewList(ShowNodes which)
        {
            var list = Enumerable.Empty<Node>();

            bool LevelCheck(Node n)
                => which.HasFlag(ExpansionForNode(n));

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

        private static void UpdateUptimes(int currentHour, IList<(Node, uint)> nodes)
        {
            for (var i = 0; i < nodes.Count; ++i)
                nodes[i] = (nodes[i].Item1, nodes[i].Item1.Times!.NextUptime(currentHour));
        }

        public static void SortByUptime(int currentHour, List<(Node, uint)> nodes)
        {
            UpdateUptimes(currentHour, nodes);
            nodes.Sort(new Comparer());
        }
    }
}
