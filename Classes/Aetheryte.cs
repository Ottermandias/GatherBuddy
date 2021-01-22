using System;
using Dalamud;
using Otter;

namespace Gathering
{
    public class Aetheryte : IComparable
    {
        public int       id;
        public FFName    nameList;
        public Territory territory = null;
        public int       xCoord;
        public int       yCoord;
        public int       xStream = 0;
        public int       yStream = 0;

        public Aetheryte(int id, int x, int y)
        {
            this.id       = id;
            this.xCoord   = x;
            this.yCoord   = y;
            this.nameList = new FFName();
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Aetheryte rhs = obj as Aetheryte;
            return id - rhs.id;
        }

        public double WorldDistance(uint mapId, int x, int y)
        {
            if (mapId != (territory?.id ?? 0))
                return Double.PositiveInfinity;
            x -= xCoord;
            y -= yCoord;
            return Math.Sqrt(x * x + y * y);
        }

        public double AetherDistance(int x, int y)
        {
            x -= xStream;
            y -= yStream;
            return Math.Sqrt(x * x + y * y);
        }

        public double AetherDistance(Aetheryte rhs)
        {
            return AetherDistance(rhs?.xStream ?? 0, rhs?.yStream ?? 0);
        }

        override public string ToString()
        {
            return $"{nameList[ClientLanguage.English]} - {territory.nameList[ClientLanguage.English]}-{xCoord / 100.0 :F2}:{yCoord / 100.0 :F2}";
        }
    }
}