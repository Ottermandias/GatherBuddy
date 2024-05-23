using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GatherBuddy.CustomInfo
{
    public static class WorldData
    {
        public static Dictionary<uint, List<Vector3>> WorldLocationsByNodeId { get; set; } = new();

        static WorldData()
        {
            LoadLocationsFromFile();
        }

        private static void LoadLocationsFromFile()
        {
            var path = Path.Combine(Dalamud.PluginInterface.ConfigDirectory.FullName, "world_locations.json");
            var assembly = typeof(GatherBuddy).Assembly;
            var resourceName = "GatherBuddy.CustomInfo.world_locations.json";

            Dictionary<uint, List<Vector3>> defaultObj;
            // Load the embedded resource
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new FileNotFoundException("Embedded resource not found.", resourceName);

                using (var reader = new StreamReader(stream))
                {
                    var defaultContent = reader.ReadToEnd();
                    defaultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<uint, List<Vector3>>>(defaultContent);
                }
            }

            // Check if the file exists
            if (!File.Exists(path))
            {
                File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(defaultObj, Newtonsoft.Json.Formatting.Indented));
            }
            else
            {
                var fileContent = File.ReadAllText(path);
                var existingObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<uint, List<Vector3>>>(fileContent) ?? new Dictionary<uint, List<Vector3>>();

                // Append new information from the default file to the existing file
                foreach (var kvp in defaultObj)
                {
                    if (existingObj.TryGetValue(kvp.Key, out var existingList))
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
                        existingObj[kvp.Key] = new List<Vector3>(kvp.Value);
                    }
                }

                File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(existingObj, Newtonsoft.Json.Formatting.Indented));
            }

            // Read the content of the file
            var locJson = File.ReadAllText(path);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<uint, List<Vector3>>>(locJson);
            WorldLocationsByNodeId = obj ?? new Dictionary<uint, List<Vector3>>();
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
                SaveLocationsToFile();
                GatherBuddy.Log.Debug($"Added location {location} to node {nodeId}");
            }
        }

        public static void SaveLocationsToFile()
        {
            var locJson = Newtonsoft.Json.JsonConvert.SerializeObject(WorldLocationsByNodeId, Newtonsoft.Json.Formatting.Indented);
            var path = Path.Combine(Dalamud.PluginInterface.ConfigDirectory.FullName, "world_locations.json");

            File.WriteAllText(path, locJson);
        }
    }
}
