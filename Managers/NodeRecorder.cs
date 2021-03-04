using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Dalamud.Game.Internal;
using Serilog;

namespace Gathering
{

    [Serializable]
    public class Records : ISerializable
    {
        public Dictionary<uint, NodeLocation> nodes = new();

        public void Merge(Records rhs)
        {
            foreach (var P in rhs.nodes)
                if (P.Value != null)
                    nodes[P.Key].AddLocation(P.Value);
        }

        public byte[] SerializeCompressed()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (DeflateStream c = new DeflateStream(m, CompressionMode.Compress))
                {
                    using (BinaryWriter writer = new BinaryWriter(c))
                    {
                        writer.Write(nodes.Count);
                        foreach (var N in nodes)
                        {
                            writer.Write(N.Key);
                            N.Value.Write(writer);
                        }
                    }
                    c.Close();
                }
                return m.ToArray();
            }
        }

        public void DeserializeCompressed(byte[] data)
        {
            int count = 0;
            var output = new MemoryStream();
            using (MemoryStream m = new MemoryStream(data))
            using (DeflateStream c = new DeflateStream(m, CompressionMode.Decompress))
            {
                c.CopyTo(output);
                c.Close();
                output.Position = 0;
            }
                
            using (BinaryReader reader = new BinaryReader(output))
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; ++i)
                {
                    var id = reader.ReadUInt32();
                    nodes.Add(id, NodeLocation.Read(reader));
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("nodes", SerializeCompressed(), typeof(byte[]));
        }
        public Records()
        { }

        public Records(SerializationInfo info, StreamingContext context)
        {
            DeserializeCompressed( System.Convert.FromBase64String((string)info.GetValue("nodes", typeof(string))) );
        }
    }

    public class NodeRecorder : IDisposable
    {
        public NodeRecorder(Dalamud.Plugin.DalamudPluginInterface pi, NodeManager manager, Records records)
        {
            this.nodes       = manager;
            this.records     = records;
            this.pi          = pi;
            this.clientState = pi.ClientState;
            this.actors      = pi.ClientState.Actors;
            ApplyRecords();
        }
        
        public void ActivateScanning()
        {
            pi.Framework.OnUpdateEvent += OnUpdateEvent;
        }
       
        public void DeactivateScanning()
        {
            pi.Framework.OnUpdateEvent -= OnUpdateEvent;
        }
        
        public void Dispose()
        {
            DeactivateScanning();
        }
        
        bool FindOrAddNodeLocation(Node N, uint mapId, uint nodeId, int x, int y)
        {
            if (mapId != (N.nodes.territory?.id ?? 0))
            {
                Log.Error($"[GatherBuddy] [NodeRecorder] Node found on map {mapId} when it should be on {N.nodes.territory.id}.");
                return false;
            }

            if (records.nodes.TryGetValue(nodeId, out var loc))
            {
                var ret = loc.AddLocation(x, y);
                N.nodes.RecomputeAverage();
                return ret;
            }

            loc = new();
            loc.AddLocation(x, y);
            records.nodes[nodeId] = loc;

            return N.nodes.AddNodeLocation(nodeId, loc);
        }

        public int Scan()
        {
            int numNodes = 0;
            foreach(var A in actors)
            {
                if (A.ObjectKind == Dalamud.Game.ClientState.Actors.ObjectKind.GatheringPoint)
                {
                    if (A.Address != IntPtr.Zero)
                    {
                        var id = (uint) Marshal.ReadInt32(A.Address + 0x80);
                        if (skipList.Contains(id))
                            continue;

                        var mapId = clientState.TerritoryType;
                        if (!nodes.nodeIdToNode.TryGetValue(id, out var node))
                        {
                            // Apparently there are many Gathering Node actors left in the world that are not used anymore.
                            skipList.Add(id);
                            Log.Verbose($"[GatherBuddy] [NodeRecorder] Node with id {id} does not exist.");
                            continue;
                        }

                        var scale = GetMapScale(pi, mapId);
                        var x = ConvertCoord(A.Position.X, scale);
                        var y = ConvertCoord(A.Position.Y, scale);
                        if (FindOrAddNodeLocation(node, mapId, id, x, y))
                        {
                            Log.Verbose($"[GatherBuddy] [NodeRecorder] {A.Name} on map {mapId} - {A.Position.X}|{A.Position.Y} -> {x / 100.0:F2}|{y / 100.0:F2}");
                            ++numNodes;
                        }
                    }
                }
            }
            return numNodes;
        }

        public void PrintToLog()
        {
            foreach (var N in records.nodes)
            {
                if (N.Value != null && N.Value.locations.Count > 0)
                {
                    string locations = "";
                    foreach (var loc in N.Value.locations)
                        locations += $"({loc.Item1 / 100.0}, {loc.Item2 / 100.0}), ";
                    
                    Log.Information($"[GatherBuddy] [NodeRecorder] [Dump] {N.Key}: Average ({N.Value.ToX()}, {N.Value.ToY()}) | {locations.TrimEnd(',', ' ')}");
                }
            }
        }

        public void PurgeRecord(uint nodeId)
        {
            skipList.Remove(nodeId);
            if (records.nodes.TryGetValue(nodeId, out var loc))
            {
                if (loc.locations.Count > 0)
                {
                    Log.Information($"[GatherBuddy] [NodeRecorder] Purged all locations for node {nodeId}.");
                    loc.Clear();
                }
            }
        }

        public void PurgeRecords()
        {
            skipList.Clear();
            foreach (var P in records.nodes)
                P.Value.Clear();
        }

        public void MergeRecords(string compressedRecords)
        {
            Records newRec = new();
            newRec.DeserializeCompressed( System.Convert.FromBase64String(compressedRecords) );
            records.Merge(newRec);
        }


        #region privates
        private void OnUpdateEvent(Framework f)
        {
            if (frameCounter++ > 600)
            {
                frameCounter = 0;
                Scan();
            }
        }
        
        private static double GetMapScale(Dalamud.Plugin.DalamudPluginInterface pi, uint rowId)
        {
            var map = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.TerritoryType>().GetRow(rowId).Map.Row;
            var row = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Map>().GetRow(map);
            return (row != null) ? row.SizeFactor / 100.0 : 1.0;
        }
        
        private static int ConvertCoord(double val, double scale)
        {
            return (int) (100.0 * ((41.0 / scale) * (val * scale + 1024.0) / 2048.0 + 1.0));
        }

        private void ApplyRecords()
        {
            foreach (var n in records.nodes)
                if (n.Value != null)
                    nodes.nodeIdToNode[n.Key].nodes.AddNodeLocation(n.Key, n.Value);
        }

        private readonly NodeManager      nodes;
        private readonly Records          records;
        private readonly HashSet<uint>    skipList = new();
        private readonly Dalamud.Game.ClientState.ClientState  clientState = null;
        private readonly Dalamud.Game.ClientState.Actors.ActorTable actors = null;
        private readonly Dalamud.Plugin.DalamudPluginInterface          pi = null;
        private int frameCounter = 0;
        #endregion
    }
}