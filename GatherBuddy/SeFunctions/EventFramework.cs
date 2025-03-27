using System.Runtime.InteropServices;

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

public sealed unsafe class EventFramework
{
    private const int FishingManagerOffset = 0x70;

    [StructLayout(LayoutKind.Explicit)]
    internal struct FishingManagerStruct
    {
        [FieldOffset(0x228)]
        public FishingState FishingState;

        [FieldOffset(0x23C)]
        public byte CurrentSelectedSwimBait;

        [FieldOffset(0x240)]
        public uint SwimBaitId1;

        [FieldOffset(0x244)]
        public uint SwimBaitId2;

        [FieldOffset(0x248)]
        public uint SwimBaitId3;
    }

    public nint Address
        => (nint)FFXIVClientStructs.FFXIV.Client.Game.Event.EventFramework.Instance();

    internal FishingManagerStruct* FishingManager
    {
        get
        {
            if (Address == nint.Zero)
                return null;

            var managerPtr = Address + FishingManagerOffset;
            if (managerPtr == nint.Zero)
                return null;

            return *(FishingManagerStruct**)managerPtr;
        }
    }

    public uint? CurrentSwimBait
    {
        get
        {
            var ptr = FishingManager;
            if (ptr == null)
                return null;

            return ptr->CurrentSelectedSwimBait switch
            {
                0x00 when ptr->SwimBaitId1 != 0 => ptr->SwimBaitId1,
                0x01 when ptr->SwimBaitId2 != 0 => ptr->SwimBaitId2,
                0x02 when ptr->SwimBaitId3 != 0 => ptr->SwimBaitId3,
                _                               => null,
            };
        }
    }

    public uint? SwimBait(int idx)
    {
        var ptr = FishingManager;
        if (ptr == null)
            return null;

        return idx switch
        {
            0x00 when ptr->SwimBaitId1 != 0 => ptr->SwimBaitId1,
            0x01 when ptr->SwimBaitId2 != 0 => ptr->SwimBaitId2,
            0x02 when ptr->SwimBaitId3 != 0 => ptr->SwimBaitId3,
            _                               => null,
        };
    }


    public int NumSwimBait
    {
        get
        {
            var ptr = FishingManager;
            if (ptr == null)
                return 0;

            return (ptr->SwimBaitId1 != 0 ? 1 : 0)
              + (ptr->SwimBaitId2 != 0 ? 1 : 0)
              + (ptr->SwimBaitId3 != 0 ? 1 : 0);
        }
    }

    public FishingState FishingState
    {
        get
        {
            var ptr = FishingManager;
            return ptr != null ? ptr->FishingState : FishingState.None;
        }
    }
}
