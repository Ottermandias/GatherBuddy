using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatherBuddy.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum HookSet : byte
{
    Unknown    = 0,
    Precise    = 1,
    Powerful   = 2,
    Hook       = 3,
    DoubleHook = 4,
    TripleHook = 5,
    Stellar    = 6,
    None       = 255,
}

public static class HookSetExtensions
{
    public static string ToName(this HookSet value)
        => value switch
        {
            HookSet.Unknown    => "Unknown",
            HookSet.Precise    => "Precise",
            HookSet.Powerful   => "Powerful",
            HookSet.Hook       => "Regular",
            HookSet.DoubleHook => "Double",
            HookSet.TripleHook => "Triple",
            HookSet.Stellar    => "Stellar",
            HookSet.None       => "None",
            _                  => "Invalid",
        };
}
