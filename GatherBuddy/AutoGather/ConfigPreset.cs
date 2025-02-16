using GatherBuddy.Classes;
using System;
using ECommons.DalamudServices;
using Newtonsoft.Json;

namespace GatherBuddy.AutoGather
{
    public record class ConfigPreset
    {
        public const int MaxCollectability = 1000;
        public const int MaxIntegrity = 10;
        public const int MaxLevel = 100;
        public const int MaxGvl = 1000;
        public const int MaxGP = 2000;

        private int gatherableMinGP = 0;
        private int collectableMinGP = 0;
        private int collectableActionsMinGP = 0;
        private int collectableTagetScore = MaxCollectability;
        private int collectableMinScore = 400;

        public bool Enabled { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public NodeTypeRec NodeType { get; init; } = new();
        public LevelRec ItemLevel { get; init; } = new();
        public ItemTypeRec ItemType { get; init; } = new();

        public bool UseGivingLandOnCooldown { get; set; } = false;
        public int GatherableMinGP { get => gatherableMinGP; set => gatherableMinGP = Math.Max(0, Math.Min(MaxGP, value)); }
        public int CollectableMinGP { get => collectableMinGP; set => collectableMinGP = Math.Max(0, Math.Min(MaxGP, value)); }
        public int CollectableActionsMinGP { get => collectableActionsMinGP; set => collectableActionsMinGP = Math.Max(0, Math.Min(MaxGP, value)); }
        public bool CollectableAlwaysUseSolidAge { get; set; } = true;
        public int CollectableTagetScore { get => collectableTagetScore; set => collectableTagetScore = Math.Max(0, Math.Min(MaxCollectability, value)); }
        public int CollectableMinScore { get => collectableMinScore; set => collectableMinScore = Math.Max(0, Math.Min(MaxCollectability, value)); }
        public bool ChooseBestActionsAutomatically { get; set; } = false;
        public bool SpendGPOnBestNodesOnly { get; set; } = false;
        public GatheringActionsRec GatherableActions { get; init; } = new();
        public CollectableActionsRec CollectableActions { get; init; } = new();
        public ConsumablesRec Consumables { get; init; } = new();
    
        public record class ActionConfig
        {
            private int minGP = 0;
            private int maxGP = ConfigPreset.MaxGP;

            public bool Enabled { get; set; } = true;
            public int MinGP { get => minGP; set => minGP = Math.Max(0, Math.Min(ConfigPreset.MaxGP, value)); }
            public int MaxGP { get => maxGP; set => maxGP = Math.Max(0, Math.Min(ConfigPreset.MaxGP, value)); }
        }
        public record class ActionConfigYieldBonus : ActionConfig
        {
            private int minYieldBonus = 2;

            public int MinYieldBonus { get => minYieldBonus; set => minYieldBonus = Math.Max(1, Math.Min(3, value)); }
        }
        public record class ActionConfigYieldTotal : ActionConfig
        {
            private int minYieldTotal = 5;

            public int MinYieldTotal { get => minYieldTotal; set => minYieldTotal = Math.Max(1, Math.Min(30, value)); }
        }
        public record class ActionConfigIntegrity : ActionConfig
        {
            private int minIntegrity = 6;

            public bool FirstStepOnly { get; set; } = true;
            public int MinIntegrity { get => minIntegrity; set => minIntegrity = Math.Max(1, Math.Min(MaxIntegrity, value)); }
        }
        public record class ActionConfigBoon : ActionConfigIntegrity
        {
            private int minBoonChance = 0;
            private int maxBoonChance = 100;

            public int MinBoonChance { get => minBoonChance; set => minBoonChance = Math.Max(0, Math.Min(100, value)); }
            public int MaxBoonChance { get => maxBoonChance; set => maxBoonChance = Math.Max(0, Math.Min(100, value)); }
        }
        public record class ActionConfigConsumable : ActionConfig
        {
            public uint ItemId { get; set; } = 0;
        }

        public record class LevelRec {
            private int min = 1;
            private int max = MaxLevel;

            [JsonProperty(Order = 1)]
            public int Min { get => min; set => min = Math.Max(1, Math.Min(UseGlv ? MaxGvl : MaxLevel, value)); }
            [JsonProperty(Order = 1)]
            public int Max { get => max; set => max = Math.Max(1, Math.Min(UseGlv ? MaxGvl : MaxLevel, value)); }
            [JsonProperty(Order = 0)]
            public bool UseGlv { get; set; }
        }
        public record class ItemTypeRec
        {
            public bool Crystals { get; set; } = true;
            public bool Collectables { get; set; } = true;
            public bool Other { get; set; } = true;
        }
        public record class NodeTypeRec
        {
            public bool Regular { get; set; } = true;
            public bool Unspoiled { get; set; } = true;
            public bool Legendary { get; set; } = true;
            public bool Ephemeral { get; set; } = true;
        }
        public record class GatheringActionsRec
        {
            public ActionConfigYieldBonus Bountiful { get; init; } = new() { MinGP = AutoGather.Actions.Bountiful.GpCost};
            public ActionConfigIntegrity Yield1 { get; init; } = new() { MinGP = AutoGather.Actions.Yield1.GpCost, Enabled = false };
            public ActionConfigIntegrity Yield2 { get; init; } = new() { MinGP = AutoGather.Actions.Yield2.GpCost};
            public ActionConfigYieldTotal SolidAge { get; init; } = new() { MinGP = AutoGather.Actions.SolidAge.GpCost};
            public ActionConfigIntegrity TwelvesBounty { get; init; } = new() { MinGP = AutoGather.Actions.TwelvesBounty.GpCost };
            public ActionConfigIntegrity GivingLand { get; init; } = new() { MinGP = AutoGather.Actions.GivingLand.GpCost};
            public ActionConfigBoon Gift1 { get; init; } = new() { MinGP = AutoGather.Actions.Gift1.GpCost, Enabled = false, MaxBoonChance = 90 };
            public ActionConfigBoon Gift2 { get; init; } = new() { MinGP = AutoGather.Actions.Gift2.GpCost, Enabled = false, MaxBoonChance = 70 };
            public ActionConfigBoon Tidings { get; init; } = new() { MinGP = AutoGather.Actions.Tidings.GpCost, Enabled = false, MinBoonChance = 70 };
            public GatheringActionsRec(GatheringActionsRec original)
            {
                Bountiful = original.Bountiful with { };
                Yield1 = original.Yield1 with { };
                Yield2 = original.Yield2 with { };
                SolidAge = original.SolidAge with { };
                TwelvesBounty = original.TwelvesBounty with { };
                GivingLand = original.GivingLand with { };
                Gift1 = original.Gift1 with { };
                Gift2 = original.Gift2 with { };
                Tidings = original.Tidings with { };
            }
        };
        public record class CollectableActionsRec
        {
            public ActionConfig Scrutiny { get; init; } = new() { MinGP = AutoGather.Actions.Scrutiny.GpCost};
            public ActionConfig Scour { get; init; } = new() { MinGP = AutoGather.Actions.Scour.GpCost};
            public ActionConfig Brazen { get; init; } = new() { MinGP = AutoGather.Actions.Brazen.GpCost};
            public ActionConfig Meticulous { get; init; } = new() { MinGP = AutoGather.Actions.Meticulous.GpCost};
            public ActionConfig SolidAge { get; init; } = new() { MinGP = AutoGather.Actions.SolidAge.GpCost};
            public CollectableActionsRec(CollectableActionsRec original)
            {
                Scrutiny = original.Scrutiny with { };
                Scour = original.Scour with { };
                Brazen = original.Brazen with { };
                Meticulous = original.Meticulous with { };
                SolidAge = original.SolidAge with { };
            }
        }
        public record class ConsumablesRec
        {
            public ActionConfigConsumable Cordial { get; init; } = new() { Enabled = false };
            public ActionConfigConsumable Food { get; init; } = new() { Enabled = false };
            public ActionConfigConsumable Potion { get; init; } = new() { Enabled = false };
            public ActionConfigConsumable Manual { get; init; } = new() { Enabled = false };
            public ActionConfigConsumable SquadronManual { get; init; } = new() { Enabled = false };
            public ActionConfigConsumable SquadronPass { get; init; } = new() { Enabled = false };
            public ConsumablesRec(ConsumablesRec original)
            {
                Cordial = original.Cordial with { };
                Food = original.Food with { };
                Potion = original.Potion with { };
                Manual = original.Manual with { };
                SquadronManual = original.SquadronManual with { };
                SquadronPass = original.SquadronPass with { };
            }
        }

        public ConfigPreset(ConfigPreset original)
        {
            Enabled = original.Enabled;
            Name = original.Name;
            
            ItemLevel = original.ItemLevel with { };
            ItemType = original.ItemType with { };
            NodeType = original.NodeType with { };

            UseGivingLandOnCooldown = original.UseGivingLandOnCooldown;
            GatherableMinGP = original.GatherableMinGP;

            CollectableMinGP = original.CollectableMinGP;
            CollectableActionsMinGP = original.CollectableActionsMinGP;
            CollectableAlwaysUseSolidAge = original.CollectableAlwaysUseSolidAge;
            CollectableTagetScore = original.CollectableTagetScore;
            CollectableMinScore = original.CollectableMinScore;

            ChooseBestActionsAutomatically = original.ChooseBestActionsAutomatically;
            SpendGPOnBestNodesOnly = original.SpendGPOnBestNodesOnly;

            GatherableActions = original.GatherableActions with { };
            CollectableActions = original.CollectableActions with { };
            Consumables = original.Consumables with { };
        }

        public ConfigPreset MakeDefault()
        {
            return this with
            {
                Enabled = true,
                Name = "Default",
                ItemLevel = new(),
                ItemType = new(),
                NodeType = new()
            };
        }

        public bool Match(Gatherable item) =>
            Enabled
            && (
                !ItemLevel.UseGlv && item.Level >= ItemLevel.Min && item.Level <= ItemLevel.Max
              || ItemLevel.UseGlv && item.GatheringData.GatheringItemLevel.RowId >= ItemLevel.Min && item.GatheringData.GatheringItemLevel.RowId <= ItemLevel.Max
            )
            && item.NodeType switch
            {
                Enums.NodeType.Regular => NodeType.Regular,
                Enums.NodeType.Unspoiled => NodeType.Unspoiled,
                Enums.NodeType.Legendary => NodeType.Legendary,
                Enums.NodeType.Ephemeral => NodeType.Ephemeral,
                _ => false
            }
                && (   item.IsCrystal && ItemType.Crystals
                || item.ItemData.IsCollectable && ItemType.Collectables
                || !item.IsCrystal && !item.ItemData.IsCollectable && ItemType.Other);

        public string ToBase64String()
        {
            var json = JsonConvert.SerializeObject(this);

            var base64EncodedBytes = System.Text.Encoding.UTF8.GetBytes(json);
            return System.Convert.ToBase64String(base64EncodedBytes);
        }

        public static ConfigPreset? FromBase64String(string base64String)
        {
            try
            {
                var bytes = System.Convert.FromBase64String(base64String);
                var json  = System.Text.Encoding.UTF8.GetString(bytes);

                var config = JsonConvert.DeserializeObject<ConfigPreset>(json);
                return config;
            }
            catch (Exception ex)
            {
                Svc.Log.Debug($"Failed to deserialize config preset: {ex}");
                return null;
            }
        }
    }

}
