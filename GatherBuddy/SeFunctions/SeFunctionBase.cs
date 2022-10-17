using System;
using System.Runtime.InteropServices;
using Dalamud.Game;
using Dalamud.Hooking;

namespace GatherBuddy.SeFunctions;

public class SeFunctionBase<T> where T : Delegate
{
    public    IntPtr Address;
    protected T?     FuncDelegate;

    public SeFunctionBase(SigScanner sigScanner, int offset)
    {
        Address = sigScanner.Module.BaseAddress + offset;
        GatherBuddy.Log.Debug($"{GetType().Name} address 0x{Address.ToInt64():X16}, baseOffset 0x{offset:X16}.");
    }

    public SeFunctionBase(SigScanner sigScanner, string signature, int offset = 0)
    {
        Address = sigScanner.ScanText(signature);
        if (Address != IntPtr.Zero)
            Address += offset;
        var baseOffset = (ulong)Address.ToInt64() - (ulong)sigScanner.Module.BaseAddress.ToInt64();
        GatherBuddy.Log.Debug($"{GetType().Name} address 0x{Address.ToInt64():X16}, baseOffset 0x{baseOffset:X16}.");
    }

    public T? Delegate()
    {
        if (FuncDelegate != null)
            return FuncDelegate;

        if (Address != IntPtr.Zero)
        {
            FuncDelegate = Marshal.GetDelegateForFunctionPointer<T>(Address);
            return FuncDelegate;
        }

        GatherBuddy.Log.Error($"Trying to generate delegate for {GetType().Name}, but no pointer available.");
        return null;
    }

    public dynamic? Invoke(params dynamic[] parameters)
    {
        if (FuncDelegate != null)
            return FuncDelegate.DynamicInvoke(parameters);

        if (Address != IntPtr.Zero)
        {
            FuncDelegate = Marshal.GetDelegateForFunctionPointer<T>(Address);
            return FuncDelegate!.DynamicInvoke(parameters);
        }
        else
        {
            GatherBuddy.Log.Error($"Trying to call {GetType().Name}, but no pointer available.");
            return null;
        }
    }

    public Hook<T>? CreateHook(T detour)
    {
        if (Address != IntPtr.Zero)
        {
            var hook = Hook<T>.FromAddress(Address, detour);
            hook.Enable();
            GatherBuddy.Log.Debug($"Hooked onto {GetType().Name} at address 0x{Address.ToInt64():X16}.");
            return hook;
        }

        GatherBuddy.Log.Error($"Trying to create Hook for {GetType().Name}, but no pointer available.");
        return null;
    }
}
