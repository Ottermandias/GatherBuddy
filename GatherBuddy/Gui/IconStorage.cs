using System;
using System.Collections.Generic;
using Dalamud.Interface.Internal;
using Dalamud.Plugin.Services;

namespace GatherBuddy.Gui;

internal class Icons : IDisposable
{
    private readonly Dictionary<uint, IDalamudTextureWrap> _icons;
    private readonly ITextureProvider                      _provider;

    public Icons(ITextureProvider provider, int size = 0)
    {
        _icons    = new Dictionary<uint, IDalamudTextureWrap>(size);
        _provider = provider;
    }

    public IDalamudTextureWrap this[uint id]
        => LoadIcon(id);

    public IDalamudTextureWrap this[int id]
        => LoadIcon((uint)id);

    public IDalamudTextureWrap LoadIcon(uint id, bool keepAlive = false)
    {
        if (_icons.TryGetValue(id, out var ret))
            return ret;

        ret        = _provider.GetIcon(id, ITextureProvider.IconFlags.HiRes, null, keepAlive)!;
        _icons[id] = ret;
        return ret;
    }

    public void Dispose()
    {
        foreach (var icon in _icons.Values)
            icon.Dispose();
    }

    public static Icons DefaultStorage { get; private set; } = null!;

    public static void InitDefaultStorage(ITextureProvider provider)
        => DefaultStorage = new Icons(provider, 1024);
}
