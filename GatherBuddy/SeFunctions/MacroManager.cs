using System;
using System.Runtime.InteropServices;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Client.UI.Shell;

namespace GatherBuddy.SeFunctions;

public unsafe class MacroManager : IDisposable
{
    public RaptureShellModule* Module
        => Framework.Instance()->GetUIModule()->GetRaptureShellModule();

    private RaptureMacroModule.Macro* _macro;

    public MacroManager()
    {
        _macro = (RaptureMacroModule.Macro*)Marshal.AllocHGlobal(sizeof(RaptureMacroModule.Macro));

        _macro->Name.Ctor();

        for (var i = 0; i < _macro->Lines.Length; ++i)
            _macro->Lines[i].Ctor();

        _macro->Lines[0].SetString(GatherBuddy.FullIdentify);
        _macro->Lines[1].SetString(GatherBuddy.FullMapMarker);
        _macro->Lines[2].SetString(GatherBuddy.FullTeleport);
        _macro->Lines[3].SetString(GatherBuddy.FullAdditionalInfo);
        _macro->Lines[4].SetString(GatherBuddy.FullGearChange);
        _macro->Lines[5].SetString(GatherBuddy.FullSetWaymarks);
    }

    public void Dispose()
    {
        if (_macro == null)
            return;

        _macro->Name.Dtor(false);

        for (var i = 0; i < _macro->Lines.Length; ++i)
            _macro->Lines[i].Dtor(false);

        Marshal.FreeHGlobal((IntPtr)_macro);
        _macro = null;
    }

    public void Execute()
        => Module->ExecuteMacro(_macro);
}
