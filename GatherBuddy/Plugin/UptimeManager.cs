using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.SeFunctions;
using GatherBuddy.Time;
using GatherBuddy.Utility;
using Lumina.Excel.Sheets;
using FishingSpot = GatherBuddy.Classes.FishingSpot;
using GatheringType = GatherBuddy.Enums.GatheringType;

namespace GatherBuddy.Plugin;

public class UptimeManager : IDisposable
{
    // We only cache the best uptime regarding current server time and corresponding location per item.
    public readonly  IGatherable[]                                 TimedGatherables;
    private readonly (ILocation Location, TimeInterval Interval)[] _bestUptime;
    private readonly (ILocation Location, uint Reset)[]            _bestLocation;
    private          uint                                          _lastReset = 1;
    private          ushort                                        _currentTerritory;
    private          ushort                                        _aetherStreamX;
    private          ushort                                        _aetherStreamY;
    private          ushort                                        _aetherPlane;


    public event Action<IGatherable>? UptimeChange;

    public UptimeManager(GameData gameData)
    {
        // Set an array of available timed gatherables.
        TimedGatherables = new IGatherable[gameData.TimedGatherables];
        foreach (var gatherable in gameData.Gatherables.Values.Where(g => g.InternalLocationId > 0))
            TimedGatherables[gatherable.InternalLocationId - 1] = gatherable;
        foreach (var gatherable in gameData.Fishes.Values.Where(g => g.InternalLocationId > 0))
            TimedGatherables[gatherable.InternalLocationId - 1] = gatherable;

        // Ensures that any valid interval is better than this for the first update by setting them to Never.
        // +1 due to 0 being unused.
        _bestUptime = new (ILocation, TimeInterval)[gameData.TimedGatherables + 1];
        for (var i = 0; i < _bestUptime.Length; ++i)
            _bestUptime[i] = (null!, TimeInterval.Never);

        // No +1 due to just bit flipping negative values.
        _bestLocation = new (ILocation, uint)[gameData.MultiNodeGatherables];
        for (var i = 0; i < _bestLocation.Length; ++i)
            _bestLocation[i] = (null!, 0);

        SetCurrentTerritory(Dalamud.ClientState.TerritoryType);
        Dalamud.ClientState.TerritoryChanged += OnTerritoryChange;
    }

    public void ResetLocations()
        => _lastReset++;

    public void ResetModifiedUptimes()
    {
        foreach (var fish in TimedGatherables.OfType<Fish>().Where(f => f.HasOverridenData))
        {
            switch (fish.InternalLocationId)
            {
                case > 0: _bestUptime[fish.InternalLocationId]    = (null!, TimeInterval.Never); break;
                case < 0: _bestLocation[~fish.InternalLocationId] = (null!, 0); break;
            }
        }
    }

    // Get the best location if nothing has reset locations,
    // otherwise update the best aetheryte and return that.
    private ILocation GetLocation(IGatherable item)
    {
        Debug.Assert(item.InternalLocationId < 0);

        var idx = ~item.InternalLocationId;
        var (loc, reset) = _bestLocation[idx];

        if (reset >= _lastReset)
            return loc;

        var closest = item.Locations.FirstOrDefault(l =>
                l is FishingSpot f && (!f.Spearfishing || !f.SpearfishingSpotData!.Value.IsShadowNode)
             || l is GatheringNode n && n.Times.AlwaysUp())
         ?? FindClosestAetheryte(item) ?? item.Locations.FirstOrDefault();
        Debug.Assert(closest != null);
        _bestLocation[idx] = (closest, _lastReset);
        return closest;
    }

    private void OnTerritoryChange(ushort id)
        => SetCurrentTerritory(id);

    public void Dispose()
        => Dalamud.ClientState.TerritoryChanged -= OnTerritoryChange;

    // The best interval depends on current time. 
    // If both intervals have already started but not ended, choose the interval that ends later.
    // If only one has started, choose that, otherwise choose the earlier interval.
    // If the first interval has already ended, the second is better unchecked because then it does not matter.
    private static bool IntervalBetter(TimeInterval lhs, TimeInterval rhs, TimeStamp now)
    {
        if (lhs.End < now)
            return true;

        if (lhs.Start < now)
            if (rhs.Start < now)
                return rhs.End > lhs.End;
            else
                return false;

        if (rhs.Start < now)
            return true;

        return lhs.Start >= rhs.Start;
    }

    // Obtain the next uptime for a fish from now in a specific territory.
    private static TimeInterval GetUptime(Fish fish, Territory territory, TimeStamp now)
    {
        if (fish.OceanFish)
            // Ocean fish timing is based on the real world time, not server time.
            return OceanUptime.GetOceanUptime(fish, now);

        if (fish.FishRestrictions == FishRestrictions.Time)
            return fish.Interval == RepeatingInterval.Always ? TimeInterval.Invalid : fish.Interval.NextRealUptime(now);

        if (fish.FishRestrictions.HasFlag(FishRestrictions.Time) && fish.Interval == RepeatingInterval.Always)
            return TimeInterval.Invalid;

        if (fish.CurrentWeather.Length == 0 && fish.PreviousWeather.Length == 0)
            return TimeInterval.Invalid;

        var wl = GatherBuddy.WeatherManager.RequestForecast(territory, fish.CurrentWeather, fish.PreviousWeather, fish.Interval, now);
        if (wl.Timestamp == TimeStamp.Epoch)
            return TimeInterval.Invalid;

        var end     = GatherBuddy.WeatherManager.ExtendedDuration(territory, fish.CurrentWeather, fish.PreviousWeather, wl);
        var overlap = new TimeInterval(wl.Timestamp, end).FirstOverlap(fish.Interval);
        return overlap;
    }

    // Get the best uptime for a set of nodes and a specific time.
    // Can be used to only check Mining or Botanist nodes, too, by restricting the set.
    private static (ILocation?, TimeInterval) GetBestUptime(IEnumerable<GatheringNode> nodes, TimeStamp now)
    {
        var            uptime = TimeInterval.Never;
        GatheringNode? n      = null;
        foreach (var node in nodes)
        {
            Debug.Assert(!node.Times.AlwaysUp());
            var time = node.Times.NextUptime(now);
            if (!IntervalBetter(uptime, time, now))
                continue;

            uptime = time;
            n      = node;
        }

        return (n, uptime);
    }

    private static (ILocation?, TimeInterval) GetBestUptime(Fish f, IEnumerable<FishingSpot> spots, TimeStamp now)
    {
        var          uptime = TimeInterval.Never;
        FishingSpot? s      = null;
        foreach (var spot in spots)
        {
            Debug.Assert(spot.Items.Contains(f));
            var time = GetUptime(f, spot.Territory, now);
            if (!IntervalBetter(uptime, time, now))
                continue;

            uptime = time;
            s      = spot;
        }

        return (s, uptime);
    }

    // Check if the best uptime has not ended yet. If it has not, just return it.
    // If it has ended, compute the next best uptime and location and return that.
    private (ILocation, TimeInterval) UpdateUptime(IGatherable item)
    {
        var (loc, bestTime) = _bestUptime[item.InternalLocationId];
        if (bestTime.End < GatherBuddy.Time.ServerTime)
        {
            switch (item)
            {
                case Gatherable g: (loc, bestTime) = GetBestUptime(g.NodeList, GatherBuddy.Time.ServerTime); break;
                case Fish f:       (loc, bestTime) = GetBestUptime(f,          f.FishingSpots, GatherBuddy.Time.ServerTime); break;
            }

            Debug.Assert(loc != null);
            _bestUptime[item.InternalLocationId] = (loc, bestTime);
            UptimeChange?.Invoke(item);
        }

        return (loc, bestTime);
    }

    public (ILocation Location, TimeInterval Interval) BestLocation(IGatherable item)
        => item.InternalLocationId switch
        {
            > 0 => UpdateUptime(item),
            < 0 => (GetLocation(item), TimeInterval.Always),
            _   => (item.Locations.FirstOrDefault() ?? GatherBuddy.GameData.GatheringNodes.First().Value, TimeInterval.Always),
        };

    public TimeInterval NextUptime(Fish fish, Territory territory, TimeStamp now)
    {
        // Always up
        if (fish.InternalLocationId <= 0)
            return TimeInterval.Always;


        // Some spectral ocean fish have time restrictions (handled specially in GetUptime)
        // and some non-spectral ocean fish have weather restrictions (not handled).
        // Skip all of them here, as they should be considered AlwaysUp instead of Invalid.
        if (!fish.OceanFish)
        {
            // Invalid
            if (fish.FishRestrictions.HasFlag(FishRestrictions.Time) && fish.Interval.AlwaysUp())
                return TimeInterval.Invalid;

            // Unknown
            if (fish.FishRestrictions.HasFlag(FishRestrictions.Weather) && fish.PreviousWeather.Length == 0 && fish.CurrentWeather.Length == 0)
                return TimeInterval.Invalid;
        }

        return GetUptime(fish, territory, now);
    }

    public (ILocation? Location, TimeInterval interval) NextUptime(IGatherable item, TimeStamp now, IReadOnlyList<ILocation>? excludes = null)
    {
        if (item.InternalLocationId == 0)
            return (item.Locations.FirstOrDefault(), TimeInterval.Always);
        if (item.InternalLocationId < 0)
            return (FindClosestAetheryte(item, null, excludes), TimeInterval.Always);

        return item switch
        {
            Gatherable g => GetBestUptime(g.NodeList.Except(excludes ?? Array.Empty<ILocation>()).Cast<GatheringNode>(), now),
            Fish f       => GetBestUptime(f, f.FishingSpots.Except(excludes ?? Array.Empty<ILocation>()).Cast<FishingSpot>(), now),
            _            => throw new ArgumentException(),
        };
    }

    public (ILocation? Location, TimeInterval interval) NextUptime(Gatherable item, GatheringType type, TimeStamp now,
        IReadOnlyList<ILocation>? excludes = null)
    {
        if (item.InternalLocationId == 0)
            return (item.Locations.FirstOrDefault(), TimeInterval.Always);
        if (item.InternalLocationId < 0)
            return (FindClosestAetheryte(item, type), TimeInterval.Always);

        return GetBestUptime(
            item.NodeList.Where(n => n.GatheringType.ToGroup() == type).Except(excludes ?? Array.Empty<ILocation>()).Cast<GatheringNode>(),
            now);
    }


    // Tries to find the aetheryte with the lowest teleport cost that is available for this node.
    private ILocation? FindClosestAetheryteCost(IGatherable item, GatheringType? type = null, IReadOnlyList<ILocation>? excludes = null)
    {
        if (_currentTerritory == 0)
            return FindClosestAetheryteTravel(item, type, excludes);

        var enumerable = excludes != null ? item.Locations.Except(excludes) : item.Locations;
        enumerable = type == null
            ? enumerable.Where(l => l is FishingSpot || ((GatheringNode)l).Times.AlwaysUp())
            : enumerable.Where(l => l is GatheringNode n && n.GatheringType.ToGroup() == type);
        return enumerable
            .Where(a => a.ClosestAetheryte != null && Teleporter.IsAttuned(a.ClosestAetheryte.Id))
            .ArgMin(a => a.ClosestAetheryte!.AetherDistance(_aetherStreamX, _aetherStreamY, _aetherPlane));
    }

    // Tries to find the node with the closest available aetheryte in world distance.
    private static ILocation? FindClosestAetheryteTravel(IGatherable item, GatheringType? type = null,
        IReadOnlyList<ILocation>? excludes = null)
    {
        var enumerable = excludes == null ? item.Locations : item.Locations.Except(excludes);
        enumerable = type == null ? enumerable : enumerable.Where(l => l is GatheringNode n && n.GatheringType.ToGroup() == type);
        return enumerable
            .Where(l => l.ClosestAetheryte != null && Teleporter.IsAttuned(l.ClosestAetheryte.Id))
            .ArgMin(l => l.AetheryteDistance());
    }

    private ILocation? FindClosestAetheryte(IGatherable item, GatheringType? type = null, IReadOnlyList<ILocation>? excludes = null)
        => GatherBuddy.Config.AetherytePreference switch
        {
            AetherytePreference.Distance => FindClosestAetheryteTravel(item, type, excludes),
            AetherytePreference.Cost     => FindClosestAetheryteCost(item, type, excludes),
            _                            => throw new ArgumentException(),
        };

    // Get the aetherstream position of the player character, i.e. the aetherstream coordinates of the aetheryte corresponding to the current territory.
    private void SetCurrentTerritory(ushort territory)
    {
        if (territory == _currentTerritory)
            return;

        _currentTerritory = territory;
        var rawT = Dalamud.GameData.GetExcelSheet<TerritoryTypeTelepo>().GetRowOrDefault(territory);
        (_aetherStreamX, _aetherStreamY, _aetherPlane) =
            rawT == null ? ((ushort)0, (ushort)0, (ushort)0) : (rawT.Value.X, rawT.Value.Y, (ushort)rawT.Value.Relay.RowId);

        if (GatherBuddy.Config.AetherytePreference == AetherytePreference.Cost)
            ResetLocations();
    }
}
