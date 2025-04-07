using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Plugin;
using ECommons.DalamudServices;
using ECommons.ExcelServices;
using ECommons.Reflection;
using GatherBuddy.AutoGather.Lists;
using GatherBuddy.Gui;
using GatherBuddy.Plugin;

namespace GatherBuddy.AutoGather.Helpers
{
    public static class Reflection
    {
        private const BindingFlags All = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        internal static class Artisan_Reflection
        {
            public static IDalamudPlugin? ArtisanAssemblyInstance;
            public static bool ArtisanAssemblyEnabled => DalamudReflector.TryGetDalamudPlugin("Artisan", out _, false, true);
            public static bool TouchArtisanAssembly => DalamudReflector.TryGetDalamudPlugin("Artisan", out ArtisanAssemblyInstance, out _, false, true);

            public static AutoGatherList ArtisanImport(string listName)
            {
                if (TouchArtisanAssembly)
                {

                    IList artisanCraftingLists = (IList)ArtisanAssemblyInstance.GetFoP("Config").GetFoP("NewCraftingLists");
                    var artisanRootAssembly = Assembly.GetAssembly(ArtisanAssemblyInstance!.GetType())!;
                    var craftingListFunctionsType = artisanRootAssembly.GetType("Artisan.CraftingLists.CraftingListFunctions")!;
                    var listMaterialsMethodInfo = craftingListFunctionsType.GetMethod("ListMaterials", BindingFlags.Public | BindingFlags.Static);

                    AutoGatherList importedList = new()
                    {
                        Name = listName,
                        Description = "Imported from Artisan",
                        Enabled = false,
                        Fallback = false,
                    };
                    var targetList = artisanCraftingLists.Cast<object>().SingleOrDefault(l => l.GetFoP("Name").ToString() == listName);
                    if (targetList == null)
                    {
                        Svc.Log.Warning("Artisan Crafting List not found. Artisan List name must be identical to the Auto-Gather List name.");
                        return null;
                    }

                    var matList = (Dictionary<uint, int>)listMaterialsMethodInfo!.Invoke(null, [targetList])!;

                    foreach (var (itemId, quantity) in matList)
                    {
                        var gatherable =
                            GatherBuddy.GameData.Gatherables.Values.FirstOrDefault(g => g.Name[Dalamud.ClientState.ClientLanguage] == ExcelItemHelper.GetName(itemId));
                        if (gatherable == null || gatherable.NodeList.Count == 0)
                            continue;

                        importedList.Add(gatherable, (uint)quantity);
                    }

                    return importedList;
                }
                return null;
            }
        }
    }
}