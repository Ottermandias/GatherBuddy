using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.Nodes;
using GatherBuddy.SeFunctions;
using GatherBuddy.Utility;

namespace GatherBuddy.Managers;

public class AlarmManager : IDisposable
{
    private readonly NodeManager    _nodes;
    private readonly FishManager    _fish;
    private readonly WeatherManager _weather;
    private readonly PlaySound      _sounds;
    public           Node[]         AllTimedNodes { get; }
    public           Fish[]         AllTimedFish  { get; }

    private int       _currentMinute;
    private TimeStamp _currentTimestamp;

    public List<Alarm> Alarms
        => GatherBuddy.Config.Alarms;

    private readonly List<bool> _status;

    public Alarm? LastNodeAlarm { get; set; }
    public Alarm? LastFishAlarm { get; set; }

    public AlarmManager(NodeManager nodes, FishManager fish, WeatherManager weather)
    {
        _nodes   = nodes;
        _fish    = fish;
        _weather = weather;
        _sounds  = new PlaySound(Dalamud.SigScanner);
        _status  = Enumerable.Repeat(false, Alarms.Count).ToList();
        UpdateNodes();
        AllTimedNodes = nodes.BaseNodes().Where(n => !n.Times!.AlwaysUp()).ToArray();
        AllTimedFish  = fish.Fish.Values.Where(f => f.FishRestrictions != FishRestrictions.None).ToArray();
    }

    public void Dispose()
    {
        if (GatherBuddy.Config.AlarmsEnabled)
            Dalamud.Framework.Update -= OnUpdate;
    }

    public void Enable(bool force = false)
    {
        if (!force && GatherBuddy.Config.AlarmsEnabled)
            return;

        UpdateNodes();
        GatherBuddy.Config.AlarmsEnabled =  true;
        Dalamud.Framework.Update         += OnUpdate;
        GatherBuddy.Config.Save();
    }

    public void Disable()
    {
        if (!GatherBuddy.Config.AlarmsEnabled)
            return;

        GatherBuddy.Config.AlarmsEnabled =  false;
        Dalamud.Framework.Update         -= OnUpdate;
        GatherBuddy.Config.Save();
    }

    private bool NewStatusNode(Alarm nodeAlarm)
    {
        var hour      = (_currentMinute + nodeAlarm.MinuteOffset) / RealTime.MinutesPerHour;
        var hourOfDay = (uint)hour % RealTime.HoursPerDay;
        return nodeAlarm.Node!.Times!.IsUp(hourOfDay);
    }

    private bool NewStatusFish(Alarm fishAlarm)
    {
        var uptime = fishAlarm.Fish!.NextUptime(_weather);
        return uptime.Start - _currentTimestamp < fishAlarm.MinuteOffset * RealTime.SecondsPerMinute;
    }

    public void OnUpdate(object framework)
    {
        // Skip if the player isn't loaded in a territory.
        if (Dalamud.ClientState.TerritoryType == 0)
            return;

        var minute = TimeStamp.UtcNow.CurrentEorzeaMinuteOfDay();
        if (minute == _currentMinute)
            return;

        _currentMinute    = minute;
        _currentTimestamp = TimeStamp.UtcNow;
        for (var i = 0; i < Alarms.Count; ++i)
        {
            var alarm = Alarms[i];
            if (!alarm.Enabled)
                continue;

            var newStatus = alarm.Type switch
            {
                AlarmType.Node => NewStatusNode(alarm),
                AlarmType.Fish => NewStatusFish(alarm),
                _              => false,
            };

            if (_status[i] == newStatus)
                continue;

            _status[i] = newStatus;
            if (newStatus)
                Ring(alarm, (uint)_currentMinute);
        }
    }

    private static SeString ReplaceNodeFormatPlaceholders(string format, Alarm alarm, uint currentMinute)
    {
        IReadOnlyList<Payload>? Replace(string s)
        {
            if (!(s.StartsWith('{') && s.EndsWith('}')))
                return null;

            return s switch
            {
                "{Name}"        => ChatUtil.GetPayloadsFromString(alarm.Name),
                "{Offset}"      => ChatUtil.GetPayloadsFromString(alarm.MinuteOffset.ToString()),
                "{DelayString}" => ChatUtil.GetPayloadsFromString(DelayStringNode(alarm, currentMinute)),
                "{TimesShort}"  => ChatUtil.GetPayloadsFromString(alarm.Node!.Times!.PrintHours(true)),
                "{TimesLong}"   => ChatUtil.GetPayloadsFromString(alarm.Node!.Times!.PrintHours()),
                "{Location}"    => ChatUtil.CreateNodeLink(alarm.Node!).Payloads,
                "{AllItems}" => alarm.Node!.Items!.ActualItems
                    .SelectMany(i => ChatUtil.CreateLink(i.ItemData).Prepend(new TextPayload(", "))).Skip(1).ToList(),
                _ => null,
            };
        }

        return ChatUtil.Format(format, Replace);
    }

    private string DelayStringFish(Alarm alarm)
    {
        var uptime = alarm.Fish!.NextUptime(_weather);
        var ret    = "is currently up";
        if (uptime.Start <= _currentTimestamp)
            return ret;

        var diff = uptime.Start - _currentTimestamp;
        ret = $"will be up in {diff / RealTime.SecondsPerMinute}:{diff % RealTime.SecondsPerMinute:D2} minutes";

        return ret;
    }

    private static string DelayStringNode(Alarm alarm, uint currentMinute)
    {
        var ret = "is currently up";
        if (alarm.MinuteOffset <= 0)
            return ret;

        var hour       = currentMinute / RealTime.MinutesPerHour;
        var hourOfDay  = (int)hour % RealTime.HoursPerDay;
        var nextUptime = alarm.Node!.Times!.NextUptime(hourOfDay);
        var offTime    = (hour + nextUptime) * RealTime.MinutesPerHour - currentMinute;
        var (m, s) = EorzeaTimeStampExtensions.MinutesToReal(offTime);
        if (offTime > 0)
            ret = $"will be up in {m}:{s:D2} minutes";

        return ret;
    }

    private SeString ReplaceFishFormatPlaceholders(string format, Alarm alarm)
    {
        IReadOnlyList<Payload>? Replace(string s)
        {
            if (!(s.StartsWith('{') && s.EndsWith('}')))
                return null;

            return s switch
            {
                "{Name}"            => ChatUtil.GetPayloadsFromString(alarm.Name),
                "{Offset}"          => ChatUtil.GetPayloadsFromString(alarm.MinuteOffset.ToString()),
                "{DelayString}"     => ChatUtil.GetPayloadsFromString(DelayStringFish(alarm)),
                "{FishingSpotName}" => ChatUtil.CreateMapLink(alarm.Fish!.FishingSpots.First()).Payloads,
                "{BaitName}"        => ChatUtil.CreateLink(alarm.Fish!.CatchData!.InitialBait.Data),
                "{FishName}"        => ChatUtil.CreateLink(alarm.Fish!.ItemData),
                _                   => null,
            };
        }

        return ChatUtil.Format(format, Replace);
    }

    private void Ring(Alarm alarm, uint currentMinute)
    {
        if (alarm.SoundId > Sounds.Unknown)
            _sounds.Play(alarm.SoundId);

        if (alarm.PrintMessage)
        {
            if (alarm.Type == AlarmType.Node && GatherBuddy.Config.NodeAlarmFormat.Length > 0)
            {
                Dalamud.Chat.PrintError(ReplaceNodeFormatPlaceholders(GatherBuddy.Config.NodeAlarmFormat, alarm, currentMinute));
                PluginLog.Verbose(ReplaceNodeFormatPlaceholders(GatherBuddyConfiguration.DefaultNodeAlarmFormat, alarm, currentMinute)
                    .ToString());
            }
            else if (alarm.Type == AlarmType.Fish && GatherBuddy.Config.FishAlarmFormat.Length > 0)
            {
                Dalamud.Chat.PrintError(ReplaceFishFormatPlaceholders(GatherBuddy.Config.FishAlarmFormat,        alarm));
                PluginLog.Verbose(ReplaceFishFormatPlaceholders(GatherBuddyConfiguration.DefaultFishAlarmFormat, alarm).ToString());
            }
        }

        switch (alarm.Type)
        {
            case AlarmType.Node:
                LastNodeAlarm = alarm;
                break;
            case AlarmType.Fish:
                LastFishAlarm = alarm;
                break;
        }
    }

    public void AddNode(string name, uint nodeId)
    {
        Alarm alarm = new(name, _nodes, nodeId);
        if (alarm.Node == null)
            return;

        Alarms.Add(alarm);
        _status.Add(false);
        GatherBuddy.Config.Save();
    }

    public void AddFish(string name, uint fishId)
    {
        Alarm alarm = new(name, _fish, fishId);
        if (alarm.Fish == null)
            return;

        Alarms.Add(alarm);
        _status.Add(false);
        GatherBuddy.Config.Save();
    }

    public void AddNode(string name, Node node)
    {
        Alarms.Add(new Alarm(name, node));
        _status.Add(false);
        GatherBuddy.Config.Save();
    }

    public void AddFish(string name, Fish fish)
    {
        Alarms.Add(new Alarm(name, fish));
        _status.Add(false);
        GatherBuddy.Config.Save();
    }

    public void RemoveAlarm(int idx)
    {
        if (idx >= Alarms.Count)
            return;

        Alarms.RemoveAt(idx);
        _status.RemoveAt(idx);
        GatherBuddy.Config.Save();
    }

    public void ChangeNodeSound(int idx, Sounds sound)
    {
        if (idx >= Alarms.Count)
            return;

        Alarms[idx].SoundId = sound;
        GatherBuddy.Config.Save();
    }

    public void ChangeNodeOffset(int idx, int offset)
    {
        if (idx >= Alarms.Count)
            return;

        Alarms[idx].MinuteOffset = offset;
        GatherBuddy.Config.Save();
    }

    public void ChangeNodeStatus(int idx, bool enabled)
    {
        if (idx >= Alarms.Count)
            return;

        Alarms[idx].Enabled = enabled;
        GatherBuddy.Config.Save();
    }

    public void ChangePrintStatus(int idx, bool print)
    {
        if (idx >= Alarms.Count)
            return;

        Alarms[idx].PrintMessage = print;
        GatherBuddy.Config.Save();
    }

    public void UpdateNodes()
    {
        var changed = false;
        for (var i = 0; i < Alarms.Count; ++i)
        {
            switch (Alarms[i].Type)
            {
                case AlarmType.Node:
                {
                    if (Alarms[i].Node == null)
                        Alarms[i].FetchNode(_nodes);
                    if (Alarms[i].Node != null)
                        continue;

                    break;
                }
                case AlarmType.Fish:
                {
                    if (Alarms[i].Fish == null)
                        Alarms[i].FetchFish(_fish);
                    if (Alarms[i].Fish != null)
                        continue;

                    break;
                }
            }

            _status.RemoveAt(i);
            Alarms.RemoveAt(i--);
            changed = true;
        }

        if (changed)
            GatherBuddy.Config.Save();
    }
}
