using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Interface.ImGuiNotification;
using GatherBuddy.Config;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using GatherBuddy.Time;
using Newtonsoft.Json;

namespace GatherBuddy.GatherGroup;

internal static class SeStringBuilderExtension
{
    public static SeStringBuilder ColoredText(this SeStringBuilder builder, string text, int colorIdx)
    {
        if (!Configuration.ForegroundColors.ContainsKey(colorIdx))
            return builder.AddText(text);

        return builder.AddUiForeground((ushort)colorIdx).AddText(text).AddUiForegroundOff();
    }
}

public class GatherGroupManager
{
    public const string FileName = "gather_groups.json";

    public SortedList<string, TimedGroup> Groups { get; init; } = new();

    public TimedGroup? this[string value]
        => TryGetValue(value, out var ret) ? ret : null;

    public bool TryGetValue(string value, out TimedGroup ret)
        => Groups.TryGetValue(value.ToLowerInvariant().Trim(), out ret!);

    public SeString CreateHelp()
    {
        SeStringBuilder b = new();
        b.AddText("Please use with ")
            .ColoredText("/gathergroup ",                      GatherBuddy.Config.SeColorCommands)
            .ColoredText("[Name] ",                            GatherBuddy.Config.SeColorNames)
            .ColoredText("[optional: Eorzea Minute Offset]\n", GatherBuddy.Config.SeColorArguments)
            .AddText("Available groups are:\n");
        foreach (var value in Groups.Values)
        {
            b.ColoredText($"          {value.Name}", GatherBuddy.Config.SeColorNames)
                .AddText($" - {value.Description}\n");
        }

        return b.BuiltString;
    }

    public bool AddGroup(string name, TimedGroup group)
    {
        var lowerName = name.ToLowerInvariant().Trim();
        if (lowerName.Length == 0 || Groups.ContainsKey(lowerName))
            return false;

        Groups.Add(lowerName, group);
        return true;
    }

    public bool ChangeDescription(TimedGroup group, string description)
    {
        if (group.Description == description)
            return false;

        group.Description = description;
        return true;
    }

    public bool RenameGroup(TimedGroup group, string newName)
    {
        if (newName == group.Name)
            return false;

        var newSearchName = newName.ToLowerInvariant().Trim();
        if (newSearchName.Length == 0 || Groups.ContainsKey(newSearchName))
            return false;

        RemoveGroup(group);
        group.Name = newName;
        Groups.Add(newSearchName, group);
        return true;
    }

    public bool RemoveGroup(TimedGroup group)
        => Groups.Remove(group.Name.ToLowerInvariant().Trim());

    public bool ChangeGroupNodeLocation(TimedGroup group, int idx, ILocation? location)
    {
        if (ReferenceEquals(group.Nodes[idx].PreferLocation, location)
         || location != null && !location.Gatherables.Contains(@group.Nodes[idx].Item))
            return false;

        group.Nodes[idx].PreferLocation = location;
        return true;
    }

    public bool ChangeGroupNode(TimedGroup group, int idx, IGatherable? item, int? start, int? end, string? annotation, bool delete)
    {
        if (idx < 0 || group.Nodes.Count < idx)
            return false;

        if (delete)
        {
            if (idx == group.Nodes.Count)
                return false;

            group.Nodes.RemoveAt(idx);
            return true;
        }

        if (group.Nodes.Count == idx && item != null && !delete)
        {
            var newNode = new TimedGroupNode(item)
            {
                EorzeaStartMinute = start == null ? 0 : Math.Clamp(start.Value, 0, RealTime.MinutesPerDay - 1),
                EorzeaEndMinute   = end == null ? 0 : Math.Clamp(end.Value,     0, RealTime.MinutesPerDay - 1),
                Annotation        = annotation ?? string.Empty,
            };
            group.Nodes.Add(newNode);
            return true;
        }

        var changes = false;
        var node    = group.Nodes[idx];
        if (item != null)
        {
            if (!ReferenceEquals(node.Item, item))
                changes = true;
            node.Item = item;
            if (node.PreferLocation != null && !node.PreferLocation.Gatherables.Contains(item))
                node.PreferLocation = null;
        }

        if (start != null)
        {
            start = Math.Clamp(start.Value, 0, RealTime.MinutesPerDay - 1);
            if (start.Value != node.EorzeaStartMinute)
                changes = true;
            node.EorzeaStartMinute = start.Value;
        }

        if (end != null)
        {
            end = Math.Clamp(end.Value, 0, RealTime.MinutesPerDay - 1);
            if (end.Value != node.EorzeaEndMinute)
                changes = true;
            node.EorzeaEndMinute = end.Value;
        }

        if (annotation != null)
        {
            if (annotation != node.Annotation)
                changes = true;
            node.Annotation = annotation;
        }

        return changes;
    }

    public void MoveNode(TimedGroup group, int idx1, int idx2)
    {
        if (Functions.Move(group.Nodes, idx1, idx2))
            Save();
    }

    public void Save()
    {
        var file = Functions.ObtainSaveFile(FileName);
        if (file == null)
            return;

        try
        {
            var text = JsonConvert.SerializeObject(Groups.Values.Select(g => g.ToConfig()), Formatting.Indented);
            File.WriteAllText(file.FullName, text);
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not write gather groups to file {file.FullName}:\n{e}");
        }
    }

    public bool SetDefaults(bool restore = false)
    {
        var change = false;
        foreach (var cfgGroup in GroupData.DefaultGroups)
        {
            var searchName = cfgGroup.Name.ToLowerInvariant().Trim();
            if (Groups.ContainsKey(searchName))
            {
                if (!restore)
                    continue;

                Groups.Remove(searchName);
            }

            TimedGroup.FromConfig(cfgGroup, out var group);
            Groups.Add(searchName, group);
            change = true;
        }

        return change;
    }


    public static GatherGroupManager Load()
    {
        var manager = new GatherGroupManager();
        var file    = Functions.ObtainSaveFile(FileName);
        if (file is not { Exists: true })
        {
            manager.SetDefaults();
            manager.Save();
            return manager;
        }

        try
        {
            var text    = File.ReadAllText(file.FullName);
            var data    = JsonConvert.DeserializeObject<List<TimedGroup.Config>>(text)!;
            var changes = false;
            foreach (var config in data)
            {
                if (!TimedGroup.FromConfig(config, out var group))
                {
                    GatherBuddy.Log.Error($"Invalid items in gather group {group.Name} skipped.");
                    changes = true;
                }

                var searchName = group.Name.ToLowerInvariant().Trim();
                if (searchName.Length == 0)
                {
                    changes = true;
                    GatherBuddy.Log.Error("Gather group without name found, skipping.");
                    continue;
                }

                if (!manager.Groups.TryAdd(searchName, group))
                {
                    changes = true;
                    GatherBuddy.Log.Error($"Multiple gather groups with the same name {searchName} found, skipping later ones.");
                }
            }

            changes |= manager.SetDefaults();
            if (changes)
            {
                Dalamud.Notifications.AddNotification(new Notification()
                {
                    Title = "GatherBuddy Error",
                    Content =
                        "Failed to load some gather groups. See the plugin log for more details. This is not saved, if it keeps happening you need to manually change an Gather Group to cause a save.",
                    MinimizedText = "Failed to load gather groups.",
                    Type          = NotificationType.Error,
                });
            }
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Error loading gather groups:\n{e}");
            manager.Groups.Clear();
            manager.SetDefaults();
            manager.Save();
        }

        return manager;
    }
}
