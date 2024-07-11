using GatherBuddy.Enums;
using GatherBuddy.Structs;
using GatherBuddy.Time;
using OtterGui.Classes;

namespace GatherBuddy.Classes;

public partial class Fish
{
    public Patch             Patch           { get; internal set; } = Patch.Unknown;
    public Weather[]         PreviousWeather { get; internal set; } = [];
    public Weather[]         CurrentWeather  { get; internal set; } = [];
    public Bait              InitialBait     { get; internal set; } = Bait.Unknown;
    public Fish[]            Mooches         { get; internal set; } = [];
    public (Fish, int)[]     Predators       { get; internal set; } = [];
    public int               IntuitionLength { get; internal set; } = 0;
    public RepeatingInterval Interval        { get; internal set; } = RepeatingInterval.Always;
    public Snagging          Snagging        { get; internal set; } = Snagging.Unknown;
    public Lure              Lure            { get; internal set; } = Lure.None;
    public HookSet           HookSet         { get; internal set; } = HookSet.Unknown;
    public BiteType          BiteType        { get; internal set; } = BiteType.Unknown;
    public SpearfishSize     Size            { get; internal set; } = SpearfishSize.Unknown;
    public SpearfishSpeed    Speed           { get; internal set; } = SpearfishSpeed.Unknown;
    public Fish?             SurfaceSlap     { get; internal set; } = null;
    public string            Guide           { get; internal set; } = string.Empty;
    public OceanTime         OceanTime       { get; internal set; } = OceanTime.Always; 

    internal OptionalBool BigFishOverride { get; set; } = null;
}
