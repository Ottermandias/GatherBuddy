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
            AutoHook.SetPluginState?.Invoke(true);
            Svc.Log.Debug($"[AutoGather] Re-enabled existing AutoHook preset '{_currentAutoHookPresetName}'");
            return;
        }

        try
        {
            var fishingSpot = target.FishingSpot;
            
            // Only create preset for the target fish (and its mooch chain)
            var fishList = new[] { target.Fish };

            var presetName = $"GBR_{target.Fish.Name[GatherBuddy.Language].Replace(" ", "")}_{DateTime.Now:HHmmss}";
            
            Svc.Log.Information($"[AutoGather] Creating AutoHook preset '{presetName}' for {target.Fish.Name[GatherBuddy.Language]}");
            
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
                AutoHook.SetPreset?.Invoke(_currentAutoHookPresetName);
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
