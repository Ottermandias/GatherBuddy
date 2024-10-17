using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.UI;
using System.Linq;
using GatherBuddy.Plugin;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        private static CollectableRotation? CurrentRotation;

        private unsafe class CollectableRotation
        {
            public CollectableRotation(uint GPToStart)
            {
                shouldUseFullRotation = Player.Object.CurrentGp >= GPToStart;
            }

            private bool shouldUseFullRotation = false;

            public Actions.BaseAction GetNextAction(AddonGatheringMasterpiece* MasterpieceAddon, int itemsLeft)
            {
                int collectability   = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(6)->NodeText.ToString());
                int currentIntegrity = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(126)->NodeText.ToString());
                int maxIntegrity     = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(129)->NodeText.ToString());
                int scourColl        = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(84)->NodeText.ToString().Substring(2));
                int meticulousColl   = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(108)->NodeText.ToString().Substring(2));
                int brazenColl       = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(93)->NodeText.ToString().Substring(2));

                if (ShouldUseWise(currentIntegrity, maxIntegrity))
                    return Actions.Wise;

                if (collectability >= GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore)
                {
                    if ((shouldUseFullRotation || GatherBuddy.Config.AutoGatherConfig.AlwaysUseSolidAgeCollectables)
                     && ShouldSolidAgeCollectables(currentIntegrity, maxIntegrity, itemsLeft))
                        return Actions.SolidAge;
                    else
                        return Actions.Collect;
                }

                if (currentIntegrity == 1
                 && GatherBuddy.Config.AutoGatherConfig.GatherIfLastIntegrity
                 && collectability >= GatherBuddy.Config.AutoGatherConfig.GatherIfLastIntegrityMinimumCollectibility)
                    return Actions.Collect;

                if (shouldUseFullRotation && NeedScrutiny(collectability, scourColl, meticulousColl, brazenColl) && ShouldUseScrutiny())
                    return Actions.Scrutiny;

                if (meticulousColl + collectability >= GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore
                 && ShouldUseMeticulous())
                    return Actions.Meticulous;

                if (Player.Status.Any(s => s.StatusId == 3911 /*Collector's High Standard*/) && ShouldUseBrazen())
                    return Actions.Brazen;

                if (scourColl + collectability >= GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore
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

                throw new NoColectableActionsExceptions();
            }

            private bool NeedScrutiny(int collectability, int scourColl, int meticulousColl, int brazenColl)
            {
                uint collAim = GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore;
                if (scourColl + collectability >= collAim && ShouldUseScour())
                    return false;
                if (meticulousColl + collectability >= collAim && ShouldUseMeticulous())
                    return false;
                if (brazenColl + collectability >= collAim && ShouldUseBrazen())
                    return false;

                return true;
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
                if (!Player.Status.Any(s => s.StatusId == Actions.Scrutiny.EffectId))
                    return GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.UseAction;

                return false;
            }

            private static bool ShouldSolidAgeCollectables(int integrity, int maxIntegrity, int itemsLeft)
            {
                if (integrity == maxIntegrity)
                    return false;
                if (itemsLeft <= integrity)
                    return false;
                if (Player.Level < Actions.SolidAge.MinLevel)
                    return false;
                if (Player.Object.CurrentGp < Actions.SolidAge.GpCost)
                    return false;
                if (Player.Object.CurrentGp < GatherBuddy.Config.AutoGatherConfig.SolidAgeCollectablesConfig.MinimumGP
                 || Player.Object.CurrentGp > GatherBuddy.Config.AutoGatherConfig.SolidAgeCollectablesConfig.MaximumGP)
                    return false;
                if (Player.Status.Any(s => s.StatusId == Actions.SolidAge.EffectId))
                    return false;

                return GatherBuddy.Config.AutoGatherConfig.SolidAgeCollectablesConfig.UseAction;
            }
        }
    }
}
