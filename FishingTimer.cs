using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Dalamud;
using Dalamud.Game.ClientState;
using Dalamud.Plugin;
using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.SeFunctions;
using ImGuiNET;
using FishingSpot = GatherBuddy.Classes.FishingSpot;

namespace GatherBuddy
{
    public class FishingTimer : IDisposable
    {
        private readonly DalamudPluginInterface   _pi;
        private readonly GatherBuddyConfiguration _config;
        private readonly FishManager              _fish;
        private readonly ClientLanguage           _lang;
        private readonly CurrentBait              _bait;
        private readonly FishingParser            _parser;

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
                _currentSpot?.PlaceName?[ClientLanguage.English] ?? "Undiscovered Fishing Hole", _currentBait.Name[ClientLanguage.English]
                , _snagging ? "with Snagging." : "without Snagging.");
            _start.Restart();
        }

        private void OnBite()
        {
            _start.Stop();
            PluginLog.Verbose("Fish bit at {FishingSpot} after {Milliseconds} using {Bait} {Snagging}.",
                _currentSpot?.PlaceName?[ClientLanguage.English] ?? "Undiscovered Fishing Hole", _start.ElapsedMilliseconds
                , _currentBait.Name[ClientLanguage.English], _snagging ? "with Snagging." : "without Snagging.");
            _bite.Restart();
        }

        private void OnIdentification(FishingSpot spot)
        {
            _currentSpot = spot;
            PluginLog.Verbose("Identified previously unknown fishing spot as {FishingSpot}.", _currentSpot.PlaceName![ClientLanguage.English]);
        }

        private void OnCatch(Fish fish)
        {
            _lastFish = fish;
            _bite.Stop();
            if (_lastFish.Record.Update(_currentBait, (ushort) _start.ElapsedMilliseconds, _snagging, _bite.ElapsedMilliseconds))
                _fish.SaveFishRecords(_pi);
            PluginLog.Verbose("Caught {Fish} at {FishingSpot} after {Milliseconds} and {Milliseconds2} using {Bait} {Snagging}.",
                _lastFish.Name![ClientLanguage.English], _currentSpot!.PlaceName![ClientLanguage.English], _start.ElapsedMilliseconds,
                _bite.ElapsedMilliseconds, _currentBait.Name[ClientLanguage.English], _snagging ? "with Snagging." : "without Snagging.");
        }

        private void OnMooch()
        {
            _currentBait     = new Bait(_lastFish!.Id, _lastFish.Name!);
            _currentFishList = SortedFish();
            PluginLog.Verbose("Mooching with {Fish} at {FishingSpot} {Snagging}.", _lastFish!.Name![ClientLanguage.English],
                _currentSpot!.PlaceName![ClientLanguage.English], _snagging ? "with Snagging." : "without Snagging.");
            _start.Restart();
        }

        public FishingTimer(DalamudPluginInterface pi, GatherBuddyConfiguration config, FishManager fish)
        {
            _pi      = pi;
            _config  = config;
            _lang    = pi.ClientState.ClientLanguage;
            _fish    = fish;
            _bait    = new CurrentBait(pi.TargetModuleScanner);
            _parser  = new FishingParser(_pi, _fish);

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
            const uint invalidColor = 0x20FFFFFFu;
            var color = fish.Record.BiteType switch
            {
                BiteType.Weak      => 0x8030D030u,
                BiteType.Strong    => 0x8030D0D0u,
                BiteType.Legendary => 0x803030D0u,
                BiteType.Unknown   => invalidColor,
                _                  => invalidColor,
            };

            var recordsValid = RecordsValid(fish.Record);
            if (!recordsValid)
                color = invalidColor;

            var pos    = ImGui.GetCursorPosY();
            var height = ImGui.GetTextLineHeightWithSpacing() * 1.4f;

            if (recordsValid)
            {
                var biteMin = _rectMin + new Vector2(fish.Record.EarliestCatch / 30000f * _rectSize.X - 2, pos);
                var biteMax = _rectMin + new Vector2(fish.Record.LatestCatch / 30000f * _rectSize.X + 2,   pos + height);
                drawList.AddRectFilled(biteMin, biteMax, 0xFF000020);
            }

            ImGui.PushStyleColor(ImGuiCol.Button,        color);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, color);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive,  color);
            ImGui.PushStyleVar(ImGuiStyleVar.ButtonTextAlign, new Vector2(0f, 0.1f));
            ImGui.Button(fish.Name![_lang], new Vector2(_rectSize.X,          height));

            ImGui.PopStyleVar();
            ImGui.PopStyleColor(3);
        }

        private uint RecordOrder(Fish fish)
        {
            if (!RecordsValid(fish!.Record))
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
            if (!fishing)
            {
                _start.Reset();
                _bite.Reset();
                _currentFishList = new Fish[0];
                if (!EditMode)
                    return;
            }

            var diff       = _start.ElapsedMilliseconds;
            var diffPos    = _rectMin.X + 2 + _rectSize.X * diff / 30000;

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing,   new Vector2(0, 1));

            var lineHeight    = (ImGui.GetTextLineHeightWithSpacing() * 1.4f + 1);
            var textLines     = 2 * ImGui.GetTextLineHeightWithSpacing();
            var maxListHeight = 9 * lineHeight + textLines;
            var listHeight    = EditMode ? maxListHeight : _currentFishList.Length * lineHeight + textLines;

            ImGui.SetNextWindowSizeConstraints(new Vector2(200 * ImGui.GetIO().FontGlobalScale, maxListHeight), new Vector2(30000, listHeight));

            if (!ImGui.Begin("##FishingTimer", EditMode ? editFlags : flags))
                return;

            var drawList = ImGui.GetWindowDrawList();

            if (EditMode || _rectMin.X == 0)
            {
                _rectMin  = ImGui.GetWindowPos();
                _rectSize = new Vector2(ImGui.GetWindowSize().X, maxListHeight);
            }

            drawList.AddRectFilled(_rectMin, _rectMin + new Vector2(_rectSize.X, textLines), 0x80000000, 4f);
            if (fishing)
            {
                var secondText = (diff / 1000.0).ToString("00.0");

                ImGui.SetCursorPosX(5);
                ImGui.Text(_currentBait.Name[_lang]);
                ImGui.SetCursorPosX(5);
                ImGui.Text(_currentSpot?.PlaceName?[_lang] ?? "Unknown");
                ImGui.SameLine(_rectSize.X -ImGui.CalcTextSize(secondText).X - 5);
                ImGui.Text(secondText);

                foreach (var fish in _currentFishList)
                    DrawFish(drawList, fish!);

                if (_start.ElapsedMilliseconds > 0)
                {
                    drawList.AddLine(new Vector2(diffPos, _rectMin.Y  + textLines), new Vector2(diffPos, _rectMin.Y + listHeight - 2), 0xFF000000, 3);
                }
            }
            else if (EditMode)
            {
                ImGui.Text("  Bait");
                ImGui.Text("  Place and Time");
                drawList.AddRect(_rectMin, _rectMin + _rectSize - new Vector2(0, 1), 0xFF000000, 5);
                drawList.AddRectFilled(_rectMin, _rectMin + _rectSize, 0x20FFFFFF, 5);
                ImGui.SetCursorPos(new Vector2((_rectSize.X - ImGui.CalcTextSize("FISH").X) / 2,
                    (_rectSize.Y - ImGui.GetTextLineHeightWithSpacing()) / 2));
                ImGui.Text("FISH");
                ImGui.SetCursorPos(new Vector2((_rectSize.X - ImGui.CalcTextSize("TIMER").X) / 2,
                    (_rectSize.Y + ImGui.GetTextLineHeightWithSpacing()) / 2));
                ImGui.Text("TIMER");
            }


            ImGui.End();
            ImGui.PopStyleVar(2);
        }
    }
}
