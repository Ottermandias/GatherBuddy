using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using GatherBuddy.Time;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatherBuddy.Alarms;

public class Alarm
{
    public IGatherable Item           { get; set; }
    public ILocation?  PreferLocation { get; set; }
    public string      Name           { get; set; } = string.Empty;
    public int         SecondOffset   { get; set; }
    public Sounds      SoundId        { get; set; }
    public bool        Enabled        { get; set; }
    public bool        PrintMessage   { get; set; }

    public Alarm(IGatherable item)
        => Item = item;

    public Alarm Clone()
        => new(Item)
        {
            Name           = Name,
            PreferLocation = PreferLocation,
            SecondOffset   = SecondOffset,
            SoundId        = SoundId,
            Enabled        = false,
            PrintMessage   = PrintMessage,
        };

    public void SendMessage(ILocation location, TimeInterval uptime)
    {
        if (!PrintMessage)
            return;

        if (!Dalamud.Conditions.Any())
            return;

        Communicator.PrintAlarmMessage(this, location, uptime);
    }

    internal struct Config
    {
        public string Name = string.Empty;
        public uint   Id;
        public int    SecondOffset;

        [JsonConverter(typeof(StringEnumConverter))]
        public ObjectType Type;

        public uint   PreferLocationId;
        public Sounds SoundId;
        public bool   Enabled;
        public bool   PrintMessage;

        public Config(Alarm a)
        {
            Name             = a.Name;
            Id               = a.Item.ItemId;
            PreferLocationId = a.PreferLocation?.Id ?? 0;
            Type             = a.Item.Type;
            SecondOffset     = a.SecondOffset;
            SoundId          = a.SoundId;
            Enabled          = a.Enabled;
            PrintMessage     = a.PrintMessage;
        }
    }

    internal static bool FromConfig(Config config, out Alarm? alarm)
    {
        alarm                = null;
        var (item, location) = GatherBuddy.GameData.GetConfig(config.Type, config.Id, config.PreferLocationId);
        if (item is not { InternalLocationId: > 0 } || location == null && config.PreferLocationId != 0)
            return false;

        alarm = new Alarm(item)
        {
            Name           = config.Name ?? string.Empty,
            PreferLocation = location,
            Enabled        = config.Enabled,
            PrintMessage   = config.PrintMessage,
            SecondOffset   = config.SecondOffset,
            SoundId        = config.SoundId,
        };

        return true;
    }
}
