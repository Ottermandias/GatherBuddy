using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Utility;
using GatherBuddy.Config;
using GatherBuddy.Interfaces;
using GatherBuddy.Time;
using ImGuiNET;
using OtterGui;
using OtterGui.Table;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface
{
    internal static bool DrawLocationInput(IGatherable item, ILocation? current, out ILocation? ret)
    {
        const string noPreferred = "No Preferred Location";
        var          width       = SetInputWidth * 0.85f;
        ret = current;
        if (item.Locations.Count() == 1)
        {
            using var style = ImRaii.PushStyle(ImGuiStyleVar.ButtonTextAlign, new Vector2(0, 0.5f));
            ImGuiUtil.DrawTextButton(item.Locations.First().Name, new Vector2(width, 0), ImGui.GetColorU32(ImGuiCol.FrameBg));
            DrawLocationTooltip(item.Locations.First());
            return false;
        }

        var text = current?.Name ?? noPreferred;
        ImGui.SetNextItemWidth(width);
        using var combo = ImRaii.Combo("##Location", text);
        DrawLocationTooltip(current);
        if (!combo)
            return false;

        var changed = false;

        if (ImGui.Selectable(noPreferred, current == null))
        {
            ret     = null;
            changed = true;
        }

        var idx = 0;
        foreach (var loc in item.Locations)
        {
            using var id = ImRaii.PushId(idx++);
            if (ImGui.Selectable(loc.Name, loc.Id == (current?.Id ?? 0)))
            {
                ret     = loc;
                changed = true;
            }

            DrawLocationTooltip(loc);
        }

        return changed;
    }

    internal static void DrawTimeInterval(TimeInterval uptime, bool uptimeDependency = false, bool rightAligned = true)
    {
        var active = uptime.ToTimeString(GatherBuddy.Time.ServerTime, false, out var timeString);
        var colorId = (active, uptimeDependency) switch
        {
            (true, true)   => ColorId.DependentAvailableFish.Value(),
            (true, false)  => ColorId.AvailableItem.Value(),
            (false, true)  => ColorId.DependentUpcomingFish.Value(),
            (false, false) => ColorId.UpcomingItem.Value(),
        };
        using var color = ImRaii.PushColor(ImGuiCol.Text, colorId);
        if (rightAligned)
            ImGuiUtil.RightAlign(timeString);
        else
            ImGui.TextUnformatted(timeString);
        color.Pop();
        if ((uptimeDependency || !char.IsLetter(timeString[0])) && ImGui.IsItemHovered())
        {
            using var tt = ImRaii.Tooltip();

            if (uptimeDependency)
                ImGuiUtil.DrawTextButton("Uptime Dependency", Vector2.Zero, 0xFF202080);

            if (!char.IsLetter(timeString[0]))
                ImGui.Text($"{uptime.Start}\n{uptime.End}\n{uptime.DurationString()}");
        }
    }

    internal static void HoverTooltip(string text)
    {
        if (!text.StartsWith('\0'))
            ImGuiUtil.HoverTooltip(text);
    }

    public static void AlignTextToSize(string text, Vector2 size)
    {
        var cursor = ImGui.GetCursorPos();
        ImGui.SetCursorPos(cursor + new Vector2(ImGui.GetStyle().ItemSpacing.X / 2, (size.Y - ImGui.GetTextLineHeight()) / 2));
        ImGui.Text(text);
        ImGui.SameLine();
        ImGui.SetCursorPosY(cursor.Y);
        ImGui.NewLine();
    }


    private static void DrawFormatInput(string label, string tooltip, string oldValue, string defaultValue, Action<string> setValue)
    {
        var       tmp = oldValue;
        using var id  = ImRaii.PushId(label);

        ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - 50 * Scale);
        if (ImGui.InputText(string.Empty, ref tmp, 256) && tmp != oldValue)
        {
            setValue(tmp);
            GatherBuddy.Config.Save();
        }

        ImGuiUtil.HoverTooltip(tooltip);

        if (ImGuiUtil.DrawDisabledButton("Default", Vector2.Zero, defaultValue, defaultValue == oldValue))
        {
            setValue(defaultValue);
            GatherBuddy.Config.Save();
        }

        ImGui.SameLine();
        ImGui.AlignTextToFramePadding();
        ImGui.Text(label);
    }

    private static void DrawStatusLine<T>(Table<T> table, string name)
    {
        if (!GatherBuddy.Config.ShowStatusLine)
            return;

        ImGui.SameLine();
        using var style = ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, Vector2.Zero);
        ImGui.NewLine();
        ImGui.TextUnformatted($"{table.CurrentItems} / {table.TotalItems} {name} Visible");
        if (table.TotalColumns != table.VisibleColumns)
        {
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(50 * ImGuiHelpers.GlobalScale, 0));
            ImGui.SameLine();
            ImGui.TextUnformatted($"{table.TotalColumns - table.VisibleColumns} Columns Hidden");
        }
    }

    private static void DrawClippy()
    {
        const string popupName = "GatherClippy###ClippyPopup";
        const string text      = "Can't find something?";
        if (GatherBuddy.Config.HideClippy)
            return;

        var textSize   = ImGui.CalcTextSize(text).X;
        var buttonSize = new Vector2(Math.Max(200, textSize) * ImGuiHelpers.GlobalScale, ImGui.GetFrameHeight());
        var padding    = ImGuiHelpers.ScaledVector2(9, 9);

        ImGui.SetCursorPos(ImGui.GetWindowSize() - buttonSize - padding);
        using var child = ImRaii.Child("##clippyChild", buttonSize, false, ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoDecoration);
        if (!child)
            return;

        using var color = ImRaii.PushColor(ImGuiCol.Button, 0xFFA06020);

        if (ImGui.Button(text, buttonSize))
            ImGui.OpenPopup(popupName);
        if (ImGui.IsItemClicked(ImGuiMouseButton.Right) && ImGui.GetIO().KeyCtrl && ImGui.GetIO().KeyShift)
        {
            GatherBuddy.Config.HideClippy = true;
            GatherBuddy.Config.Save();
        }

        ImGuiUtil.HoverTooltip("Click for some help navigating this table.\n"
          + "Control + Shift + Right-Click to permanently hide this button.");

        color.Pop();
        var windowSize = new Vector2(1024 * ImGuiHelpers.GlobalScale,
            ImGui.GetTextLineHeightWithSpacing() * 13 + 2 * ImGui.GetFrameHeightWithSpacing());
        ImGui.SetNextWindowSize(windowSize);
        ImGui.SetNextWindowPos((ImGui.GetIO().DisplaySize - windowSize) / 2);
        using var popup = ImRaii.Popup(popupName,
            ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.Modal);
        if (!popup)
            return;

        ImGui.BulletText(
            "You can use text filters like \"Item Name...\" to only show entries that contain the given string. They are case-insensitive and are not stored for your next session.");
        ImGui.BulletText(
            "Text filters also support regular expressions, e.g. \"(blue|green)\" matches all entries that contain either blue or green.");
        ImGui.BulletText("Button filters like \"Next Uptime\", \"Node Type\" or \"Fish Type\" allow you to filter specific types on click.");
        ImGui.BulletText("Those filters are stored across sessions. For columns with active filters, the filter buttons are tinted red.");
        ImGui.NewLine();
        ImGui.BulletText(
            "You can click in the blank space of a header to sort the table in this column, ascending or descending. This is signified with a little triangle pointing up or down.");
        ImGui.BulletText(
            "You can right-click in the blank space of a header to open the table context menu, in which you can hide columns you are not interested in.");
        ImGui.BulletText(
            "You can resize text columns by dragging the small separation markers of the column. It highlights the line in blue. Size is stored across sessions.");
        ImGui.BulletText(
            "You can reorder most columns by left-clicking in the blank space, holding the mouse button and dragging them. Ordering is stored across sessions.");
        ImGui.NewLine();
        ImGui.BulletText(
            "You can right-click item names and a few other columns (like bait and fishing spot) to open further context menus with object-specific options.");
        ImGui.BulletText("You can also re-order the tabs themselves, though that is not stored across sessions.");

        ImGui.SetCursorPosY(windowSize.Y - ImGui.GetFrameHeight() - ImGui.GetStyle().WindowPadding.Y);
        if (ImGui.Button("Understood", -Vector2.UnitX))
            ImGui.CloseCurrentPopup();
    }
}
