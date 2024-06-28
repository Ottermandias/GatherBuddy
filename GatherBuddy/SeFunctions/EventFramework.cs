using System;
using Dalamud.Game;

namespace GatherBuddy.SeFunctions;

public enum FishingState : byte
{
    None       = 0,
    PoleOut    = 1,
    PullPoleIn = 2,
    Quit       = 3,
    PoleReady  = 4,
    Bite       = 5,
    Reeling    = 6,
    Waiting    = 8,
    Waiting2   = 9,
}

public sealed class EventFramework
{
    private const int FishingManagerOffset = 0x70;
    private const int FishingStateOffset   = 0x220;

    public unsafe nint Address
        => (nint) FFXIVClientStructs.FFXIV.Client.Game.Event.EventFramework.Instance();

    internal unsafe IntPtr FishingManager
    {
        get
        {
            if (Address == IntPtr.Zero)
                return IntPtr.Zero;

            var managerPtr = Address + FishingManagerOffset;
            if (managerPtr == IntPtr.Zero)
                return IntPtr.Zero;

            return *(IntPtr*)managerPtr;
        }
    }

    internal IntPtr FishingStatePtr
    {
        get
        {
            var ptr = FishingManager;
            if (ptr == IntPtr.Zero)
                return IntPtr.Zero;

            return ptr + FishingStateOffset;
        }
    }

    public unsafe FishingState FishingState
    {
        get
        {
            var ptr = FishingStatePtr;
            return ptr != IntPtr.Zero ? *(FishingState*)ptr : FishingState.None;
        }
    }
}
