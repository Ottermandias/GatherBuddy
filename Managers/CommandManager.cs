using System;
using System.Runtime.InteropServices;
using System.Text;
using Dalamud.Plugin;
using Otter.SEFunctions;
using Serilog;

namespace GatherBuddyPlugin.Managers
{
    public class CommandManager
    {
        public CommandManager( DalamudPluginInterface pi, BaseUiObject uiModule, GetUiModule getUiModule, ProcessChatBox processChatBox,
            string tag, Serilog.Events.LogEventLevel level )
        {
            _level           = level;
            _tag             = tag;
            _dalamudCommands = pi.CommandManager;
            _processChatBox  = processChatBox;
            _uiModulePtr     = getUiModule.Invoke( Marshal.ReadIntPtr( uiModule.Get() ) );
        }

        public CommandManager( DalamudPluginInterface pi, string tag, Serilog.Events.LogEventLevel level )
            : this( pi
                , new BaseUiObject( pi.TargetModuleScanner, tag, level )
                , new GetUiModule( pi.TargetModuleScanner, tag, level )
                , new ProcessChatBox( pi.TargetModuleScanner, tag, level )
                , tag, level )
        { }

        public bool Execute( string message )
        {
            // First try to process the command through Dalamud.
            if( _dalamudCommands.ProcessCommand( message ) )
            {
                Log.Verbose( $"[{_tag}] Executed Dalamud command \"{message}\"." );
                return true;
            }

            if( _uiModulePtr == IntPtr.Zero )
            {
                Log.Error( $"[{_tag}] Can not execute \"{message}\" because no uiModulePtr is available." );
                return false;
            }

            // Then prepare a string to send to the game itself.
            var (text, length) = PrepareString( message );
            var payload = PrepareContainer( text, length );

            _processChatBox.Invoke( _uiModulePtr, payload, IntPtr.Zero, 0 );

            Marshal.FreeHGlobal( payload );
            Marshal.FreeHGlobal( text );
            return false;
        }

        #region privates

        private static (IntPtr, long) PrepareString( string message )
        {
            var bytes = Encoding.UTF8.GetBytes( message );
            var mem   = Marshal.AllocHGlobal( bytes.Length + 30 );
            Marshal.Copy( bytes, 0, mem, bytes.Length );
            Marshal.WriteByte( mem + bytes.Length, 0 );
            return ( mem, bytes.Length + 1 );
        }

        private static IntPtr PrepareContainer( IntPtr message, long length )
        {
            var mem = Marshal.AllocHGlobal( 400 );
            Marshal.WriteInt64( mem, message.ToInt64() );
            Marshal.WriteInt64( mem + 0x8, 64 );
            Marshal.WriteInt64( mem + 0x10, length );
            Marshal.WriteInt64( mem + 0x18, 0 );
            return mem;
        }

        private readonly string                              _tag;
        private readonly Serilog.Events.LogEventLevel        _level;
        private readonly ProcessChatBox                      _processChatBox;
        private readonly Dalamud.Game.Command.CommandManager _dalamudCommands;

        private readonly IntPtr _uiModulePtr;

        #endregion
    }
}
