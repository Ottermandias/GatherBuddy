using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Dalamud.Game.Internal;
using Dalamud.Plugin;
using GatherBuddy.Classes;

namespace GatherBuddy.Managers
{
    public class NodeRecorder : IDisposable
    {
        private readonly NodeManager                                _nodes;
        private readonly Records                                    _records;
        private readonly HashSet<uint>                              _skipList = new();
        private readonly Dalamud.Game.ClientState.ClientState       _clientState;
        private readonly Dalamud.Game.ClientState.Actors.ActorTable _actors;
        private readonly DalamudPluginInterface                     _pluginInterface;
        private          int                                        _frameCounter;

        public NodeRecorder(DalamudPluginInterface pi, NodeManager manager, Records records)
        {
            _nodes           = manager;
            _records         = records;
            _pluginInterface = pi;
            _clientState     = pi.ClientState;
            _actors          = pi.ClientState.Actors;
            ApplyRecords();
        }

        public void ActivateScanning()
            => _pluginInterface.Framework.OnUpdateEvent += OnUpdateEvent;

        public void DeactivateScanning()
            => _pluginInterface.Framework.OnUpdateEvent -= OnUpdateEvent;

        public void Dispose()
            => DeactivateScanning();

        private bool FindOrAddNodeLocation(Node n, uint mapId, uint nodeId, int x, int y)
        {
            if (mapId != (n.Nodes!.Territory?.Id ?? 0))
            {
                PluginLog.Error($"[NodeRecorder] Node found on map {mapId} when it should be on {n.Nodes!.Territory!.Id}.");
                return false;
            }

            if (_records.nodes.TryGetValue(nodeId, out var loc))
            {
                var ret = loc.AddLocation(x, y);
                n.Nodes.RecomputeAverage();
                return ret;
            }

            loc = new NodeLocation();
            loc.AddLocation(x, y);
            _records.nodes[nodeId] = loc;

            return n.Nodes.AddNodeLocation(nodeId, loc);
        }

        public int Scan()
        {
            var numNodes = 0;
            foreach (var a in _actors.Where(a
                => a.ObjectKind == Dalamud.Game.ClientState.Actors.ObjectKind.GatheringPoint && a.Address != IntPtr.Zero))
            {
                var id = (uint) Marshal.ReadInt32(a.Address + 0x80);
                if (_skipList.Contains(id))
                    continue;

                var mapId = _clientState.TerritoryType;
                if (!_nodes.NodeIdToNode.TryGetValue(id, out var node))
                {
                    // Apparently there are many Gathering Node actors left in the world that are not used anymore.
                    _skipList.Add(id);
                    PluginLog.Verbose("[NodeRecorder] Node with id {Id} does not exist.", id);
                    continue;
                }

                var scale = GetMapScale(_pluginInterface, mapId);
                var x     = ConvertCoord(a.Position.X, scale);
                var y     = ConvertCoord(a.Position.Y, scale);
                if (!FindOrAddNodeLocation(node, mapId, id, x, y))
                    continue;

                PluginLog.Verbose("[NodeRecorder] {Name} on map {MapId} - {RawX}|{RawY} -> {FloatX:F2}|{FloatY:F2}", a.Name, mapId,
                    a.Position.X, a.Position.Y, x / 100.0, y / 100.0);
                ++numNodes;
            }

            return numNodes;
        }

        public void PrintToLog()
        {
            foreach (var n in _records.nodes.Where(n => n.Value != null && n.Value.Locations.Count > 0))
            {
                var locations = string.Join(", ", n.Value.Locations.Select(loc => $"({loc.Item1 / 100.0}, {loc.Item2 / 100.0})"));
                PluginLog.Information($"[NodeRecorder] [Dump] {n.Key}: Average ({n.Value.XCoord}, {n.Value.YCoord}) | {locations}");
            }
        }

        public void PurgeRecord(uint nodeId)
        {
            _skipList.Remove(nodeId);
            if (!_records.nodes.TryGetValue(nodeId, out var loc) || loc.Locations.Count <= 0)
                return;

            PluginLog.Information($"[NodeRecorder] Purged all locations for node {nodeId}.");
            loc.Clear();
        }

        public void PurgeRecords()
        {
            _skipList.Clear();
            foreach (var p in _records.nodes)
                p.Value.Clear();
        }

        public void MergeRecords(string compressedRecords)
        {
            Records newRec = new();
            newRec.DeserializeCompressed(Convert.FromBase64String(compressedRecords));
            _records.Merge(newRec);
        }

        private void OnUpdateEvent(Framework f)
        {
            if (_frameCounter++ <= 600)
                return;

            _frameCounter = 0;
            Scan();
        }

        private static double GetMapScale(DalamudPluginInterface pi, uint rowId)
        {
            var map = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.TerritoryType>().GetRow(rowId).Map.Row;
            var row = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Map>().GetRow(map);
            return row?.SizeFactor / 100.0 ?? 1.0;
        }

        private static int ConvertCoord(double val, double scale)
            => (int) (100.0 * (41.0 / scale * (val * scale + 1024.0) / 2048.0 + 1.0));

        private void ApplyRecords()
        {
            foreach (var n in _records.nodes.Where(n => n.Value != null))
                _nodes.NodeIdToNode[n.Key].Nodes!.AddNodeLocation(n.Key, n.Value);
        }
    }
}
