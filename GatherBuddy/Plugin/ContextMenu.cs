using System;
using System.Collections.Generic;
using Dalamud.Game.Gui.ContextMenu;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using GatherBuddy.Classes;
using GatherBuddy.Interfaces;

namespace GatherBuddy.Plugin;

public class ContextMenu : IDisposable
{
    private readonly IContextMenu _contextMenu;
    private readonly Executor     _executor;
    private          IGatherable? _lastGatherable;
    private          GatherBuddy  _plugin;

    private readonly MenuItem _menuItem;
    private readonly MenuItem _menuItemAuto;

    public ContextMenu(GatherBuddy plugin, IContextMenu menu, Executor executor)
    {
        _plugin = plugin;
        _contextMenu = menu;
        _executor    = executor;

        _menuItem = new MenuItem
        {
            IsEnabled   = true,
            IsReturn    = false,
            PrefixChar  = 'G',
            Name        = "Gather Manually",
            OnClicked   = OnClick,
            IsSubmenu   = false,
            PrefixColor = 42,
        };

        _menuItemAuto = new MenuItem
        {
            IsEnabled = true,
            IsReturn = false,
            PrefixChar = 'G',
            Name = "Add to Auto-Gather List",
            OnClicked = OnClickAuto,
            IsSubmenu = false,
            PrefixColor = 42,
        };

        if (GatherBuddy.Config.AddIngameContextMenus)
            Enable();
    }

    private void OnClick(IMenuItemClickedArgs args)
    {
        if (_lastGatherable != null)
            _executor.GatherItem(_lastGatherable);
    }

    private void OnClickAuto(IMenuItemClickedArgs args)
    {
        if (_lastGatherable is Gatherable gatherable)
        {
            var preset = _plugin.Interface.CurrentGatherWindowPreset;

            if (preset == null)
            {
                preset = new();
                _plugin.GatherWindowManager.AddPreset(preset);
            }

            _plugin.GatherWindowManager.AddItem(preset, gatherable);
        }            
    }

    public void Enable()
        => _contextMenu.OnMenuOpened += OnContextMenuOpened;

    public void Disable()
        => _contextMenu.OnMenuOpened -= OnContextMenuOpened;

    public void Dispose()
        => Disable();

    private unsafe void OnContextMenuOpened(IMenuOpenedArgs args)
    {
        if (args.MenuType is ContextMenuType.Inventory)
        {
            var target = (MenuTargetInventory)args.Target;
            _lastGatherable = target.TargetItem.HasValue ? HandleItem(target.TargetItem.Value.ItemId) : null;
        }
        else
        {
            _lastGatherable = args.AddonName switch
            {
                null                 => HandleSatisfactionSupply(),
                "ContentsInfoDetail" => CheckGameObjectItem("ContentsInfo", Offsets.ContentsInfoDetailContextItemId), // Provisioning
                "RecipeNote"         => CheckGameObjectItem("RecipeNote", Offsets.RecipeNoteContextItemId),
                "RecipeTree"         => CheckGameObjectItem(AgentById(AgentId.RecipeItemContext), Offsets.AgentItemContextItemId),
                "RecipeMaterialList" => CheckGameObjectItem(AgentById(AgentId.RecipeItemContext), Offsets.AgentItemContextItemId),
                "GatheringNote"      => CheckGatheringNote(args),
                "ItemSearch"         => HandleItem((uint)AgentContext.Instance()->UpdateCheckerParam),
                "ChatLog"            => CheckGameObjectItem("ChatLog", Offsets.ChatLogContextItemId, ValidateChatLogContext),
                _                    => null,
            };
        }

        if (_lastGatherable != null)
            args.AddMenuItem(_menuItem);
        if (_lastGatherable is Gatherable)
            args.AddMenuItem(_menuItemAuto);
    }

    private static unsafe IGatherable? CheckGatheringNote(IMenuOpenedArgs args)
    {
        var agent = Dalamud.GameGui.FindAgentInterface("GatheringNote");
        if (agent == IntPtr.Zero)
            return null;

        // This seems to be 1 when a location context is opened,
        // and 4 when an item context is opened.
        var discriminator = *(byte*)(args.AgentPtr + Offsets.GatheringNoteContextDiscriminator);
        if (discriminator != 4)
            return null;

        return HandleItem(*(uint*)(agent + Offsets.GatheringNoteContextItemId));
    }

    private static IGatherable? HandleItem(uint itemId)
    {
        if (itemId >= 1000000u)
            itemId -= 1000000u;
        else if (itemId >= 500000u)
            itemId -= 500000u;

        if (GatherBuddy.GameData.Gatherables.TryGetValue(itemId, out var g))
            return g;

        return GatherBuddy.GameData.Fishes.GetValueOrDefault(itemId);
    }

    private unsafe IGatherable? CheckGameObjectItem(IntPtr agent, int offset, Func<nint, bool> validate)
        => agent != IntPtr.Zero && validate(agent) ? HandleItem(*(uint*)(agent + offset)) : null;

    private unsafe IGatherable? CheckGameObjectItem(IntPtr agent, int offset)
        => agent != IntPtr.Zero ? HandleItem(*(uint*)(agent + offset)) : null;

    private IGatherable? CheckGameObjectItem(string name, int offset, Func<nint, bool> validate)
        => CheckGameObjectItem(Dalamud.GameGui.FindAgentInterface(name), offset, validate);

    private IGatherable? CheckGameObjectItem(string name, int offset)
        => CheckGameObjectItem(Dalamud.GameGui.FindAgentInterface(name), offset);

    private unsafe IGatherable? HandleSatisfactionSupply()
    {
        var agent = Dalamud.GameGui.FindAgentInterface("SatisfactionSupply");
        if (agent == IntPtr.Zero)
            return null;

        var itemIdx = *(byte*)(agent + Offsets.SatisfactionSupplyItemIdx);
        return itemIdx switch
        {
            1 => HandleItem(*(uint*)(agent + Offsets.SatisfactionSupplyItem1Id)),
            2 => HandleItem(*(uint*)(agent + Offsets.SatisfactionSupplyItem2Id)),
            _ => null,
        };
    }

    private static unsafe bool ValidateChatLogContext(nint agent)
        => *(uint*)(agent + Offsets.ChatLogContextItemId + 8) == 3;

    private static unsafe IntPtr AgentById(AgentId id)
    {
        var uiModule = (UIModule*)Dalamud.GameGui.GetUIModule();
        var agents   = uiModule->GetAgentModule();
        var agent    = agents->GetAgentByInternalId(id);
        return (IntPtr)agent;
    }
}
