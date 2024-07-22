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
        public bool ShouldUseLuck(Span<uint> ids, Gatherable gatherable)
        {
            if (gatherable == null)
                return false;
            if (Player.Level < Actions.Luck.MinLevel)
                return false;
            if (!gatherable.GatheringData.IsHidden)
                return false;

            if (ids.Length > 0 && ids.Contains(gatherable.ItemId))
            {
                return false;
            }

            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.LuckConfig.MinimumGP)
                return false;
            if (Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.LuckConfig.MaximumGP)
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.LuckConfig, gatherable))
                return false;

            return GatherBuddy.Config.AutoGatherConfig.LuckConfig.UseAction;
        }

        public bool ShoulduseBYII(Gatherable gatherable)
        {
            if (Player.Level < Actions.Bountiful.MinLevel)
                return false;
            if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => GatheringUpStatuses.Contains(s.StatusId)))
                return false;
            if ((Dalamud.ClientState.LocalPlayer?.CurrentGp ?? 0) < GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MinimumGP)
                return false;
            if ((Dalamud.ClientState.LocalPlayer?.CurrentGp ?? 0) > GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MaximumGP)
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.BYIIConfig, gatherable))
                return false;

            return GatherBuddy.Config.AutoGatherConfig.BYIIConfig.UseAction;
        }

        public uint[] GatheringUpStatuses = new uint[]
        {
            756,  //BYII
            219,  //KYII
            1286, //KYI
        };

        public bool ShouldUseKingII(Gatherable gatherable)
        {
            if (Player.Level < Actions.Yield2.MinLevel)
                return false;
            if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => GatheringUpStatuses.Contains(s.StatusId)))
                return false;

            if (Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.MaximumGP
             || Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.MinimumGP)
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.YieldIIConfig, gatherable))
                return false;

            return GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.UseAction;
        }

        public bool ShouldUseKingI(Gatherable gatherable)
        {
            if (Player.Level < Actions.Yield1.MinLevel)
                return false;
            if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => GatheringUpStatuses.Contains(s.StatusId)))
                return false;

            if (Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.YieldIConfig.MaximumGP
             || Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.YieldIConfig.MinimumGP)
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.YieldIConfig, gatherable))
                return false;

            return GatherBuddy.Config.AutoGatherConfig.YieldIConfig.UseAction;
        }

        private unsafe void DoActionTasks(Gatherable desiredItem)
        {
            if (EzThrottler.Throttle("Gather", 10))
            {
                if (GatheringAddon == null && MasterpieceAddon == null)
                    return;

                if (MasterpieceAddon != null)
                {
                    DoCollectibles();
                }
                else if (GatheringAddon != null && !(desiredItem?.ItemData.IsCollectable ?? false))
                {
                    TaskManager.Enqueue(() => DoGatherWindowActions(desiredItem));
                }
                else if (GatheringAddon != null && (desiredItem?.ItemData.IsCollectable ?? false))
                {
                    TaskManager.Enqueue(() => DoGatherWindowTasks(desiredItem));
                }
            }
        }

        private unsafe void DoGatherWindowActions(IGatherable? desiredItem)
        {
            if (GatheringAddon == null)
                return;

            if (EzThrottler.Throttle("Gather Window", 2000))
            {
                Span<uint> ids = GatheringAddon->ItemIds;
                if (ShouldUseLuck(ids, desiredItem as Gatherable))
                    TaskManager.Enqueue(() => UseAction(Actions.Luck));
                if (ShouldUseKingII(desiredItem as Gatherable))
                    TaskManager.Enqueue(() => UseAction(Actions.Yield2));
                if (ShouldUseKingI(desiredItem as Gatherable))
                    TaskManager.Enqueue(() => UseAction(Actions.Yield1));
                if (ShoulduseBYII(desiredItem as Gatherable))
                    TaskManager.Enqueue(() => UseAction(Actions.Bountiful));
                TaskManager.Enqueue(() => DoGatherWindowTasks(desiredItem));
            }
        }

        private unsafe void UseAction(Actions.BaseAction act)
        {
            if (EzThrottler.Throttle($"Action: {act.Name}", 10))
            {
                var amInstance = ActionManager.Instance();
                if (amInstance->GetActionStatus(ActionType.Action, act.ActionID) == 0)
                {
                    amInstance->UseAction(ActionType.Action, act.ActionID);
                }
            }
        }

        private unsafe void DoCollectibles()
        {
            if (EzThrottler.Throttle("Collectibles", 10))
            {
                if (MasterpieceAddon == null)
                    return;

                if (MasterpieceAddon->AtkUnitBase.IsVisible)
                {
                    MasterpieceAddon->AtkUnitBase.IsVisible = false;
                }

                var textNode = MasterpieceAddon->AtkUnitBase.GetTextNodeById(47);
                var text     = textNode->NodeText.ToString();

                var integrityNode = MasterpieceAddon->AtkUnitBase.GetTextNodeById(126);
                var integrityText = integrityNode->NodeText.ToString();

                if (!int.TryParse(text, out var collectibility))
                {
                    collectibility = 99999; // default value
                    //Communicator.Print("Parsing failed, item is not collectable.");
                }

                if (!int.TryParse(integrityText, out var integrity))
                {
                    collectibility = 99999;
                    integrity      = 99999;
                }

                if (collectibility < 99999)
                {
                    LastCollectability = collectibility;
                    LastIntegrity      = integrity;

                    // Check if we need to gather on the last integrity point
                    if (LastIntegrity == 1  
                     && GatherBuddy.Config.AutoGatherConfig.GatherIfLastIntegrity
                     && LastCollectability >= GatherBuddy.Config.AutoGatherConfig.GatherIfLastIntegrityMinimumCollectibility)
                    {
                        TaskManager.Enqueue(() => UseAction(Actions.Collect));
                        return;
                    }

                    if (ShouldUseScrutiny(collectibility, integrity))
                        TaskManager.Enqueue(() => UseAction(Actions.Scrutiny));
                    if (ShouldUseScour(collectibility, integrity))
                        TaskManager.Enqueue(() => UseAction(Actions.Scour));
                    if (ShouldUseMeticulous(collectibility, integrity))
                        TaskManager.Enqueue(() => UseAction((Actions.Meticulous)));
                    if (ShouldUseSolidAge(collectibility, integrity))
                        TaskManager.Enqueue(() => UseAction(Actions.SolidAge));
                    if (ShouldUseWise(collectibility, integrity))
                        TaskManager.Enqueue(() => UseAction((Actions.Wise)));
                    if (ShouldCollect(collectibility, integrity))
                        TaskManager.Enqueue(() => UseAction(Actions.Collect));
                }
            }
        }

        private bool ShouldUseScour(int collectibility, int integrity)
        {
            if (Player.Level < Actions.Scour.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Scour.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.ScourConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.ScourConfig.MaximumGP)
                return false;

            if (collectibility <= GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore
             && collectibility >= GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore * 0.8
             && !Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 2418)
             && integrity > 0)
            {
                return GatherBuddy.Config.AutoGatherConfig.ScourConfig.UseAction;
            }

            return false;
        }

        private bool ShouldUseWise(int collectability, int integrity)
        {
            if (Player.Level < Actions.Wise.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Wise.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.WiseConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.WiseConfig.MaximumGP)
                return false;

            if (collectability >= GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore
             && Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 2765)
             && integrity < 4)
            {
                return GatherBuddy.Config.AutoGatherConfig.WiseConfig.UseAction;
            }

            return false;
        }

        private bool ShouldCollect(int collectability, int integrity)
        {
            if (Player.Level < Actions.Collect.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Collect.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.CollectConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.CollectConfig.MaximumGP)
                return false;
            if (collectability >= GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore && integrity > 0)
                return GatherBuddy.Config.AutoGatherConfig.CollectConfig.UseAction;

            return false;
        }

        private bool ShouldUseMeticulous(int collectability, int integrity)
        {
            if (Player.Level < Actions.Meticulous.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Meticulous.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.MaximumGP)
                return false;
            if (collectability <= (GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore * 0.8)
             && Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 2418))
                return true;
            if (collectability <= (GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore) && integrity > 0)
                return GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.UseAction;

            return false;
        }

        private bool ShouldUseScrutiny(int collectability, int integrity)
        {
            if (Player.Level < Actions.Scrutiny.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Scrutiny.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.MaximumGP)
                return false;
            if (collectability < (GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore * 0.8) && integrity > 2)
                return GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.UseAction;

            return false;
        }

        private bool ShouldUseSolidAge(int collectability, int integrity)
        {
            if (Player.Level < Actions.SolidAge.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.SolidAge.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.SolidAgeConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.SolidAgeConfig.MaximumGP)
                return false;
            if (collectability >= GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore && integrity < 4)
                return GatherBuddy.Config.AutoGatherConfig.SolidAgeConfig.UseAction;

            return false;
        }

        private unsafe bool CheckConditions(AutoGatherConfig.ActionConfig config, Gatherable gatherable)
        {
            if (!config.Conditions.UseConditions)
                return true;
            
            var currentIntegrityNode = GatheringAddon->AtkUnitBase.GetTextNodeById(9);
            var currentIntegrityText = currentIntegrityNode->NodeText.ToString();
            if (!int.TryParse(currentIntegrityText, out var currentIntegrity))
                currentIntegrity = 0;
            
            var maxIntegrityNode = GatheringAddon->AtkUnitBase.GetTextNodeById(12);
            var maxIntegrityText = maxIntegrityNode->NodeText.ToString();
            if (!int.TryParse(maxIntegrityText, out var maxIntegrity))
                maxIntegrity = 0;

            if (config.Conditions.MinimumIntegrity > maxIntegrity)
                return false;

            if (config.Conditions.UseOnlyOnFirstStep && currentIntegrity != maxIntegrity)
                return false;

            if (config.Conditions.FilterNodeTypes)
            {
                switch (gatherable.NodeType)
                {
                    case Enums.NodeType.Unknown: break;
                    case Enums.NodeType.Regular:   
                        if (!config.Conditions.NodeFilter.UseOnRegularNode)
                            return false;
                        break;
                    case Enums.NodeType.Unspoiled:
                        if (!config.Conditions.NodeFilter.UseOnUnspoiledNode)
                            return false;
                        break;
                    case Enums.NodeType.Ephemeral:
                        if (!config.Conditions.NodeFilter.UseOnEphemeralNode)
                            return false;
                        break;
                    case Enums.NodeType.Legendary:
                        if (!config.Conditions.NodeFilter.UseOnLegendaryNode)
                            return false;
                        break;
                }
            }
            return true;
        }


        public bool HiddenRevealed = false;

        private const int ActionCooldown = 2000;
    }
}
