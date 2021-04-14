using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.Utility;
using ImGuiNET;
using ImGuiScene;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private readonly TextureWrap _hookSet;
        private readonly TextureWrap _precisionHookSet;
        private readonly TextureWrap _powerfulHookSet;
        private readonly TextureWrap _snagging;
        private readonly TextureWrap _smallGig;
        private readonly TextureWrap _normalGig;
        private readonly TextureWrap _largeGig;
        private readonly TextureWrap _gigs;

        private readonly Dictionary<Fish, CachedFish> _allCachedFish;
        private          Fish[]                       _currentlyRelevantFish;
        private          CachedFish[]                 _cachedFish;

        private static readonly string[]           Patches       = PreparePatches();
        private static readonly Func<Fish, bool>[] PatchSelector = PreparePatchSelectors();

        private readonly float  _longestName         = 0f;
        private readonly float  _longestSpot         = 0f;
        private readonly float  _longestZone         = 0f;
        private          string _fishFilter          = "";
        private          string _fishFilterLower     = "";
        private          string _fishSpotFilter      = "";
        private          string _fishSpotFilterLower = "";
        private          string _fishZoneFilter      = "";
        private          string _fishZoneFilterLower = "";

        private Func<Fish, bool> _currentSelector;

        private static void SetTooltip(CachedFish fish)
        {
            ImGui.BeginTooltip();
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(_itemSpacing.X, _itemSpacing.Y * 1.5f));
            fish.PrintTime();
            fish.PrintWeather();
            fish.PrintBait();
            fish.PrintPredators();
            fish.PrintFolklore();
            ImGui.PopStyleVar();
            ImGui.EndTooltip();
        }

        private const int NumFishColumns = 5;

        private static void PrintSeconds(long seconds)
        {
            if (seconds > RealTime.SecondsPerDay)
                ImGui.Text($"{seconds / (float) RealTime.SecondsPerDay:F2} Days");
            else if (seconds > RealTime.SecondsPerHour)
                ImGui.Text($"{seconds / RealTime.SecondsPerHour:D2}:{seconds / RealTime.SecondsPerMinute % RealTime.MinutesPerHour:D2} Hours");
            else
                ImGui.Text($"{seconds / RealTime.SecondsPerMinute:D2}:{seconds % RealTime.SecondsPerMinute:D2} Minutes");
        }

        private void DrawFish(CachedFish fish)
        {
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.Image(fish.Icon.ImGuiHandle,
                new Vector2(fish.Icon.Width * ImGui.GetTextLineHeight() / fish.Icon.Height, ImGui.GetTextLineHeight()));
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Image(fish.Icon.ImGuiHandle, new Vector2(fish.Icon.Width, fish.Icon.Height));
                ImGui.EndTooltip();
            }

            ImGui.TableNextColumn();
            if (ImGui.Selectable(fish.Name))
                _plugin.Gatherer!.OnFishActionWithFish(fish.Fish);
            if (ImGui.IsItemHovered())
                SetTooltip(fish);

            var uptime = fish.Fish.NextUptime(_plugin.Gatherer!.WeatherManager);
            ImGui.TableNextColumn();
            if (uptime.Equals(RealUptime.Always))
            {
                ImGui.TextColored(new Vector4(0f, 0.75f, 0f, 1f), "Always Up");
            }
            else if (uptime.Equals(RealUptime.Unknown))
            {
                ImGui.TextColored(new Vector4(0.75f, 0.75f, 0f, 1f), "Unknown");
            }
            else
            {
                var seconds  = (long) (uptime.Time - DateTime.UtcNow).TotalSeconds;
                var duration = (long) (DateTime.UtcNow - uptime.EndTime).TotalSeconds;
                if (seconds > 0)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0.75f, 0.75f, 0f, 1f));
                    PrintSeconds(seconds);
                    ImGui.PopStyleColor();
                }
                else if (duration < 0)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0f, 0.75f, 0f, 1f));
                    PrintSeconds(-duration);
                    ImGui.PopStyleColor();
                }

                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text($"{uptime.Time.ToLocalTime()} Next Uptime");
                    ImGui.Text($"{uptime.EndTime.ToLocalTime()} End Time");
                    ImGui.Text($"{uptime.Duration} Duration");
                    ImGui.EndTooltip();
                }
            }

            ImGui.TableNextColumn();
            if (ImGui.Selectable(fish.FishingSpot))
                _plugin.Gatherer!.OnFishActionWithSpot(fish.Fish.FishingSpots.First());
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(fish.Territory);
            
            ImGui.TableNextColumn();
            ImGui.Text(fish.Territory);
        }

        private void UpdateFish(long totalHour)
        {
            if (totalHour <= _totalHourFish)
                return;

            _totalHourFish = totalHour;
            _cachedFish = _currentlyRelevantFish
                .OrderBy(f => f.NextUptime(_plugin.Gatherer!.WeatherManager), new ComparerB())
                .Select(f => _allCachedFish[f]).ToArray();
        }

        private void SetCurrentlyRelevantFish()
        {
            var weather = _plugin.Gatherer!.WeatherManager;
            _currentlyRelevantFish = _plugin.Gatherer!.FishManager.FishByUptime
                .Where(SelectFish)
                .ToArray();
            _cachedFish = _currentlyRelevantFish
                .Select(f => _allCachedFish[f])
                .OrderBy(f => f.Fish.NextUptime(_plugin.Gatherer!.WeatherManager), new ComparerB())
                .ToArray();
        }

        private static string[] PreparePatches()
        {
            var patches = (Patch[]) Enum.GetValues(typeof(Patch));
            var expansions = new string[]
            {
                "All",
                "A Realm Reborn",
                "Heavensward",
                "Stormblood",
                "Shadowbringers",
                "Endwalker",
            };
            return expansions.Concat(patches
                .Select(PatchExtensions.ToVersionString)).ToArray();
        }

        private static Func<Fish, bool>[] PreparePatchSelectors()
        {
            var patches = (Patch[]) Enum.GetValues(typeof(Patch));
            var expansions = new Func<Fish, bool>[]
            {
                _ => true,
                f => (f.CatchData?.Patch.ToExpansion() ?? 0) == Patch.ARealmReborn,
                f => (f.CatchData?.Patch.ToExpansion() ?? 0) == Patch.Heavensward,
                f => (f.CatchData?.Patch.ToExpansion() ?? 0) == Patch.Stormblood,
                f => (f.CatchData?.Patch.ToExpansion() ?? 0) == Patch.Shadowbringers,
                f => (f.CatchData?.Patch.ToExpansion() ?? 0) == Patch.Endwalker,
            };
            return expansions.Concat(patches
                .Select(p => new Func<Fish, bool>(f => f.CatchData!.Patch == p))).ToArray();
        }

        private bool CheckFishType(Fish f)
        {
            if (f.IsBigFish)
                return _config.ShowBigFish;

            return f.IsSpearFish ? _config.ShowSpearFish : _config.ShowSmallFish;
        }

        private bool SelectFish(Fish f)
        {
            if (!_currentSelector(f))
                return false;

            if (!_config.ShowAlwaysUp && f.FishRestrictions == FishRestrictions.None)
                return false;

            if (!CheckFishType(f))
                return false;

            return true;
        }

        private bool FishUncaught(Fish f)
            => !_plugin.Gatherer!.FishManager.FishLog.IsUnlocked(f);

        private void DrawAlreadyCaughtBox()
            => DrawCheckbox("Show Already Caught", "Show fish that you are already collected in your Fish Guide.",
                _config.ShowAlreadyCaught,         b => _config.ShowAlreadyCaught = b);

        private void DrawAlwaysUpBox()
            => DrawCheckbox("Show Always Up", "Show fish that have neither weather nor time restrictions.",
                _config.ShowAlwaysUp,         b =>
                {
                    _config.ShowAlwaysUp = b;
                    SetCurrentlyRelevantFish();
                });

        private void DrawBigFishBox()
            => DrawCheckbox("Show Big Fish", "Show fish that are categorized as Big Fish.",
                _config.ShowBigFish,         b =>
                {
                    _config.ShowBigFish = b;
                    SetCurrentlyRelevantFish();
                });

        private void DrawSmallFishBox()
            => DrawCheckbox("Show Small Fish", "Show fish that are not categorized as Big Fish.",
                _config.ShowSmallFish,
                b =>
                {
                    _config.ShowSmallFish = b;
                    SetCurrentlyRelevantFish();
                });

        private void DrawSpearFishBox()
            => DrawCheckbox("Show Spearfishing", "Show fish that are caught via Spearfishing.",
                _config.ShowSpearFish,
                b =>
                {
                    _config.ShowSpearFish = b;
                    SetCurrentlyRelevantFish();
                });

        private void DrawReleasePatchCombo()
        {
            ImGui.SetNextItemWidth(-1);
            var tmp = (int) _config.ShowFishFromPatch;
            if (ImGui.Combo("Release Patch", ref tmp, Patches, Patches.Length) && tmp != _config.ShowFishFromPatch)
            {
                _config.ShowFishFromPatch = (byte) tmp;
                _currentSelector          = PatchSelector[tmp];
                Save();
                SetCurrentlyRelevantFish();
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show either all fish, fish from a specific expansion or fish from a specific release.");
        }

        private class ComparerB : IComparer<RealUptime>
        {
            public int Compare(RealUptime x, RealUptime y)
                => x.Compare(y);
        }

        private void DrawFishTab()
        {
            var height = -(_textHeight + _itemSpacing.Y * 3 + _framePadding.Y) * 2;
            var width  = ImGui.GetWindowWidth() - 4 * _itemSpacing.X;

            ImGui.SetNextItemWidth(width / 3);
            if (ImGui.InputTextWithHint("##FishFilter", "Fish Filter", ref _fishFilter, 64))
                _fishFilterLower = _fishFilter.ToLower();

            ImGui.SameLine();
            ImGui.SetNextItemWidth(width / 3);
            if (ImGui.InputTextWithHint("##SpotFilter", "Spot Filter", ref _fishSpotFilter, 64))
                _fishSpotFilterLower = _fishSpotFilter.ToLower();

            ImGui.SameLine();
            ImGui.SetNextItemWidth(width / 3);
            if (ImGui.InputTextWithHint("##TerritoryFilter", "Zone Filter", ref _fishZoneFilter, 64))
                _fishZoneFilterLower = _fishZoneFilter.ToLower();


            if (!ImGui.BeginChild("##Fish", new Vector2(-1, height), true))
                return;

            var actualFish = _config.ShowAlreadyCaught && _fishFilter.Length == 0
                ? _cachedFish
                : _cachedFish.Where(f => (_config.ShowAlreadyCaught
                     || FishUncaught(f.Fish))
                 && f.NameLower.Contains(_fishFilterLower)
                 && f.TerritoryLower.Contains(_fishZoneFilterLower)
                 && f.FishingSpotLower.Contains(_fishSpotFilterLower)).ToArray();

            var lineHeight = (int) Math.Ceiling(ImGui.GetWindowHeight() / _textHeight) + 1;

            bool BeginTable()
            {
                const ImGuiTableFlags flags = ImGuiTableFlags.BordersInner | ImGuiTableFlags.RowBg;

                if (!ImGui.BeginTable("##FishTable", NumFishColumns, flags))
                {
                    ImGui.EndChild();
                    return false;
                }

                ImGui.TableHeader("");
                ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed, ImGui.GetTextLineHeight());
                ImGui.NextColumn();

                ImGui.TableHeader("Name");
                ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthFixed, _longestName * _globalScale);
                ImGui.NextColumn();

                ImGui.TableHeader("Wait / Uptime");
                ImGui.TableSetupColumn("Wait / Uptime", ImGuiTableColumnFlags.WidthFixed, ImGui.CalcTextSize("0000:00 Minutes").X);
                ImGui.NextColumn();

                ImGui.TableHeader("Fishing Spot");
                ImGui.TableSetupColumn("Fishing Spot", ImGuiTableColumnFlags.WidthFixed, _longestSpot * _globalScale);
                ImGui.NextColumn();

                ImGui.TableHeader("Zone");
                ImGui.TableSetupColumn("Zone", ImGuiTableColumnFlags.WidthFixed, _longestZone * _globalScale);
                ImGui.NextColumn();

                return true;
            }

            if (actualFish.Length < lineHeight + 5)
            {
                BeginTable();
                foreach (var f in actualFish)
                    DrawFish(f);
                ImGui.EndTable();
            }
            else
            {
                ClippedDraw(actualFish, DrawFish, BeginTable, ImGui.EndTable);
            }

            ImGui.EndChild();
            if (!ImGui.BeginChild("##FishSelection", -Vector2.One, true))
                return;

            ImGui.BeginGroup();
            DrawAlreadyCaughtBox();
            DrawAlwaysUpBox();
            ImGui.EndGroup();
            ImGui.SameLine();
            ImGui.BeginGroup();
            DrawBigFishBox();
            DrawSmallFishBox();
            ImGui.EndGroup();
            ImGui.SameLine();
            ImGui.BeginGroup();
            DrawSpearFishBox();
            DrawReleasePatchCombo();
            ImGui.EndGroup();

            ImGui.EndChild();
        }
    }
}
