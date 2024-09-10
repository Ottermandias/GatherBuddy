using System.Collections.Generic;
using System.Linq;
using ECommons.GameHelpers;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.AutoGather
{
    public partial class AutoGather
    {
        public static readonly Item[] PossibleCordials = Dalamud.GameData.GetExcelSheet<Item>()?.Where(IsItemCordial).ToArray() ?? [];

        public static readonly Item[] PossibleFoods = Dalamud.GameData.GetExcelSheet<Item>()?.Where(item => IsItemDoLFood(item)).ToArray() ?? [];

        public static readonly Item[] PossiblePotions = Dalamud.GameData.GetExcelSheet<Item>()?.Where(item => IsItemDoLPotion(item)).ToArray() ?? [];

        public static readonly Item[] PossibleManuals = Dalamud.GameData.GetExcelSheet<Item>()?.Where(item => IsItemDoLManual(item)).ToArray() ?? [];

        private static readonly Dictionary<uint, uint> SquadronManualItemIdBuffId = new ()
        {
            { 14949u, 1081u },
            { 14951u, 1083u },
            { 14952u, 1084u },
            { 14953u, 1085u },
            { 41707u, 1084u }
        };
        public static readonly Item[] PossibleSquadronManuals = Dalamud.GameData.GetExcelSheet<Item>()?.Where(item => IsItemDoLSquadronManual(item)).ToArray() ?? [];

        private static readonly Dictionary<uint, uint> SquadronPassItemIdBuffId = new ()
        {
            { 14954u, 1061u }
        };
        public static readonly Item[] PossibleSquadronPasses = Dalamud.GameData.GetExcelSheet<Item>()?.Where(item => IsItemDoLSquadronPass(item)).ToArray() ?? [];


        public static unsafe int GetInventoryItemCount(uint itemRowId)
        {
            return InventoryManager.Instance()->GetInventoryItemCount(itemRowId < 100000 ? itemRowId : itemRowId - 100000, itemRowId >= 100000);
        }

        private static byte[] GetItemFoodProps(Item item)
        {
            // Item UI category: 44 medicine, 46 meal
            if (item.ItemUICategory.Row is not 44 and not 46
                || item.ItemAction.Value == null
                // Buff: 48 well fed, 49 medicated
                || item.ItemAction.Value.Data[0] is not 48 and not 49)
                return [];
            return Dalamud.GameData.GetExcelSheet<ItemFood>()?.GetRow(item.ItemAction.Value.Data[1])?.UnkData1.Select(v => v.BaseParam).ToArray() ?? [];
        }

        private static bool IsItemCordial(Item item)
        {
            return item.ItemAction?.Value?.Type == 1055;
        }

        private static bool IsItemDoLFood(Item item)
        {
            if (item.ItemUICategory.Row != 46)
                return false;
            // 10 GP, 71 gathering, 73 perception
            return GetItemFoodProps(item).Any(p => p is 10 or 72 or 73);
        }

        private static bool IsItemDoLPotion(Item item)
        {
            if (item.ItemUICategory.Row != 44)
                return false;
            // 10 GP, 68 spiritbond, 69 durability, 72 gathering, 73 perception
            return GetItemFoodProps(item).Any(p => p is 10 or 68 or 69 or 72 or 73);
        }

        private static bool IsItemDoLManual(Item item)
        {
            if (item.ItemUICategory.Row != 63)
                return false;
            return item.ItemAction?.Value?.Type == 816 && item.ItemAction?.Value?.Data[0] is 302 or 303 or 1752 or 5330;
        }

        private static bool IsItemDoLSquadronManual(Item item)
        {
            return SquadronManualItemIdBuffId.ContainsKey(item.RowId);
        }

        private static bool IsItemDoLSquadronPass(Item item)
        {
            return SquadronPassItemIdBuffId.ContainsKey(item.RowId);
        }

        public unsafe bool IsCordialOnCooldown
        {
            get
            {
                var cordialRecastGroup = ActionManager.Instance()->GetRecastGroupDetail(68);
                return cordialRecastGroup->Total - cordialRecastGroup->Elapsed > 0;
            }
        }

        public unsafe bool IsFoodBuffUp
        {
            get
            {
                var buff = Dalamud.ClientState?.LocalPlayer?.StatusList.FirstOrDefault(s => s.StatusId == 48);
                if (buff == null)
                {
                    return false;
                }
                else
                {
                    var configuredItem = PossibleFoods.FirstOrDefault(item => new[] { item.RowId, item.RowId + 100000 }.Contains(GatherBuddy.Config.AutoGatherConfig.FoodConfig.ItemId));
                    if (GatherBuddy.Config.AutoGatherConfig.FoodConfig.ItemId > 100000)
                    {
                        return buff.Param == configuredItem?.ItemAction.Value?.DataHQ[1] + 10000;
                    }
                    else
                    {
                        return buff.Param == configuredItem?.ItemAction.Value?.Data[1];
                    }
                }
            }
        }

        public unsafe bool IsPotionBuffUp
        {
            get
            {
                var buff = Dalamud.ClientState?.LocalPlayer?.StatusList.FirstOrDefault(s => s.StatusId == 49);
                if (buff == null)
                {
                    return false;
                }
                else
                {
                    var configuredItem = PossiblePotions.FirstOrDefault(item => new[] { item.RowId, item.RowId + 100000 }.Contains(GatherBuddy.Config.AutoGatherConfig.PotionConfig.ItemId));
                    if (GatherBuddy.Config.AutoGatherConfig.PotionConfig.ItemId > 100000)
                    {
                        return buff.Param == configuredItem?.ItemAction.Value?.DataHQ[1] + 10000;
                    }
                    else
                    {
                        return buff.Param == configuredItem?.ItemAction.Value?.Data[1];
                    }
                }
            }
        }

        public unsafe bool IsManualBuffUp => Dalamud.ClientState?.LocalPlayer?.StatusList.Any(s => s.StatusId == 46) ?? false;

        public unsafe bool IsSquadronManualBuffUp
        {
            get
            {
                if (SquadronManualItemIdBuffId.TryGetValue(GatherBuddy.Config.AutoGatherConfig.SquadronManualConfig.ItemId, out var requiredBuffId))
                {
                    return Dalamud.ClientState?.LocalPlayer?.StatusList.Any(s => s.StatusId == requiredBuffId) ?? false;
                }
                else
                {
                    return false;
                }
            }
        }

        public unsafe bool IsSquadronPassBuffUp
        {
            get
            {
                if (SquadronPassItemIdBuffId.TryGetValue(GatherBuddy.Config.AutoGatherConfig.SquadronPassConfig.ItemId, out var requiredBuffId))
                {
                    return Dalamud.ClientState?.LocalPlayer?.StatusList.Any(s => s.StatusId == requiredBuffId) ?? false;
                }
                else
                {
                    return false;
                }
            }
        }

        private unsafe void UseItem(uint itemId)
        {
            // When calling ActionManager UseAction, HQ have ids 1,000,000 more than NQ, whereas Lumina Excel is 100,000
            ActionManager.Instance()->UseAction(ActionType.Item, itemId > 100000 ? itemId + 900000 : itemId, extraParam: 65535);
        }

        // Cordial, food and potion have no cast time and can be used while mounted
        private void DoUseConsumablesWithoutCastTime()
        {
            // Check if consumables need to be refreshed every 5 seconds
            // Give sufficient time for buffs to activate otherwise items could be used multiple times and wasted
            if (EzThrottler.Throttle("DoUseConsumablesWithoutCastTime", 5000))
            {
                if (GatherBuddy.Config.AutoGatherConfig.CordialConfig.UseConsumable
                && GatherBuddy.Config.AutoGatherConfig.CordialConfig.ItemId > 0
                && !IsCordialOnCooldown
                && Player.Object.CurrentGp >= GatherBuddy.Config.AutoGatherConfig.CordialConfig.MinimumGP
                && Player.Object.CurrentGp <= GatherBuddy.Config.AutoGatherConfig.CordialConfig.MaximumGP
                && GetInventoryItemCount(GatherBuddy.Config.AutoGatherConfig.CordialConfig.ItemId) > 0
                )
                {
                    TaskManager.Enqueue(() => UseItem(GatherBuddy.Config.AutoGatherConfig.CordialConfig.ItemId));
                    return;
                }

                if (GatherBuddy.Config.AutoGatherConfig.FoodConfig.UseConsumable
                    && GatherBuddy.Config.AutoGatherConfig.FoodConfig.ItemId > 0
                    && !IsFoodBuffUp
                    && GetInventoryItemCount(GatherBuddy.Config.AutoGatherConfig.FoodConfig.ItemId) > 0
                    )
                {
                    TaskManager.Enqueue(() => UseItem(GatherBuddy.Config.AutoGatherConfig.FoodConfig.ItemId));
                    return;
                }

                if (GatherBuddy.Config.AutoGatherConfig.PotionConfig.UseConsumable
                    && GatherBuddy.Config.AutoGatherConfig.PotionConfig.ItemId > 0
                    && !IsPotionBuffUp
                    && GetInventoryItemCount(GatherBuddy.Config.AutoGatherConfig.PotionConfig.ItemId) > 0
                    )
                {
                    TaskManager.Enqueue(() => UseItem(GatherBuddy.Config.AutoGatherConfig.PotionConfig.ItemId));
                    return;
                }
            }
        }

        // Manuals have cast time and cannot be used while mounted
        private bool DoUseConsumablesWithCastTime()
        {
            if (GatherBuddy.Config.AutoGatherConfig.ManualConfig.UseConsumable
                && GatherBuddy.Config.AutoGatherConfig.ManualConfig.ItemId > 0
                && !IsManualBuffUp
                && GetInventoryItemCount(GatherBuddy.Config.AutoGatherConfig.ManualConfig.ItemId) > 0
                )
            {
                TaskManager.Enqueue(() => UseItem(GatherBuddy.Config.AutoGatherConfig.ManualConfig.ItemId));
                return true;
            }

            if (GatherBuddy.Config.AutoGatherConfig.SquadronManualConfig.UseConsumable
                && GatherBuddy.Config.AutoGatherConfig.SquadronManualConfig.ItemId > 0
                && !IsSquadronManualBuffUp
                && GetInventoryItemCount(GatherBuddy.Config.AutoGatherConfig.SquadronManualConfig.ItemId) > 0
                )
            {
                TaskManager.Enqueue(() => UseItem(GatherBuddy.Config.AutoGatherConfig.SquadronManualConfig.ItemId));
                return true;
            }

            if (GatherBuddy.Config.AutoGatherConfig.SquadronPassConfig.UseConsumable
                && GatherBuddy.Config.AutoGatherConfig.SquadronPassConfig.ItemId > 0
                && !IsSquadronPassBuffUp
                && GetInventoryItemCount(GatherBuddy.Config.AutoGatherConfig.SquadronPassConfig.ItemId) > 0
                )
            {
                TaskManager.Enqueue(() => UseItem(GatherBuddy.Config.AutoGatherConfig.SquadronPassConfig.ItemId));
                return true;
            }
            return false;
        }
    }
}
