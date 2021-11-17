using System;
using System.Numerics;
using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Gui;

public partial class Interface : IDisposable
{
    private const string PluginName             = "GatherBuddy";
    private const float  DefaultHorizontalSpace = 5;

    private readonly string      _configHeader;
    private readonly GatherBuddy _plugin;

    private FishManager FishManager
        => _plugin.Gatherer!.FishManager;

    private WeatherManager WeatherManager
        => _plugin.Gatherer!.WeatherManager;

    private readonly Cache.Icons   _icons;
    private          Cache.Header  _headerCache;
    private          Cache.Alarms  _alarmCache;
    private readonly Cache.FishTab _fishCache;
    private readonly Cache.NodeTab _nodeTabCache;
    private          Cache.Weather _weatherCache;

    public bool Visible;

    private static float   _horizontalSpace;
    private static float   _globalScale;
    private static float   _textHeight;
    private static float   _minXSize;
    private static Vector2 _itemSpacing  = Vector2.Zero;
    private static Vector2 _framePadding = Vector2.Zero;
    private static float   _textHeightOffset;
    private static Vector2 _iconSize;
    private static Vector2 _smallIconSize;
    private static Vector2 _fishIconSize;
    private static Vector2 _weatherIconSize;
    private        float   _alarmsSpacing;

    public Interface(GatherBuddy plugin)
    {
        _plugin       = plugin;
        _configHeader = GatherBuddy.Version.Length > 0 ? $"{PluginName} v{GatherBuddy.Version}###GatherBuddyMain" : PluginName;

        _nodeTabCache = new Cache.NodeTab(plugin.Gatherer!.Timeline);
        _headerCache.Setup();
        _alarmCache = new Cache.Alarms(_plugin.Alarms!, GatherBuddy.Language);

        var weatherSheet = Dalamud.GameData.GetExcelSheet<Weather>()!;
        _icons = Service<Cache.Icons>.Set((int)weatherSheet.RowCount + FishManager.FishByUptime.Count + FishManager.Bait.Count)!;

        _weatherCache = new Cache.Weather(WeatherManager);

        if (GatherBuddy.Config.ShowFishFromPatch >= Cache.FishTab.PatchSelector.Length)
        {
            GatherBuddy.Config.ShowFishFromPatch = 0;
            GatherBuddy.Config.Save();
        }

        _fishCache = new Cache.FishTab(WeatherManager, FishManager, _icons);
    }

    public void Dispose()
    {
        Service<Cache.Icons>.Dispose();
    }

    public void Draw()
    {
        if (!Visible)
            return;

        // Initialize style variables.
        _globalScale      = ImGui.GetIO().FontGlobalScale;
        _horizontalSpace  = DefaultHorizontalSpace * _globalScale;
        _minXSize         = 450f * _globalScale;
        _textHeight       = ImGui.GetTextLineHeightWithSpacing();
        _itemSpacing      = ImGui.GetStyle().ItemSpacing;
        _framePadding     = ImGui.GetStyle().FramePadding;
        _iconSize         = new Vector2(40, 40) * ImGui.GetIO().FontGlobalScale;
        _smallIconSize    = _iconSize / 2;
        _weatherIconSize  = new Vector2(30, 30) * ImGui.GetIO().FontGlobalScale;
        _fishIconSize     = new Vector2(_iconSize.X * ImGui.GetTextLineHeight() / _iconSize.Y, ImGui.GetTextLineHeight());
        _textHeightOffset = (_weatherIconSize.Y - ImGui.GetTextLineHeight()) / 2;

        ImGui.SetNextWindowSizeConstraints(
            new Vector2(_minXSize,     _textHeight * 17),
            new Vector2(_minXSize * 4, ImGui.GetIO().DisplaySize.Y * 15 / 16));

        using var raii = new ImGuiRaii();
        if (!raii.BeginWindow(_configHeader, ref Visible))
            return;

        var minute = TimeStamp.UtcNow.TotalEorzeaMinutes();
        var hour   = minute / RealTime.MinutesPerHour;

        DrawHeaderRow();
        DrawTimeRow(hour, minute);

        if (!raii.BeginTabBar("##Tabs", ImGuiTabBarFlags.NoTooltip | ImGuiTabBarFlags.Reorderable))
            return;

        var nodeTab = raii.BeginTabItem("Timed Nodes");
        ImGuiHelper.HoverTooltip("Shows timed nodes corresponding to the selection of the checkmarks below, sorted by next uptime.\n"
          + "Click on a node to do a /gather command for that node.");
        if (nodeTab)
        {
            _nodeTabCache.Update(hour);
            DrawNodesTab();
            raii.End();
        }

        var fishTab = raii.BeginTabItem("Timed Fish");
        ImGuiHelper.HoverTooltip("Shows all fish for the fishing log and their next uptimes.\n"
          + "You can click the fish name or the fishing spot name to execute a /gatherfish command.\n"
          + "You can right-click a fish name to fix (or unfix) this fish at the top of the list.");
        if (fishTab)
        {
            _fishCache.UpdateFish();
            DrawFishTab();
            raii.End();
        }

        if (raii.BeginTabItem("Weather"))
        {
            _weatherCache.Update(hour);
            DrawWeatherTab();
            raii.End();
        }

        var alertTab = raii.BeginTabItem("Alarms");
        ImGuiHelper.HoverTooltip("Setup alarms for specific timed gathering nodes.\n"
          + "You can use [/gather alarm] to directly gather the last triggered alarm.");
        if (alertTab)
        {
            DrawAlarmsTab();
            raii.End();
        }

        if (raii.BeginTabItem("Settings"))
        {
            DrawSettingsTab();
            raii.End();
        }
    }
}
