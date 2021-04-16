using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.Utility;
using ImGuiNET;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
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

        private void DrawFish(Cache.Fish fish)
        {
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.Image(fish.Icon.ImGuiHandle, _fishIconSize);
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Image(fish.Icon.ImGuiHandle, new Vector2(fish.Icon.Width, fish.Icon.Height));
                ImGui.EndTooltip();
            }

            ImGui.TableNextColumn();
            if (ImGui.Selectable(fish.Name))
                _plugin.Gatherer!.OnFishActionWithFish(fish.Base);
            if (ImGui.IsItemHovered())
                SetTooltip(fish);

            var uptime = fish.Base.NextUptime(_plugin.Gatherer!.WeatherManager);
            ImGui.TableNextColumn();
            if (uptime.Equals(RealUptime.Always))
            {
                ImGui.TextColored(Colors.FishTab.UptimeRunning, "Always Up");
            }
            else if (uptime.Equals(RealUptime.Unknown))
            {
                ImGui.TextColored(Colors.FishTab.UptimeUnknown, "Unknown");
            }
            else
            {
                var seconds  = (long) (uptime.Time - DateTime.UtcNow).TotalSeconds;
                var duration = (long) (DateTime.UtcNow - uptime.EndTime).TotalSeconds;
                if (seconds > 0)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, Colors.FishTab.UptimeUpcoming);
                    PrintSeconds(seconds);
                    ImGui.PopStyleColor();
                }
                else if (duration < 0)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, Colors.FishTab.UptimeRunning);
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
                _plugin.Gatherer!.OnFishActionWithSpot(fish.Base.FishingSpots.First());
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(fish.Territory);

            ImGui.TableNextColumn();
            ImGui.Text(fish.Territory);
        }

        private void DrawAlreadyCaughtBox()
            => DrawCheckbox("Show Already Caught", "Show fish that you are already collected in your Fish Guide.",
                _config.ShowAlreadyCaught,         b => _config.ShowAlreadyCaught = b);

        private void DrawAlwaysUpBox()
            => DrawCheckbox("Show Always Up", "Show fish that have neither weather nor time restrictions.",
                _config.ShowAlwaysUp,         b =>
                {
                    _config.ShowAlwaysUp = b;
                    _fishCache.SetCurrentlyRelevantFish();
                });

        private void DrawBigFishBox()
            => DrawCheckbox("Show Big Fish", "Show fish that are categorized as Big Fish.",
                _config.ShowBigFish,         b =>
                {
                    _config.ShowBigFish = b;
                    _fishCache.SetCurrentlyRelevantFish();
                });

        private void DrawSmallFishBox()
            => DrawCheckbox("Show Small Fish", "Show fish that are not categorized as Big Fish.",
                _config.ShowSmallFish,
                b =>
                {
                    _config.ShowSmallFish = b;
                    _fishCache.SetCurrentlyRelevantFish();
                });

        private void DrawSpearFishBox()
            => DrawCheckbox("Show Spearfishing", "Show fish that are caught via Spearfishing.",
                _config.ShowSpearFish,
                b =>
                {
                    _config.ShowSpearFish = b;
                    _fishCache.SetCurrentlyRelevantFish();
                });

        private void DrawReleasePatchCombo()
        {
            ImGui.SetNextItemWidth(-1);
            var tmp = (int) _config.ShowFishFromPatch;
            if (ImGui.Combo("Release Patch", ref tmp, Cache.FishTab.Patches, Cache.FishTab.Patches.Length) && tmp != _config.ShowFishFromPatch)
            {
                _config.ShowFishFromPatch = (byte) tmp;
                _fishCache.Selector       = Cache.FishTab.PatchSelector[tmp];
                Save();
                _fishCache.SetCurrentlyRelevantFish();
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show either all fish, fish from a specific expansion or fish from a specific release.");
        }

        private void DrawFishTab()
        {
            var height = -(_textHeight + _itemSpacing.Y * 3 + _framePadding.Y) * 2;
            var width  = ImGui.GetWindowWidth() - 4 * _itemSpacing.X;

            ImGui.SetNextItemWidth(width / 3);
            if (ImGui.InputTextWithHint("##FishFilter", "Fish Filter", ref _fishCache.FishFilter, 64))
                _fishCache.UpdateFishFilter();

            ImGui.SameLine();
            ImGui.SetNextItemWidth(width / 3);
            if (ImGui.InputTextWithHint("##SpotFilter", "Spot Filter", ref _fishCache.SpotFilter, 64))
                _fishCache.UpdateSpotFilter();

            ImGui.SameLine();
            ImGui.SetNextItemWidth(width / 3);
            if (ImGui.InputTextWithHint("##TerritoryFilter", "Zone Filter", ref _fishCache.ZoneFilter, 64))
                _fishCache.UpdateZoneFilter();


            if (!ImGui.BeginChild("##Fish", new Vector2(-1, height), true))
                return;
            
            var actualFish = _fishCache.GetFishToSettings();

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
                ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthFixed, _fishCache.LongestFish * _globalScale);
                ImGui.NextColumn();

                ImGui.TableHeader("Wait / Uptime");
                ImGui.TableSetupColumn("Wait / Uptime", ImGuiTableColumnFlags.WidthFixed, ImGui.CalcTextSize("0000:00 Minutes").X);
                ImGui.NextColumn();

                ImGui.TableHeader("Fishing Spot");
                ImGui.TableSetupColumn("Fishing Spot", ImGuiTableColumnFlags.WidthFixed, _fishCache.LongestSpot * _globalScale);
                ImGui.NextColumn();

                ImGui.TableHeader("Zone");
                ImGui.TableSetupColumn("Zone", ImGuiTableColumnFlags.WidthFixed, _fishCache.LongestZone * _globalScale);
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
