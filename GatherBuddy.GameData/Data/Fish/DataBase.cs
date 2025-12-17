using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GatherBuddy.Enums;
using GatherBuddy.Structs;
using GatherBuddy.Time;
using Newtonsoft.Json;
using OtterGui.Extensions;

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
                fish.Mooches =
                [
                    fsh,
                ];
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
                fish.Mooches =
                [
                    fsh,
                ];
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

        fish.FishType = value
            ? fish.FishType is FishType.Legendary ? FishType.Legendary : FishType.Big
            : FishType.Normal;
        return fish;
    }

    private static Classes.Fish? ForceLegendary(this Classes.Fish? fish)
    {
        if (fish == null)
            return null;

        fish.FishType = FishType.Legendary;
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

    private static Classes.Fish? MultiHook(this Classes.Fish? fish, byte value)
    {
        if (fish == null)
            return null;

        fish.MultiHookLower = value;
        fish.MultiHookUpper = value;
        return fish;
    }

    private static Classes.Fish? MultiHook(this Classes.Fish? fish, byte valueLower, byte valueUpper)
    {
        if (fish == null)
            return null;

        fish.MultiHookLower = valueLower;
        fish.MultiHookUpper = valueUpper;
        return fish;
    }

    private static Classes.Fish? Points(this Classes.Fish? fish, short value)
    {
        if (fish == null)
            return null;

        fish.Points = value;
        return fish;
    }

    private static Classes.Fish? Mission(this Classes.Fish? fish, GameData data, ushort value)
    {
        if (fish == null)
            return null;

        fish.CosmicMission = data.CosmicFishingMissions.TryGetValue(value, out var mission)
            ? mission
            : throw new Exception($"Could not find cosmic fishing mission {value}.");
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
        foreach (var fish in data.Fishes.Values.Where(f => !f.IsSpearFish))
            ApplyMooch(fish);
        return;

        (Bait, Classes.Fish[]) ApplyMooch(Classes.Fish fish)
        {
            if (fish.InitialBait != Structs.Bait.Unknown)
                return (fish.InitialBait, fish.Mooches);

            if (fish.Mooches.Length == 0)
                return (Structs.Bait.Unknown, []);

            var (b, m) = ApplyMooch(fish.Mooches[^1]);
            if (b == Structs.Bait.Unknown)
                return (Structs.Bait.Unknown, []);

            fish.InitialBait = b;
            fish.Mooches     = m.Append(fish.Mooches[^1]).ToArray();
            return (fish.InitialBait, fish.Mooches);
        }
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
        data.ApplyThePromiseOfTomorrow();
        data.ApplyIntoTheMist();
        data.ApplyMooches();
        data.ApplyOverrides();
        //DumpUnknown(Patch.IntoTheMist, data.Fishes.Values);
    }

    public static bool ApplyOverrides(this GameData data)
    {
        try
        {
            if (!File.Exists(data.OverrideFile))
                return false;

            var text = File.ReadAllText(data.OverrideFile);
            var list = JsonConvert.DeserializeObject<List<FishOverrideData>>(text);
            if (list is null)
                return false;

            var count = 0;
            foreach (var (custom, index) in list.WithIndex())
            {
                try
                {
                    if (!data.Fishes.TryGetValue(custom.ItemId, out var fish))
                    {
                        data.Log.Warning($"Could not identify fish with ItemID {custom.ItemId} at index {index}.");
                        continue;
                    }

                    fish.HasOverridenData = true;

                    if (custom.BaitId is { } bait)
                        fish.Bait(data, bait);

                    if (custom.MoochId is { } mooch)
                        fish.Mooch(data, mooch);

                    if (custom.HookSet is not HookSet.Unknown)
                        fish.Bite(data, custom.HookSet, custom.BiteType is not BiteType.Unknown ? custom.BiteType : fish.BiteType);
                    else if (custom.BiteType is not BiteType.Unknown)
                        fish.Bite(data, fish.HookSet, custom.BiteType);

                    if (custom.Lure is { } lure)
                        fish.Lure(lure);

                    if (custom.IntuitionLength is { } intuition)
                        fish.Predators(data, intuition, custom.Predators ?? fish.Predators.Select(p => (p.Item1.ItemId, p.Item2)).ToArray());
                    else if (custom.Predators is { } predators)
                        fish.Predators(data, fish.IntuitionLength, predators);

                    switch (custom.MultiHookLower, custom.MultiHookUpper)
                    {
                        case (null, null): break;
                        case (null, _) v:
                            fish.MultiHook(
                                fish.MultiHookLower > 0 && fish.MultiHookLower <= v.MultiHookUpper!.Value
                                    ? fish.MultiHookLower
                                    : v.MultiHookUpper!.Value, v.MultiHookUpper!.Value);
                            break;
                        case (_, null) v:
                            fish.MultiHook(v.MultiHookLower!.Value,
                                fish.MultiHookUpper >= v.MultiHookLower!.Value ? fish.MultiHookUpper : v.MultiHookLower!.Value);
                            break;
                    }

                    if (custom.Points is { } points)
                        fish.Points(points);

                    if (custom.Snagging is { } snagging)
                        fish.Snag(data, snagging ? Snagging.Required : Snagging.None);

                    if (custom is { UptimeMinuteOfDayStart: { } start, UptimeMinuteOfDayEnd: { } end })
                        fish.Time(start, end);

                    if (custom.Transition is { } transition)
                        fish.Transition(data, transition);

                    if (custom.Weather is { } weather)
                        fish.Transition(data, weather);
                    ++count;
                }
                catch (Exception e)
                {
                    data.Log.Warning($"Error applying fish override data at index {index}:\n{e}");
                }
            }

            data.Log.Information($"Successfully applied {count} fish data overrides.");
            return count > 0;
        }
        catch (Exception ex)
        {
            data.Log.Error($"Error reading fish override file:\n{ex}");
            return false;
        }
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
