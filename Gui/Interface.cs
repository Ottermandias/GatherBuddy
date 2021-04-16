using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud;
using Dalamud.Plugin;
using GatherBuddy.Game;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
using ImGuiNET;
using ImGuiScene;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Gui
{
    public partial class Interface : IDisposable
    {
        private const string PluginName             = "GatherBuddy";
        private const float  DefaultHorizontalSpace = 5;

        private readonly GatherBuddy                      _plugin;
        private readonly DalamudPluginInterface           _pi;
        private readonly GatherBuddyConfiguration         _config;
        private readonly ClientLanguage                   _lang;

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

        private void Save()
            => _pi.SavePluginConfig(_config);

        public Interface(GatherBuddy plugin, DalamudPluginInterface pi, GatherBuddyConfiguration config)
        {
            _pi     = pi;
            _plugin = plugin;
            _config = config;
            _lang   = pi.ClientState.ClientLanguage;

            _nodeTabCache = new Cache.NodeTab(_config, plugin.Gatherer!.Timeline);
            _headerCache.Setup();
            _alarmCache = new Cache.Alarms(_plugin.Alarms!, _lang);

            var weatherSheet = _pi.Data.GetExcelSheet<Weather>();
            _icons = Service<Cache.Icons>.Set(_pi, (int) weatherSheet.RowCount
              + FishManager.FishByUptime.Count
              + FishManager.Bait.Count);

            _weatherCache = new Cache.Weather(WeatherManager);

            if (_config.ShowFishFromPatch >= Cache.FishTab.PatchSelector.Length)
            {
                _config.ShowFishFromPatch = 0;
                Save();
            }

            _fishCache = new Cache.FishTab(WeatherManager, _config, FishManager, _icons);
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

            if (!ImGui.Begin(PluginName, ref Visible))
                return;

            var minute = EorzeaTime.CurrentMinute();
            var hour   = minute / RealTime.MinutesPerHour;

            DrawHeaderRow();
            DrawTimeRow(hour, minute);

            if (!ImGui.BeginTabBar("##Tabs", ImGuiTabBarFlags.NoTooltip))
                return;

            var nodeTab = ImGui.BeginTabItem("Timed Nodes");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Shows timed nodes corresponding to the selection of the checkmarks below, sorted by next uptime.\n"
                  + "Click on a node to do a /gather command for that node.");
            if (nodeTab)
            {
                _nodeTabCache.Update(hour);
                DrawNodesTab();
                ImGui.EndTabItem();
            }

            var fishTab = ImGui.BeginTabItem("Timed Fish");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Shows all fish for the fishing log and their next uptimes.\n"
                  + "You can click the fish name or the fishing spot name to execute a /gatherfish command.");
            if (fishTab)
            {
                _fishCache.UpdateFish(hour);
                DrawFishTab();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Weather"))
            {
                _weatherCache.Update(hour);
                DrawWeatherTab();
                ImGui.EndTabItem();
            }

            var alertTab = ImGui.BeginTabItem("Alarms");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Setup alarms for specific timed gathering nodes.\n"
                  + "You can use [/gather alarm] to directly gather the last triggered alarm.");
            if (alertTab)
            {
                DrawAlarmsTab();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Settings"))
            {
                DrawSettingsTab();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
            ImGui.End();
        }
    }
}
