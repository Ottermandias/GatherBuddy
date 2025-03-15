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
using Functions = GatherBuddy.Plugin.Functions;

namespace GatherBuddy.AutoGather.Lists;

public partial class AutoGatherListsManager : IDisposable
{
    public event Action? ActiveItemsChanged;

    private const string FileName = "auto_gather_lists.json";
    private const string FileNameFallback = "gather_window.json";

    private readonly List<AutoGatherList> _lists = [];
    private readonly List<(Gatherable Item, uint Quantity)> _activeItems = [];
    private readonly List<Gatherable> _sortedItems = [];
    private readonly List<(Gatherable Item, uint Quantity)> _fallbackItems = [];

    public ReadOnlyCollection<AutoGatherList> Lists => _lists.AsReadOnly();
    public ReadOnlyCollection<(Gatherable Item, uint Quantity)> ActiveItems => _activeItems.AsReadOnly();
    public ReadOnlyCollection<(Gatherable Item, uint Quantity)> FallbackItems => _fallbackItems.AsReadOnly();

    private          bool         _sortDirty = true;

    public AutoGatherListsManager()
    {
        GatherBuddy.UptimeManager.UptimeChange += OnUptimeChange;
    }

    public void Dispose()
        => GatherBuddy.UptimeManager.UptimeChange -= OnUptimeChange;

    private void OnUptimeChange(IGatherable item)
        => _sortDirty = true;

    public void SetActiveItems()
    {
        _activeItems.Clear();
        _sortedItems.Clear();
        _fallbackItems.Clear();
        var items = _lists
            .Where(l => l.Enabled)
            .SelectMany(l => l.Items.Select(i => (Item: i, Quantity: l.Quantities[i], l.Fallback)))
            .GroupBy(i => (i.Item, i.Fallback))
            .Select(x => (x.Key.Item, Quantity: (uint)Math.Min(x.Sum(g => g.Quantity), uint.MaxValue), x.Key.Fallback));
        
        foreach (var (item, quantity, fallback) in items)
        {
            if (fallback)
            {
                _fallbackItems.Add((item, quantity));
            }
            else
            {
                _activeItems.Add((item, quantity));
                _sortedItems.Add(item);
            }
        }

        _sortDirty = true;
        ActiveItemsChanged?.Invoke();
    }

    public IReadOnlyList<Gatherable> GetList()
    {
        if (_sortDirty && GatherBuddy.Config.SortGatherWindowByUptime)
        {
            _sortedItems.StableSort((lhs, rhs)
                => GatherBuddy.UptimeManager.BestLocation(lhs).Interval.Compare(GatherBuddy.UptimeManager.BestLocation(rhs).Interval));
            _sortDirty = false;
        }

        return _sortedItems;
    }

    [Obsolete]
    public uint GetTotalQuantitiesForItem(IGatherable item)
    {
        if (item is not Gatherable gatherable)
            return 0;

        return _activeItems.Where(x => x.Item == gatherable).Select(x => x.Quantity).FirstOrDefault();
    }
    
    public void Save()
    {
        var file = Functions.ObtainSaveFile(FileName);
        if (file == null)
            return;

        try
        {
            var text = JsonConvert.SerializeObject(_lists.Select(p => new AutoGatherList.Config(p)), Formatting.Indented);
            File.WriteAllText(file.FullName, text);
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Error serializing auto-gather lists data:\n{e}");
        }
    }

    public static AutoGatherListsManager Load()
    {
        var ret  = new AutoGatherListsManager();
        var file = Functions.ObtainSaveFile(FileName);
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
            ret._lists.Capacity = data.Length;
            foreach (var cfg in data)
            {
                change |= AutoGatherList.FromConfig(cfg, out var list);
                ret._lists.Add(list);
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
