using System;
using System.Linq;
using Dalamud.Logging;
using GatherBuddy.Enums;
using GatherBuddy.Time;

namespace GatherBuddy.Data;

public static partial class Fish
{
    private static Classes.Fish? Apply(this GameData data, uint id, Patch patch)
    {
        if (data.Fishes.TryGetValue(id, out var fish))
        {
            fish.Patch = patch;
            return fish;
        }

        PluginLog.Error($"Could not find fish {id}.");
        return null;
    }

    private static Classes.Fish? Transition(this Classes.Fish? fish, GameData data, params uint[] previousWeathers)
    {
        if (fish == null)
            return null;

        try
        {
            fish!.PreviousWeather = previousWeathers.Select(w => data.Weathers.TryGetValue(w, out var weather)
                    ? weather
                    : throw new Exception($"Could not find weather {w}."))
                .ToArray();
        }
        catch (Exception e)
        {
            PluginLog.Error(e.Message);
        }

        return fish;
    }

    private static Classes.Fish? Weather(this Classes.Fish? fish, GameData data, params uint[] weathers)
    {
        if (fish == null)
            return null;

        try
        {
            fish!.CurrentWeather = weathers.Select(w => data.Weathers.TryGetValue(w, out var weather)
                    ? weather
                    : throw new Exception($"Could not find weather {w}."))
                .ToArray();
        }
        catch (Exception e)
        {
            PluginLog.Error(e.Message);
        }

        return fish;
    }

    private static Classes.Fish? Bait(this Classes.Fish? fish, GameData data, params uint[] items)
    {
        if (fish == null)
            return null;

        if (items.Length == 0)
            return fish;

        if (fish.IsSpearFish)
        {
            PluginLog.Error("Tried to set bait for spearfish.");
            return fish;
        }

        try
        {
            fish.InitialBait = data.Bait.TryGetValue(items[0], out var bait) ? bait : throw new Exception($"Could not find bait {items[0]}.");
            fish.Mooches = items.Skip(1).Select(f
                    => data.Fishes.TryGetValue(f, out var fsh)
                        ? fsh
                        : throw new Exception($"Could not find fish {f}."))
                .ToArray();
        }
        catch (Exception e)
        {
            PluginLog.Error(e.Message);
        }

        return fish;
    }

    private static Classes.Fish? Predators(this Classes.Fish? fish, GameData data, int intuitionLength, params (uint, int)[] predators)
    {
        if (fish == null)
            return null;

        if (predators.Length == 0 || intuitionLength < 0)
            return fish;

        try
        {
            fish.IntuitionLength = intuitionLength;
            fish.Predators = predators.Where(p => p.Item2 > 0)
                .Select(p => data.Fishes.TryGetValue(p.Item1, out var fsh)
                    ? (fsh, p.Item2)
                    : throw new Exception($"Could not find fish {p.Item1}."))
                .ToArray();
        }
        catch (Exception e)
        {
            PluginLog.Error(e.Message);
        }

        return fish;
    }

    private static Classes.Fish? Time(this Classes.Fish? fish, int uptimeMinuteOfDayStart, int uptimeMinuteOfDayEnd)
    {
        if (fish == null)
            return null;

        fish.Interval = RepeatingInterval.FromEorzeanMinutes(uptimeMinuteOfDayStart, uptimeMinuteOfDayEnd);
        return fish;
    }

    private static Classes.Fish? Snag(this Classes.Fish? fish, Snagging snagging)
    {
        if (fish == null)
            return null;

        if (fish.IsSpearFish)
        {
            PluginLog.Error("Tried to set snagging for spearfish.");
            return fish;
        }

        fish.Snagging = snagging;
        return fish;
    }

    private static Classes.Fish? Bite(this Classes.Fish? fish, HookSet hookSet, BiteType biteType = BiteType.Unknown)
    {
        if (fish == null)
            return null;

        if (fish.IsSpearFish)
        {
            PluginLog.Error("Tried to set bite for spearfish.");
            return fish;
        }

        fish.HookSet  = hookSet == HookSet.Unknown ? fish.HookSet : hookSet;
        fish.BiteType = biteType == BiteType.Unknown ? fish.BiteType : biteType;
        return fish;
    }

    private static Classes.Fish? Spear(this Classes.Fish? fish, SpearfishSize size, SpearfishSpeed speed = SpearfishSpeed.Unknown)
    {
        if (fish == null)
            return null;

        if (!fish.IsSpearFish)
        {
            PluginLog.Error("Tried to set spearfish data for regular fish.");
            return fish;
        }

        fish.Size  = size == SpearfishSize.Unknown ? fish.Size : size;
        fish.Speed = speed == SpearfishSpeed.Unknown ? fish.Speed : speed;
        return fish;
    }


    internal static void Apply(GameData data)
    {
        data.ApplyARealmReborn();
        data.ApplyARealmAwoken();
        data.ApplyThroughTheMaelstrom();
        data.ApplyDefendersOfEorzea();
        data.ApplyDreamsOfIce();
        data.ApplyBeforeTheFall();
        data.ApplyHeavensward();
        data.ApplyAsGoesLightSoGoesDarkness();
        data.ApplyRevengeOfTheHorde();
        data.ApplySoulSurrender();
        data.ApplyTheFarEdgeOfFate();
        data.ApplyStormblood();
        data.ApplyTheLegendReturns();
        data.ApplyRiseOfANewSun();
        data.ApplyUnderTheMoonlight();
        data.ApplyPreludeInViolet();
        data.ApplyARequiemForHeroes();
        data.ApplyShadowbringers();
        data.ApplyVowsOfVirtueDeedsOfCruelty();
        data.ApplyEchoesOfAFallenStar();
        data.ApplyReflectionsInCrystal();
        data.ApplyFuturesRewritten();
        data.ApplyDeathUntoDawn();
        data.ApplyEndwalker();
    }
}
