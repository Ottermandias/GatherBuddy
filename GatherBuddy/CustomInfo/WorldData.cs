using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GatherBuddy.CustomInfo
{
    public static class WorldData
    {
        private static string WorldLocationsPath = Path.Combine(Dalamud.PluginInterface.ConfigDirectory.FullName, "world_locations.json");
        private static string NodeOffsetsPath = Path.Combine(Dalamud.PluginInterface.ConfigDirectory.FullName, "node_offsets.json");
        public static Dictionary<uint, List<Vector3>> WorldLocationsByNodeId { get; set; } = new();
        public static List<OffsetPair>    NodeOffsets            { get; set; } = new();

        static WorldData()
        {
            LoadLocationsFromFile();
            LoadOffsetsFromFile();
        }

        private static void LoadOffsetsFromFile()
        {
            var settings = new JsonSerializerSettings();
            var resourceName = "GatherBuddy.CustomInfo.node_offsets.json";
            NodeOffsets = LoadFromFile<List<OffsetPair>>(NodeOffsetsPath, resourceName, settings);
        }

        private static void LoadLocationsFromFile()
        {
            var resourceName = "GatherBuddy.CustomInfo.world_locations.json";
            WorldLocationsByNodeId = LoadFromFile<Dictionary<uint, List<Vector3>>>(WorldLocationsPath, resourceName, new JsonSerializerSettings());
        }

        private static T LoadFromFile<T>(string path, string resourceName, JsonSerializerSettings settings)
        {
            var assembly = typeof(GatherBuddy).Assembly;
            T   defaultObj;

            // Load the embedded resource
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new FileNotFoundException("Embedded resource not found.", resourceName);

                using (var reader = new StreamReader(stream))
                {
                    var defaultContent = reader.ReadToEnd();
                    defaultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(defaultContent, settings);
                }
            }

            // Check if the file exists
            if (!File.Exists(path))
            {
                File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(defaultObj, Newtonsoft.Json.Formatting.Indented, settings));
            }
            else
            {
                var fileContent = File.ReadAllText(path);
                var existingObj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(fileContent, settings)
                 ?? Activator.CreateInstance<T>();

                // Depending on the type of T, you might need to perform different operations here
                // In this case we assume T is a Dictionary<uint, List<Vector3>>, but for other types T you might need other merge operations 
                if (defaultObj is Dictionary<uint, List<Vector3>> defaultDict1 && existingObj is Dictionary<uint, List<Vector3>> existingDict1)
                {
                    File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(MergeData(defaultDict1, existingDict1), Newtonsoft.Json.Formatting.Indented, settings));
                }
                else if (defaultObj is List<OffsetPair> defaultDict2 && existingObj is List<OffsetPair> existingDict2)
                {
                    File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(MergeData(defaultDict2, existingDict2), Newtonsoft.Json.Formatting.Indented, settings));
                }
            }

            // Read the content of the file
            var locJson = File.ReadAllText(path);
            var obj     = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(locJson, settings);
            return obj ?? Activator.CreateInstance<T>();
        }

        private static Dictionary<uint,List<Vector3>> MergeData(Dictionary<uint, List<Vector3>> defaultData, Dictionary<uint, List<Vector3>> existingData)
        {
            foreach (var kvp in defaultData)
            {
                if (existingData.TryGetValue(kvp.Key, out var existingList))
                {
                    foreach (var vector in kvp.Value)
                    {
                        if (!existingList.Contains(vector))
                        {
                            existingList.Add(vector);
                        }
                    }
                }
                else
                {
                    existingData[kvp.Key] = new List<Vector3>(kvp.Value);
                }
            }

            return existingData;
        }

        private static List<OffsetPair> MergeData(List<OffsetPair> defaultData, List<OffsetPair> existingData)
        {
            var mergedData = existingData.GroupBy(v => v.Original).ToDictionary(v => v.Key, v => v.First());
            foreach (var item in defaultData)
            {
                if (!mergedData.ContainsKey(item.Original))
                {
                    mergedData[item.Original] = item;
                }
            }

            return mergedData.Values.ToList();
        }

        public static void AddOffset(Vector3 original, Vector3 offset)
        {
            if (NodeOffsets.Any(o => o.Original == original))
            {
                var nodeOffset = NodeOffsets.First(o => o.Original == original).Offset;
                GatherBuddy.Log.Error($"{original} already has an offset of {nodeOffset}. Unable to add new offset.");
                return;
            }
            
            NodeOffsets.Add(new OffsetPair(original, offset));
            Task.Run(SaveOffsetsToFile);
        }

        public static void SaveOffsetsToFile()
        {
            var settings = new JsonSerializerSettings();
            var offsetJson = Newtonsoft.Json.JsonConvert.SerializeObject(NodeOffsets, Formatting.Indented, settings);
            
            File.WriteAllText(NodeOffsetsPath, offsetJson);
        }

        public static void AddLocation(uint nodeId, Vector3 location)
        {
            if (!WorldLocationsByNodeId.ContainsKey(nodeId))
            {
                WorldLocationsByNodeId[nodeId] = new List<Vector3>();
            }

            if (!WorldLocationsByNodeId[nodeId].Contains(location))
            {
                WorldLocationsByNodeId[nodeId].Add(location);
                Task.Run(SaveLocationsToFile);
                GatherBuddy.Log.Debug($"Added location {location} to node {nodeId}");
            }
        }

        public static void SaveLocationsToFile()
        {
            var locJson = Newtonsoft.Json.JsonConvert.SerializeObject(WorldLocationsByNodeId, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(WorldLocationsPath, locJson);
        }
    }

    public class OffsetPair(Vector3 original, Vector3 offset)
    {
        public Vector3 Original { get; set; } = original;
        public Vector3 Offset   { get; set; } = offset;
    }
}
