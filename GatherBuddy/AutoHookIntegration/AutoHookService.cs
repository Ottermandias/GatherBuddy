using System;
using System.Collections.Generic;
using ECommons.DalamudServices;
using GatherBuddy.AutoGather;
using GatherBuddy.AutoHookIntegration.Models;
using GatherBuddy.Classes;
using GatherBuddy.FishTimer;
using GatherBuddy.Plugin;

namespace GatherBuddy.AutoHookIntegration;

public class AutoHookService
{
    public static bool IsAutoHookAvailable()
    {
        return AutoHook.Enabled;
    }

    public static bool ExportPresetToAutoHook(string presetName, IEnumerable<Fish> fishList, ConfigPreset? gbrPreset = null)
    {
        if (!IsAutoHookAvailable())
        {
            Svc.Log.Error("[AutoHook Integration] AutoHook plugin is not available");
            return false;
        }

        try
        {
            var preset = AutoHookPresetBuilder.BuildPresetFromFish(presetName, fishList, gbrPreset);
            Svc.Log.Debug($"[AutoHook Integration] Preset built, starting export...");
            var exportString = AutoHookExporter.ExportPreset(preset);
            Svc.Log.Debug($"[AutoHook Integration] Export string created, length: {exportString?.Length ?? 0}");
            
            AutoHook.ImportAndSelectPreset?.Invoke(exportString);
            Svc.Log.Debug($"[AutoHook Integration] IPC call completed");
            
            Svc.Log.Information($"[AutoHook Integration] Successfully exported preset '{presetName}' to AutoHook");
            return true;
        }
        catch (Exception ex)
        {
            Svc.Log.Error($"[AutoHook Integration] Failed to export preset: {ex.Message}");
            if (ex.InnerException != null)
                Svc.Log.Error($"[AutoHook Integration] Inner exception: {ex.InnerException.Message}");
            Svc.Log.Error($"[AutoHook Integration] Stack trace: {ex.StackTrace}");
            return false;
        }
    }

    public static bool ExportPresetFromRecords(string presetName, IEnumerable<FishRecord> records)
    {
        if (!IsAutoHookAvailable())
        {
            Svc.Log.Error("[AutoHook Integration] AutoHook plugin is not available");
            return false;
        }

        try
        {
            var preset = AutoHookPresetBuilder.BuildPresetFromRecords(presetName, records);
            var exportString = AutoHookExporter.ExportPreset(preset);
            
            AutoHook.ImportAndSelectPreset?.Invoke(exportString);
            
            Svc.Log.Information($"[AutoHook Integration] Successfully exported preset '{presetName}' to AutoHook from records");
            return true;
        }
        catch (Exception ex)
        {
            Svc.Log.Error($"[AutoHook Integration] Failed to export preset from records: {ex.Message}");
            return false;
        }
    }

    public static string ExportPresetToClipboard(string presetName, IEnumerable<Fish> fishList, ConfigPreset? gbrPreset = null)
    {
        try
        {
            var preset = AutoHookPresetBuilder.BuildPresetFromFish(presetName, fishList, gbrPreset);
            var exportString = AutoHookExporter.ExportPreset(preset);
            
            Svc.Log.Information($"[AutoHook Integration] Preset '{presetName}' exported to string");
            return exportString;
        }
        catch (Exception ex)
        {
            Svc.Log.Error($"[AutoHook Integration] Failed to create preset string: {ex.Message}");
            return string.Empty;
        }
    }

    public static string ExportPresetFromRecordsToClipboard(string presetName, IEnumerable<FishRecord> records)
    {
        try
        {
            var preset = AutoHookPresetBuilder.BuildPresetFromRecords(presetName, records);
            var exportString = AutoHookExporter.ExportPreset(preset);
            
            Svc.Log.Information($"[AutoHook Integration] Preset '{presetName}' exported from records to string");
            return exportString;
        }
        catch (Exception ex)
        {
            Svc.Log.Error($"[AutoHook Integration] Failed to create preset string from records: {ex.Message}");
            return string.Empty;
        }
    }
}
