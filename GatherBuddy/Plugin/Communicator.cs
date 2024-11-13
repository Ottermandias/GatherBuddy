using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Dalamud.Game;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using GatherBuddy.Alarms;
using GatherBuddy.Classes;
using GatherBuddy.Config;
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

    public static SeStringBuilder AddFullMapLink(this SeStringBuilder builder, string name, Territory territory, float xCoord, float yCoord,
        bool openMapLink = false, bool withCoordinates = true, float fudgeFactor = 0.05f)
    {
        var mapPayload = new MapLinkPayload(territory.Id, territory.Data.Map.RowId, xCoord, yCoord, fudgeFactor);
        if (openMapLink)
            Dalamud.GameGui.OpenMapWithMapLink(mapPayload);
        if (withCoordinates)
            name = $"{name} ({xCoord.ToString("00.0", CultureInfo.InvariantCulture)}, {yCoord.ToString("00.0", CultureInfo.InvariantCulture)})";
        return builder.AddUiForeground(0x0225)
            .AddUiGlow(0x0226)
            .Add(mapPayload)
            .AddUiForeground(500)
            .AddUiGlow(501)
            .AddText($"{(char)SeIconChar.LinkMarker}")
            .AddUiGlowOff()
            .AddUiForegroundOff()
            .AddText(name)
            .Add(RawPayload.LinkTerminator)
            .AddUiGlowOff()
            .AddUiForegroundOff();
    }

    public static SeStringBuilder AddFullItemLink(this SeStringBuilder builder, uint itemId, string itemName)
        => builder.AddItemLink(itemId, false, itemName);

    public static SeStringBuilder DelayString(this SeStringBuilder builder, TimeInterval uptime)
    {
        if (uptime.Start > GatherBuddy.Time.ServerTime)
            return builder.AddText("will be up in ")
                .AddColoredText(TimeInterval.DurationString(uptime.Start, GatherBuddy.Time.ServerTime, false),
                    GatherBuddy.Config.SeColorArguments);

        return builder.AddText("will be up for the next ")
            .AddColoredText(TimeInterval.DurationString(uptime.End, GatherBuddy.Time.ServerTime, false), GatherBuddy.Config.SeColorArguments);
    }
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
        Dalamud.Chat.Print(entry);
    }

    public static void PrintError(SeString message)
    {
        var entry = new XivChatEntry()
        {
            Message = message,
            Name    = SeString.Empty,
            Type    = GatherBuddy.Config.ChatTypeError,
        };
        Dalamud.Chat.Print(entry);
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

    public static void PrintClipboardMessage(string objectType, string name, Exception? e = null)
    {
        if (e != null)
        {
            name = name.Length > 0 ? name : "<Unnamed>";
            GatherBuddy.Log.Error($"Could not save {objectType}{name} to Clipboard:\n{e}");
            PrintError($"Could not save {objectType}", name, GatherBuddy.Config.SeColorNames, " to Clipboard.");
        }
        else if (GatherBuddy.Config.PrintClipboardMessages)
        {
            Print(objectType, name.Length > 0 ? name : "<Unnamed>", GatherBuddy.Config.SeColorNames, " saved to Clipboard.");
        }
    }

    public static void PrintUptime(TimeInterval uptime)
    {
        if (!GatherBuddy.Config.PrintUptime
         || uptime.Equals(TimeInterval.Always)
         || uptime.Equals(TimeInterval.Invalid)
         || uptime.Equals(TimeInterval.Never))
            return;

        if (uptime.Start > GatherBuddy.Time.ServerTime)
            Print("Next up in ",                     TimeInterval.DurationString(uptime.Start, GatherBuddy.Time.ServerTime, false),
                GatherBuddy.Config.SeColorArguments, ".");
        else
            Print("Currently up for the next ",      TimeInterval.DurationString(uptime.End, GatherBuddy.Time.ServerTime, false),
                GatherBuddy.Config.SeColorArguments, ".");
    }

    public static void PrintCoordinates(SeString link)
    {
        if (GatherBuddy.Config.WriteCoordinates)
            Print(link);
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


    public static void PrintIdentifiedItem(string name, IGatherable? item)
    {
        if (item == null)
        {
            Print("Could not find item corresponding to \"", name, GatherBuddy.Config.SeColorNames, "\".");
            GatherBuddy.Log.Verbose($"Could not find item corresponding to \"{name}\".");
            return;
        }

        if (GatherBuddy.Config.IdentifiedGatherableFormat.Length > 0)
            Print(FormatIdentifiedItemMessage(GatherBuddy.Config.IdentifiedGatherableFormat, name, item));
        GatherBuddy.Log.Verbose(Configuration.DefaultIdentifiedGatherableFormat, item.ItemId, item.Name[ClientLanguage.English], name);
    }

    public static void PrintAlarmMessage(Alarm alarm, ILocation location, TimeInterval uptime)
    {
        if (GatherBuddy.Config.AlarmFormat.Length > 0)
            Print(FormatAlarmMessage(GatherBuddy.Config.AlarmFormat, alarm, location, uptime));
        GatherBuddy.Log.Verbose(Configuration.DefaultAlarmFormat, alarm.Name, alarm.Item.Name[ClientLanguage.English], string.Empty,
            location.Name); // Duration string too ugly.
    }


    public static void LocationNotFound(IGatherable? item, GatheringType? type)
    {
        SeStringBuilder sb = new();
        sb.AddText("No associated location or attuned aetheryte found for ");
        if (item != null)
            sb.AddFullItemLink(item.ItemId, item.Name[GatherBuddy.Language]);
        else
            sb.AddColoredText("Unknown", GatherBuddy.Config.SeColorNames);

        if (type != null)
            sb.AddText(" with condition ")
                .AddColoredText(type.Value.ToString(), GatherBuddy.Config.SeColorArguments);
        sb.AddText(".");
        Print(sb.BuiltString);
        GatherBuddy.Log.Verbose(sb.BuiltString.TextValue);
    }

    public static void NoItemName(string command, string itemType)
    {
        PrintError(new SeStringBuilder().AddText($"Please supply a (partial) {itemType} name, ")
            .AddColoredText("alarm", GatherBuddy.Config.SeColorArguments)
            .AddText(" or ")
            .AddColoredText("next", GatherBuddy.Config.SeColorArguments)
            .AddText(" for ")
            .AddColoredText(command, GatherBuddy.Config.SeColorCommands)
            .AddText(".").BuiltString);
    }

    public static void NoBaitFound(Bait bait)
    {
        PrintError(new SeStringBuilder().AddText("Bait ")
            .AddFullItemLink(bait.Id, bait.Name)
            .AddText(" could not be equipped because you do not carry it.").BuiltString);
    }

    public static void NoGatherGroup(string groupName)
        => PrintError("The gather group ", groupName, GatherBuddy.Config.SeColorNames, " does not exist.");

    public static void NoGatherGroupItem(string groupName, int minute)
    {
        SeStringBuilder sb = new();
        sb.AddText("The gather group ")
            .AddColoredText(groupName, GatherBuddy.Config.SeColorNames)
            .AddText(" has no item corresponding to the eorzea time ")
            .AddColoredText($"{minute / RealTime.MinutesPerHour:D2}:{minute % RealTime.MinutesPerHour:D2}",
                GatherBuddy.Config.SeColorArguments)
            .AddText(".");
        PrintError(sb.BuiltString);
    }

    private static SeString FormatIdentifiedItemMessage(string format, string input, IGatherable item)
    {
        SeStringBuilder Replace(SeStringBuilder builder, string s)
            => s.ToLowerInvariant() switch
            {
                "{item}"  => builder.AddFullItemLink(item.ItemId, item.Name[GatherBuddy.Language]),
                "{input}" => builder.AddColoredText(input, GatherBuddy.Config.SeColorArguments),
                _         => builder.AddText(s),
            };

        return Format(format, Replace);
    }


    private static SeString FormatAlarmMessage(string format, Alarm alarm, ILocation location, TimeInterval uptime)
    {
        SeStringBuilder NodeReplace(SeStringBuilder builder, string s)
            => s.ToLowerInvariant() switch
            {
                "{alarm}"       => builder.AddColoredText(alarm.Name.Any() ? $"[{alarm.Name}]" : "[Alarm]", GatherBuddy.Config.SeColorNames),
                "{item}"        => builder.AddFullItemLink(alarm.Item.ItemId, alarm.Item.Name[GatherBuddy.Language]),
                "{offset}"      => builder.AddText(alarm.SecondOffset.ToString()),
                "{delaystring}" => builder.DelayString(uptime),
                "{location}" => builder.AddFullMapLink(location.Name, location.Territory, location.IntegralXCoord / 100f,
                    location.IntegralYCoord / 100f),
                _ => builder.AddText(s),
            };

        var msg = Format(format, NodeReplace);
        msg.Payloads.Insert(0, new UIForegroundPayload((ushort)GatherBuddy.Config.SeColorAlarm));
        msg.Payloads.Add(UIForegroundPayload.UIForegroundOff);
        return msg;
    }
}
