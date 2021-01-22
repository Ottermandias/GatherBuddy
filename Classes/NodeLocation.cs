using System;
using System.Collections.Generic;
using System.IO;
using Serilog;

namespace Gathering
{
    public class NodeLocation : IComparable
    {
        public HashSet<(int, int)> locations = new HashSet<(int, int)>();

        public int averageX = 0;
        public int averageY = 0;

        public int CompareTo(object r)
        {
            if (r == null) return 1;
            var rhs = r as NodeLocation;

            if (averageX != rhs.averageX)
                return averageX - rhs.averageX;

            return averageY - rhs.averageY;
        }

        public bool AddLocation(int x, int y)
        {
            if (locations.Add((x, y)))
            {
                averageX = (averageX * (locations.Count - 1) + x) / locations.Count;
                averageY = (averageY * (locations.Count - 1) + y) / locations.Count;
                return true;
            }
            return false;
        }

        public bool AddLocation(NodeLocation rhs)
        {
            var previousCount = locations.Count;
            locations.UnionWith(rhs.locations);
            if (locations.Count == previousCount)
                return false;

            var sumX = 0;
            var sumY = 0;
            foreach (var (x,y) in locations)
            {
                sumX += x;
                sumY += y;
            }
            averageX = sumX / locations.Count;
            averageY = sumY / locations.Count;
            return true;
        }

        public double ToX() { return averageX / 100.0; }
        public double ToY() { return averageY / 100.0; }

        public void Write(BinaryWriter writer)
        {
            writer.Write(locations.Count);
            foreach (var (x,y) in locations)
            {
                writer.Write(x);
                writer.Write(y);
            }
        }

        public static NodeLocation Read(BinaryReader reader)
        {
            var num   = reader.ReadInt32();
            var set   = new HashSet<(int, int)>(num);
            var sumX  = 0;
            var sumY  = 0;
            for (int i = 0; i < num; ++i)
            {
                var x = reader.ReadInt32();
                var y = reader.ReadInt32();
                sumX += x;
                sumY += y;
                set.Add((x,y));
            }
            return new NodeLocation
            { averageX  = sumX / num
            , averageY  = sumY / num
            , locations = set
            };
        }
    }
}