using System;
using System.Collections.Generic;
using System.Linq;
using GatherBuddy.AutoGather;
using GatherBuddy.AutoHookIntegration.Models;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.FishTimer;
using GatherBuddy.Models;

namespace GatherBuddy.AutoHookIntegration;

public class AutoHookPresetBuilder
{
    private static HashSet<Fish> CollectAllFishInMoochChains(Fish[] fishList)
    {
        var allFish = new HashSet<Fish>();
        
        foreach (var fish in fishList)
        {
            allFish.Add(fish);
            
            // Add all fish in the mooch chain
            // fish.Mooches contains the full chain from initial bait to immediate predecessor
            // e.g., if Fish A needs Fish B to mooch, and Fish B needs Fish C:
            // Fish A.Mooches = [C, B]
            // Fish B.Mooches = [C]
            // Fish C.Mooches = []
            if (fish.Mooches.Length > 0)
            {
                foreach (var moochFish in fish.Mooches)
                {
                    allFish.Add(moochFish);
                }
            }
        }
        
        return allFish;
    }
    
    public static AHCustomPresetConfig BuildPresetFromFish(string presetName, IEnumerable<Fish> fishList, ConfigPreset? gbrPreset = null)
    {
        var preset = new AHCustomPresetConfig(presetName);
        var fishArray = fishList.ToArray();
        
        GatherBuddy.Log.Debug($"[AutoHook] Building preset with {fishArray.Length} input fish");
        foreach (var fish in fishArray)
        {
            GatherBuddy.Log.Debug($"[AutoHook]   - {fish.Name[GatherBuddy.Language]} (ID: {fish.ItemId}, Mooches: {fish.Mooches.Length}, InitialBait: {fish.InitialBait.Id})");
        }
        
        var allFishWithMooches = CollectAllFishInMoochChains(fishArray);
        GatherBuddy.Log.Debug($"[AutoHook] After collecting mooch chains: {allFishWithMooches.Count} total fish");
        
        // Separate fish that are caught with real bait vs those caught by mooching
        var fishWithBait = allFishWithMooches.Where(f => f.Mooches.Length == 0).ToList();
        var fishWithMooch = allFishWithMooches.Where(f => f.Mooches.Length > 0).ToList();
        
        GatherBuddy.Log.Debug($"[AutoHook] Fish with bait: {fishWithBait.Count}, Fish with mooch: {fishWithMooch.Count}");
        foreach (var fish in fishWithMooch)
        {
            var moochChain = string.Join(" -> ", fish.Mooches.Select(m => m.Name[GatherBuddy.Language]));
            GatherBuddy.Log.Debug($"[AutoHook]   Mooch: {fish.Name[GatherBuddy.Language]} needs: {moochChain}");
        }
        
        // Create HookConfigs for fish caught with bait
        var baitGroups = fishWithBait.GroupBy(f => f.InitialBait.Id);
        foreach (var group in baitGroups)
        {
            var baitId = group.Key;
            if (baitId == 0) continue;

            var hookConfig = new AHHookConfig((int)baitId);
            
            foreach (var fish in group)
            {
                ConfigureHookForFish(hookConfig, fish);
            }
            
            preset.ListOfBaits.Add(hookConfig);
        }
        
        // Create HookConfigs for fish caught by mooching (grouped by the mooch fish)
        var moochGroups = fishWithMooch.GroupBy(f => f.Mooches[^1].ItemId);
        foreach (var group in moochGroups)
        {
            var moochFishId = group.Key;
            var hookConfig = new AHHookConfig((int)moochFishId);
            
            foreach (var fish in group)
            {
                ConfigureHookForFish(hookConfig, fish);
            }
            
            preset.ListOfMooch.Add(hookConfig);
        }
        
        GatherBuddy.Log.Debug($"[AutoHook] Created {preset.ListOfBaits.Count} bait configs and {preset.ListOfMooch.Count} mooch configs");
        
        // Add all fish configs
        foreach (var fish in allFishWithMooches)
        {
            AddFishConfig(preset, fish);
        }
        
        GatherBuddy.Log.Debug($"[AutoHook] Added {preset.ListOfFish.Count} fish configs");

        ConfigureAutoCasts(preset, fishArray, gbrPreset);
        
        return preset;
    }

    public static AHCustomPresetConfig BuildPresetFromRecords(string presetName, IEnumerable<FishRecord> records)
    {
        var preset = new AHCustomPresetConfig(presetName);
        
        var baitGroups = records
            .Where(r => r.HasBait && r.HasCatch)
            .GroupBy(r => r.BaitId);
        
        foreach (var group in baitGroups)
        {
            var baitId = (int)group.Key;
            var hookConfig = new AHHookConfig(baitId);
            
            foreach (var record in group)
            {
                ConfigureHookFromRecord(hookConfig, record);
            }
            
            preset.ListOfBaits.Add(hookConfig);
        }
        
        return preset;
    }

    private static void ConfigureHookForFish(AHHookConfig hookConfig, Fish fish)
    {
        var ahBiteType = ConvertBiteType(fish.BiteType);
        var ahHookType = ConvertHookSet(fish.HookSet);
        
        if (ahBiteType == AHBiteType.Unknown || ahHookType == AHHookType.Unknown)
            return;

        ConfigureLures(hookConfig.NormalHook, fish.Lure);
        SetHookConfiguration(hookConfig.NormalHook, ahBiteType, ahHookType);

        if (fish.Predators.Length > 0)
        {
            hookConfig.IntuitionHook.UseCustomStatusHook = true;
            ConfigureLures(hookConfig.IntuitionHook, fish.Lure);
            SetHookConfiguration(hookConfig.IntuitionHook, ahBiteType, ahHookType);
        }
    }

    private static void ConfigureHookFromRecord(AHHookConfig hookConfig, FishRecord record)
    {
        var ahBiteType = ConvertBiteType(record.Tug);
        var ahHookType = ConvertHookSet(record.Hook);
        
        if (ahBiteType == AHBiteType.Unknown || ahHookType == AHHookType.Unknown)
            return;

        var biteTimeSeconds = record.Bite / 1000.0;
        var minTime = Math.Max(0, biteTimeSeconds - 1.0);
        var maxTime = biteTimeSeconds + 1.0;

        SetHookConfiguration(hookConfig.NormalHook, ahBiteType, ahHookType, minTime, maxTime);

        if (record.Flags.HasFlag(Effects.Intuition))
        {
            hookConfig.IntuitionHook.UseCustomStatusHook = true;
            SetHookConfiguration(hookConfig.IntuitionHook, ahBiteType, ahHookType, minTime, maxTime);
        }
    }

    private static void SetHookConfiguration(
        AHBaseHookset hookset, 
        AHBiteType biteType, 
        AHHookType hookType,
        double minTime = 0,
        double maxTime = 0)
    {
        var biteConfig = biteType switch
        {
            AHBiteType.Weak => hookset.PatienceWeak,
            AHBiteType.Strong => hookset.PatienceStrong,
            AHBiteType.Legendary => hookset.PatienceLegendary,
            _ => null
        };

        if (biteConfig == null) return;

        biteConfig.HooksetEnabled = true;
        biteConfig.HooksetType = hookType;

        if (minTime > 0 || maxTime > 0)
        {
            biteConfig.HookTimerEnabled = true;
            biteConfig.MinHookTimer = minTime;
            biteConfig.MaxHookTimer = maxTime;
        }
    }

    private static AHBiteType ConvertBiteType(BiteType gbBiteType)
    {
        return gbBiteType switch
        {
            BiteType.Weak => AHBiteType.Weak,
            BiteType.Strong => AHBiteType.Strong,
            BiteType.Legendary => AHBiteType.Legendary,
            BiteType.None => AHBiteType.None,
            _ => AHBiteType.Unknown
        };
    }

    private static AHHookType ConvertHookSet(HookSet gbHookSet)
    {
        return gbHookSet switch
        {
            HookSet.Hook => AHHookType.Normal,
            HookSet.Precise => AHHookType.Precision,
            HookSet.Powerful => AHHookType.Powerful,
            HookSet.DoubleHook => AHHookType.Double,
            HookSet.TripleHook => AHHookType.Triple,
            HookSet.Stellar => AHHookType.Stellar,
            HookSet.None => AHHookType.None,
            _ => AHHookType.Unknown
        };
    }

    private static void ConfigureLures(AHBaseHookset hookset, Lure lure)
    {
        if (lure == Lure.None)
            return;

        hookset.CastLures = new AHLuresConfig
        {
            Enabled = true,
            AmbitiousLureEnabled = lure == Lure.Ambitious,
            ModestLureEnabled = lure == Lure.Modest
        };
    }

    private static void AddFishConfig(AHCustomPresetConfig preset, Fish fish)
    {
        // The mooch ID should be the immediate predecessor fish in the chain
        // which is the last element in the Mooches array
        var mooch = new AHAutoMooch();
        if (fish.Mooches.Length > 0)
        {
            mooch = new AHAutoMooch(fish.Mooches[^1].ItemId);
        }
        
        var surfaceSlap = new AHAutoSurfaceSlap(fish.SurfaceSlap != null);
        
        var fishConfig = new AHFishConfig((int)fish.ItemId)
        {
            Enabled = true,
            SurfaceSlap = surfaceSlap,
            Mooch = mooch,
            NeverMooch = false
        };

        preset.ListOfFish.Add(fishConfig);
    }

    private static void ConfigureAutoCasts(AHCustomPresetConfig preset, Fish[] fishList, ConfigPreset? gbrPreset)
    {
        var needsPatience = fishList.Any(f => f.ItemData.Rarity > 0 || f.IsBigFish);
        var needsCollect = fishList.Any(f => f.ItemData.Rarity > 0);
        var useCordials = gbrPreset?.Consumables.Cordial.Enabled ?? false;
        
        var hasSurfaceSlap = fishList.Any(f => f.SurfaceSlap != null);
        var hasMooches = fishList.Any(f => f.Mooches.Length > 0);
        var needsPrizeCatch = hasSurfaceSlap || hasMooches;

        preset.AutoCastsCfg = new AHAutoCastsConfig
        {
            EnableAll = true,
            DontCancelMooch = true,
            CastMooch = hasMooches ? new AHAutoMoochCast
            {
                Enabled = true
            } : null,
            CastPatience = needsPatience ? new AHAutoPatience
            {
                Enabled = true,
                PatienceVersion = 2
            } : null,
            CastCollect = needsCollect ? new AHAutoCollect
            {
                Enabled = true
            } : null,
            CastCordial = useCordials ? new AHAutoCordial
            {
                Enabled = true
            } : null,
            CastPrizeCatch = needsPrizeCatch ? new AHAutoPrizeCatch
            {
                Enabled = true,
                UseWhenMoochIIOnCD = false,
                UseOnlyWithIdenticalCast = false,
                UseOnlyWithActiveSlap = hasSurfaceSlap
            } : null,
            CastThaliaksFavor = !needsPrizeCatch ? new AHAutoThaliaksFavor
            {
                Enabled = true,
                ThaliaksFavorStacks = 3,
                ThaliaksFavorRecover = 150,
                UseWhenCordialCD = useCordials
            } : null
        };
    }
}
