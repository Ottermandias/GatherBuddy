using GatherBuddy.Classes;

namespace GatherBuddy.Interfaces;

public interface ITeleportable
{
    public Aetheryte? ClosestAetheryte { get; set; }

    public Aetheryte? DefaultAetheryte { get; }


    public bool SetAetheryte(Aetheryte? closestAetheryte)
    {
        if (closestAetheryte is { Id: 0 })
            return false;

        ClosestAetheryte = closestAetheryte;
        return true;
    }
}
