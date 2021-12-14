using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using GatherBuddy.Time;

namespace GatherBuddy.Alarms;

public partial class AlarmManager
{
    public void ToggleAlarm(AlarmGroup group, int idx)
    {
        var alarm = group.Alarms[idx];
        if (alarm.Enabled)
        {
            alarm.Enabled = false;
            ActiveAlarms.Remove(alarm);
        }
        else
        {
            alarm.Enabled = true;
            if (group.Enabled)
            {
                ActiveAlarms.TryAdd(alarm, TimeStamp.Epoch);
                SetDirty();
            }
        }

        Save();
    }

    public void MoveAlarm(AlarmGroup group, int idx1, int idx2)
    {
        if (Functions.Move(group.Alarms, idx1, idx2))
            Save();
    }

    public void DeleteAlarm(AlarmGroup group, int idx)
    {
        var alarm = group.Alarms[idx];
        if (group.Enabled && alarm.Enabled)
            ActiveAlarms.Remove(alarm);

        group.Alarms.RemoveAt(idx);
    }

    public void AddAlarm(AlarmGroup group, Alarm value)
    {
        group.Alarms.Add(value);
        if (group.Enabled && value.Enabled)
        {
            ActiveAlarms.Add(value, TimeStamp.Epoch);
            SetDirty();
        }

        Save();
    }

    public void ChangeAlarmName(AlarmGroup group, int idx, string name)
    {
        var alarm = group.Alarms[idx];
        if (alarm.Name == name)
            return;

        alarm.Name = name;
        Save();
    }

    public void ChangeAlarmItem(AlarmGroup group, int idx, IGatherable item)
    {
        var alarm = group.Alarms[idx];
        if (ReferenceEquals(alarm.Item, item))
            return;

        alarm.Item = item;
        if (ActiveAlarms.ContainsKey(alarm))
        {
            ActiveAlarms[alarm] = TimeStamp.Epoch;
            SetDirty();
            if (ReferenceEquals(alarm, LastFishAlarm?.Item1))
                LastFishAlarm = null;
            if (ReferenceEquals(alarm, LastItemAlarm?.Item1))
                LastItemAlarm = null;
        }

        Save();
    }

    public void ChangeAlarmMessage(AlarmGroup group, int idx, bool printMessage)
    {
        var alarm = group.Alarms[idx];
        if (alarm.PrintMessage == printMessage)
            return;

        alarm.PrintMessage = printMessage;
        Save();
    }

    public void ChangeAlarmSound(AlarmGroup group, int idx, Sounds soundId)
    {
        var alarm = group.Alarms[idx];
        if (alarm.SoundId == soundId)
            return;

        alarm.SoundId = soundId;
        Save();
    }

    public void ChangeAlarmOffset(AlarmGroup group, int idx, int secondOffset)
    {
        var alarm = group.Alarms[idx];
        if (alarm.SecondOffset == secondOffset)
            return;

        alarm.SecondOffset = secondOffset;
        if (ActiveAlarms.ContainsKey(alarm))
        {
            ActiveAlarms[alarm] = TimeStamp.Epoch;
            SetDirty();
        }

        Save();
    }
}
