using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.FishTimer;
using OtterGui.Text;
using OtterGui.Widgets;
using static GatherBuddy.FishTimer.FishRecord;
using FishingSpot = GatherBuddy.Classes.FishingSpot;
using Bait = GatherBuddy.Structs.Bait;
using Effects = GatherBuddy.Models.Effects;


namespace GatherBuddy.Gui;

public partial class Interface
{
    private static readonly FrozenDictionary<uint, FishingSpot>     Locations               = GatherBuddy.GameData.FishingSpots;
    private static readonly FrozenDictionary<uint, Bait>            Baits                   = GatherBuddy.GameData.Bait;
    private static readonly FrozenDictionary<ushort, CosmicMission> Missions                = GatherBuddy.GameData.CosmicFishingMissions;
    private                 List<FishRecord>                        _records                = [];
    private                 int                                     _selectedFishingSpotIdx = GatherBuddy.Config.FishStatsSelectedIdx;
    private                 FishingSpot                             _selectedSpot           = Locations.Values[GatherBuddy.Config.FishStatsSelectedIdx];
    private                 ClippedSelectableCombo<FishingSpot>?    _fishingSpotCombo;

    private static string CosmicHandler(FishingSpot spot)
    {
        var name = spot.Name.AsSpan();
        if (name.EndsWith(')'))
            if (ushort.TryParse(name[^5..^1], out var parsedNumber))
            {
                name = name[..^8];
                return $"{name} ({Missions[parsedNumber].Name})";
            }

        return $"{name}";
    }

    private void DrawSelectorDropdown()
    {
        if (_fishingSpotCombo == null)
        {
            // Probably use a custom FilterComboCache here.
            var spots = Locations.Values.ToList();
            _fishingSpotCombo = new ClippedSelectableCombo<FishingSpot>(
                "FishingSpotSelector",
                "Fishing Spot",
                500,
                spots,
                CosmicHandler
            );
        }

        if (_fishingSpotCombo.Draw(_selectedFishingSpotIdx, out var newIdx))
        {
            _selectedFishingSpotIdx                 = newIdx;
            _selectedSpot                           = Locations.Values[_selectedFishingSpotIdx];
            GatherBuddy.Config.FishStatsSelectedIdx = _selectedFishingSpotIdx;
            GatherBuddy.Config.Save();
        }
    }

    private void DrawCopier()
    {
        var copyStatsString = GenerateSpotReport();
        if (ImUtf8.Button($"Copy Fish Stats for {CosmicHandler(_selectedSpot)}"))
            ImUtf8.SetClipboardText(copyStatsString);

        ImUtf8.HoverTooltip(copyStatsString);
    }

    private string GenerateSpotReport()
    {
        var recordsAtSpot = _records
            .Where(r => r.SpotId == _selectedSpot.Id && r is { HasCatch: true, HasBait: true })
            .GroupBy(r => r.BaitId)
            .OrderBy(g => g.First().Bait)
            .ToList();

        var sb = new StringBuilder();

        // Location Name + Special Handler for Cosmic Missions
        var    name    = _selectedSpot.Name.AsSpan();
        ushort numbers = 0;
        if (name.EndsWith(')'))
            if (ushort.TryParse(name[^5..^1], out var parsedNumber))
            {
                numbers = parsedNumber;
                name    = name[..^8];
            }

        sb.AppendLine($"Location: {name}");
        if (numbers > 0)
            sb.AppendLine($"Mission: {Missions[numbers].Name}");
        sb.AppendLine("Collection Method: GatherBuddy Report Generator");
        sb.AppendLine("");

        foreach (var baitGroup in recordsAtSpot)
        {
            var baitRecords = baitGroup.ToList();
            var bait        = baitRecords[0].BaitId;
            var baitName = GatherBuddy.GameData.Bait.TryGetValue(bait, out var b) ? b.Name :
                GatherBuddy.GameData.Fishes.TryGetValue(bait, out var f)          ? $"Mooch - {new Bait(f.ItemData).Name}" : Bait.Unknown.Name;

            // Bait Name
            sb.AppendLine($"Bait: {baitName}");

            var fishIdToIndex = _selectedSpot.Items
                .Select((fish, index) => new { fish.FishId, index })
                .ToDictionary(x => x.FishId, x => x.index);

            var fishGroups = baitRecords
                .GroupBy(r => r.CatchId)
                .OrderBy(g =>
                {
                    var fishId = g.First().Catch!.FishId;
                    return fishIdToIndex.GetValueOrDefault(fishId, int.MaxValue);
                })
                .ToList();

            foreach (var fishGroup in fishGroups)
            {
                var fishRecords = fishGroup.ToList();
                var fish        = fishRecords[0].Catch!;
                var count       = fishRecords.Count;

                if (count == 0)
                {
                    sb.AppendLine($"{fish.Name}:\n- (0 Caught)");
                    continue;
                }

                // Always include fish name
                sb.AppendLine($"{fish.Name}:");

                // Time
                if (GatherBuddy.Config.EnableReportTime)
                {
                    var timeRecords = fishRecords.Where(r => (!r.Flags.HasLure() || r.Flags.HasValidLure()) && !r.Flags.HasFlag(Effects.Chum)).ToList();

                    if (timeRecords.Count == 0)
                    {
                        sb.AppendLine("- No valid time data.");
                    }
                    else
                    {
                        var minTime = timeRecords.Min(r => r.Bite) / 1000f;
                        var maxTime = timeRecords.Max(r => r.Bite) / 1000f;

                        var lureRecords = timeRecords.Where(r => r.Flags.HasFlag(Effects.ValidLure)).ToList();
                        var isMinLure   = lureRecords.Count != 0 && lureRecords.Min(r => r.Bite) / 1000f == minTime;

                        var timeLine = $"- Times: {minTime} - {maxTime} ({timeRecords.Count} caught)";
                        if (isMinLure)
                            timeLine += " (Lure Min!)";

                        sb.AppendLine(timeLine);
                    }
                }

                if (GatherBuddy.Config.EnableReportSize)
                {
                    var filterFlags = new[]
                    {
                        Effects.Patience,
                        Effects.Patience2,
                        Effects.PrizeCatch,
                    };

                    (string label, List<ushort> sizes, Dictionary<Effects, int> effectCounts, int count) Classify(
                        IEnumerable<FishRecord> records, string label)
                    {
                        var list          = records.ToList();
                        var sizes         = list.Select(r => r.Size).ToList();
                        var effectCounter = new Dictionary<Effects, int>();

                        foreach (var flag in filterFlags)
                            effectCounter[flag] = list.Count(r => r.Flags.HasFlag(flag));

                        return (label, sizes, effectCounter, list.Count);
                    }

                    var normal = Classify(
                        fishRecords.Where(r => !r.Flags.HasFlag(Effects.Large) && !r.Flags.HasFlag(Effects.BigGameFishing)),
                        "Average");

                    var large = Classify(
                        fishRecords.Where(r => r.Flags.HasFlag(Effects.Large) && !r.Flags.HasFlag(Effects.BigGameFishing)),
                        "Large");

                    var bgf = Classify(
                        fishRecords.Where(r => r.Flags.HasFlag(Effects.BigGameFishing)),
                        "BGF");


                    string Format((string label, List<ushort> sizes, Dictionary<Effects, int> effects, int count) group)
                    {
                        if (group.sizes.Count == 0)
                            return $"No {group.label} Fish";

                        var min = group.sizes.Min() / 10f;
                        var max = group.sizes.Max() / 10f;

                        var effectText = string.Join(", ",
                            filterFlags
                                .Where(f => group.effects[f] > 0)
                                .Select(f => $"{f}: {group.effects[f]}"));

                        return $"{min} - {max} ({group.count}" + (effectText != "" ? $" | [{effectText}])" : ")");
                    }

                    var sizeParts = new List<string>
                    {
                        Format(normal),
                        Format(large),
                        Format(bgf),
                    };

                    sb.AppendLine("- Sizes: " + string.Join(" | ", sizeParts));
                }

                if (GatherBuddy.Config.EnableReportMulti)
                {
                    var doubleHookRecords = fishRecords.Where(r => r.Hook == HookSet.DoubleHook).ToList();
                    var doubleHook        = doubleHookRecords.Count != 0 ? doubleHookRecords.Average(r => r.Amount) : 0;

                    var tripleHookRecords = fishRecords.Where(r => r.Hook == HookSet.TripleHook).ToList();
                    var tripleHook        = tripleHookRecords.Count != 0 ? tripleHookRecords.Average(r => r.Amount) : 0;

                    var hookLine = new List<string>();
                    if (doubleHook > 0)
                        hookLine.Add($"Double Hook Yield: {doubleHook}");
                    if (tripleHook > 0)
                        hookLine.Add($"Triple Hook Yield: {tripleHook}");

                    if (hookLine.Count != 0)
                        sb.AppendLine($"- {string.Join(" | ", hookLine)}");
                }
            }

            sb.AppendLine("");
        }

        return sb.ToString().TrimEnd();
    }

    private void DrawFishingSpotInfo()
    {
        ImUtf8.Text($"Coordinates: ({_selectedSpot.IntegralXCoord}, {_selectedSpot.IntegralYCoord})");
        ImUtf8.Text($"Territory: {_selectedSpot.Territory.Name}");
        ImGui.Separator();

        if (_selectedSpot.ClosestAetheryte != null)
            ImUtf8.Text($"Closest Aetheryte: {_selectedSpot.ClosestAetheryte.Name}");
        else
            ImUtf8.Text("No Aetheryte found nearby.");
        ImGui.Separator();

        if (_selectedSpot.Items.Any())
        {
            ImUtf8.Text("Available Fish:");
            foreach (var fish in _selectedSpot.Items)
                ImUtf8.Text("- " + fish.Name[GatherBuddy.Language]);
        }
        else
        {
            ImUtf8.Text("No fish available.");
        }
    }

    private void DrawFishInfo()
    {
        var recordsAtSpot = _records
            .Where(r => r.HasSpot && r.SpotId == _selectedSpot.Id && r.HasCatch && r.HasBait)
            .ToList();

        // Group by FishId
        var fishGroups = recordsAtSpot
            .GroupBy(r => r.CatchId)
            .ToDictionary(
                g => g.Key,
                g => g.ToList()
            );

        foreach (var (fishId, allEntries) in fishGroups)
        {
            var fish = GatherBuddy.GameData.Fishes.GetValueOrDefault(fishId);
            if (fish == null)
                continue;

            ImUtf8.Text(fish.Name[GatherBuddy.Language], 0xFFCCFF66);

            DrawSizeDistribution(allEntries);
            DrawBiteTimeHistogram(allEntries);

            ImGui.Spacing();

            var baitGroups = allEntries
                .GroupBy(r => r.BaitId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var (baitId, entries) in baitGroups)
            {
                var bait = GatherBuddy.GameData.Bait.GetValueOrDefault(baitId);
                ImUtf8.BulletText($"Bait: {bait?.Name ?? "Unknown"}");

                DrawAmountCaught(entries.Count);
                DrawCatchPercentage(entries, recordsAtSpot.Count(r => r.BaitId == baitId));

                ImGui.Separator();
            }

            ImGui.Separator();
        }
    }

    private static void DrawSizeDistribution(List<FishRecord> entries)
    {
        if (entries.Count == 0)
            return;

        var         windowSize   = new Vector2(ImGui.GetWindowSize().X / 2, ImGui.GetWindowSize().Y);
        var         chartWidth   = windowSize.X;
        var         bucketCount  = int.Min(10, entries.Count);
        var         totalSpacing = chartWidth * 0.1f;
        var         barAreaWidth = chartWidth - totalSpacing;
        var         barWidth     = barAreaWidth / bucketCount;
        var         barSpacing   = totalSpacing / (bucketCount - 1);
        const float chartHeight  = 150f;
        const float leftMargin   = 40f;
        const float bottomMargin = 30f;
        const int   yTicks       = 5;
        var         padding      = 5 * ImGuiHelpers.GlobalScale;

        var colorPalette = new List<Vector4>
        {
            new(0.5f, 0.5f, 0.5f, 1f),
            new(1f, 1f, 0.3f, 1f),
            new(0.4f, 0.6f, 1f, 1f),
        };

        var minSize   = entries.Min(r => r.Size / 10f);
        var maxSize   = entries.Max(r => r.Size / 10f);
        var sizeRange = maxSize - minSize;
        if (sizeRange == 0)
        {
            minSize   -= 1f;
            maxSize   += 1f;
            sizeRange =  maxSize;
        }

        var bucketWidth = sizeRange / bucketCount;

        // Buckets
        var buckets = new (int normal, int large, int bgf)[bucketCount];
        foreach (var entry in entries)
        {
            var bucketIndex = (int)((entry.Size / 10f - minSize) / bucketWidth);
            if (bucketIndex >= bucketCount)
                bucketIndex = bucketCount - 1;

            ref var b = ref buckets[bucketIndex];
            if (entry.Flags.HasFlag(Effects.BigGameFishing))
                b.bgf++;
            else if (entry.Flags.HasFlag(Effects.Large))
                b.large++;
            else
                b.normal++;
        }

        var maxCount = buckets.Max(b => b.normal + b.large + b.bgf);

        var drawList   = ImGui.GetWindowDrawList();
        var cursor     = ImGui.GetCursorScreenPos();
        var textHeight = ImGui.GetTextLineHeight();
        var origin     = new Vector2(cursor.X + leftMargin, cursor.Y + chartHeight + textHeight * (float)1.5);

        // Y-axis
        for (var i = 0; i <= yTicks; i++)
        {
            var yVal = maxCount * i / (float)yTicks;
            var y    = origin.Y - yVal / maxCount * chartHeight;

            drawList.AddLine(new Vector2(origin.X, y), new Vector2(origin.X + chartWidth, y), ImGui.GetColorU32(ImGuiCol.Border));
            drawList.AddText(new Vector2(origin.X - (ImUtf8.CalcTextSize($"{yVal}").X + padding), y - textHeight / 2),
                ImGui.GetColorU32(ImGuiCol.Text), $"{yVal}");
        }

        // Bars
        for (var i = 0; i < bucketCount; i++)
        {
            var x = origin.X + i * (barWidth + barSpacing);
            var y = origin.Y;

            var (normal, large, bgf) = buckets[i];

            DrawStack(ref y, normal, colorPalette[0]);
            DrawStack(ref y, large,  colorPalette[1]);
            DrawStack(ref y, bgf,    colorPalette[2]);

            // X-axis
            var fromVal  = minSize + i * bucketWidth;
            var fromText = $"{fromVal:F1}";
            var toText   = $"{fromVal + bucketWidth:F1}";
            var label    = $"{fromText}-{toText}";
            var textSize = ImUtf8.CalcTextSize(label);
            var labelX   = x + (barWidth - textSize.X) * 0.5f;
            drawList.AddText(new Vector2(labelX, origin.Y + 4f), ImGui.GetColorU32(ImGuiCol.Text), label);
            continue;

            void DrawStack(ref float yPos, int count, Vector4 color)
            {
                if (count <= 0)
                    return;

                var height = count / (float)maxCount * chartHeight;
                drawList.AddRectFilled(
                    new Vector2(x,            yPos - height),
                    new Vector2(x + barWidth, yPos),
                    ImGui.ColorConvertFloat4ToU32(color));
                yPos -= height;
            }
        }

        // Titles
        drawList.AddText(new Vector2(cursor.X + leftMargin + chartWidth * 0.5f - 20f, origin.Y + 20f), ImGui.GetColorU32(ImGuiCol.Text),
            "Size");
        drawList.AddText(ImGui.GetFont(), ImGui.GetFontSize(), new Vector2(cursor.X + 5f, cursor.Y), ImGui.GetColorU32(ImGuiCol.Text), "Count");

        // Legend
        var legendX = origin.X + chartWidth + 10f;
        var legendY = cursor.Y;
        for (var i = 0; i < colorPalette.Count; i++)
        {
            var sizeName =
                new List<string>
                {
                    "Average",
                    "Large",
                    "Big Game Fishing",
                }[i];
            var label = $"{sizeName}";
            drawList.AddRectFilled(new Vector2(legendX, legendY), new Vector2(legendX + textHeight, legendY + textHeight),
                ImGui.ColorConvertFloat4ToU32(colorPalette[i]));
            drawList.AddText(new Vector2(legendX + textHeight, legendY), ImGui.GetColorU32(ImGuiCol.Text), label);
            legendY += textHeight + 4f;
        }

        // Space
        ImGui.Dummy(new Vector2(leftMargin + chartWidth + 100f, chartHeight + bottomMargin + 20f));
    }

    private static void DrawAmountCaught(int count)
    {
        ImUtf8.Text($"Caught: {count} times");
    }

    private static void DrawCatchPercentage(List<FishRecord> entries, int baitTotal)
    {
        var percentage = entries.Count / (float)baitTotal * 100f;
        ImUtf8.Text($"Percent of baited catches: {percentage:F2}");
    }

    private static void DrawBiteTimeHistogram(List<FishRecord> entries)
    {
        if (entries.Count == 0)
            return;

        var         windowSize   = new Vector2(ImGui.GetWindowSize().X / 2, ImGui.GetWindowSize().Y);
        var         chartWidth   = windowSize.X;
        var         bucketCount  = Math.Min(10, entries.Count);
        var         totalSpacing = chartWidth * 0.1f;
        var         barAreaWidth = chartWidth - totalSpacing;
        var         barWidth     = barAreaWidth / bucketCount;
        var         barSpacing   = totalSpacing / (bucketCount - 1);
        const float chartHeight  = 150f;
        const float leftMargin   = 40f;
        const float bottomMargin = 30f;
        const int   yTicks       = 5;
        var         padding      = 5 * ImGuiHelpers.GlobalScale;

        var baitIds    = entries.Select(e => e.BaitId).Distinct().OrderBy(id => id).ToList();
        var baitColors = new Dictionary<uint, Vector4>();
        var colorPalette = new Vector4[]
        {
            new(0.4f, 0.6f, 1f, 1f),
            new(1f, 0.7f, 0.2f, 1f),
            new(0.6f, 1f, 0.6f, 1f),
            new(1f, 0.4f, 0.4f, 1f),
            new(1f, 1f, 0.3f, 1f),
            new(0.7f, 0.5f, 1f, 1f),
            new(0.4f, 1f, 0.9f, 1f),
        };
        for (var i = 0; i < baitIds.Count; i++)
            baitColors[baitIds[i]] = colorPalette[i % colorPalette.Length];

        var hatchColors = new List<uint>
        {
            // Blue color for "Chum"
            ImGui.ColorConvertFloat4ToU32(new Vector4(0.0f, 0.5f, 1.0f, 1.0f)), // Blue

            // Light red to dark red for "Amb"
            ImGui.ColorConvertFloat4ToU32(new Vector4(1.0f, 0.6f, 0.6f, 1.0f)), // Light red
            ImGui.ColorConvertFloat4ToU32(new Vector4(1.0f, 0.3f, 0.3f, 1.0f)), // Medium red
            ImGui.ColorConvertFloat4ToU32(new Vector4(0.6f, 0.0f, 0.0f, 1.0f)), // Dark red

            // Light green to dark green for "Mod"
            ImGui.ColorConvertFloat4ToU32(new Vector4(0.6f, 1.0f, 0.6f, 1.0f)), // Light green
            ImGui.ColorConvertFloat4ToU32(new Vector4(0.3f, 1.0f, 0.3f, 1.0f)), // Medium green
            ImGui.ColorConvertFloat4ToU32(new Vector4(0.0f, 0.5f, 0.0f, 1.0f)), // Dark green
        };

        var minTime     = entries.Min(e => e.Bite / 1000f);
        var maxTime     = entries.Max(e => e.Bite / 1000f);
        var timeRange   = maxTime - minTime;
        var bucketWidth = timeRange / bucketCount;

        var buckets = new List<Dictionary<uint, List<FishRecord>>>();
        for (var i = 0; i < bucketCount; i++)
            buckets.Add(new Dictionary<uint, List<FishRecord>>());
        foreach (var e in entries)
        {
            var bucketIndex = (int)((e.Bite / 1000f - minTime) / bucketWidth);

            if (bucketIndex < 0)
                bucketIndex = 0;
            else if (bucketIndex >= bucketCount)
                bucketIndex = bucketCount - 1;

            var bucket = buckets[bucketIndex];

            if (!bucket.TryGetValue(e.BaitId, out var list))
            {
                list             = [];
                bucket[e.BaitId] = list;
            }

            list.Add(e);
        }

        var maxCount = buckets
            .Where(dict => dict.Count > 0)
            .Select(dict => dict.Values.Sum(e => e.Count))
            .DefaultIfEmpty(0)
            .Max();

        var drawList   = ImGui.GetWindowDrawList();
        var cursor     = ImGui.GetCursorScreenPos();
        var textHeight = ImGui.GetTextLineHeight();
        var origin     = new Vector2(cursor.X + leftMargin, cursor.Y + chartHeight + textHeight * 1.5f);

        // Y-axis
        for (var i = 0; i <= yTicks; i++)
        {
            var yVal = maxCount * i / (float)yTicks;
            var y    = origin.Y - yVal / maxCount * chartHeight;

            drawList.AddLine(new Vector2(origin.X, y), new Vector2(origin.X + chartWidth, y), ImGui.GetColorU32(ImGuiCol.Border));
            var label = $"{yVal:0}";
            drawList.AddText(new Vector2(origin.X - (ImGui.CalcTextSize(label).X + padding), y - textHeight / 2),
                ImGui.GetColorU32(ImGuiCol.Text), label);
        }

        // Histogram bars
        for (var i = 0; i < bucketCount; i++)
        {
            var x = origin.X + i * (barWidth + barSpacing);
            var y = origin.Y;
            foreach (var bait in baitIds)
            {
                if (!buckets[i].ContainsKey(bait))
                    continue;

                if (!buckets[i].TryGetValue(bait, out var lRecords) || lRecords.Count == 0)
                    continue;

                var count     = lRecords.Count;
                var height    = count / (float)maxCount * chartHeight;
                var baitColor = ImGui.ColorConvertFloat4ToU32(baitColors[bait]);
                var barTop    = new Vector2(x + barWidth, y);
                var counter = new List<int>
                {
                    lRecords.Count(e =>
                        !e.Flags.HasFlag(Effects.Chum)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && !e.Flags.HasFlag(Effects.ModestLure1)
                     && !e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        e.Flags.HasFlag(Effects.Chum)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && !e.Flags.HasFlag(Effects.ModestLure1)
                     && !e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        e.Flags.HasFlag(Effects.Chum)
                     && e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && !e.Flags.HasFlag(Effects.ModestLure1)
                     && !e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        e.Flags.HasFlag(Effects.Chum)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && !e.Flags.HasFlag(Effects.ModestLure1)
                     && !e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        e.Flags.HasFlag(Effects.Chum)
                     && e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && !e.Flags.HasFlag(Effects.ModestLure1)
                     && !e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        e.Flags.HasFlag(Effects.Chum)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && e.Flags.HasFlag(Effects.ModestLure1)
                     && !e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        e.Flags.HasFlag(Effects.Chum)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && !e.Flags.HasFlag(Effects.ModestLure1)
                     && e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        e.Flags.HasFlag(Effects.Chum)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && e.Flags.HasFlag(Effects.ModestLure1)
                     && e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        !e.Flags.HasFlag(Effects.Chum)
                     && e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && !e.Flags.HasFlag(Effects.ModestLure1)
                     && !e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        !e.Flags.HasFlag(Effects.Chum)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && !e.Flags.HasFlag(Effects.ModestLure1)
                     && !e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        !e.Flags.HasFlag(Effects.Chum)
                     && e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && !e.Flags.HasFlag(Effects.ModestLure1)
                     && !e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        !e.Flags.HasFlag(Effects.Chum)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && e.Flags.HasFlag(Effects.ModestLure1)
                     && !e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        !e.Flags.HasFlag(Effects.Chum)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && !e.Flags.HasFlag(Effects.ModestLure1)
                     && e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e =>
                        !e.Flags.HasFlag(Effects.Chum)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure1)
                     && !e.Flags.HasFlag(Effects.AmbitiousLure2)
                     && e.Flags.HasFlag(Effects.ModestLure1)
                     && e.Flags.HasFlag(Effects.ModestLure2)),
                };

                for (var j = 0; j < counter.Count; j++)
                {
                    var c = counter[j];
                    if (c <= 0)
                        continue;

                    var barBot = new Vector2(x, barTop.Y);
                    barTop.Y -= c / (float)maxCount * chartHeight;

                    //First Draw bait background
                    drawList.AddRectFilled(barBot, barTop, baitColor);

                    // Then draw if it was chummed
                    if (j is >= 1 and <= 7)
                        DrawHatchedRectDown(barBot, barTop, 10f, drawList, hatchColors[0]);

                    // Then draw lure if present
                    if (j is >= 2 and <= 7)
                        DrawHatchedRectUp(barBot, barTop, 10f, drawList, hatchColors[j - 1]);
                    else if (j is >= 8 and <= 13)
                        DrawHatchedRectUp(barBot, barTop, 10f, drawList, hatchColors[j - 7]);

                    var heightTextSize = ImGui.CalcTextSize($"{c}");
                    drawList.AddText(
                        new Vector2(x + barWidth / 2 - heightTextSize.X / 2,
                            barBot.Y - c / (float)maxCount * chartHeight / 2 - heightTextSize.Y / 2), ImGui.GetColorU32(ImGuiCol.Text), $"{c}");
                }

                y -= height;
            }

            // X-axis
            var from     = minTime + i * bucketWidth;
            var to       = from + bucketWidth;
            var label    = $"{from:F1}-{to:F1}";
            var textSize = ImGui.CalcTextSize(label);
            var labelX   = x + (barWidth - textSize.X) * 0.5f;
            drawList.AddText(new Vector2(labelX, origin.Y + 4f), ImGui.GetColorU32(ImGuiCol.Text), label);
        }

        // Titles
        drawList.AddText(new Vector2(cursor.X + leftMargin + chartWidth * 0.5f - 20f, origin.Y + 20f), ImGui.GetColorU32(ImGuiCol.Text),
            "Bite Time");
        drawList.AddText(ImGui.GetFont(), ImGui.GetFontSize(), new Vector2(cursor.X + 5f, cursor.Y), ImGui.GetColorU32(ImGuiCol.Text), "Count");

        // Legend
        var legendX = origin.X + chartWidth + 10f;
        var legendY = cursor.Y;
        foreach (var bait in baitIds)
        {
            var baitName = GatherBuddy.GameData.Bait.TryGetValue(bait, out var b) ? b.Name :
                GatherBuddy.GameData.Fishes.TryGetValue(bait, out var f)          ? new Bait(f.ItemData).Name : Bait.Unknown.Name;
            var label = $"{baitName}";
            drawList.AddRectFilled(new Vector2(legendX, legendY), new Vector2(legendX + textHeight, legendY + textHeight),
                ImGui.ColorConvertFloat4ToU32(baitColors[bait]));
            drawList.AddText(new Vector2(legendX + textHeight, legendY), ImGui.GetColorU32(ImGuiCol.Text), label);
            legendY += textHeight + 4f;
        }

        for (var k = 0; k < hatchColors.Count; k++)
        {
            var baitName =
                new List<string>
                {
                    "Chum",
                    "AmbitiousLure1",
                    "AmbitiousLure2",
                    "AmbitiousLure3",
                    "ModestLure1",
                    "ModestLure2",
                    "ModestLure3",
                }[k];
            var label = $"{baitName}";
            if (k == 0)
                DrawHatchedRectDown(new Vector2(legendX, legendY), new Vector2(legendX + textHeight, legendY + textHeight), 4f, drawList,
                    hatchColors[k]);
            else
                DrawHatchedRectUp(new Vector2(legendX, legendY), new Vector2(legendX + textHeight, legendY + textHeight), 4f, drawList,
                    hatchColors[k]);

            drawList.AddText(new Vector2(legendX + textHeight, legendY), ImGui.GetColorU32(ImGuiCol.Text), label);
            legendY += textHeight + 4f;
        }

        // Space
        ImGui.Dummy(new Vector2(leftMargin + chartWidth + 100f, chartHeight + bottomMargin + 20f));
    }

    private void DrawStatsPageTab()
    {
        if (!GatherBuddy.Config.EnableFishStats)
            return;

        _records = _plugin.FishRecorder.Records;
        using var id  = ImUtf8.PushId("Fishing Spots Stats"u8);
        using var tab = ImUtf8.TabItem("Fishing Spots Stats"u8);
        ImUtf8.HoverTooltip("Aggregator of Fish Record data in a presentable format"u8);

        if (!tab)
            return;

        using var child = ImUtf8.Child(""u8, Vector2.Zero);
        if (!child)
            return;

        DrawSelectorDropdown();
        DrawCopier();
        DrawFishingSpotInfo();
        if (GatherBuddy.Config.EnableFishStatsGraphs)
            DrawFishInfo();
    }

    private static void DrawHatchedRectUp(Vector2 from, Vector2 to, float thickness, ImDrawListPtr drawList, uint colorFlag)
    {
        var min = new Vector2(Math.Min(from.X, to.X), Math.Min(from.Y, to.Y));
        var max = new Vector2(Math.Max(from.X, to.X), Math.Max(from.Y, to.Y));

        drawList.AddRectFilled(min, max, 0x00000000);
        drawList.PushClipRect(min, max, true);

        var height        = max.Y - min.Y;
        var angle         = max.X - min.X;
        var block         = (int)((height + angle) / thickness);
        var halfThickness = thickness / 2;

        var start = new Vector2(min.X, min.Y + halfThickness);
        var end   = new Vector2(max.X, min.Y + halfThickness - angle);

        for (var i = 0; i < block; ++i)
        {
            drawList.AddQuadFilled(start,
                new Vector2(start.X, start.Y - halfThickness),
                end,
                new Vector2(end.X, end.Y + halfThickness),
                colorFlag);

            start.Y += thickness;
            end.Y   += thickness;
        }

        drawList.PopClipRect();
        drawList.AddRect(min, max, colorFlag);
    }

    private static void DrawHatchedRectDown(Vector2 from, Vector2 to, float thickness, ImDrawListPtr drawList, uint colorFlag)
    {
        var min = new Vector2(Math.Min(from.X, to.X), Math.Min(from.Y, to.Y));
        var max = new Vector2(Math.Max(from.X, to.X), Math.Max(from.Y, to.Y));

        drawList.AddRectFilled(min, max, 0x00000000);
        drawList.PushClipRect(min, max, true);

        var height        = max.Y - min.Y;
        var angle         = max.X - min.X;
        var block         = (int)((height + angle) / thickness);
        var halfThickness = thickness / 2;

        var start = new Vector2(min.X, min.Y + halfThickness - angle);
        var end   = new Vector2(max.X, min.Y + halfThickness);

        for (var i = -1; i < block; ++i)
        {
            drawList.AddQuadFilled(start,
                new Vector2(start.X, start.Y - halfThickness),
                end,
                new Vector2(end.X, end.Y + halfThickness),
                colorFlag);

            start.Y += thickness;
            end.Y   += thickness;
        }

        drawList.PopClipRect();
        drawList.AddRect(min, max, colorFlag);
    }
}
