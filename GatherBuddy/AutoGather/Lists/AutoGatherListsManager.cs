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

    private const string FileName         = "auto_gather_lists.json";
    private const string FileNameFallback = "gather_window.json";

    private readonly List<AutoGatherList>                   _lists         = [];
    private readonly List<(Gatherable Item, uint Quantity)> _activeItems   = [];
    private readonly List<(Gatherable Item, uint Quantity)> _fallbackItems = [];
    private readonly List<(Fish Fish, uint Quantity)>       _activeFish    = [];

    public ReadOnlyCollection<AutoGatherList> Lists
        => _lists.AsReadOnly();

    public ReadOnlyCollection<(Gatherable Item, uint Quantity)> ActiveItems
        => _activeItems.AsReadOnly();

    public ReadOnlyCollection<(Gatherable Item, uint Quantity)> FallbackItems
        => _fallbackItems.AsReadOnly();

    public ReadOnlyCollection<(Fish Fish, uint Quantity)> ActiveFish
        => _activeFish.AsReadOnly();

    public AutoGatherListsManager()
    { }

    public void Dispose()
    { }

    public void SetActiveItems()
    {
        _activeItems.Clear();
        _activeFish.Clear();
        _fallbackItems.Clear();
        var items = _lists
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
