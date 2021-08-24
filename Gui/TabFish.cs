using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using GatherBuddy.Classes;
using GatherBuddy.Utility;
using ImGuiNET;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private const int NumFishColumns = 8;

        public static string TimeString(long seconds, bool shortString)
        {
            return seconds switch
            {
                > RealTime.SecondsPerDay => shortString
                    ? $">{seconds / (float) RealTime.SecondsPerDay:F0}d"
                    : $"{seconds / (float) RealTime.SecondsPerDay:F2} Days",
                > RealTime.SecondsPerHour => shortString
                    ? $">{seconds / RealTime.SecondsPerHour}h"
                    : $"{seconds / RealTime.SecondsPerHour:D2}:{seconds / RealTime.SecondsPerMinute % RealTime.MinutesPerHour:D2} Hours",
                _ => shortString
                    ? $"{seconds / RealTime.SecondsPerMinute}:{seconds % RealTime.SecondsPerMinute:D2}m"
                    : $"{seconds / RealTime.SecondsPerMinute:D2}:{seconds % RealTime.SecondsPerMinute:D2} Minutes",
            };
        }

        private static void PrintSeconds(long seconds)
        {
            var t = TimeString(seconds, false);
            ImGui.Text(t);
        }

        private void DrawFish(Cache.Fish fish)
        {
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.Image(fish.Icon.ImGuiHandle, _fishIconSize);
            if (ImGui.IsItemHovered())
            {
                using var tt = ImGuiRaii.NewTooltip();
                ImGui.Image(fish.Icon.ImGuiHandle, new Vector2(fish.Icon.Width, fish.Icon.Height));
            }

            ImGui.TableNextColumn();
            if (fish.IsFixed)
                ImGui.PushStyleColor(ImGuiCol.Text, GatherBuddy.Config.AvailableFishColor);
            if (ImGui.Selectable(fish.Name))
                _plugin.Gatherer!.OnFishActionWithFish(fish.Base);
            if (fish.IsFixed)
                ImGui.PopStyleColor();
            if (ImGui.IsItemHovered())
                SetTooltip(fish);
            if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                _fishCache.ToggleFishFix(fish);

            static void DependencyWarning()
                => ImGui.TextColored(Colors.FishTab.DependencyWarning, "!!! May be dependent on intuition or mooch availability !!!");

            var uptime = fish.Base.NextUptime(_plugin.Gatherer!.WeatherManager);
            ImGui.TableNextColumn();
            if (uptime.Equals(RealUptime.Always))
            {
                ImGui.TextColored(
                    fish.HasUptimeDependency ? GatherBuddy.Config.DependentAvailableFishColor : GatherBuddy.Config.AvailableFishColor,
                    "Always Up");
                if (fish.HasUptimeDependency && ImGui.IsItemHovered())
                {
                    using var tt = ImGuiRaii.NewTooltip();
                    DependencyWarning();
                }
            }
            else if (uptime.Equals(RealUptime.Unknown))
            {
                ImGui.TextColored(GatherBuddy.Config.UpcomingFishColor, "Unknown");
            }
            else
            {
                var seconds  = (long) (uptime.Time - DateTime.UtcNow).TotalSeconds;
                var duration = (long) (DateTime.UtcNow - uptime.EndTime).TotalSeconds;
                if (seconds > 0)
                {
                    using var color = new ImGuiRaii().PushColor(ImGuiCol.Text,
                        fish.HasUptimeDependency ? GatherBuddy.Config.DependentUpcomingFishColor : GatherBuddy.Config.UpcomingFishColor);
                    PrintSeconds(seconds);
                }
                else if (duration < 0)
                {
                    using var color = new ImGuiRaii().PushColor(ImGuiCol.Text,
                        fish.HasUptimeDependency ? GatherBuddy.Config.DependentAvailableFishColor : GatherBuddy.Config.AvailableFishColor);
                    PrintSeconds(-duration);
                }

                if (ImGui.IsItemHovered())
                {
                    using var tt = ImGuiRaii.NewTooltip();
                    if (fish.HasUptimeDependency)
                        DependencyWarning();
                    ImGui.Text($"{uptime.Time.ToLocalTime()} Next Uptime\n{uptime.EndTime.ToLocalTime()} End Time\n{uptime.Duration} Duration");
                }
            }

            ImGui.TableNextColumn();
            ImGui.Dummy(new Vector2(_fishCache.LongestPercentage - ImGui.CalcTextSize(fish.UptimeString).X, 0));
            ImGui.SameLine();
            ImGui.Text(fish.UptimeString);

            ImGui.TableNextColumn();
            if (fish.Bait.Length > 0)
            {
                var baitIcon = fish.Bait[0].Icon;
                var baitName = fish.Bait[0].Name;

                ImGui.Image(baitIcon.ImGuiHandle, _fishIconSize);
                if (ImGui.IsItemHovered())
                {
                    using var tt = ImGuiRaii.NewTooltip();
                    ImGui.Image(baitIcon.ImGuiHandle, new Vector2(baitIcon.Width, baitIcon.Height));
                }

                ImGui.TableNextColumn();
                if (ImGui.Selectable(baitName))
                    _plugin.Gatherer!.OnBaitAction(baitName);

                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Click to copy to clipboard.");
            }
            else
            {
                ImGui.TableNextColumn();
                ImGui.Text("Unknown Bait");
            }

            ImGui.TableNextColumn();
            if (ImGui.Selectable(fish.FishingSpot))
                _plugin.Gatherer!.OnFishActionWithSpot(fish.Base.FishingSpots.First());

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip($"{fish.Territory}\nRight-click to open TeamCraft site for this spot.");

            if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                Process.Start(fish.FishingSpotTcAddress);

            ImGui.TableNextColumn();
            ImGui.Text(fish.Territory);
        }

        private void DrawAlreadyCaughtBox()
            => DrawCheckbox("Show Already Caught",    "Show fish that you are already collected in your Fish Guide.",
                GatherBuddy.Config.ShowAlreadyCaught, b => GatherBuddy.Config.ShowAlreadyCaught = b);

        private void DrawAlwaysUpBox()
            => DrawCheckbox("Show Always Up",    "Show fish that have neither weather nor time restrictions.",
                GatherBuddy.Config.ShowAlwaysUp, b =>
                {
                    GatherBuddy.Config.ShowAlwaysUp = b;
                    _fishCache.SetCurrentlyRelevantFish();
                });

        private void DrawBigFishBox()
            => DrawCheckbox("Show Big Fish",    "Show fish that are categorized as Big Fish.",
                GatherBuddy.Config.ShowBigFish, b =>
                {
                    GatherBuddy.Config.ShowBigFish = b;
                    _fishCache.SetCurrentlyRelevantFish();
                });

        private void DrawSmallFishBox()
            => DrawCheckbox("Show Small Fish", "Show fish that are not categorized as Big Fish.",
                GatherBuddy.Config.ShowSmallFish,
                b =>
                {
                    GatherBuddy.Config.ShowSmallFish = b;
                    _fishCache.SetCurrentlyRelevantFish();
                });

        private void DrawSpearFishBox()
            => DrawCheckbox("Show Spearfishing", "Show fish that are caught via Spearfishing.",
                GatherBuddy.Config.ShowSpearFish,
                b =>
                {
                    GatherBuddy.Config.ShowSpearFish = b;
                    _fishCache.SetCurrentlyRelevantFish();
                });

        private void DrawReleasePatchCombo()
        {
            ImGui.SetNextItemWidth(-1);
            var tmp = (int) GatherBuddy.Config.ShowFishFromPatch;
            if (ImGui.Combo("Release Patch", ref tmp, Cache.FishTab.Patches, Cache.FishTab.Patches.Length)
             && tmp != GatherBuddy.Config.ShowFishFromPatch)
            {
                GatherBuddy.Config.ShowFishFromPatch = (byte) tmp;
                _fishCache.Selector                  = Cache.FishTab.PatchSelector[tmp];
                GatherBuddy.Config.Save();
                _fishCache.SetCurrentlyRelevantFish();
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show either all fish, fish from a specific expansion or fish from a specific release.");
        }

        private void DrawFishTab()
        {
            var height = -(_textHeight + _itemSpacing.Y * 3 + _framePadding.Y) * 2;
            var width  = ImGui.GetWindowWidth() - 4 * _itemSpacing.X;

            ImGui.SetNextItemWidth(width / 4);
            if (ImGui.InputTextWithHint("##FishFilter", "Fish Filter", ref _fishCache.FishFilter, 64))
                _fishCache.UpdateFishFilter();

            ImGui.SameLine();
            ImGui.SetNextItemWidth(width / 4);
            if (ImGui.InputTextWithHint("##BaitFilter", "Bait Filter", ref _fishCache.BaitFilter, 64))
                _fishCache.UpdateBaitFilter();

            ImGui.SameLine();
            ImGui.SetNextItemWidth(width / 4);
            if (ImGui.InputTextWithHint("##SpotFilter", "Spot Filter", ref _fishCache.SpotFilter, 64))
                _fishCache.UpdateSpotFilter();

            ImGui.SameLine();
            ImGui.SetNextItemWidth(width / 4);
            if (ImGui.InputTextWithHint("##TerritoryFilter", "Zone Filter", ref _fishCache.ZoneFilter, 64))
                _fishCache.UpdateZoneFilter();


            using var child = new ImGuiRaii();
            if (!child.Begin(() => ImGui.BeginChild("##Fish", new Vector2(-1, height), true), ImGui.EndChild))
                return;

            var actualFish = _fishCache.GetFishToSettings();

            var lineHeight = (int) Math.Ceiling(ImGui.GetWindowHeight() / _textHeight) + 1;

            bool BeginTable()
            {
                const ImGuiTableFlags       flags           = ImGuiTableFlags.BordersInner | ImGuiTableFlags.RowBg | ImGuiTableFlags.Hideable;
                const ImGuiTableColumnFlags offColumnFlags  = ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoReorder;
                const ImGuiTableColumnFlags mainColumnFlags = offColumnFlags | ImGuiTableColumnFlags.NoHide;
                if (!ImGui.BeginTable("##FishTable", NumFishColumns, flags))
                    return false;

                ImGui.TableHeader("");
                ImGui.TableSetupColumn("", mainColumnFlags, ImGui.GetTextLineHeight());
                ImGui.NextColumn();

                ImGui.TableHeader("Name");
                ImGui.TableSetupColumn("Name", mainColumnFlags, _fishCache.LongestFish * _globalScale);
                ImGui.NextColumn();

                ImGui.TableHeader("Wait / Uptime");
                ImGui.TableSetupColumn("Wait / Uptime", mainColumnFlags, _fishCache.LongestMinutes * _globalScale);
                ImGui.NextColumn();

                ImGui.TableHeader("%Uptime");
                ImGui.TableSetupColumn("%Uptime", offColumnFlags, _fishCache.LongestPercentage * _globalScale);
                ImGui.NextColumn();

                ImGui.TableHeader("BaitIcon");
                ImGui.TableSetupColumn("", offColumnFlags, ImGui.GetTextLineHeight());
                ImGui.NextColumn();

                ImGui.TableHeader("Bait");
                ImGui.TableSetupColumn("Bait", offColumnFlags, _fishCache.LongestBait * _globalScale);
                ImGui.NextColumn();

                ImGui.TableHeader("Fishing Spot");
                ImGui.TableSetupColumn("Fishing Spot", offColumnFlags, _fishCache.LongestSpot * _globalScale);
                ImGui.NextColumn();

                ImGui.TableHeader("Zone");
                ImGui.TableSetupColumn("Zone", offColumnFlags, _fishCache.LongestZone * _globalScale);
                ImGui.NextColumn();

                ImGui.TableHeadersRow();
                ImGui.TableNextRow();

                return true;
            }

            var num = actualFish.Length + _fishCache.FixedFish.Count;
            if (num < lineHeight + 5 && BeginTable())
                try
                {
                    // FixedFish might be changed
                    foreach (var f in _fishCache.FixedFish.ToArray())
                        DrawFish(f);

                    foreach (var f in actualFish)
                        DrawFish(f);
                }
                finally
                {
                    ImGui.EndTable();
                }
            else
                ClippedDraw(new FusedList<Cache.Fish>(_fishCache.FixedFish.ToArray(), actualFish), DrawFish, BeginTable, ImGui.EndTable);

            child.End();
            if (!child.Begin(() => ImGui.BeginChild("##FishSelection", -Vector2.One, true), ImGui.EndChild))
                return;

            child.Group();
            DrawAlreadyCaughtBox();
            DrawAlwaysUpBox();
            child.End();
            ImGui.SameLine();
            child.Group();
            DrawBigFishBox();
            DrawSmallFishBox();
            child.End();
            ImGui.SameLine();
            child.Group();
            DrawSpearFishBox();
            DrawReleasePatchCombo();
        }
    }
}
