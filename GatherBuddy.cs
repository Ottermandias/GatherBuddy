using System;
using Dalamud.Game.Command;
using Dalamud.Plugin;
using Gathering;
using Otter;
using Serilog;

namespace GatherBuddyPlugin
{

    public class GatherBuddy : IDalamudPlugin
    {
        public string Name => "GatherBuddy Plugin";

        private DalamudPluginInterface   pluginInterface;
        public  Gatherer                 gatherer;
        private Otter.CommandManager     commandManager;
        private GatherBuddyConfiguration configuration;
        private Interface                gatherInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
            this.commandManager  = new Otter.CommandManager(pluginInterface, "GatherBuddy", Serilog.Events.LogEventLevel.Verbose);
            this.configuration   = pluginInterface.GetPluginConfig() as GatherBuddyConfiguration ?? new GatherBuddyConfiguration();
            this.gatherer        = new Gatherer(pluginInterface, configuration, commandManager);
            this.gatherInterface = new Interface(this, pluginInterface, configuration);

            this.pluginInterface.CommandManager.AddHandler("/gatherbuddy", new CommandInfo(OnGatherBuddy)
            {
                HelpMessage = "Use for settings. Use without arguments for interface.",
                ShowInHelp = true
            });

            this.pluginInterface.CommandManager.AddHandler("/gather", new CommandInfo(OnGather)
            {
                HelpMessage = "Mark the nearest node containing the item supplied, teleport to the nearest aetheryte, equip appropriate gear.",
                ShowInHelp = true
            });

            this.pluginInterface.CommandManager.AddHandler("/gathergroup", new CommandInfo(OnGatherGroup)
            {
                HelpMessage = "Teleport to the node of a group corresponding to current time. Use /gathergroup for more details.",
                ShowInHelp = true
            });

            this.pluginInterface.CommandManager.AddHandler("/gatherdebug", new CommandInfo(OnGatherDebug)
            {
                HelpMessage = "Dump some collected information.",
                ShowInHelp = false
            });

            pluginInterface.ClientState.TerritoryChanged += gatherer.OnTerritoryChange;
            pluginInterface.UiBuilder.OnBuildUi += gatherInterface.Draw;
            pluginInterface.UiBuilder.OnOpenConfigUi += OnConfigCommandHandler;

            if (configuration.DoRecord)
                gatherer.StartRecording();
        }

        public void Dispose()
        {
            pluginInterface.UiBuilder.OnOpenConfigUi -= OnConfigCommandHandler;
            pluginInterface.UiBuilder.OnBuildUi -= gatherInterface.Draw;
            pluginInterface.SavePluginConfig(configuration);
            this.gatherer.Dispose();
            pluginInterface.ClientState.TerritoryChanged -= gatherer.OnTerritoryChange;
            this.commandManager.RemoveHook();
            this.pluginInterface.CommandManager.RemoveHandler("/gatherdebug");
            this.pluginInterface.CommandManager.RemoveHandler("/gather");
            this.pluginInterface.CommandManager.RemoveHandler("/gathergroup");
            this.pluginInterface.CommandManager.RemoveHandler("/gatherbuddy");
            this.pluginInterface.Dispose();
        }

        private void OnGather(string command, string arguments)
        {
            if (arguments == null || arguments.Length == 0)
                pluginInterface.Framework.Gui.Chat.Print("Please supply a (partial) item name for /gather.");
            else
                gatherer.OnGatherAction(arguments);
        }

        private void OnConfigCommandHandler(object a, object b)
        {
            gatherInterface.Visible = true;
        }

        private void PrintHelp()
        {
            pluginInterface.Framework.Gui.Chat.Print("Please use with [setting] [value], where setting can be");
            pluginInterface.Framework.Gui.Chat.Print("        -- SwitchGear [0|off|false|1|on|true]: do change the gear set to the correct one for the node.");
            pluginInterface.Framework.Gui.Chat.Print("        -- Miner [string]: the name of your miner gear set of choice.");
            pluginInterface.Framework.Gui.Chat.Print("        -- Botanist [string]: the name of your botanist gear set of choice.");
            pluginInterface.Framework.Gui.Chat.Print("        -- Teleport [0|off|false|1|on|true]: Teleport to the nearest aetheryte to the node. Requires Teleporter plugin.");
            pluginInterface.Framework.Gui.Chat.Print("        -- SetFlag [0|off|false|1|on|true]: Set a map marker on the approximate location of the node. Requires ChatCoordinates plugin.");
            pluginInterface.Framework.Gui.Chat.Print("        -- Record [0|off|false|1|on|true]: Start recording encountered nodes for more accurate positions.");
            pluginInterface.Framework.Gui.Chat.Print("        -- Snapshot: Records currently visible nodes a single time for more accurate positions.");
        }

        private void OnGatherBuddy(string command, string arguments)
        {
            var chat = pluginInterface.Framework.Gui.Chat;
            var argumentParts = arguments.Split(new char[]{' '}, 2);

            if (argumentParts.Length == 0 || argumentParts[0].Length == 0)
            {
                gatherInterface.Visible = !gatherInterface.Visible;
                return;
            }

            string output = "";
            bool setting;
            if (Util.CompareCI(argumentParts[0], "snap") || Util.CompareCI(argumentParts[0], "snapshot"))
            {
                output = $"Recorded {gatherer.Snapshot()} new nearby gathering nodes.";
            }
            else if (argumentParts.Length < 2 || argumentParts[1].Length == 0)
            {
                PrintHelp();
                return;
            }
            else if (Util.CompareCI(argumentParts[0], "miner"))
            {
                string earlierName = configuration.MinerSetName;
                configuration.MinerSetName = argumentParts[1];
                output = $"Set the Gearset for Miner from '{earlierName}' to '{configuration.MinerSetName}'.";
            }
            else if (Util.CompareCI(argumentParts[0], "botanist"))
            {
                string earlierName = configuration.BotanistSetName;
                configuration.BotanistSetName = argumentParts[1];
                output = $"Set the Gearset for Botanist from '{earlierName}' to '{configuration.BotanistSetName}'.";
                
            }
            else if (Util.CompareCI(argumentParts[0], "switchgear"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    chat.Print("/gatherbuddy switchgear requires an argument of [0|off|false|1|on|true].");
                    return;
                }
                else
                {
                    var oldSetting = configuration.UseGearChange;
                    configuration.UseGearChange = setting;
                    output = $"Set the value of SwitchGear from {oldSetting} to {setting}.";
                }
            }
            else if (Util.CompareCI(argumentParts[0], "teleport"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    chat.Print("/gatherbuddy teleport requires an argument of [0|off|false|1|on|true].");
                    return;
                }
                else
                {
                    var oldSetting = configuration.UseTeleport;
                    configuration.UseTeleport = setting;
                    output = $"Set the value of Teleport from {oldSetting} to {setting}.";
                }
            }
            else if (Util.CompareCI(argumentParts[0], "setflag"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    chat.Print("/gatherbuddy SetFlag requires an argument of [0|off|false|1|on|true].");
                    return;
                }
                else
                {
                    var oldSetting = configuration.UseCoordinates;
                    configuration.UseCoordinates = setting;
                    output = $"Set the value of SetFlag from {oldSetting} to {setting}.";
                }
            }
            else if (Util.CompareCI(argumentParts[0], "record"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    chat.Print("/gatherbuddy record requires an argument of [0|off|false|1|on|true].");
                    return;
                }
                else
                {
                    var oldSetting = configuration.DoRecord;
                    configuration.DoRecord = setting;
                    output = $"Set the value of DoRecord from {oldSetting} to {setting}.";
                    if (setting != oldSetting)
                    {
                        if (setting)
                            gatherer.StartRecording();
                        else
                            gatherer.StopRecording();
                    }
                }
            }
            else
            {
                PrintHelp();
                return;
            }

            pluginInterface.SavePluginConfig(configuration);
            chat.Print(output);
            Log.Information($"[GatherBuddy] {output}");
        }

        private void OnGatherGroup(string command, string arguments)
        {
            var argumentParts = arguments.Split();
            if (argumentParts.Length == 0)
                gatherer.OnGroupGatherAction("", 0);
            else if (argumentParts.Length == 1)
                gatherer.OnGroupGatherAction(argumentParts[0], 0);
            else
            {
                if (Int32.TryParse(argumentParts[1], out int offset))
                    gatherer.OnGroupGatherAction(argumentParts[0], offset);
                else
                    gatherer.OnGroupGatherAction(argumentParts[0], 0);
            }
        }

        private void OnGatherDebug(string command, string arguments)
        {
            var argumentParts = arguments.Split();
            if (Util.CompareCI(argumentParts[0], "dump"))
            {
                if (Util.CompareCI(argumentParts[1], "aetherytes"))
                    gatherer.dumpAetherytes();
                else if (Util.CompareCI(argumentParts[1], "territories"))
                    gatherer.dumpTerritories();
                else if (Util.CompareCI(argumentParts[1], "items"))
                    gatherer.dumpItems();
                else if (Util.CompareCI(argumentParts[1], "nodes"))
                    gatherer.dumpNodes();
                else if (Util.CompareCI(argumentParts[1], "records"))
                    gatherer.PrintRecords();
            }
        }
    }
}