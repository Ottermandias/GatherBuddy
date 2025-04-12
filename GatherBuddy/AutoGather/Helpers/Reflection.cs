using System;
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

        public class ArtisanExporter
        {
            private readonly AutoGatherListsManager _listsManager;
            public ArtisanExporter(AutoGatherListsManager listsManager)
            {
                _listsManager = listsManager;
            }
            public IDalamudPlugin? ArtisanAssemblyInstance;

            public bool ArtisanAssemblyEnabled
                => DalamudReflector.TryGetDalamudPlugin("Artisan", out _, false, true);

            public bool TouchArtisanAssembly
                => DalamudReflector.TryGetDalamudPlugin("Artisan", out ArtisanAssemblyInstance, out _, false, true);

            public Dictionary<int, string> GetArtisanListNames()
            {
                Dictionary<int, string> listNames = [];
                try
                {
                    if (TouchArtisanAssembly)
                    {
                        System.Collections.IList artisanCraftingLists = (System.Collections.IList)ArtisanAssemblyInstance.GetFoP("Config").GetFoP("NewCraftingLists");
                        foreach (var list in artisanCraftingLists)
                        {
                            var name = list.GetFoP("Name").ToString();
                            var id   = (int)list.GetFoP("ID");
                            if (name != null)
                                listNames.Add(id, name);
                        }
                    }
                    return listNames;
                }
                catch (Exception e)
                {
                    Svc.Log.Error(e, "Error while getting Artisan List names: ");
                    return listNames;
                }
            }

            public void StartArtisanImport(KeyValuePair<int, string> listKvp)
            {
                Task.Run(() => ImportArtisanList(listKvp));
            }

            private bool ImportArtisanList(KeyValuePair<int, string> listKvp)
            {
                try
                {
                    if (TouchArtisanAssembly)
                    {
                        System.Collections.IList artisanCraftingLists = (System.Collections.IList)ArtisanAssemblyInstance.GetFoP("Config").GetFoP("NewCraftingLists");
                        var   artisanRootAssembly  = Assembly.GetAssembly(ArtisanAssemblyInstance!.GetType())!;
                        var   craftingListFunctionsType = artisanRootAssembly.GetType("Artisan.CraftingLists.CraftingListFunctions")!;
                        var listMaterialsMethodInfo = craftingListFunctionsType.GetMethod("ListMaterials", All);
                        var targetList = artisanCraftingLists.Cast<object>().SingleOrDefault(l => (int)l.GetFoP("ID") == listKvp.Key);
                        if (targetList == null)
                        {
                            Svc.Log.Error($"Artisan list '{listKvp.Value}' ({listKvp.Key}) could not be found");
                            return false;
                        }

                        var matList = (Dictionary<uint, int>)listMaterialsMethodInfo!.Invoke(null, [targetList])!;

                        AutoGatherList list = new AutoGatherList();
                        list.Name        = listKvp.Value;
                        list.Description = "Imported from Artisan";
                        foreach (var (itemId, quantity) in matList)
                        {
                            var gatherable = GatherBuddy.GameData.Gatherables.FirstOrDefault(g => g.Key == itemId);
                            if (gatherable.Value == null || gatherable.Value.NodeList.Count == 0)
                                continue;
                            list.Add(gatherable.Value, (uint)quantity);
                        }
                        _listsManager.AddList(list);
                        Communicator.Print($"List '{listKvp.Value}' imported successfully!");
                        return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Svc.Log.Error(e, "Error while importing Artisan List: ");
                    throw;
                }
            }

            // public static async Task<AutoGatherList?> ArtisanImportAsync(string listName)
            // {
            //     try
            //     {
            //         if (TouchArtisanAssembly)
            //         {
            //             IList artisanCraftingLists      = (IList)ArtisanAssemblyInstance.GetFoP("Config").GetFoP("NewCraftingLists");
            //             var   artisanRootAssembly       = Assembly.GetAssembly(ArtisanAssemblyInstance!.GetType())!;
            //             var   craftingListFunctionsType = artisanRootAssembly.GetType("Artisan.CraftingLists.CraftingListFunctions")!;
            //             var listMaterialsMethodInfo =
            //                 craftingListFunctionsType.GetMethod("ListMaterials", BindingFlags.Public | BindingFlags.Static);
            //
            //             AutoGatherList importedList = new()
            //             {
            //                 Name        = listName,
            //                 Description = "Imported from Artisan",
            //                 Enabled     = false,
            //                 Fallback    = false,
            //             };
            //             var targetList = artisanCraftingLists.Cast<object>().SingleOrDefault(l => l.GetFoP("Name").ToString() == listName);
            //             if (targetList == null)
            //             {
            //                 Svc.Log.Error("Artisan Crafting List not found. Artisan List name must be identical to the Auto-Gather List name.");
            //                 return null;
            //             }
            //
            //             var matList = (Dictionary<uint, int>)listMaterialsMethodInfo!.Invoke(null, [targetList])!;
            //
            //             foreach (var (itemId, quantity) in matList)
            //             {
            //                 var gatherable =
            //                     GatherBuddy.GameData.Gatherables.FirstOrDefault(g => g.Key == itemId);
            //                 if (gatherable.Value == null || gatherable.Value.NodeList.Count == 0)
            //                     continue;
            //
            //                 importedList.Add(gatherable.Value, (uint)quantity);
            //             }
            //
            //             return importedList;
            //         }
            //
            //         return null;
            //     }
            //     catch (Exception e)
            //     {
            //         Svc.Log.Error(e, "Error while importing Artisan List: ");
            //         throw;
            //     }
            // }
        }
    }
}
