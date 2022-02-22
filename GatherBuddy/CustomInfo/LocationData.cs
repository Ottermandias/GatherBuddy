using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatherBuddy.CustomInfo;

public struct LocationData
{
    public uint Id;

    [JsonConverter(typeof(StringEnumConverter))]
    public ObjectType Type;

    public int AetheryteId;
    public int XCoord;
    public int YCoord;

    public LocationData(ILocation loc)
    {
        Id          = loc.Id;
        Type        = loc.Type;
        AetheryteId = loc.ClosestAetheryte == null ? -1 : (int)loc.ClosestAetheryte.Id;
        XCoord      = loc.IntegralXCoord;
        YCoord      = loc.IntegralYCoord;
    }
}
