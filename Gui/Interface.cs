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
        private readonly Lumina.Excel.ExcelSheet<Weather> _weatherSheet;

        private FishManager FishManager
            => _plugin.Gatherer!.FishManager;

        private WeatherManager WeatherManager
            => _plugin.Gatherer!.WeatherManager;

        private readonly Cache.Icons  _icons;
        private          Cache.Header _headerCache;
        private          Cache.Alarms _alarmCache;
        private readonly Cache.Fish   _fishCache;
        private          Cache.Node   _nodeCache;

        private long _totalHourWeather = 0;
        private long _totalHourFish    = 0;

        public bool Visible;

        private static float   _horizontalSpace;
        private static float   _globalScale;
        private static float   _textHeight;
        private static float   _minXSize;
        private static Vector2 _itemSpacing  = Vector2.Zero;
        private static Vector2 _framePadding = Vector2.Zero;

        private void Save()
            => _pi.SavePluginConfig(_config);

        public Interface(GatherBuddy plugin, DalamudPluginInterface pi, GatherBuddyConfiguration config)
        {
            _pi     = pi;
            _plugin = plugin;
            _config = config;
            _lang   = pi.ClientState.ClientLanguage;

            _nodeCache = new Cache.Node(_config, plugin.Gatherer!.Timeline);
            _headerCache.Setup();
            _alarmCache = new Cache.Alarms(_plugin.Alarms!, _lang);

            _weatherSheet = _pi.Data.GetExcelSheet<Weather>();
            _icons = Service<Cache.Icons>.Set(_pi, (int) _weatherSheet.RowCount
              + FishManager.FishByUptime.Count
              + FishManager.Bait.Count);

            _fishCache = new Cache.Fish(_icons);


            // Weather Setup
            _nextWeatherTimes = WeatherManager.NextWeatherChangeTimes(NumWeathers, -WeatherManager.SecondsPerWeather * 2);
            _nextWeatherTimeStrings = UpdateWeatherTimeStrings();

            _weatherCache   = CreateWeatherCache(WeatherManager);
            _zoneFilterSize = _weatherCache.Max(c => ImGui.CalcTextSize(c.Zone).X);

            if (_config.ShowFishFromPatch >= PatchSelector.Length)
            {
                _config.ShowFishFromPatch = 0;
                Save();
            }

            _allCachedFish         = FishManager.FishByUptime.ToDictionary(f => f, f => new CachedFish(this, f));
            _currentSelector       = PatchSelector[_config.ShowFishFromPatch];
            _currentlyRelevantFish = new Fish[0];
            _cachedFish            = new CachedFish[0];
            SetCurrentlyRelevantFish();

            foreach (var fish in _allCachedFish.Values)
            {
                _longestName = Math.Max(_longestName, ImGui.CalcTextSize(fish.Name).X / ImGui.GetIO().FontGlobalScale);
                _longestSpot = Math.Max(_longestSpot, ImGui.CalcTextSize(fish.FishingSpot).X / ImGui.GetIO().FontGlobalScale);
                _longestZone = Math.Max(_longestZone, ImGui.CalcTextSize(fish.Territory).X / ImGui.GetIO().FontGlobalScale);
            }
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
            _weatherIconSize  = new Vector2(30, 30) * ImGui.GetIO().FontGlobalScale;
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
                _nodeCache.Update(hour);
                DrawNodesTab();
                ImGui.EndTabItem();
            }

            var fishTab = ImGui.BeginTabItem("Timed Fish");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Shows all fish for the fishing log and their next uptimes.\n"
                  + "You can click the fish name or the fishing spot name to execute a /gatherfish command.");
            if (fishTab)
            {
                UpdateFish(hour);
                DrawFishTab();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Weather"))
            {
                UpdateWeather(hour);
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
