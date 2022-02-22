using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GatherBuddy.Plugin;
using Newtonsoft.Json;

namespace GatherBuddy.GatherGroup;

public class TimedGroup
{
    public string Name        { get; set; }
    public string Description { get; set; } = string.Empty;

    public TimedGroup(string name)
        => Name = name;

    public List<TimedGroupNode> Nodes { get; init; } = new();

    public TimedGroupNode? CurrentNode(uint eorzeaMinuteOfDay)
        => Nodes.FirstOrDefault(node => node.IsUp(eorzeaMinuteOfDay));

    public TimedGroup Clone(string newName)
        => new(newName)
        {
            Description = Description,
            Nodes       = Nodes.Select(n => n.Clone()).ToList(),
        };

    // Returns true if no problems arose, false if some nodes were invalid.
    internal static bool FromConfig(Config cfg, out TimedGroup group)
    {
        group = new TimedGroup(cfg.Name)
        {
            Description = cfg.Description,
        };
        var ret = true;
        foreach (var configNode in cfg.Nodes)
        {
            if (!TimedGroupNode.FromConfig(configNode, out var node))
            {
                ret = false;
                continue;
            }

            group.Nodes.Add(node!);
        }

        return ret;
    }

    internal struct Config
    {
        public const byte                    CurrentVersion = 1;
        public       string                  Name;
        public       string                  Description;
        public       TimedGroupNode.Config[] Nodes;

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

    internal Config ToConfig()
        => new()
        {
            Name        = Name,
            Description = Description,
            Nodes       = Nodes.Select(n => n.ToConfig()).ToArray(),
        };
};
