using System.Collections.Generic;
using System.Linq;
using Dalamud.Game;
using Dalamud.Game.Text.SeStringHandling;
using Lumina.Excel.GeneratedSheets;
using FishingSpot = GatherBuddy.Classes.FishingSpot;

namespace GatherBuddy.FishTimer.Parser;

public partial class FishingParser
{
    public readonly  Dictionary<string, FishingSpot> FishingSpotNames;
    private readonly Regexes                         _regexes = Regexes.FromLanguage(GatherBuddy.Language);

    private static Dictionary<string, FishingSpot> SetupFishingSpotNames()
        => GatherBuddy.Language == ClientLanguage.German
            ? SetupFishingSpotNamesGerman()
            : GatherBuddy.GameData.FishingSpots.Values
                .Where(fs => !fs.Spearfishing)
                .GroupBy(fs => fs.Name.ToLowerInvariant())
                .ToDictionary(g => g.Key, g => g.First());

    // German has some weird rules for fishing spot names, thus special consideration.
    private static Dictionary<string, FishingSpot> SetupFishingSpotNamesGerman()
    {
        var ret            = new Dictionary<string, FishingSpot>();
        var placeNameSheet = Dalamud.GameData.GetExcelSheet<PlaceName>(ClientLanguage.German)!;

        foreach (var fs in GatherBuddy.GameData.FishingSpots.Values.Where(fs => !fs.Spearfishing))
        {
            var seBytes = placeNameSheet.GetRow(fs.FishingSpotData!.PlaceName.Row)!.Unknown8.RawData;
            if (seBytes.Length <= 0)
                continue;

            var name1Length = seBytes[6] - 1;
            var name2Start  = 9 + name1Length;
            var name2Length = seBytes[name2Start - 1] - 1;
            var name1       = SeString.Parse(seBytes.Slice(7,          name1Length)).ToString().ToLowerInvariant();
            var name2       = SeString.Parse(seBytes.Slice(name2Start, name2Length)).ToString().ToLowerInvariant();

            ret[name1]   = fs;
            ret[name2]   = fs;
            ret[fs.Name] = fs;
        }

        return ret;
    }

    // Some fishing spots obey weird rules.
    private static string FishingSpotNameHacks(string lowerName)
    {
        return lowerName switch
        {
            "flounders' floor" => "the flounders' floor",
            _                  => lowerName,
        };
    }
}
