using System;
using System.Collections.Generic;
using Dalamud.Plugin;
using Dalamud;
using System.Linq;
using GatherBuddy.Classes;

namespace GatherBuddy.Managers
{
    public class World
    {
        private readonly DalamudPluginInterface _pi;
        public           ClientLanguage         Language    { get; }
        public           TerritoryManager       Territories { get; }
        public           AetheryteManager       Aetherytes  { get; }
        public           ItemManager            Items       { get; }
        public           NodeManager            Nodes       { get; }
        private          int                    _currentXStream = 0;
        private          int                    _currentYStream = 0;

        public void SetPlayerStreamCoords(ushort territory)
        {
            var rawT = _pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.TerritoryType>().GetRow(territory);
            var rawA = rawT?.Aetheryte?.Value;

            _currentXStream = rawA?.AetherstreamX ?? 0;
            _currentYStream = rawA?.AetherstreamY ?? 0;
        }

        public Node? ClosestNodeFromNodeList(IEnumerable<Node> nodes, GatheringType? type = null)
        {
            Node? minNode = null;
            var   minDist = double.MaxValue;

            foreach (var node in nodes)
            {
                var closest = node.GetClosestAetheryte();
                var dist    = closest?.AetherDistance(_currentXStream, _currentYStream) ?? double.MaxValue;
                if (!(dist < minDist) || closest == null || type != null && type!.Value.ToGroup() != node.Meta!.GatheringType.ToGroup())
                    continue;

                minDist = dist;
                minNode = node;
            }

            return minNode;
        }

        private void AddIdyllshireToDravania()
        {
            var dravania = Territories.Territories.Values.First(t => t.NameList[ClientLanguage.English] == "The Dravanian Hinterlands");
            if (dravania == null)
                return;

            var idyllshire = Aetherytes.Aetherytes.First(a => a.NameList[ClientLanguage.English] == "Idyllshire");
            if (idyllshire == null)
                return;

            dravania.Aetherytes.Add(idyllshire);
        }

        public World(DalamudPluginInterface pi, GatherBuddyConfiguration config)
        {
            _pi         = pi;
            Language    = pi.ClientState.ClientLanguage;
            Territories = new TerritoryManager();
            Aetherytes  = new AetheryteManager(pi, Territories);
            Items       = new ItemManager(pi);
            Nodes       = new NodeManager(pi, config, Territories, Aetherytes, Items);

            AddIdyllshireToDravania();

            PluginLog.Verbose("{Count} regions collected.",     Territories.Regions.Count);
            PluginLog.Verbose("{Count} territories collected.", Territories.Territories);
        }

        public Gatherable? FindItemByName(string itemName)
            => Items.FindItemByName(itemName, Language);

        public Node? ClosestNodeForItem(Gatherable item, GatheringType? type = null)
            => item == null ? null : ClosestNodeFromNodeList(item.NodeList, type);
    }
}
