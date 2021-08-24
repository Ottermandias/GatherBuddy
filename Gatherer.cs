using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dalamud;
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
        private          ClientLanguage                    _teleporterLanguage;
        private          FileSystemWatcher?                _teleporterWatcher;
        private readonly CommandManager                    _commandManager;
        private readonly World                             _world;
        private readonly Dictionary<string, TimedGroup>    _groups;
        public           NodeTimeLine                      Timeline { get; }
        public           AlarmManager                      Alarms   { get; }

        public FishManager FishManager
            => _world.Fish;

        public WeatherManager WeatherManager
            => _world.Weather;

        public void TryCreateTeleporterWatcher(bool useTeleport)
        {
            const string teleporterPluginConfigFile = "TeleporterPlugin.json";

            _teleporterLanguage = GatherBuddy.Language;
            if (!useTeleport || _teleporterWatcher != null)
            {
                _teleporterWatcher?.Dispose();
                _teleporterWatcher = null;
                return;
            }

            var dir = new DirectoryInfo(GatherBuddy.PluginInterface.GetPluginConfigDirectory());
            if (!dir.Exists || (dir.Parent?.Exists ?? false))
                return;

            dir = dir.Parent;

            var file = new FileInfo(Path.Combine(dir!.FullName, teleporterPluginConfigFile));
            if (file.Exists)
                ParseTeleporterFile(file.FullName);

            void OnTeleporterConfigChange(object source, FileSystemEventArgs args)
            {
                PluginLog.Verbose("Reloading Teleporter Config.");
                if (args.ChangeType != WatcherChangeTypes.Changed && args.ChangeType != WatcherChangeTypes.Created)
                    return;

                ParseTeleporterFile(args.FullPath);
            }

            _teleporterWatcher = new FileSystemWatcher
            {
                Path         = dir.FullName,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter       = teleporterPluginConfigFile,
            };
            _teleporterWatcher.Changed              += OnTeleporterConfigChange;
            _teleporterWatcher!.EnableRaisingEvents =  true;
        }

        private void ParseTeleporterFile(string filePath)
        {
            try
            {
                const string teleporterLanguageString = "\"teleporterlanguage\":";

                var content = File.ReadAllText(filePath).ToLowerInvariant();
                var idx     = content.IndexOf(teleporterLanguageString, StringComparison.Ordinal);
                if (idx < 0)
                    return;

                content = content.Substring(idx + teleporterLanguageString.Length).Trim();
                if (content.Length < 1)
                    return;

                _teleporterLanguage = content[0] switch
                {
                    '0' => ClientLanguage.Japanese,
                    '1' => ClientLanguage.English,
                    '2' => ClientLanguage.German,
                    '3' => ClientLanguage.French,
                    _   => GatherBuddy.Language,
                };
            }
            catch (Exception e)
            {
                PluginLog.Error($"Could not read Teleporter Config:\n{e}");
                _teleporterLanguage = GatherBuddy.Language;
            }
        }

        public Gatherer(CommandManager commandManager)
        {
            _commandManager = commandManager;
            _world          = new World();
            _groups         = GroupData.CreateGroups(_world.Nodes);
            Timeline        = new NodeTimeLine(_world.Nodes);
            Alarms          = new AlarmManager(_world.Nodes, _world.Fish, _world.Weather);
            TryCreateTeleporterWatcher(GatherBuddy.Config.UseTeleport);
        }

        public void OnTerritoryChange(object? _, ushort territory)
            => _world.SetPlayerStreamCoords(territory);

        void IDisposable.Dispose()
        {
            Alarms.Dispose();
            _teleporterWatcher?.Dispose();
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

        private static string ReplaceFormatPlaceholders(string format, string input, Gatherable item)
        {
            var result = format.Replace("{Id}", item.ItemId.ToString());
            result = result.Replace("{Name}",  item.Name[GatherBuddy.Language]);
            result = result.Replace("{Input}", input);
            return result;
        }

        private static string ReplaceFormatPlaceholders(string format, string input, Fish fish)
        {
            var result = format.Replace("{Id}", fish.ItemId.ToString());
            result = result.Replace("{Name}",  fish.Name![GatherBuddy.Language]);
            result = result.Replace("{Input}", input);
            return result;
        }

        private static string ReplaceFormatPlaceholders(string format, string input, Fish fish, FishingSpot spot)
        {
            var result = format.Replace("{Id}", spot.Id.ToString());
            result = result.Replace("{Name}",     spot.PlaceName![GatherBuddy.Language]);
            result = result.Replace("{FishName}", fish.Name![GatherBuddy.Language]);
            result = result.Replace("{FishId}",   fish.ItemId.ToString());
            result = result.Replace("{Input}",    input);
            return result;
        }

        private Gatherable? FindItemLogging(string itemName)
        {
            var item = _world.FindItemByName(itemName);
            if (item == null)
            {
                string output = $"Could not find corresponding item to \"{itemName}\".";
                GatherBuddy.Chat.Print(output);
                PluginLog.Verbose(output);
                return null;
            }

            if (GatherBuddy.Config.IdentifiedItemFormat.Length > 0)
                GatherBuddy.Chat.Print(ReplaceFormatPlaceholders(GatherBuddy.Config.IdentifiedItemFormat, itemName, item));
            PluginLog.Verbose(GatherBuddyConfiguration.DefaultIdentifiedItemFormat, item.ItemId, item.Name[GatherBuddy.Language], itemName);
            return item;
        }

        private Fish? FindFishLogging(string fishName)
        {
            var fish = _world.FindFishByName(fishName);
            if (fish == null)
            {
                string output = $"Could not find corresponding item to \"{fishName}\".";
                GatherBuddy.Chat.Print(output);
                PluginLog.Verbose(output);
                return null;
            }

            if (GatherBuddy.Config.IdentifiedFishFormat.Length > 0)
                GatherBuddy.Chat.Print(ReplaceFormatPlaceholders(GatherBuddy.Config.IdentifiedFishFormat, fishName, fish));
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
                GatherBuddy.Chat.PrintError(output);
                PluginLog.Debug(output);
                return null;
            }

            var closestNode = _world.ClosestNodeForItem(item, type);
            if (closestNode?.GetValidAetheryte() == null)
            {
                if (type == null)
                {
                    GatherBuddy.Chat.PrintError(
                        $"No nodes containing {item.Name[GatherBuddy.Language]} have associated coordinates or aetheryte.");
                    GatherBuddy.Chat.PrintError(
                        "They will become available after encountering the respective node while having recording enabled.");
                }
                else
                {
                    GatherBuddy.Chat.PrintError(
                        $"No nodes containing {item.Name[GatherBuddy.Language]} for the specified job have been found.");
                }
            }

            if (GatherBuddy.Config.PrintUptime && (!closestNode?.Times.AlwaysUp() ?? false))
            {
                var nextUptime = closestNode!.Times!.NextRealUptime();
                var now        = DateTime.UtcNow;
                if (nextUptime.Time > now)
                {
                    var diff = nextUptime.Time - now;
                    if (diff.Minutes > 0)
                        GatherBuddy.Chat.Print($"Node is up at {closestNode!.Times!.PrintHours()} (in {diff.Minutes} Minutes).");
                    else
                        GatherBuddy.Chat.Print($"Node is up at {closestNode!.Times!.PrintHours()} (in {diff.Seconds} Seconds).");
                }
                else
                {
                    var diff = nextUptime.EndTime - now;
                    if (diff.Minutes > 0)
                        GatherBuddy.Chat.Print($"Node is up at {closestNode!.Times!.PrintHours()} (for the next {diff.Minutes} Minutes).");
                    else
                        GatherBuddy.Chat.Print($"Node is up at {closestNode!.Times!.PrintHours()} (for the next {diff.Seconds} Seconds).");
                }
            }

            return closestNode;
        }

        private async Task ExecuteTeleport(string name)
        {
            if (!_commandManager.Execute("/tp " + name))
            {
                GatherBuddy.Chat.PrintError(
                    "It seems like you have activated teleporting, but you have not installed the required plugin Teleporter by Pohky.");
                GatherBuddy.Chat.PrintError("Please either deactivate teleporting or install the plugin.");
            }

            await Task.Delay(100);
        }

        private async Task<bool> TeleportToNode(Node node)
        {
            if (!GatherBuddy.Config.UseTeleport)
                return true;

            var name = node.GetClosestAetheryte()?.Name[_teleporterLanguage] ?? "";
            if (name.Length == 0)
            {
                PluginLog.Debug("No valid aetheryte found for node {NodeId}.", node!.Meta!.PointBaseId);
                return false;
            }

            await ExecuteTeleport(name);

            return true;
        }

        private async Task<bool> TeleportToFishingSpot(FishingSpot spot)
        {
            if (!GatherBuddy.Config.UseTeleport)
                return true;

            var name = spot.ClosestAetheryte?.Name[_teleporterLanguage] ?? "";
            if (name.Length == 0)
            {
                PluginLog.Debug("No valid aetheryte found for fishing spot {SpotId}.", spot.Id);
                return false;
            }

            await ExecuteTeleport(name);

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

        private async Task ExecuteMapMarker(string x, string y, string territory)
        {
            if (!_commandManager.Execute($"/coord {x}, {y} : {territory}"))
            {
                GatherBuddy.Chat.PrintError(
                    "It seems like you have activated map markers, but you have not installed the required plugin ChatCoordinates by kij.");
                GatherBuddy.Chat.PrintError("Please either deactivate map markers or install the plugin.");
            }

            await Task.Delay(100);
        }

        private async Task<bool> SetNodeFlag(Node node)
        {
            // Coordinates = 0.0 are acceptable because of the diadem, so no error message.
            if (!GatherBuddy.Config.UseCoordinates || node.GetX() == 0.0 || node.GetY() == 0.0)
                return true;

            var xString   = node.GetX().ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            var yString   = node.GetY().ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            var territory = node.Nodes!.Territory?.Name[GatherBuddy.Language] ?? "";

            if (territory.Length == 0)
            {
                PluginLog.Debug("No territory set for node {NodeId}.", node.Meta!.PointBaseId);
                return false;
            }

            await ExecuteMapMarker(xString, yString, territory);

            await Task.Delay(100);
            return true;
        }

        private async Task<bool> SetFishingSpotFlag(FishingSpot spot)
        {
            if (!GatherBuddy.Config.UseCoordinates)
                return true;


            var xString   = (spot.XCoord / 100.0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            var yString   = (spot.YCoord / 100.0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            var territory = spot.Territory?.Name[GatherBuddy.Language] ?? "";

            if (territory.Length == 0)
            {
                PluginLog.Debug("No territory set for node {SpotId}.", spot.Id);
                return false;
            }

            await ExecuteMapMarker(xString, yString, territory);

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
            GatherBuddy.Chat.Print($"Copied [{baitName}] to clipboard.");
        }

        public void OnFishActionWithFish(Fish? fish, string fishName = "")
        {
            var closestSpot = _world.ClosestSpotForItem(fish);
            if (closestSpot == null)
            {
                var outputError = $"Could not find fishing spot for \"{fish!.Name![GatherBuddy.Language]}\".";
                GatherBuddy.Chat.PrintError(outputError);
                PluginLog.Error(outputError);
                return;
            }

            if (GatherBuddy.Config.IdentifiedFishingSpotFormat.Length > 0)
                GatherBuddy.Chat.Print(ReplaceFormatPlaceholders(GatherBuddy.Config.IdentifiedFishingSpotFormat, fishName, fish!, closestSpot));
            if (GatherBuddy.Config.PrintGigHead && fish!.IsSpearFish)
                GatherBuddy.Chat.Print($"Use {(fish.Gig != GigHead.Unknown ? fish.Gig : fish.CatchData?.GigHead ?? GigHead.Unknown)} gig head.");
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
                    GatherBuddy.Chat.PrintError("No active alarm was triggered, yet.");
                }
                else
                {
                    GatherBuddy.Chat.Print($"Teleporting to [Alarm {Alarms.LastFishAlarm!.Name}] ({fish.Name[GatherBuddy.Language]}):");
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
                        GatherBuddy.Chat.PrintError("No active alarm was triggered, yet.");
                    }
                    else
                    {
                        GatherBuddy.Chat.Print($"Teleporting to [Alarm {Alarms.LastNodeAlarm!.Name}] ({node.Times!.PrintHours()}):");
                        GatherBuddy.Chat.Print(node.Items!.PrintItems(", ", GatherBuddy.Language) + '.');
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
                    TimedGroup.PrintHelp(GatherBuddy.Chat, _groups);
                    return;
                }

                if (!_groups.TryGetValue(groupName, out var group))
                {
                    GatherBuddy.Chat.PrintError($"\"{groupName}\" is not a valid group.");
                    return;
                }

                var currentHour = EorzeaTime.CurrentHourOfDay(minuteOffset);
                var (node, desc) = group.CurrentNode(currentHour);
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
                    GatherBuddy.Chat.Print(
                        $"Gather [{desc}] at coordinates ({node.GetX():F2} | {node.GetY():F2}).");
                else
                    GatherBuddy.Chat.Print($"Gather [{desc}].");
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
                var output = $"Found no gathering nodes for item {item.ItemId}.";
                GatherBuddy.Chat.PrintError(output);
                PluginLog.Debug(output);
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
