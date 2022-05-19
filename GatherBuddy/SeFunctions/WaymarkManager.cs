using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Dalamud.Utility.Signatures;

namespace GatherBuddy.SeFunctions;

public unsafe class WaymarkManager
{
    [StructLayout(LayoutKind.Explicit, Size = 0x20)]
    public struct Waymark
    {
        [FieldOffset(0x00)]
        public float X;

        [FieldOffset(0x04)]
        public float Y;

        [FieldOffset(0x08)]
        public float Z;

        [FieldOffset(0x10)]
        public ulong Unk1;

        [FieldOffset(0x18)]
        public uint Unk2;

        [FieldOffset(0x1C)]
        public bool IsSet;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct MarkingController
    {
        [FieldOffset(0x1B0)]
        public Waymark WaymarkA;

        [FieldOffset(0x1B0 + 0x20)]
        public Waymark WaymarkB;

        [FieldOffset(0x1B0 + 0x40)]
        public Waymark WaymarkC;

        [FieldOffset(0x1B0 + 0x60)]
        public Waymark WaymarkD;

        [FieldOffset(0x1B0 + 0x80)]
        public Waymark Waymark1;

        [FieldOffset(0x1B0 + 0xA0)]
        public Waymark Waymark2;

        [FieldOffset(0x1B0 + 0xC0)]
        public Waymark Waymark3;

        [FieldOffset(0x1B0 + 0xE0)]
        public Waymark Waymark4;

        public void Set(int idx, float x, float y, float z)
        {
            var w = new Waymark
            {
                IsSet = true,
                X     = x,
                Y     = y,
                Z     = z,
            };
            _ = idx switch
            {
                0 => Waymark1 = w,
                1 => Waymark2 = w,
                2 => Waymark3 = w,
                3 => Waymark4 = w,
                4 => WaymarkA = w,
                5 => WaymarkB = w,
                6 => WaymarkC = w,
                7 => WaymarkD = w,
                _ => w,
            };
        }

        public void Clear(int idx)
        {
            _ = idx switch
            {
                0 => Waymark1 = new Waymark(),
                1 => Waymark2 = new Waymark(),
                2 => Waymark3 = new Waymark(),
                3 => Waymark4 = new Waymark(),
                4 => WaymarkA = new Waymark(),
                5 => WaymarkB = new Waymark(),
                6 => WaymarkC = new Waymark(),
                7 => WaymarkD = new Waymark(),
                _ => new Waymark(),
            };
        }
    }

    [Signature("48 8D 0D ?? ?? ?? ?? FF CA E8", ScanType = ScanType.StaticAddress)]
#pragma warning disable CS0649
    private MarkingController* _controller;
#pragma warning restore CS0649

    public IntPtr Address
        => (IntPtr)_controller;

    public WaymarkManager()
    {
        SignatureHelper.Initialise(this);
    }

    public Waymark this[int idx]
        => _controller == null
            ? default
            : idx switch
            {
                0 => _controller->Waymark1,
                1 => _controller->Waymark2,
                2 => _controller->Waymark3,
                3 => _controller->Waymark4,
                4 => _controller->WaymarkA,
                5 => _controller->WaymarkB,
                6 => _controller->WaymarkC,
                7 => _controller->WaymarkD,
                _ => default,
            };

    public IList<Vector3> GetWaymarks()
    {
        if (_controller == null)
            return Array.Empty<Vector3>();

        var list = new List<Vector3>(8);
        for (var i = 0; i < 8; ++i)
        {
            var w = this[i];
            if (w.IsSet)
                list.Add(new Vector3(w.X, w.Y, w.Z));
        }
        return list;
    }

    public void ClearWaymark(int idx)
    {
        if (idx is < 0 or > 7 || _controller == null)
            return;

        _controller->Clear(idx);
    }

    public void SetWaymark(int idx)
    {
        if (idx is < 0 or > 7 || _controller == null || Dalamud.ClientState.LocalPlayer == null)
            return;

        _controller->Set(idx, Dalamud.ClientState.LocalPlayer.Position.X, Dalamud.ClientState.LocalPlayer.Position.Y,
            Dalamud.ClientState.LocalPlayer.Position.Z);
    }

    public void SetWaymarks(IEnumerable<Vector3> waymarks)
    {
        if (_controller == null)
            return;

        var idx = 0;
        foreach (var waymark in waymarks.Take(8))
            _controller->Set(idx++, waymark.X, waymark.Y, waymark.Z);
        for (; idx < 8; ++idx)
            _controller->Clear(idx);
    }
}
