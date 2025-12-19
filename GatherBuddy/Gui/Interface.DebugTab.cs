using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Dalamud.Bindings.ImGui;
using Dalamud.Game;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.FishTimer;
using GatherBuddy.Levenshtein;
using GatherBuddy.Plugin;
using GatherBuddy.Structs;
using GatherBuddy.Time;
using Lumina.Excel.Sheets;
using OtterGui;
using OtterGui.Text;
using static GatherBuddy.FishTimer.FishRecord;
using Aetheryte = GatherBuddy.Classes.Aetheryte;
using FishingSpot = GatherBuddy.Classes.FishingSpot;
using ImGuiTable = OtterGui.ImGuiTable;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface
{
    [GeneratedRegex(@"(?<Name>.*) \((?<Id>\d{5})\)$", RegexOptions.ExplicitCapture | RegexOptions.NonBacktracking)]
    private static partial Regex CosmicMissionRegex();

    private static uint _startId = 10031;
    private static uint _endId   = 10096;

    private static void DrawDebugAetheryte(Aetheryte a)
    {
        ImGuiUtil.DrawTableColumn(a.Id.ToString());
        ImGuiUtil.DrawTableColumn(a.Name);
        ImGuiUtil.DrawTableColumn(a.Territory.Name);
        ImGuiUtil.DrawTableColumn($"{a.XCoord}-{a.YCoord}");
        ImGuiUtil.DrawTableColumn($"{a.XStream}-{a.YStream}-{a.Plane}");
    }

    private static void DrawDebugTerritory(Territory t)
    {
        ImGuiUtil.DrawTableColumn(t.Id.ToString());
        ImGuiUtil.DrawTableColumn(t.Name);
        ImGuiUtil.DrawTableColumn(t.SizeFactor.ToString(CultureInfo.InvariantCulture));
        ImGuiUtil.DrawTableColumn(t.WeatherRates.Rates.Length.ToString());
        ImGuiUtil.DrawTableColumn(string.Join(", ", t.WeatherRates.Rates.Select(r => $"{r.Weather.Name} ({r.Weather.Id})")));
    }

    private static void DrawDebugBait(Bait b)
    {
        ImGuiUtil.DrawTableColumn(b.Id.ToString());
        ImGuiUtil.DrawTableColumn(b.Name);
    }

    private static void DrawGatherableDebug(Gatherable g)
    {
        ImGuiUtil.DrawTableColumn(g.ItemId.ToString());
        ImGuiUtil.DrawTableColumn(g.GatheringId.ToString());
        ImGuiUtil.DrawTableColumn(g.Name.English);
        ImGuiUtil.DrawTableColumn(g.LevelString());
        ImGuiUtil.DrawTableColumn(g.NodeList.Count.ToString());
    }

    private static void DrawGatheringNodeDebug(GatheringNode n)
    {
        ImGuiUtil.DrawTableColumn(n.Id.ToString());
        ImGuiUtil.DrawTableColumn(n.Name);
        ImGuiUtil.DrawTableColumn(n.GatheringType.ToString());
        ImGuiUtil.DrawTableColumn(n.Level.ToString());
        ImGuiUtil.DrawTableColumn(n.NodeType.ToString());
        ImGuiUtil.DrawTableColumn($"{n.Territory.Name} ({n.Territory.Id})");
        ImGuiUtil.DrawTableColumn($"{n.IntegralXCoord}-{n.IntegralYCoord}");
        ImGuiUtil.DrawTableColumn(n.ClosestAetheryte?.Name ?? "Unknown");
        ImGuiUtil.DrawTableColumn(n.Folklore);
        ImGuiUtil.DrawTableColumn(n.Times.PrintHours(true));
        ImGuiUtil.DrawTableColumn(n.PrintItems());
    }

    private static void DrawFishDebug(Fish f)
    {
        ImGuiUtil.DrawTableColumn(f.ItemId.ToString());
        ImGuiUtil.DrawTableColumn($"{f.FishId}{(f.IsSpearFish ? " (sf)" : "")}");
        ImGuiUtil.DrawTableColumn(f.Name.English);
        ImGuiUtil.DrawTableColumn(f.FishRestrictions.ToString());
        ImGuiUtil.DrawTableColumn(f.Folklore);
        ImGuiUtil.DrawTableColumn(f.InLog.ToString());
        ImGuiUtil.DrawTableColumn(f.IsBigFish.ToString());
        ImGuiUtil.DrawTableColumn(string.Join('|', f.FishingSpots.Select(s => s.Name)));
    }

    private static void DrawFishingSpotDebug(FishingSpot s)
    {
        ImGuiUtil.DrawTableColumn($"{s.Id}{(s.Spearfishing ? " (sf)" : "")}");
        ImGuiUtil.DrawTableColumn(s.Name);
        ImGuiUtil.DrawTableColumn($"{s.Territory.Name} ({s.Territory.Id})");
        ImGuiUtil.DrawTableColumn(s.ClosestAetheryte?.Name ?? "Unknown");
        ImGuiUtil.DrawTableColumn($"{s.IntegralXCoord / 100f:00.00}-{s.IntegralYCoord / 100f:00.00}");
        ImGuiUtil.DrawTableColumn($"{s.SpearfishingSpotData?.IsShadowNode ?? false}");
        ImGuiUtil.DrawTableColumn(string.Join('|', s.Items.Select(fish => fish.Name)));
    }

    private static void PrintNode<T>(PatriciaTrie<T>.Node node)
    {
        var name = node.TotalWord.ToString();
        if (name.Length == 0)
            name = "Root";
        if (node.Children.Count == 0)
        {
            ImGui.Text(name);
        }
        else
        {
            if (!ImGui.TreeNodeEx(name))
                return;

            foreach (var child in node.Children)
                PrintNode(child);
            ImGui.TreePop();
        }
    }

    private void DrawDebugButtons()
    {
        if (ImGui.CollapsingHeader("Debug"))
        {
            if (ImGui.Button("Set Weather Dirty"))
                _weatherTable.SetDirty();
            if (ImGui.Button("Set Locations Dirty"))
                GatherBuddy.UptimeManager.ResetLocations();

            if (FishTimerWindow.CollectableIcon.TryGetWrap(out var wrapCollectable, out _))
                ImGui.Image(wrapCollectable.Handle, wrapCollectable.Size);

            ImGui.SameLine();
            if (FishTimerWindow.DoubleHookIcon.TryGetWrap(out var wrapDoubleHook, out _))
                ImGui.Image(wrapDoubleHook.Handle, wrapDoubleHook.Size);

            ImGui.SameLine();
            if (FishTimerWindow.TripleHookIcon.TryGetWrap(out var wrapTripleHook, out _))
                ImGui.Image(wrapTripleHook.Handle, wrapTripleHook.Size);

            ImGui.SameLine();
            if (FishTimerWindow.QuadHookIcon.TryGetWrap(out var wrapQuadHook, out _))
                ImGui.Image(wrapQuadHook.Handle, wrapQuadHook.Size);
        }
    }

    private static unsafe void DrawDebugTime()
    {
        if (!ImGui.CollapsingHeader("Time"))
            return;

        using var table = ImRaii.Table("##Times", 2);
        if (!table)
            return;

        var fw = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance();
        ImGuiUtil.DrawTableColumn("Framework Timestamp");
        ImGuiUtil.DrawTableColumn(fw == null ? "NULL" : fw->UtcTime.Timestamp.ToString());
        ImGuiUtil.DrawTableColumn("Framework Eorzea");
        ImGuiUtil.DrawTableColumn(fw == null ? "NULL" : fw->ClientTime.EorzeaTime.ToString());
        ImGuiUtil.DrawTableColumn("Framework Func");
        ImGuiUtil.DrawTableColumn(FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.GetServerTime().ToString());
        ImGuiUtil.DrawTableColumn("DateTimeOffset");
        ImGuiUtil.DrawTableColumn(DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
        ImGuiUtil.DrawTableColumn("GatherBuddy TimeStamp");
        ImGuiUtil.DrawTableColumn(GatherBuddy.Time.ServerTime.Time.ToString());
        ImGuiUtil.DrawTableColumn("GatherBuddy EorzeaTime");
        ImGuiUtil.DrawTableColumn(GatherBuddy.Time.EorzeaTime.Time.ToString());
        ImGuiUtil.DrawTableColumn("Current Computed Weather");
        ImGuiUtil.DrawTableColumn(Dalamud.ClientState.TerritoryType != 0
            ? GatherBuddy.WeatherManager.FindLastCurrentNextWeather(Dalamud.ClientState.TerritoryType).Current.Name
            : "None");
        ImGuiUtil.DrawTableColumn("Current True Weather");
        ImGuiUtil.DrawTableColumn(Dalamud.ClientState.TerritoryType != 0
         && GatherBuddy.GameData.Weathers.TryGetValue(GatherBuddy.CurrentWeather.Current, out var w)
                ? w.Name
                : "None");
    }

    private static unsafe void DrawDebugFishingState()
    {
        if (!ImGui.CollapsingHeader("Fishing State"))
            return;

        using var table = ImRaii.Table("##Framework", 2);
        if (!table)
            return;

        ImGuiUtil.DrawTableColumn("Current Save Changes");
        ImGuiUtil.DrawTableColumn(_plugin.FishRecorder.Changes.ToString());
        ImGuiUtil.DrawTableColumn("Next Timed Save");
        ImGuiUtil.DrawTableColumn(_plugin.FishRecorder.SaveTime == TimeStamp.MaxValue
            ? "Never"
            : TimeInterval.DurationString(_plugin.FishRecorder.SaveTime, TimeStamp.UtcNow, false));
        ImGuiUtil.DrawTableColumn("UiState Address");
        ImGui.TableNextColumn();
        GatherBuddy.Dynamis.DrawPointer(FFXIVClientStructs.FFXIV.Client.Game.UI.UIState.Instance());
        ImGuiUtil.DrawTableColumn("Event Framework Address");
        ImGui.TableNextColumn();
        GatherBuddy.Dynamis.DrawPointer(GatherBuddy.EventFramework.Address);
        ImGuiUtil.DrawTableColumn("Fishing Manager Address");
        ImGui.TableNextColumn();
        GatherBuddy.Dynamis.DrawPointer(GatherBuddy.EventFramework.FishingManager);
        ImGuiUtil.DrawTableColumn("Fishing State");
        ImGuiUtil.DrawTableColumn(GatherBuddy.EventFramework.FishingState.ToString());
        ImGuiUtil.DrawTableColumn("Num SwimBait");
        ImGuiUtil.DrawTableColumn(GatherBuddy.EventFramework.NumSwimBait.ToString());
        ImGuiUtil.DrawTableColumn("Selected SwimBait");
        ImGuiUtil.DrawTableColumn(GatherBuddy.EventFramework.CurrentSwimBait?.ToString() ?? "NULL");
        ImGuiUtil.DrawTableColumn("SwimBait 1");
        ImGuiUtil.DrawTableColumn(GatherBuddy.EventFramework.SwimBait(0)?.ToString() ?? "NULL");
        ImGuiUtil.DrawTableColumn("SwimBait 2");
        ImGuiUtil.DrawTableColumn(GatherBuddy.EventFramework.SwimBait(1)?.ToString() ?? "NULL");
        ImGuiUtil.DrawTableColumn("SwimBait 3");
        ImGuiUtil.DrawTableColumn(GatherBuddy.EventFramework.SwimBait(2)?.ToString() ?? "NULL");
        ImGuiUtil.DrawTableColumn("Bite Type Address");
        ImGuiUtil.DrawTableColumn(GatherBuddy.TugType.Address.ToString("X"));
        ImGuiUtil.DrawTableColumn("Bite Type");
        ImGuiUtil.DrawTableColumn(GatherBuddy.TugType.Bite.ToString());

        var record = _plugin.FishRecorder.Record;
        ImGuiUtil.DrawTableColumn("Last Fishing State");
        ImGuiUtil.DrawTableColumn(_plugin.FishRecorder.LastState.ToString());
        ImGuiUtil.DrawTableColumn("Current Step");
        ImGuiUtil.DrawTableColumn(_plugin.FishRecorder.Step.ToString());
        ImGuiUtil.DrawTableColumn("ContentIdHash");
        ImGuiUtil.DrawTableColumn(record.ContentIdHash.ToString());
        ImGuiUtil.DrawTableColumn("Gathering");
        ImGuiUtil.DrawTableColumn(record.Gathering.ToString());
        ImGuiUtil.DrawTableColumn("Perception");
        ImGuiUtil.DrawTableColumn(record.Perception.ToString());
        ImGuiUtil.DrawTableColumn("Start Time");
        ImGuiUtil.DrawTableColumn((record.TimeStamp / 1000).ToString());
        ImGuiUtil.DrawTableColumn("Current Spot");
        ImGuiUtil.DrawTableColumn($"{record.FishingSpot?.Name ?? "Unknown"} ({record.FishingSpot?.Id ?? 0})");
        if (CosmicMissionRegex().Match(record.FishingSpot?.Name ?? string.Empty).Groups["Id"] is { Success: true, Value: { } mission })
        {
            var id = uint.Parse(mission);
            if (Dalamud.GameData.GetExcelSheet<WKSMissionUnit>().TryGetRow(id, out var row))
            {
                ImGuiUtil.DrawTableColumn("Current Mission");
                ImGuiUtil.DrawTableColumn($"{row.Name.ExtractText()} ({id})");
            }
        }

        ImGuiUtil.DrawTableColumn("Selected Bait");
        var baitId = GatherBuddy.CurrentBait.Current;
        ImGuiUtil.DrawTableColumn($"{GatherBuddy.GameData.Bait.GetValueOrDefault(baitId, Bait.Unknown).Name} ({baitId})");
        ImGuiUtil.DrawTableColumn("Current Bait");
        ImGuiUtil.DrawTableColumn($"{record.Bait.Name} ({record.Bait.Id})");
        ImGuiUtil.DrawTableColumn("Duration");
        ImGuiUtil.DrawTableColumn(_plugin.FishRecorder.Timer.ElapsedMilliseconds.ToString());
        ImGuiUtil.DrawTableColumn("BiteType");
        ImGuiUtil.DrawTableColumn(record.Tug.ToString());
        ImGuiUtil.DrawTableColumn("HookSet");
        ImGuiUtil.DrawTableColumn(record.Hook.ToString());
        ImGuiUtil.DrawTableColumn("Last Catch");
        ImGuiUtil.DrawTableColumn(
            $"{_plugin.FishRecorder.LastCatch?.Name[ClientLanguage.English] ?? "None"} ({_plugin.FishRecorder.LastCatch?.ItemId ?? 0} - {_plugin.FishRecorder.LastCatch?.FishId ?? 0})");
        ImGuiUtil.DrawTableColumn("Current Catch");
        ImGuiUtil.DrawTableColumn(
            $"{record.Catch?.Name[ClientLanguage.English] ?? "None"} ({record.Catch?.ItemId ?? 0} - {record.Catch?.FishId ?? 0}) - of size {record.Size / 10f} times {record.Amount}");
        foreach (var flag in Enum.GetValues<Effects>())
        {
            ImGuiUtil.DrawTableColumn(flag.ToString());
            ImGuiUtil.DrawTableColumn(record.Flags.HasFlag(flag).ToString());
        }
    }

    private unsafe void DrawDebugFishingTimes()
    {
        if (!ImGui.CollapsingHeader("Fishing Times"))
            return;

        using var table = ImRaii.Table("##Fishing Times", 6);
        if (!table)
            return;

        foreach (var (fishId, data) in _plugin.FishRecorder.Times)
        {
            ImGuiUtil.DrawTableColumn(GatherBuddy.GameData.Fishes[fishId].Name[ClientLanguage.English]);
            ImGuiUtil.DrawTableColumn("Overall");
            ImGuiUtil.DrawTableColumn(data.All.Min.ToString());
            ImGuiUtil.DrawTableColumn(data.All.Max.ToString());
            ImGuiUtil.DrawTableColumn(data.All.MinChum.ToString());
            ImGuiUtil.DrawTableColumn(data.All.MaxChum.ToString());
            foreach (var (baitId, times) in data.Data)
            {
                var bait = GatherBuddy.GameData.Bait.TryGetValue(baitId, out var b)
                    ? b
                    : new Bait(GatherBuddy.GameData.Fishes[fishId].ItemData);
                ImGui.TableNextColumn();
                ImGuiUtil.DrawTableColumn(bait.Name);
                ImGuiUtil.DrawTableColumn(times.Min.ToString());
                ImGuiUtil.DrawTableColumn(times.Max.ToString());
                ImGuiUtil.DrawTableColumn(times.MinChum.ToString());
                ImGuiUtil.DrawTableColumn(times.MaxChum.ToString());
            }
        }
    }

    private static void DrawUptimeManagerTable()
    {
        if (!ImGui.CollapsingHeader($"Uptimes ({GatherBuddy.GameData.TimedGatherables})"))
            return;

        using var table = ImRaii.Table("##Uptimes", 6, ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit);
        if (!table)
            return;

        foreach (var item in GatherBuddy.GameData.Gatherables.Values)
        {
            if (item.InternalLocationId == 0)
                continue;

            ImGuiUtil.DrawTableColumn(Math.Abs(item.InternalLocationId).ToString("0000"));
            ImGuiUtil.DrawTableColumn(item.Name[ClientLanguage.English]);
            ImGuiUtil.DrawTableColumn(item.NodeList.Count.ToString());
            var (loc, time) = GatherBuddy.UptimeManager.BestLocation(item);
            ImGuiUtil.DrawTableColumn(loc.Name);
            if (item.InternalLocationId > 0)
            {
                if (time == TimeInterval.Invalid)
                {
                    ImGuiUtil.DrawTableColumn("Invalid");
                    ImGuiUtil.DrawTableColumn(string.Empty);
                }
                else if (time == TimeInterval.Never)
                {
                    ImGuiUtil.DrawTableColumn("Never");
                    ImGuiUtil.DrawTableColumn(string.Empty);
                }
                else
                {
                    ImGuiUtil.DrawTableColumn(time.Start.ToString());
                    ImGuiUtil.DrawTableColumn(time.End.ToString());
                }
            }
            else
            {
                ImGuiUtil.DrawTableColumn("Always");
                ImGui.TableNextColumn();
            }
        }

        foreach (var fish in GatherBuddy.GameData.Fishes.Values)
        {
            if (fish.InternalLocationId == 0)
                continue;

            ImGuiUtil.DrawTableColumn(Math.Abs(fish.InternalLocationId).ToString("0000"));
            ImGuiUtil.DrawTableColumn(fish.Name[ClientLanguage.English]);
            ImGuiUtil.DrawTableColumn(fish.FishingSpots.Count.ToString());
            var (loc, time) = GatherBuddy.UptimeManager.BestLocation(fish);
            ImGuiUtil.DrawTableColumn(loc.Name);
            if (fish.InternalLocationId > 0)
            {
                if (time == TimeInterval.Invalid)
                {
                    ImGuiUtil.DrawTableColumn("Invalid");
                    ImGuiUtil.DrawTableColumn(string.Empty);
                }
                else if (time == TimeInterval.Never)
                {
                    ImGuiUtil.DrawTableColumn("Never");
                    ImGuiUtil.DrawTableColumn(string.Empty);
                }
                else
                {
                    ImGuiUtil.DrawTableColumn(time.Start.ToString());
                    ImGuiUtil.DrawTableColumn(time.End.ToString());
                }
            }
            else
            {
                ImGuiUtil.DrawTableColumn("Always");
                ImGui.TableNextColumn();
            }
        }
    }

    private void DrawAlarmDebug()
    {
        if (!ImGui.CollapsingHeader("Alarms##AlarmDebug"))
            return;

        using var table = ImRaii.Table("##Alarms", 2, ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit);
        if (!table)
            return;

        var nextAlarm = _plugin.AlarmManager.ActiveAlarms.Count > 0 ? _plugin.AlarmManager.ActiveAlarms[0].Item2 : TimeStamp.Epoch;
        var (abs, rel) = nextAlarm != TimeStamp.Epoch
            ? (nextAlarm.LocalTime.ToString(CultureInfo.InvariantCulture),
                TimeInterval.DurationString(nextAlarm, GatherBuddy.Time.ServerTime, false))
            : ("Never", "Never");

        ImGuiUtil.DrawTableColumn("Enabled");
        ImGuiUtil.DrawTableColumn(GatherBuddy.Config.AlarmsEnabled.ToString());
        ImGuiUtil.DrawTableColumn("Dirty");
        ImGuiUtil.DrawTableColumn(_plugin.AlarmManager.Dirty.ToString());
        ImGuiUtil.DrawTableColumn("Next Change (Absolute)");
        ImGuiUtil.DrawTableColumn(abs);
        ImGuiUtil.DrawTableColumn("Next Change (Relative)");
        ImGuiUtil.DrawTableColumn(rel);
        ImGuiUtil.DrawTableColumn("#Alarm Groups");
        ImGuiUtil.DrawTableColumn(_plugin.AlarmManager.Alarms.Count.ToString());
        ImGuiUtil.DrawTableColumn("#Enabled Alarms");
        ImGuiUtil.DrawTableColumn(_plugin.AlarmManager.ActiveAlarms.Count.ToString());
        foreach (var (alarm, state) in _plugin.AlarmManager.ActiveAlarms)
        {
            ImGuiUtil.DrawTableColumn(alarm.Name.Any() ? alarm.Name : alarm.Item.Name[ClientLanguage.English]);
            ImGuiUtil.DrawTableColumn($"{state} ({TimeInterval.DurationString(state, GatherBuddy.Time.ServerTime, false)})");
        }
    }

    private string _identifyTest       = string.Empty;
    private uint   _lastItemIdentified = 0;

    private void DrawWaymarkTab()
    {
        if (!ImGui.CollapsingHeader("Waymarks##WaymarkDebug"))
            return;

        ImGui.TextUnformatted($"Waymark Manager: 0x{GatherBuddy.WaymarkManager.Address:X}");
        ImGui.TextUnformatted(
            $"Waymark Manager Offset: +0x{(ulong)GatherBuddy.WaymarkManager.Address - (ulong)Dalamud.SigScanner.Module.BaseAddress:X}");
        using var table = ImRaii.Table("##Waymarks", 9, ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit);
        if (!table)
            return;

        for (var i = 0; i < GatherBuddy.WaymarkManager.Count; ++i)
        {
            using var id      = ImRaii.PushId(i);
            var       waymark = GatherBuddy.WaymarkManager[i];
            ImGui.TableNextColumn();
            if (ImGui.Button("Clear"))
                GatherBuddy.WaymarkManager.ClearWaymark(i);
            ImGui.TableNextColumn();
            if (ImGui.Button("Set"))
                GatherBuddy.WaymarkManager.SetWaymark(i);
            ImGuiUtil.DrawTableColumn(waymark.Active.ToString());
            ImGuiUtil.DrawTableColumn(waymark.Position.X.ToString());
            ImGuiUtil.DrawTableColumn(waymark.Position.Y.ToString());
            ImGuiUtil.DrawTableColumn(waymark.Position.Z.ToString());
            ImGuiUtil.DrawTableColumn(waymark.X.ToString());
            ImGuiUtil.DrawTableColumn(waymark.Y.ToString());
            ImGuiUtil.DrawTableColumn(waymark.Z.ToString());
        }
    }

    private static void DrawOceanTab()
    {
        if (!ImGui.CollapsingHeader("Ocean Routes##OceanDebug"))
            return;

        using (var table = ImRaii.Table("##Ocean", 8, ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit))
        {
            if (table)
                foreach (var route in GatherBuddy.GameData.OceanRoutes)
                {
                    ImGuiUtil.DrawTableColumn(route.ToString());
                    ImGuiUtil.DrawTableColumn(route.StartTime.ToString());
                    ImGuiUtil.DrawTableColumn(route.GetSpots(0).Normal.Name);
                    ImGuiUtil.DrawTableColumn(route.GetSpots(0).Spectral.Name);
                    ImGuiUtil.DrawTableColumn(route.GetSpots(1).Normal.Name);
                    ImGuiUtil.DrawTableColumn(route.GetSpots(1).Spectral.Name);
                    ImGuiUtil.DrawTableColumn(route.GetSpots(2).Normal.Name);
                    ImGuiUtil.DrawTableColumn(route.GetSpots(2).Spectral.Name);
                }
        }

        using (var table = ImRaii.Table("##OceanTimeline", 9, ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit))
        {
            if (table)
                for (var idx = 0; idx < GatherBuddy.GameData.OceanTimeline.Count; ++idx)
                {
                    var routeAldenard = GatherBuddy.GameData.OceanTimeline[OceanArea.Aldenard][idx];
                    var routeOthard   = GatherBuddy.GameData.OceanTimeline[OceanArea.Othard][idx];
                    ImGuiUtil.DrawTableColumn(idx.ToString());
                    ImGuiUtil.DrawTableColumn(routeAldenard.ToString());
                    ImGuiUtil.DrawTableColumn(routeAldenard.GetSpots(0).Normal.Name);
                    ImGuiUtil.DrawTableColumn(routeAldenard.GetSpots(1).Normal.Name);
                    ImGuiUtil.DrawTableColumn(routeAldenard.GetSpots(2).Normal.Name);
                    ImGuiUtil.DrawTableColumn(routeOthard.ToString());
                    ImGuiUtil.DrawTableColumn(routeOthard.GetSpots(0).Normal.Name);
                    ImGuiUtil.DrawTableColumn(routeOthard.GetSpots(1).Normal.Name);
                    ImGuiUtil.DrawTableColumn(routeOthard.GetSpots(2).Normal.Name);
                }
        }
    }

    private static void DrawCosmicTab()
    {
        if (!ImUtf8.CollapsingHeader("Cosmic Exploration Fishing Missions##CosmicDebug"u8))
            return;

        using (var table = ImUtf8.Table("##Cosmic", 2, ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit))
        {
            if (table)
                foreach (var mission in GatherBuddy.GameData.CosmicFishingMissions.Values.OrderBy(m => m.Id))
                {
                    ImUtf8.DrawTableColumn($"{mission.Id}");
                    ImUtf8.DrawTableColumn(mission.Name);
                }
        }
    }

    private void DrawDebugTab()
    {
        if (!GatherBuddy.DebugMode)
            return;

        using var id  = ImRaii.PushId("Debug");
        using var tab = ImRaii.TabItem("Debug");
        ImGuiUtil.HoverTooltip("I really hope there is a good reason for you seeing this.");

        if (!tab)
            return;

        using var child = ImRaii.Child(string.Empty);
        if (!child)
            return;

        const ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit;

        DrawDebugButtons();
        DrawDebugTime();
        DrawDebugFishingState();
        DrawDebugFishingTimes();
        DrawAlarmDebug();
        ImGuiTable.DrawTabbedTable($"Aetherytes ({GatherBuddy.GameData.Aetherytes.Count})", GatherBuddy.GameData.Aetherytes.Values,
            DrawDebugAetheryte, flags, "Id", "Name", "Territory", "Coords", "Aetherstream");
        ImGuiTable.DrawTabbedTable($"Territories ({GatherBuddy.GameData.WeatherTerritories.Length})", GatherBuddy.GameData.WeatherTerritories,
            DrawDebugTerritory, flags, "Id", "Name", "SizeFactor", "#Weathers", "Weathers");
        ImGuiTable.DrawTabbedTable($"Bait ({GatherBuddy.GameData.Bait.Count})", GatherBuddy.GameData.Bait.Values,
            DrawDebugBait, flags, "Id", "Name");
        ImGuiTable.DrawTabbedTable($"Gatherables ({GatherBuddy.GameData.Gatherables.Count})",
            GatherBuddy.GameData.Gatherables.Values.OrderBy(g => g.ItemId),
            DrawGatherableDebug, flags, "ItemId", "GatheringId", "Name", "Level", "#Nodes");
        ImGuiTable.DrawTabbedTable($"Gathering Nodes ({GatherBuddy.GameData.GatheringNodes.Count})", GatherBuddy.GameData.GatheringNodes.Values,
            DrawGatheringNodeDebug, flags, "Id", "Name", "Job", "Level", "Type", "Territory", "Coords", "Aetheryte", "Folklore", "Times",
            "Items");
        ImGuiTable.DrawTabbedTable($"Fish ({GatherBuddy.GameData.Fishes.Count})", GatherBuddy.GameData.Fishes.Values,
            DrawFishDebug, flags, "ItemId", "FishId", "Name", "Restrictions", "Folklore", "InLog", "Big", "Fishing Spots");
        ImGuiTable.DrawTabbedTable($"Fishing Spots ({GatherBuddy.GameData.FishingSpots.Count})", GatherBuddy.GameData.FishingSpots.Values,
            DrawFishingSpotDebug, flags, "Id", "Name", "Territory", "Aetheryte", "Coords", "Shadow", "Fishes");
        DrawUptimeManagerTable();
        DrawOceanTab();
        DrawCosmicTab();
        DrawWaymarkTab();
        if (ImGui.CollapsingHeader("GatheringTree"))
        {
            id.Push("GatheringTree");
            PrintNode(GatherBuddy.GameData.GatherablesTrie.Root);
            id.Pop();
        }

        if (ImGui.CollapsingHeader("FishingTree"))
        {
            id.Push("FishingTree");
            PrintNode(GatherBuddy.GameData.FishTrie.Root);
            id.Pop();
        }

        if (ImGui.CollapsingHeader("IPC"))
        {
            using (var group1 = ImRaii.Group())
            {
                ImGui.Text("Version");
                ImGui.Text(GatherBuddyIpc.VersionName);
                ImGui.Text(GatherBuddyIpc.IdentifyName);
                if (_plugin.Ipc._identifyProvider != null && ImGui.InputTextWithHint("##IPCIdentifyTest", "Identify...", ref _identifyTest, 64))
                    _lastItemIdentified = Dalamud.PluginInterface.GetIpcSubscriber<string, uint>(GatherBuddyIpc.IdentifyName)
                        .InvokeFunc(_identifyTest);
            }

            ImGui.SameLine();
            using var group2 = ImRaii.Group();
            ImGui.Text(GatherBuddyIpc.IpcVersion.ToString());
            ImGui.Text(_plugin.Ipc._versionProvider != null ? "Available" : "Unavailable");
            ImGui.Text(_plugin.Ipc._identifyProvider != null ? "Available" : "Unavailable");
            ImGui.Text(_lastItemIdentified.ToString());
        }

        DrawCosmicFishDataButton();
    }

    private static void DrawCosmicFishDataButton()
    {
        ImGui.PushItemWidth(100);
        ImUtf8.InputScalar($"Start ID: {GatherBuddy.GameData.FishingSpots.GetValueOrDefault(_startId)?.Name}", ref _startId);
        ImUtf8.InputScalar($"End ID: {GatherBuddy.GameData.FishingSpots.GetValueOrDefault(_endId)?.Name}",     ref _endId);
        ImGui.PopItemWidth();

        if (!ImUtf8.Button("Copy Most Recent Unknown Fish Data"u8))
            return;

        var patch = $"{nameof(Patch)}.{Enum.GetValues<Patch>().Last()}";
        var text  = "";
        foreach (var spot in GatherBuddy.GameData.FishingSpots.Values)
        {
            if (spot.Id < _startId || spot.Id > _endId)
                continue;

            if (spot.Items.Length is 0)
                continue;

            var  match     = CosmicMissionRegex().Match(spot.Name);
            uint missionId = 0;
            var  name      = spot.Name;
            if (match.Success)
            {
                var spotName = match.Groups[1].Value;
                missionId = uint.Parse(match.Groups[2].Value);
                name = spotName
                  + " "
                  + (Dalamud.GameData.GetExcelSheet<WKSMissionUnit>().GetRowOrDefault(missionId)?.Name.ExtractText() ?? "Unknown");
            }

            text += $"\n        // {name}\n";
            foreach (var fish in spot.Items)
            {
                text += $"        data.Apply({fish.ItemId}, {patch}) // {fish.Name}\n";
                text += "            .Bait(data)\n";
                if (missionId is not 0)
                    text += $"            .Mission(data, {missionId})\n";
                text += "            .Bite(data, HookSet.Unknown, BiteType.Unknown);\n";
            }
        }

        ImGui.SetClipboardText(text);
    }
}
