using SeHousingManager = FFXIVClientStructs.FFXIV.Client.Game.HousingManager;

namespace GatherBuddy.SeFunctions;

public static unsafe class HousingManager
{
    public static bool IsInHousing()
    {
        var housingManager = SeHousingManager.Instance();
        if (housingManager == null)
            return false;

        ref var housingTerritory = ref housingManager->CurrentTerritory;
        if (housingTerritory == null)
            return false;
        
        return true;
    }
}
