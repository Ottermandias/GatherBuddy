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
using GatherBuddy.AutoGather.Lists;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.CustomInfo;
using GatherBuddy.Plugin;
using ImGuiNET;
using OtterGui;
using OtterGui.Widgets;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private class AutoGatherListsDragDropData
    {
        public AutoGatherList     list;
        public Gatherable         Item;
        public int                ItemIdx;

        public AutoGatherListsDragDropData(AutoGatherList list, Gatherable item, int idx)
        {
            this.list  = list;
            Item    = item;
            ItemIdx = idx;
        }
    }

    private class AutoGatherListsCache : IDisposable
    {
        public class AutoGatherListSelector : ItemSelector<AutoGatherList>
        {
            public AutoGatherListSelector()
                : base(_plugin.AutoGatherListsManager.Lists, Flags.All)
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
                _plugin.AutoGatherListsManager.DeleteList(idx);
                return true;
            }

            protected override bool OnAdd(string name)
            {
                _plugin.AutoGatherListsManager.AddList(new AutoGatherList()
                {
                    Name = name,
                });
                return true;
            }

            protected override bool OnClipboardImport(string name, string data)
            {
                if (!AutoGatherList.Config.FromBase64(data, out var cfg))
                    return false;

                AutoGatherList.FromConfig(cfg, out var list);
                list.Name = name;
                _plugin.AutoGatherListsManager.AddList(list);
                return true;
            }

            protected override bool OnDuplicate(string name, int idx)
            {
                if (Items.Count <= idx || idx < 0)
                    return false;

                var list = _plugin.AutoGatherListsManager.Lists[idx].Clone();
                list.Name = name;
                _plugin.AutoGatherListsManager.AddList(list);
                return true;
            }

            protected override void OnDrop(object? data, int idx)
            {
                if (Items.Count <= idx || idx < 0)
                    return;
                if (data is not AutoGatherListsDragDropData obj)
                    return;

                var list = _plugin.AutoGatherListsManager.Lists[idx];
                _plugin.AutoGatherListsManager.RemoveItem(obj.list, obj.ItemIdx);
                _plugin.AutoGatherListsManager.AddItem(list, obj.Item);
            }


            protected override bool OnMove(int idx1, int idx2)
            {
                _plugin.AutoGatherListsManager.MoveList(idx1, idx2);
                return true;
            }
        }

        public AutoGatherListsCache()
        {
            UpdateGatherables();
            WorldData.WorldLocationsChanged += UpdateGatherables;
        }

        public readonly AutoGatherListSelector Selector = new();

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

    private readonly AutoGatherListsCache _autoGatherListsCache;
    public AutoGatherList? CurrentAutoGatherList => _autoGatherListsCache.Selector.EnsureCurrent();

    private void DrawAutoGatherListsLine()
    {
        if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Copy.ToIconString(), IconButtonSize, "Copy current auto-gather list to clipboard.",
                _autoGatherListsCache.Selector.Current == null, true))
        {
            var list = _autoGatherListsCache.Selector.Current!;
            try
            {
                var s = new AutoGatherList.Config(list).ToBase64();
                ImGui.SetClipboardText(s);
                Communicator.PrintClipboardMessage("Auto-gather list ", list.Name);
            }
            catch (Exception e)
            {
                Communicator.PrintClipboardMessage("Auto-gather list ", list.Name, e);
            }
        }

        if (ImGuiUtil.DrawDisabledButton("Import from TeamCraft", Vector2.Zero, "Populate list from clipboard contents (TeamCraft format)",
                _autoGatherListsCache.Selector.Current == null))
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
                    
                    var list = _autoGatherListsCache.Selector.Current!;

                    foreach (var (itemName, quantity) in items)
                    {
                        var gatherable =
                            GatherBuddy.GameData.Gatherables.Values.FirstOrDefault(g => g.Name[Dalamud.ClientState.ClientLanguage] == itemName);
                        if (gatherable == null || gatherable.NodeList.Count == 0)
                            continue;

                        list.Add(gatherable, (uint)quantity);
                    }

                    _plugin.AutoGatherListsManager.Save();

                    if (list.Enabled)
                        _plugin.AutoGatherListsManager.SetActiveItems();
                }
                catch (Exception e)
                {
                    Communicator.PrintClipboardMessage("Error importing auto-gather list", e.ToString());
                }
            }
        }

        ImGuiComponents.HelpMarker(
            "If the config option to sort by location is not selected, items are gathered in order of enabled list, then order of item in list.\n"
          + "You can drag and draw lists to move them.\n"
          + "You can drag and draw items in a specific list to move them.\n"
          + "You can drag and draw an item onto a different list from the selector to add it to that list and remove it from the current.\n"
          + "In the Gather Window, you can hold Control and Right-Click an item to delete it from the list it comes from.");
    }

    private void DrawAutoGatherList(AutoGatherList list)
    {
        if (ImGuiUtil.DrawEditButtonText(0, _autoGatherListsCache.EditName ? list.Name : CheckUnnamed(list.Name), out var newName,
                ref _autoGatherListsCache.EditName, IconButtonSize, SetInputWidth, 64))
            _plugin.AutoGatherListsManager.ChangeName(list, newName);
        if (ImGuiUtil.DrawEditButtonText(1, _autoGatherListsCache.EditDesc ? list.Description : CheckUndescribed(list.Description),
                out var newDesc, ref _autoGatherListsCache.EditDesc, IconButtonSize, 2 * SetInputWidth, 128))
            _plugin.AutoGatherListsManager.ChangeDescription(list, newDesc);

        var tmp = list.Enabled;
        if (ImGui.Checkbox("Enabled##list", ref tmp) && tmp != list.Enabled)
            _plugin.AutoGatherListsManager.ToggleList(list);

        ImGui.SameLine();
        ImGuiUtil.Checkbox("Fallback##list",
            "Items from fallback lists won't be auto-gathered.\n"
          + "But if a node doesn't contain any items from regular lists or if you gathered enough of them,\n"
          + "items from fallback lists would be gathered instead if they could be found in that node.", 
            list.Fallback, (v) => _plugin.AutoGatherListsManager.SetFallback(list, v));

        ImGui.NewLine();
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() - ImGui.GetStyle().ItemInnerSpacing.X);
        using var box = ImRaii.ListBox("##gatherWindowList", new Vector2(-1.5f * ImGui.GetStyle().ItemSpacing.X, -1));
        if (!box)
            return;

        _autoGatherListsCache.SetExcludedGatherbales(list.Items.OfType<Gatherable>());
        var gatherables = _autoGatherListsCache.FilteredGatherables;
        var selector = _autoGatherListsCache.GatherableSelector;
        int changeIndex = -1, changeItemIndex = -1, deleteIndex = -1;

        for (var i = 0; i < list.Items.Count; ++i)
        {
            var       item  = list.Items[i];
            using var id    = ImRaii.PushId((int)item.ItemId);
            using var group = ImRaii.Group();
            if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Trash.ToIconString(), IconButtonSize, "Delete this item from the list", false, true))
                deleteIndex = i;

            ImGui.SameLine();
            if (selector.Draw(item.Name[GatherBuddy.Language], out var newIdx))
            {
                changeIndex = i;
                changeItemIndex = newIdx;
            }
            ImGui.SameLine();
            ImGui.Text("Inventory: ");
            var invTotal = _plugin.AutoGatherListsManager.GetInventoryCountForItem(item);
            ImGui.SameLine(0f, ImGui.CalcTextSize($"0000 / ").X - ImGui.CalcTextSize($"{invTotal} / ").X);
            ImGui.Text($"{invTotal} / ");
            ImGui.SameLine(0, 3f);
            var quantity = list.Quantities.TryGetValue(item, out var q) ? (int)q : 1;
            ImGui.SetNextItemWidth(100f);
            if (ImGui.InputInt("##quantity", ref quantity, 1, 10))
                _plugin.AutoGatherListsManager.ChangeQuantity(list, item, (uint)quantity);
            ImGui.SameLine();
            if (DrawLocationInput(item, list.PreferredLocations.GetValueOrDefault(item), out var newLoc))
                _plugin.AutoGatherListsManager.ChangePreferredLocation(list, item, newLoc as GatheringNode);
            group.Dispose();

            _autoGatherListsCache.Selector.CreateDropSource(new AutoGatherListsDragDropData(list, item, i), item.Name[GatherBuddy.Language]);

            var localIdx = i;
            _autoGatherListsCache.Selector.CreateDropTarget<AutoGatherListsDragDropData>(d
                => _plugin.AutoGatherListsManager.MoveItem(d.list, d.ItemIdx, localIdx));
        }

        if (deleteIndex >= 0)
            _plugin.AutoGatherListsManager.RemoveItem(list, deleteIndex);

        if (changeIndex >= 0)
            _plugin.AutoGatherListsManager.ChangeItem(list, gatherables[changeItemIndex], changeIndex);

        if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Plus.ToIconString(), IconButtonSize, "Add this item at the end of the list", false, true))
            _plugin.AutoGatherListsManager.AddItem(list, gatherables[_autoGatherListsCache.NewGatherableIdx]);

        ImGui.SameLine();
        if (selector.Draw(_autoGatherListsCache.NewGatherableIdx, out var idx))
        {
            _autoGatherListsCache.NewGatherableIdx = idx;
            _plugin.AutoGatherListsManager.AddItem(list, gatherables[_autoGatherListsCache.NewGatherableIdx]);
        }
    }

    private void DrawAutoGatherTab()
    {
        using var id  = ImRaii.PushId("AutoGatherLists");
        using var tab = ImRaii.TabItem("Auto-Gather");

        ImGuiUtil.HoverTooltip(
            "You read that right! Auto-gather!");

        if (!tab)
            return;

        AutoGather.AutoGatherUI.DrawAutoGatherStatus();

        _autoGatherListsCache.Selector.Draw(SelectorWidth);
        ImGui.SameLine();

        ItemDetailsWindow.Draw("List Details", DrawAutoGatherListsLine, () =>
        {
            if (_autoGatherListsCache.Selector.Current != null)
                DrawAutoGatherList(_autoGatherListsCache.Selector.Current);
        });
    }
}
