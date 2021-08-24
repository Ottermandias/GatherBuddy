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
                GatherBuddy.Config.OpenOnStart, b => GatherBuddy.Config.OpenOnStart = b);

        private void DrawGearChangeBox()
            => DrawCheckbox("Gear Change",
                "Toggle whether to automatically switch gear to the correct job gear for a node.\nUses Miner Set, Botanist Set and Fisher Set.",
                GatherBuddy.Config.UseGearChange, b => GatherBuddy.Config.UseGearChange = b);

        private void DrawTeleportBox()
            => DrawCheckbox("Teleport",
                "Toggle whether to automatically teleport to a chosen node.\nRequires the Teleporter plugin and uses /tp.",
                GatherBuddy.Config.UseTeleport, b => GatherBuddy.Config.UseTeleport = b);

        private void DrawMapMarkerBox()
            => DrawCheckbox("Map Marker",
                "Toggle whether to automatically set a map marker on the approximate location of the chosen node.\nRequires the ChatCoordinates plugin and uses /coord.",
                GatherBuddy.Config.UseCoordinates, b => GatherBuddy.Config.UseCoordinates = b);

        private void DrawFishTimerBox()
            => DrawCheckbox("Show Fish Timer",
                "Toggle whether to show the fish timer window while fishing.",
                GatherBuddy.Config.ShowFishTimer, b => GatherBuddy.Config.ShowFishTimer = b);

        private void DrawFishTimerEditBox()
            => DrawCheckbox("Edit Fish Timer",
                "Enable editing the fish timer window.",
                GatherBuddy.Config.FishTimerEdit, b => GatherBuddy.Config.FishTimerEdit = b);

        private void DrawFishTimerHideBox()
            => DrawCheckbox("Hide Uncaught Fish",
                "Hide all fish from the fish timer window that have not been recorded with the given combination of snagging and bait.",
                GatherBuddy.Config.HideUncaughtFish, b => GatherBuddy.Config.HideUncaughtFish = b);

        private void DrawFishWindowTimersBox()
            => DrawCheckbox("Show Window Timers",
                "Show the the times for fishing windows for restricted fish in the fish timer window.",
                GatherBuddy.Config.ShowWindowTimers, b => GatherBuddy.Config.ShowWindowTimers = b);

        private void DrawFishTimerHideBox2()
            => DrawCheckbox("Hide Unavailable Fish",
                "Hide all fish from the fish timer window that have have known requirements that are unfulfilled, like Fisher's Intuition or Snagging.",
                GatherBuddy.Config.HideUnavailableFish, b => GatherBuddy.Config.HideUnavailableFish = b);

        private void DrawPrintUptimesBox()
            => DrawCheckbox("Print Node Uptimes", "Print the uptimes of nodes you try to /gather in the chat if they are not always up.",
                GatherBuddy.Config.PrintUptime,   b => GatherBuddy.Config.PrintUptime = b);

        private void DrawPrintGigHeadBox()
            => DrawCheckbox("Print Gig-head",
                "Print the required Gig-head for the fish you try to /gatherfish, if it is obtained via spearfishing.",
                GatherBuddy.Config.PrintGigHead, b => GatherBuddy.Config.PrintGigHead = b);

        private void DrawMinerSetInput(float width)
            => DrawSetInput(width, "Miner", GatherBuddy.Config.MinerSetName, s => GatherBuddy.Config.MinerSetName = s);

        private void DrawBotanistSetInput(float width)
            => DrawSetInput(width, "Botanist", GatherBuddy.Config.BotanistSetName, s => GatherBuddy.Config.BotanistSetName = s);

        private void DrawFisherSetInput(float width)
            => DrawSetInput(width, "Fisher", GatherBuddy.Config.FisherSetName, s => GatherBuddy.Config.FisherSetName = s);

        private void DrawNodeAlarmFormatInput()
            => DrawFormatInput("Node Alarm Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Name} with the alarm name.\n- {Offset} with the alarm offset.\n- {TimesShort} with the corresponding uptimes in ##-##|##-## format.\n- {TimesLong} with the corresponding uptimes in long format.\n- {AllItems} with the corresponding items, separated by comma and in client language.\n- {DelayString} with the time until the node is up.",
                GatherBuddy.Config.NodeAlarmFormat, GatherBuddyConfiguration.DefaultNodeAlarmFormat,
                s => GatherBuddy.Config.NodeAlarmFormat = s);

        private void DrawFishAlarmFormatInput()
            => DrawFormatInput("Fish Alarm Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Name} with the alarm name.\n- {Offset} with the alarm offset.\n- {FishName} with the name of the fish.\n- {FishingSpotName} with the name of the default fishing spot for the fish.\n- {BaitName} with the name of the initial bait to use.\n- {DelayString} with the time until the node is up.",
                GatherBuddy.Config.FishAlarmFormat, GatherBuddyConfiguration.DefaultFishAlarmFormat,
                s => GatherBuddy.Config.FishAlarmFormat = s);

        private void DrawItemIdentifiedFormatInput()
            => DrawFormatInput("Item Identification Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Id} with the item ID.\n- {Name} with the item name in client language.\n- {Input} with the input string.",
                GatherBuddy.Config.IdentifiedItemFormat,
                GatherBuddyConfiguration.DefaultIdentifiedItemFormat, s => GatherBuddy.Config.IdentifiedItemFormat = s);

        private void DrawFishIdentifiedFormatInput()
            => DrawFormatInput("Fish Identification Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Id} with the fish item ID.\n- {Name} with the fish name in client language.\n- {Input} with the input string.",
                GatherBuddy.Config.IdentifiedFishFormat,
                GatherBuddyConfiguration.DefaultIdentifiedFishFormat, s => GatherBuddy.Config.IdentifiedFishFormat = s);

        private void DrawFishingSpotIdentifiedFormatInput()
            => DrawFormatInput("Fishing Spot Choice Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Id} with the fishing spot id.\n- {Name} with the fishing spot name in client language.\n- {Input} with the input string.\n- {FishId} with the fish item id.\n- {FishName} with the fish name in client language.",
                GatherBuddy.Config.IdentifiedFishingSpotFormat,
                GatherBuddyConfiguration.DefaultIdentifiedFishingSpotFormat, s => GatherBuddy.Config.IdentifiedFishingSpotFormat = s);

        private void DrawColorPickerAvailableFish()
            => DrawColorPicker("Available Fish",       "The color used to display currently available uptimes in the timed fish tab.",
                GatherBuddy.Config.AvailableFishColor, Colors.FishTab.UptimeRunning, v => GatherBuddy.Config.AvailableFishColor = v);

        private void DrawColorPickerUpcomingFish()
            => DrawColorPicker("Upcoming Fish",
                "The color used to display the remaining waiting time until a fish is available the next time in the timed fish tab.",
                GatherBuddy.Config.UpcomingFishColor, Colors.FishTab.UptimeUpcoming, v => GatherBuddy.Config.UpcomingFishColor = v);

        private void DrawColorPickerDepAvailableFish()
            => DrawColorPicker("Available Fish (Dependency)",
                "The color used to display currently available uptimes in the timed fish tab, if the fish depends on mooching or catching of fish that have their own weather or time requirements.",
                GatherBuddy.Config.DependentAvailableFishColor, Colors.FishTab.UptimeRunningDependency,
                v => GatherBuddy.Config.DependentAvailableFishColor = v);

        private void DrawColorPickerDepUpcomingFish()
            => DrawColorPicker("Upcoming Fish (Dependency)",
                "The color used to display the remaining waiting time until a fish is available the next time in the timed fish tab, if the fish depends on mooching or catching of fish that have their own weather or time requirements.",
                GatherBuddy.Config.DependentUpcomingFishColor, Colors.FishTab.UptimeUpcomingDependency,
                v => GatherBuddy.Config.DependentUpcomingFishColor = v);

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
            DrawPrintGigHeadBox();

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
