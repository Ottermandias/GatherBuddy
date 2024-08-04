using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Alarms;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using Newtonsoft.Json;
using OtterGui;
using Functions = GatherBuddy.Plugin.Functions;

namespace GatherBuddy.GatherHelper;

public partial class GatherWindowManager : IDisposable
{
    public const string FileName = "gather_window.json";

    public List<GatherWindowPreset> Presets = new();

    public List<IGatherable> ActiveItems = new();
    public List<IGatherable> SortedItems = new();

    private readonly AlarmManager _alarms;
    private          bool         _sortDirty = true;

    public GatherWindowManager(AlarmManager alarms)
    {
        _alarms                                =  alarms;
        GatherBuddy.UptimeManager.UptimeChange += OnUptimeChange;
        SetShowGatherWindowAlarms(GatherBuddy.Config.ShowGatherWindowAlarms);
    }

    public void SetShowGatherWindowAlarms(bool value)
    {
        if (value)
            _alarms.ActiveAlarmsChanged += OnActiveAlarmsChange;
        else
            _alarms.ActiveAlarmsChanged -= OnActiveAlarmsChange;
        SetActiveItems();
    }

    public void Dispose()
        => GatherBuddy.UptimeManager.UptimeChange -= OnUptimeChange;

    private void OnUptimeChange(IGatherable item)
        => _sortDirty = true;

    private void OnActiveAlarmsChange()
        => SetActiveItems();

    public void SetActiveItems()
    {
        ActiveItems.Clear();
        foreach (var item in Presets.Where(p => p.Enabled)
                     .SelectMany(p => p.Items)
                     .Where(i => !ActiveItems.Contains(i)))
            ActiveItems.Add(item);
        if (GatherBuddy.Config.ShowGatherWindowAlarms && GatherBuddy.Config.AlarmsEnabled)
            foreach (var item in _alarms.ActiveAlarms.Select(a => a.Item1.Item)
                         .Where(i => !ActiveItems.Contains(i)))
                ActiveItems.Add(item);
        SortedItems.Clear();
        SortedItems.InsertRange(0, ActiveItems);
        _sortDirty = true;
    }

    public IList<IGatherable> GetList()
    {
        if (!GatherBuddy.Config.SortGatherWindowByUptime)
            return ActiveItems;

        if (_sortDirty)
            SortedItems.StableSort((lhs, rhs)
                => GatherBuddy.UptimeManager.BestLocation(lhs).Interval.Compare(GatherBuddy.UptimeManager.BestLocation(rhs).Interval));

        return SortedItems;
    }

    public uint GetTotalQuantitiesForItem(IGatherable gatherable)
    {
        uint total = 0;
        foreach (var preset in Presets)
        {
            if (!preset.Enabled)
                continue;
            if (preset.Quantities.TryGetValue(gatherable.ItemId, out var quantity))
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
            var text = JsonConvert.SerializeObject(Presets.Select(p => new GatherWindowPreset.Config(p)), Formatting.Indented);
            File.WriteAllText(file.FullName, text);
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Error serializing gather window data:\n{e}");
        }
    }

    public static GatherWindowManager Load(AlarmManager alarms)
    {
        var ret  = new GatherWindowManager(alarms);
        var file = Functions.ObtainSaveFile(FileName);
        if (file is not { Exists: true })
        {
            ret.Save();
            return ret;
        }

        try
        {
            var text = File.ReadAllText(file.FullName);
            var data = JsonConvert.DeserializeObject<GatherWindowPreset.Config[]>(text)!;
            ret.Presets.Capacity = data.Length;
            var change = false;
            foreach (var cfg in data)
            {
                change |= GatherWindowPreset.FromConfig(cfg, out var preset);
                ret.Presets.Add(preset);
            }

            if (change)
                ret.Save();
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Error deserializing gather window data:\n{e}");
            Communicator.PrintError($"[GatherBuddy Reborn] Gather Window Presets failed to load. Gathering Lists have been reset.");
            ret.Save();
        }

        ret.SetActiveItems();
        return ret;
    }
}
