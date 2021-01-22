using Dalamud.Game;
using Dalamud.Hooking;
using System;
using Serilog;
using System.Runtime.InteropServices;

namespace Otter
{
    public class ProcessChatBox
    {
        public delegate IntPtr FuncDelegate(IntPtr raptureModule, IntPtr message, IntPtr uiModule);
        private readonly IntPtr address;
        private readonly FuncDelegate funcDelegate = null;

        public ProcessChatBox(SigScanner sig, string tag, Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Verbose)
        {
            address = sig.ScanText("40 53 56 57 48 83 EC 70 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 44 24 ?? 48 8B 02");
            Log.Write(level, $"[{tag}] {GetType().Name} address 0x{address.ToInt64():X16}, offset 0x{address.ToInt64() - sig.Module.BaseAddress.ToInt64():X16}.");

            if (address != IntPtr.Zero)
                funcDelegate = Marshal.GetDelegateForFunctionPointer<FuncDelegate>(address);
            else
                Log.Error($"[{tag}] No Pointer for {GetType().Name}.");
        }

        public IntPtr Invoke(IntPtr raptureModule, IntPtr message, IntPtr uiModule)
        {
            if (funcDelegate != null)
                return funcDelegate.Invoke(raptureModule, message, uiModule);
            Log.Error($"Trying to call {GetType().Name}, but no pointer available.");
            return IntPtr.Zero;
        }

        public Hook<FuncDelegate> CreateHook(FuncDelegate detour, object callback, string tag, Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Verbose)
        {
            if (address != IntPtr.Zero)
            {
                var hook = new Hook<FuncDelegate>(address, detour, callback);
                hook?.Enable();
                Log.Write(level, $"[{tag}] Hooked onto {GetType().Name} on address 0x{address.ToInt64():X16}.");
                return hook;
            }
            Log.Error($"[{tag}] Trying to create Hook for {GetType().Name}, but no pointer available.");
            return null;
        }
    }
}
