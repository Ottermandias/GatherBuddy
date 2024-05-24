using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Command;
using Dalamud.Game.Text.SeStringHandling;
using GatherBuddy.Enums;
using GatherBuddy.Plugin;
using GatherBuddy.Time;

namespace GatherBuddy;

public partial class GatherBuddy
{
    public const string IdentifyCommand       = "identify";
    public const string GearChangeCommand     = "gearchange";
    public const string TeleportCommand       = "teleport";
    public const string MapMarkerCommand      = "mapmarker";
    public const string AdditionalInfoCommand = "information";
    public const string SetWaymarksCommand    = "waymarks";
    public const string AutoCommand           = "auto";
    public const string FullIdentify          = $"/gatherbuddy {IdentifyCommand}";
    public const string FullGearChange        = $"/gatherbuddy {GearChangeCommand}";
    public const string FullTeleport          = $"/gatherbuddy {TeleportCommand}";
    public const string FullMapMarker         = $"/gatherbuddy {MapMarkerCommand}";
    public const string FullAdditionalInfo    = $"/gatherbuddy {AdditionalInfoCommand}";
    public const string FullSetWaymarks       = $"/gatherbuddy {SetWaymarksCommand}";
    public const string FullAuto              = $"/gatherbuddy {AutoCommand}";

    private readonly Dictionary<string, CommandInfo> _commands = new();

    private void InitializeCommands()
    {
        _commands["/gatherbuddy"] = new CommandInfo(OnGatherBuddy)
        {
            HelpMessage = "Use to open the GatherBuddy interface.",
            ShowInHelp  = false,
        };

        _commands["/gbr"] = new CommandInfo(OnGatherBuddy)
        {
            HelpMessage = "Use to open the GatherBuddy interface.",
            ShowInHelp  = true,
        };

        _commands["/gather"] = new CommandInfo(OnGather)
        {
            HelpMessage = "Mark the nearest node containing the item supplied, teleport to the nearest aetheryte, equip appropriate gear.\n"
              + "You can use 'alarm' to gather the last triggered alarm or 'next' to gather the same item as before, but in the next-best location.",
            ShowInHelp = true,
        };

        _commands["/gatherbtn"] = new CommandInfo(OnGatherBtn)
        {
            HelpMessage =
                "Mark the nearest botanist node containing the item supplied, teleport to the nearest aetheryte, equip appropriate gear.",
            ShowInHelp = true,
        };

        _commands["/gathermin"] = new CommandInfo(OnGatherMin)
        {
            HelpMessage =
                "Mark the nearest miner node containing the item supplied, teleport to the nearest aetheryte, equip appropriate gear.",
            ShowInHelp = true,
        };

        _commands["/gatherfish"] = new CommandInfo(OnGatherFish)
        {
            HelpMessage =
                "Mark the nearest fishing spot containing the fish supplied, teleport to the nearest aetheryte and equip fishing gear.",
            ShowInHelp = true,
        };

        _commands["/gathergroup"] = new CommandInfo(OnGatherGroup)
        {
            HelpMessage = "Teleport to the node of a group corresponding to current time. Use /gathergroup for more details.",
            ShowInHelp  = true,
        };

        _commands["/gbc"] = new CommandInfo(OnGatherBuddyShort)
        {
            HelpMessage = "Some quick toggles for config options. Use without argument for help.",
            ShowInHelp  = true,
        };

        _commands["/gatherdebug"] = new CommandInfo(OnGatherDebug)
        {
            ShowInHelp = false,
        };

        foreach (var (command, info) in _commands)
            Dalamud.Commands.AddHandler(command, info);
    }

    private void DisposeCommands()
    {
        foreach (var command in _commands.Keys)
            Dalamud.Commands.RemoveHandler(command);
    }

    private void OnGatherBuddy(string command, string arguments)
    {
        if (!Executor.DoCommand(arguments))
            Interface.Toggle();
    }

    private void OnGather(string command, string arguments)
    {
        if (arguments.Length == 0)
            Communicator.NoItemName(command, "item");
        else
            Executor.GatherItemByName(arguments);
    }

    private void OnGatherBtn(string command, string arguments)
    {
        if (arguments.Length == 0)
            Communicator.NoItemName(command, "item");
        else
            Executor.GatherItemByName(arguments, GatheringType.Botanist);
    }

    private void OnGatherMin(string command, string arguments)
    {
        if (arguments.Length == 0)
            Communicator.NoItemName(command, "item");
        else
            Executor.GatherItemByName(arguments, GatheringType.Miner);
    }

    private void OnGatherFish(string command, string arguments)
    {
        if (arguments.Length == 0)
            Communicator.NoItemName(command, "fish");
        else
            Executor.GatherFishByName(arguments);
    }

    private void OnGatherGroup(string command, string arguments)
    {
        if (arguments.Length == 0)
        {
            Communicator.Print(GatherGroupManager.CreateHelp());
            return;
        }

        var argumentParts = arguments.Split();
        var minute = (Time.EorzeaMinuteOfDay + (argumentParts.Length < 2 ? 0 : int.TryParse(argumentParts[1], out var offset) ? offset : 0))
          % RealTime.MinutesPerDay;
        if (!GatherGroupManager.TryGetValue(argumentParts[0], out var group))
        {
            Communicator.NoGatherGroup(argumentParts[0]);
            return;
        }

        var node = group.CurrentNode((uint)minute);
        if (node == null)
        {
            Communicator.NoGatherGroupItem(argumentParts[0], minute);
        }
        else
        {
            if (node.Annotation.Any())
                Communicator.Print(node.Annotation);
            if (node.PreferLocation == null)
                Executor.GatherItem(node.Item);
            else
                Executor.GatherLocation(node.PreferLocation);
        }
    }

    private void OnGatherBuddyShort(string command, string arguments)
    {
        switch (arguments.ToLowerInvariant())
        {
            case "window":
                Config.ShowGatherWindow = !Config.ShowGatherWindow;
                break;
            case "alarm":
                if (Config.AlarmsEnabled)
                    AlarmManager.Disable();
                else
                    AlarmManager.Enable();
                break;
            case "spear":
                Config.ShowSpearfishHelper = !Config.ShowSpearfishHelper;
                break;
            case "fish":
                Config.ShowFishTimer = !Config.ShowFishTimer;
                break;
            case "edit":
                if (!Config.FishTimerEdit)
                {
                    Config.ShowFishTimer = true;
                    Config.FishTimerEdit = true;
                }
                else
                {
                    Config.FishTimerEdit = false;
                }

                break;
            case "unlock":
                Config.MainWindowLockPosition = false;
                Config.MainWindowLockResize   = false;
                break;
            default:
                var shortHelpString = new SeStringBuilder().AddText("Use ").AddColoredText(command, Config.SeColorCommands)
                    .AddText(" with one of the following arguments:\n")
                    .AddColoredText("        window", Config.SeColorArguments).AddText(" - Toggle the Gather Window on or off.\n")
                    .AddColoredText("        alarm",  Config.SeColorArguments).AddText(" - Toggle Alarms on or off.\n")
                    .AddColoredText("        spear",  Config.SeColorArguments).AddText(" - Toggle the Spearfishing Helper on or off.\n")
                    .AddColoredText("        fish",   Config.SeColorArguments).AddText(" - Toggle the Fish Timer window on or off.\n")
                    .AddColoredText("        edit",   Config.SeColorArguments).AddText(" - Toggle edit mode for the fish timer.\n")
                    .AddColoredText("        unlock", Config.SeColorArguments).AddText(" - Unlock the main window position and size.")
                    .BuiltString;
                Communicator.Print(shortHelpString);
                return;
        }

        Config.Save();
    }

    private static void OnGatherDebug(string command, string arguments)
    {
        DebugMode = !DebugMode;
    }
}
