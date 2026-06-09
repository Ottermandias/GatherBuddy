using FFXIVClientStructs.FFXIV.Client.System.String;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace GatherBuddy.SeFunctions;

public class CommandManager
{
    public static unsafe bool Execute(string message)
    {
        // First try to process the command through Dalamud.
        if (Dalamud.Commands.ProcessCommand(message))
        {
            GatherBuddy.Log.Verbose($"Executed Dalamud command \"{message}\".");
            return true;
        }

        const AllowedEntities combinedEntities =
            AllowedEntities.UppercaseLetters |
            AllowedEntities.LowercaseLetters |
            AllowedEntities.Numbers |
            AllowedEntities.SpecialCharacters |
            AllowedEntities.OtherCharacters |
            AllowedEntities.Payloads |
            AllowedEntities.Unknown8 |
            AllowedEntities.Unknown9;

        using var msg = new Utf8String(message);
        msg.SanitizeString(combinedEntities, null);
        UIModule.Instance()->ProcessChatBoxEntry(&msg);

        return false;
    }
}
