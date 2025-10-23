using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using Newtonsoft.Json;
using OtterGui;
using OtterGui.Filesystem;
using Functions = GatherBuddy.Plugin.Functions;

namespace GatherBuddy.AutoGather.Lists;

public class ManualOrderSortMode : ISortMode<AutoGatherList>
{
    public ReadOnlySpan<byte> Name
        => "Manual Order"u8;

    public ReadOnlySpan<byte> Description
        => "Sort by manually assigned order, with folders first."u8;

    public IEnumerable<FileSystem<AutoGatherList>.IPath> GetChildren(FileSystem<AutoGatherList>.Folder folder)
    {
        var folders = folder.GetSubFolders().Cast<FileSystem<AutoGatherList>.IPath>();
        var leaves = folder.GetLeaves()
            .OrderBy(l => l.Value.Order)
            .ThenBy(l => l.Name, StringComparer.OrdinalIgnoreCase)
            .Cast<FileSystem<AutoGatherList>.IPath>();
        return folders.Concat(leaves);
    }
}

public partial class AutoGatherListsManager : IDisposable
{
    public event Action? ActiveItemsChanged;

    private const string FileName         = "auto_gather_lists.json";
    private const string FileNameFallback = "gather_window.json";

    private readonly FileSystem<AutoGatherList>             _fileSystem;
    private readonly List<(Gatherable Item, uint Quantity)> _activeItems   = [];
    private readonly List<(Gatherable Item, uint Quantity)> _fallbackItems = [];
    private readonly List<(Fish Fish, uint Quantity)>       _activeFish    = [];

    public FileSystem<AutoGatherList> FileSystem
        => _fileSystem;

    public IEnumerable<AutoGatherList> Lists
        => _fileSystem.Select(kvp => kvp.Key);

    public ReadOnlyCollection<(Gatherable Item, uint Quantity)> ActiveItems
        => _activeItems.AsReadOnly();

    public ReadOnlyCollection<(Gatherable Item, uint Quantity)> FallbackItems
        => _fallbackItems.AsReadOnly();

    public ReadOnlyCollection<(Fish Fish, uint Quantity)> ActiveFish
        => _activeFish.AsReadOnly();

    public AutoGatherListsManager()
    {
        _fileSystem = new FileSystem<AutoGatherList>();
        _fileSystem.Changed += OnFileSystemChanged;
    }

    private FileSystem<AutoGatherList>.IPath? _dropTarget = null;
    private FileSystem<AutoGatherList>.IPath? _movedPath = null;

    public void SetDropTarget(FileSystem<AutoGatherList>.IPath dropTarget, FileSystem<AutoGatherList>.IPath? movedPath)
    {
        _dropTarget = dropTarget;
        _movedPath = movedPath;
    }

    private void OnFileSystemChanged(FileSystemChangeType type, FileSystem<AutoGatherList>.IPath changedObject, FileSystem<AutoGatherList>.IPath? previousParent, FileSystem<AutoGatherList>.IPath? newParent)
    {
        if (type == FileSystemChangeType.ObjectMoved)
        {
            
            if (changedObject is FileSystem<AutoGatherList>.Leaf movedLeaf && _dropTarget != null && changedObject == _movedPath)
            {
                ReorderAfterDrop(movedLeaf, _dropTarget);
                _dropTarget = null;
                _movedPath = null;
            }
            else
            {
                ReorderListsInFolder(newParent as FileSystem<AutoGatherList>.Folder ?? _fileSystem.Root);
            }
            
            if (previousParent != newParent && previousParent is FileSystem<AutoGatherList>.Folder oldFolder)
            {
                ReorderListsInFolder(oldFolder);
            }
            Save();
        }
        else if (type == FileSystemChangeType.LeafAdded)
        {
            if (changedObject is FileSystem<AutoGatherList>.Leaf leaf)
            {
                var parent = leaf.Parent;
                var siblings = parent.GetLeaves().Where(l => l != leaf).ToList();
                
                if (siblings.Count > 0 && siblings.Any(l => l.Value.Order == leaf.Value.Order))
                {
                    var maxOrder = siblings.Select(l => l.Value.Order).Max();
                    leaf.Value.Order = maxOrder + 1;
                }
            }
        }
    }

    private void ReorderAfterDrop(FileSystem<AutoGatherList>.Leaf movedLeaf, FileSystem<AutoGatherList>.IPath dropTarget)
    {
        var targetFolder = dropTarget as FileSystem<AutoGatherList>.Folder ?? dropTarget.Parent;
        
        if (dropTarget is FileSystem<AutoGatherList>.Leaf targetLeaf && targetLeaf != movedLeaf)
        {
            var leaves = targetFolder.GetLeaves().Where(l => l != movedLeaf).OrderBy(l => l.Value.Order).ToList();
            var targetIndex = leaves.IndexOf(targetLeaf);
            
            for (int i = 0; i < leaves.Count; i++)
            {
                if (i < targetIndex)
                {
                    leaves[i].Value.Order = i;
                }
                else if (i == targetIndex)
                {
                    leaves[i].Value.Order = i + 1;
                }
                else
                {
                    leaves[i].Value.Order = i + 1;
                }
            }
            
            movedLeaf.Value.Order = targetIndex;
        }
        else
        {
            ReorderListsInFolder(targetFolder);
        }
    }

    private void ReorderListsInFolder(FileSystem<AutoGatherList>.Folder folder)
    {
        var leaves = folder.GetLeaves().OrderBy(l => l.Value.Order).ToList();
        for (int i = 0; i < leaves.Count; i++)
        {
            leaves[i].Value.Order = i;
        }
    }

    public void Dispose()
    { }

    public void SetActiveItems()
    {
        _activeItems.Clear();
        _activeFish.Clear();
        _fallbackItems.Clear();
        var items = _fileSystem.Select(kvp => kvp.Key)
            .Where(l => l.Enabled)
            .SelectMany(l => l.Items.Select(i => (Item: i, Quantity: l.Quantities[i], l.Fallback, ItemEnabled: l.EnabledItems[i])))
            .Where(i => i.ItemEnabled)
            .GroupBy(i => (i.Item, i.Fallback))
            .Select(x => (x.Key.Item, Quantity: (uint)Math.Min(x.Sum(g => g.Quantity), uint.MaxValue), x.Key.Fallback));

        foreach (var (item, quantity, fallback) in items)
        {
            if (item is Fish fish)
            {
                _activeFish.Add((fish, quantity));
            }

            if (item is Gatherable gatherable)
            {
                if (fallback)
                {
                    _fallbackItems.Add((gatherable, quantity));
                }
                else
                {
                    _activeItems.Add((gatherable, quantity));
                }
            }
        }

        ActiveItemsChanged?.Invoke();
    }

    public void Save()
    {
        var file = Functions.ObtainSaveFile(FileName);
        if (file == null)
        {
            GatherBuddy.Log.Error("Failed to obtain save file for auto-gather lists");
            return;
        }

        try
        {
            var allLists = _fileSystem.Select(kvp => kvp.Key).ToList();
            foreach (var list in allLists)
            {
                if (_fileSystem.TryGetValue(list, out var leaf))
                    list.FolderPath = leaf.Parent.IsRoot ? string.Empty : leaf.Parent.FullName();
            }

            var text = JsonConvert.SerializeObject(allLists.Select(p => new AutoGatherList.Config(p)), Formatting.Indented);
            File.WriteAllText(file.FullName, text);
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Error serializing auto-gather lists data:\n{e}");
        }
    }

    public static AutoGatherListsManager Load()
    {
        var ret    = new AutoGatherListsManager();
        var file   = Functions.ObtainSaveFile(FileName);
        var change = false;
        if (file is not { Exists: true })
        {
            file = Functions.ObtainSaveFile(FileNameFallback);
            if (file is not { Exists: true })
            {
                ret.Save();
                return ret;
            }

            change = true;
        }

        try
        {
            var text = File.ReadAllText(file.FullName);
            var data = JsonConvert.DeserializeObject<AutoGatherList.Config[]>(text)!;
            foreach (var cfg in data)
            {
                change |= AutoGatherList.FromConfig(cfg, out var list);
                
                var folderPath = string.IsNullOrEmpty(list.FolderPath) ? string.Empty : list.FolderPath;
                
                if (folderPath == list.Name)
                {
                    folderPath = string.Empty;
                    change = true;
                }
                
                var folderNames = folderPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                
                var folder = ret._fileSystem.Root;
                foreach (var folderName in folderNames)
                {
                    (folder, _) = ret._fileSystem.FindOrCreateFolder(folder, folderName);
                }
                
                try
                {
                    ret._fileSystem.CreateLeaf(folder, list.Name, list);
                }
                catch
                {
                    ret._fileSystem.CreateDuplicateLeaf(folder, list.Name, list);
                    change = true;
                }
            }

            if (change)
                ret.Save();
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Error deserializing auto gather lists:\n{e}");
            Communicator.PrintError($"[GatherBuddy Reborn] Auto gather lists failed to load and have been reset.");
            ret.Save();
        }

        ret.SetActiveItems();
        return ret;
    }
}
