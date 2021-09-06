using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.Managers;
using GatherBuddy.SeFunctions;
using GatherBuddy.Utility;
using ImGuiNET;
using ImGuiScene;
using DateTime = System.DateTime;
using FishingSpot = GatherBuddy.Game.FishingSpot;

namespace GatherBuddy.Gui
{
    public class FishingTimer : IDisposable
    {
        private const    float   MaxTimerSeconds  = FishRecord.MaxTime;
        private readonly Vector2 _buttonTextAlign = new(0f, 0.1f);
        private readonly Vector2 _itemSpacing     = new(0, 1);

        private readonly FishManager    _fish;
        private readonly WeatherManager _weather;
        private readonly CurrentBait    _bait;
        private readonly FishingParser  _parser;
        private readonly Cache.Icons    _icons;
        private readonly EventFramework _eventFramework;

        private static bool Visible
            => GatherBuddy.Config.ShowFishTimer;

        private static bool EditMode
            => GatherBuddy.Config.FishTimerEdit;

        private          bool         _snagging;
        private          bool         _chum;
        private          bool         _intuition;
        private          bool         _fishEyes;
        private readonly Stopwatch    _start        = new();
        private          bool         _catchHandled = true;
        private          FishingSpot? _currentSpot;
        private          Bait         _currentBait = Bait.Unknown;
        private          Fish?        _lastFish;

        private Vector2     _rectMin;
        private Vector2     _rectSize;
        private Vector2     _iconSize;
        private float       _lineHeight;
        private FishCache[] _currentFishList = new FishCache[0];

        private readonly struct FishCache
        {
            private readonly RealUptime  _nextUptime;
            private readonly string      _textLine;
            private readonly uint        _color;
            private readonly TextureWrap _icon;
            private readonly float       _sizeMin;
            private readonly float       _sizeMax;
            public readonly  bool        Valid;
            public readonly  bool        Uncaught;
            public readonly  bool        Unavailable;
            public readonly  ulong       SortOrder;

            public FishCache(FishingTimer timer, Fish fish)
            {
                var fishBase = fish;
                var bite     = fish.CatchData?.BiteType ?? BiteType.Unknown;

                var catchMin = timer._chum ? fish.Record.EarliestCatchChum : fish.Record.EarliestCatch;
                var catchMax = timer._chum ? fish.Record.LatestCatchChum : fish.Record.LatestCatch;
                _sizeMin  = Math.Max(catchMin / MaxTimerSeconds, 0.0f);
                _sizeMax  = Math.Min(catchMax / MaxTimerSeconds, 1.0f);
                SortOrder = ((ulong) catchMin << 16) | catchMax;

                _icon = timer._icons[fishBase.ItemData.Icon];

                Unavailable = false;
                Uncaught    = false;

                if ((fish.CatchData?.Predator.Length ?? 0) > 0)
                    if (!timer._intuition)
                        Unavailable = true;
                if (DateTime.UtcNow < fish.NextUptime(timer._weather, timer._currentSpot?.Territory).Time)
                    if (!timer._fishEyes || fish.IsBigFish || fish.FishRestrictions.HasFlag(FishRestrictions.Weather))
                        Unavailable = true;
                if ((fish.CatchData?.Snagging ?? Snagging.Unknown) == Snagging.Required)
                    if (!timer._snagging)
                        Unavailable = true;

                if (!timer.RecordsValid(fish.Record))
                    Uncaught = true;

                _color = Colors.FishTimer.FromBiteType(bite, Uncaught);

                _textLine = fishBase.Name[GatherBuddy.Language];
                if (Unavailable)
                {
                    _color    = Colors.FishTimer.Unavailable;
                    SortOrder = ulong.MaxValue;
                }
                else if (Uncaught)
                {
                    SortOrder |= 1ul << 33;
                }

                Valid = !Unavailable && _sizeMin > 0.001f && _sizeMax < 0.999f && _sizeMin <= _sizeMax;
                _nextUptime = GatherBuddy.Config.ShowWindowTimers
                    ? fish.NextUptime(timer._weather, timer._currentSpot?.Territory)
                    : RealUptime.Always;
                if (_nextUptime.Equals(RealUptime.Unknown) || _nextUptime.Equals(RealUptime.Never))
                    _nextUptime = RealUptime.Always;
            }

            public void Draw(FishingTimer timer, ImDrawListPtr ptr)
            {
                var pos    = ImGui.GetCursorPosY();
                var height = ImGui.GetTextLineHeightWithSpacing() * 1.4f;

                var biteMin = Vector2.Zero;
                var biteMax = Vector2.Zero;
                var scale   = ImGui.GetIO().FontGlobalScale;
                var begin   = timer._rectMin + new Vector2(timer._iconSize.X,  0);
                var size    = timer._rectSize - new Vector2(timer._iconSize.X, 0);
                if (Valid)
                {
                    biteMin = begin + new Vector2(_sizeMin * size.X - 2 * scale, pos);
                    biteMax = begin + new Vector2(_sizeMax * size.X + 2 * scale, pos + height);
                    ptr.AddRectFilled(biteMin, biteMax, Colors.FishTimer.Background);
                }

                ImGui.Image(_icon.ImGuiHandle, timer._iconSize);
                ImGui.SameLine();

                var buttonWidth = timer._rectSize.X - timer._iconSize.X;
                using (var _ = new ImGuiRaii()
                    .PushColor(ImGuiCol.Button,        _color)
                    .PushColor(ImGuiCol.ButtonHovered, _color)
                    .PushColor(ImGuiCol.ButtonActive,  _color)
                    .PushStyle(ImGuiStyleVar.ButtonTextAlign, timer._buttonTextAlign))
                {
                    ImGui.Button(_textLine, new Vector2(buttonWidth, height));
                }

                if (!_nextUptime.Equals(RealUptime.Always))
                {
                    var now = DateTime.UtcNow;
                    var time = (int) (_nextUptime.Time < now
                        ? (_nextUptime.EndTime - now).TotalSeconds
                        : (_nextUptime.Time - now).TotalSeconds);
                    var s         = Interface.TimeString(time, true);
                    var t         = ImGui.CalcTextSize(s);
                    var width     = t.X;
                    var fishWidth = ImGui.CalcTextSize(_textLine).X;
                    if (buttonWidth - width - fishWidth >= 5 * ImGui.GetIO().FontGlobalScale)
                    {
                        var oldPos = ImGui.GetCursorPos();
                        ImGui.SetCursorScreenPos(begin + new Vector2(size.X - width - 2.5f * ImGui.GetIO().FontGlobalScale, pos));
                        ImGui.AlignTextToFramePadding();
                        ImGui.TextColored(Colors.FishTimer.WindowTimes, s);
                        ImGui.SetCursorPos(oldPos);
                    }
                }

                if (!Valid)
                    return;

                ptr.AddLine(biteMin, biteMin + new Vector2(0, height), Colors.FishTimer.Separator, 2 * scale);
                ptr.AddLine(biteMax, biteMax - new Vector2(0, height), Colors.FishTimer.Separator, 2 * scale);
            }
        }

        private Bait GetCurrentBait(uint id)
        {
            if (_fish.Bait.TryGetValue(id, out var bait))
                return bait;

            PluginLog.Error("Item with id {Id} is not a known type of bait.", id);
            return Bait.Unknown;
        }

        private void CheckBuffs()
        {
            const uint snaggingEffectId  = 761;
            const uint chumEffectId      = 763;
            const uint intuitionEffectId = 568;
            const uint fishEyesEffectId  = 762;

            _snagging  = false;
            _chum      = false;
            _intuition = false;

            if (Dalamud.ClientState.LocalPlayer?.StatusList == null)
                return;

            foreach (var buff in Dalamud.ClientState.LocalPlayer.StatusList)
            {
                switch (buff.StatusId)
                {
                    case snaggingEffectId:
                        _snagging = true;
                        break;
                    case chumEffectId:
                        _chum = true;
                        break;
                    case intuitionEffectId:
                        _intuition = true;
                        break;
                    case fishEyesEffectId:
                        _fishEyes = true;
                        break;
                }
            }
        }


        private void OnBeganFishing(FishingSpot? spot)
        {
            _currentSpot = spot;
            _currentBait = GetCurrentBait(_bait.Current);
            CheckBuffs();
            _currentFishList = SortedFish();
            PluginLog.Verbose("Began fishing at {FishingSpot} using {Bait} {Snagging} and {Chum}.",
                _currentSpot?.PlaceName ?? "Undiscovered Fishing Hole", _currentBait.Name
                , _snagging ? "with Snagging" : "without Snagging"
                , _chum ? "with Chum" : "without Chum");
            _start.Restart();
            _catchHandled = false;
        }

        private void OnBite()
        {
            _start.Stop();
            PluginLog.Verbose("Fish bit at {FishingSpot} after {Milliseconds} using {Bait} {Snagging} and {Chum}.",
                _currentSpot?.PlaceName ?? "Undiscovered Fishing Hole", _start.ElapsedMilliseconds
                , _currentBait.Name, _snagging ? "with Snagging" : "without Snagging", _chum ? "with Chum" : "without Chum");
        }

        private void OnIdentification(FishingSpot spot)
        {
            _currentSpot = spot;
            PluginLog.Verbose("Identified previously unknown fishing spot as {FishingSpot}.", _currentSpot.PlaceName ?? "Unknown");
        }

        private void OnCatch(Fish fish)
        {
            _lastFish = fish;

            if (_lastFish.Record.Update(_currentBait, (ushort) _start.ElapsedMilliseconds, _snagging, _chum))
            {
                _fish.SaveFishRecords();
                _currentFishList = SortedFish();
            }

            PluginLog.Verbose("Caught {Fish} at {FishingSpot} after {Milliseconds} using {Bait} {Snagging} and {Chum}.",
                _lastFish.Name, _currentSpot?.PlaceName ?? "Unknown", _start.ElapsedMilliseconds,
                _currentBait.Name, _snagging ? "with Snagging" : "without Snagging",
                _chum ? "with Chum" : "without Chum");
            _catchHandled = true;
        }

        private void OnMooch()
        {
            _currentBait     = new Bait(_lastFish!.ItemData, _lastFish.Name);
            CheckBuffs();
            _currentFishList = SortedFish();
            PluginLog.Verbose("Mooching with {Fish} at {FishingSpot} {Snagging} and {Chum}.", _lastFish!.Name,
                _currentSpot!.PlaceName ?? "Unknown", _snagging ? "with Snagging" : "without Snagging", _chum ? "with Chum" : "without Chum");
            _start.Restart();
            _catchHandled = false;
        }

        public FishingTimer(FishManager fish, WeatherManager weather)
        {
            _fish           = fish;
            _weather        = weather;
            _bait           = new CurrentBait(Dalamud.SigScanner);
            _parser         = new FishingParser(_fish);
            _icons          = Service<Cache.Icons>.Get();
            _eventFramework = new EventFramework(Dalamud.SigScanner);

            Dalamud.PluginInterface.UiBuilder.Draw += Draw;
            _parser.BeganFishing                   += OnBeganFishing;
            _parser.BeganMooching                  += OnMooch;
            _parser.IdentifiedSpot                 += OnIdentification;
            _parser.SomethingBit                   += OnBite;
            _parser.CaughtFish                     += OnCatch;
        }

        public void Dispose()
        {
            Dalamud.PluginInterface.UiBuilder.Draw -= Draw;
            _parser.BeganFishing                   -= OnBeganFishing;
            _parser.BeganMooching                  -= OnMooch;
            _parser.IdentifiedSpot                 -= OnIdentification;
            _parser.SomethingBit                   -= OnBite;
            _parser.CaughtFish                     -= OnCatch;
            _parser.Dispose();
        }

        private bool RecordsValid(FishRecord record)
            => record.SuccessfulBaits.Contains(_currentBait.Id) && record.WithoutSnagging || _snagging;

        private FishCache[] SortedFish()
        {
            if (_currentSpot == null)
                return new FishCache[0];

            var enumerable = _currentSpot.Items.Where(f => f != null).Select(f => new FishCache(this, f!));

            if (GatherBuddy.Config.HideUncaughtFish)
                enumerable = enumerable.Where(f => !f.Uncaught);
            if (GatherBuddy.Config.HideUnavailableFish)
                enumerable = enumerable.Where(f => !f.Unavailable);

            return enumerable.OrderBy(f => f.SortOrder).ToArray();
        }

        private void DrawEditModeTimer(ImDrawListPtr drawList, float rounding)
        {
            ImGui.Text("  Bait");
            ImGui.Text("  Place and Time");
            drawList.AddRect(_rectMin, _rectMin + _rectSize - _itemSpacing, Colors.FishTimer.Line, rounding);
            drawList.AddRectFilled(_rectMin, _rectMin + _rectSize, Colors.FishTimer.EditBackground, rounding);
            ImGui.SetCursorPosY((_rectSize.Y - ImGui.GetTextLineHeightWithSpacing()) / 2);
            DrawCenteredText(_rectSize.X, "FISH");
            ImGui.SetCursorPosY((_rectSize.Y + ImGui.GetTextLineHeightWithSpacing()) / 2);
            DrawCenteredText(_rectSize.X, "TIMER");
            DrawCenteredText(_rectSize.X, "\nDisable \"Edit Fish Timer\"");
            DrawCenteredText(_rectSize.X, "in /GatherBuddy -> Settings");
            DrawCenteredText(_rectSize.X, "to hide this when not fishing.");
        }

        public void Draw()
        {
            const ImGuiWindowFlags editFlags =
                ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar;

            const ImGuiWindowFlags flags = editFlags | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoInputs;

            if (!Visible)
                return;

            if (Dalamud.ClientState.LocalPlayer?.ClassJob == null || !Dalamud.Conditions.Any())
                return;

            var fishing = _start.IsRunning && Dalamud.Conditions[ConditionFlag.Fishing];
            var rodOut  = Dalamud.ClientState.LocalPlayer.ClassJob.Id == 18 && Dalamud.Conditions[ConditionFlag.Gathering];


            if (_eventFramework.FishingState == FishingState.Bite)
            {
                if (_start.IsRunning)
                    PluginLog.Verbose("Fish bit after {Milliseconds} milliseconds.", _start.ElapsedMilliseconds);
                _start.Stop();
            }

            if (!fishing)
                _start.Stop();
            if (!rodOut)
            {
                _currentFishList = Array.Empty<FishCache>();
                if (!EditMode)
                    return;
            }

            var diff    = _start.ElapsedMilliseconds;
            var diffPos = _rectMin.X + _iconSize.X + 2 + (_rectSize.X - _iconSize.X) * diff / MaxTimerSeconds;

            using var imgui = new ImGuiRaii()
                .PushStyle(ImGuiStyleVar.WindowPadding, Vector2.Zero)
                .PushStyle(ImGuiStyleVar.ItemSpacing,   _itemSpacing);

            _lineHeight = ImGui.GetTextLineHeightWithSpacing() * 1.4f;
            _iconSize   = new Vector2(_lineHeight, _lineHeight);
            var textLines     = 2 * ImGui.GetTextLineHeightWithSpacing();
            var maxListHeight = 10 * (_lineHeight + 1) + textLines;
            var listHeight    = EditMode ? maxListHeight : _currentFishList.Length * (_lineHeight + 1) + textLines;
            var globalScale   = ImGui.GetIO().FontGlobalScale;
            var fivePx        = 5 * globalScale;

            ImGui.SetNextWindowSizeConstraints(new Vector2(225 * globalScale, maxListHeight),
                new Vector2(30000 * globalScale,                              listHeight));

            if (!imgui.BeginWindow("##FishingTimer", EditMode ? editFlags : flags))
                return;

            var drawList = ImGui.GetWindowDrawList();

            _rectSize = new Vector2(ImGui.GetWindowSize().X, maxListHeight);
            _rectMin  = ImGui.GetWindowPos();

            drawList.AddRectFilled(_rectMin, _rectMin + new Vector2(_rectSize.X, textLines), Colors.FishTimer.RectBackground,
                4f * globalScale);
            if (rodOut)
            {
                ImGui.SetCursorPosX(fivePx);
                ImGui.Text(_currentBait.Name[GatherBuddy.Language]);
                ImGui.SetCursorPosX(fivePx);
                ImGui.Text(_currentSpot?.PlaceName?[GatherBuddy.Language] ?? "Unknown");
                var displayTimer = (fishing || !_catchHandled) && _start.ElapsedMilliseconds > 0;

                if (displayTimer)
                {
                    var secondText = (diff / 1000.0).ToString("00.0");
                    ImGui.SameLine(_rectSize.X - ImGui.CalcTextSize(secondText).X - fivePx);
                    ImGui.Text(secondText);
                }

                foreach (var fish in _currentFishList)
                    fish.Draw(this, drawList);

                if (displayTimer)
                    drawList.AddLine(new Vector2(diffPos, _rectMin.Y + textLines),
                        new Vector2(diffPos,              _rectMin.Y + listHeight - 2 * globalScale),
                        Colors.FishTimer.Line, 3 * globalScale);
            }
            else if (EditMode)
            {
                DrawEditModeTimer(drawList, fivePx);
            }
        }

        public static void DrawCenteredText(float xSize, string text)
        {
            var textSize = ImGui.CalcTextSize(text).X;
            ImGui.SetCursorPosX((xSize - textSize) / 2);
            ImGui.Text(text);
        }
    }
}
