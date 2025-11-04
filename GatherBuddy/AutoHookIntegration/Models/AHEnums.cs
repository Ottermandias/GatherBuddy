namespace GatherBuddy.AutoHookIntegration.Models;

public enum AHBiteType : byte
{
    Unknown = 0,
    Weak = 36,
    Strong = 37,
    Legendary = 38,
    None = 255,
}

public enum AHHookType : uint
{
    None = 0,
    Normal = 296,
    Precision = 4179,
    Powerful = 4103,
    Double = 27523,
    Triple = 27524,
    Stellar = 39127,
    Unknown = 255,
}
