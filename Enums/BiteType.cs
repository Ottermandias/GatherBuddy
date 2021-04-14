namespace GatherBuddy.Enums
{
    public enum BiteType : byte
    {
        Unknown,
        Weak,
        Strong,
        Legendary,
    }

    internal static class BiteTypeExtension
    {
        public static BiteType FromBiteTime(long time)
        {
            return time switch
            {
                < 8000  => BiteType.Weak,
                < 10700 => BiteType.Strong,
                < 20000 => BiteType.Legendary,
                _       => BiteType.Unknown,
            };
        }
    }
}
