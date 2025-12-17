using System;
using Dalamud.Plugin.Services;

namespace GatherBuddy.SeFunctions;

public delegate IntPtr ProcessChatBoxDelegate(IntPtr uiModule, IntPtr message, IntPtr unk1, byte unk2);

public sealed class ProcessChatBox : SeFunctionBase<ProcessChatBoxDelegate>
{
    public ProcessChatBox(ISigScanner sigScanner)
        : base(sigScanner, "48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC ?? 48 8B F2 48 8B F9 45 84 C9")
    { }
}
