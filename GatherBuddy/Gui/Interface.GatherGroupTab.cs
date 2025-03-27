using System;
using System.Collections.Generic;
using GatherBuddy.Interfaces;
using GatherBuddy.Time;
using ImGuiNET;
using OtterGui;
using System.Linq;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using GatherBuddy.Alarms;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.GatherGroup;
using GatherBuddy.GatherHelper;
using GatherBuddy.AutoGather.Lists;
using GatherBuddy.Plugin;
using OtterGui.Widgets;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private class GatherGroupDragDropData
    {
        public TimedGroup     Group;
        public TimedGroupNode Node;
        public int            NodeIdx;

        public GatherGroupDragDropData(TimedGroup group, TimedGroupNode node, int idx)
        {
            Group   = group;
            Node    = node;
            NodeIdx = idx;
        }
    }

    private class GatherGroupCache
    {
        public sealed class GatherGroupSelector : ItemSelector<TimedGroup>
        {
            private readonly GatherGroupManager _manager;

            public GatherGroupSelector(GatherGroupManager manager)
                : base(manager.Groups.Values, Flags.All & ~Flags.Move)
                => _manager = manager;

            protected override bool Filtered(int idx)
                => Filter.Length != 0 && !Items[idx].Name.Contains(Filter, StringComparison.InvariantCultureIgnoreCase);

            protected override bool OnDraw(int idx)
            {
                using var id = ImRaii.PushId(idx);
                return ImGui.Selectable(Items[idx].Name, idx == CurrentIdx);
            }

            protected override bool OnDelete(int idx)
            {
                if (Items.Count <= idx || idx < 0)
                    return false;

                _manager.Groups.RemoveAt(idx);
                _manager.Save();
                return true;
            }

            protected override bool OnAdd(string name)
                => _manager.AddGroup(name, new TimedGroup(name));

            protected override bool OnClipboardImport(string name, string data)
            {
                if (!TimedGroup.Config.FromBase64(data, out var cfgGroup))
                    return false;

                TimedGroup.FromConfig(cfgGroup, out var group);
                group.Name = name;
                return _manager.AddGroup(name, group);
            }

            protected override bool OnDuplicate(string name, int idx)
            {
                if (Items.Count <= idx || idx < 0)
                    return false;

                var group = _manager.Groups.Values[idx].Clone(name);
                return _manager.AddGroup(name, group);
            }

            protected override void OnDrop(object? data, int idx)
            {
                if (Items.Count <= idx || idx < 0 || data is not GatherGroupDragDropData d)
                    return;

                var group = Items[idx];

                if (!_plugin.GatherGroupManager.ChangeGroupNode(@group, @group.Nodes.Count, d.Node.Item, d.Node.EorzeaStartMinute,
                        d.Node.EorzeaEndMinute, d.Node.Annotation, false))
                {
                    GatherBuddy.Log.Error($"Could not move node from group {d.Group.Name} to group {group.Name}.");
                    return;
                }

                _plugin.GatherGroupManager.ChangeGroupNode(d.Group, d.NodeIdx, null, null, null, null, true);
                _plugin.GatherGroupManager.Save();
            }
        }

        public static readonly IGatherable[] AllGatherables = GatherBuddy
            .GameData
            .Gatherables.Values
            .Concat(GatherBuddy.GameData.Fishes.Values.Cast<IGatherable>())
            .Where(g => g.Locations.Any())
            .OrderBy(g => g.Name[GatherBuddy.Language])
            .ToArray();

        public readonly ClippedSelectableCombo<IGatherable> GatherableSelector =
            new("AllGatherables", string.Empty, 250, AllGatherables, g => g.Name[GatherBuddy.Language]);

        public readonly GatherGroupSelector Selector;

        public bool NameEdit;
        public bool DescriptionEdit;
        public int  AnnotationEditIdx = -1;

        public readonly string DefaultGroupTooltip;
        public          int    NewItemIdx = 0;

        private          bool        _itemPerMinuteDirty = true;
        private readonly List<short> _itemPerMinute      = new(24);

        public void SetDirty()
            => _itemPerMinuteDirty = true;

        public List<short> UpdateItemPerMinute(TimedGroup group)
        {
            if (!_itemPerMinuteDirty && group.Nodes.Count + 1 == _itemPerMinute.Count)
                return _itemPerMinute;

            _itemPerMinute.Clear();
            _itemPerMinute.AddRange(Enumerable.Repeat((short)0, group.Nodes.Count + 1));
            for (var i = 0; i < RealTime.MinutesPerDay; ++i)
            {
                var node = group.CurrentNode((uint)i);
                if (node == null)
                {
                    _itemPerMinute[0]++;
                }
                else
                {
                    var idx = group.Nodes.IndexOf(node);
                    _itemPerMinute[idx + 1]++;
                }
            }

            _itemPerMinuteDirty = false;
            return _itemPerMinute;
        }

        public GatherGroupCache(GatherGroupManager gatherGroupManager)
        {
            Selector = new GatherGroupSelector(gatherGroupManager);
            DefaultGroupTooltip =
                "Restore the gather groups provided by default if they have been deleted or changed in any way.\n"
              + "Hold Control to apply. Default Groups are:\n\t- "
              + $"{string.Join("\n\t- ", GroupData.DefaultGroups.Select(g => g.Name))}";
        }
    }

    private readonly GatherGroupCache _gatherGroupCache;

    private void DrawTimeInput(string label, float width, int value, Action<int> setter)
    {
        var       hour   = value / RealTime.MinutesPerHour;
        var       minute = value % RealTime.MinutesPerHour;
        using var group  = ImRaii.Group();
        using var id     = ImRaii.PushId(label);
        ImGui.SetNextItemWidth(width);
        using var style  = ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, Vector2.One * 2 * ImGuiHelpers.GlobalScale);
        var       change = ImGui.DragInt("##hour", ref hour, 0.05f, 0, RealTime.HoursPerDay - 1, "%02d", ImGuiSliderFlags.AlwaysClamp);
        ImGui.SameLine();
        ImGui.Text(":");
        ImGui.SameLine();
        style.Pop();
        ImGui.SetNextItemWidth(width);
        change |= ImGui.DragInt("##minute", ref minute, 0.2f, 0, RealTime.MinutesPerHour - 1, "%02d", ImGuiSliderFlags.AlwaysClamp);

        if (change)
        {
            var newValue = Math.Clamp(hour * RealTime.MinutesPerHour + minute, 0, RealTime.MinutesPerDay - 1);
            if (newValue != value)
                setter(newValue);
        }
    }

    private void DrawTimeInput(int fromValue, int toValue, Action<int, int> setter)
    {
        var       width = 20 * ImGuiHelpers.GlobalScale;
        using var group = ImRaii.Group();

        ImGui.Text(" from ");
        ImGui.SameLine();
        DrawTimeInput("##from", width, fromValue, v => setter(v, toValue));
        ImGui.SameLine();
        ImGui.Text(" to ");
        ImGui.SameLine();
        DrawTimeInput("##to", width, toValue, v => setter(fromValue, v));
        ImGui.SameLine();
        ImGui.Text(" Eorzea Time");
    }

    private static void DrawLocationTooltip(ILocation? loc)
    {
        if (loc == null || !ImGui.IsItemHovered())
            return;

        var tt = $"{string.Join("\n", loc.Gatherables.Select(g => g.Name[GatherBuddy.Language]))}";
        if (loc is GatheringNode g)
            tt = $"{loc.Territory.Name}\n{loc.GatheringType}\n{g.Times.PrintHours()}\n{tt}";
        ImGui.SetTooltip(tt);
    }

    private static void DrawLocationInput(TimedGroup group, int nodeIdx, TimedGroupNode node)
    {
        if (DrawLocationInput(node.Item, node.PreferLocation, out var newLoc)
         && _plugin.GatherGroupManager.ChangeGroupNodeLocation(group, nodeIdx, newLoc))
            _plugin.GatherGroupManager.Save();
    }

    private void DrawGatherGroupNode(TimedGroup group, ref int idx, int minutes)
    {
        var       node           = group.Nodes[idx];
        using var id             = ImRaii.PushId(idx);
        var       i              = idx;
        var       annotationEdit = _gatherGroupCache.AnnotationEditIdx;
        ImGui.TableNextColumn();
        if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Trash.ToIconString(), IconButtonSize, "Delete this item.", false, true))
            if (_plugin.GatherGroupManager.ChangeGroupNode(group, i, null, null, null, null, true))
            {
                --idx;
                _plugin.GatherGroupManager.Save();
                _gatherGroupCache.SetDirty();
            }

        ImGui.TableNextColumn();
        if (_gatherGroupCache.GatherableSelector.Draw(node.Item.Name[GatherBuddy.Language], out var newIdx)
         && _plugin.GatherGroupManager.ChangeGroupNode(group, i, GatherGroupCache.AllGatherables[newIdx], null, null, null, false))
            _plugin.GatherGroupManager.Save();

        _gatherGroupCache.Selector.CreateDropSource(new GatherGroupDragDropData(group, node, i), node.Item.Name[GatherBuddy.Language]);

        _gatherGroupCache.Selector.CreateDropTarget<GatherGroupDragDropData>(d => _plugin.GatherGroupManager.MoveNode(group, d.NodeIdx, i));

        ImGui.TableNextColumn();
        DrawTimeInput(node.EorzeaStartMinute, node.EorzeaEndMinute, (from, to) =>
        {
            if (_plugin.GatherGroupManager.ChangeGroupNode(group, i, null, from, to, null, false))
            {
                _plugin.GatherGroupManager.Save();
                _gatherGroupCache.SetDirty();
            }
        });
        ImGui.TableNextColumn();
        DrawLocationInput(group, i, node);
        ImGui.TableNextColumn();
        var length = node.Length();
        ImGuiUtil.DrawTextButton($"{length} minutes", Vector2.Zero,
            minutes < length ? ColorId.WarningBg.Value() : ImGui.GetColorU32(ImGuiCol.FrameBg));
        if (minutes < length)
            HoverTooltip($"{length - minutes} minutes are overwritten by overlap with earlier items.");


        ImGui.TableNextColumn();
        var annotation = node.Annotation;
        if (_gatherGroupCache.AnnotationEditIdx != i)
        {
            ImGuiComponents.HelpMarker(annotation.Length > 0 ? annotation : "No annotation. Right-click to edit.");
            if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
            {
                _gatherGroupCache.AnnotationEditIdx = i;
                ImGui.SetKeyboardFocusHere();
            }

            ImGui.SameLine();
            using var alpha = ImRaii.PushStyle(ImGuiStyleVar.Alpha, 0f);
            ImGui.SetNextItemWidth(0);
            ImGui.InputTextWithHint("##annotation", string.Empty, ref annotation, 256);
        }
        else
        {
            ImGui.SetNextItemWidth(400 * ImGuiHelpers.GlobalScale);
            if (ImGui.InputTextWithHint("##annotation", "Annotation...", ref annotation, 256, ImGuiInputTextFlags.EnterReturnsTrue)
             && _plugin.GatherGroupManager.ChangeGroupNode(group, i, null, null, null, annotation, false))
                _plugin.GatherGroupManager.Save();
            if (annotationEdit == _gatherGroupCache.AnnotationEditIdx && !ImGui.IsItemActive())
                _gatherGroupCache.AnnotationEditIdx = -1;
        }
    }

    private static void DrawMissingTimesHint(bool missingTimes)
    {
        if (missingTimes)
            ImGuiUtil.DrawTextButton("Not all minutes have a corresponding item.", new Vector2(-ImGui.GetStyle().WindowPadding.X, 0),
                ColorId.WarningBg.Value());
    }

    private void DrawGatherGroupNodeTable(TimedGroup group)
    {
        var times = _gatherGroupCache.UpdateItemPerMinute(group);
        DrawMissingTimesHint(times[0] > 0);

        using var table = ImRaii.Table("##nodes", 6, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.ScrollX);
        if (!table)
            return;

        for (var i = 0; i < group.Nodes.Count; ++i)
            DrawGatherGroupNode(group, ref i, times[i + 1]);

        var idx = _gatherGroupCache.NewItemIdx;
        ImGui.TableNextColumn();
        if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Plus.ToIconString(), IconButtonSize, "Add new item...", false, true))
        {
            var gatherable = GatherGroupCache.AllGatherables[idx];
            if (gatherable.InternalLocationId > 0)
            {
                var locations = gatherable.Locations.ToList();
                if (locations.Count is 1 && locations[0] is GatheringNode node)
                {
                    var changes = false;
                    foreach (var (start, end) in node.Times.AllUptimes())
                        changes |= _plugin.GatherGroupManager.ChangeGroupNode(group, group.Nodes.Count, gatherable, (int)start * RealTime.MinutesPerHour, (int)end * RealTime.MinutesPerHour, null,
                            false);
                    if (changes)
                    {
                        _gatherGroupCache.SetDirty();
                        _plugin.GatherGroupManager.Save();
                    }
                }
            }
            else
            {
                if (_plugin.GatherGroupManager.ChangeGroupNode(group, group.Nodes.Count, gatherable, null, null, null, false))
                {
                    _gatherGroupCache.SetDirty();
                    _plugin.GatherGroupManager.Save();
                }
            }
        }

        ImGui.TableNextColumn();
        if (_gatherGroupCache.GatherableSelector.Draw(idx, out idx))
            _gatherGroupCache.NewItemIdx = idx;
        ImGui.TableNextColumn();
        ImGui.TableNextColumn();
    }


    private void DrawNameField(TimedGroup group)
    {
        var r = ImGuiUtil.DrawEditButtonText(0, group.Name, out var newName, ref _gatherGroupCache.NameEdit, IconButtonSize, SetInputWidth, 64);
        if (newName.Length == 0)
        {
            ImGui.SameLine();
            ImGuiUtil.DrawTextButton("Name can not be empty.", Vector2.Zero, ColorId.WarningBg.Value());
            r = false;
        }
        else if (newName != group.Name && _plugin.GatherGroupManager.Groups.ContainsKey(newName.ToLowerInvariant().Trim()))
        {
            ImGui.SameLine();
            ImGuiUtil.DrawTextButton("Name is already in use.", Vector2.Zero, ColorId.WarningBg.Value());
            r = false;
        }

        if (r && _plugin.GatherGroupManager.RenameGroup(group, newName))
            _plugin.GatherGroupManager.Save();
    }

    private void DrawDescField(TimedGroup group)
    {
        if (!ImGuiUtil.DrawEditButtonText(1, group.Description, out var newDesc, ref _gatherGroupCache.DescriptionEdit, IconButtonSize,
                2 * SetInputWidth, 128)
         || newDesc == group.Description)
            return;

        if (_plugin.GatherGroupManager.ChangeDescription(group, newDesc))
            _plugin.GatherGroupManager.Save();
    }

    private void DrawGatherGroup(TimedGroup group)
    {
        using var id = ImRaii.PushId(group.Name);

        DrawNameField(group);
        DrawDescField(group);
        ImGui.NewLine();
        DrawGatherGroupNodeTable(group);
    }

    private void DrawGatherGroupHeaderLine()
    {
        if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Copy.ToIconString(), IconButtonSize, "Copy current Gather Group to clipboard.",
                _gatherGroupCache.Selector.Current == null, true))
        {
            var group = _gatherGroupCache.Selector.Current!;
            try
            {
                var s = group.ToConfig().ToBase64();
                ImGui.SetClipboardText(s);
                Communicator.PrintClipboardMessage("Gather Group ", group.Name);
            }
            catch (Exception e)
            {
                Communicator.PrintClipboardMessage("Gather Group ", group.Name, e);
            }
        }

        if (ImGuiUtil.DrawDisabledButton("Create Auto-Gather List", Vector2.Zero, "Create a new Auto-Gather List from this gather group.",
                _gatherGroupCache.Selector.Current == null))
        {
            var preset = new AutoGatherList(_gatherGroupCache.Selector.Current!);
            _plugin.AutoGatherListsManager.AddList(preset);
        }

        if (ImGuiUtil.DrawDisabledButton("Create Window Preset", Vector2.Zero, "Create a new Gather Window Preset from this gather group.",
                _gatherGroupCache.Selector.Current == null))
        {
            var preset = new GatherWindowPreset(_gatherGroupCache.Selector.Current!);
            _plugin.GatherWindowManager.AddPreset(preset);
        }

        if (ImGuiUtil.DrawDisabledButton("Create Alarms", Vector2.Zero, "Create a new Alarm Group from this gather group.",
                _gatherGroupCache.Selector.Current == null))
        {
            var preset = new AlarmGroup(_gatherGroupCache.Selector.Current!);
            _plugin.AlarmManager.AddGroup(preset);
        }

        var       holdingCtrl = ImGui.GetIO().KeyCtrl;
        using var color       = ImRaii.PushColor(ImGuiCol.ButtonHovered, 0x8000A000, holdingCtrl);
        if (ImGui.Button("Restore Default Groups") && holdingCtrl && _plugin.GatherGroupManager.SetDefaults(true))
        {
            _gatherGroupCache.Selector.TryRestoreCurrent();
            _plugin.GatherGroupManager.Save();
        }

        color.Pop();
        ImGuiUtil.HoverTooltip(_gatherGroupCache.DefaultGroupTooltip);

        ImGui.SameLine();

        ImGuiComponents.HelpMarker("Use /gathergroup [name] [optional:minute offset] to call a group.\n"
          + "This will /gather the item that is up currently (or [minute offset] eorzea minutes in the future).\n"
          + "If times for multiple items overlap, the first item from top to bottom will be gathered.");
    }

    private void DrawGatherGroupTab()
    {
        using var id  = ImRaii.PushId("Gather Groups");
        using var tab = ImRaii.TabItem("Gather Groups");

        ImGuiUtil.HoverTooltip(
            "Do you really need to catch a Dirty Herry from 8PM to 10PM but gather mythril ore otherwise?\n"
          + "Set up your own gather groups! You can even share them with others!");

        if (!tab)
            return;

        _gatherGroupCache.Selector.Draw(SelectorWidth);
        ImGui.SameLine();

        ItemDetailsWindow.Draw("Group Details", DrawGatherGroupHeaderLine, () =>
        {
            if (_gatherGroupCache.Selector.Current != null)
                DrawGatherGroup(_gatherGroupCache.Selector.Current);
        });
    }
}
