using System;
using System.Linq;
using System.Runtime.InteropServices;
using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Plugin;
using Lumina.Excel.GeneratedSheets;
using Action = System.Action;

namespace GatherBuddy.SeFunctions;

public unsafe class FishLog
{
    public const     uint  SpearFishIdOffset = 20000;
    private readonly byte* _fish;
    private readonly byte* _spearFish;
    private readonly uint  _numFish;
    private readonly uint  _numSpearFish;

    private readonly byte[] _fishStore;
    private readonly byte[] _spearFishStore;

    public event Action? Change;

    public FishLog(SigScanner sigScanner, DataManager gameData)
    {
        _numFish      = (uint)(gameData.GetExcelSheet<FishParameter>()?.Count(f => f.IsInLog) ?? 0);
        _numSpearFish = gameData.GetExcelSheet<SpearfishingItem>()?.RowCount ?? 0;

        _fish      = (byte*)new FishLogData(sigScanner).Address;
        _spearFish = (byte*)new SpearFishLogData(sigScanner).Address;

        _fishStore      = new byte[(_numFish + 7) / 8];
        _spearFishStore = new byte[(_numSpearFish + 7) / 8];
        CheckForChanges();
    }

    private static bool CheckForChanges(byte* ptr, byte[] data, int num)
    {
        fixed (byte* ptr2 = data)
        {
            if (Functions.MemCmpUnchecked(ptr, ptr2, num))
                return false;
        }

        Marshal.Copy((IntPtr)ptr, data, 0, num);
        return true;
    }

    public bool CheckForChanges()
    {
        if (!CheckForChanges(_fish,      _fishStore,      _fishStore.Length)
         && !CheckForChanges(_spearFish, _spearFishStore, _spearFishStore.Length))
            return false;

        Change?.Invoke();
        return true;
    }


    public bool SpearFishIsUnlocked(uint spearFishId)
    {
        spearFishId -= SpearFishIdOffset;
        if (spearFishId >= _numSpearFish)
        {
            PluginLog.Error($"Spearfish Id {spearFishId} is larger than number of spearfish in log {_numSpearFish}.");
            return false;
        }

        if (_spearFish == null)
        {
            PluginLog.Error("Requesting spearfish log completion, but pointer not set.");
            return false;
        }

        var offset = spearFishId / 8;
        var bit    = (byte)spearFishId % 8;
        return ((_spearFish[offset] >> bit) & 1) == 1;
    }

    public bool FishIsUnlocked(uint fishId)
    {
        if (fishId >= _numFish)
        {
            PluginLog.Error($"Fish Id {fishId} is larger than number of fish in log {_numFish}.");
            return false;
        }

        if (_fish == null)
        {
            PluginLog.Error("Requesting fish log completion, but pointer not set.");
            return false;
        }

        var offset = fishId / 8;
        var bit    = (byte)fishId % 8;
        return ((_fish[offset] >> bit) & 1) == 1;
    }

    public bool IsUnlocked(Fish fish)
        => fish.IsSpearFish ? SpearFishIsUnlocked(fish.FishId) : FishIsUnlocked(fish.FishId);
}
