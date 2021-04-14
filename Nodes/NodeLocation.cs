using System;
using System.Collections.Generic;
using System.IO;

namespace GatherBuddy.Nodes
{
    public class NodeLocation : IComparable
    {
        public HashSet<(int, int)> Locations { get; set; } = new();

        public int AverageX { get; set; }
        public int AverageY { get; set; }

        public double XCoord
            => AverageX / 100.0;

        public double YCoord
            => AverageY / 100.0;

        public int CompareTo(object r)
        {
            if (!(r is NodeLocation rhs))
                return 1;

            if (AverageX != rhs.AverageX)
                return AverageX - rhs.AverageX;

            return AverageY - rhs.AverageY;
        }

        public void Clear()
        {
            AverageX = 0;
            AverageY = 0;
            Locations.Clear();
        }

        public bool AddLocation(int x, int y)
        {
            if (!Locations.Add((x, y)))
                return false;

            AverageX = (AverageX * (Locations.Count - 1) + x) / Locations.Count;
            AverageY = (AverageY * (Locations.Count - 1) + y) / Locations.Count;
            return true;
        }

        public bool AddLocation(NodeLocation rhs)
        {
            var previousCount = Locations.Count;
            Locations.UnionWith(rhs.Locations);
            if (Locations.Count == previousCount)
                return false;

            var sumX = 0;
            var sumY = 0;
            foreach (var (x, y) in Locations)
            {
                sumX += x;
                sumY += y;
            }

            AverageX = sumX / Locations.Count;
            AverageY = sumY / Locations.Count;
            return true;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Locations.Count);
            foreach (var (x, y) in Locations)
            {
                writer.Write(x);
                writer.Write(y);
            }
        }

        public static NodeLocation Read(BinaryReader reader)
        {
            var num  = reader.ReadInt32();
            var set  = new HashSet<(int, int)>(num);
            var sumX = 0;
            var sumY = 0;
            for (var i = 0; i < num; ++i)
            {
                var x = reader.ReadInt32();
                var y = reader.ReadInt32();
                sumX += x;
                sumY += y;
                set.Add((x, y));
            }

            return new NodeLocation
            {
                AverageX  = sumX / num,
                AverageY  = sumY / num,
                Locations = set,
            };
        }
    }
}
