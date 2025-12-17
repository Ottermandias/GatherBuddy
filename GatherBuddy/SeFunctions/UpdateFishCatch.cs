using System;
using Dalamud.Plugin.Services;

namespace GatherBuddy.SeFunctions;

public delegate void UpdateCatchDelegate(IntPtr module, uint fishId, bool large, ushort size, byte amount, byte level, byte unk7, byte unk8, byte unk9, byte unk10,
    byte unk11, byte unk12);

public sealed class UpdateFishCatch : SeFunctionBase<UpdateCatchDelegate>
{
    public UpdateFishCatch(ISigScanner sigScanner)
        : base(sigScanner, "48 89 6C 24 ?? 56 41 56 41 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 48 8B 01")
    {}
}
