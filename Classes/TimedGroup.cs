using System.Collections.Generic;
using System.Linq;
using Dalamud;
using Dalamud.Game.Internal.Gui;
using Dalamud.Plugin;
using GatherBuddy.Managers;

namespace GatherBuddy.Classes
{
    public class TimedGroup
    {
        private static   ClientLanguage             _lang;
        public           string                     Name { get; }
        public           string                     Desc { get; }
        private readonly (Node node, string desc)[] _nodes;

        private static string? CorrectItemName(Node? node, string? itemName)
        {
            if (node == null || itemName == null)
                return null;

            var item = node.Items!.Items.FirstOrDefault(I => I?.NameList[ClientLanguage.English] == itemName);
            if (item != null)
                return item.NameList[_lang];

            PluginLog.Error("Node {NodeId} does not contain an item called {Name}.", node.Meta!.PointBaseId, itemName);
            return null;
        }

        public TimedGroup(string name, string desc, params (Node? node, string? desc)[] nodes)
        {
            Name   = name;
            Desc   = desc;
            _nodes = new (Node Node, string desc)[24];

            nodes = nodes.Where(n => n.node != null)
                .Select(n => (n.node, CorrectItemName(n.node!, n.desc!)))
                .ToArray();

            for (var i = 0; i < _nodes.Length; ++i)
                foreach (var n in nodes)
                    if (n.node!.Times!.IsUp(i))
                        _nodes[i] = n!;
            for (var i = _nodes.Length - 1; i >= 0; --i)
                if (_nodes[i].node == null)
                    _nodes[i] = _nodes[i + 1];

            // Repeat ones for circularity, this time check for correctness.
            for (var i = _nodes.Length - 1; i >= 0; --i)
            {
                if (_nodes[i].node == null)
                    _nodes[i] = _nodes[i + 1];
                if (_nodes[i].node == null)
                    PluginLog.Debug("TimedGroup {Name} has no node at hour {Hour}.", name, i);
            }
        }

        public (Node? node, string? desc) CurrentNode(int hour)
        {
            hour = hour switch
            {
                > 23 => 23,
                < 0  => 0,
                _    => hour,
            };
            return _nodes[hour];
        }

        private static void Add(IDictionary<string, TimedGroup> dict, string name, string desc, params (Node? node, string? desc)[] nodes)
            => dict.Add(name, new TimedGroup(name, desc, nodes));

        public static Dictionary<string, TimedGroup> CreateGroups(World W)
        {
            _lang = W.Language;
            var dict = new Dictionary<string, TimedGroup>();

            var nodeValues = W.Nodes.NodeIdToNode.Values;

            Add(dict, "80***", "Contains exarchic crafting nodes."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 758), null) // Hard Water
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 759), null) // Solstice Stone
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 760), null) // Dolomite
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 761), null) // Wattle Petribark
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 762), null) // Silver Beech Log
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 763), null) // Raindrop Cotton Boll               
            );

            Add(dict, "80**", "Contains neo-ishgardian / aesthete crafting nodes."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 681), null) // Brashgold
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 682), null) // Purpure
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 683), null) // Merbau
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 684), null) // Tender Dill
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 713), null) // Ashen Alumen
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 714), null) // Duskblooms
            );

            Add(dict, "levinsand", "Contains Shadowbringers aethersand reduction nodes."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 622), null)
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 624), null)
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 626), null)
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 597), null)
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 599), null)
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 601), null)
            );

            Add(dict, "dusksand", "Contains Stormblood aethersand reduction nodes."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 515), null)
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 518), null)
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 520), null)
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 494), null)
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 496), null)
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 492), null)
            );

            Add(dict, "80ws", "Contains Shadowbringers white scrip collectibles."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 781), "Rarefied Manasilver Sand")    // 6
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 777), "Rarefied Urunday Log")        // 0
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 775), "Rarefied Amber Cloves")       // 2
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 776), "Rarefied Coral")              // 4
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 334), "Rarefied Raw Onyx")           // 8
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 767), "Rarefied Gyr Abanian Alumen") // 10
            );

            Add(dict, "80ys", "Contains Shadowbringers yellow scrip collectibles."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 784), "Rarefied Bright Flax")       // 0
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 766), "Rarefied Reef Rock")         // 2
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 330), "Rarefied Raw Petalite")      // 4
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 332), "Rarefied Raw Lazurite")      // 6
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 334), "Rarefied Sea Salt")          // 8
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 773), "Rarefied Miracle Apple Log") // 10
            );

            Add(dict, "80ysmin", "Contains Shadowbringers yellow scrip miner collectibles."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 780), "Rarefied Bluespirit Ore") // 0, 10
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 766), "Rarefied Reef Rock")      // 2
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 330), "Rarefied Raw Petalite")   // 4
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 332), "Rarefied Raw Lazurite")   // 6
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 334), "Rarefied Sea Salt")       // 8
            );

            Add(dict, "80ysbot", "Contains Shadowbringers yellow scrip botanist collectibles."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 784), "Rarefied Bright Flax")       // 0, 6
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 775), "Rarefied Sandteak Log")      // 2
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 776), "Rarefied Kelp")              // 4
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 774), "Rarefied White Oak Log")     // 8
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 773), "Rarefied Miracle Apple Log") // 10
            );

            Add(dict, "70ys", "Contains Stormblood yellow scrip collectibles."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 772), "Rarefied Pine Log")          // 0
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 770), "Rarefied Larch Log")         // 2
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 771), "Rarefied Shiitake Mushroom") // 4
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 328), "Rarefied Silvergrace Ore")   // 6
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 310), "Rarefied Raw Kyanite")       // 8
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 312), "Rarefied Raw Star Spinel")   // 10
            );

            Add(dict, "70ysmin", "Contains Stormblood yellow scrip miner collectibles."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 779), "Rarefied Gyr Abanian Mineral Water") // 0, 2, 4
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 328), "Rarefied Silvergrace Ore")           // 6
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 310), "Rarefied Raw Kyanite")               // 8
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 312), "Rarefied Raw Star Spinel")           // 10
            );

            Add(dict, "70ysbot", "Contains Stormblood yellow scrip botanist collectibles."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 783), "Rarefied Bloodhemp")         // 6, 8, 10
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 772), "Rarefied Pine Log")          // 0
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 770), "Rarefied Larch Log")         // 2
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 771), "Rarefied Shiitake Mushroom") // 4
            );

            Add(dict, "60ys", "Contains Heavensward yellow scrip collectibles."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 778), "Rarefied Mythrite Sand")     // 6, 8
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 769), "Rarefied Dark Chestnut")     // 0
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 308), "Rarefied Aurum Regis Sand")  // 2
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 306), "Rarefied Limonite")          // 4
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 768), "Rarefied Dark Chestnut Log") // 10
            );

            Add(dict, "60ysmin", "Contains Heavensward yellow scrip miner collectibles."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 778), "Rarefied Mythrite Sand")    // 0, 6, 8, 10
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 308), "Rarefied Aurum Regis Sand") // 2
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 306), "Rarefied Limonite")         // 4
            );

            Add(dict, "60ysbot", "Contains Heavensward yellow scrip botanist collectibles."
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 782), "Rarefied Rainbow Cotton Boll") // 2, 4, 6, 8
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 769), "Rarefied Dark Chestnut")       // 0
                ,     (nodeValues.FirstOrDefault(n => n.Meta!.PointBaseId == 768), "Rarefied Dark Chestnut Log")   // 10
            );

            return dict;
        }

        public static void PrintHelp(ChatGui chat, Dictionary<string, TimedGroup> groups)
        {
            chat.Print("Use with [GroupName] [optional:minute offset], valid GroupNames are:");
            foreach (var group in groups)
                chat.Print($"        {group.Key} - {group.Value.Desc}");
        }
    };
}
