namespace GatherBuddy.Enums
{
    public enum GatheringType
    {
        Mining       = 0,
        Quarrying    = 1,
        Logging      = 2,
        Harvesting   = 3,
        Spearfishing = 4,
        Botanist     = 5,
        Miner        = 6,
        Fisher       = 7,
    };

    public static class GatheringTypeExtension
    {
        public static GatheringType ToGroup(this GatheringType type)
        {
            return type switch
            {
                GatheringType.Mining       => GatheringType.Miner,
                GatheringType.Quarrying    => GatheringType.Miner,
                GatheringType.Miner        => GatheringType.Miner,
                GatheringType.Logging      => GatheringType.Botanist,
                GatheringType.Harvesting   => GatheringType.Botanist,
                GatheringType.Botanist     => GatheringType.Botanist,
                GatheringType.Spearfishing => GatheringType.Fisher,
                _                          => type,
            };
        }
    }
}
