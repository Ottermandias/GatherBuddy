using System.Text.RegularExpressions;
using Dalamud;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Logging;

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
            PluginLog.Error($"Began fishing at unknown fishing spot: \"{fishingSpotName}\".");
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
            PluginLog.Error($"Discovered unknown fishing spot: \"{fishingSpotName}\".");
    }

    private const XivChatType FishingMessage      = (XivChatType)2243;


    private void OnMessageDelegate(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
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
