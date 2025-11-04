using Newtonsoft.Json;

namespace GatherBuddy.AutoHookIntegration.Models;

public class AHAutoCastsConfig
{
    [JsonProperty("EnableAll")]
    public bool EnableAll { get; set; } = true;

    [JsonProperty("DontCancelMooch")]
    public bool DontCancelMooch { get; set; } = true;
    
    [JsonProperty("CastMooch")]
    public AHAutoMoochCast? CastMooch { get; set; }

    [JsonProperty("CastPatience")]
    public AHAutoPatience? CastPatience { get; set; }

    [JsonProperty("CastCollect")]
    public AHAutoCollect? CastCollect { get; set; }

    [JsonProperty("CastCordial")]
    public AHAutoCordial? CastCordial { get; set; }

    [JsonProperty("CastPrizeCatch")]
    public AHAutoPrizeCatch? CastPrizeCatch { get; set; }

    [JsonProperty("CastThaliaksFavor")]
    public AHAutoThaliaksFavor? CastThaliaksFavor { get; set; }
}

public class AHAutoPatience
{
    [JsonProperty("Enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("PatienceVersion")]
    public int PatienceVersion { get; set; } = 2;
}

public class AHAutoCollect
{
    [JsonProperty("Enabled")]
    public bool Enabled { get; set; }
}

public class AHAutoCordial
{
    [JsonProperty("Enabled")]
    public bool Enabled { get; set; }
}

public class AHAutoPrizeCatch
{
    [JsonProperty("Enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("UseWhenMoochIIOnCD")]
    public bool UseWhenMoochIIOnCD { get; set; }

    [JsonProperty("UseOnlyWithIdenticalCast")]
    public bool UseOnlyWithIdenticalCast { get; set; }

    [JsonProperty("UseOnlyWithActiveSlap")]
    public bool UseOnlyWithActiveSlap { get; set; }
}

public class AHAutoThaliaksFavor
{
    [JsonProperty("Enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("ThaliaksFavorStacks")]
    public int ThaliaksFavorStacks { get; set; } = 3;

    [JsonProperty("ThaliaksFavorRecover")]
    public int ThaliaksFavorRecover { get; set; } = 150;

    [JsonProperty("UseWhenCordialCD")]
    public bool UseWhenCordialCD { get; set; }
}

public class AHAutoMoochCast
{
    [JsonProperty("Enabled")]
    public bool Enabled { get; set; }
}
