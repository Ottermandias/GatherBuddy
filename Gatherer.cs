using Serilog;
using System.Threading.Tasks;
using Dalamud.Plugin;
using Dalamud;
using System.Linq;
using System;
using System.Collections.Generic;
using GatherBuddyPlugin;
using GatherBuddyPlugin.Managers;
using System.IO;

namespace Gathering
{
    public class Gatherer : IDisposable
    {
        private ClientLanguage                             teleporterLanguage;
        private FileSystemWatcher                          teleporterWatcher = null;
        private readonly ClientLanguage                    language;
        private readonly Dalamud.Game.Internal.Gui.ChatGui chat;
        private readonly CommandManager                    commandManager;
        private readonly World                             world;
        private readonly Dictionary<string, TimedGroup>    groups;
        private readonly GatherBuddyConfiguration          configuration;
        public  readonly NodeTimeLine                      timeline;

        public void TryCreateTeleporterWatcher(DalamudPluginInterface pi, bool useTeleport)
        {
            teleporterLanguage = language;
            if (!useTeleport || teleporterWatcher != null)
            {
                teleporterWatcher?.Dispose();
                teleporterWatcher = null;
                return;
            }

            const string TeleporterPluginConfigFile = "TeleporterPlugin.json";

            var dir = new DirectoryInfo(pi.GetPluginConfigDirectory());
            if (!dir.Exists || !dir.Parent.Exists)
                return;
            dir = dir.Parent;

            var file = new FileInfo(Path.Combine(dir.FullName, TeleporterPluginConfigFile));
            if (file.Exists)
                ParseTeleporterFile(file.FullName);

            void OnTeleporterConfigChange(object source, FileSystemEventArgs args)
            {
                Log.Verbose("[GatherBuddy] Reloading Teleporter Config.");
                if (args.ChangeType != WatcherChangeTypes.Changed && args.ChangeType != WatcherChangeTypes.Created)
                    return;
                ParseTeleporterFile(args.FullPath);
            }

            teleporterWatcher = new();
            teleporterWatcher.Path = dir.FullName;
            teleporterWatcher.NotifyFilter = NotifyFilters.LastWrite;
            teleporterWatcher.Filter = TeleporterPluginConfigFile;
            teleporterWatcher.Changed += OnTeleporterConfigChange;
            teleporterWatcher.EnableRaisingEvents = true;
        }

        private void ParseTeleporterFile(string filePath)
        {
            try
            {
                const string TeleporterLanguageString = "\"teleporterlanguage\":";

                var content = File.ReadAllText(filePath).ToLowerInvariant();
                var idx = content.IndexOf(TeleporterLanguageString);
                if (idx < 0)
                    return;
                content = content.Substring(idx + TeleporterLanguageString.Length).Trim();
                if (content.Length < 1)
                    return;
                switch(content[0])
                {
                    case '0': teleporterLanguage = ClientLanguage.Japanese; return;
                    case '1': teleporterLanguage = ClientLanguage.English;  return;
                    case '2': teleporterLanguage = ClientLanguage.German;   return;
                    case '3': teleporterLanguage = ClientLanguage.French;   return;
                    case '4': teleporterLanguage = language;                return;
                };
            }
            catch(Exception e)
            {
                Log.Error($"[GatherBuddy] Could not read Teleporter Config:\n{e}");
                teleporterLanguage = language;
            }
        }

        public Gatherer(DalamudPluginInterface pi, GatherBuddyConfiguration config, CommandManager commandManager)
        {
            this.commandManager = commandManager;
            this.chat           = pi.Framework.Gui.Chat;
            this.language       = pi.ClientState.ClientLanguage;
            this.configuration  = config;
            this.world          = new World(pi, configuration);
            this.groups         = TimedGroup.CreateGroups(world);
            this.timeline       = new(world.nodes);
            TryCreateTeleporterWatcher(pi, configuration.UseTeleport);
        }

        public void OnTerritoryChange(object sender, UInt16 territory)
        {
            world.SetPlayerStreamCoords(territory);
        }

        public void Dispose()
        {
            teleporterWatcher?.Dispose();
            world.nodes.records.Dispose();
        }

        public void StartRecording()
        {
            world.nodes.records.ActivateScanning();
        }

        public void StopRecording()
        {
            world.nodes.records.DeactivateScanning();
        }

        public int Snapshot()
        {
            return world.nodes.records.Scan();
        }

        public void PrintRecords()
        {
            world.nodes.records.PrintToLog();
        }

        public void PurgeRecord(uint nodeId)
        {
            world.nodes.records.PurgeRecord(nodeId);
        }

        public void PurgeAllRecords()
        {
            world.nodes.records.PurgeRecords();
        }

        private Gatherable FindItemLogging(string itemName)
        {
            Gatherable item = world.FindItemByName(itemName);
            string output;
            if (item == null)            
                output = $"Could not find corresponding item to \"{itemName}\".";
            else
                output = $"Identified [{item.itemId}: {item.nameList[language]}] for \"{itemName}\".";
            chat.Print(output);
            Log.Verbose($"[GatherBuddy] {output}");
            return item;
        }

        private Node GetClosestNode(string itemName, GatheringType? type = null)
        {
            Gatherable item = FindItemLogging(itemName);
            if (item == null)
                return null;

            string output;
            if (item.NodeList.Count ==  0)
            {
                output = $"Found no gathering nodes for item {item?.itemId ?? -1}.";
                chat.PrintError(output);
                Log.Debug($"[GatherBuddy] {output}");
                return null;
            }

            Node closestNode = world.ClosestNodeForItem(item, type);
            if (closestNode?.GetValidAetheryte() == null)
            {
                if (type == null)
                {
                    chat.PrintError(
                        $"No nodes containing {item.nameList[language]} have associated coordinates or aetheryte.");
                    chat.PrintError(
                        $"They will become available after encountering the respective node while having recording enabled.");
                }
                else
                {
                    chat.PrintError(
                        $"No nodes containing {item.nameList[language]} for the specified job have been found.");
                }
            }

            if (!closestNode?.times.AlwaysUp() ?? false)
                chat.Print($"Node is up at {closestNode.times.PrintHours()}.");
            
            return closestNode;
        }

        private async Task<bool> TeleportToNode(Node node)
        {
            if (!configuration.UseTeleport)
                return true;

            var name = node?.GetClosestAetheryte()?.nameList[teleporterLanguage] ?? "";
            if (name.Length == 0)
            {
                Log.Debug($"[GatherBuddy] No valid aetheryte found for node {node.meta.pointBaseId}.");
                return false;
            }
            if (!commandManager.Execute("/tp " + name))
            {
                chat.PrintError("It seems like you have activated teleporting, but you have not installed the required plugin Teleporter by Pohky.");
                chat.PrintError("Please either deactivate teleporting or install the plugin.");
            }
            await Task.Delay(100);
            return true;
        }

        private async Task<bool> EquipForNode(Node node)
        {
            if (!configuration.UseGearChange)
                return true;

            if (node.meta.IsBotanist())
            {
                commandManager.Execute($"/gearset change {configuration?.BotanistSetName ?? "Botanist"}");
                await Task.Delay(200);
            }
            else if (node.meta.IsMiner())
            {
                commandManager.Execute($"/gearset change {configuration?.MinerSetName ?? "Miner"}");
                await Task.Delay(200);
            }
            else
            {
                Log.Debug($"[GatherBuddy] No jobtype set for node {node.meta.pointBaseId}.");
                return false;
            }
            return true;
        }

        private async Task<bool> SetNodeFlag(Node node)
        {
            // Coordinates = 0.0 are acceptable because of the diadem, so no error message.
            if (!configuration.UseCoordinates || node.GetX() == 0.0 || node.GetY() == 0.0)
                return true;

            var xString = node.GetX().ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            var yString = node.GetY().ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            var territory = node.nodes.territory?.nameList?[language] ?? "";

            if (territory.Length == 0)
            {
                Log.Debug($"[GatherBuddy] No territory set for node {node.meta.pointBaseId}.");
                return false;
            }

            if (!commandManager.Execute($"/coord {xString}, {yString} : {territory}" ))
            {
                chat.PrintError("It seems like you have activated map markers, but you have not installed the required plugin ChatCoordinates by kij.");
                chat.PrintError("Please either deactivate teleporting or install the plugin.");
            }
            await Task.Delay(100);
            return true;
        }

        public async void OnGatherActionWithNode(Node node)
        {
            try
            {
                if (await   EquipForNode(node) == false) return;
                if (await TeleportToNode(node) == false) return;
                if (await    SetNodeFlag(node) == false) return;
            }
            catch(Exception e)
            {
                Log.Error($"[GatherBuddy] Exception caught: {e}");
            }
        }
        public void OnGatherAction(string itemName, GatheringType? type = null)
        {
            try
            {
                Node closestNode = GetClosestNode(itemName, type);
                if (closestNode == null)
                    return;
                OnGatherActionWithNode(closestNode);
            }
            catch(Exception e)
            {
                Log.Error($"[GatherBuddy] Exception caught: {e}");
            }
        }

        public async void OnGroupGatherAction(string groupName, int minuteOffset)
        {
            try
            {
                if (groupName.Length == 0)
                {
                    TimedGroup.PrintHelp(chat, groups);
                    return;
                }

                if (!groups.TryGetValue(groupName, out TimedGroup group))
                {
                    chat.PrintError($"\"{groupName}\" is not a valid group.");
                    return;
                }
                var currentHour = EorzeaTime.CurrentHours(minuteOffset);
                var node = group.CurrentNode(currentHour);
                if (node == null)
                {
                    Log.Debug($"[GatherBuddy] No node for hour {currentHour} set in group {group.name}.");
                    return;
                }

                if (await   EquipForNode(node) == false) return;
                if (await TeleportToNode(node) == false) return;
                if (await    SetNodeFlag(node) == false) return;
            }
            catch(Exception e)
            {
                Log.Error($"[GatherBuddy] Exception caught: {e}");
            }
        }

        public void PurgeRecords(string itemName)
        {
            Gatherable item = FindItemLogging(itemName);
            if (item == null)
                return;

            string output;
            if (item.NodeList.Count ==  0)
            {
                output = $"Found no gathering nodes for item {item?.itemId ?? -1}.";
                chat.PrintError(output);
                Log.Debug($"[GatherBuddy] {output}");
                return;
            }
            foreach (var baseNode in item.NodeList)
            {
                foreach (var loc in baseNode.nodes.nodes)
                    if (loc.Value != null)
                    {
                        if(loc.Value.locations.Count > 0)
                            Log.Information($"[GatherBuddy] [NodeRecorder] Purged all records for node {loc.Key} containing item {loc.Value}.");
                        loc.Value.Clear();
                    }
            }
        }

        public void DumpAetherytes()
        {
            foreach (var A in world.aetherytes.aetherytes)
            {
                Log.Information($"[GatherBuddy] [AetheryteDump] |{A.id}|{A.nameList}|{A.territory?.id ?? 0}|{A.xCoord:F2}|{A.yCoord:F2}|{A.xStream}|{A.yStream}|");
            }
        }

        public void DumpTerritories()
        {
            foreach (var pair in world.territories.territories)
            {
                var T = pair.Value;
                Log.Information($"[GatherBuddy] [TerritoryDump] |{T.id}|{T.nameList}|{T.region}|{T.xStream}|{T.yStream}|{string.Join("|", T.aetherytes.Select(A => A.id))}|");
            }
        }

        public void DumpItems()
        {
            foreach (var I in world.items.items)
            {
                Log.Information($"[GatherBuddy] [ItemDump] |{I.itemId}|{I.gatheringId}|{I.nameList}|{I.Level()}{I.StarsString()}|{string.Join("|", I.NodeList.Select(N => N.meta.pointBaseId))}|");
            }
        }

        public void DumpNodes()
        {
            foreach (var N in world.nodes.BaseNodes())
            {
                Log.Information($"[GatherBuddy] [NodeDump] |{string.Join(",", N.nodes.nodes.Keys)}|{N.meta.pointBaseId}|{N.meta.gatheringType}|{N.meta.nodeType}|{N.meta.level}|{N.GetX()}|{N.GetY()}|{N.nodes.territory.id}|{N.placeNameEN}|{N.GetClosestAetheryte()?.id ?? -1}|{N.times.UptimeTable()}|{N.items.PrintItems()}");
            }
        }
    }
}
