using ECommons.UIHelpers;
using ECommons.UIHelpers.AddonMasterImplementations;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.Classes;

namespace GatherBuddy.AutoGather.AtkReaders;

// NOTE: Rewritten to use AtkReader (value-based) so it remains valid when the window is recreated by Revisit. 

public unsafe class GatheringMasterpieceReader(AtkUnitBase* addon) : AtkReader(addon)
{
    private uint ItemId => ReadUInt(2).GetValueOrDefault();
    public  Gatherable Item => GatherBuddy.GameData.Gatherables[ItemId];
    public bool HighVisible
        => addon->GetNodeById(15)->IsVisible();
    public bool MidVisible => addon->GetNodeById(14)->IsVisible();
    public bool LowVisible => addon->GetNodeById(13)->IsVisible();
    public int  CollectabilityCurrent => (int)(ReadUInt(13) ?? 0);
    public int  CollectabilityMax     => (int)(ReadUInt(14) ?? 0);

    public int IntegrityCurrent => (int)(ReadUInt(62) ?? 0u);
    public int IntegrityMax     => (int)(ReadUInt(63) ?? 0u);

    public int ScourGain      => (int)(ReadUInt(48) ?? 0);
    public int BrazenGainMin  => (int)(ReadUInt(49) ?? 0);
    public int BrazenGainMax  => (int)(ReadUInt(50) ?? 0);
    public int MeticulousGain => (int)(ReadUInt(51) ?? 0);

    public int LowThreshold  => (int)(ReadUInt(65) ?? 0u);
    public int MidThreshold  => (int)(ReadUInt(66) ?? 0u);
    public int HighThreshold => (int)(ReadUInt(67) ?? 0u);

    public bool IsValid => !IsNull;
}

