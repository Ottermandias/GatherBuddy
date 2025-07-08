using GatherBuddy.Enums;

namespace GatherBuddy.Structs;

public struct FishOverrideData
{
    public required uint           ItemId;
    public          uint[]?        Transition;
    public          uint[]?        Weather;
    public          uint?          BaitId;
    public          Lure?          Lure;
    public          uint?          MoochId;
    public          int?           IntuitionLength;
    public          (uint, int)[]? Predators;
    public          int?           UptimeMinuteOfDayStart;
    public          int?           UptimeMinuteOfDayEnd;
    public          bool?          Snagging;
    public          HookSet        HookSet;
    public          BiteType       BiteType;
    public          byte?          MultiHookLower;
    public          byte?          MultiHookUpper;
    public          short?         Points;
}
