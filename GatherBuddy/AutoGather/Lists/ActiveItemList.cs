using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Utility;
using ECommons.ExcelServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using GatherBuddy.AutoGather.Extensions;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.Time;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using ECommons.DalamudServices;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using GatherBuddy.SeFunctions;
using UmbralNodes = GatherBuddy.Data.UmbralNodes;
using EnhancedCurrentWeather = GatherBuddy.SeFunctions.EnhancedCurrentWeather;
namespace GatherBuddy.AutoGather.Lists
{
    internal class ActiveItemList : IEnumerable<GatherTarget>, IDisposable
    {
        private readonly List<GatherTarget>                      _gatherableItems = [];
        private readonly AutoGatherListsManager                  _listsManager;
        private readonly Dictionary<uint, int>                   _teleportationCosts = [];
        private readonly Dictionary<GatheringNode, TimeInterval> _visitedTimedNodes  = [];
        private          TimeStamp                               _lastUpdateTime     = TimeStamp.MinValue;
        private          uint                                    _lastTerritoryId;
        private          bool                                    _activeItemsChanged;
        private          bool                                    _gatheredSomething;

        internal ReadOnlyDictionary<GatheringNode, TimeInterval> DebugVisitedTimedLocations
            => _visitedTimedNodes.AsReadOnly();

        /// <summary>
        /// First item on the list as of the last enumeration or default.
        /// </summary>
        public GatherTarget CurrentOrDefault
            => _gatherableItems.FirstOrDefault();

        /// <summary>
        /// Determines whether there are any items that need to be gathered,
        /// including items that are not up yet.
        /// </summary>
        /// <value>
        /// True if there are items that need to be gathered; otherwise, false.
        /// </value>
        public bool HasItemsToGather
            => _listsManager.ActiveItems.Where(NeedsGathering).Any() 
            || _listsManager.ActiveFish.Where(NeedsGathering).Any();

        public bool IsInitialized
            => _lastUpdateTime != TimeStamp.MinValue;

        public ActiveItemList(AutoGatherListsManager listsManager)
        {
            _listsManager                    =  listsManager;
            _listsManager.ActiveItemsChanged += OnActiveItemsChanged;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the available gather targets.
        /// </summary>
        /// <returns>
        /// An enumerator for the available gather targets.
        /// </returns>
        public IEnumerator<GatherTarget> GetEnumerator()
        {
            return _gatherableItems.GetEnumerator();
        }

        /// <summary>
        /// Refreshes the list of items to gather (if needed) and returns the first item.
        /// </summary>
        /// <returns>The next item to gather. </returns>
        public IEnumerable<GatherTarget> GetNextOrDefault(IEnumerable<uint> nearbyNodes)
        {
            if (IsUpdateNeeded())
                DoUpdate();

            //Svc.Log.Verbose($"Nearby nodes: {string.Join(", ", nearbyNodes.Select(x => x.ToString("X8")))}.");
            
            GatherTarget firstItemNeedingGathering;
            if (Plugin.Functions.InTheDiadem())
            {
                var currentWeather = EnhancedCurrentWeather.GetCurrentWeatherId();
                var isUmbralWeather = UmbralNodes.IsUmbralWeather(currentWeather);
                
                if (isUmbralWeather)
                {
                    var umbralWeatherType = (UmbralNodes.UmbralWeatherType)currentWeather;
                    var currentJob = Player.Job switch
                    {
                        Job.MIN => GatheringType.Miner,
                        Job.BTN => GatheringType.Botanist,
                        _ => GatheringType.Unknown
                    };
                    
                    var priorityUmbralItem = _gatherableItems
                        .Where(NeedsGathering)
                        .Where(target => target.Gatherable != null && UmbralNodes.IsUmbralItem(target.Gatherable.ItemId))
                        .FirstOrDefault(target => 
                        {
                            var umbralInfo = UmbralNodes.GetUmbralItemInfo(target.Gatherable.ItemId);
                            return umbralInfo.HasValue && 
                                   umbralInfo.Value.Weather == umbralWeatherType;
                        });
                    
                    if (priorityUmbralItem != default)
                    {
                        firstItemNeedingGathering = priorityUmbralItem;
                    }
                    else
                    {
                        firstItemNeedingGathering = _gatherableItems
                            .Where(NeedsGathering)
                            .FirstOrDefault(target => target.Gatherable == null || !UmbralNodes.IsUmbralItem(target.Gatherable.ItemId));
                    }
                }
                else
                {
                    firstItemNeedingGathering = _gatherableItems.FirstOrDefault(NeedsGathering);
                }
            }
            else
            {
                firstItemNeedingGathering = _gatherableItems.FirstOrDefault(NeedsGathering);
            }
            
            if (firstItemNeedingGathering == default)
                return [];
            
            IEnumerable<GatherTarget> nearbyItems = [];
            
            if (this.Any(n => !n.Node?.Times.AlwaysUp() ?? false))
            {
                nearbyItems = [this.First(n => n.Time.InRange(AutoGather.AdjustedServerTime))];
            }
            else
            {
                var isUmbralItem = firstItemNeedingGathering.Gatherable != null && 
                                  UmbralNodes.IsUmbralItem(firstItemNeedingGathering.Gatherable.ItemId);
                                  
                if (isUmbralItem && Plugin.Functions.InTheDiadem())
                {
                    var currentWeather = EnhancedCurrentWeather.GetCurrentWeatherId();
                    var isUmbralWeather = UmbralNodes.IsUmbralWeather(currentWeather);
                    
                    if (isUmbralWeather)
                    {
                        
                        var currentJob = Player.Job switch
                        {
                            Job.MIN => GatheringType.Miner,
                            Job.BTN => GatheringType.Botanist,
                            _ => GatheringType.Unknown
                        };
                        var umbralWeather = (UmbralNodes.UmbralWeatherType)currentWeather;
                        
                        nearbyItems = _gatherableItems
                            .Where(target => target.Gatherable != null && UmbralNodes.IsUmbralItem(target.Gatherable.ItemId))
                            .Where(target => 
                            {
                                var umbralInfo = UmbralNodes.GetUmbralItemInfo(target.Gatherable.ItemId);
                                return umbralInfo.HasValue && 
                                       umbralInfo.Value.Weather == umbralWeather &&
                                       UmbralNodes.GetGatheringType(umbralInfo.Value.NodeType) == currentJob;
                            })
                            .Where(NeedsGathering);
                    }
                    else
                    {
                        nearbyItems = [];
                    }
                }
                else
                {
                    nearbyItems = this
                        .Where(i => i.Item == firstItemNeedingGathering.Item)
                        .Where(i => i.Node?.WorldPositions.Keys.Any(nearbyNodes.Contains) ?? false);
                }
                    
            }

            var result = nearbyItems.Any() ? nearbyItems : [firstItemNeedingGathering];
            return result;
        }

        /// <summary>
        /// Marks a node as visited.
        /// </summary>
        /// <param name="info">The GatherTarget containing the node to mark as visited.</param>
        public void MarkVisited(IGameObject target)
        {
            _gatheredSomething = true;
            // In almost all cases, the target is the first item in the list, so it's O(1).
            var x = _gatherableItems.FirstOrDefault(x => x.Node.WorldPositions.ContainsKey(target.DataId));
            if (x != default && x.Time != TimeInterval.Always && x.Node?.NodeType is NodeType.Legendary or NodeType.Unspoiled)
                _visitedTimedNodes[x.Node] = x.Time;
        }

        internal void DebugMarkVisited(GatherTarget x)
        {
            _gatheredSomething = true;
            if (x.Time != TimeInterval.Always && x.Node.NodeType is NodeType.Legendary or NodeType.Unspoiled)
                _visitedTimedNodes[x.Node] = x.Time;
        }

        private bool NeedsGathering((Gatherable item, uint quantity) value)
        {
            var (item, quantity) = value;
            return item.GetInventoryCount() < (item.IsTreasureMap ? 1 : quantity);
        }

        private bool NeedsGathering((Fish fish, uint quantity) value)
        {
            var (item, quantity) = value;
            return item.GetInventoryCount() < quantity;
        }

        private bool NeedsGathering(GatherTarget target)
        {
            return target.Item.GetInventoryCount() < target.Quantity;
        }


        private void OnActiveItemsChanged()
        {
            _activeItemsChanged = true;
        }

        private void RemoveExpiredVisited(TimeStamp adjustedServerTime)
        {
            foreach (var (loc, time) in _visitedTimedNodes)
                if (time.End <= adjustedServerTime)
                    _visitedTimedNodes.Remove(loc);
        }

        internal void DebugClearVisited()
        {
            _visitedTimedNodes.Clear();
        }

        /// <summary>
        /// Updates the list of items to gather based on the current territory and player levels.
        /// </summary>
        private void UpdateItemsToGather()
        {
            // Items are unlocked in tiers of 5 levels, so we round up to the nearest 5.
            var minerLevel = (DiscipleOfLand.MinerLevel + 5) / 5 * 5;
            var botanistLevel = (DiscipleOfLand.BotanistLevel + 5) / 5 * 5;
            var adjustedServerTime = _lastUpdateTime;
            var territoryId = _lastTerritoryId;
            DateTime? nextAllowance = null;

            var nodes = _listsManager.ActiveItems
                // Filter out items that are already gathered.
                .Where(NeedsGathering)
                .Where(x => (RequiresHomeWorld(x) && Functions.OnHomeWorld()) || !RequiresHomeWorld(x))
                // If treasure map, only gather if the allowance is up.
                .Where(x => !x.Item.IsTreasureMap || (nextAllowance ??= DiscipleOfLand.NextTreasureMapAllowance) < adjustedServerTime.DateTime)
                // Fetch preferred location.
                .Select(x => (x.Item, x.Quantity, PreferredLocation: _listsManager.GetPreferredLocation(x.Item)))
                // Flatten node list add calculate the next uptime.
                .SelectMany(x => x.Item.NodeList.Select(Node
                    => (x.Item, Node, Time: Node.Times.NextUptime(adjustedServerTime), x.Quantity, x.PreferredLocation)))
                // Remove nodes with a level higher than the player can gather.
                .Where(info => info.Node.GatheringType.ToGroup() switch
                {
                    GatheringType.Miner => info.Node.Level <= minerLevel,
                    GatheringType.Botanist => info.Node.Level <= botanistLevel,
                    _ => false
                })
                // Remove nodes that are not up.
                .Where(x => x.Time.InRange(adjustedServerTime))
                // Remove nodes that are already gathered.
                .Where(x => !_visitedTimedNodes.ContainsKey(x.Node))
                // Group by item and select the best node.
                .GroupBy(x => x.Item, x => x, (_, g) => g
                    // Prioritize preferred location, then current job, then preferred job, then the rest.
                    .OrderBy(x =>
                        x.Node == x.PreferredLocation ? 0
                        : x.Node.GatheringType.ToGroup() == (Player.Job switch
                        {
                            Job.MIN => GatheringType.Miner,
                            Job.BTN => GatheringType.Botanist,
                            _ => GatheringType.Unknown
                        }) ? 1
                        : x.Node.GatheringType.ToGroup() == GatherBuddy.Config.PreferredGatheringType ? 2
                            : 3)
                    // Prioritize closest nodes in the current territory.
                    .ThenBy(x => GetHorizontalSquaredDistanceToPlayer(x.Node))
                    // Order by end time, longest first as in the original UptimeManager.NextUptime().
                    .ThenByDescending(x => x.Time.End)
                    .ThenBy(x => GatherBuddy.Config.AetherytePreference switch
                    {
                        // Order by distance to the closest aetheryte.
                        AetherytePreference.Distance => AutoGather.FindClosestAetheryte(x.Node)
                                ?.WorldDistance(x.Node.Territory.Id, x.Node.IntegralXCoord, x.Node.IntegralYCoord)
                         ?? int.MaxValue,
                        // Order by teleportation cost.
                        AetherytePreference.Cost => GetTeleportationCost(x.Node),
                        _ => 0
                    })
                    .First()
                )
                // Prioritize timed nodes first.
                .OrderBy(x => x.Time == TimeInterval.Always)
                .ThenBy(x => x.Node.GatheringType.ToGroup() != (Player.Job switch
                {
                    Job.MIN => GatheringType.Miner,
                    Job.BTN => GatheringType.Botanist,
                    _ => GatheringType.Unknown
                }));

            var fish = _listsManager.ActiveFish
                .Where(NeedsGathering)
                .Where(x => (RequiresHomeWorld(x) && Functions.OnHomeWorld()) || !RequiresHomeWorld(x))
                .Select(x => (x.Fish, x.Quantity, PreferredLocation: _listsManager.GetPreferredLocation(x.Fish) ?? x.Fish.FishingSpots.First()))
                .Select(x => (x.Fish, x.PreferredLocation, Time: GatherBuddy.UptimeManager.NextUptime(x.Fish, adjustedServerTime).interval,
                    x.Quantity))
                .Where(x => x.Time.InRange(adjustedServerTime))
                .GroupBy(x => x.Fish, x => x, (_, g) => g
                    // Order by end time, longest first as in the original UptimeManager.NextUptime().
                    .OrderByDescending(x => x.Time.End)
                    .ThenBy(x => GatherBuddy.Config.AetherytePreference switch
                    {
                        // Order by distance to the closest aetheryte.
                        AetherytePreference.Distance => AutoGather.FindClosestAetheryte(x.PreferredLocation)
                                ?.WorldDistance(x.PreferredLocation.Territory.Id, x.PreferredLocation.IntegralXCoord,
                                    x.PreferredLocation.IntegralYCoord)
                         ?? int.MaxValue,
                        // Order by teleportation cost.
                        AetherytePreference.Cost => GetTeleportationCost(x.PreferredLocation),
                        _ => 0
                    })
                    .First()
                )
                .OrderBy(x => x.Time == TimeInterval.Always);

            if (GatherBuddy.Config.AutoGatherConfig.SortingMethod == AutoGatherConfig.SortingType.Location)
            {
                nodes = nodes
                    // Order by node type.
                    .ThenBy(x => GetNodeTypeAsPriority(x.Item))
                    // Then by teleportation cost.
                    .ThenBy(x => x.Node.Territory.Id == territoryId ? 0 : GetTeleportationCost(x.Node))
                    // Then by distance to the player (for current territory).
                    .ThenBy(x => GetHorizontalSquaredDistanceToPlayer(x.Node));
            }

            _gatherableItems.Clear();
            _gatherableItems.AddRange(nodes.Select(x => new GatherTarget(x.Item, x.Node, x.Time, x.Quantity)));
            _gatherableItems.AddRange(fish.Select(x => new GatherTarget(x.Fish, x.PreferredLocation, x.Time, x.Quantity)));
            
            AddUmbralItemsIfAvailable(adjustedServerTime, minerLevel, botanistLevel);
            
            Svc.Log.Verbose($"Gatherable items: ({_gatherableItems.Count}): {string.Join(", ", _gatherableItems.Select(x => x.Item.Name))}.");
        }

        private bool RequiresHomeWorld((Gatherable Item, uint Quantity) valueTuple)
        {
            var item = valueTuple.Item1;
            return item.NodeType == NodeType.Legendary
             || item.NodeType == NodeType.Unspoiled
             || item.NodeList.Any(nl => nl.Territory.Id is 901 or 929 or 939); // The Diadem
        }

        private bool RequiresHomeWorld((Fish fish, uint quantity) valueTuple)
        {
            return false;
        }

        private static float GetHorizontalSquaredDistanceToPlayer(GatheringNode node)
        {
            if (node.Territory.Id != Dalamud.ClientState.TerritoryType)
                return float.MaxValue;

            // Node coordinates are map coordinates multiplied by 100.
            var playerPos3D = Player.Object.GetMapCoordinates();
            var playerPos   = new Vector2(playerPos3D.X * 100f,             playerPos3D.Y * 100f);
            return Vector2.DistanceSquared(new Vector2(node.IntegralXCoord, node.IntegralYCoord), playerPos);
        }

        /// <summary>
        /// For sorting items in the following order: Legendary, Unspoiled, Ephemeral, Regular.
        /// </summary>
        private static int GetNodeTypeAsPriority(Gatherable item)
        {
            return item.NodeType switch
            {
                NodeType.Legendary => 0,
                NodeType.Unspoiled => 1,
                NodeType.Ephemeral => 2,
                NodeType.Regular   => 9,
                _                  => 99,
            };
        }

        private int GetTeleportationCost(ILocation location)
        {
            var aetheryte = AutoGather.FindClosestAetheryte(location);
            if (aetheryte == null)
                return int.MaxValue; // If there's no aetheryte, put it at the end

            return _teleportationCosts.GetValueOrDefault(aetheryte.Id, int.MaxValue);
        }

        /// <summary>
        /// Stores teleportation costs in the dictionary.
        /// </summary>
        private unsafe void UpdateTeleportationCosts()
        {
            _teleportationCosts.Clear();

            var telepo = Telepo.Instance();
            if (telepo == null)
                return;

            telepo->UpdateAetheryteList();
            _teleportationCosts.EnsureCapacity(telepo->TeleportList.Count);

            for (var i = 0; i < telepo->TeleportList.Count; i++)
            {
                var entry = telepo->TeleportList[i];
                _teleportationCosts[entry.AetheryteId] = (int)entry.GilCost;
            }
        }

        /// <summary>
        /// Returns true in the following cases:
        /// 1) The active item list has changed.
        /// 2) The Eorzea hour has changed.
        /// 3) The territory has changed.
        /// 4) The player has gathered enough of an item or it can no longer be gathered in the current territory.
        /// </summary>
        /// <returns>
        /// True if an update is needed; otherwise, false.
        /// </returns>
        private bool IsUpdateNeeded()
        {
            if (_activeItemsChanged
             || _lastUpdateTime.TotalEorzeaHours() != AutoGather.AdjustedServerTime.TotalEorzeaHours()
             || _lastTerritoryId != Dalamud.ClientState.TerritoryType)
                return true;

            if (_gatheredSomething)
            {
                _gatheredSomething = false;
                var current = CurrentOrDefault;
                foreach (var item in _gatherableItems.Where(NeedsGathering).Where(x => !_visitedTimedNodes.ContainsKey(x.Node)))
                {
                    if (item == current)
                        return false;

                    if (item.Node.Territory.Id != _lastTerritoryId)
                        break;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the active item list, teleportation costs, and clears expired visited nodes.
        /// </summary>
        private void DoUpdate()
        {
            var territoryId        = Dalamud.ClientState.TerritoryType;
            var adjustedServerTime = AutoGather.AdjustedServerTime;
            var eorzeaHour         = adjustedServerTime.TotalEorzeaHours();
            var lastTerritoryId    = _lastTerritoryId;
            var lastEorzeaHour     = _lastUpdateTime.TotalEorzeaHours();

            _activeItemsChanged = false;
            _gatheredSomething  = false;
            _lastUpdateTime     = adjustedServerTime;
            _lastTerritoryId    = territoryId;

            if (territoryId != lastTerritoryId)
                UpdateTeleportationCosts();

            if (eorzeaHour != lastEorzeaHour)
                RemoveExpiredVisited(adjustedServerTime);

            UpdateItemsToGather();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Reset()
        {
            _lastTerritoryId = 0;
            _lastUpdateTime  = TimeStamp.MinValue;
            _gatherableItems.Clear();
            _gatherableItems.TrimExcess();
            _teleportationCosts.Clear();
            _teleportationCosts.TrimExcess();
        }

        private void AddUmbralItemsIfAvailable(TimeStamp adjustedServerTime, int minerLevel, int botanistLevel)
        {
            var currentWeather = EnhancedCurrentWeather.GetCurrentWeatherId();
            var isInDiadem = Plugin.Functions.InTheDiadem();
            var isUmbralWeather = UmbralNodes.IsUmbralWeather(currentWeather);
            
            if (!isUmbralWeather || !isInDiadem)
                return;
                
            var umbralWeatherType = (UmbralNodes.UmbralWeatherType)currentWeather;
            
            var umbralItemsToGather = _listsManager.ActiveItems
                .Where(NeedsGathering)
                .Where(x => UmbralNodes.UmbralNodeData.Any(entry => entry.ItemIds.Contains(x.Item.ItemId)))
                .Where(x => (RequiresHomeWorld(x) && Plugin.Functions.OnHomeWorld()) || !RequiresHomeWorld(x))
                .ToList();
                
            if (!umbralItemsToGather.Any())
                return;
                
            foreach (var itemEntry in umbralItemsToGather)
            {
                var item = itemEntry.Item;
                var quantity = itemEntry.Quantity;
                
                var umbralNodeEntry = UmbralNodes.UmbralNodeData.FirstOrDefault(entry => 
                    entry.ItemIds.Contains(item.ItemId));
                    
                if (umbralNodeEntry.NodeId == 0)
                    continue;
                    
                var itemGatheringType = item.GatheringType.ToGroup();
                
                var umbralGatheringType = umbralNodeEntry.NodeType switch
                {
                    UmbralNodes.CloudedNodeType.CloudedRockyOutcrop => GatheringType.Miner,
                    UmbralNodes.CloudedNodeType.CloudedMineralDeposit => GatheringType.Miner,
                    UmbralNodes.CloudedNodeType.CloudedMatureTree => GatheringType.Botanist,
                    UmbralNodes.CloudedNodeType.CloudedLushVegetation => GatheringType.Botanist,
                    _ => itemGatheringType
                };
                
                if (umbralNodeEntry.Weather != umbralWeatherType)
                    continue;
                
                var requiredLevel = item.Level;
                var playerLevel = umbralGatheringType switch
                {
                    GatheringType.Miner => minerLevel,
                    GatheringType.Botanist => botanistLevel,
                    _ => 0
                };
                
                if (requiredLevel > playerLevel)
                    continue;
                    
                var currentTerritoryId = Dalamud.ClientState.TerritoryType;
                var templateNode = GatherBuddy.GameData.GatheringNodes.Values
                    .Where(node => node.Territory.Id is 901 or 929 or 939 && 
                        node.GatheringType.ToGroup() == umbralGatheringType)
                    .OrderBy(node => node.Territory.Id == currentTerritoryId ? 0 : 1)
                    .FirstOrDefault();
                        
                if (templateNode != null)
                {
                    var gatherTarget = new GatherTarget(item, templateNode, TimeInterval.Always, quantity);
                    _gatherableItems.Add(gatherTarget);
                }
            }
        }

        public void Dispose()
        {
            if (_listsManager != null)
            {
                _listsManager.ActiveItemsChanged -= OnActiveItemsChanged;
            }
        }
    }



    public record struct GatherTarget(IGatherable Item, ILocation Location, TimeInterval Time, uint Quantity)
    {
        public readonly uint         Quantity = Quantity;
        public readonly TimeInterval Time     = Time;
        public readonly ILocation    Location = Location;
        public readonly IGatherable  Item     = Item;

        public GatheringNode? Node
            => Location as GatheringNode;

        public Gatherable? Gatherable
            => Item as Gatherable;

        public FishingSpot? FishingSpot
            => Location as FishingSpot;

        public Fish? Fish
            => Item as Fish;
    }
}
