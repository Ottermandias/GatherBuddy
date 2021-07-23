using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface;
using ImGuiNET;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private static void HorizontalSpace(float width)
        {
            ImGui.SameLine();
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + width);
        }

        private void DrawCheckbox(string label, string tooltip, bool current, Action<bool> setter)
        {
            var tmp = current;
            if (ImGui.Checkbox(label, ref tmp) && tmp != current)
            {
                setter(tmp);
                Save();
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(tooltip);
        }

        private void DrawSetInput(float width, string jobName, string oldName, Action<string> setName)
        {
            var tmp = oldName;
            ImGui.SetNextItemWidth(width);
            if (ImGui.InputText($"{jobName} Set", ref tmp, 15) && tmp != oldName)
            {
                setName(tmp);
                Save();
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip($"Set the name of your {jobName.ToLowerInvariant()} set. Can also be the numerical id instead.");
        }

        private void DrawFormatInput(string label, string tooltip, string oldValue, string defaultValue, Action<string> setValue)
        {
            var tmp = oldValue;

            if (ImGui.Button($"Default##{label}"))
            {
                setValue(defaultValue);
                Save();
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(defaultValue);

            HorizontalSpace(10);
            ImGui.AlignTextToFramePadding();
            ImGui.Text(label);

            ImGui.SetNextItemWidth(-ImGui.GetStyle().FramePadding.X);
            if (ImGui.InputText($"##{label}", ref tmp, 256) && tmp != oldValue)
            {
                setValue(tmp);
                Save();
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(tooltip);
        }

        private void DrawVisibilityBox(ShowNodes flag, string label, string tooltip)
        {
            var tmp = _config.ShowNodes.HasFlag(flag);
            if (ImGui.Checkbox(label, ref tmp))
            {
                if (tmp)
                    _config.ShowNodes |= flag;
                else
                    _config.ShowNodes &= ~flag;
                _nodeTabCache.Rebuild();
                Save();
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(tooltip);
        }

        private void DrawColorPicker(string label, string tooltip, Vector4 current, Vector4 defaultValue, Action<Vector4> setter)
        {
            const ImGuiColorEditFlags flags = ImGuiColorEditFlags.Float | ImGuiColorEditFlags.AlphaPreviewHalf | ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.NoLabel;

            var                       tmp   = current;
            if (ImGui.ColorEdit4(label, ref tmp, flags) && tmp != current)
            {
                setter(tmp);
                Save();
            }

            ImGui.SameLine();
            if (ImGui.Button($"Default##{label}") && current != defaultValue)
            {
                setter(defaultValue);
                Save();
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip($"Reset the color to its default value #{ImGui.ColorConvertFloat4ToU32(defaultValue):X8}.");

            ImGui.SameLine();
            ImGui.Text(label);
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(tooltip);
        }

        private static void ClippedDraw<T>(IList<T> data, Action<T> func, Func<bool> pre, Action post)
        {
            var windowHeight = ImGui.GetWindowHeight();
            var scrollY      = ImGui.GetScrollY();
            var lines        = scrollY / _textHeight;

            var minY = Math.Max((int) Math.Floor(lines), 2) - 2;
            var maxY = (int) Math.Ceiling(windowHeight / _textHeight) + 2;

            if (scrollY != 0)
                ImGui.Dummy(new Vector2(-1, _textHeight * (float) Math.Floor(lines - 2)));

            if (!pre())
                return;

            var lastIdx = Math.Min(minY + maxY, data.Count);
            for (var idx = minY; idx < lastIdx; ++idx)
            {
                var datum = data[idx];
                func(datum);
            }

            post();

            if (scrollY <= ImGui.GetScrollMaxY() - _textHeight * 2)
                ImGui.Dummy(new Vector2(-1, _textHeight * (data.Count - lastIdx)));
        }

        private static void DrawComboWithFilter(string label, IList<string> options, ref int currentIdx, ref string filter, ref bool focus,
            float size, int items)
        {
            if (ImGui.BeginCombo(label, options[currentIdx]))
            {
                try
                {
                    ImGui.SetNextItemWidth(-1);
                    ImGui.InputTextWithHint($"{label}_filter", "Filter", ref filter, 255);
                    var isFocused = ImGui.IsItemActive();
                    if (!focus)
                        ImGui.SetKeyboardFocusHere();

                    using var child = new ImGuiRaii();

                    if (!child.Begin(() => ImGui.BeginChild($"{label}_list", new Vector2(size, items * ImGui.GetTextLineHeightWithSpacing())), ImGui.EndChild))
                        return;

                    if (!focus)
                    {
                        ImGui.SetScrollY(0);
                        focus = true;
                    }

                    var filterLower = filter.ToLowerInvariant();
                    var numItems    = 0;
                    var node        = 0;
                    for (var i = 0; i < options.Count; ++i)
                    {
                        if (!options[i].ToLowerInvariant().Contains(filterLower))
                            continue;

                        ++numItems;
                        node = i;
                        if (!ImGui.Selectable(options[i], i == currentIdx))
                            continue;

                        currentIdx = i;
                        ImGui.CloseCurrentPopup();
                    }

                    child.End();
                    if (!isFocused && numItems <= 1)
                    {
                        currentIdx = node;
                        ImGui.CloseCurrentPopup();
                    }
                }
                finally
                {
                    ImGui.EndCombo();
                }
            }
            else
            {
                focus  = false;
                filter = "";
            }
        }
    }
}
