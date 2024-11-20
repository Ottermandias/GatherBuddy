using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using GatherBuddy.Alarms;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.CustomInfo;
using GatherBuddy.GatherHelper;
using GatherBuddy.Plugin;
using ImGuiNET;
using OtterGui;
using OtterGui.Widgets;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private class GatherWindowDragDropData
    {
        public GatherWindowPreset Preset;
        public Gatherable         Item;
        public int                ItemIdx;

        public GatherWindowDragDropData(GatherWindowPreset preset, Gatherable item, int idx)
        {
            Preset  = preset;
            Item    = item;
            ItemIdx = idx;
        }
    }

    private class GatherWindowCache : IDisposable
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

        public GatherWindowCache()
        {
            UpdateGatherables();
            WorldData.WorldLocationsChanged += UpdateGatherables;
        }

        public readonly GatherWindowSelector Selector = new();

        public ReadOnlyCollection<Gatherable> AllGatherables { get; private set; }
        public ReadOnlyCollection<Gatherable> FilteredGatherables { get; private set; }
        public ClippedSelectableCombo<Gatherable> GatherableSelector { get; private set; }
        private HashSet<Gatherable> ExcludedGatherables = [];

        public void SetExcludedGatherbales(IEnumerable<Gatherable> exclude)
        {
            var excludeSet = exclude.ToHashSet();
            if (!ExcludedGatherables.SetEquals(excludeSet))
            {
                var newGatherables = AllGatherables.Except(excludeSet).ToList().AsReadOnly();
                UpdateGatherables(newGatherables, excludeSet);
            }
        }

        private static ReadOnlyCollection<Gatherable> GenAllGatherables()
            => GatherBuddy.GameData.Gatherables.Values
            .Where(g => g.NodeList.SelectMany(l => l.WorldPositions.Values).SelectMany(p => p).Any())
            .OrderBy(g => g.Name[GatherBuddy.Language])
            .ToArray()
            .AsReadOnly();

        [MemberNotNull(nameof(FilteredGatherables)), MemberNotNull(nameof(GatherableSelector)), MemberNotNull(nameof(AllGatherables))]
        private void UpdateGatherables() => UpdateGatherables(AllGatherables = GenAllGatherables(), []);

        [MemberNotNull(nameof(FilteredGatherables)), MemberNotNull(nameof(GatherableSelector))]
        private void UpdateGatherables(ReadOnlyCollection<Gatherable> newGatherables, HashSet<Gatherable> newExcluded)
        {
            while (NewGatherableIdx > 0)
            {
                var item = FilteredGatherables![NewGatherableIdx];
                var idx = newGatherables.IndexOf(item);
                if (idx < 0)
                    NewGatherableIdx--;
                else
                {
                    NewGatherableIdx = idx;
                    break;
                }
            }
            FilteredGatherables = newGatherables;
            ExcludedGatherables = newExcluded;
            GatherableSelector = new("GatherablesSelector", string.Empty, 250, FilteredGatherables, g => g.Name[GatherBuddy.Language]);
        }

        public void Dispose()
        {
            WorldData.WorldLocationsChanged -= UpdateGatherables;
        }

        public int  NewGatherableIdx;
        public bool EditName;
        public bool EditDesc;
    }

    private readonly GatherWindowCache _gatherWindowCache;
    public GatherWindowPreset? CurrentGatherWindowPreset => _gatherWindowCache.Selector.EnsureCurrent();

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
                    
                    var preset = _gatherWindowCache.Selector.Current!;

                    foreach (var (itemName, quantity) in items)
                    {
                        var gatherable =
                            GatherBuddy.GameData.Gatherables.Values.FirstOrDefault(g => g.Name[Dalamud.ClientState.ClientLanguage] == itemName);
                        if (gatherable == null || gatherable.NodeList.Count == 0)
                            continue;

                        preset.Add(gatherable, (uint)quantity);
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
            "If the config option to sort by location is not selected, items are gathered in order of enabled preset, then order of item in preset.\n"
          + "You can drag and draw presets in the list to move them.\n"
          + "You can drag and draw items in a specific preset to move them.\n"
          + "You can drag and draw an item onto a different preset from the selector to add it to that preset and remove it from the current.\n"
          + "In the Gather Window, you can hold Control and Right-Click an item to delete it from the preset it comes from.");
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

        ImGui.SameLine();
        ImGuiUtil.Checkbox("Fallback##preset",
            "Items from fallback presets won't be auto-gathered.\n"
          + "But if a node doesn't contain any items from regular presets or if you gathered enough of them,\n"
          + "items from fallback presets would be gathered instead if they could be found in that node.", 
            preset.Fallback, (v) => _plugin.GatherWindowManager.SetFallback(preset, v));

        ImGui.NewLine();
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() - ImGui.GetStyle().ItemInnerSpacing.X);
        using var box = ImRaii.ListBox("##gatherWindowList", new Vector2(-1.5f * ImGui.GetStyle().ItemSpacing.X, -1));
        if (!box)
            return;

        _gatherWindowCache.SetExcludedGatherbales(preset.Items.OfType<Gatherable>());
        var gatherables = _gatherWindowCache.FilteredGatherables;
        var selector = _gatherWindowCache.GatherableSelector;
        int changeIndex = -1, changeItemIndex = -1, deleteIndex = -1;

        for (var i = 0; i < preset.Items.Count; ++i)
        {
            var       item  = preset.Items[i];
            using var id    = ImRaii.PushId((int)item.ItemId);
            using var group = ImRaii.Group();
            if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Trash.ToIconString(), IconButtonSize, "Delete this item from the preset", false, true))
                deleteIndex = i;

            ImGui.SameLine();
            if (selector.Draw(item.Name[GatherBuddy.Language], out var newIdx))
            {
                changeIndex = i;
                changeItemIndex = newIdx;
            }
            ImGui.SameLine();
            ImGui.Text("Inventory: ");
            var invTotal = _plugin.GatherWindowManager.GetInventoryCountForItem(item);
            ImGui.SameLine(0f, ImGui.CalcTextSize($"0000 / ").X - ImGui.CalcTextSize($"{invTotal} / ").X);
            ImGui.Text($"{invTotal} / ");
            ImGui.SameLine(0, 3f);
            var quantity = preset.Quantities.TryGetValue(item, out var q) ? (int)q : 1;
            ImGui.SetNextItemWidth(100f);
            if (ImGui.InputInt("##quantity", ref quantity, 1, 10))
                _plugin.GatherWindowManager.ChangeQuantity(preset, item, (uint)quantity);
            ImGui.SameLine();
            if (DrawLocationInput(item, preset.PreferredLocations.GetValueOrDefault(item), out var newLoc))
                _plugin.GatherWindowManager.ChangePreferredLocation(preset, item, newLoc as GatheringNode);
            group.Dispose();

            _gatherWindowCache.Selector.CreateDropSource(new GatherWindowDragDropData(preset, item, i), item.Name[GatherBuddy.Language]);

            var localIdx = i;
            _gatherWindowCache.Selector.CreateDropTarget<GatherWindowDragDropData>(d
                => _plugin.GatherWindowManager.MoveItem(d.Preset, d.ItemIdx, localIdx));
        }

        if (deleteIndex >= 0)
            _plugin.GatherWindowManager.RemoveItem(preset, deleteIndex);

        if (changeIndex >= 0)
            _plugin.GatherWindowManager.ChangeItem(preset, gatherables[changeItemIndex], changeIndex);

        if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Plus.ToIconString(), IconButtonSize, "Add this item at the end of the preset", false, true))
            _plugin.GatherWindowManager.AddItem(preset, gatherables[_gatherWindowCache.NewGatherableIdx]);

        ImGui.SameLine();
        if (selector.Draw(_gatherWindowCache.NewGatherableIdx, out var idx))
        {
            _gatherWindowCache.NewGatherableIdx = idx;
            _plugin.GatherWindowManager.AddItem(preset, gatherables[_gatherWindowCache.NewGatherableIdx]);
        }
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
