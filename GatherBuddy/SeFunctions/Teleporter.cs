using FFXIVClientStructs.FFXIV.Client.Game.UI;
using GatherBuddy.Plugin;

namespace GatherBuddy.SeFunctions;

public static unsafe class Teleporter
{
    public static bool IsAttuned(uint aetheryte)
    {
        var teleport = Telepo.Instance();
        if (teleport == null)
        {
            GatherBuddy.Log.Error("Could not check attunement: Telepo is missing.");
            return false;
        }

        if (!Dalamud.PlayerState.IsLoaded)
            return true;
        teleport->UpdateAetheryteList();

        var endPtr = teleport->TeleportList.Last;
        for (var it = teleport->TeleportList.First; it != endPtr; ++it)
        {
            if (it->AetheryteId == aetheryte)
                return true;
        }

        return false;
    }

    public static bool Teleport(uint aetheryte)
    {
        if (IsAttuned(aetheryte))
        {
            Telepo.Instance()->Teleport(aetheryte, 0);
            return true;
        }

        Communicator.PrintError("Could not teleport to ",
            GatherBuddy.GameData.Aetherytes.TryGetValue(aetheryte, out var a) ? a.Name : "Unknown Aetheryte", GatherBuddy.Config.SeColorNames,
            " not attuned.");
        return false;
    }

    // Teleport without checking for attunement. Use at own risk.
    public static void TeleportUnchecked(uint aetheryte)
    {
        Telepo.Instance()->Teleport(aetheryte, 0);
    }
}
