using System;
using System.Collections.Generic;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Utility;
using Lumina.Excel.Sheets;
using GatheringType = GatherBuddy.Enums.GatheringType;

namespace GatherBuddy.Classes;

public class Gatherable : IComparable<Gatherable>, IGatherable
{
    public Item                 ItemData      { get; }
    public GatheringItem        GatheringData { get; }
    public MultiString          Name          { get; }
    public IList<GatheringNode> NodeList      { get; } = new List<GatheringNode>();

    public int InternalLocationId { get; internal set; }

    public IEnumerable<ILocation> Locations
        => NodeList;

    public uint ItemId
        => ItemData.RowId;

    public ObjectType Type
        => ObjectType.Gatherable;

    public uint GatheringId
        => GatheringData.RowId;

    public NodeType      NodeType      { get; internal set; } = NodeType.Unknown;
    public GatheringType GatheringType { get; internal set; } = GatheringType.Unknown;

    public uint ExpansionIdx { get; internal set; } = uint.MaxValue;

    public Gatherable(GameData gameData, GatheringItem gatheringData)
    {
        GatheringData = gatheringData;
        var itemSheet = gameData.DataManager.GetExcelSheet<Item>();
        ItemData = itemSheet.GetRowOrDefault(gatheringData.Item.RowId) ?? new Item();
        if (ItemData.RowId == 0)
            gameData.Log.Error("Invalid item.");

        var levelData = gatheringData.GatheringItemLevel.ValueNullable;
        _levelStars = levelData == null ? 0 : (levelData.Value.GatheringItemLevel << 3) + levelData.Value.Stars;
        Name        = MultiString.FromItem(gameData.DataManager, gatheringData.Item.RowId);
    }

    public int Level
        => _levelStars >> 3;

    public int Stars
        => _levelStars & 0b111;

    public string StarsString()
        => StarsArray[Stars];

    public string LevelString()
        => $"{Level}{StarsString()}";

    public override string ToString()
        => $"{Name} ({Level}{StarsString()})";

    public int CompareTo(Gatherable? rhs)
        => ItemId.CompareTo(rhs?.ItemId ?? 0);

    public bool IsCrystal => ItemData.FilterGroup == 11;

    public bool IsTreasureMap => ItemData.FilterGroup == 18;

    private readonly int _levelStars;

    private static readonly string[] StarsArray =
    {
        "",
        "*",
        "**",
        "***",
        "****",
    };
}
