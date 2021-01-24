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
        static public uint FromBools(bool mining, bool botanist, bool ephemeral, bool unspoiled, bool arr, bool hw, bool sb, bool shb)
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
            return res;
        }

        static public void ToBools(uint flags, out bool mining, out bool botanist, out bool ephemeral, out bool unspoiled, out bool arr, out bool hw, out bool sb, out bool shb)
        {
            mining    = ((flags >> 0) & 1) == 1;
            botanist  = ((flags >> 1) & 1) == 1;
            ephemeral = ((flags >> 2) & 1) == 1;
            unspoiled = ((flags >> 3) & 1) == 1;
            arr       = ((flags >> 4) & 1) == 1;
            hw        = ((flags >> 5) & 1) == 1;
            sb        = ((flags >> 6) & 1) == 1;
            shb       = ((flags >> 7) & 1) == 1;
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

        private static readonly string pluginName       = "GatherBuddy";
        private static readonly Vector2 MinSettingsSize = new Vector2( 500, 400 );
        private static readonly Vector2 MaxSettingsSize = new Vector2( 1000, 4000 );
        private readonly float widgetHeight             = 40f;
        private readonly float jobBlockWidth            = MinSettingsSize.X / 2 - ImGui.GetStyle().FrameBorderSize * 2 - ImGui.GetStyle().FramePadding.X * 3f;
        private readonly float expansionBlockWidth      = MinSettingsSize.X - ImGui.GetStyle().FrameBorderSize * 2 - ImGui.GetStyle().FramePadding.X * 4;
        private readonly float jobBlockStart            = MinSettingsSize.X / 2 - ImGui.GetStyle().FramePadding.X - ImGui.GetStyle().FrameBorderSize * 2;
        private readonly float recordOffset             = ImGui.CalcTextSize("Record").X + 30f + ImGui.GetStyle().FramePadding.X * 4;
        
        private List<(Node, int)> activeNodes = new();

        void SaveNodes()
        {
            config.ShowNodes = ShowNodes.FromBools(showMiningNodes, showBotanistNodes, showEphemeralNodes
                , showUnspoiledNodes, showARRNodes, showHWNodes, showSBNodes, showShBNodes);
            pi.SavePluginConfig(config);
        }

        public Interface(GatherBuddy plugin, DalamudPluginInterface pi, GatherBuddyConfiguration config)
        {
            this.pi     = pi;
            this.plugin = plugin;
            this.config = config;
            lang        = pi.ClientState.ClientLanguage;
            ShowNodes.ToBools(config.ShowNodes, out showMiningNodes, out showBotanistNodes, out showEphemeralNodes
                , out showUnspoiledNodes, out showARRNodes, out showHWNodes, out showSBNodes, out showShBNodes);
            RebuildList(false);
        }

        private void RebuildList(bool save = true)
        {
            if (save)
                SaveNodes();
            activeNodes = plugin.gatherer.timeline.GetNewList(showUnspoiledNodes, showEphemeralNodes
                , showMiningNodes, showBotanistNodes, showARRNodes, showHWNodes, showSBNodes, showShBNodes);
            if (activeNodes.Count > 0)
                NodeTimeLine.SortByUptime(lastHour, activeNodes);
        }

        private void HorizontalSpace(float width)
        {
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(width, 0));
            ImGui.SameLine();
        }

        private void VerticalSpace(float height)
        {
            ImGui.Dummy(new Vector2(0, height));
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

            var width = ImGui.GetWindowWidth();
            var secondRowWidth = width / 2;
            var thirdRowWidth  = width / 4;
            var fourthRowWidth = (width * 10) / 35;

            ImGui.SetNextWindowSizeConstraints( MinSettingsSize, MaxSettingsSize );
            if (!ImGui.Begin(pluginName, ref Visible ))
                return;

            var useGearChange = config.UseGearChange;
            if (ImGui.Checkbox("Gear Change", ref useGearChange))
            {
                config.UseGearChange = useGearChange;
                pi.SavePluginConfig(config);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Toggle whether to automatically switch gear to the correct job gear for a node.\nUses Miner Set and Botanist Set.");

            HorizontalSpace(ImGui.GetStyle().FramePadding.X * 2);
            var useTeleport = config.UseTeleport;
            if (ImGui.Checkbox("Teleport", ref useTeleport))
            {
                config.UseTeleport = useTeleport;
                pi.SavePluginConfig(config);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Toggle whether to automatically teleport to a chosen node.\nRequires the Teleporter plugin and uses /tp.");

            HorizontalSpace(ImGui.GetStyle().FramePadding.X * 2);
            var useCoordinates = config.UseCoordinates;
            if (ImGui.Checkbox("Map Marker", ref useCoordinates))
            {
                config.UseCoordinates = useCoordinates;
                pi.SavePluginConfig(config);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Toggle whether to automatically set a map marker on the approximate location of the chosen node.\nRequires the ChatCoordinates plugin and uses /coord.");

            var doRecord = config.DoRecord;
            ImGui.SameLine(MinSettingsSize.X - recordOffset);
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

            VerticalSpace(ImGui.GetTextLineHeight() / 2);
            var minerSet = config.MinerSetName;
            ImGui.PushItemWidth(secondRowWidth / 2);
            if (ImGui.InputText( "Miner Set", ref minerSet, 15 ))
            {
                config.MinerSetName = minerSet;
                pi.SavePluginConfig(config);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Set the name of your miner set. Can also be the numerical id instead.");

            ImGui.PushItemWidth(secondRowWidth / 2);
            ImGui.SameLine(secondRowWidth);
            var botanistSet = config.BotanistSetName;
            if (ImGui.InputText("Botanist Set", ref botanistSet, 15))
            {
                config.BotanistSetName = botanistSet;
                pi.SavePluginConfig(config);
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Set the name of your botanist set. Can also be the numerical id instead.");

            ImGui.SameLine(MinSettingsSize.X - recordOffset);
            if (ImGui.Button("Snapshot", new Vector2(recordOffset-10, 0)))
                pi.Framework.Gui.Chat.Print($"Recorded {plugin.gatherer.Snapshot()} new nearby gathering nodes.");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Record currently available nodes around you once.");

            VerticalSpace(ImGui.GetTextLineHeight() / 2);
            ImGui.BeginGroup();
            ImGui.Text("Timed Nodes");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Shows timed nodes corresponding to the selection of the checkmarks below, sorted by next uptime.\n" +
                    "Click on a node to do a /gather command for that node.");

            ImGui.BeginChild( "Nodes", new Vector2( -1, -ImGui.GetFrameHeightWithSpacing() - 2 * widgetHeight ), true);
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
            
            VerticalSpace(ImGui.GetTextLineHeight() / 4);

            ImGui.BeginChild("Jobs", new Vector2(jobBlockWidth, widgetHeight), true);
            if (ImGui.Checkbox("Miner", ref showMiningNodes))
                RebuildList();
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show timed nodes for miners.");

            ImGui.SameLine(fourthRowWidth);
            if (ImGui.Checkbox("Botanist", ref showBotanistNodes))
                RebuildList();
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show timed nodes for botanists.");
            ImGui.EndChild();

            ImGui.SameLine(jobBlockStart);
            ImGui.BeginChild("Types", new Vector2(jobBlockWidth, widgetHeight), true);
            if (ImGui.Checkbox("Unspoiled", ref showUnspoiledNodes))
                RebuildList();
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show unspoiled nodes.");

            ImGui.SameLine(fourthRowWidth);
            if (ImGui.Checkbox("Ephemeral", ref showEphemeralNodes))
                RebuildList();
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show ephemeral nodes.");
            ImGui.EndChild();

            ImGui.Dummy(new Vector2(0, ImGui.GetTextLineHeight() / 4));
            ImGui.BeginChild("Expansions", new Vector2(expansionBlockWidth, widgetHeight), true);
            if (ImGui.Checkbox("ARR", ref showARRNodes))
                RebuildList();
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show nodes with level 1 to 50.");

            ImGui.SameLine(fourthRowWidth);
            if (ImGui.Checkbox("Heavensward", ref showHWNodes))
                RebuildList();
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show nodes with level 51 to 60.");

            ImGui.SameLine(2 * fourthRowWidth);
            if (ImGui.Checkbox("Stormblood", ref showSBNodes))
                RebuildList();
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show nodes with level 61 to 70.");

            ImGui.SameLine(3 * fourthRowWidth);
            if (ImGui.Checkbox("Shadowbringers", ref showShBNodes))
                RebuildList();
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show nodes with level 71 to 80.");
            ImGui.EndChild();

            ImGui.End();
        }
    }
}