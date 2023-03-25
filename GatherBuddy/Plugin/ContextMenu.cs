using System;
using Dalamud.ContextMenu;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;

namespace GatherBuddy.Plugin;

public class ContextMenu : IDisposable
{
    private readonly DalamudContextMenu _contextMenu = new();
    private readonly Executor           _executor;

    public ContextMenu(Executor executor)
    {
        _executor = executor;
        if (GatherBuddy.Config.AddIngameContextMenus)
            Enable();
    }

    public void Enable()
    {
        _contextMenu.OnOpenGameObjectContextMenu += AddGameObjectItem;
        _contextMenu.OnOpenInventoryContextMenu  += AddInventoryItem;
    }

    public void Disable()
    {
        _contextMenu.OnOpenGameObjectContextMenu -= AddGameObjectItem;
        _contextMenu.OnOpenInventoryContextMenu  -= AddInventoryItem;
    }

    public void Dispose()
    {
        Disable();
        _contextMenu.Dispose();
    }

    private static readonly SeString GatherString = new(new TextPayload("Gather"));

    private InventoryContextMenuItem? CheckInventoryItem(uint itemId)
    {
        if (itemId > 500000)
            itemId -= 500000;

        if (GatherBuddy.GameData.Gatherables.TryGetValue(itemId, out var gatherable))
            return new InventoryContextMenuItem(GatherString, _ => _executor.GatherItem(gatherable), true);
        if (GatherBuddy.GameData.Fishes.TryGetValue(itemId, out var fish))
            return new InventoryContextMenuItem(GatherString, _ => _executor.GatherItem(fish), true);

        return null;
    }

    private GameObjectContextMenuItem? CheckGameObjectItem(uint itemId)
    {
        if (itemId > 500000)
            itemId -= 500000;

        if (GatherBuddy.GameData.Gatherables.TryGetValue(itemId, out var gatherable))
            return new GameObjectContextMenuItem(GatherString, _ => _executor.GatherItem(gatherable), true);
        if (GatherBuddy.GameData.Fishes.TryGetValue(itemId, out var fish))
            return new GameObjectContextMenuItem(GatherString, _ => _executor.GatherItem(fish), true);

        return null;
    }

    private unsafe GameObjectContextMenuItem? CheckGameObjectItem(IntPtr agent, int offset, Func<nint, bool> validate)
        => agent != IntPtr.Zero && validate(agent) ? CheckGameObjectItem(*(uint*)(agent + offset)) : null;

    private unsafe GameObjectContextMenuItem? CheckGameObjectItem(IntPtr agent, int offset)
        => agent != IntPtr.Zero ? CheckGameObjectItem(*(uint*)(agent + offset)) : null;

    private GameObjectContextMenuItem? CheckGameObjectItem(string name, int offset, Func<nint, bool> validate)
        => CheckGameObjectItem(Dalamud.GameGui.FindAgentInterface(name), offset, validate);

    private GameObjectContextMenuItem? CheckGameObjectItem(string name, int offset)
        => CheckGameObjectItem(Dalamud.GameGui.FindAgentInterface(name), offset);

    private unsafe GameObjectContextMenuItem? HandleSatisfactionSupply()
    {
        var agent = Dalamud.GameGui.FindAgentInterface("SatisfactionSupply");
        if (agent == IntPtr.Zero)
            return null;

        var itemIdx = *(byte*)(agent + Offsets.SatisfactionSupplyItemIdx);
        return itemIdx switch
        {
            1 => CheckGameObjectItem(*(uint*)(agent + Offsets.SatisfactionSupplyItem1Id)),
            2 => CheckGameObjectItem(*(uint*)(agent + Offsets.SatisfactionSupplyItem2Id)),
            _ => null,
        };
    }

    private void AddGameObjectItem(GameObjectContextMenuOpenArgs args)
    {
        var item = args.ParentAddonName switch
        {
            null                 => HandleSatisfactionSupply(),
            "ContentsInfoDetail" => CheckGameObjectItem("ContentsInfo",                       Offsets.ContentsInfoDetailContextItemId), // Provisioning
            "RecipeNote"         => CheckGameObjectItem("RecipeNote",                         Offsets.RecipeNoteContextItemId),
            "RecipeTree"         => CheckGameObjectItem(AgentById(AgentId.RecipeItemContext), Offsets.AgentItemContextItemId),
            "RecipeMaterialList" => CheckGameObjectItem(AgentById(AgentId.RecipeItemContext), Offsets.AgentItemContextItemId),
            "GatheringNote"      => CheckGameObjectItem("GatheringNote",                      Offsets.GatheringNoteContextItemId),
            "ItemSearch"         => CheckGameObjectItem(args.Agent,                           Offsets.ItemSearchContextItemId),
            "ChatLog"            => CheckGameObjectItem("ChatLog",                            Offsets.ChatLogContextItemId, ValidateChatLogContext),
            _                    => null,
        };
        if (item != null)
            args.AddCustomItem(item);
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

    private void AddInventoryItem(InventoryContextMenuOpenArgs args)
    {
        var item = CheckInventoryItem(args.ItemId);
        if (item != null)
            args.AddCustomItem(item);
    }
}
