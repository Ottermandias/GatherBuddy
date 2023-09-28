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
    public const int NumRequiredLines = 6;

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

    public static void ClearString(ref Utf8String ret)
    {
        ret.BufUsed      = 1;
        ret.IsEmpty      = 1;
        ret.StringLength = 0;
        ret.StringPtr[0] = 0;
    }

    public static void CreateEmptyString(ref Utf8String ret)
    {
        ret.BufSize             = 0x40;
        ret.IsUsingInlineBuffer = 1;
        fixed (byte* ptr = ret.InlineBuffer)
        {
            ret.StringPtr = ptr;
        }

        ClearString(ref ret);
    }

    public static void CreateTempString(ref Utf8String ret)
    {
        ret.BufSize             = DefaultLineSize;
        ret.IsUsingInlineBuffer = 0;
        ret.StringPtr           = (byte*)Marshal.AllocHGlobal(DefaultLineSize);
        ClearString(ref ret);
    }

    public static void DisposeString(ref Utf8String ret)
    {
        if (ret.BufSize == DefaultLineSize)
            Marshal.FreeHGlobal((nint)ret.StringPtr);
        CreateEmptyString(ref ret);
    }

    private static bool CopyBytes(byte[] bytes, ref Utf8String ret)
    {
        if (bytes.Length + 1 >= ret.BufSize)
            return false;

        Marshal.Copy(bytes, 0, (nint)ret.StringPtr, bytes.Length);
        ret.BufUsed                 = bytes.Length + 1;
        ret.StringLength            = bytes.Length;
        ret.StringPtr[bytes.Length] = 0;
        return true;
    }

    public static bool CopyString(string text, ref Utf8String ret)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        return CopyBytes(bytes, ref ret);
    }

    public static bool CopyString(SeString text, ref Utf8String ret)
    {
        var bytes = text.Encode();
        return CopyBytes(bytes, ref ret);
    }

    public static void PrepareMacro(RaptureMacroModule.Macro* macro)
    {
        CreateEmptyString(ref macro->Name);
        for (var i = 0; i < NumRequiredLines; ++i)
            CreateTempString(ref macro->LinesSpan[i]);
        for (var i = NumRequiredLines; i < NumMacroLines; ++i)
            CreateEmptyString(ref macro->LinesSpan[i]);
    }

    public static void DisposeMacro(RaptureMacroModule.Macro* macro)
    {
        for (var i = 0; i < NumRequiredLines; ++i)
            DisposeString(ref macro->LinesSpan[i]);
    }

    public bool ExecuteMacroLines(IList<SeString> lines)
    {
        Debug.Assert(lines.Count <= NumRequiredLines);
        for (var i = 0; i < lines.Count; ++i)
        {
            if (!CopyString(lines[i], ref Macro->LinesSpan[i]))
                return false;
        }

        for (var i = lines.Count; i < NumRequiredLines; ++i)
            ClearString(ref Macro->LinesSpan[i]);

        Module->ExecuteMacro(Macro);
        return true;
    }

    public bool ExecuteMacroLines(params SeString[] lines)
        => ExecuteMacroLines((IList<SeString>)lines);

    public void PrepareDefault()
    {
        CopyString(GatherBuddy.FullIdentify,       ref Macro->LinesSpan[0]);
        CopyString(GatherBuddy.FullMapMarker,      ref Macro->LinesSpan[1]);
        CopyString(GatherBuddy.FullTeleport,       ref Macro->LinesSpan[2]);
        CopyString(GatherBuddy.FullAdditionalInfo, ref Macro->LinesSpan[3]);
        CopyString(GatherBuddy.FullGearChange,     ref Macro->LinesSpan[4]);
        CopyString(GatherBuddy.FullSetWaymarks,    ref Macro->LinesSpan[5]);
    }

    public void Execute()
        => Module->ExecuteMacro((RaptureMacroModule.Macro*)(byte*)Macro);
}
