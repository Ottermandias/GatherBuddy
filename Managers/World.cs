using System;
using System.Collections.Generic;
using Dalamud.Plugin;
using Dalamud;
using System.Linq;
using GatherBuddy.Classes;
using Lumina.Excel.GeneratedSheets;
using FishingSpot = GatherBuddy.Classes.FishingSpot;
using GatheringType = GatherBuddy.Classes.GatheringType;

namespace GatherBuddy.Managers
{
    public class World
    {
        private readonly DalamudPluginInterface _pi;
        public           ClientLanguage         Language    { get; }
        public           TerritoryManager       Territories { get; }
        public           AetheryteManager       Aetherytes  { get; }
        public           ItemManager            Items       { get; }
        public           FishManager            Fish        { get; }
        public           NodeManager            Nodes       { get; }
        private          int                    _currentXStream = 0;
        private          int                    _currentYStream = 0;

        private void AddAetherytes(Territory territory)
        {
            var aetheryteName = territory.NameList[ClientLanguage.English] switch
            {
                "The Dravanian Hinterlands" => "Idyllshire",
                "Limsa Lominsa Upper Decks" => "Limsa Lominsa Lower Decks",
                "Mist"                      => "Limsa Lominsa Lower Decks",
                "Old Gridania"              => "New Gridania",
                "The Lavender Beds"         => "New Gridania",
                "The Goblet"                => "Ul'dah - Steps of Nald",
                "Shirogane"                 => "Kugane",
                "The Endeavor"              => "Limsa Lominsa Lower Decks",
                "The Diadem"                => "Foundation",
                _                           => null,
            };
            if (aetheryteName == null)
                return;

            var aetheryte = Aetherytes.Aetherytes.FirstOrDefault(a => a.NameList[ClientLanguage.English] == aetheryteName);
            if (aetheryte != null)
                territory.Aetherytes.Add(aetheryte);
            else
                PluginLog.Error($"Tried to add {aetheryteName} to {territory.NameList[ClientLanguage.English]}, but aetheryte not found.");
        }

        public Territory? FindOrAddTerritory(TerritoryType territory)
        {
            var newTerritory = Territories.FindOrAddTerritory(_pi, territory);
            if (newTerritory != null)
                AddAetherytes(newTerritory);
            return newTerritory;
        }

        public void SetPlayerStreamCoords(ushort territory)
        {
            var rawT = _pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.TerritoryType>().GetRow(territory);
            var rawA = rawT?.Aetheryte?.Value;

            _currentXStream = rawA?.AetherstreamX ?? 0;
            _currentYStream = rawA?.AetherstreamY ?? 0;
        }

        public Node? ClosestNodeFromNodeList(IEnumerable<Node> nodes, GatheringType? type = null)
        {
            Node? minNode   = null;
            var   minDist   = double.MaxValue;
            var   worldDist = double.MaxValue;

            foreach (var node in nodes)
            {
                var closest = node.GetClosestAetheryte();
                if (closest == null || type != null && type!.Value.ToGroup() != node.Meta!.GatheringType.ToGroup())
                    continue;

                var dist    = closest.AetherDistance(_currentXStream, _currentYStream);
                if (dist > minDist)
                    continue;

                if (dist == minDist)
                {
                    var newWorldDist = closest.WorldDistance(node.Nodes!.Territory!.Id, (int) (node.GetX() * 100.0), (int) (node.GetY() * 100.0));
                    if (newWorldDist >= worldDist)
                        continue;

                    worldDist = newWorldDist;
                }
                else
                    worldDist = closest.WorldDistance(node.Nodes!.Territory!.Id, (int) (node.GetX() * 100.0), (int) (node.GetY() * 100.0));

                minDist = dist;
                minNode = node;
            }

            return minNode;
        }

        public FishingSpot? ClosestSpotFromSpotList(IEnumerable<FishingSpot> spots)
        {
            FishingSpot? minSpot   = null;
            var          minDist   = double.MaxValue;
            var          worldDist = double.MaxValue;

            foreach (var spot in spots)
            {
                var closest = spot.ClosestAetheryte;
                if (closest == null)
                    continue;

                var dist    = closest.AetherDistance(_currentXStream, _currentYStream);
                if (dist > minDist)
                    continue;

                if (dist == minDist)
                {
                    var newWorldDist = closest.WorldDistance(spot.Territory!.Id, spot.XCoord, spot.YCoord);
                    if (newWorldDist >= worldDist)
                        continue;

                    worldDist = newWorldDist;
                }
                else
                    worldDist = closest.WorldDistance(spot.Territory!.Id, spot.XCoord, spot.YCoord);

                minDist = dist;
                minSpot = spot;
            }

            return minSpot;
        }

        public World(DalamudPluginInterface pi, GatherBuddyConfiguration config)
        {
            _pi         = pi;
            Language    = pi.ClientState.ClientLanguage;
            Territories = new TerritoryManager();
            Aetherytes  = new AetheryteManager(pi, Territories);
            Items       = new ItemManager(pi);
            Nodes       = new NodeManager(pi, config, this, Aetherytes, Items);
            Fish        = new FishManager(pi, this, Aetherytes);

            PluginLog.Verbose("{Count} regions collected.",     Territories.Regions.Count);
            PluginLog.Verbose("{Count} territories collected.", Territories.Territories);
        }

        public Gatherable? FindItemByName(string itemName)
            => Items.FindItemByName(itemName, Language);

        public Fish? FindFishByName(string fishName)
            => Fish.FindFishByName(fishName, Language);

        public Node? ClosestNodeForItem(Gatherable? item, GatheringType? type = null)
            => item == null ? null : ClosestNodeFromNodeList(item.NodeList, type);

        public FishingSpot? ClosestSpotForItem(Fish? fish)
            => fish == null ? null : ClosestSpotFromSpotList(fish.FishingSpots);
    }
}
