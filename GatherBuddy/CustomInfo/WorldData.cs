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

            // Check if the file exists
            if (!File.Exists(path))
            {
                // Load the embedded resource
                var assembly = typeof(GatherBuddy).Assembly;
                var resourceName = "GatherBuddy.CustomInfo.world_locations.json";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                        throw new FileNotFoundException("Embedded resource not found.", resourceName);

                    using (var reader = new StreamReader(stream))
                    {
                        var defaultContent = reader.ReadToEnd();
                        File.WriteAllText(path, defaultContent);
                    }
                }
            }

            // Read the content of the file
            var locJson = File.ReadAllText(path);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<uint, List<Vector3>>>(locJson);
            WorldLocationsByNodeId = obj ?? new();
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
