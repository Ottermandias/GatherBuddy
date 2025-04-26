using System.Text.Json.Serialization;
using GatherBuddy.Models;
using Newtonsoft.Json;

namespace GatherBuddy.DataImport;

class Program
{
    private const string OutputPath      = "../../../../GatherBuddy/CustomInfo";
    private const string FishRecordsFile = "fish_records.json";
    static void Main(string[] args)
    {
        var input = args[0];
        if (!File.Exists(input))
            throw new FileNotFoundException("Input file not found.", input);
        var output = Path.Combine(OutputPath, FishRecordsFile);

        var oldFishJson = File.ReadAllText(output);
        var newFishJson = File.ReadAllText(input);

        var oldFishRecords = JsonConvert.DeserializeObject<List<SimpleFishRecord>>(oldFishJson);
        var newFishRecords = JsonConvert.DeserializeObject<List<SimpleFishRecord>>(newFishJson);

        if (oldFishRecords == null || newFishRecords == null)
            throw new Exception("Failed to deserialize json.");

        var fishRecords = newFishRecords.Union(oldFishRecords).ToList();
        File.WriteAllText(output, JsonConvert.SerializeObject(fishRecords, Formatting.Indented));
    }
}
