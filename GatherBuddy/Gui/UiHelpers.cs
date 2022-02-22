using System;
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

    
    private static void DrawFormatInput(string label, string tooltip, string oldValue, string defaultValue, Action<string> setValue)
    {
        var       tmp = oldValue;
        using var id  = ImGuiRaii.PushId(label);

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
}
