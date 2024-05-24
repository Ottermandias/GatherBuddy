using System;
using System.Collections.Generic;
using System.Numerics;

namespace GatherBuddy.Classes;

public partial class GatheringNode
{
    public Territory  Territory        { get; init; }
    public Aetheryte? ClosestAetheryte { get; set; }

    public int IntegralXCoord { get; set; }
    public int IntegralYCoord { get; set; }

    public Aetheryte? DefaultAetheryte { get; internal set; }
    public int        DefaultXCoord    { get; internal set; }
    public int        DefaultYCoord    { get; internal set; }

    public ushort Radius        { get; set; }
    public ushort DefaultRadius { get; internal set; }
    public Dictionary<uint, List<Vector3>> WorldPositions { get; internal set; } = new();
}
