using System.Linq;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;

namespace GatherBuddy.Alarms;

public partial class AlarmManager
{
    public void ToggleAlarm(AlarmGroup group, int idx)
    {
        var alarm = group.Alarms[idx];
        if (alarm.Enabled)
        {
            alarm.Enabled = false;
            if (group.Enabled)
                RemoveActiveAlarm(alarm);
        }
        else
        {
            alarm.Enabled = true;
            if (group.Enabled)
                AddActiveAlarm(alarm);
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
            RemoveActiveAlarm(alarm);

        group.Alarms.RemoveAt(idx);
        Save();
    }

    public void AddAlarm(AlarmGroup group, Alarm value)
    {
        group.Alarms.Add(value);
        if (group.Enabled && value.Enabled)
        {
            AddActiveAlarm(value);
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

        RemoveActiveAlarm(alarm);
        alarm.Item = item;
        if (alarm.PreferLocation != null && !alarm.PreferLocation.Gatherables.Contains(item))
            alarm.PreferLocation = null;
        if (group.Enabled && alarm.Enabled)
            AddActiveAlarm(alarm);
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

    public void ChangeAlarmLocation(AlarmGroup group, int idx, ILocation? location)
    {
        var alarm = group.Alarms[idx];
        if (alarm.PreferLocation == location)
            return;

        RemoveActiveAlarm(alarm);
        alarm.PreferLocation = location;
        if (group.Enabled && alarm.Enabled)
            AddActiveAlarm(alarm);
        Save();
    }

    public void ChangeAlarmOffset(AlarmGroup group, int idx, int secondOffset)
    {
        var alarm = group.Alarms[idx];
        if (alarm.SecondOffset == secondOffset)
            return;

        RemoveActiveAlarm(alarm);
        alarm.SecondOffset = secondOffset;
        if (group.Enabled && alarm.Enabled)
            AddActiveAlarm(alarm);
        Save();
    }
}
