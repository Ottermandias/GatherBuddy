using System;
using GatherBuddy.Utility;
using Lumina.Excel.Sheets;

namespace GatherBuddy.Structs;

public class Bait : IComparable<Bait>
{
    public const uint FishingTackleRow = 30;

    public static Bait Unknown { get; } = new(0, "Unknown", 60027);

    public readonly string Name = "Unknown";
    public          uint   Id   { get; private set; }
    public          ushort Icon { get; private set; }

    public Bait(Item data)
    {
        Id   = data.RowId;
        Icon = data.Icon;
        if (data.RowId != 0)
            Name = MultiString.ParseSeStringLumina(data.Name);
    }

    public int CompareTo(Bait? other)
        => Id.CompareTo(other?.Id ?? 0);

    private Bait(uint id, string name, ushort icon)
    {
        Id   = id;
        Name = name;
        Icon = icon;
    }
}