using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using ECommons.Automation;
using ECommons.DalamudServices;
using ECommons.UIHelpers;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.Enums;

namespace GatherBuddy.AutoGather.AtkReaders;

public unsafe class GatheringReader(AtkUnitBase* addon) : AtkReader(addon)
{
    private uint GatherChancesRaw1
        => ReadUInt(1).GetValueOrDefault();

    private uint GatherChancesRaw2
        => ReadUInt(2).GetValueOrDefault();

    private uint GatherChances
        => (GatherChancesRaw1 != 0 && GatherChancesRaw1 != 0xFFFFFFFF ? BinaryPrimitives.ReverseEndianness(GatherChancesRaw1) : BinaryPrimitives.ReverseEndianness(GatherChancesRaw2));

    private uint ItemLevelRaw1
        => ReadUInt(3).GetValueOrDefault();

    private uint ItemLevelRaw2
        => ReadUInt(4).GetValueOrDefault();

    private uint ItemLevel
        => (ItemLevelRaw1 != 0 && ItemLevelRaw1 != 0xFFFFFFFF ? BinaryPrimitives.ReverseEndianness(ItemLevelRaw1) : BinaryPrimitives.ReverseEndianness(ItemLevelRaw2));

    private List<ItemSlotReader> ItemSlotReaders
        => Loop<ItemSlotReader>(5, 11, 8);

    public List<ItemSlot> ItemSlots
    {
        get
        {
            var result = new List<ItemSlot>();
            for (var i = 0; i < 8; ++i)
            {
                var slot = ItemSlotReaders[i];
                Svc.Log.Debug($"GatheringReader: Slot {i} - Item: {slot.Item?.Name.English ?? "None"} - HasBonus: {slot.HasBonus} - RequiresPerception: {slot.RequiresPerception} - HasGivingLandBuff: {slot.HasGivingLandBuff} - IsCollectable: {slot.IsCollectable} - Yield: {slot.Yield} - BoonChance: {slot.BoonChance}");
                result.Add(new ItemSlot(i, slot, ItemSlotFlags, GatherChances, ItemLevel));
            }

            return result;
        }
    }

    private uint ItemSlotFlags
        => ReadUInt(98).GetValueOrDefault();

    public bool QuickGatheringAllowed
        => ReadBool(105).GetValueOrDefault();

    public bool QuickGatheringEnabled
        => ReadBool(106).GetValueOrDefault();

    public bool QuickGatheringInProgress
        => ReadBool(107).GetValueOrDefault();

    private uint LastSelectedSlot
        => ReadUInt(108).GetValueOrDefault();

    public int IntegrityRemaining
        => (int)ReadUInt(109).GetValueOrDefault();

    public int IntegrityMax
        => (int)ReadUInt(110).GetValueOrDefault();

    public bool Touched
        => IntegrityRemaining != IntegrityMax;

    public bool HiddenRevealed
        => ItemSlots.Any(i => i.IsHidden);
}
