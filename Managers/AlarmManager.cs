using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Plugin;
using GatherBuddy.Classes;
using GatherBuddy.SeFunctions;
using GatherBuddy.SEFunctions;
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

        private int _currentMinute;

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

                var hour = (_currentMinute + alarm.MinuteOffset) / 60 % 24;

                var newStatus = alarm.Node!.Times!.IsUp(hour);
                if (_status[i] == newStatus)
                    continue;

                _status[i] = newStatus;
                if (newStatus)
                    Ring(alarm, _currentMinute);
            }
        }

        private void Ring(Alarm alarm, int currentMinute)
        {
            if (alarm.SoundId > Sounds.Unknown)
                _sounds.Invoke(alarm.SoundId);

            if (alarm.PrintMessage)
            {
                var tmp = "is currently up";
                if (alarm.MinuteOffset > 0)
                {
                    var offTime = (currentMinute / 60 + alarm.Node!.Times!.NextUptime(currentMinute / 60 % 24)) * 60 - currentMinute;
                    var (m, s) = EorzeaTime.MinutesToReal(offTime);
                    if (offTime > 0)
                        tmp = $"will be up in {m}:{s:D2} minutes (real-time)";
                }

                var items = alarm.Node!.Items!.PrintItems(", ", _pi.ClientState.ClientLanguage);
                _pi.Framework.Gui.Chat.PrintError($"[GatherBuddy][Alarm {alarm.Name}]: The gathering node for {items} {tmp}.");
            }

            LastAlarm = alarm;
        }

        public void AddNode(string name, int nodeId)
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
            if (node == null)
                return;

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
                if (Alarms[i].Node == null)
                {
                    Alarms[i].FetchNode(_nodes);
                    if (Alarms[i].Node != null)
                        continue;

                    _status.RemoveAt(i);
                    Alarms.RemoveAt(i--);
                    changed = true;
                }

            if (changed)
                _pi.SavePluginConfig(_config);
        }
    }
}
