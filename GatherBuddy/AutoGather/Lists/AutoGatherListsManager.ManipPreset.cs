using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;

namespace GatherBuddy.AutoGather.Lists;

public partial class AutoGatherListsManager
{
    public void AddList(AutoGatherList list)
    {
        _lists.Add(list);
        Save();
        if (list.HasItems())
            SetActiveItems();
    }

    public void DeleteList(int idx)
    {
        if (idx < 0 || idx >= _lists.Count)
            return;

        var enabled = _lists[idx].HasItems();
        _lists.RemoveAt(idx);
        Save();
        if (enabled)
            SetActiveItems();
    }

    public void MoveList(int idx1, int idx2)
    {
        if (Functions.Move(_lists, idx1, idx2))
            Save();
    }

    public void ChangeName(AutoGatherList list, string newName)
    {
        if (newName == list.Name)
            return;

        list.Name = newName;
        Save();
    }

    public void ChangeDescription(AutoGatherList list, string newDescription)
    {
        if (newDescription == list.Description)
            return;

        list.Description = newDescription;
        Save();
    }

    public void ToggleList(AutoGatherList list)
    {
        list.Enabled = !list.Enabled;
        Save();
        if (list.Items.Count > 0)
            SetActiveItems();
    }

    public void SetFallback(AutoGatherList list, bool value)
    {
        list.Fallback = value;
        Save();
        if (list.Items.Count > 0)
            SetActiveItems();
    }

    public void AddItem(AutoGatherList list, IGatherable item)
    {
        if (list.Add(item))
        {
            Save();
            if (list.Enabled)
                SetActiveItems();
        }
    }

    public void RemoveItem(AutoGatherList list, int idx)
    {
        if (idx < 0 || idx >= list.Items.Count)
            return;

        list.RemoveAt(idx);
        Save();
        if (list.Enabled)
            SetActiveItems();
    }

    public void ChangeItem(AutoGatherList list, IGatherable item, int idx)
    {
        if (idx < 0 || idx >= list.Items.Count)
            return;

        if (list.Replace(idx, item))
        {
            Save();
            if (list.Enabled)
                SetActiveItems();
        }
    }

    public void ChangeQuantity(AutoGatherList list, IGatherable item, uint quantity)
    {
        if (list.SetQuantity(item, quantity))
        {
            Save();
            if (list.Enabled)
                SetActiveItems();
        }
    }

    public void ChangeEnabled(AutoGatherList list, IGatherable item, bool enabled)
    {
        if (list.SetEnabled(item, enabled))
        {
            Save();
            if (list.Enabled)
                SetActiveItems();
        }
    }

    public void MoveItem(AutoGatherList list, int idx1, int idx2)
    {
        if (list.Move(idx1, idx2))
        {
            Save();
            if (list.Enabled)
                SetActiveItems();
        }
    }

    public void ChangePreferredLocation(AutoGatherList list, IGatherable? item, ILocation? location)
    {
        if (item == null)
            return;
        if (list.SetPreferredLocation(item, location))
        {
            Save();
        }
    }

    public ILocation? GetPreferredLocation(IGatherable item)
    {
        foreach (var list in _lists)
        {
            if (list.Enabled && !list.Fallback && list.PreferredLocations.TryGetValue(item, out var loc))
            {
                return loc;
            }
        }
        return null;
    }
}
