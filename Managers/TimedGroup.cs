using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Internal.Gui;
using Serilog;

namespace Gathering
{
    public class TimedGroup
    {
        public string name;
        public string desc;
        private readonly Node[] nodes;
    
        public TimedGroup(string name, string desc, params Node[] nodes)
        {
            this.name = name;
            this.desc = desc;
            this.nodes = new Node[24];

            for(int i = 0; i < this.nodes.Length; ++i)
                foreach (var N in nodes.Where( N => N != null))
                    if (N.times.IsUp(i))
                        this.nodes[i] = N;
            for(int i = this.nodes.Length - 1; i >= 0; --i)
                if (this.nodes[i] == null)
                    this.nodes[i] = this.nodes[i + 1];

            // Repeat ones for circularity, this time check for correctness.
            for(int i = this.nodes.Length - 1; i >= 0; --i)
            {
                if (this.nodes[i] == null)
                    this.nodes[i] = this.nodes[i + 1];
                if (this.nodes[i] == null)
                    Log.Debug($"[GatherBuddy] TimedGroup {name} has no node at hour {i}.");
            }
        }
    
        public Node CurrentNode(int hour)
        {
            if (hour > 23)
                hour = 23;
            else if (hour < 0)
                hour = 0;
            return nodes[hour];
        }

        private static void Add(Dictionary<string, TimedGroup> dict, string name, string desc, params Node[] nodes)
        {
            dict.Add(name, new TimedGroup(name, desc, nodes));
        }
    
        public static Dictionary<string, TimedGroup> CreateGroups(World W)
        {
            var dict = new Dictionary<string, TimedGroup>();

            Add( dict, "80***", "Contains exarchic crafting nodes."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 758) // Hard Water
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 759) // Solstice Stone
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 760) // Dolomite
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 761) // Wattle Petribark
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 762) // Silver Beech Log
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 763) // Raindrop Cotton Boll               
            );

            Add( dict, "80**", "Contains neo-ishgardian / aesthete crafting nodes."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 681) // Brashgold
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 682) // Purpure
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 683) // Merbau
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 684) // Tender Dill
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 713) // Ashen Alumen
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 714) // Duskblooms
            );

            Add( dict, "levinsand", "Contains Shadowbringers aethersand reduction nodes."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 622)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 624)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 626)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 597)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 599)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 601)
            );

            Add( dict, "dusksand", "Contains Stormblood aethersand reduction nodes."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 515)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 518)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 520)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 494)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 496)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 492)
            );

            Add( dict, "80ws", "Contains Shadowbringers white scrip collectibles."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 781) // Rarefied Manasilver Sand, always, used for 6-8.
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 777) // Rarefied Urunday Log, 0
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 775) // Rarefied Amber Cloves, 2
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 776) // Rarefied Coral, 4
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 334) // Rarefied Raw Onyx, 8
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 767) // Rarefied Gyr Abanian Alumen, 10
            );

            Add( dict, "80ys", "Contains Shadowbringers yellow scrip collectibles."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 784) // Rarefied Bright Flax, always, used for 0-2
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 766) // Rarefied Reef Rock, 2
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 330) // Rarefied Raw Petalite, 4
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 332) // Rarefied Raw Lazurite, 6
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 334) // Rarefied Sea Salt, 8
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 773) // Rarefied Miracle Apple Log, 10
            );

            Add( dict, "80ysmin", "Contains Shadowbringers yellow scrip miner collectibles."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 780) // Rarefied Bluespirit Ore, always, used for 0-2, 10-12.
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 766) // Rarefied Reef Rock, 2
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 330) // Rarefied Raw Petalite, 4
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 332) // Rarefied Raw Lazurite, 6
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 334) // Rarefied Sea Salt, 8
            );

            Add( dict, "80ysbot", "Contains Shadowbringers yellow scrip botanist collectibles."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 784) // Rarefied Bright Flax, always, used for 0-2, 6-8
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 775) // Rarefied Sandteak Log, 2
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 776) // Rarefied Kelp, 4
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 774) // Rarefied White Oak Log, 8
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 773) // Rarefied Miracle Apple Log, 10
            );

            Add( dict, "70ys", "Contains Stormblood yellow scrip collectibles."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 772) // 0
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 770) // 2
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 771) // 4
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 328) // 6
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 310) // 8
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 312) // 10
            );

            Add( dict, "70ysmin", "Contains Stormblood yellow scrip miner collectibles."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 779) // Rarefied Gyr Abanian Mineral Water, always, used for 0-6
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 328) // 6
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 310) // 8
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 312) // 10
            );

            Add( dict, "70ysbot", "Contains Stormblood yellow scrip miner collectibles."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 783) // Rarefied Blood Hemp, always, used for 6-12
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 772) // 0
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 770) // 2
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 771) // 4
            );

            Add( dict, "60ys", "Contains Heavensward yellow scrip collectibles."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 778) // Rarefied Mythrite Sand, used for 6-10
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 769) // 0
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 308) // 2
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 310) // 4
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 768) // 10
            );

            Add( dict, "60ysmin", "Contains Heavensward yellow scrip miner collectibles."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 778) // Rarefied Mythrite Sand, used for 0-2, 6-12
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 308) // 2
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 310) // 4
            );

            Add( dict, "60ysbot", "Contains Heavensward yellow scrip miner collectibles."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 782) // Rarefied Rainbow Cotton Boll, Always, used for 2-10
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 769) // 0
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 768) // 10
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