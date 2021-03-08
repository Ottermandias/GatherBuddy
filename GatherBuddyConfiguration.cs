using System;
using System.Collections.Generic;
using Dalamud.Configuration;
using Gathering;

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
        public bool AlarmsEnabled { get; set; }

        public Gathering.Records Records{ get; set; }
        public List<Alarm> Alarms { get; set; }

        public GatherBuddyConfiguration()
        {
            Version         = 2;
            BotanistSetName = "BOT";
            MinerSetName    = "MIN";
            UseGearChange   = true;
            UseTeleport     = true;
            UseCoordinates  = true;
            DoRecord        = true;
            AlarmsEnabled   = false;
            ShowNodes       = 0b11111111;
            Records         = new Gathering.Records();
            Alarms = new();
        }
    }
}
