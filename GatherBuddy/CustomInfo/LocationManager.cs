using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using Newtonsoft.Json;

namespace GatherBuddy.CustomInfo;

public class LocationManager
{
    public readonly List<ILocation> AllLocations = GatherBuddy.GameData.GatheringNodes.Values
        .Cast<ILocation>()
        .Concat(GatherBuddy.GameData.FishingSpots.Values)
        .ToList();

    public const string FileName = "custom_locations.json";

    private static bool HasCustomization(ILocation loc)
        => !ReferenceEquals(loc.ClosestAetheryte, loc.DefaultAetheryte)
         || loc.IntegralXCoord != loc.DefaultXCoord
         || loc.IntegralYCoord != loc.DefaultYCoord;

    public IEnumerable<LocationData> CustomLocations 
        => AllLocations.Where(HasCustomization).Select(l => new LocationData(l));


    private string CustomLocationData()
        => JsonConvert.SerializeObject(CustomLocations);

    public void SetXCoord(ILocation loc, int newCoord)
    {
        if (loc.SetXCoord(newCoord))
            Save();
    }

    public void SetYCoord(ILocation loc, int newCoord)
    {
        if (loc.SetYCoord(newCoord))
            Save();
    }

    public void SetAetheryte(ILocation loc, Aetheryte? newAetheryte)
    {
        if (loc.SetAetheryte(newAetheryte))
            Save();
    }

    public void Save()
    {
        var file = Functions.ObtainSaveFile(FileName);
        if (file == null)
            return;
        
        try
        {
            var text = CustomLocationData();
            File.WriteAllText(file.FullName, text);
        }
        catch (Exception e)
        {
            PluginLog.Error($"Could not write custom locations to file {file.FullName}:\n{e}");
        }
    }

    public static LocationManager Load()
    {
        var             file = Functions.ObtainSaveFile(FileName);
        LocationManager ret  = new();
        if (file is not { Exists: true })
        {
            ret.Save();
            return ret;
        }

        try
        {
            var changes   = false;
            var text      = File.ReadAllText(file.FullName);
            var locations = JsonConvert.DeserializeObject<LocationData[]>(text);
            foreach (var location in locations)
            {
                ILocation? loc = location.Type switch
                {
                    ObjectType.Gatherable => GatherBuddy.GameData.GatheringNodes.TryGetValue(location.Id, out var l) ? l : null,
                    ObjectType.Fish       => GatherBuddy.GameData.FishingSpots.TryGetValue(location.Id, out var l) ? l : null,
                    _                     => null,
                };
                if (loc == null)
                {
                    PluginLog.Error($"Invalid custom location {location.Id} of type {location.Type}, skipped.");
                    changes = true;
                    continue;
                }


                Aetheryte? aetheryte = null;
                if (location.AetheryteId != -1)
                {
                    if (!GatherBuddy.GameData.Aetherytes.TryGetValue((uint)location.AetheryteId, out aetheryte))
                    {
                        PluginLog.Error($"Invalid aetheryte id {location.AetheryteId} in custom location for {loc.Name}.");
                        changes = true;
                        continue;
                    }
                }

                changes |= !loc.SetAetheryte(aetheryte);
                changes |= !loc.SetXCoord(location.XCoord);
                changes |= !loc.SetYCoord(location.YCoord);
            }

            if (changes)
                ret.Save();
        }
        catch (Exception e)
        {
            PluginLog.Error($"Error loading custom infos:\n{e}");
        }

        return ret;
    }
}
