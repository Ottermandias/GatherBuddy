using Dalamud.Game;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace GatherBuddy.SeFunctions;

public sealed class CurrentBait : SeAddressBase
{
    public CurrentBait(SigScanner sigScanner)
        : base(sigScanner, "48 83 C4 30 5B C3 49 8B C8 E8 ?? ?? ?? ?? 3B 05")
    {
        SignatureHelper.Initialise(this);
    }

    public unsafe uint Current
        => *(uint*)Address;

    private delegate byte ExecuteCommandDelegate(int id, int unk1, uint baitId, int unk2, int unk3);

    [Signature("E8 ?? ?? ?? ?? 8D 43 0A")]
    private readonly ExecuteCommandDelegate _executeCommand = null!;

    public enum ChangeBaitReturn
    {
        Success,
        AlreadyEquipped,
        NotInInventory,
        InvalidBait,
        UnknownError,
    }

    public unsafe ChangeBaitReturn ChangeBait(uint baitId)
    {
        if (baitId == Current)
            return ChangeBaitReturn.AlreadyEquipped;

        if (baitId == 0 || !GatherBuddy.GameData.Bait.ContainsKey(baitId))
            return ChangeBaitReturn.InvalidBait;

        if (InventoryManager.Instance()->GetInventoryItemCount(baitId) <= 0)
            return ChangeBaitReturn.NotInInventory;


        return _executeCommand(701, 4, baitId, 0, 0) == 1 ? ChangeBaitReturn.Success : ChangeBaitReturn.UnknownError;
    }
}
