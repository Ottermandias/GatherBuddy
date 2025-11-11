using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GatherBuddy.Models;

namespace GatherBuddy.DataImport
{
    class Program
    {
        private const string OutputPath = "../../../../GatherBuddyReborn/GatherBuddy/CustomInfo";
        private const string FishRecordsFile = "fish_records.json";
        private const int SpotCap = 50; 

        static void Main(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("Please provide the path to the input JSON file.");

            var input = args[0];
            if (!File.Exists(input))
                throw new FileNotFoundException("Input file not found.", input);

            var output = Path.Combine(OutputPath, FishRecordsFile);

            var oldFishRecords = File.Exists(output)
                ? JsonConvert.DeserializeObject<List<SimpleFishRecord>>(File.ReadAllText(output)) ?? new List<SimpleFishRecord>()
                : new List<SimpleFishRecord>();

            var newContent = File.ReadAllText(input);
            var (newEntries, newZeroSortCount) = DeserializeNewData(newContent);

            var oldEntries = oldFishRecords
                .Select(r => new FishEntry
                {
                    Record = r,
                    SortTimestamp = r.Timestamp,
                    IsNew = false
                })
                .ToList();

            var allEntries = oldEntries
                .Concat(newEntries)
                .GroupBy(e => e.Record.Id)
                .Select(g => g.OrderByDescending(x => x.SortTimestamp).First())
                .ToList();

            var trimmedEntries = allEntries
                .GroupBy(e => e.Record.FishingSpot)
                .SelectMany(g =>
                {
                    var newForSpot = g
                        .Where(e => e.IsNew)
                        .ToList();

                    newForSpot = DedupeByPosition(newForSpot);

                    newForSpot = newForSpot
                        .OrderByDescending(e => e.SortTimestamp)
                        .Take(SpotCap)
                        .ToList();

                    if (newForSpot.Count == SpotCap)
                        return newForSpot;

                    var oldForSpot = g
                        .Where(e => !e.IsNew)
                        .ToList();

                    oldForSpot = DedupeByPosition(oldForSpot);

                    oldForSpot = oldForSpot
                        .OrderByDescending(e => e.SortTimestamp)
                        .Take(SpotCap - newForSpot.Count)
                        .ToList();

                    return newForSpot.Concat(oldForSpot);
                })
                .ToList();

            var finalRecords = trimmedEntries
                .OrderBy(e => e.Record.FishingSpot)
                .ThenByDescending(e => e.SortTimestamp)
                .Select(e => e.Record)
                .ToList();

            var oldIds = new HashSet<Guid>(oldFishRecords.Select(r => r.Id));
            var trimmedFromOld = finalRecords.Count(r => oldIds.Contains(r.Id));
            var trimmedFromNew = finalRecords.Count - trimmedFromOld;

            Console.WriteLine("======================================");
            Console.WriteLine("        GatherBuddy Data Importer     ");
            Console.WriteLine("======================================");
            Console.WriteLine($"Input file: {input}");
            Console.WriteLine($"Output file (relative): {output}");
            Console.WriteLine();
            Console.WriteLine($"Old records loaded: {oldFishRecords.Count}");
            Console.WriteLine($"New records loaded: {newEntries.Count}");
            Console.WriteLine($"New records with sortTimestamp == 0: {newZeroSortCount}");
            Console.WriteLine($"Merged total (before trimming): {allEntries.Count}");
            Console.WriteLine($"Trimmed total (after trimming): {finalRecords.Count}");
            Console.WriteLine($"Unique FishingSpots: {finalRecords.Select(r => r.FishingSpot).Distinct().Count()}");
            Console.WriteLine($"Trimmed from OLD file: {trimmedFromOld}");
            Console.WriteLine($"Trimmed from NEW file: {trimmedFromNew}");
            Console.WriteLine("======================================");
            Console.WriteLine();

            var fullOutputPath = Path.GetFullPath(output);
            Console.WriteLine($"Writing file to: {fullOutputPath}");

            Directory.CreateDirectory(OutputPath);
            File.WriteAllText(output, JsonConvert.SerializeObject(finalRecords, Formatting.Indented));

            Console.WriteLine("Write completed successfully!");
            Console.WriteLine("======================================");
        }

        /// <summary>
        /// Reads either:
        /// 1) a normal JSON array of SimpleFishRecord
        /// 2) or Dynamo-style NDJSON: one line per record with { "Item": { ... } }
        /// Returns fish entries with separate sort timestamps, and how many had 0 sort timestamps.
        /// </summary>
        private static (List<FishEntry> Records, int ZeroSortCount) DeserializeNewData(string content)
        {
            content = content.Trim();

            if (content.StartsWith("["))
            {
                var list = JsonConvert.DeserializeObject<List<SimpleFishRecord>>(content) ?? new List<SimpleFishRecord>();
                var entries = list
                    .Select(r => new FishEntry
                    {
                        Record = r,
                        SortTimestamp = r.Timestamp,
                        IsNew = true
                    })
                    .ToList();
                var zeroCount = entries.Count(x => x.SortTimestamp == 0);
                return (entries, zeroCount);
            }

            var listResult = new List<FishEntry>();
            var zeroSort = 0;

            using var reader = new StringReader(content);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Length == 0)
                    continue;

                var obj = JObject.Parse(line);
                var item = obj["Item"] as JObject;
                if (item == null)
                    continue;

                var entry = MapDynamoItemToEntry(item);
                if (entry != null)
                {
                    if (entry.SortTimestamp == 0)
                        zeroSort++;
                    listResult.Add(entry);
                }
            }

            return (listResult, zeroSort);
        }

        /// <summary>
        /// Converts a Dynamo-style item into a FishEntry, preserving Dynamo Timestamp,
        /// but using processed_at as the sort timestamp.
        /// </summary>
        private static FishEntry? MapDynamoItemToEntry(JObject item)
        {
            string? GetS(string name) => item[name]?["S"]?.ToString();
            string? GetN(string name) => item[name]?["N"]?.ToString();

            Guid id = Guid.Empty;
            var idStr = GetS("Id");
            if (!string.IsNullOrEmpty(idStr))
                Guid.TryParse(idStr, out id);
            if (id == Guid.Empty)
                id = Guid.NewGuid();

            ushort GetUShort(string name)
            {
                var s = GetN(name);
                return ushort.TryParse(s, out var v) ? v : (ushort)0;
            }

            uint GetUInt(string name)
            {
                var s = GetN(name);
                return uint.TryParse(s, out var v) ? v : 0u;
            }

            byte GetByte(string name)
            {
                var s = GetN(name);
                return byte.TryParse(s, out var v) ? v : (byte)0;
            }

            float GetFloat(string name)
            {
                var s = GetN(name);
                return float.TryParse(
                    s,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var v)
                    ? v
                    : 0f;
            }

            Effects GetEffects(string name)
            {
                var s = GetN(name);
                return int.TryParse(s, out var v) ? (Effects)v : 0;
            }

            var fishingSpot = GetUShort("FishingSpot");
            if (fishingSpot == 0)
                return null;

            var dynTimestamp = 0;
            var tsNum = GetN("Timestamp");
            if (!string.IsNullOrEmpty(tsNum) && int.TryParse(tsNum, out var parsedTs) && parsedTs > 0)
                dynTimestamp = parsedTs;

            var sortTs = 0;
            var processed = GetS("processed_at");
            if (!string.IsNullOrEmpty(processed))
            {
                if (DateTime.TryParseExact(processed, "yyyyMMdd-HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dt))
                    sortTs = (int)new DateTimeOffset(dt).ToUnixTimeSeconds();
            }

            if (sortTs == 0)
                sortTs = dynTimestamp > 0 ? dynTimestamp : 1;

            var record = new SimpleFishRecord
            {
                Id = id,
                FishingSpot = fishingSpot,
                BaitId = GetUInt("BaitId"),
                CatchId = GetUInt("CatchId"),
                Timestamp = dynTimestamp,
                Effects = GetEffects("Effects"),
                Bite = GetUShort("Bite"),
                Perception = GetUShort("Perception"),
                Gathering = GetUShort("Gathering"),
                TugAndHook = GetByte("TugAndHook"),
                Amount = GetByte("Amount"),
                PositionX = GetFloat("PositionX"),
                PositionY = GetFloat("PositionY"),
                PositionZ = GetFloat("PositionZ"),
                Rotation = GetFloat("Rotation"),
            };

            return new FishEntry
            {
                Record = record,
                SortTimestamp = sortTs,
                IsNew = true
            };
        }

        private static List<FishEntry> DedupeByPosition(List<FishEntry> entries)
        {
            var dict = new Dictionary<(float x, float y, float z, float r), FishEntry>();

            foreach (var e in entries)
            {
                var key = (e.Record.PositionX, e.Record.PositionY, e.Record.PositionZ, e.Record.Rotation);
                if (dict.TryGetValue(key, out var existing))
                {
                    if (e.SortTimestamp > existing.SortTimestamp)
                        dict[key] = e;
                }
                else
                {
                    dict[key] = e;
                }
            }

            return dict.Values.ToList();
        }

        class FishEntry
        {
            public SimpleFishRecord Record { get; set; } = null!;
            public int SortTimestamp { get; set; }
            public bool IsNew { get; set; }
        }
    }
}
