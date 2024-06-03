using ECommons.DalamudServices;
using ECommons.ExcelServices;
using ECommons.GameHelpers;

namespace GatherBuddy.AutoGather;

public partial class AutoGather
{
    public static class Actions
    {
        public class BaseAction(uint btnId, uint minId, string name, int minLevel, int gpCost)
        {
            private uint MinerID  { get; } = minId;
            private uint BotanyID { get; } = btnId;

            public uint ActionID
            {
                get
                {
                    switch (Player.Job)
                    {
                        case Job.BTN: return BotanyID;
                        case Job.MIN: return MinerID;
                    }

                    return 0;
                }
            }

            public string Name     { get; } = name;
            public int    MinLevel { get; } = minLevel;
            public int    GpCost   { get; } = gpCost;
        }

        public static BaseAction Sneak         = new BaseAction(304,   303,   "Sneak",          8,  0);
        public static BaseAction TwelvesBounty = new BaseAction(282,   280,   "Twelves Bounty", 20, 150);
        public static BaseAction Bountiful     = new BaseAction(273,   4073,  "Bountiful",      24, 100);
        public static BaseAction SolidAge      = new BaseAction(215,   232,   "SolidAge",       25, 300);
        public static BaseAction Yield1        = new BaseAction(222,   239,   "Yield 1",        30, 400);
        public static BaseAction Yield2        = new BaseAction(224,   241,   "Yield 2",        40, 500);
        public static BaseAction Collect       = new BaseAction(815,   240,   "Collect",        50, 0);
        public static BaseAction Scour         = new BaseAction(22186, 22182, "Scour",          50, 0);
        public static BaseAction Brazen        = new BaseAction(22187, 22183, "Brazen",         50, 0);
        public static BaseAction Meticulous    = new BaseAction(22188, 22184, "Meticulous",     50, 0);
        public static BaseAction Scrutiny      = new BaseAction(22189, 22185, "Scrutiny",       50, 200);
        public static BaseAction Luck          = new BaseAction(4095,  4081,  "Luck",           55, 200);
        public static BaseAction Wise          = new BaseAction(26522, 26521, "Wise",           90, 0);
    }
}
