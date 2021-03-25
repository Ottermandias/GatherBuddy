using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using Dalamud;
using Dalamud.Plugin;
using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.SEFunctions;
using GatherBuddy.Utility;
using ImGuiNET;

namespace GatherBuddy
{
    public class Interface
    {
        private readonly GatherBuddy              _plugin;
        private readonly DalamudPluginInterface   _pi;
        private readonly GatherBuddyConfiguration _config;
        private readonly ClientLanguage           _lang;

        private int  _lastHour = 0;
        public  bool Visible;

        private float _minXSize;

        private const string            PluginName             = "GatherBuddy";
        private const float             DefaultHorizontalSpace = 5;
        private       List<(Node, int)> _activeNodes           = new();

        private          string _newAlarmName            = "";
        private          int    _newAlertIdx             = 0;
        private readonly float  _longestNodeStringLength = 0f;

        private void SaveNodes()
            => _pi.SavePluginConfig(_config);

        public Interface(GatherBuddy plugin, DalamudPluginInterface pi, GatherBuddyConfiguration config)
        {
            _pi     = pi;
            _plugin = plugin;
            _config = config;
            _lang   = pi.ClientState.ClientLanguage;
            RebuildList(false);

            _allTimedNodesNames = plugin.Alarms!.AllTimedNodes
                .Select(n => $"{n.Times!.PrintHours(true)}: {n.Items!.PrintItems(", ", _lang)}")
                .ToArray();
            _longestNodeStringLength = _allTimedNodesNames.Max(n => ImGui.CalcTextSize(n).X);
        }

        private readonly string[] _allTimedNodesNames;

        private void RebuildList(bool save = true)
        {
            if (save)
                SaveNodes();
            _activeNodes = _plugin.Gatherer!.Timeline.GetNewList(_config.ShowNodes);
            if (_activeNodes.Count > 0)
                NodeTimeLine.SortByUptime(_lastHour, _activeNodes);
        }

        private static void HorizontalSpace(float width)
        {
            ImGui.SameLine();
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + width);
        }

        private void DrawCheckbox(string label, string tooltip, bool current, Action<bool> setter)
        {
            var tmp = current;
            if (ImGui.Checkbox(label, ref tmp) && tmp != current)
            {
                setter(tmp);
                _pi.SavePluginConfig(_config);
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(tooltip);
        }

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


        private void DrawRecordBox()
            => DrawCheckbox("Record",
                "Toggle whether to record all encountered nodes in regular intervals.\n"
              + "Recorded node coordinates are more accurate than pre-programmed ones and will be used for map markers and aetherytes instead.\n"
              + "Records are saved in compressed form in the plugin configuration.",
                _config.DoRecord, b =>
                {
                    if (b)
                        _plugin.Gatherer!.StartRecording();
                    else
                        _plugin.Gatherer!.StopRecording();
                    _config.DoRecord = b;
                });

        private void DrawAlarmToggle()
            => DrawCheckbox("Alarms",  "Toggle all alarms on or off.",
                _config.AlarmsEnabled, b =>
                {
                    if (b)
                        _plugin.Alarms!.Enable();
                    else
                        _plugin.Alarms!.Disable();
                    _config.AlarmsEnabled = b;
                });

        private void DrawSetInput(float width, string jobName, string oldName, Action<string> setName)
        {
            var tmp = oldName;
            ImGui.SetNextItemWidth(width);
            if (ImGui.InputText($"{jobName} Set", ref tmp, 15) && tmp != oldName)
            {
                setName(tmp);
                _pi.SavePluginConfig(_config);
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip($"Set the name of your {jobName.ToLowerInvariant()} set. Can also be the numerical id instead.");
        }

        private void DrawMinerSetInput(float width)
            => DrawSetInput(width, "Miner", _config.MinerSetName, s => _config.MinerSetName = s);

        private void DrawBotanistSetInput(float width)
            => DrawSetInput(width, "Botanist", _config.BotanistSetName, s => _config.BotanistSetName = s);

        private void DrawFisherSetInput(float width)
            => DrawSetInput(width, "Fisher", _config.FisherSetName, s => _config.FisherSetName = s);

        private void DrawSnapshotButton(float width)
        {
            if (ImGui.Button("Snapshot", new Vector2(-1, 0)))
                _pi.Framework.Gui.Chat.Print($"Recorded {_plugin.Gatherer!.Snapshot()} new nearby gathering nodes.");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Record currently available nodes around you once.");
        }

        private void DrawTimedNodes(float widgetHeight)
        {
            ImGui.BeginChild("Nodes", new Vector2(-1, -widgetHeight - ImGui.GetStyle().FramePadding.Y), true);
            foreach (var (n, _) in _activeNodes)
            {
                Vector4 colors = new(0.8f, 0.8f, 0.8f, 1f);

                if (n.Times!.IsUp(_lastHour))
                    colors = new Vector4(0, 1, 0, 1);
                else if (n.Times.IsUp(_lastHour + 1))
                    colors = new Vector4(0.6f, 1, 0.4f, 1);
                else if (n.Times.IsUp(_lastHour + 2))
                    colors = new Vector4(1f, 1f, 0, 1);
                else if (n.Times.IsUp(_lastHour + 3))
                    colors = new Vector4(1f, 1f, 0.2f, 1);
                else if (n.Times.IsUp(_lastHour + 4))
                    colors = new Vector4(1f, 1f, 0.6f, 1);

                ImGui.PushStyleColor(ImGuiCol.Text, colors);

                if (ImGui.Selectable(n.Items!.PrintItems(", ", _pi.ClientState.ClientLanguage)))
                    _plugin.Gatherer!.OnGatherActionWithNode(n);
                ImGui.PopStyleColor();

                if (!ImGui.IsItemHovered())
                    continue;

                var coords = n.GetX() != 0 ? $"({n.GetX()}|{n.GetY()})" : "(Unknown Location)";
                var tooltip = $"{n.Nodes!.Territory!.NameList[_lang]}, {coords} - {n.GetClosestAetheryte()?.NameList[_lang] ?? ""}\n"
                  + $"{n.Meta!.NodeType}, up at {n.Times!.PrintHours()}\n"
                  + $"{n.Meta!.GatheringType} at {n.Meta!.Level}";
                ImGui.SetTooltip(tooltip);
            }

            ImGui.EndChild();
        }

        private void DrawVisibilityBox(ShowNodes flag, string label, string tooltip)
        {
            var tmp = _config.ShowNodes.HasFlag(flag);
            if (ImGui.Checkbox(label, ref tmp))
            {
                if (tmp)
                    _config.ShowNodes |= flag;
                else
                    _config.ShowNodes &= ~flag;
                RebuildList();
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(tooltip);
        }

        private void DrawMinerBox()
            => DrawVisibilityBox(ShowNodes.Mining, "Miner", "Show timed nodes for miners.");

        private void DrawBotanistBox()
            => DrawVisibilityBox(ShowNodes.Botanist, "Botanist", "Show timed nodes for botanists.");

        private void DrawUnspoiledBox()
            => DrawVisibilityBox(ShowNodes.Unspoiled, "Unspoiled", "Show unspoiled nodes.");

        private void DrawEphemeralBox()
            => DrawVisibilityBox(ShowNodes.Ephemeral, "Ephemeral", "Show ephemeral nodes.");

        private void DrawArrBox()
            => DrawVisibilityBox(ShowNodes.ARealmReborn, "ARR", "Show nodes with level 1 to 50.");

        private void DrawHeavenswardBox()
            => DrawVisibilityBox(ShowNodes.Heavensward, "HW", "Show nodes with level 51 to 60.");

        private void DrawStormbloodBox()
            => DrawVisibilityBox(ShowNodes.Stormblood, "SB", "Show nodes with level 61 to 70.");

        private void DrawShadowbringersBox()
            => DrawVisibilityBox(ShowNodes.Shadowbringers, "ShB", "Show nodes with level 71 to 80.");

        private void DrawEndwalkerBox()
            => DrawVisibilityBox(ShowNodes.Endwalker, "EW", "Show nodes with level 81 to 90.");

        private void DrawTimedTab(float space)
        {
            var boxHeight = 2 * ImGui.GetTextLineHeightWithSpacing() + ImGui.GetStyle().ItemSpacing.X + ImGui.GetStyle().FramePadding.X * 4;
            DrawTimedNodes(boxHeight + ImGui.GetStyle().FramePadding.Y);
            var checkBoxAdd  = ImGui.GetStyle().ItemSpacing.X * 3 + ImGui.GetStyle().FramePadding.X * 2 + ImGui.GetTextLineHeight();
            var jobBoxWidth  = ImGui.CalcTextSize("Botanist").X + checkBoxAdd;
            var typeBoxWidth = Math.Max(ImGui.CalcTextSize("Unspoiled").X, ImGui.CalcTextSize("Ephemeral").X) + checkBoxAdd;
            ImGui.BeginChild("Jobs", new Vector2(jobBoxWidth, boxHeight), true);
            DrawMinerBox();
            DrawBotanistBox();
            ImGui.EndChild();

            ImGui.SameLine();
            ImGui.BeginChild("Types", new Vector2(typeBoxWidth, boxHeight), true);
            DrawUnspoiledBox();
            DrawEphemeralBox();
            ImGui.EndChild();

            ImGui.SameLine();
            ImGui.BeginChild("Expansion", new Vector2(0, boxHeight), true);
            ImGui.BeginGroup();
            DrawArrBox();
            DrawHeavenswardBox();
            ImGui.EndGroup();
            HorizontalSpace(space);
            ImGui.BeginGroup();
            DrawStormbloodBox();
            DrawShadowbringersBox();
            ImGui.EndGroup();
            HorizontalSpace(space);
            DrawEndwalkerBox();
            ImGui.EndChild();
        }

        private static readonly string[] SoundNameList = Enum.GetNames(typeof(Sounds)).Where(s => s != "Unknown").ToArray();

        private void DrawDeleteAndEnable(float space)
        {
            ImGui.BeginGroup();
            ImGui.Dummy(new Vector2(0, ImGui.GetTextLineHeightWithSpacing()));
            for (var idx = 0; idx < _plugin.Alarms!.Alarms.Count; ++idx)
            {
                var alert = _plugin.Alarms.Alarms[idx];
                if (ImGui.Button($"  -  ##{idx}"))
                    _plugin.Alarms.RemoveNode(idx--);
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Delete this alarm.");

                HorizontalSpace(space);
                var enabled = alert.Enabled;
                if (ImGui.Checkbox($"##enabled_{idx}", ref enabled) && enabled != alert.Enabled)
                    _plugin.Alarms.ChangeNodeStatus(idx, enabled);
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Enable or disable this alarm.");
            }

            ImGui.EndGroup();
        }

        public void DrawNames(float space)
        {
            ImGui.BeginGroup();
            ImGui.Text("Name");
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, ImGui.GetTextLineHeightWithSpacing()));
            foreach (var alert in _plugin.Alarms!.Alarms)
            {
                ImGui.AlignTextToFramePadding();
                ImGui.Text(alert.Name);
                HorizontalSpace(space);
                ImGui.NewLine();
            }

            ImGui.EndGroup();
        }

        public void DrawOffsets(float space)
        {
            ImGui.BeginGroup();
            ImGui.Text("Pre");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Trigger the respective alarm the given number (0-1439) of Eorzea Minutes earlier.");
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, ImGui.GetTextLineHeightWithSpacing()));
            for (var idx = 0; idx < _plugin.Alarms!.Alarms.Count; ++idx)
            {
                var alert  = _plugin.Alarms.Alarms[idx];
                var offset = alert.MinuteOffset.ToString();
                ImGui.SetNextItemWidth(ImGui.CalcTextSize("99999").X);
                if (!ImGui.InputText($"##Offset{idx}", ref offset, 4, ImGuiInputTextFlags.CharsDecimal))
                    continue;

                if (int.TryParse(offset, out var minutes))
                {
                    minutes %= 24 * 60;
                    if (minutes != alert.MinuteOffset)
                        _plugin.Alarms.ChangeNodeOffset(idx, minutes);
                }
                else if (offset.Length == 0 && alert.MinuteOffset != 0)
                {
                    _plugin.Alarms.ChangeNodeOffset(idx, 0);
                }
            }

            ImGui.EndGroup();
        }

        private void DrawAlarms()
        {
            var boxSize = ImGui.CalcTextSize("Sound9999999").X;
            ImGui.BeginGroup();
            ImGui.Text("Alarm Sound");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Set an optional sound that should be played when this alarm triggers.\n"
                  + "The sounds are the same as in the character configuration -> log window -> notification sounds.");
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, ImGui.GetTextLineHeightWithSpacing()));
            for (var idx = 0; idx < _plugin.Alarms!.Alarms.Count; ++idx)
            {
                var alert = _plugin.Alarms.Alarms[idx];
                var sound = alert.SoundId.ToIdx();
                if (sound == -1)
                {
                    _plugin.Alarms.ChangeNodeSound(idx, Sounds.None);
                    sound = 0;
                }

                ImGui.SetNextItemWidth(boxSize);
                if (!ImGui.Combo($"##sound_{idx}", ref sound, SoundNameList, SoundNameList.Length))
                    continue;

                var tmp = SoundsExtensions.FromIdx(sound);
                if (tmp != Sounds.Unknown && tmp != alert.SoundId)
                    _plugin.Alarms.ChangeNodeSound(idx, tmp);
            }

            ImGui.EndGroup();
        }

        private void DrawPrintMessageBoxes()
        {
            ImGui.BeginGroup();
            ImGui.Text("Chat");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Toggle whether the alarm is printed to chat or not.");
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, ImGui.GetTextLineHeightWithSpacing()));
            for (var idx = 0; idx < _plugin.Alarms!.Alarms.Count; ++idx)
            {
                var alert = _plugin.Alarms.Alarms[idx];
                var print = alert.PrintMessage;
                if (ImGui.Checkbox($"##print{idx}", ref print) && print != alert.PrintMessage)
                    _plugin.Alarms.ChangePrintStatus(idx, print);
            }

            ImGui.EndGroup();
        }

        private void DrawHours()
        {
            ImGui.BeginGroup();
            ImGui.Text("Alarm Times");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("The uptimes for the node monitored in this alarm. Hover for the items.");
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, ImGui.GetTextLineHeightWithSpacing()));
            foreach (var alert in _plugin.Alarms!.Alarms)
            {
                ImGui.AlignTextToFramePadding();
                ImGui.Text(alert.Node!.Times!.PrintHours(true, " | "));
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip(alert.Node!.Items!.PrintItems("\n", _lang));
            }

            ImGui.EndGroup();
        }

        private bool   _focusComboFilter = false;
        private string _nodeFilter       = "";

        private void DrawNewAlarm()
        {
            if (ImGui.Button("  + "))
            {
                _plugin.Alarms!.AddNode(_newAlarmName, _plugin.Alarms!.AllTimedNodes[_newAlertIdx]);
                _newAlarmName = "";
            }

            ImGui.SameLine();
            ImGui.SetNextItemWidth(ImGui.CalcTextSize("mmmmmmmmmmmm").X);
            ImGui.InputTextWithHint("##Name", "New Alarm Name", ref _newAlarmName, 64);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(-1);
            if (ImGui.BeginCombo("##Node", _allTimedNodesNames[_newAlertIdx]))
            {
                ImGui.SetNextItemWidth(-1);
                ImGui.InputTextWithHint("##NodeFilter", "Filter", ref _nodeFilter, 60);
                var isFocused = ImGui.IsItemActive();
                if (!_focusComboFilter)
                    ImGui.SetKeyboardFocusHere();

                if (!ImGui.BeginChild("##nodeList",
                    new Vector2(_longestNodeStringLength * ImGui.GetIO().FontGlobalScale,
                        ImGui.GetTextLineHeightWithSpacing() * 6)))
                {
                    ImGui.EndCombo();
                    return;
                }

                if (!_focusComboFilter)
                {
                    ImGui.SetScrollY(0);
                    _focusComboFilter = true;
                }

                var filter   = _nodeFilter.ToLowerInvariant();
                var numNodes = 0;
                var node     = 0;
                for (var i = 0; i < _allTimedNodesNames.Length; ++i)
                {
                    if (!_allTimedNodesNames[i].ToLowerInvariant().Contains(filter))
                        continue;

                    ++numNodes;
                    node = i;
                    if (!ImGui.Selectable(_allTimedNodesNames[i], i == _newAlertIdx))
                        continue;

                    _newAlertIdx = i;
                    ImGui.CloseCurrentPopup();
                }

                ImGui.EndChild();
                if (!isFocused && numNodes <= 1)
                {
                    _newAlertIdx = node;
                    ImGui.CloseCurrentPopup();
                }

                ImGui.EndCombo();
            }
            else if (_focusComboFilter)
            {
                _focusComboFilter = false;
                _nodeFilter       = "";
            }
        }

        private void DrawAlarmsTab()
        {
            var space    = ImGui.GetStyle().ItemSpacing.X / 2;
            var listSize = new Vector2(-1, -ImGui.GetTextLineHeightWithSpacing() - 2 * ImGui.GetStyle().FramePadding.X);
            if (ImGui.BeginChild("##alarmlist", listSize, true))
            {
                DrawDeleteAndEnable(space);
                ImGui.SameLine();
                DrawNames(space);
                ImGui.SameLine();
                DrawOffsets(space);
                ImGui.SameLine();
                DrawAlarms();
                ImGui.SameLine();
                DrawPrintMessageBoxes();
                ImGui.SameLine();
                DrawHours();
                ImGui.EndChild();
            }

            DrawNewAlarm();
        }

        private void DrawFormatInput(string label, string tooltip, string oldValue, string defaultValue, Action<string> setValue)
        {
            var tmp = oldValue;

            if (ImGui.Button($"Default##{label}"))
            {
                setValue(defaultValue);
                _pi.SavePluginConfig(_config);
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(defaultValue);

            HorizontalSpace(10);
            ImGui.AlignTextToFramePadding();
            ImGui.Text(label);

            ImGui.SetNextItemWidth(-ImGui.GetStyle().FramePadding.X);
            if (ImGui.InputText($"##{label}", ref tmp, 256) && tmp != oldValue)
            {
                setValue(tmp);
                _pi.SavePluginConfig(_config);
            }

            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(tooltip);
        }

        private void DrawAlarmFormatInput()
            => DrawFormatInput("Alarm Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Name} with the alarm name.\n- {Offset} with the alarm offset.\n- {TimesShort} with the corresponding uptimes in ##-##|##-## format.\n- {TimesLong} with the corresponding uptimes in long format.\n- {AllItems} with the corresponding items, separated by comma and in client language.",
                _config.AlarmFormat, GatherBuddyConfiguration.DefaultAlarmFormat,
                s => _config.AlarmFormat = s);

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

        private void DrawSettingsTab(float inputSize)
        {
            if (!ImGui.BeginChild("##settingsList", new Vector2(-1, -1), true))
                return;

            DrawMinerSetInput(inputSize);
            DrawBotanistSetInput(inputSize);
            DrawFisherSetInput(inputSize);

            DrawGearChangeBox();
            DrawTeleportBox();
            DrawMapMarkerBox();

            var printNodeUptimes = _config.PrintUptime;
            if (ImGui.Checkbox("Print Node Uptimes", ref printNodeUptimes) && printNodeUptimes != _config.PrintUptime)
            {
                _config.PrintUptime = printNodeUptimes;
                _pi.SavePluginConfig(_config);
            }

            DrawFishTimerBox();
            DrawFishTimerEditBox();
            DrawFishTimerHideBox();

            ImGui.Dummy(new Vector2(0, 20));
            DrawAlarmFormatInput();
            DrawItemIdentifiedFormatInput();
            DrawFishIdentifiedFormatInput();
            DrawFishingSpotIdentifiedFormatInput();

            ImGui.EndChild();
        }

        public void Draw()
        {
            if (!Visible)
                return;

            if (_activeNodes.Count > 0)
            {
                var hour = EorzeaTime.CurrentHours();
                if (hour != _lastHour)
                {
                    _lastHour = hour;
                    NodeTimeLine.SortByUptime(_lastHour, _activeNodes);
                }
            }

            var globalScale = ImGui.GetIO().FontGlobalScale;
            var buttonSize  = ImGui.CalcTextSize("Snapshot").X + 4 * ImGui.GetStyle().FramePadding.X * globalScale;
            var inputSize = Math.Max(ImGui.CalcTextSize("Miner Set").X, ImGui.CalcTextSize("Botanist Set").X)
              + 4 * ImGui.GetStyle().ItemSpacing.X;

            var space = DefaultHorizontalSpace * globalScale;
            _minXSize = inputSize * 4.25f;

            ImGui.SetNextWindowSizeConstraints(
                new Vector2(_minXSize,     ImGui.GetTextLineHeightWithSpacing() * 16),
                new Vector2(_minXSize * 4, ImGui.GetIO().DisplaySize.Y * 15 / 16));

            if (!ImGui.Begin(PluginName, ref Visible))
                return;

            ImGui.BeginGroup();
            DrawAlarmToggle();
            HorizontalSpace(5 * space);
            DrawRecordBox();
            HorizontalSpace(5 * space);
            DrawSnapshotButton(buttonSize);
            ImGui.EndGroup();
            ImGui.Dummy(new Vector2(0, space / 2));

            if (!ImGui.BeginTabBar("##Tabs", ImGuiTabBarFlags.NoTooltip))
                return;

            var timedTab = ImGui.BeginTabItem("Timed Nodes");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Shows timed nodes corresponding to the selection of the checkmarks below, sorted by next uptime.\n"
                  + "Click on a node to do a /gather command for that node.");
            if (timedTab)
            {
                DrawTimedTab(space);
                ImGui.EndTabItem();
            }

            var alertTab = ImGui.BeginTabItem("Alarms");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Setup alarms for specific timed gathering nodes.\n"
                  + "You can use [/gather alarm] to directly gather the last triggered alarm.");
            if (alertTab)
            {
                DrawAlarmsTab();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Settings"))
            {
                DrawSettingsTab(inputSize);
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
            ImGui.End();
        }
    }
}
