using System;
using System.Collections.Generic;
using System.Numerics;
using GatherBuddy.Managers;
using GatherBuddy.Nodes;
using GatherBuddy.Utility;
using ImGuiNET;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private List<(Node, uint)> _activeNodes = new();

        private float _boxHeight = 0f;
        private uint  _lastHourOfDay;

        private void RebuildList(bool save = true)
        {
            if (save)
                Save();
            else
                _lastHourOfDay = EorzeaTime.CurrentHourOfDay();

            _activeNodes = _plugin.Gatherer!.Timeline.GetNewList(_config.ShowNodes);
            if (_activeNodes.Count > 0)
                NodeTimeLine.SortByUptime(_lastHourOfDay, _activeNodes);
        }

        private void UpdateNodes(long totalHour)
        {
            if (_activeNodes.Count > 0 && totalHour <= _totalHourNodes)
                return;

            _totalHourNodes = totalHour;
            _lastHourOfDay  = (uint) _totalHourNodes % RealTime.HoursPerDay;
            NodeTimeLine.SortByUptime(_lastHourOfDay, _activeNodes);
        }

        private void DrawMinerBox()
            => DrawVisibilityBox(ShowNodes.Mining, "Miner", "Show timed nodes for miners.");

        private void DrawBotanistBox()
            => DrawVisibilityBox(ShowNodes.Botanist, "Botanist", "Show timed nodes for botanists.");

        private void DrawUnspoiledBox()
            => DrawVisibilityBox(ShowNodes.Unspoiled, "Unspoiled", "Show unspoiled nodes.");

        private void DrawEphemeralBox()
            => DrawVisibilityBox(ShowNodes.Ephemeral, "Ephemeral", "Show ephemeral nodes.");

        private void DrawArrBox()
            => DrawVisibilityBox(ShowNodes.ARealmReborn, "ARR", "Show nodes with level 1 to 50.");

        private void DrawHeavenswardBox()
            => DrawVisibilityBox(ShowNodes.Heavensward, "HW", "Show nodes with level 51 to 60.");

        private void DrawStormbloodBox()
            => DrawVisibilityBox(ShowNodes.Stormblood, "SB", "Show nodes with level 61 to 70.");

        private void DrawShadowbringersBox()
            => DrawVisibilityBox(ShowNodes.Shadowbringers, "ShB", "Show nodes with level 71 to 80.");

        private void DrawEndwalkerBox()
            => DrawVisibilityBox(ShowNodes.Endwalker, "EW", "Show nodes with level 81 to 90.");

        private void SetNodeTooltip(Node n)
        {
            var coords = n.GetX() != 0 ? $"({n.GetX()}|{n.GetY()})" : "(Unknown Location)";
            var tooltip = $"{n.Nodes!.Territory!.Name[_lang]}, {coords} - {n.GetClosestAetheryte()?.Name[_lang] ?? ""}\n"
              + $"{n.Meta!.NodeType}, up at {n.Times!.PrintHours()}\n"
              + $"{n.Meta!.GatheringType} at {n.Meta!.Level}";
            ImGui.SetTooltip(tooltip);
        }

        private void DrawTimedNodes(float widgetHeight)
        {
            ImGui.BeginChild("Nodes", new Vector2(-1, -widgetHeight - _framePadding.Y), true);
            foreach (var (n, _) in _activeNodes)
            {
                var colors = Colors.GetColor(n, _lastHourOfDay);
                ImGui.PushStyleColor(ImGuiCol.Text, colors);

                if (ImGui.Selectable(n.Items!.PrintItems(", ", _pi.ClientState.ClientLanguage)))
                    _plugin.Gatherer!.OnGatherActionWithNode(n);
                ImGui.PopStyleColor();

                if (ImGui.IsItemHovered())
                    SetNodeTooltip(n);
            }

            ImGui.EndChild();
        }

        private void DrawTimedSelectors()
        {
            var checkBoxAdd  = ImGui.GetStyle().ItemSpacing.X * 3 + _framePadding.X * 2 + ImGui.GetTextLineHeight();
            var jobBoxWidth  = ImGui.CalcTextSize("Botanist").X + checkBoxAdd;
            var typeBoxWidth = Math.Max(ImGui.CalcTextSize("Unspoiled").X, ImGui.CalcTextSize("Ephemeral").X) + checkBoxAdd;

            ImGui.BeginChild("Jobs", new Vector2(jobBoxWidth, _boxHeight), true);
            DrawMinerBox();
            DrawBotanistBox();
            ImGui.EndChild();

            ImGui.SameLine();
            ImGui.BeginChild("Types", new Vector2(typeBoxWidth, _boxHeight), true);
            DrawUnspoiledBox();
            DrawEphemeralBox();
            ImGui.EndChild();

            ImGui.SameLine();
            ImGui.BeginChild("Expansion", new Vector2(0, _boxHeight), true);
            ImGui.BeginGroup();
            DrawArrBox();
            DrawHeavenswardBox();
            ImGui.EndGroup();
            HorizontalSpace(_horizontalSpace);
            ImGui.BeginGroup();
            DrawStormbloodBox();
            DrawShadowbringersBox();
            ImGui.EndGroup();
            HorizontalSpace(_horizontalSpace);
            DrawEndwalkerBox();
            ImGui.EndChild();
        }

        private void DrawNodesTab(float space)
        {
            _boxHeight = 2 * ImGui.GetTextLineHeightWithSpacing() + _itemSpacing.X + ImGui.GetStyle().FramePadding.X * 4;

            DrawTimedNodes(_boxHeight + ImGui.GetStyle().FramePadding.Y);
            DrawTimedSelectors();
        }

        private static class Colors
        {
            private static readonly Vector4 Default     = new(0.8f, 0.8f, 0.8f, 1.0f);
            private static readonly Vector4 CurrentlyUp = new(0.0f, 1.0f, 0.0f, 1.0f);
            private static readonly Vector4 SoonUp      = new(0.6f, 1.0f, 0.4f, 1.0f);
            private static readonly Vector4 SoonishUp   = new(1.0f, 1.0f, 0.0f, 1.0f);
            private static readonly Vector4 LateUp      = new(1.0f, 1.0f, 0.2f, 1.0f);
            private static readonly Vector4 FarUp       = new(1.0f, 1.0f, 0.6f, 1.0f);

            public static Vector4 GetColor(Node n, uint hour)
            {
                Vector4 colors;
                if (n.Times!.IsUp(hour))
                    colors = CurrentlyUp;
                else if (n.Times.IsUp(hour + 1))
                    colors = SoonUp;
                else if (n.Times.IsUp(hour + 2))
                    colors = SoonishUp;
                else if (n.Times.IsUp(hour + 3))
                    colors = LateUp;
                else if (n.Times.IsUp(hour + 4))
                    colors = FarUp;
                else
                    colors = Default;
                return colors;
            }
        }
    }
}
