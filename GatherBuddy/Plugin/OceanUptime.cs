using GatherBuddy.Time;
using System;
using GatherBuddy.Classes;
using System.Linq;
using GatherBuddy.Enums;

namespace GatherBuddy.Plugin;

// A helper class for UptimeManager to deal with ocean fish uptime.
public static class OceanUptime
{
    public const ushort OceanFishingTerritoryId = 900;

    // The full loop of 144 timed routes at 2 hours each takes 12 days, beginning at this timestamp.
    public const long LoopTimestampEpoch       = 748800000;
    public const long TripDurationMilliseconds = 2 * RealTime.MillisecondsPerHour;

    public static readonly long LoopDurationMilliseconds = GatherBuddy.GameData.OceanRouteTimeline.Count * TripDurationMilliseconds;

    // Returns the current/next TimeInterval from a utc timestamp for this ocean fish.
    public static TimeInterval GetOceanUptime(Fish fish, TimeStamp utcNow)
    {
        // Only consider weather when on an ocean fishing boat.
        // The weather is random per zone, so you need to be there to see it.
        if (Dalamud.ClientState.TerritoryType == OceanFishingTerritoryId && fish.CurrentWeather.Length > 0)
        {
            var currentWeather = GatherBuddy.CurrentWeather.Current;

            if (!Array.Exists(fish.CurrentWeather, weather => weather.Id == currentWeather))
            {
                GatherBuddy.Log.Error($"{fish.Name.English} cw:{currentWeather} w:{string.Join(", ", fish.CurrentWeather.Select(x => x.Id))}");
                return TimeInterval.Invalid;
            }
        }

        if (fish.OceanTime == OceanTime.Always)
            return TimeInterval.Always;

        // The offset in milliseconds from when the current loop started to the current time.
        var loopOffsetMilliseconds = (utcNow.Time - LoopTimestampEpoch) % LoopDurationMilliseconds;
        // The index into RouteLoop for the given timestamp.
        var loopIdx = loopOffsetMilliseconds / TripDurationMilliseconds;
        // The timestamp that this loop started at.
        var loopStartTimestamp = utcNow.Time - loopOffsetMilliseconds;

        for (var loopEnd = GatherBuddy.GameData.OceanRouteTimeline.Count + loopIdx; loopIdx < loopEnd; ++loopIdx)
        {
            var route = GatherBuddy.GameData.OceanRouteTimeline[(int)(loopIdx % GatherBuddy.GameData.OceanRouteTimeline.Count)];
            foreach (var time in fish.OceanTime.Enumerate())
            {
                var (normal, spectral) = route.GetSpots(time);
                if (fish.FishingSpots.All(s => s != normal && s != spectral))
                    continue;

                var start = loopStartTimestamp + loopIdx * TripDurationMilliseconds;
                var end   = start + TripDurationMilliseconds;
                if (end < utcNow)
                    continue;

                return new TimeInterval(new TimeStamp(start), new TimeStamp(end));
            }
        }

        // The lookup above shouldn't fail unless data is wrong or something is awry.
        GatherBuddy.Log.Warning("Ocean fish was not found accessible in entire loop.");
        return TimeInterval.Always;
    }
}
