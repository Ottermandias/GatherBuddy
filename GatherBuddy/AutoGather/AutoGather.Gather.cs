using ClickLib.Bases;
using ClickLib.Structures;
using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
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
        private void DoGatherTasks()
        {
            if (IsGathering)
            {
                TaskManager.Enqueue(DoActionTasks);
                TaskManager.Enqueue(DoGatherWindowTasks);
            }
            else
            {
                HiddenRevealed = false;
                TaskManager.Enqueue(InteractWithNode);
            }
        }

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

        private unsafe void DoGatherWindowTasks()
        {
            var gatheringWindow = (AddonGathering*)Dalamud.GameGui.GetAddonByName("Gathering", 1);
            if (gatheringWindow == null) return;

            var ids = new List<uint>()
                    {
                    gatheringWindow->GatheredItemId1,
                    gatheringWindow->GatheredItemId2,
                    gatheringWindow->GatheredItemId3,
                    gatheringWindow->GatheredItemId4,
                    gatheringWindow->GatheredItemId5,
                    gatheringWindow->GatheredItemId6,
                    gatheringWindow->GatheredItemId7,
                    gatheringWindow->GatheredItemId8
                    };


            List<uint> desiredItemIds = ItemsToGather.Select(i => i.ItemId).ToList();
            var itemIndex = ids.IndexOf(ids.FirstOrDefault(desiredItemIds.Contains));
            if (itemIndex < 0) itemIndex = ids.IndexOf(ids.FirstOrDefault(i => i > 0));

            var receiveEventAddress = new nint(gatheringWindow->AtkUnitBase.AtkEventListener.vfunc[2]);
            var eventDelegate = Marshal.GetDelegateForFunctionPointer<ReceiveEventDelegate>(receiveEventAddress);

            var target = AtkStage.GetSingleton();
            var eventData = EventData.ForNormalTarget(target, &gatheringWindow->AtkUnitBase);
            var inputData = InputData.Empty();

            eventDelegate.Invoke(&gatheringWindow->AtkUnitBase.AtkEventListener, ClickLib.Enums.EventType.CHANGE, (uint)itemIndex, eventData.Data, inputData.Data);

        }
    }
}
