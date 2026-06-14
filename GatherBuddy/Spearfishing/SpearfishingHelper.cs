using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Objects.Enums;
using FFXIVClientStructs.FFXIV.Client.UI;
using GatherBuddy.Enums;
using GatherBuddy.SeFunctions;
using Lumina.Excel.Sheets;
using FishingSpot = GatherBuddy.Classes.FishingSpot;
using SpearfishSize = GatherBuddy.Enums.SpearfishSize;

namespace GatherBuddy.Spearfishing;

public partial class SpearfishingHelper
{
    public readonly Dictionary<uint, FishingSpot> SpearfishingSpots;

    private FishingSpot? _currentSpot;
    private bool         _isOpen;

    public SpearfishingHelper(GameData gameData)
        : base("SpearfishingHelper", WindowFlags, true)
    {
        var points = Dalamud.GameData.GetExcelSheet<GatheringPoint>();

        // We go through all fishing spots and correspond them to their gathering point base.
        var baseNodes = gameData.FishingSpots.Values
            .Where(fs => fs.Spearfishing)
            .ToFrozenDictionary(fs => fs.SpearfishingSpotData!.Value.GatheringPointBase.RowId, fs => fs);

        // Now we correspond all gathering nodes to their associated fishing spot.
        SpearfishingSpots = new Dictionary<uint, FishingSpot>(baseNodes.Count);
        foreach (var point in points)
        {
            if (!baseNodes.TryGetValue(point.GatheringPointBase.RowId, out var node))
                continue;

            SpearfishingSpots.Add(point.RowId, node);
        }

        IsOpen              = GatherBuddy.Config.ShowSpearfishHelper;
        RespectCloseHotkey  = false;
        DisableWindowSounds = true;
        Namespace           = "SpearfishingHelper";
    }

    // We should always have to target a spearfishing spot when opening the window.
    // If we are not, hackery is afoot.
    private FishingSpot? GetTargetFishingSpot()
    {
        if (Dalamud.Targets.Target == null)
            return null;

        if (Dalamud.Targets.Target.ObjectKind != ObjectKind.GatheringPoint)
            return null;

        var id = Dalamud.Targets.Target.BaseId;
        return SpearfishingSpots.GetValueOrDefault(id);
    }

    // Given the current spot we can read the spearfish window and correspond fish to their speed and size.
    // This may result in more than one fish, but does so rarely. Unknown attributes are seen as valid for any attribute.
    private static string Identify(FishingSpot? spot, AddonSpearFishing.FishInfo info)
    {
        const string unknown = "Unknown Fish";

        if (spot == null)
            return unknown;

        var fishes = spot.Items.Where(f =>
                (f.Speed == info.SpearfishSpeed || f.Speed == SpearfishSpeed.Unknown)
             && (f.Size == info.SpearfishSize || f.Size == SpearfishSize.None));
        return fishes.Any() ? string.Join("\n", fishes.Select(f => f.Name[GatherBuddy.Language])) : unknown;
    }
}
