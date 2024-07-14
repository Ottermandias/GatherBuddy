using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Textures;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.Plugin;
using GatherBuddy.Time;
using ImGuiNET;
using OtterGui;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private class HeaderCache : IDisposable
    {
        public readonly Vector4 LastWeatherTint = new(1f, 0.5f, 0.5f, 1f);

        private uint                     _currentTerritory;
        public  Structs.Weather          LastWeather    = Structs.Weather.Invalid;
        public  Structs.Weather          CurrentWeather = Structs.Weather.Invalid;
        public  Structs.Weather          NextWeather    = Structs.Weather.Invalid;
        public  ISharedImmediateTexture? LastWeatherIcon;
        public  ISharedImmediateTexture? CurrentWeatherIcon;
        public  ISharedImmediateTexture? NextWeatherIcon;
        public  Vector2                  AlarmButtonSize = Vector2.Zero;

        private void NullWeather()
        {
            LastWeatherIcon    = null;
            CurrentWeatherIcon = null;
            NextWeatherIcon    = null;
            LastWeather        = Structs.Weather.Invalid;
            CurrentWeather     = Structs.Weather.Invalid;
            NextWeather        = Structs.Weather.Invalid;
        }

        private void UpdateWeather()
        {
            if (_currentTerritory == 0)
            {
                NullWeather();
            }
            else
            {
                (LastWeather, CurrentWeather, NextWeather) = GatherBuddy.WeatherManager.FindLastCurrentNextWeather(_currentTerritory);
                if (LastWeather.Id != 0)
                {
                    LastWeatherIcon    = Icons.DefaultStorage.TextureProvider.GetFromGameIcon(LastWeather.Data.Icon);
                    CurrentWeatherIcon = Icons.DefaultStorage.TextureProvider.GetFromGameIcon(CurrentWeather.Data.Icon);
                    NextWeatherIcon    = Icons.DefaultStorage.TextureProvider.GetFromGameIcon(NextWeather.Data.Icon);
                }
                else
                {
                    NullWeather();
                }
            }
        }

        public HeaderCache()
            => GatherBuddy.Time.WeatherChanged += UpdateWeather;

        public void Dispose()
            => GatherBuddy.Time.WeatherChanged -= UpdateWeather;

        public void UpdateCurrentTerritory()
        {
            if (_currentTerritory == Dalamud.ClientState.TerritoryType)
                return;

            _currentTerritory = Dalamud.ClientState.TerritoryType;
            UpdateWeather();
        }
    }

    private readonly HeaderCache _headerCache = new();

    private void DrawLastAlarm(bool which, string failureText)
    {
        var alarmData = which ? _plugin.AlarmManager.LastItemAlarm : _plugin.AlarmManager.LastFishAlarm;
        if (alarmData == null)
        {
            ImGuiUtil.DrawDisabledButton(failureText, _headerCache.AlarmButtonSize, "Click to /gather this alarm.", true);
            return;
        }

        var (alarm, loc, time) = alarmData.Value;

        var text = $"{(alarm.Name.Any() ? alarm.Name : alarm.Item.Name[GatherBuddy.Language])}###{(which ? "itemAlarm" : "fishAlarm")}";
        var desc =
            $"Click to /gather this alarm.\n{loc.Name} - {loc.ClosestAetheryte?.Name ?? "None"}\n{time.Start.LocalTime}\n{time.End.LocalTime}";

        if (!ImGuiUtil.DrawDisabledButton(text, _headerCache.AlarmButtonSize, desc, false))
            return;

        if (which)
            _plugin.Executor.GatherItemByName("alarm");
        else
            _plugin.Executor.GatherFishByName("alarm");
    }

    private void DrawLastItemAlarm()
        => DrawLastAlarm(true, "No Item Alarm Triggered");

    private void DrawLastFishAlarm()
        => DrawLastAlarm(false, "No Fish Alarm Triggered");


    private void DrawAlarmRow()
    {
        using var _ = ImRaii.Group();
        ImGui.SameLine();
        ConfigFunctions.DrawAlarmToggle();
        ImGui.SameLine();
        _headerCache.AlarmButtonSize = (ImGui.GetContentRegionAvail().X - ItemSpacing.X) / 2 * Vector2.UnitX;
        DrawLastItemAlarm();
        ImGui.SameLine();
        DrawLastFishAlarm();
    }

    private static void DrawEorzeaTime(string time)
    {
        ImGuiUtil.DrawTextButton(time, Vector2.UnitY * WeatherIconSize.Y, ColorId.HeaderEorzeaTime.Value());
        if (ImGui.IsItemHovered())
        {
            using var tt = ImRaii.Tooltip();
            ImGui.TextUnformatted("If this does not correspond to your in-game Eorzea Time, verify that your windows system time is accurate.");
            ImGui.TextUnformatted($"Next Aldenard Ocean Route: {OceanUptime.NextOceanRoute(OceanArea.Aldenard, TimeStamp.UtcNow)}");
            ImGui.TextUnformatted($"Next Othard Ocean Route: {OceanUptime.NextOceanRoute(OceanArea.Othard,     TimeStamp.UtcNow)}");
        }
    }

    private static void DrawNextEorzeaHour(string hour, Vector2 size)
        => ImGuiUtil.DrawTextButton(hour, size, ColorId.HeaderNextHour.Value());

    private static void DrawIconTint(Structs.Weather weather, ISharedImmediateTexture? icon, Vector2 size, Vector4 tint)
    {
        if (icon != null && icon.TryGetWrap(out var wrap, out _))
        {
            ImGui.Image(wrap.ImGuiHandle, size, Vector2.Zero, Vector2.One, tint);
            ImGuiUtil.HoverTooltip($"{weather.Name} ({weather.Id})");
        }
        else
        {
            ImGui.Dummy(size);
        }
    }

    private static void DrawIcon(Structs.Weather weather, ISharedImmediateTexture? wrap, Vector2 size)
        => DrawIconTint(weather, wrap, size, Vector4.One);

    private void DrawNextWeather(string nextWeather)
    {
        using var style = ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, Vector2.Zero);
        DrawIconTint(_headerCache.LastWeather, _headerCache.LastWeatherIcon, WeatherIconSize, _headerCache.LastWeatherTint);
        ImGui.SameLine();
        DrawIcon(_headerCache.CurrentWeather, _headerCache.CurrentWeatherIcon, WeatherIconSize);
        style.Pop();
        ImGui.SameLine();
        ImGuiUtil.DrawTextButton(nextWeather, Vector2.UnitY * WeatherIconSize.Y, ColorId.HeaderWeather.Value());
        ImGui.SameLine();
        DrawIcon(_headerCache.NextWeather, _headerCache.NextWeatherIcon, WeatherIconSize);
    }

    private void DrawTimeRow()
    {
        var now       = GatherBuddy.Time.ServerTime;
        var nextHourS = (now.SyncToEorzeaHour().AddEorzeaHours(1) - GatherBuddy.Time.ServerTime) / RealTime.MillisecondsPerSecond;
        var nextHourM = nextHourS / RealTime.SecondsPerMinute;

        var nextWeatherS = (now.SyncToEorzeaWeather().AddEorzeaHours(8) - GatherBuddy.Time.ServerTime) / RealTime.MillisecondsPerSecond;
        var nextWeatherM = nextWeatherS / RealTime.SecondsPerMinute;

        nextHourS    -= nextHourM * RealTime.SecondsPerMinute;
        nextWeatherS -= nextWeatherM * RealTime.SecondsPerMinute;

        var nextWeatherString = $"  {nextWeatherM:D2}:{nextWeatherS:D2} Min.  ";
        var width = -(ImGui.CalcTextSize(nextWeatherString).X
          + (WeatherIconSize.X + ItemSpacing.X + FramePadding.X) * 3);

        _headerCache.UpdateCurrentTerritory();
        using var _ = ImRaii.Group();
        DrawEorzeaTime($"ET {GatherBuddy.Time.EorzeaHourOfDay:D2}:{GatherBuddy.Time.EorzeaMinuteOfHour:D2}");
        ImGui.SameLine();
        DrawNextEorzeaHour($"{nextHourM:D2}:{nextHourS:D2} Min to next hour.", new Vector2(width, WeatherIconSize.Y));
        ImGui.SameLine();
        DrawNextWeather(nextWeatherString);
    }

    private void DrawHeader()
    {
        DrawAlarmRow();
        ImGui.Dummy(HorizontalSpace);
        DrawTimeRow();
        ImGui.Dummy(HorizontalSpace);
    }
}
