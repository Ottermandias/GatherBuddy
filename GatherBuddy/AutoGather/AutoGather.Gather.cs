using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.Classes;
using System.Linq;
using System.Runtime.InteropServices;
using ECommons.Automation.UIInput;
using ItemSlot = GatherBuddy.AutoGather.GatheringTracker.ItemSlot;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        private unsafe void EnqueueNodeInteraction(IGameObject gameObject, Gatherable targetItem)
        {
            if (!CanAct)
                return;

            var targetSystem = TargetSystem.Instance();
            if (targetSystem == null)
                return;

            TaskManager.Enqueue(() =>
            {
                targetSystem->OpenObjectInteraction((FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)gameObject.Address);
            });
            TaskManager.DelayNext(500);
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

            //Communicator.Print("Queuing click.");
            EnqueueGatherAction(() => eventDelegate.Invoke(&GatheringAddon->AtkUnitBase.AtkEventListener, EventType.CHANGE, (uint)itemIndex, eventData.Data,
                inputData.Data));
            if (slot.Item.IsTreasureMap)
                EnqueueGatherAction(RefreshNextTresureMapAllowance);
        }

        /// <summary>
        /// Checks if desired item could or should be gathered and may change it to something more suitable
        /// </summary>
        /// <returns>UseSkills: True if the selected item is in the gathering list; false if we gather collectable or some unneeded junk
        /// Slot: ItemSlot of item to gather</returns>
        private (bool UseSkills, ItemSlot Slot) GetItemSlotToGather(Gatherable? desiredItem)
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

                //Items in a gathering list
            var shouldGather = ItemsToGather
                //Join node slots, retaing list order
                .Join(aviable, i => i.Item, s => s.Item, (i, s) => s)
                //And we need more of them
                .Where(s => InventoryCount(s.Item) < QuantityTotal(s.Item));

                //Items in a fallback list
            var fallback = _plugin.GatherWindowManager.FallbackItems
                //Join node slots, retaing list order
                .Join(aviable, i => i, s => s.Item, (i, s) => s)
                //And we need more of them
                .Where(s => InventoryCount(s.Item) < QuantityTotal(s.Item));

            var fallbackSkills = GatherBuddy.Config.AutoGatherConfig.UseSkillsForFallbackItems;

            if (desiredItem != null)
            {
                var orig = aviable.Where(s => s.Item == desiredItem).FirstOrDefault();
                if (orig != null)
                {
                    if (InventoryCount(desiredItem) < QuantityTotal(desiredItem))
                    {
                        //The desired item is found in the node, would not overcap and we need to gather more of it
                        return (!orig.Collectable, orig);
                    }
                    else
                    {
                        //If we have gathered enough of the current item and there is another item in the node that we want, gather it instead
                        var other = shouldGather.FirstOrDefault();
                        if (other != null)
                        {
                            return (!other.Collectable, other);
                        }

                        other = fallback.FirstOrDefault();
                        if (other != null)
                        {
                            return (fallbackSkills && !other.Collectable, other);
                        }

                        return (!orig.Collectable, orig);
                    }
                }
            }
            //If there is any other item that we want in the node, gather it
            var slot = shouldGather.FirstOrDefault();
            if (slot != null)
            {
                return (!slot.Collectable, slot);
            }

            //If there is any fallback item, gather it
            slot = fallback.FirstOrDefault();
            if (slot != null)
            {
                return (fallbackSkills && !slot.Collectable, slot);
            }

            //Otherwise gather any crystals
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
