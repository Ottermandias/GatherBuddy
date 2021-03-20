using System;
using System.Collections.Generic;
using Dalamud.Configuration;
using GatherBuddy.Classes;

namespace GatherBuddy
{
    [Flags]
    public enum ShowNodes
    {
        NoNodes  = 0,
        Mining   = 0x0001,
        Botanist = 0x0002,

        Ephemeral = 0x0004,
        Unspoiled = 0x0008,

        ARealmReborn   = 0x0010,
        Heavensward    = 0x0020,
        Stormblood     = 0x0040,
        Shadowbringers = 0x0080,
        Endwalker      = 0x0100,

        AllNodes = 0x01FF,
    }

    [Serializable]
    public class GatherBuddyConfiguration : IPluginConfiguration
    {
        public string    BotanistSetName { get; set; }
        public string    MinerSetName    { get; set; }
        public int       Version         { get; set; }
        public ShowNodes ShowNodes       { get; set; }

        public bool UseGearChange  { get; set; }
        public bool UseTeleport    { get; set; }
        public bool UseCoordinates { get; set; }
        public bool DoRecord       { get; set; }
        public bool AlarmsEnabled  { get; set; }

        public Records Records { get; set; }
        public List<Alarm>       Alarms  { get; set; }

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
            ShowNodes       = ShowNodes.AllNodes;
            Records         = new Records();
            Alarms          = new List<Alarm>();
        }
    }
}
