using ECommons.DalamudServices;
using ECommons.ExcelServices;
using ECommons.GameHelpers;
using Lumina.Excel.Sheets;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using System;
using Dalamud.Game.ClientState.Conditions;
using ECommons.UIHelpers.AddonMasterImplementations;
namespace GatherBuddy.AutoGather;

public unsafe partial class AutoGather
{   
    private Item? EquipmentNeedingRepair(){
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

    private bool HasRepairJob(Item itemToRepair){
        if (itemToRepair.ClassJobRepair.RowId > 0)
        {
            var repairJobLevel = PlayerState.Instance()->ClassJobLevels[Svc.Data.GetExcelSheet<ClassJob>()?.GetRow(itemToRepair.ClassJobRepair.RowId).ExpArrayIndex ?? 0];
            if (Math.Max(1, itemToRepair.LevelEquip - 10) <= repairJobLevel)
                return true;
        }
        AbortAutoGather("Repairs needed, but no repair job found.");
        return false;
    }

    private bool HasDarkMatter(Item itemToRepair){
        var darkMatters = Svc.Data.Excel.GetSheet<ItemRepairResource>();
        foreach (var darkMatter in darkMatters)
        {
            if (darkMatter.Item.RowId < itemToRepair.ItemRepair.Value.Item.RowId)
                continue;

            if (InventoryManager.Instance()->GetInventoryItemCount(darkMatter.Item.RowId) > 0)
                return true;
        }
        AbortAutoGather("Repairs needed, but no dark matter found.");
        return false;
    }

    private void Repair(){
        AutoStatus = "Repairing...";
        TaskManager.Enqueue(StopNavigation);

        var itemToRepair = EquipmentNeedingRepair();
        if (itemToRepair == null || !HasRepairJob((Item)itemToRepair) || !HasDarkMatter((Item)itemToRepair)){
            AbortAutoGather("Repairs needed, but no repair job or dark matter found.");
        }

        var delay = (int)GatherBuddy.Config.AutoGatherConfig.ExecutionDelay;
        TaskManager.Enqueue(() => ActionManager.Instance()->UseAction(ActionType.GeneralAction, 6), 1000, true, "Open repair menu.");
        TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.Occupied39], 5000, true, "Wait for repairs.");
        TaskManager.DelayNext(delay);
        TaskManager.Enqueue(() => new AddonMaster.Repair(RepairAddon).RepairAll(), 3000, true, "Repairing all.");
        TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.Occupied39], 5000, true, "Wait for repairs.");
        TaskManager.DelayNext(delay);
        TaskManager.Enqueue(() => ActionManager.Instance()->UseAction(ActionType.GeneralAction, 6), 1000, true, "Close repair menu.");
        TaskManager.DelayNext(delay);
    }
}