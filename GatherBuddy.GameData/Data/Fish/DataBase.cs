using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GatherBuddy.Enums;
using GatherBuddy.Structs;
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

        data.Log.Error($"Could not find fish {id}.");
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
            fish.FishRestrictions |= FishRestrictions.Weather;
        }
        catch (Exception e)
        {
            data.Log.Error(e.Message);
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
            fish.FishRestrictions |= FishRestrictions.Weather;
        }
        catch (Exception e)
        {
            data.Log.Error(e.Message);
        }

        return fish;
    }

    private static Classes.Fish? Bait(this Classes.Fish? fish, GameData data, uint? baitId = null)
    {
        if (fish == null)
            return null;

        if (baitId == null)
            return fish;

        if (fish.IsSpearFish)
        {
            data.Log.Error("Tried to set bait for spearfish.");
            return fish;
        }

        try
        {
            if (data.Bait.TryGetValue(baitId.Value, out var bait))
                fish.InitialBait = bait;
            else if (data.Fishes.TryGetValue(baitId.Value, out var fsh))
                fish.Mooches = new[]
                {
                    fsh,
                };
            else
                throw new Exception($"Could not find bait {baitId.Value}.");
        }
        catch (Exception e)
        {
            data.Log.Error(e.Message);
        }

        return fish;
    }

    private static Classes.Fish? Lure(this Classes.Fish? fish, Lure lure)
    {
        if (fish == null)
            return null;

        fish.Lure = lure;
        return fish;
    }

    private static Classes.Fish? Mooch(this Classes.Fish? fish, GameData data, uint moochId)
    {
        if (fish == null)
            return null;

        if (fish.IsSpearFish)
        {
            data.Log.Error("Tried to set bait for spearfish.");
            return fish;
        }

        try
        {
            if (data.Fishes.TryGetValue(moochId, out var fsh))
                fish.Mooches = new[]
                {
                    fsh,
                };
            else
                throw new Exception($"Could not find fish {moochId}.");
        }
        catch (Exception e)
        {
            data.Log.Error(e.Message);
        }

        return fish;
    }

    private static Classes.Fish? Mooch(this Classes.Fish? fish, GameData data, uint baitId, uint mooch1, params uint[] items)
    {
        if (fish == null)
            return null;

        if (fish.IsSpearFish)
        {
            data.Log.Error("Tried to set bait for spearfish.");
            return fish;
        }

        try
        {
            fish.InitialBait = data.Bait.TryGetValue(baitId, out var bait) ? bait : throw new Exception($"Could not find bait {baitId}.");
            fish.Mooches = items.Prepend(mooch1).Select(f => data.Fishes.TryGetValue(f, out var fsh)
                    ? fsh
                    : throw new Exception($"Could not find fish {f}."))
                .ToArray();
        }
        catch (Exception e)
        {
            data.Log.Error(e.Message);
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
            data.Log.Error(e.Message);
        }

        return fish;
    }

    private static Classes.Fish? Time(this Classes.Fish? fish, int uptimeMinuteOfDayStart, int uptimeMinuteOfDayEnd)
    {
        if (fish == null)
            return null;

        fish.Interval         =  RepeatingInterval.FromEorzeanMinutes(uptimeMinuteOfDayStart, uptimeMinuteOfDayEnd);
        fish.FishRestrictions |= FishRestrictions.Time;
        return fish;
    }

    private static Classes.Fish? Snag(this Classes.Fish? fish, GameData data, Snagging snagging)
    {
        if (fish == null)
            return null;

        if (fish.IsSpearFish)
        {
            data.Log.Error("Tried to set snagging for spearfish.");
            return fish;
        }

        fish.Snagging = snagging;
        return fish;
    }

    private static Classes.Fish? Bite(this Classes.Fish? fish, GameData data, HookSet hookSet, BiteType biteType = BiteType.Unknown)
    {
        if (fish == null)
            return null;

        if (fish.IsSpearFish)
        {
            data.Log.Error("Tried to set bite for spearfish.");
            return fish;
        }

        fish.HookSet  = hookSet == HookSet.Unknown ? fish.HookSet : hookSet;
        fish.BiteType = biteType == BiteType.Unknown ? fish.BiteType : biteType;
        return fish;
    }

    private static Classes.Fish? Spear(this Classes.Fish? fish, GameData data, SpearfishSize size,
        SpearfishSpeed speed = SpearfishSpeed.Unknown)
    {
        if (fish == null)
            return null;

        if (!fish.IsSpearFish)
        {
            data.Log.Error("Tried to set spearfish data for regular fish.");
            return fish;
        }

        fish.Size  = size == SpearfishSize.Unknown ? fish.Size : size;
        fish.Speed = speed == SpearfishSpeed.Unknown ? fish.Speed : speed;
        return fish;
    }

    private static Classes.Fish? ForceBig(this Classes.Fish? fish, bool value)
    {
        if (fish == null)
            return null;

        fish.BigFishOverride = value;
        return fish;
    }

    private static Classes.Fish? Slap(this Classes.Fish? fish, GameData data, uint fishId)
    {
        if (fish == null)
            return null;

        fish.SurfaceSlap = data.Fishes.TryGetValue(fishId, out var fsh)
            ? fsh
            : throw new Exception($"Could not find fish {fishId}.");
        return fish;
    }

    private static Classes.Fish? Comment(this Classes.Fish? fish, string value)
    {
        if (fish == null)
            return null;

        fish.Guide = value;
        return fish;
    }

    private static Classes.Fish? Ocean(this Classes.Fish? fish, params OceanTime[] times)
    {
        if (fish == null)
            return null;

        fish.OceanTime = times.Aggregate(OceanTime.Never, (a, b) => a | b);
        if (fish.OceanTime != OceanTime.Always)
            fish.FishRestrictions |= FishRestrictions.Time;
        return fish;
    }

    private static void ApplyMooches(this GameData data)
    {
        (Bait, Classes.Fish[]) ApplyMooch(Classes.Fish fish)
        {
            if (fish.InitialBait != Structs.Bait.Unknown)
                return (fish.InitialBait, fish.Mooches);

            if (fish.Mooches.Length == 0)
                return (Structs.Bait.Unknown, Array.Empty<Classes.Fish>());

            var (b, m) = ApplyMooch(fish.Mooches[^1]);
            if (b == Structs.Bait.Unknown)
                return (Structs.Bait.Unknown, Array.Empty<Classes.Fish>());

            fish.InitialBait = b;
            fish.Mooches     = m.Append(fish.Mooches[^1]).ToArray();
            return (fish.InitialBait, fish.Mooches);
        }

        foreach (var fish in data.Fishes.Values.Where(f => !f.IsSpearFish))
            ApplyMooch(fish);
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
        data.ApplyNewfoundAdventure();
        data.ApplyBuriedMemory();
        data.ApplyGodsRevelLandsTremble();
        data.ApplyTheDarkThrone();
        data.ApplyGrowingLight();
        data.ApplyDawntrail();
        data.ApplyCrossroads();
        data.ApplySeekersOfEternity();
        data.ApplyMooches();
        //DumpUnknown(Patch.SeekersOfEternity, data.Fishes.Values);
    }

    public static void DumpUnknown(Patch patch, IEnumerable<Classes.Fish> fish, [CallerFilePath] string? directory = null)
    {
        try
        {
            var       path   = Path.Combine(Path.GetDirectoryName(directory) ?? string.Empty, $"Data{patch.ToMajor()}.{patch.ToMinor()}.cs");
            using var stream = File.Open(path, FileMode.Create);
            using var w      = new StreamWriter(stream);
            w.WriteLine("using GatherBuddy.Enums;");
            w.WriteLine(string.Empty);
            w.WriteLine("namespace GatherBuddy.Data;");
            w.WriteLine(string.Empty);
            w.WriteLine("public static partial class Fish");
            w.WriteLine("{");
            w.WriteLine("    // @formatter:off");
            w.WriteLine($"    private static void Apply{patch}(this GameData data)");
            w.WriteLine("    {");
            foreach (var f in fish.Where(f => f.Patch is Patch.Unknown && f.InLog).OrderBy(f => f.IsSpearFish).ThenBy(f => f.ItemId))
            {
                w.WriteLine($"        data.Apply({f.ItemId}, Patch.{patch}) // {f.Name.English}");
                w.WriteLine("            .Bait(data)");
                w.WriteLine("            .Bite(data, HookSet.Unknown, BiteType.Unknown);");
            }

            w.WriteLine("    }");
            w.WriteLine("    // @formatter:on");
            w.WriteLine("}");
        }
        catch
        {
            // ignored
        }
    }
}
