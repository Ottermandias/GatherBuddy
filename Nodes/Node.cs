using System;
using System.Linq;
using Dalamud.Plugin;
using GatherBuddy.Classes;
using GatherBuddy.Game;

namespace GatherBuddy.Nodes
{
    public class Node : IComparable
    {
        public NodeMeta?            Meta        { get; set; }
        public SubNodes?            Nodes       { get; set; }
        public InitialNodePosition? InitialPos  { get; set; }
        public NodeItems?           Items       { get; set; }
        public Uptime               Times       { get; set; } = Uptime.AllHours;
        public string?              PlaceNameEn { get; set; }

        private static double CoordinateCap(double input)
            => input > 50.0 ? 0.0 : input;

        public double GetX()
        {
            if (InitialPos == null)
                return CoordinateCap(Nodes?.XCoord ?? 0);
            if (InitialPos.Prefer || Nodes == null || Nodes.AverageX == 0)
                return CoordinateCap(InitialPos.XCoord);

            return CoordinateCap(Nodes.XCoord);
        }

        public double GetY()
        {
            if (InitialPos == null)
                return CoordinateCap(Nodes?.YCoord ?? 0);
            if (InitialPos.Prefer || Nodes == null || Nodes.AverageY == 0)
                return CoordinateCap(InitialPos.YCoord);

            return CoordinateCap(Nodes.YCoord);
        }

        public Aetheryte? GetValidAetheryte()
        {
            if (InitialPos == null)
                return Nodes?.ClosestAetheryte;
            if (InitialPos.Prefer || Nodes?.ClosestAetheryte == null)
                return InitialPos.ClosestAetheryte;

            return Nodes?.ClosestAetheryte;
        }

        public Aetheryte? GetClosestAetheryte()
        {
            var ret = GetValidAetheryte();
            if (ret == null && (Nodes?.Territory?.Aetherytes.Count ?? 0) > 0)
                return Nodes!.Territory!.Aetherytes.First();

            return ret;
        }

        public int CompareTo(object obj)
            => Meta?.CompareTo(obj) ?? 1;

        public void AddItem(Gatherable item)
        {
            if (!Items?.SetFirstNullItem(this, item) ?? true)
                PluginLog.Error($"Could not add additional item {item} to node {Meta?.PointBaseId}, all 9 slots are used.");
        }
    }
}
