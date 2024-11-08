using System.Collections.Generic;
using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;

namespace GatherBuddy.GatherHelper;

public partial class GatherWindowManager
{
    public void AddPreset(GatherWindowPreset preset)
    {
        Presets.Add(preset);
        Save();
        if (preset.HasItems())
            SetActiveItems();
    }

    public void DeletePreset(int idx)
    {
        if (idx < 0 || idx >= Presets.Count)
            return;

        var enabled = Presets[idx].HasItems();
        Presets.RemoveAt(idx);
        Save();
        if (enabled)
            SetActiveItems();
    }

    public void MovePreset(int idx1, int idx2)
    {
        if (Functions.Move(Presets, idx1, idx2))
            Save();
    }

    public void ChangeName(GatherWindowPreset preset, string newName)
    {
        if (newName == preset.Name)
            return;

        preset.Name = newName;
        Save();
    }

    public void ChangeDescription(GatherWindowPreset preset, string newDescription)
    {
        if (newDescription == preset.Description)
            return;

        preset.Description = newDescription;
        Save();
    }

    public void TogglePreset(GatherWindowPreset preset)
    {
        preset.Enabled = !preset.Enabled;
        Save();
        if (preset.Items.Count > 0)
            SetActiveItems();
    }
    public void SetFallback(GatherWindowPreset preset, bool value)
    {
        preset.Fallback = value;
        Save();
        if (preset.Items.Count > 0)
            SetActiveItems();
    }

    public void AddItem(GatherWindowPreset preset, Gatherable item)
    {
        if (preset.Add(item))
        {
            Save();
            if (preset.Enabled)
                SetActiveItems();
        }
    }

    public void RemoveItem(GatherWindowPreset preset, int idx)
    {
        if (idx < 0 || idx >= preset.Items.Count)
            return;
        
        preset.RemoveAt(idx);
        Save();
        if (preset.Enabled)
            SetActiveItems();
    }

    public void ChangeItem(GatherWindowPreset preset, Gatherable item, int idx)
    {
        if (idx < 0 || idx >= preset.Items.Count)
            return;

        if (preset.Replace(idx, item))
        {
            Save();
            if (preset.Enabled)
                SetActiveItems();
        }
    }

    public void ChangeQuantity(GatherWindowPreset preset, Gatherable item, uint quantity)
    {
        if (preset.SetQuantity(item, quantity))
        {
            Save();
            if (preset.Enabled)
                SetActiveItems();
        }
    }

    public void MoveItem(GatherWindowPreset preset, int idx1, int idx2)
    {
        if (preset.Move(idx1, idx2))
        {
            Save();
            if (preset.Enabled)
                SetActiveItems();
        }
    }

    public void ChangePreferredLocation(GatherWindowPreset preset, Gatherable item, GatheringNode? location)
    {
        if (preset.SetPrefferedLocation(item, location))
        {
            Save();
        }
    }

    public GatheringNode? GetPreferredLocation(Gatherable item)
    {
        foreach (var preset in Presets)
        {
            if (preset.Enabled && !preset.Fallback && preset.PreferredLocations.TryGetValue(item, out var loc))
            {
                return loc;
            }
        }
        return null;
    }
}
