using System.Linq;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.Managers;
using GatherBuddy.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatherBuddy.Classes
{
    public enum AlarmType : byte
    {
        Node,
        Fish,
    }

    public class Alarm
    {
        [JsonIgnore]
        private object? _data = null;

        [JsonIgnore]
        public Node? Node
            => _data as Node;

        [JsonIgnore]
        public Fish? Fish
            => _data as Fish;

        public string Name         { get; set; } = string.Empty;
        public uint   Id           { get; set; }
        public int    MinuteOffset { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AlarmType Type { get; set; } = AlarmType.Node;

        public Sounds SoundId      { get; set; }
        public bool   Enabled      { get; set; }
        public bool   PrintMessage { get; set; }

        // backward compatibility.
        public uint NodeId
        {
            set => Id = value;
        }

        public Alarm()
        { }

        public Alarm(AlarmType type, string name, uint id, bool enabled = true, int offset = 0, Sounds sound = Sounds.None,
            bool printMessage = true)
        {
            Name         = name;
            NodeId       = id;
            MinuteOffset = offset;
            SoundId      = sound;
            PrintMessage = printMessage;
            Enabled      = enabled;
            Type         = type;
        }

        public Alarm(string name, Node? node, bool enabled = true, int offset = 0, Sounds sound = Sounds.None, bool printMessage = true)
            : this(AlarmType.Node, name, node?.Meta?.PointBaseId ?? 0, enabled, offset, sound, printMessage)
            => _data = node;

        public Alarm(string name, NodeManager nodes, uint nodeId, bool enabled = true, int offset = 0, Sounds sound = Sounds.None,
            bool printMessage = true)
            : this(AlarmType.Node, name, nodeId, enabled, offset, sound, printMessage)
        {
            FetchNode(nodes);
        }

        public void FetchNode(NodeManager nodes)
        {
            _data = nodes.NodeIdToNode.Values.FirstOrDefault(n => n.Meta!.PointBaseId == Id);
        }

        public Alarm(string name, Fish? fish, bool enabled = true, int offset = 0, Sounds sound = Sounds.None, bool printMessage = true)
            : this(AlarmType.Fish, name, fish?.ItemId ?? 0, enabled, offset, sound, printMessage)
            => _data = fish;

        public Alarm(string name, FishManager fishes, uint fishId, bool enabled = true, int offset = 0, Sounds sound = Sounds.None,
            bool printMessage = true)
            : this(AlarmType.Fish, name, fishId, enabled, offset, sound, printMessage)
        {
            FetchFish(fishes);
        }

        public void FetchFish(FishManager fish)
        {
            _data = fish.Fish.TryGetValue(Id, out var tmp) ? tmp : null;
        }
    }
}
