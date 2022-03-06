﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Dalamud.Interface;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.FishTimer;
using GatherBuddy.Plugin;
using ImGuiNET;
using ImGuiOtter;
using ImGuiOtter.Table;
using ImGuiScene;
using Newtonsoft.Json;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private sealed class RecordTable : Table<FishRecord>
    {
        public const string FileNamePopup = "FileNamePopup";

        public RecordTable()
            : base("Fish Records", _plugin.FishRecorder.Records, TextHeight, _catchHeader, _baitHeader, _durationHeader, _castStartHeader,
                _biteTypeHeader, _hookHeader, _amountHeader, _spotHeader, _contentIdHeader, _gatheringHeader, _perceptionHeader, _sizeHeader,
                _flagHeader)
            => Flags |= ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Hideable;

        private        int _lastCount;
        private static int _deleteIdx = -1;

        protected override void PreDraw()
        {
            ExtraHeight = ImGui.GetFrameHeightWithSpacing() / ImGuiHelpers.GlobalScale;
            if (_deleteIdx > -1)
            {
                _plugin.FishRecorder.Remove(_deleteIdx);
                _deleteIdx = -1;
            }

            if (_lastCount != Items.Count)
            {
                FilterDirty = true;
                _lastCount  = Items.Count;
            }
        }

        private static readonly ContentIdHeader  _contentIdHeader  = new() { Label = "Content ID" };
        private static readonly BaitHeader       _baitHeader       = new() { Label = "Bait" };
        private static readonly SpotHeader       _spotHeader       = new() { Label = "Fishing Spot" };
        private static readonly CatchHeader      _catchHeader      = new() { Label = "Caught Fish" };
        private static readonly CastStartHeader  _castStartHeader  = new() { Label = "TimeStamp" };
        private static readonly BiteTypeHeader   _biteTypeHeader   = new() { Label = "Tug" };
        private static readonly HookHeader       _hookHeader       = new() { Label = "Hookset" };
        private static readonly DurationHeader   _durationHeader   = new() { Label = "Bite" };
        private static readonly GatheringHeader  _gatheringHeader  = new() { Label = "Gath." };
        private static readonly PerceptionHeader _perceptionHeader = new() { Label = "Perc." };
        private static readonly AmountHeader     _amountHeader     = new() { Label = "Amt" };
        private static readonly SizeHeader       _sizeHeader       = new() { Label = "Ilm" };
        private static readonly FlagHeader       _flagHeader       = new() { Label = "Flags" };

        private sealed class GatheringHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord record)
                => record.Gathering.ToString();

            public override float Width
                => 50 * ImGuiHelpers.GlobalScale;

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.Gathering.CompareTo(rhs.Gathering);

            public override void DrawColumn(FishRecord record, int _)
                => ImGuiUtil.RightAlign(ToName(record));
        }

        private sealed class PerceptionHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord record)
                => record.Perception.ToString();

            public override float Width
                => 50 * ImGuiHelpers.GlobalScale;

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.Perception.CompareTo(rhs.Gathering);

            public override void DrawColumn(FishRecord record, int _)
                => ImGuiUtil.RightAlign(ToName(record));
        }

        private sealed class AmountHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord record)
                => record.Amount.ToString();

            public override float Width
                => 35 * ImGuiHelpers.GlobalScale;

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.Amount.CompareTo(rhs.Amount);

            public override void DrawColumn(FishRecord record, int _)
            {
                ImGuiUtil.RightAlign(ToName(record));
            }
        }

        private sealed class SizeHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord record)
                => $"{record.Size / 10f:F1}";

            public override float Width
                => 50 * ImGuiHelpers.GlobalScale;

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.Size.CompareTo(rhs.Size);

            public override void DrawColumn(FishRecord record, int _)
            {
                var tt = string.Empty;
                if (record.Flags.HasFlag(FishRecord.Effects.Large))
                    tt = "Large Catch!";
                if (record.Flags.HasFlag(FishRecord.Effects.Collectible))
                    tt += tt.Length > 0 ? "\nCollectible!" : "Collectible!";
                using var color = ImGuiRaii.PushColor(ImGuiCol.Text, ColorId.DisabledText.Value(), tt.Length == 0);
                ImGuiUtil.RightAlign(ToName(record));
                ImGuiUtil.HoverTooltip(tt);
            }
        }


        private sealed class ContentIdHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord item)
                => item.Flags.HasFlag(FishRecord.Effects.Legacy) ? "Legacy" : item.ContentIdHash.ToString("X8");

            public override float Width
                => 75 * ImGuiHelpers.GlobalScale;

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.ContentIdHash.CompareTo(rhs.ContentIdHash);
        }

        private sealed class BaitHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord item)
                => item.Bait.Name;

            public override float Width
                => 150 * ImGuiHelpers.GlobalScale;
        }

        private sealed class SpotHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord item)
                => item.FishingSpot?.Name ?? "Unknown";

            public override float Width
                => 200 * ImGuiHelpers.GlobalScale;
        }

        private sealed class CatchHeader : HeaderConfigString<FishRecord>
        {
            public CatchHeader()
            {
                Flags |= ImGuiTableColumnFlags.NoHide;
                Flags |= ImGuiTableColumnFlags.NoReorder;
            }

            public override string ToName(FishRecord record)
                => record.Catch?.Name[GatherBuddy.Language] ?? "None";

            public override float Width
                => 200 * ImGuiHelpers.GlobalScale;

            public override void DrawColumn(FishRecord record, int idx)
            {
                base.DrawColumn(record, idx);
                if (ImGui.GetIO().KeyCtrl && ImGui.IsItemClicked(ImGuiMouseButton.Right))
                    _deleteIdx = idx;
                ImGuiUtil.HoverTooltip("Hold Control and right-click to delete...");
            }
        }

        private sealed class CastStartHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord record)
                => (record.TimeStamp.Time / 1000).ToString();

            public override float Width
                => 80 * ImGuiHelpers.GlobalScale;

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.TimeStamp.CompareTo(rhs.TimeStamp);

            public override void DrawColumn(FishRecord record, int _)
            {
                base.DrawColumn(record, _);
                ImGuiUtil.HoverTooltip(record.TimeStamp.ToString());
            }
        }

        [Flags]
        private enum TugTypeFilter : byte
        {
            Weak      = 0x01,
            Strong    = 0x02,
            Legendary = 0x04,
            Invalid   = 0x08,
        }

        private sealed class BiteTypeHeader : HeaderConfigFlags<TugTypeFilter, FishRecord>
        {
            public BiteTypeHeader()
            {
                AllFlags = TugTypeFilter.Weak | TugTypeFilter.Strong | TugTypeFilter.Legendary | TugTypeFilter.Invalid;
                _filter  = AllFlags;
            }

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.Tug.CompareTo(rhs.Tug);

            public override void DrawColumn(FishRecord item, int idx)
                => ImGui.Text(item.Tug.ToString());

            private TugTypeFilter _filter;

            protected override void SetValue(TugTypeFilter value, bool enable)
            {
                if (enable)
                    _filter |= value;
                else
                    _filter &= ~value;
            }

            public override TugTypeFilter FilterValue
                => _filter;

            public override bool FilterFunc(FishRecord item)
                => item.Tug switch
                {
                    BiteType.Weak      => _filter.HasFlag(TugTypeFilter.Weak),
                    BiteType.Strong    => _filter.HasFlag(TugTypeFilter.Strong),
                    BiteType.Legendary => _filter.HasFlag(TugTypeFilter.Legendary),
                    _                  => _filter.HasFlag(TugTypeFilter.Invalid),
                };

            public override float Width
                => 60 * ImGuiHelpers.GlobalScale;
        }

        [Flags]
        private enum HookSetFilter : byte
        {
            Regular  = 0x01,
            Precise  = 0x02,
            Powerful = 0x04,
            Double   = 0x08,
            Triple   = 0x10,
            Invalid  = 0x20,
        }

        private sealed class HookHeader : HeaderConfigFlags<HookSetFilter, FishRecord>
        {
            public HookHeader()
            {
                AllFlags = HookSetFilter.Precise
                  | HookSetFilter.Powerful
                  | HookSetFilter.Regular
                  | HookSetFilter.Double
                  | HookSetFilter.Triple
                  | HookSetFilter.Invalid;
                _filter = AllFlags;
            }

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.Hook.CompareTo(rhs.Hook);

            public override void DrawColumn(FishRecord item, int idx)
                => ImGui.Text(item.Hook.ToName());

            private HookSetFilter _filter;

            protected override void SetValue(HookSetFilter value, bool enable)
            {
                if (enable)
                    _filter |= value;
                else
                    _filter &= ~value;
            }

            public override HookSetFilter FilterValue
                => _filter;

            public override bool FilterFunc(FishRecord item)
                => item.Hook switch
                {
                    HookSet.Precise    => _filter.HasFlag(HookSetFilter.Precise),
                    HookSet.Powerful   => _filter.HasFlag(HookSetFilter.Powerful),
                    HookSet.Hook       => _filter.HasFlag(HookSetFilter.Regular),
                    HookSet.DoubleHook => _filter.HasFlag(HookSetFilter.Double),
                    HookSet.TripleHook => _filter.HasFlag(HookSetFilter.Triple),
                    _                  => _filter.HasFlag(HookSetFilter.Invalid),
                };

            public override float Width
                => 75 * ImGuiHelpers.GlobalScale;
        }

        private sealed class DurationHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord record)
                => $"{record.Bite / 1000}.{record.Bite % 1000:D3}";

            public override float Width
                => 50 * ImGuiHelpers.GlobalScale;

            public override void DrawColumn(FishRecord record, int _)
                => ImGuiUtil.RightAlign(ToName(record));

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.Bite.CompareTo(rhs.Bite);
        }


        private class FlagHeader : HeaderConfigFlags<FishRecord.Effects, FishRecord>
        {
            private readonly float                               _iconScale;
            private readonly (TextureWrap, FishRecord.Effects)[] _effects;

            private static readonly FishRecord.Effects[] _values =
            {
                FishRecord.Effects.Patience,
                (FishRecord.Effects)((uint)FishRecord.Effects.Patience << 16),
                FishRecord.Effects.Patience2,
                (FishRecord.Effects)((uint)FishRecord.Effects.Patience2 << 16),
                FishRecord.Effects.Intuition,
                (FishRecord.Effects)((uint)FishRecord.Effects.Intuition << 16),
                FishRecord.Effects.Snagging,
                (FishRecord.Effects)((uint)FishRecord.Effects.Snagging << 16),
                FishRecord.Effects.FishEyes,
                (FishRecord.Effects)((uint)FishRecord.Effects.FishEyes << 16),
                FishRecord.Effects.Chum,
                (FishRecord.Effects)((uint)FishRecord.Effects.Chum << 16),
                FishRecord.Effects.PrizeCatch,
                (FishRecord.Effects)((uint)FishRecord.Effects.PrizeCatch << 16),
                FishRecord.Effects.IdenticalCast,
                (FishRecord.Effects)((uint)FishRecord.Effects.IdenticalCast << 16),
                FishRecord.Effects.SurfaceSlap,
                (FishRecord.Effects)((uint)FishRecord.Effects.SurfaceSlap << 16),
                FishRecord.Effects.Collectible,
                (FishRecord.Effects)((uint)FishRecord.Effects.Collectible << 16),
            };

            private const FishRecord.Effects Mask = FishRecord.Effects.Patience
              | FishRecord.Effects.Patience2
              | FishRecord.Effects.Intuition
              | FishRecord.Effects.Snagging
              | FishRecord.Effects.FishEyes
              | FishRecord.Effects.Chum
              | FishRecord.Effects.PrizeCatch
              | FishRecord.Effects.IdenticalCast
              | FishRecord.Effects.SurfaceSlap
              | FishRecord.Effects.Collectible;

            private static readonly string[] _names =
            {
                "Patience On",
                "Patience Off",
                "Patience II On",
                "Patience II Off",
                "Intuition On",
                "Intuition Off",
                "Snagging On",
                "Snagging Off",
                "Fish Eyes On",
                "Fish Eyes Off",
                "Chum On",
                "Chum Off",
                "Prize Catch On",
                "Prize Catch Off",
                "Identical Cast On",
                "Identical Cast Off",
                "Surface Slap On",
                "Surface Slap Off",
                "Collectible On",
                "Collectible Off",
            };

            protected override IReadOnlyList<FishRecord.Effects> Values
                => _values;

            protected override string[] Names
                => _names;

            protected override void SetValue(FishRecord.Effects value, bool enable)
            {
                if (enable)
                    _filter |= value;
                else
                    _filter &= ~value;
            }

            private FishRecord.Effects _filter;

            public FlagHeader()
            {
                _effects = new[]
                {
                    (Icons.DefaultStorage[16023], _values[0]),
                    (Icons.DefaultStorage[11106], _values[2]),
                    (Icons.DefaultStorage[11101], _values[4]),
                    (Icons.DefaultStorage[11102], _values[6]),
                    (Icons.DefaultStorage[11103], _values[8]),
                    (Icons.DefaultStorage[11104], _values[10]),
                    (Icons.DefaultStorage[11119], _values[12]),
                    (Icons.DefaultStorage[11116], _values[14]),
                    (Icons.DefaultStorage[11115], _values[16]),
                    (Icons.DefaultStorage[11008], _values[18]),
                };
                _iconScale = (float)_effects[0].Item1.Width / _effects[0].Item1.Height;
                AllFlags   = Mask | (FishRecord.Effects)((uint)Mask << 16);
                _filter    = AllFlags;
            }

            public override float Width
                => 10 * (_iconScale * TextHeight + 1);

            public override bool FilterFunc(FishRecord item)
            {
                var enabled  = _filter & Mask;
                var disabled = (FishRecord.Effects)((int)_filter >> 16) & Mask;
                var flags    = item.Flags & Mask;
                var invFlags = ~flags & Mask;
                return (flags & enabled) == flags && (invFlags & disabled) == invFlags;
            }

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.Flags.CompareTo(rhs.Flags);

            public override FishRecord.Effects FilterValue
                => _filter;

            private void DrawIcon(FishRecord item, TextureWrap icon, FishRecord.Effects flag)
            {
                var size = new Vector2(TextHeight * _iconScale, TextHeight);
                var tint = item.Flags.HasFlag(flag) ? Vector4.One : new Vector4(0.75f, 0.75f, 0.75f, 0.5f);
                ImGui.Image(icon.ImGuiHandle, size, Vector2.Zero, Vector2.One, tint);
                if (!ImGui.IsItemHovered())
                    return;

                using var tt = ImGuiRaii.NewTooltip();
                ImGui.Image(icon.ImGuiHandle, new Vector2(icon.Width, icon.Height));
                ImGui.Text(flag.ToString());
            }

            public override void DrawColumn(FishRecord item, int idx)
            {
                using var space = ImGuiRaii.PushStyle(ImGuiStyleVar.ItemSpacing, Vector2.One);
                foreach (var (icon, flag) in _effects)
                {
                    DrawIcon(item, icon, flag);
                    ImGui.SameLine();
                }

                ImGui.NewLine();
            }
        }
    }

    private readonly RecordTable _recordTable;


    private void DrawRecordTab()
    {
        using var id  = ImGuiRaii.PushId("Fish Records");
        var       ret = ImGui.BeginTabItem("Fish Records");
        ImGuiUtil.HoverTooltip("The records of my fishing prowess have been greatly exaggerated.\n"
          + "Find, cleanup and share all data you have collected while fishing.");
        if (!ret)
            return;

        using var end = ImGuiRaii.DeferredEnd(ImGui.EndTabItem);
        _recordTable.Draw();
        if (ImGui.Button("Cleanup"))
        {
            _plugin.FishRecorder.RemoveDuplicates();
            _plugin.FishRecorder.RemoveInvalid();
        }

        ImGuiUtil.HoverTooltip("Delete all entries that were marked as invalid for some reason,\n"
          + "as well as all entries that have a duplicate (with the same content id and timestamp).\n"
          + "Usually, there should be none such entries.\n"
          + "Use at your own risk, no backup will be created automatically.");

        ImGui.SameLine();
        try
        {
            if (ImGui.Button("Copy to Clipboard"))
                ImGui.SetClipboardText(_plugin.FishRecorder.ExportBase64(0, _plugin.FishRecorder.Records.Count));
            ImGuiUtil.HoverTooltip("Export all fish records to your clipboard, to share them with other people. This may be a lot");
        }
        catch
        {
            // ignored
        }

        ImGui.SameLine();
        try
        {
            if (ImGui.Button("Import from Clipboard"))
                _plugin.FishRecorder.ImportBase64(ImGui.GetClipboardText());
            ImGuiUtil.HoverTooltip("Import a set of fish records shared with you from your clipboard. Should automatically skip duplicates.");
        }
        catch
        {
            // ignored
        }

        ImGui.SameLine();
        try
        {
            if (ImGui.Button("Export JSON"))
                ImGui.OpenPopup(RecordTable.FileNamePopup);
            ImGuiUtil.HoverTooltip("Given a path, export all records as a single JSON file.");
        }
        catch
        {
            // ignored
        }

        ImGui.SameLine();
        try
        {
            if (ImGui.Button("Copy Caught Fish JSON"))
            {
                var logFish = GatherBuddy.GameData.Fishes.Values.Where(f => f.InLog && f.FishingSpots.Count > 0).ToArray();
                var ids     = logFish.Where(f => GatherBuddy.FishLog.IsUnlocked(f)).Select(f => f.ItemId).ToArray();
                Communicator.PrintClipboardMessage("List of ", $"{ids.Length}/{logFish.Length} caught fish ");
                ImGui.SetClipboardText(JsonConvert.SerializeObject(ids, Formatting.Indented));
            }
        }
        catch
        {
            // ignored
        }

        var name = string.Empty;
        if (!ImGuiUtil.OpenNameField(RecordTable.FileNamePopup, ref name) || name.Length <= 0)
            return;

        try
        {
            var file = new FileInfo(name);
            _plugin.FishRecorder.ExportJson(file);
        }
        catch
        {
            // ignored
        }
    }
}
