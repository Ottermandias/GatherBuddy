using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using GatherBuddy.Alarms;
using GatherBuddy.Classes;
using GatherBuddy.GatherGroup;
using GatherBuddy.Plugin;
using Newtonsoft.Json;

namespace GatherBuddy.AutoGather.Lists;

public class AutoGatherList
{
    public ReadOnlyCollection<Gatherable> Items => items.AsReadOnly();
    public ReadOnlyDictionary<Gatherable, uint> Quantities => quantities.AsReadOnly();
    public ReadOnlyDictionary<Gatherable, GatheringNode> PreferredLocations => preferredLocations.AsReadOnly();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Enabled { get; set; } = false;
    public bool Fallback { get; set; } = false;

    private List<Gatherable> items = [];
    private Dictionary<Gatherable, uint> quantities = [];
    private Dictionary<Gatherable, GatheringNode> preferredLocations = [];

    public AutoGatherList Clone()
        => new()
        {
            items = new(items),
            quantities = new(quantities),
            preferredLocations = new(preferredLocations),
            Name = Name,
            Description = Description,
            Enabled = false,
            Fallback = Fallback
        };

    public bool Add(Gatherable item, uint quantity = 1)
    {
        if (quantities.ContainsKey(item))
            return false;

        items.Add(item);
        quantities[item] = NormalizeQuantity(item, quantity);
        return true;
    }

    public void RemoveAt(int index)
    {
        var item = items[index];
        quantities.Remove(item);
        preferredLocations.Remove(item);
        items.RemoveAt(index);
    }

    public bool Replace(int index, Gatherable item)
    {
        if (quantities.ContainsKey(item))
            return false;

        var old = items[index];
        quantities.Remove(old, out var quantity);
        preferredLocations.Remove(old);
        items[index] = item;
        quantities[item] = NormalizeQuantity(item, quantity);

        return true;
    }

    public bool Move(int from, int to)
    {
        return Functions.Move(items, from, to);
    }

    public bool SetQuantity(Gatherable item, uint quantity)
    {
        if (quantities.TryGetValue(item, out var old))
        {
            quantity = NormalizeQuantity(item, quantity);

            if (old != quantity)
            {
                quantities[item] = quantity;
                return true;
            }
        }
        return false;
    }

    private static uint NormalizeQuantity(Gatherable item, uint quantity)
    {
        if (quantity < 1)
            quantity = 1;
        if (quantity > 9999)
            quantity = 9999;
        if (quantity > 1 && item.IsTreasureMap)
            quantity = 1;
        return quantity;
    }

    public bool SetPreferredLocation(Gatherable item, GatheringNode? location)
    {
        if (quantities.ContainsKey(item))
        {
            var old = preferredLocations.GetValueOrDefault(item);
            if (old != location)
            {
                if (location == null)
                    preferredLocations.Remove(item);
                else
                    preferredLocations[item] = location;

                return true;
            }
        }
        return false;
    }

    public bool HasItems()
        => Enabled && Items.Count > 0;

    public struct Config(AutoGatherList list)
    {
        public const byte CurrentVersion = 4;

        public uint[] ItemIds = list.Items.Select(i => i.ItemId).ToArray();
        public Dictionary<uint, uint> Quantities = list.Quantities.ToDictionary(v => v.Key.ItemId, v => v.Value);
        public Dictionary<uint, uint> PrefferedLocations = list.PreferredLocations.ToDictionary(v => v.Key.ItemId, v => v.Value.Id);
        public string Name = list.Name;
        public string Description = list.Description;
        public bool Enabled = list.Enabled;
        public bool Fallback = list.Fallback;

        internal readonly string ToBase64()
        {
            var json = JsonConvert.SerializeObject(this);
            var bytes = Encoding.UTF8.GetBytes(json).Prepend(CurrentVersion).ToArray();
            return Functions.CompressedBase64(bytes);
        }

        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        internal static bool FromBase64(string data, out Config cfg)
        {
            cfg = default;
            try
            {
                var bytes = Functions.DecompressedBase64(data);
                if (bytes.Length == 0 || bytes[0] != CurrentVersion)
                    return false;

                var json = Encoding.UTF8.GetString(bytes.AsSpan()[1..]);
                cfg = JsonConvert.DeserializeObject<Config>(json);
                if (cfg.ItemIds == null || cfg.Name == null || cfg.Description == null || cfg.Quantities == null || cfg.PrefferedLocations == null)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public static bool FromConfig(Config cfg, out AutoGatherList list)
    {
        list = new AutoGatherList()
        {
            Name = cfg.Name,
            Description = cfg.Description,
            Enabled = cfg.Enabled,
            Fallback = cfg.Fallback,
            items = new(cfg.ItemIds.Length),
            quantities = new(cfg.ItemIds.Length),
            preferredLocations = new(cfg.PrefferedLocations.Count)
        };
        var changes = false;
        foreach (var itemId in cfg.ItemIds)
        {
            uint quantity;
            if (GatherBuddy.GameData.Gatherables.TryGetValue(itemId, out var item) && list.Add(item, quantity = cfg.Quantities.GetValueOrDefault(item.ItemId)))
            {
                changes |= list.quantities[item] != quantity;
                if (cfg.PrefferedLocations.TryGetValue(itemId, out var locId))
                {
                    if (item.NodeList.Where(n => n.Id == locId).FirstOrDefault() is var loc and not null)
                        list.SetPreferredLocation(item, loc);
                    else
                        changes = true;
                }
            }
            else
            {
                changes = true;
            }
        }

        return changes;
    }

    public AutoGatherList()
    { }


    public AutoGatherList(TimedGroup group)
    {
        Name = group.Name;
        Description = group.Description;
        foreach (var item in group.Nodes.Select(n => n.Item).OfType<Gatherable>())
            Add(item);
    }

    public AutoGatherList(AlarmGroup group)
    {
        Name = group.Name;
        Description = group.Description;
        foreach (var item in group.Alarms.Select(n => n.Item).OfType<Gatherable>())
            Add(item);
    }
}
