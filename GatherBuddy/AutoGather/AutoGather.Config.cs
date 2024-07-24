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

        public ActionConfig BYIIConfig                                 { get; set; } = new(true, 100, uint.MaxValue, new ActionConditions());
        public ActionConfig LuckConfig                                 { get; set; } = new(true, 200, uint.MaxValue, new ActionConditions());
        public ActionConfig YieldIIConfig                              { get; set; } = new(true, 500, uint.MaxValue, new ActionConditions());
        public ActionConfig YieldIConfig                               { get; set; } = new(true, 400, uint.MaxValue, new ActionConditions());
        public ActionConfig ScrutinyConfig                             { get; set; } = new(true, (uint)AutoGather.Actions.Scrutiny.GpCost, uint.MaxValue, new ActionConditions());
        public ActionConfig MeticulousConfig                           { get; set; } = new(true, (uint)AutoGather.Actions.Meticulous.GpCost, uint.MaxValue, new ActionConditions());
        public ActionConfig BrazenConfig                               { get; set; } = new(true, (uint)AutoGather.Actions.Brazen.GpCost, uint.MaxValue, new ActionConditions());
        public ActionConfig SolidAgeConfig                             { get; set; } = new(true, (uint)AutoGather.Actions.SolidAge.GpCost, uint.MaxValue, new ActionConditions());
        public ActionConfig WiseConfig                                 { get; set; } = new(true, (uint)AutoGather.Actions.Wise.GpCost, uint.MaxValue, new ActionConditions());
        public ActionConfig CollectConfig                              { get; set; } = new(true, (uint)AutoGather.Actions.Collect.GpCost, uint.MaxValue, new ActionConditions());
        public ActionConfig ScourConfig                                { get; set; } = new(true, (uint)AutoGather.Actions.Scour.GpCost, uint.MaxValue, new ActionConditions());
        public int          TimedNodePrecog                            { get; set; } = 20;
        public bool         DoGathering                                { get; set; } = true;
        public uint         MinimumGPForGathering                      { get; set; } = 0;
        public uint         MinimumGPForCollectableRotation            { get; set; } = 700;
        public float        NavResetCooldown                           { get; set; } = 3.0f;
        public float        NavResetThreshold                          { get; set; } = 2.0f;
        public bool         ForceWalking                               { get; set; } = false;
        public float        FarNodeFilterDistance                      { get; set; } = 50.0f;
        public bool         DisableFlagPathing                         { get; set; } = false;
        public uint         MinimumCollectibilityScore                 { get; set; } = 1000;
        public bool         GatherIfLastIntegrity                      { get; set; } = false;
        public uint         GatherIfLastIntegrityMinimumCollectibility { get; set; } = 600;
        public bool UseExperimentalNavigation { get; set; } = false;
        public ConsumableConfig CordialConfig { get; set; } = new(false, 0, 700, 0);
        public ConsumableConfig FoodConfig { get; set; } = new(false, 0, 0, 0);
        public ConsumableConfig PotionConfig { get; set; } = new(false, 0, 0, 0);
        public ConsumableConfig ManualConfig { get; set; } = new(false, 0, 0, 0);
        public ConsumableConfig SquadronManualConfig { get; set; } = new(false, 0, 0, 0);
        public ConsumableConfig SquadronPassConfig { get; set; } = new(false, 0, 0, 0);

        public class ActionConfig
        {
            public ActionConfig(bool useAction, uint minGP, uint maximumGP, ActionConditions conditions)
            {
                UseAction  = useAction;
                MinimumGP  = minGP;
                MaximumGP  = maximumGP;
                Conditions = conditions;
            }

            public bool UseAction { get; set; }
            public uint MinimumGP { get; set; }
            public uint MaximumGP { get; set; }
            public ActionConditions Conditions { get; set; }
        }

        public class ActionConditions
        {
            public ActionConditions(bool useCondition, bool onlyFirstStep, bool filterNodeTypes, uint requiredIntegrity)
            {
                UseConditions      = useCondition;
                UseOnlyOnFirstStep = onlyFirstStep;
                FilterNodeTypes    = filterNodeTypes;
                NodeFilter         = new NodeFilters();
                RequiredIntegrity   = requiredIntegrity;
            }

            public ActionConditions()
            {
                UseConditions      = false;
                UseOnlyOnFirstStep = false;
                FilterNodeTypes    = false;
                NodeFilter         = new NodeFilters();
                RequiredIntegrity   = 1;
            }

            public class NodeFilters
            {
                public NodeFilters()
                {
                    UseOnRegularNode   = true;
                    UseOnUnspoiledNode = true;
                    UseOnEphemeralNode = true;
                    UseOnLegendaryNode = true;
                }
                
                public bool UseOnRegularNode   { get; set; }
                public bool UseOnUnspoiledNode { get; set; }
                public bool UseOnEphemeralNode { get; set; }
                public bool UseOnLegendaryNode { get; set; }
            }
            
            public bool        UseConditions      { get; set; }
            public bool        UseOnlyOnFirstStep { get; set; }
            public bool        FilterNodeTypes    { get; set; }
            public NodeFilters NodeFilter         { get; set; }
            public uint        RequiredIntegrity   { get; set; }
        }

        public class ConsumableConfig
        {
            public ConsumableConfig(bool useConsumable, uint minGP, uint maximumGP, uint itemId)
            {
                UseConsumable = useConsumable;
                MinimumGP = minGP;
                MaximumGP = maximumGP;
                ItemId = itemId;
            }

            public bool UseConsumable { get; set; }
            public uint MinimumGP { get; set; }
            public uint MaximumGP { get; set; }
            public uint ItemId { get; set; }
        }
    }
}
