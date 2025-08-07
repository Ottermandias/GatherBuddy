using System;
using Dalamud.Game.Text.SeStringHandling;
using ECommons.UIHelpers;
using GatherBuddy.Classes;

namespace GatherBuddy.AutoGather.AtkReaders;

public class ItemSlotReader(IntPtr addon, int beginOffset = 0) : AtkReader(addon, beginOffset)
{
    //public  bool        Enabled                => ReadBool(0).GetValueOrDefault();
    private uint        ItemId                 => ReadUInt(1).GetValueOrDefault();
    public  Gatherable? Item                   => ItemId > 0 ? GatherBuddy.GameData.Gatherables[ItemId] : null;
    private uint        FlagsRaw               => ReadUInt(5).GetValueOrDefault();
    public  bool        HasBonus               => (FlagsRaw & 4) != 0;
    public  bool        RequiresPerception     => (FlagsRaw & 1) != 0;
    private SeString    RequiresPerceptionText => ReadSeString(6);
    private uint        BuffsValuesRaw         => ReadUInt(7).GetValueOrDefault();

    public sbyte Yield
        => (sbyte)(BuffsValuesRaw & 0xff);

    public sbyte BoonChance => (sbyte)((BuffsValuesRaw >> 8) & 0xff);

    public bool HasGivingLandBuff => ReadBool(9).GetValueOrDefault();

    private uint CollectableRaw
        => ReadUInt(10).GetValueOrDefault();

    public bool IsCollectable => CollectableRaw == 2;
}
