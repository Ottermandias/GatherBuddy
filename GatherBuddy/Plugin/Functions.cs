using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Keys;
using ECommons.DalamudServices;
using Lumina.Excel.Sheets;
using OtterGui.Classes;

namespace GatherBuddy.Plugin;

public static class Functions
{
    // Clamps invalid indices to valid indices.
    public static bool Swap<T>(IList<T> list, int idx1, int idx2)
    {
        idx1 = Math.Clamp(idx1, 0, list.Count);
        idx2 = Math.Clamp(idx2, 0, list.Count);
        if (idx1 == idx2)
            return false;

        (list[idx1], list[idx2]) = (list[idx2], list[idx1]);
        return true;
    }

    public static FileInfo? ObtainSaveFile(string fileName)
    {
        var dir = new DirectoryInfo(Dalamud.PluginInterface.GetPluginConfigDirectory());
        if (dir.Exists)
            return new FileInfo(Path.Combine(dir.FullName, fileName));

        try
        {
            dir.Create();
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not create save directory at {dir.FullName}:\n{e}");
            return null;
        }

        return new FileInfo(Path.Combine(dir.FullName, fileName));
    }

    public static bool CompareCi(string lhs, string rhs)
        => string.Compare(lhs, rhs, StringComparison.InvariantCultureIgnoreCase) == 0;

    public static bool TryParseBoolean(string text, out bool parsed)
    {
        parsed = false;
        if (text.Length == 1)
        {
            if (text[0] != '1')
                return text[0] == '0';

            parsed = true;
            return true;
        }

        if (!CompareCi(text,       "on") && !CompareCi(text, "true"))
            return CompareCi(text, "off") || CompareCi(text, "false");

        parsed = true;
        return true;
    }

    [DllImport("msvcrt.dll")]
    private static extern unsafe int memcmp(byte* b1, byte* b2, int count);

    public static unsafe bool MemCmpUnchecked(byte* ptr1, byte* ptr2, int count)
        => memcmp(ptr1, ptr2, count) == 0;

    public static unsafe bool MemCmp(byte* ptr1, byte* ptr2, int length1, int length2)
    {
        if (length1 != length2)
            return false;

        if (ptr1 == ptr2 || length1 == 0)
            return true;

        if (ptr1 == null || ptr2 == null)
            return false;

        return memcmp(ptr1, ptr2, length1) == 0;
    }

    public static string CompressedBase64(byte[] data)
    {
        using var compressedStream = new MemoryStream();
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
        {
            zipStream.Write(data, 0, data.Length);
        }

        return Convert.ToBase64String(compressedStream.ToArray());
    }

    public static byte[] DecompressedBase64(string compressedBase64)
    {
        var       data             = Convert.FromBase64String(compressedBase64);
        using var compressedStream = new MemoryStream(data);
        using var zipStream        = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var resultStream     = new MemoryStream();
        zipStream.CopyTo(resultStream);
        return resultStream.ToArray();
    }

    public static bool BoundByDuty()
        => (Dalamud.Conditions[ConditionFlag.BoundByDuty]
             || Dalamud.Conditions[ConditionFlag.BoundByDuty56]
             || Dalamud.Conditions[ConditionFlag.BoundByDuty95]
             || Dalamud.Conditions[ConditionFlag.WatchingCutscene]
             || Dalamud.Conditions[ConditionFlag.WatchingCutscene78]
             || Dalamud.Conditions[ConditionFlag.InDeepDungeon])
         && !InIslandSanctuary()
         && !InTheDiadem();

    public static bool BetweenAreas()
        => Dalamud.Conditions[ConditionFlag.BetweenAreas]
         || Dalamud.Conditions[ConditionFlag.BetweenAreas51];

    public static bool InTheDiadem()
        => Dalamud.ClientState.TerritoryType is 901 or 929 or 939;

    public static bool OnHomeWorld()
    {
        var world = Dalamud.ClientState.LocalPlayer?.CurrentWorld;
        var home = Dalamud.ClientState.LocalPlayer?.HomeWorld;
        return world?.Value.RowId == home?.Value.RowId;
    }

    public static bool InIslandSanctuary()
        => Dalamud.GameData.GetExcelSheet<TerritoryType>()
            .GetRowOrDefault(Dalamud.ClientState.TerritoryType)?.TerritoryIntendedUse.RowId == 49;

    public static bool Move<T>(IList<T> list, int idx1, int idx2)
    {
        idx1 = Math.Clamp(idx1, 0, list.Count - 1);
        idx2 = Math.Clamp(idx2, 0, list.Count - 1);
        if (idx1 == idx2)
            return false;

        var tmp = list[idx1];
        // move element down and shift other elements up
        if (idx1 < idx2)
            for (var i = idx1; i < idx2; i++)
                list[i] = list[i + 1];
        // move element up and shift other elements down
        else
            for (var i = idx1; i > idx2; i--)
                list[i] = list[i - 1];
        list[idx2] = tmp;
        return true;
    }

    public static bool CheckModifier(ModifierHotkey key, bool noKey)
        => key.Modifier switch
        {
            VirtualKey.CONTROL => Dalamud.Keys[VirtualKey.CONTROL],
            VirtualKey.SHIFT   => Dalamud.Keys[VirtualKey.SHIFT],
            VirtualKey.MENU    => Dalamud.Keys[VirtualKey.MENU],
            VirtualKey.NO_KEY  => noKey,
            _                  => false,
        };

    public static bool CheckKeyState(ModifiableHotkey key, bool noKey)
    {
        if (key.Hotkey == VirtualKey.NO_KEY)
            return noKey;

        return Dalamud.Keys[key.Hotkey] && CheckModifier(key.Modifier1, true) && CheckModifier(key.Modifier2, true);
    }
}
