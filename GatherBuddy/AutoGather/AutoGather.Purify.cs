using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Plugin;
using Dalamud.Game.ClientState.Conditions;
using PurifyResult = ECommons.UIHelpers.AddonMasterImplementations.AddonMaster.PurifyResult;
using ECommons.Automation;
using ECommons.DalamudServices;
using ECommons.EzSharedDataManager;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        private bool HasReducibleItems()
        {
            if (!GatherBuddy.Config.AutoGatherConfig.DoReduce || Svc.Condition[ConditionFlag.Mounted])
                return false;

            if (!QuestManager.IsQuestComplete(67633)) // No Longer a Collectable
            {
                GatherBuddy.Config.AutoGatherConfig.DoReduce = false;
                Communicator.PrintError(
                    "[GatherBuddyReborn] Aetherial reduction is enabled, but the relevant quest has not been completed yet. The feature has been disabled.");
                return false;
            }

            unsafe
            {
                var manager = InventoryManager.Instance();
                if (manager == null)
                    return false;

                foreach (var invType in InventoryTypes)
                {
                    var container = manager->GetInventoryContainer(invType);
                    if (container == null || !container->IsLoaded)
                        continue;

                    for (int i = 0; i < container->Size; i++)
                    {
                        var slot = container->GetInventorySlot(i);
                        if (slot != null
                         && slot->ItemId != 0
                         && GatherBuddy.GameData.Gatherables.TryGetValue(slot->ItemId, out var gatherable)
                         && gatherable.ItemData.AetherialReduce != 0)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private unsafe void ReduceItems(bool reduceAll)
        {
            AutoStatus = "Aetherial reduction";
            var delay = (int)GatherBuddy.Config.AutoGatherConfig.ExecutionDelay;
            TaskManager.Enqueue(StopNavigation);
            if (PurifyItemSelectorAddon == null)
            {
                EnqueueActionWithDelay(() => { ActionManager.Instance()->UseAction(ActionType.GeneralAction, 21); });
                // Prevent the "Unable to execute command while occupied" message right after entering a house.
                TaskManager.DelayNext(500);
            }

            TaskManager.Enqueue(ReduceFirstItem,                                3000, true, "Reduce first item");
            TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.Occupied39], 5000, true, "Wait until first item reduction is complete");
            TaskManager.DelayNext(delay);
            TaskManager.Enqueue(StartAutoReduction,                             1000, true, "Start auto reduction");
            TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.Occupied39], 180000, true, "Wait until all items have been reduced");
            TaskManager.DelayNext(delay);
            TaskManager.Enqueue(() =>
            {
                EnqueueActionWithDelay(() =>
                {
                    if (PurifyResultAddon is var addon and not null)
                        Callback.Fire(addon, true, -1);
                });
                if (reduceAll && HasReducibleItems())
                    ReduceItems(true);
                else
                    EnqueueActionWithDelay(() =>
                    {
                        if (PurifyItemSelectorAddon is var addon and not null)
                            Callback.Fire(addon, true, -1);
                    });
            });
        }

        private unsafe bool? ReduceFirstItem()
        {
            var addon = PurifyItemSelectorAddon;
            if (addon == null)
                return false;

            Callback.Fire(addon, true, 12, 0u);
            return true;
        }

        private unsafe bool? StartAutoReduction()
        {
            var addon = PurifyResultAddon;
            if (addon == null)
                return false;

            new PurifyResult(addon).Automatic();
            return true;
        }
    }
}
