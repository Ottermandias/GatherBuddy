using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
using ImGuiNET;
using ImGuiScene;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private const int NumWeathers = 8;

        private          DateTime[]                   _nextWeatherTimes;
        private          string[]                     _nextWeatherTimeStrings;
        private readonly CachedWeather[]              _weatherCache;

        private void UpdateWeatherTimes(int addSeconds)
        {
            _nextWeatherTimes       = _nextWeatherTimes.Select(t => t.AddSeconds(addSeconds)).ToArray();
            _nextWeatherTimeStrings = UpdateWeatherTimeStrings();
        }

        private string[] UpdateWeatherTimeStrings()
            => _nextWeatherTimes.Select(t => t.TimeOfDay.ToString()).ToArray();


        private readonly struct CachedWeather
        {
            public readonly string        Zone;
            public readonly string[]      WeatherNames;
            public readonly TextureWrap[] Icons;

            public CachedWeather(string name)
            {
                Zone         = name;
                WeatherNames = new string[NumWeathers];
                Icons        = new TextureWrap[NumWeathers];
            }

            public void Update(WeatherManager weather, int idx)
            {
                var timeline = weather.UniqueZones[idx];
                timeline.Update(NumWeathers);
                var icons = Service<Cache.Icons>.Get();
                for (var i = 0; i < NumWeathers; ++i)
                {
                    WeatherNames[i] = timeline.List[i].Weather.Name;
                    Icons[i]        = icons[timeline.List[i].Weather.Icon];
                }
            }
        }

        private static CachedWeather[] CreateWeatherCache(WeatherManager weather)
        {
            var ret = new CachedWeather[weather.UniqueZones.Count];
            for (var i = 0; i < weather.UniqueZones.Count; ++i)
            {
                ret[i] = new CachedWeather(weather.UniqueZones[i].Territory.Name);
                ret[i].Update(weather, i);
            }

            return ret;
        }

        private void UpdateWeatherCache()
        {
            var weather = _plugin.Gatherer!.WeatherManager;
            for (var i = 0; i < _weatherCache.Length; ++i)
                _weatherCache[i].Update(weather, i);
        }

        private void UpdateWeather(long totalHour)
        {
            if (totalHour - _totalHourWeather < 8)
                return;

            var hour = totalHour % RealTime.HoursPerDay;
            _totalHourWeather = totalHour - (hour & 0b111);

            UpdateWeatherTimes(WeatherManager.SecondsPerWeather);
            for (var i = 0; i < _weatherCache.Length; ++i)
                _weatherCache[i].Update(_plugin.Gatherer!.WeatherManager, i);
        }

        private          string  _zoneFilter      = "";
        private          string  _zoneFilterLower = "";
        private readonly float   _zoneFilterSize;

        private static float   _textHeightOffset;
        private static Vector2 _iconSize;
        private static Vector2 _weatherIconSize;

        private void DrawHeaderLine()
        {
            ImGui.TableSetupScrollFreeze(1, 1);

            ImGui.TableSetupColumn("Zone");
            foreach (var name in _nextWeatherTimeStrings)
                ImGui.TableSetupColumn(name);
            ImGui.TableNextRow(ImGuiTableRowFlags.Headers);
            ImGui.TableSetColumnIndex(0);
            ImGui.PushID(0);
            ImGui.SetNextItemWidth(_zoneFilterSize * ImGui.GetIO().FontGlobalScale);
            if (ImGui.InputTextWithHint("##weatherZone", "Zone Filter", ref _zoneFilter, 64))
                _zoneFilterLower = _zoneFilter.ToLowerInvariant();
            ImGui.SameLine();
            ImGui.TableHeader("##Weather0");

            ImGui.PopID();
            for (var column = 1; column < _nextWeatherTimeStrings.Length + 1; column++)
            {
                ImGui.TableSetColumnIndex(column);
                ImGui.AlignTextToFramePadding();
                ImGui.Text(ImGui.TableGetColumnName(column));
                ImGui.SameLine();
                ImGui.TableHeader($"##Weather{column}");
            }

            ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, 0xFF008000, 2);
            ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, 0xFF000080, 1);
        }

        private static Vector2 AlignedTextToWeatherIcon(string text)
        {
            var pos = ImGui.GetCursorPosY();
            ImGui.SetCursorPosY(pos + _textHeightOffset);
            ImGui.Text(text);
            ImGui.SameLine();
            return new Vector2(ImGui.GetCursorPosX(), pos);
        }

        private static void AlignedTextToWeatherIcon(Vector4 color, string text)
        {
            var pos = ImGui.GetCursorPosY();
            ImGui.SetCursorPosY(_textHeightOffset);
            ImGui.TextColored(color, text);
            ImGui.SetCursorPosY(pos);
        }

        private static void NamedWeather(TextureWrap icon, string name)
        {
            var cursor = ImGui.GetCursorPos();
            ImGui.Image(icon.ImGuiHandle, _weatherIconSize);
            ImGui.SetCursorPos(cursor + new Vector2(_weatherIconSize.X + _itemSpacing.X / 2, _textHeightOffset));
            ImGui.Text(name);
        }

        private void DrawWeatherLine(CachedWeather cache)
        {
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, 0x1000FF00, 2);
            ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, 0x100000FF, 1);
            ImGui.SetCursorPosY(ImGui.GetCursorPosY() + _textHeightOffset);
            ImGui.Text(cache.Zone);
            for (var i = 0; i < NumWeathers; ++i)
            {
                ImGui.TableNextColumn();
                NamedWeather(cache.Icons[i], cache.WeatherNames[i]);
            }
        }

        private bool FilterZone(string zoneName)
            => _zoneFilter == string.Empty || zoneName.ToLowerInvariant().Contains(_zoneFilterLower);

        private void DrawWeatherTab()
        {
            const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit
              | ImGuiTableFlags.ScrollX
              | ImGuiTableFlags.ScrollY
              | ImGuiTableFlags.RowBg
              | ImGuiTableFlags.BordersInner;

            var pos = ImGui.GetCursorPos();
            if (!ImGui.BeginTable("##weatherTable", NumWeathers + 1, flags, -Vector2.One))
                return;

            var weather = _plugin.Gatherer!.WeatherManager;

            DrawHeaderLine();

            foreach (var cache in _weatherCache.Where(cache => FilterZone(cache.Zone)))
                DrawWeatherLine(cache);

            ImGui.EndTable();
        }
    }
}
