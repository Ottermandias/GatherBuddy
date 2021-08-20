using System.Numerics;
using Dalamud.Interface;
using ImGuiNET;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private void DrawOpenOnStartBox()
            => DrawCheckbox("Open On Start",
                "Toggle whether the GatherBuddy GUI should be visible after you start the game.",
                _config.OpenOnStart, b => _config.OpenOnStart = b);

        private void DrawGearChangeBox()
            => DrawCheckbox("Gear Change",
                "Toggle whether to automatically switch gear to the correct job gear for a node.\nUses Miner Set, Botanist Set and Fisher Set.",
                _config.UseGearChange, b => _config.UseGearChange = b);

        private void DrawTeleportBox()
            => DrawCheckbox("Teleport",
                "Toggle whether to automatically teleport to a chosen node.\nRequires the Teleporter plugin and uses /tp.",
                _config.UseTeleport, b => _config.UseTeleport = b);

        private void DrawMapMarkerBox()
            => DrawCheckbox("Map Marker",
                "Toggle whether to automatically set a map marker on the approximate location of the chosen node.\nRequires the ChatCoordinates plugin and uses /coord.",
                _config.UseCoordinates, b => _config.UseCoordinates = b);

        private void DrawFishTimerBox()
            => DrawCheckbox("Show Fish Timer",
                "Toggle whether to show the fish timer window while fishing.",
                _config.ShowFishTimer, b => _config.ShowFishTimer = b);

        private void DrawFishTimerEditBox()
            => DrawCheckbox("Edit Fish Timer",
                "Enable editing the fish timer window.",
                _config.FishTimerEdit, b => _config.FishTimerEdit = b);

        private void DrawFishTimerHideBox()
            => DrawCheckbox("Hide Uncaught Fish",
                "Hide all fish from the fish timer window that have not been recorded with the given combination of snagging and bait.",
                _config.HideUncaughtFish, b => _config.HideUncaughtFish = b);

        private void DrawFishWindowTimersBox()
            => DrawCheckbox("Show Window Timers",
                "Show the the times for fishing windows for restricted fish in the fish timer window.",
                _config.ShowWindowTimers, b => _config.ShowWindowTimers = b);

        private void DrawFishTimerHideBox2()
            => DrawCheckbox("Hide Unavailable Fish",
                "Hide all fish from the fish timer window that have have known requirements that are unfulfilled, like Fisher's Intuition or Snagging.",
                _config.HideUnavailableFish, b => _config.HideUnavailableFish = b);

        private void DrawPrintUptimesBox()
            => DrawCheckbox("Print Node Uptimes", "Print the uptimes of nodes you try to /gather in the chat if they are not always up.",
                _config.PrintUptime,              b => _config.PrintUptime = b);

        private void DrawPrintGigheadBox()
            => DrawCheckbox("Print Gighead",
                "Print the required Gighead for the fish you try to /gatherfish, if it is obtained via spearfishing.",
                _config.PrintGighead, b => _config.PrintGighead = b);

        private void DrawMinerSetInput(float width)
            => DrawSetInput(width, "Miner", _config.MinerSetName, s => _config.MinerSetName = s);

        private void DrawBotanistSetInput(float width)
            => DrawSetInput(width, "Botanist", _config.BotanistSetName, s => _config.BotanistSetName = s);

        private void DrawFisherSetInput(float width)
            => DrawSetInput(width, "Fisher", _config.FisherSetName, s => _config.FisherSetName = s);

        private void DrawNodeAlarmFormatInput()
            => DrawFormatInput("Node Alarm Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Name} with the alarm name.\n- {Offset} with the alarm offset.\n- {TimesShort} with the corresponding uptimes in ##-##|##-## format.\n- {TimesLong} with the corresponding uptimes in long format.\n- {AllItems} with the corresponding items, separated by comma and in client language.\n- {DelayString} with the time until the node is up.",
                _config.NodeAlarmFormat, GatherBuddyConfiguration.DefaultNodeAlarmFormat,
                s => _config.NodeAlarmFormat = s);

        private void DrawFishAlarmFormatInput()
            => DrawFormatInput("Fish Alarm Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Name} with the alarm name.\n- {Offset} with the alarm offset.\n- {FishName} with the name of the fish.\n- {FishingSpotName} with the name of the default fishing spot for the fish.\n- {BaitName} with the name of the initial bait to use.\n- {DelayString} with the time until the node is up.",
                _config.FishAlarmFormat, GatherBuddyConfiguration.DefaultFishAlarmFormat,
                s => _config.FishAlarmFormat = s);

        private void DrawItemIdentifiedFormatInput()
            => DrawFormatInput("Item Identification Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Id} with the item ID.\n- {Name} with the item name in client language.\n- {Input} with the input string.",
                _config.IdentifiedItemFormat,
                GatherBuddyConfiguration.DefaultIdentifiedItemFormat, s => _config.IdentifiedItemFormat = s);

        private void DrawFishIdentifiedFormatInput()
            => DrawFormatInput("Fish Identification Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Id} with the fish item ID.\n- {Name} with the fish name in client language.\n- {Input} with the input string.",
                _config.IdentifiedFishFormat,
                GatherBuddyConfiguration.DefaultIdentifiedFishFormat, s => _config.IdentifiedFishFormat = s);

        private void DrawFishingSpotIdentifiedFormatInput()
            => DrawFormatInput("Fishing Spot Choice Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Id} with the fishing spot id.\n- {Name} with the fishing spot name in client language.\n- {Input} with the input string.\n- {FishId} with the fish item id.\n- {FishName} with the fish name in client language.",
                _config.IdentifiedFishingSpotFormat,
                GatherBuddyConfiguration.DefaultIdentifiedFishingSpotFormat, s => _config.IdentifiedFishingSpotFormat = s);

        private void DrawColorPickerAvailableFish()
            => DrawColorPicker("Available Fish", "The color used to display currently available uptimes in the timed fish tab.",
                _config.AvailableFishColor,      Colors.FishTab.UptimeRunning, v => _config.AvailableFishColor = v);

        private void DrawColorPickerUpcomingFish()
            => DrawColorPicker("Upcoming Fish",
                "The color used to display the remaining waiting time until a fish is available the next time in the timed fish tab.",
                _config.UpcomingFishColor, Colors.FishTab.UptimeUpcoming, v => _config.UpcomingFishColor = v);

        private void DrawColorPickerDepAvailableFish()
            => DrawColorPicker("Available Fish (Dependency)",
                "The color used to display currently available uptimes in the timed fish tab, if the fish depends on mooching or catching of fish that have their own weather or time requirements.",
                _config.DependentAvailableFishColor, Colors.FishTab.UptimeRunningDependency, v => _config.DependentAvailableFishColor = v);

        private void DrawColorPickerDepUpcomingFish()
            => DrawColorPicker("Upcoming Fish (Dependency)",
                "The color used to display the remaining waiting time until a fish is available the next time in the timed fish tab, if the fish depends on mooching or catching of fish that have their own weather or time requirements.",
                _config.DependentUpcomingFishColor, Colors.FishTab.UptimeUpcomingDependency, v => _config.DependentUpcomingFishColor = v);

        private void DrawSettingsTab()
        {
            using var imgui = new ImGuiRaii();
            if (!imgui.Begin(() => ImGui.BeginChild("##settingsList", -Vector2.One, true), ImGui.EndChild))
                return;

            var inputSize = 125 * _globalScale;

            DrawOpenOnStartBox();

            DrawMinerSetInput(inputSize);
            DrawBotanistSetInput(inputSize);
            DrawFisherSetInput(inputSize);

            DrawGearChangeBox();
            DrawTeleportBox();
            DrawMapMarkerBox();

            DrawPrintUptimesBox();
            DrawPrintGigheadBox();

            DrawFishTimerBox();
            DrawFishTimerEditBox();
            DrawFishWindowTimersBox();
            DrawFishTimerHideBox();
            DrawFishTimerHideBox2();

            ImGuiHelpers.ScaledDummy(new Vector2(0, 20));
            DrawColorPickerAvailableFish();
            DrawColorPickerUpcomingFish();
            DrawColorPickerDepAvailableFish();
            DrawColorPickerDepUpcomingFish();

            ImGuiHelpers.ScaledDummy(new Vector2(0, 20));
            DrawNodeAlarmFormatInput();
            DrawFishAlarmFormatInput();
            DrawItemIdentifiedFormatInput();
            DrawFishIdentifiedFormatInput();
            DrawFishingSpotIdentifiedFormatInput();
        }
    }
}
