using System;
using System.Globalization;
using System.Linq;
using Dalamud;
using GatherBuddy.Classes;
using GatherBuddy.Levenshtein;
using GatherBuddy.Plugin;
using GatherBuddy.Structs;
using GatherBuddy.Time;
using ImGuiNET;
using OtterGui;
using static GatherBuddy.FishTimer.FishRecord;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private static void DrawDebugAetheryte(Aetheryte a)
    {
        ImGuiUtil.DrawTableColumn(a.Id.ToString());
        ImGuiUtil.DrawTableColumn(a.Name);
        ImGuiUtil.DrawTableColumn(a.Territory.Name);
        ImGuiUtil.DrawTableColumn($"{a.XCoord}-{a.YCoord}");
        ImGuiUtil.DrawTableColumn($"{a.XStream}-{a.YStream}");
    }

    private static void DrawDebugTerritory(Territory t)
    {
        ImGuiUtil.DrawTableColumn(t.Id.ToString());
        ImGuiUtil.DrawTableColumn(t.Name);
        ImGuiUtil.DrawTableColumn(t.SizeFactor.ToString(CultureInfo.InvariantCulture));
        ImGuiUtil.DrawTableColumn(t.WeatherRates.Rates.Length.ToString());
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
        }
    }

    private static unsafe void DrawDebugTime()
    {
        if (!ImGui.CollapsingHeader("Time"))
            return;

        if (!ImGui.BeginTable("##Times", 2))
            return;
        using var end = ImRaii.DeferredEnd(ImGui.EndTable);

        var fw  = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance();
        ImGuiUtil.DrawTableColumn("Framework Timestamp");
        ImGuiUtil.DrawTableColumn(fw == null ? "NULL" : fw->ServerTime.ToString());
        ImGuiUtil.DrawTableColumn("Framework Eorzea");
        ImGuiUtil.DrawTableColumn(fw == null ? "NULL" : fw->EorzeaTime.ToString());
        ImGuiUtil.DrawTableColumn("Framework Func");
        ImGuiUtil.DrawTableColumn(FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.GetServerTime().ToString());
        ImGuiUtil.DrawTableColumn("DateTimeOffset");
        ImGuiUtil.DrawTableColumn(DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
        ImGuiUtil.DrawTableColumn("GatherBuddy TimeStamp");
        ImGuiUtil.DrawTableColumn(GatherBuddy.Time.ServerTime.Time.ToString());
        ImGuiUtil.DrawTableColumn("GatherBuddy EorzeaTime");
        ImGuiUtil.DrawTableColumn(GatherBuddy.Time.EorzeaTime.Time.ToString());
    }
    
    private static unsafe void DrawDebugFishingState()
    {
        if (!ImGui.CollapsingHeader("Fishing State"))
            return;

        if (!ImGui.BeginTable("##Framework", 2))
            return;

        using var end     = ImRaii.DeferredEnd(ImGui.EndTable);
        ImGuiUtil.DrawTableColumn("UiState Address");
        ImGuiUtil.DrawTableColumn($"{(IntPtr)FFXIVClientStructs.FFXIV.Client.Game.UI.UIState.Instance():X}");
        ImGuiUtil.DrawTableColumn("Event Framework Address");
        ImGuiUtil.DrawTableColumn(GatherBuddy.EventFramework.Address.ToString("X"));
        ImGuiUtil.DrawTableColumn("Fishing Manager Address");
        ImGuiUtil.DrawTableColumn(GatherBuddy.EventFramework.FishingManager.ToString("X"));
        ImGuiUtil.DrawTableColumn("Fishing State Address");
        ImGuiUtil.DrawTableColumn(GatherBuddy.EventFramework.FishingStatePtr.ToString("X"));
        ImGuiUtil.DrawTableColumn("Fishing State");
        ImGuiUtil.DrawTableColumn(GatherBuddy.EventFramework.FishingState.ToString());
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
        ImGuiUtil.DrawTableColumn("Current Bait");
        ImGuiUtil.DrawTableColumn($"{record.Bait.Name} ({record.Bait.Id})");
        ImGuiUtil.DrawTableColumn("Duration");
        ImGuiUtil.DrawTableColumn(_plugin.FishRecorder.Timer.ElapsedMilliseconds.ToString());
        ImGuiUtil.DrawTableColumn("BiteType");
        ImGuiUtil.DrawTableColumn(record.Tug.ToString());
        ImGuiUtil.DrawTableColumn("HookSet");
        ImGuiUtil.DrawTableColumn(record.Hook.ToString());
        ImGuiUtil.DrawTableColumn("Last Catch");
        ImGuiUtil.DrawTableColumn($"{_plugin.FishRecorder.LastCatch?.Name[ClientLanguage.English] ?? "None"} ({_plugin.FishRecorder.LastCatch?.ItemId ?? 0} - {_plugin.FishRecorder.LastCatch?.FishId ?? 0})");
        ImGuiUtil.DrawTableColumn("Current Catch");
        ImGuiUtil.DrawTableColumn($"{record.Catch?.Name[ClientLanguage.English] ?? "None"} ({record.Catch?.ItemId ?? 0} - {record.Catch?.FishId ?? 0}) - of size {record.Size / 10f} times {record.Amount}");
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

        if (!ImGui.BeginTable("##Fishing Times", 6))
            return;

        using var end = ImRaii.DeferredEnd(ImGui.EndTable);
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

    private void DrawUptimeManagerTable()
    {
        if (!ImGui.CollapsingHeader($"Uptimes ({GatherBuddy.GameData.TimedGatherables})"))
            return;
        if (!ImGui.BeginTable("##Uptimes", 6, ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit))
            return;

        using var end = ImRaii.DeferredEnd(ImGui.EndTable);
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
                ImGuiUtil.DrawTableColumn(time.Start.ToString());
                ImGuiUtil.DrawTableColumn(time.End.ToString());
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
                ImGuiUtil.DrawTableColumn(time.Start.ToString());
                ImGuiUtil.DrawTableColumn(time.End.ToString());
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
        if(!ImGui.BeginTable("##Alarms", 2, ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit))
            return;
        using var end = ImRaii.DeferredEnd(ImGui.EndTable);

        ImGuiUtil.DrawTableColumn("Enabled");
        ImGuiUtil.DrawTableColumn(GatherBuddy.Config.AlarmsEnabled.ToString());
        ImGuiUtil.DrawTableColumn("Dirty");
        ImGuiUtil.DrawTableColumn(_plugin.AlarmManager.Dirty.ToString());
        ImGuiUtil.DrawTableColumn("Next Change (Absolute)");
        ImGuiUtil.DrawTableColumn(_plugin.AlarmManager.ActiveAlarms[0].Item2.LocalTime.ToString(CultureInfo.InvariantCulture));
        ImGuiUtil.DrawTableColumn("Next Change (Relative)");
        ImGuiUtil.DrawTableColumn(TimeInterval.DurationString(_plugin.AlarmManager.ActiveAlarms[0].Item2, GatherBuddy.Time.ServerTime, false));
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

    private void DrawDebugTab()
    {
        if (!GatherBuddy.DebugMode)
            return;

        using var id  = ImRaii.PushId("Debug");
        var       ret = ImGui.BeginTabItem("Debug");
        ImGuiUtil.HoverTooltip("I really hope there is a good reason for you seeing this.");

        if (!ret)
            return;

        using var end = ImRaii.DeferredEnd(ImGui.EndTabItem);
        if (!ImGui.BeginChild(string.Empty))
        {
            ImGui.EndChild();
            return;
        }

        end.Push(ImGui.EndChild);

        const ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit;

        DrawDebugButtons();
        DrawDebugTime();
        DrawDebugFishingState();
        DrawDebugFishingTimes();
        DrawAlarmDebug();
        ImGuiTable.DrawTabbedTable($"Aetherytes ({GatherBuddy.GameData.Aetherytes.Count})", GatherBuddy.GameData.Aetherytes.Values,
            DrawDebugAetheryte, flags, "Id", "Name", "Territory", "Coords", "Aetherstream");
        ImGuiTable.DrawTabbedTable($"Territories ({GatherBuddy.GameData.WeatherTerritories.Length})", GatherBuddy.GameData.WeatherTerritories,
            DrawDebugTerritory, flags, "Id", "Name", "SizeFactor", "#Weathers");
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
            using var group1 = ImRaii.NewGroup();
            ImGui.Text("Version");
            ImGui.Text(GatherBuddyIpc.VersionName);
            ImGui.Text(GatherBuddyIpc.IdentifyName);
            if (_plugin.Ipc._identifyProvider != null && ImGui.InputTextWithHint("##IPCIdentifyTest", "Identify...", ref _identifyTest, 64))
                _lastItemIdentified = Dalamud.PluginInterface.GetIpcSubscriber<string, uint>(GatherBuddyIpc.IdentifyName)
                    .InvokeFunc(_identifyTest);
            group1.Pop();
            ImGui.SameLine();
            using var group2 = ImRaii.NewGroup();
            ImGui.Text(GatherBuddyIpc.IpcVersion.ToString());
            ImGui.Text(_plugin.Ipc._versionProvider != null ? "Available" : "Unavailable");
            ImGui.Text(_plugin.Ipc._identifyProvider != null ? "Available" : "Unavailable");
            ImGui.Text(_lastItemIdentified.ToString());
        }
    }
}
