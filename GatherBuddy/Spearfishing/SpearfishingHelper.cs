using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Objects.Enums;
using GatherBuddy.Enums;
using GatherBuddy.SeFunctions;
using Lumina.Excel.GeneratedSheets;
using FishingSpot = GatherBuddy.Classes.FishingSpot;

namespace GatherBuddy.Spearfishing;

public partial class SpearfishingHelper
{
    public readonly Dictionary<uint, FishingSpot> SpearfishingSpots;

    private FishingSpot? _currentSpot;
    private bool         _isOpen;

    public SpearfishingHelper(GameData gameData)
    {
        var points = Dalamud.GameData.GetExcelSheet<GatheringPoint>()!;

        // We go through all fishingspots and correspond them to their gathering point base.
        var baseNodes = GatherBuddy.GameData.FishingSpots.Values
            .Where(fs => fs.Spearfishing)
            .ToDictionary(fs => fs.SpearfishingSpotData!.GatheringPointBase.Row, fs => fs);

        // Now we correspond all gathering nodes to their associated fishing spot.
        SpearfishingSpots = new Dictionary<uint, FishingSpot>(baseNodes.Count);
        foreach (var point in points)
        {
            if (!baseNodes.TryGetValue(point.GatheringPointBase.Row, out var node))
                continue;

            SpearfishingSpots.Add(point.RowId, node);
        }
    }

    // We should always have to target a spearfishing spot when opening the window.
    // If we are not, hackery is afoot.
    private FishingSpot? GetTargetFishingSpot()
    {
        if (Dalamud.Targets.Target == null)
            return null;

        if (Dalamud.Targets.Target.ObjectKind != ObjectKind.GatheringPoint)
            return null;

        var id = Dalamud.Targets.Target.DataId;
        return !SpearfishingSpots.TryGetValue(id, out var spot) ? null : spot;
    }

    // Given the current spot we can read the spearfish window and correspond fish to their speed and size.
    // This may result in more than one fish, but does so rarely. Unknown attributes are seen as valid for any attribute.
    private static string Identify(FishingSpot? spot, SpearfishWindow.Info info)
    {
        const string unknown = "Unknown Fish";

        if (spot == null)
            return unknown;

        var fishes = spot.Items.Where(f =>
                (f.Speed == info.Speed || f.Speed == SpearfishSpeed.Unknown)
             && (f.Size == info.Size || f.Size == SpearfishSize.Unknown))
            .ToList();
        return fishes.Count == 0 ? unknown : string.Join("\n", fishes.Select(f => f.Name[GatherBuddy.Language]));
    }
}
