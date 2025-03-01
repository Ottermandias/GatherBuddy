using System;
using Dalamud.Plugin.Ipc;
using ECommons.DalamudServices;
using ECommons.EzIpcManager;

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
        EzIPC.Init(this, prefix: "GatherBuddy");

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

        _identifyProvider = Dalamud.PluginInterface.GetIpcProvider<string, uint>(IdentifyName);
        try
        {
            _identifyProvider.RegisterFunc(Identify);
        }
        catch (Exception e)
        {
            _identifyProvider = null;
            GatherBuddy.Log.Error($"Could not obtain provider for {IdentifyName}:\n{e}");
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

    #region AutoGatherer IPC
    /// <summary>
    /// Event called when AutoGatherer is waiting for the next node to be ready
    /// </summary>
    [EzIPCEvent] public Action AutoGathererEventWaitingForNextNode;

    /// <summary>
    /// Event called then AutoGatherer is aborting its operations
    /// </summary>
    [EzIPCEvent] public Action<string?> AutoGathererEventAborting;

    /// <summary>
    /// Returns wether AutoGatherer is active or not.
    /// </summary>
    [EzIPC]
    public bool IsAutoGathererEnabled()
    {
        return GatherBuddy.AutoGather.Enabled;
    }
    
    /// <summary>
    /// Returns wether AutoGatherer is idling (waiting for the next node) or not.
    /// </summary>
    [EzIPC]
    public bool IsAutoGathererIdling()
    {
        return GatherBuddy.AutoGather.AutoStatus == "No available items to gather";
    }

    /// <summary>
    /// Enable/Disable AutoGatherer
    /// </summary>
    /// <param name="param">this parameter is to set the Enabled property of AutoGatherer</param>
    [EzIPC]
    public void SetAutoGathererEnabled(bool isEnabled)
    {
        GatherBuddy.AutoGather.Enabled = isEnabled;
    }

    #endregion
}
