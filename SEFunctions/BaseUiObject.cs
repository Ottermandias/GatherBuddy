using Dalamud.Game;
using Serilog;
using System;

namespace AutoVisor.SEFunctions
{
    public class BaseUiObject
    {
        private readonly IntPtr _address;

        public BaseUiObject( SigScanner sig, string tag, Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Verbose )
        {
            _address = sig.GetStaticAddressFromSig( "48 8B 0D ?? ?? ?? ?? 48 8D 54 24 ?? 48 83 C1 10 E8" );
            Log.Write( level,
                $"[{tag}] {GetType().Name} address 0x{_address.ToInt64():X16}, offset 0x{_address.ToInt64() - sig.Module.BaseAddress.ToInt64():X16}." );

            if( _address == IntPtr.Zero )
                Log.Error( $"[{tag}] No Pointer for {GetType().Name}." );
        }

        public IntPtr Get()
        {
            if( _address != IntPtr.Zero )
                return _address;

            Log.Error( $"Trying to use {GetType().Name}, but no pointer available." );
            return IntPtr.Zero;
        }
    }
}
