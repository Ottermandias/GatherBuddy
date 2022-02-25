using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.Text;
using Dalamud.Interface;
using ImGuiNET;

namespace ImGuiOtter;

public static partial class ImGuiUtil
{
    private static void DrawColorBox(string label, uint color, Vector2 iconSize, string description, bool push)
    {
        using var c     = ImGuiRaii.PushColor(ImGuiCol.ChildBg, color, push);
        using var style = ImGuiRaii.PushStyle(ImGuiStyleVar.ChildRounding, ImGui.GetStyle().FrameRounding);
        ImGui.BeginChild(label, iconSize, true);
        ImGui.EndChild();
        c.Pop();
        HoverTooltip(description);
    }

    public static bool PaletteColorPicker(string label, Vector2 iconSize, int currentColorIdx, int defaultColorIdx,
        IDictionary<int, uint> colors, out int newColorIdx)
    {
        newColorIdx = -1;
        using var group = ImGuiRaii.NewGroup();
        using var id    = ImGuiRaii.PushId(label);
        if (colors.TryGetValue(currentColorIdx, out var currentColor))
            DrawColorBox("##preview", currentColor, iconSize, $"{currentColorIdx} - {ColorBytes(currentColor)}\nRight-click to clear.", true);
        else
            DrawColorBox("##preview", 0, iconSize, "None", false);
        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
        {
            newColorIdx = -1;
            return currentColorIdx != -1;
        }

        if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
            ImGui.OpenPopup("##popup");
        if (colors.TryGetValue(defaultColorIdx, out var def))
        {
            ImGui.SameLine();
            if (DrawDisabledButton("Default", Vector2.Zero, $"Reset this color to {defaultColorIdx} ({ColorBytes(def)}).",
                    currentColorIdx == defaultColorIdx))
            {
                newColorIdx = defaultColorIdx;
                return true;
            }
        }

        if (label.Length > 0 && !label.StartsWith("##"))
        {
            ImGui.SameLine();
            ImGui.Text(label);
        }

        if (ImGui.BeginPopupContextWindow("##popup"))
        {
            using var end     = ImGuiRaii.DeferredEnd(ImGui.EndPopup);
            var       counter = 0;
            foreach (var (idx, value) in colors)
            {
                var text = $"{idx} - {ColorBytes(value)}";
                DrawColorBox(text, value, iconSize, text, true);
                if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
                {
                    newColorIdx = idx;
                    ImGui.CloseCurrentPopup();
                }

                if (counter++ % 10 != 9)
                    ImGui.SameLine();
            }
        }

        return newColorIdx != -1;
    }

    public static bool DrawDisabledButton(string label, Vector2 size, string description, bool disabled, bool icon = false)
    {
        using var alpha = ImGuiRaii.PushStyle(ImGuiStyleVar.Alpha, 0.5f, disabled);
        using var font  = ImGuiRaii.PushFont(UiBuilder.IconFont, icon);
        var       ret   = ImGui.Button(label, size);
        alpha.Pop();
        font.Pop();
        HoverTooltip(description);
        return ret && !disabled;
    }

    public static void DrawTextButton(string text, Vector2 size, uint buttonColor)
    {
        using var color = ImGuiRaii.PushColor(ImGuiCol.Button, buttonColor)
            .Push(ImGuiCol.ButtonActive,  buttonColor)
            .Push(ImGuiCol.ButtonHovered, buttonColor);
        ImGui.Button(text, size);
    }

    public static void DrawTextButton(string text, Vector2 size, uint buttonColor, uint textColor)
    {
        using var color = ImGuiRaii.PushColor(ImGuiCol.Button, buttonColor)
            .Push(ImGuiCol.ButtonActive,  buttonColor)
            .Push(ImGuiCol.ButtonHovered, buttonColor)
            .Push(ImGuiCol.Text,          textColor);
        ImGui.Button(text, size);
    }

    public static void HoverTooltip(string tooltip)
    {
        if (tooltip.Any() && ImGui.IsItemHovered())
            ImGui.SetTooltip(tooltip);
    }

    public static bool Checkbox(string label, string description, bool current, Action<bool> setter)
    {
        var tmp    = current;
        var result = ImGui.Checkbox(label, ref tmp);
        HoverTooltip(description);
        if (!result || tmp == current)
            return false;

        setter(tmp);
        return true;
    }

    public static void DrawTableColumn(string text)
    {
        ImGui.TableNextColumn();
        ImGui.Text(text);
    }

    public static uint ReorderColor(uint seColor)
    {
        var fa = seColor & 255;
        var fb = (seColor >> 8) & 255;
        var fg = (seColor >> 16) & 255;
        var fr = seColor >> 24;
        return fr | (fg << 8) | (fb << 16) | (fa << 24);
    }

    private static string ColorBytes(uint color)
        => $"#{(byte)(color & 0xFF):X2}{(byte)(color >> 8):X2}{(byte)(color >> 16):X2}{(byte)(color >> 24):X2}";

    public static bool ColorPicker(string label, string tooltip, uint current, Action<uint> setter, uint standard)
    {
        var       ret = false;
        var       old = ImGui.ColorConvertU32ToFloat4(current);
        var       tmp = old;
        using var _   = ImGuiRaii.PushId(label);
        ImGui.BeginGroup();
        if (ImGui.ColorEdit4("", ref tmp, ImGuiColorEditFlags.AlphaPreviewHalf | ImGuiColorEditFlags.NoInputs) && tmp != old)
        {
            setter(ImGui.ColorConvertFloat4ToU32(tmp));
            ret = true;
        }

        ImGui.SameLine();
        using var alpha = ImGuiRaii.PushStyle(ImGuiStyleVar.Alpha, 0.5f, current == standard);
        if (ImGui.Button("Default") && current != standard)
        {
            setter(standard);
            ret = true;
        }

        alpha.Pop();
        HoverTooltip($"Reset this color to {ColorBytes(standard)}.");

        ImGui.SameLine();
        ImGui.Text(label);
        if (tooltip.Any())
            HoverTooltip(tooltip);
        ImGui.EndGroup();

        return ret;
    }

    public static bool DrawEditButtonText(int id, string current, out string newText, ref bool edit, Vector2 buttonSize, float inputWidth,
        uint maxLength = 256)
    {
        newText = current;
        var       tmpEdit = edit;
        using var style   = ImGuiRaii.PushStyle(ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemSpacing / 2);
        using var _       = ImGuiRaii.PushId(id);
        if (DrawDisabledButton(FontAwesomeIcon.Edit.ToIconString(), buttonSize, "Rename", edit, true))
            edit = true;
        ImGui.SameLine();
        style.Pop();
        if (!edit)
        {
            DrawTextButton(current, Vector2.Zero, ImGui.GetColorU32(ImGuiCol.FrameBg));
            return false;
        }

        ImGui.SetNextItemWidth(inputWidth);
        if (edit != tmpEdit)
        {
            ImGui.SetKeyboardFocusHere();
            ImGui.SetItemDefaultFocus();
        }

        if (ImGui.InputText("##rename", ref newText, maxLength, ImGuiInputTextFlags.EnterReturnsTrue))
            return true;

        if (edit == tmpEdit && !ImGui.IsItemActive())
            edit = false;
        return false;
    }

    public static void ClippedDraw<T>(IReadOnlyList<T> data, Action<T> func, float lineHeight)
    {
        ImGuiListClipperPtr clipper;
        unsafe
        {
            clipper = new ImGuiListClipperPtr(ImGuiNative.ImGuiListClipper_ImGuiListClipper());
        }

        clipper.Begin(data.Count, lineHeight);
        while (clipper.Step())
        {
            for (var actualRow = clipper.DisplayStart; actualRow < clipper.DisplayEnd; actualRow++)
            {
                if (actualRow >= data.Count)
                    return;

                if (actualRow < 0)
                    continue;

                func(data[actualRow]);
            }
        }

        clipper.End();
    }

    public static void HoverIcon(ImGuiScene.TextureWrap icon, Vector2 iconSize)
    {
        var size = new Vector2(icon.Width, icon.Height);
        ImGui.Image(icon.ImGuiHandle, iconSize);
        if (iconSize.X > size.X || iconSize.Y > size.Y || !ImGui.IsItemHovered())
            return;

        ImGui.BeginTooltip();
        ImGui.Image(icon.ImGuiHandle, size);
        ImGui.EndTooltip();
    }

    public static uint MiddleColor(uint c1, uint c2)
    {
        var r = ((c1 & 0xFF) + (c2 & 0xFF)) / 2;
        var g = (((c1 >> 8) & 0xFF) + ((c2 >> 8) & 0xFF)) / 2;
        var b = (((c1 >> 16) & 0xFF) + ((c2 >> 16) & 0xFF)) / 2;
        var a = (((c1 >> 24) & 0xFF) + ((c2 >> 24) & 0xFF)) / 2;
        return r | (g << 8) | (b << 16) | (a << 24);
    }

    public static void RightAlign(string text)
    {
        var offset = ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize(text).X;
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + offset);
        ImGui.Text(text);
    }

    public static void Center(string text)
    {
        var offset = (ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize(text).X) / 2;
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + offset);
        ImGui.Text(text);
    }

    public static bool OpenNameField(string popupName, ref string newName)
    {
        if (!ImGui.BeginPopup(popupName))
            return false;

        if (ImGui.IsKeyPressed(ImGui.GetKeyIndex(ImGuiKey.Escape)))
            ImGui.CloseCurrentPopup();

        using var end = ImGuiRaii.DeferredEnd(ImGui.EndPopup);
        ImGui.SetNextItemWidth(300 * ImGuiHelpers.GlobalScale);
        var enterPressed = ImGui.InputTextWithHint("##newName", "Enter New Name...", ref newName, 64, ImGuiInputTextFlags.EnterReturnsTrue);
        if (ImGui.IsWindowAppearing())
            ImGui.SetKeyboardFocusHere();

        if (!enterPressed)
            return false;

        ImGui.CloseCurrentPopup();
        return true;
    }

    public static bool DrawChatTypeSelector(string label, string description, XivChatType currentValue, Action<XivChatType> setter)
    {
        using var id  = ImGuiRaii.PushId(label);
        var       ret = ImGui.BeginCombo(label, currentValue.ToString());
        using var end = ImGuiRaii.DeferredEnd(ImGui.EndCombo, ret);
        HoverTooltip(description);
        if (ret)
            ret = false;
        else
            return false;

        foreach (var type in Enum.GetValues<XivChatType>())
        {
            if (!ImGui.Selectable(type.ToString(), currentValue == type) || type == currentValue)
                continue;

            setter(type);
            ret = true;
        }

        return ret;
    }

    public static bool KeySelector(string label, string description, VirtualKey currentValue, Action<VirtualKey> setter,
        IReadOnlyList<VirtualKey> keys)
    {
        using var id  = ImGuiRaii.PushId(label);
        var       ret = ImGui.BeginCombo(label, currentValue.GetFancyName());
        using var end = ImGuiRaii.DeferredEnd(ImGui.EndCombo, ret);
        HoverTooltip(description);
        if (ret)
            ret = false;
        else
            return false;

        foreach (var key in keys)
        {
            if (!ImGui.Selectable(key.GetFancyName(), currentValue == key) || currentValue == key)
                continue;

            setter(key);
            ret = true;
        }

        return ret;
    }

    public static bool ModifierSelector(string label, string description, ModifierHotkey currentValue, Action<ModifierHotkey> setter)
        => KeySelector(label, description, currentValue, k => setter(k), ModifierHotkey.ValidKeys);

    public static bool ModifiableKeySelector(string label, string description, float width, ModifiableHotkey currentValue,
        Action<ModifiableHotkey> setter,
        IReadOnlyList<VirtualKey> keys)
    {
        using var id   = ImGuiRaii.PushId(label);
        var       copy = currentValue;
        ImGui.SetNextItemWidth(width);
        var changes = KeySelector(label, description, currentValue.Hotkey, k => copy.SetHotkey(k), keys);

        if (currentValue.Hotkey != VirtualKey.NO_KEY)
        {
            using var indent = ImGuiRaii.PushIndent();
            ImGui.SetNextItemWidth(width - indent.Indentation);
            changes |= ModifierSelector("Modifier", "Set an optional modifier key to be used in conjunction with the selected hotkey.",
                currentValue.Modifier1,             k => copy.SetModifier1(k));

            if (currentValue.Modifier1 != VirtualKey.NO_KEY)
            {
                ImGui.SetNextItemWidth(width - indent.Indentation);
                changes |= ModifierSelector("Additional Modifier",
                    "Set another optional modifier key to be used in conjunction with the selected hotkey and the first modifier.",
                    currentValue.Modifier2, k => copy.SetModifier2(k));
            }
        }

        if (changes)
            setter(copy);
        return changes;
    }
}
