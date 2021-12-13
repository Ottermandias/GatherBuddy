using System;
using Dalamud.Game;

namespace GatherBuddy.SeFunctions
{
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

    public sealed class EventFramework : SeAddressBase
    {
        private const int FishingManagerOffset = 0x70;
        private const int FishingStateOffset   = 0x220;

        private readonly IntPtr _fishingManager;
        private readonly IntPtr _fishingState;

        public unsafe FishingState FishingState
            => _fishingState != IntPtr.Zero ? *(FishingState*) _fishingState : FishingState.None;

        public unsafe EventFramework(SigScanner sigScanner)
            : base(sigScanner,
                "48 8B 2D ?? ?? ?? ?? 48 8B F1 48 8B 85 ?? ?? ?? ?? 48 8B 18 48 3B D8 74 35 0F 1F 00 F6 83 ?? ?? ?? ?? ?? 75 1D 48 8B 46 28 48 8D 4E 28 48 8B 93 ?? ?? ?? ??")
        {
            if (Address != IntPtr.Zero)
            {
                _fishingManager = *(IntPtr*) (*(IntPtr*) Address + FishingManagerOffset);
                _fishingState   = _fishingManager + FishingStateOffset;
            }
            else
            {
                _fishingManager = IntPtr.Zero;
                _fishingState   = IntPtr.Zero;
            }
        }
    }
}
