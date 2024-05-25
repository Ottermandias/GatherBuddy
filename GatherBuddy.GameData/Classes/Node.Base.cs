using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    public Vector3[]          Markers      { get; set; } = Array.Empty<Vector3>();
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


    public GatheringNode(GameData data, IReadOnlyDictionary<uint, List<uint>> gatheringPoint,
        IReadOnlyDictionary<uint, List<uint>> gatheringItemPoint, GatheringPointBase node)
    {
        BaseNodeData = node;

        // Obtain the territory from the first node that has this as a base.
        var nodes    = data.DataManager.GetExcelSheet<GatheringPoint>()!;
        var nodeList = gatheringPoint.TryGetValue(node.RowId, out var nl) ? (IReadOnlyList<uint>)nl : Array.Empty<uint>();
        var nodeRow  = nodeList.Count > 0 ? nodes.GetRow(nodeList[0]) : null;
        Territory = data.FindOrAddTerritory(nodeRow?.TerritoryType.Value) ?? Territory.Invalid;
        Name      = MultiString.ParseSeStringLumina(nodeRow?.PlaceName.Value?.Name);
        // Obtain the center of the coordinates. We do not care for the radius.
        var coords   = data.DataManager.GetExcelSheet<ExportedGatheringPoint>();
        var coordRow = coords?.GetRow(node.RowId);
        IntegralXCoord = coordRow != null ? Maps.NodeToMap(coordRow.X, Territory.SizeFactor) : 100;
        IntegralYCoord = coordRow != null ? Maps.NodeToMap(coordRow.Y, Territory.SizeFactor) : 100;
        
        foreach (var nodeRow2 in nodeList)
        {
            var worldCoords = data.WorldCoords.TryGetValue(nodeRow2, out var wc) ? wc : new List<Vector3>();
            WorldPositions[nodeRow2] = worldCoords;
        }

        Radius = coordRow?.Radius ?? 10;

        ClosestAetheryte = Territory.Aetherytes.Count > 0
            ? Territory.Aetherytes.ArgMin(a => a.WorldDistance(Territory.Id, IntegralXCoord, IntegralYCoord))
            : null;

        DefaultXCoord    = IntegralXCoord;
        DefaultYCoord    = IntegralYCoord;
        DefaultAetheryte = ClosestAetheryte;
        DefaultRadius    = Radius;

        // Obtain additional information.
        Folklore = MultiString.ParseSeStringLumina(nodeRow?.GatheringSubCategory.Value?.FolkloreBook);
        var extendedRow = nodeRow == null ? null : data.DataManager.GetExcelSheet<GatheringPointTransient>()?.GetRow(nodeRow.RowId);
        (Times, NodeType) = GetTimes(extendedRow);
        if (Folklore.Length > 0 && NodeType == NodeType.Unspoiled && nodeRow!.GatheringSubCategory.Value!.Item.Row != 0)
            NodeType = NodeType.Legendary;

        // Obtain the items and add the node to their individual lists.
        Items = node.Item
            .Select(i => data.GatherablesByGatherId.TryGetValue((uint)i, out var gatherable) ? gatherable : null)
            .OfType<Gatherable>()
            .ToList();

        foreach (var n in nodeList)
        {
            if (!gatheringItemPoint.TryGetValue(n, out var gatherableList))
                break;

            foreach (var g in gatherableList)
            {
                if (data.GatherablesByGatherId.TryGetValue(g, out var gatherable)
                 && gatherable.GatheringData.IsHidden
                 && !Items.Contains(gatherable))
                    Items.Add(gatherable);
            }
        }

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
