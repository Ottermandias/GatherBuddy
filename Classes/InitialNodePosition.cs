namespace GatherBuddy.Classes
{
    public class InitialNodePosition
    {
        public Aetheryte? ClosestAetheryte { get; set; }
        public bool       Prefer           { get; set; } = false;
        public int        xCoordIntegral = 0;
        public int        yCoordIntegral = 0;


        public double XCoord
            => xCoordIntegral / 100.0;

        public double YCoord
            => yCoordIntegral / 100.0;
    }
}
