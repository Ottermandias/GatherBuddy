using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Interfaces;

namespace GatherBuddy.AutoGather.Extensions;

/// <summary>
/// Extension methods for the IGatherable interface.
/// </summary>
public static class GatherableExtensions
{
    /// <summary>
    /// Gets the inventory count for a gatherable item.
    /// </summary>
    /// <param name="gatherable">The gatherable item to check.</param>
    /// <returns>The count of the item in the inventory.</returns>
    public unsafe static int GetInventoryCount(this IGatherable gatherable)
    {
        var inventory = InventoryManager.Instance();
        return inventory->GetInventoryItemCount(gatherable.ItemId, false, false, false, (short)(gatherable.ItemData.IsCollectable ? 1 : 0));
    }
}
