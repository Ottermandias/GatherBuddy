using System;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin.Ipc;
using GatherBuddy.Interfaces;
using GatherBuddy.Time;

namespace GatherBuddy.Plugin;

public class GatherBuddyIpc : IDisposable
{
    public const int    IpcVersion   = 1;
    public const string VersionName  = $"{GatherBuddy.InternalName}.Version";
    public const string IdentifyName = $"{GatherBuddy.InternalName}.Identify";
    public const string FullIdentifyName = $"{GatherBuddy.InternalName}.FullIdentify";

    private readonly  GatherBuddy  _plugin;
    internal readonly ICallGateProvider<int>?          _versionProvider;
    internal readonly ICallGateProvider<string, uint>? _identifyProvider;
    internal readonly ICallGateProvider<string, (uint, MapLinkPayload?, string?)>? _fullIdenitfyProvider;

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

        _fullIdenitfyProvider = Dalamud.PluginInterface.GetIpcProvider<string, (uint, MapLinkPayload?, string?)>(FullIdentifyName);
        try
        {
            _fullIdenitfyProvider.RegisterFunc(FullIdentify);
        }
        catch (Exception e)
        {
            _fullIdenitfyProvider = null;
            GatherBuddy.Log.Error($"Could not obtain provider for {FullIdentifyName}:\n{e}");
        }
    }

    private static int Version()
        => IpcVersion;

    private uint Identify(string text)
        => _plugin.Executor.Identificator.IdentifyGatherable(text)?.ItemId
         ?? _plugin.Executor.Identificator.IdentifyFish(text)?.ItemId ?? 0;

    private (uint, MapLinkPayload?, string?) FullIdentify(string text)
    {
        IGatherable? item;
        ILocation? location;
        TimeInterval? uptime;

        if ((item = _plugin.Executor.Identificator.IdentifyGatherable(text)) is null)
            if ((item = _plugin.Executor.Identificator.IdentifyFish(text)) is null)
                return (0, null, null);

        (location, uptime) = _plugin.Executor.FindClosestLocation(item);

        if (location is null)
            return (item.ItemId, null, null);

        var mapPayload = new MapLinkPayload(location.Territory.Id, location.Territory.Data.Map.Row, location.IntegralXCoord / 100f, location.IntegralYCoord / 100f, 0.05f);

        string uptimeString;

        if (uptime.Equals(TimeInterval.Always))
            uptimeString = "Always";
        else if (uptime.Equals(TimeInterval.Invalid))
            uptimeString = "Invalid";
        else if (uptime.Equals(TimeInterval.Never))
            uptimeString = "Never";
        else if (uptime.Value.Start > GatherBuddy.Time.ServerTime)
            uptimeString = $"Next up in {TimeInterval.DurationString(uptime.Value.Start, GatherBuddy.Time.ServerTime, false)}";
        else
            uptimeString = $"Currently up for the next {TimeInterval.DurationString(uptime.Value.End, GatherBuddy.Time.ServerTime, false)}";
        
        return (item.ItemId, mapPayload, uptimeString);
    }

    public void Dispose()
    {
        _identifyProvider?.UnregisterFunc();
        _versionProvider?.UnregisterFunc();
        _fullIdenitfyProvider?.UnregisterFunc();
    }
}
