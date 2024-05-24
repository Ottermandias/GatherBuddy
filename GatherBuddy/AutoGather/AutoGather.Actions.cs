using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        public bool ShouldUseLuck()
        {
            if (HiddenRevealed)
                return false;
            var targetNodeDataId = NearestNode.DataId;
            foreach (var node in DesiredNodesInZone)
            {
                if (node.Key == targetNodeDataId)
                {
                    return true;
                }
            }
            return false;
        }
        private void DoActionTasks()
        {
            TaskManager.Enqueue(UseLuck);
        }
        public bool HiddenRevealed = false;
        private unsafe void UseLuck()
        {
            var actionManager = ActionManager.Instance();
            switch (Svc.ClientState.LocalPlayer.ClassJob.Id)
            {
                case 17: //BTN
                    if (actionManager->GetActionStatus(ActionType.Action, 4095) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 4095);
                        HiddenRevealed = true;
                    }
                    break;
                case 16: //MIN
                    if (actionManager->GetActionStatus(ActionType.Action, 4081) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 4081);
                        HiddenRevealed = true;
                    }
                    break;
            }
        }

    }
}
