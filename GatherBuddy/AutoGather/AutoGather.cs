using ECommons.Automation.LegacyTaskManager;
using GatherBuddy.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
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
using ECommons.ExcelServices;
using ECommons.Automation;
using GatherBuddy.Data;
using NodeType = GatherBuddy.Enums.NodeType;
using ECommons.UIHelpers.AddonMasterImplementations;
using GatherBuddy.AutoGather.Helpers;
using GatherBuddy.AutoGather.Lists;
using GatherBuddy.Classes;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather : IDisposable
    {
        public AutoGather(GatherBuddy plugin)
        {
            // Initialize the task manager
            TaskManager           = new();
            TaskManager.ShowDebug = false;
            _plugin               = plugin;
            _soundHelper          = new SoundHelper();
            _advancedUnstuck      = new();
            _activeItemList       = new ActiveItemList(plugin.AutoGatherListsManager);
        }

        private readonly GatherBuddy     _plugin;
        private readonly SoundHelper     _soundHelper;
        private readonly AdvancedUnstuck _advancedUnstuck;
        private readonly ActiveItemList  _activeItemList;

        public TaskManager TaskManager { get; }

        private           bool             _enabled { get; set; } = false;
        internal readonly GatheringTracker NodeTracker = new();

        public bool Waiting { get; private set; }

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

                    _activeItemList.Reset();
                    Waiting                    = false;
                    ActionSequence             = null;
                    CurrentCollectableRotation = null;

                    if (VNavmesh.Enabled && IsPathGenerating)
                        VNavmesh.Nav.PathfindCancelAll();
                    StopNavigation();
                    CurrentFarNodeLocation = null;
                    FarNodesSeenSoFar.Clear();
                    VisitedNodes.Clear();
                }
                else
                {
                    WentHome = true; //Prevents going home right after enabling auto-gather
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
                Lifestream.ExecuteCommand("auto");
                TaskManager.EnqueueImmediate(() => !Lifestream.IsBusy(), 120000, "Wait until Lifestream is done");
                return true;
            }
            else
            {
                GatherBuddy.Log.Warning("Lifestream not found or not ready");
                return false;
            }
        }

        private class NoGatherableItemsInNodeException : Exception
        { }

        private class NoCollectableActionsException : Exception
        { }

        public void HandleArtisanImport()
        {
            if (Reflection.Artisan_Reflection.ArtisanExportInProgress)
            {
                Svc.Log.Verbose("Artisan export in progress, skipping HandleArtisanImport");
                return;
            }
            else if (Reflection.Artisan_Reflection.ArtisanExportFailed)
            {
                Svc.Log.Verbose("Artisan export failed, clearing task");
                Reflection.Artisan_Reflection.ResetArtisanExportTask();
                return;
            }
            else if (Reflection.Artisan_Reflection.ArtisanExportComplete)
            {
                Svc.Log.Verbose("Artisan export complete, finishing import");
                var importedList = Reflection.Artisan_Reflection.ArtisanExportTask!.Result;
                if (importedList == null)
                {
                    Svc.Log.Error("Import list was null, aborting import");
                    Reflection.Artisan_Reflection.ResetArtisanExportTask();
                    return;
                }

                var listToReplaceIdx = Reflection.Artisan_Reflection.ListIndexToReplace;
                if (listToReplaceIdx == null)
                {
                    Svc.Log.Error("List index to replace was null, aborting import");
                    Reflection.Artisan_Reflection.ResetArtisanExportTask();
                    return;
                }

                var listToModify = _plugin.AutoGatherListsManager.Lists[listToReplaceIdx.Value];
                listToModify.Description = importedList.Description;
                listToModify.Name        = importedList.Name;
                for (var i = 0; i < listToModify.Items.Count; i++)
                {
                    listToModify.RemoveAt(i);
                }

                foreach (var item in importedList.Items)
                {
                    listToModify.Add(item);
                    listToModify.SetQuantity(item, importedList.Quantities[item]);
                }

                Reflection.Artisan_Reflection.ResetArtisanExportTask();
                Communicator.Print($"Successfully imported {importedList.Name} from Artisan!");
            }
        }

        public void DoAutoGather()
        {
            if (!IsGathering)
                LuckUsed = new(0); //Reset the flag even if auto-gather was disabled mid-gathering

            if (!Enabled)
            {
                return;
            }

            try
            {
                if (!NavReady)
                {
                    AutoStatus = "Waiting for Navmesh...";
                    return;
                }
            }
            catch (Exception e)
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

            if (DiscipleOfLand.NextTreasureMapAllowance == DateTime.MinValue)
            {
                //Wait for timer refresh
                AutoStatus = "Refreshing timers...";
                DiscipleOfLand.RefreshNextTreasureMapAllowance();
                return;
            }

            if (!CanAct)
            {
                AutoStatus = Dalamud.Conditions[ConditionFlag.Gathering] ? "Gathering..." : "Player is busy...";
                return;
            }

            YesAlready.Unlock(); // Clean up lock that may have been left behind by cancelled tasks

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

            if (IsGathering)
            {
                GatherTarget gatherTarget;
                if (!_activeItemList.IsInitialized)
                    // If Auto-Gather is enabled after opening the node, the active item list is not initialized.
                    gatherTarget = _activeItemList.GetNextOrDefault(new List<uint>()).FirstOrDefault();
                else
                    // Otherwise, we don't want the list to suddenly change while gathering.
                    gatherTarget = _activeItemList.CurrentOrDefault;

                if (gatherTarget != default)
                {
                    var target = Svc.Targets.Target;
                    if (target != null && target.ObjectKind is ObjectKind.GatheringPoint)
                    {
                        _activeItemList.MarkVisited(target);

                        if (gatherTarget.Item.NodeType is NodeType.Regular or NodeType.Ephemeral
                         && VisitedNodes.Last?.Value != target.DataId
                         && gatherTarget.Node?.WorldPositions.ContainsKey(target.DataId) == true)
                        {
                            FarNodesSeenSoFar.Clear();
                            VisitedNodes.AddLast(target.DataId);
                            while (VisitedNodes.Count > (gatherTarget.Node.WorldPositions.Count <= 4 ? 2 : 4))
                                VisitedNodes.RemoveFirst();
                        }
                    }
                }

                if (!GatherBuddy.Config.AutoGatherConfig.DoGathering)
                    return;

                AutoStatus = "Gathering...";
                StopNavigation();
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

            if (Player.Job is Job.BTN or Job.MIN
             && !isPathing
             && !Svc.Condition[ConditionFlag.Mounted])
            {
                if (SpiritbondMax > 0)
                {
                    DoMateriaExtraction();
                    return;
                }

                if (FreeInventorySlots < 20 && HasReducibleItems())
                {
                    ReduceItems(false);
                    return;
                }
            }

            var nearbyNodes = Svc.Objects.Where(o => o.ObjectKind == ObjectKind.GatheringPoint).Select(o => o.DataId);
            var next        = _activeItemList.GetNextOrDefault(nearbyNodes)
                .OrderByDescending(nodes => nodes.Item.ItemId);
            if (!next.Any())
            {
                if (!_activeItemList.HasItemsToGather)
                {
                    AbortAutoGather();
                    return;
                }

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

            Waiting = false;

            if (next.Any(n => n.Item.ItemData.IsCollectable && !CheckCollectablesUnlocked(n.Item.GatheringType)))
            {
                AbortAutoGather();
                return;
            }

            if (RepairIfNeeded())
                return;

            var territoryId = Svc.ClientState.TerritoryType;
            //Idyllshire to The Dravanian Hinterlands
            if (territoryId == 478 && next.First().Node.Territory.Id == 399 && Lifestream.Enabled)
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
                        var exit = next.First().Node.DefaultXCoord < 2000 ? 91u : 92u;
                        var name = Dalamud.GameData.GetExcelSheet<Lumina.Excel.Sheets.Aetheryte>().GetRow(exit).AethernetName.Value.Name
                            .ToString();
                        Lifestream.AethernetTeleport(name);
                    }

                    return;
                }
            }

            var forcedAetheryte = ForcedAetherytes.ZonesWithoutAetherytes.Where(z => z.ZoneId == next.First().Node.Territory.Id)
                .FirstOrDefault();
            if (forcedAetheryte.ZoneId != 0
             && (GatherBuddy.GameData.Aetherytes[forcedAetheryte.AetheryteId].Territory.Id == territoryId
                 || forcedAetheryte.AetheryteId == 70 && territoryId == 886)) //The Firmament
            {
                if (territoryId == 478 && !Lifestream.Enabled)
                    AutoStatus = $"Install Lifestream or teleport to {next.First().Node.Territory.Name} manually";
                else
                    AutoStatus = "Manual teleporting required";
                return;
            }

            //At this point, we are definitely going to gather something, so we may go home after that.
            if (Lifestream.Enabled)
                Lifestream.Abort();
            WentHome = false;

            if (next.First().Node.Territory.Id != territoryId)
            {
                if (Dalamud.Conditions[ConditionFlag.BoundByDuty])
                {
                    AutoStatus = "Can not teleport when bound by duty";
                    return;
                }

                AutoStatus = "Teleporting...";
                StopNavigation();

                if (!MoveToTerritory(next.First().Node))
                    AbortAutoGather();

                // Reset target to pick up closest item after teleport
                next = default;

                return;
            }

            if (ActivateGatheringBuffs(next.First().Item.NodeType is NodeType.Unspoiled or NodeType.Legendary))
                return;

            var config = MatchConfigPreset(next.First().Item);

            if (DoUseConsumablesWithoutCastTime(config))
                return;

            if (!LocationMatchesJob(next.First().Node))
            {
                if (!ChangeGearSet(next.First().Node.GatheringType.ToGroup()))
                    AbortAutoGather();

                return;
            }

            var allPositions = next.SelectMany(ne => ne.Node.WorldPositions
                .ExceptBy(VisitedNodes, n => n.Key)
                .SelectMany(w => w.Value)
                .Where(v => !IsBlacklisted(v))).Select(s => s)
                .ToHashSet();

            //Svc.Log.Verbose($"All positions: {allPositions.Count()}");

            var visibleNodes = Svc.Objects
                .Where(o => allPositions.Contains(o.Position))
                .ToList();

            var closestTargetableNode = visibleNodes
                .Where(o => o.IsTargetable)
                .MinBy(o => Vector3.Distance(Player.Position, o.Position));

            if (closestTargetableNode != null)
            {
                AutoStatus = "Moving to node...";
                var targetItem = next.First(ti => ti.Node.WorldPositions.ContainsKey(closestTargetableNode.DataId)).Item;
                MoveToCloseNode(closestTargetableNode, targetItem, config);
                return;
            }

            AutoStatus = "Moving to far node...";
            if (CurrentDestination != default && CurrentFarNodeLocation != next.First().Node)
            {
                GatherBuddy.Log.Debug("Current destination doesn't match the target location, resetting navigation");
                StopNavigation();
                FarNodesSeenSoFar.Clear();
                VisitedNodes.Clear();
            }

            CurrentFarNodeLocation = next.First().Node;

            if (CurrentDestination != default)
            {
                var currentNode = visibleNodes.FirstOrDefault(o => o.Position == CurrentDestination);
                if (currentNode != null && !currentNode.IsTargetable)
                    GatherBuddy.Log.Verbose($"Far node is not targetable, distance {currentNode.Position.DistanceToPlayer()}.");

                //It takes some time (roundtrip to the server) before a node becomes targetable after it becomes visible,
                //so we need to delay excluding it. But instead of measuring time, we use distance, since character is traveling at a constant speed.
                //Value 80 was determined empirically.
                foreach (var node in visibleNodes.Where(o => o.Position.DistanceToPlayer() < 80))
                    FarNodesSeenSoFar.Add(node.Position);

                if (CurrentDestination.DistanceToPlayer() < 80)
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

        private void AbortAutoGather(string? status = null)
        {
            if (!string.IsNullOrEmpty(status))
                AutoStatus = status;
            if (GatherBuddy.Config.AutoGatherConfig.HonkMode)
                Task.Run(() => _soundHelper.PlayHonkSound(3));
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

        private bool ChangeGearSet(GatheringType job)
        {
            var set = job switch
            {
                GatheringType.Miner    => GatherBuddy.Config.MinerSetName,
                GatheringType.Botanist => GatherBuddy.Config.BotanistSetName,
                _                      => null,
            };
            if (string.IsNullOrEmpty(set))
            {
                Communicator.PrintError($"No gear set for {job} configured.");
                return false;
            }

            Chat.Instance.ExecuteCommand($"/gearset change \"{set}\"");
            TaskManager.DelayNext(Random.Shared.Next(500, 1500)); //Add a random delay to be less suspicious
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

        public void Dispose()
        {
            _advancedUnstuck.Dispose();
            NodeTracker.Dispose();
            _activeItemList.Dispose();
        }
    }
}
