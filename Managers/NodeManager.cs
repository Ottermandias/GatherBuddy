using System.Collections.Generic;
using Dalamud.Plugin;
using Lumina.Excel.GeneratedSheets;
using Otter;
using System.Linq;
using GatherBuddyPlugin;
using Serilog;

namespace Gathering
{
    public class NodeManager
    {
        public NodeRecorder           records;
        public Dictionary<uint, Node> nodeIdToNode;

        private (NodeTimes, NodeType) GetTimes(DalamudPluginInterface pi, uint nodeId)
        {
            var timeSheet = pi.Data.GetExcelSheet<GatheringPointTransient>();
            var hours = timeSheet.GetRow(nodeId);

            // Check for ephemeral nodes
            if (hours.GatheringRarePopTimeTable.Row == 0)
            {
                var time = new NodeTimes(hours.EphemeralStartTime, hours.EphemeralEndTime);
                if (time.AlwaysUp())
                    return (time, NodeType.Regular); 
                else
                    return (time, NodeType.Ephemeral);
            }
            // and for unspoiled
            else
            {
                var time = new NodeTimes(hours.GatheringRarePopTimeTable.Value);
                if (time.AlwaysUp())
                    return (time, NodeType.Regular); 
                else
                    return (time, NodeType.Unspoiled);
            }
        }

        private void ApplyHiddenItemsAndCoordinates(ItemManager gatherables, AetheryteManager aetherytes, Dictionary<uint, Node> baseIdToNode)
        {
            var hidden = new NodeHidden(gatherables);
            foreach (var node in baseIdToNode)
            {
                NodeCoords.SetCoords(node.Value, aetherytes);
                hidden.SetHiddenItems(node.Value);
            }
        }

        public IEnumerable<Node> BaseNodes()
        {
            return nodeIdToNode.Values.Distinct();
        }

        public NodeManager(DalamudPluginInterface pi, GatherBuddyConfiguration config, TerritoryManager territories, AetheryteManager aetherytes, ItemManager gatherables)
        {
            var baseSheet  = pi.Data.GetExcelSheet<GatheringPointBase>();
            var nodeSheet  = pi.Data.GetExcelSheet<GatheringPoint>();

            Dictionary<uint, Node> baseIdToNode = new((int) baseSheet.RowCount);
            nodeIdToNode = new((int) nodeSheet.RowCount);

            foreach (var nodeRow in nodeSheet)
            {
                var baseId = nodeRow.GatheringPointBase.Row;
                
                if (baseIdToNode.TryGetValue(baseId, out var node))
                {
                    nodeIdToNode[nodeRow.RowId] = node;
                    if ((node.nodes.territory?.id ?? 0) != nodeRow.TerritoryType.Row)
                        Log.Error($"[GatherBuddy] Different gathering nodes to the same base {baseId} have different territories.");

                    if (!node.nodes.nodes.ContainsKey(nodeRow.RowId))
                        node.nodes.nodes[nodeRow.RowId] = null;
                    continue;
                }
                if (nodeRow.TerritoryType.Row < 2)
                    continue;

                node = new()
                {
                    placeNameEN = FFName.FromPlaceName(pi, nodeRow.PlaceName.Row)[Dalamud.ClientLanguage.English]
                };
                node.nodes = new()
                {
                    territory = territories.FindOrAddTerritory(pi, nodeRow.TerritoryType.Value)
                };
                node.nodes.nodes[nodeRow.RowId] = null;
                if (node.nodes.territory == null)
                {
                    //Log.Error($"[GatherBuddy] Could not add territory {nodeRow.TerritoryType.Value.PlaceName.Row}.");
                    continue;
                }

                var (times, type) = GetTimes(pi, nodeRow.RowId);
                node.times = times;

                var baseRow = baseSheet.GetRow(baseId);
                node.meta = new NodeMeta(baseRow, type);
                
                if (node.meta.gatheringType >= GatheringType.Spearfishing)
                    continue;
                
                node.items = new NodeItems(node, baseRow.Item, gatherables);
                if (node.items.NoItems())
                {
                    Log.Debug($"[GatherBuddy] Gathering node {nodeRow.RowId} has no items, skipped.");
                    continue;
                }
                baseIdToNode[baseId] = node;
                nodeIdToNode[nodeRow.RowId] = node;
            }
            records = new NodeRecorder(pi, this, config.Records);

            Log.Verbose($"[GatherBuddy] {nodeIdToNode.Count} unique gathering nodes collected.");
            Log.Verbose($"[GatherBuddy] {baseIdToNode.Count} base gathering nodes collected.");

            ApplyHiddenItemsAndCoordinates(gatherables, aetherytes, baseIdToNode);
        }
    }
}