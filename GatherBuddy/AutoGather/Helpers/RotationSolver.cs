using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Actions = GatherBuddy.AutoGather.AutoGather.Actions;
using ItemSlot = GatherBuddy.AutoGather.GatheringTracker.ItemSlot;
using ECommons.ExcelServices;
using ECommons;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using GatherBuddy.CustomInfo;

namespace GatherBuddy.AutoGather.Helpers
{
    internal static class RotationSolver
    {
        //On regular node accept solved rotation if yield/gp is within X percent of the possible best value
        private const uint MaxYieldDiffPercent = 10;
        //Count The Giving Land's yield bonus as X. Should be set to an average value.
        private const int GivingLandYield = 20;
        //When using a filler sequence, keep GP below max - X.
        private const int KeepGPBelowMaxMinus = 50;
        //Edgyth's Winning Streak
        private const uint GPRegenQuestId = 68160;

        [Flags]
        private enum EffectType : byte
        {
            None = 0,
            TwelvesBounty = 1 << 0,
            Bountiful = 1 << 1,
            Yield1 = 1 << 2,
            Yield2 = 1 << 3,
            GivingLand = 1 << 4,
            Gift1 = 1 << 5,
            Gift2 = 1 << 6,
            Tidings = 1 << 7
        }

        private sealed record class GlobalState
        {
            public Job PlayerJob { get; init; }
            public int PlayerLevel { get; init; }
            public uint MaxGP { get; init; }
            public uint InitialGP { get; init; }
            public int MaxIntegrity { get; init; }
            public int BountifulYield { get; init; }
            public int BountyYield { get; init; }
            public bool IsCrystal { get; init; }
            public bool OptimizeForCost { get; init; }
            public List<SolverAction> AvailableActions { get; init; } = [];
            public Stack<Actions.BaseAction?> UsedActions { get; init; } = [];
            public uint BestYield { get; set; } = 0; // Milli
            public uint BaselineYield { get; set; } = 0; // Milli
            public uint BaseBoonChance { get; set; } = 0;
            public int BestGP { get; set; } = 0;
            public List<Actions.BaseAction?> BestActions { get; init; } = [];
            public ulong Iterations { get; set; } = 0;
            public uint GPRegenPerHit { get; init; }
            public uint GPRegenPerTick { get; init; }
            public bool SpendGPOnBestNodesOnly { get; set; }

            public GlobalState(GlobalState other)
            {
                AvailableActions = other.AvailableActions;
                BaseBoonChance = other.BaseBoonChance;
                BaselineYield = other.BaselineYield;
                BestGP = other.BestGP;
                BestYield = other.BestYield;
                BestActions = [.. other.BestActions];
                BountifulYield = other.BountifulYield;
                BountyYield = other.BountyYield;
                GPRegenPerHit = other.GPRegenPerHit;
                GPRegenPerTick = other.GPRegenPerTick;
                InitialGP = other.InitialGP;
                IsCrystal = other.IsCrystal;
                Iterations = other.Iterations;
                MaxGP = other.MaxGP;
                MaxIntegrity = other.MaxIntegrity;
                OptimizeForCost = other.OptimizeForCost;
                PlayerJob = other.PlayerJob;
                PlayerLevel = other.PlayerLevel;
                SpendGPOnBestNodesOnly = other.SpendGPOnBestNodesOnly;
                UsedActions = new(other.UsedActions.Reverse());
            }
        }

        //Using compact data types instead of ints gives us 15-20% performance bonus.
        //Using decimal for TotalYield is twice slower, and using float gives weird results,
        //because many decimal numbers can't be represented by float.
        private readonly record struct State()
        {
            public GlobalState Global { get; init; }
            public uint TotalYield { get; init; } // Milli
            public ushort GP { get; init; }
            public byte Integrity { get; init; }
            public byte BoonChance { get; init; }
            public byte Yield { get; init; }
            public byte BoonYield { get; init; }
            public EffectType Effects { get; init; }
            public byte RestoredGP { get; init; }
        }

        private class SolverAction
        {
            private readonly Actions.BaseAction? _action;
            private readonly CheckDelegate _check;
            private readonly ExecuteDelegate _execute;
            private readonly EffectType _effect;
            private readonly bool _singleUse;

            public delegate bool CheckDelegate(State state);
            public delegate State ExecuteDelegate(State state);
            public SolverAction(Actions.BaseAction? Action, CheckDelegate Check, ExecuteDelegate Execute, EffectType Effect, bool SingleUse)
            {
                _action = Action;
                _check = Check;
                _execute = Execute;
                _effect = Effect;
                _singleUse = SingleUse;
            }

            public Actions.BaseAction? Action => _action;
            public EffectType Effect => _effect;

            public bool Filter(ItemSlot slot)
            {
                if (_action == null)
                    return true;
                if (_action.MinLevel > Player.Level)
                    return false;
                if (_action.QuestId != 0 && !QuestManager.IsQuestComplete(_action.QuestId))
                    return false;
                if (_action.EffectType is Actions.EffectType.CrystalsYield && !slot.Item.IsCrystal)
                    return false;
                if (_action.EffectType is Actions.EffectType.BoonChance or Actions.EffectType.BoonYield && slot.BoonChance <= 0)
                    return false;
                if (_action.EffectType is not Actions.EffectType.Other and not Actions.EffectType.GatherChance && slot.Rare)
                    return false;
                if (_action == Actions.GivingLand && !AutoGather.IsGivingLandOffCooldown)
                    return false;
                if (_action == Actions.TwelvesBounty && (Player.Level < 50 && slot.Item.Level == 50 || Player.Level < 41 && slot.Item.Level == 25))
                    return false;
                return true;
            }

            public bool Check(State state)
                => (_action?.GpCost ?? 0) <= state.GP
                && (_effect & state.Effects) == 0
                //Only on the first step or if there were not enough GP on the first step
                && (_singleUse || state.TotalYield == 0 || state.GP - state.RestoredGP < _action?.GpCost)
                && _check(state);
            public State Execute(State state) => _execute(state with { GP = (ushort)(state.GP - (_action?.GpCost ?? 0)), Effects = state.Effects | _effect });
        }

        private static readonly SolverAction[] SolverActions = [
            new SolverAction(Actions.TwelvesBounty, x => true, s => s with { Yield = (byte)(s.Yield + s.Global.BountyYield) }, EffectType.TwelvesBounty, false),
            new SolverAction(Actions.SolidAge, x => x.Integrity < x.Global.MaxIntegrity - 1, SolidAge, EffectType.None, true),
            new SolverAction(Actions.Yield1, x => (x.Effects & EffectType.Yield2) == 0, s => s with { Yield = (byte)(s.Yield + 1) }, EffectType.Yield1, false),
            new SolverAction(Actions.Yield2, x => (x.Effects & EffectType.Yield1) == 0, s => s with { Yield = (byte)(s.Yield + 2) }, EffectType.Yield2, false),
            new SolverAction(Actions.Gift1, x => true, s => s with { BoonChance = (byte)Math.Min(s.BoonChance + 10, 100) }, EffectType.Gift1, false),
            new SolverAction(Actions.Gift2, x => true, s => s with { BoonChance = (byte)Math.Min(s.BoonChance + 30, 100) }, EffectType.Gift2, false),
            new SolverAction(Actions.Tidings, x => true, s => s with { BoonYield = 2 }, EffectType.Tidings, false),
            new SolverAction(Actions.Bountiful, x => true, s => s, EffectType.Bountiful, true),
            new SolverAction(Actions.GivingLand, x => true, s => s with { Yield = (byte)(s.Yield + GivingLandYield) }, EffectType.GivingLand, false),
            new SolverAction(null, x => true, Gather, EffectType.None, true)
        ];

        private static State SolidAge(State state)
        {
            state = state with
            {
                Integrity = (byte)(state.Integrity + 1)
            };
            if (state.Global.PlayerLevel >= 90)
            {
                state = state with
                {
                    TotalYield = state.TotalYield + (uint)(state.Yield * 1000 + state.BoonYield * state.BoonChance * 10) / 2
                };
            }

            return state;
        }

        private static State Gather(State state)
        {
            state = state with
            {
                TotalYield = state.TotalYield + (uint)(state.Yield * 1000 + state.BoonYield * state.BoonChance * 10),
                Integrity = (byte)(state.Integrity - 1),
                GP = (ushort)Math.Min(state.GP + state.Global.GPRegenPerHit, state.Global.MaxGP),
                RestoredGP = (byte)(state.RestoredGP + Math.Min(state.Global.GPRegenPerHit, state.Global.MaxGP - state.GP))
            };
            if ((state.Effects & EffectType.Bountiful) != 0)
            {
                state = state with
                {
                    TotalYield = state.TotalYield + (uint)state.Global.BountifulYield * 1000,
                    Effects = state.Effects ^ EffectType.Bountiful
                };
            }
            return state;
        }

        private static void SolveInternal(State state)
        {
            if (state.Integrity == 0)
            {
                state.Global.Iterations++;
                if (state.Global.OptimizeForCost)
                {
                    var extraYield = checked(state.TotalYield - state.Global.BaselineYield);
                    var usedGP = checked(state.Global.InitialGP + state.RestoredGP - state.GP);
                    var yield = checked(usedGP == 0 ? 0 : (uint)((long)extraYield * 100 / usedGP));

                    if (state.Global.BestYield == 0 || yield > state.Global.BestYield || yield == state.Global.BestYield && usedGP > state.Global.BestGP)
                    {
                        //GatherBuddy.Log.Debug($"Rotation solver: new best. Thread {Environment.CurrentManagedThreadId}. Iteration {state.Global.Iterations}. Yield/100GP {yield / 1000m} vs. {state.Global.BestYield / 1000m}. GP used {usedGP} vs. {state.Global.BestGP}. Sequence: {string.Join(", ", state.Global.UsedActions.Reverse().Select(a => GetActionName(state.Global, a)))}");
                        state.Global.BestYield = yield;
                        state.Global.BestGP = (int)usedGP;
                        state.Global.BestActions.Clear();
                        state.Global.BestActions.AddRange(state.Global.UsedActions);
                    }
                }
                else
                {
                    if (state.TotalYield > state.Global.BestYield || state.TotalYield == state.Global.BestYield && state.GP > state.Global.BestGP)
                    {
                        //GatherBuddy.Log.Debug($"Rotation solver: new best. Iteration {state.Global.Iterations}. Yield {state.TotalYield/1000m} vs. {state.Global.BestYield/1000m}. GP {state.GP} vs. {state.Global.BestGP}. Sequence: {string.Join(", ", state.Global.Actions.Reverse().Select(a => a?.Name ?? "Gather"))}");
                        state.Global.BestYield = state.TotalYield;
                        state.Global.BestGP = state.GP;
                        state.Global.BestActions.Clear();
                        state.Global.BestActions.AddRange(state.Global.UsedActions);
                    }
                }
            }
            else
            {
                foreach (var action in state.Global.AvailableActions)
                {
                    if (action.Check(state))
                    {
                        state.Global.UsedActions.Push(action.Action);
                        SolveInternal(action.Execute(state));
                        state.Global.UsedActions.Pop();
                    }
                }
            }
        }

        public static async Task<IEnumerable<Actions.BaseAction?>> SolveAsync(ItemSlot slot, ConfigPreset config)
        {
            Debug.Assert(Svc.Framework.IsInFrameworkUpdateThread);

            if (slot.Rare) return [null];

            var timer = new Stopwatch();
            timer.Start();

            var gpQuest = QuestManager.IsQuestComplete(GPRegenQuestId);

            var global = new GlobalState()
            {
                PlayerJob = Player.Job,
                PlayerLevel = Player.Level,
                MaxGP = Player.Object.MaxGp,
                InitialGP = Player.Object.CurrentGp,
                MaxIntegrity = slot.Node.MaxIntegrity,
                BountifulYield = AutoGather.CalculateBountifulBonus(slot.Item),
                BountyYield = Player.Level >= 71 ? 3 : 2,
                IsCrystal = slot.Item.IsCrystal,
                OptimizeForCost = slot.Item.NodeType == Enums.NodeType.Regular,
                AvailableActions = SolverActions.Where(a => a.Filter(slot)).ToList(),
                GPRegenPerTick = gpQuest ? (Player.Level switch { >= 83 => 8u, >= 80 => 7u, _ => 6u }) : 5u,
                GPRegenPerHit = (gpQuest && Player.Level >= 80) ? 6u : 5u,
                SpendGPOnBestNodesOnly = config.SpendGPOnBestNodesOnly,
                BaseBoonChance = slot.BoonChance > 0 ? CalculateBoonChance(slot.Item.GatheringData.GatheringItemLevel.RowId) : 0
            };
            var state = new State()
            {
                Global = global,
                TotalYield = 0,
                GP = (ushort)Player.Object.CurrentGp,
                Integrity = (byte)slot.Node.Integrity,
                BoonChance = (byte)Math.Max(slot.BoonChance, 0),
                Yield = (byte)(slot.Yield
                    - (Player.Status.Any(s => s.StatusId == Actions.BountifulII.EffectId || s.StatusId == Actions.Bountiful.EffectId) ? global.BountifulYield : 0)
                    + (slot.RandomYield ? GivingLandYield : 0)
                    - (slot.Bonus ? 1 : 0)), //May vary, but there is no easy way to tell the exact value.
                BoonYield = (byte)(Player.Status.Any(s => s.StatusId == Actions.Tidings.EffectId) ? 2 : 1),
                Effects = Player.Status.Join(SolverActions, s => s.StatusId, a => a.Action?.EffectId ?? 0, (s, a) => a.Effect).Aggregate(EffectType.None, (a, b) => a | b)
                    | (Player.Status.Any(s => s.StatusId == Actions.BountifulII.EffectId) ? EffectType.Bountiful : EffectType.None)
            };

            if (slot.Item.NodeType is Enums.NodeType.Unspoiled or Enums.NodeType.Legendary)
            {
                await Task.Run(() => SolveInternal(state));
            }
            else if (slot.Item.NodeType is Enums.NodeType.Regular)
            {
                await SolveForRegularNodes(state);
            }
            else
            {
                return [.. Enumerable.Repeat<Actions.BaseAction?>(null, state.Integrity)];
            }

            global.BestActions.Reverse();
            timer.Stop();
            GatherBuddy.Log.Debug($"Rotation solver: simulated {global.Iterations} sequences in {timer.ElapsedTicks * 1000m / Stopwatch.Frequency:F3} ms. Yield: {global.BestYield / 1000m}. Sequence: {string.Join(", ", global.BestActions.Select(a => GetActionName(global, a)))}.");

            return global.BestActions;
        }

        //Prevents accessing Player.Job from a worker thread
        private static string GetActionName(GlobalState global, Actions.BaseAction? action)
            => (global.PlayerJob == Job.BTN ? action?.Names.Botanist : action?.Names.Miner) ?? "Gather";

        private static async Task SolveForRegularNodes(State state)
        {
            var isCrystal = state.Global.IsCrystal;
            var givingLand = isCrystal ? state.Global.AvailableActions.FirstOrDefault(x => x.Action == Actions.GivingLand) : null;

            if (givingLand?.Check(state) == true && state.Integrity >= 4)
            {
                //The Giving Land is the only ability with a cooldown, so we give it a special treatment.
                //I did some math, and it appears that there is no point in waiting for +1 or +2 integrity nodes,
                //because it would yield either the same or even less yield/min. So we use it on cooldown.

                state.Global.BestActions.Add(givingLand.Action);
                return;
            }

            var givingCooldown = TheGivingLandCooldown;

            var bountiful = state.Global.AvailableActions.FirstOrDefault(x => x.Action == Actions.Bountiful);
            var bountifulYield = bountiful != null ? (uint)state.Global.BountifulYield * 1000u : 0u;

            var bounty = isCrystal ? state.Global.AvailableActions.FirstOrDefault(x => x.Action == Actions.TwelvesBounty) : null;
            var bountyYield = bounty != null ? (uint)state.Global.BountyYield * 1000u * state.Integrity * 100 / (uint)Actions.TwelvesBounty.GpCost : 0u;

            var bestSimulatedYield = 0u;
            var fillerYield = Math.Max(bountifulYield, bountyYield);

            //When Bountiful's yield is 3 and the item is not a crystal, we can skip all the calculations,
            //because nothing can beat that, except actions for crystals.
            if (bountifulYield < 3000 || bounty != null)
            {
                state.Global.AvailableActions.RemoveAll(x => x.Action == Actions.Bountiful || x.Action == Actions.GivingLand);
                state.Global.BaselineYield = (uint)(state.Yield * 1000 + state.BoonChance * 10 * state.BoonYield) * state.Integrity;

                if (state.Global.SpendGPOnBestNodesOnly)
                {
                    //Assumptions: there are nodes with +2 integrity, +3 yield or +100% boon bonus and we can hit breakpoints;
                    //there are no nodes with more than 1 bonus.
                    //We solve rotation for those 3 hypothetical nodes to find out best yield/gp ratio then accept
                    //any rotation within MaxYieldDiffPercent% of the best value.

                    //With end-game gear+food and full GP while hitting all breakpoints:
                    //+60-100% boon: 2 yield / 100 gp
                    //+3 yield: 2.484 yield / 100 gp
                    //+2 integrity: 2.4 yield / 100 gp

                    //Start with little less than full GP
                    var startingGP = (ushort)state.Global.MaxGP;
                    var startingGPDelta = KeepGPBelowMaxMinus + (bounty?.Action?.GpCost ?? bountiful?.Action?.GpCost ?? 0);
                    if (startingGP > startingGPDelta) startingGP -= (ushort)startingGPDelta;

                    //Solve for +2 integrity bonus
                    var stateIntegrity = new State()
                    {
                        Global = state.Global with
                        {
                            MaxIntegrity = 6,
                            InitialGP = startingGP,
                            BaselineYield = (1 * 1000 + state.Global.BaseBoonChance * 10) * 6
                        },
                        BoonChance = (byte)state.Global.BaseBoonChance,
                        BoonYield = 1,
                        Integrity = 6,
                        Yield = 1,
                        Effects = EffectType.None,
                        GP = startingGP
                    };

                    //Solve for +3 yield bonus
                    var stateYield = new State()
                    {
                        Global = state.Global with
                        {
                            MaxIntegrity = 4,
                            InitialGP = startingGP,
                            BaselineYield = (4 * 1000 + state.Global.BaseBoonChance * 10) * 4
                        },
                        BoonChance = (byte)state.Global.BaseBoonChance,
                        BoonYield = 1,
                        Integrity = 4,
                        Yield = 4,
                        Effects = EffectType.None,
                        GP = startingGP
                    };

                    //Solve for +100% boon chance bonus
                    var stateBoon = new State()
                    {
                        Global = state.Global with
                        {
                            MaxIntegrity = 4,
                            InitialGP = startingGP,
                            BaselineYield = (uint)(1 * 1000 + 100 * 10) * 4
                        },
                        BoonChance = 100,
                        BoonYield = 1,
                        Integrity = 4,
                        Yield = 1,
                        Effects = EffectType.None,
                        GP = startingGP
                    };

                    var tasks = new List<Task>
                    {
                        Task.Run(() => SolveInternal(state)),
                        Task.Run(() => SolveInternal(stateIntegrity)),
                        Task.Run(() => SolveInternal(stateYield))
                    };

                    if (state.BoonChance != 0)
                        tasks.Add(Task.Run(() => SolveInternal(stateBoon)));

                    await Task.WhenAll(tasks);

                    GatherBuddy.Log.Debug($"Rotation solver: yield / 100gp current: {state.Global.BestYield / 1000m}; integrity+2: {stateIntegrity.Global.BestYield / 1000m}; yield+3: {stateYield.Global.BestYield / 1000m}; boon+100: {stateBoon.Global.BestYield / 1000m}; filler {fillerYield / 1000m}.");

                    bestSimulatedYield = Math.Max(Math.Max(stateIntegrity.Global.BestYield, stateYield.Global.BestYield), stateBoon.Global.BestYield);
                    state.Global.Iterations += stateIntegrity.Global.Iterations + stateYield.Global.Iterations + stateBoon.Global.Iterations;
                }
                else
                {
                    await Task.Run(() => SolveInternal(state));
                    GatherBuddy.Log.Debug($"Rotation solver: yield / 100gp current: {state.Global.BestYield / 1000m}; filler {fillerYield / 1000m}.");
                }
            }

            if (state.Global.BestActions.Count == 0 || state.Global.BestYield <= fillerYield || state.Global.BestYield < bestSimulatedYield * (100 - MaxYieldDiffPercent) / 100)
            {
                //Use Bountiful/Bounty as either best or filler rotation
                //GatherBuddy.Log.Debug($"Rotation solver: using filler sequence.");

                var canUseGiving = (isCrystal || GatherBuddy.Config.AutoGatherConfig.UseGivingLandOnCooldown)
                    && state.Global.PlayerLevel >= Actions.GivingLand.MinLevel;
                //Aim to have 200 GP 10 seconds before The Giving Land is off cooldown
                var keepGP = canUseGiving ? Actions.GivingLand.GpCost - Math.Max((int)((givingCooldown - 10) * state.Global.GPRegenPerTick / 3), 0) : 0;
                //If Bountiful bonus is +1, we may consider using Tidings
                if (fillerYield <= 1000 && state.Global.PlayerLevel >= Actions.Tidings.MinLevel) keepGP = Math.Max(keepGP, Actions.Tidings.GpCost);
                var overcapGP = bestSimulatedYield > fillerYield ? (int)state.Global.MaxGP - KeepGPBelowMaxMinus : 0;
                keepGP = Math.Clamp(keepGP, 0, (int)state.Global.MaxGP);
                overcapGP = Math.Clamp(overcapGP, 0, (int)state.Global.MaxGP);

                var gather = state.Global.AvailableActions.First(a => a.Action == null);

                if (checked(bounty != null && bounty.Check(state)
                    && state.GP - bounty.Action!.GpCost + state.Global.GPRegenPerHit * state.Integrity >= keepGP
                    && state.GP + state.Global.GPRegenPerHit * state.Integrity > overcapGP))
                {
                    state.Global.UsedActions.Push(bounty.Action);
                    state = bounty.Execute(state);
                }

                while (state.Integrity > 0)
                {
                    if (checked(bountiful != null && bounty == null && bountiful.Check(state)
                        && state.GP - bountiful.Action!.GpCost + state.Global.GPRegenPerHit >= keepGP
                        && state.GP + state.Global.GPRegenPerHit > overcapGP))
                    {
                        state.Global.UsedActions.Push(bountiful.Action);
                        state = bountiful.Execute(state);
                    }
                    state.Global.UsedActions.Push(gather.Action);
                    state = gather.Execute(state);
                }

                var extraYield = checked(state.TotalYield - state.Global.BaselineYield);
                var usedGP = state.Global.UsedActions.Sum(a => a?.GpCost ?? 0);
                var yield = checked(usedGP == 0 ? 0u : (uint)((long)extraYield * 100 / usedGP));

                state.Global.BestYield = yield;
                state.Global.BestGP = usedGP;
                state.Global.BestActions.Clear();
                state.Global.BestActions.AddRange(state.Global.UsedActions);
            }
        }

        private static unsafe float TheGivingLandCooldown
        {
            get
            {
                if (!Svc.Framework.IsInFrameworkUpdateThread)
                {
                    GatherBuddy.Log.Error("BUG: RotationSolver.TheGivingLandCooldown is accessed from a worker thread.");
                    return 0f;
                }
                var recastGroup = Actions.GivingLand.Actions.Botanist.CooldownGroup - 1;
                var detail = ActionManager.Instance()->GetRecastGroupDetail(recastGroup);
                return detail->Total - detail->Elapsed;
            }
        }

        private static uint CalculateBoonChance(uint glvl)
        {
            if (!Svc.Framework.IsInFrameworkUpdateThread)
            {
                GatherBuddy.Log.Error("BUG: RotationSolver.CalculateBoonChance is accessed from a worker thread.");
                return 0;
            }
            var basePerception = WorldData.IlvConvertTable[(int)glvl].BasePerception;
            if (basePerception == 0) return 0;

            var score = (uint)Math.Min(150, 100 * CharacterPerceptionStat / basePerception);
            if (score >= 100) return (score - 100) * (60 - 35) / (150 - 100) + 35;
            if (score >=  80) return (score -  80) * (35 - 15) / (100 -  80) + 15;
            if (score >=  70) return (score -  70) * (15 - 10) / ( 80 -  70) + 10;
            if (score >=  60) return (score -  60) * (10 -  0) / ( 70 -  60);

            return 0;
        }

        private static unsafe int CharacterPerceptionStat => PlayerState.Instance()->Attributes[73];
    }
}
