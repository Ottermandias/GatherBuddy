using System;
using GatherBuddy.Interfaces;
using GatherBuddy.Time;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatherBuddy.GatherGroup;

public class TimedGroupNode
{
    public IGatherable Item;
    public ILocation?  PreferLocation;
    public int         EorzeaStartMinute;
    public int         EorzeaEndMinute;
    public string      Annotation = string.Empty;

    public TimedGroupNode(IGatherable item)
        => Item = item;

    public bool IsUp(uint eorzeaMinuteOfDay)
    {
        if (EorzeaStartMinute == EorzeaEndMinute)
            return true;

        if (EorzeaStartMinute < EorzeaEndMinute)
            return eorzeaMinuteOfDay >= EorzeaStartMinute && eorzeaMinuteOfDay < EorzeaEndMinute;

        return eorzeaMinuteOfDay >= EorzeaStartMinute || eorzeaMinuteOfDay < EorzeaEndMinute;
    }

    public int Length()
    {
        if (EorzeaStartMinute == EorzeaEndMinute)
            return RealTime.MinutesPerDay;

        if (EorzeaStartMinute < EorzeaEndMinute)
            return EorzeaEndMinute - EorzeaStartMinute;

        return RealTime.MinutesPerDay - EorzeaStartMinute + EorzeaEndMinute;
    }

    public TimedGroupNode Clone()
        => new(Item)
        {
            EorzeaStartMinute = EorzeaStartMinute,
            EorzeaEndMinute   = EorzeaEndMinute,
            Annotation        = Annotation,
        };

    internal struct Config
    {
        public uint ItemId;
        public uint PreferLocation;

        [JsonConverter(typeof(StringEnumConverter))]
        public ObjectType Type;

        public short  StartMinute;
        public short  EndMinute;
        public string Annotation;
    }

    internal Config ToConfig()
        => new()
        {
            ItemId         = Item.ItemId,
            Type           = Item.Type,
            PreferLocation = PreferLocation?.Id ?? 0,
            StartMinute    = (short)EorzeaStartMinute,
            EndMinute      = (short)EorzeaEndMinute,
            Annotation     = Annotation,
        };

    private static (IGatherable?, ILocation?) GetNodeData(Config cfg)
    {
        var        item = GatherBuddy.GameData.Gatherables.TryGetValue(cfg.ItemId, out var i) ? i : null;
        ILocation? loc  = null;
        if (cfg.PreferLocation != 0 && GatherBuddy.GameData.GatheringNodes.TryGetValue(cfg.PreferLocation, out var l))
            loc = l;
        return (item, loc);
    }

    private static (IGatherable?, ILocation?) GetFishData(Config cfg)
    {
        var        item = GatherBuddy.GameData.Fishes.TryGetValue(cfg.ItemId, out var f) ? f : null;
        ILocation? loc  = null;
        if (cfg.PreferLocation != 0 && GatherBuddy.GameData.FishingSpots.TryGetValue(cfg.PreferLocation, out var l))
            loc = l;
        return (item, loc);
    }

    internal static bool FromConfig(Config cfg, out TimedGroupNode? timedGroupNode)
    {
        timedGroupNode = null;
        var (item, location) = cfg.Type switch
        {
            ObjectType.Gatherable => GetNodeData(cfg),
            ObjectType.Fish       => GetFishData(cfg),
            _                     => (null, null),
        };
        if (item == null || location == null && cfg.PreferLocation != 0)
            return false;

        timedGroupNode = new TimedGroupNode(item)
        {
            PreferLocation    = location,
            EorzeaStartMinute = Math.Clamp((int)cfg.StartMinute, 0, RealTime.MinutesPerDay - 1),
            EorzeaEndMinute   = Math.Clamp((int)cfg.EndMinute,   0, RealTime.MinutesPerDay - 1),
            Annotation        = cfg.Annotation,
        };
        return true;
    }
}
