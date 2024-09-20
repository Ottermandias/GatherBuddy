using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GatherBuddy.Enums;

namespace GatherBuddy.AutoGather
{
    public class AutoGatherConfig
    {
        public float                           MountUpDistance               { get; set; } = 15.0f;
        public uint                            AutoGatherMountId             { get; set; } = 1;
        public Dictionary<uint, List<Vector3>> BlacklistedNodesByTerritoryId { get; set; } = new();

        public ActionConfig BYIIConfig    { get; set; } = new(true, 100, uint.MaxValue, new ActionConditions(), new Dictionary<string, object> { { "UseWithCystals", false } });
        public ActionConfig LuckConfig    { get; set; } = new(true, 200, uint.MaxValue, new ActionConditions());
        public ActionConfig YieldIIConfig { get; set; } = new(true, 500, uint.MaxValue, new ActionConditions(), new Dictionary<string, object> { { "UseWithCystals", false } });
        public ActionConfig YieldIConfig  { get; set; } = new(true, 400, uint.MaxValue, new ActionConditions(), new Dictionary<string, object> { { "UseWithCystals", false } });
        public ActionConfig GivingLandConfig { get; set; } = new(true, 200, uint.MaxValue, new ActionConditions());
        public ActionConfig TwelvesBountyConfig { get; set; } = new(true, 150, uint.MaxValue, new ActionConditions());
        public bool UseGivingLandOnCooldown { get; set; } = false;

        public ActionConfig ScrutinyConfig { get; set; } =
            new(true, (uint)AutoGather.Actions.Scrutiny.GpCost, uint.MaxValue, new ActionConditions());

        public ActionConfig MeticulousConfig { get; set; } =
            new(true, (uint)AutoGather.Actions.Meticulous.GpCost, uint.MaxValue, new ActionConditions());

        public ActionConfig BrazenConfig { get; set; } =
            new(true, (uint)AutoGather.Actions.Brazen.GpCost, uint.MaxValue, new ActionConditions());

        public ActionConfig SolidAgeCollectablesConfig { get; set; } =
            new(true, (uint)AutoGather.Actions.SolidAge.GpCost, uint.MaxValue, new ActionConditions());

        public ActionConfig SolidAgeGatherablesConfig { get; set; } = new(true, (uint)AutoGather.Actions.SolidAge.GpCost, uint.MaxValue,
            new ActionConditions(), new Dictionary<string, object> { { "MinimumYield", (uint)1 }, { "UseWithCystals", false } });

        public ActionConfig CollectConfig { get; set; } =
            new(true, (uint)AutoGather.Actions.Collect.GpCost, uint.MaxValue, new ActionConditions());

        public ActionConfig ScourConfig { get; set; } = new(true, (uint)AutoGather.Actions.Scour.GpCost, uint.MaxValue, new ActionConditions());
        public int TimedNodePrecog { get; set; } = 20;
        public bool DoGathering { get; set; } = true;
        public uint MinimumGPForGathering { get; set; } = 0;
        public uint MinimumGPForCollectableRotation { get; set; } = 700;
        public bool AlwaysUseSolidAgeCollectables { get; set; } = false;
        public uint MinimumGPForCollectable { get; set; } = 0;
        public float NavResetCooldown { get; set; } = 3.0f;
        public float NavResetThreshold { get; set; } = 2.0f;
        public bool ForceWalking { get; set; } = false;
        public float FarNodeFilterDistance { get; set; } = 50.0f;
        public bool DisableFlagPathing { get; set; } = false;
        public uint MinimumCollectibilityScore { get; set; } = 1000;
        public bool GatherIfLastIntegrity { get; set; } = false;
        public uint GatherIfLastIntegrityMinimumCollectibility { get; set; } = 600;
        public bool UseExperimentalUnstuck { get; set; } = false;
        public ConsumableConfig CordialConfig { get; set; } = new(false, 0, 700, 0);
        public ConsumableConfig FoodConfig { get; set; } = new(false, 0, 0, 0);
        public ConsumableConfig PotionConfig { get; set; } = new(false, 0, 0, 0);
        public ConsumableConfig ManualConfig { get; set; } = new(false, 0, 0, 0);
        public ConsumableConfig SquadronManualConfig { get; set; } = new(false, 0, 0, 0);
        public ConsumableConfig SquadronPassConfig { get; set; } = new(false, 0, 0, 0);
        public bool DoMaterialize { get; set; } = false;
        public bool HonkMode { get; set; } = true;
        public SortingType SortingMethod { get; set; } = SortingType.Location;
        public bool GoHomeWhenIdle { get; set; } = true;

        public enum SortingType
        {
            None = 0,
            Location = 1,
        }
        public class ActionConfig
        {
            public ActionConfig(bool useAction, uint minGP, uint maximumGP, ActionConditions conditions,
                Dictionary<string, object> optionalProperties = null)
            {
                UseAction          = useAction;
                MinimumGP          = minGP;
                MaximumGP          = maximumGP;
                Conditions         = conditions;
                OptionalProperties = optionalProperties ?? new Dictionary<string, object>();
            }

            public bool                       UseAction          { get; set; }
            public uint                       MinimumGP          { get; set; }
            public uint                       MaximumGP          { get; set; }
            public ActionConditions           Conditions         { get; set; }
            public Dictionary<string, object> OptionalProperties { get; set; }

            public void SetOptionalProperty(string key, object value)
            {
                OptionalProperties[key] = value;
            }

            public void RemoveOptionalProperty(string key)
            {
                if (OptionalProperties.ContainsKey(key))
                {
                    OptionalProperties.Remove(key);
                }
            }

            public T GetOptionalProperty<T>(string key)
            {
                if (OptionalProperties.TryGetValue(key, out var value))
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }

                throw new KeyNotFoundException($"Optional property with key '{key}' not found.");
            }
        }

        public class ActionConditions
        {
            public ActionConditions(bool useCondition, bool onlyFirstStep, bool filterNodeTypes, uint requiredIntegrity)
            {
                UseConditions      = useCondition;
                UseOnlyOnFirstStep = onlyFirstStep;
                FilterNodeTypes    = filterNodeTypes;
                NodeFilter         = new NodeFilters();
                RequiredIntegrity  = requiredIntegrity;
            }

            public ActionConditions()
            {
                UseConditions      = false;
                UseOnlyOnFirstStep = false;
                FilterNodeTypes    = false;
                NodeFilter         = new NodeFilters();
                RequiredIntegrity  = 1;
            }

            public class NodeFilters
            {
                public class NodeConfig
                {
                    public bool Use       { get; set; } = true;
                    public int  NodeLevel { get; set; } = 100;
                    public bool AvoidCap  { get; set; } = false;
                }

                public NodeConfig RegularNode   { get; set; } = new();
                public NodeConfig UnspoiledNode { get; set; } = new();
                public NodeConfig EphemeralNode { get; set; } = new();
                public NodeConfig LegendaryNode { get; set; } = new();

                public NodeConfig GetNodeConfig(NodeType nodeType)
                {
                    return nodeType switch
                    {
                        NodeType.Regular   => RegularNode,
                        NodeType.Unspoiled => UnspoiledNode,
                        NodeType.Ephemeral => EphemeralNode,
                        NodeType.Legendary => LegendaryNode,
                        _                  => RegularNode
                    };
                }
            }

            public bool        UseConditions      { get; set; }
            public bool        UseOnlyOnFirstStep { get; set; }
            public bool        FilterNodeTypes    { get; set; }
            public NodeFilters NodeFilter         { get; set; }
            public uint        RequiredIntegrity  { get; set; }
        }

        public class ConsumableConfig
        {
            public ConsumableConfig(bool useConsumable, uint minGP, uint maximumGP, uint itemId)
            {
                UseConsumable = useConsumable;
                MinimumGP     = minGP;
                MaximumGP     = maximumGP;
                ItemId        = itemId;
            }

            public bool UseConsumable { get; set; }
            public uint MinimumGP     { get; set; }
            public uint MaximumGP     { get; set; }
            public uint ItemId        { get; set; }
        }
    }
}
