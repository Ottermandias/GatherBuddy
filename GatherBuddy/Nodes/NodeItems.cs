using System.Collections.Generic;
using System.Linq;
using Dalamud;
using GatherBuddy.Game;
using GatherBuddy.Managers;

namespace GatherBuddy.Nodes
{
    public class NodeItems
    {
        // Two extra items for multiple overlapping hidden items (esp. maps)
        public Gatherable?[] Items { get; } = new Gatherable[10];

        // Print all items that are not null separated by '|'.
        public string PrintItems(string separator = "|", ClientLanguage lang = ClientLanguage.English)
        {
            return string.Join(separator, Items.Where(it => it != null)
                .Select(it => it!.Name[lang]));
        }

        public IEnumerable<Gatherable> ActualItems
            => Items.Where(i => i != null).Cast<Gatherable>();

        public IEnumerable<string> EnglishItemNames
            => ActualItems.Select(i => (string) i.Name);

        // Node contains any of the given items (in english names).
        public bool HasItems(params string[] it)
            => it.Length == 0 || EnglishItemNames.Any(it.Contains);

        // Set the first item that is currently null. Returns false if all items were null.
        public bool SetFirstNullItem(Node node, Gatherable item)
        {
            for (var i = 0; i < Items.Length; ++i)
            {
                if (Items[i] == null)
                {
                    Items[i] = item;
                    item.NodeList.Add(node);
                    return true;
                }
            }

            return false;
        }

        // Check if the node has no items.
        public bool NoItems()
            => Items.All(i => i == null);

        public NodeItems(Node node, IEnumerable<int> itemIdList, ItemManager gatherables)
        {
            foreach (var itemId in itemIdList)
            {
                if (gatherables.GatheringToItem.TryGetValue(itemId, out var item))
                    SetFirstNullItem(node, item);
            }
        }
    }
}
