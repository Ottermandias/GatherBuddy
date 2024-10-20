using Dalamud.Game.ClientState.Conditions;
using ECommons.Automation;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

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
            TaskManager.Enqueue(StopNavigation);
            EnqueueActionWithDelay(() => ActionManager.Instance()->UseAction(ActionType.GeneralAction, 14));
            TaskManager.Enqueue(() => MaterializeAddon != null);
            return;
        }

        EnqueueActionWithDelay(() => { if (MaterializeAddon is var addon and not null) Callback.Fire(&addon->AtkUnitBase, true, 2, 0); });
        TaskManager.Enqueue(() => MaterializeDialogAddon != null, 1000);
        EnqueueActionWithDelay(() => { if (MaterializeDialogAddon is var addon and not null) new MaterializeDialog(addon).Materialize(); });
        TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.Occupied39]);

        if (SpiritBondMax == 1) 
        {
            EnqueueActionWithDelay(() => { if (MaterializeAddon is var addon and not null) addon->Close(true); });
        }
    }
}
