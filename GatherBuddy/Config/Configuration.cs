using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Dalamud.Configuration;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.Text;
using GatherBuddy.Alarms;
using GatherBuddy.AutoGather;
using GatherBuddy.Enums;
using OtterGui.Classes;

namespace GatherBuddy.Config;

public partial class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 7;

    // Set Names
    public string BotanistSetName { get; set; } = "Botanist";
    public string MinerSetName    { get; set; } = "Miner";
    public string FisherSetName   { get; set; } = "Fisher";

    // formats
    public string IdentifiedGatherableFormat { get; set; } = DefaultIdentifiedGatherableFormat;
    public string AlarmFormat                { get; set; } = DefaultAlarmFormat;


    // Interface
    public AetherytePreference AetherytePreference { get; set; } = AetherytePreference.Distance;
    public ItemFilter          ShowItems           { get; set; } = ItemFilter.All;
    public FishFilter          ShowFish            { get; set; } = FishFilter.All;
    public PatchFlag           HideFishPatch       { get; set; } = 0;
    public JobFlags            LocationFilter      { get; set; } = (JobFlags)0x3F;


    // General Config
    public bool             OpenOnStart               { get; set; } = false;
    public bool             MainWindowLockPosition    { get; set; } = false;
    public bool             MainWindowLockResize      { get; set; } = false;
    public bool             CloseOnEscape             { get; set; } = true;
    public bool             UseGearChange             { get; set; } = true;
    public bool             UseTeleport               { get; set; } = true;
    public bool             UseCoordinates            { get; set; } = true;
    public bool             UseFlag                   { get; set; } = true;
    public bool             WriteCoordinates          { get; set; } = true;
    public bool             PrintUptime               { get; set; } = true;
    public bool             SkipTeleportIfClose       { get; set; } = true;
    public XivChatType      ChatTypeMessage           { get; set; } = XivChatType.Echo;
    public XivChatType      ChatTypeError             { get; set; } = XivChatType.ErrorMessage;
    public bool             AddIngameContextMenus     { get; set; } = true;
    public bool             StoreFishRecords          { get; set; } = true;
    public bool             UseUnixTimeFishRecords    { get; set; } = true;
    public bool             PrintClipboardMessages    { get; set; } = true;
    public bool             HideClippy                { get; set; } = false;
    public bool             ShowStatusLine            { get; set; } = true;
    public ModifiableHotkey MainInterfaceHotkey       { get; set; } = new();
    public bool             PlaceCustomWaymarks       { get; set; } = true;
    public GatheringType    PreferredGatheringType    { get; set; } = GatheringType.Multiple;

    // AutoGather Config
    public AutoGatherConfig AutoGatherConfig              { get; set; } = new();
    public float            AutoGatherListSelectorWidth { get; set; } = 225f;

    // Weather tab
    public bool ShowWeatherNames { get; set; } = true;

    // Alarms
    public bool   AlarmsEnabled          { get; set; } = false;
    public bool   AlarmsInDuty           { get; set; } = true;
    public bool   AlarmsOnlyWhenLoggedIn { get; set; } = false;
    public Sounds WeatherAlarm           { get; set; } = Sounds.None;
    public Sounds HourAlarm              { get; set; } = Sounds.None;

    // Colors
    public Dictionary<ColorId, uint> Colors { get; set; }
        = Enum.GetValues<ColorId>().ToDictionary(c => c, c => c.Data().DefaultColor);

    public int SeColorNames     = DefaultSeColorNames;
    public int SeColorCommands  = DefaultSeColorCommands;
    public int SeColorArguments = DefaultSeColorArguments;
    public int SeColorAlarm     = DefaultSeColorAlarm;

    // Fish Timer
    public bool   ShowFishTimer           { get; set; } = true;
    public bool   FishTimerEdit           { get; set; } = true;
    public bool   FishTimerClickthrough   { get; set; } = false;
    public bool   HideUncaughtFish        { get; set; } = false;
    public bool   HideUnavailableFish     { get; set; } = false;
    public bool   ShowFishTimerUptimes    { get; set; } = true;
    public bool   HideFishSizePopup       { get; set; } = false;
    public ushort FishTimerScale          { get; set; } = 40000;
    public byte   ShowSecondIntervals     { get; set; } = 7;
    public int    SecondIntervalsRounding { get; set; } = 1;
    public bool   ShowCollectableHints    { get; set; } = true;
    public bool   ShowMultiHookHints      { get; set; } = true;
    
    // Fish Stats Tab
    public bool EnableFishStats       { get; set; } = false;
    public bool EnableReportTime      { get; set; } = true;
    public bool EnableReportSize      { get; set; } = true;
    public bool EnableReportMulti     { get; set; } = true;
    public bool EnableFishStatsGraphs { get; set; } = false;
    public int  FishStatsSelectedIdx  { get; set; }

    // Spearfish Helper
    public bool ShowSpearfishHelper          { get; set; } = true;
    public bool ShowSpearfishNames           { get; set; } = true;
    public bool ShowAvailableSpearfish       { get; set; } = true;
    public bool ShowSpearfishSpeed           { get; set; } = false;
    public bool ShowSpearfishCenterLine      { get; set; } = true;
    public bool ShowSpearfishListIconsAsText { get; set; } = false;
    public bool FixNamesOnPosition           { get; set; } = false;
    public byte FixNamesPercentage           { get; set; } = 55;


    // Gather Window
    public bool             ShowGatherWindow               { get; set; } = true;
    public bool             ShowGatherWindowTimers         { get; set; } = true;
    public bool             ShowGatherWindowAlarms         { get; set; } = true;
    public bool             SortGatherWindowByUptime       { get; set; } = false;
    public bool             ShowGatherWindowOnlyAvailable  { get; set; } = false;
    public bool             HideGatherWindowCompletedItems { get; set; } = false;
    public bool             HideGatherWindowInDuty         { get; set; } = true;
    public bool             OnlyShowGatherWindowHoldingKey { get; set; } = false;
    public bool             LockGatherWindow               { get; set; } = false;
    public bool             GatherWindowBottomAnchor       { get; set; } = false;
    public ModifiableHotkey GatherWindowHotkey             { get; set; } = new(VirtualKey.G, VirtualKey.CONTROL);
    public ModifierHotkey   GatherWindowDeleteModifier     { get; set; } = VirtualKey.CONTROL;
    public VirtualKey       GatherWindowHoldKey            { get; set; } = VirtualKey.MENU;

    public void Save()
        => Dalamud.PluginInterface.SavePluginConfig(this);


    // Add missing colors to the dictionary if necessary.
    private void AddColors()
    {
        var save = false;
        foreach (var color in Enum.GetValues<ColorId>())
            save |= Colors.TryAdd(color, color.Data().DefaultColor);
        if (save)
            Save();
    }

    public static Configuration Load()
    {
        try
        {
            if (Dalamud.PluginInterface.GetPluginConfig() is Configuration config)
            {
                config.AddColors();
                config.Migrate4To5();
                config.Migrate5To6();
                config.Migrate6To7();
                return config;
            }
        }
        catch (Exception ex)
        {
            GatherBuddy.Log.Error($"Failed to load configuration, creating new one: {ex}");
            try
            {
                var configPath = Dalamud.PluginInterface.ConfigFile.FullName;
                var backupPath = $"{configPath}.backup_{DateTime.Now:yyyyMMdd_HHmmss}";
                if (File.Exists(configPath))
                {
                    File.Copy(configPath, backupPath);
                    GatherBuddy.Log.Warning($"Corrupted config backed up to: {backupPath}");
                }
            }
            catch (Exception backupEx)
            {
                GatherBuddy.Log.Error($"Failed to backup corrupted config: {backupEx}");
            }
        }

        var newConfig = new Configuration();
        newConfig.Save();
        return newConfig;
    }

    public void Migrate4To5()
    {
        if (Version >= 5)
            return;

        ShowFish |= FishFilter.Collectible | FishFilter.NotCollectible;
        Version  =  5;
        Save();
    }

    public void Migrate5To6()
    {
        if (Version >= 6)
            return;

        ShowItems |= ItemFilter.Dawntrail;
        Version   =  6;
        Save();
    }

    public void Migrate6To7()
    {
        if (Version >= 7)
            return;

        if (AutoGatherConfig == null)
        {
            AutoGatherConfig = new();
        }

        Version = 7;
        Save();
    }
}
