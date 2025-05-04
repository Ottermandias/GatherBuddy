using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using System;
using System.Linq;
using ItemSlot = GatherBuddy.AutoGather.GatheringTracker.ItemSlot;
using GatherBuddy.CustomInfo;
using System.Collections.Generic;
using ECommons;
using ECommons.DalamudServices;
using GatherBuddy.AutoGather.Helpers;
using GatherBuddy.AutoGather.Extensions;
using GatherBuddy.AutoGather.Lists;
using GatherBuddy.Enums;
using GatherBuddy.FishTimer;
using GatherBuddy.SeFunctions;

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
            if (!CheckConditions(Actions.Bountiful, config.Bountiful, slot.Item, slot))
                return false;
            if (Player.Status.Any(s => s.StatusId == Actions.BountifulII.EffectId))
                return false;
            if (CalculateBountifulBonus(slot.Item) < config.Bountiful.MinYieldBonus)
                return false;

            return true;
        }

        public bool ShouldUseKingII(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.Yield2, config.Yield2, slot.Item, slot))
                return false;

            return true;
        }

        public bool ShouldUseKingI(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.Yield1, config.Yield1, slot.Item, slot))
                return false;

            return true;
        }

        private bool ShouldUseGivingLand(ItemSlot slot, ConfigPreset config)
        {
            if (!CheckConditions(Actions.GivingLand, config.GatherableActions.GivingLand, slot.Item, slot,
                    config.ChooseBestActionsAutomatically))
                return false;
            if (!IsGivingLandOffCooldown)
                return false;
            if (slot.Item.GetInventoryCount() > 9999 - GivingLandYield - slot.Yield)
                return false;

            return true;
        }

        private unsafe bool ShouldUseTwelvesBounty(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.TwelvesBounty, config.TwelvesBounty, slot.Item, slot))
                return false;
            if (slot.Item.GetInventoryCount() > 9999 - 3 - slot.Yield - (slot.RandomYield ? GivingLandYield : 0))
                return false;

            return true;
        }

        private unsafe bool ShouldUseGift1(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.Gift1, config.Gift1, slot.Item, slot))
                return false;

            return true;
        }

        private unsafe bool ShouldUseGift2(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.Gift2, config.Gift2, slot.Item, slot))
                return false;

            return true;
        }

        private unsafe bool ShouldUseTiding(ItemSlot slot, ConfigPreset.GatheringActionsRec config)
        {
            if (!CheckConditions(Actions.Tidings, config.Tidings, slot.Item, slot))
                return false;

            return true;
        }


        private unsafe void DoActionTasks(GatherTarget target)
        {
            if (MasterpieceAddon != null)
            {
                if (CurrentCollectableRotation == null)
                {
                    // Player clicked the item himself, or has just enabled auto-gather.
                    // We can't detect what item is being gathered from inside the GatheringMasterpiece addon, so we need to reopen it.
                    CloseGatheringAddons(false);
                    return;
                }

                DoCollectibles();
            }
            else
            {
                CurrentCollectableRotation = null;
                if (GatheringAddon != null && NodeTracker.Ready)
                {
                    DoGatherWindowActions(target);
                }
            }
        }

        public FishingState LastState = FishingState.None;

        private void DoFishingTasks(GatherTarget target)
        {
            var state = GatherBuddy.EventFramework.FishingState;

            switch (state)
            {
                case FishingState.Bite:      HandleBite(target); break;
                case FishingState.PoleReady: HandleReady(target); break;
                case FishingState.Waiting3:  HandleWaiting(target); break;
            }
        }

        private void HandleWaiting(GatherTarget target)
        {
            if (target.Fish?.Lure == Lure.Ambitious && !LureSuccess)
            {
                EnqueueActionWithDelay(() => UseAction(Actions.AmbitiousLure));
            }
            if (target.Fish?.Lure == Lure.Modest && !LureSuccess)
            {
                EnqueueActionWithDelay(() => UseAction(Actions.ModestLure));
            }
        }

        private void HandleReady(GatherTarget target)
        {
            LureSuccess = false;
            if (Player.Status.Any(s => s is { StatusId: 2778, Param: >= 3 }))
                EnqueueActionWithDelay(() => UseAction(Actions.ThaliaksFavor));
            if (target.Fish?.Snagging == Snagging.Required)
                EnqueueActionWithDelay(() => UseAction(Actions.Snagging));
            if (target.Fish?.HookSet is HookSet.Powerful or HookSet.Precise)
                EnqueueActionWithDelay(() => UseAction(Actions.Patience));
            EnqueueActionWithDelay(() => UseAction(Actions.Cast));
        }

        private void HandleBite(GatherTarget target)
        {
            if (GatherBuddy.TugType.Bite == target.Fish?.BiteType)
            {
                switch (target.Fish.HookSet)
                {
                    case HookSet.Powerful:
                        EnqueueActionWithDelay(() => UseAction(Actions.PowerfulHookset));
                        break;
                    case HookSet.Precise:
                        EnqueueActionWithDelay(() => UseAction(Actions.PrecisionHookset));
                        break;
                    default:
                        EnqueueActionWithDelay(() => UseAction(Actions.Hook));
                        break;
                }
            }
        }

        private unsafe void DoGatherWindowActions(GatherTarget target)
        {
            if (LuckUsed[1] && !LuckUsed[2] && NodeTracker.Revisit)
                LuckUsed = new(0);

            //Use The Giving Land out of order to gather random crystals.
            if (ShouldUseGivingLandOutOfOrder(target.Gatherable))
            {
                EnqueueActionWithDelay(() => UseAction(Actions.GivingLand));
                return;
            }

            if (!HasGivingLandBuff && ShouldUseLuck(target.Gatherable))
            {
                LuckUsed[1] = true;
                LuckUsed[2] = NodeTracker.Revisit;
                EnqueueActionWithDelay(() => UseAction(Actions.Luck));
                return;
            }

            var (useSkills, slot) = GetItemSlotToGather(target);
            if (useSkills)
            {
                var configPreset = MatchConfigPreset(slot.Item);
                var config       = configPreset.GatherableActions;

                if (configPreset.ChooseBestActionsAutomatically)
                {
                    if (ShouldUseWise(NodeTracker.Integrity, NodeTracker.MaxIntegrity))
                    {
                        ActionSequence = null; //Recalculate rotation since we've got unaccounted 6 GP and 1 integrity.
                        EnqueueActionWithDelay(() => UseAction(Actions.Wise));
                    }
                    else
                    {
                        if (ActionSequence == null)
                        {
                            var task = RotationSolver.SolveAsync(slot, configPreset);

                            if (task.Wait(1))
                            {
                                ActionSequence = task.Result.AsEnumerable().GetEnumerator();
                            }
                            else
                            {
                                TaskManager.Enqueue(() =>
                                {
                                    if (task.IsCompleted)
                                        ActionSequence = task.Result.GetEnumerator();
                                    return task.IsCompleted;
                                });
                                AutoStatus = "Calculating best action sequence...";
                                return;
                            }
                        }

                        if (!ActionSequence.MoveNext())
                        {
                            ActionSequence = null;
                        }
                        else
                        {
                            var action = ActionSequence.Current;
                            if (action != null)
                                EnqueueActionWithDelay(() => UseAction(action));
                            else
                                EnqueueGatherItem(slot);
                        }
                    }
                }
                else
                {
                    if (ShouldUseWise(NodeTracker.Integrity, NodeTracker.MaxIntegrity))
                        EnqueueActionWithDelay(() => UseAction(Actions.Wise));
                    else if (ShouldUseGift2(slot, config))
                        EnqueueActionWithDelay(() => UseAction(Actions.Gift2));
                    else if (ShouldUseGift1(slot, config))
                        EnqueueActionWithDelay(() => UseAction(Actions.Gift1));
                    else if (ShouldUseTiding(slot, config))
                        EnqueueActionWithDelay(() => UseAction(Actions.Tidings));
                    else if (ShouldUseSolidAgeGatherables(slot, config))
                        EnqueueActionWithDelay(() => UseAction(Actions.SolidAge));
                    else if (ShouldUseGivingLand(slot, configPreset))
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
            }
            else
            {
                EnqueueGatherItem(slot);
            }
        }

        private bool ShouldUseGivingLandOutOfOrder(Gatherable? desiredItem)
        {
            if (GatherBuddy.Config.AutoGatherConfig.UseGivingLandOnCooldown
             && desiredItem != null
             && desiredItem.NodeType == Enums.NodeType.Regular)
            {
                var anyCrystal = GetAnyCrystalInNode();
                return anyCrystal != null && ShouldUseGivingLand(anyCrystal, MatchConfigPreset(anyCrystal.Item));
            }

            return false;
        }

        private unsafe void UseAction(Actions.FishingAction act)
        {
            var amInstance = ActionManager.Instance();
            if (amInstance->GetActionStatus(ActionType.Action, act.ActionId) == 0)
            {
                //Communicator.Print("Action used: " + act.Name);
                amInstance->UseAction(ActionType.Action, act.ActionId);
            }
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

            //Always delay the next action by at least 1 tick (2 or 3 in fact in the current implementation).
            //There is a possibility that client state update is happening the same tick when CanAct becomes true, and GBR won't see it if executed before it is done.
            //Since GBR Update() may be called in the same tick after TaskManager gets CanAct == true (depending on call order in the Update event),
            //we must always add a delay, which adds 2 extra ticks.
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

        private unsafe void DoCollectibles()
        {
            if (MasterpieceAddon == null || CurrentCollectableRotation == null)
                return;

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

                var collectibleAction = CurrentCollectableRotation.GetNextAction(MasterpieceAddon);

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
            if (!CheckConditions(Actions.SolidAge, config.SolidAge, slot.Item, slot))
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

        private bool CheckConditions(Actions.BaseAction action, ConfigPreset.ActionConfig config, Gatherable item, ItemSlot slot,
            bool autoMode = false)
        {
            // autoMode = true is used for TGL out-of-order check that occurs before the rotation solver kicks in.
            if (config.Enabled == false && !autoMode)
                return false;
            if (Player.Level < action.MinLevel)
                return false;
            if (Player.Object.CurrentGp < action.GpCost)
                return false;
            if (Player.Object.CurrentGp < config.MinGP && !autoMode)
                return false;
            if (Player.Object.CurrentGp > config.MaxGP && !autoMode)
                return false;
            if (action.EffectId != 0 && Player.Status.Any(s => s.StatusId == action.EffectId))
                return false;
            if (action.QuestId != 0 && !QuestManager.IsQuestComplete(action.QuestId))
                return false;
            if (action.EffectType is Actions.EffectType.CrystalsYield && !item.IsCrystal)
                return false;
            if (action.EffectType is Actions.EffectType.Integrity && NodeTracker.Integrity > Math.Min(2, NodeTracker.MaxIntegrity - 1))
                return false;
            if (action.EffectType is not Actions.EffectType.Other and not Actions.EffectType.GatherChance && slot.Rare)
                return false;
            if (config is ConfigPreset.ActionConfigIntegrity config2
             && (!autoMode && config2.MinIntegrity > NodeTracker.MaxIntegrity || (config2.FirstStepOnly || autoMode) && NodeTracker.Touched))
                return false;
            if (config is ConfigPreset.ActionConfigBoon config3
             && (slot.BoonChance == -1 || !autoMode && (slot.BoonChance < config3.MinBoonChance || slot.BoonChance > config3.MaxBoonChance)))
                return false;
            if (action.EffectType is Actions.EffectType.BoonChance && slot.BoonChance == 100)
                return false;

            return true;
        }

        public static int CalculateBountifulBonus(Gatherable item)
        {
            if (!QuestManager.IsQuestComplete(Actions.BountifulII.QuestId))
                return 1;

            try
            {
                var glvl      = item.GatheringData.GatheringItemLevel.RowId;
                var baseValue = WorldData.IlvConvertTable[(int)glvl].BaseGathering;
                var stat      = DiscipleOfLand.Gathering;

                if (stat >= baseValue * 11 / 10)
                    return 3;
                if (stat >= baseValue * 9 / 10)
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

        private void QueueStartFishingTasks()
        {
            EnqueueActionWithDelay(() => UseAction(Actions.Cast));
        }

        private void QueueQuitFishingTasks()
        {
            EnqueueActionWithDelay(() => UseAction(Actions.Quit));
        }
    }
}
