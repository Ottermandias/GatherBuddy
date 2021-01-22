using System.Collections.Generic;
using Dalamud;
using Otter;

namespace Gathering
{
    public class Territory
    {
        public uint               id;
        public FFName             region;
        public FFName             nameList;
        public HashSet<Aetheryte> aetherytes = new();
        public int                xStream = 0;
        public int                yStream = 0;

        public Territory(uint id, FFName region, FFName placeName)
        {
            this.nameList = placeName;
            this.id       = id;
            this.region   = region;
        }

        override public string ToString()
        {
            return $"[{nameList[ClientLanguage.English]}][{region[ClientLanguage.English]}]";
        }
    }
}