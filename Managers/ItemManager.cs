using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Plugin;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using Dalamud;
using GatherBuddy.Game;
using GatherBuddy.Utility;

namespace GatherBuddy.Managers
{
    public class ItemManager
    {
        // Sorted Set so the item search uses the lexicographically first item on partial match, rather than a random one.
        public SortedSet<Gatherable> Items { get; } = new();

        public Dictionary<string, Gatherable>[] FromLanguage { get; } = new Dictionary<string, Gatherable>[]
        {
            new(),
            new(),
            new(),
            new(),
        };

        public Dictionary<int, Gatherable> GatheringToItem { get; } = new();

        public ItemManager(DalamudPluginInterface pi)
        {
            var gatheringExcel = pi.Data.GetExcelSheet<GatheringItem>();
            var itemSheets     = new ExcelSheet<Item>[4];
            foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
                itemSheets[(int) lang] = pi.Data.GetExcelSheet<Item>(lang);

            var defaultSheet = itemSheets[(int) pi.ClientState.ClientLanguage];
            // Skip invalid items.
            // There are a bunch of ids in gathering that do belong to quest or leve items.
            foreach (var item in gatheringExcel.Where(i => i != null && i.Item != 0 && i.Item < 1000000))
            {
                var row     = defaultSheet.GetRow((uint) item.Item);
                var newItem = new Gatherable(row, item, item.GatheringItemLevel.Value.GatheringItemLevel, item.GatheringItemLevel.Value.Stars);
                foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
                {
                    var it = itemSheets[(int) lang].GetRow((uint) item.Item);
                    if (it == null)
                    {
                        PluginLog.Error($"No name for {item.Item} in language {Enum.GetName(typeof(ClientLanguage), lang)}.");
                        continue;
                    }

                    FromLanguage[(int) lang][it.Name] = newItem;
                    newItem.Name[lang]                = it.Name;
                }

                Items.Add(newItem);
                GatheringToItem[(int) item.RowId] = newItem;
            }

            PluginLog.Verbose("{Count} items collected for gathering.", Items.Count);
        }

        public Gatherable? FindItemByName(string itemName, ClientLanguage firstLanguage)
        {
            // Check for full matches in first language first.
            if (FromLanguage[(int) firstLanguage].TryGetValue(itemName, out var item))
                return item;

            // If no full match was found in the first language, check for full matches in other languages.
            // Skip actual client language.
            foreach (var lang in Enum.GetValues(typeof(ClientLanguage)).Cast<ClientLanguage>()
                .Where(l => l != firstLanguage))
            {
                if (FromLanguage[(int) lang].TryGetValue(itemName, out item))
                    return item;
            }

            var         itemNameLc = itemName.ToLowerInvariant();
            var         minDist    = int.MaxValue;
            Gatherable? minItem    = null;

            foreach (var it in Items)
            {
                // Check if item name in first language only contains the search-string.
                var haystack = it.Name[firstLanguage].ToLowerInvariant();
                if (haystack.Contains(itemNameLc))
                    return it;

                // Compute the Levenshtein distance between the item name and the search-string.
                // Keep the name with minimal distance logged.
                var dist = Levenshtein.Distance(itemNameLc, haystack);
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
            return minDist > 4 ? null : minItem;
        }
    }
}
