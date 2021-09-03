using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game.UI;

namespace GatherBuddy.Utility
{
    public static unsafe class Teleporter
    {
        public static bool Teleport(uint aetheryte)
        {
            var teleport = Telepo.Instance();
            if (teleport == null)
            {
                PluginLog.Error("Could not teleport: Telepo is missing.");
                return false;
            }

            if (teleport->TeleportList.Size() == 0)
                teleport->UpdateAetheryteList();

            var infoPtr = teleport->TeleportList.First;
            var endPtr  = teleport->TeleportList.End;
            while (infoPtr != endPtr)
            {
                if (infoPtr->AetheryteId == aetheryte)
                    return teleport->Teleport(aetheryte, 0);
                ++infoPtr;
            }
            Dalamud.Chat.PrintError("Could not teleport, not attuned.");
            return false;
        }
    }
}
