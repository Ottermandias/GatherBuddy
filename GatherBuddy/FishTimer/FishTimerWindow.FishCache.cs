using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Utility;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.Time;
using ImGuiNET;
using static GatherBuddy.Gui.Interface;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.FishTimer;

public partial class FishTimerWindow
{
    private readonly struct FishCache
    {
        private readonly ExtendedFish?           _fish;
        private readonly string                  _textLine;
        private readonly ISharedImmediateTexture _icon;
        private readonly FishRecordTimes.Times   _all;
        private readonly FishRecordTimes.Times   _baitSpecific;
        private readonly ColorId                 _color;
        public readonly  bool                    Uncaught;
        public readonly  bool                    Unavailable;
        public readonly  ulong                   SortOrder;
        public readonly  TimeInterval            NextUptime;


        private static ulong MakeSortOrder(ushort min, ushort max)
            => ((ulong)min << 16) | max;

        private static ColorId FromData(BiteType bite, HookSet hook, bool uncaught, bool unavailable)
        {
            if (unavailable)
                return ColorId.FishTimerUnavailable;

            return (bite, uncaught) switch
            {
                (BiteType.Weak, false)   => ColorId.FishTimerWeakTug,
                (BiteType.Weak, true)    => ColorId.FishTimerWeakTugUncaught,
                (BiteType.Strong, false) => ColorId.FishTimerStrongTug,
                (BiteType.Strong, true)  => ColorId.FishTimerStrongTugUncaught,

                (BiteType.Legendary, false) => hook == HookSet.Precise
                    ? ColorId.FishTimerLegendaryTugPrecision
                    : ColorId.FishTimerLegendaryTugPowerful,

                (BiteType.Legendary, true) => hook == HookSet.Precise
                    ? ColorId.FishTimerLegendaryTugPrecisionUncaught
                    : ColorId.FishTimerLegendaryTugPowerfulUncaught,

                _ => ColorId.FishTimerUnknown,
            };
        }

        public FishCache(FishRecorder recorder, Fish fish, FishingSpot spot)
        {
            _fish = ExtendedFishList.FirstOrDefault(f => f.Data == fish);

            // Get All and Bait Times and set caught-with-bait information.
            _all          = new FishRecordTimes.Times();
            _baitSpecific = new FishRecordTimes.Times();
            Uncaught      = true;
            if (recorder.Times.TryGetValue(fish.ItemId, out var times))
            {
                _all = times.All;
                if (times.Data.TryGetValue(recorder.Record.BaitId, out var baitTimes))
                {
                    _baitSpecific = baitTimes;
                    Uncaught      = false;
                }
            }

            // If using Chum, only use Chum times. Sort Order prioritizes earlier bite times, then shorter windows.
            var flags = recorder.Record.Flags;
            if (flags.HasFlag(FishRecord.Effects.Chum))
            {
                SortOrder         = MakeSortOrder(Math.Min(_all.MinChum, _baitSpecific.MinChum), Math.Max(_all.MaxChum, _baitSpecific.MaxChum));
                _baitSpecific.Max = _baitSpecific.MaxChum;
                _baitSpecific.Min = _baitSpecific.MinChum;
                _all.Max          = _all.MaxChum;
                _all.Min          = _all.MinChum;
            }
            else
            {
                SortOrder = MakeSortOrder(Math.Min(_all.Min, _baitSpecific.Min), Math.Max(_all.Max, _baitSpecific.Max));
            }

            // Uncaught fish should always be behind caught fish.
            if (Uncaught)
                SortOrder |= 1ul << 33;

            _icon       = Icons.DefaultStorage.TextureProvider.GetFromGameIcon(new GameIconLookup(fish.ItemData.Icon));
            Unavailable = false;
            if (fish.Predators.Length > 0 && !recorder.Record.Flags.HasFlag(FishRecord.Effects.Intuition))
            {
                Unavailable = true;
                SortOrder   = ulong.MaxValue;
            }

            NextUptime = TimeInterval.Always;
            _textLine  = fish.Name[GatherBuddy.Language];

            var uptime = GatherBuddy.UptimeManager.NextUptime(fish, spot.Territory, GatherBuddy.Time.ServerTime);
            if (GatherBuddy.Config.ShowFishTimerUptimes && uptime != TimeInterval.Invalid && uptime != TimeInterval.Never)
                NextUptime = uptime;
            // Some non-spectral ocean fish have weather restrictions, but this is not handled.
            var hasWeatherRestriction = fish.FishRestrictions.HasFlag(FishRestrictions.Weather) && !fish.OceanFish;
            if (GatherBuddy.Time.ServerTime < uptime.Start
             && (!flags.HasFlag(FishRecord.Effects.FishEyes) || fish.IsBigFish || hasWeatherRestriction))
                Unavailable = true;
            if (fish.Snagging == Snagging.Required && !flags.HasFlag(FishRecord.Effects.Snagging))
                Unavailable = true;
            // Unavailable fish should be last.
            if (Unavailable)
                SortOrder = ulong.MaxValue;

            _color = FromData(fish.BiteType, fish.HookSet, Uncaught, Unavailable);
        }

        private void DrawMarkers(ImDrawListPtr ptr, Vector2 pos, float height, float size)
        {
            var difference      = 2 * ImGuiHelpers.GlobalScale;
            var difference2     = 2 * difference;
            var offsetStartBait = size * _baitSpecific.Min / GatherBuddy.Config.FishTimerScale;
            var offsetEndBait   = size * _baitSpecific.Max / GatherBuddy.Config.FishTimerScale;
            var offsetStartAll  = size * _all.Min / GatherBuddy.Config.FishTimerScale;
            var offsetEndAll    = size * _all.Max / GatherBuddy.Config.FishTimerScale;

            // Do not use All-markers if they would be the same as Bait-markers
            if (Math.Abs(offsetStartBait - offsetStartAll) < difference2)
                offsetStartAll = -1;

            if (Math.Abs(offsetEndAll - offsetEndBait) < difference2)
                offsetEndAll = -1;

            // If the distance between bait-min and bait-max is too low, add a bit room between them.
            if (Math.Abs(offsetEndBait - offsetStartBait) < difference2)
            {
                offsetStartBait -= difference;
                offsetEndBait   += difference;
            }

            // Highlight via a shaded front drop.
            var areaEnd   = offsetEndBait < 0 || offsetEndBait > size - difference2 ? size : offsetEndBait;
            var areaStart = offsetStartBait < 0 || offsetStartBait > size - difference2 ? 0 : offsetStartBait;
            if (areaStart > 0 || areaEnd < size)
            {
                var begin = new Vector2(pos.X + areaStart, pos.Y);
                var end   = new Vector2(pos.X + areaEnd,   pos.Y + height);
                ptr.AddRectFilled(begin, end, 0x40000000, 0);
            }

            // Draw marker lines only if they are useful.
            void DrawLine(float offset, uint color)
            {
                if (offset < difference2 || offset > size - difference2)
                    return;

                var begin = new Vector2(pos.X + offset, pos.Y);
                var end   = new Vector2(begin.X,        begin.Y + height);
                ptr.AddLine(begin, end, color, difference);
            }

            DrawLine(offsetStartBait, ColorId.FishTimerMarkersBait.Value());
            DrawLine(offsetEndBait,   ColorId.FishTimerMarkersBait.Value());
            DrawLine(offsetStartAll,  ColorId.FishTimerMarkersAll.Value());
            DrawLine(offsetEndAll,    ColorId.FishTimerMarkersAll.Value());
        }

        public void Draw(FishTimerWindow window)
        {
            var padding = 5 * ImGuiHelpers.GlobalScale;
            var pos     = window._windowPos + ImGui.GetCursorPos();
            var size    = window._windowSize with { Y = ImGui.GetFrameHeight() };
            var ptr     = ImGui.GetWindowDrawList();

            // Background
            ptr.AddRectFilled(pos, pos + size, _color.Value(), 0);

            // Markers and highlights.
            pos.X  += window._iconSize.X;
            size.X -= window._iconSize.X;
            DrawMarkers(ptr, pos, size.Y, size.X);

            // Icon
            if (_icon.TryGetWrap(out var wrap, out _))
                ImGui.Image(wrap.ImGuiHandle, window._iconSize);
            else
                ImGui.Dummy(window._iconSize);

            var hovered = ImGui.IsItemHovered();

            // Name
            ImGui.SameLine(window._iconSize.X + padding);
            ImGui.AlignTextToFramePadding();
            using var color = ImRaii.PushColor(ImGuiCol.Text, ColorId.FishTimerText.Value());
            ImGui.Text(_textLine);
            hovered |= ImGui.IsItemHovered();

            if (hovered && _fish != null)
            {
                window._style.Pop();
                _fish.SetTooltip(window._spot?.Territory ?? Territory.Invalid,
                    ImGuiHelpers.ScaledVector2(40, 40),
                    ImGuiHelpers.ScaledVector2(20, 20),
                    ImGuiHelpers.ScaledVector2(30, 30));
                window._style.Push(ImGuiStyleVar.ItemSpacing, window._itemSpacing);
            }

            // Time
            if (NextUptime == TimeInterval.Always)
                return;

            var timeString =
                NextUptime.Start > GatherBuddy.Time.ServerTime
                    ? TimeInterval.DurationString(NextUptime.Start, GatherBuddy.Time.ServerTime, true)
                    : NextUptime.End < GatherBuddy.Time.ServerTime
                        ? "(ended)"
                        : TimeInterval.DurationString(NextUptime.End, GatherBuddy.Time.ServerTime, true);
            var offset = ImGui.CalcTextSize(timeString).X;
            ImGui.SameLine(window._windowSize.X - offset - padding);
            ImGui.AlignTextToFramePadding();
            ImGui.Text(timeString);
        }
    }
}
