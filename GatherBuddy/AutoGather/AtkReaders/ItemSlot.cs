using ECommons;
using ECommons.Automation;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.Classes;

namespace GatherBuddy.AutoGather.AtkReaders;

public class ItemSlot(int index, ItemSlotReader reader, uint itemSlotFlags, uint gatherChances, uint itemLevels)
{
    public unsafe void Gather()
    {
        if (GenericHelpers.TryGetAddonByName("Gathering", out AtkUnitBase* addon))
        {
            Callback.Fire(addon, true, index, 0);
        }
    }
    //public bool Enabled => reader.Enabled;
    public bool HasBonus => reader.HasBonus;
    public bool RequiresPerception => reader.RequiresPerception;
    public bool HasGivingLandBuff => reader.HasGivingLandBuff;
    public bool IsCollectable => reader.IsCollectable;
    public sbyte Yield => reader.Yield;
    public sbyte BoonChance => reader.BoonChance;
    public bool        IsEmpty => reader.Item == null;
    public Gatherable? Item    => reader.Item;

    public int GatherChance
        => (sbyte)((gatherChances >> (index * 8)) & 0xFF);

    public int ItemLevel
        => (sbyte)((itemLevels >> (index * 8)) & 0xFF);

    private const uint RareFlagMask = 1u << 16;

    public bool IsRare
    {
        get
        {
            uint mask = RareFlagMask << index;
            return (itemSlotFlags & mask) != 0;
        }
    }

    public bool IsHidden => (itemSlotFlags & (1u << (index))) != 0;
}
