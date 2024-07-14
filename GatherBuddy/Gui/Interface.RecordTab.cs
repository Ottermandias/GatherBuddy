using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.FishTimer;
using GatherBuddy.Plugin;
using ImGuiNET;
using OtterGui;
using OtterGui.Table;
using Newtonsoft.Json;
using ImRaii = OtterGui.Raii.ImRaii;
using System.Text;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Utility;
using GatherBuddy.Time;
using GatherBuddy.Weather;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private sealed class RecordTable : Table<FishRecord>
    {
        public const string FileNamePopup = "FileNamePopup";

        public RecordTable()
            : base("Fish Records", _plugin.FishRecorder.Records, _catchHeader, _baitHeader, _durationHeader, _castStartHeader,
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

        private sealed class GatheringHeader : ColumnString<FishRecord>
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

        private sealed class PerceptionHeader : ColumnString<FishRecord>
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

        private sealed class AmountHeader : ColumnString<FishRecord>
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

        private sealed class SizeHeader : ColumnString<FishRecord>
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
                using var color = ImRaii.PushColor(ImGuiCol.Text, ColorId.DisabledText.Value(), tt.Length == 0);
                ImGuiUtil.RightAlign(ToName(record));
                ImGuiUtil.HoverTooltip(tt);
            }
        }


        private sealed class ContentIdHeader : ColumnString<FishRecord>
        {
            public override string ToName(FishRecord item)
                => item.Flags.HasFlag(FishRecord.Effects.Legacy) ? "Legacy" : item.ContentIdHash.ToString("X8");

            public override float Width
                => 75 * ImGuiHelpers.GlobalScale;

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.ContentIdHash.CompareTo(rhs.ContentIdHash);
        }

        private sealed class BaitHeader : ColumnString<FishRecord>
        {
            public override string ToName(FishRecord item)
                => item.Bait.Name;

            public override float Width
                => 150 * ImGuiHelpers.GlobalScale;
        }

        private sealed class SpotHeader : ColumnString<FishRecord>
        {
            public override string ToName(FishRecord item)
                => item.FishingSpot?.Name ?? "Unknown";

            public override float Width
                => 200 * ImGuiHelpers.GlobalScale;
        }

        private sealed class CatchHeader : ColumnString<FishRecord>
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

        private sealed class CastStartHeader : ColumnString<FishRecord>
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

        private sealed class BiteTypeHeader : ColumnFlags<TugTypeFilter, FishRecord>
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

        private sealed class HookHeader : ColumnFlags<HookSetFilter, FishRecord>
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

        private sealed class DurationHeader : ColumnString<FishRecord>
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

        private class FlagHeader : ColumnFlags<FlagHeader.ColumnEffects, FishRecord>
        {
            private          float                                           _iconScale;
            private readonly (ISharedImmediateTexture, FishRecord.Effects)[] _effects;

            [Flags]
            public enum ColumnEffects : ulong
            {
                PatienceOn        = FishRecord.Effects.Patience,
                PatienceOff       = (ulong)FishRecord.Effects.Patience << 32,
                Patience2On       = FishRecord.Effects.Patience2,
                Patience2Off      = (ulong)FishRecord.Effects.Patience2 << 32,
                IntuitionOn       = FishRecord.Effects.Intuition,
                IntuitionOff      = (ulong)FishRecord.Effects.Intuition << 32,
                SnaggingOn        = FishRecord.Effects.Snagging,
                SnaggingOff       = (ulong)FishRecord.Effects.Snagging << 32,
                FishEyesOn        = FishRecord.Effects.FishEyes,
                FishEyesOff       = (ulong)FishRecord.Effects.FishEyes << 32,
                ChumOn            = FishRecord.Effects.Chum,
                ChumOff           = (ulong)FishRecord.Effects.Chum << 32,
                PrizeCatchOn      = FishRecord.Effects.PrizeCatch,
                PrizeCatchOff     = (ulong)FishRecord.Effects.PrizeCatch << 32,
                IdenticalCastOn   = FishRecord.Effects.IdenticalCast,
                IdenticalCastOff  = (ulong)FishRecord.Effects.IdenticalCast << 32,
                SurfaceSlapOn     = FishRecord.Effects.SurfaceSlap,
                SurfaceSlapOff    = (ulong)FishRecord.Effects.SurfaceSlap << 32,
                CollectibleOn     = FishRecord.Effects.Collectible,
                CollectibleOff    = (ulong)FishRecord.Effects.Collectible << 32,
                BigGameFishingOn  = FishRecord.Effects.BigGameFishing,
                BigGameFishingOff = (ulong)FishRecord.Effects.BigGameFishing << 32,
                AmbitiousLureOn   = FishRecord.Effects.AmbitiousLure1 | FishRecord.Effects.AmbitiousLure2,
                AmbitiousLureOff  = (ulong)(FishRecord.Effects.AmbitiousLure1 | FishRecord.Effects.AmbitiousLure2) << 32,
                ModestLureOn      = FishRecord.Effects.ModestLure1 | FishRecord.Effects.ModestLure2,
                ModestLureOff     = (ulong)(FishRecord.Effects.ModestLure1 | FishRecord.Effects.ModestLure2) << 32,
            }

            private static readonly ColumnEffects Mask = Enum.GetValues<ColumnEffects>().Aggregate((a, b) => a | b);

            private static readonly ColumnEffects[] _values =
            [
                ColumnEffects.PatienceOn,
                ColumnEffects.PatienceOff,
                ColumnEffects.Patience2On,
                ColumnEffects.Patience2Off,
                ColumnEffects.IntuitionOn,
                ColumnEffects.IntuitionOff,
                ColumnEffects.SnaggingOn,
                ColumnEffects.SnaggingOff,
                ColumnEffects.FishEyesOn,
                ColumnEffects.FishEyesOff,
                ColumnEffects.ChumOn,
                ColumnEffects.ChumOff,
                ColumnEffects.PrizeCatchOn,
                ColumnEffects.PrizeCatchOff,
                ColumnEffects.IdenticalCastOn,
                ColumnEffects.IdenticalCastOff,
                ColumnEffects.SurfaceSlapOn,
                ColumnEffects.SurfaceSlapOff,
                ColumnEffects.CollectibleOn,
                ColumnEffects.CollectibleOff,
                ColumnEffects.BigGameFishingOn,
                ColumnEffects.BigGameFishingOff,
                ColumnEffects.AmbitiousLureOn,
                ColumnEffects.AmbitiousLureOff,
                ColumnEffects.ModestLureOn,
                ColumnEffects.ModestLureOff,
            ];

            private static readonly string[] _names =
            [
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
                "Big Game Fishing On",
                "Big Game Fishing Off",
                "Ambitious Lure On",
                "Ambitious Lure Off",
                "Modest Lure On",
                "Modest Lure Off",
            ];

            protected override IReadOnlyList<ColumnEffects> Values
                => _values;

            protected override string[] Names
                => _names;

            protected override void SetValue(ColumnEffects value, bool enable)
            {
                if (enable)
                    _filter |= value;
                else
                    _filter &= ~value;
            }

            private ColumnEffects _filter;

            public FlagHeader()
            {
                _effects =
                [
                    (Icons.DefaultStorage.TextureProvider.GetFromGameIcon(16023), (FishRecord.Effects)_values[0]),
                    (Icons.DefaultStorage.TextureProvider.GetFromGameIcon(11106), (FishRecord.Effects)_values[2]),
                    (Icons.DefaultStorage.TextureProvider.GetFromGameIcon(11101), (FishRecord.Effects)_values[4]),
                    (Icons.DefaultStorage.TextureProvider.GetFromGameIcon(11102), (FishRecord.Effects)_values[6]),
                    (Icons.DefaultStorage.TextureProvider.GetFromGameIcon(11103), (FishRecord.Effects)_values[8]),
                    (Icons.DefaultStorage.TextureProvider.GetFromGameIcon(11104), (FishRecord.Effects)_values[10]),
                    (Icons.DefaultStorage.TextureProvider.GetFromGameIcon(11119), (FishRecord.Effects)_values[12]),
                    (Icons.DefaultStorage.TextureProvider.GetFromGameIcon(11116), (FishRecord.Effects)_values[14]),
                    (Icons.DefaultStorage.TextureProvider.GetFromGameIcon(11115), (FishRecord.Effects)_values[16]),
                    (Icons.DefaultStorage.TextureProvider.GetFromGameIcon(11008), (FishRecord.Effects)_values[18]),
                    (Icons.DefaultStorage.TextureProvider.GetFromGameIcon(11122), (FishRecord.Effects)_values[20]),
                ];
                AllFlags = Mask;
                _filter  = AllFlags;
            }

            public override float Width
            {
                get
                {
                    if (_iconScale == 0)
                    {
                        var scale = _effects[0].Item1.TryGetWrap(out var wrap, out _) ? (float)wrap.Width / wrap.Height : 0;
                        if (scale == 0)
                            return 10 * (TextHeight + 1);

                        _iconScale = scale;
                    }

                    return 13 * (_iconScale * TextHeight + 1);
                }
            }

            public override bool FilterFunc(FishRecord item)
            {
                var enabled  = (FishRecord.Effects)(_filter & Mask);
                var disabled = (FishRecord.Effects)(((ulong)_filter >> 32) & (ulong)Mask);
                var flags    = item.Flags & (FishRecord.Effects)Mask;
                var invFlags = ~flags & (FishRecord.Effects)Mask;
                return (flags & enabled) == flags && (invFlags & disabled) == invFlags;
            }

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.Flags.CompareTo(rhs.Flags);

            public override ColumnEffects FilterValue
                => _filter;

            private void DrawIcon(FishRecord item, ISharedImmediateTexture icon, FishRecord.Effects flag)
                => DrawIcon(icon, item.Flags.HasFlag(flag), flag.ToString());

            private void DrawIcon(ISharedImmediateTexture icon, bool enabled, string tooltip)
            {
                var size = new Vector2(TextHeight * _iconScale, TextHeight);
                var tint = enabled ? Vector4.One : new Vector4(0.75f, 0.75f, 0.75f, 0.5f);
                if (!icon.TryGetWrap(out var wrap, out _))
                {
                    ImGui.Dummy(size);
                    return;
                }

                ImGui.Image(wrap.ImGuiHandle, size, Vector2.Zero, Vector2.One, tint);
                if (!ImGui.IsItemHovered())
                    return;

                using var tt = ImRaii.Tooltip();
                ImGui.Image(wrap.ImGuiHandle, new Vector2(wrap.Width, wrap.Height));
                ImGui.TextUnformatted(tooltip);
            }

            public override void DrawColumn(FishRecord item, int idx)
            {
                using var space = ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, Vector2.One);
                foreach (var (icon, flag) in _effects)
                {
                    DrawIcon(item, icon, flag);
                    ImGui.SameLine();
                }

                switch (item.Flags.AmbitiousLure())
                {
                    case 0:
                        DrawIcon(Icons.DefaultStorage.TextureProvider.GetFromGameIcon(18905), false, "Ambitious Lure");
                        ImGui.SameLine();
                        switch (item.Flags.ModestLure())
                        {
                            case 0:
                                DrawIcon(Icons.DefaultStorage.TextureProvider.GetFromGameIcon(18909), false, "Modest Lure");
                                break;
                            case 1:
                                DrawIcon(Icons.DefaultStorage.TextureProvider.GetFromGameIcon(18909), true, "Modest Lure");
                                break;
                            case 2:
                                DrawIcon(Icons.DefaultStorage.TextureProvider.GetFromGameIcon(18910), true, "Modest Lure");
                                break;
                            case 3:
                                DrawIcon(Icons.DefaultStorage.TextureProvider.GetFromGameIcon(18911), true, "Modest Lure");
                                break;
                        }

                        return;
                    case 1:
                        DrawIcon(Icons.DefaultStorage.TextureProvider.GetFromGameIcon(18905), true, "Ambitious Lure");
                        break;
                    case 2:
                        DrawIcon(Icons.DefaultStorage.TextureProvider.GetFromGameIcon(18906), true, "Ambitious Lure");
                        break;
                    case 3:
                        DrawIcon(Icons.DefaultStorage.TextureProvider.GetFromGameIcon(18907), true, "Ambitious Lure");
                        break;
                }

                ImGui.SameLine();
                DrawIcon(Icons.DefaultStorage.TextureProvider.GetFromGameIcon(18909), false, "Modest Lure");
            }
        }

        public string CreateTsv()
        {
            var sb = new StringBuilder(Items.Count * 128);
            sb.Append(
                "Fish\tFishId\tBite\tBait\tBaitId\tSpot\tSpotId\tTug\tHookset\tTimestamp\tEorzea Time\tTransition\tWeather\tAmount\tIlm\tGathering\tPerception\tPatience\tPatience2\tIntuition\tSnagging\tFish Eyes\tChum\tPrize Catch\tIdentical Cast\tSurface Slap\tCollectible\tBig Game Fishing\tAmbitious Lure\tModest Lure\n");
            foreach (var record in Items.OrderBy(r => r.TimeStamp))
            {
                var (hour, minute) = record.TimeStamp.CurrentEorzeaTimeOfDay();
                var spot = record.FishingSpot;
                var (weather, transition) = ("Unknown", "Unknown");
                if (spot != null)
                {
                    var weathers = WeatherManager.GetForecast(spot.Territory, 2, record.TimeStamp.AddEorzeaHours(-8));
                    transition = weathers[0].Weather.Name;
                    weather    = weathers[1].Weather.Name;
                }

                sb.Append(_catchHeader.ToName(record)).Append('\t')
                    .Append(record.CatchId).Append('\t')
                    .Append(_durationHeader.ToName(record)).Append('\t')
                    .Append(_baitHeader.ToName(record)).Append('\t')
                    .Append(record.BaitId).Append('\t')
                    .Append(_spotHeader.ToName(record)).Append('\t')
                    .Append(record.SpotId).Append('\t')
                    .Append(record.Tug.ToString()).Append('\t')
                    .Append(record.Hook.ToString()).Append('\t')
                    .Append(_castStartHeader.ToName(record)).Append('\t')
                    .Append($"{hour}:{minute:D2}").Append('\t')
                    .Append(transition).Append('\t')
                    .Append(weather).Append('\t')
                    .Append(_amountHeader.ToName(record)).Append('\t')
                    .Append(_sizeHeader.ToName(record)).Append('\t')
                    .Append(_gatheringHeader.ToName(record)).Append('\t')
                    .Append(_perceptionHeader.ToName(record)).Append('\t')
                    .Append(record.Flags.HasFlag(FishRecord.Effects.Patience) ? "x\t" : "\t")
                    .Append(record.Flags.HasFlag(FishRecord.Effects.Patience2) ? "x\t" : "\t")
                    .Append(record.Flags.HasFlag(FishRecord.Effects.Intuition) ? "x\t" : "\t")
                    .Append(record.Flags.HasFlag(FishRecord.Effects.Snagging) ? "x\t" : "\t")
                    .Append(record.Flags.HasFlag(FishRecord.Effects.FishEyes) ? "x\t" : "\t")
                    .Append(record.Flags.HasFlag(FishRecord.Effects.Chum) ? "x\t" : "\t")
                    .Append(record.Flags.HasFlag(FishRecord.Effects.PrizeCatch) ? "x\t" : "\t")
                    .Append(record.Flags.HasFlag(FishRecord.Effects.IdenticalCast) ? "x\t" : "\t")
                    .Append(record.Flags.HasFlag(FishRecord.Effects.SurfaceSlap) ? "x\t" : "\t")
                    .Append(record.Flags.HasFlag(FishRecord.Effects.Collectible) ? "x\t" : "\t")
                    .Append(record.Flags.HasFlag(FishRecord.Effects.BigGameFishing) ? "x\t" : "\t")
                    .Append($"{record.Flags.AmbitiousLure()}\t")
                    .Append($"{record.Flags.ModestLure()}\t")
                    .Append('\n');
            }

            return sb.ToString();
        }
    }

    private readonly RecordTable _recordTable;
    private          bool        WriteTsv  = false;
    private          bool        WriteJson = false;


    private void DrawRecordTab()
    {
        using var id  = ImRaii.PushId("Fish Records");
        using var tab = ImRaii.TabItem("Fish Records");
        ImGuiUtil.HoverTooltip("The records of my fishing prowess have been greatly exaggerated.\n"
          + "Find, cleanup and share all data you have collected while fishing.");
        if (!tab)
            return;

        _recordTable.Draw(ImGui.GetTextLineHeightWithSpacing());

        var textSize = ImGui.CalcTextSize("00000000") with { Y = 0 };
        if (_recordTable.CurrentItems != _recordTable.TotalItems)
            ImGuiUtil.DrawTextButton($"{_recordTable.CurrentItems}", textSize, ImGui.GetColorU32(ImGuiCol.Button),
                ColorId.AvailableItem.Value());
        else
            ImGuiUtil.DrawTextButton($"{_recordTable.CurrentItems}", textSize, ImGui.GetColorU32(ImGuiCol.Button));
        ImGui.SameLine();
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
                ImGui.SetClipboardText(_plugin.FishRecorder.ExportBase64());
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
            {
                ImGui.OpenPopup(RecordTable.FileNamePopup);
                WriteJson = true;
            }

            ImGuiUtil.HoverTooltip("Given a path, export all records as a single JSON file.");
        }
        catch
        {
            // ignored
        }

        ImGui.SameLine();
        try
        {
            if (ImGui.Button("Export TSV"))
            {
                ImGui.OpenPopup(RecordTable.FileNamePopup);
                WriteTsv = true;
            }

            ImGuiUtil.HoverTooltip("Given a path, export all records as a single TSV file.");
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

        if (WriteJson)
        {
            try
            {
                var file = new FileInfo(name);
                _plugin.FishRecorder.ExportJson(file);
            }
            catch
            {
                // ignored
            }

            WriteJson = false;
        }

        if (WriteTsv)
        {
            try
            {
                var data = _recordTable.CreateTsv();
                File.WriteAllText(name, data);
                GatherBuddy.Log.Information($"Exported {_recordTable.TotalItems} fish records to {name}.");
            }
            catch (Exception e)
            {
                GatherBuddy.Log.Warning($"Could not export tsv file to {name}:\n{e}");
            }

            WriteTsv = false;
        }
    }
}
