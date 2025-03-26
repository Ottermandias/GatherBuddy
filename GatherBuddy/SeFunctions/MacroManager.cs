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
        => Framework.Instance()->GetUIModule()->GetRaptureShellModule();

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
        ret.IsEmpty      = true;
        ret.StringLength = 0;
        ret.StringPtr.Value[0] = 0;
    }

    public static void CreateEmptyString(ref Utf8String ret)
    {
        ret.BufSize             = 0x40;
        ret.IsUsingInlineBuffer = true;
        fixed (byte* ptr = ret.InlineBuffer)
        {
            ret.StringPtr = ptr;
        }

        ClearString(ref ret);
    }

    public static void CreateTempString(ref Utf8String ret)
    {
        ret.BufSize             = DefaultLineSize;
        ret.IsUsingInlineBuffer = false;
        ret.StringPtr           = (byte*)Marshal.AllocHGlobal(DefaultLineSize);
        ClearString(ref ret);
    }

    public static void DisposeString(ref Utf8String ret)
    {
        if (ret.BufSize == DefaultLineSize)
            Marshal.FreeHGlobal((nint)ret.StringPtr.Value);
        CreateEmptyString(ref ret);
    }

    private static bool CopyBytes(byte[] bytes, ref Utf8String ret)
    {
        if (bytes.Length + 1 >= ret.BufSize)
            return false;

        Marshal.Copy(bytes, 0, (nint)ret.StringPtr.Value, bytes.Length);
        ret.BufUsed                 = bytes.Length + 1;
        ret.StringLength            = bytes.Length;
        ret.StringPtr.Value[bytes.Length] = 0;
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
            CreateTempString(ref macro->Lines[i]);
        for (var i = NumRequiredLines; i < NumMacroLines; ++i)
            CreateEmptyString(ref macro->Lines[i]);
    }

    public static void DisposeMacro(RaptureMacroModule.Macro* macro)
    {
        for (var i = 0; i < NumRequiredLines; ++i)
            DisposeString(ref macro->Lines[i]);
    }

    public bool ExecuteMacroLines(IList<SeString> lines)
    {
        Debug.Assert(lines.Count <= NumRequiredLines);
        for (var i = 0; i < lines.Count; ++i)
        {
            if (!CopyString(lines[i], ref Macro->Lines[i]))
                return false;
        }

        for (var i = lines.Count; i < NumRequiredLines; ++i)
            ClearString(ref Macro->Lines[i]);

        Module->ExecuteMacro(Macro);
        return true;
    }

    public bool ExecuteMacroLines(params SeString[] lines)
        => ExecuteMacroLines((IList<SeString>)lines);

    public void PrepareDefault()
    {
        CopyString(GatherBuddy.FullIdentify,       ref Macro->Lines[0]);
        CopyString(GatherBuddy.FullMapMarker,      ref Macro->Lines[1]);
        CopyString(GatherBuddy.FullTeleport,       ref Macro->Lines[2]);
        CopyString(GatherBuddy.FullAdditionalInfo, ref Macro->Lines[3]);
        CopyString(GatherBuddy.FullGearChange,     ref Macro->Lines[4]);
        CopyString(GatherBuddy.FullSetWaymarks,    ref Macro->Lines[5]);
    }

    public void Execute()
        => Module->ExecuteMacro((RaptureMacroModule.Macro*)(byte*)Macro);
}
