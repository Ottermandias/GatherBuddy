namespace GatherBuddy.Utility;

public static class Maps
{
    public static int MarkerToMap(double coord, double scale)
        => (int)(100.0 * (coord * 41.0 / 2048.0 / scale + 1.0) + 0.9);

    public static int NodeToMap(double coord, double scale)
        => (int)(100.0 * (41 * coord / scale / 2048 + 21.5) + 0.9);
}
