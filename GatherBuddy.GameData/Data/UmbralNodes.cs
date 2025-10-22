using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Structs;

namespace GatherBuddy.Data;

public static class UmbralNodes
{
    public enum UmbralWeatherType : uint
    {
        UmbralFlare = 133,
        UmbralDuststorms = 134,
        UmbralLevin = 135,
        UmbralTempest = 136,
    }

    public enum CloudedNodeType
    {
        CloudedRockyOutcrop,   
        CloudedMineralDeposit,   
        CloudedMatureTree,       
        CloudedLushVegetation,   
    }

    public static readonly (uint NodeId, CloudedNodeType NodeType, UmbralWeatherType Weather, uint[] ItemIds)[] UmbralNodeData =
    {
        (33836, CloudedNodeType.CloudedMineralDeposit, UmbralWeatherType.UmbralFlare, [29946, 31318, 32047]),
        (33838, CloudedNodeType.CloudedMatureTree, UmbralWeatherType.UmbralTempest, [29944, 31316, 32045]),
        (33837, CloudedNodeType.CloudedRockyOutcrop, UmbralWeatherType.UmbralLevin, [29947, 31319, 32048]),
        (33839, CloudedNodeType.CloudedLushVegetation, UmbralWeatherType.UmbralDuststorms, [29945, 31317, 32046]),
    };

    public static readonly Dictionary<CloudedNodeType, string> NodeNames = new()
    {
        { CloudedNodeType.CloudedRockyOutcrop, "Clouded Rocky Outcrop" },
        { CloudedNodeType.CloudedMineralDeposit, "Clouded Mineral Deposit" },
        { CloudedNodeType.CloudedMatureTree, "Clouded Mature Tree" },
        { CloudedNodeType.CloudedLushVegetation, "Clouded Lush Vegetation" },
    };

    public static GatheringType GetGatheringType(CloudedNodeType nodeType)
    {
        return nodeType switch
        {
            CloudedNodeType.CloudedRockyOutcrop => GatheringType.Miner,
            CloudedNodeType.CloudedMineralDeposit => GatheringType.Miner,
            CloudedNodeType.CloudedMatureTree => GatheringType.Botanist,
            CloudedNodeType.CloudedLushVegetation => GatheringType.Botanist,
            _ => throw new ArgumentOutOfRangeException(nameof(nodeType), nodeType, null)
        };
    }

    public static bool IsUmbralWeather(uint weatherId)
    {
        return Enum.IsDefined(typeof(UmbralWeatherType), weatherId);
    }

    public static IEnumerable<uint> GetNodesForWeatherAndType(UmbralWeatherType weather, GatheringType gatheringType)
    {
        return UmbralNodeData
            .Where(entry => entry.Weather == weather && GetGatheringType(entry.NodeType) == gatheringType)
            .Select(entry => entry.NodeId);
    }

    public static uint[] GetItemsForNode(uint nodeId)
    {
        var entry = UmbralNodeData.FirstOrDefault(data => data.NodeId == nodeId);
        return entry.ItemIds ?? [];
    }

    public static UmbralWeatherType? GetRequiredWeatherForNode(uint nodeId)
    {
        var entry = UmbralNodeData.FirstOrDefault(data => data.NodeId == nodeId);
        return entry.NodeId == 0 ? null : entry.Weather;
    }

    public static CloudedNodeType? GetNodeType(uint nodeId)
    {
        var entry = UmbralNodeData.FirstOrDefault(data => data.NodeId == nodeId);
        return entry.NodeId == 0 ? null : entry.NodeType;
    }

    public static void Apply(GameData data)
    {
        var validItems = 0;
        var validWeatherPositions = 0;
        
        data.Log.Information($"Validating umbral system with {UmbralNodeData.Length} configurations...");
        
        foreach (var (nodeId, nodeType, weather, itemIds) in UmbralNodeData)
        {
            data.Log.Debug($"Validating umbral configuration {nodeId} ({nodeType}) for {weather} weather");
            
            foreach (var itemId in itemIds)
            {
                if (data.Gatherables.TryGetValue(itemId, out var item))
                {
                    data.Log.Information($"✓ Umbral item found: {item.Name[data.DataManager.Language]} (ID: {itemId})");
                    validItems++;
                }
                else
                {
                    data.Log.Warning($"✗ Umbral item {itemId} not found in game data");
                }
            }
            
            if (data.WorldCoords.TryGetValue(nodeId, out var positions) && positions.Count > 0)
            {
                data.Log.Information($"✓ Found {positions.Count} world positions for umbral node {nodeId}");
                validWeatherPositions++;
            }
            else
            {
                data.Log.Warning($"✗ No world positions found for umbral node {nodeId}");
            }
        }
        
        data.Log.Information($"Umbral validation complete. {validItems} valid items, {validWeatherPositions} nodes with positions.");
        data.Log.Information($"Umbral items will be handled by AutoGather umbral logic.");
    }
    
    public static bool IsUmbralItem(uint itemId)
    {
        return UmbralNodeData.Any(entry => entry.ItemIds.Contains(itemId));
    }
    
    public static (uint NodeId, CloudedNodeType NodeType, UmbralWeatherType Weather)? GetUmbralItemInfo(uint itemId)
    {
        var entry = UmbralNodeData.FirstOrDefault(data => data.ItemIds.Contains(itemId));
        return entry.NodeId == 0 ? null : (entry.NodeId, entry.NodeType, entry.Weather);
    }
}
