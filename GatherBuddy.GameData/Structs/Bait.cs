using System;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Structs;

public class Bait : IComparable<Bait>
{
    public const uint FishingTackleRow = 30;

    public static Bait Unknown { get; } = new(new Item { Icon = 60027 });

    public readonly Item   Data;
    public readonly string Name = "Unknown";

    public uint Id
        => Data.RowId;

    public Bait(Item data)
    {
        Data = data;
        if (data.RowId != 0)
            Name = MultiString.ParseSeStringLumina(data.Name);
    }

    public int CompareTo(Bait? other)
        => Id.CompareTo(other?.Id ?? 0);
}
