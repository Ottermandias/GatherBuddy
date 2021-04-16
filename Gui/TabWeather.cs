using System.Linq;
using System.Numerics;
using ImGuiNET;
using ImGuiScene;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private void DrawHeaderLine()
        {
            ImGui.TableSetupScrollFreeze(1, 1);

            ImGui.TableSetupColumn("Zone");
            foreach (var name in _weatherCache.WeatherTimeStrings)
                ImGui.TableSetupColumn(name);
            ImGui.TableNextRow(ImGuiTableRowFlags.Headers);
            ImGui.TableSetColumnIndex(0);
            ImGui.PushID(0);
            ImGui.SetNextItemWidth(_weatherCache.FilterSize * ImGui.GetIO().FontGlobalScale);
            if (ImGui.InputTextWithHint("##weatherZone", "Zone Filter", ref _weatherCache.Filter, 64))
                _weatherCache.FilterLower = _weatherCache.Filter.ToLowerInvariant();
            ImGui.SameLine();
            ImGui.TableHeader("##Weather0");

            ImGui.PopID();
            for (var column = 1; column < Cache.Weather.NumWeathers + 1; column++)
            {
                ImGui.TableSetColumnIndex(column);
                ImGui.AlignTextToFramePadding();
                ImGui.Text(ImGui.TableGetColumnName(column));
                ImGui.SameLine();
                ImGui.TableHeader($"##Weather{column}");
            }

            ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, Colors.WeatherTab.CurrentWeather, 2);
            ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, Colors.WeatherTab.LastWeather,    1);
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

        private static void DrawWeatherLine(Cache.Weather.CachedWeather cache)
        {
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, Colors.WeatherTab.HeaderCurrentWeather, 2);
            ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, Colors.WeatherTab.HeaderLastWeather,    1);
            ImGui.SetCursorPosY(ImGui.GetCursorPosY() + _textHeightOffset);
            ImGui.Text(cache.Zone);
            for (var i = 0; i < Cache.Weather.NumWeathers; ++i)
            {
                ImGui.TableNextColumn();
                NamedWeather(cache.Icons[i], cache.WeatherNames[i]);
            }
        }

        private bool FilterZone(string zoneNameLower)
            => _weatherCache.FilterLower == string.Empty || zoneNameLower.Contains(_weatherCache.FilterLower);

        private void DrawWeatherTab()
        {
            const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit
              | ImGuiTableFlags.ScrollX
              | ImGuiTableFlags.ScrollY
              | ImGuiTableFlags.RowBg
              | ImGuiTableFlags.BordersInner;

            var pos = ImGui.GetCursorPos();
            if (!ImGui.BeginTable("##weatherTable", Cache.Weather.NumWeathers + 1, flags, -Vector2.One))
                return;

            var weather = _plugin.Gatherer!.WeatherManager;

            DrawHeaderLine();

            if (_weatherCache.Filter.Length > 0)
                foreach (var cache in _weatherCache.Weathers.Where(cache => FilterZone(cache.ZoneLower)))
                    DrawWeatherLine(cache);
            else
                foreach (var cache in _weatherCache.Weathers)
                    DrawWeatherLine(cache);

            ImGui.EndTable();
        }
    }
}
