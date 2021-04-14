using System.Numerics;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
using ImGuiNET;
using ImGuiScene;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private void DrawRecordBox()
            => DrawCheckbox("Record",
                "Toggle whether to record all encountered nodes in regular intervals.\n"
              + "Recorded node coordinates are more accurate than pre-programmed ones and will be used for map markers and aetherytes instead.\n"
              + "Records are saved in compressed form in the plugin configuration.",
                _config.DoRecord, b =>
                {
                    if (b)
                        _plugin.Gatherer!.StartRecording();
                    else
                        _plugin.Gatherer!.StopRecording();
                    _config.DoRecord = b;
                });

        private void DrawAlarmToggle()
            => DrawCheckbox("Alarms",  "Toggle all alarms on or off.",
                _config.AlarmsEnabled, b =>
                {
                    if (b)
                        _plugin.Alarms!.Enable();
                    else
                        _plugin.Alarms!.Disable();
                    _config.AlarmsEnabled = b;
                });

        private void DrawSnapshotButton()
        {
            if (ImGui.Button("Snapshot", new Vector2(-1, 0)))
                _pi.Framework.Gui.Chat.Print($"Recorded {_plugin.Gatherer!.Snapshot()} new nearby gathering nodes.");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Record currently available nodes around you once.");
        }

        private void DrawHeaderRow()
        {
            var spacing = 5 * _horizontalSpace;
            ImGui.BeginGroup();
            DrawAlarmToggle();
            HorizontalSpace(spacing);
            DrawRecordBox();
            HorizontalSpace(spacing);
            DrawSnapshotButton();
            ImGui.EndGroup();
            ImGui.Dummy(new Vector2(0, _horizontalSpace / 2));
        }

        private long         _lastDrawHour = 0;
        private uint         _lastTerritory;
        private TextureWrap? _lastCurrentWeather;
        private TextureWrap? _lastNextWeather;

        private void UpdateTimeRow(long hour, uint territory)
        {
            if (hour - _lastDrawHour < 8 && _lastTerritory == territory)
                return;

            _lastDrawHour = hour - (hour & 0b111);
            if (territory == 0)
            {
                _lastTerritory      = 0;
                _lastCurrentWeather = null;
                _lastNextWeather    = null;
                return;
            }

            _lastTerritory = territory;
            var weathers = Service<SkyWatcher>.Get().GetForecast(_lastTerritory, 2);
            _lastCurrentWeather = _icons[weathers[0].Weather.Icon];
            _lastNextWeather    = _icons[weathers[1].Weather.Icon];
        }

        private void DrawTimeRow(long eorzeaHour, long eorzeaMinute)
        {
            var minuteOfHour = eorzeaMinute % RealTime.MinutesPerHour;
            var hourOfDay    = eorzeaHour % RealTime.HoursPerDay;

            var nextHourS = EorzeaTime.SecondsPerEorzeaHour - RealTime.CurrentTimestamp() % EorzeaTime.SecondsPerEorzeaHour;
            var nextHourM = nextHourS / RealTime.SecondsPerMinute;

            var nextWeatherS = nextHourS + EorzeaTime.SecondsPerEorzeaHour * (7 - hourOfDay % 8);
            var nextWeatherM = nextWeatherS / RealTime.SecondsPerMinute;

            nextHourS    -= nextHourM * RealTime.SecondsPerMinute;
            nextWeatherS -= nextWeatherM * RealTime.SecondsPerMinute;

            ImGui.PushStyleColor(ImGuiCol.Button,        0xFF008080);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xFF008080);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive,  0xFF008080);

            var size = new Vector2(0, _iconSize.Y);

            ImGui.Button($"ET {hourOfDay:D2}:{minuteOfHour:D2}", size);
            ImGui.SameLine();

            ImGui.PushStyleColor(ImGuiCol.Button,        0xFF404040);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xFF404040);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive,  0xFF404040);

            var nextWeatherString = $"  {nextWeatherM:D2}.{nextWeatherS:D2} Min.  ";
            var width = -(ImGui.CalcTextSize(nextWeatherString).X + (_iconSize.X + _itemSpacing.X * 1.5f + ImGui.GetStyle().FramePadding.X) * 2);
            ImGui.Button($"{nextHourM:D2}.{nextHourS:D2} Min to next Eorzea hour.", new Vector2(width, size.Y));

            ImGui.PopStyleColor(6);

            var territory = _pi.ClientState?.TerritoryType ?? 0;
            UpdateTimeRow(eorzeaHour, territory);

            ImGui.PushStyleColor(ImGuiCol.Button,        0xFFA0A000);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xFFA0A000);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive,  0xFFA0A000);
            if (_lastTerritory != 0)
            {
                ImGui.SameLine();
                ImGui.Image(_lastCurrentWeather!.ImGuiHandle, _iconSize);
                ImGui.SameLine();
                ImGui.Button(nextWeatherString, size);
                ImGui.SameLine();
                ImGui.Image(_lastNextWeather!.ImGuiHandle, _iconSize);
            }
            else
            {
                ImGui.SameLine();
                ImGui.Dummy(_iconSize);
                ImGui.SameLine();
                ImGui.Button(nextWeatherString, size);
                ImGui.SameLine();
                ImGui.Dummy(_iconSize);
            }
            ImGui.PopStyleColor(3);

            ImGui.Dummy(new Vector2(0, _horizontalSpace / 2));
        }
    }
}
