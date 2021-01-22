using System;
using Dalamud.Configuration;

namespace GatherBuddyPlugin
{
    [Serializable]
    public class GatherBuddyConfiguration : IPluginConfiguration
    {
        public string BotanistSetName{ get; set; }
        public string MinerSetName{ get; set; }
        public int Version { get; set; }
        public uint ShowNodes{ get; set; }

        public bool UseGearChange{ get; set; }
        public bool UseTeleport{ get; set; }
        public bool UseCoordinates{ get; set; }
        public bool DoRecord{ get; set; }

        public Gathering.Records Records{ get; set; }

        public GatherBuddyConfiguration()
        {
            Version         = 2;
            BotanistSetName = "Botanist";
            MinerSetName    = "Miner";
            UseGearChange   = true;
            UseTeleport     = true;
            UseCoordinates  = true;
            DoRecord        = true;
            ShowNodes       = 0;
            Records         = new Gathering.Records();
        }
    }
}
