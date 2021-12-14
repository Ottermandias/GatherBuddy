using System;
using Dalamud.Game.Gui.ContextMenus;
using Dalamud.Logging;
using GatherBuddy.Plugin;

namespace GatherBuddy;

public class ContextMenu : IDisposable
{
    private readonly Executor _executor;

    public ContextMenu(Executor executor)
    {
        _executor = executor;
        if (GatherBuddy.Config.AddIngameContextMenus)
            Enable();
    }

    public void Enable()
        => Dalamud.ContextMenu.ContextMenuOpened += AddMenuItem;

    public void Disable()
        => Dalamud.ContextMenu.ContextMenuOpened -= AddMenuItem;

    public void Dispose()
        => Disable();

    private bool AddEntry(ContextMenuOpenedArgs args, uint id)
    {
        if (id > 500000)
            id -= 500000;

        if (GatherBuddy.GameData.Gatherables.TryGetValue(id, out var gatherable))
            args.AddCustomItem("Gather", _ => _executor.GatherItem(gatherable));
        else if (GatherBuddy.GameData.Fishes.TryGetValue(id, out var fish))
            args.AddCustomItem("Gather", _ => _executor.GatherItem(fish));
        return true;
    }

    private unsafe bool AddEntry(ContextMenuOpenedArgs args, IntPtr agent, int offset)
        => agent != IntPtr.Zero && AddEntry(args, *(uint*)(agent + offset));

    private bool AddEntry(ContextMenuOpenedArgs args, string name, int offset)
        => AddEntry(args, Dalamud.GameGui.FindAgentInterface(name), offset);

    private unsafe bool HandleSatisfactionSupply(ContextMenuOpenedArgs args)
    {
        var agent = Dalamud.GameGui.FindAgentInterface("SatisfactionSupply");
        if (agent == IntPtr.Zero)
            return false;

        var itemIdx = *(byte*)(agent + 0x54);
        return itemIdx switch
        {
            1 => AddEntry(args, *(uint*)(agent + 0x7C + 0x38)),
            2 => AddEntry(args, *(uint*)(agent + 0x7C + 0x70)),
            _ => false,
        };
    }

    private unsafe void AddMenuItem(ContextMenuOpenedArgs args)
    {
        if (args.InventoryItemContext != null)
        {
            AddEntry(args, args.InventoryItemContext.Id);
        }
        else if (args.ParentAddonName != null)
        {
            PluginLog.Information(args.ParentAddonName);
            var _ = args.ParentAddonName switch
            {
                "ContentsInfoDetail" => AddEntry(args, "ContentsInfo",     0x1764),
                "RecipeNote"         => AddEntry(args, "RecipeNote",       0x398),
                "GatheringNote"      => AddEntry(args, "GatheringNote",    0x398),
                "ItemSearch"         => AddEntry(args, (IntPtr)args.Agent, 0x398),
                "ChatLog"            => AddEntry(args, "ChatLog",          0x940),
                _                    => false,
            };
        }
        else
        {
            HandleSatisfactionSupply(args);
        }
    }
}
