using ECommons.ExcelServices;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using System;

namespace GatherBuddy.AutoGather
{
    public static class DiscipleOfLand
    {
        private static readonly sbyte _minerExpArrayIndex = Dalamud.GameData.GetExcelSheet<Lumina.Excel.Sheets.ClassJob>().GetRow((uint)Job.MIN).ExpArrayIndex;
        private static readonly sbyte _botanistExpArrayIndex = Dalamud.GameData.GetExcelSheet<Lumina.Excel.Sheets.ClassJob>().GetRow((uint)Job.BTN).ExpArrayIndex;

        public static unsafe int MinerLevel => PlayerState.Instance()->ClassJobLevels[_minerExpArrayIndex];
        public static unsafe int BotanistLevel => PlayerState.Instance()->ClassJobLevels[_botanistExpArrayIndex];
        public static unsafe int Gathering => PlayerState.Instance()->Attributes[72];
        public static unsafe int Perception => PlayerState.Instance()->Attributes[73];
        public static unsafe DateTime NextTreasureMapAllowance => UIState.Instance()->GetNextMapAllowanceDateTime();

    }
}
