using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dalamud;
using Dalamud.Plugin;
using GatherBuddy.Classes;
using GatherBuddy.Utility;
using Lumina.Excel.GeneratedSheets;
using FishingSpot = GatherBuddy.Classes.FishingSpot;

namespace GatherBuddy.Managers
{
    public class FishManager
    {
        private const string SaveFileName = "fishing_records.data";

        public Dictionary<uint, Fish> Fish = new();

        public Dictionary<string, Fish>[] FishNameToFish = new Dictionary<string, Fish>[4]
        {
            new(),
            new(),
            new(),
            new(),
        };

        public Dictionary<uint, FishingSpot> FishingSpots = new();

        public Dictionary<string, FishingSpot> FishingSpotNames            = new();
        public Dictionary<string, FishingSpot> FishingSpotNamesWithArticle = new ();
        public Dictionary<uint, Bait>          Bait;


        public Fish FindOrAddFish(Lumina.Excel.ExcelSheet<Item>[] itemSheets, uint fish)
        {
            if (Fish.TryGetValue(fish, out var newFish))
                return newFish;

            newFish = new Fish{ Id = fish };

            var name = new FFName();
            foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
            {
                var langName = itemSheets[(int) lang].GetRow(fish).Name;
                name[lang]                           = langName;
                FishNameToFish[(int) lang][langName] = newFish;
            }

            newFish.Name = name;
            Fish[fish]   = newFish;
            return newFish;
        }

        private static Dictionary<uint, Bait> CollectBait(Lumina.Excel.ExcelSheet<Item>[] items)
        {
            const uint fishingTackleRow = 30;

            Dictionary<uint, Bait> ret = new()
            {
                { 0, Classes.Bait.Unknown }
            };
            foreach (var item in items[0].Where( i => i.ItemSearchCategory.Row == fishingTackleRow))
            {
                FFName        name    = new();
                foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
                    name[lang] = items[(int) lang].GetRow(item.RowId).Name;
                ret.Add(item.RowId, new Bait(item.RowId, name));
            }

            return ret;
        }

        private static int ConvertCoord(int val, double scale)
            =>(int) (100.0 * (41.0 / scale * val / 2048.0 + 1.0) + 0.5);

        public FishManager(DalamudPluginInterface pi, World territories, AetheryteManager aetherytes)
        {
            var fishingSpots = pi.Data.Excel.GetSheet<Lumina.Excel.GeneratedSheets.FishingSpot>();
            var itemSheets   = new Lumina.Excel.ExcelSheet<Item>[4];
            foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
                itemSheets[(int) lang] = pi.Data.GetExcelSheet<Item>(lang);

            foreach (var spot in fishingSpots)
            {
                var territory = spot.TerritoryType.Value;
                if (territory == null)
                    continue;

                var newSpot = new FishingSpot()
                {
                    Id        = (int) spot.RowId,
                    Radius    = spot.Radius,
                    Territory = territories.FindOrAddTerritory(territory),
                    XCoord    = spot.X,
                    YCoord    = spot.Z,
                    PlaceName = FFName.FromPlaceName(pi, spot.PlaceName.Row),
                };

                var scale = territory.Map.Value?.SizeFactor / 100.0  ?? 1.0;
                newSpot.XCoord = ConvertCoord(newSpot.XCoord, scale);
                newSpot.YCoord = ConvertCoord(newSpot.YCoord, scale);

                if (newSpot.Territory!.Aetherytes.Count > 0)
                    newSpot.ClosestAetheryte = newSpot.Territory!.Aetherytes
                        .Select(a => (a.WorldDistance(newSpot.Territory.Id, newSpot.XCoord, newSpot.YCoord), a)).Min().a;

                for (var i = 0; i < 9; ++i)
                {
                    var fishId = spot.Item[i].Row;
                    if (fishId == 0)
                        continue;

                    var fish = FindOrAddFish(itemSheets, fishId);
                    fish.FishingSpots.Add(newSpot);
                    newSpot.Items[i] = fish;
                }

                FishingSpots[spot.RowId]                                           = newSpot;
                FishingSpotNames[newSpot.PlaceName[pi.ClientState.ClientLanguage]] = newSpot;
                if (pi.ClientState.ClientLanguage != ClientLanguage.German)
                    continue;

                var ffName = new FFName
                {
                    [ClientLanguage.German] =
                        pi.Data.GetExcelSheet<PlaceName>(ClientLanguage.German).GetRow(spot.PlaceName.Row).Unknown8
                };
                FishingSpotNamesWithArticle[ffName[ClientLanguage.German]] = newSpot;
            }

            Bait = CollectBait(itemSheets);

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

        public void SaveFishRecords(DalamudPluginInterface pi)
        {
            var dir = new DirectoryInfo(pi.GetPluginConfigDirectory());
            if (!dir.Exists)
            {
                try
                {
                    dir.Create();
                }
                catch (Exception e)
                {
                    PluginLog.Error($"Could not create save directory at {dir.FullName}:\n{e}");
                    return;
                }
            }

            var file = new FileInfo(Path.Combine(dir.FullName, SaveFileName));
            try
            {
                File.WriteAllLines(file.FullName, Fish.Values.Select(f => f.Record.WriteLine(f.Id)));
            }
            catch (Exception e)
            {
                PluginLog.Error($"Could not write fishing records to file {file.FullName}:\n{e}");
            }
        }

        public FileInfo GetSaveFileName(DalamudPluginInterface pi)
            => new(Path.Combine(new DirectoryInfo(pi.GetPluginConfigDirectory()).FullName, SaveFileName));

        public void LoadFishRecords(DalamudPluginInterface pi)
            => LoadFishRecords(GetSaveFileName(pi));

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
    }
}
