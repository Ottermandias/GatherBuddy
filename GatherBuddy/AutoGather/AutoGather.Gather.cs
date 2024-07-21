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
using ECommons;
using ECommons.Automation.UIInput;
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
            if (GatherBuddy.UptimeManager.TimedGatherables.Contains(targetItem) && targetItem.NodeType != NodeType.Ephemeral) TaskManager.Enqueue(() => TimedNodesGatheredThisTrip.Add(targetItem.ItemId));
        }

        private unsafe void DoGatherWindowTasks(IGatherable item)
        {
            if (GatheringAddon == null)
                return;

            uint[] ids       = GatheringAddon->ItemIds.ToArray();
            var    itemIndex = GetIndexOfItemToClick(ids, item);
            if (itemIndex < 0)
                itemIndex = GatherBuddy.GameData.Gatherables.Where(item => ids.Contains(item.Key)).Select(item => ids.IndexOf(item.Key))
                    .FirstOrDefault();
            var receiveEventAddress = new nint(GatheringAddon->AtkUnitBase.AtkEventListener.VirtualTable->ReceiveEvent);
            var eventDelegate       = Marshal.GetDelegateForFunctionPointer<ClickHelper.ReceiveEventDelegate>(receiveEventAddress);

            var target    = AtkStage.Instance();
            var eventData = EventData.ForNormalTarget(target, &GatheringAddon->AtkUnitBase);
            var inputData = InputData.Empty();

            eventDelegate.Invoke(&GatheringAddon->AtkUnitBase.AtkEventListener, EventType.CHANGE, (uint)itemIndex, eventData.Data,
                inputData.Data);
        }

        private int GetIndexOfItemToClick(uint[] ids, IGatherable item)
        {
            var gatherable = item as Gatherable;
            if (gatherable == null)
            {
                return GatherBuddy.GameData.Gatherables.Where(item => ids.Contains(item.Key)).Select(item => ids.IndexOf(item.Key))
                    .FirstOrDefault();

                ;
            }

            if (!gatherable.GatheringData.IsHidden
             || (gatherable.GatheringData.IsHidden && (HiddenRevealed || !ShouldUseLuck(ids, gatherable))))
            {
                return ids.IndexOf(gatherable.ItemId);
            }

            // If no matching item is found, return the index of the first non-hidden item
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                gatherable = GatherBuddy.GameData.Gatherables.FirstOrDefault(it => it.Key == id).Value;
                if (gatherable == null)
                {
                    continue;
                }

                if (!gatherable.GatheringData.IsHidden || (gatherable.GatheringData.IsHidden && HiddenRevealed))
                {
                    return i;
                }
            }

            // If all items are hidden or none found, return -1
            return GatherBuddy.GameData.Gatherables.Where(item => ids.Contains(item.Key)).Select(item => ids.IndexOf(item.Key))
                .FirstOrDefault();

            ;
        }
    }
}
