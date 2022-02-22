using System;
using System.Runtime.InteropServices;
using System.Text;
using Dalamud.Game;
using Dalamud.Game.Gui;
using Dalamud.Logging;

namespace GatherBuddy.SeFunctions;

public class CommandManager
{
    private readonly ProcessChatBox _processChatBox;
    private readonly IntPtr         _uiModulePtr;

    public CommandManager(GameGui gameGui, ProcessChatBox processChatBox)
    {
        _processChatBox = processChatBox;
        _uiModulePtr    = gameGui.GetUIModule();
    }

    public CommandManager(GameGui gameGui, SigScanner sigScanner)
        : this(gameGui, new ProcessChatBox(sigScanner))
    { }

    public bool Execute(string message)
    {
        // First try to process the command through Dalamud.
        if (Dalamud.Commands.ProcessCommand(message))
        {
            PluginLog.Verbose("Executed Dalamud command \"{Message:l}\".", message);
            return true;
        }

        if (_uiModulePtr == IntPtr.Zero)
        {
            PluginLog.Error("Can not execute \"{Message:l}\" because no uiModulePtr is available.", message);
            return false;
        }

        // Then prepare a string to send to the game itself.
        var (text, length) = PrepareString(message);
        var payload = PrepareContainer(text, length);

        _processChatBox.Invoke(_uiModulePtr, payload, IntPtr.Zero, (byte)0);

        Marshal.FreeHGlobal(payload);
        Marshal.FreeHGlobal(text);
        return false;
    }

    private static (IntPtr, long) PrepareString(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var mem   = Marshal.AllocHGlobal(bytes.Length + 30);
        Marshal.Copy(bytes, 0, mem, bytes.Length);
        Marshal.WriteByte(mem + bytes.Length, 0);
        return (mem, bytes.Length + 1);
    }

    private static IntPtr PrepareContainer(IntPtr message, long length)
    {
        var mem = Marshal.AllocHGlobal(400);
        Marshal.WriteInt64(mem,        message.ToInt64());
        Marshal.WriteInt64(mem + 0x8,  64);
        Marshal.WriteInt64(mem + 0x10, length);
        Marshal.WriteInt64(mem + 0x18, 0);
        return mem;
    }
}
