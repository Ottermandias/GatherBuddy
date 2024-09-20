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

    public void AddItem(GatherWindowPreset preset, IGatherable item)
    {
        if (!preset.Add(item))
            return;

        Save();
        if (preset.Enabled)
            SetActiveItems();
    }

    public void RemoveItem(GatherWindowPreset preset, int idx)
    {
        if (idx < 0 || idx >= preset.Items.Count)
            return;
        
        if (preset.Quantities.ContainsKey(preset.Items[idx].ItemId))
            preset.Quantities.Remove(preset.Items[idx].ItemId);

        preset.Items.RemoveAt(idx);
        Save();
        if (preset.Enabled)
            SetActiveItems();
    }

    public void ChangeItem(GatherWindowPreset preset, IGatherable item, int idx)
    {
        if (idx < 0 || idx >= preset.Items.Count)
            return;

        if (ReferenceEquals(preset.Items[idx], item))
            return;

        preset.Items[idx] = item;
        Save();
        if (preset.Enabled)
            SetActiveItems();
    }

    public void ChangeQuantity(GatherWindowPreset preset, uint quantity, uint itemId)
    {
        if (!preset.Quantities.TryGetValue(itemId, out var presetQuantity))
            return;
        if (presetQuantity == quantity)
            return;
        
        if (quantity < 1)
            quantity = 1;
        if (quantity > 9999)
            quantity = 9999;
        if (GatherBuddy.GameData.Gatherables[itemId].ItemData.FilterGroup == 18)
            quantity = 1;
        
        preset.Quantities[itemId] = quantity;
        Save();
        if (preset.Enabled)
            SetActiveItems();
    }

    public void MoveItem(GatherWindowPreset preset, int idx1, int idx2)
    {
        if (!Functions.Move(preset.Items, idx1, idx2))
            return;

        Save();
        if (preset.Enabled)
            SetActiveItems();
    }

    public void ChangePreferredLocation(GatherWindowPreset preset, IGatherable item, ILocation? location)
    {
        if (item is not Gatherable) return;

        if (location is not GatheringNode)
            preset.PreferredLocations.Remove(item.ItemId);
        else
            preset.PreferredLocations[item.ItemId] = location.Id;
        
        Save();
    }

    public GatheringNode? GetPreferredLocation(Gatherable item)
    {
        foreach (var preset in Presets)
        {
            if (preset.Enabled)
            {
                if (preset.PreferredLocations.TryGetValue(item.ItemId, out var locId))
                {
                    return GatherBuddy.GameData.GatheringNodes.GetValueOrDefault(locId);
                }
            }
        }
        return null;
    }
}
