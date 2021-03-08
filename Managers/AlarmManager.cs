using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState;
using Dalamud.Plugin;
using GatherBuddyPlugin;
using Otter.SEFunctions;

namespace Gathering
{
    public class AlarmManager : IDisposable
    {
        private readonly DalamudPluginInterface _pi;
        private readonly NodeManager _nodes;
        private readonly GatherBuddyConfiguration _config;
        private readonly PlaySound _sounds;
        public readonly Node[] AllTimedNodes;

        private int currentMinute = 0;

        public List<Alarm> Alarms => _config.Alarms;
        private List<bool> Status;

        public Alarm LastAlarm { get; set; } = null;

        public AlarmManager(DalamudPluginInterface pi, NodeManager nodes, GatherBuddyConfiguration config)
        {
            _pi = pi;
            _nodes = nodes;
            _config = config;
            _sounds = new PlaySound(_pi.TargetModuleScanner, "[GatherBuddy]");
            Status = Enumerable.Repeat(false, Alarms.Count).ToList();
            UpdateNodes();
            AllTimedNodes = nodes.BaseNodes().Where(N => !N.times.AlwaysUp()).ToArray();
        }

        public void Dispose()
        {
            if (_config.AlarmsEnabled)
            {
                _pi.Framework.OnUpdateEvent -= OnUpdate;
            }
        }

        public void Enable(bool force = false)
        {
            if (force || !_config.AlarmsEnabled)
            {
                UpdateNodes();
                _config.AlarmsEnabled = true;
                _pi.Framework.OnUpdateEvent += OnUpdate;
                _pi.SavePluginConfig(_config);
            }
        }

        public void Disable()
        {
            if (_config.AlarmsEnabled)
            {
                _config.AlarmsEnabled = false;
                _pi.Framework.OnUpdateEvent -= OnUpdate;
                _pi.SavePluginConfig(_config);
            }
        }

        public void OnUpdate(object framework)
        {
            // Skip if the player isn't loaded in a territory.
            if (_pi.ClientState.TerritoryType == 0)
                return;

            var minute = EorzeaTime.CurrentMinuteOfDay();
            if (minute != currentMinute)
            {
                currentMinute = minute;
                for (var i = 0; i < Alarms.Count; ++i)
                {
                    var alarm = Alarms[i];
                    if (!alarm.Enabled)
                        continue;

                    var hour = ((currentMinute + alarm.MinuteOffset) / 60) % 24;

                    var newStatus = alarm.Node.times.IsUp(hour);
                    if (Status[i] == newStatus)
                        continue;

                    Status[i] = newStatus;
                    if (newStatus)
                        Ring(alarm, currentMinute);
                }
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
                    var offTime = (currentMinute / 60 + alarm.Node.times.NextUptime((currentMinute / 60) % 24)) * 60 -
                                  currentMinute;
                    var (m, s) = EorzeaTime.MinutesToReal(offTime);
                    if (offTime > 0)
                        tmp = $"will be up in {m}:{s:D2} minutes (real-time)";
                }

                var items = alarm.Node.items.PrintItems(", ", _pi.ClientState.ClientLanguage);
                _pi.Framework.Gui.Chat.PrintError($"[GatherBuddy][Alarm {alarm.Name}]: The gathering node for {items} {tmp}.");
            }

            LastAlarm = alarm;
        }

        public void AddNode(string name, int nodeId)
        {
            Alarm alarm = new(name, _nodes, nodeId);
            if (alarm.Node != null)
            {
                Alarms.Add(alarm);
                Status.Add(false);
                _pi.SavePluginConfig(_config);
            }
        }

        public void AddNode(string name, Node node)
        {
            if (node != null)
            {
                Alarms.Add(new(name, node));
                Status.Add(false);
                _pi.SavePluginConfig(_config);
            }
        }

        public void RemoveNode(int idx)
        {
            if (idx >= Alarms.Count)
                return;
            Alarms.RemoveAt(idx);
            Status.RemoveAt(idx);
            _pi.SavePluginConfig(_config);
        }

        public void ChangeNodeSound(int idx, Sounds sound)
        {
            if (idx >= Alarms.Count)
                return;
            Alarms[idx].SetSound(sound);
            _pi.SavePluginConfig(_config);
        }

        public void ChangeNodeOffset(int idx, int offset)
        {
            if (idx >= Alarms.Count)
                return;
            Alarms[idx].SetOffset(offset);
            _pi.SavePluginConfig(_config);
        }

        public void ChangeNodeStatus(int idx, bool enabled)
        {
            if (idx >= Alarms.Count)
                return;
            Alarms[idx].SetEnabled(enabled);
            _pi.SavePluginConfig(_config);
        }

        public void ChangePrintStatus(int idx, bool print)
        {
            if (idx >= Alarms.Count)
                return;
            Alarms[idx].SetPrintMessage(print);
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
                    if (Alarms[i].Node == null)
                    {
                        Status.RemoveAt(i);
                        Alarms.RemoveAt(i--);
                        changed = true;
                    }
                }
            }
            if (changed)
                _pi.SavePluginConfig(_config);
        }
    }
}