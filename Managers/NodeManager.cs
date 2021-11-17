using System.Collections.Generic;
using System.Linq;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Data;
using GatherBuddy.Enums;
using GatherBuddy.Nodes;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;
using GatheringType = GatherBuddy.Enums.GatheringType;

namespace GatherBuddy.Managers
{
    public class NodeManager
    {
        public NodeRecorder           Records      { get; }
        public Dictionary<uint, Node> NodeIdToNode { get; }

        private static (NodeUptime, NodeType) GetTimes(uint nodeId)
        {
            var timeSheet = Dalamud.GameData.GetExcelSheet<GatheringPointTransient>()!;
            var hours     = timeSheet.GetRow(nodeId)!;

            // Check for ephemeral nodes
            if (hours.GatheringRarePopTimeTable.Row == 0)
            {
                var time = new NodeUptime(hours.EphemeralStartTime, hours.EphemeralEndTime);
                return time.AlwaysUp() ? (time, NodeType.Regular) : (time, NodeType.Ephemeral);
            }
            // and for unspoiled
            else
            {
                var time = new NodeUptime(hours.GatheringRarePopTimeTable.Value!);
                return time.AlwaysUp() ? (time, NodeType.Regular) : (time, NodeType.Unspoiled);
            }
        }

        private static void ApplyHiddenItemsAndCoordinates(ItemManager gatherables, AetheryteManager aetherytes,
            Dictionary<uint, Node> baseIdToNode)
        {
            var hidden = new NodeHidden(gatherables);
            foreach (var (_, node) in baseIdToNode)
            {
                NodeCoords.SetCoords(node, aetherytes);
                hidden.SetHiddenItems(node);
            }
        }

        public IEnumerable<Node> BaseNodes()
            => NodeIdToNode.Values.Distinct();

        public NodeManager(World territories, AetheryteManager aetherytes, ItemManager gatherables)
        {
            var baseSheet = Dalamud.GameData.GetExcelSheet<GatheringPointBase>()!;
            var nodeSheet = Dalamud.GameData.GetExcelSheet<GatheringPoint>()!;

            Dictionary<uint, Node> baseIdToNode = new((int) baseSheet.RowCount);
            NodeIdToNode = new Dictionary<uint, Node>((int) nodeSheet.RowCount);

            foreach (var nodeRow in nodeSheet)
            {
                var baseId = nodeRow.GatheringPointBase.Row;
                if (baseId >= baseSheet.RowCount)
                    continue;

                if (baseIdToNode.TryGetValue(baseId, out var node))
                {
                    NodeIdToNode[nodeRow.RowId] = node;
                    if ((node.Nodes!.Territory?.Id ?? 0) != nodeRow.TerritoryType.Row)
                        PluginLog.Error($"Different gathering nodes to the same base {baseId} have different territories.");

                    if (!node.Nodes.Nodes.ContainsKey(nodeRow.RowId))
                        node.Nodes.Nodes[nodeRow.RowId] = null;
                    continue;
                }

                if (nodeRow.TerritoryType.Row < 2)
                    continue;

                node = new Node
                {
                    PlaceNameEn = FFName.FromPlaceName(nodeRow.PlaceName.Row),
                    Nodes = new SubNodes()
                    {
                        Territory = territories.FindOrAddTerritory(nodeRow.TerritoryType.Value!),
                    },
                };
                node.Nodes.Nodes[nodeRow.RowId] = null;
                if (node.Nodes.Territory == null)
                    continue;

                var (times, type) = GetTimes(nodeRow.RowId);
                node.Times        = times;

                var baseRow = baseSheet.GetRow(baseId)!;
                node.Meta = new NodeMeta(baseRow, type);

                if (node.Meta.GatheringType >= GatheringType.Spearfishing)
                    continue;

                node.Items = new NodeItems(node, baseRow.Item, gatherables);
                if (node.Items.NoItems())
                {
                    PluginLog.Debug("Gathering node {RowId} has no items, skipped.", nodeRow.RowId);
                    continue;
                }

                baseIdToNode[baseId]        = node;
                NodeIdToNode[nodeRow.RowId] = node;
            }

            Records = new NodeRecorder(this, GatherBuddy.Config.Records);

            PluginLog.Verbose("{Count} unique gathering nodes collected.", NodeIdToNode.Count);
            PluginLog.Verbose("{Count} base gathering nodes collected.",   baseIdToNode.Count);

            ApplyHiddenItemsAndCoordinates(gatherables, aetherytes, baseIdToNode);
        }
    }
}
