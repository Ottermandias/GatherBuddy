using System;
using System.Collections.Generic;
using Dalamud;
using System.Linq;
using Serilog;
using Lumina.Excel.GeneratedSheets;

namespace Gathering
{
    public enum GatheringType
    {
        Mining       = 0,
        Quarrying    = 1,
        Logging      = 2,
        Harvesting   = 3,
        Spearfishing = 4,
        Botanist     = 5,
        Miner        = 6,
        Fisher       = 7,
    };

    public static class GatheringTypeExtension
    {
        public static GatheringType ToGroup(this GatheringType type)
        {
            return type switch
            {
                GatheringType.Mining       => GatheringType.Miner,
                GatheringType.Quarrying    => GatheringType.Miner,
                GatheringType.Miner        => GatheringType.Miner,
                GatheringType.Logging      => GatheringType.Botanist,
                GatheringType.Harvesting   => GatheringType.Botanist,
                GatheringType.Botanist     => GatheringType.Botanist,
                GatheringType.Spearfishing => GatheringType.Fisher,
                _                          => type,
            };
        }
    }

    public enum NodeType
    {
        Regular   = 0,
        Unspoiled = 1,
        Ephemeral = 2
    };

    public class InitialNodePosition
    {
        public Aetheryte closestAetheryte = null;
        public int       xCoord = 0;
        public int       yCoord = 0;
        public bool      prefer = false;

        public double ToX() => xCoord / 100.0;
        public double ToY() => yCoord / 100.0;
        

    }

    public class NodeMeta : IComparable
    {
        public GatheringType gatheringType;
        public NodeType      nodeType;
        public int           pointBaseId;
        public int           level;
        
        public NodeMeta(GatheringPointBase row, NodeType type)
        {
            level         = row.GatheringLevel;
            pointBaseId   = (int) row.RowId;
            gatheringType = (GatheringType) row.GatheringType.Row;
            nodeType      = type;
        }

        public bool IsMiner()      { return gatheringType.ToGroup() == GatheringType.Miner; }
        public bool IsBotanist()   { return gatheringType.ToGroup() == GatheringType.Botanist; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var rhs = obj as NodeMeta;

            return pointBaseId - rhs.pointBaseId;
        }
    }

    public class NodeItems
    {
        // One extra item for multiple overlapping hidden items (esp. maps)
        public readonly Gatherable[] items;

        // Print all items that are not null separated by '|'.
        public string PrintItems(string separator = "|", ClientLanguage lang = ClientLanguage.English)
        {
            string s = "";
            for(int i = 0; i < items.Length; ++i)
            {
                var it = items[i];
                if (it != null)
                    s += ((s.Length > 0) ? separator + it.nameList[lang] : it.nameList[lang]);
            }
            return s;
        }

        // Node contains any of the given items (in english names).
        public bool HasItems(params string[] it)
        {
            if (it == null || it.Length == 0)
                return true;
            foreach (var i in it)
            {
                bool found = false;
                foreach (var I in this.items)
                {
                    if (I == null)
                        continue;
                    if (i == I.nameList[ClientLanguage.English])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return false;
            }
            return true;
        }

        // Set the first item that is currently null. Returns false if all items were null.
        public bool SetFirstNullItem(Node node, Gatherable item)
        {
            for (int i = 0; i < items.Length; ++i)
                if (items[i] == null)
                {
                    items[i] = item;
                    item.NodeList.Add(node);
                    return true;
                }
            return false;
        }

        // Check if the node has no items.
        public bool NoItems()
        {
            foreach (var i in items)
                if (i != null)
                    return false;
            return true;
        }

        public NodeItems(Node node, int[] itemIdList, ItemManager gatherables)
        {
            items = new Gatherable[9]{ null, null, null, null, null, null, null, null, null };
            foreach (var itemId in itemIdList)
            {
                if (gatherables.gatheringToItem.TryGetValue(itemId, out Gatherable item))
                    SetFirstNullItem(node, item);
            }
        }
    }

    public class SubNodes
    {
        public Territory territory        = null;
        public Aetheryte closestAetheryte = null;
        public Dictionary<uint, NodeLocation> nodes = new();
        public int averageX = 0;
        public int averageY = 0;

        public double ToX() => averageX / 100.0;
        public double ToY() => averageY / 100.0;

        public void RecomputeAverage()
        {
            if (nodes.Count > 0)
            {
                var num = 0;
                averageX = 0;
                averageY = 0;
                foreach (var p in nodes.Values.Where( N => N != null))
                { 
                    ++num;
                    averageX += p.averageX;
                    averageY += p.averageY;
                }
                averageX /= num;
                averageY /= num;

                if (territory == null || territory.aetherytes.Count == 0)
                    closestAetheryte = null;
                else 
                    closestAetheryte = territory.aetherytes.Select(a => (a.WorldDistance(territory.id, averageX, averageY), a)).Min().a;
            }
        }

        public bool AddNodeLocation(uint nodeId, NodeLocation loc)
        {
            if (nodes.TryGetValue(nodeId, out var oldLoc) && oldLoc != null)
            {
                if (oldLoc.AddLocation(loc))
                {
                    RecomputeAverage();
                    return true;
                }
                return false;
            }
            nodes[nodeId] = loc;
            RecomputeAverage();
            return true;
        }
    }

    public class NodeTimes
    {
        private const int allHours = 0x00FFFFFF;

        private readonly int hours; // bitfield, 0-23 for each hour.

        public bool AlwaysUp() { return hours == allHours; }
        public bool IsUp(int hour) { if (hour > 23) hour %= 24; return ((hours >> hour) & 1) == 1; }

        public int NextUptime(int currentHour)
        {
            for (var i = 0; i < 24; ++i)
            {
                if (IsUp(currentHour))
                    return i;

                ++currentHour;
                if (currentHour > 23)
                    currentHour -= 24;
            }
            return 25;
        }

        // Print a string of 24 '0' or '1' as uptimes.
        public string UptimeTable()
        {
            return new (Convert.ToString(hours, 2).PadLeft(24, '0').Reverse().ToArray());
        }

        // Print hours in human readable format.
        public string PrintHours(bool simple = false, string simpleSeparator = "|")
        {
            string ret = "";
            int min = -1, max = -1;

            void AddString()
            {
                if (min >= 0)
                {
                    if (ret.Length > 0)
                    {
                        if (simple)
                            ret += simpleSeparator;
                        else
                        {
                            ret = ret.Replace(" and ", ", ");
                            ret += " and ";
                        }
                    }

                    if (simple)
                        ret += $"{min:D2}-{(max + 1):D2}";
                    else
                        ret += $"{min:D2}:00 - {(max + 1):D2}:00 ET";

                    min = -1;
                    max = -1;
                }
            }

            for (int i = 0; i < 24; ++i)
            {
                if (IsUp(i))
                {
                    if (min < 0)
                        min = i;
                    else
                        max = i;
                }
                else
                    AddString();
            }
            AddString();

            return ret;
        }

        // Convert the ephemeral time as given by the table to a bitfield.
        private static int ConvertFromEphemeralTime(ushort start, ushort end)
        {
            // Up at all times
            if (start == end || start > 2400 || end > 2400)
                return allHours;

            int ret = 0;
            start  /= 100;
            end    /= 100;

            if (end < start) 
                end += 24;

            for (int i = start; i < end; ++i)
                ret |= (1 << (i % 24));
            return ret;
        }

        public NodeTimes(){ hours = allHours; }
        public NodeTimes(ushort start, ushort end)
        {
            hours = ConvertFromEphemeralTime(start, end);
        }

        // Convert the rare pop time given by the table to a bitfield.
        public NodeTimes(GatheringRarePopTimeTable table)
        {
            if (table == null)
                return;

            hours = 0;
            // Convert the time slots to ephemeral format to reuse that function.
            foreach (var time in table.UnkStruct0)
            {
                if (time.Durationm == 0)
                    continue;
                ushort duration = (time.Durationm == 160) ? (ushort) 200 : time.Durationm;
                ushort end      = (ushort) ((time.StartTime + duration) % 2400);
                hours |= ConvertFromEphemeralTime(time.StartTime, end);
            }
        }
    }

    public class Node : IComparable
    {
        public NodeMeta            meta        = null;
        public SubNodes            nodes       = null;
        public InitialNodePosition initialPos  = null;
        public NodeItems           items       = null;
        public NodeTimes           times       = null;
        public string              placeNameEN = null; // Relevant for manual coordinates and aetheryte.
        
        private double CoordinateCap(double input) => input > 50.0 ? 0.0 : input;
        public double GetX()
        {
            if (initialPos != null)
            {
                if (initialPos.prefer || nodes == null || nodes.averageX == 0)
                    return CoordinateCap(initialPos.ToX());
                return CoordinateCap(nodes.ToX());
            }
            return CoordinateCap(nodes?.ToX() ?? 0);
        }
        public double GetY()
        {
            if (initialPos != null)
            {
                if (initialPos.prefer || nodes == null || nodes.averageY == 0)
                    return CoordinateCap(initialPos.ToY());
                return CoordinateCap(nodes.ToY());
            }
            return CoordinateCap(nodes?.ToY() ?? 0);
        }

        public Aetheryte GetValidAetheryte()
        {
            if (initialPos != null)
            {
                if (initialPos.prefer || nodes?.closestAetheryte == null)
                    return initialPos.closestAetheryte;
                return nodes?.closestAetheryte;
            }
            return nodes?.closestAetheryte;
        }
        public Aetheryte GetClosestAetheryte()
        {
            var ret = GetValidAetheryte();
            if (ret == null && nodes.territory.aetherytes.Count > 0)
                return nodes.territory.aetherytes.First();
            return ret;
        }

        public int CompareTo(object obj)
        {
            return meta.CompareTo(obj);
        }

        public void AddItem(Gatherable item)
        {
            if (!items.SetFirstNullItem(this, item))
                Log.Error($"[GatherBuddy] Could not add additional item {item} to node {meta.pointBaseId}, all 9 slots are used.");
        }
    }
}