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

            public Actions.BaseAction? GetNextAction(AddonGatheringMasterpiece* MasterpieceAddon)
            {
                int collectability   = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(6)->NodeText.ToString());
                int currentIntegrity = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(126)->NodeText.ToString());
                int maxIntegrity     = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(129)->NodeText.ToString());
                int scourColl        = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(84)->NodeText.ToString().Substring(2));
                int meticulousColl   = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(108)->NodeText.ToString().Substring(2));
                int brazenColl       = int.Parse(MasterpieceAddon->AtkUnitBase.GetTextNodeById(93)->NodeText.ToString().Substring(2));

                if (currentIntegrity < maxIntegrity 
                 && ShouldUseWise(currentIntegrity, maxIntegrity))
                    return Actions.Wise;

                if (collectability >= GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore)
                {
                    if (currentIntegrity <= maxIntegrity && (shouldUseFullRotation || GatherBuddy.Config.AutoGatherConfig.AlwaysUseSolidAgeCollectables)
                     && ShouldSolidAgeCollectables(currentIntegrity, maxIntegrity))
                        return Actions.SolidAge;

                    if (ShouldCollect())
                        return Actions.Collect;
                }

                if (currentIntegrity == 1
                 && GatherBuddy.Config.AutoGatherConfig.GatherIfLastIntegrity
                 && collectability >= GatherBuddy.Config.AutoGatherConfig.GatherIfLastIntegrityMinimumCollectibility
                 && ShouldCollect())
                    return Actions.Collect;

                if (shouldUseFullRotation && NeedScrutiny(collectability, scourColl, meticulousColl, brazenColl) && ShouldUseScrutiny())
                    return Actions.Scrutiny;

                if (meticulousColl + collectability >= GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore
                 && ShouldUseMeticulous())
                    return Actions.Meticulous;

                if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 3911) && ShouldUseBrazen())
                    return Actions.Brazen;

                if (scourColl + collectability >= GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore
                 && ShouldUseScour())
                    return Actions.Scour;

                if (ShouldUseMeticulous())
                    return Actions.Meticulous;

                return null;
            }

            private bool NeedScrutiny(int collectability, int scourColl, int meticulousColl, int brazenColl)
            {
                uint collAim = GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore;
                if (scourColl + collectability >= collAim)
                    return false;
                if (meticulousColl + collectability >= collAim)
                    return false;
                if (Dalamud.ClientState.LocalPlayer.StatusList.Any(s => s.StatusId == 3911)
                 && brazenColl + collectability >= collAim)
                    return false;

                return true;
            }

            private bool ShouldCollect()
            {
                return GatherBuddy.Config.AutoGatherConfig.CollectConfig.UseAction;
            }
        }
    }
}
