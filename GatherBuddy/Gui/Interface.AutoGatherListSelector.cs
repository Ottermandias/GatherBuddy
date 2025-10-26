using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Bindings.ImGui;
using GatherBuddy.AutoGather.Lists;
using GatherBuddy.Config;
using GatherBuddy.Classes;
using GatherBuddy.Plugin;
using OtterGui;
using OtterGui.Classes;
using OtterGui.FileSystem.Selector;
using OtterGui.Filesystem;
using OtterGui.Log;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private class AutoGatherListFileSystemSelector : FileSystemSelector<AutoGatherList, int>
    {
        private static readonly ManualOrderSortMode _manualOrderSortMode = new();

        public override ISortMode<AutoGatherList> SortMode
            => _manualOrderSortMode;

        public void RefreshView()
        {
            SetFilterDirty();
        }

        public float SelectorWidth
        {
            get => GatherBuddy.Config.AutoGatherListSelectorWidth * ImGuiHelpers.GlobalScale;
            set
            {
                GatherBuddy.Config.AutoGatherListSelectorWidth = value / ImGuiHelpers.GlobalScale;
                GatherBuddy.Config.Save();
            }
        }

        public AutoGatherListFileSystemSelector()
            : base(_plugin.AutoGatherListsManager.FileSystem, Dalamud.Keys, new Logger(), null, "##AutoGatherListsFileSystem", false)
        {
            SetFilterDirty();
            AddButton(AddListButton, 0);
            AddButton(ImportFromClipboardButton, 10);
            SubscribeRightClickLeaf(MoveUpContext, 50);
            SubscribeRightClickLeaf(MoveDownContext, 60);
            SubscribeRightClickLeaf(DeleteListContext, 100);
            SubscribeRightClickLeaf(DuplicateListContext, 200);
            SubscribeRightClickLeaf(ToggleListContext, 300);
            SubscribeRightClickLeaf(ExportListContext, 400);
            SubscribeRightClickFolder(CreateFolderContext, 500);
            SubscribeRightClickFolder(DeleteFolderContext, 600);
            UnsubscribeRightClickLeaf(RenameLeaf);
        }

        protected override bool FoldersDefaultOpen
            => false;

        protected override uint ExpandedFolderColor
            => 0xFFFFFFFF;

        protected override uint CollapsedFolderColor
            => 0xFFFFFFFF;

        protected override void DrawLeafName(FileSystem<AutoGatherList>.Leaf leaf, in int state, bool selected)
        {
            var list = leaf.Value;
            var flag = selected ? ImGuiTreeNodeFlags.Selected | LeafFlags : LeafFlags;
            
            using var color = ImRaii.PushColor(ImGuiCol.Text, ColorId.DisabledText.Value(), !list.Enabled);
            var displayName = CheckUnnamed(list.Name);
            
            using var _ = ImRaii.TreeNode(displayName, flag);
            
            if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
            {
                _plugin.AutoGatherListsManager.ToggleList(list);
            }
        }

        protected override int GetState(FileSystem<AutoGatherList>.IPath path)
            => 0;

        private void AddListButton(Vector2 size)
        {
            const string newListName = "newListName";
            if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Plus.ToIconString(), size, "Create a new auto-gather list.", false, true))
                ImGui.OpenPopup(newListName);

            string name = string.Empty;
            if (ImGuiUtil.OpenNameField(newListName, ref name) && name.Length > 0)
            {
                var list = new AutoGatherList() { Name = name };
                _plugin.AutoGatherListsManager.AddList(list);
            }
        }

        private void ImportFromClipboardButton(Vector2 size)
        {
            const string importName = "importListName";
            if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Clipboard.ToIconString(), size, "Import an auto-gather list from clipboard.", false, true))
                ImGui.OpenPopup(importName);

            string name = string.Empty;
            if (ImGuiUtil.OpenNameField(importName, ref name) && name.Length > 0)
            {
                var clipboardText = ImGuiUtil.GetClipboardText();
                if (AutoGatherList.Config.FromBase64(clipboardText, out var cfg))
                {
                    AutoGatherList.FromConfig(cfg, out var list);
                    list.Name = name;
                    _plugin.AutoGatherListsManager.AddList(list);
                }
            }
        }

        private void MoveUpContext(FileSystem<AutoGatherList>.Leaf leaf)
        {
            if (ImGui.MenuItem("Move Up"))
                _plugin.AutoGatherListsManager.MoveListUp(leaf.Value);
        }

        private void MoveDownContext(FileSystem<AutoGatherList>.Leaf leaf)
        {
            if (ImGui.MenuItem("Move Down"))
                _plugin.AutoGatherListsManager.MoveListDown(leaf.Value);
        }

        private void DeleteListContext(FileSystem<AutoGatherList>.Leaf leaf)
        {
            if (ImGui.MenuItem("Delete List"))
                _plugin.AutoGatherListsManager.DeleteList(leaf.Value);
        }

        private void DuplicateListContext(FileSystem<AutoGatherList>.Leaf leaf)
        {
            if (ImGui.MenuItem("Duplicate List"))
            {
                var clone = leaf.Value.Clone();
                clone.Name = $"{leaf.Value.Name} (Copy)";
                _plugin.AutoGatherListsManager.AddList(clone, leaf.Parent);
            }
        }

        private void ToggleListContext(FileSystem<AutoGatherList>.Leaf leaf)
        {
            var list = leaf.Value;
            if (ImGui.MenuItem(list.Enabled ? "Disable" : "Enable"))
                _plugin.AutoGatherListsManager.ToggleList(list);
        }

        private void ExportListContext(FileSystem<AutoGatherList>.Leaf leaf)
        {
            if (ImGui.MenuItem("Export to Clipboard"))
            {
                try
                {
                    var config = new AutoGatherList.Config(leaf.Value);
                    var base64 = config.ToBase64();
                    ImGui.SetClipboardText(base64);
                    Communicator.PrintClipboardMessage("Auto-gather list", leaf.Value.Name);
                }
                catch (Exception e)
                {
                    Communicator.PrintClipboardMessage("Auto-gather list", leaf.Value.Name, e);
                }
            }
        }

        private void CreateFolderContext(FileSystem<AutoGatherList>.Folder folder)
        {
            const string newFolderName = "newFolderName";
            if (ImGui.MenuItem("Create Subfolder"))
                ImGui.OpenPopup(newFolderName);

            string name = string.Empty;
            if (ImGuiUtil.OpenNameField(newFolderName, ref name) && name.Length > 0)
            {
                _plugin.AutoGatherListsManager.CreateFolder(name, folder);
            }
        }

        private void DeleteFolderContext(FileSystem<AutoGatherList>.Folder folder)
        {
            if (folder.IsRoot)
                return;

            if (ImGui.MenuItem("Delete Folder"))
            {
                _plugin.AutoGatherListsManager.DeleteFolder(folder);
            }
        }
    }
}
