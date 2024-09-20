using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using System;
using System.Linq;
using Dalamud.Game.ClientState.Conditions;
using ECommons;
using OtterGui;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        public bool ShouldUseLuck(Gatherable? gatherable)
        {
            if (gatherable == null)
                return false;
            if (HiddenRevealed)
                return false;
            if (Player.Level < Actions.Luck.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Luck.GpCost)
                return false;
            if (!gatherable.GatheringData.IsHidden && !IsTreasureMap(gatherable))
                return false;
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
            if (Player.Object.CurrentGp < Actions.Bountiful.GpCost)
                return false;
            if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => BountifulYieldStatuses.Contains(s.StatusId)))
                return false;
            if ((Dalamud.ClientState.LocalPlayer?.CurrentGp ?? 0) < GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MinimumGP)
                return false;
            if ((Dalamud.ClientState.LocalPlayer?.CurrentGp ?? 0) > GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MaximumGP)
                return false;
            if (gatherable != null && IsCrystal(gatherable) && !GatherBuddy.Config.AutoGatherConfig.BYIIConfig.GetOptionalProperty<bool>("UseWithCystals"))
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.BYIIConfig, gatherable))
                return false;

            return GatherBuddy.Config.AutoGatherConfig.BYIIConfig.UseAction;
        }

        public uint[] KingsYieldStatuses = new uint[]
        {
            219, //KYI and KYII
        };

        public uint[] BountifulYieldStatuses = new uint[]
        {
            756,  //BYI?
            1286, //BYII
        };

        public bool ShouldUseKingII(Gatherable gatherable)
        {
            if (Player.Level < Actions.Yield2.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Yield2.GpCost)
                return false;
            if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => KingsYieldStatuses.Contains(s.StatusId)))
                return false;

            if (Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.MaximumGP
             || Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.MinimumGP)
                return false;
            if (gatherable != null && IsCrystal(gatherable) && !GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.GetOptionalProperty<bool>("UseWithCystals"))
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.YieldIIConfig, gatherable))
                return false;

            return GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.UseAction;
        }

        public bool ShouldUseKingI(Gatherable gatherable)
        {
            if (Player.Level < Actions.Yield1.MinLevel)
                return false;
            if (Player.Object.CurrentGp < Actions.Yield1.GpCost)
                return false;
            if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => KingsYieldStatuses.Contains(s.StatusId)))
                return false;

            if (Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.YieldIConfig.MaximumGP
             || Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.YieldIConfig.MinimumGP)
                return false;
            if (gatherable != null && IsCrystal(gatherable) && !GatherBuddy.Config.AutoGatherConfig.YieldIConfig.GetOptionalProperty<bool>("UseWithCystals"))
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.YieldIConfig, gatherable))
                return false;

            return GatherBuddy.Config.AutoGatherConfig.YieldIConfig.UseAction;
        }

        private bool ShouldUseGivingLand(Gatherable? item, uint[] ids)
        {
            if (item == null)
                return false;
            if (!IsCrystal(item))
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
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.GivingLandConfig, item))
                return false;
            if (InventoryCount(item) > 9999 - GivingLandYeild - GetCurrentYield(ids.IndexOf(item.ItemId)))
                return false;

            return true;
        }

        private unsafe bool ShouldUseTwelvesBounty(Gatherable? item, uint[] ids)
        {
            if (item == null)
                return false;
            if (!IsCrystal(item))
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
            if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 825))
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig, item))
                return false;
            if (InventoryCount(item) > 9999 - 3 - GetCurrentYield(ids.IndexOf(item.ItemId)))
                return false;

            return true;
        }


        private unsafe void DoActionTasks(Gatherable? desiredItem)
        {
            if (GatheringAddon == null && MasterpieceAddon == null
             || Svc.Condition[ConditionFlag.Gathering42])
                return;

            if (MasterpieceAddon != null)
            {
                DoCollectibles();
            }
            else if (GatheringAddon != null && !(desiredItem?.ItemData.IsCollectable ?? false))
            {
                DoGatherWindowActions(desiredItem);
            }
            else if (GatheringAddon != null && (desiredItem?.ItemData.IsCollectable ?? false))
            {
                DoGatherWindowTasks(desiredItem);
            }

            if (MasterpieceAddon == null)
                CurrentRotation = null;
        }

        private unsafe void DoGatherWindowActions(Gatherable? desiredItem)
        {
            if (GatheringAddon != null)
            {
                var node9  = GatheringAddon->AtkUnitBase.GetTextNodeById(9);
                var node12 = GatheringAddon->AtkUnitBase.GetTextNodeById(12);

                if (node9 != null && node12 != null)
                {
                    if (int.TryParse(node9->NodeText.ToString(),  out int currentIntegrity)
                     && int.TryParse(node12->NodeText.ToString(), out int maxIntegrity))
                    {
                        var ids = GetGatherableIds();

                        if (LastIntegrity <= 1 && currentIntegrity == maxIntegrity)
                            HiddenRevealed = false; //Revisit. Doesn't work with quick gathering
                        
                        LastIntegrity = currentIntegrity;

                        if (!HiddenRevealed && currentIntegrity == maxIntegrity 
                            && ids.Select(GatherBuddy.GameData.Gatherables.GetValueOrDefault).Any(item => item != null && (item.GatheringData.IsHidden || IsTreasureMap(item))))
                        {
                            //Don't use Luck if hidden item is already revealed
                            HiddenRevealed = true;
                        }

                        if (!HasGivingLandBuff && ShouldUseLuck(desiredItem))
                        {
                            var anyCrystall = ids.Select(GatherBuddy.GameData.Gatherables.GetValueOrDefault).Where(i => i != null && IsCrystal(i)).OrderBy(i => GetInventoryItemCount(i!.ItemId));
                            if (!GatherBuddy.Config.AutoGatherConfig.UseGivingLandOnCooldown || !ShouldUseGivingLand(anyCrystall.FirstOrDefault(), ids))
                            {
                                HiddenRevealed = true;
                                EnqueueGatherAction(() => UseAction(Actions.Luck));
                                return;
                            }
                        }
                        if (MaybeGatherSometingElse(ref desiredItem!, ids))
                        {

                            if (ShouldUseWise(currentIntegrity, maxIntegrity))
                                EnqueueGatherAction(() => UseAction(Actions.Wise));
                            if (ShouldUseSolidAgeGatherables(currentIntegrity, maxIntegrity, desiredItem, ids))
                                EnqueueGatherAction(() => UseAction(Actions.SolidAge));
                            if (ShouldUseGivingLand(desiredItem, ids))
                                EnqueueGatherAction(() => UseAction(Actions.GivingLand));
                            else if (ShouldUseTwelvesBounty(desiredItem, ids))
                                EnqueueGatherAction(() => UseAction(Actions.TwelvesBounty));
                            else if (ShouldUseKingII(desiredItem))
                                EnqueueGatherAction(() => UseAction(Actions.Yield2));
                            else if (ShouldUseKingI(desiredItem))
                                EnqueueGatherAction(() => UseAction(Actions.Yield1));
                            else if (ShoulduseBYII(desiredItem))
                                EnqueueGatherAction(() => UseAction(Actions.Bountiful));
                            else
                                DoGatherWindowTasks(desiredItem);
                        }
                        else
                        {
                            DoGatherWindowTasks(desiredItem);
                        }
                    }
                }
            }
        }

        private unsafe int GetCurrentYield(int itemPosition, bool accountForGivingLand = true)
        {
            var itemCheckbox = GatheringAddon->GatheredItemComponentCheckbox[itemPosition].Value;

            var icon      = itemCheckbox->UldManager.SearchNodeById(31)->GetAsAtkComponentNode();
            var itemYield = icon->Component->UldManager.SearchNodeById(7)->GetAsAtkTextNode();


            var yield = itemYield->NodeText.ExtractText();
            if (!int.TryParse(yield, out int result))
            {
                result = 1;
                if (HasGivingLandBuff)
                {
                    var match = Regex.Match(yield, @"^(\d)+\+\?$");
                    if (match.Success)
                        result = int.Parse(match.Groups[1].Value) + (accountForGivingLand ? GivingLandYeild : 0);
                }
            }

            if (Dalamud.ClientState.LocalPlayer!.StatusList.Any(s
                    => BountifulYieldStatuses
                        .Contains(s.StatusId))) // Has BYII. This is a quality check as Solid will take priority over BYII anyway.
                result -= 3;                    //I consider the maximum proc, I don't know if we have a way to make a better check.

            return result;
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

            if (CurrentRotation == null)
                CurrentRotation = new CollectableRotation(GatherBuddy.Config.AutoGatherConfig.MinimumGPForCollectableRotation);

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

            if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 2765) && integrity < maxIntegrity)
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
            if (!Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 757))
                return GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.UseAction;

            return false;
        }

        private static bool ShouldSolidAgeCollectables(int integrity, int maxIntegrity)
            => ShouldUseSolidAge(GatherBuddy.Config.AutoGatherConfig.SolidAgeCollectablesConfig, integrity, maxIntegrity);

        private bool ShouldUseSolidAgeGatherables(int integrity, int maxIntegrity, Gatherable gatherable, uint[] ids)
        {
            var targetItemId = GetIndexOfItemToClick(ids, gatherable);

            if (GetCurrentYield(targetItemId)
              < GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.GetOptionalProperty<int>("MinimumYield"))
                return false;
            if (gatherable != null && IsCrystal(gatherable) && !GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.GetOptionalProperty<bool>("UseWithCystals"))
                return false;
            if (!CheckConditions(GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig, gatherable))
                return false;

            if (!ShouldUseSolidAge(GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig, integrity, maxIntegrity))
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
            if (!(Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 2765))
             && integrity < maxIntegrity)
                return SolidAgeConfig.UseAction;

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

            if (config.Conditions.RequiredIntegrity > maxIntegrity)
                return false;

            if (config.Conditions.UseOnlyOnFirstStep && currentIntegrity != maxIntegrity)
                return false;

            if (config.Conditions.FilterNodeTypes)
            {
                var node = config.Conditions.NodeFilter.GetNodeConfig(gatherable.NodeType);

                if (!node.Use || (gatherable.Level < node.NodeLevel && !(node.AvoidCap && IsGpMax)))
                    return false;
            }

            return true;
        }

        private static bool IsGpMax
            => Dalamud.ClientState.LocalPlayer?.CurrentGp == Dalamud.ClientState.LocalPlayer?.MaxGp;

        private const int ActionCooldown = 2000;
    }
}
