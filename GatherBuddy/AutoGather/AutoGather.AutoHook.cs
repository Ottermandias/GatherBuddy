using System;
using System.IO;
using System.Linq;
using ECommons.DalamudServices;
using GatherBuddy.AutoGather.Lists;
using GatherBuddy.AutoHookIntegration;
using GatherBuddy.Plugin;
using Newtonsoft.Json.Linq;

namespace GatherBuddy.AutoGather;

public partial class AutoGather
{
    private GatherTarget? _currentAutoHookTarget;
    private string? _currentAutoHookPresetName;
    private bool _isCurrentPresetUserOwned;

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
            var fishName = target.Fish.Name[GatherBuddy.Language];
            var fishId = target.Fish.ItemId;
            string? presetName = null;
            bool isUserPreset = false;

            if (GatherBuddy.Config.AutoGatherConfig.UseExistingAutoHookPresets)
            {
                var userPresetName = FindAutoHookPreset(fishId.ToString());
                if (userPresetName != null)
                {
                    presetName = userPresetName;
                    isUserPreset = true;

                    Svc.Log.Information($"[AutoGather] Found user preset '{presetName}' for {fishName}");
                    AutoHook.SetPreset?.Invoke(presetName);
                }
                else
                {
                    Svc.Log.Debug($"[AutoGather] No user preset found for fish ID {fishId}, will generate one");
                }
            }

            if (presetName == null)
            {
                var fishList = new[] { target.Fish };
                presetName = $"GBR_{fishName.Replace(" ", "")}_{DateTime.Now:HHmmss}";
                
                Svc.Log.Information($"[AutoGather] Creating AutoHook preset '{presetName}' for {fishName}");
                
                var gbrPreset = MatchConfigPreset(target.Fish);
                var success = AutoHookService.ExportPresetToAutoHook(presetName, fishList, gbrPreset);
                
                if (!success)
                {
                    Svc.Log.Error($"[AutoGather] Failed to create AutoHook preset");
                    return;
                }
                
                AutoHook.SetPreset?.Invoke(presetName);
            }

            _currentAutoHookTarget = target;
            _currentAutoHookPresetName = presetName;
            _isCurrentPresetUserOwned = isUserPreset;
            
            AutoHook.SetPluginState?.Invoke(true);
            
            var presetType = isUserPreset ? "user" : "generated";
            Svc.Log.Information($"[AutoGather] AutoHook preset '{presetName}' ({presetType}) for {fishName} selected and activated successfully");
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
                if (_isCurrentPresetUserOwned)
                {
                    Svc.Log.Debug($"[AutoGather] Preserving user-owned preset '{_currentAutoHookPresetName}'");
                }
                else
                {
                    AutoHook.SetPreset?.Invoke(_currentAutoHookPresetName);
                    AutoHook.DeleteSelectedPreset?.Invoke();
                    Svc.Log.Debug($"[AutoGather] Deleted GBR-generated preset '{_currentAutoHookPresetName}'");
                }
            }
            
            AutoHook.SetPluginState?.Invoke(false);
            Svc.Log.Debug("[AutoGather] AutoHook disabled");
            
            _currentAutoHookTarget = null;
            _currentAutoHookPresetName = null;
            _isCurrentPresetUserOwned = false;
        }
        catch (Exception ex)
        {
            Svc.Log.Error($"[AutoGather] Exception cleaning up AutoHook: {ex.Message}");
        }
    }

    private string? FindAutoHookPreset(string fishId)
    {
        try
        {
            // Resolve AutoHook config path: .../pluginConfigs/AutoHook.json
            var pluginConfigsDir = Svc.PluginInterface.ConfigDirectory.Parent?.FullName;
            if (string.IsNullOrEmpty(pluginConfigsDir))
                return null;

            var configPath = Path.Combine(pluginConfigsDir, "AutoHook.json");
            if (!File.Exists(configPath))
            {
                Svc.Log.Debug($"[AutoGather] AutoHook config not found at {configPath}");
                return null;
            }

            var json = File.ReadAllText(configPath);
            var config = JObject.Parse(json);

            var customPresets = config["HookPresets"]?["CustomPresets"] as JArray;
            if (customPresets == null)
            {
                Svc.Log.Debug("[AutoGather] No CustomPresets found in AutoHook config");
                return null;
            }

            foreach (var preset in customPresets)
            {
                var presetName = preset?["PresetName"]?.ToString();
                if (presetName != null && presetName.Equals(fishId, StringComparison.Ordinal))
                {
                    Svc.Log.Debug($"[AutoGather] Found matching preset in config: {presetName}");
                    return presetName;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            Svc.Log.Error($"[AutoGather] Error reading AutoHook config: {ex.Message}");
            return null;
        }
    }
}
