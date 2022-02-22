using System;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using GatherBuddy.Config;
using ImGuiNET;
using ImGuiOtter;

namespace GatherBuddy.Gui;

public partial class Interface : Window, IDisposable
{
    private const string PluginName = "GatherBuddy";
    private const float  MinSize    = 550;

    private static GatherBuddy _plugin = null!;

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

    private static void VerifyTabOrder()
    {
        if (Enum.IsDefined(GatherBuddy.Config.TabSortOrder))
            return;

        GatherBuddy.Config.TabSortOrder = TabSortOrder.ItemFishWeather;
        GatherBuddy.Config.Save();
    }

    public override void Draw()
    {
        SetupValues();
        DrawHeader();
        if (!ImGui.BeginTabBar("ConfigTabs"))
            return;


        using var end = ImGuiRaii.DeferredEnd(ImGui.EndTabBar);
        VerifyTabOrder();
        (Action, Action, Action) tabs = GatherBuddy.Config.TabSortOrder switch
        {
            TabSortOrder.ItemFishWeather => (DrawItemTab, DrawFishTab, DrawWeatherTab),
            TabSortOrder.ItemWeatherFish => (DrawItemTab, DrawWeatherTab, DrawFishTab),
            TabSortOrder.FishNodeWeather => (DrawFishTab, DrawItemTab, DrawWeatherTab),
            TabSortOrder.FishWeatherItem => (DrawFishTab, DrawWeatherTab, DrawItemTab),
            TabSortOrder.WeatherFishItem => (DrawWeatherTab, DrawFishTab, DrawItemTab),
            TabSortOrder.WeatherItemFish => (DrawWeatherTab, DrawItemTab, DrawFishTab),
            _                            => throw new ArgumentOutOfRangeException(),
        };
        tabs.Item1();
        tabs.Item2();
        tabs.Item3();
        DrawAlarmTab();
        DrawGatherGroupTab();
        DrawGatherWindowTab();
        DrawConfigTab();
        DrawLocationsTab();
        DrawRecordTab();
        DrawDebugTab();
    }

    public void Dispose()
    {
        _headerCache.Dispose();
        _weatherTable.Dispose();
        _itemTable.Dispose();
    }
}
