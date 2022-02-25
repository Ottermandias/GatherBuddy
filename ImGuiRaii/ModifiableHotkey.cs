using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Keys;
using Newtonsoft.Json;

namespace ImGuiOtter;

public struct ModifiableHotkey : IEquatable<ModifiableHotkey>
{
    public VirtualKey     Hotkey    { get; private set; } = VirtualKey.NO_KEY;
    public ModifierHotkey Modifier1 { get; private set; } = VirtualKey.NO_KEY;
    public ModifierHotkey Modifier2 { get; private set; } = VirtualKey.NO_KEY;


    public ModifiableHotkey()
    { }

    public ModifiableHotkey(VirtualKey hotkey, VirtualKey[]? validKeys = null)
    {
        SetHotkey(hotkey);
    }

    public ModifiableHotkey(VirtualKey hotkey, ModifierHotkey modifier1, VirtualKey[]? validKeys = null)
    {
        SetHotkey(hotkey);
        SetModifier1(modifier1);
    }

    [JsonConstructor]
    public ModifiableHotkey(VirtualKey hotkey, ModifierHotkey modifier1, ModifierHotkey modifier2, VirtualKey[]? validKeys = null)
    {
        SetHotkey(hotkey);
        SetModifier1(modifier1);
        SetModifier2(modifier2);
    }

    public bool SetHotkey(VirtualKey key, IReadOnlyList<VirtualKey>? validKeys = null)
    {
        if (Hotkey == key || validKeys != null && !validKeys.Contains(key))
            return false;

        if (key == VirtualKey.NO_KEY)
        {
            Modifier1 = VirtualKey.NO_KEY;
            Modifier2 = VirtualKey.NO_KEY;
        }

        Hotkey = key;
        return true;
    }

    public bool SetModifier1(ModifierHotkey key)
    {
        if (Modifier1 == key)
            return false;

        if (key == VirtualKey.NO_KEY || key == Modifier2)
            Modifier2 = VirtualKey.NO_KEY;

        Modifier1 = key;
        return true;
    }

    public bool SetModifier2(ModifierHotkey key)
    {
        if (Modifier2 == key)
            return false;

        Modifier2 = Modifier1 == key ? VirtualKey.NO_KEY : key;
        return true;
    }

    public bool Equals(ModifiableHotkey other)
        => Hotkey == other.Hotkey
         && Modifier1 == other.Modifier1
         && Modifier2 == other.Modifier2;

    public override bool Equals(object? obj)
        => obj is ModifiableHotkey other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine((int)Hotkey, Modifier1, Modifier2);

    public static bool operator ==(ModifiableHotkey lhs, ModifiableHotkey rhs)
        => lhs.Equals(rhs);

    public static bool operator !=(ModifiableHotkey lhs, ModifiableHotkey rhs)
        => !lhs.Equals(rhs);
}
