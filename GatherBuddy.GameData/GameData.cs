using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud;
using Dalamud.Logging;
using Dalamud.Game;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using GatherBuddy.Classes;
using GatherBuddy.Data;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Levenshtein;
using GatherBuddy.Structs;
using Lumina.Excel.Sheets;
using OtterGui.Log;
using Aetheryte = GatherBuddy.Classes.Aetheryte;
using AetheryteRow = Lumina.Excel.Sheets.Aetheryte;
using Fish = GatherBuddy.Classes.Fish;
using FishingSpot = GatherBuddy.Classes.FishingSpot;
using Weather = GatherBuddy.Structs.Weather;
using WeatherRow = Lumina.Excel.Sheets.Weather;
using FishingSpotRow = Lumina.Excel.Sheets.FishingSpot;

namespace GatherBuddy;

public class GameData
{
    internal IDataManager                                   DataManager { get; }
    internal FrozenDictionary<byte, CumulativeWeatherRates> CumulativeWeatherRates = FrozenDictionary<byte, CumulativeWeatherRates>.Empty;

    public readonly Logger Log;

    public FrozenDictionary<uint, Weather> Weathers           { get; } = FrozenDictionary<uint, Weather>.Empty;
    public Territory[]                     WeatherTerritories { get; } = [];

    public Dictionary<uint, Territory>           Territories           { get; } = [];
    public FrozenDictionary<uint, Aetheryte>     Aetherytes            { get; } = FrozenDictionary<uint, Aetheryte>.Empty;
    public FrozenDictionary<uint, Gatherable>    Gatherables           { get; } = FrozenDictionary<uint, Gatherable>.Empty;
    public FrozenDictionary<uint, Gatherable>    GatherablesByGatherId { get; } = FrozenDictionary<uint, Gatherable>.Empty;
    public FrozenDictionary<uint, GatheringNode> GatheringNodes        { get; } = FrozenDictionary<uint, GatheringNode>.Empty;
    public FrozenDictionary<uint, Bait>          Bait                  { get; } = FrozenDictionary<uint, Bait>.Empty;
    public FrozenDictionary<uint, Fish>          Fishes                { get; } = FrozenDictionary<uint, Fish>.Empty;
    public FrozenDictionary<uint, FishingSpot>   FishingSpots          { get; } = FrozenDictionary<uint, FishingSpot>.Empty;
    public Dictionary<uint, List<Vector3>> WorldCoords           { get; init; } = new();

    public IReadOnlyList<OceanRoute> OceanRoutes   { get; } = Array.Empty<OceanRoute>();
    public OceanTimeline             OceanTimeline { get; } = null!;

    public PatriciaTrie<Gatherable> GatherablesTrie { get; } = new();
    public PatriciaTrie<Fish>       FishTrie        { get; } = new();

    public GatheringIcons GatheringIcons { get; } = null!;

    public int TimedGatherables     { get; }
    public int MultiNodeGatherables { get; }

    public (IGatherable? Item, ILocation? Location) GetConfig(ObjectType type, uint itemId, uint locationId)
        => type switch
        {
            ObjectType.Gatherable => (itemId == 0 ? null : Gatherables.GetValueOrDefault(itemId),
                locationId == 0 ? null : GatheringNodes.GetValueOrDefault(locationId)),
            ObjectType.Fish => (itemId == 0 ? null : Fishes.GetValueOrDefault(itemId),
                locationId == 0 ? null : FishingSpots.GetValueOrDefault(locationId)),
            _ => (null, null),
        };

    public GameData(IDataManager gameData, Logger log, Dictionary<uint, List<Vector3>> worldCoordsDict)
    {
        Log         = log;
        DataManager = gameData;
        WorldCoords = worldCoordsDict;
        try
        {
            GatheringIcons = new GatheringIcons(gameData);

            Weathers = DataManager.GetExcelSheet<WeatherRow>()
                .ToFrozenDictionary(w => w.RowId, w => new Weather(w));
            Log.Verbose("Collected {NumWeathers} different Weathers.", Weathers.Count);

            CumulativeWeatherRates = DataManager.GetExcelSheet<WeatherRate>()
                .ToFrozenDictionary(w => (byte)w.RowId, w => new CumulativeWeatherRates(this, w));

            WeatherTerritories = DataManager.GetExcelSheet<TerritoryType>()
                .Where(t => t.PCSearch && t.WeatherRate.RowId != 0)
                .Select(t => FindOrAddTerritory(t))
                .Where(t => t is { WeatherRates.Rates.Length: > 1 })
                .OfType<Territory>()
                .GroupBy(t => t.Name)
                .Select(group => group.First())
                .OrderBy(t => t.Name)
                .ToArray();
            Log.Verbose("Collected {NumWeatherTerritories} different territories with dynamic weather.", WeatherTerritories.Length);

            Aetherytes = DataManager.GetExcelSheet<AetheryteRow>()
                .Where(a => a is { IsAetheryte: true, RowId: > 1 } && a.PlaceName.RowId != 0)
                .ToFrozenDictionary(a => a.RowId, a => new Aetheryte(this, a));
            Log.Verbose("Collected {NumAetherytes} different aetherytes.", Aetherytes.Count);
            ForcedAetherytes.ApplyMissingAetherytes(this);

            Gatherables = DataManager.GetExcelSheet<GatheringItem>()
                .Where(g => g.Item.RowId != 0 && g.Item.RowId < 1000000)
                .GroupBy(g => g.Item)
                .Select(group => group.First())
                .ToFrozenDictionary(g => g.Item.RowId, g => new Gatherable(this, g));
            GatherablesByGatherId = Gatherables.Values.ToFrozenDictionary(g => g.GatheringId, g => g);
            Log.Verbose("Collected {NumGatherables} different gatherable items.", Gatherables.Count);

            // Create GatheringItemPoint dictionary.
            var tmpGatheringItemPoint = DataManager.GetSubrowExcelSheet<GatheringItemPoint>().SelectMany(g => g)
                .GroupBy(row => row.GatheringPoint.RowId)
                .ToFrozenDictionary(group => group.Key, group => group.Select(g => g.RowId).Distinct().ToList());

            var tmpGatheringPoints = DataManager.GetExcelSheet<GatheringPoint>()
                .Where(row => row.PlaceName.RowId > 0)
                .GroupBy(row => row.GatheringPointBase.RowId)
                .ToFrozenDictionary(group => group.Key, group => group.Select(g => g.RowId).Distinct().ToList());

            GatheringNodes = DataManager.GetExcelSheet<GatheringPointBase>()
                .Where(b => b.GatheringType.RowId < (int)Enums.GatheringType.Spearfishing)
                .Select(b => new GatheringNode(this, tmpGatheringPoints, tmpGatheringItemPoint, b))
                .Where(n => n.Territory.Id > 1 && n.Items.Count > 0)
                .ToFrozenDictionary(n => n.Id, n => n);
            Log.Verbose("Collected {NumGatheringNodes} different gathering nodes", GatheringNodes.Count);

            Bait = DataManager.GetExcelSheet<Item>()
                .Where(i => i.ItemSearchCategory.RowId == Structs.Bait.FishingTackleRow)
                .ToFrozenDictionary(b => b.RowId, b => new Bait(b));
            Log.Verbose("Collected {NumBaits} different types of bait.", Bait.Count);

            var catchData = DataManager.GetExcelSheet<FishingNoteInfo>();
            Fishes = DataManager.GetExcelSheet<FishParameter>()
                .Where(f => f.Item.RowId != 0 && f.Item.RowId < 1000000)
                .Select(f => new Fish(DataManager, f, catchData))
                .Concat(DataManager.GetExcelSheet<SpearfishingItem>()
                    .Where(sf => sf.Item.RowId != 0 && sf.Item.RowId < 1000000)
                    .Select(sf => new Fish(DataManager, sf, catchData)))
                .GroupBy(f => f.ItemId)
                .Select(group => group.First())
                .ToFrozenDictionary(f => f.ItemId, f => f);
            Log.Verbose("Collected {NumFishes} different types of fish.", Fishes.Count);
            Data.Fish.Apply(this);

            FishingSpots = DataManager.GetExcelSheet<FishingSpotRow>()
                .Where(f => f.PlaceName.RowId != 0 && (f.TerritoryType.RowId > 0 || f.RowId == 10000 || f.RowId >= 10017))
                .Select(f => new FishingSpot(this, f))
                .Concat(
                    DataManager.GetExcelSheet<SpearfishingNotebook>()
                        .Where(sf => sf.PlaceName.RowId != 0 && sf.TerritoryType.RowId > 0)
                        .Select(sf => new FishingSpot(this, sf)))
                .Where(f => f.Territory.Id != 0)
                .ToFrozenDictionary(f => f.Id, f => f);
            Log.Verbose("Collected {NumFishingSpots} different fishing spots.", FishingSpots.Count);

            HiddenMaps.Apply(this);
            ForcedAetherytes.Apply(this);

            OceanRoutes   = SetupOceanRoutes(gameData, FishingSpots);
            OceanTimeline = new OceanTimeline(gameData, OceanRoutes);
            SetOceanFish(OceanRoutes, Fishes.Values);

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
                if (fish.FishingSpots.Count > 0 && !fish.OceanFish && fish.FishRestrictions != FishRestrictions.None
                 || fish is { OceanFish: true, FishRestrictions: FishRestrictions.Time })
                    fish.InternalLocationId = ++TimedGatherables;
                else if (fish.FishingSpots.Count > 0)
                    fish.InternalLocationId = -++MultiNodeGatherables;
                FishTrie.Add(fish.Name[gameData.Language].ToLowerInvariant(), fish);
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error while setting up data:\n{e}");
        }
    }

    public Territory? FindOrAddTerritory(TerritoryType? t)
    {
        if (t == null || t.Value.RowId < 2)
            return null;

        if (Territories.TryGetValue(t.Value.RowId, out var territory))
            return territory;

        // Create territory if it does not exist.
        var aether = DataManager.GetExcelSheet<TerritoryTypeTelepo>().GetRowOrDefault(t.Value.RowId);
        territory = new Territory(this, t.Value, aether);
        Territories.Add(t.Value.RowId, territory);
        return territory;
    }

    private static void SetOceanFish(IEnumerable<OceanRoute> routes, IEnumerable<Fish> fishes)
    {
        var set = new Dictionary<uint, OceanArea>(128);
        foreach (var route in routes)
        {
            set.TryAdd(route.SpotDay.Normal.Id,      route.Area);
            set.TryAdd(route.SpotDay.Spectral.Id,    route.Area);
            set.TryAdd(route.SpotNight.Normal.Id,    route.Area);
            set.TryAdd(route.SpotNight.Spectral.Id,  route.Area);
            set.TryAdd(route.SpotSunset.Normal.Id,   route.Area);
            set.TryAdd(route.SpotSunset.Spectral.Id, route.Area);
        }

        foreach (var fish in fishes)
        {
            var spot = fish.FishData?.FishingSpot.RowId ?? 0u;
            if (set.TryGetValue(spot, out var area))
                fish.OceanArea = fish.OceanArea == OceanArea.None || fish.OceanArea == area ? area : OceanArea.Unknown;
        }
    }

    private static OceanRoute[] SetupOceanRoutes(IDataManager manager, IReadOnlyDictionary<uint, FishingSpot> fishingSpots)
    {
        var routeSheet = manager.GetExcelSheet<IKDRoute>(ClientLanguage.English);
        var spotSheet  = manager.GetExcelSheet<IKDSpot>();
        var ret        = new OceanRoute[routeSheet.Count - 1];

        var spots = spotSheet.Skip(1).Select(r
                => fishingSpots.TryGetValue(r.SpotMain.RowId, out var main) && fishingSpots.TryGetValue(r.SpotSub.RowId, out var sub)
                    ? (main, Sub: sub)
                    : throw new Exception("Invalid fishing spot!"))
            .ToArray();

        for (var i = 1u; i < routeSheet.Count; ++i)
        {
            var row = routeSheet.GetRow(i);
            var (start, day, sunset, night) = row.Time[0].RowId switch
            {
                1 => (OceanTime.Sunset, spots[(int)row.Spot[1].RowId - 1], spots[(int)row.Spot[2].RowId - 1],
                    spots[(int)row.Spot[0].RowId - 1]),
                2 => (OceanTime.Night, spots[(int)row.Spot[0].RowId - 1], spots[(int)row.Spot[1].RowId - 1],
                    spots[(int)row.Spot[2].RowId - 1]),
                3 => (OceanTime.Day, spots[(int)row.Spot[2].RowId - 1], spots[(int)row.Spot[0].RowId - 1],
                    spots[(int)row.Spot[1].RowId - 1]),
                _ => (OceanTime.Sunset, spots[(int)row.Spot[1].RowId - 1], spots[(int)row.Spot[2].RowId - 1],
                    spots[(int)row.Spot[0].RowId - 1]),
            };
            ret[i - 1] = new OceanRoute
            {
                Id         = (byte)i,
                Name       = row.Name.ToDalamudString().TextValue,
                StartTime  = start,
                SpotDay    = day,
                SpotSunset = sunset,
                SpotNight  = night,
                Area       = i < 13 ? OceanArea.Aldenard : i < 19 ? OceanArea.Othard : OceanArea.Unknown,
            };
        }

        return ret;
    }
}
