using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Game;
using ECommons;
using ECommons.Automation.UIInput;
using GatherBuddy.Plugin;
using OtterGui;
using NodeType = GatherBuddy.Enums.NodeType;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        private unsafe void InteractWithNode(IGameObject gameObject, Gatherable targetItem)
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
            TaskManager.DelayNext(1000);
        }

        private unsafe void DoGatherWindowTasks(IGatherable item)
        {
            if (GatheringAddon == null)
                return;

            uint[] ids = GetGatherableIds();
            var    itemIndex = GetIndexOfItemToClick(ids, item);
            if (itemIndex < 0)
                itemIndex = GatherBuddy.GameData.Gatherables.Where(item => ids.Contains(item.Key)).Select(item => ids.IndexOf(item.Key))
                    .FirstOrDefault();
            var receiveEventAddress = new nint(GatheringAddon->AtkUnitBase.AtkEventListener.VirtualTable->ReceiveEvent);
            var eventDelegate       = Marshal.GetDelegateForFunctionPointer<ClickHelper.ReceiveEventDelegate>(receiveEventAddress);

            var target    = AtkStage.Instance();
            var eventData = EventData.ForNormalTarget(target, &GatheringAddon->AtkUnitBase);
            var inputData = InputData.Empty();

            //Communicator.Print("Queuing click.");
            EnqueueGatherAction(() => eventDelegate.Invoke(&GatheringAddon->AtkUnitBase.AtkEventListener, EventType.CHANGE, (uint)itemIndex, eventData.Data,
                inputData.Data));
            if (IsTreasureMap(item))
                EnqueueGatherAction(RefreshNextTresureMapAllowance);
        }

        private unsafe uint[] GetGatherableIds()
        {
            uint[] ids = GatheringAddon->ItemIds.ToArray();
            foreach (var id in ids)
            {
                var gatherable = GatherBuddy.GameData.Gatherables.FirstOrDefault(it => it.Key == id).Value;
                if (GatherBuddy.UptimeManager.TimedGatherables.Contains(gatherable) && gatherable.NodeType != NodeType.Ephemeral && !TimedNodesGatheredThisTrip.Contains(gatherable.ItemId))
                {
                    GatherBuddy.Log.Information($"Saw timed item {gatherable.Name[GatherBuddy.Language]} in node. We should remember that.");
                    TimedNodesGatheredThisTrip.Add(gatherable.ItemId);
                }
            }

            return ids;
        }
        private int GetIndexOfItemToClick(uint[] ids, IGatherable item)
            => ids.IndexOf(item.ItemId);

        /// <summary>
        /// Checks if desired item could or should be gathered and may change it to something more suitable
        /// </summary>
        /// <returns>True if the selected item is in the gathering list; false if we gather some unneeded junk</returns>
        private bool MaybeGatherSometingElse(ref Gatherable desiredItem, uint[] ids)
        {
            var aviable = ids
                .Select(GatherBuddy.GameData.Gatherables.GetValueOrDefault)
                .Where((item, index) => item != null && CheckItemOvercap(item, index))
                .Select(item => item!)
                .ToArray();

            var crystals = aviable
                .Where(IsCrystal)
                //Prioritize crystals with a lower amount in the inventory
                .OrderBy(InventoryCount)
                //Prioritize crystals in the gathering list
                .OrderBy(item => ItemsToGather.Any(toGather => toGather.ItemId == item.ItemId) ? 0 : 1)
                .ToArray();

            //Gather crystals when using The Giving Land
            if (crystals.Any() && (HasGivingLandBuff || GatherBuddy.Config.AutoGatherConfig.UseGivingLandOnCooldown && ShouldUseGivingLand(crystals.First())))
            {
                desiredItem = crystals.First();
                return true;
            }

            var shouldGather = aviable
                //Item is in gathering list
                .Where(item => ItemsToGather.Any(g => g.ItemId == item.ItemId))
                //And we need more of it
                .Where(item => InventoryCount(item) < QuantityTotal(item));

            var originalItem = desiredItem;

            if (aviable.Any(item => item.ItemId == originalItem.ItemId))
            {
                if (InventoryCount(originalItem) < QuantityTotal(originalItem))
                {
                    //The desired item is found in the node, would not overcap and we need to gather more of it
                    return true;
                }
                else
                {
                    //If we have gathered enough of the current item and there is another item in the node that we want, gather it instead
                    desiredItem = shouldGather.FirstOrDefault(originalItem);
                    return true;
                }

            }
            else
            {
                //If there is any other item that we want in the node, gather it
                var otherItem = shouldGather.FirstOrDefault();
                if (otherItem != null)
                {
                    desiredItem = otherItem;
                    return true;
                }
                //Otherwise gather any crystals
                else if (crystals.Any())
                {
                    desiredItem = crystals.First();
                }
                //If there are no crystals, gather anything which is not treasure map
                else if (aviable.Any(item => !IsTreasureMap(item)))
                {
                    desiredItem = aviable.First(item => !IsTreasureMap(item));
                }
                //Abort if there are no items we can gather
                else
                {
                    throw new NoGatherableItemsInNodeExceptions();
                }
                return false;
            }
        }

        private bool CheckItemOvercap(Gatherable item, int index)
        {
            //If it's a treasure map, we can have only one in the inventory
            if (IsTreasureMap(item) && InventoryCount(item) != 0)
                return false;
            //If it's a crystal, we can't have more than 9999
            if (IsCrystal(item) && InventoryCount(item) > 9999 - (HasGivingLandBuff ? GivingLandYeild : GetCurrentYield(index)))
                return false;
            return true;
        }
    }
}
