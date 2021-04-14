using GatherBuddy.Game;

namespace GatherBuddy.Classes
{
    public class InitialNodePosition
    {
        public Aetheryte? ClosestAetheryte { get; set; }
        public bool       Prefer           { get; set; } = false;
        public int        XCoordIntegral = 0;
        public int        YCoordIntegral = 0;


        public double XCoord
            => XCoordIntegral / 100.0;

        public double YCoord
            => YCoordIntegral / 100.0;
    }
}
