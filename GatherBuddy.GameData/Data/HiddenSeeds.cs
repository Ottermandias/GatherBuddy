using System.Collections.Generic;
using Dalamud.Logging;
using GatherBuddy.Classes;

namespace GatherBuddy.Data;

public static class HiddenSeeds
{
    public static readonly (uint ItemId, uint SeedId)[] Seeds =
    {
        (4785, 7715), // Paprika          
        (4777, 7716), // Wild Onion       
        (4778, 7717), // Coerthan Carrot  
        (4782, 7718), // La Noscean Lettuce
        (4804, 7719), // Cinderfoot Olive 
        (4787, 7720), // Popoto           
        (4821, 7721), // Millioncorn      
        (4788, 7722), // Wizard Eggplant  
        (4789, 7723), // Midland Cabbage  
        (4809, 7725), // La Noscean Orange
        (4808, 7726), // Lowland Grapes   
        (4810, 7727), // Faerie Apple     
        (4811, 7728), // Sun Lemon        
        (4812, 7729), // Pixie Plums      
        (4814, 7730), // Blood Currants   
        (6146, 7731), // Mirror Apple     
        (4815, 7732), // Rolanberry       
        (4829, 7735), // Garlean Garlic   
        (5539, 7736), // Lavender         
        (4830, 7737), // Black Pepper     
        (4835, 7738), // Ala Mhigan Mustard
        (4836, 7739), // Pearl Ginger     
        (5542, 7740), // Chamomile        
        (5346, 7741), // Flax         
        (4837, 7742), // Midland Basil
        (5543, 7743), // Mandrake     
        (4842, 7744), // Almonds

        (29669, 29670), // Oddly Specific Latex                 
        (29671, 29672), // Oddly Specific Obsidian              
        (29674, 29675), // Oddly Specific Amber                 
        (29676, 29677), // Oddly Specific Dark Matter           
        (31125, 31126), // Oddly Specific Leafborne Aethersand  
        (31130, 31131), // Oddly Specific Primordial Resin      
        (31127, 31128), // Oddly Specific Landborne Aethersand  
        (31132, 31133), // Oddly Specific Primordial Asphaltum  
    };

    public static void Apply(GameData data)
    {
        var list = new List<(Gatherable, Gatherable)>(Seeds.Length);

        foreach (var (parent, seed) in Seeds)
        {
            if (!data.Gatherables.TryGetValue(parent, out var parentItem))
            {
                PluginLog.Error($"Could not find item {parent}.");
                continue;
            }

            if (!data.Gatherables.TryGetValue(seed, out var seedItem))
            {
                PluginLog.Error($"Could not find item {seed}.");
                continue;
            }

            list.Add((parentItem, seedItem));
        }

        foreach (var node in data.GatheringNodes.Values)
        {
            foreach (var (parent, seed) in list)
            {
                if (node.HasItems(parent))
                    node.AddItem(seed);
            }
        }
    }
}
