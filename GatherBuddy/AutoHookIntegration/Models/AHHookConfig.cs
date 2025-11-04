using System;
using Newtonsoft.Json;

namespace GatherBuddy.AutoHookIntegration.Models;

public class AHHookConfig
{
    [JsonProperty("UniqueId")]
    public Guid UniqueId { get; set; } = Guid.NewGuid();

    [JsonProperty("Enabled")]
    public bool Enabled { get; set; } = true;

    [JsonProperty("BaitFish")]
    public AHBaitFishClass BaitFish { get; set; } = new();

    [JsonProperty("NormalHook")]
    public AHBaseHookset NormalHook { get; set; } = new(0);

    [JsonProperty("IntuitionHook")]
    public AHBaseHookset IntuitionHook { get; set; } = new(762);

    public AHHookConfig()
    {
    }

    public AHHookConfig(AHBaitFishClass baitFish)
    {
        BaitFish = baitFish;
    }

    public AHHookConfig(int baitFishId)
    {
        BaitFish = new AHBaitFishClass(baitFishId);
    }
}
