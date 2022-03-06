﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud;
using Dalamud.Game.Gui;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Interface;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using ImGuiNET;
using ImGuiOtter;
using ImGuiOtter.Table;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private sealed class FishTable : Table<ExtendedFish>, IDisposable
    {
        private static float _nameColumnWidth             = 0;
        private static float _caughtColumnWidth           = 0;
        private static float _nextUptimeColumnWidth       = 0;
        private static float _uptimeColumnWidth           = 0;
        private static float _baitColumnWidth             = 0;
        private static float _closestAetheryteColumnWidth = 0;
        private static float _typeColumnWidth             = 0;
        private static float _patchColumnWidth            = 0;
        private static float _folkloreColumnWidth         = 0;
        private static float _bestSpotColumnWidth         = 0;
        private static float _bestZoneColumnWidth         = 0;
        private static float _itemIdColumnWidth           = 0;
        private static float _fishIdColumnWidth           = 0;

        protected override void PreDraw()
        {
            if (_nameColumnWidth == 0)
            {
                _caughtColumnWidth = TextWidth("Log") / Scale + Table.ArrowWidth;
                _nameColumnWidth   = (Items.Max(i => TextWidth(i.Data.Name[GatherBuddy.Language])) + ItemSpacing.X + LineIconSize.X) / Scale;
                _nextUptimeColumnWidth = Math.Max(TextWidth("99:99 Minutes") / Scale,
                    TextWidth(_nextUptimeColumn.Label) / Scale + Table.ArrowWidth);
                _uptimeColumnWidth           = TextWidth("999.9%") / Scale;
                _baitColumnWidth             = (Items.Max(f => TextWidth(f.Bait.First().Name)) + ItemSpacing.X + LineIconSize.X) / Scale;
                _closestAetheryteColumnWidth = GatherBuddy.GameData.Aetherytes.Values.Max(a => TextWidth(a.Name)) / Scale;
                _typeColumnWidth             = TextWidth("Spearfishing");
                _patchColumnWidth            = TextWidth("Patch") / Scale + Table.ArrowWidth;
                _folkloreColumnWidth         = Items.Max(i => TextWidth(i.Data.Folklore)) / Scale;
                _bestSpotColumnWidth         = GatherBuddy.GameData.FishingSpots.Values.Max(a => TextWidth(a.Name)) / Scale;
                _bestZoneColumnWidth         = GatherBuddy.GameData.Territories.Values.Max(a => TextWidth(a.Name)) / Scale;
                _itemIdColumnWidth           = Math.Max(TextWidth("999999") / Scale, TextWidth(_itemIdColumn.Label) / Scale + Table.ArrowWidth);
                _fishIdColumnWidth           = Math.Max(TextWidth("99999") / Scale,  TextWidth(_fishIdColumn.Label) / Scale + Table.ArrowWidth);
            }

            GatherBuddy.FishLog.CheckForChanges();
        }

        public FishTable()
            : base("##FishTable",
                GatherBuddy.GameData.Fishes.Values
                    .Where(f => f.FishingSpots.Count > 0 && f.InLog)
                    .Select(f => new ExtendedFish(f)).ToList(), TextHeight, _nameColumn, _caughtColumn, _nextUptimeColumn, _uptimeColumn,
                _baitColumn, _bestSpotColumn, _typeColumn, _patchColumn, _folkloreColumn, _aetheryteColumn, _bestZoneColumn, _itemIdColumn,
                _fishIdColumn)
        {
            Sortable                               =  true;
            GatherBuddy.UptimeManager.UptimeChange += OnUptimeChange;
            Flags                                  |= ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Resizable;
            GatherBuddy.FishLog.Change             += OnLogChange;
        }

        private static readonly NameColumn       _nameColumn       = new() { Label = "Item Name..." };
        private static readonly CaughtColumn     _caughtColumn     = new() { Label = "Log" };
        private static readonly NextUptimeColumn _nextUptimeColumn = new() { Label = "Next Uptime" };
        private static readonly UptimesColumn    _uptimeColumn     = new() { Label = "Up%" };
        private static readonly BaitColumn       _baitColumn       = new() { Label = "Bait" };
        private static readonly AetheryteColumn  _aetheryteColumn  = new() { Label = "Aetheryte" };
        private static readonly TypeColumn       _typeColumn       = new() { Label = "Fish Type" };
        private static readonly PatchColumn      _patchColumn      = new() { Label = "Patch" };
        private static readonly FolkloreColumn   _folkloreColumn   = new() { Label = "Folklore" };
        private static readonly BestSpotColumn   _bestSpotColumn   = new() { Label = "Best Spot" };
        private static readonly BestZoneColumn   _bestZoneColumn   = new() { Label = "Best Zone" };
        private static readonly ItemIdColumn     _itemIdColumn     = new() { Label = "Item Id" };
        private static readonly FishIdColumn     _fishIdColumn     = new() { Label = "G. Id" };

        private class FishFilterColumn : HeaderConfigFlags<FishFilter, ExtendedFish>
        {
            private FishFilter[] FlagValues = Array.Empty<FishFilter>();
            private string[]     FlagNames  = Array.Empty<string>();

            protected void SetFlags(params FishFilter[] flags)
            {
                FlagValues = flags;
                AllFlags   = flags.Aggregate((f, g) => f | g);
            }

            protected void SetNames(params string[] names)
                => FlagNames = names;

            protected void SetFlagsAndNames(params FishFilter[] flags)
            {
                SetFlags(flags);
                SetNames(flags.Select(f => f.ToString()).ToArray());
            }

            protected sealed override IReadOnlyList<FishFilter> Values
                => FlagValues;

            protected sealed override string[] Names
                => FlagNames;

            public sealed override FishFilter FilterValue
                => GatherBuddy.Config.ShowFish;

            protected sealed override void SetValue(FishFilter f, bool value)
            {
                var tmp = value ? GatherBuddy.Config.ShowFish | f : GatherBuddy.Config.ShowFish & ~f;
                if (tmp == GatherBuddy.Config.ShowFish)
                    return;

                GatherBuddy.Config.ShowFish = tmp;
                GatherBuddy.Config.Save();
            }
        }

        private sealed class NameColumn : HeaderConfigString<ExtendedFish>
        {
            public NameColumn()
                => Flags |= ImGuiTableColumnFlags.NoHide | ImGuiTableColumnFlags.NoReorder;

            public override string ToName(ExtendedFish item)
                => item.Data.Name[GatherBuddy.Language];

            public override float Width
                => _nameColumnWidth * ImGuiHelpers.GlobalScale;

            public override void DrawColumn(ExtendedFish item, int id)
            {
                using var style = ImGuiRaii.PushStyle(ImGuiStyleVar.ItemSpacing, ItemSpacing / 2);
                ImGuiUtil.HoverIcon(item.Icon, LineIconSize);
                ImGui.SameLine();
                var selected = ImGui.Selectable(item.Data.Name[GatherBuddy.Language]);
                var hovered  = ImGui.IsItemHovered();
                _plugin.Interface.CreateContextMenu(item.Data);

                if (selected)
                    _plugin.Executor.GatherItem(item.Data);
                if (hovered)
                    item.SetTooltip(IconSize, SmallIconSize, WeatherIconSize);
            }
        }

        private sealed class CaughtColumn : FishFilterColumn
        {
            public CaughtColumn()
            {
                Flags |= ImGuiTableColumnFlags.NoReorder;
                SetFlags(FishFilter.AlreadyCaught, FishFilter.Uncaught, FishFilter.NotInLog);
                SetNames("Already Caught", "Uncaught", "Not In Log");
            }

            public override float Width
                => _caughtColumnWidth * ImGuiHelpers.GlobalScale;

            public override void DrawColumn(ExtendedFish fish, int _)
            {
                using var font = ImGuiRaii.PushFont(UiBuilder.IconFont);
                if (!fish.Data.InLog)
                {
                    using var color = ImGuiRaii.PushColor(ImGuiCol.Text, 0xFFA00000);
                    ImGuiUtil.Center(FontAwesomeIcon.Question.ToIconString());
                    return;
                }

                if (fish.Unlocked)
                {
                    using var color = ImGuiRaii.PushColor(ImGuiCol.Text, 0xFF008000);
                    ImGuiUtil.Center(FontAwesomeIcon.Check.ToIconString());
                }
                else
                {
                    using var color = ImGuiRaii.PushColor(ImGuiCol.Text, 0xFF000080);
                    ImGuiUtil.Center(FontAwesomeIcon.Times.ToIconString());
                }
            }

            public override bool FilterFunc(ExtendedFish fish)
            {
                if (!fish.Data.InLog)
                    return FilterValue.HasFlag(FishFilter.NotInLog);

                return fish.Unlocked
                    ? FilterValue.HasFlag(FishFilter.AlreadyCaught)
                    : FilterValue.HasFlag(FishFilter.Uncaught);
            }

            public override int Compare(ExtendedFish lhs, ExtendedFish rhs)
            {
                if (!lhs.Data.InLog)
                    return rhs.Data.InLog ? 1 : 0;

                return lhs.Unlocked ? rhs.Unlocked ? 0 : 1 : -1;
            }
        }

        private sealed class NextUptimeColumn : FishFilterColumn
        {
            public NextUptimeColumn()
            {
                Flags |= ImGuiTableColumnFlags.DefaultSort;
                SetFlags(FishFilter.Available, FishFilter.Unavailable, FishFilter.FishDependency);
                SetNames("Currently Available", "Currently Unavailable", "Dependent On Other Restrictions");
            }

            public override float Width
                => _nextUptimeColumnWidth * ImGuiHelpers.GlobalScale;

            public override void DrawColumn(ExtendedFish item, int _)
                => DrawTimeInterval(item.Uptime.Item2, item.UptimeDependency);

            public override int Compare(ExtendedFish lhs, ExtendedFish rhs)
                => lhs.Uptime.Item2.Compare(rhs.Uptime.Item2);

            public override bool FilterFunc(ExtendedFish item)
            {
                if (item.UptimeDependency && !FilterValue.HasFlag(FishFilter.FishDependency))
                    return false;

                var (_, uptime) = item.Uptime;
                return FilterValue.HasFlag(uptime.InRange(GatherBuddy.Time.ServerTime)
                    ? FishFilter.Available
                    : FishFilter.Unavailable);
            }
        }

        private sealed class BaitColumn : HeaderConfigString<ExtendedFish>
        {
            public override string ToName(ExtendedFish item)
                => item.Bait.First().Name;

            public override float Width
                => _baitColumnWidth * ImGuiHelpers.GlobalScale;

            public override void DrawColumn(ExtendedFish item, int _)
            {
                using var style = ImGuiRaii.PushStyle(ImGuiStyleVar.ItemSpacing, ItemSpacing / 2);
                ImGuiUtil.HoverIcon(item.Bait.First().Icon, LineIconSize);
                ImGui.SameLine();
                ImGui.Selectable(item.Bait.First().Name);
                CreateContextMenu(item.Data.InitialBait);
            }
        }

        private sealed class AetheryteColumn : HeaderConfigString<ExtendedFish>
        {
            public override string ToName(ExtendedFish item)
                => item.Uptime.Item1.ClosestAetheryte?.Name ?? "None";

            public override float Width
                => _closestAetheryteColumnWidth * ImGuiHelpers.GlobalScale;

            public override void DrawColumn(ExtendedFish item, int _)
            {
                var aetheryte = item.Uptime.Item1.ClosestAetheryte;
                if (aetheryte == null)
                {
                    ImGui.Text("None");
                    return;
                }

                if (ImGui.Selectable(aetheryte.Name))
                    Executor.TeleportToAetheryte(aetheryte);
                HoverTooltip(item.Aetherytes);
            }

            public override bool FilterFunc(ExtendedFish item)
            {
                var name = item.Aetherytes;
                if (FilterValue.Length == 0)
                    return true;

                return FilterRegex?.IsMatch(name) ?? name.Contains(FilterValue, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private sealed class PatchColumn : HeaderConfigFlags<PatchFlag, ExtendedFish>
        {
            public PatchColumn()
                => AllFlags = Enum.GetValues<PatchFlag>().Aggregate((l, r) => l | r);

            private static readonly string[] FlagNames = Enum.GetValues<PatchFlag>()
                .Select(p => $"{p.ToPatch().ToVersionString()} - {p.ToPatch().ToPatchName()}")
                .ToArray();

            protected override string[] Names
                => FlagNames;

            protected override IReadOnlyList<PatchFlag> Values
                => Enum.GetValues<PatchFlag>();

            public override float Width
                => _patchColumnWidth * ImGuiHelpers.GlobalScale;

            public override PatchFlag FilterValue
                => ~GatherBuddy.Config.HideFishPatch;

            protected override void SetValue(PatchFlag f, bool v)
            {
                var tmp = v ? GatherBuddy.Config.HideFishPatch & ~f : GatherBuddy.Config.HideFishPatch | f;
                if (tmp == GatherBuddy.Config.HideFishPatch)
                    return;

                GatherBuddy.Config.HideFishPatch = tmp;
                GatherBuddy.Config.Save();
            }

            public override void DrawColumn(ExtendedFish item, int _)
            {
                ImGui.Text(item.Data.Patch.ToVersionString());
                ImGuiUtil.HoverTooltip(item.Data.Patch.ToPatchName());
            }

            public override int Compare(ExtendedFish lhs, ExtendedFish rhs)
                => lhs.Data.Patch.CompareTo(rhs.Data.Patch);

            public override bool FilterFunc(ExtendedFish item)
                => FilterValue.HasFlag(item.Data.Patch.ToPatchFlag());
        }

        private sealed class TypeColumn : FishFilterColumn
        {
            public override float Width
                => _typeColumnWidth * ImGuiHelpers.GlobalScale;

            public TypeColumn()
            {
                SetFlags(FishFilter.SmallFish, FishFilter.BigFish, FishFilter.Spearfishing, FishFilter.OceanFish);
                SetNames("Regular Fish", "Big Fish", "Spearfishing", "Oceanfishing");
            }

            public override void DrawColumn(ExtendedFish item, int _)
                => ImGui.Text(item.FishType);

            public override int Compare(ExtendedFish lhs, ExtendedFish rhs)
                => string.Compare(lhs.FishType, rhs.FishType, StringComparison.InvariantCulture);

            public override bool FilterFunc(ExtendedFish item)
            {
                if (item.Data.OceanFish)
                    return FilterValue.HasFlag(FishFilter.OceanFish);

                if (item.Data.IsSpearFish)
                    return FilterValue.HasFlag(FishFilter.Spearfishing);

                if (item.Data.IsBigFish)
                    return FilterValue.HasFlag(FishFilter.BigFish);

                return FilterValue.HasFlag(FishFilter.SmallFish);
            }
        }

        private sealed class FolkloreColumn : HeaderConfigString<ExtendedFish>
        {
            public override string ToName(ExtendedFish item)
                => item.Data.Folklore;

            public override float Width
                => _folkloreColumnWidth * ImGuiHelpers.GlobalScale;
        }

        private sealed class UptimesColumn : FishFilterColumn
        {
            public UptimesColumn()
            {
                SetFlags(FishFilter.TimeDependency, FishFilter.WeatherDependency, FishFilter.NoDependency);
                SetNames("Dependent on Time of Day", "Dependent on Weather", "No Dependencies");
            }

            public override float Width
                => _uptimeColumnWidth * ImGuiHelpers.GlobalScale;

            public override void DrawColumn(ExtendedFish fish, int _)
                => ImGuiUtil.RightAlign(fish.UptimeString);

            public override int Compare(ExtendedFish lhs, ExtendedFish rhs)
                => lhs.UptimePercent.CompareTo(rhs.UptimePercent);

            public override bool FilterFunc(ExtendedFish fish)
                => fish.Data.FishRestrictions switch
                {
                    FishRestrictions.None           => FilterValue.HasFlag(FishFilter.NoDependency),
                    FishRestrictions.Time           => FilterValue.HasFlag(FishFilter.TimeDependency),
                    FishRestrictions.Weather        => FilterValue.HasFlag(FishFilter.WeatherDependency),
                    FishRestrictions.TimeAndWeather => FilterValue.HasFlag(FishFilter.TimeDependency | FishFilter.WeatherDependency),
                    _                               => false,
                };
        }

        private sealed class BestSpotColumn : HeaderConfigString<ExtendedFish>
        {
            public override string ToName(ExtendedFish item)
                => item.Uptime.Item1.Name;

            public override float Width
                => _bestSpotColumnWidth * ImGuiHelpers.GlobalScale;

            public override void DrawColumn(ExtendedFish item, int _)
            {
                if (ImGui.Selectable(ToName(item)))
                    _plugin.Executor.GatherLocation(item.Uptime.Item1);
                CreateContextMenu(item.Uptime.Item1 as FishingSpot);
                HoverTooltip(item.SpotNames);
            }

            public override bool FilterFunc(ExtendedFish item)
            {
                var name = item.SpotNames;
                if (FilterValue.Length == 0)
                    return true;

                return FilterRegex?.IsMatch(name) ?? name.Contains(FilterValue, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private sealed class BestZoneColumn : HeaderConfigString<ExtendedFish>
        {
            public override string ToName(ExtendedFish item)
                => item.Uptime.Item1.Territory.Name;

            public override float Width
                => _bestZoneColumnWidth * ImGuiHelpers.GlobalScale;

            public override void DrawColumn(ExtendedFish item, int _)
            {
                if (ImGui.Selectable(ToName(item)))
                    Executor.TeleportToTerritory(item.Uptime.Item1.Territory);
                HoverTooltip(item.Territories);
            }

            public override bool FilterFunc(ExtendedFish item)
            {
                var name = item.Territories;
                if (FilterValue.Length == 0)
                    return true;

                return FilterRegex?.IsMatch(name) ?? name.Contains(FilterValue, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private sealed class ItemIdColumn : HeaderConfig<ExtendedFish>
        {
            public override float Width
                => _itemIdColumnWidth;

            public override int Compare(ExtendedFish lhs, ExtendedFish rhs)
                => lhs.Data.ItemId.CompareTo(rhs.Data.ItemId);

            public override void DrawColumn(ExtendedFish item, int _)
                => ImGuiUtil.RightAlign($"{item.Data.ItemId}");
        }

        private sealed class FishIdColumn : HeaderConfig<ExtendedFish>
        {
            public override float Width
                => _fishIdColumnWidth;

            public override int Compare(ExtendedFish lhs, ExtendedFish rhs)
                => lhs.Data.FishId.CompareTo(rhs.Data.FishId);

            public override void DrawColumn(ExtendedFish item, int _)
                => ImGuiUtil.RightAlign($"{item.Data.FishId}{(item.Data.IsSpearFish ? " (sf)" : string.Empty)}");
        }


        public void Dispose()
        {
            GatherBuddy.UptimeManager.UptimeChange -= OnUptimeChange;
            GatherBuddy.FishLog.Change             -= OnLogChange;
        }

        private void OnLogChange()
        {
            foreach (var fish in Items)
                fish.Unlocked = GatherBuddy.FishLog.IsUnlocked(fish.Data);
            if (_caughtColumn.FilterValue != _caughtColumn.AllFlags)
                FilterDirty = true;
            else if (SortIdx == 1)
                SortDirty = true;
        }

        private void OnUptimeChange(IGatherable item)
        {
            if (item.Type != ObjectType.Fish)
                return;

            FilterDirty = true;
        }
    }

    private readonly FishTable _fishTable = new();

    private void DrawFishTab()
    {
        using var id  = ImGuiRaii.PushId("Fish");
        var       ret = ImGui.BeginTabItem("Fish");
        ImGuiUtil.HoverTooltip("There are plenty of fish in the sea. And the air. And the sand. And the lava. And space, for some reason.\n"
          + " Gotta catch'em all!\n"
          + "Enough information about fish to get you started, and for everything else there's TeamCraft!");
        if (!ret)
            return;

        using var end = ImGuiRaii.DeferredEnd(ImGui.EndTabItem);
        _fishTable.Draw();
    }
}
