using System;
using Dalamud.Game;
using GatherBuddy.Enums;

namespace GatherBuddy.SeFunctions
{
    public sealed class SeTugType : SeAddressBase
    {
        public unsafe BiteType Tug
        {
            get
            {
                if (Address != IntPtr.Zero)
                    return BiteType.Unknown;

                var tug = *(byte*) Address;
                return tug switch
                {
                    36 => BiteType.Weak,
                    37 => BiteType.Strong,
                    38 => BiteType.Legendary,
                    _  => BiteType.Unknown,
                };
            }
        }

        public SeTugType(SigScanner sigScanner)
            : base(sigScanner,
                "4C 8D 0D ?? ?? ?? ?? 4D 8B 13 49 8B CB 45 0F B7 43 ?? 49 8B 93 ?? ?? ?? ?? 88 44 24 20 41 FF 92 ?? ?? ?? ?? 48 83 C4 38 C3")
        { }
    }
}
