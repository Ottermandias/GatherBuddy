using Gathering;
using System.Collections.Generic;
using System.Linq;

namespace GatherBuddyPlugin
{
    public class NodeTimeLine
    {
        public readonly Dictionary<NodeType, Dictionary<GatheringType, List<Node>>> timedNodes = new();

        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node lhs, Node rhs)
            {
                return rhs.meta.level - lhs.meta.level;
            }
        }

        public NodeTimeLine(NodeManager nodes)
        {
            timedNodes = new()
            {
                { 
                    NodeType.Unspoiled, new()
                    { 
                        { GatheringType.Botanist, new() }, 
                        { GatheringType.Miner,    new() } 
                    }
                },
                {
                    NodeType.Ephemeral, new()
                    {
                        { GatheringType.Botanist, new() }, 
                        { GatheringType.Miner,    new() } 
                    }
                }
            };

            foreach(var node in nodes.BaseNodes())
            {
                if (node.meta.nodeType == NodeType.Regular)
                    continue;
                if (node.meta.gatheringType == GatheringType.Spearfishing)
                    continue;

                timedNodes[node.meta.nodeType][node.meta.gatheringType.ToGroup()].Add(node);
            }
            timedNodes[NodeType.Unspoiled][GatheringType.Miner   ].Sort( new NodeComparer() );
            timedNodes[NodeType.Unspoiled][GatheringType.Botanist].Sort( new NodeComparer() );
            timedNodes[NodeType.Ephemeral][GatheringType.Miner   ].Sort( new NodeComparer() );
            timedNodes[NodeType.Ephemeral][GatheringType.Botanist].Sort( new NodeComparer() );
        }

        public List<(Node, int)> GetNewList(bool unspoiled, bool ephemeral, bool miner, bool botanist, bool arr, bool hw, bool sb, bool shb, bool ew)
        {
            IEnumerable<Node> list = Enumerable.Empty<Node>();

            bool LevelCheck(Node N)
            {
                if (N.meta.level <= 50)
                    return arr;
                if (N.meta.level <= 60)
                    return hw;
                if (N.meta.level <= 70)
                    return sb;
                if (N.meta.level <= 80)
                    return shb;
                if (N.meta.level <= 90)
                    return ew;
                return false;
            }

            if (unspoiled)
            {
                if (miner)
                    list = list.Concat(timedNodes[NodeType.Unspoiled][GatheringType.Miner]).Where( N => LevelCheck(N) );
                if (botanist)
                    list = list.Concat(timedNodes[NodeType.Unspoiled][GatheringType.Botanist]).Where( N => LevelCheck(N) );
            }
            if (ephemeral)
            {
                if (miner)
                    list = list.Concat(timedNodes[NodeType.Ephemeral][GatheringType.Miner]).Where( N => LevelCheck(N) );
                if (botanist)
                    list = list.Concat(timedNodes[NodeType.Ephemeral][GatheringType.Botanist]).Where( N => LevelCheck(N) );
            }

            return list.Select( N => (N, 25) ).ToList();
        }

        private class Comparer : IComparer<(Node, int)>
        {
            public int Compare((Node, int) lhs, (Node, int) rhs)
            {
                if (lhs.Item2 != rhs.Item2)
                    return lhs.Item2 - rhs.Item2;
                return rhs.Item1.meta.level - lhs.Item1.meta.level;
            }
        }

        private static void UpdateUptimes(int currentHour, List<(Node, int)> nodes)
        {
            for(var i = 0; i < nodes.Count; ++i)
                nodes[i] = (nodes[i].Item1, nodes[i].Item1.times.NextUptime(currentHour));
        }

        public static void SortByUptime(int currentHour, List<(Node, int)> nodes)
        {
            UpdateUptimes(currentHour, nodes);
            nodes.Sort( new Comparer() );
        }
    }
}