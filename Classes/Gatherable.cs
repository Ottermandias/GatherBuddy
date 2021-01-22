using System;
using System.Collections.Generic;
using Otter;

namespace Gathering
{
    public class Gatherable : IComparable
    {
        public Gatherable(int id, int gatheringId, int level, int stars)
        {
            this.itemId      = id;
            this.gatheringId = gatheringId;
            this.nameList    = new();
            this.levelStars  = (level << 3) + stars;
            this.NodeList    = new();
        }

        public int    Level() { return levelStars >> 3; }
        public int    Stars() { return levelStars & 0b111; }
        public string StarsString() { return starsArray[Stars()]; }

        override public string ToString()
        {
            return $"{nameList[Dalamud.ClientLanguage.English]} ({Level()}{StarsString()})";
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Gatherable rhs = obj as Gatherable;
            return itemId - rhs.itemId;
        }

        public int    itemId;
        public int    gatheringId;
        public FFName nameList;
        public HashSet<Node> NodeList { get; private set; }
        private readonly int levelStars;

        private static readonly string[] starsArray = new string[]
        { ""
        , "*"
        , "**"
        , "***"
        , "****"
        };
    }
}