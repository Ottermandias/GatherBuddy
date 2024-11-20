using FFXIVClientStructs.FFXIV.Client.Game.UI;
using GatherBuddy.Structs;

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

    public WaymarkSet GetWaymarks()
    {
        var list = new WaymarkSet();
        if (_markingController == null)
            return list;

        for (var i = 0; i < WaymarkSet.Count; ++i)
        {
            var w = this[i];
            if (w.Active)
                list[i] = w.Position;
        }

        return list;
    }

    public void ClearWaymark(int idx)
    {
        if (idx is < 0 || idx > WaymarkSet.Count || _markingController == null)
            return;

        _markingController->FieldMarkers[idx] = default;
    }

    public void SetWaymark(int idx)
    {
        if (idx is < 0 || idx > WaymarkSet.Count || _markingController == null || Dalamud.ClientState.LocalPlayer == null)
            return;

        ref var marker = ref _markingController->FieldMarkers[idx];
        var     pos    = Dalamud.ClientState.LocalPlayer.Position;
        marker.Position = pos;
        marker.X        = (int)(pos.X * 1000 + 0.9f);
        marker.Y        = (int)(pos.Y * 1000 + 0.9f);
        marker.Z        = (int)(pos.Z * 1000 + 0.9f);
        marker.Active   = true;
    }

    public void SetWaymarks(in WaymarkSet waymarks)
    {
        if (_markingController == null)
            return;

        var idx = 0;
        for (var i = 0; i < WaymarkSet.Count; ++i)
        {
            ref var          marker  = ref _markingController->FieldMarkers[idx++];
            ref readonly var waymark = ref waymarks[i];
            if (float.IsNaN(waymark.X))
            {
                marker = default;
            }
            else
            {
                marker.Position = waymark;
                marker.X        = (int)(waymark.X * 1000 + 0.9f);
                marker.Y        = (int)(waymark.Y * 1000 + 0.9f);
                marker.Z        = (int)(waymark.Z * 1000 + 0.9f);
                marker.Active   = true;
            }
        }
    }
}
