using Dalamud.Interface.Components;
using Dalamud.Interface.Utility.Raii;
using GatherBuddy.AutoGather;
using GatherBuddy.Config;
using ImGuiNET;
using Lumina.Excel.Sheets;
using Newtonsoft.Json;
using OtterGui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using GatherBuddy.Classes;
using static GatherBuddy.AutoGather.AutoGather;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private static readonly (string name, uint id)[] CrystalTypes = [("Elemental Shards", 2), ("Elemental Crystals", 8), ("Elemental Clusters", 14)];
        private readonly ConfigPresetsSelector _configPresetsSelector = new();
        private (bool EditingName, bool ChangingMin) _configPresetsUIState;
        public IReadOnlyCollection<ConfigPreset> GatherActionsPresets => _configPresetsSelector.Presets;

        public class ConfigPresetsSelector : ItemSelector<ConfigPreset>
        {
            private const string FileName = "actions.json";

            public ConfigPresetsSelector()
                : base([], Flags.All ^ Flags.Import ^ Flags.Drop)
            {
                Load();
            }

            public IReadOnlyCollection<ConfigPreset> Presets => Items.AsReadOnly();

            protected override bool Filtered(int idx)
                => Filter.Length != 0 && !Items[idx].Name.Contains(Filter, StringComparison.InvariantCultureIgnoreCase);

            protected override bool OnDraw(int idx)
            {
                using var id = ImRaii.PushId(idx);
                using var color = ImRaii.PushColor(ImGuiCol.Text, ColorId.DisabledText.Value(), !Items[idx].Enabled);
                return ImGui.Selectable(CheckUnnamed(Items[idx].Name), idx == CurrentIdx);
            }

            protected override bool OnDelete(int idx)
            {
                if (idx == Items.Count - 1) return false;

                Items.RemoveAt(idx);
                Save();
                return true;
            }

            protected override bool OnAdd(string name)
            {
                Items.Insert(Items.Count - 1, new()
                {
                    Name = name,
                });
                Save();
                return true;
            }

            //protected override bool OnClipboardImport(string name, string data)
            //{
            //    if (!GatherWindowPreset.Config.FromBase64(data, out var cfg))
            //        return false;

            //    GatherWindowPreset.FromConfig(cfg, out var preset);
            //    preset.Name = name;
            //    _plugin.GatherWindowManager.AddPreset(preset);
            //    return true;
            //}

            protected override bool OnDuplicate(string name, int idx)
            {
                var preset = Items[idx] with { Enabled = false, Name = name };
                Items.Insert(Math.Min(idx + 1, Items.Count - 1), preset);
                Save();
                return true;
            }

            protected override bool OnMove(int idx1, int idx2)
            {
                idx2 = Math.Min(idx2, Items.Count - 2);
                if (idx1 >= Items.Count - 1) return false;
                if (idx1 < 0 || idx2 < 0) return false;

                Plugin.Functions.Move(Items, idx1, idx2);
                Save();
                return true;
            }

            public void Save()
            {
                var file = Plugin.Functions.ObtainSaveFile(FileName);
                if (file == null)
                    return;

                try
                {
                    var text = JsonConvert.SerializeObject(Items, Formatting.Indented);
                    File.WriteAllText(file.FullName, text);
                }
                catch (Exception e)
                {
                    GatherBuddy.Log.Error($"Error serializing config presets data:\n{e}");
                }
            }

            private void Load()
            {
                List<ConfigPreset>? items = null;

                var file = Plugin.Functions.ObtainSaveFile(FileName);
                if (file != null && file.Exists)
                {
                    var text = File.ReadAllText(file.FullName);
                    items = JsonConvert.DeserializeObject<List<ConfigPreset>>(text);
                }
                if (items != null && items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                }
                else
                {
                    //Convert old settings to the new Default preset
                    Items.Add(GatherBuddy.Config.AutoGatherConfig.ConvertToPreset());
                    Save();
                    GatherBuddy.Config.AutoGatherConfig.ConfigConversionFixed = true;
                    GatherBuddy.Config.Save();
                }
                Items[Items.Count - 1] = Items[Items.Count - 1].MakeDefault();

                if (!GatherBuddy.Config.AutoGatherConfig.ConfigConversionFixed)
                {
                    var def = Items[Items.Count - 1];
                    fixAction(def.GatherableActions.Bountiful);
                    fixAction(def.GatherableActions.Yield1);
                    fixAction(def.GatherableActions.Yield2);
                    fixAction(def.GatherableActions.SolidAge);
                    fixAction(def.GatherableActions.TwelvesBounty);
                    fixAction(def.GatherableActions.GivingLand);
                    fixAction(def.GatherableActions.Gift1);
                    fixAction(def.GatherableActions.Gift2);
                    fixAction(def.GatherableActions.Tidings);
                    fixAction(def.GatherableActions.Bountiful);
                    fixAction(def.CollectableActions.Scrutiny);
                    fixAction(def.CollectableActions.Scour);
                    fixAction(def.CollectableActions.Brazen);
                    fixAction(def.CollectableActions.Meticulous);
                    fixAction(def.CollectableActions.SolidAge);
                    fixAction(def.Consumables.Cordial);
                    Save();
                    GatherBuddy.Config.AutoGatherConfig.ConfigConversionFixed = true;
                    GatherBuddy.Config.Save();
                }
                void fixAction(ConfigPreset.ActionConfig action)
                {
                    if (action.MaxGP == 0) action.MaxGP = ConfigPreset.MaxGP;
                }
            }

            public ConfigPreset Match(Gatherable? item)
            {
                return item == null ? Items[Items.Count - 1] : Items.SkipLast(1).Where(i => i.Match(item)).FirstOrDefault(Items[Items.Count - 1]);
            }
        }

        public ConfigPreset MatchConfigPreset(Gatherable? item) => _configPresetsSelector.Match(item);

        public void DrawConfigPresetsTab()
        {
            using var tab = ImRaii.TabItem("Config Presets");

            ImGuiUtil.HoverTooltip("Configure what actions to use with Auto-Gather.");

            if (!tab)
                return;

            var selector = _configPresetsSelector;
            selector.Draw(SelectorWidth);
            ImGui.SameLine();
            ItemDetailsWindow.Draw("Preset Details", DrawConfigPresetHeader, () =>
            {
                DrawConfigPreset(selector.EnsureCurrent()!, selector.CurrentIdx == selector.Presets.Count - 1);
            });
        }
        
        private void DrawConfigPresetHeader()
        {
            if (ImGui.Button("Check"))
            {
                ImGui.OpenPopup("Config Presets Checker");
            }
            ImGuiUtil.HoverTooltip("Check what presets are used for items from the auto-gather list");

            var open = true;
            using (var popup = ImRaii.PopupModal("Config Presets Checker", ref open, ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoTitleBar))
            {
                if (popup)
                {
                    using (var table = ImRaii.Table("Items", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
                    {
                        ImGui.TableSetupColumn("Gather List");
                        ImGui.TableSetupColumn("Item");
                        ImGui.TableSetupColumn("Config Preset");
                        ImGui.TableHeadersRow();

                        var crystals = CrystalTypes
                            .Select(x => ("", x.name, GatherBuddy.GameData.Gatherables[x.id]));
                        var items = _plugin.GatherWindowManager.Presets
                            .Where(x => x.Enabled && !x.Fallback)
                            .SelectMany(x => x.Items.Select(i => (x.Name, i.Name[GatherBuddy.Language], i)));

                        foreach (var (list, name, item) in crystals.Concat(items))
                        {
                            ImGui.TableNextRow();
                            ImGui.TableNextColumn();
                            ImGui.Text(list);
                            ImGui.TableNextColumn();
                            ImGui.Text(name);
                            ImGui.TableNextColumn();
                            ImGui.Text(MatchConfigPreset(item).Name);
                        }
                    }

                    var size = ImGui.CalcTextSize("Close").X + ImGui.GetStyle().FramePadding.X * 2.0f;
                    var offset = (ImGui.GetContentRegionAvail().X - size) * 0.5f;
                    if (offset > 0.0f) ImGui.SetCursorPosX(ImGui.GetCursorPosX() + offset);
                    if (ImGui.Button("Close")) ImGui.CloseCurrentPopup();
                }
            }

            ImGuiComponents.HelpMarker(
                "Presets are checked against the current target item in order from top to bottom.\n" +
                "Only the first matched preset is used, the rest are ignored.\n" +
                "The Default preset is always last and is used if no other preset matches the item.");
        }

        private void DrawConfigPreset(ConfigPreset preset, bool isDefault)
        {
            var selector = _configPresetsSelector;
            ref var state = ref _configPresetsUIState;

            if (!isDefault)
            {
                if (ImGuiUtil.DrawEditButtonText(0, CheckUnnamed(preset.Name), out var name, ref state.EditingName, IconButtonSize, SetInputWidth, 64) && name != CheckUnnamed(preset.Name))
                {
                    preset.Name = name;
                    selector.Save();
                }

                var enabled = preset.Enabled;
                if (ImGui.Checkbox("Enabled", ref enabled) && enabled != preset.Enabled)
                {
                    preset.Enabled = enabled;
                    selector.Save();
                }
                ImGui.Spacing();
                ImGui.Separator();
                ImGui.Spacing();

                var useGlv = preset.ItemLevel.UseGlv;
                using var box = ImRaii.ListBox("##ConfigPresetListbox", new Vector2(-1.5f * ImGui.GetStyle().ItemSpacing.X, ImGui.GetFrameHeightWithSpacing() * 3 + ItemSpacing.Y));
                Span<int> ilvl = [preset.ItemLevel.Min, preset.ItemLevel.Max];
                ImGui.SetNextItemWidth(SetInputWidth);
                if (ImGui.DragInt2("Minimum and maximum item", ref ilvl[0], 0.2f, 1,  useGlv ? ConfigPreset.MaxGvl : ConfigPreset.MaxLevel))
                {
                    state.ChangingMin = preset.ItemLevel.Min != ilvl[0];
                    preset.ItemLevel.Min = ilvl[0];
                    preset.ItemLevel.Max = ilvl[1];
                }
                if (ImGui.IsItemDeactivatedAfterEdit())
                {
                    if (preset.ItemLevel.Min > preset.ItemLevel.Max)
                    {
                        if (state.ChangingMin)
                            preset.ItemLevel.Max = preset.ItemLevel.Min;
                        else
                            preset.ItemLevel.Min = preset.ItemLevel.Max;
                    }
                    selector.Save();
                }
                ImGui.SameLine();
                if (ImGui.RadioButton("level", !useGlv)) useGlv = false;
                ImGui.SameLine();
                if (ImGui.RadioButton("glv", useGlv)) useGlv = true;
                if (useGlv != preset.ItemLevel.UseGlv)
                {
                    int min, max;
                    if (useGlv)
                    {
                        min = GatherBuddy.GameData.Gatherables.Values
                            .Where(i => i.Level >= preset.ItemLevel.Min)
                            .Select(i => (int)i.GatheringData.GatheringItemLevel.RowId)
                            .DefaultIfEmpty(ConfigPreset.MaxGvl)
                            .Min();
                        max = GatherBuddy.GameData.Gatherables.Values
                            .Where(i => i.Level <= preset.ItemLevel.Max)
                            .Select(i => (int)i.GatheringData.GatheringItemLevel.RowId)
                            .DefaultIfEmpty(1)
                            .Max();
                    }
                    else
                    {
                        min = GatherBuddy.GameData.Gatherables.Values
                            .Where(i => i.GatheringData.GatheringItemLevel.RowId >= preset.ItemLevel.Min)
                            .Select(i => i.Level)
                            .DefaultIfEmpty(ConfigPreset.MaxLevel)
                            .Min();
                        max = GatherBuddy.GameData.Gatherables.Values
                            .Where(i => i.GatheringData.GatheringItemLevel.RowId <= preset.ItemLevel.Max)
                            .Select(i => i.Level)
                            .DefaultIfEmpty(1)
                            .Max();
                    }
                    preset.ItemLevel.UseGlv = useGlv;
                    preset.ItemLevel.Min = min;
                    preset.ItemLevel.Max = max;
                    selector.Save();
                }

                ImGui.Text("Node types:");
                ImGui.SameLine();
                if (ImGuiUtil.Checkbox("Regular", "", preset.NodeType.Regular, x => preset.NodeType.Regular = x)) selector.Save();
                ImGui.SameLine(0, ImGui.CalcTextSize("Crystals").X - ImGui.CalcTextSize("Regular").X + ItemSpacing.X);
                if (ImGuiUtil.Checkbox("Unspoiled", "", preset.NodeType.Unspoiled, x => preset.NodeType.Unspoiled = x)) selector.Save();
                ImGui.SameLine(0, ImGui.CalcTextSize("Collectables").X - ImGui.CalcTextSize("Unspoiled").X + ItemSpacing.X);
                if (ImGuiUtil.Checkbox("Legendary", "", preset.NodeType.Legendary, x => preset.NodeType.Legendary = x)) selector.Save();
                ImGui.SameLine();
                if (ImGuiUtil.Checkbox("Ephemeral", "", preset.NodeType.Ephemeral, x => preset.NodeType.Ephemeral = x)) selector.Save();

                ImGui.Text("Item types:");
                ImGui.SameLine(0, ImGui.CalcTextSize("Node types:").X - ImGui.CalcTextSize("Item types:").X + ItemSpacing.X);
                if (ImGuiUtil.Checkbox("Crystals", "", preset.ItemType.Crystals, x => preset.ItemType.Crystals = x)) selector.Save();
                ImGui.SameLine();
                if (ImGuiUtil.Checkbox("Collectables", "", preset.ItemType.Collectables, x => preset.ItemType.Collectables = x)) selector.Save();
                ImGui.SameLine();
                if (ImGuiUtil.Checkbox("Other", "", preset.ItemType.Other, x => preset.ItemType.Other = x)) selector.Save();
            }

            using var child = ImRaii.Child("ConfigPresetSettings", new Vector2(-1.5f * ItemSpacing.X, -ItemSpacing.Y));

            using var width = ImRaii.ItemWidth(SetInputWidth);

            using (var node = ImRaii.TreeNode("General Settings", ImGuiTreeNodeFlags.Framed))
            {
                if (node)
                {
                    if (preset.ItemType.Crystals || preset.ItemType.Other)
                    {
                        var tmp = preset.GatherableMinGP;
                        if (ImGui.DragInt("Minimum GP for gathering regular items or crystals", ref tmp, 1f, 0, ConfigPreset.MaxGP))
                            preset.GatherableMinGP = tmp;
                        if (ImGui.IsItemDeactivatedAfterEdit())
                            selector.Save();
                    }

                    if (preset.ItemType.Collectables)
                    {
                        var tmp = preset.CollectableMinGP;
                        if (ImGui.DragInt("Minimum GP for collecting collectables", ref tmp, 1f, 0, ConfigPreset.MaxGP))
                            preset.CollectableMinGP = tmp;
                        if (ImGui.IsItemDeactivatedAfterEdit())
                            selector.Save();

                        tmp = preset.CollectableActionsMinGP;
                        if (ImGui.DragInt("Minimum GP for using actions on collectibles", ref tmp, 1f, 0, ConfigPreset.MaxGP))
                            preset.CollectableActionsMinGP = tmp;
                        if (ImGui.IsItemDeactivatedAfterEdit())
                            selector.Save();

                        ImGui.SameLine();
                        if (ImGuiUtil.Checkbox($"Always use {ConcatNames(Actions.SolidAge)}",
                            $"Use {ConcatNames(Actions.SolidAge)} regardless of starting GP if target collectability score is reached",
                            preset.CollectableAlwaysUseSolidAge,
                            x => preset.CollectableAlwaysUseSolidAge = x))
                            selector.Save();

                        tmp = preset.CollectableTagetScore;
                        if (ImGui.DragInt("Target collectability score to reach before collecting", ref tmp, 1f, 0, ConfigPreset.MaxCollectability))
                            preset.CollectableTagetScore = tmp;
                        if (ImGui.IsItemDeactivatedAfterEdit())
                            selector.Save();

                        tmp = preset.CollectableMinScore;
                        if (ImGui.DragInt($"Minimum collectability score to collect on the last integrity point ({ConfigPreset.MaxCollectability} to disable)", ref tmp, 1f, 0, ConfigPreset.MaxCollectability))
                            preset.CollectableMinScore = tmp;
                        if (ImGui.IsItemDeactivatedAfterEdit())
                            selector.Save();
                    }

                }
            }
            using var width2 = ImRaii.ItemWidth(SetInputWidth - ImGui.GetStyle().IndentSpacing);
            if (preset.ItemType.Crystals || preset.ItemType.Other)
            {
                using var node = ImRaii.TreeNode("Gathering Actions", ImGuiTreeNodeFlags.Framed);
                if (node)
                {
                    DrawActionConfig(ConcatNames(Actions.Bountiful), preset.GatherableActions.Bountiful, selector.Save);
                    DrawActionConfig(ConcatNames(Actions.Yield1), preset.GatherableActions.Yield1, selector.Save);
                    DrawActionConfig(ConcatNames(Actions.Yield2), preset.GatherableActions.Yield2, selector.Save);
                    DrawActionConfig(ConcatNames(Actions.SolidAge), preset.GatherableActions.SolidAge, selector.Save);
                    DrawActionConfig(ConcatNames(Actions.Gift1), preset.GatherableActions.Gift1, selector.Save);
                    DrawActionConfig(ConcatNames(Actions.Gift2), preset.GatherableActions.Gift2, selector.Save);
                    DrawActionConfig(ConcatNames(Actions.Tidings), preset.GatherableActions.Tidings, selector.Save);
                    if (preset.ItemType.Crystals)
                    {
                        DrawActionConfig(Actions.TwelvesBounty.Names.Botanist, preset.GatherableActions.TwelvesBounty, selector.Save);
                        DrawActionConfig(Actions.GivingLand.Names.Botanist, preset.GatherableActions.GivingLand, selector.Save);
                    }
                }
            }
            if (preset.ItemType.Collectables)
            {
                using var node = ImRaii.TreeNode("Collectable Actions", ImGuiTreeNodeFlags.Framed);
                if (node)
                {
                    DrawActionConfig(Actions.Scour.Names.Botanist, preset.CollectableActions.Scour, selector.Save);
                    DrawActionConfig(Actions.Brazen.Names.Botanist, preset.CollectableActions.Brazen, selector.Save);
                    DrawActionConfig(Actions.Meticulous.Names.Botanist, preset.CollectableActions.Meticulous, selector.Save);
                    DrawActionConfig(Actions.Scrutiny.Names.Botanist, preset.CollectableActions.Scrutiny, selector.Save);
                    DrawActionConfig(ConcatNames(Actions.SolidAge), preset.CollectableActions.SolidAge, selector.Save);
                }
            }
            {
                using var node = ImRaii.TreeNode("Consumables", ImGuiTreeNodeFlags.Framed);
                if (node)
                {
                    DrawActionConfig("Cordial", preset.Consumables.Cordial, selector.Save, PossibleCordials);
                    DrawActionConfig("Food", preset.Consumables.Food, selector.Save, PossibleFoods, true);
                    DrawActionConfig("Potion", preset.Consumables.Potion, selector.Save, PossiblePotions, true);
                    DrawActionConfig("Manual", preset.Consumables.Manual, selector.Save, PossibleManuals, true);
                    DrawActionConfig("Squadron Manual", preset.Consumables.SquadronManual, selector.Save, PossibleSquadronManuals, true);
                    DrawActionConfig("Squadron Pass", preset.Consumables.SquadronPass, selector.Save, PossibleSquadronPasses, true);
                }
            }

            static string ConcatNames(Actions.BaseAction action) => $"{action.Names.Miner} / {action.Names.Botanist}";
        }

        private void DrawActionConfig(string name, ConfigPreset.ActionConfig action, System.Action save, IEnumerable<Item>? items = null, bool hideGP = false)
        {
            using var node = ImRaii.TreeNode(name);
            if (!node) return;

            ref var state = ref _configPresetsUIState;

            if (ImGuiUtil.Checkbox("Enabled", "", action.Enabled, x => action.Enabled = x)) save();
            if (!action.Enabled) return;

            if (action is ConfigPreset.ActionConfigIntegrity action2)
            {
                if (ImGuiUtil.Checkbox("Use only on first step", "Use only if no items have been gathered from the node yet", action2.FirstStepOnly, x => action2.FirstStepOnly = x)) save();
            }

            if (!hideGP)
            {
                Span<int> gp = [action.MinGP, action.MaxGP];
                if (ImGui.DragInt2("Minimum and maximum GP", ref gp[0], 1, 0, ConfigPreset.MaxGP))
                {
                    state.ChangingMin = action.MinGP != gp[0];
                    action.MinGP = gp[0];
                    action.MaxGP = gp[1];
                }
                if (ImGui.IsItemDeactivatedAfterEdit())
                {
                    if (action.MinGP > action.MaxGP)
                    {
                        if (state.ChangingMin)
                            action.MaxGP = action.MinGP;
                        else
                            action.MinGP = action.MaxGP;
                    }
                    save();
                }
            }

            if (action is ConfigPreset.ActionConfigBoon action3)
            {
                Span<int> chance = [action3.MinBoonChance, action3.MaxBoonChance];
                if (ImGui.DragInt2("Minimum and maximum boon chance", ref chance[0], 0.2f, 0, 100))
                {
                    state.ChangingMin = action3.MinBoonChance != chance[0];
                    action3.MinBoonChance = chance[0];
                    action3.MaxBoonChance = chance[1];
                }
                if (ImGui.IsItemDeactivatedAfterEdit())
                {
                    if (action3.MinBoonChance > action3.MaxBoonChance)
                    {
                        if (state.ChangingMin)
                            action3.MaxBoonChance = action3.MinBoonChance;
                        else
                            action3.MinBoonChance = action3.MaxBoonChance;
                    }
                    save();
                }
            }

            if (action is ConfigPreset.ActionConfigIntegrity action4)
            {
                var tmp = action4.MinIntegrity;
                if (ImGui.DragInt("Minumum initial node integrity", ref tmp, 0.1f, 1, ConfigPreset.MaxIntegrity))
                    action4.MinIntegrity = tmp;
                if (ImGui.IsItemDeactivatedAfterEdit())
                    save();
            }
            if (action is ConfigPreset.ActionConfigYieldBonus action5)
            {
                var tmp = action5.MinYieldBonus;
                if (ImGui.DragInt("Minimum yield bonus", ref tmp, 0.1f, 1, 3))
                    action5.MinYieldBonus = tmp;
                if (ImGui.IsItemDeactivatedAfterEdit())
                    save();
            }

            if (action is ConfigPreset.ActionConfigYieldTotal action6)
            {
                var tmp = action6.MinYieldTotal;
                if (ImGui.DragInt("Minimum total yield", ref tmp, 0.1f, 1, 30))
                    action6.MinYieldTotal = tmp;
                if (ImGui.IsItemDeactivatedAfterEdit())
                    save();
            }

            if (action is ConfigPreset.ActionCofigConsumable action7 && items != null)
            {
                var list = items
                    .SelectMany(item => new[] { (item, rowid: item.RowId), (item, rowid: item.RowId + 100000) })
                    .Where(x => x.item.CanBeHq || x.rowid < 100000)
                    .Select(x => (name: x.item.Name.ExtractText(), x.rowid, count: GetInventoryItemCount(x.rowid)))
                    .OrderBy(x => x.count == 0)
                    .ThenBy(x => x.name)
                    .Select(x => x with { name = $"{(x.rowid > 100000 ? " " : "")}{x.name} ({x.count})" } )
                    .ToList();

                var selected = (action7.ItemId > 0 ? list.FirstOrDefault(x => x.rowid == action7.ItemId).name : null) ?? string.Empty;
                using var combo = ImRaii.Combo($"Select {name.ToLower()}", selected);
                if (combo)
                {
                    if (ImGui.Selectable(string.Empty, action7.ItemId <= 0))
                    {
                        action7.ItemId = 0;
                        save();
                    }

                    bool? separatorState = null;
                    foreach (var (itemname, rowid, count) in list)
                    {
                        if (count != 0) separatorState = true;
                        else if (separatorState ?? false)
                        {
                            ImGui.Separator();
                            separatorState = false;
                        }

                        if (ImGui.Selectable(itemname, action7.ItemId == rowid))
                        {
                            action7.ItemId = rowid;
                            save();
                        }
                    }
                }
            }
        }
    }
}
