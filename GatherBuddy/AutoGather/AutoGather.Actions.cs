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
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using Lumina.Data.Parsing;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        public bool ShouldUseLuck(List<uint> ids, Gatherable gatherable)
        {
            if (gatherable == null)
                return false;
            if (Player.Level < Actions.Luck.MinLevel)
                return false;
            if (!gatherable.GatheringData.IsHidden)
                return false;

            if (ids.Count > 0 && ids.Any(i => i == gatherable.ItemId))
            {
                return false;
            }

            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.LuckConfig.MinimumGP)
                return false;
            if (Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.LuckConfig.MaximumGP)
                return false;

            return GatherBuddy.Config.AutoGatherConfig.LuckConfig.UseAction;
        }

        public bool ShoulduseBYII()
        {
            if (Player.Level < Actions.Bountiful.MinLevel)
                return false;
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
            if (EzThrottler.Throttle("Gather", 1600))
            {
                var gatheringWindow = (AddonGathering*)Dalamud.GameGui.GetAddonByName("Gathering", 1);

                var gatheringMasterpiece = (AddonGatheringMasterpiece*)Dalamud.GameGui.GetAddonByName("GatheringMasterpiece", 1);
                if (gatheringWindow == null && gatheringMasterpiece == null)
                    return;

                var desiredItem = ItemsToGatherInZone.FirstOrDefault();

                if (gatheringMasterpiece != null && (desiredItem?.ItemData.IsCollectable ?? false))
                {
                    DoCollectibles(gatheringMasterpiece);
                }
                else if (gatheringWindow != null && !(desiredItem?.ItemData.IsCollectable ?? false))
                {
                    TaskManager.Enqueue(() => DoGatherWindowActions(gatheringWindow, desiredItem));
                }
                else if (gatheringWindow != null && (desiredItem?.ItemData.IsCollectable ?? false))
                {
                    TaskManager.Enqueue(() => DoGatherWindowTasks(gatheringWindow, desiredItem));
                }
            }
        }

        private unsafe void DoGatherWindowActions(AddonGathering* gatheringWindow, IGatherable? desiredItem)
        {
            List<uint> ids = new List<uint>()
            {
                gatheringWindow->GatheredItemId1,
                gatheringWindow->GatheredItemId2,
                gatheringWindow->GatheredItemId3,
                gatheringWindow->GatheredItemId4,
                gatheringWindow->GatheredItemId5,
                gatheringWindow->GatheredItemId6,
                gatheringWindow->GatheredItemId7,
                gatheringWindow->GatheredItemId8,
            };
            if (ShouldUseLuck(ids, desiredItem as Gatherable))
                TaskManager.Enqueue(() => UseAction(Actions.Luck));
            if (ShoulduseBYII())
                TaskManager.Enqueue(() => UseAction(Actions.Bountiful));
            TaskManager.Enqueue(() => DoGatherWindowTasks(gatheringWindow, desiredItem));
        }

        private unsafe void UseAction(Actions.BaseAction act)
        {
            if (EzThrottler.Throttle($"Action: {act.Name}", ActionCooldown))
            {
                var amInstance = ActionManager.Instance();
                if (amInstance->GetActionStatus(ActionType.Action, act.ActionID) == 0)
                {
                    amInstance->UseAction(ActionType.Action, act.ActionID);
                }
            }
        }

        private unsafe void DoCollectibles(AddonGatheringMasterpiece* gatheringMasterpiece)
        {
            if (gatheringMasterpiece->AtkUnitBase.IsVisible)
            {
                gatheringMasterpiece->AtkUnitBase.IsVisible = false;
            }
            var textNode = gatheringMasterpiece->AtkUnitBase.GetTextNodeById(47);
            var text     = textNode->NodeText.ToString();


            if (!int.TryParse(text, out var collectibility))
            {
                collectibility = 99999; // default value
                //Communicator.Print("Parsing failed, item is not collectable.");
            }

            if (collectibility < 99999)
            {
                LastCollectability = collectibility;
                if (ShouldUseScrutiny(collectibility))
                    TaskManager.Enqueue(() => UseAction(Actions.Scrutiny));
                if (ShouldUseMeticulous(collectibility))
                    TaskManager.Enqueue(() => UseAction((Actions.Meticulous)));
                if (ShouldUseSolidAge(collectibility))
                    TaskManager.Enqueue(() => UseAction(Actions.SolidAge));
                if (ShouldUseWise(collectibility))
                    TaskManager.Enqueue(() => UseAction((Actions.Wise)));
                if (ShouldCollect(collectibility))
                    TaskManager.Enqueue(() => UseAction(Actions.Collect));
            }
        }

        private bool ShouldUseWise(int collectability)
        {
            if (Player.Level < Actions.Wise.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Wise.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.WiseConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.WiseConfig.MaximumGP)
                return false;
            if (collectability == 1000 && Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 2765))
            {
                return true;
            }

            return false;
        }

        private bool ShouldCollect(int collectability)
        {
            if (Player.Level < Actions.Collect.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Collect.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.CollectConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.CollectConfig.MaximumGP)
                return false;
            if (collectability == 1000)
                return true;

            return false;
        }

        private bool ShouldUseMeticulous(int collectability)
        {
            if (Player.Level < Actions.Meticulous.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Meticulous.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.MaximumGP)
                return false;
            if (collectability < 1000)
                return true;

            return false;
        }

        private bool ShouldUseScrutiny(int collectability)
        {
            if (Player.Level < Actions.Scrutiny.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Scrutiny.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.MaximumGP)
                return false;
            if (collectability < 800)
                return true;

            return false;
        }

        private bool ShouldUseSolidAge(int collectability)
        {
            if (Player.Level < Actions.SolidAge.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.SolidAge.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.SolidAgeConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.SolidAgeConfig.MaximumGP)
                return false;
            if (collectability == 1000)
                return true;

            return false;
        }


        public bool HiddenRevealed = false;

        private const int ActionCooldown = 2000;
    }
}
