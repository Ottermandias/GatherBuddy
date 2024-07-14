using System.Linq;
using Dalamud.Interface.Textures;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Time;

namespace GatherBuddy.Gui;

public partial class Interface
{
    public class ExtendedGatherable
    {
        public Gatherable              Data;
        public ISharedImmediateTexture Icon;
        public string                  Territories;
        public string                  Uptimes;
        public string                  Folklore;
        public string                  Level;
        public string                  NodeNames;
        public string                  Expansion;
        public string                  Aetherytes;

        public (ILocation, TimeInterval) Uptime
            => GatherBuddy.UptimeManager.BestLocation(Data);

        public ExtendedGatherable(Gatherable data)
        {
            Data = data;
            Icon = Icons.DefaultStorage.TextureProvider.GetFromGameIcon(new GameIconLookup(data.ItemData.Icon));

            Territories = string.Join("\n", data.NodeList.Select(n => n.Territory.Name).Distinct());
            if (!Territories.Contains('\n'))
                Territories = '\0' + Territories;

            Folklore = data.NodeList.Count == 0 || data.NodeList.Any(n => n.Folklore.Length == 0)
                ? string.Empty
                : data.NodeList.First().Folklore;
            Uptimes = data.NodeType switch
            {
                NodeType.Regular => "Always",
                NodeType.Unknown => "Unknown",
                _                => data.NodeList.Select(n => n.Times).Aggregate(BitfieldUptime.Combine).PrintHours(true),
            };
            Level     = Data.LevelString();
            NodeNames = string.Join("\n", data.NodeList.Select(n => n.Name).Distinct());
            if (!NodeNames.Contains('\n'))
                NodeNames = '\0' + NodeNames;

            Expansion = data.ExpansionIdx switch
            {
                0 => "ARR",
                1 => "HW",
                2 => "SB",
                3 => "ShB",
                4 => "EW",
                5 => "DT",
                _ => "Unk",
            };
            Aetherytes = string.Join("\n",
                data.NodeList.Where(n => n.ClosestAetheryte != null).Select(n => n.ClosestAetheryte!.Name).Distinct());
            if (!Aetherytes.Contains('\n'))
                Aetherytes = '\0' + Aetherytes;
        }
    }
}
