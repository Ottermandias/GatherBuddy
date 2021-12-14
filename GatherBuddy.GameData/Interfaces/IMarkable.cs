using GatherBuddy.Classes;

namespace GatherBuddy.Interfaces;

public interface IMarkable
{
    public const int CoordMin = 100;
    public const int CoordMax = 4200;

    public string    Name           { get; }
    public Territory Territory      { get; }
    public int       IntegralXCoord { get; set; }
    public int       IntegralYCoord { get; set; }

    public int DefaultXCoord { get; }
    public int DefaultYCoord { get; }


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
}
