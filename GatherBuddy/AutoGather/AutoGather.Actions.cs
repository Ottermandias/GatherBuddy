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
        public bool ShoulduseBYII()
        {
            if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 1286 || s.StatusId == 756))
                return false;
            if ((Dalamud.ClientState.LocalPlayer?.CurrentGp ?? 0) < 100)
                return false;
            return true;
        }
        private void DoActionTasks()
        {
            if (ShouldUseLuck())
            {
                TaskManager.Enqueue(UseLuck);
            }
            if (ShoulduseBYII())
            {
                TaskManager.Enqueue(UseBYII);
            }
            TaskManager.Enqueue(DoGatherWindowTasks);
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

        private unsafe void UseBYII()
        {
            var actionManager = ActionManager.Instance();
            switch (Svc.ClientState.LocalPlayer.ClassJob.Id)
            {
                case 17:
                    if (actionManager->GetActionStatus(ActionType.Action, 273) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 273);
                    }
                    else if (actionManager->GetActionStatus(ActionType.Action, 4087) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 4087);
                    }
                    break;
                case 16:
                    if (actionManager->GetActionStatus(ActionType.Action, 272) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 272);
                    }
                    else if (actionManager->GetActionStatus(ActionType.Action, 4073) == 0)
                    {
                        actionManager->UseAction(ActionType.Action, 4073);
                    }
                    break;
            }
        }
    }
}
