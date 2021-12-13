using System.Collections.Generic;
using GatherBuddy.Utility;
using TerritoryType = Lumina.Excel.GeneratedSheets.TerritoryType;

namespace GatherBuddy.Game
{
    public class Territory
    {
        public TerritoryType      Data       { get; }
        public FFName             Region     { get; }
        public FFName             Name       { get; }
        public HashSet<Aetheryte> Aetherytes { get; }      = new();
        public float              SizeFactor { get; set; } = 1.0f;
        public int                XStream    { get; set; } = 0;
        public int                YStream    { get; set; } = 0;

        public uint Id
            => Data.RowId;

        public Territory(TerritoryType data, FFName region, FFName placeName)
        {
            Name   = placeName;
            Data   = data;
            Region = region;
        }

        public override string ToString()
            => $"[{Name}][{Region}]";
    }
}
