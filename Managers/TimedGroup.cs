using System.Collections.Generic;
using System.Linq;
using Dalamud;
using Dalamud.Game.Internal.Gui;
using Serilog;

namespace Gathering
{
    public class TimedGroup
    {
        private static ClientLanguage lang;
        public string name;
        public string desc;
        private readonly (Node node, string desc)[] nodes;

        private static string CorrectItemName(Node node, string itemName)
        {
            if (node == null || itemName == null)
                return null;


            var item = node.items.items.FirstOrDefault(I => I?.nameList[ClientLanguage.English] == itemName);
            if (item == null)
            {
                Log.Error($"[GatherBuddy] Node {node.meta.pointBaseId} does not contain an item called {itemName}.");
                return null;
            }

            return item.nameList[lang];
        }

        public TimedGroup(string name, string desc, params (Node node, string desc)[] nodes)
        {
            this.name = name;
            this.desc = desc;
            this.nodes = new (Node Node, string desc)[24];

            nodes = nodes.Where(N => N.node != null)
                .Select( N => (N.node, CorrectItemName(N.node, N.desc)))
                .ToArray();

            for(int i = 0; i < this.nodes.Length; ++i)
                foreach (var N in nodes)
                    if (N.node.times.IsUp(i))
                        this.nodes[i] = N;
            for(int i = this.nodes.Length - 1; i >= 0; --i)
                if (this.nodes[i].node == null)
                    this.nodes[i] = this.nodes[i + 1];

            // Repeat ones for circularity, this time check for correctness.
            for(int i = this.nodes.Length - 1; i >= 0; --i)
            {
                if (this.nodes[i].node == null)
                    this.nodes[i] = this.nodes[i + 1];
                if (this.nodes[i].node == null)
                    Log.Debug($"[GatherBuddy] TimedGroup {name} has no node at hour {i}.");
            }
        }
    
        public (Node node, string desc) CurrentNode(int hour)
        {
            if (hour > 23)
                hour = 23;
            else if (hour < 0)
                hour = 0;
            return nodes[hour];
        }

        private static void Add(Dictionary<string, TimedGroup> dict, string name, string desc, params (Node node, string desc)[] nodes)
        {
            dict.Add(name, new TimedGroup(name, desc, nodes));
        }
    
        public static Dictionary<string, TimedGroup> CreateGroups(World W)
        {
            lang = W.language;
            var dict = new Dictionary<string, TimedGroup>();

            var nodeValues = W.nodes.nodeIdToNode.Values;

            Add( dict, "80***", "Contains exarchic crafting nodes."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 758), null) // Hard Water
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 759), null) // Solstice Stone
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 760), null) // Dolomite
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 761), null) // Wattle Petribark
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 762), null) // Silver Beech Log
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 763), null) // Raindrop Cotton Boll               
            );

            Add( dict, "80**", "Contains neo-ishgardian / aesthete crafting nodes."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 681), null) // Brashgold
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 682), null) // Purpure
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 683), null) // Merbau
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 684), null) // Tender Dill
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 713), null) // Ashen Alumen
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 714), null) // Duskblooms
            );

            Add( dict, "levinsand", "Contains Shadowbringers aethersand reduction nodes."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 622), null)
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 624), null)
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 626), null)
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 597), null)
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 599), null)
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 601), null)
            );

            Add( dict, "dusksand", "Contains Stormblood aethersand reduction nodes."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 515), null)
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 518), null)
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 520), null)
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 494), null)
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 496), null)
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 492), null)
            );

            Add( dict, "80ws", "Contains Shadowbringers white scrip collectibles."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 781), "Rarefied Mansilver Sand")     // 6
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 777), "Rarefied Urunday Log")        // 0
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 775), "Rarefied Amber Cloves")       // 2
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 776), "Rarefied Coral")              // 4
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 334), "Rarefied Raw Onyx")           // 8
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 767), "Rarefied Gyr Abanian Alumen") // 10
            );

            Add( dict, "80ys", "Contains Shadowbringers yellow scrip collectibles."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 784), "Rarefied Bright Flax")       // 0
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 766), "Rarefied Reef Rock")         // 2
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 330), "Rarefied Raw Petalite")      // 4
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 332), "Rarefied Raw Lazurite")      // 6
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 334), "Rarefied Sea Salt")          // 8
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 773), "Rarefied Miracle Apple Log") // 10
            );

            Add( dict, "80ysmin", "Contains Shadowbringers yellow scrip miner collectibles."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 780), "Rarefied Bluespirit Ore") // 0, 10
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 766), "Rarefied Reef Rock")      // 2
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 330), "Rarefied Raw Petalite")   // 4
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 332), "Rarefied Raw Lazurite")   // 6
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 334), "Rarefied Sea Salt")       // 8
            );

            Add( dict, "80ysbot", "Contains Shadowbringers yellow scrip botanist collectibles."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 784), "Rarefied Bright Flax")       // 0, 6
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 775), "Rarefied Sandteak Log")      // 2
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 776), "Rarefied Kelp")              // 4
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 774), "Rarefied White Oak Log")     // 8
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 773), "Rarefied Miracle Apple Log") // 10
            );

            Add( dict, "70ys", "Contains Stormblood yellow scrip collectibles."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 772), "Rarefied Pine Log")          // 0
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 770), "Rarefied Larch Log")         // 2
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 771), "Rarefied Shiitake Mushroom") // 4
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 328), "Rarefied Silvergrace Ore")   // 6
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 310), "Rarefied Raw Kyanite")       // 8
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 312), "Rarefied Raw Star Spinel")   // 10
            );

            Add( dict, "70ysmin", "Contains Stormblood yellow scrip miner collectibles."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 779), "Rarefied Gyr Abanian Mineral Water") // 0, 2, 4
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 328), "Rarefied Silvergrace Ore")           // 6
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 310), "Rarefied Raw Kyanite")               // 8
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 312), "Rarefied Raw Star Spinel")           // 10
            );

            Add( dict, "70ysbot", "Contains Stormblood yellow scrip botanist collectibles."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 783), "Rarefied Blood Hemp")        // 6, 8, 10
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 772), "Rarefied Pine Log")          // 0
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 770), "Rarefied Larch Log")         // 2
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 771), "Rarefied Shiitake Mushroom") // 4
            );

            Add( dict, "60ys", "Contains Heavensward yellow scrip collectibles."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 778), "Rarefied Mythrite Sand")     // 6, 8
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 769), "Rarefied Dark Chestnut")     // 0
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 308), "Rarefied Aurum Regis Sand")  // 2
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 310), "Rarefied Limonite")          // 4
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 768), "Rarefied Dark Chestnut Log") // 10
            );

            Add( dict, "60ysmin", "Contains Heavensward yellow scrip miner collectibles."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 778), "Rarefied Mythrite Sand") // 0, 6, 8, 10
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 308), "Rarefied Aurum Regis Sand") // 2
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 310), "Rarefied Limonite") // 4
            );

            Add( dict, "60ysbot", "Contains Heavensward yellow scrip botanist collectibles."
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 782), "Rarefied Rainbow Cotton Boll") // 2, 4, 6, 8
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 769), "Rarefied Dark Chestnut")       // 0
               , (nodeValues.FirstOrDefault(N => N.meta.pointBaseId == 768), "Rarefied Dark Chestnut Log")   // 10
            );

            return dict;
        }

        public static void PrintHelp(ChatGui chat, Dictionary<string, TimedGroup> groups)
        {
            chat.Print("Use with [GroupName] [optional:minute offset], valid GroupNames are:");
            foreach (var group in groups)
                chat.Print($"        {group.Key} - {group.Value.desc}");
        }
    };
}