using Dalamud.Game;

namespace GatherBuddy.SeFunctions;

public sealed class SpearFishLogData : SeAddressBase
{
    public SpearFishLogData(SigScanner sigScanner)
        : base(sigScanner, "48 8D 0D ?? ?? ?? ?? 41 0F B6 0C 08 41 B0 01 84 D1 48 8B 4E 38")
    { }
}
