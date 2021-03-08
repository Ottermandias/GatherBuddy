using System;
using System.Runtime.InteropServices;
using Dalamud.Game;
using Dalamud.Hooking;
using Serilog;

namespace Otter.SEFunctions
{
    public enum Sounds : byte
    {
        None = 0x00,
        Unknown = 0x01,
        Sound01 = 0x25,
        Sound02 = 0x26,
        Sound03 = 0x27,
        Sound04 = 0x28,
        Sound05 = 0x29,
        Sound06 = 0x2A,
        Sound07 = 0x2B,
        Sound08 = 0x2C,
        Sound09 = 0x2D,
        Sound10 = 0x2E,
        Sound11 = 0x2F,
        Sound12 = 0x30,
        Sound13 = 0x31,
        Sound14 = 0x32,
        Sound15 = 0x33,
        Sound16 = 0x34,
    }

    public static class SoundsExtensions
    {
        public static int ToIdx(this Sounds value)
        {
            return value switch
            {
                Sounds.None => 0,
                Sounds.Sound01 => 1,
                Sounds.Sound02 => 2,
                Sounds.Sound03 => 3,
                Sounds.Sound04 => 4,
                Sounds.Sound05 => 5,
                Sounds.Sound06 => 6,
                Sounds.Sound07 => 7,
                Sounds.Sound08 => 8,
                Sounds.Sound09 => 9,
                Sounds.Sound10 => 10,
                Sounds.Sound11 => 11,
                Sounds.Sound12 => 12,
                Sounds.Sound13 => 13,
                Sounds.Sound14 => 14,
                Sounds.Sound15 => 15,
                Sounds.Sound16 => 16,
                _ => -1
            };
        }

        public static Sounds FromIdx(int idx)
        {
            return idx switch
            {
                0  => Sounds.None,
                1  => Sounds.Sound01,
                2  => Sounds.Sound02,
                3  => Sounds.Sound03,
                4  => Sounds.Sound04,
                5  => Sounds.Sound05,
                6  => Sounds.Sound06,
                7  => Sounds.Sound07,
                8  => Sounds.Sound08,
                9  => Sounds.Sound09,
                10 => Sounds.Sound10,
                11 => Sounds.Sound11,
                12 => Sounds.Sound12,
                13 => Sounds.Sound13,
                14 => Sounds.Sound14,
                15 => Sounds.Sound15,
                16 => Sounds.Sound16,
                _  => Sounds.Unknown,
            };
        }
    }

    public class PlaySound
    {
        private IntPtr _address;

        public delegate ulong FuncDelegate(int id, ulong unknown1, ulong unknown2);

        private readonly FuncDelegate _funcDelegate;

        public PlaySound( SigScanner sig, string tag, Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Verbose )
        {
            _address = sig.ScanText( "E8 ?? ?? ?? ?? 4D 39 BE" );
            Log.Write( level,
                $"[{tag}] {GetType().Name} address 0x{_address.ToInt64():X16}, offset 0x{_address.ToInt64() - sig.Module.BaseAddress.ToInt64():X16}." );

            if( _address != IntPtr.Zero )
                _funcDelegate = Marshal.GetDelegateForFunctionPointer< FuncDelegate >( _address );
            else
                Log.Error( $"[{tag}] No Pointer for {GetType().Name}." );
        }

        public ulong Invoke( Sounds id )
        {
            if( _funcDelegate != null )
                return _funcDelegate.Invoke( (int)id, 0, 0 );

            Log.Error( $"Trying to call {GetType().Name}, but no pointer available." );
            return 0;
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