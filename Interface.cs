using System;
using ImGuiNET;
using System.Numerics;
using Dalamud.Plugin;
using System.Collections.Generic;
using Gathering;
using Dalamud;

namespace GatherBuddyPlugin
{
    static public class ShowNodes
    {
        static public uint FromBools(bool mining, bool botanist, bool ephemeral, bool unspoiled, bool arr, bool hw, bool sb, bool shb, bool ew)
        {
            var res = 0u;
            if (mining)    res |= 1 << 0;
            if (botanist)  res |= 1 << 1;
            if (ephemeral) res |= 1 << 2;
            if (unspoiled) res |= 1 << 3;
            if (arr)       res |= 1 << 4;
            if (hw)        res |= 1 << 5;
            if (sb)        res |= 1 << 6;
            if (shb)       res |= 1 << 7;
            if (ew)        res |= 1 << 8;
            return res;
        }

        static public void ToBools(uint flags, out bool mining, out bool botanist, out bool ephemeral, out bool unspoiled, out bool arr, out bool hw, out bool sb, out bool shb, out bool ew)
        {
            mining    = (flags & (1 << 0)) == (1 << 0);
            botanist  = (flags & (1 << 1)) == (1 << 1);
            ephemeral = (flags & (1 << 2)) == (1 << 2);
            unspoiled = (flags & (1 << 3)) == (1 << 3);
            arr       = (flags & (1 << 4)) == (1 << 4);
            hw        = (flags & (1 << 5)) == (1 << 5);
            sb        = (flags & (1 << 6)) == (1 << 6);
            shb       = (flags & (1 << 7)) == (1 << 7);
            ew        = (flags & (1 << 8)) == (1 << 8);
        }
    }
    public class Interface
    {
        private readonly GatherBuddy            plugin;
        private readonly DalamudPluginInterface pi;
        private GatherBuddyConfiguration        config;
        private readonly ClientLanguage         lang;

        private int  lastHour           = 0;
        public  bool Visible            = false;
        private bool showMiningNodes    = false;
        private bool showBotanistNodes  = false;
        private bool showEphemeralNodes = false;
        private bool showUnspoiledNodes = false;
        private bool showARRNodes       = false;
        private bool showHWNodes        = false;
        private bool showSBNodes        = false;
        private bool showShBNodes       = false;
        private bool showEWNodes        = false;
        private float minXSize = 0;

        private const string pluginName       = "GatherBuddy";
        private const float horizontalSpace = 5;
        private List<(Node, int)> activeNodes = new();

        void SaveNodes()
        {
            config.ShowNodes = ShowNodes.FromBools(showMiningNodes, showBotanistNodes, showEphemeralNodes
                , showUnspoiledNodes, showARRNodes, showHWNodes, showSBNodes, showShBNodes, showEWNodes);
            pi.SavePluginConfig(config);
        }

        public Interface(GatherBuddy plugin, DalamudPluginInterface pi, GatherBuddyConfiguration config)
        {
            this.pi     = pi;
            this.plugin = plugin;
            this.config = config;
            lang        = pi.ClientState.ClientLanguage;
            ShowNodes.ToBools(config.ShowNodes, out showMiningNodes, out showBotanistNodes, out showEphemeralNodes
                , out showUnspoiledNodes, out showARRNodes, out showHWNodes, out showSBNodes, out showShBNodes, out showEWNodes);
            RebuildList(false);
        }

        private void RebuildList(bool save = true)
        {
            if (save)
                SaveNodes();
            activeNodes = plugin.gatherer.timeline.GetNewList(showUnspoiledNodes, showEphemeralNodes
                , showMiningNodes, showBotanistNodes, showARRNodes, showHWNodes, showSBNodes, showShBNodes, showEWNodes);
            if (activeNodes.Count > 0)
                NodeTimeLine.SortByUptime(lastHour, activeNodes);
        }

        private void HorizontalSpace(float width)
        {
            ImGui.SameLine();
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + width);
        }

        private void DrawGearChangeBox()
        {
            var useGearChange = config.UseGearChange;
            if (ImGui.Checkbox("Gear Change", ref useGearChange))
            {
                config.UseGearChange = useGearChange;
                pi.SavePluginConfig(config);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Toggle whether to automatically switch gear to the correct job gear for a node.\nUses Miner Set and Botanist Set.");
        }

        private void DrawTeleportBox()
        {
            var useTeleport = config.UseTeleport;
            if (ImGui.Checkbox("Teleport", ref useTeleport))
            {
                config.UseTeleport = useTeleport;
                plugin.gatherer.TryCreateTeleporterWatcher(pi, useTeleport);
                pi.SavePluginConfig(config);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Toggle whether to automatically teleport to a chosen node.\nRequires the Teleporter plugin and uses /tp.");
        }

        private void DrawMapMarkerBox()
        {
            var useCoordinates = config.UseCoordinates;
            if (ImGui.Checkbox("Map Marker", ref useCoordinates))
            {
                config.UseCoordinates = useCoordinates;
                pi.SavePluginConfig(config);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Toggle whether to automatically set a map marker on the approximate location of the chosen node.\nRequires the ChatCoordinates plugin and uses /coord.");
        }

        private void DrawRecordBox()
        {
            var doRecord = config.DoRecord;
            if (ImGui.Checkbox("Record", ref doRecord))
            {
                if (doRecord != config.DoRecord)
                {
                    if (doRecord)
                        plugin.gatherer.StartRecording();
                    else
                        plugin.gatherer.StopRecording();
                }
                config.DoRecord = doRecord;
                pi.SavePluginConfig(config);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Toggle whether to record all encountered nodes in regular intervals.\n" +
                                 "Recorded node coordinates are more accurate than pre-programmed ones and will be used for map markers and aetherytes instead.\n" +
                                 "Records are saved in compressed form in the plugin configuration.");
        }

        private void DrawMinerSetInput(float width)
        {
            var minerSet = config.MinerSetName;
            ImGui.SetNextItemWidth(width);
            if (ImGui.InputText( "Miner Set", ref minerSet, 15 ))
            {
                config.MinerSetName = minerSet;
                pi.SavePluginConfig(config);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Set the name of your miner set. Can also be the numerical id instead.");
        }

        private void DrawBotanistSetInput(float width)
        {
            var botanistSet = config.BotanistSetName;
            ImGui.SetNextItemWidth(width);
            if (ImGui.InputText("Botanist Set", ref botanistSet, 15))
            {
                config.BotanistSetName = botanistSet;
                pi.SavePluginConfig(config);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Set the name of your botanist set. Can also be the numerical id instead.");
        }

        private void DrawSnapshotButton(float width)
        {
            if (ImGui.Button("Snapshot", new Vector2(width, 0)))
                pi.Framework.Gui.Chat.Print($"Recorded {plugin.gatherer.Snapshot()} new nearby gathering nodes.");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Record currently available nodes around you once.");
        }

        private void DrawTimedNodes(float widgetHeight)
        {
            ImGui.BeginChild( "Nodes", new Vector2( -1, - widgetHeight - ImGui.GetStyle().FramePadding.Y), true);
            foreach (var (N, _) in activeNodes)
            {
                Vector4 colors = new(0.8f, 0.8f, 0.8f, 1f);

                if (N.times.IsUp(lastHour))
                    colors = new(0, 1, 0, 1);
                else if (N.times.IsUp(lastHour + 1))
                    colors = new(0.6f, 1, 0.4f, 1);
                else if (N.times.IsUp(lastHour + 2))
                    colors = new(1f, 1f, 0, 1);
                else if (N.times.IsUp(lastHour + 3))
                    colors = new(1f, 1f, 0.2f, 1);
                else if (N.times.IsUp(lastHour + 4))
                    colors = new(1f, 1f, 0.6f, 1);

                ImGui.PushStyleColor(ImGuiCol.Text, colors);

                if (ImGui.Selectable(N.items.PrintItems(", ", pi.ClientState.ClientLanguage)))
                {
                    plugin.gatherer.OnGatherActionWithNode(N);
                }
                ImGui.PopStyleColor();

                if (ImGui.IsItemHovered())
                {
                    var coords = (N.GetX() != 0) ? $"({N.GetX()}|{N.GetY()})" : "(Unknown Location)";
                    var tooltip = $"{N.nodes.territory.nameList[lang]}, {coords} - {N.GetClosestAetheryte().nameList[lang]}\n" +
                                  $"{N.meta.nodeType}, up at {N.times.PrintHours()}\n" +
                                  $"{N.meta.gatheringType} at {N.meta.level}";
                    ImGui.SetTooltip(tooltip);
                }
            }
            ImGui.EndChild();
        }

        private void DrawVisibilityBox(ref bool value, string label, string tooltip)
        {
            if (ImGui.Checkbox(label, ref value))
                RebuildList();
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(tooltip);
        }

        private void DrawMinerBox()
            => DrawVisibilityBox(ref showMiningNodes, "Miner", "Show timed nodes for miners.");

        private void DrawBotanistBox()
            => DrawVisibilityBox(ref showBotanistNodes, "Botanist", "Show timed nodes for botanists.");

        private void DrawUnspoiledBox()
            => DrawVisibilityBox(ref showUnspoiledNodes, "Unspoiled", "Show unspoiled nodes.");

        private void DrawEphemeralBox()
            => DrawVisibilityBox(ref showEphemeralNodes, "Ephemeral", "Show ephemeral nodes.");

        private void DrawArrBox()
            => DrawVisibilityBox(ref showARRNodes, "ARR", "Show nodes with level 1 to 50.");

        private void DrawHeavenswardBox()
            => DrawVisibilityBox(ref showHWNodes, "HW", "Show nodes with level 51 to 60.");

        private void DrawStormbloodBox()
            => DrawVisibilityBox(ref showSBNodes, "SB", "Show nodes with level 61 to 70.");

        private void DrawShadowbringersBox()
            => DrawVisibilityBox(ref showShBNodes, "ShB", "Show nodes with level 71 to 80.");

        private void DrawEndwalkerBox()
            => DrawVisibilityBox(ref showEWNodes, "EW", "Show nodes with level 81 to 90.");

        private void DrawTimedTab(float space)
        {
            var boxHeight = 2 * ImGui.GetTextLineHeightWithSpacing() + ImGui.GetStyle().ItemSpacing.X +
                            ImGui.GetStyle().FramePadding.X * 4;
            DrawTimedNodes(boxHeight + ImGui.GetStyle().FramePadding.Y);
            var checkBoxAdd = ImGui.GetStyle().ItemSpacing.X * 3 + ImGui.GetStyle().FramePadding.X * 2 +
                              ImGui.GetTextLineHeight();
            var jobBoxWidth = ImGui.CalcTextSize("Botanist").X + checkBoxAdd;
            var typeBoxWidth = Math.Max(ImGui.CalcTextSize("Unspoiled").X, ImGui.CalcTextSize("Ephemeral").X) +
                               checkBoxAdd;
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
            ImGui.EndTabItem();
        }

        private void DrawAlertsTab()
        {

        }

        public void Draw()
        {
            if (!Visible)
                return;

            if (activeNodes.Count > 0)
            {
                var hour = EorzeaTime.CurrentHours();
                if (hour != lastHour)
                {
                    lastHour = hour;
                    NodeTimeLine.SortByUptime(lastHour, activeNodes);
                }
            }

            var globalScale = ImGui.GetIO().FontGlobalScale;
            var buttonSize = ImGui.CalcTextSize("Snapshot").X + 4 * ImGui.GetStyle().FramePadding.X * globalScale;
            var inputSize = Math.Max(ImGui.CalcTextSize("Miner Set").X, ImGui.CalcTextSize("Botanist Set").X) +
                            4 * ImGui.GetStyle().ItemSpacing.X;
            var space = horizontalSpace * globalScale;
            if (minXSize == 0)
                minXSize = inputSize * 5;

            ImGui.SetNextWindowSizeConstraints(
                new Vector2(minXSize, ImGui.GetTextLineHeightWithSpacing() * 16),
                new Vector2(minXSize * 2, ImGui.GetIO().DisplaySize.Y * 15 / 16));

            if (!ImGui.Begin(pluginName, ref Visible ))
                return;

            ImGui.BeginGroup();
            ImGui.BeginGroup();
            
            DrawMinerSetInput(inputSize);
            DrawBotanistSetInput(inputSize);
            ImGui.EndGroup();

            ImGui.SameLine();
            HorizontalSpace(space);
            ImGui.BeginGroup();
            DrawGearChangeBox();
            DrawTeleportBox();
            ImGui.EndGroup();

            ImGui.SameLine();
            HorizontalSpace(space);
            ImGui.BeginGroup();
            DrawRecordBox();
            DrawMapMarkerBox();
            ImGui.EndGroup();

            ImGui.SameLine();
            HorizontalSpace(3 * space);
            ImGui.BeginGroup();
            DrawSnapshotButton(buttonSize);
            ImGui.EndGroup();
            ImGui.EndGroup();

            if (!ImGui.BeginTabBar("##Tabs"))
                return;

            minXSize = ImGui.GetItemRectSize().X + 2 * ImGui.GetStyle().WindowPadding.X;

            if (ImGui.BeginTabItem("Timed Nodes"))
                DrawTimedTab(space);
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Shows timed nodes corresponding to the selection of the checkmarks below, sorted by next uptime.\n" +
                                 "Click on a node to do a /gather command for that node.");

            if (ImGui.BeginTabItem("Alerts"))
                DrawAlertsTab();
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Setup alarms for specific timed gathering nodes.");

            ImGui.EndTabBar();

            minXSize = Math.Max(minXSize, ImGui.GetItemRectSize().X + 2 * ImGui.GetStyle().WindowPadding.X);

            ImGui.End();
        }
    }
}