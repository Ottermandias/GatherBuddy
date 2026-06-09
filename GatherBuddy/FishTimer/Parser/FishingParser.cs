using System;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using GatherBuddy.Classes;
using GatherBuddy.Enums;

namespace GatherBuddy.FishTimer.Parser;

public unsafe partial class FishingParser : IDisposable
{
    public event Action<FishingSpot?>?                       BeganFishing;
    public event Action?                                     BeganMooching;
    public event Action<Fish, ushort, byte, bool, bool>?     CaughtFish;
    public event Action<FishingSpot>?                        IdentifiedSpot;
    public event Action<HookSet>?                            HookedIn;
    private readonly Hook<AgentCatch.Delegates.UpdateCatch>  _catchHook;
    private readonly Hook<ActionManager.Delegates.UseAction> _hookHook;

    public FishingParser(IGameInteropProvider provider)
    {
        FishingSpotNames = SetupFishingSpotNames();

        _catchHook = provider.HookFromAddress<AgentCatch.Delegates.UpdateCatch>(
            (nint)AgentCatch.MemberFunctionPointers.UpdateCatch,
            OnCatchUpdate);

        _hookHook = provider.HookFromAddress<ActionManager.Delegates.UseAction>(
            (nint)ActionManager.MemberFunctionPointers.UseAction,
            OnUseAction);
    }

    public void Enable()
    {
        _catchHook.Enable();
        _hookHook.Enable();
        Dalamud.Chat.ChatMessage += OnMessageDelegate;
    }

    public void Disable()
    {
        _catchHook.Disable();
        _hookHook.Disable();
        Dalamud.Chat.ChatMessage -= OnMessageDelegate;
    }

    public void Dispose()
    {
        Disable();
        _catchHook.Dispose();
        _hookHook.Dispose();
    }

    private void OnCatchUpdate(
        AgentCatch* thisPtr,
        uint itemId,
        bool isLarge,
        ushort size,
        byte amount,
        byte level,
        byte stars,
        byte oceanStars,
        bool isMoochable,
        bool isFirstTimeCatch,
        byte a11,
        byte a12)
    {
        if (!GatherBuddy.Config.HideFishSizePopup)
            _catchHook.Original(thisPtr, itemId, isLarge, size, amount, level, stars, oceanStars, isMoochable, isFirstTimeCatch, a11, a12);

        // Check against collectibles.
        var collectible = false;
        if (itemId > 500000)
        {
            itemId      -= 500000;
            collectible =  true;
        }

        if (!GatherBuddy.GameData.Fishes.TryGetValue(itemId, out var fish))
        {
            GatherBuddy.Log.Error($"Unknown fish id {itemId} caught.");
            return;
        }

        CaughtFish?.Invoke(fish, size, amount, isLarge, collectible);
    }

    private bool OnUseAction(ActionManager* thisPtr, ActionType actionType, uint actionId, ulong targetId, uint extraParam, ActionManager.UseActionMode mode, uint comboRouteId, bool* outOptAreaTargeted)
    {
        if (actionType == ActionType.Action)
        {
            switch (actionId)
            {
                case 296:   HookedIn?.Invoke(HookSet.Hook); break;
                case 269:   HookedIn?.Invoke(HookSet.DoubleHook); break;
                case 4103:  HookedIn?.Invoke(HookSet.Powerful); break;
                case 4179:  HookedIn?.Invoke(HookSet.Precise); break;
                case 27523: HookedIn?.Invoke(HookSet.TripleHook); break;
                case 41278: HookedIn?.Invoke(HookSet.Stellar); break;
            }
        }

        return _hookHook!.Original(thisPtr, actionType, actionId, targetId, extraParam, mode, comboRouteId, outOptAreaTargeted);
    }
}
