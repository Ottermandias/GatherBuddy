using Dalamud.Game;

namespace GatherBuddy.SeFunctions;

public sealed class SpearFishLogData : SeAddressBase
{
    public SpearFishLogData(ISigScanner sigScanner)
        : base(sigScanner, "48 8D 05 ?? ?? ?? ?? 41 0F B6 04 ?? D3 E2 84 D0 0F B6 46")
    { }
}
