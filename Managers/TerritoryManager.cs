using System.Collections.Generic;
using Dalamud.Plugin;
using Otter;
using Serilog;

namespace Gathering
{
    public class TerritoryManager
    {
        public Dictionary<uint, FFName>    regions     = new();
        public Dictionary<uint, Territory> territories = new();

        // Add region name if it does not exist.
        private FFName FindOrAddRegionName(DalamudPluginInterface pi, uint regionNameRowId)
        {
            if (!regions.TryGetValue(regionNameRowId, out FFName names))
            {
                names = FFName.FromPlaceName(pi, regionNameRowId);
                if (names.AnyEmpty())
                    return null;
                regions[regionNameRowId] = names;
            }
            return names;
        }

        public Territory FindOrAddTerritory(DalamudPluginInterface pi, Lumina.Excel.GeneratedSheets.TerritoryType T)
        {
            // Create territory if it does not exist. Otherwise add the aetheryte to its list.
            if (!territories.TryGetValue(T.RowId, out Territory territory))
            {
                var names = FFName.FromPlaceName(pi, T.PlaceName.Row);
                if (names.AnyEmpty())
                    return null;

                territory = new Territory(T.RowId, FindOrAddRegionName(pi, T.PlaceNameRegion.Row), names)
                {
                    xStream  = T.Aetheryte.Value?.AetherstreamX ?? 0,
                    yStream  = T.Aetheryte.Value?.AetherstreamY ?? 0,
                };
                
                territories.Add(T.RowId, territory);
            }
            return territory;
        }
    }
}