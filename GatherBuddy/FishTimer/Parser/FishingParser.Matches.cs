using System.Text.RegularExpressions;
using Dalamud.Game;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Memory;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace GatherBuddy.FishTimer.Parser;

public partial class FishingParser
{
    private void HandleCastMatch(Match match)
    {
        var tmp = match.Groups["FishingSpot"];
        var fishingSpotName = tmp.Success
            ? FishingSpotNameHacks(tmp.Value.ToLowerInvariant())
            : match.Groups["FishingSpotWithArticle"].Value.ToLowerInvariant();

        if (FishingSpotNames.TryGetValue(fishingSpotName, out var fishingSpot))
            BeganFishing?.Invoke(fishingSpot);
        // Hack against 'The' special cases.
        else if (GatherBuddy.Language == ClientLanguage.English
              && fishingSpotName.StartsWith("the ")
              && FishingSpotNames.TryGetValue(fishingSpotName[4..], out fishingSpot))
            BeganFishing?.Invoke(fishingSpot);
        else
            GatherBuddy.Log.Error($"Began fishing at unknown fishing spot: \"{fishingSpotName}\".");
    }

    private void HandleSpotDiscoveredMatch(Match match)
    {
        var fishingSpotName = match.Groups["FishingSpot"].Value.ToLowerInvariant();
        if (FishingSpotNames.TryGetValue(fishingSpotName, out var fishingSpot))
            IdentifiedSpot?.Invoke(fishingSpot);
        // Hack against 'The' special cases.
        else if (GatherBuddy.Language == ClientLanguage.English
              && fishingSpotName.StartsWith("the ")
              && FishingSpotNames.TryGetValue(fishingSpotName[4..], out fishingSpot))
            IdentifiedSpot?.Invoke(fishingSpot);
        else
            GatherBuddy.Log.Error($"Discovered unknown fishing spot: \"{fishingSpotName}\".");
    }

    private void HandleCastMatchCosmic(Match match, string missionName)
    {      
        var tmp = match.Groups["FishingSpot"];
        var fishingSpotName = tmp.Success
            ? FishingSpotNameHacks(tmp.Value.ToLowerInvariant())
            : match.Groups["FishingSpotWithArticle"].Value.ToLowerInvariant();
        fishingSpotName += " | " + missionName.ToLowerInvariant();
        if (FishingSpotNames.TryGetValue(fishingSpotName, out var fishingSpot))
            BeganFishing?.Invoke(fishingSpot);
        // Hack against 'The' special cases.
        else if (GatherBuddy.Language == ClientLanguage.English
              && fishingSpotName.StartsWith("the ")
              && FishingSpotNames.TryGetValue(fishingSpotName[4..], out fishingSpot))
            BeganFishing?.Invoke(fishingSpot);
        else
            GatherBuddy.Log.Error($"Began fishing at unknown fishing spot: \"{fishingSpotName}\".");
    }

    private const XivChatType FishingMessage = (XivChatType)2243;

    private unsafe void OnMessageDelegate(XivChatType type, int timeStamp, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        switch (type)
        {
            case FishingMessage:
            {
                var text = message.TextValue;

                if (text.Contains(_regexes.Undiscovered))
                {
                    BeganFishing?.Invoke(null);
                    return;
                }

                var match = _regexes.Cast.Match(text);
                if (match.Success)
                {
                    if(Dalamud.ClientState.TerritoryType == 1237)
                    {
                        var missionName = MemoryHelper.ReadSeString(&((AtkUnitBase*)Dalamud.GameGui.GetAddonByName("_ToDoList", 1))->UldManager.NodeList[11]->GetAsAtkComponentNode()->Component->UldManager.NodeList[13]->GetAsAtkTextNode()->NodeText).ToString();
                        GatherBuddy.Log.Verbose($"Loaded quest: {missionName}");
                        HandleCastMatchCosmic(match, missionName);
                    }
                    else 
                        HandleCastMatch(match);
                    return;
                }

                match = _regexes.Mooch.Match(text);
                if (match.Success)
                {
                    BeganMooching?.Invoke();
                    return;
                }

                match = _regexes.AreaDiscovered.Match(text);
                if (match.Success)
                    HandleSpotDiscoveredMatch(match);
                break;
            }
        }
    }
}
