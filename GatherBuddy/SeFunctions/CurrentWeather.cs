using Dalamud.Game;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Plugin;

namespace GatherBuddy.SeFunctions;

public sealed class CurrentWeather : SeAddressBase
{
    // Discovery notes:
    //
    // Weather is a one byte value in memory that corresponds to the Weather table.
    // There are duplicate names in that table, but all of the ARR overworld zones use
    // the first weather value of that type, usually 1-10.  There's ~4 static memory locations.
    // One of these locations is the predicted weather (precalculated based on zone and time)
    // and the other three are the true weather (based on in-game compass / zone effects / etc).
    // You can use Bismark hard mode to differentiate these, as Bismark's weather
    // effects only affect the true weather.
    public CurrentWeather(ISigScanner sigScanner)
        : base(sigScanner, "48 8B 05 ?? ?? ?? ?? 0F B6 12", 8)
    { }

    public unsafe byte Current
    {
        get
        {
            if (Functions.InTheDiadem())
            {
                var weatherManager = WeatherManager.Instance();
                if (weatherManager != null)
                {
                    return weatherManager->GetCurrentWeather();
                }
            }
            
            // normal detection for non-Diadem
            return *(byte*)Address;
        }
    }
}
