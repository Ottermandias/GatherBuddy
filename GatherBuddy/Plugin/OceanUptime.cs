using System.Collections.Generic;
using System.Diagnostics;
using GatherBuddy.Time;
using System;
using GatherBuddy.Classes;
using System.Linq;
using GatherBuddy.Enums;

namespace GatherBuddy.Plugin;

// A helper class for UptimeManager to deal with ocean fish uptime.
public static class OceanUptime
{
    private enum OceanRoute
    {
        BloodbrineSea,
        RothlytSound,
        NorthernStraitofMerlthor,
        RhotanoSea,
    }

    // A named enum with FishingSpot.Id values for convenience.
    private enum OceanStop
    {
        TheCieldalaes = 246,
        TheNorthernStraitOfMerlthor = 243,
        TheBloodbrineSea = 248,
        RhotanoSea = 241,
        TheRothlytSound = 250,
        TheSouthernStraitOfMerlthor = 239,
        GaladionBay = 237,

        TheCieldalaesSpectralCurrent = 247,
        TheNorthernStraitOfMerlthorSpectralCurrent = 244,
        TheBloodbrineSeaSpectralCurrent = 249,
        RhotanoSeaSpectralCurrent = 242,
        TheRothlytSoundSpectralCurrent = 251,
        TheSouthernStraitOfMerlthorSpectralCurrent = 240,
        GaladionBaySpectralCurrent = 238,
    }

    // The spots are ordered the same way they are in game.
    // If this is Bloodbrine Day, then the progression is Cieldales Sunset, Northern Strait Night, Bloodbrine Day.
    private static readonly Dictionary<OceanRoute, OceanStop[]> RouteToStops = new Dictionary<OceanRoute, OceanStop[]>()
    {
        {
            OceanRoute.BloodbrineSea,
            new OceanStop[6]
            {
                OceanStop.TheCieldalaes,
                OceanStop.TheCieldalaesSpectralCurrent,
                OceanStop.TheNorthernStraitOfMerlthor,
                OceanStop.TheNorthernStraitOfMerlthorSpectralCurrent,
                OceanStop.TheBloodbrineSea,
                OceanStop.TheBloodbrineSeaSpectralCurrent,
            }
        },
        {
            OceanRoute.RothlytSound,
            new OceanStop[6]
            {
                OceanStop.TheCieldalaes,
                OceanStop.TheCieldalaesSpectralCurrent,
                OceanStop.RhotanoSea,
                OceanStop.RhotanoSeaSpectralCurrent,
                OceanStop.TheRothlytSound,
                OceanStop.TheRothlytSoundSpectralCurrent,
            }
        },
        {
            OceanRoute.NorthernStraitofMerlthor,
            new OceanStop[6]
            {
                OceanStop.TheSouthernStraitOfMerlthor,
                OceanStop.TheSouthernStraitOfMerlthorSpectralCurrent,
                OceanStop.GaladionBay,
                OceanStop.GaladionBaySpectralCurrent,
                OceanStop.TheNorthernStraitOfMerlthor,
                OceanStop.TheNorthernStraitOfMerlthorSpectralCurrent,
            }
        },
        {
            OceanRoute.RhotanoSea,
            new OceanStop[6]
            {
                OceanStop.GaladionBay,
                OceanStop.GaladionBaySpectralCurrent,
                OceanStop.TheSouthernStraitOfMerlthor,
                OceanStop.TheSouthernStraitOfMerlthorSpectralCurrent,
                OceanStop.RhotanoSea,
                OceanStop.RhotanoSeaSpectralCurrent,
            }
        },
    };

    // Convenience function to return the subsequent OceanTime.
    private static OceanTime NextTime(OceanTime time) {
        return (OceanTime)(((int)time + 1) % 3);
    }

    private class TimedRoute
    {
        public OceanRoute Route;
        public OceanTime Time;

        public Dictionary<OceanTime, OceanStop[]> StopsByTime = new Dictionary<OceanTime, OceanStop[]>();

        public TimedRoute(OceanRoute route, OceanTime time)
        {
            Route = route;
            Time = time;

            var stops = RouteToStops[route];

            // Map all of the stops on the route by time for lookup later.
            var firstTime = NextTime(time);
            StopsByTime[firstTime] = new OceanStop[] { stops[0], stops[1] };
            var secondTime = NextTime(firstTime);
            StopsByTime[secondTime] = new OceanStop[] { stops[2], stops[3] };
            var thirdTime = NextTime(secondTime);
            StopsByTime[thirdTime] = new OceanStop[] { stops[4], stops[5] };
        }
    }

    // The loop of 144 TimedRoutes.
    private static readonly TimedRoute[] RouteLoop;

    // The full loop of 144 timed routes at 2 hours each takes 12 days, beginning at this timestamp.
    private static long LoopTimestampEpoch = 748800000;
    private static long LoopDurationMilliseconds = 12 * 24 * 60 * 60 * 1000;
    private static long OneHourInMilliseconds = 60 * 60 * 1000;
    private static long TwoHoursInMilliseconds = 2 * OneHourInMilliseconds;

    private static TimedRoute[] BuildRouteLoop()
    {
        var fixedRouteOrder = new TimedRoute[12]
        {
            new TimedRoute(OceanRoute.BloodbrineSea, OceanTime.Sunset),
            new TimedRoute(OceanRoute.RothlytSound, OceanTime.Sunset),
            new TimedRoute(OceanRoute.NorthernStraitofMerlthor, OceanTime.Sunset),
            new TimedRoute(OceanRoute.RhotanoSea, OceanTime.Sunset),

            new TimedRoute(OceanRoute.BloodbrineSea, OceanTime.Night),
            new TimedRoute(OceanRoute.RothlytSound, OceanTime.Night),
            new TimedRoute(OceanRoute.NorthernStraitofMerlthor, OceanTime.Night),
            new TimedRoute(OceanRoute.RhotanoSea, OceanTime.Night),

            new TimedRoute(OceanRoute.BloodbrineSea, OceanTime.Day),
            new TimedRoute(OceanRoute.RothlytSound, OceanTime.Day),
            new TimedRoute(OceanRoute.NorthernStraitofMerlthor, OceanTime.Day),
            new TimedRoute(OceanRoute.RhotanoSea, OceanTime.Day),
        };

        var loop = new TimedRoute[144];

        int outputIdx = 0;

        // The full fixed route first (-1), and then the fixed route
        // but removing one item (in order) each time, for a total of
        // 12 + 11 * 12 = 144 items in the loop.
        for (int skipIdx = -1; skipIdx < fixedRouteOrder.Length; skipIdx++)
        {
            for (int routeIdx = 0; routeIdx < fixedRouteOrder.Length; routeIdx++)
            {
                if (routeIdx == skipIdx)
                    continue;
                loop[outputIdx] = fixedRouteOrder[routeIdx];
                outputIdx++;
            }
        }

        Debug.Assert(outputIdx == 144);
        return loop;
    }


    // Returns the current/next TimeInterval from a utc timestamp for this ocean fish.
    public static TimeInterval GetOceanUptime(Fish fish, TimeStamp utcNow)
    {
        if (!fish.OceanFish || fish.OceanTimes.Length == 0)
            return TimeInterval.Always;

        // The offset in seconds from when the current loop started to the current time.
        long loopOffsetMilliseconds = (utcNow.Time - LoopTimestampEpoch) % LoopDurationMilliseconds;
        // The index into RouteLoop for the current timestamp.
        var loopIdx = loopOffsetMilliseconds / TwoHoursInMilliseconds;
        // The timestamp that this loop started.
        long loopStartTimestamp = utcNow.Time - loopOffsetMilliseconds;

        var spotIds = fish.FishingSpots.Select(spot => spot.Id);

        // Every stop/time combination exists in at least two adjacent fixed routes (since it might
        // be skipped in one of the routes, but can't be skipped twice), so this will need to check
        // ~22 routes at most.
        for (int loopCount = 0; loopCount < 25; loopCount++)
        {
            var timedRoute = RouteLoop[loopIdx % RouteLoop.Length];
            foreach (var time in fish.OceanTimes)
            {
                if (!Array.Exists(timedRoute.StopsByTime[time], spotId => spotIds.Contains((uint)spotId)))
                    continue;

                // Arbitrarily say that the window is an hour long rather than two.
                // Boats can queue up to 15 minutes past the hour, and boats are roughly 25 minutes.
                // There can be delay for cutscenes/players loading as well.  An hour is safe.
                var start = loopStartTimestamp + loopIdx * TwoHoursInMilliseconds;
                var end = start + OneHourInMilliseconds;
                if (end < utcNow)
                    continue;
                return new TimeInterval(new TimeStamp(start), new TimeStamp(end));
            }

            loopIdx++;
        }

        // The lookup above shouldn't fail unless data is wrong or something is awry.
        return TimeInterval.Always;
    }

    static OceanUptime()
    {
        RouteLoop = BuildRouteLoop();
    }
}