using ClickLib.Bases;
using ClickLib.Enums;
using ClickLib.Structures;
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

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {

        private unsafe void InteractWithNode()
        {
            if (!CanAct) return;
            var targetSystem = TargetSystem.Instance();
            if (targetSystem == null)
                return;
            TaskManager.EnqueueDelay(1000);
            TaskManager.Enqueue(() =>
            {
                targetSystem->OpenObjectInteraction((FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)NearestNode.Address);
            });
        }

        private unsafe void DoGatherWindowTasks(List<uint> ids, AddonGathering* gatheringWindow)
        {

            var itemIndex = GetIndexOfItemToClick(ids);

            var receiveEventAddress = new nint(gatheringWindow->AtkUnitBase.AtkEventListener.vfunc[2]);
            var eventDelegate = Marshal.GetDelegateForFunctionPointer<ReceiveEventDelegate>(receiveEventAddress);

            var target = AtkStage.GetSingleton();
            var eventData = EventData.ForNormalTarget(target, &gatheringWindow->AtkUnitBase);
            var inputData = InputData.Empty();

            eventDelegate.Invoke(&gatheringWindow->AtkUnitBase.AtkEventListener, EventType.CHANGE, (uint)itemIndex, eventData.Data, inputData.Data);
        }

        private int GetIndexOfItemToClick(List<uint> ids)
        {
            // Check each ID in the list against ItemsToGather
            for (int i = 0; i < ids.Count; i++)
            {
                var id = ids[i];
                var item = ItemsToGather.FirstOrDefault(it => it.ItemId == id);
                var gatherable = item as Gatherable;
                if (gatherable == null)
                {
                    continue;
                }
                if (!gatherable.GatheringData.IsHidden || (gatherable.GatheringData.IsHidden && (HiddenRevealed || !ShouldUseLuck(ids))))
                {
                    return i;
                }
            }

            // If no matching item is found, return the index of the first non-hidden item
            for (int i = 0; i < ids.Count; i++)
            {
                var id = ids[i];
                var gatherable = GatherBuddy.GameData.Gatherables.FirstOrDefault(it => it.Key == id).Value;
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
            return 0;
        }
    }
}
