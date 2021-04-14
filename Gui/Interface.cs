using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud;
using Dalamud.Data.LuminaExtensions;
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
        private          Lumina.Excel.ExcelSheet<Weather> _weatherSheet;

        private long _totalHourNodes   = 0;
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

        private TextureWrap LoadIcon(int id)
        {
            if (_icons.TryGetValue(id, out var ret))
                return ret;

            var icon     = _pi.Data.GetIcon(id);
            var iconData = icon.GetRgbaImageData();

            ret        = _pi.UiBuilder.LoadImageRaw(iconData, icon.Header.Width, icon.Header.Height, 4);
            _icons[id] = ret;
            return ret;
        }

        public Interface(GatherBuddy plugin, DalamudPluginInterface pi, GatherBuddyConfiguration config)
        {
            _pi     = pi;
            _plugin = plugin;
            _config = config;
            _lang   = pi.ClientState.ClientLanguage;

            // Node List setup
            RebuildList(false);

            // Alarms Setup
            _allTimedNodesNames = plugin.Alarms!.AllTimedNodes
                .Select(n => $"{n.Times!.PrintHours(true)}: {n.Items!.PrintItems(", ", _lang)}")
                .ToArray();
            _longestNodeStringLength = _allTimedNodesNames.Max(n => ImGui.CalcTextSize(n).X);

            // Weather Setup
            _nextWeatherTimes = _plugin.Gatherer!.WeatherManager.NextWeatherChangeTimes(NumWeathers, -WeatherManager.SecondsPerWeather * 2);
            _nextWeatherTimeStrings = UpdateWeatherTimeStrings();

            _weatherSheet = _pi.Data.GetExcelSheet<Weather>();

            _icons = new SortedList<int, TextureWrap>((int) _weatherSheet.RowCount
              + 3 * _allTimedNodesNames.Length
              + _plugin.Gatherer!.FishManager.FishByUptime.Count
              + _plugin.Gatherer!.FishManager.Bait.Count);

            foreach (var weather in _weatherSheet)
                LoadIcon(weather.Icon);
            foreach (var fish in plugin.Gatherer!.FishManager.Fish.Values)
                LoadIcon(fish.ItemData.Icon);
            foreach (var bait in plugin.Gatherer!.FishManager.Bait.Values)
                LoadIcon(bait.Data.Icon);

            _hookSet          = LoadIcon(1103);
            _powerfulHookSet  = LoadIcon(1115);
            _precisionHookSet = LoadIcon(1116);
            _snagging         = LoadIcon(1109);
            _gigs             = LoadIcon(1121);
            _smallGig         = LoadIcon(60671);
            _normalGig        = LoadIcon(60672);
            _largeGig         = LoadIcon(60673);

            _weatherCache   = CreateWeatherCache(_plugin.Gatherer!.WeatherManager, _icons);
            _zoneFilterSize = _weatherCache.Max(c => ImGui.CalcTextSize(c.Zone).X);

            if (_config.ShowFishFromPatch >= PatchSelector.Length)
            {
                _config.ShowFishFromPatch = 0;
                Save();
            }

            _allCachedFish         = _plugin.Gatherer!.FishManager.FishByUptime.ToDictionary(f => f, f => new CachedFish(this, f));
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
            foreach (var icon in _icons.Values)
                icon.Dispose();
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
            _iconSize         = new Vector2(_icons.Last().Value.Width, _icons.Last().Value.Height) * ImGui.GetIO().FontGlobalScale;
            _textHeightOffset = (_iconSize.Y - ImGui.GetTextLineHeight()) / 2;

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
                UpdateNodes(hour);
                DrawNodesTab(_horizontalSpace);
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
