using System.Numerics;
using GatherBuddy.Config;
using GatherBuddy.Time;
using ImGuiNET;
using ImGuiOtter;

namespace GatherBuddy.Gui;

public partial class Interface
{
    internal static void DrawTimeInterval(TimeInterval uptime, bool uptimeDependency = false)
    {

        var active = uptime.ToTimeString(GatherBuddy.Time.ServerTime, false, out var timeString);
        var colorId = (active, fish: uptimeDependency) switch
        {
            (true, true)   => ColorId.DependentAvailableFish.Value(),
            (true, false)  => ColorId.AvailableItem.Value(),
            (false, true)  => ColorId.DependentUpcomingFish.Value(),
            (false, false) => ColorId.UpcomingItem.Value(),
        };
        using var color = ImGuiRaii.PushColor(ImGuiCol.Text, colorId);
        ImGuiUtil.RightAlign(timeString);
        color.Pop();
        if ((uptimeDependency || !char.IsLetter(timeString[0])) && ImGui.IsItemHovered())
        {
            using var tt = ImGuiRaii.NewTooltip();

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

    //
    // private static void DrawFormatInput(string label, string tooltip, string oldValue, string defaultValue, Action<string> setValue)
    // {
    //     var tmp = oldValue;
    //
    //     if (ImGui.Button($"Default##{label}"))
    //     {
    //         setValue(defaultValue);
    //         GatherBuddy.Config.Save();
    //     }
    //
    //     ImGuiHelper.HoverTooltip(defaultValue);
    //
    //     HorizontalSpace(10);
    //     ImGui.AlignTextToFramePadding();
    //     ImGui.Text(label);
    //
    //     ImGui.SetNextItemWidth(-ImGui.GetStyle().FramePadding.X);
    //     if (ImGui.InputText($"##{label}", ref tmp, 256) && tmp != oldValue)
    //     {
    //         setValue(tmp);
    //         GatherBuddy.Config.Save();
    //     }
    //
    //     ImGuiHelper.HoverTooltip(tooltip);
    // }

    //private static void DrawColorPicker(string label, string tooltip, Vector4 current, Vector4 defaultValue, Action<Vector4> setter)
    //{
    //    const ImGuiColorEditFlags flags = ImGuiColorEditFlags.Float
    //      | ImGuiColorEditFlags.AlphaPreviewHalf
    //      | ImGuiColorEditFlags.NoInputs
    //      | ImGuiColorEditFlags.NoLabel;
    //
    //    var tmp = current;
    //    if (ImGui.ColorEdit4(label, ref tmp, flags) && tmp != current)
    //    {
    //        setter(tmp);
    //        GatherBuddy.Config.Save();
    //    }
    //
    //    ImGui.SameLine();
    //    if (ImGui.Button($"Default##{label}") && current != defaultValue)
    //    {
    //        setter(defaultValue);
    //        GatherBuddy.Config.Save();
    //    }
    //
    //    ImGuiHelper.HoverTooltip($"Reset the color to its default value #{ImGui.ColorConvertFloat4ToU32(defaultValue):X8}.");
    //
    //    ImGui.SameLine();
    //    ImGui.Text(label);
    //    ImGuiHelper.HoverTooltip(tooltip);
    //}


    //private static void DrawComboWithFilter(string label, IList<string> options, ref int currentIdx, ref string filter, ref bool focus,
    //    float size, int items)
    //{
    //    if (ImGui.BeginCombo(label, options[currentIdx]))
    //    {
    //        try
    //        {
    //            ImGui.SetNextItemWidth(-1);
    //            ImGui.InputTextWithHint($"{label}_filter", "Filter", ref filter, 255);
    //            var isFocused = ImGui.IsItemActive();
    //            if (!focus)
    //                ImGui.SetKeyboardFocusHere();
    //
    //            using var child = new ImGuiRaii();
    //
    //            if (!child.BeginChild($"{label}_list", new Vector2(size, items * ImGui.GetTextLineHeightWithSpacing())))
    //                return;
    //
    //            if (!focus)
    //            {
    //                ImGui.SetScrollY(0);
    //                focus = true;
    //            }
    //
    //            var filterLower = filter.ToLowerInvariant();
    //            var numItems    = 0;
    //            var node        = 0;
    //            for (var i = 0; i < options.Count; ++i)
    //            {
    //                if (!options[i].ToLowerInvariant().Contains(filterLower))
    //                    continue;
    //
    //                ++numItems;
    //                node = i;
    //                if (!ImGui.Selectable(options[i], i == currentIdx))
    //                    continue;
    //
    //                currentIdx = i;
    //                ImGui.CloseCurrentPopup();
    //            }
    //
    //            child.End();
    //            if (!isFocused && numItems <= 1)
    //            {
    //                currentIdx = node;
    //                ImGui.CloseCurrentPopup();
    //            }
    //        }
    //        finally
    //        {
    //            ImGui.EndCombo();
    //        }
    //    }
    //    else
    //    {
    //        focus  = false;
    //        filter = "";
    //    }
    //}
}
