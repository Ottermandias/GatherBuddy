using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.Text;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using GatherBuddy.Alarms;
using GatherBuddy.AutoGather;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.FishTimer;
using GatherBuddy.Plugin;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets2;
using OtterGui;
using OtterGui.Table;
using OtterGui.Widgets;
using Action = System.Action;
using FishRecord = GatherBuddy.FishTimer.FishRecord;
using GatheringType = GatherBuddy.Enums.GatheringType;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private static class ConfigFunctions
    {
        public static Interface _base = null!;

        public static void DrawSetInput(string jobName, string oldName, Action<string> setName)
        {
            var tmp = oldName;
            ImGui.SetNextItemWidth(SetInputWidth);
            if (ImGui.InputText($"{jobName} Set", ref tmp, 15) && tmp != oldName)
            {
                setName(tmp);
                GatherBuddy.Config.Save();
            }

            ImGuiUtil.HoverTooltip($"Set the name of your {jobName.ToLowerInvariant()} set. Can also be the numerical id instead.");
        }

        private static void DrawCheckbox(string label, string description, bool oldValue, Action<bool> setter)
        {
            if (ImGuiUtil.Checkbox(label, description, oldValue, setter))
                GatherBuddy.Config.Save();
        }

        private static void DrawChatTypeSelector(string label, string description, XivChatType currentValue, Action<XivChatType> setter)
        {
            ImGui.SetNextItemWidth(SetInputWidth);
            if (Widget.DrawChatTypeSelector(label, description, currentValue, setter))
                GatherBuddy.Config.Save();
        }

        // Auto-Gather Config
        public static void DrawAutoGatherBox()
            => DrawCheckbox("Enable Gathering Window Interaction (DISABLING THIS IS UNSUPPORTED)",
                "Toggle whether to automatically gather items. (Disable this for 'nav only mode')",
                GatherBuddy.Config.AutoGatherConfig.DoGathering, b => GatherBuddy.Config.AutoGatherConfig.DoGathering = b);

        public static void DrawGoHomeBox()
            => DrawCheckbox("Go home when idle", "Uses the '/li auto' command to take you home when done gathering or waiting for timed nodes",
                GatherBuddy.Config.AutoGatherConfig.GoHomeWhenIdle, b => GatherBuddy.Config.AutoGatherConfig.GoHomeWhenIdle = b);

        public static void DrawAdvancedUnstuckBox()
            => DrawCheckbox("Enable Experimental Unstuck Method",
                "Use super special movement techniques to manually move your character without navmesh when stuck",
                GatherBuddy.Config.AutoGatherConfig.UseExperimentalUnstuck,
                b => GatherBuddy.Config.AutoGatherConfig.UseExperimentalUnstuck = b);

        public static void DrawHonkModeBox()
            => DrawCheckbox("Play a sound when done gathering", "Play a sound when auto-gathering shuts down because your list is complete",
                GatherBuddy.Config.AutoGatherConfig.HonkMode,   b => GatherBuddy.Config.AutoGatherConfig.HonkMode = b);

        public static void DrawMaterialExtraction()
            => DrawCheckbox("Enable materia extraction",
                "Automatically extract materia from items with a complete spiritbond",
                GatherBuddy.Config.AutoGatherConfig.DoMaterialize,
                b => GatherBuddy.Config.AutoGatherConfig.DoMaterialize = b);

        public static void DrawMinimumGPGathering()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.MinimumGPForGathering;
            if (ImGui.DragInt("Minimum GP for Gathering", ref tmp, 1, 0, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.MinimumGPForGathering = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawMinimumGPCollectibleRotation()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.MinimumGPForCollectableRotation;
            if (ImGui.DragInt("Minimum GP for using skills on collectibles", ref tmp, 1, 0, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.MinimumGPForCollectableRotation = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }
        public static void DrawAlwaysUseSolidAgeCollectables()
            => DrawCheckbox("Ignore the above setting for Solid Reason / Ageless Words", "Use Solid Reason / Ageless Words regardless of starting GP if target collectability score is reached",
                GatherBuddy.Config.AutoGatherConfig.AlwaysUseSolidAgeCollectables, b => GatherBuddy.Config.AutoGatherConfig.AlwaysUseSolidAgeCollectables = b);

        public static void DrawMinimumGPCollectable()
        {
            ImGui.PushItemWidth(300);
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.MinimumGPForCollectable;
            if (ImGui.DragInt("Minimum GP for Gathering Collectables", ref tmp, 1, 0, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.MinimumGPForCollectable = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawMinimumCollectibilityScore()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore;
            if (ImGui.DragInt("Collectibility score to reach before gathering", ref tmp, 1, 1, 1000))
            {
                GatherBuddy.Config.AutoGatherConfig.MinimumCollectibilityScore = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawGatherIfLastIntegrity()
            => DrawCheckbox(
                "Gather instead losing the node",
                "Will gather the node instead losing it if the collectibility score hasn't been reached",
                GatherBuddy.Config.AutoGatherConfig.GatherIfLastIntegrity,
                b => GatherBuddy.Config.AutoGatherConfig.GatherIfLastIntegrity = b);

        public static void DrawGatherIfLastIntegrityMinimumCollectibility()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.GatherIfLastIntegrityMinimumCollectibility;
            if (ImGui.DragInt("Minimum collectibility score to reach before gathering on the last integrity point", ref tmp, 1, 1000))
            {
                GatherBuddy.Config.AutoGatherConfig.GatherIfLastIntegrityMinimumCollectibility = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawUseFlagBox()
            => DrawCheckbox("Disable map marker navigation",            "Whether or not to navigate using map markers (timed nodes only)",
                GatherBuddy.Config.AutoGatherConfig.DisableFlagPathing, b => GatherBuddy.Config.AutoGatherConfig.DisableFlagPathing = b);

        public static void DrawFarNodeFilterDistance()
        {
            var tmp = GatherBuddy.Config.AutoGatherConfig.FarNodeFilterDistance;
            if (ImGui.DragFloat("Far Node Filter Distance", ref tmp, 0.1f, 0.1f, 100f))
            {
                GatherBuddy.Config.AutoGatherConfig.FarNodeFilterDistance = tmp;
                GatherBuddy.Config.Save();
            }

            ImGuiUtil.HoverTooltip(
                "When looking for non-empty nodes GBR will filter out any nodes that are closer to you than this. Prevents checking nodes you can already see are empty.");
        }

        public static void DrawTimedNodePrecog()
        {
            var tmp = GatherBuddy.Config.AutoGatherConfig.TimedNodePrecog;
            if (ImGui.DragInt("Timed Node Precognition (Seconds)", ref tmp, 1, 0, 600))
            {
                GatherBuddy.Config.AutoGatherConfig.TimedNodePrecog = tmp;
                GatherBuddy.Config.Save();
            }

            ImGuiUtil.HoverTooltip("How far in advance of the node actually being up GBR should consider the node to be up");
        }

        public static void DrawBYIIBox()
            => DrawCheckbox("Use Bountiful Yield/Harvest", "Toggle whether to use Bountiful Yield/Harvest for gathering.", GatherBuddy.Config.AutoGatherConfig.BYIIConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.BYIIConfig.UseAction = b);

        public static void DrawBYIIMinGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MinimumGP;
            if (ImGui.DragInt("Bountiful Yield/Harvest Min GP", ref tmp, 1, 100, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawBYIIMaxGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MaximumGP;
            if (ImGui.DragInt("Bountiful Yield/Harvest Max GP", ref tmp, 1, 100, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.BYIIConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }
        public static void DrawBYIIMinimumIncrease()
        {
            var tmp = GatherBuddy.Config.AutoGatherConfig.BYIIConfig.GetOptionalProperty<int>("MinimumIncrease");
            if (ImGui.DragInt("Minimum yield increase", ref tmp, 0.1f, 1, 3))
            {
                GatherBuddy.Config.AutoGatherConfig.BYIIConfig.SetOptionalProperty("MinimumIncrease", tmp);
                GatherBuddy.Config.Save();
            }
        }
        public static void DrawBYIIUseWithCrystals()
            => DrawCheckbox("Use when gathering crystals", "", GatherBuddy.Config.AutoGatherConfig.BYIIConfig.GetOptionalProperty<bool>("UseWithCystals"),
                b => GatherBuddy.Config.AutoGatherConfig.BYIIConfig.SetOptionalProperty("UseWithCystals", b));

        public static void DrawLuckBox()
            => DrawCheckbox("Use Luck", "Toggle whether to use Luck for gathering.", GatherBuddy.Config.AutoGatherConfig.LuckConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.LuckConfig.UseAction = b);

        public static void DrawLuckMinGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.LuckConfig.MinimumGP;
            if (ImGui.DragInt("Luck Min GP", ref tmp, 1, 200, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.LuckConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawLuckMaxGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.LuckConfig.MaximumGP;
            if (ImGui.DragInt("Luck Max GP", ref tmp, 1, 200, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.LuckConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawGivingLandBox()
            => DrawCheckbox("Use The Giving Land", "Toggle whether to use The Giving Land for gathering crystals.", GatherBuddy.Config.AutoGatherConfig.GivingLandConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.GivingLandConfig.UseAction = b);

        public static void DrawGivingLandMinGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.GivingLandConfig.MinimumGP;
            if (ImGui.DragInt("The Giving Land Min GP", ref tmp, 1, 200, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.GivingLandConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawGivingLandMaxGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.GivingLandConfig.MaximumGP;
            if (ImGui.DragInt("The Giving Land Max GP", ref tmp, 1, 200, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.GivingLandConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }
        public static void DrawUseGivingLandOnCooldown()
            => DrawCheckbox("Gather any crystals when The Giving Land is off cooldown", "Gather random crystals on any regular node when The Giving Land is avaiable regardles of current target item.", GatherBuddy.Config.AutoGatherConfig.UseGivingLandOnCooldown,
                b => GatherBuddy.Config.AutoGatherConfig.UseGivingLandOnCooldown = b);
        public static void DrawTwelvesBountyBox()
            => DrawCheckbox("Use The Twelve's Bounty", "Toggle whether to use The Twelve's Bounty for gathering crystals.", GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig.UseAction = b);

        public static void DrawTwelvesBountyMinGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig.MinimumGP;
            if (ImGui.DragInt("The Twelve's Bounty Min GP", ref tmp, 1, 150, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawTwelvesBountyMaxGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig.MaximumGP;
            if (ImGui.DragInt("The Twelve's Bounty Max GP", ref tmp, 1, 150, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawYieldIIMaxGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.MaximumGP;
            if (ImGui.DragInt("Yield II Max GP", ref tmp, 1, 200, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawYieldIIMinGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.MinimumGP;
            if (ImGui.DragInt("Yield II Min GP", ref tmp, 1, 200, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }
        public static void DrawYieldIIUseWithCrystals()
            => DrawCheckbox("Use when gathering crystals", "", GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.GetOptionalProperty<bool>("UseWithCystals"),
                b => GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.SetOptionalProperty("UseWithCystals", b));

        public static void DrawYieldIMaxGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.YieldIConfig.MaximumGP;
            if (ImGui.DragInt("Yield I Max GP", ref tmp, 1, 200, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.YieldIConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawYieldIMinGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.YieldIConfig.MinimumGP;
            if (ImGui.DragInt("Yield I Min GP", ref tmp, 1, 200, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.YieldIConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }
        public static void DrawYieldIUseWithCrystals()
            => DrawCheckbox("Use when gathering crystals", "", GatherBuddy.Config.AutoGatherConfig.YieldIConfig.GetOptionalProperty<bool>("UseWithCystals"),
                b => GatherBuddy.Config.AutoGatherConfig.YieldIConfig.SetOptionalProperty("UseWithCystals", b));

        public static void DrawConditions(AutoGatherConfig.ActionConfig config)
        {
            DrawCheckbox("Use conditions", "Apply specific conditions to the action",
                config.Conditions.UseConditions,
                b => config.Conditions.UseConditions = b);

            if (config.Conditions.UseConditions)
            {
                if (ImGui.TreeNodeEx("Action conditions"))
                {
                    DrawCheckbox("Use only on first step", "Use only if this is the first action done in the gathering",
                        config.Conditions.UseOnlyOnFirstStep,
                        b => config.Conditions.UseOnlyOnFirstStep = b);

                    int tmp = (int)config.Conditions.RequiredIntegrity;
                    if (ImGui.DragInt("Total node integrity required to use", ref tmp, 0.1f, 1, 10))
                    {
                        config.Conditions.RequiredIntegrity = (uint)tmp;
                        GatherBuddy.Config.Save();
                    }

                    DrawCheckbox("Use node type filter", "Use only on specific node types",
                        config.Conditions.FilterNodeTypes,
                        b => config.Conditions.FilterNodeTypes = b);

                    if (config.Conditions.FilterNodeTypes)
                    {
                        if (ImGui.TreeNodeEx("Node filters"))
                        {
                            var node = config.Conditions.NodeFilter;
                            DrawNodeFilter("Regular",   node.RegularNode);
                            DrawNodeFilter("Unspoiled", node.UnspoiledNode);
                            DrawNodeFilter("Ephemeral", node.EphemeralNode);
                            DrawNodeFilter("Legendary", node.LegendaryNode);
                        }
                    }
                }
            }
        }

        private static void DrawNodeFilter(string name, AutoGatherConfig.ActionConditions.NodeFilters.NodeConfig node)
        {
            using var id = ImRaii.PushId(name);
            if (ImGuiUtil.Checkbox($"Use on {name} Nodes", $"Use the action on {name} Nodes", node.Use, b => node.Use = b))
                GatherBuddy.Config.Save();

            if (!node.Use)
                return;

            ImGui.Indent();
            var tmp = node.NodeLevel;

            ImGui.AlignTextToFramePadding();
            ImGuiUtil.TextWrapped("Minimum node level: ");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100 * ImGuiHelpers.GlobalScale);
            if (ImGui.InputInt($"", ref tmp, 1, 1))
            {
                // make sure the level is within bounds, max 100
                node.NodeLevel = Math.Clamp(tmp, 1, 100);
                GatherBuddy.Config.Save();
            }

            if (ImGuiUtil.Checkbox($"Allow lower level nodes if GP is full", "Helpful to avoid gp overcap in some situations", node.AvoidCap,
                    b => node.AvoidCap = b))
                GatherBuddy.Config.Save();
            ImGui.Unindent();
        }


        public static void DrawYieldIICheckbox()
            => DrawCheckbox("Use Kings Yield/Blessed Harvest II", "Use these actions when available",
                GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.YieldIIConfig.UseAction = b);

        public static void DrawYieldICheckbox()
            => DrawCheckbox("Use Kings Yield/Blessed Harvest I", "Use these actions when available",
                GatherBuddy.Config.AutoGatherConfig.YieldIConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.YieldIConfig.UseAction = b);

        public static void DrawScrutinyCheckbox()
            => DrawCheckbox("Use Scrutiny", "Use scrutiny to gather collectibles", GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.UseAction = b);

        public static void DrawMeticulousCheckbox()
            => DrawCheckbox("Use Meticulous Prospector", "Use Meticulous Prospector to gather collectibles",
                GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.UseAction = b);

        public static void DrawScourCheckbox()
            => DrawCheckbox("Use Scour", "Use Scour to gather collectibles when appropriate",
                GatherBuddy.Config.AutoGatherConfig.ScourConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.ScourConfig.UseAction = b);

        public static void DrawBrazenCheckbox()
            => DrawCheckbox("Use Brazen Prospector", "Use Brazen Prospector to gather collectibles when appropriate",
                GatherBuddy.Config.AutoGatherConfig.BrazenConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.BrazenConfig.UseAction = b);

        public static void DrawSolidAgeCollectablesCheckbox()
            => DrawCheckbox("Use Solid Reason/Ageless Words (Collectibles)", "Use Solid Reason/Ageless Words to gather collectibles",
                GatherBuddy.Config.AutoGatherConfig.SolidAgeCollectablesConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.SolidAgeCollectablesConfig.UseAction = b);

        public static void DrawSolidAgeGatherablesCheckbox()
            => DrawCheckbox("Use Solid Reason/Ageless Words (Gatherables)", "Use Solid Reason/Ageless Words to gather collectibles",
                GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.UseAction,
                b => GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.UseAction = b);

        public static void DrawMountUpDistance()
        {
            var tmp = GatherBuddy.Config.AutoGatherConfig.MountUpDistance;
            if (ImGui.DragFloat("Mount Up Distance", ref tmp, 0.1f, 0.1f, 100f))
            {
                GatherBuddy.Config.AutoGatherConfig.MountUpDistance = tmp;
                GatherBuddy.Config.Save();
            }

            ImGuiUtil.HoverTooltip("The distance at which you will mount up to move to a node.");
        }

        public static void DrawScrutinyMaxGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.MaximumGP;
            if (ImGui.DragInt("Scrutiny Max GP", ref tmp, 1, AutoGather.AutoGather.Actions.Scrutiny.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawMeticulousMaxGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.MaximumGP;
            if (ImGui.DragInt("Meticulous Prospector Max GP", ref tmp, 1, AutoGather.AutoGather.Actions.Meticulous.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawScourMaxGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.ScourConfig.MaximumGP;
            if (ImGui.DragInt("Scour Max GP", ref tmp, 1, AutoGather.AutoGather.Actions.Scour.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.ScourConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawBrazenMaxGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.BrazenConfig.MaximumGP;
            if (ImGui.DragInt("Brazen Prospector Max GP", ref tmp, 1, AutoGather.AutoGather.Actions.Brazen.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.BrazenConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawSolidAgeCollectablesMaxGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.SolidAgeCollectablesConfig.MaximumGP;
            if (ImGui.DragInt("Solid Reason/Ageless Words Max GP", ref tmp, 1, AutoGather.AutoGather.Actions.SolidAge.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.SolidAgeCollectablesConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawSolidAgeGatherablesMaxGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.MaximumGP;
            if (ImGui.DragInt("Solid Reason/Ageless Words Max GP", ref tmp, 1, AutoGather.AutoGather.Actions.SolidAge.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }
        public static void DrawSolidAgeGatherablesUseWithCrystals()
            => DrawCheckbox("Use when gathering crystals", "", GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.GetOptionalProperty<bool>("UseWithCystals"),
                b => GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.SetOptionalProperty("UseWithCystals", b));

        public static void DrawSolidAgeGatherablesMinYield()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.GetOptionalProperty<int>("MinimumYield");
            if (ImGui.DragInt("Minimum yield", ref tmp, 0.1f, 1, 20))
            {
                GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.SetOptionalProperty("MinimumYield", tmp);
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawScrutinyMinGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.MinimumGP;
            if (ImGui.DragInt("Scrutiny Min GP", ref tmp, 1, AutoGather.AutoGather.Actions.Scrutiny.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.ScrutinyConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawMeticulousMinGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.MinimumGP;
            if (ImGui.DragInt("Meticulous Prospector Min GP", ref tmp, 1, AutoGather.AutoGather.Actions.Meticulous.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.MeticulousConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawScourMinGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.ScourConfig.MinimumGP;
            if (ImGui.DragInt("Scour Min GP", ref tmp, 1, AutoGather.AutoGather.Actions.Scour.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.ScourConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawBrazenMinGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.BrazenConfig.MinimumGP;
            if (ImGui.DragInt("Brazen Prospector Min GP", ref tmp, 1, AutoGather.AutoGather.Actions.Brazen.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.BrazenConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawSolidAgeCollectablesMinGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.SolidAgeCollectablesConfig.MinimumGP;
            if (ImGui.DragInt("Solid Reason/Ageless Words Min GP", ref tmp, 1, AutoGather.AutoGather.Actions.SolidAge.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.SolidAgeCollectablesConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawSolidAgeGatherablesMinGp()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.MinimumGP;
            if (ImGui.DragInt("Solid Reason/Ageless Words Min GP", ref tmp, 1, AutoGather.AutoGather.Actions.SolidAge.GpCost, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawCordialCheckbox()
            => DrawCheckbox(
                "Use Cordial",
                "Use Cordial if item is available, not on cooldown and GP is within the specified range.",
                GatherBuddy.Config.AutoGatherConfig.CordialConfig.UseConsumable,
                b => GatherBuddy.Config.AutoGatherConfig.CordialConfig.UseConsumable = b);

        public static void DrawCordialMinGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.CordialConfig.MinimumGP;
            if (ImGui.DragInt("Cordial Min GP", ref tmp, 1, 0, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.CordialConfig.MinimumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawCordialMaxGP()
        {
            int tmp = (int)GatherBuddy.Config.AutoGatherConfig.CordialConfig.MaximumGP;
            if (ImGui.DragInt("Cordial Max GP", ref tmp, 1, 1000, 30000))
            {
                GatherBuddy.Config.AutoGatherConfig.CordialConfig.MaximumGP = (uint)tmp;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawFoodCheckbox()
            => DrawCheckbox(
                "Use Food",
                "Use food if item is available and buff is not up.",
                GatherBuddy.Config.AutoGatherConfig.FoodConfig.UseConsumable,
                b => GatherBuddy.Config.AutoGatherConfig.FoodConfig.UseConsumable = b);

        public static void DrawPotionCheckbox()
            => DrawCheckbox(
                "Use Potion",
                "Use potion if item is available and buff is not up.",
                GatherBuddy.Config.AutoGatherConfig.PotionConfig.UseConsumable,
                b => GatherBuddy.Config.AutoGatherConfig.PotionConfig.UseConsumable = b);

        public static void DrawManualCheckbox()
            => DrawCheckbox(
                "Use Manual",
                "Use manual if item is available and buff is not up.",
                GatherBuddy.Config.AutoGatherConfig.ManualConfig.UseConsumable,
                b => GatherBuddy.Config.AutoGatherConfig.ManualConfig.UseConsumable = b);

        public static void DrawSquadronManualCheckbox()
            => DrawCheckbox(
                "Use Squadron Manual",
                "Use squadron manual if item is available and buff is not up.",
                GatherBuddy.Config.AutoGatherConfig.SquadronManualConfig.UseConsumable,
                b => GatherBuddy.Config.AutoGatherConfig.SquadronManualConfig.UseConsumable = b);

        public static void DrawSquadronPassCheckbox()
            => DrawCheckbox(
                "Use Squadron Pass",
                "Use squadron pass if item is available and buff is not up.",
                GatherBuddy.Config.AutoGatherConfig.SquadronPassConfig.UseConsumable,
                b => GatherBuddy.Config.AutoGatherConfig.SquadronPassConfig.UseConsumable = b);

        public static void DrawAntiStuckCooldown()
        {
            var tmp = GatherBuddy.Config.AutoGatherConfig.NavResetCooldown;
            if (ImGui.DragFloat("Anti-Stuck Cooldown", ref tmp, 0.1f, 0.1f, 10f))
            {
                GatherBuddy.Config.AutoGatherConfig.NavResetCooldown = tmp;
                GatherBuddy.Config.Save();
            }

            ImGuiUtil.HoverTooltip("The time in seconds before the navigation system will reset if you are stuck.");
        }

        public static void DrawForceWalkingBox()
            => DrawCheckbox("Force Walking",                      "Force walking to nodes instead of using mounts.",
                GatherBuddy.Config.AutoGatherConfig.ForceWalking, b => GatherBuddy.Config.AutoGatherConfig.ForceWalking = b);

        public static void DrawStuckThreshold()
        {
            var tmp = GatherBuddy.Config.AutoGatherConfig.NavResetThreshold;
            if (ImGui.DragFloat("Stuck Threshold", ref tmp, 0.1f, 0.1f, 10f))
            {
                GatherBuddy.Config.AutoGatherConfig.NavResetThreshold = tmp;
                GatherBuddy.Config.Save();
            }

            ImGuiUtil.HoverTooltip("The time in seconds before the navigation system will consider you stuck.");
        }

        public static void DrawSortingMethodCombo()
        {
            var v = GatherBuddy.Config.AutoGatherConfig.SortingMethod;
            ImGui.SetNextItemWidth(SetInputWidth);

            using var combo = ImRaii.Combo("Item Sorting Method", v.ToString());
            ImGuiUtil.HoverTooltip("What method to use when sorting items internally");
            if (!combo)
                return;

            if (ImGui.Selectable(AutoGatherConfig.SortingType.Location.ToString(), v == AutoGatherConfig.SortingType.Location))
            {
                GatherBuddy.Config.AutoGatherConfig.SortingMethod = AutoGatherConfig.SortingType.Location;
                GatherBuddy.Config.Save();
            }

            if (ImGui.Selectable(AutoGatherConfig.SortingType.None.ToString(), v == AutoGatherConfig.SortingType.None))
            {
                GatherBuddy.Config.AutoGatherConfig.SortingMethod = AutoGatherConfig.SortingType.None;
                GatherBuddy.Config.Save();
            }
        }

        // General Config
        public static void DrawOpenOnStartBox()
            => DrawCheckbox("Open Config UI On Start",
                "Toggle whether the GatherBuddy GUI should be visible after you start the game.",
                GatherBuddy.Config.OpenOnStart, b => GatherBuddy.Config.OpenOnStart = b);

        public static void DrawLockPositionBox()
            => DrawCheckbox("Lock Config UI Movement",
                "Toggle whether the GatherBuddy GUI movement should be locked.",
                GatherBuddy.Config.MainWindowLockPosition, b =>
                {
                    GatherBuddy.Config.MainWindowLockPosition = b;
                    _base.UpdateFlags();
                });

        public static void DrawLockResizeBox()
            => DrawCheckbox("Lock Config UI Size",
                "Toggle whether the GatherBuddy GUI size should be locked.",
                GatherBuddy.Config.MainWindowLockResize, b =>
                {
                    GatherBuddy.Config.MainWindowLockResize = b;
                    _base.UpdateFlags();
                });

        public static void DrawRespectEscapeBox()
            => DrawCheckbox("Escape Closes Main Window",
                "Toggle whether pressing escape while having the main window focused shall close it.",
                GatherBuddy.Config.CloseOnEscape, b =>
                {
                    GatherBuddy.Config.CloseOnEscape = b;
                    _base.UpdateFlags();
                });

        public static void DrawGearChangeBox()
            => DrawCheckbox("Enable Gear Change",
                "Toggle whether to automatically switch gear to the correct job gear for a node.\nUses Miner Set, Botanist Set and Fisher Set.",
                GatherBuddy.Config.UseGearChange, b => GatherBuddy.Config.UseGearChange = b);

        public static void DrawTeleportBox()
            => DrawCheckbox("Enable Teleport",
                "Toggle whether to automatically teleport to a chosen node.",
                GatherBuddy.Config.UseTeleport, b => GatherBuddy.Config.UseTeleport = b);

        public static void DrawMapOpenBox()
            => DrawCheckbox("Open Map With Location",
                "Toggle whether to automatically open the map of the territory of the chosen node with its gathering location highlighted.",
                GatherBuddy.Config.UseCoordinates, b => GatherBuddy.Config.UseCoordinates = b);

        public static void DrawPlaceMarkerBox()
            => DrawCheckbox("Place Flag Marker on Map",
                "Toggle whether to automatically set a red flag marker on the approximate location of the chosen node without opening the map.",
                GatherBuddy.Config.UseFlag, b => GatherBuddy.Config.UseFlag = b);

        public static void DrawMapMarkerPrintBox()
            => DrawCheckbox("Print Map Location",
                "Toggle whether to automatically write a map link to the approximate location of the chosen node to chat.",
                GatherBuddy.Config.WriteCoordinates, b => GatherBuddy.Config.WriteCoordinates = b);

        public static void DrawPlaceWaymarkBox()
            => DrawCheckbox("Place Custom Waymarks",
                "Toggle whether to place custom Waymarks you set manually set up for certain locations.",
                GatherBuddy.Config.PlaceCustomWaymarks, b => GatherBuddy.Config.PlaceCustomWaymarks = b);

        public static void DrawPrintUptimesBox()
            => DrawCheckbox("Print Node Uptimes On Gather",
                "Print the uptimes of nodes you try to /gather in the chat if they are not always up.",
                GatherBuddy.Config.PrintUptime, b => GatherBuddy.Config.PrintUptime = b);

        public static void DrawSkipTeleportBox()
            => DrawCheckbox("Skip Nearby Teleports",
                "Skips teleports if you are in the same map and closer to the target than the selected aetheryte already.",
                GatherBuddy.Config.SkipTeleportIfClose, b => GatherBuddy.Config.SkipTeleportIfClose = b);

        public static void DrawShowStatusLineBox()
            => DrawCheckbox("Show Status Line",
                "Show a status line below the gatherables and fish tables.",
                GatherBuddy.Config.ShowStatusLine, v => GatherBuddy.Config.ShowStatusLine = v);

        public static void DrawHideClippyBox()
            => DrawCheckbox("Hide GatherClippy Button",
                "Permanently hide the GatherClippy Button in the Gatherables and Fish tabs.",
                GatherBuddy.Config.HideClippy, v => GatherBuddy.Config.HideClippy = v);

        private const string ChatInformationString =
            "Note that the message only gets printed to your chat log, regardless of the selected channel"
          + " - other people will not see your 'Say' message.";

        public static void DrawPrintTypeSelector()
            => DrawChatTypeSelector("Chat Type for Messages",
                "The chat type used to print regular messages issued by GatherBuddy.\n"
              + ChatInformationString,
                GatherBuddy.Config.ChatTypeMessage, t => GatherBuddy.Config.ChatTypeMessage = t);

        public static void DrawErrorTypeSelector()
            => DrawChatTypeSelector("Chat Type for Errors",
                "The chat type used to print error messages issued by GatherBuddy.\n"
              + ChatInformationString,
                GatherBuddy.Config.ChatTypeError, t => GatherBuddy.Config.ChatTypeError = t);

        public static void DrawContextMenuBox()
            => DrawCheckbox("Add In-Game Context Menus",
                "Add a 'Gather' entry to in-game right-click context menus for gatherable items.",
                GatherBuddy.Config.AddIngameContextMenus, b =>
                {
                    GatherBuddy.Config.AddIngameContextMenus = b;
                    if (b)
                        _plugin.ContextMenu.Enable();
                    else
                        _plugin.ContextMenu.Disable();
                });

        public static void DrawPreferredJobSelect()
        {
            var v       = GatherBuddy.Config.PreferredGatheringType;
            var current = v == GatheringType.Multiple ? "No Preference" : v.ToString();
            ImGui.SetNextItemWidth(SetInputWidth);
            using var combo = ImRaii.Combo("Preferred Job", current);
            ImGuiUtil.HoverTooltip(
                "Choose your job preference when gathering items that can be gathered by miners as well as botanists.\n"
              + "This effectively turns the regular gather command to /gathermin or /gatherbtn when an item can be gathered by both, "
              + "ignoring the other options even on successive tries.");
            if (!combo)
                return;

            if (ImGui.Selectable("No Preference", v == GatheringType.Multiple) && v != GatheringType.Multiple)
            {
                GatherBuddy.Config.PreferredGatheringType = GatheringType.Multiple;
                GatherBuddy.Config.Save();
            }

            if (ImGui.Selectable(GatheringType.Miner.ToString(), v == GatheringType.Miner) && v != GatheringType.Miner)
            {
                GatherBuddy.Config.PreferredGatheringType = GatheringType.Miner;
                GatherBuddy.Config.Save();
            }

            if (ImGui.Selectable(GatheringType.Botanist.ToString(), v == GatheringType.Botanist) && v != GatheringType.Botanist)
            {
                GatherBuddy.Config.PreferredGatheringType = GatheringType.Botanist;
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawPrintClipboardBox()
            => DrawCheckbox("Print Clipboard Information",
                "Print to the chat whenever you save an object to the clipboard. Failures will be printed regardless.",
                GatherBuddy.Config.PrintClipboardMessages, b => GatherBuddy.Config.PrintClipboardMessages = b);

        // Weather Tab
        public static void DrawWeatherTabNamesBox()
            => DrawCheckbox("Show Names in Weather Tab",
                "Toggle whether to write the names in the table for the weather tab, or just the icons with names on hover.",
                GatherBuddy.Config.ShowWeatherNames, b => GatherBuddy.Config.ShowWeatherNames = b);

        // Alarms
        public static void DrawAlarmToggle()
            => DrawCheckbox("Enable Alarms", "Toggle all alarms on or off.", GatherBuddy.Config.AlarmsEnabled,
                b =>
                {
                    if (b)
                        _plugin.AlarmManager.Enable();
                    else
                        _plugin.AlarmManager.Disable();
                });

        private static bool _gatherDebug = false;

        public static void DrawAlarmsInDutyToggle()
            => DrawCheckbox("Enable Alarms in Duty", "Set whether alarms should trigger while you are bound by a duty.",
                GatherBuddy.Config.AlarmsInDuty,     b => GatherBuddy.Config.AlarmsInDuty = b);

        public static void DrawAlarmsOnlyWhenLoggedInToggle()
            => DrawCheckbox("Enable Alarms Only In-Game",  "Set whether alarms should trigger while you are not logged into any character.",
                GatherBuddy.Config.AlarmsOnlyWhenLoggedIn, b => GatherBuddy.Config.AlarmsOnlyWhenLoggedIn = b);

        private static void DrawAlarmPicker(string label, string description, Sounds current, Action<Sounds> setter)
        {
            var cur = (int)current;
            ImGui.SetNextItemWidth(90 * ImGuiHelpers.GlobalScale);
            if (ImGui.Combo(label, ref cur, AlarmCache.SoundIdNames))
                setter((Sounds)cur);
            ImGuiUtil.HoverTooltip(description);
        }

        public static void DrawWeatherAlarmPicker()
            => DrawAlarmPicker("Weather Change Alarm", "Choose a sound that is played every 8 Eorzea hours on regular weather changes.",
                GatherBuddy.Config.WeatherAlarm,       _plugin.AlarmManager.SetWeatherAlarm);

        public static void DrawHourAlarmPicker()
            => DrawAlarmPicker("Eorzea Hour Change Alarm", "Choose a sound that is played every time the current Eorzea hour changes.",
                GatherBuddy.Config.HourAlarm,              _plugin.AlarmManager.SetHourAlarm);

        // Fish Timer
        public static void DrawFishTimerBox()
            => DrawCheckbox("Show Fish Timer",
                "Toggle whether to show the fish timer window while fishing.",
                GatherBuddy.Config.ShowFishTimer, b => GatherBuddy.Config.ShowFishTimer = b);

        public static void DrawFishTimerEditBox()
            => DrawCheckbox("Edit Fish Timer",
                "Enable editing the fish timer window.",
                GatherBuddy.Config.FishTimerEdit, b => GatherBuddy.Config.FishTimerEdit = b);

        public static void DrawFishTimerClickthroughBox()
            => DrawCheckbox("Enable Fish Timer Clickthrough",
                "Allow clicking through the fish timer and disabling the context menus instead.",
                GatherBuddy.Config.FishTimerClickthrough, b => GatherBuddy.Config.FishTimerClickthrough = b);

        public static void DrawFishTimerHideBox()
            => DrawCheckbox("Hide Uncaught Fish in Fish Timer",
                "Hide all fish from the fish timer window that have not been recorded with the given combination of snagging and bait.",
                GatherBuddy.Config.HideUncaughtFish, b => GatherBuddy.Config.HideUncaughtFish = b);

        public static void DrawFishTimerHideBox2()
            => DrawCheckbox("Hide Unavailable Fish in Fish Timer",
                "Hide all fish from the fish timer window that have have known requirements that are unfulfilled, like Fisher's Intuition or Snagging.",
                GatherBuddy.Config.HideUnavailableFish, b => GatherBuddy.Config.HideUnavailableFish = b);

        public static void DrawFishTimerUptimesBox()
            => DrawCheckbox("Show Uptimes in Fish Timer",
                "Show the uptimes for restricted fish in the fish timer window.",
                GatherBuddy.Config.ShowFishTimerUptimes, b => GatherBuddy.Config.ShowFishTimerUptimes = b);

        public static void DrawKeepRecordsBox()
            => DrawCheckbox("Keep Fish Records",
                "Store Fish Records on your computer. This is necessary for bite timings for the fish timer window.",
                GatherBuddy.Config.StoreFishRecords, b => GatherBuddy.Config.StoreFishRecords = b);

        public static void DrawFishTimerScale()
        {
            var value = GatherBuddy.Config.FishTimerScale / 1000f;
            ImGui.SetNextItemWidth(SetInputWidth);
            var ret = ImGui.DragFloat("Fish Timer Bite Time Scale", ref value, 0.1f, FishRecord.MinBiteTime / 500f,
                FishRecord.MaxBiteTime / 1000f,
                "%2.3f Seconds");

            ImGuiUtil.HoverTooltip("The fishing timer window bite times are scaled to this value.\n"
              + "If your bite time exceeds the value, the progress bar and bite windows will not be displayed.\n"
              + "You should probably keep this as high as your highest bite window and as low as possible. About 40 seconds is usually enough.");

            if (!ret)
                return;

            var newValue = (ushort)Math.Clamp((int)(value * 1000f + 0.9), FishRecord.MinBiteTime * 2, FishRecord.MaxBiteTime);
            if (newValue == GatherBuddy.Config.FishTimerScale)
                return;

            GatherBuddy.Config.FishTimerScale = newValue;
            GatherBuddy.Config.Save();
        }

        public static void DrawFishTimerIntervals()
        {
            int value = GatherBuddy.Config.ShowSecondIntervals;
            ImGui.SetNextItemWidth(SetInputWidth);
            var ret = ImGui.DragInt("Fish Timer Interval Separators", ref value, 0.01f, 0, 16);
            ImGuiUtil.HoverTooltip("The fishing timer window can show a number of interval lines and corresponding seconds between 0 and 16.\n"
              + "Set to 0 to turn this feature off.");
            if (!ret)
                return;

            var newValue = (byte)Math.Clamp(value, 0, 16);
            if (newValue == GatherBuddy.Config.ShowSecondIntervals)
                return;

            GatherBuddy.Config.ShowSecondIntervals = newValue;
            GatherBuddy.Config.Save();
        }

        public static void DrawHideFishPopupBox()
            => DrawCheckbox("Hide Catch Popup",
                "Prevents the popup window that shows you your caught fish and its size, amount and quality from being shown.",
                GatherBuddy.Config.HideFishSizePopup, b => GatherBuddy.Config.HideFishSizePopup = b);


        // Spearfishing Helper
        public static void DrawSpearfishHelperBox()
            => DrawCheckbox("Show Spearfishing Helper",
                "Toggle whether to show the Spearfishing Helper while spearfishing.",
                GatherBuddy.Config.ShowSpearfishHelper, b => GatherBuddy.Config.ShowSpearfishHelper = b);

        public static void DrawSpearfishNamesBox()
            => DrawCheckbox("Show Fish Name Overlay",
                "Toggle whether to show the identified names of fish in the spearfishing window.",
                GatherBuddy.Config.ShowSpearfishNames, b => GatherBuddy.Config.ShowSpearfishNames = b);

        public static void DrawAvailableSpearfishBox()
            => DrawCheckbox("Show List of Available Fish",
                "Toggle whether to show the list of fish available in your current spearfishing spot on the side of the spearfishing window.",
                GatherBuddy.Config.ShowAvailableSpearfish, b => GatherBuddy.Config.ShowAvailableSpearfish = b);

        public static void DrawSpearfishSpeedBox()
            => DrawCheckbox("Show Speed of Fish in Overlay",
                "Toggle whether to show the speed of fish in the spearfishing window in addition to their names.",
                GatherBuddy.Config.ShowSpearfishSpeed, b => GatherBuddy.Config.ShowSpearfishSpeed = b);

        public static void DrawSpearfishCenterLineBox()
            => DrawCheckbox("Show Center Line",
                "Toggle whether to show a straight line up from the center of the spearfishing gig in the spearfishing window.",
                GatherBuddy.Config.ShowSpearfishCenterLine, b => GatherBuddy.Config.ShowSpearfishCenterLine = b);

        public static void DrawSpearfishIconsAsTextBox()
            => DrawCheckbox("Show Speed and Size as Text",
                "Toggle whether to show the speed and size of available fish as text instead of icons.",
                GatherBuddy.Config.ShowSpearfishListIconsAsText, b => GatherBuddy.Config.ShowSpearfishListIconsAsText = b);

        public static void DrawSpearfishFishNameFixed()
            => DrawCheckbox("Show Fish Names in Fixed Position",
                "Toggle whether to show the identified names of fish on the moving fish themselves or in a fixed position.",
                GatherBuddy.Config.FixNamesOnPosition, b => GatherBuddy.Config.FixNamesOnPosition = b);

        public static void DrawSpearfishFishNamePercentage()
        {
            if (!GatherBuddy.Config.FixNamesOnPosition)
                return;

            var tmp = (int)GatherBuddy.Config.FixNamesPercentage;
            ImGui.SetNextItemWidth(SetInputWidth);
            if (!ImGui.DragInt("Fish Name Position Percentage", ref tmp, 0.1f, 0, 100, "%i%%"))
                return;

            tmp = Math.Clamp(tmp, 0, 100);
            if (tmp == GatherBuddy.Config.FixNamesPercentage)
                return;

            GatherBuddy.Config.FixNamesPercentage = (byte)tmp;
            GatherBuddy.Config.Save();
        }

        // Gather Window
        public static void DrawShowGatherWindowBox()
            => DrawCheckbox("Show Gather Window",
                "Show a small window with pinned Gatherables and their uptimes.",
                GatherBuddy.Config.ShowGatherWindow, b => GatherBuddy.Config.ShowGatherWindow = b);

        public static void DrawGatherWindowAnchorBox()
            => DrawCheckbox("Anchor Gather Window to Bottom Left",
                "Lets the Gather Window grow to the top and shrink from the top instead of the bottom.",
                GatherBuddy.Config.GatherWindowBottomAnchor, b => GatherBuddy.Config.GatherWindowBottomAnchor = b);

        public static void DrawGatherWindowTimersBox()
            => DrawCheckbox("Show Gather Window Timers",
                "Show the uptimes for gatherables in the gather window.",
                GatherBuddy.Config.ShowGatherWindowTimers, b => GatherBuddy.Config.ShowGatherWindowTimers = b);

        public static void DrawGatherWindowAlarmsBox()
            => DrawCheckbox("Show Active Alarms in Gather Window",
                "Additionally show active alarms as a last gather window preset, obeying the regular rules for the window.",
                GatherBuddy.Config.ShowGatherWindowAlarms, b =>
                {
                    GatherBuddy.Config.ShowGatherWindowAlarms = b;
                    _plugin.GatherWindowManager.SetShowGatherWindowAlarms(b);
                });

        public static void DrawSortGatherWindowBox()
            => DrawCheckbox("Sort Gather Window by Uptime",
                "Sort the items selected for the gather window by their uptimes.",
                GatherBuddy.Config.SortGatherWindowByUptime, b => GatherBuddy.Config.SortGatherWindowByUptime = b);

        public static void DrawGatherWindowShowOnlyAvailableBox()
            => DrawCheckbox("Show Only Available Items",
                "Show only those items from your gather window setup that are currently available.",
                GatherBuddy.Config.ShowGatherWindowOnlyAvailable, b => GatherBuddy.Config.ShowGatherWindowOnlyAvailable = b);

        public static void DrawHideGatherWindowInDutyBox()
            => DrawCheckbox("Hide Gather Window in Duty",
                "Hide the gather window when bound by any duty.",
                GatherBuddy.Config.HideGatherWindowInDuty, b => GatherBuddy.Config.HideGatherWindowInDuty = b);

        public static void DrawGatherWindowHoldKey()
        {
            DrawCheckbox("Only Show Gather Window if Holding Key",
                "Only show the gather window if you are holding your selected key.",
                GatherBuddy.Config.OnlyShowGatherWindowHoldingKey, b => GatherBuddy.Config.OnlyShowGatherWindowHoldingKey = b);

            if (!GatherBuddy.Config.OnlyShowGatherWindowHoldingKey)
                return;

            ImGui.SetNextItemWidth(SetInputWidth);
            Widget.KeySelector("Hotkey to Hold", "Set the hotkey to hold to keep the window visible.",
                GatherBuddy.Config.GatherWindowHoldKey,
                k => GatherBuddy.Config.GatherWindowHoldKey = k, Configuration.ValidKeys);
        }

        public static void DrawGatherWindowLockBox()
            => DrawCheckbox("Lock Gather Window Position",
                "Prevent moving the gather window by dragging it around.",
                GatherBuddy.Config.LockGatherWindow, b => GatherBuddy.Config.LockGatherWindow = b);


        public static void DrawGatherWindowHotkeyInput()
        {
            if (Widget.ModifiableKeySelector("Hotkey to Open Gather Window", "Set a hotkey to open the Gather Window.", SetInputWidth,
                    GatherBuddy.Config.GatherWindowHotkey, k => GatherBuddy.Config.GatherWindowHotkey = k, Configuration.ValidKeys))
                GatherBuddy.Config.Save();
        }

        public static void DrawMainInterfaceHotkeyInput()
        {
            if (Widget.ModifiableKeySelector("Hotkey to Open Main Interface", "Set a hotkey to open the main GatherBuddy interface.",
                    SetInputWidth,
                    GatherBuddy.Config.MainInterfaceHotkey, k => GatherBuddy.Config.MainInterfaceHotkey = k, Configuration.ValidKeys))
                GatherBuddy.Config.Save();
        }


        public static void DrawGatherWindowDeleteModifierInput()
        {
            ImGui.SetNextItemWidth(SetInputWidth);
            if (Widget.ModifierSelector("Modifier to Delete Items on Right-Click",
                    "Set the modifier key to be used while right-clicking items in the gather window to delete them.",
                    GatherBuddy.Config.GatherWindowDeleteModifier, k => GatherBuddy.Config.GatherWindowDeleteModifier = k))
                GatherBuddy.Config.Save();
        }


        public static void DrawAetherytePreference()
        {
            var tmp     = GatherBuddy.Config.AetherytePreference == AetherytePreference.Cost;
            var oldPref = GatherBuddy.Config.AetherytePreference;
            if (ImGui.RadioButton("Prefer Cheaper Aetherytes", tmp))
                GatherBuddy.Config.AetherytePreference = AetherytePreference.Cost;
            var hovered = ImGui.IsItemHovered();
            ImGui.SameLine();
            if (ImGui.RadioButton("Prefer Less Travel Time", !tmp))
                GatherBuddy.Config.AetherytePreference = AetherytePreference.Distance;
            hovered |= ImGui.IsItemHovered();
            if (hovered)
                ImGui.SetTooltip(
                    "Specify whether you prefer aetherytes that are closer to your target (less travel time)"
                  + " or aetherytes that are cheaper to teleport to when scanning through all available nodes for an item."
                  + " Only matters if the item is not timed and has multiple sources.");

            if (oldPref != GatherBuddy.Config.AetherytePreference)
            {
                GatherBuddy.UptimeManager.ResetLocations();
                GatherBuddy.Config.Save();
            }
        }

        public static void DrawAlarmFormatInput()
            => DrawFormatInput("Alarm Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Alarm} with the alarm name in brackets.\n- {Item} with the item link.\n- {Offset} with the alarm offset in seconds.\n- {DurationString} with 'will be up for the next ...' or 'is currently up for ...'.\n- {Location} with the map flag link and location name.",
                GatherBuddy.Config.AlarmFormat, Configuration.DefaultAlarmFormat, s => GatherBuddy.Config.AlarmFormat = s);

        public static void DrawIdentifiedGatherableFormatInput()
            => DrawFormatInput("Identified Gatherable Chat Format",
                "Keep empty to have no chat output.\nCan replace:\n- {Input} with the entered search text.\n- {Item} with the item link.",
                GatherBuddy.Config.IdentifiedGatherableFormat, Configuration.DefaultIdentifiedGatherableFormat,
                s => GatherBuddy.Config.IdentifiedGatherableFormat = s);
    }

    private void DrawConfigTab()
    {
        using var id  = ImRaii.PushId("Config");
        using var tab = ImRaii.TabItem("Config");
        ImGuiUtil.HoverTooltip("Set up your very own GatherBuddy to your meticulous specifications.\n"
          + "If you treat him well, he might even become a real boy.");

        if (!tab)
            return;

        using var child = ImRaii.Child("ConfigTab");
        if (!child)
            return;

        if (ImGui.CollapsingHeader("Auto-Gather"))
        {
            if (ImGui.TreeNodeEx("General##autoGeneral"))
            {
                ConfigFunctions.DrawHonkModeBox();
                AutoGatherUI.DrawMountSelector();
                ConfigFunctions.DrawMountUpDistance();
                ConfigFunctions.DrawMinimumGPGathering();
                ConfigFunctions.DrawSortingMethodCombo();
                ConfigFunctions.DrawUseGivingLandOnCooldown();
                ConfigFunctions.DrawGoHomeBox();
                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Collectables"))
            {
                ConfigFunctions.DrawMinimumGPCollectable();
                ConfigFunctions.DrawMinimumGPCollectibleRotation();
                ConfigFunctions.DrawAlwaysUseSolidAgeCollectables();
                ConfigFunctions.DrawMinimumCollectibilityScore();
                ConfigFunctions.DrawGatherIfLastIntegrity();

                if (GatherBuddy.Config.AutoGatherConfig.GatherIfLastIntegrity)
                    ConfigFunctions.DrawGatherIfLastIntegrityMinimumCollectibility();

                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Gathering Actions"))
            {
                if (ImGui.TreeNodeEx("Bountiful Yield/Harvest"))
                {
                    ConfigFunctions.DrawBYIIBox();
                    ConfigFunctions.DrawBYIIMinGP();
                    ConfigFunctions.DrawBYIIMaxGP();
                    ConfigFunctions.DrawBYIIMinimumIncrease();
                    ConfigFunctions.DrawBYIIUseWithCrystals();
                    ConfigFunctions.DrawConditions(GatherBuddy.Config.AutoGatherConfig.BYIIConfig);
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Kings Yield/Blessed Harvest II"))
                {
                    ConfigFunctions.DrawYieldIICheckbox();
                    ConfigFunctions.DrawYieldIIMinGP();
                    ConfigFunctions.DrawYieldIIMaxGP();
                    ConfigFunctions.DrawYieldIIUseWithCrystals();
                    ConfigFunctions.DrawConditions(GatherBuddy.Config.AutoGatherConfig.YieldIIConfig);
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Kings Yield/Blessed Harvest I"))
                {
                    ConfigFunctions.DrawYieldICheckbox();
                    ConfigFunctions.DrawYieldIMinGP();
                    ConfigFunctions.DrawYieldIMaxGP();
                    ConfigFunctions.DrawYieldIUseWithCrystals();
                    ConfigFunctions.DrawConditions(GatherBuddy.Config.AutoGatherConfig.YieldIConfig);
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Solid Reason/Ageless Words"))
                {
                    ConfigFunctions.DrawSolidAgeGatherablesCheckbox();
                    ConfigFunctions.DrawSolidAgeGatherablesMinGp();
                    ConfigFunctions.DrawSolidAgeGatherablesMaxGp();
                    ConfigFunctions.DrawSolidAgeGatherablesMinYield();
                    ConfigFunctions.DrawSolidAgeGatherablesUseWithCrystals();
                    ConfigFunctions.DrawConditions(GatherBuddy.Config.AutoGatherConfig.SolidAgeGatherablesConfig);
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Luck"))
                {
                    ConfigFunctions.DrawLuckBox();
                    ConfigFunctions.DrawLuckMinGP();
                    ConfigFunctions.DrawLuckMaxGP();
                    ConfigFunctions.DrawConditions(GatherBuddy.Config.AutoGatherConfig.LuckConfig);
                    ImGui.TreePop();
                }
                if (ImGui.TreeNodeEx("The Giving Land"))
                {
                    ConfigFunctions.DrawGivingLandBox();
                    ConfigFunctions.DrawGivingLandMinGP();
                    ConfigFunctions.DrawGivingLandMaxGP();
                    ConfigFunctions.DrawConditions(GatherBuddy.Config.AutoGatherConfig.GivingLandConfig);
                    ImGui.TreePop();
                }
                if (ImGui.TreeNodeEx("The Twelve's Bounty"))
                {
                    ConfigFunctions.DrawTwelvesBountyBox();
                    ConfigFunctions.DrawTwelvesBountyMinGP();
                    ConfigFunctions.DrawTwelvesBountyMaxGP();
                    ConfigFunctions.DrawConditions(GatherBuddy.Config.AutoGatherConfig.TwelvesBountyConfig);
                    ImGui.TreePop();
                }

                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Collectible actions"))
            {
                if (ImGui.TreeNodeEx("Scour"))
                {
                    ConfigFunctions.DrawScourCheckbox();
                    ConfigFunctions.DrawScourMinGp();
                    ConfigFunctions.DrawScourMaxGp();
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Brazen Prospector"))
                {
                    ConfigFunctions.DrawBrazenCheckbox();
                    ConfigFunctions.DrawBrazenMinGp();
                    ConfigFunctions.DrawBrazenMaxGp();
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Meticulous Prospector"))
                {
                    ConfigFunctions.DrawMeticulousCheckbox();
                    ConfigFunctions.DrawMeticulousMinGp();
                    ConfigFunctions.DrawMeticulousMaxGp();
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Scrutiny"))
                {
                    ConfigFunctions.DrawScrutinyCheckbox();
                    ConfigFunctions.DrawScrutinyMinGp();
                    ConfigFunctions.DrawScrutinyMaxGp();
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Solid Reason/Ageless Words"))
                {
                    ConfigFunctions.DrawSolidAgeCollectablesCheckbox();
                    ConfigFunctions.DrawSolidAgeCollectablesMinGp();
                    ConfigFunctions.DrawSolidAgeCollectablesMaxGp();
                    ImGui.TreePop();
                }

                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Consumables"))
            {
                if (ImGui.TreeNodeEx("Cordial"))
                {
                    ConfigFunctions.DrawCordialCheckbox();
                    ConfigFunctions.DrawCordialMinGP();
                    ConfigFunctions.DrawCordialMaxGP();
                    AutoGatherUI.DrawCordialSelector();
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Food"))
                {
                    ConfigFunctions.DrawFoodCheckbox();
                    AutoGatherUI.DrawFoodSelector();
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Potion"))
                {
                    ConfigFunctions.DrawPotionCheckbox();
                    AutoGatherUI.DrawPotionSelector();
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Manual"))
                {
                    ConfigFunctions.DrawManualCheckbox();
                    AutoGatherUI.DrawManualSelector();
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Squadron Manual"))
                {
                    ConfigFunctions.DrawSquadronManualCheckbox();
                    AutoGatherUI.DrawSquadronManualSelector();
                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Squadron Pass"))
                {
                    ConfigFunctions.DrawSquadronPassCheckbox();
                    AutoGatherUI.DrawSquadronPassSelector();
                    ImGui.TreePop();
                }

                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Advanced"))
            {
                ConfigFunctions.DrawAutoGatherBox();
                ConfigFunctions.DrawUseFlagBox();
                ConfigFunctions.DrawForceWalkingBox();
                ConfigFunctions.DrawAdvancedUnstuckBox();
                ConfigFunctions.DrawMaterialExtraction();
                ConfigFunctions.DrawAntiStuckCooldown();
                ConfigFunctions.DrawStuckThreshold();
                ConfigFunctions.DrawTimedNodePrecog();
                ImGui.TreePop();
            }
        }

        if (ImGui.CollapsingHeader("General"))
        {
            if (ImGui.TreeNodeEx("Gather Command"))
            {
                ConfigFunctions.DrawPreferredJobSelect();
                ConfigFunctions.DrawGearChangeBox();
                ConfigFunctions.DrawTeleportBox();
                ConfigFunctions.DrawMapOpenBox();
                ConfigFunctions.DrawPlaceMarkerBox();
                ConfigFunctions.DrawPlaceWaymarkBox();
                ConfigFunctions.DrawAetherytePreference();
                ConfigFunctions.DrawSkipTeleportBox();
                ConfigFunctions.DrawContextMenuBox();
                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Set Names"))
            {
                ConfigFunctions.DrawSetInput("Miner",    GatherBuddy.Config.MinerSetName,    s => GatherBuddy.Config.MinerSetName    = s);
                ConfigFunctions.DrawSetInput("Botanist", GatherBuddy.Config.BotanistSetName, s => GatherBuddy.Config.BotanistSetName = s);
                ConfigFunctions.DrawSetInput("Fisher",   GatherBuddy.Config.FisherSetName,   s => GatherBuddy.Config.FisherSetName   = s);
                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Alarms"))
            {
                ConfigFunctions.DrawAlarmToggle();
                ConfigFunctions.DrawAlarmsInDutyToggle();
                ConfigFunctions.DrawAlarmsOnlyWhenLoggedInToggle();
                ConfigFunctions.DrawWeatherAlarmPicker();
                ConfigFunctions.DrawHourAlarmPicker();
                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Messages"))
            {
                ConfigFunctions.DrawPrintTypeSelector();
                ConfigFunctions.DrawErrorTypeSelector();
                ConfigFunctions.DrawMapMarkerPrintBox();
                ConfigFunctions.DrawPrintUptimesBox();
                ConfigFunctions.DrawPrintClipboardBox();
                ConfigFunctions.DrawAlarmFormatInput();
                ConfigFunctions.DrawIdentifiedGatherableFormatInput();
                ImGui.TreePop();
            }

            ImGui.NewLine();
        }

        if (ImGui.CollapsingHeader("Interface"))
        {
            if (ImGui.TreeNodeEx("Config Window"))
            {
                ConfigFunctions._base = this;
                ConfigFunctions.DrawOpenOnStartBox();
                ConfigFunctions.DrawRespectEscapeBox();
                ConfigFunctions.DrawLockPositionBox();
                ConfigFunctions.DrawLockResizeBox();
                ConfigFunctions.DrawWeatherTabNamesBox();
                ConfigFunctions.DrawShowStatusLineBox();
                ConfigFunctions.DrawHideClippyBox();
                ConfigFunctions.DrawMainInterfaceHotkeyInput();
                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Fish Timer"))
            {
                ConfigFunctions.DrawKeepRecordsBox();
                ConfigFunctions.DrawFishTimerBox();
                ConfigFunctions.DrawFishTimerEditBox();
                ConfigFunctions.DrawFishTimerClickthroughBox();
                ConfigFunctions.DrawFishTimerHideBox();
                ConfigFunctions.DrawFishTimerHideBox2();
                ConfigFunctions.DrawFishTimerUptimesBox();
                ConfigFunctions.DrawFishTimerScale();
                ConfigFunctions.DrawFishTimerIntervals();
                ConfigFunctions.DrawHideFishPopupBox();
                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Gather Window"))
            {
                ConfigFunctions.DrawShowGatherWindowBox();
                ConfigFunctions.DrawGatherWindowAnchorBox();
                ConfigFunctions.DrawGatherWindowTimersBox();
                ConfigFunctions.DrawGatherWindowAlarmsBox();
                ConfigFunctions.DrawSortGatherWindowBox();
                ConfigFunctions.DrawGatherWindowShowOnlyAvailableBox();
                ConfigFunctions.DrawHideGatherWindowInDutyBox();
                ConfigFunctions.DrawGatherWindowHoldKey();
                ConfigFunctions.DrawGatherWindowLockBox();
                ConfigFunctions.DrawGatherWindowHotkeyInput();
                ConfigFunctions.DrawGatherWindowDeleteModifierInput();
                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Spearfishing Helper"))
            {
                ConfigFunctions.DrawSpearfishHelperBox();
                ConfigFunctions.DrawSpearfishNamesBox();
                ConfigFunctions.DrawSpearfishSpeedBox();
                ConfigFunctions.DrawAvailableSpearfishBox();
                ConfigFunctions.DrawSpearfishIconsAsTextBox();
                ConfigFunctions.DrawSpearfishCenterLineBox();
                ConfigFunctions.DrawSpearfishFishNameFixed();
                ConfigFunctions.DrawSpearfishFishNamePercentage();
                ImGui.TreePop();
            }

            ImGui.NewLine();
        }

        if (ImGui.CollapsingHeader("Colors"))
        {
            foreach (var color in Enum.GetValues<ColorId>())
            {
                var (defaultColor, name, description) = color.Data();
                var currentColor = GatherBuddy.Config.Colors.TryGetValue(color, out var current) ? current : defaultColor;
                if (Widget.ColorPicker(name, description, currentColor, c => GatherBuddy.Config.Colors[color] = c, defaultColor))
                    GatherBuddy.Config.Save();
            }

            ImGui.NewLine();

            if (Widget.PaletteColorPicker("Names in Chat", Vector2.One * ImGui.GetFrameHeight(), GatherBuddy.Config.SeColorNames,
                    Configuration.DefaultSeColorNames, Configuration.ForegroundColors, out var idx))
                GatherBuddy.Config.SeColorNames = idx;
            if (Widget.PaletteColorPicker("Commands in Chat", Vector2.One * ImGui.GetFrameHeight(), GatherBuddy.Config.SeColorCommands,
                    Configuration.DefaultSeColorCommands, Configuration.ForegroundColors, out idx))
                GatherBuddy.Config.SeColorCommands = idx;
            if (Widget.PaletteColorPicker("Arguments in Chat", Vector2.One * ImGui.GetFrameHeight(), GatherBuddy.Config.SeColorArguments,
                    Configuration.DefaultSeColorArguments, Configuration.ForegroundColors, out idx))
                GatherBuddy.Config.SeColorArguments = idx;
            if (Widget.PaletteColorPicker("Alarm Message in Chat", Vector2.One * ImGui.GetFrameHeight(), GatherBuddy.Config.SeColorAlarm,
                    Configuration.DefaultSeColorAlarm, Configuration.ForegroundColors, out idx))
                GatherBuddy.Config.SeColorAlarm = idx;

            ImGui.NewLine();
        }
    }
}
