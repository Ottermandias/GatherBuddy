using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        private unsafe void Dismount()
        {
            var am = ActionManager.Instance();
            am->UseAction(ActionType.Mount, 0);
        }

        private unsafe void MountUp()
        {
            var am = ActionManager.Instance();
            var mount = GatherBuddy.Config.AutoGatherMountId;
            if (am->GetActionStatus(ActionType.Mount, mount) != 0) return;
            am->UseAction(ActionType.Mount, mount);
        }

        private void MoveToCloseNode()
        {
            if (NearestNodeDistance < 3)
            {
                TaskManager.Enqueue(DoGatherTasks);
                return;
            }
        }
    }
}
