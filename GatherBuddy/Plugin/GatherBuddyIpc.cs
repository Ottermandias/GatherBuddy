using System;
using Dalamud.Logging;
using Dalamud.Plugin.Ipc;

namespace GatherBuddy.Plugin;

public class GatherBuddyIpc : IDisposable
{
    public const int    IpcVersion   = 1;
    public const string VersionName  = $"{GatherBuddy.InternalName}.Version";
    public const string IdentifyName = $"{GatherBuddy.InternalName}.Identify";

    private readonly  GatherBuddy  _plugin;
    internal readonly ICallGateProvider<int>?          _versionProvider;
    internal readonly ICallGateProvider<string, uint>? _identifyProvider;

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
            PluginLog.Error($"Could not obtain provider for {VersionName}:\n{e}");
        }

        _identifyProvider = Dalamud.PluginInterface.GetIpcProvider<string, uint>(IdentifyName);
        try
        {
            _identifyProvider.RegisterFunc(Identify);
        }
        catch (Exception e)
        {
            _identifyProvider = null;
            PluginLog.Error($"Could not obtain provider for {IdentifyName}:\n{e}");
        }
    }

    private static int Version()
        => IpcVersion;

    private uint Identify(string text)
        => _plugin.Executor.Identificator.IdentifyGatherable(text)?.ItemId
         ?? _plugin.Executor.Identificator.IdentifyFish(text)?.ItemId ?? 0;

    public void Dispose()
    {
        _identifyProvider?.UnregisterFunc();
        _versionProvider?.UnregisterFunc();
    }
}
