using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyBuriedMemory(this GameData data)
    {
        data.Apply(37697, Patch.BuriedMemory) // Mayashell
            .Bait(data); 
        data.Apply(37845, Patch.BuriedMemory) // Greatsword Snook
            .Bait(data);
        data.Apply(37846, Patch.BuriedMemory) // Swampsucker Bowfin
            .Bait(data);
        data.Apply(37847, Patch.BuriedMemory) // Lale Crab
            .Bait(data);
        data.Apply(37848, Patch.BuriedMemory) // Bigcuda
            .Bait(data);
        data.Apply(37849, Patch.BuriedMemory) // Sovereign Shadow
            .Bait(data);
        data.Apply(37850, Patch.BuriedMemory) // Disappirarucu
           .Bait(data);
        data.Apply(37851, Patch.BuriedMemory) // Starscryer
           .Bait(data);
        data.Apply(37852, Patch.BuriedMemory) // Argonauta argo
           .Bait(data);
        data.Apply(37853, Patch.BuriedMemory) // Planetes
           .Bait(data);
    }
    // @formatter:on
}
