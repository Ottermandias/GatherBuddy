using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Dalamud;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using GatherBuddy.Alarms;
using GatherBuddy.Config;
using GatherBuddy.CustomInfo;
using GatherBuddy.FishTimer;
using GatherBuddy.GatherHelper;
using GatherBuddy.Gui;
using GatherBuddy.Plugin;
using GatherBuddy.SeFunctions;
using GatherBuddy.Spearfishing;
using GatherBuddy.Weather;
using OtterGui.Classes;
using OtterGui.Log;
using ECommons;

namespace GatherBuddy;

public partial class GatherBuddy : IDalamudPlugin
{
    public const string InternalName = "GatherBuddy";

    public string Name
        => InternalName;

    public static string Version = string.Empty;

    public static Configuration  Config   { get; private set; } = null!;
    public static GameData       GameData { get; private set; } = null!;
    public static Logger         Log      { get; private set; } = null!;
    public static ClientLanguage Language { get; private set; } = ClientLanguage.English;
    public static SeTime         Time     { get; private set; } = null!;
#if DEBUG
    public static bool DebugMode { get; private set; } = true;
#else
    public static bool DebugMode { get; private set; } = false;
#endif

    public static readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromMilliseconds(1500),
    };

    public static WeatherManager WeatherManager { get; private set; } = null!;
    public static UptimeManager  UptimeManager  { get; private set; } = null!;
    public static FishLog        FishLog        { get; private set; } = null!;
    public static EventFramework EventFramework { get; private set; } = null!;
    public static CurrentBait    CurrentBait    { get; private set; } = null!;
    public static CurrentWeather CurrentWeather { get; private set; } = null!;
    public static SeTugType      TugType        { get; private set; } = null!;
    public static WaymarkManager WaymarkManager { get; private set; } = null!;
    public static AutoGather AutoGather { get; private set; } = null!;


    internal readonly GatherGroup.GatherGroupManager GatherGroupManager;
    internal readonly LocationManager                LocationManager;
    internal readonly AlarmManager                   AlarmManager;
    internal readonly GatherWindowManager            GatherWindowManager;
    internal readonly WindowSystem                   WindowSystem;
    internal readonly Interface                      Interface;
    internal readonly Executor                       Executor;
    internal readonly ContextMenu                    ContextMenu;
    internal readonly FishRecorder                   FishRecorder;

    internal readonly GatherBuddyIpc Ipc;
    //    internal readonly WotsitIpc Wotsit;

    public GatherBuddy(DalamudPluginInterface pluginInterface)
    {
        try
        {
            Dalamud.Initialize(pluginInterface);
            ECommonsMain.Init(pluginInterface, this);
            Icons.InitDefaultStorage(Dalamud.Textures);
            Log     = new Logger();
            Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "";
            Backup.CreateAutomaticBackup(Log, pluginInterface.ConfigDirectory, GatherBuddyBackupFiles());
            Config         = Configuration.Load();
            Language       = Dalamud.ClientState.ClientLanguage;
            GameData       = new GameData(Dalamud.GameData, Log, WorldData.WorldLocationsByNodeId);
            Time           = new SeTime();
            WaymarkManager = new WaymarkManager();

            WeatherManager      = new WeatherManager(GameData);
            UptimeManager       = new UptimeManager(GameData);
            FishLog             = new FishLog(Dalamud.SigScanner, Dalamud.GameData);
            EventFramework      = new EventFramework(Dalamud.SigScanner);
            CurrentBait         = new CurrentBait(Dalamud.SigScanner);
            CurrentWeather      = new CurrentWeather(Dalamud.SigScanner);
            TugType             = new SeTugType(Dalamud.SigScanner);
            Executor            = new Executor(this);
            ContextMenu         = new ContextMenu(Dalamud.ContextMenu, Executor);
            GatherGroupManager  = GatherGroup.GatherGroupManager.Load();
            LocationManager     = LocationManager.Load();
            AlarmManager        = AlarmManager.Load();
            GatherWindowManager = GatherWindowManager.Load(AlarmManager);
            AutoGather = new AutoGather(this);
            AlarmManager.ForceEnable();

            InitializeCommands();

            FishRecorder = new FishRecorder(Dalamud.Interop);
            FishRecorder.Enable();
            WindowSystem = new WindowSystem(Name);
            Interface    = new Interface(this);
            WindowSystem.AddWindow(Interface);
            WindowSystem.AddWindow(new GatherWindow(this));
            WindowSystem.AddWindow(new FishTimerWindow(FishRecorder));
            WindowSystem.AddWindow(new SpearfishingHelper(GameData));
            Dalamud.PluginInterface.UiBuilder.Draw         += WindowSystem.Draw;
            Dalamud.PluginInterface.UiBuilder.OpenConfigUi += Interface.Toggle;
            Dalamud.Framework.Update += Update;

            Ipc = new GatherBuddyIpc(this);
            //Wotsit = new WotsitIpc();
        }
        catch
        {
            ((IDisposable)this).Dispose();
            throw;
        }
    }

    private void Update(IFramework framework)
    {
        var objs = Dalamud.ObjectTable.Where(o => o.ObjectKind == ObjectKind.GatheringPoint);
        foreach (var obj in objs)
            WorldData.AddLocation(obj.DataId, obj.Position);
        AutoGather.DoAutoGather();
    }

    void IDisposable.Dispose()
    {
        FishRecorder?.Dispose();
        ContextMenu?.Dispose();
        UptimeManager?.Dispose();
        Ipc?.Dispose();
        //Wotsit?.Dispose();
        if (Interface != null)
            Dalamud.PluginInterface.UiBuilder.OpenConfigUi -= Interface.Toggle;
        if (WindowSystem != null)
            Dalamud.PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;
        Interface?.Dispose();
        WindowSystem?.RemoveAllWindows();
        DisposeCommands();
        Time?.Dispose();
        Icons.DefaultStorage?.Dispose();
        HttpClient?.Dispose();
        ECommonsMain.Dispose();
    }

    // Collect all relevant files for GatherBuddy configuration
    private static IReadOnlyList<FileInfo> GatherBuddyBackupFiles()
    {
        var list = Directory.Exists(Dalamud.PluginInterface.GetPluginConfigDirectory())
            ? Dalamud.PluginInterface.ConfigDirectory.EnumerateFiles("*.*").ToList()
            : new List<FileInfo>();
        list.Add(Dalamud.PluginInterface.ConfigFile);
        return list;
    }
}
