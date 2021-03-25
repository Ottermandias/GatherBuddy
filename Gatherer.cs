using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dalamud;
using Dalamud.Plugin;
using GatherBuddy.Classes;
using GatherBuddy.Managers;
using GatherBuddy.Utility;

namespace GatherBuddy
{
    public class Gatherer : IDisposable
    {
        private          ClientLanguage                    _teleporterLanguage;
        private          FileSystemWatcher?                _teleporterWatcher;
        private readonly ClientLanguage                    _language;
        private readonly Dalamud.Game.Internal.Gui.ChatGui _chat;
        private readonly CommandManager                    _commandManager;
        private readonly World                             _world;
        private readonly Dictionary<string, TimedGroup>    _groups;
        private readonly GatherBuddyConfiguration          _configuration;
        public           NodeTimeLine                      Timeline { get; }
        public           AlarmManager                      Alarms   { get; }

        public FishManager FishManager
            => _world.Fish;

        public void TryCreateTeleporterWatcher(DalamudPluginInterface pi, bool useTeleport)
        {
            const string teleporterPluginConfigFile = "TeleporterPlugin.json";

            _teleporterLanguage = _language;
            if (!useTeleport || _teleporterWatcher != null)
            {
                _teleporterWatcher?.Dispose();
                _teleporterWatcher = null;
                return;
            }

            var dir = new DirectoryInfo(pi.GetPluginConfigDirectory());
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
                var idx     = content.IndexOf(teleporterLanguageString);
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
                    _   => _language,
                };
            }
            catch (Exception e)
            {
                PluginLog.Error($"Could not read Teleporter Config:\n{e}");
                _teleporterLanguage = _language;
            }
        }

        public Gatherer(DalamudPluginInterface pi, GatherBuddyConfiguration config, CommandManager commandManager)
        {
            _commandManager = commandManager;
            _chat           = pi.Framework.Gui.Chat;
            _language       = pi.ClientState.ClientLanguage;
            _configuration  = config;
            _world          = new World(pi, _configuration);
            _groups         = TimedGroup.CreateGroups(_world);
            Timeline        = new NodeTimeLine(_world.Nodes);
            Alarms          = new AlarmManager(pi, _world.Nodes, _configuration);
            TryCreateTeleporterWatcher(pi, _configuration.UseTeleport);
        }

        public void OnTerritoryChange(object sender, ushort territory)
            => _world.SetPlayerStreamCoords(territory);

        public void Dispose()
        {
            Alarms?.Dispose();
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

        private string ReplaceFormatPlaceholders(string format, string input, Gatherable item)
        {
            var result = format.Replace("{Id}", item.ItemId.ToString());
            result = result.Replace("{Name}",  item.NameList[_language]);
            result = result.Replace("{Input}", input);
            return result;
        }

        private string ReplaceFormatPlaceholders(string format, string input, Fish fish)
        {
            var result = format.Replace("{Id}", fish.Id.ToString());
            result = result.Replace("{Name}",  fish.Name![_language]);
            result = result.Replace("{Input}", input);
            return result;
        }

        private string ReplaceFormatPlaceholders(string format, string input, Fish fish, FishingSpot spot)
        {
            var result = format.Replace("{Id}", spot.Id.ToString());
            result = result.Replace("{Name}",     spot.PlaceName![_language]);
            result = result.Replace("{FishName}", fish.Name![_language]);
            result = result.Replace("{FishId}",   fish.Id.ToString());
            result = result.Replace("{Input}",    input);
            return result;
        }

        private Gatherable? FindItemLogging(string itemName)
        {
            var item = _world.FindItemByName(itemName);
            if (item == null)
            {
                string output = $"Could not find corresponding item to \"{itemName}\".";
                _chat.Print(output);
                PluginLog.Verbose(output);
                return null;
            }

            if (_configuration.IdentifiedItemFormat.Length > 0)
                _chat.Print(ReplaceFormatPlaceholders(_configuration.IdentifiedItemFormat, itemName, item));
            PluginLog.Verbose(GatherBuddyConfiguration.DefaultIdentifiedItemFormat, item.ItemId, item.NameList[_language], itemName);
            return item;
        }

        private Fish? FindFishLogging(string fishName)
        {
            var fish = _world.FindFishByName(fishName);
            if (fish == null)
            {
                string output = $"Could not find corresponding item to \"{fishName}\".";
                _chat.Print(output);
                PluginLog.Verbose(output);
                return null;
            }
            if (_configuration.IdentifiedFishFormat.Length > 0)
                _chat.Print(ReplaceFormatPlaceholders(_configuration.IdentifiedFishFormat, fishName, fish));
            PluginLog.Verbose(GatherBuddyConfiguration.DefaultIdentifiedFishFormat, fish.Id, fish!.Name![_language], fishName);
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
                _chat.PrintError(output);
                PluginLog.Debug(output);
                return null;
            }

            var closestNode = _world.ClosestNodeForItem(item, type);
            if (closestNode?.GetValidAetheryte() == null)
            {
                if (type == null)
                {
                    _chat.PrintError(
                        $"No nodes containing {item.NameList[_language]} have associated coordinates or aetheryte.");
                    _chat.PrintError(
                        "They will become available after encountering the respective node while having recording enabled.");
                }
                else
                {
                    _chat.PrintError(
                        $"No nodes containing {item.NameList[_language]} for the specified job have been found.");
                }
            }

            if (_configuration.PrintUptime && (!closestNode?.Times?.AlwaysUp() ?? false))
                _chat.Print($"Node is up at {closestNode!.Times!.PrintHours()}.");

            return closestNode;
        }

        private async Task ExecuteTeleport(string name)
        {
            if (!_commandManager.Execute("/tp " + name))
            {
                _chat.PrintError(
                    "It seems like you have activated teleporting, but you have not installed the required plugin Teleporter by Pohky.");
                _chat.PrintError("Please either deactivate teleporting or install the plugin.");
            }

            await Task.Delay(100);
        }

        private async Task<bool> TeleportToNode(Node node)
        {
            if (!_configuration.UseTeleport)
                return true;

            var name = node?.GetClosestAetheryte()?.NameList[_teleporterLanguage] ?? "";
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
            if (!_configuration.UseTeleport)
                return true;

            var name = spot.ClosestAetheryte?.NameList[_teleporterLanguage] ?? "";
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
            if (!_configuration.UseGearChange)
                return true;

            if (node.Meta!.IsBotanist())
            {
                _commandManager.Execute($"/gearset change {_configuration?.BotanistSetName ?? "BOT"}");
                await Task.Delay(200);
            }
            else if (node.Meta!.IsMiner())
            {
                _commandManager.Execute($"/gearset change {_configuration?.MinerSetName ?? "MIN"}");
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
            if (!_configuration.UseGearChange)
                return;

            _commandManager.Execute($"/gearset change {_configuration?.FisherSetName ?? "FSH"}");
            await Task.Delay(200);
        }

        private async Task ExecuteMapMarker(string x, string y, string territory)
        {
            if (!_commandManager.Execute($"/coord {x}, {y} : {territory}"))
            {
                _chat.PrintError(
                    "It seems like you have activated map markers, but you have not installed the required plugin ChatCoordinates by kij.");
                _chat.PrintError("Please either deactivate map markers or install the plugin.");
            }

            await Task.Delay(100);
        }

        private async Task<bool> SetNodeFlag(Node node)
        {
            // Coordinates = 0.0 are acceptable because of the diadem, so no error message.
            if (!_configuration.UseCoordinates || node.GetX() == 0.0 || node.GetY() == 0.0)
                return true;

            var xString   = node.GetX().ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            var yString   = node.GetY().ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            var territory = node.Nodes!.Territory?.NameList?[_language] ?? "";

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
            if (!_configuration.UseCoordinates)
                return true;


            var xString   = (spot.XCoord / 100.0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            var yString   = (spot.YCoord / 100.0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            var territory = spot.Territory?.NameList[_language] ?? "";

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

        private async void OnFishActionWithSpot(FishingSpot spot)
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

        public void OnFishAction(string fishName)
        {
            var fish        = FindFishLogging(fishName);
            var closestSpot = _world.ClosestSpotForItem(fish);
            if (closestSpot == null)
            {
                var outputError = $"Could not find fishing spot for \"{fish!.Name![_language]}\".";
                _chat.PrintError(outputError);
                PluginLog.Error(outputError);
                return;
            }

            if (_configuration.IdentifiedFishingSpotFormat.Length > 0)
                _chat.Print(ReplaceFormatPlaceholders(_configuration.IdentifiedFishingSpotFormat, fishName, fish!, closestSpot));
            PluginLog.Verbose(GatherBuddyConfiguration.DefaultIdentifiedFishingSpotFormat, closestSpot.PlaceName![_language], fish!.Name![_language]);

            OnFishActionWithSpot(closestSpot);
        }

        public void OnGatherAction(string itemName, GatheringType? type = null)
        {
            try
            {
                if (Util.CompareCi(itemName, "alarm"))
                {
                    var node = Alarms.LastAlarm?.Node;
                    if (node == null)
                    {
                        _chat.PrintError("No active alarm was triggered, yet.");
                    }
                    else
                    {
                        _chat.Print($"Teleporting to [Alarm {Alarms.LastAlarm!.Name}] ({node.Times!.PrintHours()}):");
                        _chat.Print(node.Items!.PrintItems(", ", _language) + '.');
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
                    TimedGroup.PrintHelp(_chat, _groups);
                    return;
                }

                if (!_groups.TryGetValue(groupName, out var group))
                {
                    _chat.PrintError($"\"{groupName}\" is not a valid group.");
                    return;
                }

                var currentHour = EorzeaTime.CurrentHours(minuteOffset);
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

                if (!_configuration.UseCoordinates && node.Meta!.NodeType == NodeType.Regular)
                    _chat.Print(
                        $"Gather [{desc}] at coordinates ({node.GetX():F2} | {node.GetY():F2}).");
                else
                    _chat.Print($"Gather [{desc}].");
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
                _chat.PrintError(output);
                PluginLog.Debug(output);
                return;
            }

            foreach (var loc in item.NodeList
                .SelectMany(baseNode => baseNode.Nodes!.Nodes
                    .Where(loc => loc.Value != null)))
            {
                if (loc.Value!.Locations.Count > 0)
                    PluginLog.Information("[NodeRecorder] Purged all records for node {Key} containing item {Value}.", loc.Key, loc.Value);
                loc.Value.Clear();
            }
        }

        public void DumpAetherytes()
        {
            foreach (var a in _world.Aetherytes.Aetherytes)
            {
                PluginLog.Information(
                    $"[AetheryteDump] |{a.Id}|{a.NameList}|{a.Territory?.Id ?? 0}|{a.XCoord:F2}|{a.YCoord:F2}|{a.XStream}|{a.YStream}|");
            }
        }

        public void DumpTerritories()
        {
            foreach (var t in _world.Territories.Territories.Values)
            {
                PluginLog.Information(
                    $"[TerritoryDump] |{t.Id}|{t.NameList}|{t.Region}|{t.XStream}|{t.YStream}|{string.Join("|", t.Aetherytes.Select(a => a.Id))}|");
            }
        }

        public void DumpItems()
        {
            foreach (var i in _world.Items.Items)
            {
                PluginLog.Information(
                    $"[ItemDump] |{i.ItemId}|{i.GatheringId}|{i.NameList}|{i.Level}{i.StarsString()}|{string.Join("|", i.NodeList.Select(n => n.Meta!.PointBaseId))}|");
            }
        }

        public void DumpNodes()
        {
            foreach (var n in _world.Nodes.BaseNodes())
            {
                PluginLog.Information(
                    $"[NodeDump] |{string.Join(",", n.Nodes!.Nodes.Keys)}|{n.Meta!.PointBaseId}|{n.Meta.GatheringType}|{n.Meta.NodeType}|{n.Meta.Level}|{n.GetX()}|{n.GetY()}|{n.Nodes!.Territory!.Id}|{n.PlaceNameEn}|{n.GetClosestAetheryte()?.Id ?? -1}|{n.Times!.UptimeTable()}|{n.Items!.PrintItems()}");
            }
        }

        public void DumpFishingSpots()
        {
            foreach (var f in _world.Fish.FishingSpots.Values)
            {
                PluginLog.Information(
                    $"[FishingSpotDump] |{f.Id}|{f.XCoord}|{f.YCoord}|{f.Radius}|{f.PlaceName?[ClientLanguage.English] ?? "MISSING"}|{f.Territory?.NameList[ClientLanguage.English] ?? "MISSING"}|{f.ClosestAetheryte?.NameList[ClientLanguage.English] ?? "MISSING"}|{string.Join("|", f.Items.Where(i => i != null).Select(i => i!.Id))}");
            }
        }

        public void DumpFish()
        {
            foreach (var f in _world.Fish.Fish.Values)
                PluginLog.Information($"[FishDump] |{f.Id}|{f.Name}|");
        }
    }
}
