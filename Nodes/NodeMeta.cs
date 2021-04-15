using System;
using GatherBuddy.Enums;
using GatheringType = GatherBuddy.Enums.GatheringType;
using PointBase = Lumina.Excel.GeneratedSheets.GatheringPointBase;

namespace GatherBuddy.Nodes
{
    public class NodeMeta : IComparable
    {
        public GatheringType GatheringType { get; }
        public NodeType      NodeType      { get; }
        public PointBase     NodeData      { get; }

        public uint PointBaseId
            => NodeData.RowId;

        public int Level { get; }

        public NodeMeta(PointBase row, NodeType type)
        {
            Level         = row.GatheringLevel;
            NodeData      = row;
            GatheringType = (GatheringType) row.GatheringType.Row;
            NodeType      = type;
        }

        public bool IsMiner()
            => GatheringType.ToGroup() == GatheringType.Miner;

        public bool IsBotanist()
            => GatheringType.ToGroup() == GatheringType.Botanist;

        public int CompareTo(object obj)
        {
            var rhs = obj as NodeMeta;
            return PointBaseId.CompareTo(rhs?.PointBaseId ?? 0);
        }
    }
}
