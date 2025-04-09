using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

            public static bool ArtisanAssemblyEnabled
                => DalamudReflector.TryGetDalamudPlugin("Artisan", out _, false, true);

            public static bool TouchArtisanAssembly
                => DalamudReflector.TryGetDalamudPlugin("Artisan", out ArtisanAssemblyInstance, out _, false, true);

            public static Task<AutoGatherList?>? ArtisanExportTask  = null;
            public static int?                   ListIndexToReplace = null;

            public static bool ArtisanExportInProgress
                => ArtisanExportTask != null && ArtisanExportTask.Status == TaskStatus.Running;

            public static bool ArtisanExportComplete
                => ArtisanExportTask != null && ArtisanExportTask.Status == TaskStatus.RanToCompletion;

            public static bool ArtisanExportFailed
                => ArtisanExportTask != null && ArtisanExportTask.Status == TaskStatus.Faulted;

            public static bool ArtisanExportBusy
                => ArtisanExportTask != null && ListIndexToReplace != null;

            public static void StartArtisanImportTask(string listName, int listIndexToReplace)
            {
                ListIndexToReplace = listIndexToReplace;
                ArtisanExportTask  = Task.Run(() => ArtisanImportAsync(listName));
            }

            public static void ResetArtisanExportTask()
            {
                ArtisanExportTask  = null;
                ListIndexToReplace = null;
            }

            public static async Task<AutoGatherList?> ArtisanImportAsync(string listName)
            {
                try
                {
                    if (TouchArtisanAssembly)
                    {
                        IList artisanCraftingLists      = (IList)ArtisanAssemblyInstance.GetFoP("Config").GetFoP("NewCraftingLists");
                        var   artisanRootAssembly       = Assembly.GetAssembly(ArtisanAssemblyInstance!.GetType())!;
                        var   craftingListFunctionsType = artisanRootAssembly.GetType("Artisan.CraftingLists.CraftingListFunctions")!;
                        var listMaterialsMethodInfo =
                            craftingListFunctionsType.GetMethod("ListMaterials", BindingFlags.Public | BindingFlags.Static);

                        AutoGatherList importedList = new()
                        {
                            Name        = listName,
                            Description = "Imported from Artisan",
                            Enabled     = false,
                            Fallback    = false,
                        };
                        var targetList = artisanCraftingLists.Cast<object>().SingleOrDefault(l => l.GetFoP("Name").ToString() == listName);
                        if (targetList == null)
                        {
                            Svc.Log.Error("Artisan Crafting List not found. Artisan List name must be identical to the Auto-Gather List name.");
                            return null;
                        }

                        var matList = (Dictionary<uint, int>)listMaterialsMethodInfo!.Invoke(null, [targetList])!;

                        foreach (var (itemId, quantity) in matList)
                        {
                            var gatherable =
                                GatherBuddy.GameData.Gatherables.FirstOrDefault(g => g.Key == itemId);
                            if (gatherable.Value == null || gatherable.Value.NodeList.Count == 0)
                                continue;

                            importedList.Add(gatherable.Value, (uint)quantity);
                        }

                        return importedList;
                    }

                    return null;
                }
                catch (Exception e)
                {
                    Svc.Log.Error(e, "Error while importing Artisan List: ");
                    throw;
                }
            }
        }
    }
}
