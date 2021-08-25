using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dalamud;
using Dalamud.Game;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Data;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.SeFunctions;
using GatherBuddy.Utility;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using FishingSpot = GatherBuddy.Game.FishingSpot;

namespace GatherBuddy.Managers
{
    public readonly unsafe struct FishLog
    {
        public const     uint  SpearFishIdOffset = 20000;
        private readonly byte* _fish;
        private readonly byte* _spearFish;
        private readonly int   _numFish;
        private readonly int   _numSpearFish;

        public FishLog(SigScanner sigScanner, int numFish, int numSpearFish)
        {
            _numFish      = numFish;
            _numSpearFish = numSpearFish;

            _fish      = (byte*) new FishLogData(sigScanner).Address;
            _spearFish = (byte*) new SpearFishLogData(sigScanner).Address;
        }

        public bool SpearFishIsUnlocked(uint spearFishId)
        {
            spearFishId -= SpearFishIdOffset;
            if (spearFishId >= _numSpearFish)
            {
                PluginLog.Error($"Spearfish Id {spearFishId} is larger than number of spearfish in log {_numSpearFish}.");
                return false;
            }

            if (_spearFish == null)
            {
                PluginLog.Error("Requesting spearfish log completion, but pointer not set.");
                return false;
            }

            var offset = spearFishId / 8;
            var bit    = (byte) spearFishId % 8;
            return ((_spearFish[offset] >> bit) & 1) == 1;
        }

        public bool FishIsUnlocked(uint fishId)
        {
            if (fishId >= _numFish)
            {
                PluginLog.Error($"Fish Id {fishId} is larger than number of fish in log {_numFish}.");
                return false;
            }

            if (_fish == null)
            {
                PluginLog.Error("Requesting fish log completion, but pointer not set.");
                return false;
            }

            var offset = fishId / 8;
            var bit    = (byte) fishId % 8;
            return ((_fish[offset] >> bit) & 1) == 1;
        }

        public bool IsUnlocked(Fish fish)
            => fish.IsSpearFish ? SpearFishIsUnlocked(fish.FishId) : FishIsUnlocked(fish.FishId);
    }

    public class FishManager
    {
        private const string SaveFileName = "fishing_records.data";

        public readonly Dictionary<uint, Fish> Fish = new();

        public readonly Dictionary<string, Fish>[] FishNameToFish = new Dictionary<string, Fish>[]
        {
            new(),
            new(),
            new(),
            new(),
        };

        public readonly Dictionary<uint, FishingSpot>        FishingSpots = new();
        public readonly Dictionary<Territory, FishingSpot[]> FishingSpotsByTerritory;

        public readonly Dictionary<string, FishingSpot> FishingSpotNames            = new();
        public readonly Dictionary<string, FishingSpot> FishingSpotNamesWithArticle = new();
        public readonly Dictionary<uint, Bait>          Bait;

        public readonly List<Fish> FishByUptime;

        public readonly Dictionary<byte, GigHead> GigHeadFromRecord = new();

        public FishLog FishLog { get; }

        public Fish? FindOrAddFish(ExcelSheet<FishParameter> fishList, ExcelSheet<Item>[] itemSheets, uint fish)
        {
            if (Fish.TryGetValue(fish, out var newFish))
                return newFish;

            var fishRow = fishList.FirstOrDefault(r => r.Item == fish);

            if (fishRow == null)
            {
                PluginLog.Verbose($"Error: Could not find item {fish} as a fish.");
                return null;
            }

            var name = new FFName();

            var item = itemSheets[(int) ClientLanguage.English].GetRow(fish)!;
            newFish = new Fish(item, fishRow, name);

            foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
            {
                var langName = itemSheets[(int) lang].GetRow(fish)!.Name;
                name[lang]                           = langName;
                FishNameToFish[(int) lang][langName] = newFish;
            }

            Fish[fish] = newFish;

            return newFish;
        }

        public Fish FindOrAddSpearFish(SpearfishingItem fish, ExcelSheet<Item>[] itemSheets)
        {
            if (Fish.TryGetValue(fish.Item.Row, out var newFish))
                return newFish;

            if (!GigHeadFromRecord.TryGetValue((byte) fish.FishingRecordType.Row, out var gig))
                gig = GigHead.Unknown;

            var name = new FFName();
            newFish = new Fish(fish, gig, name);

            foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
            {
                var langName = itemSheets[(int) lang].GetRow(fish.Item.Row)!.Name;
                name[lang]                           = langName;
                FishNameToFish[(int) lang][langName] = newFish;
            }

            Fish[fish.Item.Row] = newFish;
            return newFish;
        }

        private static Dictionary<uint, Bait> CollectBait(IReadOnlyList<ExcelSheet<Item>> items)
        {
            const uint fishingTackleRow = 30;

            Dictionary<uint, Bait> ret = new()
            {
                { 0, Game.Bait.Unknown },
            };
            foreach (var item in items[0].Where(i => i.ItemSearchCategory.Row == fishingTackleRow))
            {
                FFName name = new();
                foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
                    name[lang] = items[(int) lang].GetRow(item.RowId)!.Name;
                ret.Add(item.RowId, new Bait(item, name));
            }

            return ret;
        }

        private static int ConvertCoord(int val, double scale)
            => (int) (100.0 * (41.0 / scale * val / 2048.0 + 1.0) + 0.5);

        private FishingSpot? FromFishingSpot(World territories, ExcelSheet<FishParameter> fishSheet,
            ExcelSheet<Item>[] itemSheets, Lumina.Excel.GeneratedSheets.FishingSpot spot)
        {
            var territory = spot.TerritoryType.Value;
            if (territory == null && spot.RowId < 10000) // hack for diadem spots
                return null;

            var map = territory != null
                ? territories.FindOrAddTerritory(territory)
                : territories.Territories.Territories.Values.FirstOrDefault(t => t.Name == "The Diadem");
            if (map == null)
                return null;

            var newSpot = new FishingSpot
            {
                Id        = spot.RowId,
                Radius    = spot.Radius,
                Territory = map,
                XCoord    = spot.X,
                YCoord    = spot.Z,
                PlaceName = FFName.FromPlaceName(spot.PlaceName.Row),
            };

            for (var i = 0; i < spot.Item.Length; ++i)
            {
                var fishId = spot.Item[i].Row;
                if (fishId == 0)
                    continue;

                var fish = FindOrAddFish(fishSheet, itemSheets, fishId);
                if (fish == null)
                    continue;

                fish.FishingSpots.Add(newSpot);
                newSpot.Items[i] = fish;
            }

            if (GatherBuddy.Language != ClientLanguage.German)
                return newSpot;

            var seBytes = Dalamud.GameData.GetExcelSheet<PlaceName>(ClientLanguage.German)!.GetRow(spot.PlaceName.Row)!.Unknown8.RawData;
            HandleGermanString(seBytes, newSpot);

            return newSpot;
        }

        private void HandleGermanString(ReadOnlySpan<byte> seBytes, FishingSpot spot)
        {
            if (seBytes.Length <= 0)
                return;

            var name1Length = seBytes[6] - 1;
            var name2Start  = 9 + name1Length;
            var name2Length = seBytes[name2Start - 1] - 1;
            var tmp         = new byte[Math.Max(name1Length, name2Length)];
            for (var i = 0; i < name1Length; ++i)
                tmp[i] = seBytes[i + 7];
            var name1 = System.Text.Encoding.UTF8.GetString(tmp, 0, name1Length);
            for (var i = 0; i < name2Length; ++i)
                tmp[i] = seBytes[i + name2Start];
            var name2 = System.Text.Encoding.UTF8.GetString(tmp, 0, name2Length);

            name1 = Util.RemoveItalics(Util.RemoveSplitMarkers(name1)).ToLowerInvariant();
            name2 = Util.RemoveItalics(Util.RemoveSplitMarkers(name2)).ToLowerInvariant();
            FishingSpotNamesWithArticle[name1] = spot;
            FishingSpotNamesWithArticle[name2] = spot;
            FishingSpotNamesWithArticle[spot.PlaceName![ClientLanguage.German]] = spot;
        }

        private FishingSpot? FromSpearfishingSpot(World territories, ExcelSheet<Item>[] itemSheets,
            ExcelSheet<SpearfishingItem> fishSheet, SpearfishingNotebook spot)
        {
            var territory = spot.TerritoryType.Value;
            if (territory == null)
                return null;

            var newSpot = new FishingSpot
            {
                Id           = spot.RowId,
                Radius       = spot.Radius,
                Territory    = territories.FindOrAddTerritory(territory),
                XCoord       = spot.X,
                YCoord       = spot.Y,
                PlaceName    = FFName.FromPlaceName(spot.PlaceName.Row),
                Spearfishing = true,
            };

            var items = spot.GatheringPointBase.Value!.Item;
            for (var i = 0; i < items.Length; ++i)
            {
                if (items[i] == 0)
                    continue;

                var gatherFish = fishSheet.GetRow((uint) items[i])!;
                var fish       = FindOrAddSpearFish(gatherFish, itemSheets);
                fish.FishingSpots.Add(newSpot);
                newSpot.Items[i] = fish;
            }

            if (GatherBuddy.Language != ClientLanguage.German)
                return newSpot;

            return newSpot;
        }

        private void SetupGigHeads()
        {
            var records = Dalamud.GameData.Excel.GetSheet<SpearfishingRecordPage>()!;
            foreach (var record in records)
            {
                GigHeadFromRecord[record.Unknown0] = GigHead.Small;
                GigHeadFromRecord[record.Unknown1] = GigHead.Normal;
                GigHeadFromRecord[record.Unknown2] = GigHead.Large;
            }
        }

        public FishManager(World territories)
        {
            var fishingSpots      = Dalamud.GameData.Excel.GetSheet<Lumina.Excel.GeneratedSheets.FishingSpot>()!;
            var spearfishingSpots = Dalamud.GameData.Excel.GetSheet<SpearfishingNotebook>()!;
            var spearfishingItems = Dalamud.GameData.Excel.GetSheet<SpearfishingItem>()!;
            var fishSheet         = Dalamud.GameData.Excel.GetSheet<FishParameter>()!;
            var itemSheets        = new ExcelSheet<Item>[4];

            SetupGigHeads();

            foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
                itemSheets[(int) lang] = Dalamud.GameData.GetExcelSheet<Item>(lang)!;

            foreach (var spot in fishingSpots
                .Select(s => FromFishingSpot(territories, fishSheet, itemSheets, s))
                .Concat(spearfishingSpots
                    .Select(s => FromSpearfishingSpot(territories, itemSheets, spearfishingItems, s)))
                .Where(s => s != null))
            {
                var scale = spot!.Territory!.SizeFactor;
                spot.XCoord = ConvertCoord(spot.XCoord, scale);
                spot.YCoord = ConvertCoord(spot.YCoord, scale);

                if (spot.Territory!.Aetherytes.Count > 0)
                    spot.ClosestAetheryte = spot.Territory!.Aetherytes
                        .Select(a => (a.WorldDistance(spot.Territory.Id, spot.XCoord, spot.YCoord), a)).Min().a;

                FishingSpots[spot.UniqueId] = spot;
                if (!spot.Spearfishing)
                    FishingSpotNames[spot!.PlaceName![GatherBuddy.Language].ToLowerInvariant()] = spot;
            }

            Bait = CollectBait(itemSheets);

            FishingSpotsByTerritory = FishingSpots.Values.GroupBy(s => s.Territory!).ToDictionary(sg => sg.Key, sg => sg.ToArray());

            this.Apply();
            FishByUptime = Fish.Values.Where(f => f.InLog && !f.OceanFish).ToList();

            FishLog = new FishLog(Dalamud.SigScanner, fishSheet.Count(f => f.IsInLog), (int) spearfishingItems.RowCount);

            PluginLog.Verbose("{Count} Fishing Spots collected.", FishingSpots.Count);
            PluginLog.Verbose("{Count} Fish collected.",          Fish.Count);
            PluginLog.Verbose("{Count} Types of Bait collected.", Bait.Count - 1);
        }

        public Fish? FindFishByName(string fishName, ClientLanguage firstLanguage)
        {
            // Check for full matches in first language first.
            if (FishNameToFish[(int) firstLanguage].TryGetValue(fishName, out var fish))
                return fish;

            // If no full match was found in the first language, check for full matches in other languages.
            // Skip actual client language.
            foreach (var lang in Enum.GetValues(typeof(ClientLanguage)).Cast<ClientLanguage>()
                .Where(l => l != firstLanguage))
            {
                if (FishNameToFish[(int) lang].TryGetValue(fishName, out fish))
                    return fish;
            }

            var   fishNameLc = fishName.ToLowerInvariant();
            var   minDist    = int.MaxValue;
            Fish? minFish    = null;

            foreach (var it in Fish.Values)
            {
                // Check if item name in first language only contains the search-string.
                var haystack = it.Name![firstLanguage].ToLowerInvariant();
                if (haystack.Contains(fishNameLc))
                    return it;

                // Compute the Levenshtein distance between the item name and the search-string.
                // Keep the name with minimal distance logged.
                var dist = Levenshtein.Distance(fishNameLc, haystack);
                if (dist < minDist)
                {
                    minDist = dist;
                    minFish = it;
                }

                if (dist == 0)
                    break;
            }

            // If no item contains the search string
            // and the Levensthein distance is not too large
            // return the most similar item.
            return minDist > 4 ? null : minFish;
        }

        public void SaveFishRecords()
        {
            var dir = new DirectoryInfo(Dalamud.PluginInterface.GetPluginConfigDirectory());
            if (!dir.Exists)
                try
                {
                    dir.Create();
                }
                catch (Exception e)
                {
                    PluginLog.Error($"Could not create save directory at {dir.FullName}:\n{e}");
                    return;
                }

            var file = new FileInfo(Path.Combine(dir.FullName, SaveFileName));
            try
            {
                File.WriteAllLines(file.FullName, Fish.Values.Select(f => f.Record.WriteLine(f.ItemId)));
            }
            catch (Exception e)
            {
                PluginLog.Error($"Could not write fishing records to file {file.FullName}:\n{e}");
            }
        }

        public static FileInfo GetSaveFileName()
            => new(Path.Combine(new DirectoryInfo(Dalamud.PluginInterface.GetPluginConfigDirectory()).FullName, SaveFileName));

        public void LoadFishRecords()
            => LoadFishRecords(GetSaveFileName());

        public void LoadFishRecords(FileInfo file)
        {
            if (!file.Exists)
            {
                PluginLog.Error($"Could not read fishing records from file {file.FullName} because it does not exist.");
                return;
            }

            try
            {
                var lines = File.ReadAllLines(file.FullName);
                foreach (var line in lines)
                {
                    var p = FishRecord.FromLine(line);
                    if (p == null)
                        continue;

                    var (fishId, records) = p.Value;
                    Fish[fishId].Record   = records;
                }
            }
            catch (Exception e)
            {
                PluginLog.Error($"Could not read fishing records from file {file.FullName}:\n{e}");
            }
        }

        public int MergeFishRecords(FileInfo file)
        {
            if (!file.Exists)
                return -1;

            try
            {
                var oldRecords = GetSaveFileName();
                if (oldRecords.Exists)
                    File.Copy(oldRecords.FullName, oldRecords.FullName + ".bak", true);
            }
            catch (Exception e)
            {
                PluginLog.Error($"Could not create a backup of fishing records:\n{e}");
                return -2;
            }

            var sum = 0;
            try
            {
                var lines = File.ReadAllLines(file.FullName);
                foreach (var line in lines)
                {
                    var p = FishRecord.FromLine(line);
                    if (p == null)
                        continue;

                    var (fishId, records) =  p.Value;
                    sum                   += Fish[fishId].Record.Merge(records) ? 1 : 0;
                }
            }
            catch (Exception e)
            {
                PluginLog.Error($"Could not read fishing records from file {file.FullName}:\n{e}");
                return -3;
            }

            return sum;
        }

        public void DumpFishLog()
        {
            foreach (var f in FishByUptime)
                PluginLog.Information($"[FishLogDump] {(FishLog.IsUnlocked(f) ? "" : "MISSING ")}{f.Name}.");
        }
    }
}
