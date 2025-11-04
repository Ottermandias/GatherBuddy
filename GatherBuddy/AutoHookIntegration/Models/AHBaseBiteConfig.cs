using Newtonsoft.Json;

namespace GatherBuddy.AutoHookIntegration.Models;

public class AHBaseBiteConfig
{
    [JsonProperty("HooksetEnabled")]
    public bool HooksetEnabled { get; set; } = false;

    [JsonProperty("EnableHooksetSwap")]
    public bool EnableHooksetSwap { get; set; }

    [JsonProperty("HookTimerEnabled")]
    public bool HookTimerEnabled { get; set; }

    [JsonProperty("MinHookTimer")]
    public double MinHookTimer { get; set; }

    [JsonProperty("MaxHookTimer")]
    public double MaxHookTimer { get; set; }

    [JsonProperty("ChumTimerEnabled")]
    public bool ChumTimerEnabled { get; set; }

    [JsonProperty("ChumMinHookTimer")]
    public double ChumMinHookTimer { get; set; }

    [JsonProperty("ChumMaxHookTimer")]
    public double ChumMaxHookTimer { get; set; }

    [JsonProperty("OnlyWhenActiveSlap")]
    public bool OnlyWhenActiveSlap { get; set; }

    [JsonProperty("OnlyWhenNotActiveSlap")]
    public bool OnlyWhenNotActiveSlap { get; set; }

    [JsonProperty("OnlyWhenActiveIdentical")]
    public bool OnlyWhenActiveIdentical { get; set; }

    [JsonProperty("OnlyWhenNotActiveIdentical")]
    public bool OnlyWhenNotActiveIdentical { get; set; }

    [JsonProperty("PrizeCatchReq")]
    public bool PrizeCatchReq { get; set; }

    [JsonProperty("PrizeCatchNotReq")]
    public bool PrizeCatchNotReq { get; set; }

    [JsonProperty("HooksetType")]
    public AHHookType HooksetType { get; set; }

    public AHBaseBiteConfig(AHHookType type)
    {
        HooksetType = type;
    }
}
