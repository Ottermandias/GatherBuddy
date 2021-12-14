namespace GatherBuddy.Enums;

public enum NodeType : byte
{
    Unknown   = 0xFF,
    Regular   = 0,
    Unspoiled = 1,
    Ephemeral = 2,
    Legendary = 3,
};
