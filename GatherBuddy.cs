using System.Linq;
using Dalamud;
using Dalamud.Game.Command;
using Dalamud.Plugin;
using GatherBuddy.Gui;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
using GatheringType = GatherBuddy.Enums.GatheringType;
using Util = GatherBuddy.Utility.Util;

namespace GatherBuddy
{
    public class GatherBuddy : IDalamudPlugin
    {
        public string Name
            => "GatherBuddy";

        private DalamudPluginInterface?   _pluginInterface;
        public  Gatherer?                 Gatherer { get; set; }
        public  AlarmManager?             Alarms   { get; set; }
        private Managers.CommandManager?  _commandManager;
        private GatherBuddyConfiguration? _configuration;
        private Interface?                _gatherInterface;
        private FishingTimer?             _fishingTimer;

        public static ClientLanguage Language;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            _pluginInterface = pluginInterface;
            Language         = _pluginInterface.ClientState.ClientLanguage;
            Service<DalamudPluginInterface>.Set(_pluginInterface);
            _commandManager  = new Managers.CommandManager(pluginInterface);
            _configuration   = pluginInterface.GetPluginConfig() as GatherBuddyConfiguration ?? new GatherBuddyConfiguration();
            Gatherer         = new Gatherer(pluginInterface, _configuration, _commandManager);
            Alarms           = Gatherer.Alarms;
            _gatherInterface = new Interface(this, pluginInterface, _configuration);
            _fishingTimer    = new FishingTimer(_pluginInterface, _configuration, Gatherer!.FishManager);

            if (!Gatherer!.FishManager.GetSaveFileName(_pluginInterface).Exists)
                Gatherer!.FishManager.SaveFishRecords(_pluginInterface);
            else
                Gatherer!.FishManager.LoadFishRecords(_pluginInterface);

            _pluginInterface!.CommandManager.AddHandler("/gatherbuddy", new CommandInfo(OnGatherBuddy)
            {
                HelpMessage = "Use for settings. Use without arguments for interface.",
                ShowInHelp  = true,
            });

            _pluginInterface!.CommandManager.AddHandler("/gather", new CommandInfo(OnGather)
            {
                HelpMessage = "Mark the nearest node containing the item supplied, teleport to the nearest aetheryte, equip appropriate gear.",
                ShowInHelp  = true,
            });

            _pluginInterface!.CommandManager.AddHandler("/gatherbot", new CommandInfo(OnGatherBot)
            {
                HelpMessage =
                    "Mark the nearest botanist node containing the item supplied, teleport to the nearest aetheryte, equip appropriate gear.",
                ShowInHelp = true,
            });

            _pluginInterface!.CommandManager.AddHandler("/gathermin", new CommandInfo(OnGatherMin)
            {
                HelpMessage =
                    "Mark the nearest miner node containing the item supplied, teleport to the nearest aetheryte, equip appropriate gear.",
                ShowInHelp = true,
            });

            _pluginInterface!.CommandManager.AddHandler("/gatherfish", new CommandInfo(OnGatherFish)
            {
                HelpMessage =
                    "Mark the nearest fishing spot containing the fish supplied, teleport to the nearest aetheryte and equip fishing gear.",
                ShowInHelp = true,
            });

            _pluginInterface!.CommandManager.AddHandler("/gathergroup", new CommandInfo(OnGatherGroup)
            {
                HelpMessage = "Teleport to the node of a group corresponding to current time. Use /gathergroup for more details.",
                ShowInHelp  = true,
            });

            _pluginInterface!.CommandManager.AddHandler("/gatherdebug", new CommandInfo(OnGatherDebug)
            {
                HelpMessage = "Dump some collected information.",
                ShowInHelp  = false,
            });

            pluginInterface.ClientState.TerritoryChanged += Gatherer!.OnTerritoryChange;
            pluginInterface.UiBuilder.OnBuildUi          += _gatherInterface!.Draw;
            pluginInterface.UiBuilder.OnOpenConfigUi     += OnConfigCommandHandler;

            if (_configuration!.DoRecord)
                Gatherer.StartRecording();

            if (_configuration.AlarmsEnabled)
                Alarms!.Enable(true);
        }

        public void Dispose()
        {
            _gatherInterface?.Dispose();
            _pluginInterface!.UiBuilder.OnOpenConfigUi -= OnConfigCommandHandler;
            _pluginInterface!.UiBuilder.OnBuildUi      -= _gatherInterface!.Draw;
            _pluginInterface!.SavePluginConfig(_configuration);
            _pluginInterface.ClientState.TerritoryChanged -= Gatherer!.OnTerritoryChange;
            _fishingTimer?.Dispose();
            Gatherer!.Dispose();
            _pluginInterface.CommandManager.RemoveHandler("/gatherdebug");
            _pluginInterface.CommandManager.RemoveHandler("/gather");
            _pluginInterface.CommandManager.RemoveHandler("/gatherbot");
            _pluginInterface.CommandManager.RemoveHandler("/gathermin");
            _pluginInterface.CommandManager.RemoveHandler("/gatherfish");
            _pluginInterface.CommandManager.RemoveHandler("/gathergroup");
            _pluginInterface.CommandManager.RemoveHandler("/gatherbuddy");
            _pluginInterface.Dispose();
        }

        private void OnGather(string command, string arguments)
        {
            if (arguments.Length == 0)
                _pluginInterface!.Framework.Gui.Chat.Print("Please supply a (partial) item name for /gather.");
            else
                Gatherer!.OnGatherAction(arguments);
        }

        private void OnGatherBot(string command, string arguments)
        {
            if (arguments.Length == 0)
                _pluginInterface!.Framework.Gui.Chat.Print("Please supply a (partial) item name for /gatherbot.");
            else
                Gatherer!.OnGatherAction(arguments, GatheringType.Botanist);
        }

        private void OnGatherMin(string command, string arguments)
        {
            if (arguments.Length == 0)
                _pluginInterface!.Framework.Gui.Chat.Print("Please supply a (partial) item name for /gathermin.");
            else
                Gatherer!.OnGatherAction(arguments, GatheringType.Miner);
        }

        private void OnGatherFish(string command, string arguments)
        {
            if (arguments.Length == 0)
                _pluginInterface!.Framework.Gui.Chat.Print("Please supply a (partial) fish name for /gatherfish.");
            else
                Gatherer!.OnFishAction(arguments);
        }

        private void OnConfigCommandHandler(object a, object b)
            => _gatherInterface!.Visible = true;

        private void PrintHelp()
        {
            _pluginInterface!.Framework.Gui.Chat.Print("Please use with [setting] [value], where setting can be");
            _pluginInterface.Framework.Gui.Chat.Print(
                "        -- SwitchGear [0|off|false|1|on|true]: do change the gear set to the correct one for the node.");
            _pluginInterface.Framework.Gui.Chat.Print("        -- Miner [string]: the name of your miner gear set of choice.");
            _pluginInterface.Framework.Gui.Chat.Print("        -- Botanist [string]: the name of your botanist gear set of choice.");
            _pluginInterface.Framework.Gui.Chat.Print(
                "        -- Teleport [0|off|false|1|on|true]: Teleport to the nearest aetheryte to the node. Requires Teleporter plugin.");
            _pluginInterface.Framework.Gui.Chat.Print(
                "        -- SetFlag [0|off|false|1|on|true]: Set a map marker on the approximate location of the node. Requires ChatCoordinates plugin.");
            _pluginInterface.Framework.Gui.Chat.Print(
                "        -- Record [0|off|false|1|on|true]: Start recording encountered nodes for more accurate positions.");
            _pluginInterface.Framework.Gui.Chat.Print(
                "        -- Snapshot: Records currently visible nodes a single time for more accurate positions.");
        }

        private void OnGatherBuddy(string command, string arguments)
        {
            var chat = _pluginInterface!.Framework.Gui.Chat;
            var argumentParts = arguments.Split(new[]
            {
                ' ',
            }, 2);

            if (argumentParts.Length == 0 || argumentParts[0].Length == 0)
            {
                _gatherInterface!.Visible = !_gatherInterface.Visible;
                return;
            }

            string output;
            bool   setting;
            if (Util.CompareCi(argumentParts[0], "snap") || Util.CompareCi(argumentParts[0], "snapshot"))
            {
                output = $"Recorded {Gatherer!.Snapshot()} new nearby gathering nodes.";
            }
            else if (argumentParts.Length < 2 || argumentParts[1].Length == 0)
            {
                PrintHelp();
                return;
            }
            else if (Util.CompareCi(argumentParts[0], "miner"))
            {
                var earlierName = _configuration!.MinerSetName;
                _configuration.MinerSetName = argumentParts[1];
                output                      = $"Set the Gearset for Miner from '{earlierName}' to '{_configuration.MinerSetName}'.";
            }
            else if (Util.CompareCi(argumentParts[0], "botanist"))
            {
                var earlierName = _configuration!.BotanistSetName;
                _configuration.BotanistSetName = argumentParts[1];
                output                         = $"Set the Gearset for Botanist from '{earlierName}' to '{_configuration.BotanistSetName}'.";
            }
            else if (Util.CompareCi(argumentParts[0], "switchgear"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    chat.Print("/gatherbuddy switchgear requires an argument of [0|off|false|1|on|true].");
                    return;
                }

                var oldSetting = _configuration!.UseGearChange;
                _configuration.UseGearChange = setting;
                output                       = $"Set the value of SwitchGear from {oldSetting} to {setting}.";
            }
            else if (Util.CompareCi(argumentParts[0], "teleport"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    chat.Print("/gatherbuddy teleport requires an argument of [0|off|false|1|on|true].");
                    return;
                }

                var oldSetting = _configuration!.UseTeleport;
                _configuration.UseTeleport = setting;
                Gatherer!.TryCreateTeleporterWatcher(_pluginInterface, setting);
                output = $"Set the value of Teleport from {oldSetting} to {setting}.";
            }
            else if (Util.CompareCi(argumentParts[0], "setflag"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    chat.Print("/gatherbuddy SetFlag requires an argument of [0|off|false|1|on|true].");
                    return;
                }

                var oldSetting = _configuration!.UseCoordinates;
                _configuration.UseCoordinates = setting;
                output                        = $"Set the value of SetFlag from {oldSetting} to {setting}.";
            }
            else if (Util.CompareCi(argumentParts[0], "record"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    chat.Print("/gatherbuddy record requires an argument of [0|off|false|1|on|true].");
                    return;
                }

                var oldSetting = _configuration!.DoRecord;
                _configuration.DoRecord = setting;
                output                  = $"Set the value of DoRecord from {oldSetting} to {setting}.";
                if (setting == oldSetting)
                    return;

                if (setting)
                    Gatherer!.StartRecording();
                else
                    Gatherer!.StopRecording();
            }
            else
            {
                PrintHelp();
                return;
            }

            _pluginInterface.SavePluginConfig(_configuration);
            chat.Print(output);
            PluginLog.Information(output);
        }

        private void OnGatherGroup(string command, string arguments)
        {
            var argumentParts = arguments.Split();
            switch (argumentParts.Length)
            {
                case 0:
                    Gatherer!.OnGroupGatherAction("", 0);
                    break;
                case 1:
                    Gatherer!.OnGroupGatherAction(argumentParts[0], 0);
                    break;
                default:
                {
                    Gatherer!.OnGroupGatherAction(argumentParts[0], int.TryParse(argumentParts[1], out var offset) ? offset : 0);
                    break;
                }
            }
        }

        private void OnGatherDebug(string command, string arguments)
        {
            var argumentParts = arguments.Split();
            if (argumentParts.Length == 0)
                return;

            if (argumentParts.Length < 2)
                if (Util.CompareCi(argumentParts[0], "purgeallrecords"))
                    Gatherer!.PurgeAllRecords();

            if (Util.CompareCi(argumentParts[0], "dump"))
                switch (argumentParts[1].ToLowerInvariant())
                {
                    case "aetherytes":
                        Gatherer!.DumpAetherytes();
                        break;
                    case "territories":
                        Gatherer!.DumpTerritories();
                        break;
                    case "items":
                        Gatherer!.DumpItems();
                        break;
                    case "nodes":
                        Gatherer!.DumpNodes();
                        break;
                    case "fish":
                        Gatherer!.DumpFish();
                        break;
                    case "fishingspots":
                        Gatherer!.DumpFishingSpots();
                        break;
                    case "records":
                        Gatherer!.PrintRecords();
                        break;
                    case "fishlog":
                        Gatherer!.FishManager.DumpFishLog();
                        break;
                }

            if (Util.CompareCi(argumentParts[0], "purgefish"))
            {
                var name = arguments.Substring(argumentParts[0].Length + 1);
                var fish = Gatherer!.FishManager.FindFishByName(name, _pluginInterface!.ClientState.ClientLanguage);
                if (fish == null)
                    _pluginInterface.Framework.Gui.Chat.PrintError($"No fish found for [{name}].");
                else
                    fish.Record.Delete();
            }

            if (Util.CompareCi(argumentParts[0], "weather"))
            {
                var weather = Service<SkyWatcher>.Get().GetForecast(_pluginInterface!.ClientState.TerritoryType);
                _pluginInterface.Framework.Gui.Chat.Print(weather.Weather.Name);
            }

            if (!Util.CompareCi(argumentParts[0], "purge"))
                return;

            if (uint.TryParse(argumentParts[1], out var id))
                Gatherer!.PurgeRecord(id);
            else
                Gatherer!.PurgeRecords(string.Join(" ", argumentParts.Skip(1)));
        }
    }
}
