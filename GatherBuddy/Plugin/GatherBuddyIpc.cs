using System;
using System.IO;
using Dalamud.Plugin.Ipc;

namespace GatherBuddy.Plugin;

public class GatherBuddyIpc : IDisposable
{
    public const int    IpcVersion   = 2;
    public const string VersionName  = $"{GatherBuddy.InternalName}.Version";
    public const string IdentifyName = $"{GatherBuddy.InternalName}.Identify";
    public const string ExportName   = $"{GatherBuddy.InternalName}.Export";

    private readonly  GatherBuddy  _plugin;
    internal readonly ICallGateProvider<int>?                  _versionProvider;
    internal readonly ICallGateProvider<string, uint>?         _identifyProvider;
    internal readonly ICallGateProvider<string, string, bool>? _exportProvider;

    public GatherBuddyIpc(GatherBuddy plugin)
    {
        _plugin = plugin;

        try
        {
            _versionProvider = Dalamud.PluginInterface.GetIpcProvider<int>(VersionName);
            _versionProvider.RegisterFunc(Version);
        }
        catch (Exception e)
        {
            _versionProvider = null;
            GatherBuddy.Log.Error($"Could not obtain provider for {VersionName}:\n{e}");
        }

        try
        {
            _identifyProvider = Dalamud.PluginInterface.GetIpcProvider<string, uint>(IdentifyName);
            _identifyProvider.RegisterFunc(Identify);
        }
        catch (Exception e)
        {
            _identifyProvider = null;
            GatherBuddy.Log.Error($"Could not obtain provider for {IdentifyName}:\n{e}");
        }

        try
        {
            _exportProvider = Dalamud.PluginInterface.GetIpcProvider<string, string, bool>(ExportName);
            _exportProvider.RegisterFunc(Export);
        }
        catch (Exception e)
        {
            _exportProvider = null;
            GatherBuddy.Log.Error($"Could not obtain provider for {ExportName}:\n{e}");
        }

    }

    private static int Version()
        => IpcVersion;

    private uint Identify(string text)
        => _plugin.Executor.Identificator.IdentifyGatherable(text)?.ItemId
         ?? _plugin.Executor.Identificator.IdentifyFish(text)?.ItemId ?? 0;

    private bool Export(string path, string format)
    {
        if (format.ToLower() == "json")
        {
            try
            {
                GatherBuddy.Log.Information($"Exporting FishRecorder :\n{path}");
                var file = new FileInfo(path);
                _plugin.FishRecorder.ExportJson(file);
                return true;
            }
            catch
            {
                return false;
            }
        }

        return false;
    }

    public void Dispose()
    {
        _identifyProvider?.UnregisterFunc();
        _versionProvider?.UnregisterFunc();
    }
}
