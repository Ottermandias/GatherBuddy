using System.Collections.Generic;
using Dalamud.Plugin;
using System.Linq;
using GatherBuddy.Classes;
using GatherBuddy.Utility;

namespace GatherBuddy.Managers
{
    public class AetheryteManager
    {
        public HashSet<Aetheryte> Aetherytes { get; } = new();

        private static double GetMapScale(DalamudPluginInterface pi, uint rowId)
        {
            var row = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Map>().GetRow(rowId);
            return row?.SizeFactor / 100.0 ?? 1.0;
        }

        public AetheryteManager(DalamudPluginInterface pi, TerritoryManager territories)
        {
            var aetheryteExcel = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Aetheryte>();
            var mapMarkerList  = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.MapMarker>().Where(m => m.DataType == 3).ToList();

            foreach (var a in aetheryteExcel.Where(a => a.IsAetheryte && a.RowId > 0))
            {
                var nameList = FFName.FromPlaceName(pi, a.PlaceName.Row);
                if (nameList.AnyEmpty())
                    continue;

                var mapMarker = mapMarkerList.FirstOrDefault(m => m.DataKey == a.RowId);
                if (mapMarker == null)
                    continue;

                var scale     = GetMapScale(pi, a.Map.Row);
                var territory = territories.FindOrAddTerritory(pi, a.Territory.Value);
                if (territory == null)
                    continue;

                var aetheryte = new Aetheryte((int) a.RowId, Util.MapMarkerToMap(mapMarker.X, scale), Util.MapMarkerToMap(mapMarker.Y, scale))
                {
                    Territory = territory,
                    NameList  = nameList,
                    XStream   = a.AetherstreamX,
                    YStream   = a.AetherstreamY,
                };


                territory.Aetherytes.Add(aetheryte);
                Aetherytes.Add(aetheryte);
            }

            PluginLog.Verbose("{Count} aetherytes collected.", Aetherytes.Count);
        }
    }
}
