using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GatherBuddy.Classes;

namespace GatherBuddy.Interfaces;

public interface IMarkable
{
    public const int    CoordMin   = 100;
    public const int    CoordMax   = 4200;
    public const int    MarkersMax = 8;
    public const ushort RadiusMax  = 400;

    public string    Name           { get; }
    public Territory Territory      { get; }
    public int       IntegralXCoord { get; set; }
    public int       IntegralYCoord { get; set; }

    public int   DefaultXCoord { get; }
    public int   DefaultYCoord { get; }

    public ushort Radius        { get; set; }
    public ushort DefaultRadius { get; }

    public Vector3[] Markers { get; set; }

    public bool SetMarkers(IEnumerable<Vector3> markers)
    {
        var tmpMarkers = markers.Take(MarkersMax).ToArray();
        if (Markers.SequenceEqual(tmpMarkers))
            return false;

        Markers = tmpMarkers;
        return true;
    }

    public bool SetXCoord(int xCoord)
    {
        if (xCoord is < CoordMin or > CoordMax)
            return false;

        IntegralXCoord = xCoord;
        return true;
    }

    public bool SetYCoord(int yCoord)
    {
        if (yCoord is < CoordMin or > CoordMax)
            return false;

        IntegralYCoord = yCoord;
        return true;
    }

    public bool SetRadius(ushort radius)
    {
        if (radius is 0 or > RadiusMax)
            return false;

        Radius = radius;
        return true;
    }
}
