namespace GatherBuddy.Data;

public static class HiddenMaps
{
    // @formatter:off
    public static readonly (uint MapId, uint[] NodeIds)[] Maps =
    {
        (6688,  [20, 49, 137, 140, 141, 180]),                                 // Leather
        (6689,  [46, 142, 143, 185, 186]),                                     // Goatskin
        (6690,  [198, 294, 197, 147, 199, 149, 189, 284, 210, 209, 150, 151]), // Toadskin
        (6691,  [198, 294, 197, 147, 199, 149, 189, 284, 210, 209, 150, 151]), // Boarskin
        (6692,  [198, 294, 197, 147, 199, 149, 189, 284, 210, 209, 150, 151]), // Peisteskin
        (12241, [295, 287, 297, 286, 298, 296, 288, 285]),                     // Archaeoskin
        (12242, [391, 356, 354, 358, 352, 359, 361, 360, 300, 351, 353, 355]), // Wyvernskin
        (12243, [391, 356, 354, 358, 352, 359, 361, 360, 300, 351, 353, 355]), // Dragonskin
        (17835, [514, 513, 517, 516, 519, 529, 493, 491, 495]),                // Gaganaskin
        (17836, [514, 513, 517, 516, 519, 529, 493, 491, 495]),                // Gazelleskin
        (26744, [621, 620, 625, 623, 596, 648, 598, 600, 602]),                // Gliderskin
        (26745, [621, 620, 625, 623, 596, 648, 598, 600, 602]),                // Zonureskin
        (36611, [847, 848, 825, 826]),                                         // Saigaskin
        (36612, [847, 848, 825, 826]),                                         // Kumbhiraskin
        (39591, [846, 844, 824, 823]),                                         // Ophiotauroskin
        (43556, [981, 983, 983, 985, 989, 990, 991, 1001, 1002, 1003]),        // Loboskin
        (43557, [981, 983, 983, 985, 989, 990, 991, 1001, 1002, 1003]),        // Br'aaxskin
    };
    // @formatter:on

    public static void Apply(GameData data)
    {
        foreach (var (map, nodes) in Maps)
        {
            if (!data.Gatherables.TryGetValue(map, out var mapItem))
            {
                data.Log.Error($"Could not find map item {map}.");
                continue;
            }

            foreach (var node in nodes)
            {
                if (!data.GatheringNodes.TryGetValue(node, out var nodeData))
                {
                    data.Log.Error($"Could not find gathering node {node}.");
                    continue;
                }

                nodeData.AddItem(mapItem);
            }
        }
    }
}
