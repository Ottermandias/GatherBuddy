using Newtonsoft.Json;

namespace GatherBuddy.AutoHookIntegration.Models;

public class AHAutoMooch
{
    [JsonProperty("Enabled")]
    public bool Enabled { get; set; }
    
    [JsonProperty("Id")]
    public uint Id { get; set; }
    
    public AHAutoMooch()
    {
        Enabled = false;
        Id = 0;
    }
    
    public AHAutoMooch(uint moochFishId)
    {
        Enabled = true;
        Id = moochFishId;
    }
}

public class AHAutoSurfaceSlap
{
    [JsonProperty("Enabled")]
    public bool Enabled { get; set; }
    
    public AHAutoSurfaceSlap()
    {
        Enabled = false;
    }
    
    public AHAutoSurfaceSlap(bool enabled)
    {
        Enabled = enabled;
    }
}

public class AHAutoIdenticalCast
{
    [JsonProperty("Enabled")]
    public bool Enabled { get; set; }
    
    public AHAutoIdenticalCast()
    {
        Enabled = false;
    }
}

public class AHFishConfig
{
    [JsonProperty("Enabled")]
    public bool Enabled { get; set; } = true;
    
    [JsonProperty("Fish")]
    public AHBaitFishClass Fish { get; set; }

    [JsonProperty("IdenticalCast")]
    public AHAutoIdenticalCast IdenticalCast { get; set; } = new();
    
    [JsonProperty("SurfaceSlap")]
    public AHAutoSurfaceSlap SurfaceSlap { get; set; } = new();

    [JsonProperty("Mooch")]
    public AHAutoMooch Mooch { get; set; } = new();
    
    [JsonProperty("NeverMooch")]
    public bool NeverMooch { get; set; } = false;

    public AHFishConfig(int fishId)
    {
        Fish = new AHBaitFishClass(fishId);
    }
}
