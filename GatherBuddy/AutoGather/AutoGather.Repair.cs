using ECommons.DalamudServices;
using ECommons.ExcelServices;
using ECommons.GameHelpers;
using Lumina.Excel.Sheets;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
namespace GatherBuddy.AutoGather;

public partial class AutoGather
{   
    private unsafe Item? EquipmentNeedingRepair(){
        var equippedItems = InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems);
        for (var i = 0; i < equippedItems->Size; i++)
        {
            var equippedItem = equippedItems->GetInventorySlot(i);
            if (equippedItem != null && equippedItem->ItemId > 0)
            {
                if (equippedItem->Condition <= 300)
                {
                    return Svc.Data.Excel.GetSheet<Item>().GetRow(equippedItem->ItemId);
                }
            }
        }
        return null;
    }

    private unsafe bool HasRepairJob(Item itemToRepair){
        if (itemToRepair.ClassJobRepair.RowId > 0)
        {
            var repairJobLevel = PlayerState.Instance()->ClassJobLevels[Svc.Data.GetExcelSheet<ClassJob>()?.GetRow(itemToRepair.ClassJobRepair.RowId).ExpArrayIndex ?? 0];
            if (itemToRepair.LevelEquip - 10 <= repairJobLevel)
                return true;
        }

        return false;
    }

    private unsafe bool HasDarkMatter(Item itemToRepair){
        var darkMatters = Svc.Data.Excel.GetSheet<ItemRepairResource>();
        foreach (var darkMatter in darkMatters)
        {
            if (darkMatter.Item.RowId < itemToRepair.ItemRepair.Value.Item.RowId)
                continue;

            if (InventoryManager.Instance()->GetInventoryItemCount(darkMatter.Item.RowId) > 0)
                return true;
        }
        return false;
    }

    private void Repair(){
        var itemToRepair = EquipmentNeedingRepair();
        if (itemToRepair != null && HasRepairJob((Item)itemToRepair) && HasDarkMatter((Item)itemToRepair)){
            DoRepairs();
        }
    }

    private bool DoRepairs(){

        if (GatherBuddy.Config.AutoGatherConfig.DoRepair){
        return true;
    }   
        return false;
    }
}