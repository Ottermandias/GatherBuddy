using System;
using System.Collections.Generic;
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
        public Dictionary<uint, Fish> Fish = new();

        public Dictionary<string, Fish>[] FishNameToFish = new Dictionary<string, Fish>[4]
        {
            new(),
            new(),
            new(),
            new(),
        };

        public Dictionary<uint, FishingSpot> FishingSpots = new();

        public Fish FindOrAddFish(Lumina.Excel.ExcelSheet<Item>[] itemSheets, uint fish)
        {
            if (Fish.TryGetValue(fish, out var newFish))
                return newFish;

            newFish = new Fish() { Id = (int) fish };

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

                FishingSpots[spot.RowId] = newSpot;
            }

            PluginLog.Verbose("{Count} Fishing Spots collected.", FishingSpots.Count);
            PluginLog.Verbose("{Count} Fish collected.",          Fish.Count);
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
    }
}
