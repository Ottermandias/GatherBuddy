using System.Collections.Generic;
using System.Linq;
using Dalamud;
using Dalamud.Game.Internal.Gui;
using Dalamud.Plugin;
using GatherBuddy.Nodes;
using GatherBuddy.Utility;

namespace GatherBuddy.Classes
{
    public class TimedGroup
    {
        public string Name { get; }
        public string Desc { get; }

        private readonly (Node node, string desc)[] _nodes;

        private static string? CorrectItemName(ClientLanguage lang, Node? node, string? itemName)
        {
            if (node == null || itemName == null)
                return null;

            var item = node.Items!.Items.FirstOrDefault(I => (I?.Name ?? string.Empty) == itemName);
            if (item != null)
                return item.Name[lang];

            PluginLog.Error("Node {NodeId} does not contain an item called {Name}.", node.Meta!.PointBaseId, itemName);
            return null;
        }

        public TimedGroup(ClientLanguage lang, string name, string desc, params (Node? node, string? desc)[] nodes)
        {
            Name   = name;
            Desc   = desc;
            _nodes = new (Node Node, string desc)[24];

            nodes = nodes.Where(n => n.node != null)
                .Select(n => (n.node, CorrectItemName(lang, n.node!, n.desc!)))
                .ToArray();

            for (var i = 0; i < _nodes.Length; ++i)
            {
                foreach (var n in nodes)
                {
                    if (n.node!.Times!.IsUp((uint) i))
                        _nodes[i] = n!;
                }
            }

            for (var i = _nodes.Length - 1; i >= 0; --i)
            {
                if (_nodes[i].node == null)
                    _nodes[i] = _nodes[i + 1];
            }

            // Repeat ones for circularity, this time check for correctness.
            for (var i = _nodes.Length - 1; i >= 0; --i)
            {
                if (_nodes[i].node == null)
                    _nodes[i] = _nodes[i + 1];
                if (_nodes[i].node == null)
                    PluginLog.Debug("TimedGroup {Name} has no node at hour {Hour}.", name, i);
            }
        }

        public (Node? node, string? desc) CurrentNode(uint hour)
        {
            hour %= RealTime.HoursPerDay;
            return _nodes[hour];
        }

        public static void PrintHelp(ChatGui chat, Dictionary<string, TimedGroup> groups)
        {
            chat.Print("Use with [GroupName] [optional:minute offset], valid GroupNames are:");
            foreach (var group in groups)
                chat.Print($"        {group.Key} - {group.Value.Desc}");
        }
    };
}
