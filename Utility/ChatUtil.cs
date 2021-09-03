using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using GatherBuddy.Nodes;
using Lumina.Excel.GeneratedSheets;
using FishingSpot = GatherBuddy.Game.FishingSpot;

namespace GatherBuddy.Utility
{
    public static class ChatUtil
    {
        public delegate IReadOnlyList<Payload>? ReplacePlaceholder(string placeholder);

        public static TextPayload[] GetPayloadsFromString(string s)
            => new TextPayload[]
            {
                new(s),
            };

        public static List<Payload> CreateLink(Item item)
        {
            var payloadList = new List<Payload>
            {
                new UIForegroundPayload( ( ushort )( 0x223 + item.Rarity * 2 ) ),
                new UIGlowPayload( ( ushort )( 0x224       + item.Rarity * 2 ) ),
                new ItemPayload( item.RowId, false ),
                new UIForegroundPayload( 500 ),
                new UIGlowPayload( 501 ),
                new TextPayload( $"{( char )SeIconChar.LinkMarker}" ),
                new UIForegroundPayload( 0 ),
                new UIGlowPayload( 0 ),
                new TextPayload( item.Name ),
                new RawPayload( new byte[] { 0x02, 0x27, 0x07, 0xCF, 0x01, 0x01, 0x01, 0xFF, 0x01, 0x03 } ),
                new RawPayload( new byte[] { 0x02, 0x13, 0x02, 0xEC, 0x03 } ),
            };
            return payloadList;
        }

        private static void PrintPayloadList(List<Payload> payloads)
        {
            var payload = new SeString(payloads);
            Dalamud.Chat.PrintChat(new XivChatEntry
            {
                Message = payload,
            });
        }

        public static void LinkItem( Item item )
        {
            var payloadList = CreateLink(item);
            PrintPayloadList(payloadList);
        }

        public static void LinkItem(Item item, string message)
        {
            var payloadList = CreateLink(item);
            payloadList.Add(new TextPayload(message));
            PrintPayloadList(payloadList);
        }

        // Split a format string with '{text}' placeholders into a SeString with Payloads, 
        // and replace all placeholders by the returned payloads.
        public static SeString Format(string format, ReplacePlaceholder func)
        {
            List<Payload> payloads    = new();
            var           lastPayload = 0;
            var           openBracket = -1;
            for (var i = 0; i < format.Length; ++i)
            {
                if (format[i] == '{')
                {
                    openBracket = i;
                }
                else if (openBracket != -1 && format[i] == '}')
                {
                    payloads.Add(new TextPayload(format.Substring(lastPayload, openBracket - lastPayload)));
                    var placeholder = format.Substring(openBracket, i - openBracket + 1);
                    var replacementPayloads    = func(placeholder);
                    if (replacementPayloads != null)
                        payloads.AddRange(replacementPayloads);
                    else
                        payloads.Add(new TextPayload(placeholder));
                    lastPayload = i + 1;
                    openBracket = -1;
                }
            }

            if (lastPayload != format.Length)
                payloads.Add(new TextPayload(format.Substring(lastPayload)));
            return new SeString(payloads);
        }

        public static SeString CreateMapLink(string name, TerritoryType territory, float xCoord, float yCoord, bool openMapLink = false, float fudgeFactor = 0.05f)
        {
            var mapPayload = new MapLinkPayload(territory.RowId, territory.Map.Row, xCoord, yCoord, fudgeFactor);
            if (openMapLink)
                Dalamud.GameGui.OpenMapWithMapLink(mapPayload);
            name = $"{name} {mapPayload.CoordinateString}";
            var payloads = new List<Payload>
            {
                mapPayload,
                new UIForegroundPayload( 500 ),
                new UIGlowPayload( 501 ),
                new TextPayload( $"{( char )SeIconChar.LinkMarker}" ),
                new UIForegroundPayload( 0 ),
                new UIGlowPayload( 0 ),
                new TextPayload(name),
                RawPayload.LinkTerminator,
            };
            return new SeString(payloads);
        }

        public static SeString CreateMapLink(FishingSpot spot, bool openMapLink = false)
            => CreateMapLink(spot.PlaceName?[GatherBuddy.Language] ?? "Unknown", spot.Territory!.Data, spot.XCoord / 100.0f, spot.YCoord / 100.0f, openMapLink);

        public static SeString CreateNodeLink(Node node, bool openMapLink = false)
            => CreateMapLink(node.Nodes!.Territory!.Name[GatherBuddy.Language], node.Nodes!.Territory!.Data, (float) node.GetX(), (float) node.GetY(), openMapLink);
    }
}