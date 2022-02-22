using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GatherBuddy.Plugin;
using Newtonsoft.Json;

namespace GatherBuddy.Alarms;

public class AlarmGroup
{
    public string Name        { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<Alarm> Alarms { get; set; } = new();

    public bool Enabled { get; set; }

    public AlarmGroup Clone()
        => new()
        {
            Name        = Name,
            Description = Description,
            Alarms      = Alarms.Select(a => a.Clone()).ToList(),
            Enabled     = false,
        };

    internal struct Config
    {
        public const byte                      CurrentVersion = 1;
        public       string                    Name        { get; set; }
        public       string                    Description { get; set; }
        public       IEnumerable<Alarm.Config> Alarms      { get; set; }
        public       bool                      Enabled     { get; set; }

        public Config(AlarmGroup group)
        {
            Name        = group.Name;
            Description = group.Description;
            Enabled     = group.Enabled;
            Alarms      = group.Alarms.Select(a => new Alarm.Config(a));
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
}
