using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using FFXIVClientStructs.FFXIV.Client.Game.UI;

namespace GatherBuddy.SeFunctions;

public unsafe class WaymarkManager
{
    private readonly MarkingController* _markingController;

    public nint Address
        => (nint)_markingController;

    public readonly int Count;

    public WaymarkManager()
    {
        _markingController = MarkingController.Instance();
        Count              = _markingController->FieldMarkers.Length;
    }

    public FieldMarker this[int idx]
        => _markingController == null
            ? default
            : _markingController->FieldMarkers[idx];

    public IList<Vector3> GetWaymarks()
    {
        if (_markingController == null)
            return Array.Empty<Vector3>();

        var list = new List<Vector3>(8);
        for (var i = 0; i < 8; ++i)
        {
            var w = this[i];
            if (w.Active)
                list.Add(w.Position);
        }

        return list;
    }

    public void ClearWaymark(int idx)
    {
        if (idx is < 0 || idx > Count || _markingController == null)
            return;

        _markingController->FieldMarkers[idx] = default;
    }

    public void SetWaymark(int idx)
    {
        if (idx is < 0 || idx > Count || _markingController == null || Dalamud.ClientState.LocalPlayer == null)
            return;

        ref var marker = ref _markingController->FieldMarkers[idx];
        var     pos    = Dalamud.ClientState.LocalPlayer.Position;
        marker.Position = pos;
        marker.X        = (int)(pos.X * 1000 + 0.9f);
        marker.Y        = (int)(pos.Y * 1000 + 0.9f);
        marker.Z        = (int)(pos.Z * 1000 + 0.9f);
        marker.Active   = true;
    }

    public void SetWaymarks(IEnumerable<Vector3> waymarks)
    {
        if (_markingController == null)
            return;

        var idx = 0;
        foreach (var waymark in waymarks.Take(Count))
        {
            ref var marker = ref _markingController->FieldMarkers[idx++];
            marker.Position = waymark;
            marker.X        = (int)(waymark.X * 1000 + 0.9f);
            marker.Y        = (int)(waymark.Y * 1000 + 0.9f);
            marker.Z        = (int)(waymark.Z * 1000 + 0.9f);
            marker.Active   = true;
        }

        for (; idx < Count; ++idx)
            _markingController->FieldMarkers[idx] = default;
    }
}
