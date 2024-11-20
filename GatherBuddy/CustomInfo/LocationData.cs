using GatherBuddy.Interfaces;
using GatherBuddy.Structs;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatherBuddy.CustomInfo;

public struct LocationData(ILocation loc)
{
    public uint Id = loc.Id;

    [JsonConverter(typeof(StringEnumConverter))]
    public ObjectType Type = loc.Type;

    public int    AetheryteId = loc.ClosestAetheryte == null ? -1 : (int)loc.ClosestAetheryte.Id;
    public int    XCoord      = loc.IntegralXCoord;
    public int    YCoord      = loc.IntegralYCoord;
    public ushort Radius      = loc.Radius;

    public WaymarkSet Markers = loc.Markers;
}
