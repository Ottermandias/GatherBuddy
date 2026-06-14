using FFXIVClientStructs.FFXIV.Client.Game.Event;

namespace GatherBuddy.SeFunctions;

public sealed unsafe class EventFramework
{
    public FishingEventHandler* FishingEventHandler
        => FFXIVClientStructs.FFXIV.Client.Game.Event.EventFramework.Instance()->EventHandlerModule.FishingEventHandler;

    public uint? CurrentSwimBait
    {
        get
        {
            var ptr = FishingEventHandler;
            if (ptr == null)
                return null;

            return ptr->CurrentSelectedSwimBait switch
            {
                0x00 when ptr->SwimBaitItemIds[0] != 0 => ptr->SwimBaitItemIds[0],
                0x01 when ptr->SwimBaitItemIds[1] != 0 => ptr->SwimBaitItemIds[1],
                0x02 when ptr->SwimBaitItemIds[2] != 0 => ptr->SwimBaitItemIds[2],
                _                                      => null,
            };
        }
    }

    public uint? SwimBait(int idx)
    {
        var ptr = FishingEventHandler;
        if (ptr == null)
            return null;

        return idx switch
        {
            0x00 when ptr->SwimBaitItemIds[0] != 0 => ptr->SwimBaitItemIds[0],
            0x01 when ptr->SwimBaitItemIds[1] != 0 => ptr->SwimBaitItemIds[1],
            0x02 when ptr->SwimBaitItemIds[2] != 0 => ptr->SwimBaitItemIds[2],
            _                                      => null,
        };
    }


    public int NumSwimBait
    {
        get
        {
            var ptr = FishingEventHandler;
            if (ptr == null)
                return 0;

            return (ptr->SwimBaitItemIds[0] != 0 ? 1 : 0)
              + (ptr->SwimBaitItemIds[1] != 0 ? 1 : 0)
              + (ptr->SwimBaitItemIds[2] != 0 ? 1 : 0);
        }
    }

    public FishingState FishingState
    {
        get
        {
            var ptr = FishingEventHandler;
            return ptr != null ? ptr->State : FishingState.None;
        }
    }
}
