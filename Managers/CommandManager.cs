using Dalamud.Hooking;
using System;
using Serilog;
using System.Runtime.InteropServices;
using System.Text;
using Dalamud.Plugin;

namespace Otter
{
    public class CommandManager
    {
        public CommandManager(DalamudPluginInterface pi, ProcessChatBox processChatBox, string tag, Serilog.Events.LogEventLevel level)
        {
            this.level           = level;
            this.tag             = tag;
            this.dalamudCommands = pi.CommandManager;
            this.processChatBox  = processChatBox;
            // Create an initial hook to obtain the necessary pointers.
            this.initialProcessChatBoxHook = processChatBox.CreateHook(new ProcessChatBox.FuncDelegate(ProcessChatBoxDetour), this, tag, Serilog.Events.LogEventLevel.Verbose);   
        }

        public CommandManager(DalamudPluginInterface pi, string tag, Serilog.Events.LogEventLevel level) 
            : this(pi, new ProcessChatBox(pi.TargetModuleScanner, tag, level), tag, level)
        { }

        ~CommandManager()
        {
            RemoveHook();
        }

        public void RemoveHook()
        {
            if (initialProcessChatBoxHook != null)
            {
                Log.Write(level, $"[{tag}] Disabled hook on {GetType().Name}.");
                initialProcessChatBoxHook.Disable();
                initialProcessChatBoxHook.Dispose();
                initialProcessChatBoxHook = null;
            }
        }

        public bool Execute(string message)
        {
            // First try to process the command through Dalamud.
            if (dalamudCommands.ProcessCommand(message))
            {
                Log.Verbose($"[{tag}] Executed Dalamud command \"{message}\".");
                return true;
            }

            if (uiModule == IntPtr.Zero || raptureModule == IntPtr.Zero)
            {
                Log.Error($"[{tag}] Can not execute \"{message}\" because no uiModule or raptureModule are available.");
                return false;
            }
            
            // Then prepare a string to send to the game itself.
            (IntPtr text, Int64 length) = PrepareString(message);
            IntPtr payload = PrepareContainer(text, length);

            const int newPayloadOffset = 675757;
            const int unknownOffset = 169921;

            Marshal.WriteByte (uiModule + newPayloadOffset, 1);
            Marshal.WriteInt16(uiModule +    unknownOffset, 0);

            processChatBox.Invoke(raptureModule, payload, uiModule);

            Marshal.WriteByte(uiModule + newPayloadOffset, 0);

            Marshal.FreeHGlobal(payload);
            Marshal.FreeHGlobal(text);
            return false;
        }

        #region privates
        // The detour that stores the pointers on first chat handling, then removes the hook itself.
        private IntPtr ProcessChatBoxDetour(IntPtr raptureModule, IntPtr message, IntPtr uiModule)
        {
            if (this.uiModule == IntPtr.Zero)
            {
                this.uiModule      = uiModule;
                this.raptureModule = raptureModule;
                Log.Write(level, $"[{tag}] UIModule address 0x{uiModule.ToInt64():X16}.");
                Log.Write(level, $"[{tag}] RaptureModule address 0x{raptureModule.ToInt64():X16}.");
                var ret = initialProcessChatBoxHook.Original(raptureModule, message, uiModule);
                RemoveHook();
                return ret;
            }
            // Should not be reached.
            return initialProcessChatBoxHook.Original(raptureModule, message, uiModule);
        }
        private (IntPtr, Int64) PrepareString(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            IntPtr mem = Marshal.AllocHGlobal(bytes.Length + 30);
            Marshal.Copy(bytes, 0, mem, bytes.Length);
            Marshal.WriteByte(mem + bytes.Length, 0);
            return (mem, bytes.Length + 1);
        }
        private IntPtr PrepareContainer(IntPtr message, Int64 length)
        {
            IntPtr mem = Marshal.AllocHGlobal(400);
            Marshal.WriteInt64(mem, message.ToInt64());
            Marshal.WriteInt64(mem + 0x8, 64);
            Marshal.WriteInt64(mem + 0x10, length);
            Marshal.WriteInt64(mem + 0x18, 0);
            return mem;
        }

        private readonly string                              tag;
        private readonly Serilog.Events.LogEventLevel        level;
        private readonly ProcessChatBox                      processChatBox;
        private readonly Dalamud.Game.Command.CommandManager dalamudCommands;
        private Hook<ProcessChatBox.FuncDelegate>            initialProcessChatBoxHook;

        private IntPtr uiModule      = IntPtr.Zero;
        private IntPtr raptureModule = IntPtr.Zero;
        #endregion
    }
}
