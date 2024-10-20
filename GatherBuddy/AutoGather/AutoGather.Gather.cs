using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.Classes;
using System.Linq;
using System.Runtime.InteropServices;
using ECommons.Automation.UIInput;
using ItemSlot = GatherBuddy.AutoGather.GatheringTracker.ItemSlot;
using Dalamud.Game.ClientState.Conditions;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        private unsafe void EnqueueNodeInteraction(IGameObject gameObject, Gatherable targetItem)
        {
            var targetSystem = TargetSystem.Instance();
            if (targetSystem == null)
                return;

            TaskManager.Enqueue(() => targetSystem->OpenObjectInteraction((FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)gameObject.Address));
            TaskManager.Enqueue(() => Dalamud.Conditions[ConditionFlag.Gathering], 500);
        }

        private unsafe void EnqueueGatherItem(ItemSlot slot)
        {
            if (GatheringAddon == null)
                return;

            var itemIndex           = slot.Index;
            var receiveEventAddress = new nint(GatheringAddon->AtkUnitBase.AtkEventListener.VirtualTable->ReceiveEvent);
            var eventDelegate       = Marshal.GetDelegateForFunctionPointer<ClickHelper.ReceiveEventDelegate>(receiveEventAddress);

            var target    = AtkStage.Instance();
            var eventData = EventData.ForNormalTarget(target, &GatheringAddon->AtkUnitBase);
            var inputData = InputData.Empty();

            EnqueueActionWithDelay(() => eventDelegate.Invoke(&GatheringAddon->AtkUnitBase.AtkEventListener, EventType.CHANGE, (uint)itemIndex, eventData.Data, inputData.Data));

            if (slot.Item.IsTreasureMap)
            {
                TaskManager.Enqueue(() => Dalamud.Conditions[ConditionFlag.Gathering42], 1000);
                TaskManager.Enqueue(() => !Dalamud.Conditions[ConditionFlag.Gathering42]);
                TaskManager.Enqueue(RefreshNextTresureMapAllowance);
            }
        }

        /// <summary>
        /// Checks if desired item could or should be gathered and may change it to something more suitable
        /// </summary>
        /// <returns>UseSkills: True if the selected item is in the gathering list; false if we gather collectable or some unneeded junk
        /// Slot: ItemSlot of item to gather</returns>
        private (bool UseSkills, ItemSlot Slot) GetItemSlotToGather(Gatherable? targetItem)
        {
            //Gather crystals when using The Giving Land
            if (HasGivingLandBuff)
            {
                var crystal = GetAnyCrystalInNode();
                if (crystal != null)
                    return (true, crystal);
            }

            var aviable = NodeTarcker.Aviable
                .Where(CheckItemOvercap)
                .ToList();

            var target = targetItem != null ? aviable.Where(s => s.Item == targetItem).FirstOrDefault() : null;

            if (target != null && InventoryCount(targetItem!) < QuantityTotal(targetItem!))
            {
                //The target item is found in the node, would not overcap and we need to gather more of it
                return (!target.Collectable, target);
            }

            //Items in the gathering list
            var gatherList = ItemsToGather
                //Join node slots, retaing list order
                .Join(aviable, i => i.Item, s => s.Item, (i, s) => s)
                //And we need more of them
                .Where(s => InventoryCount(s.Item) < QuantityTotal(s.Item));

            //Items in the fallback list
            var fallbackList = _plugin.GatherWindowManager.FallbackItems
                //Join node slots, retaing list order
                .Join(aviable, i => i, s => s.Item, (i, s) => s)
                //And we need more of them
                .Where(s => InventoryCount(s.Item) < QuantityTotal(s.Item));

            var fallbackSkills = GatherBuddy.Config.AutoGatherConfig.UseSkillsForFallbackItems;

            //If there is any other item that we want in the node, gather it
            var slot = gatherList.FirstOrDefault();
            if (slot != null)
            {
                return (!slot.Collectable, slot);
            }

            //If there is any fallback item, gather it
            slot = fallbackList.FirstOrDefault();
            if (slot != null)
            {
                return (fallbackSkills && !slot.Collectable, slot);
            }

            //Check if we should and can abandon the node
            if (GatherBuddy.Config.AutoGatherConfig.AbandonNodes)
                throw new NoGatherableItemsInNodeExceptions();

            if (target != null)
            {
                //Gather unneeded target item as a fallback
                return (false, target);
            }

            //Gather any crystals
            slot = GetAnyCrystalInNode();
            if (slot != null)
            {
                return (false, slot);
            }
            //If there are no crystals, gather anything which is not treasure map nor collectable
            slot = aviable.Where(s => !s.Item.IsTreasureMap && !s.Collectable).FirstOrDefault();
            if (slot != null)
            {
                return (false, slot);
            }
            //Abort if there are no items we can gather
            throw new NoGatherableItemsInNodeExceptions();
        }

        private bool CheckItemOvercap(ItemSlot s)
        {
            //If it's a treasure map, we can have only one in the inventory
            if (s.Item.IsTreasureMap && InventoryCount(s.Item) != 0)
                return false;
            //If it's a crystal, we can't have more than 9999
            if (s.Item.IsCrystal && InventoryCount(s.Item) > 9999 - s.Yield)
                return false;
            return true;
        }
        
        private ItemSlot? GetAnyCrystalInNode()
        {
            return NodeTarcker.Aviable
                .Where(s => s.Item.IsCrystal)
                .Where(CheckItemOvercap)
                //Prioritize crystals in the gathering list
                .OrderBy(s => ItemsToGather.Any(g => g.Item == s.Item) ? 0 : 1)
                //Prioritize crystals with a lower amount in the inventory
                .ThenBy(s => InventoryCount(s.Item))
                .FirstOrDefault();
        }
    }
}
