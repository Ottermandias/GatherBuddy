using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.UI;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        private CollectableRotation? CurrentRotation;

        private unsafe partial class CollectableRotation
        {
            public CollectableRotation(ConfigPreset config)
            {
                this.config = config;
                shouldUseFullRotation = Player.Object.CurrentGp >= config.CollectableActionsMinGP;
            }

            private bool shouldUseFullRotation = false;
            private readonly ConfigPreset config;

            [GeneratedRegex(@"\d+")]
            private static partial Regex NumberRegex();

            public Actions.BaseAction GetNextAction(AddonGatheringMasterpiece* MasterpieceAddon, int itemsLeft)
            {
                var regex = NumberRegex();
                int collectability   = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(6)->NodeText.ToString());
                int currentIntegrity = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(126)->NodeText.ToString());
                int maxIntegrity     = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(129)->NodeText.ToString());
                int scourColl        = int.Parse(regex.Match(MasterpieceAddon->AtkUnitBase.GetTextNodeById(84)->NodeText.ToString()).Value);
                int meticulousColl   = int.Parse(regex.Match(MasterpieceAddon->AtkUnitBase.GetTextNodeById(108)->NodeText.ToString()).Value);
                int brazenColl       = int.Parse(regex.Match(MasterpieceAddon->AtkUnitBase.GetTextNodeById(93)->NodeText.ToString()).Value);

                if (ShouldUseWise(currentIntegrity, maxIntegrity))
                    return Actions.Wise;

                if (collectability >= config.CollectableTagetScore)
                {
                    if ((shouldUseFullRotation || config.CollectableAlwaysUseSolidAge)
                     && ShouldSolidAgeCollectables(currentIntegrity, maxIntegrity, itemsLeft))
                        return Actions.SolidAge;
                    else
                        return Actions.Collect;
                }

                if (currentIntegrity == 1
                 && collectability >= config.CollectableMinScore)
                    return Actions.Collect;

                if (shouldUseFullRotation && NeedScrutiny(collectability, scourColl, meticulousColl, brazenColl) && ShouldUseScrutiny())
                    return Actions.Scrutiny;

                if (meticulousColl + collectability >= config.CollectableTagetScore
                 && ShouldUseMeticulous())
                    return Actions.Meticulous;

                if (Player.Status.Any(s => s.StatusId == 3911 /*Collector's High Standard*/) && ShouldUseBrazen())
                    return Actions.Brazen;

                if (scourColl + collectability >= config.CollectableTagetScore
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

            private bool NeedScrutiny(int collectability, int scourColl, int meticulousColl, int brazenColl)
            {
                var collAim = config.CollectableTagetScore;
                if (scourColl + collectability >= collAim && ShouldUseScour())
                    return false;
                if (meticulousColl + collectability >= collAim && ShouldUseMeticulous())
                    return false;
                if (brazenColl + collectability >= collAim && ShouldUseBrazen())
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
