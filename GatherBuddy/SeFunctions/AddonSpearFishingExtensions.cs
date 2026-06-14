using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.Enums;
using SpearfishSize = GatherBuddy.Enums.SpearfishSize;

namespace GatherBuddy.SeFunctions;

public unsafe static class AddonSpearFishingExtensions
{
    extension(ref AddonSpearFishing addon)
    {
        public AtkResNode* FishLines => addon.GetNodeById(43);
        public AtkResNode* Fish1Node => (AtkResNode*)addon.GetComponentNodeById(61);
        public AtkResNode* Fish2Node => (AtkResNode*)addon.GetComponentNodeById(60);
        public AtkResNode* Fish3Node => (AtkResNode*)addon.GetComponentNodeById(59);
    }

    extension(ref AddonSpearFishing.FishInfo info)
    {
        public SpearfishSpeed SpearfishSpeed => (SpearfishSpeed)info.Speed;
        public SpearfishSize SpearfishSize => (SpearfishSize)info.Size;
    }
}
