using System;
using System.Collections.Generic;
using GatherBuddy.Utility;

namespace GatherBuddy.Classes
{
    public class Gatherable : IComparable
    {
        public int           ItemId      { get; }
        public int           GatheringId { get; }
        public FFName        NameList    { get; } = new();
        public HashSet<Node> NodeList    { get; } = new();

        private readonly int _levelStars;

        public Gatherable(int id, int gatheringId, int level, int stars)
        {
            ItemId      = id;
            GatheringId = gatheringId;
            _levelStars = (level << 3) + stars;
        }

        public int Level
            => _levelStars >> 3;

        public int Stars
            => _levelStars & 0b111;

        public string StarsString()
            => StarsArray[Stars];

        public override string ToString()
            => $"{NameList[Dalamud.ClientLanguage.English]} ({Level}{StarsString()})";

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

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
