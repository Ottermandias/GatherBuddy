using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dalamud.Game;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using GatherBuddy.SeFunctions;
using GatherBuddy.Time;
using ImGuiOtter.Table;
using Newtonsoft.Json;

namespace GatherBuddy.Alarms;

public partial class AlarmManager : IDisposable
{
    private const    string    FileName = "alarms.json";
    private readonly PlaySound _sounds;

    public   List<AlarmGroup>                  Alarms        { get; init; } = new();
    internal List<(Alarm, TimeStamp)>          ActiveAlarms  { get; init; } = new();
    public   (Alarm, ILocation, TimeInterval)? LastItemAlarm { get; private set; }
    public   (Alarm, ILocation, TimeInterval)? LastFishAlarm { get; private set; }

    internal bool Dirty = true;

    public event Action? ActiveAlarmsChanged;

    public void SortActiveAlarms()
    {
        if (!Dirty)
            return;

        ActiveAlarms.StableSort((a, b) => a.Item2.CompareTo(b.Item2));
        Dirty = false;
    }

    public void AddActiveAlarm(Alarm alarm, bool trigger = true)
    {
        var idx = ActiveAlarms.FindIndex(a => ReferenceEquals(a.Item1, alarm));
        if (idx >= 0)
            return;

        var (_, uptime) = GatherBuddy.UptimeManager.BestLocation(alarm.Item);
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
        => _sounds = new PlaySound(Dalamud.SigScanner);

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

    private void OnLogin(object? _, EventArgs _2)
        => SetActiveAlarms();

    public static (ILocation, TimeInterval) GetUptime(Alarm alarm)
    {
        if (alarm.PreferLocation == null)
            return GatherBuddy.UptimeManager.BestLocation(alarm.Item);

        return alarm.PreferLocation switch
        {
            GatheringNode node => (node, node.Times.NextUptime(GatherBuddy.Time.ServerTime)),
            FishingSpot spot   => (spot, GatherBuddy.UptimeManager.NextUptime((Fish)alarm.Item, spot.Territory, GatherBuddy.Time.ServerTime)),
            _                  => (alarm.PreferLocation, TimeInterval.Never),
        };
    }

    public void OnUpdate(Framework _)
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
            _sounds.Play(alarm.SoundId);

        // Some lax rounding for display.
        var newUptime = uptime;
        uptime = uptime.Extend(500);
        alarm.SendMessage(location, uptime);

        var newStart  = TimeStamp.MinValue;
        while (newStart <= st)
        {
            (var _, newUptime) = GatherBuddy.UptimeManager.NextUptime(alarm.Item, newUptime.End + 1);
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
            PluginLog.Error($"Could not write gather groups to file {file.FullName}:\n{e}");
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
            var data    = JsonConvert.DeserializeObject<AlarmGroup.Config[]>(text);
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
                        PluginLog.Error($"Could not add alarm to {@group.Name}.");
                        changes = true;
                        continue;
                    }

                    group.Alarms.Add(alarm!);
                }

                manager.Alarms.Add(group);
            }

            if (changes)
                manager.Save();
        }
        catch (Exception e)
        {
            PluginLog.Error($"Error loading gather groups:\n{e}");
            manager.Save();
        }

        return manager;
    }
}
