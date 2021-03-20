using System;
using Dalamud;
using GatherBuddy.Utility;

namespace GatherBuddy.Classes
{
    public class Aetheryte : IComparable
    {
        public int        Id        { get; set; }
        public FFName     NameList  { get; set; }
        public Territory? Territory { get; set; }
        public int        XCoord    { get; set; }
        public int        YCoord    { get; set; }
        public int        XStream   { get; set; }
        public int        YStream   { get; set; }

        public Aetheryte(int id, int x, int y)
        {
            Id       = id;
            XCoord   = x;
            YCoord   = y;
            NameList = new FFName();
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
                return 1;

            var rhs = obj as Aetheryte;
            return Id.CompareTo(rhs?.Id ?? 0);
        }

        public double WorldDistance(uint mapId, int x, int y)
        {
            if (mapId != (Territory?.Id ?? 0))
                return double.PositiveInfinity;

            x -= XCoord;
            y -= YCoord;
            return Math.Sqrt(x * x + y * y);
        }

        public double AetherDistance(int x, int y)
        {
            x -= XStream;
            y -= YStream;
            return Math.Sqrt(x * x + y * y);
        }

        public double AetherDistance(Aetheryte rhs)
            => AetherDistance(rhs?.XStream ?? 0, rhs?.YStream ?? 0);

        public override string ToString()
            => $"{NameList[ClientLanguage.English]} - {Territory!.NameList[ClientLanguage.English]}-{XCoord / 100.0:F2}:{YCoord / 100.0:F2}";
    }
}
