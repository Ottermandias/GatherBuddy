using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Plugin;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Nodes;
using GatherBuddy.SeFunctions;
using GatherBuddy.Utility;

namespace GatherBuddy.Managers
{
    public class AlarmManager : IDisposable
    {
        private readonly DalamudPluginInterface   _pi;
        private readonly NodeManager              _nodes;
        private readonly GatherBuddyConfiguration _config;
        private readonly PlaySound                _sounds;
        public           Node[]                   AllTimedNodes { get; }

        private uint _currentMinute;

        public List<Alarm> Alarms
            => _config.Alarms;

        private readonly List<bool> _status;

        public Alarm? LastAlarm { get; set; }

        public AlarmManager(DalamudPluginInterface pi, NodeManager nodes, GatherBuddyConfiguration config)
        {
            _pi     = pi;
            _nodes  = nodes;
            _config = config;
            _sounds = new PlaySound(_pi.TargetModuleScanner);
            _status = Enumerable.Repeat(false, Alarms.Count).ToList();
            UpdateNodes();
            AllTimedNodes = nodes.BaseNodes().Where(n => !n.Times!.AlwaysUp()).ToArray();
        }

        public void Dispose()
        {
            if (_config.AlarmsEnabled)
                _pi.Framework.OnUpdateEvent -= OnUpdate;
        }

        public void Enable(bool force = false)
        {
            if (!force && _config.AlarmsEnabled)
                return;

            UpdateNodes();
            _config.AlarmsEnabled       =  true;
            _pi.Framework.OnUpdateEvent += OnUpdate;
            _pi.SavePluginConfig(_config);
        }

        public void Disable()
        {
            if (!_config.AlarmsEnabled)
                return;

            _config.AlarmsEnabled       =  false;
            _pi.Framework.OnUpdateEvent -= OnUpdate;
            _pi.SavePluginConfig(_config);
        }

        public void OnUpdate(object framework)
        {
            // Skip if the player isn't loaded in a territory.
            if (_pi.ClientState.TerritoryType == 0)
                return;

            var minute = EorzeaTime.CurrentMinuteOfDay();
            if (minute == _currentMinute)
                return;

            _currentMinute = minute;
            for (var i = 0; i < Alarms.Count; ++i)
            {
                var alarm = Alarms[i];
                if (!alarm.Enabled)
                    continue;

                var hour      = (_currentMinute + alarm.MinuteOffset) / RealTime.MinutesPerHour;
                var hourOfDay = (uint) hour % RealTime.HoursPerDay;

                var newStatus = alarm.Node!.Times!.IsUp(hourOfDay);
                if (_status[i] == newStatus)
                    continue;

                _status[i] = newStatus;
                if (newStatus)
                    Ring(alarm, _currentMinute);
            }
        }

        private string ReplaceFormatPlaceholders(string format, Alarm alarm, uint currentMinute)
        {
            var result = format.Replace("{Name}", alarm.Name);
            result = result.Replace("{Offset}",     alarm.MinuteOffset.ToString());
            result = result.Replace("{TimesShort}", alarm.Node!.Times!.PrintHours(true));
            result = result.Replace("{TimesLong}",  alarm.Node!.Times!.PrintHours());
            result = result.Replace("{AllItems}",   alarm.Node!.Items!.PrintItems(", ", _pi.ClientState.ClientLanguage));

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

        private void Ring(Alarm alarm, uint currentMinute)
        {
            if (alarm.SoundId > Sounds.Unknown)
                _sounds.Play(alarm.SoundId);

            if (alarm.PrintMessage && _config.AlarmFormat.Length > 0)
            {
                _pi.Framework.Gui.Chat.PrintError(ReplaceFormatPlaceholders(_config.AlarmFormat,         alarm, currentMinute));
                PluginLog.Verbose(ReplaceFormatPlaceholders(GatherBuddyConfiguration.DefaultAlarmFormat, alarm, currentMinute));
            }

            LastAlarm = alarm;
        }

        public void AddNode(string name, uint nodeId)
        {
            Alarm alarm = new(name, _nodes, nodeId);
            if (alarm.Node == null)
                return;

            Alarms.Add(alarm);
            _status.Add(false);
            _pi.SavePluginConfig(_config);
        }

        public void AddNode(string name, Node node)
        {
            Alarms.Add(new Alarm(name, node));
            _status.Add(false);
            _pi.SavePluginConfig(_config);
        }

        public void RemoveNode(int idx)
        {
            if (idx >= Alarms.Count)
                return;

            Alarms.RemoveAt(idx);
            _status.RemoveAt(idx);
            _pi.SavePluginConfig(_config);
        }

        public void ChangeNodeSound(int idx, Sounds sound)
        {
            if (idx >= Alarms.Count)
                return;

            Alarms[idx].SoundId = sound;
            _pi.SavePluginConfig(_config);
        }

        public void ChangeNodeOffset(int idx, int offset)
        {
            if (idx >= Alarms.Count)
                return;

            Alarms[idx].MinuteOffset = offset;
            _pi.SavePluginConfig(_config);
        }

        public void ChangeNodeStatus(int idx, bool enabled)
        {
            if (idx >= Alarms.Count)
                return;

            Alarms[idx].Enabled = enabled;
            _pi.SavePluginConfig(_config);
        }

        public void ChangePrintStatus(int idx, bool print)
        {
            if (idx >= Alarms.Count)
                return;

            Alarms[idx].PrintMessage = print;
            _pi.SavePluginConfig(_config);
        }

        public void UpdateNodes()
        {
            var changed = false;
            for (var i = 0; i < Alarms.Count; ++i)
            {
                if (Alarms[i].Node == null)
                {
                    Alarms[i].FetchNode(_nodes);
                    if (Alarms[i].Node != null)
                        continue;

                    _status.RemoveAt(i);
                    Alarms.RemoveAt(i--);
                    changed = true;
                }
            }

            if (changed)
                _pi.SavePluginConfig(_config);
        }
    }
}
