using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using ECommons;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Alarms;
using GatherBuddy.Config;
using GatherBuddy.GatherHelper;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using ImGuiNET;
using OtterGui;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private class GatherWindowDragDropData
    {
        public GatherWindowPreset Preset;
        public IGatherable        Item;
        public int                ItemIdx;

        public GatherWindowDragDropData(GatherWindowPreset preset, IGatherable item, int idx)
        {
            Preset  = preset;
            Item    = item;
            ItemIdx = idx;
        }
    }

    private class GatherWindowCache
    {
        public class GatherWindowSelector : ItemSelector<GatherWindowPreset>
        {
            public GatherWindowSelector()
                : base(_plugin.GatherWindowManager.Presets, Flags.All)
            { }

            protected override bool Filtered(int idx)
                => Filter.Length != 0 && !Items[idx].Name.Contains(Filter, StringComparison.InvariantCultureIgnoreCase);

            protected override bool OnDraw(int idx)
            {
                using var id    = ImRaii.PushId(idx);
                using var color = ImRaii.PushColor(ImGuiCol.Text, ColorId.DisabledText.Value(), !Items[idx].Enabled);
                return ImGui.Selectable(CheckUnnamed(Items[idx].Name), idx == CurrentIdx);
            }

            protected override bool OnDelete(int idx)
            {
                _plugin.GatherWindowManager.DeletePreset(idx);
                return true;
            }

            protected override bool OnAdd(string name)
            {
                _plugin.GatherWindowManager.AddPreset(new GatherWindowPreset()
                {
                    Name = name,
                });
                return true;
            }

            protected override bool OnClipboardImport(string name, string data)
            {
                if (!GatherWindowPreset.Config.FromBase64(data, out var cfg))
                    return false;

                GatherWindowPreset.FromConfig(cfg, out var preset);
                preset.Name = name;
                _plugin.GatherWindowManager.AddPreset(preset);
                return true;
            }

            protected override bool OnDuplicate(string name, int idx)
            {
                if (Items.Count <= idx || idx < 0)
                    return false;

                var preset = _plugin.GatherWindowManager.Presets[idx].Clone();
                preset.Name = name;
                _plugin.GatherWindowManager.AddPreset(preset);
                return true;
            }

            protected override void OnDrop(object? data, int idx)
            {
                if (Items.Count <= idx || idx < 0)
                    return;
                if (data is not GatherWindowDragDropData obj)
                    return;

                var preset = _plugin.GatherWindowManager.Presets[idx];
                _plugin.GatherWindowManager.RemoveItem(obj.Preset, obj.ItemIdx);
                _plugin.GatherWindowManager.AddItem(preset, obj.Item);
            }


            protected override bool OnMove(int idx1, int idx2)
            {
                _plugin.GatherWindowManager.MovePreset(idx1, idx2);
                return true;
            }
        }

        public readonly GatherWindowSelector Selector = new();

        public int  NewGatherableIdx;
        public bool EditName;
        public bool EditDesc;
    }

    private readonly GatherWindowCache _gatherWindowCache;

    private void DrawGatherWindowPresetHeaderLine()
    {
        if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Copy.ToIconString(), IconButtonSize, "Copy current gather window preset to clipboard.",
                _gatherWindowCache.Selector.Current == null, true))
        {
            var preset = _gatherWindowCache.Selector.Current!;
            try
            {
                var s = new GatherWindowPreset.Config(preset).ToBase64();
                ImGui.SetClipboardText(s);
                Communicator.PrintClipboardMessage("Gather window preset ", preset.Name);
            }
            catch (Exception e)
            {
                Communicator.PrintClipboardMessage("Gather window preset ", preset.Name, e);
            }
        }

        if (ImGuiUtil.DrawDisabledButton("Create Alarms", Vector2.Zero, "Create a new Alarm Group from this gather window preset.", _gatherWindowCache.Selector.Current == null))
        {
            var preset = new AlarmGroup(_gatherWindowCache.Selector.Current!);
            _plugin.AlarmManager.AddGroup(preset);
        }

        if (ImGuiUtil.DrawDisabledButton("Import from TeamCraft", Vector2.Zero, "Populate list from clipboard contents (TeamCraft format)",
                _gatherWindowCache.Selector.Current == null))
        {
            var clipboardText = ImGuiUtil.GetClipboardText();
            if (!string.IsNullOrEmpty(clipboardText))
            {
                try
                {
                    Dictionary<string, int> items = new Dictionary<string, int>();

                    // Regex pattern
                    var pattern = @"\b(\d+)x\s(.+)\b";
                    var matches = Regex.Matches(clipboardText, pattern);

                    // Loop through matches and add them to dictionary
                    foreach (Match match in matches)
                    {
                        var quantity = int.Parse(match.Groups[1].Value);
                        var itemName = match.Groups[2].Value;
                        items[itemName] = quantity;
                    }
                    
                    var preset = _gatherWindowCache.Selector.Current;

                    foreach (var item in items)
                    {
                        var gatherable =
                            GatherBuddy.GameData.Gatherables.Values.FirstOrDefault(g => g.Name[Dalamud.ClientState.ClientLanguage] == item.Key);
                        if (gatherable == null || gatherable.NodeList.Count == 0)
                            continue;

                        if (preset.Quantities.TryGetValue(gatherable.ItemId, out var quantity))
                        {
                            preset.Quantities[gatherable.ItemId] += (uint)item.Value;
                        }
                        else
                        {
                            preset.Quantities.Add(gatherable.ItemId, (uint)item.Value);
                        }
                        preset.Add(gatherable);
                    }
                    
                    if (preset.Enabled)
                        _plugin.GatherWindowManager.SetActiveItems();
                }
                catch (Exception e)
                {
                    Communicator.PrintClipboardMessage("Error importing gather window preset", e.ToString());
                }
            }
        }

        ImGuiComponents.HelpMarker(
            "If not sorting the Gather Window by uptimes, items are uniquely added in order of enabled preset, then order of item in preset.\n"
          + "You can drag and draw presets in the list to move them.\n"
          + "You can drag and draw items in a specific preset to move them.\n"
          + "You can drag and draw an item onto a different preset from the selector to add it to that preset and remove it from the current.\n"
          + "In the Gather Window itself, you can hold Control and Right-Click an item to delete it from the preset it comes from. If this removes the last item in a preset, the preset will also be removed.");
    }

    private void DrawGatherWindowPreset(GatherWindowPreset preset)
    {
        if (ImGuiUtil.DrawEditButtonText(0, _gatherWindowCache.EditName ? preset.Name : CheckUnnamed(preset.Name), out var newName,
                ref _gatherWindowCache.EditName, IconButtonSize, SetInputWidth, 64))
            _plugin.GatherWindowManager.ChangeName(preset, newName);
        if (ImGuiUtil.DrawEditButtonText(1, _gatherWindowCache.EditDesc ? preset.Description : CheckUndescribed(preset.Description),
                out var newDesc, ref _gatherWindowCache.EditDesc, IconButtonSize, 2 * SetInputWidth, 128))
            _plugin.GatherWindowManager.ChangeDescription(preset, newDesc);

        var tmp = preset.Enabled;
        if (ImGui.Checkbox("Enabled##preset", ref tmp) && tmp != preset.Enabled)
            _plugin.GatherWindowManager.TogglePreset(preset);

        ImGui.NewLine();
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() - ImGui.GetStyle().ItemInnerSpacing.X);
        using var box = ImRaii.ListBox("##gatherWindowList", new Vector2(-1.5f * ImGui.GetStyle().ItemSpacing.X, -1));
        if (!box)
            return;

        for (var i = 0; i < preset.Items.Count; ++i)
        {
            using var id    = ImRaii.PushId(i);
            using var group = ImRaii.Group();
            var       item  = preset.Items[i];
            if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Trash.ToIconString(), IconButtonSize, "Delete this item from the preset...", false,
                    true))
                _plugin.GatherWindowManager.RemoveItem(preset, i--);

            ImGui.SameLine();
            if (_gatherGroupCache.GatherableSelector.Draw(item.Name[GatherBuddy.Language], out var newIdx))
                _plugin.GatherWindowManager.ChangeItem(preset, GatherGroupCache.AllGatherables[newIdx], i);
            ImGui.SameLine();
            ImGui.Text("Inventory: ");
            var invTotal = _plugin.GatherWindowManager.GetInventoryCountForItem(item);
            ImGui.SameLine(0f, ImGui.CalcTextSize($"0000 / ").X - ImGui.CalcTextSize($"{invTotal} / ").X);
            ImGui.Text($"{invTotal} / ");
            ImGui.SameLine(0, 3f);
            int quantity = preset.Quantities.TryGetValue(item.ItemId, out var q) ? (int)q : 1;
            ImGui.SetNextItemWidth(100f);
            if (ImGui.InputInt("##quantity", ref quantity, 1, 10))
                _plugin.GatherWindowManager.ChangeQuantity(preset, (uint)quantity, item.ItemId);
            ImGui.SameLine();
            if (DrawLocationInput(item, preset.PreferredLocations.TryGetValue(item.ItemId, out var locId) ? GatherBuddy.GameData.GatheringNodes.GetValueOrDefault(locId) : null, out var newLoc))
                _plugin.GatherWindowManager.ChangePreferredLocation(preset, item, newLoc);
            group.Dispose();

            _gatherWindowCache.Selector.CreateDropSource(new GatherWindowDragDropData(preset, item, i), item.Name[GatherBuddy.Language]);

            var localIdx = i;
            _gatherWindowCache.Selector.CreateDropTarget<GatherWindowDragDropData>(d
                => _plugin.GatherWindowManager.MoveItem(d.Preset, d.ItemIdx, localIdx));
        }

        if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Plus.ToIconString(), IconButtonSize,
                "Add this item at the end of the preset, if it is not already included...",
                preset.Items.Contains(GatherGroupCache.AllGatherables[_gatherWindowCache.NewGatherableIdx]), true))
            _plugin.GatherWindowManager.AddItem(preset, GatherGroupCache.AllGatherables[_gatherWindowCache.NewGatherableIdx]);

        ImGui.SameLine();
        if (_gatherGroupCache.GatherableSelector.Draw(_gatherWindowCache.NewGatherableIdx, out var idx))
            _gatherWindowCache.NewGatherableIdx = idx;
    }

    private void DrawGatherWindowTab()
    {
        using var id  = ImRaii.PushId("GatherWindow");
        using var tab = ImRaii.TabItem("Auto-Gather");

        ImGuiUtil.HoverTooltip(
            "You read that right! Auto-gather!");

        if (!tab)
            return;

        AutoGather.AutoGatherUI.DrawAutoGatherStatus();

        _gatherWindowCache.Selector.Draw(SelectorWidth);
        ImGui.SameLine();

        ItemDetailsWindow.Draw("Preset Details", DrawGatherWindowPresetHeaderLine, () =>
        {
            if (_gatherWindowCache.Selector.Current != null)
                DrawGatherWindowPreset(_gatherWindowCache.Selector.Current);
        });
    }
}
