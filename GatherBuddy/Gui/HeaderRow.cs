using System.Numerics;
using GatherBuddy.Classes;
using ImGuiNET;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private void DrawAlarmToggle()
        => DrawCheckbox("Alarms",             "Toggle all alarms on or off.",
            GatherBuddy.Config.AlarmsEnabled, b =>
            {
                if (b)
                    _plugin.Alarms!.Enable();
                else
                    _plugin.Alarms!.Disable();
                GatherBuddy.Config.AlarmsEnabled = b;
            });

    private void DrawHeaderRow()
    {
        var spacing = 5 * _horizontalSpace;
        using (var _ = ImGuiRaii.NewGroup())
        {
            DrawAlarmToggle();
        }

        ImGui.Dummy(new Vector2(0, _horizontalSpace / 2));
    }

    private static void DrawButtonText(string text, Vector2 size, uint color)
    {
        using var imgui = new ImGuiRaii()
            .PushColor(ImGuiCol.Button,        color)
            .PushColor(ImGuiCol.ButtonHovered, color)
            .PushColor(ImGuiCol.ButtonActive,  color);

        ImGui.Button(text, size);
    }

    private static void DrawEorzeaTime(string time)
        => DrawButtonText(time, new Vector2(0, _weatherIconSize.Y), Colors.HeaderRow.EorzeaTime);

    private static void DrawNextEorzeaHour(string hour, Vector2 size)
        => DrawButtonText(hour, size, Colors.HeaderRow.NextEorzeaHour);

    private void DrawNextWeather(string nextWeather)
    {
        if (_headerCache.Territory != 0)
        {
            ImGui.SameLine();
            ImGui.Image(_headerCache.CurrentWeather!.ImGuiHandle, _weatherIconSize);
            ImGui.SameLine();
            DrawButtonText(nextWeather, new Vector2(0, _weatherIconSize.Y), Colors.HeaderRow.Weather);
            ImGui.SameLine();
            ImGui.Image(_headerCache.NextWeather!.ImGuiHandle, _weatherIconSize);
        }
        else
        {
            ImGui.SameLine();
            ImGui.Dummy(_weatherIconSize);
            ImGui.SameLine();
            DrawButtonText(nextWeather, new Vector2(0, _weatherIconSize.Y), Colors.HeaderRow.Weather);
            ImGui.SameLine();
            ImGui.Dummy(_weatherIconSize);
        }
    }

    private void DrawTimeRow(long eorzeaHour, long eorzeaMinute)
    {
        var now          = TimeStamp.UtcNow;
        var minuteOfHour = eorzeaMinute % RealTime.MinutesPerHour;
        var hourOfDay    = eorzeaHour % RealTime.HoursPerDay;

        var nextHourS = (now.SyncToEorzeaHour().AddEorzeaHours(1) - now) / RealTime.MillisecondsPerSecond;
        var nextHourM = nextHourS / RealTime.SecondsPerMinute;

        var nextWeatherS = (now.SyncToEorzeaWeather().AddEorzeaHours(8) - now) / RealTime.MillisecondsPerSecond;
        var nextWeatherM = nextWeatherS / RealTime.SecondsPerMinute;

        nextHourS    -= nextHourM * RealTime.SecondsPerMinute;
        nextWeatherS -= nextWeatherM * RealTime.SecondsPerMinute;

        var nextWeatherString = $"  {nextWeatherM:D2}.{nextWeatherS:D2} Min.  ";
        var width = -(ImGui.CalcTextSize(nextWeatherString).X
          + (_weatherIconSize.X + _itemSpacing.X * 1.5f + ImGui.GetStyle().FramePadding.X) * 2);

        var territory = Dalamud.ClientState.TerritoryType;
        _headerCache.Update(eorzeaHour, territory);

        DrawEorzeaTime($"ET {hourOfDay:D2}:{minuteOfHour:D2}");
        ImGui.SameLine();
        DrawNextEorzeaHour($"{nextHourM:D2}.{nextHourS:D2} Min to next Eorzea hour.", new Vector2(width, _weatherIconSize.Y));
        ImGui.SameLine();
        DrawNextWeather(nextWeatherString);

        ImGui.Dummy(new Vector2(0, _horizontalSpace / 2));
    }
}
