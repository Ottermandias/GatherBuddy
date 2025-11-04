using System;
using Newtonsoft.Json;

namespace GatherBuddy.AutoHookIntegration.Models;

public class AHBaseHookset
{
    [JsonProperty("RequiredStatus")]
    public uint RequiredStatus { get; set; }

    [JsonProperty("PatienceWeak")]
    public AHBaseBiteConfig PatienceWeak { get; set; } = new(AHHookType.Precision);

    [JsonProperty("PatienceStrong")]
    public AHBaseBiteConfig PatienceStrong { get; set; } = new(AHHookType.Powerful);

    [JsonProperty("PatienceLegendary")]
    public AHBaseBiteConfig PatienceLegendary { get; set; } = new(AHHookType.Powerful);

    [JsonProperty("UseDoubleHook")]
    public bool UseDoubleHook { get; set; }

    [JsonProperty("LetFishEscapeDoubleHook")]
    public bool LetFishEscapeDoubleHook { get; set; }

    [JsonProperty("DoubleWeak")]
    public AHBaseBiteConfig DoubleWeak { get; set; } = new(AHHookType.Double);

    [JsonProperty("DoubleStrong")]
    public AHBaseBiteConfig DoubleStrong { get; set; } = new(AHHookType.Double);

    [JsonProperty("DoubleLegendary")]
    public AHBaseBiteConfig DoubleLegendary { get; set; } = new(AHHookType.Double);

    [JsonProperty("UseTripleHook")]
    public bool UseTripleHook { get; set; }

    [JsonProperty("LetFishEscapeTripleHook")]
    public bool LetFishEscapeTripleHook { get; set; }

    [JsonProperty("TripleWeak")]
    public AHBaseBiteConfig TripleWeak { get; set; } = new(AHHookType.Triple);

    [JsonProperty("TripleStrong")]
    public AHBaseBiteConfig TripleStrong { get; set; } = new(AHHookType.Triple);

    [JsonProperty("TripleLegendary")]
    public AHBaseBiteConfig TripleLegendary { get; set; } = new(AHHookType.Triple);

    [JsonProperty("TimeoutMax")]
    public double TimeoutMax { get; set; }

    [JsonProperty("ChumTimeoutMax")]
    public double ChumTimeoutMax { get; set; }

    [JsonProperty("StopAfterCaught")]
    public bool StopAfterCaught { get; set; }

    [JsonProperty("StopAfterResetCount")]
    public bool StopAfterResetCount { get; set; }

    [JsonProperty("StopAfterCaughtLimit")]
    public int StopAfterCaughtLimit { get; set; } = 1;

    [JsonProperty("StopFishingStep")]
    public int StopFishingStep { get; set; }

    [JsonProperty("UseCustomStatusHook")]
    public bool UseCustomStatusHook { get; set; }

    [JsonProperty("CastLures")]
    public AHLuresConfig? CastLures { get; set; }

    public AHBaseHookset(uint requiredStatus = 0)
    {
        RequiredStatus = requiredStatus;
    }
}
