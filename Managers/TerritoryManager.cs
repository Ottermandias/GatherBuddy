using System.Collections.Generic;
using Dalamud.Plugin;
using GatherBuddy.Classes;
using GatherBuddy.Utility;

namespace GatherBuddy.Managers
{
    public class TerritoryManager
    {
        public Dictionary<uint, FFName>    Regions     { get; } = new();
        public Dictionary<uint, Territory> Territories { get; } = new();

        // Add region name if it does not exist.
        private FFName? FindOrAddRegionName(DalamudPluginInterface pi, uint regionNameRowId)
        {
            if (Regions.TryGetValue(regionNameRowId, out var names))
                return names;

            names = FFName.FromPlaceName(pi, regionNameRowId);
            if (names.AnyEmpty())
                return null;

            Regions[regionNameRowId] = names;
            return names;
        }

        public Territory? FindOrAddTerritory(DalamudPluginInterface pi, Lumina.Excel.GeneratedSheets.TerritoryType T)
        {
            // Create territory if it does not exist. Otherwise add the aetheryte to its list.
            if (Territories.TryGetValue(T.RowId, out var territory))
                return territory;

            var names = FFName.FromPlaceName(pi, T.PlaceName.Row);
            if (names.AnyEmpty())
                return null;

            territory = new Territory(T.RowId, FindOrAddRegionName(pi, T.PlaceNameRegion.Row)!, names)
            {
                XStream = T.Aetheryte.Value?.AetherstreamX ?? 0,
                YStream = T.Aetheryte.Value?.AetherstreamY ?? 0,
            };

            Territories.Add(T.RowId, territory);
            return territory;
        }
    }
}
