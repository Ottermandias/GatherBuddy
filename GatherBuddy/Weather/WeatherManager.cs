using System;
using System.Collections.Generic;
using System.Linq;
using GatherBuddy.Classes;
using GatherBuddy.Time;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Weather;

public partial class WeatherManager
{
    public Dictionary<uint, WeatherTimeline> Forecast    { get; }
    public List<WeatherTimeline>             UniqueZones { get; } = new();


    public WeatherManager(GameData data)
    {
        Forecast = data.WeatherTerritories.ToDictionary(t => t.Id, t => new WeatherTimeline(t));
        foreach (var t in Forecast.Values.Where(t => UniqueZones.All(l => l.Territory.Name != t.Territory.Name)))
            UniqueZones.Add(t);
    }

    private WeatherTimeline FindOrCreateForecast(Territory territory, uint increment)
    {
        if (Forecast.TryGetValue(territory.Id, out var values))
            return values;

        var timeline = new WeatherTimeline(territory, increment);
        Forecast[territory.Id] = timeline;
        return timeline;
    }

    public WeatherTimeline RequestForecast(Territory territory, uint amount)
    {
        var list = FindOrCreateForecast(territory, amount);
        return list.Update(amount);
    }

    public (Structs.Weather Last, Structs.Weather Current, Structs.Weather Next) FindLastCurrentNextWeather(uint territoryId)
    {
        if (Forecast.TryGetValue(territoryId, out var timeline))
        {
            timeline.Update(3);
            return (timeline.LastWeather.Weather, timeline.CurrentWeather.Weather, timeline.List[2].Weather);
        }

        var territory = GatherBuddy.GameData.FindOrAddTerritory(Dalamud.GameData.GetExcelSheet<TerritoryType>()?.GetRow(territoryId));
        if (territory == null || territory.WeatherRates.Rates.Length != 1 || territory.WeatherRates.Rates[0].CumulativeRate != 100)
            return (Structs.Weather.Invalid, Structs.Weather.Invalid, Structs.Weather.Invalid);

        return (territory.WeatherRates.Rates[0].Weather, territory.WeatherRates.Rates[0].Weather, territory.WeatherRates.Rates[0].Weather);
    }

    public WeatherListing RequestForecast(Territory territory, IList<Structs.Weather> weather, TimeStamp now, uint increment = 32)
        => RequestForecast(territory, weather, Array.Empty<Structs.Weather>(), RepeatingInterval.Always, now, increment);

    public WeatherListing RequestForecast(Territory territory, IList<Structs.Weather> weather, RepeatingInterval eorzeanHours, TimeStamp now,
        uint increment = 32)
        => RequestForecast(territory, weather, Array.Empty<Structs.Weather>(), eorzeanHours, now, increment);


    public WeatherListing RequestForecast(Territory territory, IList<Structs.Weather> weather, IList<Structs.Weather> previousWeather,
        RepeatingInterval eorzeanHours, TimeStamp now, uint increment = 32)
    {
        var values = FindOrCreateForecast(territory, increment);
        return values.Find(weather, previousWeather, eorzeanHours, now, increment);
    }

    public TimeStamp ExtendedDuration(Territory territory, IList<Structs.Weather> weather, IList<Structs.Weather> previousWeather,
        WeatherListing listing, uint increment = 32)
    {
        var checkWeathers = weather.Any();
        var checkPrevious = previousWeather.Any();
        if (!checkWeathers && !checkPrevious)
            return TimeStamp.MaxValue;

        var duration = listing.End;
        if (checkPrevious && previousWeather.All(w => w.Id != listing.Weather.Id))
            return duration;

        var values = FindOrCreateForecast(territory, increment);
        values.TrimFront();
        var idx = values.FindIndex(listing);
        if (idx < 0)
            return duration;

        for (var sanityStop = 0; sanityStop < 24; ++sanityStop)
        {
            if (checkPrevious && previousWeather.All(w => w.Id != listing.Weather.Id))
                return duration;

            if (idx == values.List.Count - 1)
                values.Append(increment);
            listing = values.List[++idx];
            if (checkWeathers && weather.All(w => w.Id != listing.Weather.Id))
                return duration;

            duration += EorzeaTimeStampExtensions.MillisecondsPerEorzeaWeather;
        }

        return duration;
    }
}
