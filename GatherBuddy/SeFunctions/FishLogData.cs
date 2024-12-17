using Dalamud.Game;

namespace GatherBuddy.SeFunctions;

public sealed class FishLogData : SeAddressBase
{
    public FishLogData(ISigScanner sigScanner)
        : base(sigScanner, "4C 8D 2D ?? ?? ?? ?? 49 8B 03")
    { }
}
