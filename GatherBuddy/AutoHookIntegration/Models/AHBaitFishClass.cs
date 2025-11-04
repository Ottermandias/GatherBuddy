using Newtonsoft.Json;

namespace GatherBuddy.AutoHookIntegration.Models;

public class AHBaitFishClass
{
    [JsonProperty("Id")]
    public int Id { get; set; }

    public AHBaitFishClass()
    {
        Id = -1;
    }

    public AHBaitFishClass(int id)
    {
        Id = id;
    }

    public AHBaitFishClass(uint id)
    {
        Id = (int)id;
    }
}
