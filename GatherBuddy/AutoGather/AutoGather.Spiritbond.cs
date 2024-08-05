using Dalamud.Game.ClientState.Conditions;
using ECommons.Automation;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI;
using GatherBuddy.Plugin;

namespace GatherBuddy.AutoGather;

public partial class AutoGather
{

    unsafe int SpiritBondMax
    {
        get
        {
            var inventory = InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems);
            var result    = 0;
            for (var slot = 0; slot < inventory->Size; slot++)
            {
                var inventoryItem = inventory->GetInventorySlot(slot);
                if (inventoryItem == null || inventoryItem->ItemId <= 0)
                    continue;

                //GatherBuddy.Log.Debug("Slot " + slot + " has " + inventoryItem->Spiritbond + " Spiritbond");
                if (inventoryItem->Spiritbond == 10000)
                {
                    result++;
                }
            }

            return result;
        }
    }

    unsafe void DoMateriaExtraction()
    {
        if (MaterializeAddon == null)
        {
            TaskManager.Enqueue(VNavmesh_IPCSubscriber.Path_Stop);
            TaskManager.Enqueue(() => ActionManager.Instance()->UseAction(ActionType.GeneralAction, 14));
            TaskManager.Enqueue(() => MaterializeAddon != null);
            return;
        }

        for (var i = SpiritBondMax; i > 0; i--)
        {
            TaskManager.Enqueue(() => Callback.Fire(&MaterializeAddon->AtkUnitBase, true, 2, 0));
            TaskManager.DelayNext(1000);
            TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.Occupied39]);
        }
        //TaskManager.Enqueue(() => ActionManager.Instance()->UseAction(ActionType.GeneralAction, 14));
    }
}
