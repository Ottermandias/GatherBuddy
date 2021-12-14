using GatherBuddy.Enums;
using GatherBuddy.Structs;
using GatherBuddy.Time;

namespace GatherBuddy.Classes;

public partial class Fish
{
    public Patch             Patch           { get; internal set; } = Patch.Unknown;
    public Weather[]         PreviousWeather { get; internal set; } = System.Array.Empty<Weather>();
    public Weather[]         CurrentWeather  { get; internal set; } = System.Array.Empty<Weather>();
    public Bait              InitialBait     { get; internal set; } = Bait.Unknown;
    public Fish[]            Mooches         { get; internal set; } = System.Array.Empty<Fish>();
    public (Fish, int)[]     Predators       { get; internal set; } = System.Array.Empty<(Fish, int)>();
    public int               IntuitionLength { get; internal set; } = 0;
    public RepeatingInterval Interval        { get; internal set; } = RepeatingInterval.Always;
    public Snagging          Snagging        { get; internal set; } = Snagging.Unknown;
    public HookSet           HookSet         { get; internal set; } = HookSet.Unknown;
    public BiteType          BiteType        { get; internal set; } = BiteType.Unknown;
    public SpearfishSize     Size            { get; internal set; } = SpearfishSize.Unknown;
    public SpearfishSpeed    Speed           { get; internal set; } = SpearfishSpeed.Unknown;
}
