using System;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using GatherBuddy.Plugin;
using GatherBuddy.Time;
using ImGuiNET;
using ImGuiOtter;

namespace GatherBuddy.Gui;

public partial class Interface : Window, IDisposable
{
    private const string PluginName = "GatherBuddy";
    private const float  MinSize    = 700;

    private static GatherBuddy _plugin                 = null!;
    private        TimeStamp   _earliestKeyboardToggle = TimeStamp.Epoch;
    
    public Interface(GatherBuddy plugin)
        : base(GatherBuddy.Version.Length > 0 ? $"{PluginName} v{GatherBuddy.Version}###GatherBuddyMain" : PluginName)
    {
        _plugin            = plugin;
        _gatherGroupCache  = new GatherGroupCache(_plugin.GatherGroupManager);
        _gatherWindowCache = new GatherWindowCache();
        _locationTable     = new LocationTable();
        _alarmCache        = new AlarmCache(_plugin.AlarmManager);
        _recordTable       = new RecordTable();
        SizeConstraints = new WindowSizeConstraints()
        {
            MinimumSize = new Vector2(MinSize,     17 * ImGui.GetTextLineHeightWithSpacing() / ImGuiHelpers.GlobalScale),
            MaximumSize = new Vector2(MinSize * 4, ImGui.GetIO().DisplaySize.Y * 15 / 16 / ImGuiHelpers.GlobalScale),
        };
        IsOpen = GatherBuddy.Config.OpenOnStart;
    }

    public override void PreDraw() {
        Flags = GetFlags();
    }

    public override void Draw()
    {
        SetupValues();
        DrawHeader();
        if (!ImGui.BeginTabBar("ConfigTabs###GatherBuddyConfigTabs", ImGuiTabBarFlags.Reorderable))
            return;

        using var end = ImGuiRaii.DeferredEnd(ImGui.EndTabBar);
        DrawItemTab();
        DrawFishTab();
        DrawWeatherTab();
        DrawAlarmTab();
        DrawGatherGroupTab();
        DrawGatherWindowTab();
        DrawConfigTab();
        DrawLocationsTab();
        DrawRecordTab();
        DrawDebugTab();
    }

    public static ImGuiWindowFlags GetFlags()
    {
        var flags = ImGuiWindowFlags.None;

        if (GatherBuddy.Config.LockPosition) {
            flags |= ImGuiWindowFlags.NoMove;
        }

        if (GatherBuddy.Config.LockResize) {
            flags |= ImGuiWindowFlags.NoResize;
        }

        return flags;
    }

    public override void PreOpenCheck()
    {
        if (_earliestKeyboardToggle > GatherBuddy.Time.ServerTime || !Functions.CheckKeyState(GatherBuddy.Config.MainInterfaceHotkey, false))
            return;

        _earliestKeyboardToggle = GatherBuddy.Time.ServerTime.AddMilliseconds(500);
        Toggle();
    }

    public void Dispose()
    {
        _headerCache.Dispose();
        _weatherTable.Dispose();
        _itemTable.Dispose();
    }
}
