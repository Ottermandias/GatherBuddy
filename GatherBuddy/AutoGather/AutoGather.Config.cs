using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GatherBuddy.AutoGather
{
    public class AutoGatherConfig
    {
        public float                           MountUpDistance               { get; set; } = 15.0f;
        public uint                            AutoGatherMountId             { get; set; } = 1;
        public Dictionary<uint, List<Vector3>> BlacklistedNodesByTerritoryId { get; set; } = new();

        public ActionConfig BYIIConfig            { get; set; } = new(true, 100, uint.MaxValue);
        public ActionConfig LuckConfig            { get; set; } = new(true, 200, uint.MaxValue);
        public ActionConfig ScrutinyConfig        { get; set; } = new(true, (uint)AutoGather.Actions.Scrutiny.GpCost, uint.MaxValue);
        public ActionConfig MeticulousConfig      { get; set; } = new(true, (uint)AutoGather.Actions.Meticulous.GpCost, uint.MaxValue);
        public ActionConfig SolidAgeConfig        { get; set; } = new(true, (uint)AutoGather.Actions.SolidAge.GpCost, uint.MaxValue);
        public ActionConfig WiseConfig            { get; set; } = new(true, (uint)AutoGather.Actions.Wise.GpCost, uint.MaxValue);
        public ActionConfig CollectConfig         { get; set; } = new(true, (uint)AutoGather.Actions.Collect.GpCost, uint.MaxValue);
        public ActionConfig ScourConfig           { get; set; } = new(true, (uint)AutoGather.Actions.Scour.GpCost, uint.MaxValue);
        public int          TimedNodePrecog       { get; set; } = 120;
        public bool         DoGathering           { get; set; } = true;
        public uint         MinimumGPForGathering { get; set; } = 0;
        public float        NavResetCooldown      { get; set; } = 3.0f;
        public float        NavResetThreshold     { get; set; } = 2.0f;
        public bool         ForceWalking          { get; set; } = false;
        public float        FarNodeFilterDistance { get; set; } = 50.0f;
        public bool         DisableFlagPathing    { get; set; } = false;

        public class ActionConfig
        {
            public ActionConfig(bool useAction, uint minGP, uint maximumGP)
            {
                UseAction = useAction;
                MinimumGP = minGP;
                MaximumGP = maximumGP;
            }

            public bool UseAction { get; set; }
            public uint MinimumGP { get; set; }
            public uint MaximumGP { get; set; }
        }
    }
}
