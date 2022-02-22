using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dalamud;
using GatherBuddy.Enums;

namespace GatherBuddy.Classes;

public partial class GatheringNode
{
    public List<Gatherable> Items { get; init; }

    // Print all items separated by '|' or the given separator.
    public string PrintItems(string separator = "|", ClientLanguage lang = ClientLanguage.English)
        => string.Join(separator, Items.Select(it => it.Name[lang]));

    // Node contains any of the given items (in english names).
    public bool HasItems(params Gatherable[] it)
        => it.Length == 0 || Items.Any(it.Contains);

    private void AddNodeToItem(Gatherable item)
    {
        item.NodeList.Add(this);
        if (item.NodeType == NodeType.Unknown)
            item.NodeType = NodeType;
        else if (item.NodeType != NodeType.Regular && NodeType == NodeType.Regular)
            item.NodeType = NodeType.Regular;
        item.GatheringType = item.GatheringType.Add(GatheringType);
        item.ExpansionIdx  = Math.Min(item.ExpansionIdx, Territory.Data.ExVersion.Row);
    }

    public bool AddItem(Gatherable item)
    {
        if (Items.Contains(item))
        {
            if (item.NodeList.Contains(this))
                return false;

            AddNodeToItem(item);
            return true;
        }

        Items.Add(item);
        AddNodeToItem(item);
        return true;
    }
}
