using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Plugin;
using GatherBuddy.Time;

namespace GatherBuddy.Alarms;

public partial class AlarmManager
{
    public void ToggleGroup(int idx)
    {
        var group = Alarms[idx];
        if (group.Enabled)
        {
            group.Enabled = false;
            foreach (var alarm in group.Alarms.Where(a => a.Enabled))
                ActiveAlarms.Remove(alarm);
        }
        else
        {
            group.Enabled = true;
            foreach (var alarm in group.Alarms.Where(a => a.Enabled))
            {
                ActiveAlarms.Add(alarm, TimeStamp.Epoch);
                SetDirty();
            }
        }

        Save();
    }

    public void AddGroup(string name)
        => AddGroup(new AlarmGroup()
        {
            Name        = name,
            Description = string.Empty,
            Enabled     = false,
            Alarms      = new List<Alarm>(),
        });

    public void AddGroup(AlarmGroup group)
    {
        Alarms.Add(group);
        if (group.Enabled && group.Alarms.Any(a => a.Enabled))
            SetActiveAlarms();
        Save();
    }

    public void MoveGroup(int idx1, int idx2)
    {
        if (Functions.Move(Alarms, idx1, idx2))
            Save();
    }

    public void DeleteGroup(int idx)
    {
        var group1 = Alarms[idx];
        if (group1.Enabled)
            foreach (var alarm in group1.Alarms.Where(a => a.Enabled))
                ActiveAlarms.Remove(alarm);
        Alarms.RemoveAt(idx);
        Save();
    }

    public void ChangeGroupName(int idx, string newName)
    {
        var group = Alarms[idx];
        if (group.Name == newName)
            return;

        group.Name = newName;
        Save();
    }

    public void ChangeGroupDescription(int idx, string newDesc)
    {
        var group = Alarms[idx];
        if (group.Description == newDesc)
            return;

        group.Description = newDesc;
        Save();
    }
}
