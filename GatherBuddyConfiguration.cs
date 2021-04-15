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
        public const string DefaultIdentifiedItemFormat = "Identified [{Id}: {Name}] for \"{Input}\".";
        public const string DefaultIdentifiedFishFormat = "Identified [{Id}: {Name}] for \"{Input}\".";
        public const string DefaultIdentifiedFishingSpotFormat = "Chose fishing spot {Name} for {FishName}.";
        public const string DefaultAlarmFormat = "[GatherBuddy][Alarm {Name}]: The gathering node for {AllItems} {DelayString}.";

        public string BotanistSetName { get; set; }
        public string MinerSetName    { get; set; }
        public string FisherSetName   { get; set; }

        public string IdentifiedItemFormat        { get; set; }
        public string IdentifiedFishFormat        { get; set; }
        public string IdentifiedFishingSpotFormat { get; set; }
        public string AlarmFormat                 { get; set; }

        public int       Version   { get; set; }
        public ShowNodes ShowNodes { get; set; }

        public bool UseGearChange    { get; set; }
        public bool UseTeleport      { get; set; }
        public bool UseCoordinates   { get; set; }
        public bool DoRecord         { get; set; }
        public bool AlarmsEnabled    { get; set; }
        public bool PrintUptime      { get; set; }
        public bool PrintGighead     { get; set; }
        public bool ShowFishTimer    { get; set; }
        public bool FishTimerEdit    { get; set; }
        public bool HideUncaughtFish { get; set; }

        public bool ShowAlreadyCaught { get; set; }
        public bool ShowBigFish       { get; set; }
        public bool ShowSmallFish     { get; set; }
        public bool ShowSpearFish     { get; set; }
        public bool ShowAlwaysUp      { get; set; }
        public byte ShowFishFromPatch { get; set; }

        public Records     Records { get; set; }
        public List<Alarm> Alarms  { get; set; }

        public GatherBuddyConfiguration()
        {
            Version          = 2;
            BotanistSetName  = "BOT";
            MinerSetName     = "MIN";
            FisherSetName    = "FSH";
            UseGearChange    = true;
            UseTeleport      = true;
            UseCoordinates   = true;
            DoRecord         = true;
            AlarmsEnabled    = false;
            PrintUptime      = true;
            ShowFishTimer    = true;
            FishTimerEdit    = true;
            HideUncaughtFish = false;
            ShowNodes        = ShowNodes.AllNodes;
            Records          = new Records();
            Alarms           = new List<Alarm>();

            ShowAlreadyCaught = false;
            ShowBigFish       = true;
            ShowSmallFish     = true;
            ShowSpearFish     = true;
            ShowAlwaysUp      = true;
            ShowFishFromPatch = 0;

            IdentifiedItemFormat        = DefaultIdentifiedItemFormat;
            IdentifiedFishFormat        = DefaultIdentifiedFishFormat;
            IdentifiedFishingSpotFormat = DefaultIdentifiedFishingSpotFormat;
            AlarmFormat                 = DefaultAlarmFormat;
        }
    }
}
