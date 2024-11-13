using System.Linq;
using Lumina.Excel.Sheets;

namespace GatherBuddy.Structs;

public readonly struct CumulativeWeatherRates
{
    public static readonly CumulativeWeatherRates InvalidWeather = new(false);

    public readonly (Weather Weather, byte CumulativeRate)[] Rates;

    public CumulativeWeatherRates(GameData data, in WeatherRate rate)
    {
        Rates = rate.Rate.Zip(rate.Weather).Where(w => w.First > 0)
            .Select(w => data.Weathers.TryGetValue(w.Second.RowId, out var weather)
                ? (weather, w.First)
                : (Weather.Invalid, w.First))
            .ToArray();
        byte lastRate = 0;
        for (var i = 0; i < Rates.Length; ++i)
        {
            if (Rates[i].Weather.Id == Weather.Invalid.Id)
                data.Log.Error("Invalid Weather requested.");
            Rates[i].CumulativeRate += lastRate;
            lastRate                =  Rates[i].CumulativeRate;
        }
    }

    private CumulativeWeatherRates(bool _)
        => Rates = [];

    public byte ChanceForWeather(params Weather[] weathers)
    {
        if (weathers.Length == 0)
            return 100;

        var summedChance = 0;
        foreach (var weather in weathers)
        {
            for (var i = 0; i < Rates.Length; ++i)
            {
                if (Rates[i].Weather.Id != weather.Id)
                    continue;

                if (i == 0)
                    summedChance += Rates[i].CumulativeRate;
                else
                    summedChance += Rates[i].CumulativeRate - Rates[i - 1].CumulativeRate;
            }
        }

        return (byte)summedChance;
    }
}
