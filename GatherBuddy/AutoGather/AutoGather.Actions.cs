using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI;
using GatherBuddy.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        public bool ShouldUseLuck(List<uint> ids)
        {
            var gatherable = ItemsToGather.FirstOrDefault() as Gatherable;
            if (gatherable == null) return false;
            if (Player.Level < 55) return false;
            if (!gatherable.GatheringData.IsHidden) return false;
            if (ids.Count > 0 && ids.Any(i => i == gatherable.ItemId))
            {
                return false;
            }
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.LuckConfig.MinimumGP) return false;
            if (Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.LuckConfig.MaximumGP) return false;
            if (HiddenRevealed) return false;
            return GatherBuddy.Config.AutoGatherConfig.LuckConfig.UseAction;
        }
        public bool ShoulduseBYII()
        {
            if (Player.Level < 68) return false;
            if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 1286 || s.StatusId == 756))
                return false;
            if ((Dalamud.ClientState.LocalPlayer?.CurrentGp ?? 0) < GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MinimumGP)
                return false;
            if ((Dalamud.ClientState.LocalPlayer?.CurrentGp ?? 0) > GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MaximumGP)
                return false;
            return GatherBuddy.Config.AutoGatherConfig.BYIIConfig.UseAction;
        }
        private unsafe void DoActionTasks()
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
            if (ShouldUseLuck(ids))
            {
                TaskManager.Enqueue(UseLuck);
            }
            else if (ShoulduseBYII())
            {
                TaskManager.Enqueue(UseBYII);
            }
            else if (GatherBuddy.Config.AutoGatherConfig.DoGathering)
            {
                TaskManager.Enqueue(() => DoGatherWindowTasks(ids, gatheringWindow));
                TaskManager.EnqueueDelay(1200);
            }
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
