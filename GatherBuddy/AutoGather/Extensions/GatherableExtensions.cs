using System.Collections.Generic;
using System.Linq;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;

namespace GatherBuddy.AutoGather.Extensions;

/// <summary>
/// Extension methods for the IGatherable interface.
/// </summary>
public static class GatherableExtensions
{
    private static IReadOnlyList<InventoryType> _inventoryTypes { get; } =
        [
            InventoryType.RetainerCrystals,
            InventoryType.RetainerPage1,
            InventoryType.RetainerPage2,
            InventoryType.RetainerPage3,
            InventoryType.RetainerPage4,
            InventoryType.RetainerPage5,
            InventoryType.RetainerPage6,
            InventoryType.RetainerPage7,
            InventoryType.Inventory1,
            InventoryType.Inventory2,
            InventoryType.Inventory3,
            InventoryType.Inventory4,
            InventoryType.Crystals
        ];

    /// <summary>
    /// Gets the inventory count for a gatherable item.
    /// </summary>
    /// <param name="gatherable">The gatherable item to check.</param>
    /// <param name="checkRetainers">Check retainer inventory.</param>
    /// <returns>The count of the item in the inventory.</returns>
    public unsafe static int GetInventoryCount(this IGatherable gatherable)
    {
        if (GatherBuddy.Config.AutoGatherConfig.CheckRetainers && AllaganTools.Enabled)
        {
            return (int)AllaganTools.ItemCountOwned(gatherable.ItemId, true, _inventoryTypes.Select(it => (uint)it).ToArray());
        }

        var inventory = InventoryManager.Instance();
        return inventory->GetInventoryItemCount(gatherable.ItemId, false, false, false, (short)(gatherable.ItemData.IsCollectable ? 1 : 0));
    }
}
