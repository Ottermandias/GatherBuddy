using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Dalamud.Game;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Utility;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.FishTimer;
using GatherBuddy.FishTimer.OldRecords;
using GatherBuddy.Levenshtein;
using GatherBuddy.Plugin;
using GatherBuddy.Structs;
using GatherBuddy.Time;
using ImGuiNET;
using Lumina.Excel.Sheets;
using OtterGui;
using OtterGui.Text;
using OtterGui.Widgets;
using static GatherBuddy.FishTimer.FishRecord;
using FishingSpot = GatherBuddy.Classes.FishingSpot;
using Bait = GatherBuddy.Structs.Bait;
using ImRaii = OtterGui.Raii.ImRaii;


namespace GatherBuddy.Gui;

public partial class Interface
{
    private static readonly FrozenDictionary<uint, FishingSpot>  Locations = GatherBuddy.GameData.FishingSpots;
    private static readonly FrozenDictionary<uint,Bait>          Baits     = GatherBuddy.GameData.Bait;
    private                 List<FishRecord>                     _records;                
    private                 int                                  _selectedFishingSpotIdx = 0;
    private                 FishingSpot                          _selectedSpot           = Locations.First().Value;
    private                 ClippedSelectableCombo<FishingSpot>? _fishingSpotCombo;
    
    private void DrawSelectorDropdown()
    {
        if (_fishingSpotCombo == null)
        {
            var spots = Locations.Values.ToList();
            _fishingSpotCombo = new ClippedSelectableCombo<FishingSpot>(
                "FishingSpotSelector",
                "Fishing Spot",
                250,
                spots,
                s => s.Name.ToString()
            );
        }

        var spotsList = Locations.Values.ToList();

        if (_fishingSpotCombo.Draw(_selectedFishingSpotIdx, out var newIdx))
        {
            _selectedFishingSpotIdx = newIdx;
            _selectedSpot = spotsList[_selectedFishingSpotIdx];
        }
    }

    private void DrawFishingSpotInfo()
    {
        // if (_selectedSpot == null) return;

        // Draw Coordinates
        ImGui.Text($"Coordinates: ({_selectedSpot.IntegralXCoord}, {_selectedSpot.IntegralYCoord})");
        ImGui.Text($"Territory: {_selectedSpot.Territory.Name}");  // Assuming Territory has a Name property
        ImGui.Separator();

        // Draw Radius
        ImGui.Text($"Radius: {_selectedSpot.Radius} yards");

        // Draw Closest Aetheryte Info
        if (_selectedSpot.ClosestAetheryte != null)
        {
            ImGui.Text($"Closest Aetheryte: {_selectedSpot.ClosestAetheryte.Name}");
        }
        else
        {
            ImGui.Text("No Aetheryte found nearby.");
        }
        ImGui.Separator();

        // Draw Items (Fishes available in the spot)
        if (_selectedSpot.Items.Any())
        {
            ImGui.Text("Available Fish:");
            foreach (var fish in _selectedSpot.Items)
            {
                ImGui.Text("- " + fish.Name[GatherBuddy.Language]);
            }
        }
        else
        {
            ImGui.Text("No fish available.");
        }
    }
    
    private void DrawFishInfo()
    {
        var recordsAtSpot = _records
            .Where(r => r.HasSpot && r.SpotId == _selectedSpot.Id && r.HasCatch && r.HasBait)
            .ToList();

        // Group by FishId first
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

            ImGui.TextColored(new Vector4(0.4f, 0.8f, 1f, 1f), fish.Name[GatherBuddy.Language]);

            // Part 1: Show overall stats (all baits combined)
            ImGui.BulletText("All Baits Combined:");
            DrawSizeDistribution(allEntries);
            DrawBiteTimeHistogram(allEntries);

            ImGui.Spacing();

            // Part 2: Group by Bait
            var baitGroups = allEntries
                .GroupBy(r => r.BaitId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var (baitId, entries) in baitGroups)
            {
                var bait = GatherBuddy.GameData.Bait.GetValueOrDefault(baitId);
                ImGui.BulletText($"Bait: {bait?.Name ?? "Unknown"}");

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
        var         totalSpacing = chartWidth * 0.1f; // 10% of chart width for spacing
        var         barAreaWidth = chartWidth - totalSpacing;
        var         barWidth     = barAreaWidth / bucketCount;
        var         barSpacing   = totalSpacing / (bucketCount - 1); // spaces between bars
        const float chartHeight  = 150f;
        const float leftMargin   = 40f;
        const float bottomMargin = 30f;
        const int   yTicks       = 5;
        var         padding      = 5 * ImGuiHelpers.GlobalScale;

        // Determine the range of fish sizes
        var minSize     = entries.Min(r => r.Size / 10f);
        var maxSize     = entries.Max(r => r.Size / 10f);
        var sizeRange   = maxSize - minSize;
        var bucketWidth = sizeRange / bucketCount;

        // Initialize buckets
        var buckets = new (int normal, int large, int bgf)[bucketCount];
        foreach (var entry in entries)
        {
            var bucketIndex = (int)((entry.Size / 10f - minSize) / bucketWidth);
            if (bucketIndex >= bucketCount) 
                bucketIndex = bucketCount - 1; // Edge case for maxSize

            ref var b = ref buckets[bucketIndex];
            if (entry.Flags.HasFlag(Effects.BigGameFishing))
                b.bgf++;
            else if (entry.Flags.HasFlag(Effects.Large))
                b.large++;
            else
                b.normal++;
        }

        // Get max count per bucket for Y-axis scaling
        var maxCount = buckets.Max(b => b.normal + b.large + b.bgf);

        // Setup drawing
        var drawList   = ImGui.GetWindowDrawList();
        var cursor     = ImGui.GetCursorScreenPos();
        var textHeight = ImGui.CalcTextSize("X").Y;
        var origin     = new Vector2(cursor.X + leftMargin, cursor.Y + chartHeight + textHeight * (float)1.5);

        // Y-axis lines & labels
        for (int i = 0; i <= yTicks; i++)
        {
            var yVal = maxCount * i / (float)yTicks;
            var y  = origin.Y - (yVal/maxCount * chartHeight);

            drawList.AddLine(new Vector2(origin.X, y), new Vector2(origin.X + chartWidth, y), ImGui.GetColorU32(ImGuiCol.Border));
            drawList.AddText(new Vector2(origin.X - (ImGui.CalcTextSize($"{yVal}").X + padding), y - textHeight/2), ImGui.GetColorU32(ImGuiCol.Text), $"{yVal}");
        }
        
        // Histogram bars
        for (int i = 0; i < bucketCount; i++)
        {
            var x = origin.X + i * (barWidth + barSpacing);
            var y = origin.Y;

            var (normal, large, bgf) = buckets[i];

            DrawStack(ref y, normal, new Vector4(0.5f, 0.5f, 0.5f, 1f)); // Gray
            DrawStack(ref y, large,  new Vector4(1f, 1f, 0.3f, 1f));     // Yellow
            DrawStack(ref y, bgf,    new Vector4(0.4f, 0.6f, 1f, 1f));   // Blue

            // X-axis labels (bucket ranges)
            var fromVal  = minSize + (i * bucketWidth);
            var fromText = $"{fromVal:F1}";
            var toText   = $"{fromVal + bucketWidth:F1}";
            var label    = $"{fromText}-{toText}";
            var textSize = ImGui.CalcTextSize(label);
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

        // X and Y axis titles
        drawList.AddText(new Vector2(cursor.X + leftMargin + chartWidth * 0.5f - 20f, origin.Y + 20f), ImGui.GetColorU32(ImGuiCol.Text), "Size");
        drawList.AddText(ImGui.GetFont(), ImGui.GetFontSize(), new Vector2(cursor.X + 5f, cursor.Y), ImGui.GetColorU32(ImGuiCol.Text), "Count");

        // Reserve space for the chart
        ImGui.Dummy(new Vector2(leftMargin + chartWidth, chartHeight + bottomMargin + 20f));
    }

    void DrawAmountCaught(int count)
    {
        ImGui.Text($"Caught: {count} times");
    }

    void DrawCatchPercentage(List<FishRecord> entries, int baitTotal)
    {
        float percentage = (entries.Count / (float)baitTotal) * 100f;
        ImGui.Text($"Percent of baited catches: {percentage:F2}");
    }

    // void DrawBiteTimeDistribution(List<FishRecord> entries)
    // {
    //     var bites = entries.Select(r => (float)r.Bite).ToArray();
    //     if (bites.Length > 0)
    //         ImGui.PlotHistogram("Bite Time (ms)", ref bites[0], bites.Length, 0, null, bites.Min(), bites.Max(), new Vector2(ImGui.GetWindowSize().X / 2, 150));
    // }
    
    private static void DrawBiteTimeHistogram(List<FishRecord> entries)
    {
        if (entries.Count == 0)
            return;

        var windowSize = new Vector2(ImGui.GetWindowSize().X / 2, ImGui.GetWindowSize().Y);
        var chartWidth = windowSize.X;
        var bucketCount = Math.Min(10, entries.Count);
        var totalSpacing = chartWidth * 0.1f;
        var barAreaWidth = chartWidth - totalSpacing;
        var barWidth = barAreaWidth / bucketCount;
        var barSpacing = totalSpacing / (bucketCount - 1);
        const float chartHeight = 150f;
        const float leftMargin = 40f;
        const float bottomMargin = 30f;
        const int yTicks = 5;
        var padding = 5 * ImGuiHelpers.GlobalScale;

        // Build baitID set and assign colors
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
        for (int i = 0; i < baitIds.Count; i++)
            baitColors[baitIds[i]] = colorPalette[i % colorPalette.Length];

        // Flag hatch colors
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

        // Bucket bite times
        float minTime     = entries.Min(e => e.Bite / 1000f);
        float maxTime     = entries.Max(e => e.Bite / 1000f);
        float timeRange   = maxTime - minTime;
        float bucketWidth = timeRange / bucketCount;

        var buckets = new List<Dictionary<uint, List<FishRecord>>>();
        for (int i = 0; i < bucketCount; i++)
        {
            buckets.Add(new Dictionary<uint, List<FishRecord>>());
        }
        foreach (var e in entries)
        {
            int bucketIndex = (int)((e.Bite / 1000f - minTime) / bucketWidth);

            // Clamp bucketIndex to valid range
            if (bucketIndex < 0)
                bucketIndex = 0;
            else if (bucketIndex >= bucketCount)
                bucketIndex = bucketCount - 1;

            var bucket = buckets[bucketIndex];

            // Ensure the bait key exists
            if (!bucket.TryGetValue(e.BaitId, out var list))
            {
                list             = new List<FishRecord>();
                bucket[e.BaitId] = list;
            }

            list.Add(e);
        }

        // Max bucket count for Y-axis scaling
        int maxCount = buckets
            .Where(dict => dict.Values.Any()) // filter out empty dictionaries
            .Select(dict => dict.Values.Sum(e => e.Count))
            .DefaultIfEmpty(0) // fallback if everything is empty
            .Max();

        // Begin drawing
        var drawList = ImGui.GetWindowDrawList();
        var cursor = ImGui.GetCursorScreenPos();
        var textHeight = ImGui.CalcTextSize("X").Y;
        var origin = new Vector2(cursor.X + leftMargin, cursor.Y + chartHeight + textHeight * 1.5f);

        // Y-axis
        for (int i = 0; i <= yTicks; i++)
        {
            float yVal = maxCount * i / (float)yTicks;
            float y = origin.Y - (yVal / maxCount * chartHeight);

            drawList.AddLine(new Vector2(origin.X, y), new Vector2(origin.X + chartWidth, y), ImGui.GetColorU32(ImGuiCol.Border));
            string label = $"{yVal:0}";
            drawList.AddText(new Vector2(origin.X - (ImGui.CalcTextSize(label).X + padding), y - textHeight / 2), ImGui.GetColorU32(ImGuiCol.Text), label);
        }

        // Histogram bars
        for (int i = 0; i < bucketCount; i++)
        {
            float x = origin.X + i * (barWidth + barSpacing);
            float y = origin.Y;
            foreach (var bait in baitIds)
            {
                if(!buckets[i].ContainsKey(bait))
                    continue;

                if (!buckets[i].TryGetValue(bait, out List<FishRecord> lRecords) || lRecords.Count == 0)
                    continue;

                var   count     = lRecords.Count;
                float height    = count / (float)maxCount * chartHeight;
                var   baitColor = ImGui.ColorConvertFloat4ToU32(baitColors[bait]);
                var   barTop    = new Vector2(x + barWidth,              y);
                var counter = new List<int>{
                    lRecords.Count(e => !e.Flags.HasFlag(Effects.Chum) &&
                        !e.Flags.HasFlag(Effects.AmbitiousLure1) &&
                        !e.Flags.HasFlag(Effects.AmbitiousLure2) &&
                        !e.Flags.HasFlag(Effects.ModestLure1) &&
                        !e.Flags.HasFlag(Effects.ModestLure2)),
                    lRecords.Count(e => e.Flags.HasFlag(Effects.Chum)),                                                       // Chum
                    lRecords.Count(e => e.Flags.HasFlag(Effects.AmbitiousLure1) && !e.Flags.HasFlag(Effects.AmbitiousLure2)), // Alure1
                    lRecords.Count(e => !e.Flags.HasFlag(Effects.AmbitiousLure1) && e.Flags.HasFlag(Effects.AmbitiousLure2)), // Alure2
                    lRecords.Count(e => e.Flags.HasFlag(Effects.AmbitiousLure1) && e.Flags.HasFlag(Effects.AmbitiousLure2)),  // Alure3
                    lRecords.Count(e => e.Flags.HasFlag(Effects.ModestLure1) && !e.Flags.HasFlag(Effects.ModestLure2)),        // Mlure1
                    lRecords.Count(e => !e.Flags.HasFlag(Effects.ModestLure1) && e.Flags.HasFlag(Effects.ModestLure2)),        // Mlure2
                    lRecords.Count(e => e.Flags.HasFlag(Effects.ModestLure1) && e.Flags.HasFlag(Effects.ModestLure2)),        // Mlure3
                };
                
                for(var j = 0; j < counter.Count; j++)
                {
                    var c = counter[j];
                    if (c <= 0)
                        continue;
                    var barBot = new Vector2(x, barTop.Y);
                    barTop.Y -= c / (float)maxCount * chartHeight;
                    drawList.AddRectFilled(barBot, barTop, baitColor);
                    if (j != 0)
                        DrawHatchedRect(barBot, barTop, 10f, drawList, baitColor, hatchColors[j-1]);
                    var heightTextSize = ImGui.CalcTextSize($"{c}");
                    drawList.AddText(new Vector2(x + barWidth/2 - heightTextSize.X/2, barBot.Y - (c / (float)maxCount * chartHeight)/2 - heightTextSize.Y/2),ImGui.GetColorU32(ImGuiCol.Text),$"{c}");
                }
                
                y -= height;
            }

            // X-axis labels
            float from = minTime + i * bucketWidth;
            float to = from + bucketWidth;
            string label = $"{from:F1}-{to:F1}";
            var textSize = ImGui.CalcTextSize(label);
            float labelX = x + (barWidth - textSize.X) * 0.5f;
            drawList.AddText(new Vector2(labelX, origin.Y + 4f), ImGui.GetColorU32(ImGuiCol.Text), label);
        }

        // Axis titles
        drawList.AddText(new Vector2(cursor.X + leftMargin + chartWidth * 0.5f - 20f, origin.Y + 20f), ImGui.GetColorU32(ImGuiCol.Text), "Bite Time");
        drawList.AddText(ImGui.GetFont(), ImGui.GetFontSize(), new Vector2(cursor.X + 5f, cursor.Y), ImGui.GetColorU32(ImGuiCol.Text), "Count");

        // Legend
        float legendX = origin.X + chartWidth + 10f;
        float legendY = cursor.Y;
        foreach (var bait in baitIds)
        {
            var baitName = GatherBuddy.GameData.Bait.TryGetValue(bait, out var b) ? b.Name : 
                GatherBuddy.GameData.Fishes.TryGetValue(bait, out var f)          ? new Bait(f.ItemData).Name : Bait.Unknown.Name;
            var label    = $"{baitName}";
            drawList.AddRectFilled(new Vector2(legendX, legendY), new Vector2(legendX + textHeight, legendY + textHeight), ImGui.ColorConvertFloat4ToU32(baitColors[bait]));
            drawList.AddText(new Vector2(legendX + textHeight, legendY), ImGui.GetColorU32(ImGuiCol.Text), label);
            legendY += textHeight + 4f;
        }
        for(int k = 0; k < hatchColors.Count; k++)
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
            var label    = $"{baitName}";
            DrawHatchedRect(new Vector2(legendX, legendY), new Vector2(legendX + textHeight, legendY + textHeight), 4f, drawList, 0, hatchColors[k]);
            drawList.AddText(new Vector2(legendX + textHeight, legendY), ImGui.GetColorU32(ImGuiCol.Text), label);
            legendY += textHeight + 4f;
        }

        // Reserve space
        ImGui.Dummy(new Vector2(leftMargin + chartWidth + 100f, chartHeight + bottomMargin + 20f));
        
    }
    
    private void DrawStatsPageTab()
    {
        _records = _plugin.FishRecorder.Records;
        using var id  = ImRaii.PushId("Fishing Spots Stats");
        using var tab = ImRaii.TabItem("Fishing Spots Stats");
        ImGuiUtil.HoverTooltip("Aggregator of Fish Record data in a presentable format");
        
        if(!tab)
            return;

        using var child = ImRaii.Child(string.Empty);
        if (!child)
            return;

        DrawSelectorDropdown();
        DrawFishingSpotInfo();
        DrawFishInfo();
    }
    
    private static void DrawHatchedRect(Vector2 from, Vector2 to, float thickness, ImDrawListPtr drawList, uint colorBait, uint colorFlag)
    {
        // Ensure proper top-left to bottom-right ordering
        Vector2 min = new Vector2(Math.Min(from.X, to.X), Math.Min(from.Y, to.Y));
        Vector2 max = new Vector2(Math.Max(from.X, to.X), Math.Max(from.Y, to.Y));

        drawList.AddRectFilled(min, max, colorBait);
        drawList.PushClipRect(min, max, true); // 'true' ensures the clip rect is respected even if it's zero-sized

        float height        = max.Y - min.Y;
        float angle         = max.X - min.X;
        int   block         = (int)((height + angle) / thickness);
        float halfThickness = thickness / 2;

        var start = new Vector2(min.X, min.Y + halfThickness);
        var end   = new Vector2(max.X, min.Y + halfThickness - angle);

        for (int i = 0; i < block; ++i)
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
