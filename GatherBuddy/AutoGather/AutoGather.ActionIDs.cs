using ECommons.ExcelServices;
using ECommons.GameHelpers;
using System;
using Action = Lumina.Excel.Sheets.Action;

namespace GatherBuddy.AutoGather;

public partial class AutoGather
{
    public static class Actions
    {
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
            public static implicit operator Pair<T>((T Botanist, T Miner) value) => new(value.Botanist, value.Miner);
        }

        public class BaseAction
        {
            public BaseAction(uint btnActionId, uint minActionId, uint btnEffectId = 0, uint minEffectId = 0, EffectType type = EffectType.Other)
            {
                var actionsSheet = Dalamud.GameData.GetExcelSheet<Action>();
                var botanistRow = actionsSheet.GetRow(btnActionId);
                var minerRow = actionsSheet.GetRow(minActionId);
                actions = (botanistRow, minerRow);
                effects = (btnEffectId, minEffectId == 0 ? btnEffectId : minEffectId);
                EffectType = type;
                names = (firstToUpper(botanistRow.Name.ExtractText()), firstToUpper(minerRow.Name.ExtractText()));

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
            private readonly Pair<uint> effects;
            private readonly Pair<string> names;

            public Pair<Action> Actions => actions;
            public Pair<string> Names => names;
            public uint ActionId => GetJobValue(actions).RowId;
            public uint QuestId  => GetJobValue(actions).UnlockLink.RowId;
            public uint EffectId => GetJobValue(effects);
            public int MinLevel => actions.Botanist.ClassJobLevel;
            public int GpCost => actions.Botanist.PrimaryCostValue;
            public EffectType EffectType { get; }

            private static T GetJobValue<T>(Pair<T> pair)
            {
                return Player.Job switch
                {
                    Job.BTN => pair.Botanist,
                    Job.MIN => pair.Miner,
                    _ => throw new InvalidOperationException("Invalid job selected"),
                };
            }
        }

        public static readonly BaseAction Prospect      = new(  210,   227,   217, 225);
        public static readonly BaseAction Sneak         = new(  304,   303,    47);
        public static readonly BaseAction TwelvesBounty = new(  282,   280,   825, type: EffectType.CrystalsYield);
        public static readonly BaseAction Bountiful     = new( 4087,  4073,   756, type: EffectType.Yield);
        public static readonly BaseAction SolidAge      = new(  215,   232,  2765, type: EffectType.Integrity);
        public static readonly BaseAction Yield1        = new(  222,   239,   219, type: EffectType.Yield);
        public static readonly BaseAction Yield2        = new(  224,   241,   219, type: EffectType.Yield);
        public static readonly BaseAction Truth         = new(  221,   238,   221, 222);
        public static readonly BaseAction Collect       = new(  815,   240);
        public static readonly BaseAction Scour         = new(22186, 22182);
        public static readonly BaseAction Brazen        = new(22187, 22183);
        public static readonly BaseAction Meticulous    = new(22188, 22184);
        public static readonly BaseAction Scrutiny      = new(22189, 22185,   757);
        public static readonly BaseAction Luck          = new( 4095,  4081);
        public static readonly BaseAction BountifulII   = new(  273,   272,  1286, type: EffectType.Yield);
        public static readonly BaseAction GivingLand    = new( 4590,  4589,  1802, type: EffectType.CrystalsYield);
        public static readonly BaseAction Wise          = new(26522, 26521,        type: EffectType.Integrity);
        public static readonly BaseAction Gift1         = new(21178, 21177,  2666, type: EffectType.BoonChance);
        public static readonly BaseAction Gift2         = new(25590, 25589,   759, type: EffectType.BoonChance);
        public static readonly BaseAction Tidings       = new(21204, 21203,  2667, type: EffectType.BoonYield);
    }
}
