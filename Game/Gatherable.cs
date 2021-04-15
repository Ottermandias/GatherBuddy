using System;
using System.Collections.Generic;
using GatherBuddy.Nodes;
using GatherBuddy.Utility;
using ItemRow = Lumina.Excel.GeneratedSheets.Item;
using GatheringRow = Lumina.Excel.GeneratedSheets.GatheringItem;

namespace GatherBuddy.Game
{
    public class Gatherable : IComparable
    {
        public ItemRow       ItemData      { get; }
        public GatheringRow  GatheringData { get; }
        public FFName        Name          { get; } = new();
        public HashSet<Node> NodeList      { get; } = new();

        public uint ItemId
            => ItemData.RowId;

        public uint GatheringId
            => GatheringData.RowId;

        private readonly int _levelStars;

        public Gatherable(ItemRow itemData, GatheringRow gatheringData, int level, int stars)
        {
            ItemData      = itemData;
            GatheringData = gatheringData;
            _levelStars   = (level << 3) + stars;
        }

        public int Level
            => _levelStars >> 3;

        public int Stars
            => _levelStars & 0b111;

        public string StarsString()
            => StarsArray[Stars];

        public override string ToString()
            => $"{Name} ({Level}{StarsString()})";

        public int CompareTo(object obj)
        {
            var rhs = obj as Gatherable;
            return ItemId.CompareTo(rhs?.ItemId ?? 0);
        }


        private static readonly string[] StarsArray =
        {
            "",
            "*",
            "**",
            "***",
            "****",
        };
    }
}
