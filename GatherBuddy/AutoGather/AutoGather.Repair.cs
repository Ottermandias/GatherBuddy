using ECommons.DalamudServices;
using ECommons.ExcelServices;
using ECommons.GameHelpers;
using Lumina.Excel.Sheets;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
namespace GatherBuddy.AutoGather;

public partial class AutoGather
{

    private void Repair(){
        if (HasRepairJob() && HasDarkMatter()){
            DoRepairs();
        }
    }
    
    private unsafe uint RepairsNeeded(){
        var equippedItems = InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems);
        for (var i = 0; i < equippedItems->Size; i++)
        {
            var equippedItem = equippedItems->GetInventorySlot(i);
            if (equippedItem != null && equippedItem->ItemId > 0)
            {
                if (equippedItem->Condition <= 300)
                {
                    return equippedItem->ItemId;
                }
            }
        }
        return 0;
    }

    private unsafe bool HasRepairJob(uint equipmentId){
        var equippedItem = Svc.Data.Excel.GetSheet<Item>().GetRow(equipmentId);

        if (equippedItem.ClassJobRepair.RowId > 0)
        {
            var repairJob = (Job)equippedItem.ClassJobRepair.RowId;
            var repairItem = equippedItem.ItemRepair.Value.Item;

            if (!HasDarkMatter(repairItem.RowId))
                return false;

            var repairJobLevel = PlayerState.Instance()->ClassJobLevels[Svc.Data.GetExcelSheet<ClassJob>()?.GetRow((uint)repairJob).ExpArrayIndex ?? 0];
            if (equippedItem.LevelEquip - 10 <= repairJobLevel)
                return true;
        }

        return false;
    }

    private unsafe bool HasDarkMatter(){
        var darkMatters = Svc.Data.Excel.GetSheet<ItemRepairResource>();
        foreach (var item in darkMatters)
        {
            if (item.Item.RowId < darkMatterID)
                continue;

            if (InventoryManager.Instance()->GetInventoryItemCount(item.Item.RowId) > 0)
                return true;
        }
        return false;
    }

    private bool DoRepairs(){

        if (GatherBuddy.Config.AutoGatherConfig.DoRepair){
        return true;
    }   
        return false;
    }
}