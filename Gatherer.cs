using Serilog;
using System.Threading.Tasks;
using Dalamud.Plugin;
using Dalamud;
using System.Linq;
using System;
using System.Collections.Generic;
using GatherBuddyPlugin;
using Otter;

namespace Gathering
{
    public class Gatherer : IDisposable
    {
        private readonly ClientLanguage                    language;
        private readonly Dalamud.Game.Internal.Gui.ChatGui chat;
        private readonly CommandManager                    commandManager;
        private readonly World                             world;
        private readonly Dictionary<string, TimedGroup>    groups;
        private readonly GatherBuddyConfiguration          configuration;
        public  readonly NodeTimeLine                      timeline;
    
        public Gatherer(DalamudPluginInterface pi, GatherBuddyConfiguration config, CommandManager commandManager)
        {
            this.commandManager = commandManager;
            this.chat           = pi.Framework.Gui.Chat;
            this.language       = pi.ClientState.ClientLanguage;
            this.configuration  = config;
            this.world          = new World(pi, configuration);
            this.groups         = TimedGroup.CreateGroups(world);
            this.timeline       = new(world.nodes);
        }

        public void OnTerritoryChange(object sender, UInt16 territory)
        {
            world.SetPlayerStreamCoords(territory);
        }

        public void Dispose()
        {
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

        private Node GetClosestNode(string itemName)
        {
            Gatherable item = world.FindItemByName(itemName);
            string output;
            if (item == null)            
                output = $"Could not find corresponding item to \"{itemName}\".";
            else
                output = $"Identified [{item.itemId}: {item.nameList[language]}] for \"{itemName}\".";
            chat.Print(output);
            Log.Verbose($"[GatherBuddy] {output}");
            if (item == null)
                return null;

            if (item.NodeList.Count ==  0)
            {
                output = $"Found no gathering nodes for item {item?.itemId ?? -1}.";
                chat.PrintError(output);
                Log.Debug($"[GatherBuddy] {output}");
                return null;
            }

            Node closestNode = world.ClosestNodeForItem(item);
            if (closestNode?.GetValidAetheryte() == null)
            {
                chat.PrintError($"No nodes containing {item.nameList[language]} have associated coordinates or aetheryte.");
                chat.PrintError($"They will become available after encountering the respective node while having recording enabled.");
            }

            if (!closestNode?.times.AlwaysUp() ?? false)
                chat.Print($"Node is up at {closestNode.times.PrintHours()}.");
            
            return closestNode;
        }

        private async Task<bool> TeleportToNode(Node node)
        {
            if (!configuration.UseTeleport)
                return true;

            var name = node?.GetClosestAetheryte()?.nameList[language] ?? "";
            if (name.Length == 0)
            {
                Log.Debug($"[GatherBuddy] No valid aetheryte found for node {node.meta.pointBaseId}.");
                return false;
            }
            commandManager.Execute("/tp " + name);
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

            commandManager.Execute($"/coord {xString}, {yString} : {territory}" );
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
        public void OnGatherAction(string itemName)
        {
            try
            {
                Node closestNode = GetClosestNode(itemName);
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

        public void dumpAetherytes()
        {
            foreach (var A in world.aetherytes.aetherytes)
            {
                Log.Information($"[GatherBuddy] [AetheryteDump] |{A.id}|{A.nameList}|{A.territory?.id ?? 0}|{A.xCoord:F2}|{A.yCoord:F2}|{A.xStream}|{A.yStream}|");
            }
        }

        public void dumpTerritories()
        {
            foreach (var pair in world.territories.territories)
            {
                var T = pair.Value;
                Log.Information($"[GatherBuddy] [TerritoryDump] |{T.id}|{T.nameList}|{T.region}|{T.xStream}|{T.yStream}|{string.Join("|", T.aetherytes.Select(A => A.id))}|");
            }
        }

        public void dumpItems()
        {
            foreach (var I in world.items.items)
            {
                Log.Information($"[GatherBuddy] [ItemDump] |{I.itemId}|{I.gatheringId}|{I.nameList}|{I.Level()}{I.StarsString()}|{string.Join("|", I.NodeList.Select(N => N.meta.pointBaseId))}|");
            }
        }

        public void dumpNodes()
        {
            foreach (var N in world.nodes.BaseNodes())
            {
                Log.Information($"[GatherBuddy] [NodeDump] |{string.Join(",", N.nodes.nodes.Keys)}|{N.meta.pointBaseId}|{N.meta.gatheringType}|{N.meta.nodeType}|{N.meta.level}|{N.GetX()}|{N.GetY()}|{N.nodes.territory.id}|{N.placeNameEN}|{N.GetClosestAetheryte()?.id ?? -1}|{N.times.UptimeTable()}|{N.items.PrintItems()}");
            }
        }
    }
}