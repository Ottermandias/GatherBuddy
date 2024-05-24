using ClickLib.Bases;
using ClickLib.Structures;
using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
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
                TaskManager.Enqueue(DoGatherWindowTasks);
            }
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


            TaskManager.Enqueue(DoActionTasks(ids));
            var itemIndex = ids.IndexOf(DesiredItem?.ItemId ?? 0);
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
