using FFXIVClientStructs.FFXIV.Client.Enums;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;

namespace GatherBuddy.SeFunctions;

public sealed class CurrentBait
{
    public unsafe uint Current
    {
        get 
        {
            if (GameMain.Instance()->CurrentTerritoryIntendedUseId == TerritoryIntendedUse.CosmicExploration)
            {
                var cosmicManager = WKSManager.Instance();
                if (cosmicManager != null)
                    return cosmicManager->State.FishingBait;
            }

            return PlayerState.Instance()->FishingBait;
        }
    }

    public enum ChangeBaitReturn
    {
        Success,
        AlreadyEquipped,
        NotInInventory,
        InvalidBait,
        UnknownError,
    }

    public static unsafe int HasItem(uint itemId)
        => InventoryManager.Instance()->GetInventoryItemCount(itemId);

    public ChangeBaitReturn ChangeBait(uint baitId)
    {
        if (baitId == Current)
            return ChangeBaitReturn.AlreadyEquipped;

        if (baitId == 0 || !GatherBuddy.GameData.Bait.ContainsKey(baitId))
            return ChangeBaitReturn.InvalidBait;

        if (HasItem(baitId) <= 0)
            return ChangeBaitReturn.NotInInventory;

        return GameMain.ExecuteCommand(701, 4, (int)baitId, 0, 0) ? ChangeBaitReturn.Success : ChangeBaitReturn.UnknownError;
    }
}
