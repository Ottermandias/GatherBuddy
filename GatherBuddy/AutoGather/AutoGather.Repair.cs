using ECommons.DalamudServices;
using GatherBuddy.Plugin;
using Lumina.Excel.Sheets;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using System;
using Dalamud.Game.ClientState.Conditions;
using ECommons.UIHelpers.AddonMasterImplementations;
using ECommons.Automation;
using ECommons.ExcelServices;
using ECommons.GameHelpers;

namespace GatherBuddy.AutoGather;

public unsafe partial class AutoGather
{
    private Item? EquipmentNeedingRepair()
    {
        const int defaultThreshold = 5;
        var threshold = GatherBuddy.Config.AutoGatherConfig.DoRepair ? GatherBuddy.Config.AutoGatherConfig.RepairThreshold : defaultThreshold;

        var equippedItems = InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems);
        for (var i = 0; i < equippedItems->Size; i++)
        {
            var equippedItem = equippedItems->GetInventorySlot(i);
            if (equippedItem != null && equippedItem->ItemId > 0)
            {
                if (equippedItem->Condition / 300 <= threshold)
                {
                    return Svc.Data.Excel.GetSheet<Item>().GetRow(equippedItem->ItemId);
                }
            }
        }

        return null;
    }

    private bool HasRepairJob(Item itemToRepair)
    {
        if (itemToRepair.ClassJobRepair.RowId > 0)
        {
            var repairJobLevel =
                PlayerState.Instance()->ClassJobLevels[
                    Svc.Data.GetExcelSheet<ClassJob>()?.GetRow(itemToRepair.ClassJobRepair.RowId).ExpArrayIndex ?? 0];
            if (Math.Max(1, itemToRepair.LevelEquip - 10) <= repairJobLevel)
                return true;
        }

        return false;
    }

    private bool HasDarkMatter(Item itemToRepair)
    {
        var darkMatters = Svc.Data.Excel.GetSheet<ItemRepairResource>();
        foreach (var darkMatter in darkMatters)
        {
            if (darkMatter.Item.RowId < itemToRepair.ItemRepair.Value.Item.RowId)
                continue;

            if (GetInventoryItemCount(darkMatter.Item.RowId) > 0)
                return true;
        }

        return false;
    }

    private bool RepairIfNeeded()
    {
        if (Svc.Condition[ConditionFlag.Mounted] || Player.Job is not Job.BTN and not Job.MIN)
            return false;

        var itemToRepair = EquipmentNeedingRepair();

        if (itemToRepair == null)
            return false;

        if (!GatherBuddy.Config.AutoGatherConfig.DoRepair)
        {
            Communicator.PrintError("Your gear is almost broken. Repair it before enabling Auto-Gather.");
            AbortAutoGather("Repairs needed.");
            return true;
        }

        if (!HasRepairJob((Item)itemToRepair))
        {
            AbortAutoGather("Repairs needed, but no repair job found.");
            return true;
        }
        if (!HasDarkMatter((Item)itemToRepair))
        {
            AbortAutoGather("Repairs needed, but no dark matter found.");
            return true;
        }

        AutoStatus = "Repairing...";
        StopNavigation();
        YesAlready.Lock();

        var delay = (int)GatherBuddy.Config.AutoGatherConfig.ExecutionDelay;
        if (RepairAddon == null)
            ActionManager.Instance()->UseAction(ActionType.GeneralAction, 6);

        TaskManager.Enqueue(() => RepairAddon != null, 1000, true, "Wait until repair menu is ready.");
        TaskManager.DelayNext(delay);
        TaskManager.Enqueue(() => { if (RepairAddon is var addon && addon != null) new AddonMaster.Repair(addon).RepairAll(); }, 1000, "Repairing all.");
        TaskManager.Enqueue(() => SelectYesnoAddon != null, 1000, true, "Wait until YesnoAddon is ready.");
        TaskManager.DelayNext(delay);
        TaskManager.Enqueue(() => { if (SelectYesnoAddon is var addon && addon != null) new AddonMaster.SelectYesno(addon).Yes(); }, 1000, "Confirm repairs.");
        TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.Occupied39], 5000, "Wait for repairs.");
        TaskManager.DelayNext(delay);
        TaskManager.Enqueue(() => { if (RepairAddon is var addon and not null) Callback.Fire(&addon->AtkUnitBase, true, -1); }, 1000, true, "Close repair menu.");
        TaskManager.DelayNext(delay);
        TaskManager.Enqueue(YesAlready.Unlock);

        return true;
    }
}
