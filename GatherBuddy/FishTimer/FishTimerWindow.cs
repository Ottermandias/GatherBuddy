using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface;
using GatherBuddy.Config;
using GatherBuddy.SeFunctions;
using ImGuiNET;
using ImGuiOtter;
using FishingSpot = GatherBuddy.Classes.FishingSpot;

namespace GatherBuddy.FishTimer;

public partial class FishTimerWindow : IDisposable
{
    private const ImGuiWindowFlags EditFlags =
        ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar;

    private const ImGuiWindowFlags Flags = EditFlags | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoInputs;

    private          FishingSpot? _spot;
    private          FishCache[]  _availableFish = Array.Empty<FishCache>();
    private readonly FishRecorder _recorder;

    private float   _lineHeight;
    private Vector2 _iconSize    = Vector2.Zero;
    private Vector2 _itemSpacing = Vector2.Zero;
    private Vector2 _windowPos   = Vector2.Zero;
    private Vector2 _windowSize  = Vector2.Zero;
    private float   _textLines;
    private float   _maxListHeight;
    private float   _listHeight;
    private int     _milliseconds;

    public FishTimerWindow(FishRecorder recorder)
    {
        _recorder                              =  recorder;
        Dalamud.PluginInterface.UiBuilder.Draw += Draw;
    }

    public void Dispose()
    {
        Dalamud.PluginInterface.UiBuilder.Draw -= Draw;
    }

    private static float Rounding
        => 4 * ImGuiHelpers.GlobalScale;

    private void DrawRegular()
    {
        using var style = Preamble();
        if (!BeginWindow(Flags))
            return;

        using var end = ImGuiRaii.DeferredEnd(ImGui.End);
        DrawTextHeader(_recorder.Record.Bait.Name, _spot?.Name ?? "Unknown", _milliseconds);
        foreach (var fish in _availableFish)
            fish.Draw(this);

        DrawProgressLine();
    }

    private void DrawEditMode()
    {
        using var style = Preamble();
        if (!BeginWindow(EditFlags))
            return;

        using var end = ImGuiRaii.DeferredEnd(ImGui.End);
        DrawTextHeader("Bait", "Place", -1);
        DrawEditModeTimer();
    }

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
        DrawCenteredText(_windowSize.X, "in /GatherBuddy -> Config");
        DrawCenteredText(_windowSize.X, "-> Interface -> Fish Timer Window");
        DrawCenteredText(_windowSize.X, "to hide this when not fishing.");
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

    private ImGuiRaii.Style Preamble()
    {
        ImGui.SetNextWindowSizeConstraints(new Vector2(225 * ImGuiHelpers.GlobalScale, _maxListHeight),
            new Vector2(2000 * ImGuiHelpers.GlobalScale,                               _maxListHeight));
        var style = ImGuiRaii.PushStyle(ImGuiStyleVar.WindowPadding, Vector2.Zero)
            .Push(ImGuiStyleVar.ItemSpacing, _itemSpacing);
        SetupStyle();
        return style;
    }

    private void SetupStyle()
    {
        _lineHeight    = ImGui.GetFrameHeight();
        _iconSize      = new Vector2(_lineHeight);
        _itemSpacing   = new Vector2(0, ImGuiHelpers.GlobalScale);
        _textLines     = 2 * ImGui.GetTextLineHeightWithSpacing() + ImGuiHelpers.GlobalScale;
        _maxListHeight = 10 * (_lineHeight + _itemSpacing.Y) + _textLines;
        _listHeight    = _availableFish.Length * (_lineHeight + _itemSpacing.Y) + _textLines;
    }

    private bool BeginWindow(ImGuiWindowFlags flags)
    {
        if (!ImGui.Begin("##FishingTimer", flags))
            return false;

        _windowPos  = ImGui.GetWindowPos();
        _windowSize = new Vector2(ImGui.GetWindowSize().X, _maxListHeight);
        return true;
    }

    private void DrawTextHeader(string bait, string spot, int milliseconds)
    {
        var drawList = ImGui.GetWindowDrawList();
        drawList.AddRectFilled(_windowPos, _windowPos + new Vector2(_windowSize.X, _textLines),
            ColorId.FishTimerBackground.Value(), 4 * ImGuiHelpers.GlobalScale);
        var offset = 5 * ImGuiHelpers.GlobalScale;
        ImGui.SetCursorPosX(offset);
        ImGui.Text(bait);
        ImGui.SetCursorPosX(offset);
        ImGui.Text(spot);

        switch (milliseconds)
        {
            case 0: return;
            case -1:
                var text = "Elapsed Time";
                ImGui.SameLine(_windowSize.X - ImGui.CalcTextSize(text).X - offset);
                ImGui.Text(text);
                return;
            default:
                var secondText = (_milliseconds / 1000.0).ToString("00.0");
                ImGui.SameLine(_windowSize.X - ImGui.CalcTextSize(secondText).X - offset);
                ImGui.Text(secondText);
                return;
        }
    }

    private void SetSpot(FishingSpot? spot)
    {
        if (spot == null)
        {
            _spot = null;
            UpdateFish();
            _milliseconds = 0;
            return;
        }

        var newMilliseconds = (int)_recorder.Timer.ElapsedMilliseconds;
        if (newMilliseconds < _milliseconds || _spot == null)
        {
            _spot = spot;
            UpdateFish();
        }

        _milliseconds = newMilliseconds;
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
        }
    }

    private void Draw()
    {
        if (!GatherBuddy.Config.ShowFishTimer)
            return;

        if (GatherBuddy.Config.FishTimerEdit)
        {
            DrawEditMode();
            return;
        }

        if (GatherBuddy.EventFramework.FishingState == FishingState.None)
        {
            SetSpot(null);
            return;
        }

        SetSpot(_recorder.Record.FishingSpot);
        DrawRegular();
    }
}
