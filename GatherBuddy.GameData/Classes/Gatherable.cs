using System;
using System.Collections.Generic;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;
using GatheringType = GatherBuddy.Enums.GatheringType;

namespace GatherBuddy.Classes;

public class Gatherable : IComparable<Gatherable>, IGatherable
{
    public Item                 ItemData      { get; }
    public GatheringItem        GatheringData { get; }
    public MultiString          Name          { get; }
    public IList<GatheringNode> NodeList      { get; } = new List<GatheringNode>();

    public List<InventoryType> InventoryTypes
    {
        get
        {
            List<InventoryType> types = new List<InventoryType>()
            {
                InventoryType.Inventory1,
                InventoryType.Inventory2,
                InventoryType.Inventory3,
                InventoryType.Inventory4,
            };
            return types;
        }
    }

    public uint                Quantity { get; set; } = 1;
    public unsafe int InventoryCount
    {
        get
        {
            if (ItemData.IsCollectable)
            {
                int count   = 0;
                var manager = InventoryManager.Instance();
                if (manager == null)
                    return count;
                foreach (var inv in InventoryTypes)
                {
                    var container = manager->GetInventoryContainer(inv);
                    if (container == null || container->Loaded == 0)
                        continue;
                    for (int i = 0; i < container->Size; i++)
                    {
                        var item = container->GetInventorySlot(i);
                        if (item == null || item->ItemID == 0 || item->ItemID != ItemId) continue;

                        count++;
                    }
                }

                return count;
            }
            else
            {
                var inventory = InventoryManager.Instance();
                return inventory->GetInventoryItemCount(ItemId);
            }
        }
    }

    public int InternalLocationId { get; internal set; } = 0;

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
        ItemData = itemSheet?.GetRow((uint)gatheringData.Item) ?? new Item();
        if (ItemData.RowId == 0)
            gameData.Log.Error("Invalid item.");

        var levelData = gatheringData.GatheringItemLevel?.Value;
        _levelStars = levelData == null ? 0 : (levelData.GatheringItemLevel << 3) + levelData.Stars;
        Name        = MultiString.FromItem(gameData.DataManager, (uint)gatheringData.Item);
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
