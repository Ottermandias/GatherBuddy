using System;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Classes
{
    public class NodeMeta : IComparable
    {
        public GatheringType GatheringType { get; }
        public NodeType      NodeType      { get; }
        public int           PointBaseId   { get; }
        public int           Level         { get; }

        public NodeMeta(GatheringPointBase row, NodeType type)
        {
            Level         = row.GatheringLevel;
            PointBaseId   = (int) row.RowId;
            GatheringType = (GatheringType) row.GatheringType.Row;
            NodeType      = type;
        }

        public bool IsMiner()
            => GatheringType.ToGroup() == GatheringType.Miner;

        public bool IsBotanist()
            => GatheringType.ToGroup() == GatheringType.Botanist;

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            var rhs = obj as NodeMeta;
            return PointBaseId.CompareTo(rhs?.PointBaseId ?? 0);
        }
    }
}
