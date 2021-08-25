using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.Nodes;
using GatherBuddy.SeFunctions;
using GatherBuddy.Utility;

namespace GatherBuddy.Managers
{
    public class AlarmManager : IDisposable
    {
        private readonly NodeManager    _nodes;
        private readonly FishManager    _fish;
        private readonly WeatherManager _weather;
        private readonly PlaySound      _sounds;
        public           Node[]         AllTimedNodes { get; }
        public           Fish[]         AllTimedFish  { get; }

        private uint     _currentMinute;
        private DateTime _currentTime;

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
            var hourOfDay = (uint) hour % RealTime.HoursPerDay;
            return nodeAlarm.Node!.Times!.IsUp(hourOfDay);
        }

        private bool NewStatusFish(Alarm fishAlarm)
        {
            var uptime = fishAlarm.Fish!.NextUptime(_weather);
            return uptime.Time - _currentTime < TimeSpan.FromMinutes(fishAlarm.MinuteOffset);
        }

        public void OnUpdate(object framework)
        {
            // Skip if the player isn't loaded in a territory.
            if (Dalamud.ClientState.TerritoryType == 0)
                return;

            var minute = EorzeaTime.CurrentMinuteOfDay();
            if (minute == _currentMinute)
                return;

            _currentMinute = minute;
            _currentTime   = DateTime.UtcNow;
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
                    Ring(alarm, _currentMinute);
            }
        }

        private string ReplaceNodeFormatPlaceholders(string format, Alarm alarm, uint currentMinute)
        {
            var result = format.Replace("{Name}", alarm.Name);
            result = result.Replace("{Offset}",     alarm.MinuteOffset.ToString());
            result = result.Replace("{TimesShort}", alarm.Node!.Times!.PrintHours(true));
            result = result.Replace("{TimesLong}",  alarm.Node!.Times!.PrintHours());
            result = result.Replace("{AllItems}",   alarm.Node!.Items!.PrintItems(", ", GatherBuddy.Language));

            var tmp = "is currently up";
            if (alarm.MinuteOffset > 0)
            {
                var hour       = currentMinute / RealTime.MinutesPerHour;
                var hourOfDay  = hour % RealTime.HoursPerDay;
                var nextUptime = alarm.Node!.Times!.NextUptime(hourOfDay);
                var offTime    = (hour + nextUptime) * RealTime.MinutesPerHour - currentMinute;
                var (m, s) = EorzeaTime.MinutesToReal(offTime);
                if (offTime > 0)
                    tmp = $"will be up in {m}:{s:D2} minutes";
            }

            result = result.Replace("{DelayString}", tmp);
            return result;
        }

        private string ReplaceFishFormatPlaceholders(string format, Alarm alarm)
        {
            var result = format.Replace("{Name}", alarm.Name);
            result = result.Replace("{Offset}",          alarm.MinuteOffset.ToString());
            result = result.Replace("{FishName}",        alarm.Fish!.Name[GatherBuddy.Language]);
            result = result.Replace("{FishingSpotName}", alarm.Fish!.FishingSpots.First().PlaceName?[GatherBuddy.Language] ?? "Unknown");
            result = result.Replace("{BaitName}",        alarm.Fish!.CatchData?.InitialBait.Name[GatherBuddy.Language] ?? "Unknown");

            var uptime = alarm.Fish!.NextUptime(_weather);
            var tmp    = "is currently up";
            if (uptime.Time > _currentTime)
            {
                var diff = uptime.Time - _currentTime;
                tmp = $"will be up in {(int) diff.TotalSeconds / 60}:{(int) diff.TotalSeconds % 60:D2} minutes";
            }

            result = result.Replace("{DelayString}", tmp);
            return result;
        }

        private void Ring(Alarm alarm, uint currentMinute)
        {
            if (alarm.SoundId > Sounds.Unknown)
                _sounds.Play(alarm.SoundId);

            if (alarm.PrintMessage)
            {
                if (alarm.Type == AlarmType.Node && GatherBuddy.Config.NodeAlarmFormat.Length > 0)
                {
                    Dalamud.Chat.PrintError(ReplaceNodeFormatPlaceholders(GatherBuddy.Config.NodeAlarmFormat,        alarm, currentMinute));
                    PluginLog.Verbose(ReplaceNodeFormatPlaceholders(GatherBuddyConfiguration.DefaultNodeAlarmFormat, alarm, currentMinute));
                }
                else if (alarm.Type == AlarmType.Fish && GatherBuddy.Config.FishAlarmFormat.Length > 0)
                {
                    Dalamud.Chat.PrintError(ReplaceFishFormatPlaceholders(GatherBuddy.Config.FishAlarmFormat,        alarm));
                    PluginLog.Verbose(ReplaceFishFormatPlaceholders(GatherBuddyConfiguration.DefaultFishAlarmFormat, alarm));
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
}
