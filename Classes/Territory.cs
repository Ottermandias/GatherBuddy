using System.Collections.Generic;
using Dalamud;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Classes
{
    public class Territory
    {
        public uint               Id         { get; }
        public FFName             Region     { get; }
        public FFName             NameList   { get; }
        public HashSet<Aetheryte> Aetherytes { get; }      = new();
        public float              SizeFactor { get; set; } = 1.0f;
        public int                XStream    { get; set; } = 0;
        public int                YStream    { get; set; } = 0;

        public Territory(uint id, FFName region, FFName placeName)
        {
            NameList = placeName;
            Id       = id;
            Region   = region;
        }

        public override string ToString()
            => $"[{NameList[ClientLanguage.English]}][{Region[ClientLanguage.English]}]";
    }
}
