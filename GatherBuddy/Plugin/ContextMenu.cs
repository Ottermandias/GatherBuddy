using System;
using Dalamud.ContextMenu;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

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
            return new InventoryContextMenuItem(GatherString, _ => _executor.GatherItem(gatherable));
        if (GatherBuddy.GameData.Fishes.TryGetValue(itemId, out var fish))
            return new InventoryContextMenuItem(GatherString, _ => _executor.GatherItem(fish));

        return null;
    }

    private GameObjectContextMenuItem? CheckGameObjectItem(uint itemId)
    {
        if (itemId > 500000)
            itemId -= 500000;

        if (GatherBuddy.GameData.Gatherables.TryGetValue(itemId, out var gatherable))
            return new GameObjectContextMenuItem(GatherString, _ => _executor.GatherItem(gatherable));
        if (GatherBuddy.GameData.Fishes.TryGetValue(itemId, out var fish))
            return new GameObjectContextMenuItem(GatherString, _ => _executor.GatherItem(fish));

        return null;
    }

    private unsafe GameObjectContextMenuItem? CheckGameObjectItem(IntPtr agent, int offset)
        => agent != IntPtr.Zero ? CheckGameObjectItem(*(uint*)(agent + offset)) : null;

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
            "ContentsInfoDetail" => CheckGameObjectItem("ContentsInfo",  Offsets.ContentsInfoDetailContextItemId),
            "RecipeNote"         => CheckGameObjectItem("RecipeNote",    Offsets.RecipeNoteContextItemId),
            "GatheringNote"      => CheckGameObjectItem("GatheringNote", Offsets.GatheringNoteContextItemId),
            "ItemSearch"         => CheckGameObjectItem(args.Agent,      Offsets.ItemSearchContextItemId),
            "ChatLog"            => CheckGameObjectItem("ChatLog",       Offsets.ChatLogContextItemId),
            _                    => null,
        };
        if (item != null)
            args.AddCustomItem(item);
    }

    private void AddInventoryItem(InventoryContextMenuOpenArgs args)
    {
        var item = CheckInventoryItem(args.ItemId);
        if (item != null)
            args.AddCustomItem(item);
    }
}
