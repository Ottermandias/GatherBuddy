using System;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Components;
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
            ImGui.SameLine();
            var invTotal = GetInventoryAmountForItem(item.ItemId);
            ImGui.SetNextItemWidth(100f);
            ImGui.Text($"{invTotal} / ");
            ImGui.SameLine();
            int quantity = (int)item.Quantity;
            ImGui.SetNextItemWidth(100f);
            if (ImGui.InputInt("##quantity", ref quantity, 1, 10, ImGuiInputTextFlags.EnterReturnsTrue))
                _plugin.GatherWindowManager.ChangeQuantity(preset, quantity, i);
            group.Dispose();

            _gatherWindowCache.Selector.CreateDropSource(new GatherWindowDragDropData(preset, item, i), item.Name[GatherBuddy.Language]);

            var localIdx = i;
            _gatherWindowCache.Selector.CreateDropTarget<GatherWindowDragDropData>(d
                => _plugin.GatherWindowManager.MoveItem(d.Preset, d.ItemIdx, localIdx));
        }

        unsafe int GetInventoryAmountForItem(uint itemid)
        {
            var inventory = InventoryManager.Instance();
            if (inventory == null)
                return 0;
            else
            {
                return inventory->GetInventoryItemCount(itemid);
            }
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
