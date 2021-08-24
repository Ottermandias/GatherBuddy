using System.Collections.Generic;
using System.Linq;
using Dalamud.Logging;
using Dalamud.Plugin;
using GatherBuddy.Game;
using GatherBuddy.Utility;

namespace GatherBuddy.Managers
{
    public class AetheryteManager
    {
        public HashSet<Aetheryte> Aetherytes { get; } = new();

        private static double GetMapScale(uint rowId)
        {
            var row = GatherBuddy.GameData.GetExcelSheet<Lumina.Excel.GeneratedSheets.Map>()!.GetRow(rowId);
            return row?.SizeFactor / 100.0 ?? 1.0;
        }

        public AetheryteManager(TerritoryManager territories)
        {
            var aetheryteExcel = GatherBuddy.GameData.GetExcelSheet<Lumina.Excel.GeneratedSheets.Aetheryte>()!;
            var mapMarkerList  = GatherBuddy.GameData.GetExcelSheet<Lumina.Excel.GeneratedSheets.MapMarker>()!.Where(m => m.DataType == 3).ToList();

            foreach (var a in aetheryteExcel.Where(a => a.IsAetheryte && a.RowId > 0))
            {
                var nameList = FFName.FromPlaceName(a.PlaceName.Row);
                if (nameList.AnyEmpty())
                    continue;

                var mapMarker = mapMarkerList.FirstOrDefault(m => m.DataKey == a.RowId);
                if (mapMarker == null)
                    continue;

                var scale     = GetMapScale(a.Map.Row);
                var territory = territories.FindOrAddTerritory(a.Territory.Value!);
                if (territory == null)
                    continue;

                var x         = Util.MapMarkerToMap(mapMarker.X, scale);
                var y         = Util.MapMarkerToMap(mapMarker.Y, scale);
                var aetheryte = new Aetheryte(a, territory, nameList, x, y);

                territory.Aetherytes.Add(aetheryte);
                Aetherytes.Add(aetheryte);
            }

            PluginLog.Verbose("{Count} aetherytes collected.", Aetherytes.Count);
        }
    }
}
