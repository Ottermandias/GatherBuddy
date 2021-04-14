using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using GatherBuddy.Nodes;

namespace GatherBuddy.Classes
{
    [Serializable]
    public class Records : ISerializable
    {
        public Dictionary<uint, NodeLocation> Nodes = new();

        public void Merge(Records rhs)
        {
            foreach (var p in rhs.Nodes.Where(p => p.Value != null))
                Nodes[p.Key].AddLocation(p.Value);
        }

        public byte[] SerializeCompressed()
        {
            using var m      = new MemoryStream();
            using var c      = new DeflateStream(m, CompressionMode.Compress);
            using var writer = new BinaryWriter(c);

            writer.Write(Nodes.Count);
            foreach (var n in Nodes)
            {
                writer.Write(n.Key);
                n.Value.Write(writer);
            }

            c.Close();
            return m.ToArray();
        }

        public void DeserializeCompressed(byte[] data)
        {
            var       output = new MemoryStream();
            using var m      = new MemoryStream(data);
            using var c      = new DeflateStream(m, CompressionMode.Decompress);

            c.CopyTo(output);
            c.Close();
            output.Position = 0;

            using var reader = new BinaryReader(output);

            var count = reader.ReadInt32();
            for (var i = 0; i < count; ++i)
            {
                var id = reader.ReadUInt32();
                Nodes.Add(id, NodeLocation.Read(reader));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
            => info.AddValue("nodes", SerializeCompressed(), typeof(byte[]));

        public Records()
        { }

        public Records(SerializationInfo info, StreamingContext context)
            => DeserializeCompressed(Convert.FromBase64String((string) info.GetValue("nodes", typeof(string))));
    }
}
