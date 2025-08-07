using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Dalamud.Game;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using GatherBuddy.Classes;
using GatherBuddy.Plugin;
using Lumina.Excel.Sheets;
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

    public FishLog(ISigScanner sigScanner, IDataManager gameData)
    {
        _numFish      = (uint) gameData.GetExcelSheet<FishParameter>().Count;
        _numSpearFish = (uint) gameData.GetExcelSheet<SpearfishingItem>().Count;

        ;
        _fish      = (byte*)Unsafe.AsPointer(ref PlayerState.Instance()->CaughtFishBitmask[0]);
        _spearFish = (byte*)Unsafe.AsPointer(ref PlayerState.Instance()->CaughtSpearfishBitmask[0]);

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
            GatherBuddy.Log.Error($"Spearfish Id {spearFishId} is larger than number of spearfish in log {_numSpearFish}.");
            return false;
        }

        if (_spearFish == null)
        {
            GatherBuddy.Log.Error("Requesting spearfish log completion, but pointer not set.");
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
            GatherBuddy.Log.Error($"Fish Id {fishId} is larger than number of fish in log {_numFish}.");
            return false;
        }

        if (_fish == null)
        {
            GatherBuddy.Log.Error("Requesting fish log completion, but pointer not set.");
            return false;
        }

        var offset = fishId / 8;
        var bit    = (byte)fishId % 8;
        return ((_fish[offset] >> bit) & 1) == 1;
    }

    public bool IsUnlocked(Fish fish)
        => !fish.InLog || (fish.IsSpearFish ? SpearFishIsUnlocked(fish.FishId) : FishIsUnlocked(fish.FishId));

    public void SetAllUnlocked()
    {
        for (var i = 0; i < _fishStore.Length; ++i)
            _fish[i] = 0xFF;
    }
}
