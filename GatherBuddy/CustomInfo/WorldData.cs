using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GatherBuddy.CustomInfo
{
    public static class WorldData
    {
        private static string WorldLocationsPath = Path.Combine(Dalamud.PluginInterface.ConfigDirectory.FullName, "world_locations.json");
        private static string NodeOffsetsPath = Path.Combine(Dalamud.PluginInterface.ConfigDirectory.FullName, "node_offsets.json");
        public static Dictionary<uint, List<Vector3>> WorldLocationsByNodeId { get; set; } = new();
        public static Dictionary<Vector3, Vector3>    NodeOffsets            { get; set; } = new();
        public static ReadOnlyCollection<(ushort BaseGathering, ushort BasePerception)> IlvConvertTable { get; private set; }

        static WorldData()
        {
            LoadLocationsFromFile();
            LoadOffsetsFromFile();
            LoadIlvConvertTableFromFile();
        }

        private static void LoadOffsetsFromFile()
        {
            var settings = new JsonSerializerSettings();
            var resourceName = "GatherBuddy.CustomInfo.node_offsets.json";
            NodeOffsets = LoadFromFile<List<OffsetPair>>(NodeOffsetsPath, resourceName, settings).GroupBy(x => x.Original).ToDictionary(x => x.Key, x => x.First().Offset);
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
            return existingData.Concat(defaultData).GroupBy(v => v.Original).Select(v => v.First()).ToList();
        }

        public static void AddOffset(Vector3 original, Vector3 offset)
        {
            if (NodeOffsets.ContainsKey(original))
            {
                GatherBuddy.Log.Error($"{original} already has an offset of {NodeOffsets[original]}. Unable to add new offset.");
                return;
            }
            
            NodeOffsets[original] = offset;
            Task.Run(SaveOffsetsToFile);
        }

        public static void SaveOffsetsToFile()
        {
            var settings = new JsonSerializerSettings();
            var offsetJson = Newtonsoft.Json.JsonConvert.SerializeObject(NodeOffsets.Select(x => new OffsetPair(x.Key, x.Value)).ToList(), Formatting.Indented, settings);
            
            File.WriteAllText(NodeOffsetsPath, offsetJson);
        }

        public static void AddLocation(uint nodeId, Vector3 location)
        {
            if (!WorldLocationsByNodeId.TryGetValue(nodeId, out var list))
            {
                lock (WorldLocationsByNodeId)
                    WorldLocationsByNodeId[nodeId] = list = [];
                foreach (var node in GatherBuddy.GameData.GatheringNodes.Values.Where(v => v.WorldPositions.ContainsKey(nodeId)))
                    node.WorldPositions[nodeId] = list;
            }
                
            if (!list.Contains(location))
            {
                lock (WorldLocationsByNodeId)
                    list.Add(location);

                Task.Run(() => { lock (WorldLocationsByNodeId) SaveLocationsToFile(); });
                GatherBuddy.Log.Debug($"Added location {location} to node {nodeId}");
                WorldLocationsChanged?.Invoke();
            }
        }

        public static event Action? WorldLocationsChanged;

        public static void SaveLocationsToFile()
        {
            var locJson = Newtonsoft.Json.JsonConvert.SerializeObject(WorldLocationsByNodeId, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(WorldLocationsPath, locJson);
        }

        [MemberNotNull(nameof(IlvConvertTable))]
        private static unsafe void LoadIlvConvertTableFromFile()
        {
            var resourceName = "GatherBuddy.CustomInfo.IlvConvertTable.csv";
            var assembly = typeof(GatherBuddy).Assembly;
            var resource = assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException("Embedded resource not found.", resourceName);
            var stream = new StreamReader(resource);

            stream.ReadLine();
            var list = new List<(ushort, ushort)>(1000);
            while (stream.ReadLine() is string line and { Length: > 5 })
            {
                Span<ushort> values = [.. line.Split(',').Select(ushort.Parse)];
                while (list.Count < values[0] + 1) list.Add((0, 0));
                list[values[0]] = (values[1], values[2]);
            }
            IlvConvertTable = new([.. list]);
        }
    }

    public record struct OffsetPair(Vector3 Original, Vector3 Offset)
    {
    }
}
