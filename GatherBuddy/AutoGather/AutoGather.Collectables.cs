using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.UI;
using GatherBuddy.AutoGather.Extensions;
using GatherBuddy.AutoGather.AtkReaders;
using GatherBuddy.Classes;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using ECommons.DalamudServices;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        private CollectableRotation? CurrentCollectableRotation;

        private unsafe partial class CollectableRotation
        {
            public CollectableRotation(ConfigPreset config, Gatherable item, uint quantity)
            {
                this.config = config;
                shouldUseFullRotation = Player.Object.CurrentGp >= config.CollectableActionsMinGP;
                this.item = item;
                this.quantity = quantity;
            }

            private readonly bool shouldUseFullRotation = false;
            private readonly ConfigPreset config;
            private readonly Gatherable item;
            private readonly uint quantity;

            [GeneratedRegex(@"\d+")]
            private static partial Regex NumberRegex();

            public Actions.BaseAction GetNextAction(GatheringMasterpieceReader masterpieceReader)
            {
                var itemsLeft = (int)(quantity - item.GetInventoryCount());

                if (itemsLeft <= 0 && GatherBuddy.Config.AutoGatherConfig.AbandonNodes)
                    throw new NoGatherableItemsInNodeException();

                var regex = NumberRegex();
                int collectability   = masterpieceReader.CollectabilityCurrent;
                int currentIntegrity = masterpieceReader.IntegrityCurrent;
                int maxIntegrity     = masterpieceReader.IntegrityMax;
                int scourColl        = masterpieceReader.ScourGain;
                int meticulousColl   = masterpieceReader.MeticulousGain;
                int brazenColl       = masterpieceReader.BrazenGainMax;

                if (ShouldUseWise(currentIntegrity, maxIntegrity))
                    return Actions.Wise;

                var (targetScore, minScore) = GetCollectabilityScores(masterpieceReader);

                if (collectability >= targetScore)
                {
                    if ((shouldUseFullRotation || config.CollectableAlwaysUseSolidAge)
                     && ShouldSolidAgeCollectables(currentIntegrity, maxIntegrity, itemsLeft))
                        return Actions.SolidAge;
                    else
                        return Actions.Collect;
                }

                if (currentIntegrity == 1
                 && collectability >= minScore)
                    return Actions.Collect;

                if (shouldUseFullRotation && NeedScrutiny(collectability, scourColl, meticulousColl, brazenColl, targetScore) && ShouldUseScrutiny())
                    return Actions.Scrutiny;

                if (meticulousColl + collectability >= targetScore
                 && ShouldUseMeticulous())
                    return Actions.Meticulous;

                if (Player.Status.Any(s => s.StatusId == 3911 /*Collector's High Standard*/) && ShouldUseBrazen())
                    return Actions.Brazen;

                if (scourColl + collectability >= targetScore
                 && ShouldUseScour())
                    return Actions.Scour;

                if (ShouldUseMeticulous())
                    return Actions.Meticulous;

                //Fallback path if some actions are disabled.
                if (Player.Status.Any(s => s.StatusId == 2418 /*Collector's Standard*/) && ShouldUseBrazen())
                    return Actions.Brazen;
                if (ShouldUseScour())
                    return Actions.Scour;
                if (ShouldUseBrazen())
                    return Actions.Brazen;

                throw new NoCollectableActionsException();
            }

            private (int targetScore, int minScore) GetCollectabilityScores(GatheringMasterpieceReader masterpieceReader)
            {
                if (config.CollectableManualScores)
                    return (config.CollectableTagetScore, config.CollectableMinScore);

                int targetScore, minScore;

                // Check reward tiers in descending order and use the first visible one for target score
                if (masterpieceReader.HighThreshold > 0)
                    targetScore = masterpieceReader.HighThreshold;
                else if (masterpieceReader.MidThreshold > 0)
                    targetScore = masterpieceReader.MidThreshold;
                else
                    targetScore = masterpieceReader.LowThreshold;

                // For minScore, pick the lowest non-zero threshold
                int[] thresholds = { masterpieceReader.LowThreshold, masterpieceReader.MidThreshold, masterpieceReader.HighThreshold };
                minScore = thresholds.Where(t => t > 0).DefaultIfEmpty(1).Min();

                // For custom deliveries and quest items, we always want max collectability
                if (item.GatheringData.Unknown3 is 3 or 4 or 6)
                    minScore = targetScore;

                Svc.Log.Verbose($"Using target collectability {targetScore} and minimum collectability {minScore} for {item.Name}.");
                return (targetScore, minScore);
            }

            private bool NeedScrutiny(int collectability, int scourColl, int meticulousColl, int brazenColl, int targetScore)
            {
                if (scourColl + collectability >= targetScore && ShouldUseScour())
                    return false;
                if (meticulousColl + collectability >= targetScore && ShouldUseMeticulous())
                    return false;
                if (brazenColl + collectability >= targetScore && ShouldUseBrazen())
                    return false;

                return true;
            }
            private bool ShouldUseMeticulous()
            {
                if (Player.Level < Actions.Meticulous.MinLevel)
                    return false;
                if (Player.Object.CurrentGp < Actions.Meticulous.GpCost)
                    return false;
                if (config.ChooseBestActionsAutomatically)
                    return true;
                if (Player.Object.CurrentGp < config.CollectableActions.Meticulous.MinGP
                 || Player.Object.CurrentGp > config.CollectableActions.Meticulous.MaxGP)
                    return false;

                return config.CollectableActions.Meticulous.Enabled;
            }

            private bool ShouldUseScour()
            {
                if (Player.Level < Actions.Brazen.MinLevel)
                    return false;
                if (Player.Object.CurrentGp < Actions.Brazen.GpCost)
                    return false;
                if (config.ChooseBestActionsAutomatically)
                    return true;
                if (Player.Object.CurrentGp < config.CollectableActions.Scour.MinGP
                 || Player.Object.CurrentGp > config.CollectableActions.Scour.MaxGP)
                    return false;

                return config.CollectableActions.Scour.Enabled;
            }

            private bool ShouldUseBrazen()
            {
                if (Player.Level < Actions.Meticulous.MinLevel)
                    return false;
                if (Player.Object.CurrentGp < Actions.Meticulous.GpCost)
                    return false;
                if (config.ChooseBestActionsAutomatically)
                    return true;
                if (Player.Object.CurrentGp < config.CollectableActions.Brazen.MinGP
                 || Player.Object.CurrentGp > config.CollectableActions.Brazen.MaxGP)
                    return false;

                return config.CollectableActions.Brazen.Enabled;
            }

            private bool ShouldUseScrutiny()
            {
                if (Player.Level < Actions.Scrutiny.MinLevel)
                    return false;
                if (Player.Object.CurrentGp < Actions.Scrutiny.GpCost)
                    return false;
                if (Player.Status.Any(s => s.StatusId == Actions.Scrutiny.EffectId))
                    return false;
                if (config.ChooseBestActionsAutomatically)
                    return true;
                if (Player.Object.CurrentGp < config.CollectableActions.Scrutiny.MinGP
                 || Player.Object.CurrentGp > config.CollectableActions.Scrutiny.MaxGP)
                    return false;

                return config.CollectableActions.Scrutiny.Enabled;
            }

            private bool ShouldSolidAgeCollectables(int integrity, int maxIntegrity, int itemsLeft)
            {
                if (integrity > Math.Min(2, maxIntegrity - 1))
                    return false;
                if (itemsLeft <= integrity)
                    return false;
                if (Player.Level < Actions.SolidAge.MinLevel)
                    return false;
                if (Player.Object.CurrentGp < Actions.SolidAge.GpCost)
                    return false;
                if (Player.Status.Any(s => s.StatusId == Actions.SolidAge.EffectId))
                    return false;
                if (config.ChooseBestActionsAutomatically)
                    return true;
                if (Player.Object.CurrentGp < config.CollectableActions.SolidAge.MinGP
                 || Player.Object.CurrentGp > config.CollectableActions.SolidAge.MaxGP)
                    return false;

                return config.CollectableActions.SolidAge.Enabled;
            }
        }
    }
}
