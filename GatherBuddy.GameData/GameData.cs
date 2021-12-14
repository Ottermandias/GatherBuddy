using System;
using Dalamud.Data;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Data;
using GatherBuddy.Enums;
using GatherBuddy.Levenshtein;
using GatherBuddy.Structs;
using Lumina.Excel.GeneratedSheets;
using Aetheryte = GatherBuddy.Classes.Aetheryte;
using Fish = GatherBuddy.Classes.Fish;
using FishingSpot = GatherBuddy.Classes.FishingSpot;
using Weather = GatherBuddy.Structs.Weather;

namespace GatherBuddy;

public class GameData
{
    internal DataManager                              DataManager { get; init; }
    internal Dictionary<byte, CumulativeWeatherRates> CumulativeWeatherRates = new();

    public Dictionary<uint, Weather> Weathers           { get; init; } = new();
    public Territory[]               WeatherTerritories { get; init; } = Array.Empty<Territory>();

    public Dictionary<uint, Territory>     Territories           { get; init; } = new();
    public Dictionary<uint, Aetheryte>     Aetherytes            { get; init; } = new();
    public Dictionary<uint, Gatherable>    Gatherables           { get; init; } = new();
    public Dictionary<uint, Gatherable>    GatherablesByGatherId { get; init; } = new();
    public Dictionary<uint, GatheringNode> GatheringNodes        { get; init; } = new();
    public Dictionary<uint, Bait>          Bait                  { get; init; } = new();
    public Dictionary<uint, Fish>          Fishes                { get; init; } = new();
    public Dictionary<uint, FishingSpot>   FishingSpots          { get; init; } = new();

    public PatriciaTrie<Gatherable> GatherablesTrie { get; init; } = new();
    public PatriciaTrie<Fish>       FishTrie        { get; init; } = new();

    public int TimedGatherables     { get; init; }
    public int MultiNodeGatherables { get; init; }


    public GameData(DataManager gameData)
    {
        DataManager = gameData;
        try
        {
            Weathers = DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Weather>()!
                .ToDictionary(w => w.RowId, w => new Weather(w));
            PluginLog.Verbose("Collected {NumWeathers} different Weathers.", Weathers.Count);

            CumulativeWeatherRates = DataManager.GetExcelSheet<WeatherRate>()!
                .ToDictionary(w => (byte)w.RowId, w => new CumulativeWeatherRates(this, w));

            WeatherTerritories = DataManager.GetExcelSheet<TerritoryType>()?
                    .Where(t => t.PCSearch && t.WeatherRate != 0)
                    .Select(FindOrAddTerritory)
                    .Where(t => t != null && t.WeatherRates.Rates.Length > 1)
                    .Cast<Territory>()
                    .GroupBy(t => t.Name)
                    .Select(group => group.First())
                    .OrderBy(t => t.Name)
                    .ToArray()
             ?? Array.Empty<Territory>();
            PluginLog.Verbose("Collected {NumWeatherTerritories} different territories with dynamic weather.", WeatherTerritories.Length);

            Aetherytes = DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Aetheryte>()?
                    .Where(a => a.IsAetheryte && a.RowId > 1 && a.PlaceName.Row != 0)
                    .ToDictionary(a => a.RowId, a => new Aetheryte(this, a))
             ?? new Dictionary<uint, Aetheryte>();
            PluginLog.Verbose("Collected {NumAetherytes} different aetherytes.", Aetherytes.Count);
            ForcedAetherytes.ApplyMissingAetherytes(this);

            Gatherables = DataManager.GetExcelSheet<GatheringItem>()?
                    .Where(g => g.Item != 0 && g.Item < 1000000)
                    .GroupBy(g => g.Item)
                    .Select(group => group.First())
                    .ToDictionary(g => (uint)g.Item, g => new Gatherable(this, g))
             ?? new Dictionary<uint, Gatherable>();
            GatherablesByGatherId = Gatherables.Values.ToDictionary(g => g.GatheringId, g => g);
            PluginLog.Verbose("Collected {NumGatherables} different gatherable items.", Gatherables.Count);

            GatheringNodes = DataManager.GetExcelSheet<GatheringPointBase>()?
                    .Where(b => b.GatheringType.Row < (int)Enums.GatheringType.Spearfishing)
                    .Select(b => new GatheringNode(this, b))
                    .Where(n => n.Territory.Id > 1 && n.Items.Count > 0)
                    .ToDictionary(n => n.Id, n => n)
             ?? new Dictionary<uint, GatheringNode>();
            PluginLog.Verbose("Collected {NumGatheringNodes} different gathering nodes", GatheringNodes.Count);

            Bait = DataManager.GetExcelSheet<Item>()?
                    .Where(i => i.ItemSearchCategory.Row == Structs.Bait.FishingTackleRow)
                    .ToDictionary(b => b.RowId, b => new Bait(b))
             ?? new Dictionary<uint, Bait>();
            PluginLog.Verbose("Collected {NumBaits} different types of bait.", Bait.Count);

            Fishes = DataManager.GetExcelSheet<FishParameter>()?
                    .Where(f => f.Item != 0 && f.Item < 1000000)
                    .Select(f => new Fish(DataManager, f))
                    .Concat(DataManager.GetExcelSheet<SpearfishingItem>()?
                            .Where(sf => sf.Item.Row != 0 && sf.Item.Row < 1000000)
                            .Select(sf => new Fish(DataManager, sf))
                     ?? Array.Empty<Fish>())
                    .GroupBy(f => f.ItemId)
                    .Select(group => group.First())
                    .ToDictionary(f => f.ItemId, f => f)
             ?? new Dictionary<uint, Fish>();
            PluginLog.Verbose("Collected {NumFishes} different types of fish.", Fishes.Count);
            Data.Fish.Apply(this);

            FishingSpots = DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.FishingSpot>()?
                    .Where(f => f.PlaceName.Row != 0 && f.TerritoryType.Row > 0)
                    .Select(f => new FishingSpot(this, f))
                    .Concat(
                        DataManager.GetExcelSheet<SpearfishingNotebook>()?
                            .Where(sf => sf.PlaceName.Row != 0 && sf.TerritoryType.Row > 0)
                            .Select(sf => new FishingSpot(this, sf))
                     ?? Array.Empty<FishingSpot>())
                    .Where(f => f.Territory.Id != 0)
                    .ToDictionary(f => f.Id, f => f)
             ?? new Dictionary<uint, FishingSpot>();
            PluginLog.Verbose("Collected {NumFishingSpots} different fishing spots.", FishingSpots.Count);

            HiddenItems.Apply(this);
            HiddenItems.ApplyDarkMatter(this);
            HiddenMaps.Apply(this);
            HiddenSeeds.Apply(this);
            ForcedAetherytes.Apply(this);

            foreach (var gatherable in Gatherables.Values)
            {
                if (gatherable.NodeType != NodeType.Unknown && !gatherable.NodeList.Any(n => n.Times.AlwaysUp()))
                    gatherable.InternalLocationId = ++TimedGatherables;
                else if (gatherable.NodeList.Count > 1)
                    gatherable.InternalLocationId = -++MultiNodeGatherables;
                GatherablesTrie.Add(gatherable.Name[gameData.Language].ToLowerInvariant(), gatherable);
            }

            foreach (var fish in Fishes.Values)
            {
                if (fish.FishingSpots.Count > 0 && fish.FishRestrictions != FishRestrictions.None && !fish.OceanFish)
                    fish.InternalLocationId = ++TimedGatherables;
                else if (fish.FishingSpots.Count > 0)
                    fish.InternalLocationId = -++MultiNodeGatherables;
                FishTrie.Add(fish.Name[gameData.Language].ToLowerInvariant(), fish);
            }
        }
        catch (Exception e)
        {
            PluginLog.Error($"Error while setting up data:\n{e}");
        }
    }

    public Territory? FindOrAddTerritory(TerritoryType? t)
    {
        if (t == null || t.RowId < 2)
            return null;

        if (Territories.TryGetValue(t.RowId, out var territory))
            return territory;

        // Create territory if it does not exist.
        var aether = DataManager.GetExcelSheet<TerritoryTypeTelepo>()?.GetRow(t.RowId);
        territory = new Territory(this, t, aether);
        Territories.Add(t.RowId, territory);
        return territory;
    }
}
