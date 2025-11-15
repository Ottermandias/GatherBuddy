using ECommons.Automation.LegacyTaskManager;
using GatherBuddy.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using GatherBuddy.AutoGather.Movement;
using GatherBuddy.CustomInfo;
using GatherBuddy.Enums;
using ObjectKind = Dalamud.Game.ClientState.Objects.Enums.ObjectKind;
using FFXIVClientStructs.FFXIV.Client.Game;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Game.Text;
using Dalamud.Utility;
using ECommons;
using ECommons.ExcelServices;
using ECommons.Automation;
using ECommons.MathHelpers;
using GatherBuddy.Data;
using GatherBuddy.SeFunctions;
using GatherBuddy.AutoGather.Extensions;
using NodeType = GatherBuddy.Enums.NodeType;
using ECommons.UIHelpers.AddonMasterImplementations;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.AutoGather.Helpers;
using GatherBuddy.AutoGather.Lists;
using GatherBuddy.Classes;
using GatherBuddy.Interfaces;
using Lumina.Excel.Sheets;
using Fish = GatherBuddy.Classes.Fish;
using GatheringType = GatherBuddy.Enums.GatheringType;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather : IDisposable
    {
        public AutoGather(GatherBuddy plugin)
        {
            // Initialize the task manager
            TaskManager                  =  new();
            TaskManager.ShowDebug        =  false;
            _plugin                      =  plugin;
            _soundHelper                 =  new SoundHelper();
            _advancedUnstuck             =  new();
            _activeItemList              =  new ActiveItemList(plugin.AutoGatherListsManager);
            ArtisanExporter              =  new Reflection.ArtisanExporter(plugin.AutoGatherListsManager);
            Svc.Chat.CheckMessageHandled += OnMessageHandled;
            //Svc.AddonLifecycle.RegisterListener(AddonEvent.PreFinalize, "Gathering", OnGatheringFinalize);
            _plugin.FishRecorder.Parser.CaughtFish += OnFishCaught;
        }
        public Fish? LastCaughtFish { get; private set; }
        public Fish? PreviouslyCaughtFish { get; private set; }
        private void OnFishCaught(Fish arg1, ushort arg2, byte arg3, bool arg4, bool arg5)
        {
            PreviouslyCaughtFish = LastCaughtFish;
            LastCaughtFish       = arg1;
        }

        // Track the current gather target for robust node handling
        private GatherTarget? _currentGatherTarget;

        private void OnMessageHandled(XivChatType type, int timestamp, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            try
            {
                if (type is (XivChatType)2243)
                {
                    var text = message.TextValue;
                    var id = Svc.Data.GetExcelSheet<LogMessage>()
                        ?.FirstOrDefault(x => x.Text.ToString() == text).RowId;

                    LureSuccess = GatherBuddy.GameData.Fishes.Values.FirstOrDefault(f => f.FishData?.Unknown_70_1 == text) != null;

                    if (LureSuccess)
                        return;

                    LureSuccess = id is 5565 or 5569;
                }
            }
            catch (Exception e)
            {
                GatherBuddy.Log.Error($"Failed to handle message: {e}");
            }
        }

        private readonly GatherBuddy     _plugin;
        private readonly SoundHelper     _soundHelper;
        private readonly AdvancedUnstuck _advancedUnstuck;
        private readonly ActiveItemList  _activeItemList;

        public Reflection.ArtisanExporter ArtisanExporter;
        public TaskManager                TaskManager { get; }

        private           bool             _enabled { get; set; } = false;

        public bool Waiting
        {
            get;
            private set
            {
                field                                  = value;
            }
        } = false;

        public unsafe bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled == value)
                    return;

                if (!value)
                {
                    AutoStatus = "Idle...";
                    TaskManager.Abort();
                    YesAlready.Unlock();
                    _fishingYesAlreadyUnlocked = false;

                    _activeItemList.Reset();
                    Waiting                    = false;
                    ActionSequence             = null;
                    CurrentCollectableRotation = null;
                    
                    CleanupAutoHook();

                    if (VNavmesh.Enabled && IsPathGenerating)
                        VNavmesh.Nav.PathfindCancelAll();
                    StopNavigation();
                    CurrentFarNodeLocation   = null;
                    _homeWorldWarning        = false;
                    _diademQueuingInProgress = false;
                    FarNodesSeenSoFar.Clear();
                    VisitedNodes.Clear();
                    _diademSpawnAreaLastChecked.Clear();
                    _currentDiademPatrolTarget = null;
                    _diademRecentlyGatheredNodes.Clear();
                    _diademArborCallUsedAt = DateTime.MinValue;
                    _diademArborCallTarget = null;
                    _diademVisitedNodes.Clear();
                    _lastNonTimedNodeTerritory = 0;
                    _lastUmbralWeather = 0;
                    _hasGatheredUmbralThisSession = false;
                    _autoRetainerWasEnabledBeforeDiadem = false;
                    
                    if (_autoRetainerMultiModeEnabled && AutoRetainer.IsEnabled)
                    {
                        try
                        {
                            AutoRetainer.DisableAllFunctions?.Invoke();
                            _autoRetainerMultiModeEnabled = false;
                            _originalCharacterNameWorld = null;
                            GatherBuddy.Log.Debug("Disabled AutoRetainer MultiMode on AutoGather shutdown");
                        }
                        catch (Exception e)
                        {
                            GatherBuddy.Log.Error($"Failed to disable AutoRetainer MultiMode: {e.Message}");
                        }
                    }
                }
                else
                {
                    WentHome = true; //Prevents going home right after enabling auto-gather
                    if (AutoHook.Enabled)
                        AutoHook.SetPluginState(false); //Make sure AutoHook doesn't interfere with us
                    YesAlready.Lock();
                    DisableQuickGathering();
                }

                _enabled = value;
                _plugin.Ipc.AutoGatherEnabledChanged(value);
            }
        }

        public bool GoHome()
        {
            StopNavigation();

            if (WentHome)
                return false;

            WentHome = true;

            if (Dalamud.Conditions[ConditionFlag.BoundByDuty])
                return false;

            if (Lifestream.Enabled && !Lifestream.IsBusy())
            {
                var command = GatherBuddy.Config.AutoGatherConfig.LifestreamCommand;
                if (command.Contains("/li "))
                    command = command.Replace("/li ", "");
                Lifestream.ExecuteCommand(command);
                TaskManager.EnqueueImmediate(() => !Lifestream.IsBusy(), 120000, "Wait until Lifestream is done");
                return true;
            }
            else
            {
                GatherBuddy.Log.Warning("Lifestream not found or not ready");
                return false;
            }
        }

        private unsafe void DisableQuickGathering()
        {
            try
            {
                var raptureAtkModule = RaptureAtkModule.Instance();
                if (raptureAtkModule == null)
                    return;

                raptureAtkModule->QuickGatheringEnabled = false;
            }
            catch (Exception e)
            {
                GatherBuddy.Log.Error($"Failed to disable Quick Gathering: {e.Message}");
            }
        }

        private class NoGatherableItemsInNodeException : Exception
        { }

        private class NoCollectableActionsException : Exception
        { }

        private bool _diademQueuingInProgress = false;
        private bool _homeWorldWarning        = false;
        private bool _autoRetainerMultiModeEnabled = false;
        private string? _originalCharacterNameWorld = null;
        private bool _autoRetainerWasEnabledBeforeDiadem = false;

        public void DoAutoGather()
        {
            var currentTerritory = Svc.ClientState.TerritoryType;
            if (_lastTerritory != currentTerritory)
            {
                _lastTerritory = currentTerritory;
                
                var isInDiademOrFirmament = currentTerritory is 901 or 929 or 939 or 886;
                var wasInDiademOrFirmament = _lastTerritory is 901 or 929 or 939 or 886;
                
                if (isInDiademOrFirmament && !wasInDiademOrFirmament)
                {
                    _autoRetainerWasEnabledBeforeDiadem = GatherBuddy.Config.AutoGatherConfig.AutoRetainerMultiMode;
                    if (_autoRetainerWasEnabledBeforeDiadem)
                    {
                        GatherBuddy.Config.AutoGatherConfig.AutoRetainerMultiMode = false;
                        GatherBuddy.Log.Information("Temporarily disabled AutoRetainer integration while in Diadem/Firmament");
                    }
                }
                else if (!isInDiademOrFirmament && wasInDiademOrFirmament)
                {
                    if (_autoRetainerWasEnabledBeforeDiadem)
                    {
                        GatherBuddy.Config.AutoGatherConfig.AutoRetainerMultiMode = true;
                        GatherBuddy.Log.Information("Re-enabled AutoRetainer integration after leaving Diadem/Firmament");
                        _autoRetainerWasEnabledBeforeDiadem = false;
                    }
                }
                
                if (currentTerritory is 901 or 929 or 939)
                {
                    _hasGatheredUmbralThisSession = false;
                    _lastUmbralWeather = 0;
                }
            }

            // Always check these first
            if (!IsGathering)
                LuckUsed = false; //Reset the flag even if auto-gather was disabled mid-gathering

            if (!Enabled)
            {
                return;
            }

            // If we are not gathering and _currentGatherTarget is set, we just finished gathering or left the node
            if (!IsGathering && _currentGatherTarget != null)
            {
                var gatherTarget = _currentGatherTarget!;
                // Mark the node as visited if possible
                var targetNode = Svc.Targets.Target ?? Svc.Targets.PreviousTarget;
                if (targetNode != null && targetNode.ObjectKind is ObjectKind.GatheringPoint)
                {
                    _activeItemList.MarkVisited(targetNode);
                    var gatherable = gatherTarget.Value.Gatherable;
                    var node = gatherTarget.Value.Node;
                    
                    if (Functions.InTheDiadem())
                    {
                        var isUmbralNode = UmbralNodes.UmbralNodeData.Any(entry => entry.NodeId == targetNode.DataId);
                        
                        if (isUmbralNode)
                        {
                            _hasGatheredUmbralThisSession = true;
                            
                            FarNodesSeenSoFar.Clear();
                            _diademVisitedNodes.Clear();
                            _diademRecentlyGatheredNodes.Clear();
                        }
                        else
                        {
                            _diademRecentlyGatheredNodes.AddLast(targetNode.Position);
                            while (_diademRecentlyGatheredNodes.Count > DiademNodeRespawnWindow)
                                _diademRecentlyGatheredNodes.RemoveFirst();
                                
                            _diademVisitedNodes.AddLast(targetNode.Position);
                            while (_diademVisitedNodes.Count > DiademVisitedNodeTrackingCount)
                                _diademVisitedNodes.RemoveFirst();
                                
                            FarNodesSeenSoFar.Add(targetNode.Position);
                        }
                    }
                    
                    if (gatherable != null && (gatherable.NodeType == NodeType.Regular || gatherable.NodeType == NodeType.Ephemeral)
                        && (VisitedNodes.Last?.Value != targetNode.DataId)
                        && node != null && node.WorldPositions.ContainsKey(targetNode.DataId))
                    {
                        if (!Functions.InTheDiadem())
                            FarNodesSeenSoFar.Clear();
                        VisitedNodes.AddLast(targetNode.DataId);
                        while (VisitedNodes.Count > (node.WorldPositions.Count <= 4 ? 2 : 4))
                            VisitedNodes.RemoveFirst();
                    }
                }
                // Unset the current gather target when leaving the node
                _currentGatherTarget = null;
            }


            try
            {
                if (!NavReady)
                {
                    AutoStatus = "Waiting for Navmesh...";
                    return;
                }
            }
            catch (Exception)
            {
                //GatherBuddy.Log.Error(e.Message);
                AutoStatus = "vnavmesh communication failed. Do you have it installed??";
                return;
            }

            if (TaskManager.IsBusy)
            {
                //GatherBuddy.Log.Verbose("TaskManager has tasks, skipping DoAutoGather");
                return;
            }

            if (!_homeWorldWarning && !Functions.OnHomeWorld())
            {
                _homeWorldWarning = true;
                Communicator.PrintError("You are not on your home world, some items will not be gatherable.");
            }

            if (DiscipleOfLand.NextTreasureMapAllowance == DateTime.MinValue)
            {
                //Wait for timer refresh
                AutoStatus = "Refreshing timers...";
                DiscipleOfLand.RefreshNextTreasureMapAllowance();
                return;
            }

            if (!CanAct && !_diademQueuingInProgress)
            {
                AutoStatus = Dalamud.Conditions[ConditionFlag.Gathering] ? "Gathering..." : "Player is busy...";
                return;
            }


            if (FreeInventorySlots == 0)
            {
                if (HasReducibleItems())
                {
                    if (IsGathering)
                        CloseGatheringAddons();
                    else
                        ReduceItems(false);
                }
                else
                {
                    AbortAutoGather("Inventory is full");
                }

                return;
            }

            if (_activeItemList.GetNextOrDefault(new List<uint>()).Any(g => g.Fish != null)
             && !GatherBuddy.Config.AutoGatherConfig.FishDataCollection)
            {
                Communicator.PrintError(
                    "You have fish on your auto-gather list but you have not opted in to fishing data collection. Auto-gather cannot continue. Please enable fishing data collection in your configuration options or remove fish from your auto-gather lists.");
                AbortAutoGather();
                return;
            }

            if (IsGathering)
            {
                // Set the current gather target when entering a node
                if (_currentGatherTarget == null)
                {
                    if (!_activeItemList.IsInitialized)
                        _currentGatherTarget = _activeItemList.GetNextOrDefault([Svc.Targets.Target!.DataId]).FirstOrDefault();
                    else
                        _currentGatherTarget = _activeItemList.CurrentOrDefault;
                }

                IEnumerable<GatherTarget> gatherTarget = _currentGatherTarget != null ? new[] { (GatherTarget)_currentGatherTarget } : Array.Empty<GatherTarget>();

                if (!GatherBuddy.Config.AutoGatherConfig.DoGathering)
                    return;

                AutoStatus = "Gathering...";
                StopNavigation();

                var fish = _activeItemList.GetNextOrDefault(new List<uint>()).Where(g => g.Fish != null);
                if (fish.Any() && Player.Job == Job.FSH)
                {
                    if (GatherBuddy.Config.AutoGatherConfig.UseNavigation)
                    {
                        var pathGenerating = IsPathGenerating;
                        var pathing = IsPathing;
                        var unstuckResult = _advancedUnstuck.Check(CurrentDestination, pathGenerating, pathing);
                        if (unstuckResult == AdvancedUnstuckCheckResult.Fail)
                        {
                            StopNavigation();
                            AutoStatus = "Advanced unstuck in progress!";
                            return;
                        }
                        DoFishMovement(fish);
                    }
                    DoFishingTasks(fish);
                    return;
                }

                if (!fish.Any() && Player.Job == Job.FSH)
                {
                    QueueQuitFishingTasks();
                }

                try
                {
                    DoActionTasks(gatherTarget);
                }
                catch (NoGatherableItemsInNodeException)
                {
                    CloseGatheringAddons();
                }
                catch (NoCollectableActionsException)
                {
                    Communicator.PrintError(
                        "Unable to pick a collectability increasing action to use. Make sure that at least one of the collectable actions is enabled.");
                    AbortAutoGather();
                }


                return;
            }

            if (AutoRetainer.IsEnabled && GatherBuddy.Config.AutoGatherConfig.AutoRetainerMultiMode)
            {
                if (ShouldWaitForAutoRetainer())
                {
                    Waiting = true;
                    _plugin.Ipc.AutoGatherWaiting();
                    return;
                }
            }

            ActionSequence             = null;
            CurrentCollectableRotation = null;

            //Cache IPC call results
            var isPathGenerating = IsPathGenerating;
            var isPathing        = IsPathing;

            switch (_advancedUnstuck.Check(CurrentDestination, isPathGenerating, isPathing))
            {
                case AdvancedUnstuckCheckResult.Pass: break;
                case AdvancedUnstuckCheckResult.Wait: return;
                case AdvancedUnstuckCheckResult.Fail:
                    StopNavigation();
                    AutoStatus = $"Advanced unstuck in progress!";
                    return;
            }

            if (isPathGenerating)
            {
                AutoStatus = "Generating path...";
                return;
            }

            if (Player.Job is Job.BTN or Job.MIN or Job.FSH
             && !isPathing
             && !Svc.Condition[ConditionFlag.Mounted])
            {
                if (SpiritbondMax > 0)
                {
                    if (IsGathering)
                    {
                        QueueQuitFishingTasks();
                    }

                    DoMateriaExtraction();
                    return;
                }

                if (FreeInventorySlots < 20 && HasReducibleItems())
                {
                    ReduceItems(false);
                    return;
                }
            }

            if (Functions.InTheDiadem())
            {
                var currentWeather = EnhancedCurrentWeather.GetCurrentWeatherId();
                var isUmbralWeather = UmbralNodes.IsUmbralWeather(currentWeather);
                var wasUmbralWeather = UmbralNodes.IsUmbralWeather(_lastUmbralWeather);
                var weatherChanged = currentWeather != _lastUmbralWeather && _lastUmbralWeather != 0;
                
                var hasUmbralItems = HasUmbralItemsInActiveList();
                var hasNormalDiademItems = _activeItemList.Any(target => target.Gatherable != null && 
                    !UmbralNodes.UmbralNodeData.Any(entry => entry.ItemIds.Contains(target.Gatherable.ItemId)) &&
                    target.Location.Territory.Id is 901 or 929 or 939);
                
                if (weatherChanged && isUmbralWeather && !wasUmbralWeather && hasUmbralItems && hasNormalDiademItems)
                {
                    StopNavigation();
                    Svc.Log.Information($"[Umbral] Weather changed to umbral ({currentWeather}) - leaving Diadem for clean state");
                    _lastUmbralWeather = currentWeather;
                    LeaveTheDiadem();
                    return;
                }
                
                if (weatherChanged && !isUmbralWeather && wasUmbralWeather && hasUmbralItems && hasNormalDiademItems)
                {
                    StopNavigation();
                    Svc.Log.Information($"[Umbral] Weather changed to normal ({currentWeather}) - leaving Diadem for clean state");
                    _lastUmbralWeather = currentWeather;
                    LeaveTheDiadem();
                    return;
                }
                
                if (weatherChanged && !isUmbralWeather && wasUmbralWeather && hasUmbralItems && !hasNormalDiademItems)
                {
                    _hasGatheredUmbralThisSession = false;
                    _lastUmbralWeather = currentWeather;
                    Svc.Log.Information($"[Umbral] Weather changed to normal with only umbral items - leaving Diadem");
                    StopNavigation();
                    LeaveTheDiadem();
                    return;
                }
                
                _lastUmbralWeather = currentWeather;
            }

            var nearbyNodes = Svc.Objects.Where(o => o.ObjectKind == ObjectKind.GatheringPoint && o.IsTargetable).Select(o => o.DataId);
            
            var hasNormalDiademItemsInList = _activeItemList.Any(target => target.Gatherable != null && 
                !UmbralNodes.UmbralNodeData.Any(entry => entry.ItemIds.Contains(target.Gatherable.ItemId)) &&
                target.Location.Territory.Id is 901 or 929 or 939);
            
            var next = _activeItemList.GetNextOrDefault(nearbyNodes)
                .Where(target => {
                    if (Functions.InTheDiadem() && _hasGatheredUmbralThisSession && hasNormalDiademItemsInList)
                    {
                        var isUmbralItem = IsUmbralItem(target.Item);
                        if (isUmbralItem)
                        {
                            return false;
                        }
                    }
                    return true;
                })
                .OrderByDescending(nodes => nodes.Item.ItemId);
            
            if (Functions.InTheDiadem() && _hasGatheredUmbralThisSession)
            {
                var hasUmbralItems = HasUmbralItemsInActiveList();
                
                if (hasUmbralItems)
                {
                    Svc.Log.Information($"[Umbral] Gathered umbral node - leaving Diadem to reset session");
                    StopNavigation();
                    LeaveTheDiadem();
                    return;
                }
            }
            if (!next.Any())
            {
                if (!_activeItemList.HasItemsToGather)
                {
                    AbortAutoGather();
                    return;
                }
                
                var hasUmbralItemsInList = HasUmbralItemsInActiveList();
                if (hasUmbralItemsInList)
                {
                    if (Functions.InTheDiadem())
                    {
                        var currentWeather = EnhancedCurrentWeather.GetCurrentWeatherId();
                        var isUmbralWeather = UmbralNodes.IsUmbralWeather(currentWeather);
                        
                        if (!isUmbralWeather)
                        {
                            AutoStatus = "Waiting in Diadem for umbral weather...";
                            if (!Waiting)
                            {
                                Waiting = true;
                                _plugin.Ipc.AutoGatherWaiting();
                            }
                            return;
                        }
                    }
                    
                    var firstUmbralItem = GetFirstUmbralItemFromActiveList();
                    if (firstUmbralItem.Item != null)
                    {
                        var diademNode = GatherBuddy.GameData.GatheringNodes.Values
                            .FirstOrDefault(node => node.Territory.Id is 901 or 929 or 939);
                            
                        if (diademNode != null)
                        {
                            var syntheticTarget = new GatherTarget(firstUmbralItem.Item, diademNode, Time.TimeInterval.Always, firstUmbralItem.Quantity);
                            next = new[] { syntheticTarget }.OrderByDescending(nodes => nodes.Item.ItemId);
                            AutoStatus = "Traveling to Diadem for umbral items...";
                        }
                    }
                }
                
                if (!next.Any())
                {
                    if (GatherBuddy.Config.AutoGatherConfig.GoHomeWhenIdle)
                        if (GoHome())
                            return;

                    if (HasReducibleItems())
                    {
                        ReduceItems(true);
                        return;
                    }

                    if (!Waiting)
                    {
                        Waiting = true;
                        _plugin.Ipc.AutoGatherWaiting();
                    }

                    AutoStatus = "No available items to gather";
                    return;
                }
            }

            Waiting = false;

            if (next.Any(n => n.Item.ItemData.IsCollectable
                 && !CheckCollectablesUnlocked(n.Fish != null ? GatheringType.Fisher : n.Gatherable!.GatheringType.ToGroup())))
            {
                AbortAutoGather();
                return;
            }

            if (RepairIfNeeded())
                return;

            if (!GatherBuddy.Config.AutoGatherConfig.UseNavigation)
            {
                AutoStatus = "Waiting for Gathering Point... (No Nav Mode)";
                return;
            }

            var territoryId = Svc.ClientState.TerritoryType;
            //Idyllshire to The Dravanian Hinterlands
            if ((territoryId == 478 && (next.First().Node?.Territory.Id == 399 || next.First().FishingSpot?.Territory.Id == 399))
             || (territoryId == 418 && next.First().Node?.Territory.Id is 901 or 929 or 939) && Lifestream.Enabled)
            {
                var aetheryte = Svc.Objects.Where(x => x.ObjectKind == ObjectKind.Aetheryte && x.IsTargetable)
                    .OrderBy(x => x.Position.DistanceToPlayer()).FirstOrDefault();
                if (aetheryte != null)
                {
                    if (aetheryte.Position.DistanceToPlayer() > 10)
                    {
                        AutoStatus = "Moving to aetheryte...";
                        if (!isPathing && !isPathGenerating)
                            Navigate(aetheryte.Position, false);
                    }
                    else if (!Lifestream.IsBusy())
                    {
                        AutoStatus = "Teleporting...";
                        StopNavigation();
                        string name = string.Empty;
                        switch (territoryId)
                        {
                            case 478:
                                var xCoord = next.First().Node?.DefaultXCoord ?? next.First().FishingSpot?.DefaultXCoord ?? 0;
                                var exit = xCoord < 2000 ? 91u : 92u;
                                name = Dalamud.GameData.GetExcelSheet<Lumina.Excel.Sheets.Aetheryte>().GetRow(exit).AethernetName.Value.Name
                                    .ToString();
                                break;
                            case 418:
                                name = Dalamud.GameData.GetExcelSheet<TerritoryType>().GetRow(886).PlaceName.Value.Name.ToString()
                                    .Split(" ")[1];
                                break;
                        }

                        TaskManager.Enqueue(() => Lifestream.AethernetTeleport(name));
                        TaskManager.DelayNext(1000);
                        TaskManager.Enqueue(() => GenericHelpers.IsScreenReady());
                    }

                    return;
                }
            }

            if (territoryId == 886 && next.First().Node?.Territory.Id is 901 or 929 or 939)
            {
                if (JobAsGatheringType == GatheringType.Unknown)
                {
                    var requiredGatheringType = next.First().Location.GatheringType.ToGroup();
                    if (ChangeGearSet(requiredGatheringType, 2400))
                    {
                        return;
                    }
                    else
                    {
                        AbortAutoGather();
                        return;
                    }
                }
                
                var dutyNpc                    = Svc.Objects.FirstOrDefault(o => o.DataId == 1031694);
                var selectStringAddon          = Dalamud.GameGui.GetAddonByName("SelectString");
                var talkAddon                  = Dalamud.GameGui.GetAddonByName("Talk");
                var selectYesNoAddon           = Dalamud.GameGui.GetAddonByName("SelectYesno");
                var contentsFinderConfirmAddon = Dalamud.GameGui.GetAddonByName("ContentsFinderConfirm");
                Svc.Log.Verbose($"Addons: {selectStringAddon}, {talkAddon}, {selectYesNoAddon}, {contentsFinderConfirmAddon}");
                if (dutyNpc != null && dutyNpc.Position.DistanceToPlayer() > 3)
                {
                    AutoStatus = "Moving to Diadem NPC...";
                    var point = VNavmesh.Query.Mesh.NearestPoint(dutyNpc.Position, 10, 10000);
                    if (CurrentDestination != point || (!isPathing && !isPathGenerating))
                    {
                        Navigate(point, false);
                    }
                    return;
                }
                else
                    switch (Dalamud.Conditions[ConditionFlag.OccupiedInQuestEvent])
                    {
                        case false when contentsFinderConfirmAddon > 0:
                        {
                            var contents = new AddonMaster.ContentsFinderConfirm(contentsFinderConfirmAddon);
                            TaskManager.Enqueue(contents.Commence);
                            TaskManager.Enqueue(() => _diademQueuingInProgress = false);
                            TaskManager.Enqueue(() => Dalamud.Conditions[ConditionFlag.BoundByDuty]);
                            return;
                        }
                        case false when contentsFinderConfirmAddon == nint.Zero
                         && selectStringAddon == nint.Zero
                         && selectYesNoAddon == nint.Zero:
                            unsafe
                            {
                                var targetSystem = TargetSystem.Instance();
                                if (targetSystem == null)
                                    return;

                                TaskManager.Enqueue(StopNavigation);
                                TaskManager.Enqueue(()
                                    => targetSystem->OpenObjectInteraction(
                                        (FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)dutyNpc.Address));
                                TaskManager.Enqueue(() => Dalamud.Conditions[ConditionFlag.OccupiedInQuestEvent]);
                                TaskManager.Enqueue(() => _diademQueuingInProgress = true);
                                return;
                            }
                        case true when selectStringAddon > 0:
                        {
                            var select = new AddonMaster.SelectString(selectStringAddon);
                            TaskManager.Enqueue(() => select.Entries[0].Select());
                            return;
                        }
                        case true when selectYesNoAddon > 0:
                        {
                            var yesNo = new AddonMaster.SelectYesno(selectYesNoAddon);
                            TaskManager.Enqueue(yesNo.Yes);
                            TaskManager.DelayNext(5000);
                            return;
                        }
                        case true when talkAddon > 0:
                        {
                            var talk = new AddonMaster.Talk(talkAddon);
                            TaskManager.Enqueue(talk.Click);
                            return;
                        }
                    }
            }

            var forcedAetheryte = ForcedAetherytes.ZonesWithoutAetherytes
                .FirstOrDefault(z => z.ZoneId == next.First().Location.Territory.Id);
            if (forcedAetheryte.ZoneId != 0
             && GatherBuddy.GameData.Aetherytes[forcedAetheryte.AetheryteId].Territory.Id == territoryId)
            {
                if (territoryId == 478 && !Lifestream.Enabled)
                    AutoStatus = $"Install Lifestream or teleport to {next.First().Location.Territory.Name} manually";
                else
                    AutoStatus = "Manual teleporting required";
                return;
            }

            //At this point, we are definitely going to gather something, so we may go home after that.
            if (Lifestream.Enabled)
                Lifestream.Abort();
            WentHome = false;
            
            var isTimedNode = next.First().Node?.Times.AlwaysUp() == false;
            if (!isTimedNode && next.First().Location.Territory.Id == territoryId)
            {
                _lastNonTimedNodeTerritory = territoryId;
            }

            
            // fuck Diadem. Covering all zoneIDs for Diadem to gather g2/3/4.
            var isCurrentDiadem = territoryId is 901 or 929 or 939;
            var isTargetDiadem = next.First().Location.Territory.Id is 901 or 929 or 939;
            
            if (next.First().Location.Territory.Id != territoryId && !(isCurrentDiadem && isTargetDiadem))
            {
                if (GatherBuddy.Config.AutoGatherConfig.SortingMethod == AutoGatherConfig.SortingType.Location)
                {
                    if (_lastNonTimedNodeTerritory != 0 && _lastNonTimedNodeTerritory != territoryId)
                    {
                        var itemsInPreviousZone = _activeItemList
                            .Where(i => i.Node?.Territory.Id == _lastNonTimedNodeTerritory)
                            .Where(i => i.Node?.Times.AlwaysUp() != false)
                            .ToList();
                        
                        if (itemsInPreviousZone.Any())
                        {
                            var previousZoneItem = itemsInPreviousZone.First();
                            if (!LocationMatchesJob(previousZoneItem.Location))
                            {
                                if (ChangeGearSet(previousZoneItem.Location.GatheringType.ToGroup(), 2400))
                                {
                                    return;
                                }
                            }
                            
                            StopNavigation();
                            if (!MoveToTerritory(previousZoneItem.Location))
                                AbortAutoGather();
                            return;
                        }
                    }
                    
                    var itemsInCurrentZone = _activeItemList
                        .Where(i => i.Node?.Territory.Id == territoryId)
                        .Where(i => i.Node?.Times.AlwaysUp() != false)
                        .ToList();
                    
                    if (itemsInCurrentZone.Any())
                    {
                        var currentZoneItem = itemsInCurrentZone.First();
                        if (!LocationMatchesJob(currentZoneItem.Location))
                        {
                            if (ChangeGearSet(currentZoneItem.Location.GatheringType.ToGroup(), 2400))
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                
                if (Dalamud.Conditions[ConditionFlag.BoundByDuty] && !Functions.InTheDiadem())
                {
                    AutoStatus = "Can not teleport when bound by duty";
                    return;
                }
                else if (Functions.InTheDiadem())
                {
                    // Check if we're waiting for umbral weather - if so, don't leave due to territory mismatch
                    var currentWeather = EnhancedCurrentWeather.GetCurrentWeatherId();
                    var isUmbralWeather = UmbralNodes.IsUmbralWeather(currentWeather);
                    var hasUmbralItems = next.Any(target => target.Gatherable != null && 
                        UmbralNodes.UmbralNodeData.Any(entry => entry.ItemIds.Contains(target.Gatherable.ItemId)));
                    
                    if (!isUmbralWeather && hasUmbralItems)
                    {
                        // We're waiting for umbral weather - don't leave due to territory mismatch
                        // Also reset session flag in case it wasn't reset in DoNodeMovementDiadem
                        _hasGatheredUmbralThisSession = false;
                        AutoStatus = "Waiting in Diadem for next umbral weather...";
                        return;
                    }
                    else
                    {
                        LeaveTheDiadem();
                        return;
                    }
                }

                AutoStatus = "Teleporting...";
                StopNavigation();

                if (!MoveToTerritory(next.First().Location))
                    AbortAutoGather();

                // Reset target to pick up closest item after teleport
                next = default;

                return;
            }

            var targetGatheringType = next.First().Location.GatheringType.ToGroup();
            
            var config = MatchConfigPreset(next.First().Gatherable);

            if (DoUseConsumablesWithoutCastTime(config))
                return;
            var firstItem = next.First().Gatherable;
            
            if (firstItem != null && UmbralNodes.UmbralNodeData.Any(entry => entry.ItemIds.Contains(firstItem.ItemId)))
            {
                var currentWeather = EnhancedCurrentWeather.GetCurrentWeatherId();
                if (UmbralNodes.IsUmbralWeather(currentWeather))
                {
                    var umbralWeather = (UmbralNodes.UmbralWeatherType)currentWeather;
                    targetGatheringType = umbralWeather switch
                    {
                        UmbralNodes.UmbralWeatherType.UmbralFlare => GatheringType.Miner,
                        UmbralNodes.UmbralWeatherType.UmbralLevin => GatheringType.Miner,
                        UmbralNodes.UmbralWeatherType.UmbralDuststorms => GatheringType.Botanist,
                        UmbralNodes.UmbralWeatherType.UmbralTempest => GatheringType.Botanist,
                        _ => targetGatheringType
                    };
                }
            }
            
            var shouldSkipJobSwitch = Functions.InTheDiadem() && _hasGatheredUmbralThisSession;
            
            if (shouldSkipJobSwitch)
            {
                Svc.Log.Information($"[Umbral] Skipping job switch in Diadem after umbral gathering (staying on {JobAsGatheringType})");
            }
            
            if (JobAsGatheringType != targetGatheringType && !shouldSkipJobSwitch)
            {
                if (!ChangeGearSet(targetGatheringType, 2400))
                    AbortAutoGather();
                return;
            }

            if (next.First().Fish != null)
            {
                DoFishMovement(next);
                return;
            }

            
            if (next.First().Gatherable != null)
            {
                DoNodeMovement(next, config);
                return;
            }

            AutoStatus = "Fell out of control loop unexpectedly. Please report this error.";
            return;
        }

        public readonly Dictionary<GatherTarget, (Vector3 Position, Angle Rotation, DateTime Expiration)> FishingSpotData = new();
        private readonly Dictionary<Vector3, DateTime> _fishingSpotDismountAttempts = new();

        private void DoFishMovement(IEnumerable<GatherTarget> next)
        {
            var fish = next.First(ne => ne.Fish != null);

            if (!FishingSpotData.TryGetValue(fish, out var fishingSpotData))
            {
                var positionData = _plugin.FishRecorder.GetPositionForFishingSpot(fish!.FishingSpot);
                if (!positionData.HasValue)
                {
                    Communicator.PrintError(
                        $"No position data for fishing spot {fish.FishingSpot.Name}. Auto-Fishing cannot continue. Please, manually fish at least once at {fish.FishingSpot.Name} so GBR can know its location.");
                    AbortAutoGather();
                    return;
                }

                FishingSpotData.Add(fish, (positionData.Value.Position, positionData.Value.Rotation, DateTime.MaxValue));
                return;
            }

            if (fishingSpotData.Expiration < DateTime.Now)
            {
                Svc.Log.Debug("Time for a new fishing spot!");
                var oldPosition = fishingSpotData.Position;
                FishingSpotData.Remove(fish);

                if (GatherBuddy.Config.AutoGatherConfig.UseAutoHook && AutoHook.Enabled)
                {
                    AutoHook.SetPluginState?.Invoke(false);
                    TaskManager.DelayNext(500);
                    Svc.Log.Debug("[AutoGather] AutoHook disabled for relocation to a new position at the same fishing spot");
                }

                if (IsGathering || IsFishing)
                {
                    QueueQuitFishingTasks();
                }

                const float MinRelocationDistance = 10.0f;
                var positionData = _plugin.FishRecorder.GetPositionForFishingSpot(fish!.FishingSpot, oldPosition, MinRelocationDistance);
                if (!positionData.HasValue)
                {
                    Communicator.PrintError(
                        $"No alternate position data for fishing spot {fish.FishingSpot.Name}. Auto-Fishing cannot continue.");
                    AbortAutoGather();
                    return;
                }

                FishingSpotData.Add(fish, (positionData.Value.Position, positionData.Value.Rotation, DateTime.MaxValue));

                return;
            }

            if (IsFishing)
            {
                StopNavigation();
                AutoStatus = "Fishing...";
                DoFishingTasks(next);
                return;
            }

            if (Vector3.Distance(fishingSpotData.Position, Player.Position) < 1)
            {
                if (Dalamud.Conditions[ConditionFlag.Mounted])
                {
                    if (!_fishingSpotDismountAttempts.TryGetValue(fishingSpotData.Position, out var firstAttempt))
                    {
                        _fishingSpotDismountAttempts[fishingSpotData.Position] = DateTime.Now;
                    }
                    else if ((DateTime.Now - firstAttempt).TotalSeconds > 5)
                    {
                        Svc.Log.Warning("[AutoGather] Failed to dismount at fishing spot for 5+ seconds, forcing unstuck to find landable spot");
                        _fishingSpotDismountAttempts.Remove(fishingSpotData.Position);
                        _advancedUnstuck.ForceFishing();
                        AutoStatus = "Can't land here, finding landable spot...";
                        return;
                    }
                    
                    EnqueueDismount();
                    AutoStatus = "Dismounting...";
                    return;
                }
                
                if (_fishingSpotDismountAttempts.ContainsKey(fishingSpotData.Position))
                {
                    _fishingSpotDismountAttempts.Remove(fishingSpotData.Position);
                }

                var playerAngle = new Angle(Player.Rotation);
                if (playerAngle != fishingSpotData.Rotation)
                {
                    TaskManager.Enqueue(() => SetRotation(fishingSpotData.Rotation));
                    AutoStatus = "Adjusting rotation...";
                    return;
                }

                if (fishingSpotData.Expiration == DateTime.MaxValue)
                {
                    var newExpiration = DateTime.Now.AddMinutes(GatherBuddy.Config.AutoGatherConfig.MaxFishingSpotMinutes);
                    FishingSpotData[fish] = (fishingSpotData.Position, fishingSpotData.Rotation, newExpiration);
                    Svc.Log.Information($"[AutoGather] Started fishing spot timer: {GatherBuddy.Config.AutoGatherConfig.MaxFishingSpotMinutes} minutes");
                }
                else
                {
                    Svc.Log.Debug($"Fishing Spot is valid for {(fishingSpotData.Expiration - DateTime.Now).TotalSeconds} seconds");
                }

                StopNavigation();
                AutoStatus = "Fishing...";
                DoFishingTasks(next);
                return;
            }

            if (CurrentDestination != fishingSpotData.Position)
            {
                StopNavigation();
                AutoStatus = "Moving to fishing spot...";
                if (IsGathering || IsFishing)
                {
                    QueueQuitFishingTasks();
                }

                MoveToFishingSpot(fishingSpotData.Position, fishingSpotData.Rotation);
            }
        }

        private void DoNodeMovementDiadem(IEnumerable<GatherTarget> next, ConfigPreset config)
        {
            var targetGatheringType = next.First().Location.GatheringType;
            var currentJob = JobAsGatheringType;
            
            var currentWeather = EnhancedCurrentWeather.GetCurrentWeatherId();
            var isUmbralWeather = UmbralNodes.IsUmbralWeather(currentWeather);
            
            var hasUmbralItems = HasUmbralItemsInActiveList();
                
            if (isUmbralWeather && hasUmbralItems)
            {
                var currentUmbralWeather = (UmbralNodes.UmbralWeatherType)currentWeather;
                var relevantUmbralNodes = UmbralNodes.GetNodesForWeatherAndType(currentUmbralWeather, currentJob);
                
                foreach (var nodeId in relevantUmbralNodes)
                {
                    var nodeItems = UmbralNodes.GetItemsForNode(nodeId);
                    var hasNeededItems = GetActiveItemsNeedingGathering()
                        .Any(item => nodeItems.Contains(item.Item.ItemId));
                    
                    if (hasNeededItems && GatherBuddy.GameData.WorldCoords.TryGetValue(nodeId, out var umbralPositions))
                    {
                        var validPositions = umbralPositions
                            .Where(pos => !IsBlacklisted(pos))
                            .OrderBy(pos => Vector3.Distance(Player.Position, pos))
                            .ToList();
                        
                        if (validPositions.Any())
                        {
                            var targetPosition = validPositions.First();
                            var distance = Vector3.Distance(Player.Position, targetPosition);
                            
                            const float CloseEnoughDistance = 50f;
                            
                            if (distance > CloseEnoughDistance)
                            {
                                AutoStatus = $"Rushing to umbral node {nodeId} (Weather: {currentUmbralWeather}, {distance:F0}y)...";
                                
                                if (!Dalamud.Conditions[ConditionFlag.Mounted] && distance >= GatherBuddy.Config.AutoGatherConfig.MountUpDistance)
                                {
                                    if (GatherBuddy.Config.AutoGatherConfig.MoveWhileMounting)
                                        Navigate(targetPosition, false);
                                    EnqueueMountUp();
                                }
                                else
                                {
                                    Navigate(targetPosition, ShouldFly(targetPosition));
                                }
                                return;
                            }
                            else
                            {
                                // Don't return - let the normal node detection logic below handle finding the actual spawned node
                            }
                        }
                    }
                }
            }
            
            const float RecentlyGatheredDistance = 5f;
            
            var allVisibleNodes = Svc.Objects
                .Where(o => o.ObjectKind == ObjectKind.GatheringPoint)
                .Where(o => !IsBlacklisted(o.Position))
                .Where(o => !_diademRecentlyGatheredNodes.Any(recent => Vector3.Distance(recent, o.Position) < RecentlyGatheredDistance))
                .Where(o =>
                {
                    if (!GatherBuddy.GameData.WorldCoords.TryGetValue(o.DataId, out _))
                        return false;
                    
                    var gatheringPoint = Dalamud.GameData.GetExcelSheet<GatheringPoint>()?.GetRow(o.DataId);
                    if (gatheringPoint == null || !gatheringPoint.HasValue)
                        return false;
                    
                    var nodeBase = gatheringPoint.Value.GatheringPointBase;
                    if (nodeBase.RowId == 0)
                        return false;
                    
                    var nodeGatheringType = (GatheringType)nodeBase.Value.GatheringType.RowId;
                    return nodeGatheringType.ToGroup() == currentJob;
                })
                .ToList();
            
            var nonUmbralNodes = allVisibleNodes
                .Where(o => !UmbralNodes.UmbralNodeData.Any(entry => entry.NodeId == o.DataId))
                .OrderBy(o => Vector3.Distance(Player.Position, o.Position))
                .ToList();
                
            if (isUmbralWeather && hasUmbralItems)
            {
                var umbralNodes = allVisibleNodes
                    .Where(o => UmbralNodes.UmbralNodeData.Any(entry => entry.NodeId == o.DataId))
                    .OrderBy(o => Vector3.Distance(Player.Position, o.Position))
                    .ToList();
                
                if (umbralNodes.Any())
                {
                    allVisibleNodes = umbralNodes;
                    AutoStatus = $"Targeting umbral nodes (Weather: {(UmbralNodes.UmbralWeatherType)currentWeather})...";
                }
                else
                {
                    allVisibleNodes = nonUmbralNodes;
                }
            }
            else
            {
                allVisibleNodes = nonUmbralNodes;
            }

            if (ActivateGatheringBuffs(false))
                return;

            const float NearbyNodeDistance = 150f;
            foreach (var visibleNode in allVisibleNodes.Where(node => !node.IsTargetable))
            {
                var nodePosition = visibleNode.Position;
                if (Vector3.Distance(Player.Position, nodePosition) < NearbyNodeDistance)
                {
                    if (!FarNodesSeenSoFar.Contains(nodePosition))
                        FarNodesSeenSoFar.Add(nodePosition);
                }
            }
            
            const int MaxSeenNodes = 100;
            if (FarNodesSeenSoFar.Count > MaxSeenNodes)
                FarNodesSeenSoFar.Clear();

            var targetableNodes = allVisibleNodes.Where(o => o.IsTargetable).ToList();
            
            if (targetableNodes.Any())
            {
                var closestNode = targetableNodes.First();
                var distance = Vector3.Distance(Player.Position, closestNode.Position);
                
                AutoStatus = $"Moving to Diadem node ({distance:F0}y)...";
                
                Gatherable targetItem;
                if (isUmbralWeather && hasUmbralItems && UmbralNodes.UmbralNodeData.Any(entry => entry.NodeId == closestNode.DataId))
                {
                    var currentUmbralWeather = (UmbralNodes.UmbralWeatherType)currentWeather;
                    var matchingUmbralItem = next
                        .Where(target => target.Gatherable != null)
                        .Where(target => UmbralNodes.UmbralNodeData.Any(entry => entry.ItemIds.Contains(target.Gatherable.ItemId) && 
                                                                                entry.Weather == currentUmbralWeather &&
                                                                                UmbralNodes.GetGatheringType(entry.NodeType) == currentJob))
                        .Select(target => target.Gatherable)
                        .FirstOrDefault();
                        
                    if (matchingUmbralItem != null)
                    {
                        targetItem = matchingUmbralItem;
                    }
                    else
                    {
                        targetItem = next.First().Gatherable;
                    }
                }
                else
                {
                    targetItem = next.First().Gatherable;
                }
                
                MoveToCloseNode(closestNode, targetItem, config);
                return;
            }
            
            AutoStatus = "Searching for next Diadem node...";
            
            var currentTerritoryId = Svc.ClientState.TerritoryType;
            if (!Functions.InTheDiadem())
            {
                AutoStatus = "Not in Diadem, aborting...";
                return;
            }
            
            var potentialNodePositions = GatherBuddy.GameData.GatheringNodes.Values
                .Where(node => node.Territory.Id == currentTerritoryId)
                .Where(node => node.GatheringType.ToGroup() == currentJob)
                .SelectMany(node => node.WorldPositions.Values.SelectMany(positions => positions))
                .Where(pos => !IsBlacklisted(pos))
                .Where(pos => !_diademVisitedNodes.Any(visited => Vector3.Distance(visited, pos) < DiademNodeProximityThreshold))
                .Where(pos => !FarNodesSeenSoFar.Contains(pos))
                .OrderBy(pos => Vector3.Distance(Player.Position, pos))
                .ToList();
            
            if (potentialNodePositions.Any())
            {
                var targetPosition = potentialNodePositions.First();
                var distance = Vector3.Distance(Player.Position, targetPosition);
                
                AutoStatus = $"Moving to next Diadem node ({distance:F0}y)...";
                
                if (!Dalamud.Conditions[ConditionFlag.Mounted] && distance >= GatherBuddy.Config.AutoGatherConfig.MountUpDistance)
                {
                    if (GatherBuddy.Config.AutoGatherConfig.MoveWhileMounting)
                        Navigate(targetPosition, false);
                    EnqueueMountUp();
                }
                else
                {
                    Navigate(targetPosition, ShouldFly(targetPosition));
                }
            }
            else
            {
                AutoStatus = "No suitable Diadem nodes found, waiting...";
            }
        }

        private void DoNodeMovement(IEnumerable<GatherTarget> next, ConfigPreset config)
        {
            if (Functions.InTheDiadem())
            {
                DoNodeMovementDiadem(next, config);
                return;
            }

            var allPositions = next.Where(n => n.Location.Territory.Id == Player.Territory)
                .SelectMany(ne => ne.Node?.WorldPositions
                        .ExceptBy(VisitedNodes, n => n.Key)
                        .SelectMany(w => w.Value)
                        .Where(v => !IsBlacklisted(v))
                 ?? []).Select(s => s)
                .ToHashSet();

            var visibleNodes = Svc.Objects
                .Where(o => allPositions.Contains(o.Position))
                .ToList();

            var closestTargetableNode = visibleNodes
                .Where(o => o.IsTargetable)
                .MinBy(o => Vector3.Distance(Player.Position, o.Position));

            if (ActivateGatheringBuffs(next.First().Gatherable.NodeType is NodeType.Unspoiled or NodeType.Legendary))
                return;

            if (closestTargetableNode != null)
            {
                AutoStatus = "Moving to node...";
                var targetItem = next.First(ti => ti.Node != null && ti.Node.WorldPositions.ContainsKey(closestTargetableNode.DataId))
                    .Gatherable;
                MoveToCloseNode(closestTargetableNode, targetItem, config);
                return;
            }

            AutoStatus = "Moving to far node...";

            if (CurrentDestination != default)
            {
                var currentNode = visibleNodes.FirstOrDefault(o => o.Position == CurrentDestination);
                if (currentNode != null && !currentNode.IsTargetable)
                    GatherBuddy.Log.Verbose($"Far node is not targetable, distance {currentNode.Position.DistanceToPlayer()}.");

                //It takes some time (roundtrip to the server) before a node becomes targetable after it becomes visible,
                //so we need to delay excluding it. But instead of measuring time, we use distance, since character is traveling at a constant speed.
                //Value 50 was determined empirically.
                foreach (var node in allPositions.Where(o => o.DistanceToPlayer() < 50))
                    FarNodesSeenSoFar.Add(node);

                if (CurrentDestination.DistanceToPlayer() < 50)
                {
                    GatherBuddy.Log.Verbose("Far node is not targetable, choosing another");
                }
                else
                {
                    return;
                }
            }

            Vector3 selectedFarNode;

            // only Legendary and Unspoiled show marker
            var timedNode = next.FirstOrDefault(n => n.Time.Start > GatherBuddy.Time.ServerTime.AddSeconds(-8));
            if (ShouldUseFlag && timedNode != default)
            {
                var pos = TimedNodePosition;
                // marker not yet loaded on game
                if (pos == null || timedNode.Time.Start > GatherBuddy.Time.ServerTime.AddSeconds(-8))
                {
                    AutoStatus = "Waiting on flag show up";
                    return;
                }

                selectedFarNode = allPositions
                    .Where(o => Vector2.Distance(pos.Value, new Vector2(o.X, o.Z)) < 10)
                    .OrderBy(o => Vector2.Distance(pos.Value, new Vector2(o.X, o.Z)))
                    .FirstOrDefault();
                if (selectedFarNode == default)
                    selectedFarNode = VNavmesh.Query.Mesh.NearestPoint(new Vector3(pos.Value.X, 0, pos.Value.Y), 10, 10000);
            }
            else
            {
                //Select the closest node
                selectedFarNode = allPositions
                    .Where(fn => !visibleNodes.Select(vn => vn.Position).Contains(fn))
                    .OrderBy(v => Vector3.Distance(Player.Position, v))
                    .FirstOrDefault(n => !FarNodesSeenSoFar.Contains(n));

                if (selectedFarNode == default)
                {
                    FarNodesSeenSoFar.Clear();
                    GatherBuddy.Log.Verbose($"Selected node was null and far node filters have been cleared");
                    return;
                }
            }

            MoveToFarNode(selectedFarNode);
        }

        private unsafe void LeaveTheDiadem()
        {
            AgentModule.Instance()->GetAgentByInternalId(AgentId.ContentsFinderMenu)->Show();
            if (GenericHelpers.TryGetAddonByName("ContentsFinderMenu", out AtkUnitBase* addon))
            {
                TaskManager.Enqueue(() => Callback.Fire(addon, true,  0));
                TaskManager.Enqueue(() => Callback.Fire(addon, false, -2));
                TaskManager.DelayNext(1000);
                TaskManager.Enqueue(() => Callback.Fire((AtkUnitBase*)(nint)Dalamud.GameGui.GetAddonByName("SelectYesno"), true, 0));
                TaskManager.Enqueue(() => GenericHelpers.IsScreenReady());
                return;
            }
        }

        private void AbortAutoGather(string? status = null)
        {
            if (Functions.InTheDiadem())
            {
                LeaveTheDiadem();
                return;
            }

            if (!string.IsNullOrEmpty(status))
                AutoStatus = status;
            if (GatherBuddy.Config.AutoGatherConfig.HonkMode)
                Task.Run(() => _soundHelper.StartHonkSoundTask(3));
            CloseGatheringAddons();
            if (GatherBuddy.Config.AutoGatherConfig.GoHomeWhenDone)
                EnqueueActionWithDelay(() => { GoHome(); });
            TaskManager.Enqueue(() =>
            {
                Enabled    = false;
                AutoStatus = status ?? AutoStatus;
            });
        }

        private unsafe void CloseGatheringAddons(bool closeGathering = true)
        {
            var masterpieceOpen = MasterpieceAddon != null;
            var gatheringOpen   = GatheringAddon != null;
            if (masterpieceOpen)
            {
                EnqueueActionWithDelay(() =>
                {
                    if (MasterpieceAddon is var addon and not null)
                    {
                        Callback.Fire(&addon->AtkUnitBase, true, -1);
                    }
                });
                TaskManager.Enqueue(() => MasterpieceAddon == null,                 "Wait until GatheringMasterpiece addon is closed");
                TaskManager.Enqueue(() => GatheringAddon is var addon and not null, "Wait until Gathering addon pops up");
                TaskManager.DelayNext(
                    300); //There is some delay after the moment the addon pops up (and is ready) before the callback can be used to close it. We wait some time and retry the callback.
            }

            if (closeGathering && (gatheringOpen || masterpieceOpen))
            {
                TaskManager.Enqueue(() =>
                {
                    if (GatheringAddon is var gathering and not null && gathering->IsReady)
                    {
                        Callback.Fire(&gathering->AtkUnitBase, true, -1);
                        TaskManager.DelayNextImmediate(100);
                        return false;
                    }

                    var addon = SelectYesnoAddon;
                    if (addon != null)
                    {
                        EnqueueActionWithDelay(() =>
                        {
                            if (SelectYesnoAddon is var addon and not null)
                            {
                                var master = new AddonMaster.SelectYesno(addon);
                                master.Yes();
                            }
                        }, true);
                        TaskManager.EnqueueImmediate(() => !IsGathering, "Wait until Gathering addon is closed");
                        return true;
                    }

                    return !IsGathering;
                }, "Wait until Gathering addon is closed or SelectYesno addon pops up");
            }
        }

        private bool CheckCollectablesUnlocked(GatheringType gatheringType)
        {
            var level = gatheringType switch
            {
                GatheringType.Miner    => DiscipleOfLand.MinerLevel,
                GatheringType.Botanist => DiscipleOfLand.BotanistLevel,
                GatheringType.Fisher   => DiscipleOfLand.FisherLevel,
                GatheringType.Multiple => Math.Max(DiscipleOfLand.MinerLevel, DiscipleOfLand.BotanistLevel),
                _                      => 0
            };
            if (level < Actions.Collect.MinLevel)
            {
                Communicator.PrintError("You've put a collectable on the gathering list, but your level is not high enough to gather it.");
                return false;
            }

            var questId = gatheringType switch
            {
                GatheringType.Miner    => Actions.Collect.QuestIds.Miner,
                GatheringType.Botanist => Actions.Collect.QuestIds.Botanist,
                _                      => 0u
            };

            if (questId != 0 && !QuestManager.IsQuestComplete(questId))
            {
                Communicator.PrintError("You've put a collectable on the gathering list, but you haven't unlocked the collectables.");
                var sheet      = Dalamud.GameData.GetExcelSheet<Lumina.Excel.Sheets.Quest>()!;
                var row        = sheet.GetRow(questId)!;
                var loc        = row.IssuerLocation.Value!;
                var map        = loc.Map.Value!;
                var pos        = MapUtil.WorldToMap(new Vector2(loc.X, loc.Z), map);
                var mapPayload = new MapLinkPayload(loc.Territory.RowId, loc.Map.RowId, pos.X, pos.Y);
                var text       = new SeStringBuilder();
                text.AddText("Collectables are unlocked by ")
                    .AddUiForeground(0x0225)
                    .AddUiGlow(0x0226)
                    .AddQuestLink(questId)
                    .AddUiForeground(500)
                    .AddUiGlow(501)
                    .AddText($"{(char)SeIconChar.LinkMarker}")
                    .AddUiGlowOff()
                    .AddUiForegroundOff()
                    .AddText(row.Name.ToString())
                    .Add(RawPayload.LinkTerminator)
                    .AddUiGlowOff()
                    .AddUiForegroundOff()
                    .AddText(" quest, which can be started in ")
                    .AddUiForeground(0x0225)
                    .AddUiGlow(0x0226)
                    .Add(mapPayload)
                    .AddUiForeground(500)
                    .AddUiGlow(501)
                    .AddText($"{(char)SeIconChar.LinkMarker}")
                    .AddUiGlowOff()
                    .AddUiForegroundOff()
                    .AddText($"{mapPayload.PlaceName} {mapPayload.CoordinateString}")
                    .Add(RawPayload.LinkTerminator)
                    .AddUiGlowOff()
                    .AddUiForegroundOff()
                    .AddText(".");
                Communicator.Print(text.BuiltString);
                return false;
            }

            return true;
        }

        private bool ChangeGearSet(GatheringType job, int delay)
        {
            var set = job switch
            {
                GatheringType.Miner    => GatherBuddy.Config.MinerSetName,
                GatheringType.Botanist => GatherBuddy.Config.BotanistSetName,
                GatheringType.Fisher   => GatherBuddy.Config.FisherSetName,
                _                      => null,
            };
            if (string.IsNullOrEmpty(set))
            {
                Communicator.PrintError($"No gear set for {job} configured.");
                return false;
            }

            Chat.ExecuteCommand($"/gearset change \"{set}\"");
            TaskManager.DelayNext(Random.Shared.Next(delay, delay + 500)); //Add a random delay to be less suspicious
            return true;
        }

        internal void DebugClearVisited()
        {
            _activeItemList.DebugClearVisited();
        }

        internal void DebugMarkVisited(GatherTarget target)
        {
            _activeItemList.DebugMarkVisited(target);
        }
        
        private bool HasUmbralItemsInActiveList()
        {
            return _activeItemList.GetType()
                .GetField("_listsManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_activeItemList) is AutoGatherListsManager listsManager
                && listsManager.ActiveItems.Any(item => UmbralNodes.UmbralNodeData.Any(entry => entry.ItemIds.Contains(item.Item.ItemId)));
        }
        
        private (Gatherable Item, uint Quantity) GetFirstUmbralItemFromActiveList()
        {
            if (_activeItemList.GetType()
                .GetField("_listsManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_activeItemList) is not AutoGatherListsManager listsManager)
                return (null, 0);
                
            return listsManager.ActiveItems
                .Where(item => item.Item.GetInventoryCount() < item.Quantity)
                .FirstOrDefault(item => UmbralNodes.UmbralNodeData.Any(entry => entry.ItemIds.Contains(item.Item.ItemId)));
        }
        
        private IEnumerable<(Gatherable Item, uint Quantity)> GetActiveItemsNeedingGathering()
        {
            if (_activeItemList.GetType()
                .GetField("_listsManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_activeItemList) is not AutoGatherListsManager listsManager)
                return Enumerable.Empty<(Gatherable, uint)>();
                
            return listsManager.ActiveItems
                .Where(item => item.Item.GetInventoryCount() < item.Quantity); // Only items that need gathering
        }
        
        private bool IsUmbralItem(IGatherable item)
        {
            return UmbralNodes.UmbralNodeData.Any(entry => entry.ItemIds.Contains(item.ItemId));
        }

        private bool ShouldWaitForAutoRetainer()
        {
            try
            {
                if (GatherBuddy.Config.AutoGatherConfig.AutoRetainerDelayForTimedNodes)
                {
                    if (_currentGatherTarget != null)
                    {
                        var target = _currentGatherTarget.Value;
                        if (target.Node?.NodeType is NodeType.Legendary or NodeType.Unspoiled)
                        {
                            return false;
                        }
                    }
                    
                    var nextItems = _activeItemList.GetNextOrDefault(new List<uint>());
                    if (nextItems.Any())
                    {
                        var nextItem = nextItems.First();
                        if (nextItem.Node?.NodeType is NodeType.Legendary or NodeType.Unspoiled)
                        {
                            if (nextItem.Time.InRange(AdjustedServerTime) && 
                                !_activeItemList.DebugVisitedTimedLocations.ContainsKey(nextItem.Node))
                            {
                                return false;
                            }
                        }
                    }
                }
                
                if (AutoRetainer.GetEnabledRetainers == null || AutoRetainer.GetOfflineCharacterData == null)
                    return false;

                var enabledRetainers = AutoRetainer.GetEnabledRetainers();
                
                if (enabledRetainers == null || !enabledRetainers.Any())
                {
                    if (_autoRetainerMultiModeEnabled)
                    {
                        AutoRetainer.AbortAllTasks?.Invoke();
                        AutoRetainer.DisableAllFunctions?.Invoke();
                        _autoRetainerMultiModeEnabled = false;
                    }
                    return false;
                }

                var threshold = GatherBuddy.Config.AutoGatherConfig.AutoRetainerMultiModeThreshold;
                var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                bool hasRetainersReady = false;
                long? closestTime = null;

                foreach (var (cid, retainerNames) in enabledRetainers)
                {
                    if (!retainerNames.Any())
                        continue;

                    var charData = AutoRetainer.GetOfflineCharacterData(cid);
                    if (charData == null || charData.RetainerData == null)
                        continue;

                    if (!charData.Enabled)
                        continue;

                    foreach (var retainer in charData.RetainerData)
                    {
                        if (!retainerNames.Contains(retainer.Name))
                            continue;

                        if (!retainer.HasVenture)
                            continue;

                        var secondsRemaining = (long)retainer.VentureEndsAt - currentTime;
                        
                        if (secondsRemaining <= 0 || secondsRemaining <= threshold)
                        {
                            hasRetainersReady = true;
                            var effectiveTime = secondsRemaining <= 0 ? 0 : secondsRemaining;
                            if (!closestTime.HasValue || effectiveTime < closestTime.Value)
                                closestTime = effectiveTime;
                        }
                    }
                }

                if (hasRetainersReady && closestTime.HasValue)
                {
                    if (!_autoRetainerMultiModeEnabled)
                    {
                        var player = Svc.ClientState.LocalPlayer;
                        if (player != null)
                            _originalCharacterNameWorld = $"{player.Name}@{player.HomeWorld.Value.Name}";
                        
                        AutoRetainer.EnableMultiMode?.Invoke();
                        _autoRetainerMultiModeEnabled = true;
                    }
                    AutoStatus = $"Waiting for retainers ({closestTime.Value}s remaining)...";
                    return true;
                }
                else
                {
                    if (_autoRetainerMultiModeEnabled)
                    {
                        AutoRetainer.AbortAllTasks?.Invoke();
                        AutoRetainer.DisableAllFunctions?.Invoke();
                        _autoRetainerMultiModeEnabled = false;
                    }
                    
                    if (!string.IsNullOrEmpty(_originalCharacterNameWorld))
                    {
                        var currentPlayer = Svc.ClientState.LocalPlayer;
                        if (currentPlayer != null)
                        {
                            var currentCharacter = $"{currentPlayer.Name}@{currentPlayer.HomeWorld.Value.Name}";
                            if (currentCharacter != _originalCharacterNameWorld)
                            {
                                if (Lifestream.IsBusy != null && Lifestream.IsBusy())
                                {
                                    AutoStatus = $"Waiting for character change to complete...";
                                    return true;
                                }
                                
                                if (Lifestream.Enabled && Lifestream.ChangeCharacter != null)
                                {
                                    var parts = _originalCharacterNameWorld.Split('@');
                                    if (parts.Length == 2)
                                    {
                                        var charName = parts[0];
                                        var worldName = parts[1];
                                        
                                        AutoStatus = $"Relogging to {charName}@{worldName}...";
                                        
                                        var errorCode = Lifestream.ChangeCharacter(charName, worldName);
                                        if (errorCode == 0)
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            GatherBuddy.Log.Warning($"Failed to relog to {_originalCharacterNameWorld}. Error code: {errorCode}");
                                            _originalCharacterNameWorld = null;
                                        }
                                    }
                                    else
                                    {
                                        GatherBuddy.Log.Warning($"Invalid character format: {_originalCharacterNameWorld}");
                                        _originalCharacterNameWorld = null;
                                    }
                                }
                                else
                                {
                                    GatherBuddy.Log.Warning("Cannot relog - Lifestream not available");
                                    _originalCharacterNameWorld = null;
                                }
                            }
                            else
                            {
                                if (!Player.Available || !Player.Interactable)
                                {
                                    AutoStatus = "Waiting for player to be ready...";
                                    return true;
                                }
                                
                                _originalCharacterNameWorld = null;
                            }
                        }
                        
                        return true;
                    }
                    
                    return false;
                }
            }
            catch (Exception e)
            {
                GatherBuddy.Log.Error($"Failed to check AutoRetainer venture times: {e.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _advancedUnstuck.Dispose();
            _activeItemList.Dispose();
            Svc.Chat.CheckMessageHandled -= OnMessageHandled;
            //Svc.AddonLifecycle.UnregisterListener(AddonEvent.PreFinalize, "Gathering", OnGatheringFinalize);
        }
    }
}
