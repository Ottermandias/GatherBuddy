using ECommons.ExcelServices;
using ECommons.GameHelpers;
using System;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using Action = Lumina.Excel.Sheets.Action;

namespace GatherBuddy.AutoGather;

public partial class AutoGather
{
    public static class Actions
    {
        public class FishingAction(uint actionId, int minLevel = 1, int gpCost = 0, uint[]? statusProvide = null)
        {
            public readonly uint    ActionId      = actionId;
            public readonly int     MinLevel      = minLevel;
            public readonly int     GpCost        = gpCost;
            public readonly uint[]? StatusProvide = statusProvide;
            public string  Name          => Svc.Data.Excel.GetSheet<Action>().GetRow(actionId).Name.ExtractText();
        }

        public enum EffectType
        {
            Other,
            Yield,
            BoonYield,
            CrystalsYield,
            GatherChance,
            BoonChance,
            Integrity
        }

        public record struct Pair<T>(T Botanist, T Miner)
        {
            public static implicit operator Pair<T>((T Botanist, T Miner) value)
                => new(value.Botanist, value.Miner);
        }

        public class BaseAction
        {
            public BaseAction(uint btnActionId, uint minActionId, uint btnEffectId = 0, uint minEffectId = 0,
                EffectType type = EffectType.Other)
            {
                var actionsSheet = Dalamud.GameData.GetExcelSheet<Action>();
                var botanistRow  = actionsSheet.GetRow(btnActionId);
                var minerRow     = actionsSheet.GetRow(minActionId);
                actions    = (botanistRow, minerRow);
                effects    = (btnEffectId, minEffectId == 0 ? btnEffectId : minEffectId);
                EffectType = type;
                names      = (firstToUpper(botanistRow.Name.ExtractText()), firstToUpper(minerRow.Name.ExtractText()));

                static string firstToUpper(string str)
                {
                    ReadOnlySpan<char> first = [char.ToUpper(str[0])];
                    if (first[0] != str[0])
                    {
                        str = string.Concat(first, str.AsSpan(1));
                    }

                    return str;
                }
            }

            private readonly Pair<Action> actions;
            private readonly Pair<uint>   effects;
            private readonly Pair<string> names;

            public Pair<Action> Actions
                => actions;

            public Pair<string> Names
                => names;

            public Pair<uint> QuestIds
                => (actions.Botanist.UnlockLink.RowId, actions.Miner.UnlockLink.RowId);

            public uint ActionId
                => GetJobValue(actions).RowId;

            public uint QuestId
                => GetJobValue(actions).UnlockLink.RowId;

            public uint EffectId
                => GetJobValue(effects);

            public int MinLevel
                => actions.Botanist.ClassJobLevel;

            public int GpCost
                => actions.Botanist.PrimaryCostValue;

            public EffectType EffectType { get; }

            private static T GetJobValue<T>(Pair<T> pair)
            {
                return Player.Job switch
                {
                    Job.BTN => pair.Botanist,
                    Job.MIN => pair.Miner,
                    _       => throw new InvalidOperationException("Invalid job selected"),
                };
            }
        }

        //MIN/BTN Actions
        public static readonly BaseAction Prospect      = new(210, 227, 217, 225);
        public static readonly BaseAction Sneak         = new(304, 303, 47);
        public static readonly BaseAction TwelvesBounty = new(282, 280, 825, type: EffectType.CrystalsYield);
        public static readonly BaseAction Bountiful     = new(4087, 4073, 756, type: EffectType.Yield);
        public static readonly BaseAction SolidAge      = new(215, 232, 2765, type: EffectType.Integrity);
        public static readonly BaseAction Yield1        = new(222, 239, 219, type: EffectType.Yield);
        public static readonly BaseAction Yield2        = new(224, 241, 219, type: EffectType.Yield);
        public static readonly BaseAction Truth         = new(221, 238, 221, 222);
        public static readonly BaseAction Collect       = new(815, 240);
        public static readonly BaseAction Scour         = new(22186, 22182);
        public static readonly BaseAction Brazen        = new(22187, 22183);
        public static readonly BaseAction Meticulous    = new(22188, 22184);
        public static readonly BaseAction Scrutiny      = new(22189, 22185, 757);
        public static readonly BaseAction Luck          = new(4095, 4081);
        public static readonly BaseAction BountifulII   = new(273, 272, 1286, type: EffectType.Yield);
        public static readonly BaseAction GivingLand    = new(4590, 4589, 1802, type: EffectType.CrystalsYield);
        public static readonly BaseAction Wise          = new(26522, 26521, type: EffectType.Integrity);
        public static readonly BaseAction Gift1         = new(21178, 21177, 2666, type: EffectType.BoonChance);
        public static readonly BaseAction Gift2         = new(25590, 25589, 759, type: EffectType.BoonChance);
        public static readonly BaseAction Tidings       = new(21204, 21203, 2667, type: EffectType.BoonYield);

        //Fishing Actions
        public static readonly FishingAction Cast             = new FishingAction(289);
        public static readonly FishingAction Quit             = new FishingAction(299);
        public static readonly FishingAction Hook             = new FishingAction(296);
        public static readonly FishingAction Chum             = new FishingAction(4104,  5,  100, [763]);
        public static readonly FishingAction Patience         = new FishingAction(4102,  15, 200, [764, 850]);
        public static readonly FishingAction PowerfulHookset  = new FishingAction(4103,  15, 50);
        public static readonly FishingAction PrecisionHookset = new FishingAction(4179,  15, 50);
        public static readonly FishingAction ThaliaksFavor    = new FishingAction(26804, 15);
        public static readonly FishingAction Release          = new FishingAction(300,   22);
        public static readonly FishingAction Mooch            = new FishingAction(297,   25);
        public static readonly FishingAction Snagging         = new FishingAction(4100,  36,  0,   [761]);
        public static readonly FishingAction CollectorsGlove  = new FishingAction(4101,  50,  0,   [805]);
        public static readonly FishingAction FishEyes         = new FishingAction(4105,  57,  550, [762]);
        public static readonly FishingAction PatienceII       = new FishingAction(4106,  60,  560, [764, 765, 850]);
        public static readonly FishingAction MoochII          = new FishingAction(268,   63,  100);
        public static readonly FishingAction VeteranTrade     = new FishingAction(7906,  63,  200);
        public static readonly FishingAction DoubleHook       = new FishingAction(269,   65,  400);
        public static readonly FishingAction SurfaceSlap      = new FishingAction(4595,  71,  200, [1803]);
        public static readonly FishingAction IdenticalCast    = new FishingAction(4596,  79,  350, [1804]);
        public static readonly FishingAction BaitedBreath     = new FishingAction(26871, 75,  300);
        public static readonly FishingAction PrizeCatch       = new FishingAction(26806, 81,  200, [2780]);
        public static readonly FishingAction TripleHook       = new FishingAction(27523, 90,  700);
        public static readonly FishingAction SparefulHand     = new FishingAction(37045, 91,  100);
        public static readonly FishingAction BigGameFishing   = new FishingAction(37046, 95,  0,  [3907]);
        public static readonly FishingAction AmbitiousLure    = new FishingAction(37594, 100, 10, [3972]);
        public static readonly FishingAction ModestLure       = new FishingAction(37595, 100, 10, [3973]);
    }
}
