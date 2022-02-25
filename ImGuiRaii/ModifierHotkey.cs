using System;
using System.Linq;
using Dalamud.Game.ClientState.Keys;
using Newtonsoft.Json;

namespace ImGuiOtter;

public readonly struct ModifierHotkey : IEquatable<ModifierHotkey>
{
    public static readonly ModifierHotkey NoKey = new(VirtualKey.NO_KEY);

    public static readonly ModifierHotkey[] ValidModifiers =
    {
        VirtualKey.NO_KEY,
        VirtualKey.CONTROL,
        VirtualKey.MENU,
        VirtualKey.SHIFT,
    };

    public static readonly VirtualKey[] ValidKeys = ValidModifiers.Select(m => m.Modifier).ToArray();

    public readonly VirtualKey Modifier;

    public static implicit operator VirtualKey(ModifierHotkey k)
        => k.Modifier;

    public static implicit operator ModifierHotkey(VirtualKey k)
        => new(k);

    [JsonConstructor]
    public ModifierHotkey(VirtualKey modifier)
    {
        Modifier = modifier switch
        {
            VirtualKey.NO_KEY   => VirtualKey.NO_KEY,
            VirtualKey.CONTROL  => VirtualKey.CONTROL,
            VirtualKey.MENU     => VirtualKey.MENU,
            VirtualKey.SHIFT    => VirtualKey.SHIFT,
            VirtualKey.LCONTROL => VirtualKey.CONTROL,
            VirtualKey.RCONTROL => VirtualKey.CONTROL,
            VirtualKey.LMENU    => VirtualKey.MENU,
            VirtualKey.RMENU    => VirtualKey.MENU,
            VirtualKey.LSHIFT   => VirtualKey.SHIFT,
            VirtualKey.RSHIFT   => VirtualKey.SHIFT,
            _                   => VirtualKey.NO_KEY,
        };
    }

    public bool Equals(ModifierHotkey other)
        => Modifier == other.Modifier;

    public override bool Equals(object? obj)
        => obj is ModifierHotkey other && Equals(other);

    public override int GetHashCode()
        => (int)Modifier;

    public static bool operator ==(ModifierHotkey lhs, ModifierHotkey rhs)
        => lhs.Modifier == rhs.Modifier;

    public static bool operator !=(ModifierHotkey lhs, ModifierHotkey rhs)
        => lhs.Modifier != rhs.Modifier;
}
