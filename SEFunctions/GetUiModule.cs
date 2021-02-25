using System;
using System.Runtime.InteropServices;
using Dalamud.Game;
using Dalamud.Hooking;
using Serilog;

namespace AutoVisor.SEFunctions
{
    public class GetUiModule
    {
        private IntPtr _address;

        public delegate IntPtr FuncDelegate( IntPtr baseUiObj );

        private readonly FuncDelegate _funcDelegate;

        public GetUiModule( SigScanner sig, string tag, Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Verbose )
        {
            _address = sig.ScanText( "E8 ?? ?? ?? ?? 48 83 7F ?? 00 48 8B F0" );
            Log.Write( level,
                $"[{tag}] {GetType().Name} address 0x{_address.ToInt64():X16}, offset 0x{_address.ToInt64() - sig.Module.BaseAddress.ToInt64():X16}." );

            if( _address != IntPtr.Zero )
                _funcDelegate = Marshal.GetDelegateForFunctionPointer< FuncDelegate >( _address );
            else
                Log.Error( $"[{tag}] No Pointer for {GetType().Name}." );
        }

        public IntPtr Invoke( IntPtr baseUiObj )
        {
            if( _funcDelegate != null )
                return _funcDelegate.Invoke( baseUiObj );

            Log.Error( $"Trying to call {GetType().Name}, but no pointer available." );
            return IntPtr.Zero;
        }

        public Hook< FuncDelegate > CreateHook( FuncDelegate detour, object callback, string tag,
            Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Verbose )
        {
            if( _address != IntPtr.Zero )
            {
                var hook = new Hook< FuncDelegate >( _address, detour, callback );
                hook.Enable();
                Log.Write( level, $"[{tag}] Hooked onto {GetType().Name} on address 0x{_address.ToInt64():X16}." );
                return hook;
            }

            Log.Error( $"[{tag}] Trying to create Hook for {GetType().Name}, but no pointer available." );
            return null;
        }
    }
}
