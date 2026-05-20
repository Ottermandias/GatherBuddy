using System;
using System.Collections.Generic;
using Dalamud.Game.Gui.ContextMenu;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using GatherBuddy.Interfaces;

namespace GatherBuddy.Plugin;

public class ContextMenu : IDisposable
{
    private readonly IContextMenu _contextMenu;
    private readonly Executor     _executor;
    private          IGatherable? _lastGatherable;

    private readonly MenuItem _menuItem;

    public ContextMenu(IContextMenu menu, Executor executor)
    {
        _contextMenu = menu;
        _executor    = executor;

        _menuItem = new MenuItem
        {
            IsEnabled   = true,
            IsReturn    = false,
            PrefixChar  = 'G',
            Name        = "Gather",
            OnClicked   = OnClick,
            IsSubmenu   = false,
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
                "ContentsInfoDetail" => HandleItem(AgentContentsTimer.Instance()->ContextMenuItemId), // Provisioning
                "RecipeNote"         => HandleItem(AgentRecipeNote.Instance()->ContextMenuResultItemId),
                "RecipeTree"         => HandleItem(AgentRecipeItemContext.Instance()->ResultItemId),
                "RecipeMaterialList" => HandleItem(AgentRecipeItemContext.Instance()->ResultItemId),
                "GatheringNote"      => HandleGatheringNote(args),
                "ItemSearch"         => HandleItem((uint)AgentContext.Instance()->UpdateCheckerParam),
                "ChatLog"            => HandleChatLog(args),
                _                    => null,
            };
        }

        if (_lastGatherable != null)
            args.AddMenuItem(_menuItem);
    }

    private static unsafe IGatherable? HandleGatheringNote(IMenuOpenedArgs args)
    {
        // This seems to be 1 when a location context is opened,
        // and 4 when an item context is opened.
        var discriminator = *(byte*)(args.AgentPtr + Offsets.GatheringNoteContextDiscriminator);
        if (discriminator != 4)
            return null;

        return HandleItem(AgentGatheringNote.Instance()->ContextMenuItemId);
    }

    private unsafe IGatherable? HandleChatLog(IMenuOpenedArgs args)
    {
        var agent = AgentChatLog.Instance();

        if (*(uint*)((nint)(&agent->ContextItemId) + 8) != 3) // Validate context
            return null;

        return HandleItem(agent->ContextItemId);
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

    private unsafe IGatherable? HandleSatisfactionSupply()
    {
        var agent = AgentSatisfactionSupply.Instance();

        var itemIdx = agent->NpcInfo.SelectedItemIndex;
        if (itemIdx < 0 || itemIdx >= agent->Items.Length)
            return null;

        return HandleItem(agent->Items[itemIdx].Id);
    }
}
