using System;
using System.Collections.Generic;
using Dalamud.Utility;
using ImGuiScene;
using Lumina.Data.Files;

namespace GatherBuddy.Gui;

internal class Icons : IDisposable
{
    private readonly Dictionary<uint, TextureWrap> _icons;

    public Icons(int size = 0)
        => _icons = new Dictionary<uint, TextureWrap>(size);

    public TextureWrap this[uint id]
        => LoadIcon(id);

    public TextureWrap this[int id]
        => LoadIcon((uint)id);

    private static TexFile? GetHdIcon(uint id)
    {
        var path = $"ui/icon/{id / 1000 * 1000:000000}/{id:000000}_hr1.tex";
        return Dalamud.GameData.GetFile<TexFile>(path);
    }

    public TextureWrap LoadIcon(uint id)
    {
        if (_icons.TryGetValue(id, out var ret))
            return ret;

        var icon     = GetHdIcon(id) ?? Dalamud.GameData.GetIcon(id)!;
        var iconData = icon.GetRgbaImageData();

        ret        = Dalamud.PluginInterface.UiBuilder.LoadImageRaw(iconData, icon.Header.Width, icon.Header.Height, 4);
        _icons[id] = ret;
        return ret;
    }

    public void Dispose()
    {
        foreach (var icon in _icons.Values)
            icon.Dispose();
    }

    public static Icons DefaultStorage { get; } = new(1024);
}
