using Serilog;
using System.Collections.Generic;
using Dalamud.Plugin;
using Otter;
using System.Linq;

namespace Gathering
{
    public class AetheryteManager
    {
        public readonly HashSet<Aetheryte> aetherytes = new();

        private static double GetMapScale(DalamudPluginInterface pi, uint rowId)
        {
            var row = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Map>().GetRow(rowId);
            return (row != null) ? row.SizeFactor / 100.0 : 1.0;
        }

        public AetheryteManager(DalamudPluginInterface pi, TerritoryManager territories)
        {
            var aetheryteExcel   = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Aetheryte>();
            var mapMarkerList    = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.MapMarker>().Where(m => m.DataType == 3).ToList();
            var territoriesExcel = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.TerritoryType>();

            foreach (var a in aetheryteExcel)
            {
                if (!a.IsAetheryte || a.RowId <= 0)
                    continue;

                var nameList = FFName.FromPlaceName(pi, a.PlaceName.Row);
                if (nameList.AnyEmpty())
                    continue;

                var mapMarker = mapMarkerList.FirstOrDefault(m => m.DataKey == a.RowId);
                if (mapMarker == null)
                    continue;

                var scale = GetMapScale(pi, a.Map.Row);
                var A = new Aetheryte((int)a.RowId, Util.MapMarkerToMap(mapMarker.X, scale), Util.MapMarkerToMap(mapMarker.Y, scale))
                {
                    nameList = nameList,
                    xStream  = a.AetherstreamX,
                    yStream  = a.AetherstreamY
                };

                var T = territories.FindOrAddTerritory(pi, a.Territory.Value);
                if (T == null)
                    continue;

                T.aetherytes.Add(A);
                A.territory = T;
                aetherytes.Add(A);
            }
            Log.Verbose($"[GatherBuddy] {aetherytes.Count} aetherytes collected.");
        }        

    }
}