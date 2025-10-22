using System;
using FFXIVClientStructs.FFXIV.Client.Game;
using GatherBuddy.Plugin;

namespace GatherBuddy.SeFunctions;


public sealed class EnhancedCurrentWeather
{

    public static unsafe byte GetCurrentWeatherId()
    {
        var weatherManager = WeatherManager.Instance();
        if (weatherManager == null)
            return 0;

        var territoryType = Dalamud.ClientState.TerritoryType;
        
        if (territoryType != 0 && weatherManager->HasIndividualWeather(territoryType))
        {
            var individualWeather = weatherManager->GetIndividualWeather(territoryType);
            return individualWeather;
        }
        
        var standardWeather = weatherManager->GetCurrentWeather();
        return standardWeather;
    }

    public static unsafe byte GetCurrentWeatherWithDebug()
    {
        var weatherManager = WeatherManager.Instance();
        if (weatherManager == null)
            return 0;

        var territoryType = Dalamud.ClientState.TerritoryType;
        var isInDiadem = Functions.InTheDiadem();
        
        var currentWeather = weatherManager->GetCurrentWeather();
        
        if (territoryType != 0)
        {
            var hasIndividual = weatherManager->HasIndividualWeather(territoryType);
            
            if (hasIndividual)
            {
                var individualWeather = weatherManager->GetIndividualWeather(territoryType);
                
                if (isInDiadem)
                {
                    return individualWeather;
                }
            }
        }
        
        return currentWeather;
    }
    

}