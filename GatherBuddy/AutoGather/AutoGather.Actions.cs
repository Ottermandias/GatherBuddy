using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using System;
using System.Linq;
using Dalamud.Game.ClientState.Conditions;
using ItemSlot = GatherBuddy.AutoGather.GatheringTracker.ItemSlot;
using GatherBuddy.CustomInfo;
using System.Collections.Generic;

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
            if (!CheckConditions(Actions.Luck, GatherBuddy.Config.AutoGatherConfig.LuckConfig, gatherable, false /*not used*/))
                return false;
            if (!gatherable.GatheringData.IsHidden && !gatherable.IsTreasureMap)
                return false;

            return true;
        }

        public bool ShouldUseBountiful(ItemSlot slot)
        {
            if (!CheckConditions(Actions.Bountiful, GatherBuddy.Config.AutoGatherConfig.BYIIConfig, slot.Item, slot.Rare))
                return false;
            if (Player.Status.Any(s => s.StatusId == Actions.BountifulII.EffectId))
                return false;
            if (CalculateBountifulBonus(slot.Item) < GatherBuddy.Config.AutoGatherConfig.BYIIConfig.GetOptionalProperty<int>("MinimumIncrease"))
                return false;

            return true;
        }
        public bool ShouldUseKingII(ItemSlot slot)
        {
            if (!CheckConditions(Actions.Yield2, GatherBuddy.Config.AutoGatherConfig.YieldIIConfig, slot.Item, slot.Rare))
                return false;

            return true;
        }

        public bool ShouldUseKingI(ItemSlot slot)
        {
            if (!CheckConditions(Actions.Yield1, GatherBuddy.Config.AutoGatherConfig.YieldIConfig, slot.Item, slot.Rare))
                return false;

            return true;
        }

        private bool ShouldUseGivingLand(ItemSlot slot)
        {
            if (!CheckConditions(Actions.GivingLand, GatherBuddy.Config.AutoGatherConfig.GivingLandConfig, slot.Item, slot.Rare))
                return false;
            if (!IsGivingLandOffCooldown)
                return false;
            if (InventoryCount(slot.Item) > 9999 - GivingLandYeild - slot.Yield)
                return false;

            return true;
        }

        private unsafe bool ShouldUseTwelvesBounty(ItemSlot slot)
        {
            if (!CheckConditions(Actions.TwelvesBounty, GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig, slot.Item, slot.Rare))
                return false;
            if (InventoryCount(slot.Item) > 9999 - 3 - slot.Yield - (slot.RandomYield ? GivingLandYeild : 0))
                return false;

            return true;
        }


        private unsafe void DoActionTasks(Gatherable? desiredItem)
        {
            if (MasterpieceAddon != null)
            {
                var left = int.MaxValue;
                if (GatherBuddy.Config.AutoGatherConfig.AbandonNodes)
                { 
                    if (desiredItem == null) throw new NoGatherableItemsInNodeExceptions();
                    left = (int)QuantityTotal(desiredItem) - InventoryCount(desiredItem);
                    if (left < 1) throw new NoGatherableItemsInNodeExceptions();
                }

                DoCollectibles(left);
            }
            else if (GatheringAddon != null && NodeTarcker.Ready)
            {
                DoGatherWindowActions(desiredItem);
            }
            if (MasterpieceAddon == null)
                CurrentRotation = null;
        }

        private unsafe void DoGatherWindowActions(Gatherable? desiredItem)
        {
            if (LuckUsed[1] && !LuckUsed[2] && NodeTarcker.Revisit) LuckUsed = new(0);

            //Use The Giving Land out of order to gather random crystals.
            if (ShouldUseGivingLandOutOfOrder(desiredItem))
            {
                EnqueueActionWithDelay(() => UseAction(Actions.GivingLand));
                return;
            }

            if (!HasGivingLandBuff && ShouldUseLuck(desiredItem))
            {
                LuckUsed[1] = true;
                LuckUsed[2] = NodeTarcker.Revisit;
                EnqueueActionWithDelay(() => UseAction(Actions.Luck));
                return;
            }

            var (useSkills, slot) = GetItemSlotToGather(desiredItem);
            if (useSkills)
            {
                if (ShouldUseWise(NodeTarcker.Integrity, NodeTarcker.MaxIntegrity))
                    EnqueueActionWithDelay(() => UseAction(Actions.Wise));
                else if (ShouldUseSolidAgeGatherables(slot))
                    EnqueueActionWithDelay(() => UseAction(Actions.SolidAge));
                else if (ShouldUseGivingLand(slot))
                    EnqueueActionWithDelay(() => UseAction(Actions.GivingLand));
                else if (ShouldUseTwelvesBounty(slot))
                    EnqueueActionWithDelay(() => UseAction(Actions.TwelvesBounty));
                else if (ShouldUseKingII(slot))
                    EnqueueActionWithDelay(() => UseAction(Actions.Yield2));
                else if (ShouldUseKingI(slot))
                    EnqueueActionWithDelay(() => UseAction(Actions.Yield1));
                else if (ShouldUseBountiful(slot))
                    EnqueueActionWithDelay(() => UseAction(Actions.Bountiful));
                else
                    EnqueueGatherItem(slot);
            }
            else
            {
                EnqueueGatherItem(slot);
            }
        }

        private bool ShouldUseGivingLandOutOfOrder(Gatherable? desiredItem)
        {
            if (GatherBuddy.Config.AutoGatherConfig.UseGivingLandOnCooldown && desiredItem != null && desiredItem.NodeType == Enums.NodeType.Regular)
            {
                var anyCrystal = GetAnyCrystalInNode();
                if (anyCrystal != null && ShouldUseGivingLand(anyCrystal))
                    return true;
            }

            return false;
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

        private void EnqueueActionWithDelay(Action action)
        {
            var delay = GatherBuddy.Config.AutoGatherConfig.ExecutionDelay;
            if (delay > 0)
            {
                TaskManager.DelayNext((int)delay);
            }

            TaskManager.Enqueue(action);
        }

        private unsafe void DoCollectibles(int itemsLeft)
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

                var collectibleAction = CurrentRotation.GetNextAction(MasterpieceAddon, itemsLeft);

                EnqueueActionWithDelay(() => UseAction(collectibleAction));
            }
        }

        private static bool ShouldUseWise(int integrity, int maxIntegrity)
        {
            if (integrity == maxIntegrity)
                return false;
            if (Player.Level < Actions.Wise.MinLevel)
                return false;
            if (!Player.Status.Any(s => s.StatusId == Actions.SolidAge.EffectId))
                return false;

            return true;
        }

        private bool ShouldUseSolidAgeGatherables(ItemSlot slot)
        {
            if (!CheckConditions(Actions.SolidAge, GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig, slot.Item, slot.Rare))
                return false;
            var yield = slot.Yield;
            if (Dalamud.ClientState.LocalPlayer!.StatusList.Any(s => s.StatusId == Actions.Bountiful.EffectId))
                yield -= 1;
            if (Dalamud.ClientState.LocalPlayer!.StatusList.Any(s => s.StatusId == Actions.BountifulII.EffectId))
                yield -= CalculateBountifulBonus(slot.Item);
            if (yield < GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.GetOptionalProperty<int>("MinimumYield"))
                return false;

            return true;
        }

        private bool CheckConditions(Actions.BaseAction action, AutoGatherConfig.ActionConfig config, Gatherable item, bool rare)
        {
            if (config.UseAction == false)
                return false;
            if (Player.Level < action.MinLevel)
                return false;
            if (Player.Object.CurrentGp < action.GpCost)
                return false;
            if (Player.Object.CurrentGp < config.MinimumGP)
                return false;
            if (Player.Object.CurrentGp > config.MaximumGP)
                return false;
            if (action.EffectId != 0 && Player.Status.Any(s => s.StatusId == action.EffectId))
                return false;
            if (action.QuestID != 0 && !QuestManager.IsQuestComplete(action.QuestID))
                return false; 
            if (item.IsCrystal && config.TryGetOptionalProperty<bool>("UseWithCystals", out var useWithCystals) && !useWithCystals)
                return false;
            if (action.EffectType is Actions.EffectType.CrystalsYield && !item.IsCrystal)
                return false;
            if (action.EffectType is Actions.EffectType.Integrity && NodeTarcker.Integrity == NodeTarcker.MaxIntegrity)
                return false;
            if (action.EffectType is not Actions.EffectType.Other and not Actions.EffectType.GatherChance && rare)
                return false;

            if (config.Conditions.UseConditions)
            {

                if (config.Conditions.RequiredIntegrity > NodeTarcker.MaxIntegrity)
                    return false;
                if (config.Conditions.UseOnlyOnFirstStep && NodeTarcker.Touched)
                    return false;

                if (config.Conditions.FilterNodeTypes)
                {
                    var node = config.Conditions.NodeFilter.GetNodeConfig(item.NodeType);

                    if (!node.Use || item.Level < node.NodeLevel && !(node.AvoidCap && Player.Object.CurrentGp == Player.Object.MaxGp))
                        return false;
                }
            }

            return true;
        }

        private static int CalculateBountifulBonus(Gatherable item)
        {
            if (!QuestManager.IsQuestComplete(Actions.BountifulII.QuestID))
                return 1;
            
            try
            {
                var glvl = item.GatheringData.GatheringItemLevel.Row;
                var baseValue = WorldData.BaseGathering[glvl];
                var stat = CharacterGatheringStat;

                if (stat >= (int)(baseValue * 1.1))
                    return 3;
                if (stat >= (int)(baseValue * 0.9))
                    return 2;

                return 1;
            }
            catch (KeyNotFoundException)
            {
                return 1;
            }
        }

        private bool ActivateGatheringBuffs(bool activateTruth)
        {
            if (!Player.Status.Any(s => s.StatusId == Actions.Prospect.EffectId) && Player.Level >= Actions.Prospect.MinLevel)
            {
                EnqueueActionWithDelay(() => UseAction(Actions.Prospect));
                return true;
            }
            if (!Player.Status.Any(s => s.StatusId == Actions.Sneak.EffectId) && Player.Level >= Actions.Sneak.MinLevel)
            {
                EnqueueActionWithDelay(() => UseAction(Actions.Sneak));
                return true;
            }
            if (activateTruth && !Player.Status.Any(s => s.StatusId == Actions.Truth.EffectId))
            {
                EnqueueActionWithDelay(() => UseAction(Actions.Truth));
                return true;
            }
            return false;
        }
    }
}
