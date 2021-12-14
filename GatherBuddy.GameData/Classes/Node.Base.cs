using System;
using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Time;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;
using GatheringType = GatherBuddy.Enums.GatheringType;

namespace GatherBuddy.Classes;

public partial class GatheringNode : IComparable<GatheringNode>, ILocation
{
    public NodeType           NodeType     { get; init; }
    public GatheringPointBase BaseNodeData { get; init; }
    public string             Name         { get; init; }
    public BitfieldUptime     Times        { get; init; }

    public uint Id
        => BaseNodeData.RowId;

    public IEnumerable<IGatherable> Gatherables
        => Items;

    public ObjectType Type
        => ObjectType.Gatherable;

    public int Level
        => BaseNodeData.GatheringLevel;

    public GatheringType GatheringType
        => (GatheringType)BaseNodeData.GatheringType.Row;

    public bool IsMiner
        => GatheringType.ToGroup() == GatheringType.Miner;

    public bool IsBotanist
        => GatheringType.ToGroup() == GatheringType.Botanist;

    public string Folklore { get; init; }

    public GatheringNode(GameData data, GatheringPointBase node)
    {
        BaseNodeData = node;

        // Obtain the territory from the first node that has this as a base.
        var nodes   = data.DataManager.GetExcelSheet<GatheringPoint>();
        var nodeRow = nodes?.FirstOrDefault(n => n.GatheringPointBase.Row == node.RowId && n.PlaceName.Row > 0);
        Territory = data.FindOrAddTerritory(nodeRow?.TerritoryType.Value) ?? Territory.Invalid;
        Name      = MultiString.ParseSeStringLumina(nodeRow?.PlaceName.Value?.Name);
        // Obtain the center of the coordinates. We do not care for the radius.
        var coords   = data.DataManager.GetExcelSheet<ExportedGatheringPoint>();
        var coordRow = coords?.GetRow(node.RowId);
        IntegralXCoord = coordRow != null ? Maps.NodeToMap(coordRow.X, Territory.SizeFactor) : 100;
        IntegralYCoord = coordRow != null ? Maps.NodeToMap(coordRow.Y, Territory.SizeFactor) : 100;

        ClosestAetheryte = Territory.Aetherytes.Count > 0
            ? Territory.Aetherytes.ArgMin(a => a.WorldDistance(Territory.Id, IntegralXCoord, IntegralYCoord))
            : null;

        DefaultXCoord    = IntegralXCoord;
        DefaultYCoord    = IntegralYCoord;
        DefaultAetheryte = ClosestAetheryte;

        // Obtain additional information.
        Folklore = MultiString.ParseSeStringLumina(nodeRow?.GatheringSubCategory.Value?.FolkloreBook);
        var extendedRow = nodeRow == null ? null : data.DataManager.GetExcelSheet<GatheringPointTransient>()?.GetRow(nodeRow.RowId);
        (Times, NodeType) = GetTimes(extendedRow);
        if (Folklore.Any() && NodeType == NodeType.Unspoiled && nodeRow!.GatheringSubCategory.Value!.Item.Row != 0)
            NodeType = NodeType.Legendary;

        // Obtain the items and add the node to their individual lists.
        Items = node.Item
            .Select(i => data.GatherablesByGatherId.TryGetValue((uint)i, out var gatherable) ? gatherable : null)
            .Where(g => g != null)
            .Cast<Gatherable>()
            .ToList();
        if (Territory.Id <= 0)
            return;

        foreach (var item in Items)
            AddNodeToItem(item);
    }

    public int CompareTo(GatheringNode? obj)
        => Id.CompareTo(obj?.Id ?? 0);


    private static (BitfieldUptime, NodeType) GetTimes(GatheringPointTransient? row)
    {
        if (row == null)
            return (BitfieldUptime.AllHours, NodeType.Regular);

        // Check for ephemeral nodes
        if (row.GatheringRarePopTimeTable.Row == 0)
        {
            var time = new BitfieldUptime(row.EphemeralStartTime, row.EphemeralEndTime);
            return time.AlwaysUp() ? (time, NodeType.Regular) : (time, NodeType.Ephemeral);
        }
        // and for unspoiled
        else
        {
            var time = new BitfieldUptime(row.GatheringRarePopTimeTable.Value!);
            return time.AlwaysUp() ? (time, NodeType.Regular) : (time, NodeType.Unspoiled);
        }
    }
}
