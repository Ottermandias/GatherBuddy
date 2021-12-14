using System;
using Dalamud.Plugin.Ipc;
using Dalamud.Plugin.Ipc.Exceptions;

namespace GatherBuddy.Plugin;

public class WotsitIpc : IDisposable
{
    private readonly ICallGateSubscriber<string, string, uint, string>? _register;
    private readonly ICallGateSubscriber<string, bool>?                 _unregister;
    private readonly ICallGateSubscriber<string, bool>?                 _invoke;
    private readonly string                                             _guid;

    public WotsitIpc()
    {
        _register   = Dalamud.PluginInterface.GetIpcSubscriber<string, string, uint, string>("FA.Register");
        _unregister = Dalamud.PluginInterface.GetIpcSubscriber<string, bool>("FA.UnregisterAll");
        _invoke     = Dalamud.PluginInterface.GetIpcSubscriber<string, bool>("FA.Invoke");

        try
        {
            _guid = _register.InvokeFunc("GatherBuddy", "GatherBuddyIPCTest", 62911);
        }
        catch (IpcNotReadyError)
        {
            _guid = string.Empty;
        }

        try
        {
            _invoke.Subscribe(DoThings);
        }
        catch (IpcError)
        { }
    }

    public void Dispose()
    {
        try
        {
            _unregister?.InvokeFunc("GatherBuddy");
        }
        catch (IpcNotReadyError)
        { }

        try
        {
            _invoke?.Unsubscribe(DoThings);
        }
        catch (IpcError)
        { }
    }

    private static void DoThings(string data)
    {
        Dalamud.Chat.Print(data);
    }
}
