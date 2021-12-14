namespace GatherBuddy.Utility;

public static class Math
{
    public static int SquaredDistance(int x1, int y1, int x2, int y2)
    {
        x1 -= x2;
        y1 -= y2;
        return x1 * x1 + y1 * y1;
    }

    public static int SquaredDistance(int x1, int y1, int z1, int x2, int y2, int z2)
    {
        x1 -= x2;
        y1 -= y2;
        z1 -= z2;
        return x1 * x1 + y1 * y1 + z1 * z1;
    }
}
