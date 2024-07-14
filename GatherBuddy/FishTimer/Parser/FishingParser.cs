using System;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.SeFunctions;

namespace GatherBuddy.FishTimer.Parser;

public partial class FishingParser : IDisposable
{
    private delegate bool UseActionDelegate(IntPtr manager, ActionType actionType, uint actionId, ulong targetId, uint a4, uint a5,
        uint a6, IntPtr a7);

    public event Action<FishingSpot?>?                   BeganFishing;
    public event Action?                                 BeganMooching;
    public event Action<Fish, ushort, byte, bool, bool>? CaughtFish;
    public event Action<FishingSpot>?                    IdentifiedSpot;
    public event Action<HookSet>?                        HookedIn;

    private readonly Hook<UpdateCatchDelegate>? _catchHook;
    private readonly Hook<UseActionDelegate>?   _hookHook;

    public unsafe FishingParser(IGameInteropProvider provider)
    {
        FishingSpotNames = SetupFishingSpotNames();
        _catchHook       = new UpdateFishCatch(Dalamud.SigScanner).CreateHook(provider, OnCatchUpdate);
        var hookPtr = (IntPtr)ActionManager.MemberFunctionPointers.UseAction;
        _hookHook = provider.HookFromAddress<UseActionDelegate>(hookPtr, OnUseAction);
    }

    public void Enable()
    {
        _hookHook?.Enable();
        _catchHook?.Enable();
        Dalamud.Chat.CheckMessageHandled += OnMessageDelegate;
    }

    public void Disable()
    {
        _catchHook?.Disable();
        _hookHook?.Disable();
        Dalamud.Chat.CheckMessageHandled -= OnMessageDelegate;
    }

    public void Dispose()
    {
        Disable();
        _catchHook?.Dispose();
        _hookHook?.Dispose();
    }

    private void OnCatchUpdate(IntPtr module, uint fishId, bool large, ushort size, byte amount, byte level, byte unk7, byte unk8, byte unk9,
        byte unk10, byte unk11, byte unk12)
    {
        if (!GatherBuddy.Config.HideFishSizePopup)
            _catchHook!.Original(module, fishId, large, size, amount, level, unk7, unk8, unk9, unk10, unk11, unk12);

        // Check against collectibles.
        var collectible = false;
        if (fishId > 500000)
        {
            fishId      -= 500000;
            collectible =  true;
        }

        if (!GatherBuddy.GameData.Fishes.TryGetValue(fishId, out var fish))
        {
            GatherBuddy.Log.Error($"Unknown fish id {fishId} caught.");
            return;
        }

        CaughtFish?.Invoke(fish, size, amount, large, collectible);
    }

    private bool OnUseAction(IntPtr manager, ActionType actionType, uint actionId, ulong targetId, uint a4, uint a5, uint a6, IntPtr a7)
    {
        if (actionType == ActionType.Action)
            switch (actionId)
            {
                case 296:
                    HookedIn?.Invoke(HookSet.Hook);
                    break;
                case 269:
                    HookedIn?.Invoke(HookSet.DoubleHook);
                    break;
                case 4103:
                    HookedIn?.Invoke(HookSet.Powerful);
                    break;
                case 4179:
                    HookedIn?.Invoke(HookSet.Precise);
                    break;
                case 27523:
                    HookedIn?.Invoke(HookSet.TripleHook);
                    break;
            }

        return _hookHook!.Original(manager, actionType, actionId, targetId, a4, a5, a6, a7);
    }
}
