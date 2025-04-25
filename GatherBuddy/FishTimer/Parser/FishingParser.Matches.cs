using System.Text.RegularExpressions;
using Dalamud.Game;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using GatherBuddy.Plugin;

namespace GatherBuddy.FishTimer.Parser;

public partial class FishingParser
{
    private void HandleCastMatch(Match match, ushort? cosmicMissionHack = null)
    {
        var tmp = match.Groups["FishingSpot"];
        var fishingSpotName = tmp.Success
            ? FishingSpotNameHacks(tmp.Value.ToLowerInvariant())
            : match.Groups["FishingSpotWithArticle"].Value.ToLowerInvariant();
        if (cosmicMissionHack.HasValue)
            fishingSpotName += $" ({cosmicMissionHack.Value:D5})";
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
                    ushort? missionId = null;
                    if (GatherBuddy.GameData.Territories.TryGetValue(Dalamud.ClientState.TerritoryType, out var territory)
                     && territory.Data.TerritoryIntendedUse.RowId is 60)
                    {
                        var wks = WKSManager.Instance();
                        if (wks is not null)
                        {
                            missionId = *(ushort*)((byte*)wks + Offsets.CurrentCosmicQuestOffset);
                            GatherBuddy.Log.Verbose($"Loaded quest: {missionId.Value}");
                        }
                    }

                    HandleCastMatch(match, missionId);

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
