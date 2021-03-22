using System;
using System.Collections.Generic;
using GatherBuddy.Utility;

namespace GatherBuddy.Classes
{
    public class Fish : IComparable
    {
        public int                  Id           { get; set; }
        public HashSet<FishingSpot> FishingSpots { get; set; } = new();
        public FFName?              Name         { get; set; }

        public int CompareTo(object? obj)
        {
            if (obj == null)
                return 1;

            var rhs = obj as Fish;
            return Id.CompareTo(rhs?.Id ?? 0);
        }
    }
}
