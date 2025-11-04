using System;
using System.Linq;
using ECommons.DalamudServices;
using GatherBuddy.AutoGather.Lists;
using GatherBuddy.AutoHookIntegration;
using GatherBuddy.Plugin;

namespace GatherBuddy.AutoGather;

public partial class AutoGather
{
    private GatherTarget? _currentAutoHookTarget;
    private string? _currentAutoHookPresetName;

    private void CleanupAutoHookIfNeeded(GatherTarget newTarget)
    {
        if (!GatherBuddy.Config.AutoGatherConfig.UseAutoHook)
            return;

        if (_currentAutoHookTarget == null || _currentAutoHookPresetName == null)
            return;

        if (_currentAutoHookTarget.Value.FishingSpot?.Id != newTarget.FishingSpot?.Id)
        {
            CleanupAutoHook();
        }
    }

    private void SetupAutoHookForFishing(GatherTarget target)
    {
        if (!GatherBuddy.Config.AutoGatherConfig.UseAutoHook)
            return;

        if (!AutoHook.Enabled)
        {
            Svc.Log.Debug("[AutoGather] AutoHook not available, skipping preset generation");
            return;
        }

        if (target.Fish == null)
            return;

        CleanupAutoHookIfNeeded(target);

        if (_currentAutoHookTarget?.Fish?.ItemId == target.Fish.ItemId 
            && _currentAutoHookPresetName != null)
        {
            return;
        }

        try
        {
            var fishingSpot = target.FishingSpot;
            var fishList = fishingSpot.Items
                .Where(f => f is Classes.Fish)
                .Cast<Classes.Fish>()
                .ToList();

            if (!fishList.Any())
            {
                Svc.Log.Warning($"[AutoGather] No fish found at {fishingSpot.Name}, skipping AutoHook preset");
                return;
            }

            var presetName = $"GBR_{fishingSpot.Name.Replace(" ", "")}_{DateTime.Now:HHmmss}";
            
            Svc.Log.Information($"[AutoGather] Creating AutoHook preset '{presetName}' with {fishList.Count} fish");
            
            var gbrPreset = MatchConfigPreset(target.Fish);
            var success = AutoHookService.ExportPresetToAutoHook(presetName, fishList, gbrPreset);
            
            if (success)
            {
                _currentAutoHookTarget = target;
                _currentAutoHookPresetName = presetName;
                
                AutoHook.SetPreset?.Invoke(presetName);
                AutoHook.SetPluginState?.Invoke(true);
                
                Svc.Log.Information($"[AutoGather] AutoHook preset '{presetName}' selected and activated successfully");
            }
            else
            {
                Svc.Log.Error($"[AutoGather] Failed to create AutoHook preset");
            }
        }
        catch (Exception ex)
        {
            Svc.Log.Error($"[AutoGather] Exception setting up AutoHook: {ex.Message}");
        }
    }

    private void CleanupAutoHook()
    {
        if (!AutoHook.Enabled)
            return;

        try
        {
            if (_currentAutoHookPresetName != null)
            {
                AutoHook.DeleteSelectedPreset?.Invoke();
                Svc.Log.Debug($"[AutoGather] Deleted AutoHook preset '{_currentAutoHookPresetName}'");
            }
            
            AutoHook.SetPluginState?.Invoke(false);
            Svc.Log.Debug("[AutoGather] AutoHook disabled");
            
            _currentAutoHookTarget = null;
            _currentAutoHookPresetName = null;
        }
        catch (Exception ex)
        {
            Svc.Log.Error($"[AutoGather] Exception cleaning up AutoHook: {ex.Message}");
        }
    }
}
