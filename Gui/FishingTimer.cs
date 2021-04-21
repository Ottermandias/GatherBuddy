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
using ImGuiScene;
using DateTime = System.DateTime;
using FishingSpot = GatherBuddy.Game.FishingSpot;

namespace GatherBuddy.Gui
{
    public class FishingTimer : IDisposable
    {
        private const    float   MaxTimerSeconds  = 35000f;
        private readonly Vector2 _buttonTextAlign = new(0f, 0.1f);
        private readonly Vector2 _itemSpacing     = new(0, 1);

        private readonly DalamudPluginInterface   _pi;
        private readonly GatherBuddyConfiguration _config;
        private readonly FishManager              _fish;
        private readonly WeatherManager           _weather;
        private readonly ClientLanguage           _lang;
        private readonly CurrentBait              _bait;
        private readonly FishingParser            _parser;
        private readonly Cache.Icons              _icons;

        private bool Visible
            => _config.ShowFishTimer;

        private bool EditMode
            => _config.FishTimerEdit;

        private          bool         _snagging;
        private          bool         _chum;
        private          bool         _intuition;
        private          bool         _fishEyes;
        private readonly Stopwatch    _start = new();
        private readonly Stopwatch    _bite  = new();
        private          FishingSpot? _currentSpot;
        private          Bait         _currentBait;
        private          Fish?        _lastFish;

        private Vector2     _rectMin;
        private Vector2     _rectSize;
        private Vector2     _iconSize;
        private float       _lineHeight;
        private FishCache[] _currentFishList = new FishCache[0];

        private readonly struct FishCache
        {
            private readonly Fish        _fish;
            private readonly uint        _color;
            private readonly TextureWrap _icon;
            private readonly float       _sizeMin;
            private readonly float       _sizeMax;
            public readonly  bool        Valid;
            public readonly  bool        Uncaught;
            public readonly  bool        Unavailable;
            public readonly  uint        SortOrder;

            private void SetUnavailable()
            {

            }

            public FishCache(FishingTimer timer, Fish fish)
            {
                _fish = fish;
                var bite = fish.Record.BiteType != BiteType.Unknown ? fish.Record.BiteType : fish.CatchData?.BiteType ?? BiteType.Unknown;
                _color = Colors.FishTimer.FromBiteType(bite);

                var catchMin = timer._chum ? fish.Record.EarliestCatchChum : fish.Record.EarliestCatch;
                var catchMax = timer._chum ? fish.Record.LatestCatchChum : fish.Record.LatestCatch;
                _sizeMin  = Math.Max(catchMin / MaxTimerSeconds, 0.0f);
                _sizeMax  = Math.Min(catchMax / MaxTimerSeconds, 1.0f);
                SortOrder = ((uint) catchMin << 16) | catchMax;

                _icon = timer._icons[_fish.ItemData.Icon];

                Unavailable = false;
                Uncaught    = false;

                if ((fish.CatchData?.Predator.Length ?? 0) > 0)
                {
                    if (!timer._intuition)
                        Unavailable = true;
                }
                if (DateTime.UtcNow < fish.NextUptime(timer._weather).Time)
                {
                    if (!timer._fishEyes || fish.IsBigFish || fish.FishRestrictions.HasFlag(FishRestrictions.Weather))
                        Unavailable = true;
                }
                if ((fish.CatchData?.Snagging ?? Snagging.Unknown) == Snagging.Required)
                {
                    if (!timer._snagging)
                        Unavailable = true;
                }

                if (!timer.RecordsValid(fish.Record))
                    Uncaught  = true;

                if (Unavailable)
                {
                    _color    = Colors.FishTimer.Unavailable;
                    SortOrder = uint.MaxValue;
                }
                else if (Uncaught)
                {
                    _color    = Colors.FishTimer.Invalid;
                    SortOrder = uint.MaxValue - 1;
                }

                Valid = !Unavailable && !Uncaught && _sizeMin > 0.001f && _sizeMax < 0.999f && _sizeMin <= _sizeMax;
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

                ImGui.PushStyleColor(ImGuiCol.Button,        _color);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, _color);
                ImGui.PushStyleColor(ImGuiCol.ButtonActive,  _color);
                ImGui.PushStyleVar(ImGuiStyleVar.ButtonTextAlign, timer._buttonTextAlign);

                ImGui.Button(_fish.Name![GatherBuddy.Language], new Vector2(timer._rectSize.X - timer._iconSize.X, height));

                ImGui.PopStyleVar();
                ImGui.PopStyleColor(3);

                if (Valid)
                {
                    ptr.AddLine(biteMin, biteMin + new Vector2(0, height), Colors.FishTimer.Separator, 2 * scale);
                    ptr.AddLine(biteMax, biteMax - new Vector2(0, height), Colors.FishTimer.Separator, 2 * scale);
                }
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
            const short snaggingEffectId  = 761;
            const short chumEffectId      = 763;
            const short intuitionEffectId = 568;
            const short fishEyesEffectId  = 762;

            _snagging  = false;
            _chum      = false;
            _intuition = false;

            if (_pi.ClientState?.LocalPlayer?.StatusEffects == null)
                return;

            foreach (var buff in _pi.ClientState.LocalPlayer.StatusEffects)
            {
                switch (buff.EffectId)
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
        }

        private void OnBite()
        {
            _start.Stop();
            PluginLog.Verbose("Fish bit at {FishingSpot} after {Milliseconds} using {Bait} {Snagging} and {Chum}.",
                _currentSpot?.PlaceName ?? "Undiscovered Fishing Hole", _start.ElapsedMilliseconds
                , _currentBait.Name, _snagging ? "with Snagging" : "without Snagging", _chum ? "with Chum" : "without Chum");
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
            if (_lastFish.Record.Update(_currentBait, (ushort) _start.ElapsedMilliseconds, _snagging, _chum, _bite.ElapsedMilliseconds))
                _fish.SaveFishRecords(_pi);
            PluginLog.Verbose("Caught {Fish} at {FishingSpot} after {Milliseconds} and {Milliseconds2} using {Bait} {Snagging} and {Chum}.",
                _lastFish.Name, _currentSpot!.PlaceName, _start.ElapsedMilliseconds,
                _bite.ElapsedMilliseconds, _currentBait.Name, _snagging ? "with Snagging" : "without Snagging",
                _chum ? "with Chum" : "without Chum");
        }

        private void OnMooch()
        {
            _currentBait     = new Bait(_lastFish!.ItemData, _lastFish.Name);
            _currentFishList = SortedFish();
            PluginLog.Verbose("Mooching with {Fish} at {FishingSpot} {Snagging} and {Chum}.", _lastFish!.Name,
                _currentSpot!.PlaceName, _snagging ? "with Snagging" : "without Snagging", _chum ? "with Chum" : "without Chum");
            _start.Restart();
        }

        public FishingTimer(DalamudPluginInterface pi, GatherBuddyConfiguration config, FishManager fish, WeatherManager weather)
        {
            _pi      = pi;
            _config  = config;
            _lang    = pi.ClientState.ClientLanguage;
            _fish    = fish;
            _weather = weather;
            _bait    = new CurrentBait(pi.TargetModuleScanner);
            _parser  = new FishingParser(_pi, _fish);
            _icons   = Service<Cache.Icons>.Get();

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

        private FishCache[] SortedFish()
        {
            if (_currentSpot == null)
                return new FishCache[0];

            var enumerable = _currentSpot.Items.Where(f => f != null).Cast<Fish>().Select(f => new FishCache(this, f));

            if (_config.HideUncaughtFish)
                enumerable = enumerable.Where(f => !f.Uncaught);
            if (_config.HideUnavailableFish)
                enumerable = enumerable.Where(f => !f.Unavailable);

            return enumerable.OrderBy(f => f.SortOrder).ToArray();
        }

        public void Draw()
        {
            const ImGuiWindowFlags editFlags =
                ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar;

            const ImGuiWindowFlags flags = editFlags | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoInputs;

            if (!Visible)
                return;

            var fishing = _pi.ClientState.Condition[ConditionFlag.Fishing] && _start.IsRunning;
            var rodOut  = _pi.ClientState.Condition[ConditionFlag.Gathering] && _pi.ClientState.LocalPlayer.ClassJob.Id == 18;

            if (!fishing)
                _start.Stop();
            if (!rodOut)
            {
                _currentFishList = new FishCache[0];
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
            var maxListHeight = 10 * (_lineHeight + 1) + textLines;
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
                ImGui.Text(_currentBait.Name[GatherBuddy.Language]);
                ImGui.SetCursorPosX(fivePx);
                ImGui.Text(_currentSpot?.PlaceName?[GatherBuddy.Language] ?? "Unknown");
                var displayTimer = (fishing || _bite.IsRunning) && _start.ElapsedMilliseconds > 0;

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
