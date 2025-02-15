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
    private const string FileName = "auto_gather_lists.json";
    private const string FileNameFallback = "gather_window.json";

    private readonly List<AutoGatherList> _lists = [];
    private readonly List<Gatherable> _activeItems = [];
    private readonly List<Gatherable> _sortedItems = [];
    private readonly List<(Gatherable Item, uint Quantity)> _fallbackItems = [];

    public ReadOnlyCollection<AutoGatherList> Lists => _lists.AsReadOnly();
    public ReadOnlyCollection<Gatherable> ActiveItems => _activeItems.AsReadOnly();
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

    private void OnActiveAlarmsChange()
        => SetActiveItems();

    public void SetActiveItems()
    {
        _activeItems.Clear();
        foreach (var item in _lists.Where(p => p.Enabled && !p.Fallback)
                     .SelectMany(p => p.Items)
                     .Where(i => !_activeItems.Contains(i)))
            _activeItems.Add(item);
        _sortedItems.Clear();
        _sortedItems.InsertRange(0, _activeItems);
        _sortDirty = true;

        var fallback = _lists
            .Where(p => p.Enabled && p.Fallback)
            .SelectMany(p => p.Items.Select(i => (Item: i, Quantity: p.Quantities[i])))
            .GroupBy(i => i.Item)
            .Select(x => (x.Key, (uint)Math.Min(x.Sum(g => g.Quantity), uint.MaxValue)));
        _fallbackItems.Clear();
        _fallbackItems.AddRange(fallback);
    }

    public IReadOnlyList<Gatherable> GetList()
    {
        if (!GatherBuddy.Config.SortGatherWindowByUptime)
            return _activeItems;

        if (_sortDirty)
            _sortedItems.StableSort((lhs, rhs)
                => GatherBuddy.UptimeManager.BestLocation(lhs).Interval.Compare(GatherBuddy.UptimeManager.BestLocation(rhs).Interval));

        return _sortedItems;
    }

    public uint GetTotalQuantitiesForItem(IGatherable item)
    {
        if (item is not Gatherable gatherable)
            return 0;

        uint total = 0;
        foreach (var list in _lists)
        {
            if (list.Enabled && !list.Fallback && list.Quantities.TryGetValue(gatherable, out var quantity))
            {
                total += quantity;
            }
        }

        return total;
    }

    public unsafe int GetInventoryCountForItem(IGatherable gatherable)
    {
        if (gatherable.ItemData.IsCollectable)
        {
            int count   = 0;
            var manager = InventoryManager.Instance();
            if (manager == null)
                return count;
            foreach (var inv in InventoryTypes)
            {
                var container = manager->GetInventoryContainer(inv);
                if (container == null || container->Loaded == 0)
                    continue;
                for (int i = 0; i < container->Size; i++)
                {
                    var item = container->GetInventorySlot(i);
                    if (item == null || item->ItemId == 0 || item->ItemId != gatherable.ItemId) continue;
        
                    count++;
                }
            }
        
            return count;
        }
        else
        {
            var inventory = InventoryManager.Instance();
            return inventory->GetInventoryItemCount(gatherable.ItemId);
        }
    }
    
    public List<InventoryType> InventoryTypes
    {
        get
        {
            List<InventoryType> types = new List<InventoryType>()
            {
                InventoryType.Inventory1,
                InventoryType.Inventory2,
                InventoryType.Inventory3,
                InventoryType.Inventory4,
            };
            return types;
        }
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
