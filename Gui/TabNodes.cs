using System.Linq;
using System.Numerics;
using GatherBuddy.Nodes;
using ImGuiNET;

namespace GatherBuddy.Gui;

public partial class Interface
{
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
        var tooltip =
            $"{n.Nodes!.Territory!.Name[GatherBuddy.Language]}, {coords} - {n.GetClosestAetheryte()?.Name[GatherBuddy.Language] ?? ""}\n"
          + $"{n.Meta!.NodeType}, up at {n.Times!.PrintHours()}\n"
          + $"{n.Meta!.GatheringType} at {n.Meta!.Level}";
        using var tt = ImGuiRaii.NewTooltip();
        foreach (var item in n.Items!.ActualItems)
        {
            var icon = _icons[item.ItemData.Icon];
            ImGui.Image(icon.ImGuiHandle, _iconSize);
            ImGui.SameLine();
        }

        ImGui.NewLine();
        ImGui.Text(tooltip);
    }

    private void DrawNodeFilter()
    {
        ImGui.SetNextItemWidth(-1);
        if (ImGui.InputTextWithHint("##NodeFilter", "Filter Nodes...", ref _nodeTabCache.NodeFilter, 64))
            _nodeTabCache.NodeFilterLower = _nodeTabCache.NodeFilter.ToLowerInvariant();
    }

    private void DrawTimedNodes(float widgetHeight)
    {
        using var imgui = new ImGuiRaii();
        if (!imgui.BeginChild("Nodes", new Vector2(-1, -widgetHeight - _framePadding.Y), true))
            return;

        var enumerator = _nodeTabCache.NodeFilterLower.Length == 0
            ? _nodeTabCache.ActiveNodeItems
            : _nodeTabCache.ActiveNodeItems.Where(p => p.Item3.Contains(_nodeTabCache.NodeFilterLower));
        foreach (var (n, i, _) in enumerator)
        {
            var colors = Colors.NodeTab.GetColor(n, (uint)_nodeTabCache.HourOfDay);
            imgui.PushColor(ImGuiCol.Text, colors);

            if (ImGui.Selectable(i))
                _plugin.Gatherer!.OnGatherActionWithNode(n);
            imgui.PopColors();

            if (ImGui.IsItemHovered())
                SetNodeTooltip(n);
        }
    }

    private void DrawTimedSelectors(float boxHeight)
    {
        var checkBoxAdd  = _itemSpacing.X * 3 + _framePadding.X * 2 + ImGui.GetTextLineHeight();
        var jobBoxWidth  = _nodeTabCache.BotanistTextSize * ImGui.GetIO().FontGlobalScale + checkBoxAdd;
        var typeBoxWidth = _nodeTabCache.NodeTypeTextSize * ImGui.GetIO().FontGlobalScale + checkBoxAdd;

        using var imgui = new ImGuiRaii();

        if (imgui.BeginChild("Jobs", new Vector2(jobBoxWidth, boxHeight), true))
        {
            DrawMinerBox();
            DrawBotanistBox();
        }

        imgui.End();

        ImGui.SameLine();
        if (imgui.BeginChild("Types", new Vector2(typeBoxWidth, boxHeight), true))
        {
            DrawUnspoiledBox();
            DrawEphemeralBox();
        }

        imgui.End();

        ImGui.SameLine();
        if (!imgui.BeginChild("Expansion", new Vector2(0, boxHeight), true))
            return;

        imgui.Group();
        DrawArrBox();
        DrawHeavenswardBox();
        imgui.End();
        HorizontalSpace(_horizontalSpace);
        imgui.Group();
        DrawStormbloodBox();
        DrawShadowbringersBox();
        imgui.End();
        HorizontalSpace(_horizontalSpace);
        DrawEndwalkerBox();
    }

    private void DrawNodesTab()
    {
        var boxHeight = 2 * _textHeight + _itemSpacing.X + _framePadding.X * 4;
        DrawNodeFilter();
        DrawTimedNodes(boxHeight + _framePadding.Y);
        DrawTimedSelectors(boxHeight);
    }
}
