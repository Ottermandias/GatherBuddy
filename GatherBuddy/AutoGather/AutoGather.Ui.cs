using Dalamud.Game.ClientState.Objects.Enums;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using GatherBuddy.Plugin;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace GatherBuddy.AutoGather
{
    public static class AutoGatherUI
    {
        public class CollectableDebugUi : Window
        {
            public unsafe override bool DrawConditions()
            {
                var gatheringMasterpiece = (AddonGatheringMasterpiece*)Dalamud.GameGui.GetAddonByName("GatheringMasterpiece", 1);
                if (gatheringMasterpiece == null)
                    return false;

                return !gatheringMasterpiece->AtkUnitBase.IsVisible;
            }

            public CollectableDebugUi() : base("GBR Collectable Replacement", ImGuiWindowFlags.Modal | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoNavFocus, false)
            {
                Size          = new Vector2(100, 60);
                SizeCondition = ImGuiCond.FirstUseEver;
                IsOpen        = true;
            }

            public override void Draw()
            {
                ImGui.Text($"GBR Collectable Replacement Window");
                ImGui.Text($"Collectable Score: {GatherBuddy.AutoGather.LastCollectability}");
            }
        }
        private static bool _gatherDebug;
        public static void DrawAutoGatherStatus()
        {
            var enabled = GatherBuddy.AutoGather.Enabled;
            if (ImGui.Checkbox("Enabled", ref enabled))
            {
                GatherBuddy.AutoGather.Enabled = enabled;
            }
            ImGui.Text($"Status: {GatherBuddy.AutoGather.AutoStatus}");
            var lastNavString = GatherBuddy.AutoGather.LastNavigationResult.HasValue ? GatherBuddy.AutoGather.LastNavigationResult.Value ? "Successful" : "Failed (If you're seeing this you probably need to restart your game)" : "None";
            ImGui.Text($"Navigation: {lastNavString}");
        }
        

        public static void DrawDebugTables()
        {
            // First column: Nearby nodes table
            if (ImGui.BeginTable("##nearbyNodesTable", 6, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
            {
                ImGui.TableSetupColumn("Name");
                ImGui.TableSetupColumn("Targetable");
                ImGui.TableSetupColumn("Desirable");
                ImGui.TableSetupColumn("Position");
                ImGui.TableSetupColumn("Distance");
                ImGui.TableSetupColumn("Action");

                ImGui.TableHeadersRow();

                var playerPosition = Player.Object.Position;
                foreach (var node in Dalamud.ObjectTable.Where(o => o.ObjectKind == ObjectKind.GatheringPoint).OrderBy(o => Vector3.Distance(o.Position, playerPosition)))
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text(node.Name.ToString());
                    ImGui.TableSetColumnIndex(1);
                    ImGui.Text(node.IsTargetable ? "Y" : "N");
                    ImGui.TableSetColumnIndex(2);
                    ImGui.Text(GatherBuddy.AutoGather.IsDesiredNode(node) ? "Y" : "N");
                    ImGui.TableSetColumnIndex(3);
                    ImGui.Text(node.Position.ToString());
                    ImGui.TableSetColumnIndex(4);
                    var distance = Vector3.Distance(Player.Object.Position, node.Position);
                    ImGui.Text(distance.ToString());
                    ImGui.TableSetColumnIndex(5);

                    var territoryId = Dalamud.ClientState.TerritoryType;
                    var isBlacklisted = GatherBuddy.Config.AutoGatherConfig.BlacklistedNodesByTerritoryId.TryGetValue(territoryId, out var list) && list.Contains(node.Position);

                    if (isBlacklisted)
                    {
                        if (ImGui.Button($"Unblacklist##{node.Position}"))
                        {
                            list.Remove(node.Position);
                            if (list.Count == 0)
                            {
                                GatherBuddy.Config.AutoGatherConfig.BlacklistedNodesByTerritoryId.Remove(territoryId);
                            }
                            GatherBuddy.Config.Save();
                        }
                    }
                    else
                    {
                        if (ImGui.Button($"Blacklist##{node.Position}"))
                        {
                            if (list == null)
                            {
                                list = new List<Vector3>();
                                GatherBuddy.Config.AutoGatherConfig.BlacklistedNodesByTerritoryId[territoryId] = list;
                            }
                            list.Add(node.Position);
                            GatherBuddy.Config.Save();
                        }
                    }
                }

                ImGui.EndTable();
            }
        }
        public unsafe static void DrawMountSelector()
        {
            ImGui.PushItemWidth(300);
            var ps = PlayerState.Instance();
            var preview = Dalamud.GameData.GetExcelSheet<Mount>().First(x => x.RowId == GatherBuddy.Config.AutoGatherConfig.AutoGatherMountId).Singular.ToString().ToProperCase();
            if (ImGui.BeginCombo("Select Mount", preview))
            {
                if (ImGui.Selectable("", GatherBuddy.Config.AutoGatherConfig.AutoGatherMountId == 0))
                {
                    GatherBuddy.Config.AutoGatherConfig.AutoGatherMountId = 0;
                    GatherBuddy.Config.Save();
                }

                foreach (var mount in Dalamud.GameData.GetExcelSheet<Mount>().OrderBy(x => x.Singular.ToString().ToProperCase()))
                {
                    if (ps->IsMountUnlocked(mount.RowId))
                    {
                        var selected = ImGui.Selectable(mount.Singular.ToString().ToProperCase(), GatherBuddy.Config.AutoGatherConfig.AutoGatherMountId == mount.RowId);

                        if (selected)
                        {
                            GatherBuddy.Config.AutoGatherConfig.AutoGatherMountId = mount.RowId;
                            GatherBuddy.Config.Save();
                        }
                    }
                }

                ImGui.EndCombo();
            }
        }
        
/// <summary>
/// Extension method to convert the strings to Proper Case.
/// </summary>
/// <param name="input">The string input.</param>
/// <returns>The string in Proper Case.</returns>
public static string ToProperCase(this string input)
{
    return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
}

    }
}
