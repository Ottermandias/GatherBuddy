using System;
using System.Net.Http;
using System.Reflection;
using Dalamud;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
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

namespace GatherBuddy;

public partial class GatherBuddy : IDalamudPlugin
{
    public const string InternalName = "GatherBuddy";

    public string Name
        => InternalName;

    public static string Version = string.Empty;

    public static Configuration  Config   { get; private set; } = null!;
    public static GameData       GameData { get; private set; } = null!;
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
    public static SeTugType      TugType        { get; private set; } = null!;
    public static WaymarkManager WaymarkManager { get; private set; } = null!;

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
        Dalamud.Initialize(pluginInterface);
        Version        = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "";
        Config         = Configuration.Load();
        Language       = Dalamud.ClientState.ClientLanguage;
        GameData       = new GameData(Dalamud.GameData);
        Time           = new SeTime();
        WaymarkManager = new WaymarkManager();

        WeatherManager      = new WeatherManager(GameData);
        UptimeManager       = new UptimeManager(GameData);
        FishLog             = new FishLog(Dalamud.SigScanner, Dalamud.GameData);
        EventFramework      = new EventFramework(Dalamud.SigScanner);
        CurrentBait         = new CurrentBait(Dalamud.SigScanner);
        TugType             = new SeTugType(Dalamud.SigScanner);
        Executor            = new Executor(this);
        ContextMenu         = new ContextMenu(Executor);
        GatherGroupManager  = GatherGroup.GatherGroupManager.Load();
        LocationManager     = LocationManager.Load();
        AlarmManager        = AlarmManager.Load();
        GatherWindowManager = GatherWindowManager.Load(AlarmManager);
        AlarmManager.ForceEnable();

        InitializeCommands();

        FishRecorder = new FishRecorder();
        FishRecorder.Enable();
        WindowSystem = new WindowSystem(Name);
        Interface    = new Interface(this);
        WindowSystem.AddWindow(Interface);
        WindowSystem.AddWindow(new GatherWindow(this));
        WindowSystem.AddWindow(new FishTimerWindow(FishRecorder));
        WindowSystem.AddWindow(new SpearfishingHelper(GameData));
        Dalamud.PluginInterface.UiBuilder.Draw         += WindowSystem.Draw;
        Dalamud.PluginInterface.UiBuilder.OpenConfigUi += Interface.Toggle;

        Ipc = new GatherBuddyIpc(this);
        //Wotsit = new WotsitIpc();
    }

    void IDisposable.Dispose()
    {
        FishRecorder.Disable();
        ContextMenu.Dispose();
        UptimeManager.Dispose();
        Ipc.Dispose();
        //Wotsit.Dispose();
        Dalamud.PluginInterface.UiBuilder.OpenConfigUi -= Interface.Toggle;
        Dalamud.PluginInterface.UiBuilder.Draw         -= WindowSystem.Draw;
        Interface.Dispose();
        WindowSystem.RemoveAllWindows();
        DisposeCommands();
        Time.Dispose();
        Icons.DefaultStorage.Dispose();
        HttpClient.Dispose();
    }
}
