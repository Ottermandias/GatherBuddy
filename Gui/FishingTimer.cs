using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Dalamud;
using Dalamud.Game.ClientState;
using Dalamud.Plugin;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.Managers;
using GatherBuddy.SeFunctions;
using GatherBuddy.Utility;
using ImGuiNET;
using FishingSpot = GatherBuddy.Game.FishingSpot;

namespace GatherBuddy.Gui
{
    public class FishingTimer : IDisposable
    {
        private const    float   MaxTimerSeconds  = 30000f;
        private readonly Vector2 _buttonTextAlign = new(0f, 0.1f);
        private readonly Vector2 _itemSpacing     = new(0, 1);

        private readonly DalamudPluginInterface   _pi;
        private readonly GatherBuddyConfiguration _config;
        private readonly FishManager              _fish;
        private readonly ClientLanguage           _lang;
        private readonly CurrentBait              _bait;
        private readonly FishingParser            _parser;
        private readonly Cache.Icons              _icons;

        private bool Visible
            => _config.ShowFishTimer;

        private bool EditMode
            => _config.FishTimerEdit;

        private          bool         _snagging;
        private readonly Stopwatch    _start = new();
        private readonly Stopwatch    _bite  = new();
        private          FishingSpot? _currentSpot;
        private          Bait         _currentBait;
        private          Fish?        _lastFish;

        private Vector2 _rectMin;
        private Vector2 _rectSize;
        private Vector2 _iconSize;
        private float   _lineHeight;
        private Fish[]  _currentFishList = new Fish[0];

        private Bait GetCurrentBait(uint id)
        {
            if (_fish.Bait.TryGetValue(id, out var bait))
                return bait;

            PluginLog.Error("Item with id {Id} is not a known type of bait.", id);
            return Bait.Unknown;
        }

        private bool CheckForSnagging()
        {
            const uint snaggingEffectId = 761;
            return _pi.ClientState.LocalPlayer?.StatusEffects?.Any(s => s.EffectId == snaggingEffectId) ?? false;
        }

        private void OnBeganFishing(FishingSpot? spot)
        {
            _currentSpot     = spot;
            _currentBait     = GetCurrentBait(_bait.Current);
            _snagging        = CheckForSnagging();
            _currentFishList = SortedFish();
            PluginLog.Verbose("Began fishing at {FishingSpot} using {Bait} {Snagging}.",
                _currentSpot?.PlaceName ?? "Undiscovered Fishing Hole", _currentBait.Name
                , _snagging ? "with Snagging." : "without Snagging.");
            _start.Restart();
        }

        private void OnBite()
        {
            _start.Stop();
            PluginLog.Verbose("Fish bit at {FishingSpot} after {Milliseconds} using {Bait} {Snagging}.",
                _currentSpot?.PlaceName ?? "Undiscovered Fishing Hole", _start.ElapsedMilliseconds
                , _currentBait.Name, _snagging ? "with Snagging." : "without Snagging.");
            _bite.Restart();
        }

        private void OnIdentification(FishingSpot spot)
        {
            _currentSpot = spot;
            PluginLog.Verbose("Identified previously unknown fishing spot as {FishingSpot}.", _currentSpot.PlaceName);
        }

        private void OnCatch(Fish fish)
        {
            _lastFish = fish;
            _bite.Stop();
            if (_lastFish.Record.Update(_currentBait, (ushort) _start.ElapsedMilliseconds, _snagging, _bite.ElapsedMilliseconds))
                _fish.SaveFishRecords(_pi);
            PluginLog.Verbose("Caught {Fish} at {FishingSpot} after {Milliseconds} and {Milliseconds2} using {Bait} {Snagging}.",
                _lastFish.Name, _currentSpot!.PlaceName, _start.ElapsedMilliseconds,
                _bite.ElapsedMilliseconds, _currentBait.Name, _snagging ? "with Snagging." : "without Snagging.");
        }

        private void OnMooch()
        {
            _currentBait     = new Bait(_lastFish!.ItemData, _lastFish.Name);
            _currentFishList = SortedFish();
            PluginLog.Verbose("Mooching with {Fish} at {FishingSpot} {Snagging}.", _lastFish!.Name,
                _currentSpot!.PlaceName, _snagging ? "with Snagging." : "without Snagging.");
            _start.Restart();
        }

        public FishingTimer(DalamudPluginInterface pi, GatherBuddyConfiguration config, FishManager fish)
        {
            _pi     = pi;
            _config = config;
            _lang   = pi.ClientState.ClientLanguage;
            _fish   = fish;
            _bait   = new CurrentBait(pi.TargetModuleScanner);
            _parser = new FishingParser(_pi, _fish);
            _icons  = Service<Cache.Icons>.Get();

            _pi.UiBuilder.OnBuildUi += Draw;
            _parser.BeganFishing    += OnBeganFishing;
            _parser.BeganMooching   += OnMooch;
            _parser.IdentifiedSpot  += OnIdentification;
            _parser.SomethingBit    += OnBite;
            _parser.CaughtFish      += OnCatch;
            _currentBait            =  GetCurrentBait(0);
        }

        public void Dispose()
        {
            _pi.UiBuilder.OnBuildUi -= Draw;
            _parser.BeganFishing    -= OnBeganFishing;
            _parser.BeganMooching   -= OnMooch;
            _parser.IdentifiedSpot  -= OnIdentification;
            _parser.SomethingBit    -= OnBite;
            _parser.CaughtFish      -= OnCatch;
            _parser.Dispose();
        }

        private bool RecordsValid(FishRecord record)
            => record.SuccessfulBaits.Contains(_currentBait.Id) && record.WithoutSnagging || _snagging;

        public void DrawFish(ImDrawListPtr drawList, Fish fish)
        {
            var bite  = fish.Record.BiteType != BiteType.Unknown ? fish.Record.BiteType : fish.CatchData?.BiteType ?? BiteType.Unknown;
            var color = Colors.FishTimer.FromBiteType(bite);

            var recordsValid = RecordsValid(fish.Record);
            if (!recordsValid)
                color = Colors.FishTimer.Invalid;

            var pos    = ImGui.GetCursorPosY();
            var height = ImGui.GetTextLineHeightWithSpacing() * 1.4f;

            if (recordsValid)
            {
                var biteMin = _rectMin + new Vector2(fish.Record.EarliestCatch / MaxTimerSeconds * _rectSize.X - 2, pos);
                var biteMax = _rectMin + new Vector2(fish.Record.LatestCatch / MaxTimerSeconds * _rectSize.X + 2,   pos + height);
                drawList.AddRectFilled(biteMin, biteMax, Colors.FishTimer.Background);
            }

            ImGui.Image(_icons[fish.ItemData.Icon].ImGuiHandle, _iconSize);
            ImGui.SameLine();

            ImGui.PushStyleColor(ImGuiCol.Button,        color);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, color);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive,  color);
            ImGui.PushStyleVar(ImGuiStyleVar.ButtonTextAlign, _buttonTextAlign);

            ImGui.Button(fish.Name![_lang], new Vector2(_rectSize.X - _iconSize.X, height));

            ImGui.PopStyleVar();
            ImGui.PopStyleColor(3);
        }

        private uint RecordOrder(Fish fish)
        {
            if (!RecordsValid(fish.Record))
                return uint.MaxValue;

            return ((uint) fish.Record.EarliestCatch << 16) | fish.Record.LatestCatch;
        }

        private Fish[] SortedFish()
        {
            if (_currentSpot == null)
                return new Fish[0];

            if (!_config.HideUncaughtFish)
                return _currentSpot.Items.Where(f => f != null).Cast<Fish>().OrderBy(RecordOrder).ToArray();

            return _currentSpot.Items.Where(f => f != null && RecordsValid(f.Record)).Cast<Fish>().OrderBy(RecordOrder).ToArray();
        }

        public void Draw()
        {
            const ImGuiWindowFlags editFlags =
                ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar;

            const ImGuiWindowFlags flags = editFlags | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoInputs;

            if (!Visible)
                return;

            var fishing = _pi.ClientState.Condition[ConditionFlag.Fishing] && _start.IsRunning;
            var rodOut  = _pi.ClientState.Condition[ConditionFlag.Gathering];
            if (!fishing)
                _start.Stop();
            if (!rodOut)
            {
                _currentFishList = new Fish[0];
                if (!EditMode)
                    return;
            }

            var diff    = _start.ElapsedMilliseconds;
            var diffPos = _rectMin.X + _iconSize.X + 2 + (_rectSize.X - _iconSize.X) * diff / MaxTimerSeconds;

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing,   _itemSpacing);

            _lineHeight = ImGui.GetTextLineHeightWithSpacing() * 1.4f;
            _iconSize   = new Vector2(_lineHeight, _lineHeight);
            var textLines     = 2 * ImGui.GetTextLineHeightWithSpacing();
            var maxListHeight = 9 * (_lineHeight + 1) + textLines;
            var listHeight    = EditMode ? maxListHeight : _currentFishList.Length * (_lineHeight + 1) + textLines;

            ImGui.SetNextWindowSizeConstraints(new Vector2(200 * ImGui.GetIO().FontGlobalScale, maxListHeight), new Vector2(30000, listHeight));

            if (!ImGui.Begin("##FishingTimer", EditMode ? editFlags : flags))
                return;

            var drawList = ImGui.GetWindowDrawList();

            if (EditMode || _rectMin.X == 0)
            {
                _rectMin  = ImGui.GetWindowPos();
                _rectSize = new Vector2(ImGui.GetWindowSize().X, maxListHeight);
            }

            var globalScale = ImGui.GetIO().FontGlobalScale;
            var fivePx      = 5 * globalScale;

            drawList.AddRectFilled(_rectMin, _rectMin + new Vector2(_rectSize.X, textLines), Colors.FishTimer.RectBackground, 4f * globalScale);
            if (rodOut)
            {
                ImGui.SetCursorPosX(fivePx);
                ImGui.Text(_currentBait.Name);
                ImGui.SetCursorPosX(fivePx);
                ImGui.Text(_currentSpot?.PlaceName?[_lang] ?? "Unknown");
                var displayTimer = (fishing || _bite.IsRunning) && _start.ElapsedMilliseconds > 0;

                if (displayTimer)
                {
                    var secondText = (diff / 1000.0).ToString("00.0");
                    ImGui.SameLine(_rectSize.X - ImGui.CalcTextSize(secondText).X - fivePx);
                    ImGui.Text(secondText);
                }

                foreach (var fish in _currentFishList)
                    DrawFish(drawList, fish!);

                if (displayTimer)
                    drawList.AddLine(new Vector2(diffPos, _rectMin.Y + textLines),
                        new Vector2(diffPos,              _rectMin.Y + listHeight - 2 * globalScale),
                        Colors.FishTimer.Line, 3 * globalScale);
            }
            else if (EditMode)
            {
                ImGui.Text("  Bait");
                ImGui.Text("  Place and Time");
                drawList.AddRect(_rectMin, _rectMin + _rectSize - _itemSpacing, Colors.FishTimer.Line, fivePx);
                drawList.AddRectFilled(_rectMin, _rectMin + _rectSize, Colors.FishTimer.EditBackground, fivePx);
                ImGui.SetCursorPosY((_rectSize.Y - ImGui.GetTextLineHeightWithSpacing()) / 2);
                DrawCenteredText(_rectSize.X, "FISH");
                ImGui.SetCursorPosY((_rectSize.Y + ImGui.GetTextLineHeightWithSpacing()) / 2);
                DrawCenteredText(_rectSize.X, "TIMER");
                DrawCenteredText(_rectSize.X, "\nDisable \"Edit Fish Timer\"");
                DrawCenteredText(_rectSize.X, "in /GatherBuddy -> Settings");
                DrawCenteredText(_rectSize.X, "to hide this when not fishing.");
            }


            ImGui.End();
            ImGui.PopStyleVar(2);
        }

        public static void DrawCenteredText(float xSize, string text)
        {
            var textSize = ImGui.CalcTextSize(text).X;
            ImGui.SetCursorPosX((xSize - textSize) / 2);
            ImGui.Text(text);
        }
    }
}
