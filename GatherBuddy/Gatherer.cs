using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Data;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.Managers;
using GatherBuddy.Nodes;
using GatherBuddy.Utility;
using ImGuiNET;
using FishingSpot = GatherBuddy.Game.FishingSpot;
using GatheringType = GatherBuddy.Enums.GatheringType;
using World = GatherBuddy.Managers.World;

namespace GatherBuddy
{
    public class Gatherer : IDisposable
    {
        private readonly CommandManager                 _commandManager;
        private readonly World                          _world;
        private readonly Dictionary<string, TimedGroup> _groups;
        public           NodeTimeLine                   Timeline { get; }
        public           AlarmManager                   Alarms   { get; }

        public FishManager FishManager
            => _world.Fish;

        public WeatherManager WeatherManager
            => _world.Weather;

        public Gatherer(CommandManager commandManager)
        {
            _commandManager = commandManager;
            _world          = new World();
            _groups         = GroupData.CreateGroups(_world.Nodes);
            Timeline        = new NodeTimeLine(_world.Nodes);
            Alarms          = new AlarmManager(_world.Nodes, _world.Fish, _world.Weather);
        }

        public void OnTerritoryChange(object? _, ushort territory)
            => _world.SetPlayerStreamCoords(territory);

        void IDisposable.Dispose()
        {
            Alarms.Dispose();
            _world.Nodes.Records.Dispose();
        }

        public void StartRecording()
            => _world.Nodes.Records.ActivateScanning();

        public void StopRecording()
            => _world.Nodes.Records.DeactivateScanning();

        public int Snapshot()
            => _world.Nodes.Records.Scan();

        public void PrintRecords()
            => _world.Nodes.Records.PrintToLog();

        public void PurgeRecord(uint nodeId)
            => _world.Nodes.Records.PurgeRecord(nodeId);

        public void PurgeAllRecords()
            => _world.Nodes.Records.PurgeRecords();

        private static SeString ReplaceFormatPlaceholders(string format, string input, Gatherable item)
        {
            IReadOnlyList<Payload>? Replace(string s)
            {
                if (!(s.StartsWith('{') && s.EndsWith('}')))
                    return null;

                return s switch
                {
                    "{Id}"    => ChatUtil.GetPayloadsFromString(item.ItemId.ToString()),
                    "{Name}"  => ChatUtil.CreateLink(item.ItemData),
                    "{Input}" => ChatUtil.GetPayloadsFromString(input),
                    _         => null,
                };
            }

            return ChatUtil.Format(format, Replace);
        }

        private static SeString ReplaceFormatPlaceholders(string format, string input, Fish fish)
        {
            IReadOnlyList<Payload>? Replace(string s)
            {
                if (!(s.StartsWith('{') && s.EndsWith('}')))
                    return null;

                return s switch
                {
                    "{Id}"    => ChatUtil.GetPayloadsFromString(fish.ItemId.ToString()),
                    "{Name}"  => ChatUtil.CreateLink(fish.ItemData),
                    "{Input}" => ChatUtil.GetPayloadsFromString(input),
                    _         => null,
                };
            }

            return ChatUtil.Format(format, Replace);
        }


        private static SeString ReplaceFormatPlaceholders(string format, string input, Fish fish, FishingSpot spot)
        {
            IReadOnlyList<Payload>? Replace(string s)
            {
                if (!(s.StartsWith('{') && s.EndsWith('}')))
                    return null;

                return s switch
                {
                    "{Id}"       => ChatUtil.GetPayloadsFromString(spot.Id.ToString()),
                    "{Name}"     => ChatUtil.CreateMapLink(spot).Payloads,
                    "{FishId}"   => ChatUtil.GetPayloadsFromString(fish.ItemId.ToString()),
                    "{FishName}" => ChatUtil.CreateLink(fish.ItemData),
                    "{Input}"    => ChatUtil.GetPayloadsFromString(input),
                    _            => null,
                };
            }

            return ChatUtil.Format(format, Replace);
        }

        private Gatherable? FindItemLogging(string itemName)
        {
            var item = _world.FindItemByName(itemName);
            if (item == null)
            {
                var output = $"Could not find corresponding item to \"{itemName}\".";
                Dalamud.Chat.Print(output);
                PluginLog.Verbose(output);
                return null;
            }

            if (GatherBuddy.Config.IdentifiedItemFormat.Length > 0)
                Dalamud.Chat.Print(ReplaceFormatPlaceholders(GatherBuddy.Config.IdentifiedItemFormat, itemName, item));
            PluginLog.Verbose(GatherBuddyConfiguration.DefaultIdentifiedItemFormat, item.ItemId, item.Name[GatherBuddy.Language], itemName);
            return item;
        }

        private Fish? FindFishLogging(string fishName)
        {
            var fish = _world.FindFishByName(fishName);
            if (fish == null)
            {
                var output = $"Could not find corresponding item to \"{fishName}\".";
                Dalamud.Chat.Print(output);
                PluginLog.Verbose(output);
                return null;
            }

            if (GatherBuddy.Config.IdentifiedFishFormat.Length > 0)
                Dalamud.Chat.Print(ReplaceFormatPlaceholders(GatherBuddy.Config.IdentifiedFishFormat, fishName, fish));
            PluginLog.Verbose(GatherBuddyConfiguration.DefaultIdentifiedFishFormat, fish.ItemId, fish!.Name![GatherBuddy.Language], fishName);
            return fish;
        }

        private Node? GetClosestNode(string itemName, GatheringType? type = null)
        {
            var item = FindItemLogging(itemName);
            if (item == null)
                return null;

            if (item.NodeList.Count == 0)
            {
                var output = $"Found no gathering nodes for item {item.ItemId}.";
                Dalamud.Chat.PrintError(output);
                PluginLog.Debug(output);
                return null;
            }

            var closestNode = _world.ClosestNodeForItem(item, type);
            if (closestNode?.GetValidAetheryte() == null)
            {
                if (type == null)
                {
                    Dalamud.Chat.PrintError(
                        $"No nodes containing {item.Name[GatherBuddy.Language]} have associated coordinates or aetheryte.");
                    Dalamud.Chat.PrintError(
                        "They will become available after encountering the respective node while having recording enabled.");
                }
                else
                {
                    Dalamud.Chat.PrintError(
                        $"No nodes containing {item.Name[GatherBuddy.Language]} for the specified job have been found.");
                }
            }

            if (!GatherBuddy.Config.PrintUptime || !(!closestNode?.Times.AlwaysUp() ?? false))
                return closestNode;

            var nextUptime = closestNode!.Times!.NextUptime();
            var now        = TimeStamp.UtcNow;
            if (nextUptime.Start > now)
            {
                var diff    = nextUptime.Start.AddMilliseconds(-now);
                var minutes = diff.CurrentMinuteOfDay;
                var seconds = diff.CurrentSecond;
                Dalamud.Chat.Print(minutes > 0
                    ? $"Node is up at {closestNode!.Times!.PrintHours()} (in {minutes}:{seconds:D2} Minutes)."
                    : $"Node is up at {closestNode!.Times!.PrintHours()} (in {seconds} Seconds).");
            }
            else
            {
                var diff    = nextUptime.End.AddMilliseconds(-now);
                var minutes = diff.CurrentMinuteOfDay;
                var seconds = diff.CurrentSecond;
                Dalamud.Chat.Print(minutes > 0
                    ? $"Node is up at {closestNode!.Times!.PrintHours()} (for the next {minutes}:{seconds:D2} Minutes)."
                    : $"Node is up at {closestNode!.Times!.PrintHours()} (for the next {seconds} Seconds).");
            }

            return closestNode;
        }

        private static async Task ExecuteTeleport(uint id)
        {
            Teleporter.Teleport(id);
            await Task.Delay(100);
        }

        private static async Task<bool> TeleportToNode(Node node)
        {
            if (!GatherBuddy.Config.UseTeleport)
                return true;

            var aetheryte = node.GetClosestAetheryte();
            if (aetheryte == null)
            {
                PluginLog.Debug("No valid aetheryte found for node {NodeId}.", node!.Meta!.PointBaseId);
                return false;
            }


            await ExecuteTeleport(aetheryte.Id);

            return true;
        }

        private static async Task<bool> TeleportToFishingSpot(FishingSpot spot)
        {
            if (!GatherBuddy.Config.UseTeleport)
                return true;

            var aetheryte = spot.ClosestAetheryte;
            if (aetheryte == null)
            {
                PluginLog.Debug("No valid aetheryte found for fishing spot {SpotId}.", spot.Id);
                return false;
            }

            await ExecuteTeleport(aetheryte.Id);

            return true;
        }

        private async Task<bool> EquipForNode(Node node)
        {
            if (!GatherBuddy.Config.UseGearChange)
                return true;

            if (node.Meta!.IsBotanist())
            {
                _commandManager.Execute($"/gearset change {GatherBuddy.Config.BotanistSetName}");
                await Task.Delay(200);
            }
            else if (node.Meta!.IsMiner())
            {
                _commandManager.Execute($"/gearset change {GatherBuddy.Config.MinerSetName}");
                await Task.Delay(200);
            }
            else
            {
                PluginLog.Debug("No job type set for node {NodeId}.", node.Meta.PointBaseId);
                return false;
            }

            return true;
        }

        private async Task EquipFisher()
        {
            if (!GatherBuddy.Config.UseGearChange)
                return;

            _commandManager.Execute($"/gearset change {GatherBuddy.Config.FisherSetName}");
            await Task.Delay(200);
        }

        private static async Task<bool> SetNodeFlag(Node node)
        {
            // Coordinates = 0.0 are acceptable because of the diadem, so no error message.
            if (!(GatherBuddy.Config.WriteCoordinates || GatherBuddy.Config.UseCoordinates) || node.GetX() == 0.0 || node.GetY() == 0.0)
                return true;

            if (node.Nodes!.Territory == null)
            {
                PluginLog.Debug("No territory set for node {NodeId}.", node.Meta!.PointBaseId);
                return false;
            }

            var link = ChatUtil.CreateNodeLink(node, GatherBuddy.Config.UseCoordinates);
            if (GatherBuddy.Config.WriteCoordinates)
                Dalamud.Chat.Print(link);
            await Task.Delay(100);
            return true;
        }

        private static async Task<bool> SetFishingSpotFlag(FishingSpot spot)
        {
            if (!GatherBuddy.Config.UseCoordinates)
                return true;

            if (spot.Territory == null)
            {
                PluginLog.Debug("No territory set for node {SpotId}.", spot.Id);
                return false;
            }

            ChatUtil.CreateMapLink(spot, GatherBuddy.Config.UseCoordinates);
            await Task.Delay(100);
            return true;
        }

        public async void OnGatherActionWithNode(Node node)
        {
            try
            {
                if (await EquipForNode(node) == false)
                    return;
                if (await TeleportToNode(node) == false)
                    return;
                if (await SetNodeFlag(node) == false)
                    return;
            }
            catch (Exception e)
            {
                PluginLog.Error($"Exception caught: {e}");
            }
        }

        public async void OnFishActionWithSpot(FishingSpot spot)
        {
            try
            {
                await EquipFisher();

                if (await TeleportToFishingSpot(spot) == false)
                    return;
                if (await SetFishingSpotFlag(spot) == false)
                    return;
            }
            catch (Exception e)
            {
                PluginLog.Error($"Exception caught: {e}");
            }
        }

        public void OnBaitAction(string baitName = "")
        {
            ImGui.SetClipboardText(baitName);
            Dalamud.Chat.Print($"Copied [{baitName}] to clipboard.");
        }

        public void OnFishActionWithFish(Fish? fish, string fishName = "")
        {
            var closestSpot = _world.ClosestSpotForItem(fish);
            if (closestSpot == null)
            {
                var outputError = $"Could not find fishing spot for \"{fish!.Name![GatherBuddy.Language]}\".";
                Dalamud.Chat.PrintError(outputError);
                PluginLog.Error(outputError);
                return;
            }

            if (GatherBuddy.Config.IdentifiedFishingSpotFormat.Length > 0)
                Dalamud.Chat.Print(ReplaceFormatPlaceholders(GatherBuddy.Config.IdentifiedFishingSpotFormat, fishName, fish!, closestSpot));
            if (GatherBuddy.Config.PrintGigHead && fish!.IsSpearFish)
                Dalamud.Chat.Print($"Use {(fish.Gig != GigHead.Unknown ? fish.Gig : fish.CatchData?.GigHead ?? GigHead.Unknown)} gig head.");
            PluginLog.Verbose(GatherBuddyConfiguration.DefaultIdentifiedFishingSpotFormat, closestSpot.PlaceName![GatherBuddy.Language],
                fish!.Name![GatherBuddy.Language]);

            OnFishActionWithSpot(closestSpot);
        }

        public void OnFishAction(string fishName)
        {
            if (Util.CompareCi(fishName, "alarm"))
            {
                var fish = Alarms.LastFishAlarm?.Fish;
                if (fish == null)
                {
                    Dalamud.Chat.PrintError("No active alarm was triggered, yet.");
                }
                else
                {
                    var itemLink = ChatUtil.CreateLink(fish.ItemData);
                    itemLink.Insert(0, new TextPayload($"Teleporting to [Alarm {Alarms.LastFishAlarm!.Name}] ("));
                    itemLink.Add(new TextPayload(")."));

                    Dalamud.Chat.Print(new SeString(itemLink));
                    OnFishActionWithFish(fish);
                }
            }
            else
            {
                var fish = FindFishLogging(fishName);
                OnFishActionWithFish(fish, fishName);
            }
        }

        public void OnGatherAction(string itemName, GatheringType? type = null)
        {
            try
            {
                if (Util.CompareCi(itemName, "alarm"))
                {
                    var node = Alarms.LastNodeAlarm?.Node;
                    if (node == null)
                    {
                        Dalamud.Chat.PrintError("No active alarm was triggered, yet.");
                    }
                    else
                    {
                        List<Payload> text = new(2)
                        {
                            new TextPayload($"Teleporting to [Alarm {Alarms.LastNodeAlarm!.Name}] ({node.Times!.PrintHours()}):\n"),
                        };
                        foreach (var item in node.Items!.ActualItems)
                        {
                            text.AddRange(ChatUtil.CreateLink(item.ItemData));
                            text.Add(new TextPayload(", "));
                        }

                        ((TextPayload)text.Last())!.Text = ".";
                        Dalamud.Chat.Print(new SeString(text));
                        OnGatherActionWithNode(node);
                    }
                }
                else
                {
                    var closestNode = GetClosestNode(itemName, type);
                    if (closestNode == null)
                        return;

                    OnGatherActionWithNode(closestNode);
                }
            }
            catch (Exception e)
            {
                PluginLog.Error($"Exception caught: {e}");
            }
        }

        public async void OnGroupGatherAction(string groupName, int minuteOffset)
        {
            try
            {
                if (groupName.Length == 0)
                {
                    TimedGroup.PrintHelp(Dalamud.Chat, _groups);
                    return;
                }

                if (!_groups.TryGetValue(groupName, out var group))
                {
                    Dalamud.Chat.PrintError($"\"{groupName}\" is not a valid group.");
                    return;
                }

                var currentHour = TimeStamp.UtcNow.AddEorzeaMinutes(minuteOffset).CurrentEorzeaHour();
                var (node, desc) = group.CurrentNode((uint) currentHour);
                if (node == null)
                {
                    PluginLog.Debug("No node for hour {CurrentHour} set in group {Name}.", currentHour, group.Name);
                    return;
                }

                if (await EquipForNode(node) == false)
                    return;
                if (await TeleportToNode(node) == false)
                    return;
                if (await SetNodeFlag(node) == false)
                    return;

                if (desc == null)
                    return;

                if (!GatherBuddy.Config.UseCoordinates && node.Meta!.NodeType == NodeType.Regular)
                    Dalamud.Chat.Print(
                        $"Gather [{desc}] at coordinates ({node.GetX():F2} | {node.GetY():F2}).");
                else
                    Dalamud.Chat.Print($"Gather [{desc}].");
            }
            catch (Exception e)
            {
                PluginLog.Error($"Exception caught: {e}");
            }
        }

        public void PurgeRecords(string itemName)
        {
            var item = FindItemLogging(itemName);
            if (item == null)
                return;

            if (item.NodeList.Count == 0)
            {
                var text = ChatUtil.CreateLink(item.ItemData);
                text.Insert(0, new TextPayload("Found no gathering nodes for item "));
                text.Add(new TextPayload("."));
                var output = new SeString(text);
                Dalamud.Chat.PrintError(output);
                PluginLog.Debug(output.ToString());
                return;
            }

            foreach (var (id, location) in item.NodeList
                         .SelectMany(baseNode => baseNode.Nodes!.Nodes
                             .Where(loc => loc.Value != null)))
            {
                if (location!.Locations.Count > 0)
                    PluginLog.Information("[NodeRecorder] Purged all records for node {Key} containing item {Value}.", id, location);
                location!.Clear();
            }
        }

        public void DumpAetherytes()
        {
            foreach (var a in _world.Aetherytes.Aetherytes)
            {
                PluginLog.Information(
                    $"[AetheryteDump] |{a.Id}|{a.Name}|{a.Territory.Id}|{a.XCoord:F2}|{a.YCoord:F2}|{a.XStream}|{a.YStream}|");
            }
        }

        public void DumpTerritories()
        {
            foreach (var t in _world.Territories.Territories.Values)
            {
                PluginLog.Information(
                    $"[TerritoryDump] |{t.Id}|{t.Name}|{t.Region}|{t.XStream}|{t.YStream}|{string.Join("|", t.Aetherytes.Select(a => a.Id))}|");
            }
        }

        public void DumpItems()
        {
            foreach (var i in _world.Items.Items)
            {
                PluginLog.Information(
                    $"[ItemDump] |{i.ItemId}|{i.GatheringId}|{i.Name}|{i.Level}{i.StarsString()}|{string.Join("|", i.NodeList.Select(n => n.Meta!.PointBaseId))}|");
            }
        }

        public void DumpNodes()
        {
            foreach (var n in _world.Nodes.BaseNodes())
            {
                PluginLog.Information(
                    $"[NodeDump] |{string.Join(",", n.Nodes!.Nodes.Keys)}|{n.Meta!.PointBaseId}|{n.Meta.GatheringType}|{n.Meta.NodeType}|{n.Meta.Level}|{n.GetX()}|{n.GetY()}|{n.Nodes!.Territory!.Id}|{n.PlaceNameEn}|{n.GetClosestAetheryte()?.Id ?? 0}|{n.Times!.UptimeTable()}|{n.Items!.PrintItems()}");
            }
        }

        public void DumpFishingSpots()
        {
            foreach (var f in _world.Fish.FishingSpots.Values)
            {
                PluginLog.Information(
                    $"[FishingSpotDump] |{f.Id}|{f.XCoord}|{f.YCoord}|{f.Radius}|{f.PlaceName}|{f.Territory?.Name ?? "MISSING"}|{f.ClosestAetheryte?.Name ?? "MISSING"}|{string.Join("|", f.Items.Where(i => i != null).Select(i => i!.ItemId))}");
            }
        }

        public void DumpFish()
        {
            foreach (var f in _world.Fish.Fish.Values.OrderBy((f, g) => f.ItemId.CompareTo(g.ItemId)))
                PluginLog.Information($"[FishDump] |{f.ItemId}|{f.Name}|");
        }
    }
}
