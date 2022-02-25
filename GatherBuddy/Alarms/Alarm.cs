using System;
using Dalamud.Logging;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using GatherBuddy.Time;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatherBuddy.Alarms;

public class Alarm
{
    public IGatherable Item         { get; set; }
    public string      Name         { get; set; } = string.Empty;
    public int         SecondOffset { get; set; }
    public Sounds      SoundId      { get; set; }
    public bool        Enabled      { get; set; }
    public bool        PrintMessage { get; set; }

    public Alarm(IGatherable item)
        => Item = item;

    public Alarm Clone()
        => new(Item)
        {
            Name         = Name,
            SecondOffset = SecondOffset,
            SoundId      = SoundId,
            Enabled      = false,
            PrintMessage = PrintMessage,
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
        public uint Id;

        [JsonConverter(typeof(StringEnumConverter))]
        public ObjectType Type;

        public string Name;
        public int    SecondOffset;
        public Sounds SoundId;
        public bool   Enabled;
        public bool   PrintMessage;

        public Config(Alarm a)
        {
            Name         = a.Name;
            Id           = a.Item.ItemId;
            Type         = a.Item.Type;
            SecondOffset = a.SecondOffset;
            SoundId      = a.SoundId;
            Enabled      = a.Enabled;
            PrintMessage = a.PrintMessage;
        }
    }

    internal static bool FromConfig(Config config, out Alarm? alarm)
    {
        alarm = null;
        IGatherable? item = config.Type switch
        {
            ObjectType.Gatherable => GatherBuddy.GameData.Gatherables.TryGetValue(config.Id, out var i) ? i : null,
            ObjectType.Fish       => GatherBuddy.GameData.Fishes.TryGetValue(config.Id, out var i) ? i : null,
            _                     => null,
        };
        if (item is not { InternalLocationId: > 0 })
            return false;

        alarm = new Alarm(item)
        {
            Name         = config.Name,
            Enabled      = config.Enabled,
            PrintMessage = config.PrintMessage,
            SecondOffset = config.SecondOffset,
            SoundId      = config.SoundId,
        };

        return true;
    }
}
