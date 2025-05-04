using Dalamud.Interface.Textures;
using Dalamud.Interface.Utility;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.Time;
using ImGuiNET;
using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using GatherBuddy.Models;
using OtterGui.Text;
using static GatherBuddy.Gui.Interface;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.FishTimer;

public partial class FishTimerWindow
{
    public static readonly ISharedImmediateTexture CollectableIcon =
        Icons.DefaultStorage.TextureProvider.GetFromGameIcon(new GameIconLookup(001110));

    public static readonly ISharedImmediateTexture DoubleHookIcon =
        Icons.DefaultStorage.TextureProvider.GetFromGameIcon(new GameIconLookup(001118));

    public static readonly ISharedImmediateTexture TripleHookIcon =
        Icons.DefaultStorage.TextureProvider.GetFromGameIcon(new GameIconLookup(001138));

    public static readonly ISharedImmediateTexture QuadHookIcon =
        Dalamud.Textures.GetFromManifestResource(Assembly.GetExecutingAssembly(), "QuadHookIcon.bmp");

    private readonly struct FishCache
    {
        private readonly ExtendedFish            _fish;
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
            _fish = ExtendedFishList.FirstOrDefault(f => f.Data == fish) ?? new ExtendedFish(fish);

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
            if (flags.HasFlag(Effects.Chum))
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
            if (fish.Predators.Length > 0 && !recorder.Record.Flags.HasFlag(Effects.Intuition))
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
             && (!flags.HasFlag(Effects.FishEyes) || fish.IsBigFish || hasWeatherRestriction))
                Unavailable = true;
            if (fish.Snagging == Snagging.Required && !flags.HasFlag(Effects.Snagging))
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

        public void DrawBackground(FishTimerWindow window, int i)
        {
            var ptr = ImGui.GetWindowDrawList();
            var pos = window._windowPos + ImGui.GetCursorPos();
            pos.Y += i * ImGui.GetFrameHeightWithSpacing();
            var size = window._windowSize with { Y = ImGui.GetFrameHeight() };
            // Background
            ImGui.GetWindowDrawList().AddRectFilled(pos, pos + size, _color.Value(), 0);

            // Markers and highlights.
            pos.X  += window._iconSize.X;
            size.X -= window._iconSize.X;
            DrawMarkers(ptr, pos, size.Y, size.X);
        }

        public void Draw(FishTimerWindow window)
        {
            var padding = 5 * ImGuiHelpers.GlobalScale;

            var timeString = NextUptime == TimeInterval.Always
                ? null
                : NextUptime.Start > GatherBuddy.Time.ServerTime
                    ? TimeInterval.DurationString(NextUptime.Start, GatherBuddy.Time.ServerTime, true)
                    : NextUptime.End < GatherBuddy.Time.ServerTime
                        ? "(ended)"
                        : TimeInterval.DurationString(NextUptime.End, GatherBuddy.Time.ServerTime, true);
            var textWidth = timeString is null ? 0 : ImUtf8.CalcTextSize(timeString).X;

            // Icon
            if (_icon.TryGetWrap(out var wrap, out _))
                ImGui.Image(wrap.ImGuiHandle, window._iconSize);
            else
                ImGui.Dummy(window._iconSize);

            var hovered = ImGui.IsItemHovered();

            // Name
            ImGui.SameLine(window._iconSize.X + padding);
            using var color       = ImRaii.PushColor(ImGuiCol.Text, ColorId.FishTimerText.Value());
            var       clipRectMin = ImGui.GetCursorScreenPos();
            var       clipRectMax = clipRectMin + ImGui.GetContentRegionAvail();
            var       collectible = _fish.Collectible && GatherBuddy.Config.ShowCollectableHints;
            var       multiHook   = _fish.DoubleHook > 1 && GatherBuddy.Config.ShowMultiHookHints;
            if (collectible)
                clipRectMax.X -= window._iconSize.X;
            if (multiHook)
                clipRectMax.X -= window._iconSize.X;
            if (textWidth > 0)
                clipRectMax.X -= textWidth + window._originalSpacing.X + padding;

            using (ImUtf8.PushClipRect(clipRectMin, clipRectMax))
            {
                ImUtf8.TextFrameAligned(_textLine);
            }

            hovered |= ImGui.IsItemHovered();

            if (hovered)
            {
                window._style.Push(ImGuiStyleVar.ItemSpacing, window._originalSpacing);
                _fish.SetTooltip(window._spot?.Territory ?? Territory.Invalid,
                    ImGuiHelpers.ScaledVector2(40, 40),
                    ImGuiHelpers.ScaledVector2(20, 20),
                    ImGuiHelpers.ScaledVector2(30, 30), true);
                window._style.Pop();
            }

            if (multiHook)
            {
                ImGui.SameLine(window._windowSize.X - window._iconSize.X);

                var hookIcon = _fish.DoubleHook switch
                {
                    2 => DoubleHookIcon,
                    3 => TripleHookIcon,
                    4 => QuadHookIcon,
                    _ => null,
                };

                if (hookIcon?.TryGetWrap(out var wrap2, out _) ?? false)
                {
                    ImGui.Image(wrap2.ImGuiHandle, window._iconSize);
                    if (ImGui.IsItemHovered())
                    {
                        using var tooltip = ImRaii.Tooltip();
                        window._style.Push(ImGuiStyleVar.ItemSpacing, window._originalSpacing);
                        ImUtf8.Text($"Double Hook for {_fish.DoubleHook} fish.");
                        ImUtf8.Text($"Triple Hook for {2 * _fish.DoubleHook - 1} fish.");
                        window._style.Pop();
                    }
                }
                else
                {
                    ImGui.Dummy(window._iconSize);
                }
            }

            // Collectable Icon
            if (collectible)
            {
                var tint = Dalamud.ClientState.LocalPlayer?.StatusList.Any(s => s.StatusId is 805) is true
                    ? Vector4.One
                    : new Vector4(0.75f, 0.75f, 0.75f, 0.5f);

                ImGui.SameLine(window._windowSize.X - window._iconSize.X - (multiHook ? window._iconSize.X : 0));
                if (CollectableIcon.TryGetWrap(out var wrap3, out _))
                    ImGui.Image(wrap3.ImGuiHandle, window._iconSize, Vector2.Zero, Vector2.One, tint);
                else
                    ImGui.Dummy(window._iconSize);
            }

            // Time
            if (timeString is null)
                return;

            var offset = ImGui.CalcTextSize(timeString).X + (collectible ? window._iconSize.X : 0) + (multiHook ? window._iconSize.X : 0);
            ImGui.SameLine(window._windowSize.X - offset - padding);
            ImUtf8.TextFrameAligned(timeString);
        }
    }
}
