using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using GatherBuddy.Time;
using Functions = GatherBuddy.Plugin.Functions;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface : Window, IDisposable
{
    private const string PluginName = "GatherBuddy Reborn";
    private const float  MinSize    = 700;

    private static GatherBuddy _plugin                 = null!;
    private        TimeStamp   _earliestKeyboardToggle = TimeStamp.Epoch;

    private static List<ExtendedFish>? _extendedFishList;

    public static IReadOnlyList<ExtendedFish> ExtendedFishList
        => _extendedFishList ??= GatherBuddy.GameData.Fishes.Values
            .Where(f => f.FishingSpots.Count > 0)
            .Select(f => new ExtendedFish(f)).ToList();

    public Interface(GatherBuddy plugin)
        : base(GatherBuddy.Version.Length > 0 ? $"{PluginName} v{GatherBuddy.Version}###GatherBuddyMain" : PluginName)
    {
        _plugin            = plugin;
        _gatherGroupCache  = new GatherGroupCache(_plugin.GatherGroupManager);
        _autoGatherListsCache = new AutoGatherListsCache();
        _gatherWindowCache = new GatherWindowCache();
        _locationTable     = new LocationTable();
        _alarmCache        = new AlarmCache(_plugin.AlarmManager);
        _recordTable       = new RecordTable();
        IsOpen             = GatherBuddy.Config.OpenOnStart;
        UpdateFlags();
    }

    public override void PreDraw()
    {
        SetupValues();
        // Skip dalamud size constraints because that would just require unscaling, then scaling.
        var minSize = new Vector2(MinSize,     17 * ImGui.GetTextLineHeightWithSpacing());
        var maxSize = new Vector2(MinSize * 4, ImGui.GetIO().DisplaySize.Y * 15 / 16);
        ImGui.SetNextWindowSizeConstraints(minSize, maxSize);
    }

    public override void Draw()
    {
        DrawHeader();
        using var tab = ImRaii.TabBar("ConfigTabs###GatherBuddyConfigTabs", ImGuiTabBarFlags.Reorderable);
        if (!tab)
            return;

        DrawItemTab();
        DrawFishTab();
        DrawWeatherTab();
        DrawAlarmTab();
        DrawGatherGroupTab();
        DrawGatherWindowTab();
        DrawAutoGatherTab();
        DrawConfigTab();
        DrawConfigPresetsTab();
        DrawLocationsTab();
        DrawRecordTab();
        DrawStatsPageTab();
        DrawDebugTab();
    }

    public void UpdateFlags()
    {
        if (GatherBuddy.Config.MainWindowLockPosition)
            Flags |= ImGuiWindowFlags.NoMove;
        else
            Flags &= ~ImGuiWindowFlags.NoMove;

        if (GatherBuddy.Config.MainWindowLockResize)
            Flags |= ImGuiWindowFlags.NoResize;
        else
            Flags &= ~ImGuiWindowFlags.NoResize;
        //RespectCloseHotkey = GatherBuddy.Config.CloseOnEscape;
    }

    public override void PreOpenCheck()
    {
        if (_earliestKeyboardToggle > GatherBuddy.Time.ServerTime || !Functions.CheckKeyState(GatherBuddy.Config.MainInterfaceHotkey, false))
            return;

        _earliestKeyboardToggle = GatherBuddy.Time.ServerTime.AddMilliseconds(500);
        IsOpen                  = !IsOpen;
    }

    public void Dispose()
    {
        _headerCache.Dispose();
        _weatherTable.Dispose();
        _itemTable.Dispose();
        _autoGatherListsCache.Dispose();
    }
}
