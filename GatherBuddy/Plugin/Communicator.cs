using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Structs;
using GatherBuddy.Time;

namespace GatherBuddy.Plugin;

internal static class SeStringBuilderExtension
{
    public static SeStringBuilder AddColoredText(this SeStringBuilder builder, string text, int colorId)
        => builder.AddUiForeground((ushort)colorId)
            .AddText(text)
            .AddUiForegroundOff();
}

public static class Communicator
{
    public delegate SeStringBuilder ReplacePlaceholder(SeStringBuilder builder, string placeholder);

    public static void Print(SeString message)
    {
        var entry = new XivChatEntry()
        {
            Message = message,
            Name    = SeString.Empty,
            Type    = GatherBuddy.Config.ChatTypeMessage,
        };
        Dalamud.Chat.PrintChat(entry);
    }

    public static void PrintError(SeString message)
    {
        var entry = new XivChatEntry()
        {
            Message = message,
            Name    = SeString.Empty,
            Type    = GatherBuddy.Config.ChatTypeError,
        };
        Dalamud.Chat.PrintChat(entry);
    }

    public static void Print(string message)
        => Print((SeString)message);

    public static void PrintError(string message)
        => PrintError((SeString)message);

    public static void Print(string left, string center, int color, string right)
    {
        SeStringBuilder builder = new();
        builder.AddText(left).AddColoredText(center, color).AddText(right);
        Print(builder.BuiltString);
    }

    public static void PrintError(string left, string center, int color, string right)
    {
        SeStringBuilder builder = new();
        builder.AddText(left).AddColoredText(center, color).AddText(right);
        PrintError(builder.BuiltString);
    }


    // Split a format string with '{text}' placeholders into a SeString with Payloads, 
    // and replace all placeholders by the returned payloads.
    private static SeString Format(string format, ReplacePlaceholder func)
    {
        SeStringBuilder builder     = new();
        var             lastPayload = 0;
        var             openBracket = -1;
        for (var i = 0; i < format.Length; ++i)
        {
            if (format[i] == '{')
            {
                openBracket = i;
            }
            else if (openBracket != -1 && format[i] == '}')
            {
                builder.AddText(format.Substring(lastPayload,   openBracket - lastPayload));
                var placeholder = format.Substring(openBracket, i - openBracket + 1);
                Debug.Assert(placeholder.StartsWith('{') && placeholder.EndsWith('}'));
                func(builder, placeholder);
                lastPayload = i + 1;
                openBracket = -1;
            }
        }

        if (lastPayload != format.Length)
            builder.AddText(format[lastPayload..]);
        return builder.BuiltString;
    }

    public static SeStringBuilder AddFullMapLink(SeStringBuilder builder, string name, Territory territory, float xCoord, float yCoord,
        bool openMapLink = false, bool withCoordinates = true, float fudgeFactor = 0.05f)
    {
        var mapPayload = new MapLinkPayload(territory.Id, territory.Data.Map.Row, xCoord, yCoord, fudgeFactor);
        if (openMapLink)
            Dalamud.GameGui.OpenMapWithMapLink(mapPayload);
        if (withCoordinates)
            name = $"{name} ({xCoord.ToString("00.0", CultureInfo.InvariantCulture)}, {yCoord.ToString("00.0", CultureInfo.InvariantCulture)})";
        return builder.Add(mapPayload)
            .AddUiForeground(500)
            .AddUiGlow(501)
            .AddText($"{(char)SeIconChar.LinkMarker}")
            .AddUiGlowOff()
            .AddUiForegroundOff()
            .AddText(name)
            .Add(RawPayload.LinkTerminator);
    }

    public static SeStringBuilder AddFullItemLink(SeStringBuilder builder, uint itemId, string itemName)
        => builder.AddUiForeground(0x0225)
            .AddUiGlow(0x0226)
            .AddItemLink(itemId, false)
            .AddUiForeground(0x01F4)
            .AddUiGlow(0x01F5)
            .AddText($"{(char)SeIconChar.LinkMarker}")
            .AddUiGlowOff()
            .AddUiForegroundOff()
            .AddText(itemName)
            .Add(RawPayload.LinkTerminator)
            .AddUiGlowOff()
            .AddUiForegroundOff();

    public static SeString FormatIdentifiedItemMessage(string format, string input, IGatherable item)
    {
        SeStringBuilder Replace(SeStringBuilder builder, string s)
            => s.ToLowerInvariant() switch
            {
                "{name}"  => AddFullItemLink(builder, item.ItemId, item.Name[GatherBuddy.Language]),
                "{input}" => builder.AddColoredText(input, GatherBuddy.Config.SeColorArguments),
                _         => builder.AddText(s),
            };

        return Format(format, Replace);
    }

    public static SeString FormatChoseFishingSpotMessage(string format, FishingSpot spot, Fish fish, Bait bait)
    {
        SeStringBuilder Replace(SeStringBuilder builder, string s)
            => s.ToLowerInvariant() switch
            {
                "{id}"       => builder.AddText(spot.Id.ToString()),
                "{name}"     => AddFullMapLink(builder, spot.Name, spot.Territory, spot.IntegralXCoord / 100f, spot.IntegralYCoord / 100f),
                "{fishid}"   => builder.AddText(fish.ItemId.ToString()),
                "{fishname}" => AddFullItemLink(builder, fish.ItemId, fish.Name[GatherBuddy.Language]),
                "{input}"    => builder.AddText(s),
                "{baitname}" => AddFullItemLink(builder, bait.Id, bait.Name),
                _            => builder.AddText(s),
            };

        return Format(format, Replace);
    }

    private static SeStringBuilder AddItemLinks(SeStringBuilder builder, IList<Gatherable> items)
    {
        for (var i = 0; i < items.Count - 1; ++i)
        {
            AddFullItemLink(builder, items[i].ItemId, items[i].Name[GatherBuddy.Language]);
            builder.AddText(", ");
        }

        if (items.Count > 0)
            AddFullItemLink(builder, items.Last().ItemId, items.Last().Name[GatherBuddy.Language]);
        return builder;
    }

    public static void ItemNotFound(string itemName)
    {
        SeStringBuilder sb = new();
        sb.AddText("Could not find corresponding item to \"")
            .AddUiForeground((ushort)GatherBuddy.Config.SeColorNames)
            .AddText(itemName)
            .AddUiForegroundOff()
            .AddText("\".");
        Print(sb.BuiltString);
        PluginLog.Verbose(sb.BuiltString.TextValue);
    }

    public static void LocationNotFound(IGatherable? item, GatheringType? type)
    {
        SeStringBuilder sb = new();
        sb.AddText("No associated location or attuned aetheryte found for ");
        if (item != null)
            sb.AddItemLink(item.ItemId, ItemPayload.ItemKind.Normal);
        else
            sb.AddColoredText("Unknown", GatherBuddy.Config.SeColorNames);

        if (type != null)
            sb.AddText(" with condition ")
                .AddColoredText(type.Value.ToString(), GatherBuddy.Config.SeColorArguments);
        Print(sb.BuiltString);
        PluginLog.Verbose(sb.BuiltString.TextValue);
    }

    public static void NoItemName(string command, string itemType)
        => PrintError($"please supply a (partial) {itemType} name for ", command, GatherBuddy.Config.SeColorCommands, ".");

    public static void NoGatherGroup(string groupName)
        => PrintError("The group ", groupName, GatherBuddy.Config.SeColorNames, " does not exist.");

    public static void NoGatherGroupItem(string groupName, int minute)
    {
        SeStringBuilder sb = new();
        sb.AddText("The group ")
            .AddColoredText(groupName, GatherBuddy.Config.SeColorNames)
            .AddText(" has no item corresponding to the eorzea time ")
            .AddColoredText($"{minute / RealTime.MinutesPerHour:D2}:{minute % RealTime.MinutesPerHour:D2}",
                GatherBuddy.Config.SeColorArguments)
            .AddText(".");
        PrintError(sb.BuiltString);
    }

    //public static SeString FormatNodeAlarmMessage(string format, NodeAlarm alarm, long timeDiff)
    //{
    //    SeStringBuilder NodeReplace(SeStringBuilder builder, string s)
    //        => s.ToLowerInvariant() switch
    //        {
    //            "{name}"        => builder.AddText(alarm.Name),
    //            "{offset}"      => builder.AddText(alarm.SecondOffset.ToString()),
    //            "{delaystring}" => builder.AddText(DelayString(timeDiff)),
    //            "{timesshort}"  => builder.AddText(alarm.Node!.Times.PrintHours(true)),
    //            "{timeslong}"   => builder.AddText(alarm.Node!.Times.PrintHours()),
    //            "{location}" => AddFullMapLink(builder, alarm.Node!.Name, alarm.Node.Territory, (float)alarm.Node.XCoord,
    //                (float)alarm.Node.YCoord),
    //            "{allitems}" => AddItemLinks(builder, alarm.Node!.Items),
    //            _            => builder.AddText(s),
    //        };
    //
    //    return Format(format, NodeReplace);
    //}

    //public static SeString FormatFishAlarmMessage(string format, FishAlarm alarm, long timeDiff)
    //{
    //    SeStringBuilder FishReplace(SeStringBuilder builder, string s)
    //        => s.ToLowerInvariant() switch
    //        {
    //            "{name}"        => builder.AddText(alarm.Name),
    //            "{offset}"      => builder.AddText(alarm.SecondOffset.ToString()),
    //            "{delaystring}" => builder.AddText(DelayString(timeDiff)),
    //            "{fishingspotname}" => AddFullMapLink(builder, alarm.Fish.Fish.FishingSpots.First().Name, alarm.Fish.Fish.FishingSpots.First().Territory,
    //                alarm.Fish.Fish.FishingSpots.First().XCoord,
    //                alarm.Fish.Fish.FishingSpots.First().YCoord),
    //            "{baitname}" => alarm.Fish.Fish.InitialBait.Id == 0
    //                ? builder.AddText("Unknown Bait")
    //                : builder.AddItemLink(alarm.Fish.Fish.InitialBait.Id, false),
    //            "{fishname}" => builder.AddItemLink(alarm.Fish.Fish.ItemId, false),
    //            _            => builder.AddText(s),
    //        };
    //
    //    return Format(format, FishReplace);
    //}
}
