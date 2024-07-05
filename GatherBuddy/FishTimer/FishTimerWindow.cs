using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using GatherBuddy.Config;
using GatherBuddy.Gui;
using GatherBuddy.SeFunctions;
using ImGuiNET;
using OtterGui;
using OtterGui.Raii;
using FishingSpot = GatherBuddy.Classes.FishingSpot;
using TimeStamp = GatherBuddy.Time.TimeStamp;

namespace GatherBuddy.FishTimer;

public partial class FishTimerWindow : Window
{
    private const ImGuiWindowFlags EditFlags =
        ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar;

    private const ImGuiWindowFlags NormalFlags = EditFlags
      | ImGuiWindowFlags.NoDecoration
      | ImGuiWindowFlags.NoResize
      | ImGuiWindowFlags.NoMove
      | ImGuiWindowFlags.NoNavFocus;

    private          FishingSpot? _spot;
    private          FishCache[]  _availableFish    = Array.Empty<FishCache>();
    private          TimeStamp    _nextUptimeChange = TimeStamp.MaxValue;
    private readonly FishRecorder _recorder;
    private readonly int          _maxNumLines = GatherBuddy.GameData.FishingSpots.Values.Where(f => !f.Spearfishing).Max(f => f.Items.Length);
    private readonly ImRaii.Style _style       = new();

    private float   _lineHeight;
    private Vector2 _iconSize    = Vector2.Zero;
    private Vector2 _itemSpacing = Vector2.Zero;
    private Vector2 _windowPos   = Vector2.Zero;
    private Vector2 _windowSize  = Vector2.Zero;
    private float   _textMargin  = 5 * ImGuiHelpers.GlobalScale;
    private float   _textLines;
    private float   _maxListHeight;
    private float   _listHeight;
    private int     _milliseconds;
    private string? _spotName;


    public FishTimerWindow(FishRecorder recorder)
        : base("##FishingTimer")
    {
        _recorder          = recorder;
        IsOpen             = GatherBuddy.Config.ShowFishTimer;
        Namespace          = "FishingTimer";
        RespectCloseHotkey = false;
    }

    private static float Rounding
        => 4 * ImGuiHelpers.GlobalScale;

    private void DrawEditModeTimer()
    {
        static void DrawCenteredText(float xSize, string text)
        {
            var textSize = ImGui.CalcTextSize(text).X;
            ImGui.SetCursorPosX((xSize - textSize) / 2);
            ImGui.Text(text);
        }

        ImGui.GetWindowDrawList().AddRectFilled(_windowPos + _textLines * Vector2.UnitY, _windowPos + _windowSize,
            ColorId.FishTimerBackground.Value(), Rounding);
        ImGui.GetWindowDrawList().AddRect(_windowPos, _windowPos + _windowSize, ColorId.FishTimerMarkersAll.Value(), Rounding);
        ImGui.SetCursorPosY((_windowSize.Y - 6 * ImGui.GetTextLineHeightWithSpacing()) / 2);
        DrawCenteredText(_windowSize.X, "FISH");
        ImGui.SetCursorPosY((_windowSize.Y - 4 * ImGui.GetTextLineHeightWithSpacing()) / 2);
        DrawCenteredText(_windowSize.X, "TIMER");
        DrawCenteredText(_windowSize.X, "\nDisable \"Edit Fish Timer\"");
        DrawCenteredText(_windowSize.X, "in /gatherbuddy -> Config");
        DrawCenteredText(_windowSize.X, "-> Interface -> Fish Timer Window");
        DrawCenteredText(_windowSize.X, "to enable actual functionality");
        DrawCenteredText(_windowSize.X, "and hide this when not fishing.");
    }

    private void DrawProgressLine()
    {
        if (_milliseconds == 0)
            return;

        var diff  = _windowPos.X + _iconSize.X + 2 + (_windowSize.X - _iconSize.X) * _milliseconds / GatherBuddy.Config.FishTimerScale;
        var start = new Vector2(diff, _windowPos.Y + _textLines);
        var end   = new Vector2(diff, _windowPos.Y + _listHeight - 2 * ImGuiHelpers.GlobalScale);
        ImGui.GetWindowDrawList()
            .AddLine(start, end, ColorId.FishTimerProgress.Value(), 3 * ImGuiHelpers.GlobalScale);
    }

    private void DrawSecondLines()
    {
        if (GatherBuddy.Config.ShowSecondIntervals == 0)
            return;

        var increment = (_windowSize.X - _iconSize.X) / (GatherBuddy.Config.ShowSecondIntervals + 1);
        var baseLine  = _windowPos with { X = _windowPos.X + _iconSize.X + 2 };
        var drawList  = ImGui.GetWindowDrawList();
        var time      = GatherBuddy.Config.FishTimerScale / (GatherBuddy.Config.ShowSecondIntervals + 1);
        var scale     = Vector2.UnitX * ImGuiHelpers.GlobalScale;
        for (byte i = 1; i <= GatherBuddy.Config.ShowSecondIntervals; ++i)
        {
            var start = baseLine + new Vector2(increment * i, _textLines);
            var end   = start with { Y = baseLine.Y + _listHeight - 2 * ImGuiHelpers.GlobalScale };
            drawList.AddLine(start - scale, end - scale, 0x80000000, ImGuiHelpers.GlobalScale);
            drawList.AddLine(start,         end,         0xFFFFFFFF, ImGuiHelpers.GlobalScale);
            drawList.AddLine(start + scale, end + scale, 0x80000000, ImGuiHelpers.GlobalScale);
            var t = (i * time / 1000).ToString();
            ImGuiUtil.TextShadowed(ImGui.GetWindowDrawList(), end with { X = end.X - ImGui.CalcTextSize(t).X / 2 }, t, 0xFFFFFFFF, 0x80000000);
        }
    }

    private void DrawTextHeader(string bait, string spot, int milliseconds)
    {
        var drawList = ImGui.GetWindowDrawList();
        drawList.AddRectFilled(_windowPos, _windowPos + new Vector2(_windowSize.X, _textLines),
            ColorId.FishTimerBackground.Value(), 4 * ImGuiHelpers.GlobalScale);
        ImGui.SetCursorPosX(_textMargin);
        ImGui.TextUnformatted(bait);
        Interface.CreateContextMenu(_recorder.Record.Bait);
        ImGui.SetCursorPosX(_textMargin);
        ImGui.TextUnformatted(spot);
        Interface.CreateContextMenu(_spot);

        switch (milliseconds)
        {
            case 0: return;
            case -1:
                const string text = "Elapsed Time";
                ImGui.SameLine(_windowSize.X - ImGui.CalcTextSize(text).X - _textMargin);
                ImGui.Text(text);
                return;
            default:
                var secondText = (_milliseconds / 1000.0).ToString("00.0");
                ImGui.SameLine(_windowSize.X - ImGui.CalcTextSize(secondText).X - _textMargin);
                ImGui.Text(secondText);
                return;
        }
    }

    private string EllipsifyString(string text, float maxWidth)
    {
        if (ImGui.CalcTextSize(text).X < maxWidth)
            return text;

        maxWidth -= ImGui.CalcTextSize("...").X;
        var length = Math.Max(text.Length - 3, 0);
        while (length-- > 0)
        {
            if (ImGui.CalcTextSize(text, 0, length).X < maxWidth)
                return $"{text[..length]}...";
        }

        return "";
    }

    private string GetSpotText(FishingSpot? spot)
    {
        if (spot == null)
            return "Unknown";

        var maxWidth = _windowSize.X - ImGui.CalcTextSize("100.0").X - 2 * _textMargin;
        return EllipsifyString(spot.Name, maxWidth);
    }

    private void SetSpot(FishingSpot? spot)
    {
        var newMilliseconds = (int)_recorder.Timer.ElapsedMilliseconds;
        if (_spot != spot)
        {
            _spot = spot;
            // Recalculated and cached on the first draw.
            _spotName = null;
            UpdateFish();
        }
        else if (newMilliseconds < _milliseconds
              || GatherBuddy.EventFramework.FishingState is FishingState.None or FishingState.PoleReady
              && GatherBuddy.Time.ServerTime >= _nextUptimeChange)
        {
            UpdateFish();
        }

        _milliseconds = spot == null ? 0 : newMilliseconds;
    }

    private void UpdateFish()
    {
        if (_spot == null)
        {
            _availableFish = Array.Empty<FishCache>();
        }
        else
        {
            var enumerator = _spot.Items.Select(f => new FishCache(_recorder, f, _spot));
            if (GatherBuddy.Config.HideUnavailableFish)
                enumerator = enumerator.Where(f => !f.Unavailable);
            if (GatherBuddy.Config.HideUncaughtFish)
                enumerator = enumerator.Where(f => !f.Uncaught);
            _availableFish = enumerator.OrderBy(f => f.SortOrder).ToArray();

            var currentTime = GatherBuddy.Time.ServerTime;
            _nextUptimeChange = _availableFish.Min(f => f.NextUptime.Start < currentTime ? f.NextUptime.End : f.NextUptime.Start);
        }
    }

    public override void PreOpenCheck()
    {
        IsOpen = GatherBuddy.Config.ShowFishTimer;
    }

    public override void PreDraw()
    {
        _itemSpacing = new Vector2(0, ImGuiHelpers.GlobalScale);
        _style.Push(ImGuiStyleVar.ItemSpacing,   _itemSpacing);
        _style.Push(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        _lineHeight = ImGui.GetFrameHeight();
        _iconSize   = new Vector2(_lineHeight);
        _textMargin = 5 * ImGuiHelpers.GlobalScale;
        _textLines  = 2 * ImGui.GetTextLineHeightWithSpacing() + ImGuiHelpers.GlobalScale;
        _maxListHeight = _maxNumLines * (_lineHeight + _itemSpacing.Y)
          + _textLines
          + (GatherBuddy.Config.ShowSecondIntervals > 0 ? 1.1f * ImGui.GetTextLineHeightWithSpacing() : 0);
        _listHeight = _availableFish.Length * (_lineHeight + _itemSpacing.Y) + _textLines;
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(225,  _maxListHeight / ImGuiHelpers.GlobalScale),
            MaximumSize = new Vector2(2000, _maxListHeight / ImGuiHelpers.GlobalScale),
        };
        Flags = GatherBuddy.Config.FishTimerEdit ? EditFlags : NormalFlags;
        if (!GatherBuddy.Config.FishTimerEdit && GatherBuddy.Config.FishTimerClickthrough)
            Flags |= ImGuiWindowFlags.NoMouseInputs;
    }

    public override void PostDraw()
    {
        _style.Dispose();
    }

    public override bool DrawConditions()
    {
        if (GatherBuddy.Config.FishTimerEdit)
            return true;

        if (GatherBuddy.EventFramework.FishingState == FishingState.None)
        {
            SetSpot(null);
            return false;
        }

        SetSpot(_recorder.Record.FishingSpot);
        return true;
    }

    public override void Draw()
    {
        _style.Pop();
        _windowPos  = ImGui.GetWindowPos();
        _windowSize = new Vector2(ImGui.GetWindowSize().X, _maxListHeight);
        if (GatherBuddy.Config.FishTimerEdit)
        {
            DrawTextHeader("Bait", "Place", -1);
            DrawEditModeTimer();
        }
        else
        {
            _spotName ??= GetSpotText(_spot);
            var baitString = " (M)";
            if (GatherBuddy.GameData.Bait.ContainsKey(_recorder.Record.BaitId))
            {
                var baitCount = CurrentBait.HasItem(_recorder.Record.Bait.Id);
                baitString = baitCount > 999 ? " (>1k)" : $" ({baitCount})";
            }

            DrawTextHeader(_recorder.Record.Bait.Name + baitString, _spotName, _milliseconds);
            DrawSecondLines();
            foreach (var fish in _availableFish)
                fish.Draw(this);

            DrawProgressLine();
        }

        _style.Push(ImGuiStyleVar.WindowPadding, Vector2.Zero);
    }
}
