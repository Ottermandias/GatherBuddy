using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatherBuddy.Enums;

public enum Snagging : byte
{
    Unknown  = 0,
    None     = 1,
    Required = 2,
}

[JsonConverter(typeof(StringEnumConverter))]
public enum Lure : byte
{
    None      = 0,
    Ambitious = 1,
    Modest    = 2,
}
