using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Plugin.Services;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Classes;

public class OceanTimeline
{
    public int Count
        => Aldenard.Count;

    public IReadOnlyList<OceanRoute> Aldenard;
    public IReadOnlyList<OceanRoute> Othard;

    public IReadOnlyList<OceanRoute> this[OceanArea area]
        => area switch
        {
            OceanArea.Aldenard => Aldenard,
            OceanArea.Othard => Othard,
            _ => Array.Empty<OceanRoute>(),
        };

    public OceanRoute this[OceanArea area, int idx]
        => this[area][idx];

    public OceanTimeline(IDataManager gameData, IReadOnlyList<OceanRoute> routes)
    {
        var timelineSheet = gameData.GetExcelSheet<IKDRouteTable>()!;
        Aldenard = timelineSheet.Select(r => routes[(int)r.Route.Row - 1]).ToArray();
        Othard = timelineSheet.Skip(120).Concat(timelineSheet.Take(120)).Select(r => routes[(int)r.Unknown1 - 1]).ToArray();
    }
}