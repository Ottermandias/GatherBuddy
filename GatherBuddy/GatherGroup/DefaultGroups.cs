using System;
using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Classes;
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
                    StartMinute = i.Item2 * RealTime.MinutesPerHour,
                    EndMinute   = (i.Item2 + 2) * RealTime.MinutesPerHour % RealTime.MinutesPerDay,
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
                    StartMinute = i.Item2 * RealTime.MinutesPerHour,
                    EndMinute   = (i.Item2 + 4) * RealTime.MinutesPerHour % RealTime.MinutesPerDay,
                    Annotation  = string.Empty,
                }).ToArray(),
        };

    // @formatter:off
    internal static List<TimedGroup.Config> DefaultGroups = new()
    {
        CreateDefaultGroup("80***", "Contains exarchic crafting nodes.",                    (32954, 0), (32952, 2), (32955, 4), (32950, 6), (32951, 8), (32953, 10)),
        CreateDefaultGroup("80**",  "Contains neo-ishgardian / aesthete crafting nodes.",   (30485, 0), (30486, 2), (29974, 4), (29970, 6), (29972, 8), (29976, 10)),
        CreateDefaultGroup("90*",   "Contains classical crafting nodes.",                   (36216, 0), (36167, 2), (36179, 4), (36195, 6), (36207, 8), (36217, 10)),

        CreateEphemeralGroup("90sand", "Contains the nodes for the group of 90 aethersands.", (36285, 0), (36287, 8), (36288, 12), (36286, 20), (36287, 4), (36286, 16)) // last two nodes are temporary because there is nothing there
    };
    // @formatter:on
}
