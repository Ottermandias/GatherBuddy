using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Game;

namespace GatherBuddy.Nodes
{
    public class SubNodes
    {
        public Territory?                      Territory        { get; set; } = null;
        public Aetheryte?                      ClosestAetheryte { get; set; }
        public Dictionary<uint, NodeLocation?> Nodes            { get; set; } = new();

        public int AverageX;
        public int AverageY;

        public double XCoord
            => AverageX / 100.0;

        public double YCoord
            => AverageY / 100.0;

        public void RecomputeAverage()
        {
            if (Nodes.Count <= 0)
                return;

            var num = 0;
            AverageX = 0;
            AverageY = 0;
            foreach (var p in Nodes.Values.Where(n => n != null))
            {
                ++num;
                AverageX += p!.AverageX;
                AverageY += p!.AverageY;
            }

            AverageX /= num;
            AverageY /= num;

            if (Territory == null || Territory.Aetherytes.Count == 0)
                ClosestAetheryte = null;
            else
                ClosestAetheryte = Territory.Aetherytes.Select(a => (a.WorldDistance(Territory.Id, AverageX, AverageY), a)).Min().a;
        }

        public bool AddNodeLocation(uint nodeId, NodeLocation loc)
        {
            if (Nodes.TryGetValue(nodeId, out var oldLoc) && oldLoc != null)
            {
                if (!oldLoc.AddLocation(loc))
                    return false;

                RecomputeAverage();
                return true;
            }

            Nodes[nodeId] = loc;
            RecomputeAverage();
            return true;
        }
    }
}
