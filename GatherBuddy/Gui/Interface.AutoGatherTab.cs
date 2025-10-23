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
using GatherBuddy.Data;
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

    private static AutoGatherListsDragDropData? _dragDropData;

    private class AutoGatherListsCache : IDisposable
    {
        public AutoGatherListsCache()
        {
            UpdateGatherables();
            WorldData.WorldLocationsChanged += UpdateGatherables;
            _plugin.AutoGatherListsManager.ListOrderChanged += OnListOrderChanged;
        }

        private void OnListOrderChanged()
        {
            Selector.RefreshView();
        }

        public readonly AutoGatherListFileSystemSelector Selector = new();

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
                    .SelectMany(p => p).Any() 
                    || UmbralNodes.IsUmbralItem(g.ItemId) // Include umbral items
                    || (g.NodeList.Any(n => n.Territory.Id is 901 or 929 or 939) // Include Diadem items
                        && (g.Name[GatherBuddy.Language].Contains("Grade 4") // Grade 4: include all
                            || (g.Name[GatherBuddy.Language].Contains("Artisanal") // Grade 2/3: only Artisanal
                                && (g.Name[GatherBuddy.Language].Contains("Grade 2") 
                                    || g.Name[GatherBuddy.Language].Contains("Grade 3"))))))
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
            _plugin.AutoGatherListsManager.ListOrderChanged -= OnListOrderChanged;
        }

        public int  NewGatherableIdx;
        public bool EditName;
        public bool EditDesc;
    }

    private readonly AutoGatherListsCache _autoGatherListsCache;

    public AutoGatherList? CurrentAutoGatherList
        => _autoGatherListsCache.Selector.Selected;

    private void DrawAutoGatherListsLine()
    {
        if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Copy.ToIconString(), IconButtonSize, "Copy current auto-gather list to clipboard.",
                _autoGatherListsCache.Selector.Selected == null, true))
        {
            var list = _autoGatherListsCache.Selector.Selected!;
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
                _autoGatherListsCache.Selector.Selected == null))
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

                    var list = _autoGatherListsCache.Selector.Selected!;

                    foreach (var (itemName, quantity) in items)
                    {
                        var gatherable =
                            GatherBuddy.GameData.Gatherables.Values.FirstOrDefault(g => g.Name[Dalamud.ClientState.ClientLanguage] == itemName);
                        if (gatherable == null || (gatherable.NodeList.Count == 0 
                            && !UmbralNodes.IsUmbralItem(gatherable.ItemId) 
                            && !(gatherable.NodeList.Any(n => n.Territory.Id is 901 or 929 or 939)
                                && (gatherable.Name[Dalamud.ClientState.ClientLanguage].Contains("Grade 4")
                                    || (gatherable.Name[Dalamud.ClientState.ClientLanguage].Contains("Artisanal")
                                        && (gatherable.Name[Dalamud.ClientState.ClientLanguage].Contains("Grade 2") 
                                            || gatherable.Name[Dalamud.ClientState.ClientLanguage].Contains("Grade 3")))))))
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

        ImGui.Text($"{list.Items.Count} Items in List");
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

            // Custom drag-drop for moving items within and between lists
            using (var source = ImRaii.DragDropSource())
            {
                if (source.Success)
                {
                    _dragDropData = new AutoGatherListsDragDropData(list, item, i);
                    ImGui.SetDragDropPayload("AutoGatherListItem", ReadOnlySpan<byte>.Empty);
                    ImGui.TextUnformatted(item.Name[GatherBuddy.Language]);
                }
            }

            var localIdx = i;
            using (var target = ImRaii.DragDropTarget())
            {
                if (target.Success && ImGuiUtil.IsDropping("AutoGatherListItem") && _dragDropData != null)
                {
                    _plugin.AutoGatherListsManager.MoveItem(_dragDropData.list, _dragDropData.ItemIdx, localIdx);
                    _dragDropData = null;
                }
            }
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

        var selectorWidth = _autoGatherListsCache.Selector.SelectorWidth;
        using (var child = ImRaii.Child("AutoGatherListSelector", new Vector2(selectorWidth, -1), false))
        {
            if (child)
                _autoGatherListsCache.Selector.Draw();
        }

        ImGui.SameLine();
        ImGui.Button("##splitter", new Vector2(4, -1));
        if (ImGui.IsItemActive())
        {
            var delta = ImGui.GetIO().MouseDelta.X;
            selectorWidth += delta;
            selectorWidth = Math.Clamp(selectorWidth, 150f * Scale, ImGui.GetWindowWidth() * 0.5f);
            _autoGatherListsCache.Selector.SelectorWidth = selectorWidth;
        }

        ImGui.SameLine();

        ItemDetailsWindow.Draw("List Details", DrawAutoGatherListsLine, () =>
        {
            if (_autoGatherListsCache.Selector.Selected != null)
                DrawAutoGatherList(_autoGatherListsCache.Selector.Selected);
        });
    }
}
