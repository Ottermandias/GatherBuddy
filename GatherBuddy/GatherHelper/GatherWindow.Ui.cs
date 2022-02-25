using System.Numerics;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Interface;
using GatherBuddy.Alarms;
using GatherBuddy.Config;
using GatherBuddy.Gui;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using GatherBuddy.Time;
using ImGuiNET;
using ImGuiOtter;

namespace GatherBuddy.GatherHelper;

public class GatherWindow
{
    private readonly GatherBuddy _plugin;

    private int       _deleteSet              = -1;
    private int       _deleteItemIdx          = -1;
    private TimeStamp _earliestKeyboardToggle = TimeStamp.Epoch;

    public GatherWindow(GatherBuddy plugin)
        => _plugin = plugin;

    private static void DrawTime(ILocation? loc, TimeInterval time)
    {
        if (!GatherBuddy.Config.ShowGatherWindowTimers || !ImGui.TableNextColumn())
            return;
        if (time.Equals(TimeInterval.Always))
            return;

        if (time.Start > GatherBuddy.Time.ServerTime)
        {
            using var color = ImGuiRaii.PushColor(ImGuiCol.Text, ColorId.UpcomingItem.Value());
            ImGui.Text(TimeInterval.DurationString(time.Start, GatherBuddy.Time.ServerTime, false));
        }
        else
        {
            using var color = ImGuiRaii.PushColor(ImGuiCol.Text, ColorId.AvailableItem.Value());
            ImGui.Text(TimeInterval.DurationString(time.End, GatherBuddy.Time.ServerTime, false));
        }

        CreateTooltip(loc, time);
    }

    private static void CreateTooltip(ILocation? loc, TimeInterval time)
    {
        if (!ImGui.IsItemHovered())
            return;

        var s = string.Empty;
        if (loc == null)
            s += "Unknown Location\nUnknown Territory\nUnknown Aetheryte\n";
        else
            s += $"{loc.Name}\n{loc.Territory.Name}\n{loc.ClosestAetheryte?.Name ?? "No Aetheryte"}\n";

        if (time.Equals(TimeInterval.Always))
            s += "Always Up";
        else
            s +=
                $"{time.Start}\n{time.End}\n{time.DurationString()}\n{TimeInterval.DurationString(time.Start > GatherBuddy.Time.ServerTime ? time.Start : time.End, GatherBuddy.Time.ServerTime, false)}";

        ImGui.SetTooltip(s);
    }

    private void DrawAlarm((Alarm, ILocation, TimeInterval)? alarmGroup)
    {
        if (alarmGroup == null)
            return;

        var (alarm, location, time) = alarmGroup.Value;
        if (time.End < GatherBuddy.Time.ServerTime)
            return;

        if (ImGui.TableNextColumn())
        {
            var name = alarm.Name.Length == 0
                ? $"Alarm: {alarm.Item.Name[GatherBuddy.Language]}"
                : $"{alarm.Name}: {alarm.Item.Name[GatherBuddy.Language]}";
            using var style = ImGuiRaii.PushStyle(ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemSpacing / 2);
            ImGuiUtil.HoverIcon(Icons.DefaultStorage[alarm.Item.ItemData.Icon], Vector2.One * ImGui.GetTextLineHeight());
            ImGui.SameLine();
            using var color = ImGuiRaii.PushColor(ImGuiCol.Text, ColorId.GatherWindowAvailable.Value());
            if (ImGui.Selectable(name, false))
                _plugin.Executor.GatherLocation(location);
            color.Pop();
            style.Pop();
            CreateTooltip(location, time);
        }

        DrawTime(location, time);
    }

    private void DrawItem(IGatherable item)
    {
        var (loc, time) = GatherBuddy.UptimeManager.BestLocation(item);
        if (GatherBuddy.Config.ShowGatherWindowOnlyAvailable && time.Start > GatherBuddy.Time.ServerTime)
            return;

        if (ImGui.TableNextColumn())
        {
            using var style = ImGuiRaii.PushStyle(ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemSpacing / 2);
            ImGuiUtil.HoverIcon(Icons.DefaultStorage[item.ItemData.Icon], Vector2.One * ImGui.GetTextLineHeight());
            ImGui.SameLine();
            var colorId = time == TimeInterval.Always    ? ColorId.GatherWindowText :
                time.Start > GatherBuddy.Time.ServerTime ? ColorId.GatherWindowUpcoming : ColorId.GatherWindowAvailable;
            using var color = ImGuiRaii.PushColor(ImGuiCol.Text, colorId.Value());
            if (ImGui.Selectable(item.Name[GatherBuddy.Language], false))
                _plugin.Executor.GatherItem(item);
            var clicked = ImGui.IsItemClicked(ImGuiMouseButton.Right);
            color.Pop();
            CreateTooltip(loc, time);

            if (clicked && Functions.CheckModifier(GatherBuddy.Config.GatherWindowDeleteModifier, false))
                for (var i = 0; i < _plugin.GatherWindowManager.Presets.Count; ++i)
                {
                    var preset = _plugin.GatherWindowManager.Presets[i];
                    if (!preset.Enabled)
                        continue;

                    var idx = preset.Items.IndexOf(item);
                    if (idx < 0)
                        continue;

                    _deleteSet     = i;
                    _deleteItemIdx = idx;
                    break;
                }
        }

        DrawTime(loc, time);
    }

    private void DrawAlarms()
    {
        if (!GatherBuddy.Config.ShowGatherWindowAlarms)
            return;

        DrawAlarm(_plugin.AlarmManager.LastItemAlarm);
        DrawAlarm(_plugin.AlarmManager.LastFishAlarm);
    }

    private void DeleteItem()
    {
        if (_deleteSet < 0 || _deleteItemIdx < 0)
            return;

        var preset = _plugin.GatherWindowManager.Presets[_deleteSet];
        _plugin.GatherWindowManager.RemoveItem(preset, _deleteItemIdx);
        if (preset.Items.Count == 0)
            _plugin.GatherWindowManager.DeletePreset(_deleteSet);
        _deleteSet     = -1;
        _deleteItemIdx = -1;
    }

    private bool CheckHotkeys()
    {
        if (_earliestKeyboardToggle > GatherBuddy.Time.ServerTime || !Functions.CheckKeyState(GatherBuddy.Config.GatherWindowHotkey, false))
            return !GatherBuddy.Config.ShowGatherWindow;

        _earliestKeyboardToggle             = GatherBuddy.Time.ServerTime.AddMilliseconds(500);
        GatherBuddy.Config.ShowGatherWindow = !GatherBuddy.Config.ShowGatherWindow;
        GatherBuddy.Config.Save();

        return !GatherBuddy.Config.ShowGatherWindow;
    }

    private static bool CheckHoldKey()
    {
        if (!GatherBuddy.Config.OnlyShowGatherWindowHoldingKey || GatherBuddy.Config.GatherWindowHoldKey == VirtualKey.NO_KEY)
            return false;

        return !Dalamud.Keys[GatherBuddy.Config.GatherWindowHoldKey];
    }

    private static bool CheckDuty()
        => GatherBuddy.Config.HideGatherWindowInDuty && Functions.BoundByDuty();

    public void Draw()
    {
        if (CheckHotkeys() || CheckHoldKey() || CheckDuty())
            return;

        var alarmTimers = GatherBuddy.Config.ShowGatherWindowAlarms
         && (_plugin.AlarmManager.LastFishAlarm != null || _plugin.AlarmManager.LastItemAlarm != null);

        if (_plugin.GatherWindowManager.ActiveItems.Count == 0 && !alarmTimers)
            return;

        using var color = ImGuiRaii.PushColor(ImGuiCol.WindowBg, ColorId.GatherWindowBackground.Value());
        using var style = ImGuiRaii.PushStyle(ImGuiStyleVar.WindowPadding, Vector2.One * 2 * ImGuiHelpers.GlobalScale);
        ImGui.SetNextWindowSizeConstraints(Vector2.Zero, Vector2.One * 10000);
        var flags = ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoTitleBar;
        if (GatherBuddy.Config.LockGatherWindow)
            flags |= ImGuiWindowFlags.NoMove;

        if (!ImGui.Begin("##GatherHelper", flags))
        {
            ImGui.End();
            return;
        }

        using var end = ImGuiRaii.DeferredEnd(ImGui.End);
        if (!ImGui.BeginTable("##table", GatherBuddy.Config.ShowGatherWindowTimers ? 2 : 1))
            return;

        end.Push(ImGui.EndTable);

        DrawAlarms();
        foreach (var item in _plugin.GatherWindowManager.GetList())
            DrawItem(item);

        DeleteItem();
    }
}
