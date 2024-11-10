using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GatherBuddy.Classes;
using GatherBuddy.Enums;

namespace GatherBuddy.AutoGather
{
    public class AutoGatherConfig
    {
        public float                           MountUpDistance               { get; set; } = 15.0f;
        public uint                            AutoGatherMountId             { get; set; } = 1;
        public Dictionary<uint, List<Vector3>> BlacklistedNodesByTerritoryId { get; set; } = new();

        [Obsolete] public ActionConfig BYIIConfig    { get; set; } = new(true, 100, uint.MaxValue, new ActionConditions(), new Dictionary<string, object> { { "UseWithCystals", false }, { "MinimumIncrease", 1 } });
        [Obsolete] public ActionConfig LuckConfig    { get; set; } = new(true, 200, uint.MaxValue, new ActionConditions());
        [Obsolete] public ActionConfig YieldIIConfig { get; set; } = new(true, 500, uint.MaxValue, new ActionConditions(), new Dictionary<string, object> { { "UseWithCystals", false } });
        [Obsolete] public ActionConfig YieldIConfig  { get; set; } = new(true, 400, uint.MaxValue, new ActionConditions(), new Dictionary<string, object> { { "UseWithCystals", false } });
        [Obsolete] public ActionConfig GivingLandConfig { get; set; } = new(true, 200, uint.MaxValue, new ActionConditions());
        [Obsolete] public ActionConfig TwelvesBountyConfig { get; set; } = new(true, 150, uint.MaxValue, new ActionConditions());
        public bool UseGivingLandOnCooldown { get; set; } = false;

        [Obsolete] public ActionConfig ScrutinyConfig { get; set; } =
            new(true, (uint)AutoGather.Actions.Scrutiny.GpCost, uint.MaxValue, new ActionConditions());

        [Obsolete] public ActionConfig MeticulousConfig { get; set; } =
            new(true, (uint)AutoGather.Actions.Meticulous.GpCost, uint.MaxValue, new ActionConditions());

        [Obsolete] public ActionConfig BrazenConfig { get; set; } =
            new(true, (uint)AutoGather.Actions.Brazen.GpCost, uint.MaxValue, new ActionConditions());

        [Obsolete] public ActionConfig SolidAgeCollectablesConfig { get; set; } =
            new(true, (uint)AutoGather.Actions.SolidAge.GpCost, uint.MaxValue, new ActionConditions());

        [Obsolete] public ActionConfig SolidAgeGatherablesConfig { get; set; } = new(true, (uint)AutoGather.Actions.SolidAge.GpCost, uint.MaxValue,
            new ActionConditions(), new Dictionary<string, object> { { "MinimumYield", (uint)1 }, { "UseWithCystals", false } });

        [Obsolete] public ActionConfig ScourConfig { get; set; } = new(true, (uint)AutoGather.Actions.Scour.GpCost, uint.MaxValue, new ActionConditions());
        public int TimedNodePrecog { get; set; } = 20;
        public bool DoGathering { get; set; } = true;
        [Obsolete] public uint MinimumGPForGathering { get; set; } = 0;
        [Obsolete] public uint MinimumGPForCollectableRotation { get; set; } = 700;
        [Obsolete] public bool AlwaysUseSolidAgeCollectables { get; set; } = false;
        [Obsolete] public uint MinimumGPForCollectable { get; set; } = 0;
        public float NavResetCooldown { get; set; } = 3.0f;
        public float NavResetThreshold { get; set; } = 2.0f;
        public bool ForceWalking { get; set; } = false;
        public float FarNodeFilterDistance { get; set; } = 50.0f;
        public bool DisableFlagPathing { get; set; } = false;
        [Obsolete] public uint MinimumCollectibilityScore { get; set; } = 1000;
        [Obsolete] public bool GatherIfLastIntegrity { get; set; } = false;
        [Obsolete] public uint GatherIfLastIntegrityMinimumCollectibility { get; set; } = 600;
        public bool UseExperimentalUnstuck { get; set; } = true;
        [Obsolete] public ConsumableConfig CordialConfig { get; set; } = new(false, 0, 700, 0);
        [Obsolete] public ConsumableConfig FoodConfig { get; set; } = new(false, 0, 0, 0);
        [Obsolete] public ConsumableConfig PotionConfig { get; set; } = new(false, 0, 0, 0);
        [Obsolete] public ConsumableConfig ManualConfig { get; set; } = new(false, 0, 0, 0);
        [Obsolete] public ConsumableConfig SquadronManualConfig { get; set; } = new(false, 0, 0, 0);
        [Obsolete] public ConsumableConfig SquadronPassConfig { get; set; } = new(false, 0, 0, 0);
        public bool DoMaterialize { get; set; } = false;
        public bool HonkMode { get; set; } = true;
        public SortingType SortingMethod { get; set; } = SortingType.Location;
        public bool GoHomeWhenIdle { get; set; } = true;
        public bool GoHomeWhenDone { get; set; } = true;
        public bool UseSkillsForFallbackItems { get; set; } = false;
        public bool AbandonNodes { get; set; } = false;
        public uint ExecutionDelay { get; set; } = 0;
        public bool ForceCloseLingeringMasterpieceAddon { get; set; } = false;

        public enum SortingType
        {
            None = 0,
            Location = 1,
        }
        [Obsolete]
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

            public bool TryGetOptionalProperty<T>(string key, [MaybeNullWhen(false)] out T value)
            {
                if (OptionalProperties.TryGetValue(key, out var _value))
                {
                    value = (T)Convert.ChangeType(_value, typeof(T));
                    return true;
                }
                value = default;
                return false;
            }
            public T GetOptionalProperty<T>(string key)
            {
                if (TryGetOptionalProperty<T>(key, out var value))
                {
                    return value;
                }

                throw new KeyNotFoundException($"Optional property with key '{key}' not found.");
            }
        }

        [Obsolete]
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

                public NodeConfig GetNodeConfig(Enums.NodeType nodeType)
                {
                    return nodeType switch
                    {
                        Enums.NodeType.Regular   => RegularNode,
                        Enums.NodeType.Unspoiled => UnspoiledNode,
                        Enums.NodeType.Ephemeral => EphemeralNode,
                        Enums.NodeType.Legendary => LegendaryNode,
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

        [Obsolete]
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
#pragma warning disable CS0612
        public ConfigPreset ConvertToPreset()
        {
            return new()
            {
                Enabled = true,
                Name = "Default",
                GatherableMinGP = (int)MinimumGPForGathering,
                UseGivingLandOnCooldown = UseGivingLandOnCooldown,
                CollectableMinGP = (int)MinimumGPForCollectable,
                CollectableActionsMinGP = (int)MinimumGPForCollectableRotation,
                CollectableTagetScore = (int)MinimumCollectibilityScore,
                CollectableMinScore = (int)(GatherIfLastIntegrity ? GatherIfLastIntegrityMinimumCollectibility : 1000),
                CollectableAlwaysUseSolidAge = AlwaysUseSolidAgeCollectables,

                GatherableActions = new()
                {
                    Bountiful = new()
                    {
                        Enabled = BYIIConfig.UseAction,
                        MinGP = (int)BYIIConfig.MinimumGP,
                        MaxGP = (int)BYIIConfig.MaximumGP,
                        MinYieldBonus = BYIIConfig.GetOptionalProperty<int>("MinimumIncrease")
                    },
                    Yield1 = new()
                    {
                        Enabled = YieldIConfig.UseAction,
                        MinGP = (int)YieldIConfig.MinimumGP,
                        MaxGP = (int)YieldIConfig.MaximumGP,
                        FirstStepOnly = YieldIConfig.Conditions.UseOnlyOnFirstStep,
                        MinIntegrity = (int)YieldIConfig.Conditions.RequiredIntegrity
                    },
                    Yield2 = new()
                    {
                        Enabled = YieldIIConfig.UseAction,
                        MinGP = (int)YieldIIConfig.MinimumGP,
                        MaxGP = (int)YieldIIConfig.MaximumGP,
                        FirstStepOnly = YieldIIConfig.Conditions.UseOnlyOnFirstStep,
                        MinIntegrity = (int)YieldIIConfig.Conditions.RequiredIntegrity
                    },
                    SolidAge = new()
                    {
                        Enabled = SolidAgeGatherablesConfig.UseAction,
                        MinGP = (int)SolidAgeGatherablesConfig.MinimumGP,
                        MaxGP = (int)SolidAgeGatherablesConfig.MaximumGP,
                        MinYieldTotal = (int)SolidAgeGatherablesConfig.GetOptionalProperty<uint>("MinimumYield")
                    },
                },
                CollectableActions = new()
                {
                    Scour = new()
                    {
                        Enabled = ScourConfig.UseAction,
                        MinGP = (int)ScourConfig.MinimumGP,
                        MaxGP = (int)ScourConfig.MaximumGP
                    },
                    Brazen = new()
                    {
                        Enabled = BrazenConfig.UseAction,
                        MinGP = (int)BrazenConfig.MinimumGP,
                        MaxGP = (int)BrazenConfig.MaximumGP
                    },
                    Meticulous = new()
                    {
                        Enabled = MeticulousConfig.UseAction,
                        MinGP = (int)MeticulousConfig.MinimumGP,
                        MaxGP = (int)MeticulousConfig.MaximumGP
                    },
                    Scrutiny = new()
                    {
                        Enabled = ScrutinyConfig.UseAction,
                        MinGP = (int)ScrutinyConfig.MinimumGP,
                        MaxGP = (int)ScrutinyConfig.MaximumGP
                    },
                    SolidAge = new()
                    {
                        Enabled = SolidAgeCollectablesConfig.UseAction,
                        MinGP = (int)SolidAgeCollectablesConfig.MinimumGP,
                        MaxGP = (int)SolidAgeCollectablesConfig.MaximumGP
                    }
                },
                Consumables = new()
                {
                    Cordial = new()
                    {
                        Enabled = CordialConfig.UseConsumable,
                        MinGP = (int)CordialConfig.MinimumGP,
                        MaxGP = (int)CordialConfig.MaximumGP,
                        ItemId = CordialConfig.ItemId,
                    },
                    Food = new()
                    {
                        Enabled = FoodConfig.UseConsumable,
                        MinGP = (int)FoodConfig.MinimumGP,
                        MaxGP = (int)FoodConfig.MaximumGP,
                        ItemId = FoodConfig.ItemId,
                    },
                    Potion = new()
                    {
                        Enabled = PotionConfig.UseConsumable,
                        MinGP = (int)PotionConfig.MinimumGP,
                        MaxGP = (int)PotionConfig.MaximumGP,
                        ItemId = PotionConfig.ItemId,
                    },
                    Manual = new()
                    {
                        Enabled = ManualConfig.UseConsumable,
                        MinGP = (int)ManualConfig.MinimumGP,
                        MaxGP = (int)ManualConfig.MaximumGP,
                        ItemId = ManualConfig.ItemId,
                    },
                    SquadronManual = new()
                    {
                        Enabled = SquadronManualConfig.UseConsumable,
                        MinGP = (int)SquadronManualConfig.MinimumGP,
                        MaxGP = (int)SquadronManualConfig.MaximumGP,
                        ItemId = SquadronManualConfig.ItemId,
                    },
                    SquadronPass = new()
                    {
                        Enabled = SquadronPassConfig.UseConsumable,
                        MinGP = (int)SquadronPassConfig.MinimumGP,
                        MaxGP = (int)SquadronPassConfig.MaximumGP,
                        ItemId = SquadronPassConfig.ItemId,
                    },
                }
            };
        }
#pragma warning restore CS0612
    }
}
