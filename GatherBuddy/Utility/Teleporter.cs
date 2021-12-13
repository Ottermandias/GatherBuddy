using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game.UI;

namespace GatherBuddy.Utility
{
    public static unsafe class Teleporter
    {
        public static bool Teleport(uint aetheryte)
        {
            if (Dalamud.ClientState.LocalPlayer == null)
                return false;

            var teleport = Telepo.Instance();
            if (teleport == null)
            {
                PluginLog.Error("Could not teleport: Telepo is missing.");
                return false;
            }

            if (teleport->TeleportList.Size() == 0)
                teleport->UpdateAetheryteList();

            var endPtr  = teleport->TeleportList.Last;
            for (var it = teleport->TeleportList.First; it != endPtr; ++it)
            {
                if (it->AetheryteId == aetheryte)
                    return teleport->Teleport(aetheryte, 0);
            }
            Dalamud.Chat.PrintError("Could not teleport, not attuned.");
            return false;
        }
    }
}
