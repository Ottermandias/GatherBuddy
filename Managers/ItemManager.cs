using System;
using Serilog;
using System.Collections.Generic;
using Dalamud.Plugin;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using Dalamud;
using Otter;

namespace Gathering
{
    public class ItemManager
    {
        // Sorted Set so the item search uses the lexicographically first item on partial match, rather than a random one.
        public SortedSet<Gatherable>            items = new();
        public Dictionary<string, Gatherable>[] fromLanguage = new Dictionary<string, Gatherable>[4]{ new(), new(), new(), new() };
        public Dictionary<int, Gatherable>      gatheringToItem = new();

        public ItemManager(DalamudPluginInterface pi)
        {
            var gatheringExcel = pi.Data.GetExcelSheet<GatheringItem>();
            var itemSheets     = new ExcelSheet<Item>[4];
            foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
                itemSheets[(int)lang] = pi.Data.GetExcelSheet<Item>(lang);

            foreach (var item in gatheringExcel)
            {
                // Skip invalid items.
                // There are a bunch of ids in gathering that do belong to quest or leve items.
                if (item == null || item.Item == 0 || item.Item > 1000000) 
                    continue;

                var I = new Gatherable(item.Item, (int) item.RowId, item.GatheringItemLevel.Value.GatheringItemLevel, item.GatheringItemLevel.Value.Stars);
                foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
                {
                    var it = itemSheets[(int) lang].GetRow((uint)item.Item);
                    if (it == null)
                    {
                        Log.Error($"[GatherBuddy] No name for {item.Item} in language {Enum.GetName(typeof(ClientLanguage), lang)}.");
                        continue;
                    }
                    fromLanguage[(int) lang][it.Name] = I;
                    I.nameList[lang] = it.Name;
                }

                items.Add(I);
                gatheringToItem[(int) item.RowId] = I;
            }
            Log.Verbose($"[GatherBuddy] {items.Count} items collected for gathering.");
        }

        public Gatherable FindItemByName(string itemName, ClientLanguage firstLanguage)
        {
            // Check for full matches in first language first.
            if (fromLanguage[(int) firstLanguage].TryGetValue(itemName, out Gatherable item))
                return item;
            else
            {
                // If no full match was found in the first language, check for full matches in other languages.
                foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
                {
                    // Skip actual client language.
                    if (lang == firstLanguage)
                        continue;

                    if (fromLanguage[(int)lang].TryGetValue(itemName, out item))
                        return item;
                }
            }

            string itemNameLC  = itemName.ToLowerInvariant();
            int minDist        = int.MaxValue;
            Gatherable minItem = null;

            foreach (var it in items)
            {
                // Check if item name in first language only contains the search-string.
                string haystack = it.nameList[firstLanguage].ToLowerInvariant();
                if (haystack.Contains(itemNameLC))
                    return it;

                // Compute the Levenshtein distance between the item name and the search-string.
                // Keep the name with minimal distance logged.
                var dist = Levenshtein.Distance(itemNameLC, haystack);
                if (dist < minDist)
                {
                    minDist = dist;
                    minItem = it;
                }
                if (dist == 0)
                    break;
            }

            // If no item contains the search string
            // and the Levensthein distance is not too large
            // return the most similar item.
            if (minDist > 4)
                return null;
            return minItem;
        }
    }
}