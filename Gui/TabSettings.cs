using System.Numerics;
using Dalamud.Interface;
using ImGuiNET;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private static void DrawOpenOnStartBox()
            => DrawCheckbox("Open On Start",
                "Toggle whether the GatherBuddy GUI should be visible after you start the game.",
                GatherBuddy.Config.OpenOnStart, b => GatherBuddy.Config.OpenOnStart = b);

        private static void DrawGearChangeBox()
            => DrawCheckbox("Gear Change",
                "Toggle whether to automatically switch gear to the correct job gear for a node.\nUses Miner Set, Botanist Set and Fisher Set.",
                GatherBuddy.Config.UseGearChange, b => GatherBuddy.Config.UseGearChange = b);

        private static void DrawTeleportBox()
            => DrawCheckbox("Teleport",
                "Toggle whether to automatically teleport to a chosen node.",
                GatherBuddy.Config.UseTeleport, b => GatherBuddy.Config.UseTeleport = b);

        private static void DrawMapMarkerOpenBox()
            => DrawCheckbox("Map Marker (Open)",
                "Toggle whether to automatically set a map marker on the approximate location of the chosen node and open the map.",
                GatherBuddy.Config.UseCoordinates, b => GatherBuddy.Config.UseCoordinates = b);

        private static void DrawMapMarkerPrintBox()
            => DrawCheckbox("Map Marker (Print)",
                "Toggle whether to automatically write a map link to the approximate location of the chosen node to chat.",
                GatherBuddy.Config.WriteCoordinates, b => GatherBuddy.Config.WriteCoordinates = b);

        private static void DrawFishTimerBox()
            => DrawCheckbox("Show Fish Timer",
                "Toggle whether to show the fish timer window while fishing.",
                GatherBuddy.Config.ShowFishTimer, b => GatherBuddy.Config.ShowFishTimer = b);

        private static void DrawFishTimerEditBox()
            => DrawCheckbox("Edit Fish Timer",
                "Enable editing the fish timer window.",
                GatherBuddy.Config.FishTimerEdit, b => GatherBuddy.Config.FishTimerEdit = b);

        private static void DrawFishTimerHideBox()
            => DrawCheckbox("Hide Uncaught Fish",
                "Hide all fish from the fish timer window that have not been recorded with the given combination of snagging and bait.",
                GatherBuddy.Config.HideUncaughtFish, b => GatherBuddy.Config.HideUncaughtFish = b);

        private static void DrawFishWindowTimersBox()
            => DrawCheckbox("Show Window Timers",
                "Show the the times for fishing windows for restricted fish in the fish timer window.",
                GatherBuddy.Config.ShowWindowTimers, b => GatherBuddy.Config.ShowWindowTimers = b);

        private static void DrawFishTimerHideBox2()
            => DrawCheckbox("Hide Unavailable Fish",
                "Hide all fish from the fish timer window that have have known requirements that are unfulfilled, like Fisher's Intuition or Snagging.",
                GatherBuddy.Config.HideUnavailableFish, b => GatherBuddy.Config.HideUnavailableFish = b);

        private static void DrawPrintUptimesBox()
            => DrawCheckbox("Print Node Uptimes", "Print the uptimes of nodes you try to /gather in the chat if they are not always up.",
                GatherBuddy.Config.PrintUptime,   b => GatherBuddy.Config.PrintUptime = b);

        private static void DrawPrintGigHeadBox()
            => DrawCheckbox("Print Gig-head",
                "Print the required Gig-head for the fish you try to /gatherfish, if it is obtained via spearfishing.",
                GatherBuddy.Config.PrintGigHead, b => GatherBuddy.Config.PrintGigHead = b);

        private static void DrawMinerSetInput(float width)
            => DrawSetInput(width, "Miner", GatherBuddy.Config.MinerSetName, s => GatherBuddy.Config.MinerSetName = s);

        private static void DrawBotanistSetInput(float width)
            => DrawSetInput(width, "Botanist", GatherBuddy.Config.BotanistSetName, s => GatherBuddy.Config.BotanistSetName = s);

        private static void DrawFisherSetInput(float width)
            => DrawSetInput(width, "Fisher", GatherBuddy.Config.FisherSetName, s => GatherBuddy.Config.FisherSetName = s);

        private static void DrawNodeAlarmFormatInput()
            => DrawFormatInput("Node Alarm Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Name} with the alarm name.\n- {Offset} with the alarm offset.\n- {TimesShort} with the corresponding uptimes in ##-##|##-## format.\n- {TimesLong} with the corresponding uptimes in long format.\n- {AllItems} with the corresponding items, separated by comma and in client language.\n- {DelayString} with the time until the node is up.",
                GatherBuddy.Config.NodeAlarmFormat, GatherBuddyConfiguration.DefaultNodeAlarmFormat,
                s => GatherBuddy.Config.NodeAlarmFormat = s);

        private static void DrawFishAlarmFormatInput()
            => DrawFormatInput("Fish Alarm Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Name} with the alarm name.\n- {Offset} with the alarm offset.\n- {FishName} with the name of the fish.\n- {FishingSpotName} with the name of the default fishing spot for the fish.\n- {BaitName} with the name of the initial bait to use.\n- {DelayString} with the time until the node is up.",
                GatherBuddy.Config.FishAlarmFormat, GatherBuddyConfiguration.DefaultFishAlarmFormat,
                s => GatherBuddy.Config.FishAlarmFormat = s);

        private static void DrawItemIdentifiedFormatInput()
            => DrawFormatInput("Item Identification Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Id} with the item ID.\n- {Name} with the item name in client language.\n- {Input} with the input string.",
                GatherBuddy.Config.IdentifiedItemFormat,
                GatherBuddyConfiguration.DefaultIdentifiedItemFormat, s => GatherBuddy.Config.IdentifiedItemFormat = s);

        private static void DrawFishIdentifiedFormatInput()
            => DrawFormatInput("Fish Identification Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Id} with the fish item ID.\n- {Name} with the fish name in client language.\n- {Input} with the input string.",
                GatherBuddy.Config.IdentifiedFishFormat,
                GatherBuddyConfiguration.DefaultIdentifiedFishFormat, s => GatherBuddy.Config.IdentifiedFishFormat = s);

        private static void DrawFishingSpotIdentifiedFormatInput()
            => DrawFormatInput("Fishing Spot Choice Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Id} with the fishing spot id.\n- {Name} with the fishing spot name in client language.\n- {Input} with the input string.\n- {FishId} with the fish item id.\n- {FishName} with the fish name in client language.",
                GatherBuddy.Config.IdentifiedFishingSpotFormat,
                GatherBuddyConfiguration.DefaultIdentifiedFishingSpotFormat, s => GatherBuddy.Config.IdentifiedFishingSpotFormat = s);

        private static void DrawColorPickerAvailableFish()
            => DrawColorPicker("Available Fish",       "The color used to display currently available uptimes in the timed fish tab.",
                GatherBuddy.Config.AvailableFishColor, Colors.FishTab.UptimeRunning, v => GatherBuddy.Config.AvailableFishColor = v);

        private static void DrawColorPickerUpcomingFish()
            => DrawColorPicker("Upcoming Fish",
                "The color used to display the remaining waiting time until a fish is available the next time in the timed fish tab.",
                GatherBuddy.Config.UpcomingFishColor, Colors.FishTab.UptimeUpcoming, v => GatherBuddy.Config.UpcomingFishColor = v);

        private static void DrawColorPickerDepAvailableFish()
            => DrawColorPicker("Available Fish (Dependency)",
                "The color used to display currently available uptimes in the timed fish tab, if the fish depends on mooching or catching of fish that have their own weather or time requirements.",
                GatherBuddy.Config.DependentAvailableFishColor, Colors.FishTab.UptimeRunningDependency,
                v => GatherBuddy.Config.DependentAvailableFishColor = v);

        private static void DrawColorPickerDepUpcomingFish()
            => DrawColorPicker("Upcoming Fish (Dependency)",
                "The color used to display the remaining waiting time until a fish is available the next time in the timed fish tab, if the fish depends on mooching or catching of fish that have their own weather or time requirements.",
                GatherBuddy.Config.DependentUpcomingFishColor, Colors.FishTab.UptimeUpcomingDependency,
                v => GatherBuddy.Config.DependentUpcomingFishColor = v);

        private static void DrawSettingsTab()
        {
            using var imgui = new ImGuiRaii();
            if (!imgui.BeginChild("##settingsList", -Vector2.One, true))
                return;

            var inputSize = 125 * _globalScale;

            DrawOpenOnStartBox();

            DrawMinerSetInput(inputSize);
            DrawBotanistSetInput(inputSize);
            DrawFisherSetInput(inputSize);

            DrawGearChangeBox();
            DrawTeleportBox();
            DrawMapMarkerOpenBox();
            DrawMapMarkerPrintBox();

            DrawPrintUptimesBox();
            DrawPrintGigHeadBox();

            DrawFishTimerBox();
            DrawFishTimerEditBox();
            DrawFishWindowTimersBox();
            DrawFishTimerHideBox();
            DrawFishTimerHideBox2();

            ImGui.Dummy(Vector2.UnitY * 20 * _globalScale);
            DrawColorPickerAvailableFish();
            DrawColorPickerUpcomingFish();
            DrawColorPickerDepAvailableFish();
            DrawColorPickerDepUpcomingFish();

            ImGui.Dummy(Vector2.UnitY * 20 * _globalScale);
            DrawNodeAlarmFormatInput();
            DrawFishAlarmFormatInput();
            DrawItemIdentifiedFormatInput();
            DrawFishIdentifiedFormatInput();
            DrawFishingSpotIdentifiedFormatInput();
        }
    }
}
