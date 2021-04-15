using System.Linq;
using GatherBuddy.Enums;
using GatherBuddy.Managers;
using GatherBuddy.Nodes;
using Newtonsoft.Json;

namespace GatherBuddy.Classes
{
    public class Alarm
    {
        [JsonIgnore]
        public Node? Node { get; private set; }

        public string Name         { get; set; } = string.Empty;
        public uint   NodeId       { get; set; }
        public int    MinuteOffset { get; set; }
        public Sounds SoundId      { get; set; }
        public bool   Enabled      { get; set; }
        public bool   PrintMessage { get; set; }

        public Alarm()
        { }

        public Alarm(string name, uint nodeId, bool enabled = true, int offset = 0, Sounds sound = Sounds.None, bool printMessage = true)
        {
            Name         = name;
            Node         = null;
            NodeId       = nodeId;
            MinuteOffset = offset;
            SoundId      = sound;
            PrintMessage = printMessage;
            Enabled      = enabled;
        }

        public Alarm(string name, Node? node, bool enabled = true, int offset = 0, Sounds sound = Sounds.None, bool printMessage = true)
            : this(name, node?.Meta?.PointBaseId ?? 0, enabled, offset, sound, printMessage)
            => Node = node;

        public Alarm(string name, NodeManager nodes, uint nodeId, bool enabled = true, int offset = 0, Sounds sound = Sounds.None,
            bool printMessage = true)
            : this(name, nodeId, enabled, offset, sound, printMessage)
        {
            FetchNode(nodes);
        }

        public void FetchNode(NodeManager nodes)
        {
            var id = NodeId;
            Node = nodes.NodeIdToNode.Values.FirstOrDefault(n => n.Meta!.PointBaseId == id);
        }
    }
}
