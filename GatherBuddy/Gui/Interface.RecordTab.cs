using System.IO;
using System.Linq;
using System.Numerics;
using Dalamud.Interface;
using GatherBuddy.Config;
using GatherBuddy.FishTimer;
using GatherBuddy.Plugin;
using ImGuiNET;
using ImGuiOtter;
using ImGuiOtter.Table;
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
        private static readonly AmountHeader     _amountHeader     = new() { Label = "##" };
        private static readonly SizeHeader       _sizeHeader       = new() { Label = "Ilm" };
        private static readonly FlagHeader       _flagHeader       = new() { Label = "Flags" };

        private class GatheringHeader : HeaderConfigString<FishRecord>
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

        private class PerceptionHeader : HeaderConfigString<FishRecord>
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

        private class AmountHeader : HeaderConfigString<FishRecord>
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

        private class SizeHeader : HeaderConfigString<FishRecord>
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


        private class ContentIdHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord item)
                => item.Flags.HasFlag(FishRecord.Effects.Legacy) ? "Legacy" : item.ContentIdHash.ToString("X8");

            public override float Width
                => 75 * ImGuiHelpers.GlobalScale;

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.ContentIdHash.CompareTo(rhs.ContentIdHash);
        }

        private class BaitHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord item)
                => item.Bait.Name;

            public override float Width
                => 150 * ImGuiHelpers.GlobalScale;
        }

        private class SpotHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord item)
                => item.FishingSpot?.Name ?? "Unknown";

            public override float Width
                => 200 * ImGuiHelpers.GlobalScale;
        }

        private class CatchHeader : HeaderConfigString<FishRecord>
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

        private class CastStartHeader : HeaderConfigString<FishRecord>
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

        private class BiteTypeHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord item)
                => item.Tug.ToString();

            public override float Width
                => 60 * ImGuiHelpers.GlobalScale;
        }

        private class HookHeader : HeaderConfigString<FishRecord>
        {
            public override string ToName(FishRecord item)
                => item.Hook.ToString();

            public override float Width
                => 75 * ImGuiHelpers.GlobalScale;
        }

        private class DurationHeader : HeaderConfigString<FishRecord>
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
            public override float Width
                => 9 * (TextHeight + 1);

            public override bool FilterFunc(FishRecord item)
                => item.Flags.HasFlag(FilterValue);

            public override int Compare(FishRecord lhs, FishRecord rhs)
                => lhs.Flags.CompareTo(rhs.Flags);

            private void DrawIcon(FishRecord item, uint iconId, FishRecord.Effects flag)
            {
                var icon = Icons.DefaultStorage[iconId];
                var size = new Vector2(TextHeight, icon.Height * TextHeight / icon.Width);
                var tint = item.Flags.HasFlag(flag) ? Vector4.One : new Vector4(0.75f, 0.75f, 0.75f, 0.5f);
                ImGui.Image(icon.ImGuiHandle, size, Vector2.Zero, Vector2.One, tint);
                if (ImGui.IsItemHovered())
                {
                    using var tt = ImGuiRaii.NewTooltip();
                    ImGui.Image(icon.ImGuiHandle, new Vector2(icon.Width, icon.Height));
                    ImGui.Text(flag.ToString());
                }
            }

            public override void DrawColumn(FishRecord item, int idx)
            {
                using var space = ImGuiRaii.PushStyle(ImGuiStyleVar.ItemSpacing, Vector2.One);
                if (item.Flags.HasFlag(FishRecord.Effects.Patience2))
                    DrawIcon(item, 11106, FishRecord.Effects.Patience2);
                else
                    DrawIcon(item, 16023, FishRecord.Effects.Patience);
                ImGui.SameLine();
                DrawIcon(item, 11101, FishRecord.Effects.Intuition);
                ImGui.SameLine();
                DrawIcon(item, 11102, FishRecord.Effects.Snagging);
                ImGui.SameLine();
                DrawIcon(item, 11103, FishRecord.Effects.FishEyes);
                ImGui.SameLine();
                DrawIcon(item, 11104, FishRecord.Effects.Chum);
                ImGui.SameLine();
                DrawIcon(item, 11119, FishRecord.Effects.PrizeCatch);
                ImGui.SameLine();
                DrawIcon(item, 11116, FishRecord.Effects.IdenticalCast);
                ImGui.SameLine();
                DrawIcon(item, 11115, FishRecord.Effects.SurfaceSlap);
                ImGui.SameLine();
                DrawIcon(item, 11008, FishRecord.Effects.Collectible);
            }
        }
    }

    private readonly RecordTable _recordTable;


    private void DrawRecordTab()
    {
        using var id = ImGuiRaii.PushId("Fish Records");
        if (!ImGui.BeginTabItem("Fish Records"))
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
                Communicator.PrintClipboardMessage("List of ", $"{ids.Length}/{logFish.Length} caught fish");
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
