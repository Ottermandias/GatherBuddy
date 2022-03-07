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
        public string Annotation;
        public uint   ItemId;
        public uint   PreferLocation;
        public short  StartMinute;
        public short  EndMinute;

        [JsonConverter(typeof(StringEnumConverter))]
        public ObjectType Type;
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

    internal static bool FromConfig(Config cfg, out TimedGroupNode? timedGroupNode)
    {
        timedGroupNode       = null;
        var (item, location) = GatherBuddy.GameData.GetConfig(cfg.Type, cfg.ItemId, cfg.PreferLocation);
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
