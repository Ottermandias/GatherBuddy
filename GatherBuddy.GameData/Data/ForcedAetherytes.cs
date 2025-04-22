using Lumina.Excel.Sheets;

namespace GatherBuddy.Data;

public static class ForcedAetherytes
{
    public static readonly (uint ZoneId, uint AetheryteId)[] ZonesWithoutAetherytes =
    {
        (128, 8),    // Limsa Upper Decks -> Limsa
        (900, 8),    // The Endeavor -> Limsa
        (901, 70),   // The Diadem -> Foundation
        (929, 70),   // The Diadem -> Foundation
        (939, 70),   // The Diadem -> Foundation
        (399, 75),   // The Dravanian Hinterlands -> Idyllshire
        (133, 2),    // Old Gridania -> New Gridania,
        (339, 8),    // Mist -> Limsa
        (340, 2),    // Lavender Beds -> New Gridania
        (341, 9),    // Goblet -> Ul'dah
        (641, 111),  // Shirogane -> Kugane
        (1073, 181), // Elysion -> Base Omicron
        (1237, 175), // Sinus Ardorum -> Bestway Burrows
    };

    public static void ApplyMissingAetherytes(GameData data)
    {
        var sheet = data.DataManager.GetExcelSheet<TerritoryType>();
        foreach (var (zoneId, aetheryteId) in ZonesWithoutAetherytes)
        {
            var territoryType = sheet.GetRow(zoneId);
            var territory     = data.FindOrAddTerritory(territoryType);
            if (territory == null)
            {
                data.Log.Error($"Could not find territory {zoneId}.");
                continue;
            }

            if (!data.Aetherytes.TryGetValue(aetheryteId, out var aetheryte))
            {
                data.Log.Error($"Could not find aetheryte {aetheryteId}.");
                continue;
            }

            territory.Aetherytes.Add(aetheryte);
        }
    }


    public static readonly (uint NodeId, uint AetheryteId)[] Aetherytes =
    {
        (834, 172), // Rime Dolomite -> Camp Broken Glass
        (771, 106), // Rarefied Shiitake Mushroom -> Onokoro
        (682, 148), // Purpure -> Macarenses Angle
        //(310, 104), // Rarefied Raw Triphane -> Rhalgr's Reach
        //(312, 111), // Rarefied Raw Star Spinel -> Kugane
    };

    public static void Apply(GameData data)
    {
        foreach (var (nodeId, aetheryteId) in Aetherytes)
        {
            if (!data.GatheringNodes.TryGetValue(nodeId, out var node))
            {
                data.Log.Error($"Could not find node {nodeId}.");
                continue;
            }

            if (!data.Aetherytes.TryGetValue(aetheryteId, out var aetheryte))
            {
                data.Log.Error($"Could not find aetheryte {aetheryteId}.");
                continue;
            }

            node.ClosestAetheryte = aetheryte;
            node.DefaultAetheryte = aetheryte;
        }
    }
}
