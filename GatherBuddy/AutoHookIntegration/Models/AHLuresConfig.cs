using Newtonsoft.Json;

namespace GatherBuddy.AutoHookIntegration.Models;

public class AHLuresConfig
{
    [JsonProperty("Enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("AmbitiousLureEnabled")]
    public bool AmbitiousLureEnabled { get; set; }

    [JsonProperty("ModestLureEnabled")]
    public bool ModestLureEnabled { get; set; }
}
