using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace GatherBuddy;

public class Dalamud
{
    public static void Initialize(IDalamudPluginInterface pluginInterface)
        => pluginInterface.Create<Dalamud>();

    // @formatter:off
    [PluginService] public static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] public static ICommandManager         Commands        { get; private set; } = null!;
    [PluginService] public static ISigScanner             SigScanner      { get; private set; } = null!;
    [PluginService] public static IDataManager            GameData        { get; private set; } = null!;
    [PluginService] public static IClientState            ClientState     { get; private set; } = null!;
    [PluginService] public static IChatGui                Chat            { get; private set; } = null!;
    [PluginService] public static IFramework              Framework       { get; private set; } = null!;
    [PluginService] public static ICondition              Conditions      { get; private set; } = null!;
    [PluginService] public static IKeyState               Keys            { get; private set; } = null!;
    [PluginService] public static IGameGui                GameGui         { get; private set; } = null!;
    [PluginService] public static ITargetManager          Targets         { get; private set; } = null!;
    [PluginService] public static IGameInteropProvider    Interop         { get; private set; } = null!;
    [PluginService] public static ITextureProvider        Textures        { get; private set; } = null!;
    [PluginService] public static IContextMenu            ContextMenu     { get; private set; } = null!;
    // @formatter:on
}
