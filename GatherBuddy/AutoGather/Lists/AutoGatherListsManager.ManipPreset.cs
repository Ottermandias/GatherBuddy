using System;
using System.Linq;
using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using OtterGui.Filesystem;

namespace GatherBuddy.AutoGather.Lists;

public partial class AutoGatherListsManager
{
    public void AddList(AutoGatherList list, FileSystem<AutoGatherList>.Folder? folder = null)
    {
        folder ??= _fileSystem.Root;
        try
        {
            _fileSystem.CreateLeaf(folder, list.Name, list);
        }
        catch
        {
            _fileSystem.CreateDuplicateLeaf(folder, list.Name, list);
        }
        Save();
        if (list.HasItems())
            SetActiveItems();
    }

    public void DeleteList(AutoGatherList list)
    {
        if (!_fileSystem.TryGetValue(list, out var leaf))
            return;

        var enabled = list.HasItems();
        _fileSystem.Delete(leaf);
        Save();
        if (enabled)
            SetActiveItems();
    }

    public void MoveList(AutoGatherList list, FileSystem<AutoGatherList>.Folder targetFolder)
    {
        if (!_fileSystem.TryGetValue(list, out var leaf))
            return;

        try
        {
            _fileSystem.Move(leaf, targetFolder);
            Save();
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Warning($"Failed to move list: {e.Message}");
        }
    }

    public void CreateFolder(string name, FileSystem<AutoGatherList>.Folder? parent = null)
    {
        parent ??= _fileSystem.Root;
        try
        {
            _fileSystem.CreateFolder(parent, name);
            Save();
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Warning($"Failed to create folder: {e.Message}");
        }
    }

    public void DeleteFolder(FileSystem<AutoGatherList>.Folder folder)
    {
        if (folder.IsRoot)
            return;

        try
        {
            _fileSystem.Delete(folder);
            Save();
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Warning($"Failed to delete folder: {e.Message}");
        }
    }

    public void ChangeName(AutoGatherList list, string newName)
    {
        if (newName == list.Name || !_fileSystem.TryGetValue(list, out var leaf))
            return;

        try
        {
            _fileSystem.Rename(leaf, newName);
            list.Name = newName;
            Save();
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Warning($"Failed to rename list: {e.Message}");
        }
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

    public event Action? ListOrderChanged;

    public void MoveListUp(AutoGatherList list)
    {
        if (!_fileSystem.TryGetValue(list, out var leaf))
            return;

        var parent = leaf.Parent;
        var siblings = parent.GetLeaves().OrderBy(l => l.Value.Order).ToList();
        var index = siblings.IndexOf(leaf);
        
        if (index > 0)
        {
            var prevList = siblings[index - 1].Value;
            var temp = prevList.Order;
            prevList.Order = list.Order;
            list.Order = temp;
            Save();
            ListOrderChanged?.Invoke();
        }
    }

    public void MoveListDown(AutoGatherList list)
    {
        if (!_fileSystem.TryGetValue(list, out var leaf))
            return;

        var parent = leaf.Parent;
        var siblings = parent.GetLeaves().OrderBy(l => l.Value.Order).ToList();
        var index = siblings.IndexOf(leaf);
        
        if (index < siblings.Count - 1)
        {
            var nextList = siblings[index + 1].Value;
            var temp = nextList.Order;
            nextList.Order = list.Order;
            list.Order = temp;
            Save();
            ListOrderChanged?.Invoke();
        }
    }

    public ILocation? GetPreferredLocation(IGatherable item)
    {
        foreach (var list in _fileSystem.Select(kvp => kvp.Key))
        {
            if (list.Enabled && !list.Fallback && list.PreferredLocations.TryGetValue(item, out var loc))
            {
                return loc;
            }
        }
        return null;
    }
}
