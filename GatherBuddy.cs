using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Dalamud;
using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Logging;
using Dalamud.Plugin;
using GatherBuddy.Gui;
using GatherBuddy.Managers;
using GatherBuddy.Utility;
using ImGuiNET;
using Newtonsoft.Json;
using CommandManager = Dalamud.Game.Command.CommandManager;
using GatheringType = GatherBuddy.Enums.GatheringType;
using Util = GatherBuddy.Utility.Util;

namespace GatherBuddy
{
    public class GatherBuddy : IDalamudPlugin
    {
        public string Name
            => "GatherBuddy";

        public static DalamudPluginInterface   PluginInterface { get; private set; } = null!;
        public static ClientState              ClientState     { get; private set; } = null!;
        public static Condition                Conditions      { get; private set; } = null!;
        public static DataManager              GameData        { get; private set; } = null!;
        public static Framework                Framework       { get; private set; } = null!;
        public static ObjectTable              Objects         { get; private set; } = null!;
        public static CommandManager           Commands        { get; private set; } = null!;
        public static GatherBuddyConfiguration Config          { get; private set; } = null!;
        public static ChatGui                  Chat            { get; private set; } = null!;
        public static SigScanner               SigScanner      { get; private set; } = null!;
        public static ClientLanguage           Language        { get; private set; } = ClientLanguage.English;


        public readonly  Gatherer     Gatherer;
        public readonly  AlarmManager Alarms;
        private readonly Interface    _gatherInterface;
        private readonly FishingTimer _fishingTimer;


        public static string Version = string.Empty;

        public GatherBuddy(DalamudPluginInterface pluginInterface, ClientState clientState, Condition conditions, DataManager gameData,
            Framework framework, ObjectTable objects, CommandManager commands, ChatGui chat, SigScanner sigScanner)
        {
            PluginInterface = pluginInterface;
            ClientState     = clientState;
            Conditions      = conditions;
            GameData        = gameData;
            Framework       = framework;
            Objects         = objects;
            Commands        = commands;
            Chat            = chat;
            SigScanner      = sigScanner;

            Version  = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "";
            Config   = GatherBuddyConfiguration.Load();
            Language = ClientState.ClientLanguage;

            var commandManager = new Managers.CommandManager(SigScanner);
            Gatherer         = new Gatherer(commandManager);
            Alarms           = Gatherer.Alarms;
            _gatherInterface = new Interface(this);
            _fishingTimer    = new FishingTimer(Gatherer!.FishManager, Gatherer!.WeatherManager);

            if (!FishManager.GetSaveFileName().Exists)
                Gatherer!.FishManager.SaveFishRecords();
            else
                Gatherer!.FishManager.LoadFishRecords();

            Commands.AddHandler("/gatherbuddy", new CommandInfo(OnGatherBuddy)
            {
                HelpMessage = "Use for settings. Use without arguments for interface.",
                ShowInHelp  = true,
            });

            Commands.AddHandler("/gather", new CommandInfo(OnGather)
            {
                HelpMessage = "Mark the nearest node containing the item supplied, teleport to the nearest aetheryte, equip appropriate gear.",
                ShowInHelp  = true,
            });

            Commands.AddHandler("/gatherbtn", new CommandInfo(OnGatherBtn)
            {
                HelpMessage =
                    "Mark the nearest botanist node containing the item supplied, teleport to the nearest aetheryte, equip appropriate gear.",
                ShowInHelp = true,
            });

            Commands.AddHandler("/gathermin", new CommandInfo(OnGatherMin)
            {
                HelpMessage =
                    "Mark the nearest miner node containing the item supplied, teleport to the nearest aetheryte, equip appropriate gear.",
                ShowInHelp = true,
            });

            Commands.AddHandler("/gatherfish", new CommandInfo(OnGatherFish)
            {
                HelpMessage =
                    "Mark the nearest fishing spot containing the fish supplied, teleport to the nearest aetheryte and equip fishing gear.",
                ShowInHelp = true,
            });

            Commands.AddHandler("/gathergroup", new CommandInfo(OnGatherGroup)
            {
                HelpMessage = "Teleport to the node of a group corresponding to current time. Use /gathergroup for more details.",
                ShowInHelp  = true,
            });

            Commands.AddHandler("/gatherdebug", new CommandInfo(OnGatherDebug)
            {
                HelpMessage = "Dump some collected information.",
                ShowInHelp  = false,
            });

            ClientState.TerritoryChanged           += Gatherer!.OnTerritoryChange;
            PluginInterface.UiBuilder.Draw         += _gatherInterface!.Draw;
            PluginInterface.UiBuilder.OpenConfigUi += OnConfigCommandHandler;

            if (Config!.DoRecord)
                Gatherer.StartRecording();

            if (Config.AlarmsEnabled)
                Alarms!.Enable(true);

            if (Config.OpenOnStart)
                OnConfigCommandHandler();
        }

        void IDisposable.Dispose()
        {
            _gatherInterface.Dispose();
            _fishingTimer.Dispose();
            PluginInterface.UiBuilder.OpenConfigUi -= OnConfigCommandHandler;
            PluginInterface.UiBuilder.Draw         -= _gatherInterface!.Draw;
            ClientState.TerritoryChanged           -= Gatherer!.OnTerritoryChange;
            (Gatherer as IDisposable).Dispose();
            Commands.RemoveHandler("/gatherdebug");
            Commands.RemoveHandler("/gather");
            Commands.RemoveHandler("/gatherbtn");
            Commands.RemoveHandler("/gathermin");
            Commands.RemoveHandler("/gatherfish");
            Commands.RemoveHandler("/gathergroup");
            Commands.RemoveHandler("/gatherbuddy");
        }

        private void OnGather(string command, string arguments)
        {
            if (arguments.Length == 0)
                Chat.Print("Please supply a (partial) item name for /gather.");
            else
                Gatherer!.OnGatherAction(arguments);
        }

        private void OnGatherBtn(string command, string arguments)
        {
            if (arguments.Length == 0)
                Chat.Print("Please supply a (partial) item name for /gatherbot.");
            else
                Gatherer!.OnGatherAction(arguments, GatheringType.Botanist);
        }

        private void OnGatherMin(string command, string arguments)
        {
            if (arguments.Length == 0)
                Chat.Print("Please supply a (partial) item name for /gathermin.");
            else
                Gatherer!.OnGatherAction(arguments, GatheringType.Miner);
        }

        private void OnGatherFish(string command, string arguments)
        {
            if (arguments.Length == 0)
                Chat.Print("Please supply a (partial) fish name for /gatherfish.");
            else
                Gatherer!.OnFishAction(arguments);
        }

        private void OnConfigCommandHandler()
            => _gatherInterface!.Visible = true;

        private void PrintHelp()
        {
            Chat.Print("Please use with [setting] [value], where setting can be");
            Chat.Print(
                "        -- SwitchGear [0|off|false|1|on|true]: do change the gear set to the correct one for the node.");
            Chat.Print("        -- Miner [string]: the name of your miner gear set of choice.");
            Chat.Print("        -- Botanist [string]: the name of your botanist gear set of choice.");
            Chat.Print(
                "        -- Teleport [0|off|false|1|on|true]: Teleport to the nearest aetheryte to the node. Requires Teleporter plugin.");
            Chat.Print(
                "        -- SetFlag [0|off|false|1|on|true]: Set a map marker on the approximate location of the node. Requires ChatCoordinates plugin.");
            Chat.Print(
                "        -- Record [0|off|false|1|on|true]: Start recording encountered nodes for more accurate positions.");
            Chat.Print(
                "        -- Snapshot: Records currently visible nodes a single time for more accurate positions.");
        }

        private void OnGatherBuddy(string command, string arguments)
        {
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
                var earlierName = Config.MinerSetName;
                Config.MinerSetName = argumentParts[1];
                output              = $"Set the Gearset for Miner from '{earlierName}' to '{Config.MinerSetName}'.";
            }
            else if (Util.CompareCi(argumentParts[0], "botanist"))
            {
                var earlierName = Config.BotanistSetName;
                Config.BotanistSetName = argumentParts[1];
                output                 = $"Set the Gearset for Botanist from '{earlierName}' to '{Config.BotanistSetName}'.";
            }
            else if (Util.CompareCi(argumentParts[0], "switchgear"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    Chat.Print("/gatherbuddy switchgear requires an argument of [0|off|false|1|on|true].");
                    return;
                }

                var oldSetting = Config.UseGearChange;
                Config.UseGearChange = setting;
                output               = $"Set the value of SwitchGear from {oldSetting} to {setting}.";
            }
            else if (Util.CompareCi(argumentParts[0], "teleport"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    Chat.Print("/gatherbuddy teleport requires an argument of [0|off|false|1|on|true].");
                    return;
                }

                var oldSetting = Config.UseTeleport;
                Config.UseTeleport = setting;
                Gatherer!.TryCreateTeleporterWatcher(setting);
                output = $"Set the value of Teleport from {oldSetting} to {setting}.";
            }
            else if (Util.CompareCi(argumentParts[0], "setflag"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    Chat.Print("/gatherbuddy SetFlag requires an argument of [0|off|false|1|on|true].");
                    return;
                }

                var oldSetting = Config.UseCoordinates;
                Config.UseCoordinates = setting;
                output                = $"Set the value of SetFlag from {oldSetting} to {setting}.";
            }
            else if (Util.CompareCi(argumentParts[0], "record"))
            {
                if (!Util.TryParseBoolean(argumentParts[1], out setting))
                {
                    Chat.Print("/gatherbuddy record requires an argument of [0|off|false|1|on|true].");
                    return;
                }

                var oldSetting = Config.DoRecord;
                Config.DoRecord = setting;
                output          = $"Set the value of DoRecord from {oldSetting} to {setting}.";
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

            Config.Save();
            Chat.Print(output);
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

            if (Util.CompareCi(argumentParts[0], "mergefish"))
            {
                if (argumentParts.Length < 2)
                {
                    Chat.PrintError("Please provide a filename to merge.");
                    return;
                }

                var name = arguments.Substring(argumentParts[0].Length + 1);
                var fish = Gatherer!.FishManager.MergeFishRecords(new FileInfo(name));
                switch (fish)
                {
                    case -1:
                        Chat.PrintError($"The provided file {name} does not exist.");
                        return;
                    case -2:
                        Chat.PrintError("Could not create a backup of your records, merge stopped.");
                        return;
                    case -3:
                        Chat.PrintError("Unexpected error occurred, merge stopped.");
                        return;
                    default:
                        Chat.Print($"{fish} Records updated with new data.");
                        Gatherer!.FishManager.SaveFishRecords();
                        return;
                }
            }

            if (Util.CompareCi(argumentParts[0], "purgefish"))
            {
                var name = arguments.Substring(argumentParts[0].Length + 1);
                var fish = Gatherer!.FishManager.FindFishByName(name, Language);
                if (fish == null)
                    Chat.PrintError($"No fish found for [{name}].");
                else
                    fish.Record.Delete();
            }

            if (Util.CompareCi(argumentParts[0], "weather"))
            {
                var weather = Service<SkyWatcher>.Get().GetForecast(ClientState.TerritoryType);
                Chat.Print(weather.Weather.Name);
            }

            if (Util.CompareCi(argumentParts[0], "export"))
                if (argumentParts.Length >= 2 && Util.CompareCi(argumentParts[1], "fish"))
                {
                    var ids = Gatherer!.FishManager.Fish.Values.Where(Gatherer.FishManager.FishLog.IsUnlocked).Select(i => i.ItemId).ToArray();
                    var output = $"Exported caught fish to clipboard ({ids.Length}/{Gatherer.FishManager.Fish.Count} caught).";
                    PluginLog.Information(output);
                    Chat.Print(output);
                    ImGui.SetClipboardText(JsonConvert.SerializeObject(ids, Formatting.Indented));
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
