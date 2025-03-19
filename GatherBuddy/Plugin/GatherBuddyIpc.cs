using ECommons.EzIpcManager;
using System;

namespace GatherBuddy.Plugin;

public class GatherBuddyIpc : IDisposable
{
    public const int IpcVersion = 1;

    private readonly GatherBuddy _plugin;

    public GatherBuddyIpc(GatherBuddy plugin)
    {
        _plugin = plugin;
        EzIPC.Init(this, GatherBuddy.InternalName);
    }

    [EzIPC]
    private static int Version()
        => IpcVersion;

    [EzIPC]
    private uint Identify(string text)
        => _plugin.Executor.Identificator.IdentifyGatherable(text)?.ItemId
         ?? _plugin.Executor.Identificator.IdentifyFish(text)?.ItemId ?? 0;

    public void Dispose()
    {
        // EzIPC will handle disposal automatically through ECommonsMain.Dispose()
    }
}
