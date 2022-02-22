using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Dalamud.Game.Text.SeStringHandling;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.System.String;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Client.UI.Shell;

namespace GatherBuddy.SeFunctions;

public unsafe class MacroManager : IDisposable
{
    public const int DefaultLineSize  = 256;
    public const int NumMacroLines    = 15;
    public const int NumRequiredLines = 5;

    public RaptureShellModule* Module
        => Framework.Instance()->GetUiModule()->GetRaptureShellModule();

    public RaptureMacroModule.Macro* Macro;

    public MacroManager()
    {
        Macro = (RaptureMacroModule.Macro*)Marshal.AllocHGlobal(sizeof(RaptureMacroModule.Macro));
        PrepareMacro(Macro);
        PrepareDefault();
    }

    public void Dispose()
    {
        DisposeMacro(Macro);
        Marshal.FreeHGlobal((IntPtr)Macro);
    }

    public static void ClearString(Utf8String* ret)
    {
        ret->BufUsed      = 1;
        ret->IsEmpty      = 1;
        ret->StringLength = 0;
        ret->StringPtr[0] = 0;
    }

    public static void CreateEmptyString(Utf8String* ret)
    {
        ret->BufSize             = 0x40;
        ret->IsUsingInlineBuffer = 1;
        ret->StringPtr           = ret->InlineBuffer;
        ClearString(ret);
    }

    public static void CreateTempString(Utf8String* ret)
    {
        ret->BufSize             = DefaultLineSize;
        ret->IsUsingInlineBuffer = 0;
        ret->StringPtr           = (byte*)Marshal.AllocHGlobal(DefaultLineSize);
        ClearString(ret);
    }

    public static void DisposeString(Utf8String* ret)
    {
        if (ret->BufSize == DefaultLineSize)
            Marshal.FreeHGlobal((IntPtr)ret->StringPtr);
        CreateEmptyString(ret);
    }

    private static bool CopyBytes(byte[] bytes, Utf8String* ret)
    {
        if (bytes.Length + 1 >= ret->BufSize)
            return false;

        Marshal.Copy(bytes, 0, (IntPtr)ret->StringPtr, bytes.Length);
        ret->BufUsed                 = bytes.Length + 1;
        ret->StringLength            = bytes.Length;
        ret->StringPtr[bytes.Length] = 0;
        return true;
    }

    public static bool CopyString(string text, Utf8String* ret)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        return CopyBytes(bytes, ret);
    }

    public static bool CopyString(SeString text, Utf8String* ret)
    {
        var bytes = text.Encode();
        return CopyBytes(bytes, ret);
    }

    public static void PrepareMacro(RaptureMacroModule.Macro* macro)
    {
        CreateEmptyString(&macro->Name);
        for (var i = 0; i < NumRequiredLines; ++i)
            CreateTempString(macro->Line[i]);
        for (var i = NumRequiredLines; i < NumMacroLines; ++i)
            CreateEmptyString(macro->Line[i]);
    }

    public static void DisposeMacro(RaptureMacroModule.Macro* macro)
    {
        for (var i = 0; i < NumRequiredLines; ++i)
            DisposeString(macro->Line[i]);
    }

    public bool ExecuteMacroLines(IList<SeString> lines)
    {
        Debug.Assert(lines.Count <= NumRequiredLines);
        for (var i = 0; i < lines.Count; ++i)
        {
            if (!CopyString(lines[i], Macro->Line[i]))
                return false;
        }

        for (var i = lines.Count; i < NumRequiredLines; ++i)
            ClearString(Macro->Line[i]);

        Module->ExecuteMacro(Macro);
        return true;
    }

    public bool ExecuteMacroLines(params SeString[] lines)
        => ExecuteMacroLines((IList<SeString>)lines);

    public void PrepareDefault()
    {
        CopyString(GatherBuddy.FullIdentify,       Macro->Line[0]);
        CopyString(GatherBuddy.FullMapMarker,      Macro->Line[1]);
        CopyString(GatherBuddy.FullGearChange,     Macro->Line[2]);
        CopyString(GatherBuddy.FullTeleport,       Macro->Line[3]);
        CopyString(GatherBuddy.FullAdditionalInfo, Macro->Line[4]);
    }

    public void Execute()
        => Module->ExecuteMacro((RaptureMacroModule.Macro*)(byte*)Macro);
}
