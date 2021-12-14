using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Enums;

namespace GatherBuddy.Data
{
    public static partial class FishData
    {
        private static CatchData Apply(this FishManager f, uint id, Patch patch)
        {
            var data = new CatchData(patch);
            if (f.Fish.TryGetValue(id, out var fish))
                fish.CatchData = data;
            return data;
        }

        public static void Apply(this FishManager fish)
        {
            fish.ApplyARealmReborn();
            fish.ApplyARealmAwoken();
            fish.ApplyThroughTheMaelstrom();
            fish.ApplyDefendersOfEorzea();
            fish.ApplyDreamsOfIce();
            fish.ApplyBeforeTheFall();
            fish.ApplyHeavensward();
            fish.ApplyAsGoesLightSoGoesDarkness();
            fish.ApplyRevengeOfTheHorde();
            fish.ApplySoulSurrender();
            fish.ApplyTheFarEdgeOfFate();
            fish.ApplyStormblood();
            fish.ApplyTheLegendReturns();
            fish.ApplyRiseOfANewSun();
            fish.ApplyUnderTheMoonlight();
            fish.ApplyPreludeInViolet();
            fish.ApplyARequiemForHeroes();
            fish.ApplyShadowbringers();
            fish.ApplyVowsOfVirtueDeedsOfCruelty();
            fish.ApplyEchoesOfAFallenStar();
            fish.ApplyReflectionsInCrystal();
            fish.ApplyFuturesRewritten();
            fish.ApplyDeathUntoDawn();
            fish.ApplyEndwalker();
        }
    }
}
