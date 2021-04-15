using System;
using GatherBuddy.Utility;

namespace GatherBuddy.Game
{
    public class FishingSpot : IComparable
    {
        public const uint SpearfishingIdOffset = 4096;

        public Territory? Territory        { get; set; }
        public FFName?    PlaceName        { get; set; }
        public Aetheryte? ClosestAetheryte { get; set; }
        public Fish?[]    Items            { get; set; } = new Fish[10];

        public uint Id { get; set; }

        public uint UniqueId
            => Spearfishing ? Id | SpearfishingIdOffset : Id;

        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public int Radius { get; set; }

        public bool Spearfishing { get; set; }

        internal static int ConvertCoord(double val, double scale)
            => (int) (100.0 * (41.0 / scale * (val * scale + 1024.0) / 2048.0 + 1.0));

        public int CompareTo(object? obj)
        {
            if (obj == null)
                return 1;

            var rhs = obj as FishingSpot;
            return Id.CompareTo(rhs?.Id ?? 0);
        }
    }
}
