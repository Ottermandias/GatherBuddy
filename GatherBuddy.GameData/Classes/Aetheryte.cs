using System;
using System.Linq;
using GatherBuddy.Utility;
using Lumina.Excel.Sheets;
using AetheryteRow = Lumina.Excel.Sheets.Aetheryte;

namespace GatherBuddy.Classes;

public class Aetheryte : IComparable<Aetheryte>
{
    public AetheryteRow Data      { get; set; }
    public string       Name      { get; set; }
    public Territory    Territory { get; set; }
    public int          XCoord    { get; set; }
    public int          YCoord    { get; set; }

    public ushort XStream
        => Territory.XStream;

    public ushort YStream
        => Territory.YStream;

    public ushort Plane
        => Territory.Plane;

    public uint Id
        => Data.RowId;

    public Aetheryte(GameData gameData, AetheryteRow data)
    {
        Data      = data;
        Name      = MultiString.ParseSeStringLumina(data.PlaceName.ValueNullable?.Name);
        Territory = gameData.FindOrAddTerritory(data.Territory.ValueNullable) ?? Territory.Invalid;
        var mapMarker = gameData.DataManager.GetSubrowExcelSheet<MapMarker>().SelectMany(m => m).Cast<MapMarker?>().FirstOrDefault(m => m!.Value.DataType == 3 && m.Value.DataKey.RowId == data.RowId);
        if (mapMarker == null)
        {
            gameData.Log.Error($"No Map Marker for Aetheryte {Name} [{data.RowId}].");
        }
        else
        {
            XCoord = Maps.MarkerToMap(mapMarker.Value.X, Territory.SizeFactor);
            YCoord = Maps.MarkerToMap(mapMarker.Value.Y, Territory.SizeFactor);
        }

        Territory.Aetherytes.Add(this);
    }

    public int CompareTo(Aetheryte? rhs)
        => Id.CompareTo(rhs?.Id ?? 0);

    public int WorldDistance(uint territoryId, int x, int y)
        => territoryId == Territory.Id
            ? Utility.Math.SquaredDistance(x, y, XCoord, YCoord)
            : int.MaxValue;

    public int AetherDistance(ushort x, ushort y, ushort plane)
        => Utility.Math.SquaredDistance(x, y, plane, XStream, YStream, Plane);

    public double AetherDistance(Aetheryte rhs)
        => AetherDistance(rhs.XStream, rhs.YStream, rhs.Plane);

    public double AetherDistance(Territory rhs)
        => AetherDistance(rhs.XStream, rhs.YStream, rhs.Plane);

    public override string ToString()
        => $"{Name} - {Territory.Name}-{XCoord / 100.0:F2}:{YCoord / 100.0:F2}";
}
