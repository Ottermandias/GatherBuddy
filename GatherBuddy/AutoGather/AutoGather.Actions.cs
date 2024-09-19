using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using System;
using System.Linq;
using Dalamud.Game.ClientState.Conditions;
using ItemSlot = GatherBuddy.AutoGather.GatheringTracker.ItemSlot;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        public bool ShouldUseLuck(Gatherable? gatherable)
        {
            if (gatherable == null)
                return false;
            if (LuckUsed[1] || NodeTarcker.HiddenRevealed)
                return false;
            if (Player.Level < Actions.Luck.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Luck.GpCost)
                return false;
            if (!gatherable.GatheringData.IsHidden && !gatherable.IsTreasureMap)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.LuckConfig.MinimumGP)
                return false;
            if (Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.LuckConfig.MaximumGP)
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.LuckConfig, gatherable))
                return false;

            return GatherBuddy.Config.AutoGatherConfig.LuckConfig.UseAction;
        }

        public bool ShoulduseBYII(ItemSlot slot)
        {
            if (Player.Level < Actions.Bountiful.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Bountiful.GpCost)
                return false;
            if (Dalamud.ClientState.LocalPlayer!.StatusList.Any(s => BountifulYieldStatuses.Contains(s.StatusId)))
                return false;
            if (Dalamud.ClientState.LocalPlayer!.CurrentGp < GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MinimumGP)
                return false;
            if (Dalamud.ClientState.LocalPlayer!.CurrentGp > GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MaximumGP)
                return false;
            if (slot.Item.IsCrystal && !GatherBuddy.Config.AutoGatherConfig.BYIIConfig.GetOptionalProperty<bool>("UseWithCystals"))
                return false;
            if (slot.Rare)
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.BYIIConfig, slot.Item))
                return false;

            return GatherBuddy.Config.AutoGatherConfig.BYIIConfig.UseAction;
        }

        public uint[] KingsYieldStatuses =
        [
            219, //KYI and KYII
        ];

        public uint[] BountifulYieldStatuses =
        [
            756,  //BYI?
            1286, //BYII
        ];

        public bool ShouldUseKingII(ItemSlot slot)
        {
            if (Player.Level < Actions.Yield2.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Yield2.GpCost)
                return false;
            if (Dalamud.ClientState.LocalPlayer!.StatusList.Any(s => KingsYieldStatuses.Contains(s.StatusId)))
                return false;
            if (Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.MaximumGP
             || Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.MinimumGP)
                return false;
            if (slot.Item.IsCrystal && !GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.GetOptionalProperty<bool>("UseWithCystals"))
                return false;
            if (slot.Rare)
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.YieldIIConfig, slot.Item))
                return false;

            return GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.UseAction;
        }

        public bool ShouldUseKingI(ItemSlot slot)
        {
            if (Player.Level < Actions.Yield1.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Yield1.GpCost)
                return false;
            if (Dalamud.ClientState.LocalPlayer!.StatusList.Any(s => KingsYieldStatuses.Contains(s.StatusId)))
                return false;
            if (Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.YieldIConfig.MaximumGP
             || Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.YieldIConfig.MinimumGP)
                return false;
            if (slot.Item.IsCrystal && !GatherBuddy.Config.AutoGatherConfig.YieldIConfig.GetOptionalProperty<bool>("UseWithCystals"))
                return false;
            if (slot.Rare)
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.YieldIConfig, slot.Item))
                return false;

            return GatherBuddy.Config.AutoGatherConfig.YieldIConfig.UseAction;
        }

        private bool ShouldUseGivingLand(ItemSlot slot)
        {
            if (!slot.Item.IsCrystal)
                return false;
            if (!GatherBuddy.Config.AutoGatherConfig.GivingLandConfig.UseAction)
                return false;
            if (Player.Level < Actions.GivingLand.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.GivingLand.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.GivingLandConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.GivingLandConfig.MaximumGP)
                return false;
            if (HasGivingLandBuff)
                return false;
            if (!IsGivingLandOffCooldown)
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.GivingLandConfig, slot.Item))
                return false;
            if (InventoryCount(slot.Item) > 9999 - GivingLandYeild - slot.Yield)
                return false;

            return true;
        }

        private unsafe bool ShouldUseTwelvesBounty(ItemSlot slot)
        {
            if (!slot.Item.IsCrystal)
                return false;
            if (!GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig.UseAction)
                return false;
            if (Player.Level < Actions.TwelvesBounty.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.TwelvesBounty.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig.MaximumGP)
                return false;
            if (Dalamud.ClientState.LocalPlayer!.StatusList.Any(s => s.StatusId == 825))
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig, slot.Item))
                return false;
            if (InventoryCount(slot.Item) > 9999 - 3 - slot.Yield - (slot.RandomYield ? GivingLandYeild : 0))
                return false;

            return true;
        }


        private unsafe void DoActionTasks(Gatherable? desiredItem)
        {
            if (MasterpieceAddon != null)
            {
                DoCollectibles();
            }
            else if (GatheringAddon != null && NodeTarcker.Ready)
            {
                if (desiredItem?.ItemData.IsCollectable == true)
                    EnqueueGatherItem(GetItemSlotToGather(desiredItem).Slot);
                else
                    DoGatherWindowActions(desiredItem);
            }
            if (MasterpieceAddon == null)
                CurrentRotation = null;
        }

        private unsafe void DoGatherWindowActions(Gatherable? desiredItem)
        {
            if (LuckUsed[1] && !LuckUsed[2] && NodeTarcker.Revisit) LuckUsed = new(0);

            if (!HasGivingLandBuff && ShouldUseLuck(desiredItem))
            {
                var wouldUseGivingLand = GatherBuddy.Config.AutoGatherConfig.UseGivingLandOnCooldown;
                if (wouldUseGivingLand)
                {
                    var anyCrystall = NodeTarcker.Aviable.Where(s => s.Item.IsCrystal).OrderBy(s => InventoryCount(s.Item)).FirstOrDefault();
                    if (anyCrystall == null || !ShouldUseGivingLand(anyCrystall))
                        wouldUseGivingLand = false;
                }
                if (!wouldUseGivingLand)
                {
                    LuckUsed[1] = true;
                    LuckUsed[2] = NodeTarcker.Revisit;
                    EnqueueGatherAction(() => UseAction(Actions.Luck));
                    return;
                }
            }

            var (useSkills, slot) = GetItemSlotToGather(desiredItem);
            if (useSkills)
            {
                if (ShouldUseWise(NodeTarcker.Integrity, NodeTarcker.MaxIntegrity))
                    EnqueueGatherAction(() => UseAction(Actions.Wise));
                else if (ShouldUseSolidAgeGatherables(slot))
                    EnqueueGatherAction(() => UseAction(Actions.SolidAge));
                else if (ShouldUseGivingLand(slot))
                    EnqueueGatherAction(() => UseAction(Actions.GivingLand));
                else if (ShouldUseTwelvesBounty(slot))
                    EnqueueGatherAction(() => UseAction(Actions.TwelvesBounty));
                else if (ShouldUseKingII(slot))
                    EnqueueGatherAction(() => UseAction(Actions.Yield2));
                else if (ShouldUseKingI(slot))
                    EnqueueGatherAction(() => UseAction(Actions.Yield1));
                else if (ShoulduseBYII(slot))
                    EnqueueGatherAction(() => UseAction(Actions.Bountiful));
                else
                    EnqueueGatherItem(slot);
            }
            else
            {
                EnqueueGatherItem(slot);
            }
        }

        private unsafe void UseAction(Actions.BaseAction act)
        {
            var amInstance = ActionManager.Instance();
            if (amInstance->GetActionStatus(ActionType.Action, act.ActionID) == 0)
            {
                //Communicator.Print("Action used: " + act.Name);
                amInstance->UseAction(ActionType.Action, act.ActionID);
            }
        }

        private void EnqueueGatherAction(Action action, int additionalDelay = 0)
        {
            TaskManager.Enqueue(action);
            if (additionalDelay > 0)
                TaskManager.DelayNextImmediate(additionalDelay);
            TaskManager.Enqueue(() => !Svc.Condition[ConditionFlag.Gathering42]);
            //Communicator.Print("Ready for next action.");
        }

        private unsafe void DoCollectibles()
        {
            if (MasterpieceAddon == null)
                return;

            CurrentRotation ??= new CollectableRotation(GatherBuddy.Config.AutoGatherConfig.MinimumGPForCollectableRotation);

            var textNode = MasterpieceAddon->AtkUnitBase.GetTextNodeById(6);
            if (textNode == null)
                return;
            var text = textNode->NodeText.ToString();

            var integrityNode = MasterpieceAddon->AtkUnitBase.GetTextNodeById(126);
            if (integrityNode == null)
                return;
            var integrityText = integrityNode->NodeText.ToString();

            if (!int.TryParse(text, out var collectibility))
            {
                collectibility = 99999; // default value
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

                var collectibleAction = CurrentRotation.GetNextAction(MasterpieceAddon);
                if (collectibleAction == null)
                {
                    GatherBuddy.Log.Debug("Collectible action was null, all actions are disabled by user");
                    return;
                }

                EnqueueGatherAction(() => { UseAction(collectibleAction); }, 250);
            }
        }

        private static bool ShouldUseWise(int integrity, int maxIntegrity)
        {
            if (Player.Level < Actions.Wise.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Wise.GpCost)
                return false;

            if (Dalamud.ClientState.LocalPlayer!.StatusList.Any(s => s.StatusId == 2765) && integrity < maxIntegrity)
                return true;

            return false;
        }


        private static bool ShouldUseMeticulous()
        {
            if (Player.Level < Actions.Meticulous.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Meticulous.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.MaximumGP)
                return false;

            return GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.UseAction;
        }

        private static bool ShouldUseScour()
        {
            if (Player.Level < Actions.Brazen.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Brazen.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.ScourConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.ScourConfig.MaximumGP)
                return false;

            return GatherBuddy.Config.AutoGatherConfig.ScourConfig.UseAction;
        }

        private static bool ShouldUseBrazen()
        {
            if (Player.Level < Actions.Meticulous.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Meticulous.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.BrazenConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.BrazenConfig.MaximumGP)
                return false;

            return GatherBuddy.Config.AutoGatherConfig.BrazenConfig.UseAction;
        }

        private static bool ShouldUseScrutiny()
        {
            if (Player.Level < Actions.Scrutiny.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Scrutiny.GpCost)
                return false;
            if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.MinimumGP
             || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.MaximumGP)
                return false;
            if (!Dalamud.ClientState.LocalPlayer!.StatusList.Any(s => s.StatusId == 757))
                return GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.UseAction;

            return false;
        }

        private static bool ShouldSolidAgeCollectables(int integrity, int maxIntegrity)
            => ShouldUseSolidAge(GatherBuddy.Config.AutoGatherConfig.SolidAgeCollectablesConfig, integrity, maxIntegrity);

        private bool ShouldUseSolidAgeGatherables(ItemSlot slot)
        {
            if (slot.Yield < GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.GetOptionalProperty<int>("MinimumYield"))
                return false;
            if (slot.Item.IsCrystal && !GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.GetOptionalProperty<bool>("UseWithCystals"))
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig, slot.Item))
                return false;

            if (!ShouldUseSolidAge(GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig, NodeTarcker.Integrity, NodeTarcker.MaxIntegrity))
                return false;

            return true;
        }

        private static bool ShouldUseSolidAge(AutoGatherConfig.ActionConfig SolidAgeConfig, int integrity, int maxIntegrity)
        {
            if (Player.Level < Actions.SolidAge.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.SolidAge.GpCost)
                return false;
            if (Player.Object.CurrentGp < SolidAgeConfig.MinimumGP
             || Player.Object.CurrentGp > SolidAgeConfig.MaximumGP)
                return false;
            if (!Dalamud.ClientState.LocalPlayer!.StatusList.Any(s => s.StatusId == 2765) && integrity < maxIntegrity)
                return SolidAgeConfig.UseAction;

            return false;
        }

        private unsafe bool CheckConditions(AutoGatherConfig.ActionConfig config, Gatherable item)
        {
            if (!config.Conditions.UseConditions)
                return true;

            if (config.Conditions.RequiredIntegrity > NodeTarcker.MaxIntegrity)
                return false;

            if (config.Conditions.UseOnlyOnFirstStep && NodeTarcker.Touched)
                return false;

            if (config.Conditions.FilterNodeTypes)
            {
                var node = config.Conditions.NodeFilter.GetNodeConfig(item.NodeType);

                if (!node.Use || item.Level < node.NodeLevel && !(node.AvoidCap && IsGpMax))
                    return false;
            }

            return true;
        }

        private static bool IsGpMax
            => Dalamud.ClientState.LocalPlayer?.CurrentGp == Dalamud.ClientState.LocalPlayer?.MaxGp;

        private const int ActionCooldown = 2000;
    }
}
