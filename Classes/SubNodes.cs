using System.Collections.Generic;
using System.Linq;

namespace GatherBuddy.Classes
{
    public class SubNodes
    {
        public Territory?                      Territory        { get; set; } = null;
        public Aetheryte?                      ClosestAetheryte { get; set; } = null;
        public Dictionary<uint, NodeLocation?> Nodes            { get; set; } = new();

        public int averageX = 0;
        public int averageY = 0;

        public double XCoord
            => averageX / 100.0;

        public double YCoord
            => averageY / 100.0;

        public void RecomputeAverage()
        {
            if (Nodes.Count <= 0)
                return;

            var num = 0;
            averageX = 0;
            averageY = 0;
            foreach (var p in Nodes.Values.Where(n => n != null))
            {
                ++num;
                averageX += p!.AverageX;
                averageY += p!.AverageY;
            }

            averageX /= num;
            averageY /= num;

            if (Territory == null || Territory.Aetherytes.Count == 0)
                ClosestAetheryte = null;
            else
                ClosestAetheryte = Territory.Aetherytes.Select(a => (a.WorldDistance(Territory.Id, averageX, averageY), a)).Min().a;
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
