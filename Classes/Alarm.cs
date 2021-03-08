using System.Linq;
using Dalamud.Game.Internal.Gui;
using Newtonsoft.Json;
using Otter.SEFunctions;

namespace Gathering
{
    public class Alarm
    {
        [JsonIgnore]
        public Node Node { get; private set; }

        public string Name { get; set; }
        public int NodeId { get; set; }
        public int MinuteOffset { get; set; }
        public Sounds SoundId { get; set; }
        public bool Enabled { get; set; }
        public bool PrintMessage { get; set; }

        public void SetSound(Sounds id) => SoundId = id;
        public void SetOffset(int offset) => MinuteOffset = offset;
        public void SetEnabled(bool enabled) => Enabled = enabled;
        public void SetPrintMessage(bool printMessage) => PrintMessage = printMessage;

        public Alarm()
        { }

        public Alarm(string name, int nodeId, bool enabled = true, int offset = 0, Sounds sound = Sounds.None, bool printMessage = true)
        {
            Name = name;
            Node = null;
            NodeId = nodeId;
            MinuteOffset = offset;
            SoundId = sound;
            PrintMessage = printMessage;
            Enabled = enabled;
        }

        public Alarm(string name, Node node, bool enabled = true, int offset = 0, Sounds sound = Sounds.None, bool printMessage = true)
            : this(name, node?.meta.pointBaseId ?? 0, enabled, offset, sound, printMessage)
        {
            Node = node;
        }

        public Alarm(string name, NodeManager nodes, int nodeId, bool enabled = true, int offset = 0, Sounds sound = Sounds.None, bool printMessage = true)
            : this(name, nodeId, enabled, offset, sound, printMessage)
        {
            FetchNode(nodes);
        }

        public void FetchNode(NodeManager nodes)
        {
            var id = NodeId;
            Node = nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == id);
        }
    }
}