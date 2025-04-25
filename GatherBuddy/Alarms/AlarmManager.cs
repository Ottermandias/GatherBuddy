using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dalamud.Interface.ImGuiNotification;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI;
using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using GatherBuddy.Time;
using Newtonsoft.Json;
using OtterGui.Extensions;
using Functions = GatherBuddy.Plugin.Functions;

namespace GatherBuddy.Alarms;

public partial class AlarmManager : IDisposable
{
    private const string FileName = "alarms.json";

    public   List<AlarmGroup>                  Alarms        { get; init; } = [];
    internal List<(Alarm, TimeStamp)>          ActiveAlarms  { get; init; } = [];
    public   (Alarm, ILocation, TimeInterval)? LastItemAlarm { get; private set; }
    public   (Alarm, ILocation, TimeInterval)? LastFishAlarm { get; private set; }

    internal bool Dirty = true;

    public event Action? ActiveAlarmsChanged;

    private void SortActiveAlarms()
    {
        if (!Dirty)
            return;

        ActiveAlarms.StableSort((a, b) => a.Item2.CompareTo(b.Item2));
        Dirty = false;
    }

    public void SetWeatherAlarm(Sounds sound)
    {
        if (GatherBuddy.Config.WeatherAlarm == sound)
            return;


        if (GatherBuddy.Config.WeatherAlarm == Sounds.None)
            GatherBuddy.Time.WeatherChanged += TriggerWeatherAlarm;
        else if (sound == Sounds.None)
            GatherBuddy.Time.WeatherChanged -= TriggerWeatherAlarm;
        GatherBuddy.Config.WeatherAlarm = sound;
        GatherBuddy.Config.Save();
    }

    public void SetHourAlarm(Sounds sound)
    {
        if (GatherBuddy.Config.HourAlarm == sound)
            return;

        if (GatherBuddy.Config.HourAlarm == Sounds.None)
            GatherBuddy.Time.HourChanged += TriggerHourAlarm;
        else if (sound == Sounds.None)
            GatherBuddy.Time.HourChanged -= TriggerHourAlarm;
        GatherBuddy.Config.HourAlarm = sound;
        GatherBuddy.Config.Save();
    }

    private static void TriggerWeatherAlarm()
        => UIGlobals.PlaySoundEffect((uint)GatherBuddy.Config.WeatherAlarm);

    private static void TriggerHourAlarm()
        => UIGlobals.PlaySoundEffect((uint)GatherBuddy.Config.HourAlarm);

    public static void PreviewAlarm(Sounds id)
        => UIGlobals.PlaySoundEffect((uint)id);


    public void AddActiveAlarm(Alarm alarm, bool trigger = true)
    {
        var idx = ActiveAlarms.FindIndex(a => ReferenceEquals(a.Item1, alarm));
        if (idx >= 0)
            return;

        var (_, uptime) = GetUptime(alarm);
        var start = uptime.Start.AddSeconds(-alarm.SecondOffset);
        ActiveAlarms.Add((alarm, start));
        Dirty = true;
        if (trigger)
            ActiveAlarmsChanged?.Invoke();
    }

    public void RemoveActiveAlarm(Alarm alarm)
    {
        if (ReferenceEquals(alarm, LastFishAlarm?.Item1))
            LastFishAlarm = null;
        if (ReferenceEquals(alarm, LastItemAlarm?.Item1))
            LastItemAlarm = null;

        var idx = ActiveAlarms.FindIndex(a => ReferenceEquals(a.Item1, alarm));
        if (idx < 0)
            return;

        ActiveAlarms.RemoveAt(idx);
        ActiveAlarmsChanged?.Invoke();
    }

    public AlarmManager()
    {
        if (GatherBuddy.Config.WeatherAlarm != Sounds.None)
            GatherBuddy.Time.WeatherChanged += TriggerWeatherAlarm;
        if (GatherBuddy.Config.HourAlarm != Sounds.None)
            GatherBuddy.Time.HourChanged += TriggerHourAlarm;
    }

    public void Enable()
    {
        if (GatherBuddy.Config.AlarmsEnabled)
            return;

        GatherBuddy.Config.AlarmsEnabled = true;
        GatherBuddy.Config.Save();

        SetActiveAlarms();
        Dalamud.Framework.Update  += OnUpdate;
        Dalamud.ClientState.Login += OnLogin;
    }

    internal void ForceEnable()
    {
        if (GatherBuddy.Config.AlarmsEnabled)
        {
            SetActiveAlarms();
            Dalamud.ClientState.Login -= OnLogin;
            Dalamud.Framework.Update  += OnUpdate;
        }
    }

    public void Disable()
    {
        if (!GatherBuddy.Config.AlarmsEnabled)
            return;

        GatherBuddy.Config.AlarmsEnabled = false;
        GatherBuddy.Config.Save();
        Dalamud.Framework.Update -= OnUpdate;
        SetActiveAlarms();
        LastItemAlarm = null;
        LastFishAlarm = null;
    }

    public void SetDirty()
        => Dirty = true;

    public void Dispose()
        => Disable();

    private void SetActiveAlarms()
    {
        ActiveAlarms.Clear();
        if (!GatherBuddy.Config.AlarmsEnabled)
        {
            ActiveAlarmsChanged?.Invoke();
            return;
        }

        foreach (var alarm in Alarms.Where(g => g.Enabled)
                     .SelectMany(g => g.Alarms)
                     .Where(a => a.Enabled))
            AddActiveAlarm(alarm);
        ActiveAlarmsChanged?.Invoke();
    }

    private void OnLogin()
        => SetActiveAlarms();

    public static (ILocation, TimeInterval) GetUptime(Alarm alarm)
    {
        if (alarm.PreferLocation == null)
            return GatherBuddy.UptimeManager.BestLocation(alarm.Item);

        return alarm.PreferLocation switch
        {
            GatheringNode node => (node, node.Times.NextUptime(GatherBuddy.Time.ServerTime)),
            FishingSpot spot when alarm.Item is Fish fish => (spot,
                GatherBuddy.UptimeManager.NextUptime(fish, spot.Territory, GatherBuddy.Time.ServerTime)),
            _ => (alarm.PreferLocation, TimeInterval.Never),
        };
    }

    public static (ILocation?, TimeInterval) GetUptime(Alarm alarm, TimeStamp now)
    {
        if (alarm.PreferLocation == null)
            return GatherBuddy.UptimeManager.NextUptime(alarm.Item, now);

        return alarm.PreferLocation switch
        {
            GatheringNode node                            => (node, node.Times.NextUptime(now)),
            FishingSpot spot when alarm.Item is Fish fish => (spot, GatherBuddy.UptimeManager.NextUptime(fish, spot.Territory, now)),
            _                                             => (alarm.PreferLocation, TimeInterval.Never),
        };
    }

    public void OnUpdate(IFramework _)
    {
        var st = GatherBuddy.Time.ServerTime;
        if (LastFishAlarm != null && LastFishAlarm.Value.Item3.End < st)
            LastFishAlarm = null;
        if (LastItemAlarm != null && LastItemAlarm.Value.Item3.End < st)
            LastItemAlarm = null;

        if (ActiveAlarms.Count == 0)
            return;

        if (Functions.BetweenAreas())
            return;

        if (GatherBuddy.Config.AlarmsOnlyWhenLoggedIn && Dalamud.ClientState.LocalPlayer == null)
            return;

        if (!GatherBuddy.Config.AlarmsInDuty && Functions.BoundByDuty())
            return;

        SortActiveAlarms();
        var (alarm, timeStamp) = ActiveAlarms[0];

        if (timeStamp >= st)
            return;

        var (location, uptime) = GetUptime(alarm);

        if (alarm.Item.Type == ObjectType.Fish)
            LastFishAlarm = (alarm, location, uptime);
        else if (alarm.Item.Type == ObjectType.Gatherable)
            LastItemAlarm = (alarm, location, uptime);

        if (alarm.SoundId > Sounds.Unknown)
            UIGlobals.PlaySoundEffect((uint)alarm.SoundId);

        // Some lax rounding for display.
        var newUptime = uptime;
        uptime = uptime.Extend(500);
        alarm.SendMessage(location, uptime);

        var newStart = TimeStamp.MinValue;
        while (newStart <= st)
        {
            (var _, newUptime) = GetUptime(alarm, newUptime.End + 1);
            newStart           = newUptime.Start.AddSeconds(-alarm.SecondOffset);
        }

        ActiveAlarms[0] = (alarm, newStart);
        Dirty           = true;
    }

    public void Save()
    {
        var file = Functions.ObtainSaveFile(FileName);
        if (file == null)
            return;

        try
        {
            var text = JsonConvert.SerializeObject(Alarms.Select(a => new AlarmGroup.Config(a)), Formatting.Indented);
            File.WriteAllText(file.FullName, text);
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not write gather groups to file {file.FullName}:\n{e}");
        }
    }

    public static AlarmManager Load()
    {
        var manager = new AlarmManager();
        var file    = Functions.ObtainSaveFile(FileName);
        if (file is not { Exists: true })
        {
            manager.Save();
            return manager;
        }

        try
        {
            var text    = File.ReadAllText(file.FullName);
            var data    = JsonConvert.DeserializeObject<AlarmGroup.Config[]>(text)!;
            var changes = false;
            foreach (var alarmGroup in data)
            {
                var group = new AlarmGroup()
                {
                    Name        = alarmGroup.Name,
                    Description = alarmGroup.Description,
                    Enabled     = alarmGroup.Enabled,
                };
                foreach (var item in alarmGroup.Alarms)
                {
                    if (!Alarm.FromConfig(item, out var alarm))
                    {
                        GatherBuddy.Log.Error($"Could not add alarm to {group.Name}.");
                        changes = true;
                        continue;
                    }

                    group.Alarms.Add(alarm!);
                }

                manager.Alarms.Add(group);
            }

            if (changes)
            {
                Dalamud.Notifications.AddNotification(new Notification()
                {
                    Title = "GatherBuddy Error",
                    Content =
                        "Failed to load some Alarm groups. See the plugin log for more details. This is not saved, if it keeps happening you need to manually change an Alarm Group to cause a save.",
                    MinimizedText = "Failed to load Alarm groups.",
                    Type          = NotificationType.Error,
                });
            }
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Error loading gather groups:\n{e}");
            manager.Save();
        }

        return manager;
    }
}
