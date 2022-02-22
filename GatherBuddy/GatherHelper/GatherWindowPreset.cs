using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GatherBuddy.Alarms;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using Newtonsoft.Json;

namespace GatherBuddy.GatherHelper;

public class GatherWindowPreset
{
    public List<IGatherable> Items       { get; init; } = new();
    public string            Name        { get; set; }  = string.Empty;
    public string            Description { get; set; }  = string.Empty;
    public bool              Enabled     { get; set; }  = false;

    public GatherWindowPreset Clone()
        => new()
        {
            Name        = Name,
            Description = Description,
            Enabled     = false,
            Items       = Items.ToList(),
        };

    public bool Add(IGatherable item)
    {
        if (Items.Contains(item))
            return false;

        Items.Add(item);
        return true;
    }

    public bool HasItems()
        => Enabled && Items.Count > 0;

    public struct Config
    {
        public const byte CurrentVersion = 1;

        public uint[]       ItemIds;
        public ObjectType[] ItemTypes;
        public string       Name;
        public string       Description;
        public bool         Enabled;

        public Config(GatherWindowPreset preset)
        {
            ItemIds     = preset.Items.Select(i => i.ItemId).ToArray();
            ItemTypes   = preset.Items.Select(i => i.Type).ToArray();
            Name        = preset.Name;
            Description = preset.Description;
            Enabled     = preset.Enabled;
        }

        internal string ToBase64()
        {
            var json  = JsonConvert.SerializeObject(this);
            var bytes = Encoding.UTF8.GetBytes(json).Prepend(CurrentVersion).ToArray();
            return Functions.CompressedBase64(bytes);
        }

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
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public static bool FromConfig(Config cfg, out GatherWindowPreset ret)
    {
        ret = new GatherWindowPreset()
        {
            Name        = cfg.Name,
            Description = cfg.Description,
            Enabled     = cfg.Enabled,
        };
        var maxLength = Math.Min(cfg.ItemIds.Length, cfg.ItemTypes.Length);
        var changes   = cfg.ItemIds.Length != cfg.ItemTypes.Length;
        for (var i = 0; i < maxLength; ++i)
        {
            IGatherable? gatherable = cfg.ItemTypes[i] switch
            {
                ObjectType.Gatherable => GatherBuddy.GameData.Gatherables.TryGetValue(cfg.ItemIds[i], out var item) ? item : null,
                ObjectType.Fish       => GatherBuddy.GameData.Fishes.TryGetValue(cfg.ItemIds[i], out var item) ? item : null,
                _                     => null,
            };
            if (gatherable == null)
                changes = true;
            else if (!ret.Add(gatherable))
                changes = true;
        }

        return changes;
    }

    public GatherWindowPreset()
    { }

    public GatherWindowPreset(GatherGroup.TimedGroup group)
    {
        Name        = group.Name;
        Description = group.Description;
        Items       = new List<IGatherable>(group.Nodes.Count);
        foreach (var item in group.Nodes.Select(n => n.Item))
            Add(item);
    }

    public GatherWindowPreset(AlarmGroup group)
    {
        Name        = group.Name;
        Description = group.Description;
        Items       = new List<IGatherable>(group.Alarms.Count);
        foreach (var item in group.Alarms.Select(n => n.Item))
            Add(item);
    }
}
