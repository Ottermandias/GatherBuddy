using System;
using System.Linq;
using Dalamud;
using GatherBuddy.Enums;
using GatherBuddy.Managers;
using ImGuiNET;

namespace GatherBuddy.Gui.Cache
{
    internal struct Alarms
    {
        public readonly Nodes.Node[]       AllTimedNodes;
        public readonly string[]     AllTimedNodeNames;
        public readonly string[]     SoundNames;
        public readonly float        LongestNodeNameLength;
        public readonly AlarmManager Manager;

        public readonly float NameSize;
        public readonly float SoundSize;
        public readonly float OffsetSize;

        public string NewName;
        public string NodeFilter;
        public int    NewIdx;
        public bool   FocusFilter;

        public Alarms(AlarmManager alarms, ClientLanguage lang)
        {
            Manager       = alarms;
            AllTimedNodes = Manager.AllTimedNodes;
            SoundNames    = Enum.GetNames(typeof(Sounds)).Where(s => s != "Unknown").ToArray();

            AllTimedNodeNames = alarms.AllTimedNodes
                .Select(n => $"{n.Times!.PrintHours(true)}: {n.Items!.PrintItems(", ", lang)}")
                .ToArray();
            LongestNodeNameLength = AllTimedNodeNames.Max(n => ImGui.CalcTextSize(n).X) / ImGui.GetIO().FontGlobalScale;

            NewName     = "";
            NewIdx      = 0;
            FocusFilter = false;
            NodeFilter  = "";

            NameSize   = ImGui.CalcTextSize("mmmmmmmmmmmm").X / ImGui.GetIO().FontGlobalScale;
            SoundSize  = ImGui.CalcTextSize("Sound9999999").X / ImGui.GetIO().FontGlobalScale;
            OffsetSize = ImGui.CalcTextSize("99999").X / ImGui.GetIO().FontGlobalScale;
        }

        public void AddNode()
        {
            Manager.AddNode(NewName, AllTimedNodes[NewIdx]);
            NewName = "";
        }
    }
}
