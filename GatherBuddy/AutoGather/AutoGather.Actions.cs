using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using System;
using System.Linq;
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
            if (LuckUsed[1] || NodeTracker.HiddenRevealed)
                return false;
            if (!gatherable.GatheringData.IsHidden && !gatherable.IsTreasureMap)
                return false;

            return true;
        }

        public bool ShouldUseBountiful(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.Bountiful, config.Bountiful, slot.Item, slot.Rare))
                return false;
            if (Player.Status.Any(s => s.StatusId == Actions.BountifulII.EffectId))
                return false;
            if (CalculateBountifulBonus(slot.Item) < config.Bountiful.MinYieldBonus)
                return false;

            return true;
        }
        public bool ShouldUseKingII(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.Yield2, config.Yield2, slot.Item, slot.Rare))
                return false;

            return true;
        }

        public bool ShouldUseKingI(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.Yield1, config.Yield1, slot.Item, slot.Rare))
                return false;

            return true;
        }

        private bool ShouldUseGivingLand(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.GivingLand, config.GivingLand, slot.Item, slot.Rare))
                return false;
            if (!IsGivingLandOffCooldown)
                return false;
            if (InventoryCount(slot.Item) > 9999 - GivingLandYeild - slot.Yield)
                return false;

            return true;
        }

        private unsafe bool ShouldUseTwelvesBounty(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.TwelvesBounty, config.TwelvesBounty, slot.Item, slot.Rare))
                return false;
            if (InventoryCount(slot.Item) > 9999 - 3 - slot.Yield - (slot.RandomYield ? GivingLandYeild : 0))
                return false;

            return true;
        }

        private unsafe bool ShouldUseBoonI(ItemSlot slot)
        {
            if (!CheckConditions(Actions.BoonI, GatherBuddy.Config.AutoGatherConfig.BoonIConfig, slot.Item, slot.Rare))
                return false;
            if (slot.BoonChance < GatherBuddy.Config.AutoGatherConfig.BoonIConfig.GetOptionalProperty<int>("MinBoonChance"))
                return false;

            return true;
        }

        private unsafe bool ShouldUseBoonII(ItemSlot slot)
        {
            if (!CheckConditions(Actions.BoonII, GatherBuddy.Config.AutoGatherConfig.BoonIIConfig, slot.Item, slot.Rare))
                return false;
            if (slot.BoonChance < GatherBuddy.Config.AutoGatherConfig.BoonIIConfig.GetOptionalProperty<int>("MinBoonChance"))
                return false;

            return true;
        }

        private unsafe bool ShouldUseTiding(ItemSlot slot)
        {
            if (!CheckConditions(Actions.Tidings, GatherBuddy.Config.AutoGatherConfig.TidingsConfig, slot.Item, slot.Rare))
                return false;
            if (slot.BoonChance < GatherBuddy.Config.AutoGatherConfig.TidingsConfig.GetOptionalProperty<int>("MinBoonChance"))
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

                DoCollectibles(MatchConfigPreset(desiredItem), left);
            }
            else if (GatheringAddon != null && NodeTracker.Ready)
            {
                DoGatherWindowActions(desiredItem);
            }
            if (MasterpieceAddon == null)
                CurrentRotation = null;
        }

        private unsafe void DoGatherWindowActions(Gatherable? desiredItem)
        {
            if (LuckUsed[1] && !LuckUsed[2] && NodeTracker.Revisit) LuckUsed = new(0);

            //Use The Giving Land out of order to gather random crystals.
            if (ShouldUseGivingLandOutOfOrder(desiredItem))
            {
                EnqueueActionWithDelay(() => UseAction(Actions.GivingLand));
                return;
            }

            if (!HasGivingLandBuff && ShouldUseLuck(desiredItem))
            {
                LuckUsed[1] = true;
                LuckUsed[2] = NodeTracker.Revisit;
                EnqueueActionWithDelay(() => UseAction(Actions.Luck));
                return;
            }

            var (useSkills, slot) = GetItemSlotToGather(desiredItem);
            if (useSkills)
            {
                var config = MatchConfigPreset(slot.Item).GatherableActions;
                if (ShouldUseWise(NodeTracker.Integrity, NodeTracker.MaxIntegrity))
                    EnqueueActionWithDelay(() => UseAction(Actions.Wise));
                else if(ShouldUseBoonI(slot))
                    EnqueueActionWithDelay(() => UseAction(Actions.BoonI));
                else if (ShouldUseBoonII(slot))
                    EnqueueActionWithDelay(() => UseAction(Actions.BoonII));
                else if (ShouldUseTiding(slot))
                    EnqueueActionWithDelay(() => UseAction(Actions.Tidings));
                else if (ShouldUseSolidAgeGatherables(slot, config))
                    EnqueueActionWithDelay(() => UseAction(Actions.SolidAge));
                else if (ShouldUseGivingLand(slot, config))
                    EnqueueActionWithDelay(() => UseAction(Actions.GivingLand));
                else if (ShouldUseTwelvesBounty(slot, config))
                    EnqueueActionWithDelay(() => UseAction(Actions.TwelvesBounty));
                else if (ShouldUseKingII(slot, config))
                    EnqueueActionWithDelay(() => UseAction(Actions.Yield2));
                else if (ShouldUseKingI(slot, config))
                    EnqueueActionWithDelay(() => UseAction(Actions.Yield1));
                else if (ShouldUseBountiful(slot, config))
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
                return anyCrystal != null && ShouldUseGivingLand(anyCrystal, MatchConfigPreset(anyCrystal.Item).GatherableActions);
            }

            return false;
        }

        private unsafe void UseAction(Actions.BaseAction act)
        {
            var amInstance = ActionManager.Instance();
            if (amInstance->GetActionStatus(ActionType.Action, act.ActionId) == 0)
            {
                //Communicator.Print("Action used: " + act.Name);
                amInstance->UseAction(ActionType.Action, act.ActionId);
            }
        }

        private void EnqueueActionWithDelay(Action action, bool immediate = false)
        {
            var delay = GatherBuddy.Config.AutoGatherConfig.ExecutionDelay;
            if (immediate)
                TaskManager.EnqueueImmediate(action);
            else
                TaskManager.Enqueue(action);

            if (delay > 0)
            {
                if (immediate)
                {
                    TaskManager.EnqueueImmediate(() => CanAct);
                    TaskManager.DelayNextImmediate((int)delay);
                }
                else
                {
                    TaskManager.Enqueue(() => CanAct);
                    TaskManager.DelayNext((int)delay);
                }
            }
        }

        private unsafe void DoCollectibles(ConfigPreset config, int itemsLeft)
        {
            if (MasterpieceAddon == null)
                return;

            CurrentRotation ??= new CollectableRotation(config);

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

        private bool ShouldUseSolidAgeGatherables(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.SolidAge, config.SolidAge, slot.Item, slot.Rare))
                return false;
            var yield = slot.Yield;
            if (Dalamud.ClientState.LocalPlayer!.StatusList.Any(s => s.StatusId == Actions.Bountiful.EffectId))
                yield -= 1;
            if (Dalamud.ClientState.LocalPlayer!.StatusList.Any(s => s.StatusId == Actions.BountifulII.EffectId))
                yield -= CalculateBountifulBonus(slot.Item);
            if (yield < config.SolidAge.MinYieldTotal)
                return false;

            return true;
        }

        private bool CheckConditions(Actions.BaseAction action, ConfigPreset.ActionConfig config, Gatherable item, bool rare)
        {
            if (config.Enabled == false)
                return false;
            if (Player.Level < action.MinLevel)
                return false;
            if (Player.Object.CurrentGp < action.GpCost)
                return false;
            if (Player.Object.CurrentGp < config.MinGP)
                return false;
            if (Player.Object.CurrentGp > config.MaxGP)
                return false;
            if (action.EffectId != 0 && Player.Status.Any(s => s.StatusId == action.EffectId))
                return false;
            if (action.QuestId != 0 && !QuestManager.IsQuestComplete(action.QuestId))
                return false; 
            if (action.EffectType is Actions.EffectType.CrystalsYield && !item.IsCrystal)
                return false;
            if (action.EffectType is Actions.EffectType.Integrity && NodeTracker.Integrity > Math.Min(2, NodeTracker.MaxIntegrity - 1))
                return false;
            if (action.EffectType is not Actions.EffectType.Other and not Actions.EffectType.GatherChance && rare)
                return false;
            if (config is ConfigPreset.ActionConfigIntegrity config2 && (config2.MinIntegrity > NodeTracker.MaxIntegrity || config2.FirstStepOnly && NodeTracker.Touched))
                return false;

            return true;
        }

        private static int CalculateBountifulBonus(Gatherable item)
        {
            if (!QuestManager.IsQuestComplete(Actions.BountifulII.QuestId))
                return 1;
            
            try
            {
                var glvl = item.GatheringData.GatheringItemLevel.RowId;
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
