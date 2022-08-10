namespace GatherBuddy.Utility;

public static class Maps
{
    public static int MarkerToMap(double coord, double scale)
        => (int)(2 * coord / scale + 100.9);

    public static int NodeToMap(double coord, double scale)
        => (int)(2 * coord + 2048 / scale + 100.9);
}
