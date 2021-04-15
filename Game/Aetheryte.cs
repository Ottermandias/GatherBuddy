using System;
using GatherBuddy.Utility;
using AetheryteRow = Lumina.Excel.GeneratedSheets.Aetheryte;

namespace GatherBuddy.Game
{
    public class Aetheryte : IComparable
    {
        public AetheryteRow Data      { get; set; }
        public FFName       Name      { get; set; }
        public Territory    Territory { get; set; }
        public int          XCoord    { get; set; }
        public int          YCoord    { get; set; }

        public short XStream
            => Data.AetherstreamX;

        public short YStream
            => Data.AetherstreamY;

        public uint Id
            => Data.RowId;

        public Aetheryte(AetheryteRow data, Territory territory, FFName name, int x, int y)
        {
            Data      = data;
            XCoord    = x;
            YCoord    = y;
            Territory = territory;
            Name      = name;
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
            if (mapId != Territory.Id)
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
            => AetherDistance(rhs.XStream, rhs.YStream);

        public override string ToString()
            => $"{Name} - {Territory!.Name}-{XCoord / 100.0:F2}:{YCoord / 100.0:F2}";
    }
}
