using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dalamud.Logging;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using ImGuiOtter.Table;
using Newtonsoft.Json;

namespace GatherBuddy.GatherHelper;

public partial class GatherWindowManager : IDisposable
{
    public const string FileName = "gather_window.json";

    public List<GatherWindowPreset> Presets = new();

    public List<IGatherable> ActiveItems = new();
    public List<IGatherable> SortedItems = new();

    private bool _sortDirty = true;

    public GatherWindowManager()
        => GatherBuddy.UptimeManager.UptimeChange += OnUptimeChange;

    public void Dispose()
        => GatherBuddy.UptimeManager.UptimeChange -= OnUptimeChange;

    private void OnUptimeChange(IGatherable item)
        => _sortDirty = true;


    public void SetActiveItems()
    {
        ActiveItems.Clear();
        foreach (var item in Presets.Where(p => p.Enabled)
                     .SelectMany(p => p.Items)
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
            PluginLog.Error($"Error serializing gather window data:\n{e}");
        }
    }

    public static GatherWindowManager Load()
    {
        var ret  = new GatherWindowManager();
        var file = Functions.ObtainSaveFile(FileName);
        if (file is not { Exists: true })
        {
            ret.Save();
            return ret;
        }

        try
        {
            var text = File.ReadAllText(file.FullName);
            var data = JsonConvert.DeserializeObject<GatherWindowPreset.Config[]>(text);
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
            PluginLog.Error($"Error deserializing gather window data:\n{e}");
            ret.Save();
        }

        ret.SetActiveItems();
        return ret;
    }
}
