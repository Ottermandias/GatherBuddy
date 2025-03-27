using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Interfaces;
using GatherBuddy.Time;

namespace GatherBuddy.GatherGroup;

public static class GroupData
{
    private static TimedGroup.Config CreateDefaultGroup(string name, string description, params (uint, int)[] items)
        => new()
        {
            Name        = name,
            Description = description,
            Nodes = items.Concat(items.Select(i => (i.Item1, i.Item2 + 12)))
                .Select(i => new TimedGroupNode.Config()
                {
                    Type        = ObjectType.Gatherable,
                    ItemId      = i.Item1,
                    StartMinute = (short)(i.Item2 * RealTime.MinutesPerHour),
                    EndMinute   = (short)((i.Item2 + 2) * RealTime.MinutesPerHour % RealTime.MinutesPerDay),
                    Annotation  = string.Empty,
                }).ToArray(),
        };

    private static TimedGroup.Config CreateEphemeralGroup(string name, string description, params (uint, int)[] items)
        => new()
        {
            Name        = name,
            Description = description,
            Nodes = items
                .Select(i => new TimedGroupNode.Config()
                {
                    Type        = ObjectType.Gatherable,
                    ItemId      = i.Item1,
                    StartMinute = (short)(i.Item2 * RealTime.MinutesPerHour),
                    EndMinute   = (short)((i.Item2 + 4) * RealTime.MinutesPerHour % RealTime.MinutesPerDay),
                    Annotation  = string.Empty,
                }).ToArray(),
        };

    private static TimedGroup.Config CreateDarkMatterGroup()
        => new()
        {
            Name        = "darkmatter",
            Description = "Collects all nodes that provide dark matter clusters.",
            Nodes = new (uint, short, short)[]
            {
                (215, 0, 80),
                (250, 80, 103),
                (222, 103, 126),
                (216, 126, 149),
                (340, 149, 172),
                (233, 172, 195),
                (242, 195, 218),
                (246, 218, 241),
                (230, 241, 264),
                (236, 264, 287),
                (248, 287, 310),
                (247, 310, 333),
                (237, 333, 356),
                (217, 356, 379),
                (241, 379, 402),
                (226, 402, 425),
                (215, 425, 448),
                (219, 448, 471),
                (240, 471, 494),
                (235, 494, 517),
                (245, 517, 540),
                (232, 540, 565),
                (234, 565, 590),
                (215, 590, 616),
                (231, 616, 642),
                (223, 642, 668),
                (211, 668, 694),
                (214, 694, 720),
                (249, 720, 780),
                (221, 780, 816),
                (227, 816, 852),
                (220, 852, 888),
                (212, 888, 924),
                (340, 924, 960),
                (213, 960, 1050),
                (252, 1050, 1085),
                (227, 1085, 1155),
                (224, 1155, 1190),
                (238, 1190, 1225),
                (239, 1225, 1260),
                (218, 1260, 1320),
                (225, 1320, 1380),
                (227, 1380, 0),
            }.Select(n => new TimedGroupNode.Config()
            {
                Type           = ObjectType.Gatherable,
                ItemId         = 10335,
                StartMinute    = n.Item2,
                EndMinute      = n.Item3,
                Annotation     = string.Empty,
                PreferLocation = n.Item1,
            }).ToArray(),
        };

    // @formatter:off
    internal static readonly List<TimedGroup.Config> DefaultGroups = 
    [
        CreateDefaultGroup("80***", "Contains exarchic crafting nodes.",                    (32954, 0), (32952, 2), (32955, 4), (32950, 6), (32951, 8), (32953, 10)),
        CreateDefaultGroup("80**",  "Contains neo-ishgardian / aesthete crafting nodes.",   (30485, 0), (30486, 2), (29974, 4), (29970, 6), (29972, 8), (29976, 10)),
        CreateDefaultGroup("90*",   "Contains classical crafting nodes.",                   (36216, 0), (36167, 2), (36179, 4), (36195, 6), (36207, 8), (36217, 10)),
        CreateDefaultGroup("90**",  "Contains Rinascita crafting nodes.",                   (37819, 0), (37818, 2), (37821, 4), (37817, 6), (37820, 8), (37822, 10)),
        CreateDefaultGroup("90***", "Contains Diadochos crafting nodes.",                   (39710, 0), (39705, 2), (39706, 4), (39708, 6), (39707, 8), (39709, 10)),
        CreateDefaultGroup("100*", "Contains Archeo Kingdom crafting nodes.",               (44137, 0), (44140, 2), (44138, 4), (44135, 6), (44139, 8), (44136, 10)),
        CreateDefaultGroup("100**", "Contains Ceremonial crafting nodes.",                  (45968, 0), (45973, 2), (45971, 4), (45972, 6), (45970, 8), (45969, 10)),
        CreateDefaultGroup("100purple", "Contains level 100 purple scrip nodes.",           (43926, 0), (43927, 2), (44233, 4), (44234, 6), (43920, 8), (43919, 10)),
        CreateDefaultGroup("100orange", "Contains level 100 orange scrip nodes.",           (43923, 0), (43928, 2), (43922, 4), (43929, 6), (43921, 8), (43930, 10)),
        CreateDarkMatterGroup(),
        CreateEphemeralGroup("90sand", "Contains the nodes for the group of 90 aethersands.", (36285, 0), (36287, 8), (36288, 12), (36286, 20), (36287, 4), (36286, 16)), // last two nodes are temporary because there is nothing there
    ];
    // @formatter:on
}
