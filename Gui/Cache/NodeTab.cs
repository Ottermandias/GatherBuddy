using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
using ImGuiNET;

namespace GatherBuddy.Gui.Cache
{
    internal class NodeTab
    {
        private readonly NodeTimeLine                   _nodeTimeLine;
        private readonly GatherBuddyConfiguration       _config;
        private readonly Dictionary<Nodes.Node, string> _allNodeItems;

        public List<(Nodes.Node, uint)> ActiveNodes;
        public (Nodes.Node, string)[]   ActiveNodeItems;
        public uint                     HourOfDay;
        public long                     TotalHour;

        public readonly float BotanistTextSize;
        public readonly float NodeTypeTextSize;

        private void UpdateNodes()
            => ActiveNodeItems = ActiveNodes.Select(n => (n.Item1, _allNodeItems[n.Item1])).ToArray();

        public NodeTab(GatherBuddyConfiguration config, NodeTimeLine nodeTimeLine)
        {
            _config         = config;
            _nodeTimeLine   = nodeTimeLine;
            ActiveNodes     = _nodeTimeLine.GetNewList(_config.ShowNodes);
            ActiveNodeItems = new (Nodes.Node, string)[0];
            HourOfDay       = EorzeaTime.CurrentHourOfDay();
            _allNodeItems = _nodeTimeLine.GetNewList(ShowNodes.AllNodes)
                .ToDictionary(n => n.Item1, n => n.Item1.Items!.PrintItems(", ", GatherBuddy.Language));
            UpdateNodes();
            TotalHour       = 0;

            BotanistTextSize = ImGui.CalcTextSize("Botanist").X / ImGui.GetIO().FontGlobalScale;
            NodeTypeTextSize = Math.Max(ImGui.CalcTextSize("Unspoiled").X, ImGui.CalcTextSize("Ephemeral").X) / ImGui.GetIO().FontGlobalScale;
        }

        public bool Rebuild()
        {
            HourOfDay   = EorzeaTime.CurrentHourOfDay();
            ActiveNodes = _nodeTimeLine.GetNewList(_config.ShowNodes);
            if (ActiveNodes.Count > 0)
                NodeTimeLine.SortByUptime(HourOfDay, ActiveNodes);
            UpdateNodes();
            return true;
        }

        public void Update(long totalHour)
        {
            if (ActiveNodes.Count == 0 || totalHour <= TotalHour)
                return;

            TotalHour = totalHour;
            HourOfDay = (uint) totalHour % RealTime.HoursPerDay;
            NodeTimeLine.SortByUptime(HourOfDay, ActiveNodes);
            UpdateNodes();
        }
    }
}
