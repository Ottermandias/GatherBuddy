using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud.Interface;
using ImGuiNET;

namespace ImGuiOtter.Table;

public static class Table
{
    public const float ArrowWidth = 10;
}

public class Table<T>
{
    protected          bool           FilterDirty = true;
    protected          bool           SortDirty   = true;
    protected readonly ICollection<T> Items;
    internal readonly  List<(T, int)> FilteredItems;

    protected readonly string            Label;
    protected readonly HeaderConfig<T>[] Headers;

    protected float ItemHeight  { get; init; }
    protected float ExtraHeight { get; set; } = 0;

    private int _currentIdx = 0;

    protected bool Sortable
    {
        get => Flags.HasFlag(ImGuiTableFlags.Sortable);
        set => Flags = value ? Flags | ImGuiTableFlags.Sortable : Flags & ~ImGuiTableFlags.Sortable;
    }

    protected int SortIdx = -1;

    public ImGuiTableFlags Flags = ImGuiTableFlags.RowBg
      | ImGuiTableFlags.Sortable
      | ImGuiTableFlags.BordersOuter
      | ImGuiTableFlags.ScrollY
      | ImGuiTableFlags.ScrollX
      | ImGuiTableFlags.PreciseWidths
      | ImGuiTableFlags.BordersInnerV
      | ImGuiTableFlags.NoBordersInBodyUntilResize;

    public Table(string label, ICollection<T> items, float itemHeight, params HeaderConfig<T>[] headers)
    {
        Label         = label;
        Items         = items;
        ItemHeight    = itemHeight;
        Headers       = headers;
        FilteredItems = new List<(T, int)>(Items.Count);
    }

    public void Draw()
    {
        using var id = ImGuiRaii.PushId(Label);
        UpdateFilter();
        DrawTableInternal();
    }

    protected virtual void DrawFilters()
        => throw new NotImplementedException();

    protected virtual void PreDraw()
    { }

    private void SortInternal()
    {
        if (!Sortable)
            return;

        var sortSpecs = ImGui.TableGetSortSpecs();
        SortDirty |= sortSpecs.SpecsDirty;

        if (!SortDirty)
            return;

        SortIdx = sortSpecs.Specs.ColumnIndex;

        if (Headers.Length <= SortIdx)
            SortIdx = 0;

        if (sortSpecs.Specs.SortDirection == ImGuiSortDirection.Ascending)
            FilteredItems.StableSort((a, b) => Headers[SortIdx].Compare(a.Item1, b.Item1));
        else if (sortSpecs.Specs.SortDirection == ImGuiSortDirection.Descending)
            FilteredItems.StableSort((a, b) => Headers[SortIdx].CompareInv(a.Item1, b.Item1));
        else
            SortIdx = -1;
        SortDirty            = false;
        sortSpecs.SpecsDirty = false;
    }

    private void UpdateFilter()
    {
        if (!FilterDirty)
            return;

        FilteredItems.Clear();
        var idx = 0;
        foreach (var item in Items)
        {
            if (Headers.All(header => header.FilterFunc(item)))
                FilteredItems.Add((item, idx));
            idx++;
        }

        FilterDirty = false;
        SortDirty   = true;
    }

    private void DrawItem((T, int) pair)
    {
        var       column = 0;
        using var id     = ImGuiRaii.PushId(_currentIdx);
        _currentIdx = pair.Item2;
        foreach (var header in Headers)
        {
            id.Push(column++);
            if (ImGui.TableNextColumn())
                header.DrawColumn(pair.Item1, pair.Item2);
            id.Pop();
        }
    }

    private void DrawTableInternal()
    {
        if (!ImGui.BeginTable("Table", Headers.Length, Flags,
                ImGui.GetContentRegionAvail() - ExtraHeight * Vector2.UnitY * ImGuiHelpers.GlobalScale))
            return;

        using var end = ImGuiRaii.DeferredEnd(ImGui.EndTable);

        PreDraw();
        ImGui.TableSetupScrollFreeze(1, 1);
        foreach (var header in Headers)
            ImGui.TableSetupColumn(header.Label, header.Flags, header.Width);
        ImGui.TableNextRow(ImGuiTableRowFlags.Headers);
        var i = 0;
        foreach (var header in Headers)
        {
            using var id = ImGuiRaii.PushId(i);
            if (!ImGui.TableSetColumnIndex(i++))
                continue;

            using var style = ImGuiRaii.PushStyle(ImGuiStyleVar.ItemSpacing, Vector2.Zero);
            ImGui.TableHeader(string.Empty);
            ImGui.SameLine();
            style.Pop();
            if (header.DrawFilter())
                FilterDirty = true;
        }

        SortInternal();
        _currentIdx = 0;
        ImGuiUtil.ClippedDraw(FilteredItems, DrawItem, ItemHeight);
    }
}
