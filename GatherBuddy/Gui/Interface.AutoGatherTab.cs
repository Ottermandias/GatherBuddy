using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Dalamud.Interface;
using GatherBuddy.AutoGather.Extensions;
using GatherBuddy.AutoGather.Lists;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.CustomInfo;
using GatherBuddy.Plugin;
using Dalamud.Bindings.ImGui;
using OtterGui;
using OtterGui.Widgets;
using ImRaii = OtterGui.Raii.ImRaii;
using ECommons.ImGuiMethods;
using ECommons;
using GatherBuddy.Interfaces;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private class AutoGatherListsDragDropData
    {
        public AutoGatherList list;
        public IGatherable    Item;
        public int            ItemIdx;

        public AutoGatherListsDragDropData(AutoGatherList list, IGatherable item, int idx)
        {
            this.list = list;
            Item      = item;
            ItemIdx   = idx;
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
                var list = new AutoGatherList()
                {
                    Name = name,
                };
                _plugin.AutoGatherListsManager.AddList(list);
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

        public  ReadOnlyCollection<IGatherable>     AllGatherables      { get; private set; }
        public  ReadOnlyCollection<IGatherable>     FilteredGatherables { get; private set; }
        public  ClippedSelectableCombo<IGatherable> GatherableSelector  { get; private set; }
        private HashSet<IGatherable>                ExcludedGatherables = [];

        public void SetExcludedGatherbales(IEnumerable<IGatherable> exclude)
        {
            var excludeSet = exclude.ToHashSet();
            if (!ExcludedGatherables.SetEquals(excludeSet))
            {
                var newGatherables = AllGatherables.Except(excludeSet).ToList().AsReadOnly();
                UpdateGatherables(newGatherables, excludeSet);
            }
        }

        private static ReadOnlyCollection<IGatherable> GenAllGatherables()
        {
            var all = GatherBuddy.GameData.Gatherables.Values
                .Where(g => g.NodeList.SelectMany(l => l.WorldPositions.Values)
                    .SelectMany(p => p).Any())
                .Cast<IGatherable>()
                .Concat(GatherBuddy.GameData.Fishes.Values)
                .GroupBy(g => g.ItemId)
                .Select(g => g.First())
                .OrderBy(g => g.Name[GatherBuddy.Language])
                .ToList()
                .AsReadOnly();
            return all;
        }


        [MemberNotNull(nameof(FilteredGatherables)), MemberNotNull(nameof(GatherableSelector)), MemberNotNull(nameof(AllGatherables))]
        private void UpdateGatherables()
            => UpdateGatherables(AllGatherables = GenAllGatherables(), []);

        [MemberNotNull(nameof(FilteredGatherables)), MemberNotNull(nameof(GatherableSelector))]
        private void UpdateGatherables(ReadOnlyCollection<IGatherable> newGatherables, HashSet<IGatherable> newExcluded)
        {
            while (NewGatherableIdx > 0)
            {
                var item = FilteredGatherables![NewGatherableIdx];
                var idx  = newGatherables.IndexOf(item);
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
            GatherableSelector  = new("GatherablesSelector", string.Empty, 250, FilteredGatherables, g => g.Name[GatherBuddy.Language]);
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

    public AutoGatherList? CurrentAutoGatherList
        => _autoGatherListsCache.Selector.EnsureCurrent();

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

        if (GatherBuddy.AutoGather.ArtisanExporter.ArtisanAssemblyEnabled)
        {
            if (ImGuiUtil.DrawDisabledButton("Import From Artisan", Vector2.Zero,
                    "Import your lists from Artisan into GBR\nBrings up a dropdown to select which list to import.\nA new list will be created in GBR when you click on the name of the list in the dropdown.",
                    !GatherBuddy.AutoGather.ArtisanExporter.ArtisanAssemblyEnabled))
            {
                ImGui.OpenPopup($"artisanImport");
            }

            if (ImGui.BeginPopup($"artisanImport"))
            {
                var lists = GatherBuddy.AutoGather.ArtisanExporter.GetArtisanListNames();

                float rowHeight       = ImGui.GetTextLineHeightWithSpacing();
                float totalListHeight = lists.Count * rowHeight;
                float totalListWidth  = lists.Max(n => ImGui.CalcTextSize(n.Value).X) + 40;

                float maxHeight   = ImGui.GetIO().DisplaySize.Y * 0.4f;
                float childHeight = Math.Min(totalListHeight, maxHeight);

                if (ImGui.BeginChild("ArtisanListsChild", new Vector2(totalListWidth, childHeight), true))
                {
                    foreach (var kvp in lists)
                    {
                        if (ImGui.Selectable($"{kvp.Value}##{kvp.Key}"))
                        {
                            Communicator.Print($"Importing '{kvp.Value}' from Artisan...");
                            GatherBuddy.AutoGather.ArtisanExporter.StartArtisanImport(kvp);
                        }

                        ImGuiUtil.HoverTooltip($"{kvp.Value} ({kvp.Key})\n(Click to import to new auto-gather list)");
                    }
                }

                ImGui.EndChild();
                ImGui.EndPopup();
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

        ImGui.SetCursorPosX(ImGui.GetWindowSize().X - 50);
        string agHelpText =
            "If the config option to sort by location is not selected, items are gathered in order of enabled list, then order of item in list.\n"
          + "You can drag and draw lists to move them.\n"
          + "You can drag and draw items in a specific list to move them.\n"
          + "You can drag and draw an item onto a different list from the selector to add it to that list and remove it from the current.\n"
          + "In the Gather Window, you can hold Control and Right-Click an item to delete it from the list it comes from.";

        ImGuiEx.InfoMarker(agHelpText,                    null, FontAwesomeIcon.InfoCircle.ToIconString(), false);
        ImGuiEx.InfoMarker("Auto-Gather Support Discord", null, FontAwesomeIcon.Comments.ToIconString(),   false);
        if (ImGuiEx.HoveredAndClicked())
        {
            GenericHelpers.ShellStart("https://discord.gg/p54TZMPnC9");
        }
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
        var selector    = _autoGatherListsCache.GatherableSelector;
        int changeIndex = -1, changeItemIndex = -1, deleteIndex = -1;

        for (var i = 0; i < list.Items.Count; ++i)
        {
            var       item  = list.Items[i];
            using var id    = ImRaii.PushId((int)item.ItemId);
            using var group = ImRaii.Group();
            if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Trash.ToIconString(), IconButtonSize, "Delete this item from the list", false,
                    true))
                deleteIndex = i;
            ImGui.SameLine();

            var enabled = list.EnabledItems[item];
            if (ImGui.Checkbox($"##{item.ItemId}", ref enabled))
                _plugin.AutoGatherListsManager.ChangeEnabled(list, item, enabled);

            ImGui.SameLine();
            if (selector.Draw(item.Name[GatherBuddy.Language], out var newIdx))
            {
                changeIndex     = i;
                changeItemIndex = newIdx;
            }

            ImGui.SameLine();
            ImGui.Text("Inventory: ");
            var invTotal = item.GetInventoryCount();
            ImGui.SameLine(0f, ImGui.CalcTextSize($"0000 / ").X - ImGui.CalcTextSize($"{invTotal} / ").X);
            ImGui.Text($"{invTotal} / ");
            ImGui.SameLine(0, 3f);
            var quantity = list.Quantities.TryGetValue(item, out var q) ? (int)q : 1;
            ImGui.SetNextItemWidth(100f * Scale);
            if (ImGui.InputInt("##quantity", ref quantity, 1, 10))
                _plugin.AutoGatherListsManager.ChangeQuantity(list, item, (uint)quantity);
            ImGui.SameLine();
            if (DrawLocationInput(item, list.PreferredLocations.GetValueOrDefault(item), out var newLoc))
                _plugin.AutoGatherListsManager.ChangePreferredLocation(list, item, newLoc);
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

        if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Plus.ToIconString(), IconButtonSize, "Add this item at the end of the list", false,
                true))
            _plugin.AutoGatherListsManager.AddItem(list, gatherables[_autoGatherListsCache.NewGatherableIdx]);

        ImGui.SameLine();
        var allEnabled = list.Items.All(i => list.EnabledItems[i]);
        if (ImGui.Checkbox("##AllEnabled", ref allEnabled))
        {
            list.Items.Each(i => _plugin.AutoGatherListsManager.ChangeEnabled(list, i, allEnabled));
        }
        ImGuiUtil.HoverTooltip((allEnabled ? "Disable" : "Enable" ) + " all items in the list");

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
