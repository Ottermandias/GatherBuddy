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

            Add( dict, "80***", "Contains the six nodes for exarchic crafting."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 758) // Hard Water
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 759) // Solstice Stone
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 760) // Dolomite
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 761) // Wattle Petribark
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 762) // Silver Beech Log
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 763) // Raindrop Cotton Boll               
            );

            Add( dict, "80**", "Contains the six nodes for neo-ishgardian / aesthete crafting."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 681) // Brashgold
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 682) // Purpure
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 683) // Merbau
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 684) // Tender Dill
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 713) // Ashen Alumen
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 714) // Duskblooms
            );

            Add( dict, "levinsand", "Contains the six nodes for Shadowbringers aethersand reduction."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 622)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 624)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 626)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 597)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 599)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 601)
            );

            Add( dict, "dusksand", "Contains the six nodes for Stormblood aethersand reduction."
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 515)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 518)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 520)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 494)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 496)
               , W.nodes.nodeIdToNode.Values.FirstOrDefault(N => N.meta.pointBaseId == 492)
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